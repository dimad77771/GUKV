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

public partial class Assessment_AssessmentObjects : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
	    // Generate a unique key for this instance of the page
        GetPageUniqueKey();

		if (!IsPostBack)
		{
			LoadData();
			LoadData2019();
			LoadData2018();
			LoadData2017();
		}
	}

	void LoadData()
	{
		var sql = @"select 
sfera, 
sum(A.v13) as sumval
from OlapReportTotalYear A
where 1 = 1
and vlastyp = 'Р' and sfera<> ''
and yy = 2020
group by sfera
order by 1 desc";

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
			var sumval = (decimal)row["sumval"];
			var sfera = (string)row["sfera"];

			var ser = WebChartControl1.Series[0];
			ser.Points.Add(new SeriesPoint
			{
				Argument = sfera,
				Values = new[] { (double)sumval },
			});
		}
	}

	void LoadData2019()
	{
		var sql = @"select 
sfera, 
sum(A.v13) as sumval
from OlapReportTotalYear A
where 1 = 1
and vlastyp = 'Р' and sfera<> ''
and yy = 2019
group by sfera
order by 1 desc";

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
			var sumval = (decimal)row["sumval"];
			var sfera = (string)row["sfera"];

			var ser = WebChartControl2.Series[0];
			ser.Points.Add(new SeriesPoint
			{
				Argument = sfera,
				Values = new[] { (double)sumval },
			});
		}
	}

	void LoadData2018()
	{
		var sql = @"select 
sfera, 
sum(A.v13) as sumval
from OlapReportTotalYear A
where 1 = 1
and vlastyp = 'Р' and sfera<> ''
and yy = 2018
group by sfera
order by 1 desc";

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
			var sumval = (decimal)row["sumval"];
			var sfera = (string)row["sfera"];

			var ser = WebChartControl3.Series[0];
			ser.Points.Add(new SeriesPoint
			{
				Argument = sfera,
				Values = new[] { (double)sumval },
			});
		}
	}

	void LoadData2017()
	{
		var sql = @"select 
sfera, 
sum(A.v13) as sumval
from OlapReportTotalYear A
where 1 = 1
and vlastyp = 'Р' and sfera<> ''
and yy = 2017
group by sfera
order by 1 desc";

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
			var sumval = (decimal)row["sumval"];
			var sfera = (string)row["sfera"];

			var ser = WebChartControl4.Series[0];
			ser.Points.Add(new SeriesPoint
			{
				Argument = sfera,
				Values = new[] { (double)sumval },
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