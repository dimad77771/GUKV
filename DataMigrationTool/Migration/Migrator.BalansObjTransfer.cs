using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace GUKV.DataMigration
{
//    public partial class Migrator
//    {
//        #region Automatical transfer of objects from one owner to another

//        private void PerformAutoTransfers()
//        {
//            DetectMissingObjects();
//            DetectDeletedObjects();
//        }

//        private void DetectMissingObjects()
//        {
//            string query = @"SELECT bal.id FROM balans bal WHERE (bal.is_deleted = 0 OR bal.is_deleted IS NULL) AND
//                EXISTS (SELECT rep.id FROM reports1nf rep WHERE rep.organization_id = bal.organization_id) AND
//                NOT EXISTS (SELECT b.id FROM reports1nf_balans b WHERE b.id = bal.id)";

//            HashSet<int> balansIDs = new HashSet<int>();

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        balansIDs.Add(reader.GetInt32(0));
//                    }

//                    reader.Close();
//                }
//            }

//            foreach (int balansId in balansIDs)
//            {
//                try
//                {
//                    using (SqlCommand cmd = new SqlCommand("dbo.fnAddBalansObjectToReport", connectionSqlClient))
//                    {
//                        cmd.Parameters.Add(new SqlParameter("BALANS_ID", balansId));

//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.ExecuteNonQuery();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    logger.WriteError("Exception while synchronizing user web-based report (DetectMissingObjects): " + ex.Message);
//                }

//                NotifyBalansObjectAdded(balansId);
//            }
//        }

//        private void DetectDeletedObjects()
//        {
//            string query = @"SELECT bal.id FROM balans bal WHERE (bal.is_deleted = 1) AND
//                EXISTS (SELECT b.id FROM reports1nf_balans b WHERE b.id = bal.id)";

//            HashSet<int> balansIDs = new HashSet<int>();

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        balansIDs.Add(reader.GetInt32(0));
//                    }

//                    reader.Close();
//                }
//            }

//            foreach (int balansId in balansIDs)
//            {
//                try
//                {
//                    using (SqlCommand cmd = new SqlCommand("dbo.fnDeleteBalansObjectInReport", connectionSqlClient))
//                    {
//                        cmd.Parameters.Add(new SqlParameter("BALANS_ID", balansId));

//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.ExecuteNonQuery();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    logger.WriteError("Exception while synchronizing user web-based report (DetectDeletedObjects): " + ex.Message);
//                }

//                NotifyBalansObjectRemoved(balansId);
//            }
//        }

//        #endregion (Automatical transfer of objects from one owner to another)
//    }
}
