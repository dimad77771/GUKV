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
		var series1 = new Series("Загальна площа що надається в оренду кв.м.", ViewType.FullStackedBar);
		var series2 = new Series("Загальна вільна площа що може бути надана в оренду кв.м.", ViewType.FullStackedBar);
		var series3 = new Series("Площа, що використовується для для власних потреб кв.м.", ViewType.FullStackedBar);

		/*
		// Add points to them.
		series1.Points.Add(new SeriesPoint("2017", 10));
		series1.Points.Add(new SeriesPoint("2018", 12));
		//series1.Points.Add(new SeriesPoint(3, 14));
		//series1.Points.Add(new SeriesPoint(4, 17));

		series2.Points.Add(new SeriesPoint("2017", 15));
		series2.Points.Add(new SeriesPoint("2018", 18));
		//series2.Points.Add(new SeriesPoint(3, 25));
		//series2.Points.Add(new SeriesPoint(4, 33));

		series3.Points.Add(new SeriesPoint("2017", 35));
		series3.Points.Add(new SeriesPoint("2018", 15));
		*/

		// Add both series to the chart.
		FullStackedBarChart.Series.AddRange(new Series[] { series1, series2, series3 });

		// Set the numerical argument scale types for the series,
		// as it is qualitative, by default.
		series1.ArgumentScaleType = ScaleType.Qualitative;
		series2.ArgumentScaleType = ScaleType.Qualitative;
		series3.ArgumentScaleType = ScaleType.Qualitative;

		// Access the view-type-specific options of the series.
		//((FullStackedBarSeriesView)series1.View).BarWidth = 0.4;

		// Access the type-specific options of the diagram.
		//((XYDiagram)FullStackedBarChart.Diagram).EnableAxisXZooming = true;

		// Hide the legend (if necessary).
		FullStackedBarChart.Legend.Visible = true;

		// Add the chart to the form.
		//FullStackedBarChart.Dock = DockStyle.Fill;

		int yy1 = 2017;
		int yy2 = 2021;
		for (int yy = yy1; yy <= yy2; yy++)
		{
			var sql = @"select
	sum(v7) as r1, 
	sum(v10) as r2, 
	sum(v2 - v7 - v10) as r3
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
				var r1 = (decimal)row["r1"];
				var r2 = (decimal)row["r2"];
				var r3 = (decimal)row["r3"];

				FullStackedBarChart.Series[0].Points.Add(new SeriesPoint
				{
					Argument = "" + yy,
					Values = new[] { (double)r1 },
				});
				FullStackedBarChart.Series[1].Points.Add(new SeriesPoint
				{
					Argument = "" + yy,
					Values = new[] { (double)r2 },
				});
				FullStackedBarChart.Series[2].Points.Add(new SeriesPoint
				{
					Argument = "" + yy,
					Values = new[] { (double)r3 },
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