﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web;
using FirebirdSql.Data.FirebirdClient;
using Syncfusion.XlsIO;


public partial class Reports1NF_InventoryObjectList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];
		if (string.IsNullOrEmpty(reportIdStr))
		{
			ReportID = -1;
		}
		else
		{
			ReportID = int.Parse(reportIdStr);
		}

        // Set the parameters for SQL queries
        SqlDataSourceBalansStatus.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
        SqlDataSourceReportBalans.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
        

        if (ReportID > 0)
        {
			Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
        }

        //if (!Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
        //{
        //    if (SectionMenu.Items.Count > 6)
        //        SectionMenu.Items[6].ClientVisible = false;
        //    ButtonSendAll.ClientVisible = false;
        //    CPStatus.ClientVisible = false;
        //}

        // Bind data to the grid dynamically
        this.ProcessGridDataFetch(ViewState, PrimaryGridView);
        PrimaryGridView.Settings.VerticalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;
   }

    protected int ReportID
    {
        get
        {
            object reportId = ViewState["REPORT_ID"];

            if (reportId is int)
            {
                return (int)reportId;
            }

            return 0;
        }

        set
        {
            ViewState["REPORT_ID"] = value;
        }
    }

    protected void PrimaryGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        //pgv
        if (e.Parameters.StartsWith("columns:"))
        {

            Utils.ProcessDataGridSaveLayoutCallback(e.Parameters, PrimaryGridView, Utils.GridIDBalans_Objects, ""); 
            return;
        }

        PrimaryGridView.DataBind();
    }

    protected void PrimaryGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "is_submitted")
        {
            if (e.CellValue is string && ((string)e.CellValue).Length < 3)
            {
                e.Cell.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void CPStatus_Callback(object sender, CallbackEventArgsBase e)
    {
        BalansStatusForm.DataBind();
    }

    protected void CPProgress_Callback(object sender, CallbackEventArgsBase e)
    {
        Reports1NFUtils.ProcessProgressPanelCallback(sender as ASPxCallbackPanel, e.Parameter, ProgressSendAll, LabelProgress,
            ReportID, new SendAllBalansObjectsWorkItem(), "BALANS_WORK_ITEM_", Resources.Strings.ProcessingBalansObjects);
    }

    protected void ASPxButton_BalansObjects_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterBalansObjects, PrimaryGridView, "", ViewState["PrimaryGridView.DataSourceID"] as string,
			afterBuildXlsx: ASPxButton_BalansObjects_ExportXLS_AfterBuildXlsx);
    }

    protected void ASPxButton_BalansObjects_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterBalansObjects, PrimaryGridView, "", ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void PrimaryGridView_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        //Utils.ProcessGridSortByBuildingNumber(e);
    }


	void ASPxButton_BalansObjects_ExportXLS_AfterBuildXlsx(string excelfilename)
	{
		//return;

		//var dfile = @"c:\tmp\debug2.txt";
		//System.IO.File.AppendAllText(dfile, "start" + "\n");

		var excelEngineMain = new ExcelEngine();
		var workbook = excelEngineMain.Excel.Workbooks.Open(excelfilename);
		var worksheet = workbook.Worksheets[0];

		var row = worksheet.Rows[1];
		var dcols = new[]
		{
			"Сфера управління об'єкта (місто/район)",
			"Балансоутримувач",
			"Код ЄДРПОУ",
			"Район",
			"Вулиця",
			"Номер",
			"Назва Об'єкту",
			"Короткий опис об'єкта (адміністративне, виробниче, навчальний заклад, заклад охорони здоров'я тощо)",
			"Тип об'єкту",
			"Загальний технічний стан (задовільний, потребує проведення ремонтних робіт, аварійний)",
			"Площа, що перебуває в комунальній власності територіальної громади міста Києва, кв. м",
			"Загальна площа об'єкта, кв. м",
			"Залишкова вартість, тис. грн.",
			"Площа, що використвує-ться для власних потреб, кв. м",
			"Площа, що тимчасово не використовується та може бути передана в орендне користвання, кв. м",
		};
		var sumcols = new[]
		{
			"Площа, що перебуває в комунальній власності територіальної громади міста Києва, кв. м",
			"Загальна площа об'єкта, кв. м",
			"Залишкова вартість, тис. грн.",
			"Площа, що використвує-ться для власних потреб, кв. м",
			"Площа, що тимчасово не використовується та може бути передана в орендне користвання, кв. м",
			"Площа в орендному користуванні, кв. м",
		};
		var last_colnum = -1;
		for(int i = 0; i < row.Cells.Length; i++)
		{
			var text = row.Cells[i].DisplayText;
			if (!dcols.Contains(text))
			{
				last_colnum = i;
				break;
			}
		}

		var sumcolnums = new Dictionary<int, bool>();
		for (int i = 0; i < row.Cells.Length; i++)
		{
			var text = row.Cells[i].DisplayText;
			if (sumcols.Contains(text))
			{
				sumcolnums.Add(i, dcols.Contains(text));
			}
		}

		var groups_rows = new List<List<int>>();
		var last_dtext = "";
		for(int r = 2; r < worksheet.Rows.Length; r++)
		{
			//if (r % 10 == 0) System.IO.File.AppendAllText(dfile, "r1=" + r + " / " + worksheet.Rows.Length + "\n");

			var erow = worksheet.Rows[r];
			var this_dtext = string.Join(" *** ", Enumerable.Range(0, last_colnum).Select(q => erow.Cells[q].DisplayText));
			if (!string.IsNullOrEmpty(this_dtext) && this_dtext == last_dtext)
			{
				var last_group = groups_rows.LastOrDefault();
				if (last_group != null && last_group.Last() == r - 1)
				{
					last_group.Add(r);
				}
				else
				{
					var new_group = new List<int> { r - 1, r };
					groups_rows.Add(new_group);
				}
			}

			last_dtext = this_dtext;
		}

		int gg = 0;
		foreach(var grows in groups_rows)
		{
			gg++;
			//if (gg % 10 == 0) System.IO.File.AppendAllText(dfile, "gg=" + gg + " / " + groups_rows.Count + "\n");

			for (int colnum = 0; colnum < last_colnum; colnum++)
			{
				var r1 = grows.First();
				var r2 = grows.Last();
				var mrange = worksheet.Range[r1 + 1, colnum + 1, r2 + 1, colnum + 1];
				mrange.Merge(true);
				mrange.BorderAround();
				//mrange.VerticalAlignment = ExcelVAlign.VAlignTop;
			}
		}

		//System.IO.File.AppendAllText(dfile, "hhh 1" + "\n");

		worksheet.UsedRange.BorderInside();

		//System.IO.File.AppendAllText(dfile, "hhh 2" + "\n");

		var last_rownum = worksheet.UsedRange.Rows.Length;
		var allNotFirstGroupingRows = new HashSet<int>(groups_rows.Select(q => q.Skip(1)).SelectMany(q => q));
		int nnn = 0;
		foreach (var sumcolinfo in sumcolnums)
		{
			nnn++;
			double sumval = 0;
			var colnum = sumcolinfo.Key;
			var in_dcol = sumcolinfo.Value;
			for (int r = 2; r < worksheet.Rows.Length; r++)
			{
				//if (r % 10 == 0) System.IO.File.AppendAllText(dfile, "ee=" + nnn + "; r1=" + r + " / " + worksheet.Rows.Length + "\n");

				if (in_dcol && allNotFirstGroupingRows.Contains(r))
				{
					continue;
				}

				var val2 = worksheet[r + 1, colnum + 1].Value2;
				//var val222 = worksheet.Rows[r].Cells[colnum].Value2;
				var val = val2 as Double?;
				sumval += (val ?? 0);
			}

			var srange = worksheet.Range[last_rownum + 1, colnum + 1];
			srange.Value2 = sumval;
		}

		//System.IO.File.AppendAllText(dfile, "hhh 3" + "\n");

		workbook.Save();
		//System.IO.File.AppendAllText(dfile, "hhh 4" + "\n");
		workbook.Close();
		excelEngineMain.Dispose();
		//System.IO.File.AppendAllText(dfile, "hhh 5" + "\n");
	}
}