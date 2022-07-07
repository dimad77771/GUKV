using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using GUKV;
using System.Text.RegularExpressions;

public partial class Reports1NF_FreeProzoro : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string freeIdStr = Request.QueryString["id"];
		FreeID = int.Parse(freeIdStr);

		CreateNewRow();

		SqlDataSourceMain.SelectParameters["id"].DefaultValue = FreeID.ToString();

		if (!IsPostBack)
		{
			var vals = GetAllValues();
			ViewState["DINFO"] = vals;
		}
	}

	protected int FreeID
	{
        get
        {
            object freeId = ViewState["FREE_ID"];

            if (freeId is int)
            {
                return (int)freeId;
            }

            return 0;
        }

        set
        {
            ViewState["FREE_ID"] = value;
        }
    }

    protected void SaveChanges(SqlConnection connection)
    {
		var oldVals = ViewState["DINFO"] as Dictionary<string, object>;

		var user = Membership.GetUser();
		var username = (user == null ? "System" : user.UserName);
  
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        string fieldList = "";

		var vals = GetAllValues();
		foreach (var val in vals)
		{
			var oldVal = oldVals[val.Key];
			if (ObjectToString(oldVal) != ObjectToString(val.Value))
			{
				AddQueryParameter(ref fieldList, val.Key, "p_" + val.Key, val.Value, parameters);
			}
		}

		/*
        AddQueryParameter(ref fieldList, "vidch_type_id", "vtid", Reports1NFUtils.GetDropDownValue(controls, "ComboVidchType"), parameters);
        AddQueryParameter(ref fieldList, "floors", "flrs", Reports1NFUtils.GetEditText(controls, "EditVidchFloor"), parameters);
        AddQueryParameter(ref fieldList, "sqr_total", "sqrtot", Reports1NFUtils.GetEditNumeric(controls, "EditVidchSquare"), parameters);
        AddQueryParameter(ref fieldList, "vidch_cost", "vct", Reports1NFUtils.GetEditNumeric(controls, "EditVidchCost"), parameters);
        AddQueryParameter(ref fieldList, "purpose_str", "purpstr", Reports1NFUtils.GetEditText(controls, "EditVidchPurpose"), parameters);
        AddQueryParameter(ref fieldList, "reestr_no", "reesno", Reports1NFUtils.GetEditText(controls, "EditObjBtiReestrNoDeleted"), parameters);
        AddQueryParameter(ref fieldList, "vidch_org_id", "vorg", Reports1NFUtils.GetDropDownValue(controls, "ComboVidchOrg"), parameters);
        AddQueryParameter(ref fieldList, "vidch_doc_type", "vdt", Reports1NFUtils.GetDropDownValue(controls, "ComboVidchDocKind"), parameters);
        AddQueryParameter(ref fieldList, "vidch_doc_num", "vdn", Reports1NFUtils.GetEditText(controls, "EditVidchDocNum"), parameters);
        AddQueryParameter(ref fieldList, "vidch_doc_date", "vdd", Reports1NFUtils.GetDateValue(controls, "EditVidchDocDate"), parameters);
		*/

		// System parameters
		AddQueryParameter(ref fieldList, "modify_date", "mdt", DateTime.Now, parameters);
        AddQueryParameter(ref fieldList, "modified_by", "mby", username.Left(64), parameters);

        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_balans_free_prozoro SET " + fieldList + " WHERE id = @id", connection))
        {
            cmd.Parameters.Add(new SqlParameter("id", FreeID));

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            cmd.ExecuteNonQuery();
        }

		ViewState["DINFO"] = vals;
	}

	string ObjectToString(object val)
	{
		if (val == null)
		{
			return "";
		}
		else
		{
			return val.ToString();
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

    protected void CPMainPanel_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("save:"))
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                SaveChanges(connection);

                connection.Close();
            }
        }

        AddressForm.DataBind();
    }

	protected void CallbackCopyPaste_Callback(object sender, CallbackEventArgsBase e)
	{
		CallbackCopyPaste.JSProperties["cp_clipborddata"] = "";
		var clipborddata = e.Parameter;

		SqlConnection connection = Utils.ConnectToDatabase();

		var user = Membership.GetUser();
		var username = (string.IsNullOrEmpty(user.UserName) ? "System" : user.UserName);

		if (clipborddata == "paste")
		{
			clipborddata = "";
			var command = connection.CreateCommand();
			command.CommandText = "select clipborddata from ClipbordForUsers where username = @username";
			command.Parameters.Add(new SqlParameter("username", username));
			using (SqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					clipborddata = reader.GetString(0);
				}
			}
			command.Dispose();

			CallbackCopyPaste.JSProperties["cp_clipborddata"] = clipborddata;
		}
		else
		{
			var command = connection.CreateCommand();
			command.CommandText = "delete from ClipbordForUsers where username = @username; insert into ClipbordForUsers(username,clipborddata) values (@username,@clipborddata)";
			command.Parameters.Add(new SqlParameter("username", username));
			command.Parameters.Add(new SqlParameter("clipborddata", clipborddata));
			command.ExecuteNonQuery();
			command.Dispose();
		}

		connection.Close();

	}

	void CreateNewRow()
	{
		var connection = Utils.ConnectToDatabase();

		var ex = false;
		using (SqlCommand cmd = new SqlCommand("select * from reports1nf_balans_free_prozoro where id = @id", connection))
		{
			cmd.Parameters.AddWithValue("id", FreeID);
			using (SqlDataReader reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					ex = true;
				}
			}
		}

		if (!ex)
		{
			var transaction = connection.BeginTransaction();
			using (SqlCommand cmd = new SqlCommand("" +
				"insert into [reports1nf_balans_free_prozoro](id, v202,v203,v206,v208,v210,v220, v221,v222, v92) " +
				"values(@id, 'Україна','Україна','Україна','Україна','Україна','Україна', '50.00000','30.00000', '04000000-8')", connection, transaction))
			{
				cmd.Parameters.Add(new SqlParameter("id", FreeID));
				cmd.ExecuteNonQuery();
			}

			transaction.Commit();
		}
	}

	Dictionary<string, object> GetAllValues()
	{
		var regex = new Regex(@"^v\d+$");

		Dictionary<string, object> vals = new Dictionary<string, object>();
		Dictionary<string, Control> controls = new Dictionary<string, Control>();
		Reports1NFUtils.GetAllControls(AddressForm, controls);
		foreach(var control in controls.Values)
		{
			var idControl = control.ID;
			if (regex.IsMatch(idControl))
			{
				object val;

				if (control is ASPxSpinEdit)
				{
					val = Reports1NFUtils.GetEditNumeric(controls, idControl);
				}
				else if (control is ASPxDateEdit)
				{
					val = Reports1NFUtils.GetDateValue(controls, idControl);
				}
				else if (control is ASPxTextBox)
				{
					val = Reports1NFUtils.GetEditText(controls, idControl);
				}
				else if (control is ASPxCheckBox)
				{
					val = Reports1NFUtils.GetCheckBoxValue(controls, idControl);
					val = true.Equals(val) ? "Так" : "";
				}
				else if (control is ASPxComboBox)
				{
					val = Reports1NFUtils.GetDropDownText(controls, idControl);
				}
				else throw new Exception();

				vals.Add(idControl, val);
			}
		}
		

		return vals;
	}

  

}
