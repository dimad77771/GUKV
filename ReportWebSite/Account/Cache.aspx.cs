using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using Cache;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using DevExpress.Web;

public partial class Account_Cache : System.Web.UI.Page
{
    private bool RequestHasParameter(string paramName)
    {
        if (!string.IsNullOrEmpty(Request.QueryString[paramName]))
            return true;
        string valuelessParameterNames = Request.QueryString[null];
        if (valuelessParameterNames == null)
            return false;
        return valuelessParameterNames.Split(',').Contains(paramName);
    }

    private bool RequestHasParameter(string paramName, out string paramValue)
    {
        paramValue = Request.QueryString[paramName];
        return !string.IsNullOrEmpty(paramValue);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bool actionPerformed = false;
        if (RequestHasParameter("Reset"))
        {
            DataSourceCache.Clear();
            actionPerformed = true;
        }
        if (RequestHasParameter("ForgetAll"))
        {
            DataSourceCache.Flush();
            actionPerformed = true;
        }
        string forgetUniqueID;
        if (RequestHasParameter("Forget", out forgetUniqueID))
        {
            ForgetCacheableQuery(forgetUniqueID);
            actionPerformed = true;
        }
        if (RequestHasParameter("Reload"))
        {
            DataSourceCache.Clear();

            StringBuilder errors = new StringBuilder();

            if (ConfigurationManager.ConnectionStrings["GUKVConnectionString"] != null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["GUKVConnectionString"].ConnectionString;

                foreach (DataSourceCache.CacheKey key in DataSourceCache.CacheableKeys)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            using (SqlCommand command = conn.CreateCommand())
                            {
                                command.CommandType = CommandType.Text;
                                command.CommandTimeout = 600;
                                command.CommandText = key.Query;

                                foreach (Tuple<string, object> parameter in key.Params)
                                {
                                    command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                                }

                                DataSet dataset = new DataSet();

                                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                                {
                                    adapter.Fill(dataset);
                                }

                                if (dataset.Tables.Count > 0)
                                    DataSourceCache.Put(command, "", dataset.Tables[0], false);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errors.Length > 0)
                            errors.AppendLine(new string('-', 40));
                        errors.AppendLine(ex.ToString());
                    }
                }
            }
            else
            {
                errors.AppendLine("web.config does not provide initialization for 'GUKVConnectionString' connection string");
            }

            if (errors.Length > 0)
            {
                Session["CacheAspxErrors"] = 
                    "The following error(s) occurred while executing database queries:\n\n" + errors.ToString();
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            actionPerformed = true;
        }

        if (actionPerformed)
        {
            Response.Redirect("~/Account/Cache.aspx");
        }
        else
        {
            ulong totalPhysicalMemory = 0, availablePhysicalMemory = 0;
            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(memStatus))
            {
                totalPhysicalMemory = memStatus.ullTotalPhys;
                availablePhysicalMemory = memStatus.ullAvailPhys;
            }

            ASPxLabel2.Text = string.Format(
                "WorkingSet: {0}; Total allocated memory: {1}; 64-bit OS? {2}; 64-bit process? {3}; total RAM: {4}; free RAM: {5}; Rev: {6}",
                FormatBytes(GC.GetTotalMemory(false)),
                FormatBytes(Environment.WorkingSet),
                (Environment.Is64BitOperatingSystem ? "yes" : "no"),
                (Environment.Is64BitProcess ? "yes" : "no"),
                (totalPhysicalMemory > 0 ? FormatBytes(totalPhysicalMemory) : "n/a"),
                (availablePhysicalMemory > 0 ? FormatBytes(availablePhysicalMemory) : "n/a"),
                typeof(DB).Assembly.GetName().Version);

            InitializeGridDatasource();
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
        public MEMORYSTATUSEX()
        {
            this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
        }
    }


    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

    private static string FormatBytes(long value)
    {
        string[] prefixes = new string[] { "", "K", "M", "G", "T" };
        double value0 = value;
        int index = 0;

        while (value0 > 1024 && index < prefixes.Length)
        {
            value0 /= 1024;
            ++index;
        }

        return string.Format("{0:F2}{1}B", value0, prefixes[index]);
    }

    private static string FormatBytes(ulong value)
    {
        string[] prefixes = new string[] { "", "K", "M", "G", "T" };
        double value0 = value;
        int index = 0;

        while (value0 > 1024 && index < prefixes.Length)
        {
            value0 /= 1024;
            ++index;
        }

        return string.Format("{0:F2}{1}B", value0, prefixes[index]);
    }

    private void InitializeGridDatasource()
    {
        DataTable data = new DataTable();
        data.Columns.Add("UID");
        data.Columns.Add("Query");
        data.Columns.Add("Parameters");
        data.Columns.Add("Available?", typeof(bool));
        data.Columns.Add("Last Access", typeof(DateTime));
        data.Columns.Add("Geometry");
        data.PrimaryKey = new DataColumn[] { data.Columns["UID"] };

        foreach (KeyValuePair<DataSourceCache.CacheKey, DataSourceCache.CacheValue> entry in DataSourceCache.Entries)
        {
            DataTable cachedTable = entry.Value.GetValue(false);
            data.Rows.Add(
                entry.Key.UniqueID,
                entry.Key.Query,
                string.Join("\n", entry.Key.Params.Select(x => string.Format("{0}={1}", x.Item1, x.Item2)).ToArray()),
                cachedTable != null,
                entry.Value.LastAccess,
                (cachedTable == null ? null : string.Format("{0}R x {1}C", cachedTable.Rows.Count, cachedTable.Columns.Count))
                );
        }

        ASPxGridView1.DataSource = data;
        ASPxGridView1.DataBind();

        string reloadCacheErrors = Session["CacheAspxErrors"] as string;
        if (!string.IsNullOrEmpty(reloadCacheErrors))
        {
            ASPxLabel1.ClientVisible = true;
            ASPxLabel1.Visible = true;
            ASPxLabel1.Text = reloadCacheErrors.ToString();

            Session.Remove("CacheAspxErrors");
        }
    }

    private void ForgetCacheableQuery(string uniqueID)
    {
        DataSourceCache.CacheKey key = 
            DataSourceCache.Entries.Select(x => x.Key).FirstOrDefault(x => x.UniqueID == uniqueID);
        if (key != null)
        {
            DataSourceCache.Forget(key);
        }
        else
        {
            Session["CacheAspxErrors"] = "The specified key does not represent a valid cache entry";
        }
    }
}
