//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using DevExpress.Spreadsheet;
//using DevExpress.Web;
//using GUKV.Common;

//public class ZvitProzoroMultiCreate
//{
//	public void Go()
//	{
//		VsagdeBuild();

//		string templateFileName = @"D:\Projects\DKVSOURCESFINALEDITION_v20\ReportWebSite\Reports1NF\Templates\FreeProzoro____.xlsx";
//		var tempFile = TempFile.FromExistingFile(templateFileName);

//		var connection = CommonUtils.ConnectToDatabase();
//		if (connection == null) throw new Exception("Database GUKV not found");
//		var factory = DbProviderFactories.GetFactory(connection);
//		var dataTable = new DataTable();
//		using (var cmd = factory.CreateCommand())
//		{
//			cmd.CommandText = GetSql();
//			cmd.CommandType = CommandType.Text;
//			cmd.Connection = connection;
//			using (var adapter = factory.CreateDataAdapter())
//			{
//				adapter.SelectCommand = cmd;
//				adapter.Fill(dataTable);
//			}
//		}

//		var basenum = 20000;
//		var workbook = new Workbook();
//		workbook.LoadDocument(tempFile.FileName, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
//		var wsheet = workbook.Worksheets[0];
//		var usedRange = wsheet.GetUsedRange();
//		for (int k = dataTable.Rows.Count - 1; k >= 0; k--)
//		{
//			var bcolumn1 = wsheet.Columns[k + 1];

//			foreach (DataColumn dbcol in dataTable.Columns)
//			{
//				var reg = Regex.Match(dbcol.ColumnName, @"(v\d{1,3})");
//				if (reg.Success)
//				{
//					var col = reg.Groups[1].Value;
//					var dbval = dataTable.Rows[k][col];
//					var sval = dbval.ToString();
//					if (dbval is DateTime)
//					{
//						sval = ((DateTime)dbval).ToString("dd.MM.yyyy");
//					}

					
//					var excelRow = Int32.Parse(col.Replace("v",""));
//					bcolumn1[excelRow].Value = sval;
//				}
//			}
//			bcolumn1[0].Value = basenum + k;
//			VsagdeProc(bcolumn1);
//		}
//		workbook.SaveDocument(tempFile.FileName);
//		File.Copy(tempFile.FileName, @"H:\SQLServer\8_dimad77772_export\___out.xlsx");
//	}

//	void VsagdeProc(Column bcolumn)
//	{
//		foreach(var vitem in vsegda)
//		{
//			bcolumn[vitem.Key - 1].Value = vitem.Value;
//		}
//	}

//	Dictionary<int, string> vsegda = new Dictionary<int, string>();
//	void VsagdeBuild()
//	{
//		vsegda[5] = "Наказ ДКВ від 10.05.2016 №238";
//		vsegda[6] = "Україна";
//		vsegda[7] = "Київ";
//		vsegda[8] = "Київ";
//		vsegda[9] = "Хрещатик, 10";
//		vsegda[10] = "01001";
//		vsegda[11] = "Шалюта";
//		vsegda[12] = "Олег";
//		vsegda[13] = "Федорович";
//		vsegda[14] = "orenda@gukv.gov.ua";
//		vsegda[15] = "044 202 61 96";
//		vsegda[18] = "Так";
//		vsegda[19] = "лист";
//		vsegda[20] = "так";
//		vsegda[24] = "Не вимагається";
//		vsegda[26] = "Не вимагається";
//		vsegda[30] = "Україна";
//		vsegda[31] = "Наказ .......";
//		vsegda[45] = "Наказ";
//		vsegda[46] = "Україна";
//		vsegda[47] = "Київ";
//		vsegda[48] = "Київ";
//		vsegda[73] = "Нерухоме майно";
//		vsegda[75] = "Україна";
//		vsegda[76] = "Київ";
//		vsegda[77] = "Київ";
//		vsegda[79] = "Комунальна";
//		vsegda[80] = "Перелік першого типу";
//		vsegda[81] = "Включено в перелік";
//		vsegda[101] = "04000000-8";
//		vsegda[102] = "Україна";
//		vsegda[103] = "Київ";
//		vsegda[104] = "Київ";
//		vsegda[107] = "50.00000";
//		vsegda[108] = "30.00000";
//		vsegda[115] = "Так";
//	}


//	public string GetSql()
//	{
//		return @"

//select

//U.zkpo,

//B.old_organ_id,
//H.org_balans_zkpo,
//H.org_giver_zkpo,
//H.arenda_id,
//cast(round(DATEDIFF(DAY, H.rent_start_date, H.rent_finish_date) / 365.25,0) as int) v139,
//--DATEDIFF(MONTH, H.rent_start_date, H.agreement_date) v140,
//--DATEDIFF(DAY, H.rent_start_date, H.agreement_date) v141,
//H.rent_start_date,
//H.rent_finish_date,
//H.agreement_num,


