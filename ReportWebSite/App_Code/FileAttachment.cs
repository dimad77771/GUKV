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
    public class FileAttachment
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public bool IsFolder { get; set; }
        
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public byte[] Image { get; set; }
        public DateTime LastModified { get; set; }

        private static string GetFolderPath(string scope, int recordID)
        {
            return Path.Combine(WebConfigurationManager.AppSettings["ImgFreeSquareRootFolder"], scope, recordID.ToString());
        }

        public static IEnumerable<FileAttachment> Select(string scope, int recordID)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");

            yield return new FileAttachment()
            {
                ID = "\\ROOT",
                ParentID = "\\NONE",
                IsFolder = true,
                Name = "~\\",
            };

            if (recordID <= 0)
                yield break;

            string path = GetFolderPath(scope, recordID);
            if (!Directory.Exists(path))
                yield break;

            SqlConnection connectionSql = Utils.ConnectToDatabase();
            string query = "";
            if (scope.ToLower() == "1nf")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_balans_free_square_photos] WHERE [free_square_id] = @free_square_id";
			if (scope.ToLower() == "reports1nf_report_documents")
				query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_report_documents] WHERE [report_id] = @free_square_id";
			if (scope.ToLower() == "free_square_current_stage_documents")
				query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_balans_free_square_current_stage_documents] WHERE [free_square_id] = @free_square_id";
			if (scope.ToLower() == "freecycle_step_documents")
				query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [freecycle_step_documents] WHERE [free_square_id] = @free_square_id";
			if (scope.ToLower() == "balans")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [balans_free_square_photos] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "arch")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [arch_balans_free_square_photos] WHERE [free_square_id] = @free_square_id";

            if (string.IsNullOrEmpty(query))
                throw new ArgumentException("Unknown scope value");

            using (SqlCommand cmdFiles = new SqlCommand(query, connectionSql))
            {
                cmdFiles.Parameters.AddWithValue("free_square_id", recordID);
                using (SqlDataReader reader = cmdFiles.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string file_name = reader.GetString(1);
                        string file_ext = reader.GetString(2);
                        DateTime modify_date = reader.GetDateTime(3);

                        string fullPath = Path.Combine(path, file_name + file_ext);

                        if (File.Exists(fullPath))
                        {
                            yield return new FileAttachment()
                            {
                                ID = id.ToString(),
                                ParentID = "\\ROOT",
                                Name = file_name + file_ext,
                                Image = File.ReadAllBytes(fullPath),
                                LastModified = modify_date,
                            };
                        }
                    }

                    reader.Close();
                }
            }
        }

        public static IEnumerable<FileAttachment> SelectFromTempFolder(string scope, int recordID, string tempGuid)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");

            //yield return new FileAttachment()
            //{
            //    ID = "\\ROOT",
            //    ParentID = "\\NONE",
            //    IsFolder = true,
            //    Name = "~\\",
            //};

            if (recordID <= 0)
                yield break;

            string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
            string destFolder = Path.Combine(photoRootPath, scope + "_" + recordID.ToString() + "_" + tempGuid);

            if (!Directory.Exists(destFolder))
                yield break;

            string[] files = Directory.GetFiles(destFolder);
            foreach (string f in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(f);
                string fileExt = Path.GetExtension(f);

                //string imageUrl = String.Format("http://192.168.10.84/ReportWebSite/ImgContent/{0}_{1}_{2}/{3}{4}", scope, recordID.ToString(), tempGuid, fileName, fileExt);
                //string imageUrl = String.Format("http://localhost:6660/ReportWebSite/ImgContent/{0}_{1}_{2}/{3}{4}", scope, recordID.ToString(), tempGuid, fileName, fileExt);
                string imageUrl = String.Format("~/ImgContent/{0}_{1}_{2}/{3}{4}", scope, recordID.ToString(), tempGuid, fileName, fileExt);

                yield return new FileAttachment()
                {
                    ID = fileName,
                    ParentID = "\\ROOT",
                    Name = f,
                    Image = File.ReadAllBytes(f),
                    ImageUrl = imageUrl,
                    //LastModified = Path.,
                };
            }
        }

        public static void Insert(string scope, int recordID, string Name, byte[] Image)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");
            if (recordID <= 0)
                throw new ArgumentOutOfRangeException("recordID");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("fileName must have a value");
            if (Image == null || Image.Length == 0)
                throw new ArgumentException("image must have a value");

            string path = GetFolderPath(scope, recordID);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            MembershipUser user = Membership.GetUser();
            string modified_by = user == null ? String.Empty : (String)user.UserName;

            File.WriteAllBytes(Path.Combine(path, Name), Image);

            SqlConnection connectionSql = Utils.ConnectToDatabase();
			string table = "";
			string query = "";
			if (scope == "free_square_current_stage_documents")
			{
				table = "reports1nf_balans_free_square_current_stage_documents";
			}
			else if (scope == "reports1nf_report_documents")
			{
				table = "reports1nf_report_documents";
				query = "INSERT INTO [" + table + "] ([report_id],[file_name],[file_ext],[modify_date],[modified_by]) VALUES (@free_square_id,@file_name,@file_ext,@modify_date,@modified_by)";
			}
			else if (scope == "freecycle_step_documents")
			{
				table = "freecycle_step_documents";
			}
			else
			{
				table = "reports1nf_balans_free_square_photos";
			}
			if (query == "")
			{
				query = "INSERT INTO [" + table + "] ([free_square_id],[file_name],[file_ext],[modify_date],[modified_by]) VALUES (@free_square_id,@file_name,@file_ext,@modify_date,@modified_by)";
			}
			using (SqlCommand cmdFiles = new SqlCommand(query, connectionSql))
            {
                cmdFiles.Parameters.AddWithValue("free_square_id", recordID);
                cmdFiles.Parameters.AddWithValue("file_name", Path.GetFileNameWithoutExtension(Name));
                cmdFiles.Parameters.AddWithValue("file_ext", Path.GetExtension(Name));
                cmdFiles.Parameters.AddWithValue("modify_date", DateTime.Now);
                cmdFiles.Parameters.AddWithValue("modified_by", modified_by);

                cmdFiles.ExecuteNonQuery();
            }

            
        }

        public static void Delete(string scope, int recordID, string id /* attachmentID */)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");
            if (recordID <= 0)
                throw new ArgumentOutOfRangeException("recordID");
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("id must have a value");

            string name = id;
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("fileName must have a value");

            string path = GetFolderPath(scope, recordID);
            if (!Directory.Exists(path))
                return;

			string table = "";
			if (scope == "free_square_current_stage_documents")
			{
				table = "reports1nf_balans_free_square_current_stage_documents";
			}
			else if (scope == "reports1nf_report_documents")
			{
				table = "reports1nf_report_documents";
			}
			else if (scope == "freecycle_step_documents")
			{
				table = "freecycle_step_documents";
			}
			else
			{
				table = "reports1nf_balans_free_square_photos";
			}

			SqlConnection connectionSql = Utils.ConnectToDatabase();
            string query = "SELECT [file_name],[file_ext] FROM [" + table + "] WHERE [id] = @id";
            using (SqlCommand cmdFiles = new SqlCommand(query, connectionSql))
            {
                cmdFiles.Parameters.AddWithValue("id", id);
                using (SqlDataReader reader = cmdFiles.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string file_name = reader.GetString(0);
                        string file_ext = reader.GetString(1);
                        string fullPath = Path.Combine(path, file_name + file_ext);

                        if (Directory.Exists(path))
                            File.Delete(fullPath);

                    }
                    reader.Close();
                }
                using (SqlCommand cmdDelete = new SqlCommand("DELETE FROM [" + table + "] WHERE [id] = @id", connectionSql))
                {
                    cmdDelete.Parameters.AddWithValue("id", id);
                    cmdDelete.ExecuteNonQuery();
                }

                if (Directory.GetFiles(path).Length == 0)
                {
                    try
                    {
                        Directory.Delete(path);
                    }
                    catch { }
                }
            }




            //string path = GetFolderPath(scope, recordID);
            //if (Directory.Exists(path))
            //    File.Delete(Path.Combine(path, name));
        }

        public static void DeleteAll(string scope, int recordID)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");
            if (recordID <= 0)
                throw new ArgumentOutOfRangeException("recordID");

            string path = GetFolderPath(scope, recordID);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

    }
}
