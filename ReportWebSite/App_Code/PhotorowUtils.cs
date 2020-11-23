using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.Configuration;

namespace ExtDataEntry.Models
{
    public static class PhotorowUtils
    {
        public static byte[] Read(string photofilename, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "PhotorowUtils.Read: photofilename=" + photofilename + "\n");

            byte[] result = null;
            using (SqlCommand cmdFiles = new SqlCommand("select photofilebytes from photorow where photofilename = @photofilename", connectionSql, sqlTransaction))
            {
                cmdFiles.Parameters.AddWithValue("photofilename", photofilename);
                using (SqlDataReader reader = cmdFiles.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var rbytes = reader.GetSqlBytes(0);
                        if (!rbytes.IsNull)
                        {
                            result = rbytes.Value;
                        }
                    }
                    reader.Close();
                }
            }
            return result;
        }

        public static bool Exists(string photofilename, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "PhotorowUtils.Exists: photofilename=" + photofilename + "\n");

            using (SqlCommand cmdFiles = new SqlCommand("select 1 from photorow where photofilename = @photofilename", connectionSql, sqlTransaction))
            {
                cmdFiles.Parameters.AddWithValue("photofilename", photofilename);
                using (SqlDataReader reader = cmdFiles.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    reader.Close();
                }
            }
            return false;
        }

        public static string[] GetFiles(string photofiledir, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            if (string.IsNullOrEmpty(photofiledir)) throw new Exception("GetFiles");

            photofiledir = ("" + (photofiledir ?? "")).Trim();
            if (!photofiledir.EndsWith(@"\"))
            {
                photofiledir += @"\";
            }

            var result = new List<string>();
            //var sql = "select photofilename from photorow where photofilename like @photofiledir + '%'";
            var sql = "select photofilename from photorow where photofilename like '" + photofiledir + "%'";
            using (SqlCommand cmdFiles = new SqlCommand(sql, connectionSql, sqlTransaction))
            {
                //cmdFiles.Parameters.AddWithValue("photofiledir", photofiledir);
                using (SqlDataReader reader = cmdFiles.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var photofilename = reader.GetString(0);
                        result.Add(photofilename);
                    }
                    reader.Close();
                }
            }

            return result.ToArray();
        }

        public static void Write(string photofilename, byte[] photofilebytes, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            var sql1 = "delete from photorow where photofilename = @photofilename";
            SqlCommand cmd1 = new SqlCommand(sql1, connectionSql, sqlTransaction);
            cmd1.Parameters.Add(new SqlParameter("photofilename", photofilename));
            cmd1.ExecuteNonQuery();
            cmd1.Dispose();

            var sql2 = "insert into photorow(photofilename, photofilebytes) values(@photofilename, @photofilebytes)";
            SqlCommand cmd2 = new SqlCommand(sql2, connectionSql, sqlTransaction);
            cmd2.Parameters.Add(new SqlParameter("photofilename", photofilename));
            cmd2.Parameters.Add(new SqlParameter("photofilebytes", photofilebytes));
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();
        }

        public static void Copy(string photofilenameSrc, string photofilenameDst, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            //var sql1 = "delete from photorow where photofilename = @photofilenameDst";
            //SqlCommand cmd1 = new SqlCommand(sql1, connectionSql, sqlTransaction);
            //cmd1.Parameters.Add(new SqlParameter("photofilenameDst", photofilenameDst));
            //cmd1.ExecuteNonQuery();
            //cmd1.Dispose();

            //var sql2 = "insert into photorow(photofilename, photofilebytes) select @photofilenameDst, photofilebytes from photorow where photofilename = @photofilenameSrc";
            //SqlCommand cmd2 = new SqlCommand(sql2, connectionSql, sqlTransaction);
            //cmd2.Parameters.Add(new SqlParameter("photofilenameSrc", photofilenameSrc));
            //cmd2.Parameters.Add(new SqlParameter("photofilenameDst", photofilenameDst));
            //cmd2.ExecuteNonQuery();
            //cmd2.Dispose();

            DateTime dt = DateTime.Now;
            var sql2 = "exec [dbo].[photorow_copy] @photofilenameSrc, @photofilenameDst";
            SqlCommand cmd2 = new SqlCommand(sql2, connectionSql, sqlTransaction);
            cmd2.Parameters.Add(new SqlParameter("photofilenameSrc", photofilenameSrc));
            cmd2.Parameters.Add(new SqlParameter("photofilenameDst", photofilenameDst));
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "2222222=" + (int)((DateTime.Now - dt).TotalMilliseconds) + "\n");
        }

        public static void Delete(string photofilename, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            var sql1 = "delete from photorow where photofilename = @photofilename";
            SqlCommand cmd1 = new SqlCommand(sql1, connectionSql, sqlTransaction);
            cmd1.Parameters.Add(new SqlParameter("photofilename", photofilename));
            cmd1.ExecuteNonQuery();
            cmd1.Dispose();
        }

        public static void DeleteAll(string photofiledir, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            if (string.IsNullOrEmpty(photofiledir)) throw new Exception("DeleteAll");

            photofiledir = ("" + (photofiledir ?? "")).Trim();
            if (!photofiledir.EndsWith(@"\"))
            {
                photofiledir += @"\";
            }

            var sql1 = "delete from photorow where photofilename like @photofiledir + '%'";
            SqlCommand cmd1 = new SqlCommand(sql1, connectionSql, sqlTransaction);
            cmd1.Parameters.Add(new SqlParameter("photofiledir", photofiledir));
            cmd1.ExecuteNonQuery();
            cmd1.Dispose();
        }

        public static string ImgFreeSquareRootFolder
        {
            get
            {
                return WebConfigurationManager.AppSettings["ImgFreeSquareRootFolder"];
            }
        }

        public static string ImgContentRootFolder
        {
            get
            {
                return WebConfigurationManager.AppSettings["ImgContentRootFolder"];
            }
        }
    }
}
