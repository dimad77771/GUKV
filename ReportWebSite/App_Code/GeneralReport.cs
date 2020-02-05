using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using DevExpress.XtraReports.UI;
using DevExpress.Web.ASPxGridView;

public class GridRowIndexRecord
{
    private int index = -1;

    public GridRowIndexRecord(int i)
    {
        this.index = i;
    }

    public int RowIndex
    {
        get
        {
            return index;
        }
    }
}

/// <summary>
/// XtraReport which is generated automatically from the Grid Control
/// </summary>
public class GeneralReport : XtraReport
{
    public ReportHeaderBand reportHeaderBand = new ReportHeaderBand();
    public DetailBand detailBand = new DetailBand();

    private Dictionary<int, XRLabel> detailLabels = new Dictionary<int, XRLabel>();

    private ASPxGridView grid = null;

    private const int maxReportWidth = 600;
    private const int primaryLabelHeight = 24;
    private const int secondaryLabelHeight = 24;
    private const int otherLabelHeight = 24;
    private const int otherLabelIndent = 50;

	public GeneralReport()
	{
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            reportHeaderBand,
            detailBand } );
	}

    private XRLabel CreateLabel(System.Drawing.PointF location, System.Drawing.SizeF size, System.Drawing.Font font)
    {
        XRLabel label = new XRLabel();

        label.LocationF = location;
        label.SizeF = size;
        label.Font = font;

        label.AutoWidth = false;
        label.WordWrap = true;
        label.CanGrow = true;

        return label;
    }

    public void FillDetailFromGrid(ASPxGridView gr,
        string[] autoGroupFields,
        string[] primaryFields,
        string[] secondaryFields,
        string title,
        string[] subtitles)
    {
        grid = gr;

        // Categorize grid columns
        List<GridViewDataColumn> groupColumns = new List<GridViewDataColumn>();
        List<GridViewDataColumn> primaryColumns = new List<GridViewDataColumn>();
        List<GridViewDataColumn> secondaryColumns = new List<GridViewDataColumn>();
        List<GridViewDataColumn> otherColumns = new List<GridViewDataColumn>();

        foreach (GridViewDataColumn column in grid.Columns)
        {
            if (column.Visible)
            {
                string fieldName = column.FieldName.ToLower();

                if (autoGroupFields.Contains(fieldName))
                {
                    groupColumns.Add(column);
                }
                else if (primaryFields.Contains(fieldName))
                {
                    primaryColumns.Add(column);
                }
                else if (secondaryFields.Contains(fieldName))
                {
                    secondaryColumns.Add(column);
                }
                else
                {
                    otherColumns.Add(column);
                }
            }
        }

        // Generate labels for the primary columns
        if (primaryColumns.Count > 0)
        {
            int primaryColumnWidth = maxReportWidth / primaryColumns.Count;

            for (int i = 0; i < primaryColumns.Count; i++)
            {
                XRLabel label = CreateLabel(
                    new System.Drawing.PointF(i * primaryColumnWidth, 0),
                    new System.Drawing.SizeF(primaryColumnWidth, primaryLabelHeight),
                    new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold));

                detailLabels[primaryColumns[i].Index] = label;
                detailBand.Controls.Add(label);
            }
        }

        // Generate labels for the secondary columns
        if (secondaryColumns.Count > 0)
        {
            int secondaryColumnWidth = maxReportWidth / secondaryColumns.Count;

            for (int i = 0; i < secondaryColumns.Count; i++)
            {
                XRLabel label = CreateLabel(
                    new System.Drawing.PointF(i * secondaryColumnWidth, primaryLabelHeight),
                    new System.Drawing.SizeF(secondaryColumnWidth, secondaryLabelHeight),
                    new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Italic));

                detailLabels[secondaryColumns[i].Index] = label;
                detailBand.Controls.Add(label);
            }
        }

        // Generate labels for all other visible columns
        if (otherColumns.Count > 0)
        {
            int otherColumnWidth = (maxReportWidth - otherLabelIndent) / otherColumns.Count;

            for (int i = 0; i < otherColumns.Count; i++)
            {
                XRLabel label = CreateLabel(
                    new System.Drawing.PointF(otherLabelIndent + i * otherColumnWidth, primaryLabelHeight + secondaryLabelHeight),
                    new System.Drawing.SizeF(otherColumnWidth, otherLabelHeight),
                    new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular));

                detailLabels[otherColumns[i].Index] = label;
                detailBand.Controls.Add(label);
            }
        }

        detailBand.BeforePrint += new System.Drawing.Printing.PrintEventHandler(DetailBand_BeforePrint);

        // Create labels for the header
        XRLabel labelTitle = CreateLabel(
            new System.Drawing.PointF(0, 0),
            new System.Drawing.SizeF(maxReportWidth, 24),
            new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold));

        labelTitle.Text = title;
        labelTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

        reportHeaderBand.Controls.Add(labelTitle);

        for (int n = 0; n < subtitles.Length; n++)
        {
            XRLabel labelSubtitle = CreateLabel(
                new System.Drawing.PointF(0, n * 20 + labelTitle.HeightF),
                new System.Drawing.SizeF(maxReportWidth, 20),
                new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Regular));

            labelSubtitle.Text = subtitles[n];

            reportHeaderBand.Controls.Add(labelSubtitle);
        }

        // Create a fake data source
        this.DataSource = CreateDataSource();
    }

    private ArrayList CreateDataSource()
    {
        ArrayList list = new ArrayList();

        for (int i = 0; i < grid.VisibleRowCount; i++)
        {
            list.Add(new GridRowIndexRecord(i));
        }

        return list;
    }

    private void DetailBand_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        object row = this.GetCurrentRow();

        if (row is GridRowIndexRecord && grid != null)
        {
            int visibleRowIndex = (row as GridRowIndexRecord).RowIndex;

            object dataRow = grid.GetRow(visibleRowIndex);

            if (dataRow is System.Data.DataRowView)
            {
                System.Data.DataRowView rowView = dataRow as System.Data.DataRowView;

                object[] items = rowView.Row.ItemArray;

                // Set all labels in the Detail band to the correct values (from the current Grid row)
                foreach (KeyValuePair<int, XRLabel> pair in detailLabels)
                {
                    if (pair.Key < items.Length)
                    {
                        object item = items[pair.Key];

                        pair.Value.Text = (item != null) ? item.ToString() : "NULL";
                    }
                    else
                    {
                        pair.Value.Text = "(Undefined)";
                    }
                }
            }
        }
    }
}