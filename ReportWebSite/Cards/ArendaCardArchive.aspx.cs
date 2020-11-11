using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;
using DevExpress.Web;

public partial class Cards_ArendaCardArchive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string arendaIdStr = Request.QueryString["arid"];
        string archiveIdStr = Request.QueryString["archid"];

        if (arendaIdStr != null && arendaIdStr.Length > 0)
        {
            SqlDataSourceArendaCardProperties.SelectParameters["arid"].DefaultValue = arendaIdStr.Trim();
            SqlDataSourceArendaCardPropertiesArch.SelectParameters["archid"].DefaultValue = "0";

            SqlDataSourceArendaCardNotes.SelectParameters["arid"].DefaultValue = arendaIdStr.Trim();
            SqlDataSourceArendaCardNotesArch.SelectParameters["archid"].DefaultValue = "0";

            SqlDataSourceArendaCardReasons.SelectParameters["arid"].DefaultValue = arendaIdStr.Trim();
            SqlDataSourceArendaCardReasonsArch.SelectParameters["archid"].DefaultValue = "0";

            SqlDataSourceArendaArchive.SelectParameters["arid"].DefaultValue = arendaIdStr.Trim();

            ArendaArchDetails.DataSourceID = "SqlDataSourceArendaCardProperties";
            ArendaArchDetailsPage2.DataSourceID = "SqlDataSourceArendaCardProperties";
            GridViewArendaCardNotes.DataSourceID = "SqlDataSourceArendaCardNotes";
            GridViewArendaCardReasons.DataSourceID = "SqlDataSourceArendaCardReasons";

            ArendaArchDetails.DataBind();
            ArendaArchDetailsPage2.DataBind();
            GridViewArendaCardNotes.DataBind();
            GridViewArendaCardReasons.DataBind();

            this.Title = Resources.Strings.ArendaCardTitle;
            this.CardPageControl.TabPages[0].Text = Resources.Strings.ArendaCardTitle;
        }

        if (archiveIdStr != null && archiveIdStr.Length > 0)
        {
            SqlDataSourceArendaCardProperties.SelectParameters["arid"].DefaultValue = "0";
            SqlDataSourceArendaCardPropertiesArch.SelectParameters["archid"].DefaultValue = archiveIdStr.Trim();

            SqlDataSourceArendaCardNotes.SelectParameters["arid"].DefaultValue = "0";
            SqlDataSourceArendaCardNotesArch.SelectParameters["archid"].DefaultValue = archiveIdStr.Trim();

            SqlDataSourceArendaCardReasons.SelectParameters["arid"].DefaultValue = "0";
            SqlDataSourceArendaCardReasonsArch.SelectParameters["archid"].DefaultValue = archiveIdStr.Trim();

            ArendaArchDetails.DataSourceID = "SqlDataSourceArendaCardPropertiesArch";
            ArendaArchDetailsPage2.DataSourceID = "SqlDataSourceArendaCardPropertiesArch";
            GridViewArendaCardNotes.DataSourceID = "SqlDataSourceArendaCardNotesArch";
            GridViewArendaCardReasons.DataSourceID = "SqlDataSourceArendaCardReasonsArch";

            ArendaArchDetails.DataBind();
            ArendaArchDetailsPage2.DataBind();
            GridViewArendaCardNotes.DataBind();
            GridViewArendaCardReasons.DataBind();

            this.Title = Resources.Strings.ArendaCardTitleArch;
            this.CardPageControl.TabPages[0].Text = Resources.Strings.ArendaCardTitleArch;
        }

        // Check if user has permissions to view this rent agreement
        if (PermissionGranted == 0 && Utils.RdaDistrictID > 0)
        {
            // Prevent access by default
            PermissionGranted = -1;

            if (!string.IsNullOrEmpty(arendaIdStr))
            {
                ValidatePermission(int.Parse(arendaIdStr), -1);
            }
            else if (!string.IsNullOrEmpty(archiveIdStr))
            {
                ValidatePermission(-1, int.Parse(archiveIdStr));
            }
        }

        if (PermissionGranted < 0)
        {
            Response.Redirect(Page.ResolveClientUrl("~/Account/RestrictedRentAgreement.aspx"));
        }

		PrepareTempPhotoFolder();
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

    protected void ValidatePermission(int arendaId, int archiveId)
    {
        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            if (archiveId > 0)
            {
                // Get the rent agreement Id from the archive record
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 arenda_id FROM view_arch_arenda WHERE archive_id = @archid", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("archid", archiveId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            arendaId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        }

                        reader.Close();
                    }
                }
            }

            string query = @"SELECT TOP 1 
                            case when org_balans_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) then 1 else 0 end 'balans_is_rda',
                            org_balans_district_id,
                            case when org_giver_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) then 1 else 0 end 'giver_is_rda',
                            org_giver_district_id,
                            case when org_renter_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) then 1 else 0 end 'renter_is_rda',
                            org_renter_district_id
                            FROM m_view_arenda WHERE arenda_id = @arid";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("arid", arendaId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool balans_is_rda = reader.IsDBNull(0) ? false : reader.GetInt32(0) == 1;
                        int districtIdBalans = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                        bool giver_is_rda = reader.IsDBNull(2) ? false : reader.GetInt32(2) == 1;
                        int districtIdGiver = reader.IsDBNull(3) ? -1 : reader.GetInt32(3);
                        bool renter_is_rda = reader.IsDBNull(4) ? false : reader.GetInt32(4) == 1;
                        int districtIdRenter = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);

                        PermissionGranted =
                            (balans_is_rda && districtIdBalans == Utils.RdaDistrictID) ||
                            (giver_is_rda && districtIdGiver == Utils.RdaDistrictID) ||
                            (renter_is_rda && districtIdRenter == Utils.RdaDistrictID) ? 1 : -1;
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
        return Request.QueryString["arid"] == null ? false : true;
    }

	#region imageGallery

	protected void imageGalleryDemo_DataBound(object sender, EventArgs e)
	{
		if ((imageGalleryDemo.PageIndex > imageGalleryDemo.PageCount) && (IsCallback))
			imageGalleryDemo.PageIndex = 0;
	}

	protected void ASPxCallbackPanelImageGallery_Callback(object sender, CallbackEventArgsBase e)
	{
		//if (!IsCallback)
		{
			if (e.Parameter.ToLower() == "refreshphoto:")
			{
				//imageGalleryDemo.DataBind();
				BindImageGallery(Request.QueryString["arid"]);

			}
		}
	}

	private void BindImageGallery(string agreementIdStr)
	{
		ObjectDataSourceBalansPhoto.SelectParameters["recordID"].DefaultValue = agreementIdStr;
		ObjectDataSourceBalansPhoto.SelectParameters["tempGuid"].DefaultValue = PhotoFolderID.ToString();
		imageGalleryDemo.DataSourceID = "ObjectDataSourceBalansPhoto";
		imageGalleryDemo.DataBind();
		//imageGalleryDemo.Enabled = false;
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
			string agreementIdStr = Request.QueryString["arid"];
			string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
			string destFolder = Path.Combine(photoRootPath, "1NF_" + agreementIdStr + "_" + PhotoFolderID.ToString()).ToLower();
			return destFolder;
		}
		else
			return string.Empty;
	}

	private void PrepareTempPhotoFolder()
	{
		string agreementIdStr = Request.QueryString["arid"];

		if (PhotoFolderID == Guid.Empty)
		{
			PhotoFolderID = Guid.NewGuid();

			CopySourceFiles(agreementIdStr);
		}

		BindImageGallery(agreementIdStr);
	}

	private void CopySourceFiles(string agreementIdStr)
	{
		string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
		string destFolder = TempPhotoFolder();

		SqlConnection connection = Utils.ConnectToDatabase();
		using (SqlCommand cmd = new SqlCommand("select id, file_name, file_ext from reports1nf_arendaphotos where arenda_id = @aid", connection))
		{
			cmd.Parameters.AddWithValue("aid", agreementIdStr);
			using (SqlDataReader r = cmd.ExecuteReader())
			{
				while (r.Read())
				{
					int id = r.GetInt32(0);
					string file_name = r.GetString(1);
					string file_ext = r.GetString(2);

					string sourceFileToCopy = Path.Combine(photoRootPath, "1NFARENDA", agreementIdStr, id.ToString() + file_ext);
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

    protected void PdfImageBuild_Click(object sender, EventArgs e)
    {
        var agreementId = Int32.Parse(Request.QueryString["arid"]);

        var fname = @"фото_" + agreementId + ".pdf";
        var bytes = new OrgRentAgreementPhotosPdfBulder().Go(agreementId, TempPhotoFolder());

        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fname + "\"");
        Response.BinaryWrite(bytes);

        Response.Flush();
        Response.End();
    }

    #endregion

}