//H.cost_expert_total v88,
//H.cost_expert_total v90,
//H.cost_agreement v154,

//concat(H.street_full_name, ', ', H.addr_nomer) as v77,
//concat(H.street_full_name, ', ', H.addr_nomer) as v104,

//H.rent_square v111,
//H.rent_square v112,
//R.note v109,
//R.note v110,
//(SELECT Q.name FROM dict_1nf_balans_purpose Q where Q.id = R.purpose_id) v98,
//R.purpose_str v99,

//D.zkpo_code v42,
//D.full_name v43,
//concat((select Q.name from dict_streets Q where Q.id = D.addr_street_id), ', ', D.addr_nomer) as v48,
//D.phys_addr_zip_code as v49, 
//D.director_email v53,
//(select Q.namf from zzzzz_director_title Q where Q.fio = D.director_title) v50,
//(select Q.nami from zzzzz_director_title Q where Q.fio = D.director_title) v51,
//(select Q.namo from zzzzz_director_title Q where Q.fio = D.director_title) v52,
//(select Q.tel from zzzzz_director_title Q where Q.fio = D.director_title) v54,


//----------
//--(select Q.name from dict_org_old_organ Q where Q.id = B.old_organ_id) as v43,
//(select Q.namf from zzzzz_director_title Q where Q.fio = B.director_title) v35,
//(select Q.nami from zzzzz_director_title Q where Q.fio = B.director_title) v36,
//(select Q.namo from zzzzz_director_title Q where Q.fio = B.director_title) v37,
//(select Q.tel from zzzzz_director_title Q where Q.fio = B.director_title) v39,
//concat((select Q.name from dict_streets Q where Q.id = B.addr_street_id), ', ', B.addr_nomer) as v33,
//B.phys_addr_zip_code as v34, 
//B.buhgalter_email as v38,
//B.zkpo_code v27,
//B.full_name v28,

//----------
//T.zkpo_code v2,
//T.full_name v3,
//T.director_email v13,
//(select Q.namf from zzzzz_director_title Q where Q.fio = T.director_title) v10,
//(select Q.nami from zzzzz_director_title Q where Q.fio = T.director_title) v11,
//(select Q.namo from zzzzz_director_title Q where Q.fio = T.director_title) v12,
//(select Q.tel from zzzzz_director_title Q where Q.fio = T.director_title) v14
//--into zzzzzz20221202z
//from
//--(select * from view_arenda Q) H -- where Q.arenda_id = 78483) H
//(select * from zzzzz20221202a Q) H -- where Q.arenda_id = 78483) H
//--join zzzzz20221202a P on P.arenda_id = H.arenda_id
//outer apply (select top 1 * from arenda_notes Q where Q.id = H.arenda_note_id) R
//outer apply
//(
//	SELECT 
//	TOP 1 
//	info.*
//	,CASE WHEN info.submit_date IS NULL OR info.modify_date IS NULL OR info.modify_date > info.submit_date THEN 0 ELSE 1 END AS 'report_org_info_status'
//	,dbo.[get_kazna_total](
//	(select Q.zkpo_code from reports1nf_org_info Q where report_id = info.report_id), 
//	null, null 
//	) AS 'kazna_total' 
//	FROM reports1nf_org_info info 
//	WHERE info.zkpo_code = H.org_giver_zkpo
//) T 
//outer apply
//(
//	SELECT 
//	TOP 1 
//	info.*
//	,CASE WHEN info.submit_date IS NULL OR info.modify_date IS NULL OR info.modify_date > info.submit_date THEN 0 ELSE 1 END AS 'report_org_info_status'
//	,dbo.[get_kazna_total](
//	(select Q.zkpo_code from reports1nf_org_info Q where report_id = info.report_id), 
//	null, null 
//	) AS 'kazna_total' 
//	FROM reports1nf_org_info info 
//	WHERE info.zkpo_code = H.org_balans_zkpo
//) B
//left join dict_org_old_organ_addinfo U on U.id = B.old_organ_id
//outer apply
//(
//	SELECT 
//	TOP 1 
//	info.*
//	,CASE WHEN info.submit_date IS NULL OR info.modify_date IS NULL OR info.modify_date > info.submit_date THEN 0 ELSE 1 END AS 'report_org_info_status'
//	,dbo.[get_kazna_total](
//	(select Q.zkpo_code from reports1nf_org_info Q where report_id = info.report_id), 
//	null, null 
//	) AS 'kazna_total' 
//	FROM reports1nf_org_info info 
//	WHERE info.zkpo_code = U.zkpo
//) D
//order by H.arenda_id






//		";

//	}

//}