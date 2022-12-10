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
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using DevExpress.Spreadsheet;
using GUKV.Common;
using System.IO;

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

	protected void ButtonPrint_Click(object sender, EventArgs e)
	{
		//new ZvitProzoroMultiCreate().Go(); return;

		var builder = new ZvitProzoroBuilder
		{
			Page = this,
			Ids = new[] { FreeID },
		};
		builder.Go();
	}
}




public class ZvitProzoroMultiCreate
{
	public void Go()
	{
		VsagdeBuild();

		string templateFileName = @"D:\Projects\DKVSOURCESFINALEDITION_v20\ReportWebSite\Reports1NF\Templates\FreeProzoro____.xlsx";
		var tempFile = TempFile.FromExistingFile(templateFileName);

		var connection = CommonUtils.ConnectToDatabase();
		if (connection == null) throw new Exception("Database GUKV not found");
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = GetSql();
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}

		var basenum = 20000;
		var workbook = new Workbook();
		workbook.LoadDocument(tempFile.FileName, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
		var wsheet = workbook.Worksheets[0];
		var usedRange = wsheet.GetUsedRange();
		for (int k = dataTable.Rows.Count - 1; k >= 0; k--)
		{
			var bcolumn1 = wsheet.Columns[k + 1];

			foreach (DataColumn dbcol in dataTable.Columns)
			{
				var reg = Regex.Match(dbcol.ColumnName, @"(v\d{1,3})");
				if (reg.Success)
				{
					var col = reg.Groups[1].Value;
					var dbval = dataTable.Rows[k][col];
					var sval = dbval.ToString();
					if (dbval is DateTime)
					{
						sval = ((DateTime)dbval).ToString("dd.MM.yyyy");
					}


					var excelRow = Int32.Parse(col.Replace("v", ""));



					if (string.IsNullOrEmpty(sval) && new[] { 38, 39 }.Contains(excelRow))
					{
						sval = "уточнюєтся";
					}

					bcolumn1[excelRow].Value = sval;
				}
			}
			bcolumn1[0].Value = basenum + k;
			VsagdeProc(bcolumn1);
		}
		workbook.SaveDocument(tempFile.FileName);
		File.Copy(tempFile.FileName, @"H:\SQLServer\8_dimad77772_export\___out.xlsx");
	}

	void VsagdeProc(Column bcolumn)
	{
		foreach (var vitem in vsegda)
		{
			bcolumn[vitem.Key - 1].Value = vitem.Value;
		}
	}

	Dictionary<int, string> vsegda = new Dictionary<int, string>();
	void VsagdeBuild()
	{
		vsegda[5] = "Наказ ДКВ від 10.05.2016 №238";
		vsegda[6] = "Україна";
		vsegda[7] = "Київ";
		vsegda[8] = "Київ";
		vsegda[9] = "Хрещатик, 10";
		vsegda[10] = "01001";
		vsegda[11] = "Шалюта";
		vsegda[12] = "Олег";
		vsegda[13] = "Федорович";
		vsegda[14] = "orenda@gukv.gov.ua";
		vsegda[15] = "044 202 61 96";
		vsegda[18] = "Так";
		vsegda[19] = "лист";
		vsegda[20] = "так";
		vsegda[24] = "Не вимагається";
		vsegda[26] = "Не вимагається";
		vsegda[30] = "Україна";
		vsegda[31] = "Наказ .......";
		vsegda[32] = "Київ";
		vsegda[33] = "Київ";
		vsegda[45] = "Наказ";
		vsegda[46] = "Україна";
		vsegda[47] = "Київ";
		vsegda[48] = "Київ";
		vsegda[73] = "Нерухоме майно";
		vsegda[75] = "Україна";
		vsegda[76] = "Київ";
		vsegda[77] = "Київ";
		vsegda[79] = "Комунальна";
		vsegda[80] = "Перелік першого типу";
		vsegda[81] = "Включено в перелік";
		vsegda[101] = "04000000-8";
		vsegda[102] = "Україна";
		vsegda[103] = "Київ";
		vsegda[104] = "Київ";
		vsegda[107] = "50.00000";
		vsegda[108] = "30.00000";
		vsegda[115] = "Так";
	}


