using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Web;
using DevExpress.Web;
using System.Data;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using GUKV;
using System.Web.Configuration;
using System.IO;
using DevExpress.Web;
using ExtDataEntry.Models;
using DevExpress.Web;


public partial class Cards_DocCard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string docIdStr = Request.QueryString["docid"];

        if (docIdStr != null && docIdStr.Length > 0)
        {
            int documentId = int.Parse(docIdStr);

            // Set the parameters for SQL queries
            SqlDataSourceDocCardProperties.SelectParameters["doc_id"].DefaultValue = documentId.ToString();
            SqlDataSourceDocCardChildDocs.SelectParameters["m_doc_id"].DefaultValue = documentId.ToString();
            SqlDataSourceDocCardObjects.SelectParameters["doc_id"].DefaultValue = documentId.ToString();
        }

		PrepareTempPhotoFolder();
	}



	#region Photos

	protected string GetPageUniqueKey()
	{
		object key = ViewState["PageUniqueKey"];

		if (key is string)
		{
			return (string)key;
		}

		// Generate unique key
		Guid guid = Guid.NewGuid();

		string str = guid.ToString();

		ViewState["PageUniqueKey"] = str;

		return str;
	}

	protected Guid PhotoFolderID
	{
		get
		{
			//return string.IsNullOrEmpty(TempFolderIDField.Value) ? Guid.Empty : new Guid(TempFolderIDField.Value);

			object val = Session[GetPageUniqueKey() + "_PHOTO_GUID"];

			if (val is Guid)
			{
				return (Guid)val;
			}

			return Guid.Empty;
		}

		set
		{
			Session[GetPageUniqueKey() + "_PHOTO_GUID"] = value;
			//TempFolderIDField.Value = value.ToString(); ;
		}
	}

	private string TempPhotoFolder()
	{
		if (PhotoFolderID != Guid.Empty)
		{
			string agreementIdStr = Request.QueryString["docid"];
			string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
			string destFolder = Path.Combine(photoRootPath, "1NF_" + agreementIdStr + "_" + PhotoFolderID.ToString()).ToLower();
			//            string destFolder = Path.Combine(photoRootPath, "Tmp","1NF_" + agreementIdStr + "_" + PhotoFolderID.ToString()).ToLower();


			if (!Directory.Exists(destFolder))
				Directory.CreateDirectory(destFolder);
			return destFolder;
		}
		else
			return string.Empty;
	}

	private void PrepareTempPhotoFolder()
	{
		string agreementIdStr = Request.QueryString["docid"];

		if (PhotoFolderID == Guid.Empty)
		{
			PhotoFolderID = Guid.NewGuid();

			CopySourceFiles(agreementIdStr);
		}

		BindImageGallery(agreementIdStr);
	}

	private void CopySourceFiles(string documentIdStr)
	{


		string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];

		string destFolder = TempPhotoFolder();

		if (!Directory.Exists(destFolder))
			Directory.CreateDirectory(destFolder);
		else
		{
		}


		SqlConnection connection = Utils.ConnectToDatabase();
		using (SqlCommand cmd = new SqlCommand("select id, file_name, file_ext from documents_photos where document_id = @docid", connection))
		{
			cmd.Parameters.AddWithValue("docid", documentIdStr);
			using (SqlDataReader r = cmd.ExecuteReader())
			{
				while (r.Read())
				{
					int id = r.GetInt32(0);
					string file_name = r.GetString(1);
					string file_ext = r.GetString(2);

					string sourceFileToCopy = Path.Combine(photoRootPath, "DOCUMENTPHOTO", documentIdStr, id.ToString() + file_ext);
					if (File.Exists(sourceFileToCopy))
					{
						string destFileToCopy = Path.Combine(destFolder, PhotoUtils.DbFilename2LocalFilename(file_name, file_ext));
						if (File.Exists(destFileToCopy))
							File.Delete(destFileToCopy);
						File.Copy(sourceFileToCopy, destFileToCopy);
					}
				}

				r.Close();
			}
		}
		connection.Close();
	}

	private void BindImageGallery(string documentIdStr)
	{
		ObjectDataSourceBalansPhoto.SelectParameters["recordID"].DefaultValue = documentIdStr;
		ObjectDataSourceBalansPhoto.SelectParameters["tempGuid"].DefaultValue = PhotoFolderID.ToString();
		imageGalleryDemo.DataSourceID = "ObjectDataSourceBalansPhoto";
		imageGalleryDemo.DataBind();

	}
	protected void TempFolderIDField_ValueChanged(object sender, EventArgs e)
	{
		//PhotoFolderID = new Guid(TempFolderIDField.Value);
	}
	protected void btnUpload_Click(object sender, EventArgs e)
	{

	}

	protected void ContentCallback_Callback(object sender, CallbackEventArgsBase e)
	{


		if (e.Parameter.ToLower().StartsWith("deleteimage:"))
		{
			DeleteImage(e.Parameter.Split(':')[1], string.Empty);
			BindImageGallery(Request.QueryString["docid"]);
			SavePhotoChanges();
			//imageGalleryDemo.DataBind();
			//ASPxFileManagerPhotoFiles.Refresh();
		}
	}
	protected void imageGalleryDemo_DataBound(object sender, EventArgs e)
	{
		if ((imageGalleryDemo.PageIndex > imageGalleryDemo.PageCount) && (IsCallback))
			imageGalleryDemo.PageIndex = 0;
	}

	protected void imageGalleryDemo_CustomCallback(object sender, CallbackEventArgsBase e)
	{
		ContentCallback_Callback(sender, e);
	}

	protected void ASPxCallbackPanelImageGallery_Callback(object sender, CallbackEventArgsBase e)
	{
		//if (!IsCallback)
		{
			if (e.Parameter.ToLower() == "refreshphoto:")
			{
				//imageGalleryDemo.DataBind();
				BindImageGallery(Request.QueryString["docid"]);

			}
		}
	}

	protected void delete_Callback(object source, CallbackEventArgs e)
	{
		string indexStr = e.Parameter;
		string imageUrl = string.Empty;

		imageUrl = DeleteImage(indexStr, imageUrl);
		e.Result = imageUrl;
	}

	private string DeleteImage(string indexStr, string imageUrl)
	{
		string archiveIdStr = Request.QueryString["arid"];
		if (archiveIdStr != null && archiveIdStr.Length > 0)
			throw new Exception("Ви не можете видалити фото з архівного стану.");


		string documentId = Request.QueryString["docid"];

		if (imageGalleryDemo != null)
		{
			if (imageGalleryDemo.Items.Count == 0)
				BindImageGallery(documentId);

			ImageGalleryItem item = imageGalleryDemo.Items[int.Parse(indexStr)];
			FileAttachment drv = (FileAttachment)item.DataItem;
			string fileExt = Path.GetExtension(drv.Name);

			if (File.Exists(drv.Name))
				File.Delete(drv.Name);


			imageUrl = item.ImageUrl;

		}
		return imageUrl;
	}

	protected void ASPxUploadPhotoControl_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
	{
		//this.LoadViewState("PageUniqueKey");

		string agreementIdStr = Request.QueryString["docid"];

		string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
		string serverLocal1NFObjectFolder = Path.Combine(photoRootPath, "DOCUMENTPHOTO", agreementIdStr);



		string fullPath = string.Empty;

		try
		{
			if (!System.IO.Directory.Exists(serverLocal1NFObjectFolder))
				System.IO.Directory.CreateDirectory(serverLocal1NFObjectFolder);

			PhotoUtils.AddUploadedFile(TempPhotoFolder(), e.UploadedFile.FileName, e.UploadedFile.FileBytes);

			SavePhotoChanges();
		}
		catch (Exception ex)
		{
			if ((!string.IsNullOrEmpty(fullPath)) && (System.IO.File.Exists(fullPath)))
				System.IO.File.Delete(fullPath);
			throw ex;
		}


	}


	private void SavePhotoChanges()
	{
		string documentIdStr = Request.QueryString["docid"];
		Int32 newId = 0;
		string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
		string serverLocal1NFObjectFolder = Path.Combine(photoRootPath, "DOCUMENTPHOTO", documentIdStr);

		//if (!Directory.Exists(TempPhotoFolder())) throw new Exception("Temp Photo Folder not found");

		if (Directory.Exists(serverLocal1NFObjectFolder))
		{
			foreach (string fileToDelete in Directory.GetFiles(serverLocal1NFObjectFolder))
			{
				File.Delete(fileToDelete);
			}

		}

		SqlConnection connection = Utils.ConnectToDatabase();
		SqlTransaction trans = connection.BeginTransaction();

		try
		{
			using (SqlCommand cmd = new SqlCommand("delete from documents_photos where document_id = @docid", connection, trans))
			{
				cmd.Parameters.AddWithValue("docid", int.Parse(documentIdStr));
				cmd.ExecuteNonQuery();
			}

			var allfiles = Directory.GetFiles(TempPhotoFolder());
			foreach (string filePath in allfiles)
			{
				var dbfile = PhotoUtils.LocalFilename2DbFilename(filePath);
				string fullPath = string.Empty;

				SqlCommand cmd = new SqlCommand("INSERT INTO documents_photos (document_id, file_name, file_ext, user_id, create_date) VALUES (@docid, @filename, @fileext, @usrid, @createdate); ; SELECT CAST(SCOPE_IDENTITY() AS int)", connection, trans);
				cmd.Parameters.Add(new SqlParameter("docid", int.Parse(documentIdStr)));
				cmd.Parameters.Add(new SqlParameter("filename", dbfile.file_name));
				cmd.Parameters.Add(new SqlParameter("fileext", dbfile.file_ext));
				cmd.Parameters.Add(new SqlParameter("usrid", (Guid)Membership.GetUser().ProviderUserKey));
				cmd.Parameters.Add(new SqlParameter("createdate", DateTime.Now));
				newId = (Int32)cmd.ExecuteScalar();

				fullPath = System.IO.Path.Combine(serverLocal1NFObjectFolder, newId.ToString() + System.IO.Path.GetExtension(filePath));

				if (!System.IO.Directory.Exists(serverLocal1NFObjectFolder))
					System.IO.Directory.CreateDirectory(serverLocal1NFObjectFolder);

				File.Copy(filePath, fullPath);
			}

			trans.Commit();
			connection.Close();
		}
		catch
		{
			trans.Rollback();
			throw;
		}


		//!!! //pgv
		//!!! if (Directory.Exists(TempPhotoFolder()))
		//!!!	Directory.Delete(TempPhotoFolder(), true);
	}

	#endregion


}