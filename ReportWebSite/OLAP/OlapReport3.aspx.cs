using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using GUKV.Common;
using System.Data.Common;
using System.Data;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;

public partial class Assessment_AssessmentObjects : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
	    // Generate a unique key for this instance of the page
        GetPageUniqueKey();

		if (!IsPostBack)
		{
			LoadData();
		}
	}

	void LoadData()
	{
		var yy1 = 2017;
		var yy2 = 2022;
		for (var yy = yy1; yy <= yy2; yy++)
		{
			var seria = new Series("" + yy, ViewType.Bar);
			//seria.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
			seria.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
			((BarSeriesLabel)seria.Label).Indent = 2;
			((SideBySideBarSeriesLabel)seria.Label).Position = BarSeriesLabelPosition.TopInside;
			seria.Label.TextOrientation = TextOrientation.TopToBottom;
			seria.Label.TextPattern = "{V:F2}";

			WebChartControl1.Series.Add(seria);
		}

		var sql = @"select
A.yy,
A.vlastyp,
A.sfera,
sum(A.v2) / 1000 as vv
from OlapReportTotalYear A
where A.sfera <> ''
group by
A.yy,
A.vlastyp,
A.sfera
order by 1,2,3";

		var connection = CommonUtils.ConnectToDatabase();
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}

		for (int r = 0; r < dataTable.Rows.Count; r++)
		{
			var row = dataTable.Rows[r];
			var yy = (int)row["yy"];
			var vlastyp = (string)row["vlastyp"];
			var sfera = (string)row["sfera"];
			var vv = (decimal)row["vv"];

			var seria = WebChartControl1.Series[yy - yy1];

			seria.Points.Add(new SeriesPoint
			{
				Argument = sfera,
				Values = new[] { (double)vv },
			});
		}

		/*
		WebChartControl1.Series[0].Points.Add(new SeriesPoint
		{
			Argument = "AAA",
			Values = new[] { 22.2 },
		});
		WebChartControl1.Series[1].Points.Add(new SeriesPoint
		{
			Argument = "AAA",
			Values = new[] { 32.2 },
		});
		WebChartControl1.Series[2].Points.Add(new SeriesPoint
		{
			Argument = "BBB",
			Values = new[] { 72.2 },
		});
		*/

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