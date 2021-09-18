using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Cards_AssessmentCard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
		//AddQueryParameter(ref fieldList, "expert_obj_type_id", "expert_obj_type_id", Reports1NFUtils.GetDropDownValue(controls, "expert_obj_type_id"), parameters);

		AddQueryParameter(ref fieldList, "balans_id", "balans_id", Reports1NFUtils.GetDropDownValue(controls, "balans_id"), parameters);


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
}