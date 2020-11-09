using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using DevExpress.Web;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Spreadsheet;

public partial class OrendnaPlata_OrPlataVipiski : System.Web.UI.Page
{
    #region Nested classes

    /// <summary>
    /// Describes a single organization entry within 'Vipiski' report
    /// </summary>
    protected class OrganizationPayments
    {
        public string orgName = "";
        public string orgZkpo = "";
        public string orgOccupation = "";

        public Dictionary<uint, decimal> dailyPayments = new Dictionary<uint, decimal>();

        public decimal orgTotal = 0m;

        public OrganizationPayments()
        {
        }
    }

    #endregion (Nested classes)

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        // Generate a unique key for this instance of the page
        GetPageUniqueKey();

        // Adjust enabled/disabled state of some controls
        EditFilterByZKPO.ClientEnabled = CheckFilterByZKPO.Checked;
        EditFilterByOrgName.ClientEnabled = CheckFilterByOrgName.Checked;
        ListBoxFilterByOccupation.ClientEnabled = CheckFilterByOccupation.Checked;

        // Disable the 'Vipiski' menu item if user is RDA
        if (Utils.RdaDistrictID > 0)
        {
            SectionMenu.Items[6].ClientVisible = false;

            Response.Redirect(Page.ResolveClientUrl("~/Account/RestrictedObject.aspx"));
        }
    }

    protected void ButtonGenerateVipiski_Click(object sender, EventArgs e)
    {
        if (DateEditFilterByStartDate.Value is DateTime && DateEditFilterByEndDate.Value is DateTime)
        {
            using (TempFile tempFile = new TempFile())
            {
                DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadsheet = OpenXMLExcel.CreateWorkbook(tempFile.FileName);

                if (spreadsheet != null)
                {
                    GenerateVipiski(tempFile, spreadsheet,
                        (DateTime)DateEditFilterByStartDate.Value,
                        (DateTime)DateEditFilterByEndDate.Value);

                    spreadsheet.Close();

                    // Dump the document contents to the output stream
                    System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=Paymentreport.xlsx; size=" + info.Length.ToString());

                    // Pipe the stream contents to the output stream
                    using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName,
                        System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
                    {
                        stream.CopyTo(Response.OutputStream);
                    }
                }

                Response.End();
            }
        }
    }

    protected void GenerateVipiski(TempFile tempFile,
        DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadsheet,
        DateTime periodStart,
        DateTime periodEnd)
    {
        OpenXMLExcel.AddBasicStyles(spreadsheet);
        OpenXMLExcel.AddWorksheet(spreadsheet, Resources.Strings.VipiskiWorksheetTitle);

        DocumentFormat.OpenXml.Spreadsheet.Worksheet worksheet = spreadsheet.WorkbookPart.WorksheetParts.First().Worksheet;

        uint nCurrentRow = 0;

        // Add the report header
        int numDays = (periodEnd - periodStart).Days + 1;

        nCurrentRow++;

        OpenXMLExcel.SetCellValue(spreadsheet, worksheet, 1, nCurrentRow, "Код ЄДРПОУ", 9, false);
        OpenXMLExcel.SetCellValue(spreadsheet, worksheet, 2, nCurrentRow, "Район, балансоутримувач або галузь", 9, false);
        OpenXMLExcel.SetCellValue(spreadsheet, worksheet, 3, nCurrentRow, "Всього, грн.", 9, false);

        for (uint i = 0; i < numDays; i++)
        {
            DateTime dt = periodStart.Date.AddDays(i);

            OpenXMLExcel.SetCellValue(spreadsheet, worksheet, i + 4, nCurrentRow, dt.ToString("dd MMM"), 9, false);
        }

        // Get the report data
        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            string curOccupation = "";
            decimal curTotal = 0m;

            Dictionary<int, OrganizationPayments> orgPayments = new Dictionary<int, OrganizationPayments>();
            Dictionary<uint, decimal> dailyTotals = new Dictionary<uint, decimal>();

            string query = @"SELECT 
                   v.organization_id,
                   org.full_name,
                   org.zkpo_code,
                   v.payment_date,
                   v.payment_sum,
                   dict_rent_occupation.name AS 'rent_occupation'
            FROM
                rent_vipiski v
                LEFT OUTER JOIN rent_balans_org bal_org ON bal_org.organization_id = v.org_balans_id
                LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = bal_org.rent_occupation_id
                LEFT OUTER JOIN organizations org ON org.id = v.organization_id and (org.is_deleted is null or org.is_deleted = 0)
            WHERE
                v.payment_date >= @dt_start AND v.payment_date <= @dt_end $ # !
            ORDER BY
                bal_org.rent_occupation_id, full_name, payment_date";

            // Consider all parameters
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            string filterZkpo = EditFilterByZKPO.Text.Trim();
            string filterOrgName = EditFilterByOrgName.Text.Trim();

            if (CheckFilterByZKPO.Checked && filterZkpo.Length > 0)
            {
                query = query.Replace("$", " AND org.zkpo_code LIKE @p_zkpo");

                parameters.Add("p_zkpo", "%" + filterZkpo + "%");
            }
            else
            {
                query = query.Replace("$", "");
            }

            if (CheckFilterByOrgName.Checked && filterOrgName.Length > 0)
            {
                query = query.Replace("#", " AND org.full_name LIKE @p_name");

                parameters.Add("p_name", "%" + filterOrgName + "%");
            }
            else
            {
                query = query.Replace("#", "");
            }

            string occupationList = "";

            foreach (ListEditItem item in ListBoxFilterByOccupation.Items)
            {
                if (item.Selected)
                {
                    if (occupationList.Length > 0)
                    {
                        occupationList += ", ";
                    }

                    occupationList += item.Value.ToString();
                }
            }

            if (CheckFilterByOccupation.Checked && occupationList.Length > 0)
            {
                query = query.Replace("!", " AND bal_org.rent_occupation_id IN (" + occupationList + ")");
            }
            else
            {
                query = query.Replace("!", "");
            }

            // Get the data from SQL Server
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("dt_start", periodStart.Date));
                cmd.Parameters.Add(new SqlParameter("dt_end", periodEnd.Date));

                foreach (KeyValuePair<string, object> param in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int organizationId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        string orgName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string orgZkpo = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        DateTime paymentDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                        decimal paymentSum = reader.IsDBNull(4) ? -1m : reader.GetDecimal(4);
                        string orgOccupation = reader.IsDBNull(5) ? "" : reader.GetString(5);

                        if (organizationId > 0 && orgName.Length > 0 && paymentDate > DateTime.MinValue && paymentSum > 0)
                        {
                            // The report must contain a total for each different Occupation.
                            // If a new occupation is encountered, dump the current total and the current rows
                            if (orgOccupation != curOccupation)
                            {
                                AddVipiskiSection(spreadsheet, worksheet, ref nCurrentRow, orgPayments,
                                    curOccupation, curTotal, dailyTotals, periodStart, periodEnd);

                                // Reset the state of all totals
                                curOccupation = orgOccupation;
                                curTotal = 0m;

                                orgPayments.Clear();
                                dailyTotals.Clear();
                            }

                            curTotal += paymentSum;

                            OrganizationPayments payments = null;

                            if (!orgPayments.TryGetValue(organizationId, out payments))
                            {
                                payments = new OrganizationPayments();

                                payments.orgName = orgName;
                                payments.orgZkpo = orgZkpo;
                                payments.orgOccupation = orgOccupation;

                                orgPayments.Add(organizationId, payments);
                            }

                            // Calculate the daily totals and organization totals
                            uint dayIndex = GetDayIndex(paymentDate, periodStart);
                            decimal dailyTotal = 0m;
                            decimal orgDailyTotal = 0m;

                            if (dailyTotals.TryGetValue(dayIndex, out dailyTotal))
                            {
                                dailyTotals[dayIndex] = dailyTotal + paymentSum;
                            }
                            else
                            {
                                dailyTotals[dayIndex] = paymentSum;
                            }

                            payments.orgTotal += paymentSum;

                            if (payments.dailyPayments.TryGetValue(dayIndex, out orgDailyTotal))
                            {
                                payments.dailyPayments[dayIndex] = orgDailyTotal + paymentSum;
                            }
                            else
                            {
                                payments.dailyPayments[dayIndex] = paymentSum;
                            }
                        }
                    }

                    // Process the final section
                    if (orgPayments.Count > 0)
                    {
                        AddVipiskiSection(spreadsheet, worksheet, ref nCurrentRow, orgPayments,
                            curOccupation, curTotal, dailyTotals, periodStart, periodEnd);
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        // Set column width
        OpenXMLExcel.SetColumnWidth(worksheet, 1, 15);
        OpenXMLExcel.SetColumnWidth(worksheet, 2, 50);
        OpenXMLExcel.SetColumnWidth(worksheet, 3, 12);

        for (int i = 0; i < numDays; i++)
        {
            OpenXMLExcel.SetColumnWidth(worksheet, i + 4, 11);
        }

        worksheet.Save();
        spreadsheet.WorkbookPart.Workbook.Save();
    }

    protected void AddVipiskiSection(
        DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadsheet,
        DocumentFormat.OpenXml.Spreadsheet.Worksheet worksheet,
        ref uint nCurrentRow,
        Dictionary<int, OrganizationPayments> orgPayments,
        string occupation,
        decimal total,
        Dictionary<uint, decimal> dailyTotals,
        DateTime periodStart,
        DateTime periodEnd)
    {
        int numDays = (periodEnd - periodStart).Days + 1;

        // Add the 'totals' row
        nCurrentRow++;

        OpenXMLExcel.SetCellValue(spreadsheet, worksheet, 1, nCurrentRow, "", 6, false);
        OpenXMLExcel.SetCellValue(spreadsheet, worksheet, 2, nCurrentRow, occupation, 6, false);
        OpenXMLExcel.SetCellValue(spreadsheet, worksheet, 3, nCurrentRow, (double)total, 7, false);

        for (uint i = 0; i < numDays; i++)
        {
            decimal dailyTotal = 0m;

            if (!dailyTotals.TryGetValue(i, out dailyTotal))
            {
                dailyTotal = 0m;
            }

            OpenXMLExcel.SetCellValue(spreadsheet, worksheet, i + 4, nCurrentRow, (double)dailyTotal, 7, false);
        }

        // Add an individual row for each organization
        foreach (KeyValuePair<int, OrganizationPayments> payment in orgPayments)
        {
            nCurrentRow++;

            OpenXMLExcel.SetCellValue(spreadsheet, worksheet, 1, nCurrentRow, payment.Value.orgZkpo, false, false);
            OpenXMLExcel.SetCellValue(spreadsheet, worksheet, 2, nCurrentRow, payment.Value.orgName, 8, false);
            OpenXMLExcel.SetCellValue(spreadsheet, worksheet, 3, nCurrentRow, (double)payment.Value.orgTotal, 4, false);

            for (uint i = 0; i < numDays; i++)
            {
                decimal orgDailyTotal = 0m;

                if (!payment.Value.dailyPayments.TryGetValue(i, out orgDailyTotal))
                {
                    orgDailyTotal = 0m;
                }

                if (orgDailyTotal != 0m)
                {
                    OpenXMLExcel.SetCellValue(spreadsheet, worksheet, i + 4, nCurrentRow, (double)orgDailyTotal, 4, false);
                }
            }
        }
    }

    protected uint GetDayIndex(DateTime date, DateTime periodStart)
    {
        return (uint)(date.Date - periodStart.Date).Days;
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

    #region Data binding support

    public string EvaluateObjectLink(object buildingId, object address)
    {
        if (!(address is string))
        {
            return "";
        }

        if (!(buildingId is int))
        {
            return address.ToString();
        }

        return "<a href=\"javascript:ShowObjectCardSimple(" + buildingId.ToString() + ")\">" + address.ToString() + "</a>";
    }

    public string EvaluateOrganizationLink(object organizationId, object name)
    {
        if (!(name is string))
        {
            return "";
        }

        if (!(organizationId is int))
        {
            return name.ToString();
        }

        return "<a href=\"javascript:ShowOrganizationCard(" + organizationId.ToString() + ")\">" + name.ToString() + "</a>";
    }

    #endregion (Data binding support)
}