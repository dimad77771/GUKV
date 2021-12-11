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
		var seria1 = new Series("Нараховано орендної плати за звітний період грн. (без ПДВ)", ViewType.Area);
		var seria2 = new Series("Надходження орендної плати за звітний період всього грн. (без ПДВ)", ViewType.Area);
		WebChartControl1.Series.Add(seria1);
		WebChartControl1.Series.Add(seria2);
		seria1.ArgumentScaleType = ScaleType.Qualitative;
		seria2.ArgumentScaleType = ScaleType.Qualitative;

		var sql = @"select
A.yy,
sum(A.v11) v1,
sum(A.v13) v2
from OlapReportTotalYear A
group by A.yy
order by 1
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
			var yy = (int)row["yy"];
			var v1 = (decimal)row["v1"];
			var v2 = (decimal)row["v2"];

			seria1.Points.Add(new SeriesPoint
			{
				Argument = "" + yy,
				Values = new[] { (double)v1 },
			});
			seria2.Points.Add(new SeriesPoint
			{
				Argument = "" + yy,
				Values = new[] { (double)v2 },
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