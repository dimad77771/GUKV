using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web.ASPxImageGallery;
using DevExpress.Web.ASPxCallback;
using System.Web.Configuration;
using System.IO;
using DevExpress.Web.ASPxClasses;
using GUKV.Common;

public partial class Cards_BalansCardArchive : PhotoPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string balansIdStr = Request.QueryString["balid"];
        string archiveIdStr = Request.QueryString["arid"];

        string tab = Request.QueryString["tab"];
        if (!string.IsNullOrEmpty(tab))
            CardPageControl.ActiveTabIndex = int.Parse(tab);

        if (balansIdStr != null && balansIdStr.Length > 0)
        {
            SqlDataSourceBalansCardProperties.SelectParameters["balid"].DefaultValue = balansIdStr.Trim();
            SqlDataSourceBalansCardPropertiesArch.SelectParameters["arid"].DefaultValue = "0";

            SqlDataSourceBalansCardDocs.SelectParameters["balid"].DefaultValue = balansIdStr.Trim();
            SqlDataSourceBalansCardDocsArch.SelectParameters["arid"].DefaultValue = "0";

            SqlDataSourceBalansArchive.SelectParameters["balid"].DefaultValue = balansIdStr.Trim();

            SqlDataSourcePhoto.SelectParameters["bal_id"].DefaultValue = balansIdStr.ToString();
            SqlDataSourcePhoto.SelectParameters["folder_prefix"].DefaultValue = "~/ImgContent/Balans/";

            SqlDataSourceFreeSquare.SelectParameters["balans_id"].DefaultValue = balansIdStr.ToString();

            ObjectDataSourcePhotoFiles.SelectParameters["RecordID"].DefaultValue = balansIdStr.ToString();

            BalansDetails.DataSourceID = "SqlDataSourceBalansCardProperties";
            BalansDetailsPage2.DataSourceID = "SqlDataSourceBalansCardProperties";
            BalansDetailsPage3.DataSourceID = "SqlDataSourceBalansCardProperties";
            GridViewBalansCardDocs.DataSourceID = "SqlDataSourceBalansCardDocs";

            imageGalleryDemo.DataSourceID = "SqlDataSourcePhoto";
            ASPxGridViewFreeSquare.DataSourceID = "SqlDataSourceFreeSquare";
            ASPxFileManagerPhotoFiles.DataSourceID = "ObjectDataSourcePhotoFiles";

            BalansDetails.DataBind();
            BalansDetailsPage2.DataBind();
            BalansDetailsPage3.DataBind();
            GridViewBalansCardDocs.DataBind();

            imageGalleryDemo.DataBind();
            ASPxGridViewFreeSquare.DataBind();
            ASPxFileManagerPhotoFiles.DataBind();

            this.Title = Resources.Strings.BalansCardTitle;
            this.CardPageControl.TabPages[0].Text = Resources.Strings.BalansCardTitle;
        }

        if (archiveIdStr != null && archiveIdStr.Length > 0)
        {
            SqlDataSourceBalansCardProperties.SelectParameters["balid"].DefaultValue = "0";
            SqlDataSourceBalansCardPropertiesArch.SelectParameters["arid"].DefaultValue = archiveIdStr.Trim();

            SqlDataSourceBalansCardDocs.SelectParameters["balid"].DefaultValue = "0";
            SqlDataSourceBalansCardDocsArch.SelectParameters["arid"].DefaultValue = archiveIdStr.Trim();

            SqlDataSourcePhotoArch.SelectParameters["archive_id"].DefaultValue = archiveIdStr.ToString();
            SqlDataSourcePhotoArch.SelectParameters["folder_prefix"].DefaultValue = "~/ImgContent/Arch/";


            SqlDataSourceFreeSquareArch.SelectParameters["archive_id"].DefaultValue = archiveIdStr.ToString();
            ObjectDataSourcePhotoFilesArch.SelectParameters["RecordID"].DefaultValue = archiveIdStr.ToString();

            BalansDetails.DataSourceID = "SqlDataSourceBalansCardPropertiesArch";
            BalansDetailsPage2.DataSourceID = "SqlDataSourceBalansCardPropertiesArch";
            BalansDetailsPage3.DataSourceID = "SqlDataSourceBalansCardPropertiesArch";
            GridViewBalansCardDocs.DataSourceID = "SqlDataSourceBalansCardDocsArch";

            imageGalleryDemo.DataSourceID = "SqlDataSourcePhotoArch";
            ASPxGridViewFreeSquare.DataSourceID = "SqlDataSourceFreeSquareArch";
            ASPxFileManagerPhotoFiles.DataSourceID = "ObjectDataSourcePhotoFilesArch";

            BalansDetails.DataBind();
            BalansDetailsPage2.DataBind();
            BalansDetailsPage3.DataBind();
            GridViewBalansCardDocs.DataBind();

            imageGalleryDemo.DataBind();
            ASPxGridViewFreeSquare.DataBind();
            ASPxFileManagerPhotoFiles.DataBind();

            this.Title = Resources.Strings.BalansCardTitleArch;
            this.CardPageControl.TabPages[0].Text = Resources.Strings.BalansCardTitleArch;

            uploadPanel.Visible = false;
        }

        // Check if user has permissions to view this balans object
        if (PermissionGranted == 0 && Utils.RdaDistrictID > 0)
        {
            // Prevent access by default
            PermissionGranted = -1;

            if (!string.IsNullOrEmpty(balansIdStr))
            {
                ValidatePermission(int.Parse(balansIdStr), -1);
            }
            else if (!string.IsNullOrEmpty(archiveIdStr))
            {
                ValidatePermission(-1, int.Parse(archiveIdStr));
            }
        }

        if (PermissionGranted < 0)
        {
            Response.Redirect(Page.ResolveClientUrl("~/Account/RestrictedObject.aspx"));
        }
    }

    protected int PermissionGranted
    {
        get
        {
            object val = ViewState["PERMISSION_GRANTED"];

            if (val is int)
            {
                return (int)val;
            }

            return 0;
        }

        set
        {
            ViewState["PERMISSION_GRANTED"] = value;
        }
    }

    protected void ValidatePermission(int balansId, int archiveId)
    {
        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            if (archiveId > 0)
            {
                // Get the balans object Id from the archive record
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 balans_id FROM view_arch_balans WHERE archive_id = @archid", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("archid", archiveId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            balansId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        }

                        reader.Close();
                    }
                }
            }

            using (SqlCommand cmd = new SqlCommand(@"SELECT TOP 1 
                CASE WHEN org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) THEN 1 ELSE 0 END 'is_rda',
                org_district_id 
                FROM view_balans_all WHERE balans_id = @bid", connection))
            {
                cmd.Parameters.Add(new SqlParameter("bid", balansId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool is_rda = reader.IsDBNull(0) ? false : reader.GetInt32(0) == 1;
                        int districtId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);

                        PermissionGranted = is_rda && districtId == Utils.RdaDistrictID ? 1 : -1;
                    }
                    else
                    {
                        PermissionGranted = -1;
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }
    }

    public string EvaluateSignature(object modifiedBy, object modifyDate)
    {
        string userName = (modifiedBy is string) ? (string)modifiedBy : Resources.Strings.SignatureUnknownUser;
        string date = (modifyDate is DateTime) ? ((DateTime)modifyDate).ToShortDateString() : Resources.Strings.SignatureUnknownDate;

        return string.Format(Resources.Strings.SignatureObjCard, userName, date);
    }

    public bool IsHistoryButtonVisible()
    {
        return Request.QueryString["balid"] == null ? false : true;
    }

    protected void ASPxUploadPhotoControl_FileUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs e)
    {
        string balansIdStr = Request.QueryString["balid"];
        Int32 newId = 0;

        if (string.IsNullOrEmpty(balansIdStr))
            throw new Exception("Ви не можете додати фото до архівного стану.");

        string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
        string serverLocalBalansObjectFolder = Path.Combine(photoRootPath, "Balans", balansIdStr);
        string serverLocal1NFObjectFolder = Path.Combine(photoRootPath, "1NF", balansIdStr);

        if (!System.IO.Directory.Exists(serverLocalBalansObjectFolder))
            System.IO.Directory.CreateDirectory(serverLocalBalansObjectFolder);

        if (!System.IO.Directory.Exists(serverLocal1NFObjectFolder))
            System.IO.Directory.CreateDirectory(serverLocal1NFObjectFolder);

        string fullPath1NF = string.Empty;
        string fullPathBalans = string.Empty;

        SqlConnection connection = Utils.ConnectToDatabase();
        SqlTransaction trans = connection.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO reports1nf_photos (bal_id, file_name, file_ext, user_id, create_date) VALUES (@balid, @filename, @fileext, @usrid, @createdate); SELECT CAST(SCOPE_IDENTITY() AS int)", connection, trans);
            cmd.Parameters.Add(new SqlParameter("balid", int.Parse(balansIdStr)));
            cmd.Parameters.Add(new SqlParameter("filename", System.IO.Path.GetFileNameWithoutExtension(e.UploadedFile.FileName)));
            cmd.Parameters.Add(new SqlParameter("fileext", System.IO.Path.GetExtension(e.UploadedFile.FileName)));
            cmd.Parameters.Add(new SqlParameter("usrid", (Guid)System.Web.Security.Membership.GetUser().ProviderUserKey));
            cmd.Parameters.Add(new SqlParameter("createdate", DateTime.Now));
            newId = (Int32)cmd.ExecuteScalar();

            cmd.CommandText = "INSERT INTO balans_photos (id, bal_id, file_name, file_ext, user_id, create_date) VALUES (@id, @balid, @filename, @fileext, @usrid, @createdate)";
            cmd.Parameters.Add(new SqlParameter("id", newId));
            cmd.ExecuteNonQuery();

            fullPathBalans = System.IO.Path.Combine(serverLocalBalansObjectFolder, newId.ToString() + System.IO.Path.GetExtension(e.UploadedFile.FileName));

            System.IO.FileStream file = System.IO.File.Create(fullPathBalans);
            file.Write(e.UploadedFile.FileBytes, 0, (int)e.UploadedFile.ContentLength);
            file.Flush();
            file.Close();

            fullPath1NF = System.IO.Path.Combine(serverLocal1NFObjectFolder, newId.ToString() + System.IO.Path.GetExtension(e.UploadedFile.FileName));
            System.IO.File.Copy(fullPathBalans, fullPath1NF);

            trans.Commit();
            connection.Close();
        }
        catch
        {
            trans.Rollback();
            if ((!string.IsNullOrEmpty(fullPathBalans)) && (System.IO.File.Exists(fullPathBalans)))
                System.IO.File.Delete(fullPathBalans);
            if ((!string.IsNullOrEmpty(fullPath1NF)) && (System.IO.File.Exists(fullPath1NF)))
                System.IO.File.Delete(fullPath1NF);
        }


    }

    protected void delete_Callback(object source, CallbackEventArgs e)
    {
        string archiveIdStr = Request.QueryString["arid"];
        if (archiveIdStr != null && archiveIdStr.Length > 0)
            throw new Exception("Ви не можете видалити фото з архівного стану.");

        if (imageGalleryDemo != null)
        {
            ImageGalleryItem item = imageGalleryDemo.Items[int.Parse(e.Parameter)];
            System.Data.DataRowView drv = (System.Data.DataRowView)item.DataItem;
            int elementToDelete = (int)drv.Row["Id"];
            int balId = (int)drv.Row["bal_id"];
            string fileExt = (string)drv.Row["file_ext"];

            Reports1NFUtils.DeleteBalansPhotoFileItem(elementToDelete, balId, fileExt, "Balans", "balans_photos");
            Reports1NFUtils.DeleteBalansPhotoFileItem(elementToDelete, balId, fileExt, "1NF", "reports1nf_photos");

            e.Result = item.ImageUrl;
        }
    }

    protected void ContentCallback_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.ToLower() == "refreshphoto:")
        {

        }
    }



}