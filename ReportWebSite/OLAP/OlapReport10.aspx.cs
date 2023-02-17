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
		WebChartControl FullStackedBarChart = WebChartControl1;

		// Create two full-stacked bar series.
		var series1 = new Series("Кількість об'єктів на балансі", ViewType.Bar3D);
		series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
		//((Bar3DSeriesLabel)series1.Label).po = PointLabelPosition.Outside;
		//((Bar3DSeriesLabel)series1.Label).Angle = 315;
		series1.Label.TextOrientation = TextOrientation.Horizontal;
		series1.Label.TextPattern = "{V:F0}";


		// Add both series to the chart.
		FullStackedBarChart.Series.AddRange(new Series[] { series1 });

		// Set the numerical argument scale types for the series,
		// as it is qualitative, by default.
		series1.ArgumentScaleType = ScaleType.Qualitative;

		// Access the view-type-specific options of the series.
		//((FullStackedBarSeriesView)series1.View).BarWidth = 0.4;

		// Access the type-specific options of the diagram.
		//((XYDiagram)FullStackedBarChart.Diagram).EnableAxisXZooming = true;

		// Hide the legend (if necessary).
		FullStackedBarChart.Legend.Visible = true;

		// Add the chart to the form.
		//FullStackedBarChart.Dock = DockStyle.Fill;

		int yy1 = 2017;
		int yy2 = 2022;
		for (int yy = yy1; yy <= yy2; yy++)
		{
			var sql = @"select
	sum(v1) as r1 
	from OlapReportTotalYear A
	where yy = " + yy;

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
				var r1 = (int)row["r1"];

				FullStackedBarChart.Series[0].Points.Add(new SeriesPoint
				{
					Argument = "" + yy,
					Values = new[] { (double)r1 },
				});
			}
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