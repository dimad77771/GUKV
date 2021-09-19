using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Cards_AssessmentCard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (Request.QueryString["isnew"] == "1")
		{
			OpenNewCard();
			return;
		}

        string valuationIdStr = Request.QueryString["vid"];
		VID = Int32.Parse(valuationIdStr);

		if (!IsPostBack && valuationIdStr != null && valuationIdStr.Length > 0)
        {
            SqlDataSourceAssessmentProperties.SelectParameters["vid"].DefaultValue = valuationIdStr.Trim();
            SqlDataSourceAssessmentInputDoc.SelectParameters["vid"].DefaultValue = valuationIdStr.Trim();
            SqlDataSourceAssessmentOutputDoc.SelectParameters["vid"].DefaultValue = valuationIdStr.Trim();
            SqlDataSourceAssessmentDetails.SelectParameters["vid"].DefaultValue = valuationIdStr.Trim();
        }
    }

	void OpenNewCard()
	{
		var id = Utils.GetNextSequenceValue("sequence_expert_note");

		var user = Membership.GetUser();
		var username = (user == null ? "System" : user.UserName);

		SqlConnection connection = Utils.ConnectToDatabase();

		if (connection != null)
		{
			using (SqlCommand cmd = new SqlCommand("insert into expert_note(id) values(@id)", connection))
			{
				cmd.Parameters.Add(new SqlParameter("id", id));
				//cmd.Parameters.Add(new SqlParameter("usr", username));
				//cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now));
				cmd.ExecuteNonQuery();
			}

			connection.Close();
		}


		var url = "AssessmentCard.aspx?vid=" + id;
		Response.Redirect(url);
	}

	protected int VID
	{
		get
		{
			object Id = ViewState["VID"];

			if (Id is int)
			{
				return (int)Id;
			}

			return 0;
		}

		set
		{
			ViewState["VID"] = value;
		}
	}

	protected void CPMainPanel_Callback(object sender, CallbackEventArgsBase e)
	{
		//////
		try
		{
			if (e.Parameter.StartsWith("save:"))
			{
				SqlConnection connectionSql = Utils.ConnectToDatabase();

				if (connectionSql != null)
				{
					SaveChanges(connectionSql);

					connectionSql.Close();
				}
			}
		}
		catch (Exception ex)
		{
			var lognet = log4net.LogManager.GetLogger("ReportWebSite");
			lognet.Debug("--------------- Orgbalansobject CPMainPanel_Callback ----------------", ex);
			throw ex;
		}
	}

	protected void SaveChanges(SqlConnection connection)
	{
		//!!!! -> нужно выставить в connection.string сменить 
		//		"MultipleActiveResultSets=True" 
		//			на
		//		"MultipleActiveResultSets=False" 
		//!!SqlCommand cmd22 = new SqlCommand("begin tran", connection);
		//!!cmd22.ExecuteNonQuery();

		System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
		string username = (user == null ? "System" : user.UserName);

		Dictionary<string, Control> controls = new Dictionary<string, Control>();

		Reports1NFUtils.GetAllControls(ObjDetails, controls);
		Reports1NFUtils.GetAllControls(FormView1, controls);


		Dictionary<string, object> parameters = new Dictionary<string, object>();
		string fieldList = "";

		AddQueryParameter(ref fieldList, "addr_district_id", "addr_district_id", Reports1NFUtils.GetDropDownValue(controls, "addr_district_id"), parameters);
		AddQueryParameter(ref fieldList, "expert_obj_type_id", "expert_obj_type_id", Reports1NFUtils.GetDropDownValue(controls, "expert_obj_type_id"), parameters);
		AddQueryParameter(ref fieldList, "addr_street_id", "addr_street_id", Reports1NFUtils.GetDropDownValue(controls, "addr_street_id"), parameters);
		AddQueryParameter(ref fieldList, "addr_nomer", "addr_nomer", Reports1NFUtils.GetDropDownText(controls, "ComboBuilding"), parameters);

		AddQueryParameter(ref fieldList, "org_balans_id", "org_balans_id", Reports1NFUtils.GetDropDownValue(controls, "org_balans_id"), parameters);
		AddQueryParameter(ref fieldList, "org_renter_id", "org_renter_id", Reports1NFUtils.GetDropDownValue(controls, "org_renter_id"), parameters);

		AddQueryParameter(ref fieldList, "expert_id", "expert_id", Reports1NFUtils.GetDropDownValue(controls, "expert_id"), parameters);
		AddQueryParameter(ref fieldList, "rezenz_id", "rezenz_id", Reports1NFUtils.GetDropDownValue(controls, "rezenz_id"), parameters);
		AddQueryParameter(ref fieldList, "valuation_kind_id", "valuation_kind_id", Reports1NFUtils.GetDropDownValue(controls, "valuation_kind_id"), parameters);
		AddQueryParameter(ref fieldList, "valuation_date", "valuation_date", Reports1NFUtils.GetDateValue(controls, "valuation_date"), parameters);
		AddQueryParameter(ref fieldList, "obj_square", "obj_square", Reports1NFUtils.GetEditNumeric(controls, "obj_square"), parameters);
		AddQueryParameter(ref fieldList, "cost_prim", "cost_prim", Reports1NFUtils.GetEditNumeric(controls, "cost_prim"), parameters);

		AddQueryParameter(ref fieldList, "is_archived", "is_archived", Reports1NFUtils.GetCheckBoxValue(controls, "is_archived") ? 1 :0, parameters);
		AddQueryParameter(ref fieldList, "arch_num", "arch_num", Reports1NFUtils.GetEditText(controls, "arch_num"), parameters);
		AddQueryParameter(ref fieldList, "final_date", "final_date", Reports1NFUtils.GetDateValue(controls, "final_date"), parameters);

		AddQueryParameter(ref fieldList, "note_text", "note_text", Reports1NFUtils.GetEditText(controls, "AssessmentMemo"), parameters);

		AddQueryParameter(ref fieldList, "modify_date", "mdt", DateTime.Now, parameters);
		AddQueryParameter(ref fieldList, "modified_by", "mby", username.Left(64), parameters);

		using (SqlCommand cmd = new SqlCommand("UPDATE expert_note SET " + fieldList + " WHERE id = @id", connection))
		{
			cmd.Parameters.Add(new SqlParameter("id", VID));

			foreach (KeyValuePair<string, object> param in parameters)
			{
				cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
			}

			cmd.ExecuteNonQuery();
		}
	}

	protected void AddQueryParameter(ref string fieldList, string fieldName, string paramName,
		object value, Dictionary<string, object> parameters)
	{
		bool valueExists = false;

		if (value is int)
		{
			valueExists = ((int)value) >= 0;
		}
		else if (value is string)
		{
			valueExists = ((string)value).Length > 0;
		}
		else if (value is decimal)
		{
			valueExists = true;
		}
		else if (value is DateTime)
		{
			valueExists = true;
		}

		if (fieldList.Length > 0)
		{
			fieldList += ", ";
		}

		if (valueExists)
		{
			fieldList += fieldName + " = @" + paramName;
			parameters[paramName] = value;
		}
		else
		{
			fieldList += fieldName + " = NULL";
		}
	}

	protected void CPMainPanel_Unload(object sender, EventArgs e)
	{

	}

	protected void SqlDataSourceDictBuildings_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		e.Command.Parameters["@street_id"].Value = AddressStreetID;
	}

	private int AddressStreetID
	{
		get
		{
			object streetId = Session["OrgArendaList_AddressStreetID"];
			return (streetId is int) ? (int)streetId : 0;
		}

		set
		{
			Session["OrgArendaList_AddressStreetID"] = value;
		}
	}

	protected void ComboBuilding_Callback(object source, CallbackEventArgsBase e)
	{
		try
		{
			int streetId = int.Parse(e.Parameter);
			AddressStreetID = streetId;
			(source as ASPxComboBox).DataBind();
		}
		finally
		{
		}
	}

	protected void SqlDataSourceFreeSquare_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		e.Command.Parameters["@modify_date"].Value = DateTime.Now;
		MembershipUser user = Membership.GetUser();
		e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;
	}

	protected void SqlDataSourceFreeSquare_Inserting(object sender, SqlDataSourceCommandEventArgs e)
	{
		e.Command.Parameters["@expert_note_id"].Value = VID;

		e.Command.Parameters["@modify_date"].Value = DateTime.Now;
		MembershipUser user = Membership.GetUser();
		e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;
	}

	protected void SqlDataSourceAssessmentDetails_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		e.Command.Parameters["@modify_date"].Value = DateTime.Now;
		MembershipUser user = Membership.GetUser();
		e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;
	}

	protected void SqlDataSourceAssessmentDetails_Inserting(object sender, SqlDataSourceCommandEventArgs e)
	{
		e.Command.Parameters["@expert_note_id"].Value = VID;

		e.Command.Parameters["@modify_date"].Value = DateTime.Now;
		MembershipUser user = Membership.GetUser();
		e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;
	}

	protected void SqlDataSourceAssessmentOutputDoc_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		e.Command.Parameters["@modify_date"].Value = DateTime.Now;
		MembershipUser user = Membership.GetUser();
		e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;
	}

	protected void SqlDataSourceAssessmentOutputDoc_Inserting(object sender, SqlDataSourceCommandEventArgs e)
	{
		e.Command.Parameters["@expert_note_id"].Value = VID;

		e.Command.Parameters["@modify_date"].Value = DateTime.Now;
		MembershipUser user = Membership.GetUser();
		e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;
	}
}
