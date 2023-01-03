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
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using WP = DocumentFormat.OpenXml.Wordprocessing;

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

	protected void ButtonDocx1_Click(object sender, EventArgs e)
	{
		var builder = new ZvitProzoroOgoloshena
		{
			ID = FreeID,
			Page = Page,
			IsOgoloshena = true,
		};
		builder.Run();
	}

	protected void ButtonDocx2_Click(object sender, EventArgs e)
	{
		var builder = new ZvitProzoroOgoloshena
		{
			ID = FreeID,
			Page = Page,
			IsOgoloshena = false,
		};
		builder.Run();
	}

	Dictionary<string, Control> GetAllControls()
	{
		Dictionary<string, Control> controls = new Dictionary<string, Control>();
		Reports1NFUtils.GetAllControls(AddressForm, controls);
		return controls;
	}

	DataTable GetReports1nfOrgInfo(string zkpo)
	{
		var connection = CommonUtils.ConnectToDatabase();
		if (connection == null) throw new Exception("Database GUKV not found");
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = @"
select
D.zkpo_code,
D.full_name,
concat((select Q.name from dict_streets Q where Q.id = D.addr_street_id), ', ', D.addr_nomer) as adr,
D.phys_addr_zip_code, 
D.director_email,
(select Q.namf from zzzzz_director_title Q where Q.fio = D.director_title) namf,
(select Q.nami from zzzzz_director_title Q where Q.fio = D.director_title) nami,
(select Q.namo from zzzzz_director_title Q where Q.fio = D.director_title) namo,
(select Q.tel from zzzzz_director_title Q where Q.fio = D.director_title) tel
from reports1nf_org_info D
where D.zkpo_code = '" + zkpo + "'";
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}
		return dataTable;
	}

	T GetValue<T>(object value)
	{
		if (value is DBNull)
			return default(T);
		else
			return (T)value;
	}

	protected void v46_TextChanged(object sender, EventArgs e)
	{
		var controls = GetAllControls();
		var zkpo = (ASPxTextBox)controls["v46"];
		var full_name = (ASPxTextBox)controls["v47"];
		var adr = (ASPxTextBox)controls["v50"];
		var phys_addr_zip_code = (ASPxTextBox)controls["v51"];

		var data = GetReports1nfOrgInfo(zkpo.Text);
		if (data.Rows.Count > 0)
		{
			var row = data.Rows[0];
			full_name.Text = GetValue<string>(row["full_name"]);
			adr.Text = GetValue<string>(row["adr"]);
			phys_addr_zip_code.Text = GetValue<string>(row["phys_addr_zip_code"]);
		}
	}

	protected void v57_TextChanged(object sender, EventArgs e)
	{
		var controls = GetAllControls();
		var zkpo = (ASPxTextBox)controls["v57"];
		var full_name = (ASPxTextBox)controls["v58"];
		var adr = (ASPxTextBox)controls["v61"];
		var phys_addr_zip_code = (ASPxTextBox)controls["v62"];
		var director_email = (ASPxTextBox)controls["v66"];
		var namf = (ASPxTextBox)controls["v63"];
		var nami = (ASPxTextBox)controls["v64"];
		var namo = (ASPxTextBox)controls["v65"];
		var tel = (ASPxTextBox)controls["v67"];

		var data = GetReports1nfOrgInfo(zkpo.Text);
		if (data.Rows.Count > 0)
		{
			var row = data.Rows[0];
			full_name.Text = GetValue<string>(row["full_name"]);
			adr.Text = GetValue<string>(row["adr"]);
			phys_addr_zip_code.Text = GetValue<string>(row["phys_addr_zip_code"]);
			director_email.Text = GetValue<string>(row["director_email"]);
			namf.Text = GetValue<string>(row["namf"]);
			nami.Text = GetValue<string>(row["nami"]);
			namo.Text = GetValue<string>(row["namo"]);
			tel.Text = GetValue<string>(row["tel"]);
		}
	}
	

	protected void v68_TextChanged(object sender, EventArgs e)
	{
		var controls = GetAllControls();
		var zkpo = (ASPxTextBox)controls["v68"];
		var full_name = (ASPxTextBox)controls["v69"];
		var adr = (ASPxTextBox)controls["v72"];
		var phys_addr_zip_code = (ASPxTextBox)controls["v73"];
		var director_email = (ASPxTextBox)controls["v77"];
		var namf = (ASPxTextBox)controls["v74"];
		var nami = (ASPxTextBox)controls["v75"];
		var namo = (ASPxTextBox)controls["v76"];
		var tel = (ASPxTextBox)controls["v78"];

		var data = GetReports1nfOrgInfo(zkpo.Text);
		if (data.Rows.Count > 0)
		{
			var row = data.Rows[0];
			full_name.Text = GetValue<string>(row["full_name"]);
			adr.Text = GetValue<string>(row["adr"]);
			phys_addr_zip_code.Text = GetValue<string>(row["phys_addr_zip_code"]);
			director_email.Text = GetValue<string>(row["director_email"]);
			namf.Text = GetValue<string>(row["namf"]);
			nami.Text = GetValue<string>(row["nami"]);
			namo.Text = GetValue<string>(row["namo"]);
			tel.Text = GetValue<string>(row["tel"]);
		}
	}

	protected void v79_TextChanged(object sender, EventArgs e)
	{
		var controls = GetAllControls();
		var zkpo = (ASPxTextBox)controls["v79"];
		var full_name = (ASPxTextBox)controls["v80"];
		var adr = (ASPxTextBox)controls["v83"];
		var phys_addr_zip_code = (ASPxTextBox)controls["v84"];
		var director_email = (ASPxTextBox)controls["v88"];
		var namf = (ASPxTextBox)controls["v85"];
		var nami = (ASPxTextBox)controls["v86"];
		var namo = (ASPxTextBox)controls["v87"];
		var tel = (ASPxTextBox)controls["v89"];

		var data = GetReports1nfOrgInfo(zkpo.Text);
		if (data.Rows.Count > 0)
		{
			var row = data.Rows[0];
			full_name.Text = GetValue<string>(row["full_name"]);
			adr.Text = GetValue<string>(row["adr"]);
			phys_addr_zip_code.Text = GetValue<string>(row["phys_addr_zip_code"]);
			director_email.Text = GetValue<string>(row["director_email"]);
			namf.Text = GetValue<string>(row["namf"]);
			nami.Text = GetValue<string>(row["nami"]);
			namo.Text = GetValue<string>(row["namo"]);
			tel.Text = GetValue<string>(row["tel"]);
		}
	}

	DataTable GetOrandodavecUserInfo(string namf)
	{
		var connection = CommonUtils.ConnectToDatabase();
		if (connection == null) throw new Exception("Database GUKV not found");
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = @"select * from dict_orandodavec_user where namf = '" + (namf ?? "").Replace("'", "''") + "'";
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}
		return dataTable;
	}

	protected void v52_TextChanged(object sender, EventArgs e)
	{
		var controls = GetAllControls();
		var namf = (ASPxTextBox)controls["v52"];
		var nami = (ASPxTextBox)controls["v53"];
		var namo = (ASPxTextBox)controls["v54"];
		var email = (ASPxTextBox)controls["v55"];
		var phone = (ASPxTextBox)controls["v56"];

		var data = GetOrandodavecUserInfo(namf.Text);
		if (data.Rows.Count > 0)
		{
			var row = data.Rows[0];
			email.Text = GetValue<string>(row["email"]);
			namf.Text = GetValue<string>(row["namf"]);
			nami.Text = GetValue<string>(row["nami"]);
			namo.Text = GetValue<string>(row["namo"]);
			phone.Text = GetValue<string>(row["phone"]);
		}
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

public class ZvitProzoroOgoloshena
{
	public Page Page;
	public int ID;
	public bool IsOgoloshena;

	void UpdateTemplateFile(MainDocumentPart mainPart)
	{
		Dictionary<string, object> properties = new Dictionary<string, object>();

		SqlConnection connection = Utils.ConnectToDatabase();

		if (connection != null)
		{
			if (IsOgoloshena)
			{
				GetOgoloshena(connection, properties);
			}
			else
			{
				GetDogovor(connection, properties);
			}
			connection.Close();
		}

		foreach (KeyValuePair<string, object> pair in properties)
		{
			var value = pair.Value == null ? "" : pair.Value.ToString();
			ReplaceDocTagInElements(mainPart, pair.Key, value);
		}
	}

	void GetDogovor(SqlConnection connection, Dictionary<string, object> properties)
	{
		DateTime dtNow = DateTime.Now;
		string currentDate = "\xAB" + " " + dtNow.Day.ToString() + " " + "\xBB" + " " + GetDateMonthName(dtNow) + " " + dtNow.Year.ToString();
		properties.Add("{REPORT_PRINT_DATE}", currentDate);

		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = @"SELECT * FROM reports1nf_balans_free_prozoro a where a.id = " + ID;
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}
		var r = dataTable.Rows[0];

		properties.Add("{1}", r["v47"]);
		properties.Add("{2}", r["v46"]);
		properties.Add("{3}", j(", ", r["v49"], r["v51"], r["v50"]));
		properties.Add("{4}", ("" + r["v52"] + " " + r["v53"] + " " + r["v54"]).Trim());
		properties.Add("{5}", r["v201"]);
		properties.Add("{6}", r["v55"]);

		properties.Add("{7}", r["v58"]);
		properties.Add("{8}", r["v57"]);
		properties.Add("{9}", j(", ", r["v60"], r["v62"], r["v61"]));
		properties.Add("{10}", ("" + r["v63"] + " " + r["v64"] + " " + r["v65"]).Trim());
		properties.Add("{11}", r["v66"]);

		properties.Add("{12}", "Нежитлові приміщення загальною площею " + GetDecimal(r["v102"]) + " кв.м, що розташовані за адресою: " + j(", ", r["v94"], r["v95"])
									+ " та обліковуються на балансі " + r["v58"]);

		var text35 = "";
		var v35 = (decimal?)r["v35"];
		if (v35 != null)
		{
			text35 = v35.Value.ToString("#,0.00") + " грн" + " (" + NumByWords.GrnPhrase(v35.Value).ToLower() + ")" + ", без податку на додану вартість";
		}
		properties.Add("{13}", text35);

		properties.Add("{14}", "http://eis.gukv.gov.ua/gukv/Reports1NF/Report1NFFreeMap__GIS.aspx?fs_id=" + ID);
		properties.Add("{15}", r["v214"]);

		properties.Add("{16}", r["v33"]);
	}

	void GetOgoloshena(SqlConnection connection, Dictionary<string, object> properties)
	{
		DateTime dtNow = DateTime.Now;
		string currentDate = "\xAB" + " " + dtNow.Day.ToString() + " " + "\xBB" + " " + GetDateMonthName(dtNow) + " " + dtNow.Year.ToString();
		properties.Add("{REPORT_PRINT_DATE}", currentDate);

		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = @"SELECT * FROM reports1nf_balans_free_prozoro a where a.id = " + ID;
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}
		var r = dataTable.Rows[0];


		properties.Add("{{1}}", "Оренда нежитлових приміщень площею " + GetDecimal(r["v102"]) + " кв.м за адресою: " + j(", ", r["v94"], r["v95"]));

		properties.Add("{{2_1}}", r["v47"]);
		properties.Add("{{2_2}}", p("код ЄДРПОУ ", r["v46"]));
		properties.Add("{{2_3}}", p("місцезнаходження: ", j(", ", r["v50"], r["v49"], r["v51"])));
		properties.Add("{{2_4}}", j(" ", p("тел.", r["v56"]), p("e-mail: ", r["v55"])));

		properties.Add("{{3_1}}", p("", r["v58"]));
		properties.Add("{{3_2}}", p("код ЄДРПОУ ", r["v57"]));
		properties.Add("{{3_3}}", p("місцезнаходження: ", j(", ", r["v61"], r["v60"], r["v62"])));
		properties.Add("{{3_4}}", j(" ", p("тел.", r["v67"]), p("e-mail: ", r["v66"])));

		properties.Add("{{4}}", "Нежитлові приміщення загальною площею " + GetDecimal(r["v102"]) + " кв.м, що розташовані за адресою: " + j(", ", r["v94"], r["v95"]) 
									+ " та обліковуються на балансі " + r["v58"]);

		properties.Add("{{5}}", r["v3"]);

		properties.Add("{{6_1}}", "залишкова балансова вартість об’єкта оренди станом на " + " ??? – " + GetDecimal(r["v35"]) +  " грн");
		properties.Add("{{6_2}}", "первісна балансова вартість об'єкта оренди станом на " + " ??? – " + GetDecimal(r["v34"]) + " грн");

		properties.Add("{{7}}", r["v209"]);
		properties.Add("{{8}}", "" + GetDecimal(r["v21"], "0.###") + " років");
		properties.Add("{{9}}", r["v15"]);
		properties.Add("{{10}}", r["v213"]);

		properties.Add("{{11_1}}", "Загальна – " + GetDecimal(r["v102"]) + " кв.м");
		properties.Add("{{11_2}}", "Корисна – " + GetDecimal(r["v103"]) + " кв.м");

		properties.Add("{{12_1}}", "Технічний стан - " + r["v107"]);
		properties.Add("{{12_2}}", "Електропостачання - " + nayavn(r["v108"]));
		properties.Add("{{12_3}}", "Водопостачання - " + r["v109"]);
		properties.Add("{{12_4}}", "Теплопостачання - " + r["v110"]);
		properties.Add("{{12_5}}", "Газопостачання - " + r["v119"]);

		properties.Add("{{13}}", r["v214"]);
		properties.Add("{{14}}", r["v17"]);
		properties.Add("{{15}}", r["v13"]);

		properties.Add("{{16}}", "" + GetDecimal(r["v21"], "0.###") + " років");
		properties.Add("{{17}}", r["v223"]);
		properties.Add("{{18}}", r["v33"]);

		properties.Add("{{19_1}}", ("" + r["v63"] + " " + r["v64"] + " " + r["v65"]).Trim());
		properties.Add("{{19_2}}", r["v67"]);
		properties.Add("{{19_3}}", r["v66"]);

		properties.Add("{{20}}", r["v44"]);
	}

	string nayavn(object arg)
	{
		if (arg == null) return "";
		else if (arg.ToString().ToLower() == "Так") return "в наявності";
		else if (arg.ToString().ToLower() == "Ні") return "відсутнє";
		else return arg.ToString().Trim();
	}

	string p(string prefix, object arg)
	{
		var text = (arg == null ? "" : arg.ToString()).Trim();
		return (prefix + text).Trim();
	}

	string j(string del, params object[] arg)
	{
		return string.Join(del, arg.Select(q => q == null ? "" : q.ToString()).Where(q => !string.IsNullOrEmpty(q)));
	}

	string GetDecimal(object arg, string format = "0.00")
	{
		return arg is DBNull ? "" : ((decimal)arg).ToString(format);
	}

	string GetDate(object arg)
	{
		return arg is DBNull ? "" : ((DateTime)arg).ToString("dd.MM.yyyy");
	}

	void ReplaceDocTagInElements(MainDocumentPart mainPart, string tag, string replacement)
	{
		List<OpenXmlElement> elements = mainPart.Document.Body.OfType<OpenXmlElement>().ToList();

		foreach (OpenXmlElement element in elements)
		{
			if (element is WP.Paragraph)
			{
				ReplaceDocTag(mainPart, element as WP.Paragraph, tag, replacement);
			}
			else if (element is DocumentFormat.OpenXml.Wordprocessing.Table)
			{
				List<DocumentFormat.OpenXml.Wordprocessing.TableRow> rows = element.ChildElements.OfType<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ToList();

				foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow row in rows)
				{
					List<DocumentFormat.OpenXml.Wordprocessing.TableCell> cells = row.ChildElements.OfType<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ToList();

					foreach (DocumentFormat.OpenXml.Wordprocessing.TableCell cell in cells)
					{
						foreach (OpenXmlElement e in cell.ChildElements)
						{
							if (e is WP.Paragraph)
							{
								ReplaceDocTag(mainPart, e as WP.Paragraph, tag, replacement);
							}
						}
					}
				}
			}
		}
	}

	void ReplaceDocTag(MainDocumentPart mainPart, WP.Paragraph para, string tag, string replacement)
	{
		string paragraphText = GetParagraphText(para);

		// Replace the tag
		bool replaced = false;
		int pos = paragraphText.IndexOf(tag);

		while (pos >= 0)
		{
			replaced = true;

			paragraphText = paragraphText.Replace(tag, replacement);

			// Search once again
			pos = paragraphText.IndexOf(tag);
		}

		if (replaced)
		{
			WP.Run firstRun = para.OfType<WP.Run>().First<WP.Run>();

			if (firstRun == null)
			{
				firstRun = new WP.Run();
			}

			// Delete all paragraph sub-items
			para.RemoveAllChildren<WP.Run>();

			// Add the text to the first Run
			firstRun.RemoveAllChildren<WP.Text>();
			firstRun.AppendChild(new WP.Text(paragraphText));

			// Create a new run with the modified text
			para.AppendChild(firstRun);
		}
	}

	string GetParagraphText(WP.Paragraph para)
	{
		string paragraphText = "";

		List<WP.Run> runs = para.OfType<WP.Run>().ToList();

		foreach (WP.Run run in runs)
		{
			List<WP.Text> texts = run.OfType<WP.Text>().ToList();

			foreach (WP.Text text in texts)
			{
				paragraphText += text.Text;
			}
		}

		return paragraphText;
	}

	public void Run()
	{
		string templateFileName = Page.Server.MapPath("Templates/" + (IsOgoloshena ? "ProzoroOgoloshena.docx" : "ProzoroDogovor.docx"));

		if (templateFileName.Length > 0)
		{
			using (TempFile tempFile = TempFile.FromExistingFile(templateFileName))
			{
				using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(tempFile.FileName, true))
				{
					MainDocumentPart mainPart = wordDocument.MainDocumentPart;

					if (mainPart != null)
					{
						UpdateTemplateFile(mainPart);
					}

					//SaltedHash sh = new SaltedHash("123");
					//DocumentSettingsPart docSett = wordDocument.MainDocumentPart.DocumentSettingsPart;
					//DocumentProtection documentProtection = new DocumentProtection();
					//documentProtection.Edit = DocumentProtectionValues.ReadOnly;
					//OnOffValue docProtection = new OnOffValue(true);
					//documentProtection.Enforcement = docProtection;

					//documentProtection.CryptographicAlgorithmClass = CryptAlgorithmClassValues.Hash;
					//documentProtection.CryptographicProviderType = CryptProviderValues.RsaFull;
					//documentProtection.CryptographicAlgorithmType = CryptAlgorithmValues.TypeAny;
					//documentProtection.CryptographicAlgorithmSid = 4; // SHA1
					//												  //    The iteration count is unsigned
					//												  //UInt32Value uintVal = new UInt32Value();
					//												  //uintVal.Value = (uint)123;
					//documentProtection.CryptographicSpinCount = 1;
					//documentProtection.Hash = sh.Hash;
					//documentProtection.Salt = sh.Salt;
					//wordDocument.MainDocumentPart.DocumentSettingsPart.Settings.AppendChild(documentProtection);
					//wordDocument.MainDocumentPart.DocumentSettingsPart.Settings.Save();

					wordDocument.Close();

					// Dump the document contents to the output stream
					System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

					var outfile = (IsOgoloshena ? "ProzoroOgoloshena.docx" : "ProzoroDogovor.docx");
					Page.Response.Clear();
					Page.Response.ClearHeaders();
					Page.Response.ClearContent();
					Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
					Page.Response.AddHeader("content-disposition", "attachment; filename=" + outfile + "; size=" + info.Length.ToString());

					// Pipe the stream contents to the output stream
					using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName,
						System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
					{
						stream.CopyTo(Page.Response.OutputStream);
					}
				}

				Page.Response.End();
			}
		}
	}

	string GetDateMonthName(object date)
	{
		if (date is DateTime)
		{
			switch (((DateTime)date).Month)
			{
				case 1:
					return Resources.Strings.Month1;

				case 2:
					return Resources.Strings.Month2;

				case 3:
					return Resources.Strings.Month3;

				case 4:
					return Resources.Strings.Month4;

				case 5:
					return Resources.Strings.Month5;

				case 6:
					return Resources.Strings.Month6;

				case 7:
					return Resources.Strings.Month7;

				case 8:
					return Resources.Strings.Month8;

				case 9:
					return Resources.Strings.Month9;

				case 10:
					return Resources.Strings.Month10;

				case 11:
					return Resources.Strings.Month11;

				case 12:
					return Resources.Strings.Month12;
			}
		}

		return "";
	}

}


 
/// <summary>
/// Класс отображения суммы прописью.
/// 3 варианта - рубли, доллары и просто для использования
/// других любых единиц (вагоны, мешки и т.п.)
/// --------------------------------------
/// Автор - Глеб Уфимцев (dnkvpb@nm.ru)
/// </summary>
 
