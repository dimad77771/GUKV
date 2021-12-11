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
			LoadData(WebChartControl_2020, 2020);
			LoadData(WebChartControl_2019, 2019);
			LoadData(WebChartControl_2018, 2018);
			LoadData(WebChartControl_2017, 2017);
		}
	}

	void LoadData(WebChartControl webChart, int yy)
	{
		//var seria1 = new Series("Загальна заборгованість по орендній платі грн. (без ПДВ)", ViewType.FullStackedBar);
		var seria2 = new Series("заборгованість по орендній платі за звітний період млн.грн. (без ПДВ)", ViewType.StackedBar);
		var seria3 = new Series("заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання млн.грн. (без ПДВ)", ViewType.StackedBar);
		var seria4 = new Series("заборгованість по орендній платі з минулих років млн.грн. (без ПДВ)", ViewType.StackedBar);
		var serias = new[] { seria2, seria3, seria4 };
		webChart.Series.AddRange(serias);
		//seria1.ArgumentScaleType = ScaleType.Qualitative;
		seria2.ArgumentScaleType = ScaleType.Qualitative;
		seria3.ArgumentScaleType = ScaleType.Qualitative;
		seria4.ArgumentScaleType = ScaleType.Qualitative;
		foreach (var seria in serias)
		{
			//seria.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
			////seria.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
			//((PointSeriesLabel)seria.Label).Position = PointLabelPosition.Outside;
			//((PointSeriesLabel)seria.Label).Angle = 315;
			//seria.Label.TextOrientation = TextOrientation.Horizontal;
			//seria.Label.TextPattern = "{V:F0}";
		}


		var sql = @"select
top 10
A.nam,
A.v17 / 1000000 v1,
isnull(A.v18,0) / 1000000 v2,
isnull(A.v19,0) / 1000000 v3,
(isnull(A.v17,0) - isnull(A.v18,0) - isnull(A.v19,0)) / 1000000 v4
from OlapReportTotalYear A
where yy = " + yy + @"
order by A.v17 desc
";

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
			var nam = (string)row["nam"];
			var v1 = (decimal)row["v1"];
			var v2 = (decimal)row["v2"];
			var v3 = (decimal)row["v3"];
			var v4 = (decimal)row["v4"];

			//nam = "r-" + r;

			//seria1.Points.Add(new SeriesPoint
			//{
			//	Argument = nam,
			//	Values = new[] { (double)v1 },
			//});
			seria2.Points.Add(new SeriesPoint
			{
				Argument = nam,
				Values = new[] { (double)v2 },
			});
			seria3.Points.Add(new SeriesPoint
			{
				Argument = nam,
				Values = new[] { (double)v3 },
			});
			seria4.Points.Add(new SeriesPoint
			{
				Argument = nam,
				Values = new[] { (double)v4 },
			});
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