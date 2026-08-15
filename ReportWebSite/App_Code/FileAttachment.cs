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
        private const string ArendaInsuranceScope =
            "reports1nf_arenda_insurance_attachfiles";

        public string ID { get; set; }
        public string ParentID { get; set; }
        public bool IsFolder { get; set; }
        
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public byte[] Image { get; set; }
        public DateTime LastModified { get; set; }

        private static string GetFolderPath(string scope, int recordID)
        {
            return Path.Combine(LLLLhotorowUtils.ImgFreeSquareRootFolder, scope, recordID.ToString());
        }

        private static string GetArendaInsuranceFolderPath(int reportID, int arendaID)
        {
            return Path.Combine(
                LLLLhotorowUtils.ImgFreeSquareRootFolder,
                ArendaInsuranceScope,
                reportID.ToString(),
                arendaID.ToString());
        }

		public static IEnumerable<FileAttachment> Select(string scope, int recordID)
		{
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "scope=" + scope + "\n");
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "recordID=" + recordID + "\n");

            FileAttachment[] result;

			if (scope == "reports1nf_arenda_dogcontinue___all")
			{
				if (recordID >= 1000000)
				{
					result = SelectCore("reports1nf_arenda_dogcontinue_current_stage_documents", recordID).ToArray();
				}
				else
				{
					result = SelectCore("1NF", recordID).ToArray();
				}
			}
			else
			{
				result = SelectCore(scope, recordID).ToArray();
			}

			return result;
		}

		public static IEnumerable<FileAttachment> SelectCore(string scope, int recordID)
        {
			//System.Diagnostics.Debug.WriteLine("scope=" + scope + "\t" + "recordID=" + recordID);

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

            SqlConnection connectionSql = Utils.ConnectToDatabase();
            string query = "";
            if (scope.ToLower() == "1nf")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_balans_free_square_photos] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "reports1nf_arenda_dogcontinue_photos")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_arenda_dogcontinue_photos] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "reports1nf_report_documents")
				query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_report_documents] WHERE [report_id] = @free_square_id";
            if (scope.ToLower() == "reports1nf_inventar_documents")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_inventar_documents] WHERE [report_id] = @free_square_id";
            if (scope.ToLower() == "free_square_current_stage_documents")
				query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_balans_free_square_current_stage_documents] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "reports1nf_arenda_dogcontinue_current_stage_documents")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_arenda_dogcontinue_current_stage_documents] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "privatisat_documents")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [privatisat_documents] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "freecycle_step_documents")
				query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [freecycle_step_documents] WHERE [free_square_id] = @free_square_id";
			if (scope.ToLower() == "balans")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [balans_free_square_photos] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "arch")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [arch_balans_free_square_photos] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "documents_attachfiles")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [documents_attachfiles] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "transfer_requests_rish_attachfiles")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [transfer_requests_rish_attachfiles] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "transfer_requests_akt_attachfiles")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [transfer_requests_akt_attachfiles] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "reports1nf_balans_rish_attachfiles")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_balans_rish_attachfiles] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "reports1nf_arenda_scandocument_attachfiles")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_arenda_scandocument_attachfiles] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "reports1nf_balans_akt_attachfiles")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_balans_akt_attachfiles] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "reports1nf_balans_bti_attachfiles")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_balans_bti_attachfiles] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "reports1nf_balans_dinfo_attachfiles")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_balans_dinfo_attachfiles] WHERE [free_square_id] = @free_square_id";
            if (scope.ToLower() == "reports1nf_balans_znizhino_attachfiles")
                query = "SELECT [id],[file_name],[file_ext],[modify_date],[modified_by] FROM [reports1nf_balans_znizhino_attachfiles] WHERE [free_square_id] = @free_square_id";


            if (string.IsNullOrEmpty(query))
                throw new ArgumentException("Unknown scope value");

            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "query=" + query + "\n");
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "recordID=" + recordID + "\n");
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

                        //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "fullPath=" + fullPath + "\n");
                        var image = LLLLhotorowUtils.Read(fullPath, connectionSql);
                        if (image != null)
                        {
                            yield return new FileAttachment()
                            {
                                ID = id.ToString(),
                                ParentID = "\\ROOT",
                                Name = file_name + file_ext,
                                Image = LLLLhotorowUtils.Read(fullPath, connectionSql),
                                LastModified = modify_date,
                            };
                        }
                    }

                    reader.Close();
                }
            }
        }

        public static IEnumerable<FileAttachment> SelectArendaInsurance(
            int reportID, int arendaID)
        {
            yield return new FileAttachment()
            {
                ID = "\\ROOT",
                ParentID = "\\NONE",
                IsFolder = true,
                Name = "~\\",
            };

            if (reportID <= 0 || arendaID <= 0)
                yield break;

            string path = GetArendaInsuranceFolderPath(reportID, arendaID);
            const string query = @"
SELECT [id], [file_name], [file_ext], [modify_date], [modified_by]
FROM [reports1nf_arenda_insurance_attachfiles]
WHERE [report_id] = @report_id AND [arenda_id] = @arenda_id
ORDER BY [id]";

            using (SqlConnection connectionSql = Utils.ConnectToDatabase())
            {
                if (connectionSql == null)
                    throw new InvalidOperationException("Database connection is not available");

                using (SqlCommand cmdFiles = new SqlCommand(query, connectionSql))
                {
                    cmdFiles.Parameters.AddWithValue("report_id", reportID);
                    cmdFiles.Parameters.AddWithValue("arenda_id", arendaID);

                    using (SqlDataReader reader = cmdFiles.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string fileName = reader.GetString(1);
                            string fileExt = reader.GetString(2);
                            DateTime modifyDate = reader.GetDateTime(3);
                            string fullPath = Path.Combine(path, fileName + fileExt);
                            byte[] image = LLLLhotorowUtils.Read(fullPath, connectionSql);

                            if (image != null)
                            {
                                yield return new FileAttachment()
                                {
                                    ID = id.ToString(),
                                    ParentID = "\\ROOT",
                                    Name = fileName + fileExt,
                                    Image = image,
                                    LastModified = modifyDate,
                                };
                            }
                        }
                    }
                }
            }
        }

        public static void InsertArendaInsurance(
            int reportID, int arendaID, string Name, byte[] Image)
        {
            if (reportID <= 0)
                throw new ArgumentOutOfRangeException("reportID");
            if (arendaID <= 0)
                throw new ArgumentOutOfRangeException("arendaID");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("fileName must have a value");
            if (Image == null || Image.Length == 0)
                throw new ArgumentException("image must have a value");

            string safeName = Path.GetFileName(Name);
            if (string.IsNullOrEmpty(safeName))
                throw new ArgumentException("fileName must have a value");

            string path = GetArendaInsuranceFolderPath(reportID, arendaID);

            using (SqlConnection connectionSql = Utils.ConnectToDatabase())
            {
                if (connectionSql == null)
                    throw new InvalidOperationException("Database connection is not available");

                using (SqlTransaction sqlTransaction = connectionSql.BeginTransaction())
                {
                    const string query = @"
INSERT INTO [reports1nf_arenda_insurance_attachfiles]
    ([report_id], [arenda_id], [file_name], [file_ext], [modify_date], [modified_by])
VALUES
    (@report_id, @arenda_id, @file_name, @file_ext, @modify_date, @modified_by)";

                    MembershipUser user = Membership.GetUser();
                    string modifiedBy = user == null ? String.Empty : (String)user.UserName;

                    using (SqlCommand cmdFiles = new SqlCommand(query, connectionSql, sqlTransaction))
                    {
                        cmdFiles.Parameters.AddWithValue("report_id", reportID);
                        cmdFiles.Parameters.AddWithValue("arenda_id", arendaID);
                        cmdFiles.Parameters.AddWithValue(
                            "file_name", Path.GetFileNameWithoutExtension(safeName));
                        cmdFiles.Parameters.AddWithValue("file_ext", Path.GetExtension(safeName));
                        cmdFiles.Parameters.AddWithValue("modify_date", DateTime.Now);
                        cmdFiles.Parameters.AddWithValue("modified_by", modifiedBy);
                        cmdFiles.ExecuteNonQuery();
                    }

                    LLLLhotorowUtils.Write(
                        Path.Combine(path, safeName), Image, connectionSql, sqlTransaction);

                    sqlTransaction.Commit();
                }
            }
        }

        public static void DeleteArendaInsurance(
            int reportID, int arendaID, string id)
        {
            if (reportID <= 0)
                throw new ArgumentOutOfRangeException("reportID");
            if (arendaID <= 0)
                throw new ArgumentOutOfRangeException("arendaID");

            int attachmentID;
            if (!Int32.TryParse(id, out attachmentID) || attachmentID <= 0)
                throw new ArgumentException("id must contain a valid attachment ID");

            string path = GetArendaInsuranceFolderPath(reportID, arendaID);

            using (SqlConnection connectionSql = Utils.ConnectToDatabase())
            {
                if (connectionSql == null)
                    throw new InvalidOperationException("Database connection is not available");

                using (SqlTransaction sqlTransaction = connectionSql.BeginTransaction())
                {
                    const string selectQuery = @"
SELECT [file_name], [file_ext]
FROM [reports1nf_arenda_insurance_attachfiles]
WHERE [id] = @id AND [report_id] = @report_id AND [arenda_id] = @arenda_id";

                    string fullPath = null;
                    using (SqlCommand cmdFiles = new SqlCommand(
                        selectQuery, connectionSql, sqlTransaction))
                    {
                        cmdFiles.Parameters.AddWithValue("id", attachmentID);
                        cmdFiles.Parameters.AddWithValue("report_id", reportID);
                        cmdFiles.Parameters.AddWithValue("arenda_id", arendaID);

                        using (SqlDataReader reader = cmdFiles.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                fullPath = Path.Combine(
                                    path, reader.GetString(0) + reader.GetString(1));
                            }
                        }
                    }

                    if (fullPath == null)
                        throw new InvalidOperationException("Attachment was not found");

                    const string deleteQuery = @"
DELETE FROM [reports1nf_arenda_insurance_attachfiles]
WHERE [id] = @id AND [report_id] = @report_id AND [arenda_id] = @arenda_id";

                    using (SqlCommand cmdDelete = new SqlCommand(
                        deleteQuery, connectionSql, sqlTransaction))
                    {
                        cmdDelete.Parameters.AddWithValue("id", attachmentID);
                        cmdDelete.Parameters.AddWithValue("report_id", reportID);
                        cmdDelete.Parameters.AddWithValue("arenda_id", arendaID);

                        if (cmdDelete.ExecuteNonQuery() != 1)
                            throw new InvalidOperationException("Attachment was not deleted");
                    }

                    LLLLhotorowUtils.Delete(fullPath, connectionSql, sqlTransaction);
                    sqlTransaction.Commit();
                }
            }
        }

        public static IEnumerable<FileAttachment> SelectFromTempFolder(string scope, int recordID, string tempGuid)
        {
            DateTime nw = DateTime.Now;

            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");

            if (recordID <= 0)
                yield break;
            
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-000:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
            var fileAttachments = new List<FileAttachment>();
            var connectionSql = Utils.ConnectToDatabase();
            string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
            string destFolder = Path.Combine(photoRootPath, scope + "_" + recordID.ToString() + "_" + tempGuid + "_a");

            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-001a:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
            string[] files = LLLLhotorowUtils.GetFiles(destFolder, connectionSql);
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-001b:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
            foreach (string f in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(f);
                string fileExt = Path.GetExtension(f);

                string imageUrl = String.Format("~/ImgContent/{0}_{1}_{2}_a/{3}{4}", scope, recordID.ToString(), tempGuid, fileName, fileExt);

                //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-002:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
                var fileAttachment = new FileAttachment()
                {
                    ID = fileName,
                    ParentID = "\\ROOT",
                    Name = f,
                    Image = LLLLhotorowUtils.Read(f, connectionSql),
                    ImageUrl = imageUrl,
                };
                //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-003:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
                fileAttachments.Add(fileAttachment);
            }

            foreach (var fileAttachment in fileAttachments)
            {
                yield return fileAttachment;
            }
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-010:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
        }

        public static IEnumerable<FileAttachment> SelectFromTempFolder__2(string scope, int recordID, string tempGuid)
        {
            DateTime nw = DateTime.Now;

            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");

            if (recordID <= 0)
                yield break;

            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-000:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
            var fileAttachments = new List<FileAttachment>();
            var connectionSql = Utils.ConnectToDatabase();
            string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
            string destFolder = Path.Combine(photoRootPath, scope + "_" + recordID.ToString() + "_" + tempGuid + "_b");

            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-001a:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
            string[] files = LLLLhotorowUtils.GetFiles(destFolder, connectionSql);
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-001b:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
            foreach (string f in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(f);
                string fileExt = Path.GetExtension(f);

                string imageUrl = String.Format("~/ImgContent/{0}_{1}_{2}_b/{3}{4}", scope, recordID.ToString(), tempGuid, fileName, fileExt);

                //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-002:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
                var fileAttachment = new FileAttachment()
                {
                    ID = fileName,
                    ParentID = "\\ROOT",
                    Name = f,
                    Image = LLLLhotorowUtils.Read(f, connectionSql),
                    ImageUrl = imageUrl,
                };
                //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-003:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
                fileAttachments.Add(fileAttachment);
            }

            foreach (var fileAttachment in fileAttachments)
            {
                yield return fileAttachment;
            }
            //File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "SelectFromTempFolder-010:" + (DateTime.Now - nw).TotalMilliseconds + "\n");
        }

        public static IEnumerable<FileAttachment> SelectFromTransferRequestFolder(string scope, string tempGuid)
		{
			if (string.IsNullOrEmpty(scope))
				throw new ArgumentException("scope must have a value");

			var fileAttachments = new List<FileAttachment>();
			var connectionSql = Utils.ConnectToDatabase();
			string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
			string destFolder = Path.Combine(photoRootPath, scope + "_" + tempGuid);

			string[] files = LLLLhotorowUtils.GetFiles(destFolder, connectionSql);
			foreach (string f in files)
			{
				string fileName = Path.GetFileNameWithoutExtension(f);
				string fileExt = Path.GetExtension(f);

				string imageUrl = String.Format("~/ImgContent/{0}_{1}/{2}{3}", scope, tempGuid, fileName, fileExt);

				var fileAttachment = new FileAttachment()
				{
					ID = fileName,
					ParentID = "\\ROOT",
					Name = f,
					Image = LLLLhotorowUtils.Read(f, connectionSql),
					ImageUrl = imageUrl,
				};
				fileAttachments.Add(fileAttachment);
			}

			foreach (var fileAttachment in fileAttachments)
			{
				yield return fileAttachment;
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

            var connectionSql = Utils.ConnectToDatabase();
            var sqlTransaction = connectionSql.BeginTransaction();

            MembershipUser user = Membership.GetUser();
            string modified_by = user == null ? String.Empty : (String)user.UserName;

            LLLLhotorowUtils.Write(Path.Combine(path, Name), Image, connectionSql, sqlTransaction);

			string table = "";
			string query = "";
			if (scope == "free_square_current_stage_documents")
			{
				table = "reports1nf_balans_free_square_current_stage_documents";
			}
            else if (scope == "reports1nf_arenda_dogcontinue_current_stage_documents")
            {
                table = "reports1nf_arenda_dogcontinue_current_stage_documents";
            }
            else if (scope == "privatisat_documents")
            {
                table = "privatisat_documents";
            }
            else if (scope == "reports1nf_report_documents")
			{
				table = "reports1nf_report_documents";
				query = "INSERT INTO [" + table + "] ([report_id],[file_name],[file_ext],[modify_date],[modified_by]) VALUES (@free_square_id,@file_name,@file_ext,@modify_date,@modified_by)";
			}
            else if (scope == "reports1nf_inventar_documents")
            {
                table = "reports1nf_inventar_documents";
                query = "INSERT INTO [" + table + "] ([report_id],[file_name],[file_ext],[modify_date],[modified_by]) VALUES (@free_square_id,@file_name,@file_ext,@modify_date,@modified_by)";
            }
            else if (scope == "freecycle_step_documents")
			{
				table = "freecycle_step_documents";
			}
            else if (scope == "reports1nf_arenda_dogcontinue_photos")
            {
                table = "reports1nf_arenda_dogcontinue_photos";
            }
            else if (scope == "documents_attachfiles")
            {
                table = "documents_attachfiles";
            }
            else if (scope == "transfer_requests_rish_attachfiles")
            {
                table = "transfer_requests_rish_attachfiles";
            }
            else if (scope == "transfer_requests_akt_attachfiles")
            {
                table = "transfer_requests_akt_attachfiles";
            }
            else if (scope == "reports1nf_balans_rish_attachfiles")
            {
                table = "reports1nf_balans_rish_attachfiles";
            }
            else if (scope == "reports1nf_arenda_scandocument_attachfiles")
            {
                table = "reports1nf_arenda_scandocument_attachfiles";
            }
            else if (scope == "reports1nf_balans_akt_attachfiles")
            {
                table = "reports1nf_balans_akt_attachfiles";
            }
            else if (scope == "reports1nf_balans_bti_attachfiles")
            {
                table = "reports1nf_balans_bti_attachfiles";
            }
            else if (scope == "reports1nf_balans_dinfo_attachfiles")
            {
                table = "reports1nf_balans_dinfo_attachfiles";
            }
            else if (scope == "reports1nf_balans_znizhino_attachfiles")
            {
                table = "reports1nf_balans_znizhino_attachfiles";
            }
            else
            {
				table = "reports1nf_balans_free_square_photos";
			}
			if (query == "")
			{
				query = "INSERT INTO [" + table + "] ([free_square_id],[file_name],[file_ext],[modify_date],[modified_by]) VALUES (@free_square_id,@file_name,@file_ext,@modify_date,@modified_by)";
			}
			using (SqlCommand cmdFiles = new SqlCommand(query, connectionSql, sqlTransaction))
            {
                cmdFiles.Parameters.AddWithValue("free_square_id", recordID);
                cmdFiles.Parameters.AddWithValue("file_name", Path.GetFileNameWithoutExtension(Name));
                cmdFiles.Parameters.AddWithValue("file_ext", Path.GetExtension(Name));
                cmdFiles.Parameters.AddWithValue("modify_date", DateTime.Now);
                cmdFiles.Parameters.AddWithValue("modified_by", modified_by);

                cmdFiles.ExecuteNonQuery();
            }

            sqlTransaction.Commit();
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

			string table = "";
			if (scope == "free_square_current_stage_documents")
			{
				table = "reports1nf_balans_free_square_current_stage_documents";
			}
            else if (scope == "reports1nf_arenda_dogcontinue_current_stage_documents")
            {
                table = "reports1nf_arenda_dogcontinue_current_stage_documents";
            }
            else if (scope == "privatisat_documents")
            {
                table = "privatisat_documents";
            }
            else if (scope == "reports1nf_report_documents")
			{
				table = "reports1nf_report_documents";
			}
            else if (scope == "reports1nf_inventar_documents")
            {
                table = "reports1nf_inventar_documents";
            }
            else if (scope == "freecycle_step_documents")
			{
				table = "freecycle_step_documents";
			}
            else if (scope == "reports1nf_arenda_dogcontinue_photos")
            {
                table = "reports1nf_arenda_dogcontinue_photos";
            }
            else if (scope == "documents_attachfiles")
            {
                table = "documents_attachfiles";
            }
            else if (scope == "transfer_requests_rish_attachfiles")
            {
                table = "transfer_requests_rish_attachfiles";
            }
            else if (scope == "transfer_requests_akt_attachfiles")
            {
                table = "transfer_requests_akt_attachfiles";
            }
            else if (scope == "reports1nf_balans_rish_attachfiles")
            {
                table = "reports1nf_balans_rish_attachfiles";
            }
            else if (scope == "reports1nf_arenda_scandocument_attachfiles")
            {
                table = "reports1nf_arenda_scandocument_attachfiles";
            }
            else if (scope == "reports1nf_balans_akt_attachfiles")
            {
                table = "reports1nf_balans_akt_attachfiles";
            }
            else if (scope == "reports1nf_balans_bti_attachfiles")
            {
                table = "reports1nf_balans_bti_attachfiles";
            }
            else if (scope == "reports1nf_balans_dinfo_attachfiles")
            {
                table = "reports1nf_balans_dinfo_attachfiles";
            }
            else if (scope == "reports1nf_balans_znizhino_attachfiles")
            {
                table = "reports1nf_balans_znizhino_attachfiles";
            }
            else
            {
				table = "reports1nf_balans_free_square_photos";
			}

            var connectionSql = Utils.ConnectToDatabase();
            var sqlTransaction = connectionSql.BeginTransaction();
            string query = "SELECT [file_name],[file_ext] FROM [" + table + "] WHERE [id] = @id";
            using (SqlCommand cmdFiles = new SqlCommand(query, connectionSql, sqlTransaction))
            {
                cmdFiles.Parameters.AddWithValue("id", id);
                using (SqlDataReader reader = cmdFiles.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string file_name = reader.GetString(0);
                        string file_ext = reader.GetString(1);
                        string fullPath = Path.Combine(path, file_name + file_ext);

                        LLLLhotorowUtils.Delete(fullPath, connectionSql, sqlTransaction);
                    }
                    reader.Close();
                }
                using (SqlCommand cmdDelete = new SqlCommand("DELETE FROM [" + table + "] WHERE [id] = @id", connectionSql, sqlTransaction))
                {
                    cmdDelete.Parameters.AddWithValue("id", id);
                    cmdDelete.ExecuteNonQuery();
                }

                sqlTransaction.Commit();
            }
        }

        public static void DeleteAll(string scope, int recordID)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");
            if (recordID <= 0)
                throw new ArgumentOutOfRangeException("recordID");

            string path = GetFolderPath(scope, recordID);

            var connectionSql = Utils.ConnectToDatabase();
            var sqlTransaction = connectionSql.BeginTransaction();

            LLLLhotorowUtils.DeleteAll(path, connectionSql, sqlTransaction);

            sqlTransaction.Commit();
        }

    }
}
