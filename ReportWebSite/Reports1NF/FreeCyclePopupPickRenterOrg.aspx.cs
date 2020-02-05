using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxRoundPanel;
using System.Data;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using GUKV;
using System.Web.Configuration;
using System.IO;
using DevExpress.Web.ASPxImageGallery;
using ExtDataEntry.Models;
using DevExpress.Web.ASPxCallback;

public partial class Reports1NF_FreeCyclePopupPickRenterOrg : System.Web.UI.Page
{
	//protected int freecycle_orendar_id
	//{
	//	get
	//	{
	//		return Int32.Parse(Request.QueryString["freecycle_orendar_id"]);
	//	}
	//}

	protected void Page_Load(object sender, EventArgs e)
    {
		GetPageUniqueKey();

		SqlConnection connection = Utils.ConnectToDatabase();
		SqlDataAdapter adp = new SqlDataAdapter(@"SELECT id as org_renter_id, org.* FROM organizations org where 1=2", connection);
		DataSet ds = new DataSet();
		adp.Fill(ds);
		var rows = ds.Tables[0].Rows;
		rows.Add(ds.Tables[0].NewRow());
		OrganizationsForm.DataSource = ds;
		OrganizationsForm.DataBind();
	}

	protected void SqlDataSourceOrgSearchRenter_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		string orgName = RenterOrgNamePattern;
		string zkpo = RenterOrgZkpoPattern;

		if (orgName.Length == 0 && zkpo.Length == 0)
		{
			e.Command.Parameters["@zkpo"].Value = "^";
			e.Command.Parameters["@fname"].Value = "^";
		}
		else
		{
			e.Command.Parameters["@zkpo"].Value = zkpo.Length > 0 ? zkpo : "%";
			e.Command.Parameters["@fname"].Value = orgName.Length > 0 ? "%" + orgName + "%" : "%";
		}
	}

	protected void CPPopUp_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter.StartsWith("create_org:") || e.Parameter.StartsWith("edit_org:"))
		{
		}
	}

	protected void ComboRenterOrg_Callback(object sender, CallbackEventArgsBase e)
	{
		string[] parts = e.Parameter.Split(new char[] { '|' });

		if (parts.Length == 2)
		{
			RenterOrgZkpoPattern = parts[0].Trim();
			RenterOrgNamePattern = parts[1].Trim().ToUpper();

			var сomboBox = (sender as ASPxComboBox);
			сomboBox.DataBind();

			if (сomboBox.Items.Count > 0)
			{
				сomboBox.SelectedIndex = 0;
			}
		}
	}

	private string RenterOrgZkpoPattern
	{
		get
		{
			object pattern = Session[GetPageUniqueKey() + "_RenterOrgZkpoPattern"];
			return (pattern is string) ? (string)pattern : "";
		}

		set
		{
			Session[GetPageUniqueKey() + "_RenterOrgZkpoPattern"] = value;
		}
	}

	private string RenterOrgNamePattern
	{
		get
		{
			object pattern = Session[GetPageUniqueKey() + "_RenterOrgNamePattern"];
			return (pattern is string) ? (string)pattern : "";
		}

		set
		{
			Session[GetPageUniqueKey() + "_RenterOrgNamePattern"] = value;
		}
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


}