public class NumByWords
{
	public static string GrnPhrase(decimal money)
	{
		return CurPhrase(money, "гривня", "гривні", "гривень", "копійка", "копійки", "копійок");
	}


	public static string NumPhrase(ulong Value, bool IsMale)
	{
		if (Value == 0UL) return "Нуль";
		string[] Dek1 = { "", " од", " дв", " три", " чотири", " п'ять", " шість", " сім", " вісім", " дев'ять", " десять", " одинадцять", " дванадцять", " тринадцять", " чотирнадцять", " п'ятнадцять", " шістнадцять", " сімнадцять", " вісімнадцять", " дев'ятнадцять" };
		string[] Dek2 = { "", "", " двадцять", " тридцять", " сорок", " п'ятдесят", " шістдесят", " сімдесят", " вісімдесят", " дев'яносто" };
		string[] Dek3 = { "", " сто", " двісті", " триста", " чотириста", " п'ятсот", " шістсот", " сімсот", " вісімсот", " дев'ятсот" };
		string[] Th = { "", "", " тисяч", " мільйон", " міліард", " триліон", " квадриліон", " квинтиліон" };
		string str = "";
		for (byte th = 1; Value > 0; th++)
		{
			ushort gr = (ushort)(Value % 1000);
			Value = (Value - gr) / 1000;
			if (gr > 0)
			{
				byte d3 = (byte)((gr - gr % 100) / 100);
				byte d1 = (byte)(gr % 10);
				byte d2 = (byte)((gr - d3 * 100 - d1) / 10);
				if (d2 == 1) d1 += (byte)10;
				bool ismale = (th > 2) || ((th == 1) && IsMale);
				str = Dek3[d3] + Dek2[d2] + Dek1[d1] + EndDek1(d1, ismale) + Th[th] + EndTh(th, d1) + str;
			};
		};
		str = str.Substring(1, 1).ToUpper() + str.Substring(2);
		return str;
	}