	public string GetSql()
	{
		return @"

select

(select Q.tel from zzzzz_director_title Q where Q.fio = B.director_title) v39,
U.zkpo,

B.old_organ_id,
H.org_balans_zkpo,
H.org_giver_zkpo,
H.arenda_id,
cast(round(DATEDIFF(DAY, H.rent_start_date, H.rent_finish_date) / 365.25,0) as int) v139,
--DATEDIFF(MONTH, H.rent_start_date, H.agreement_date) v140,
--DATEDIFF(DAY, H.rent_start_date, H.agreement_date) v141,
H.rent_start_date,
H.rent_finish_date,
H.agreement_num,


H.cost_expert_total v88,
H.cost_expert_total v90,
H.cost_agreement v154,

concat(H.street_full_name, ', ', H.addr_nomer) as v77,
concat(H.street_full_name, ', ', H.addr_nomer) as v104,

H.rent_square v111,
H.rent_square v112,
R.note v109,
R.note v110,
(SELECT Q.name FROM dict_1nf_balans_purpose Q where Q.id = R.purpose_id) v98,
R.purpose_str v99,

D.zkpo_code v42,
D.full_name v43,
concat((select Q.name from dict_streets Q where Q.id = D.addr_street_id), ', ', D.addr_nomer) as v48,
D.phys_addr_zip_code as v49, 
D.director_email v53,
(select Q.namf from zzzzz_director_title Q where Q.fio = D.director_title) v50,
(select Q.nami from zzzzz_director_title Q where Q.fio = D.director_title) v51,
(select Q.namo from zzzzz_director_title Q where Q.fio = D.director_title) v52,
(select Q.tel from zzzzz_director_title Q where Q.fio = D.director_title) v54,


----------
--(select Q.name from dict_org_old_organ Q where Q.id = B.old_organ_id) as v43,
(select Q.namf from zzzzz_director_title Q where Q.fio = B.director_title) v35,
(select Q.nami from zzzzz_director_title Q where Q.fio = B.director_title) v36,
(select Q.namo from zzzzz_director_title Q where Q.fio = B.director_title) v37,

concat((select Q.name from dict_streets Q where Q.id = B.addr_street_id), ', ', B.addr_nomer) as v33,
B.phys_addr_zip_code as v34, 
B.buhgalter_email as v38,
B.zkpo_code v27,
B.full_name v28,

----------
T.zkpo_code v2,
T.full_name v3,
T.director_email v13,
(select Q.namf from zzzzz_director_title Q where Q.fio = T.director_title) v10,
(select Q.nami from zzzzz_director_title Q where Q.fio = T.director_title) v11,
(select Q.namo from zzzzz_director_title Q where Q.fio = T.director_title) v12,
(select Q.tel from zzzzz_director_title Q where Q.fio = T.director_title) v14
--into zzzzzz20221202z
from
--(select * from view_arenda Q) H -- where Q.arenda_id = 78483) H
(select * from zzzzz20221202a Q) H -- where Q.arenda_id = 78483) H
--join zzzzz20221202a P on P.arenda_id = H.arenda_id
outer apply (select top 1 * from arenda_notes Q where Q.id = H.arenda_note_id) R
outer apply
(
	SELECT 
	TOP 1 
	info.*
	,CASE WHEN info.submit_date IS NULL OR info.modify_date IS NULL OR info.modify_date > info.submit_date THEN 0 ELSE 1 END AS 'report_org_info_status'
	,dbo.[get_kazna_total](
	(select Q.zkpo_code from reports1nf_org_info Q where report_id = info.report_id), 
	null, null 
	) AS 'kazna_total' 
	FROM reports1nf_org_info info 
	WHERE info.zkpo_code = H.org_giver_zkpo
) T 
outer apply
(
	SELECT 
	TOP 1 
	info.*
	,CASE WHEN info.submit_date IS NULL OR info.modify_date IS NULL OR info.modify_date > info.submit_date THEN 0 ELSE 1 END AS 'report_org_info_status'
	,dbo.[get_kazna_total](
	(select Q.zkpo_code from reports1nf_org_info Q where report_id = info.report_id), 
	null, null 
	) AS 'kazna_total' 
	FROM reports1nf_org_info info 
	WHERE info.zkpo_code = H.org_balans_zkpo
) B
left join dict_org_old_organ_addinfo U on U.id = B.old_organ_id
outer apply
(
	SELECT 
	TOP 1 
	info.*
	,CASE WHEN info.submit_date IS NULL OR info.modify_date IS NULL OR info.modify_date > info.submit_date THEN 0 ELSE 1 END AS 'report_org_info_status'
	,dbo.[get_kazna_total](
	(select Q.zkpo_code from reports1nf_org_info Q where report_id = info.report_id), 
	null, null 
	) AS 'kazna_total' 
	FROM reports1nf_org_info info 
	WHERE info.zkpo_code = U.zkpo
) D
order by H.arenda_id






		";

	}

}