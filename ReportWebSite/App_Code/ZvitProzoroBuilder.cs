using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using GUKV.Common;

public class ZvitProzoroBuilder
{
	public Page Page { get; set; }
	public int[] Ids { get; set; }

	public void Go()
	{
		string templateFileName = Page.Server.MapPath("Templates/FreeProzoro.xlsx");
		var tempFile = TempFile.FromExistingFile(templateFileName);

		var connection = CommonUtils.ConnectToDatabase();
		if (connection == null) throw new Exception("Database GUKV not found");
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = "select * from reports1nf_balans_free_prozoro where id in (" + string.Join(",", Ids.Select(q => q.ToString())) + ") order by id desc";
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}

		var workbook = new Workbook();
		workbook.LoadDocument(tempFile.FileName, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
		var wsheet = workbook.Worksheets[0];
		var usedRange = wsheet.GetUsedRange();
		for (int k = dataTable.Rows.Count - 1; k >= 0; k--)
		{
			var bcolumn1 = wsheet.Columns[k + 1];
			var bcolumn2 = wsheet.Columns["B"];
			for (int r = 0; r < usedRange.RowCount; r++)
			{
				var value = bcolumn2[r].Value.TextValue ?? "";
				var reg = Regex.Match(value, @"\{(v\d{1,3})\}");
				if (reg.Success)
				{
					var col = reg.Groups[1].Value;
					var dbval = dataTable.Rows[k][col];
					var sval = dbval.ToString();
					if (dbval is DateTime)
					{
						sval = ((DateTime)dbval).ToString("dd.MM.yyyy");
					}
					bcolumn1[r].Value = sval;
				}
			}
		}
		workbook.SaveDocument(tempFile.FileName);

		var fname = "Prozoro_" + (Ids.Length == 1 ? Ids[0].ToString() : DateTime.Now.ToString("yyyyMMdd_HHmmss")) + ".xlsx";
		var info = new System.IO.FileInfo(tempFile.FileName);
		Page.Response.Clear();
		Page.Response.ClearHeaders();
		Page.Response.ClearContent();
		Page.Response.ContentType = "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		Page.Response.AddHeader("content-disposition", "attachment; filename=" + fname + "; size=" + info.Length.ToString());
		using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
		{
			stream.CopyTo(Page.Response.OutputStream);
		}
		tempFile.Dispose();
		Page.Response.End();
	}


}