	#region Private members
	private static string CurPhrase(decimal money,
		string word1, string word234, string wordmore,
		string sword1, string sword234, string swordmore)
	{
		//money=decimal.Round(money,2);
		decimal decintpart = decimal.Truncate(money);
		ulong intpart = decimal.ToUInt64(decintpart);
		string str = NumPhrase(intpart, true) + " ";
		byte endpart = (byte)(intpart % 100UL);
		if (endpart > 19) endpart = (byte)(endpart % 10);
		switch (endpart)
		{
			case 1: str += word1; break;
			case 2:
			case 3:
			case 4: str += word234; break;
			default: str += wordmore; break;
		}
		byte fracpart = decimal.ToByte((money - decintpart) * 100M);
		str += " " + ((fracpart < 10) ? "0" : "") + fracpart.ToString() + " ";
		if (fracpart > 19) fracpart = (byte)(fracpart % 10);
		switch (fracpart)
		{
			case 1: str += sword1; break;
			case 2:
			case 3:
			case 4: str += sword234; break;
			default: str += swordmore; break;
		};
		return str;
	}
	private static string EndTh(byte ThNum, byte Dek)
	{
		bool In234 = ((Dek >= 2) && (Dek <= 4));
		bool More4 = ((Dek > 4) || (Dek == 0));
		if (((ThNum > 2) && In234) || ((ThNum == 2) && (Dek == 1))) return "і";
		else if ((ThNum > 2) && More4) return "ів";
		else if ((ThNum == 2) && In234) return "і";
		else return "";
	}
	private static string EndDek1(byte Dek, bool IsMale)
	{
		if ((Dek > 2) || (Dek == 0)) return "";
		else if (Dek == 1)
		{
			if (IsMale) return "ин";
			else return "на";
		}
		else
		{
			if (IsMale) return "а";
			else return "а";
		}
	}
	#endregion
}