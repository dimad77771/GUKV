using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using log4net;
using System.Data.SqlClient;
using GUKV;

/// <summary>
/// Summary description for WorkItem
/// </summary>

public abstract class WorkItem
{
    protected static readonly ILog log = LogManager.GetLogger("ReportWebSite");

    protected int reportID = 0;
    protected string userName = string.Empty;
    protected Dictionary<int, bool> allIDList = new Dictionary<int, bool>();
    private HashSet<int> validIDList = new HashSet<int>();

    public string id = "";
    public Thread thread = null;
    public int numObjectsToProcess = 0;
    public int numObjectsProcessed = 0;
    public string message = "";
    public bool finished = false;

    public bool IncludeDeleted = false;
    public bool ValidateDeleted = false;

    public WorkItem()
    {
    }

    public abstract string GetTableName();

    public abstract ValidatorBase GetValidator();

    public virtual void Init(int reportId, string user)
    {
        //SELECT a.id FROM reports1nf_balans a LEFT JOIN reports1nf r ON a.report_id = r.id WHERE a.report_id = @rid
        //AND (((a.submit_date IS NULL) AND (a.modify_date > r.create_date)) OR ((NOT a.modify_date IS NULL) AND (a.modify_date > a.submit_date) AND (a.modify_date > r.create_date)))

        reportID = reportId;
        userName = user;

        // Get all balans objects to validate
        SqlConnection connection = Utils.ConnectToDatabase();

        allIDList = new Dictionary<int,bool>();
        
        if (connection != null)
        {
            string query = @"SELECT a.id,a.is_deleted FROM " + GetTableName() + @" a 
                    LEFT JOIN reports1nf r ON a.report_id = r.id 
                    WHERE a.report_id = @rid";
            if (!IncludeDeleted)
                query += " and ((a.is_deleted is null) or (a.is_deleted = 0))";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        bool isDel = reader.IsDBNull(1) ? false : reader.GetInt32(1) == 1;
                        allIDList.Add(id, isDel);
                    }
                    reader.Close();
                }
            }

            foreach (KeyValuePair<int, bool> pair in allIDList)
            {
                if (!pair.Value)
                {
                    ValidatorBase validator = GetValidator();
                    validator.ValidateDB(connection, GetTableName(), string.Format("report_id = {0} and id = {1}", reportID, pair.Key), true);
                }
                else
                {
                    if (ValidateDeleted)
                    {
                        ValidatorBase validator = GetValidator();
                        validator.ValidateDB(connection, GetTableName(), string.Format("report_id = {0} and id = {1}", reportID, pair.Key), true);
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("UPDATE " + GetTableName() + " SET is_valid = 1, validation_errors = null where id = @id", connection))
                        {
                            cmd.Parameters.AddWithValue("id", pair.Key);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            connection.Close();
        }

        this.numObjectsToProcess = allIDList.Count();
    }

    public virtual void StartProcessing()
    {
        this.thread = new Thread(new ThreadStart(this.DoWork));
        this.thread.Start();
    }

    public void DoWork()
    {
        string finalMessage = "Обробку завершено.";

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            try
            {
                string query = @"SELECT a.id FROM " + GetTableName() + @" a 
                        LEFT JOIN reports1nf r ON a.report_id = r.id 
                        WHERE a.report_id = @rid AND a.is_valid = 1 AND
                        (
                        (a.submit_date IS NULL)
                        OR
                        (a.modify_date > a.submit_date))";

                validIDList = new HashSet<int>();

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("rid", reportID));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            validIDList.Add(reader.GetInt32(0));
                        }

                        reader.Close();
                    }
                }

                lock (Reports1NFUtils.thisLock)
                {
                    this.numObjectsToProcess = validIDList.Count;
                    this.numObjectsProcessed = 0;
                }

                foreach (int id in validIDList)
                {
                    Send(connection, reportID, id, userName);

                    // Increase the counter when each object is processed
                    lock (Reports1NFUtils.thisLock)
                    {
                        
                        this.numObjectsProcessed++;
                    }
                }


            }
            catch (Exception ex)
            {
                finalMessage = ex.Message;
                log.Error(ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);
            }

            connection.Close();
        }

        // Finished!
        lock (Reports1NFUtils.thisLock)
        {
            this.numObjectsProcessed = this.numObjectsToProcess;
            this.finished = true;
            this.message = finalMessage;
        }
    }

    public abstract void Send(SqlConnection connection, int reportID, int id, string userName);
}

