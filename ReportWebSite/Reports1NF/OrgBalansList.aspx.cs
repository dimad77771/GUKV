using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Data.Common;
using ExtDataEntry.Models;
using System.IO;

public partial class Reports1NF_OrgBalansList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];

        if (reportIdStr != null && reportIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);

            // Set the parameters for SQL queries
            SqlDataSourceBalansStatus.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
            SqlDataSourceReportBalans.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
        }

        if (ReportID > 0)
        {
            int reportRdaDistrictId = -1;
            int reportOrganizationId = Reports1NFUtils.GetReportOrganizationId(ReportID, ref reportRdaDistrictId);

            for (int i = 0; i < SectionMenu.Items.Count; i++)
            {
                if (SectionMenu.Items[i].NavigateUrl.IndexOf('?') < 0)
                    SectionMenu.Items[i].NavigateUrl += "?rid=" + ReportID.ToString();
            }

            if (Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
            {
                if (Utils.UserOrganizationID != reportOrganizationId)
                {
                    Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
                }
            }
            else if (Roles.IsUserInRole(Utils.RDAControllerRole))
            {
                if (Utils.RdaDistrictID != reportRdaDistrictId)
                {
                    Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
                }
            }
        }
        else
        {
            Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
        }

        if (!Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
        {
            if (SectionMenu.Items.Count > 6)
                SectionMenu.Items[6].ClientVisible = false;

            ButtonSendAll.ClientVisible = false;
            CPStatus.ClientVisible = false;
        }

        // Bind data to the grid dynamically
        this.ProcessGridDataFetch(ViewState, PrimaryGridView);
        PrimaryGridView.Settings.VerticalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;

        if (!Roles.IsUserInRole(Utils.RDAControllerRole))
        {
            SectionMenu.Visible = (Utils.GetLastReportId() <= 0);
        }
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
        this.ExportGridToXLS(ASPxGridViewExporterBalansObjects, PrimaryGridView, "", ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_BalansObjects_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterBalansObjects, PrimaryGridView, "", ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void PrimaryGridView_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
    }

	protected void TestMoveProc_Click(object sender, EventArgs e)
	{
        new Load_Reports1NF_OrgBalansList_20231201().Run();
    }
}



public class Load_Reports1NF_OrgBalansList_20231201
{
    SqlConnection connection;
    SqlTransaction transaction;
    int rep_id = 978;

    public void Run()
    {
        try
        {
            var bti_attachfiles = Path.Combine(LLLLhotorowUtils.ImgFreeSquareRootFolder, "reports1nf_balans_bti_attachfiles");
            var dir_2NF = Path.Combine(LLLLhotorowUtils.ImgContentRootFolder, "2NF");
        
            int npp = 0;
            using (connection = Utils.ConnectToDatabase())
            using (transaction = connection.BeginTransaction())
            {
                var bdata = GetDataTable("select * from zzzzz20231125a "); //where balans_id = 55215
                for (var br = 0; br < bdata.Rows.Count; br++)
                {
                    var balans_id = (int)bdata.Rows[br]["balans_id"];
                    var free_square_id = 500000 * rep_id + balans_id;
                    var newImages = new List<byte[]>();

                    npp++;
                    log("npp=" + npp + "; balans_id=" + balans_id);

                    var dfiles = GetDataTable("select * from [reports1nf_balans_bti_attachfiles] where [free_square_id] = " + free_square_id);
                    foreach(DataRow dfilerow in dfiles.Rows)
                    {
                        var dfile = dfilerow["file_name"].ToString() + dfilerow["file_ext"].ToString();
                        dfile = Path.Combine(bti_attachfiles, free_square_id.ToString(), dfile);
                        if (File.Exists(dfile))
                        {
                            dfile = dfile;
						    System.Diagnostics.Debug.WriteLine("dfile=" + dfile);

                            var uploadedFileBytes = File.ReadAllBytes(dfile);
                            var jpeginfo = PhotoUtils.ProcImage(uploadedFileBytes);
                            if (jpeginfo.IsImage)
                            {
                                newImages.Add(jpeginfo.Jpegbytes);
                            }
                            else
                            {
                                var pdfinfo = PhotoUtils.ProcPdf("", uploadedFileBytes);
                                if (pdfinfo.IsPdf)
                                {
                                    for (int i = 0; i < pdfinfo.ListFileNames.Count; i++)
                                    {
                                        newImages.Add(pdfinfo.ListJpegbytes[i]);
                                    }
                                }
                            }
                        }
                    }

                    if (newImages.Any())
                    {
                        log("npp=" + npp + "; balans_id=" + balans_id + "; newImages.Count=" + newImages.Count);

                        newImages = newImages;

                        var ndir = Path.Combine(dir_2NF, balans_id.ToString());

                        for (int i = 0; i < newImages.Count; i++)
                        {
                            var newImage = newImages[i];
                            var file_name = "001_" + (i + 1).ToString("0000");
                            var newId = -1;
                            var sql = @"insert into reports1nf_btiphoto(bal_id, file_name, file_ext, user_id, create_date) 
                                        values(@bal_id, @file_name, @file_ext, @user_id, @create_date); 
                                        SELECT SCOPE_IDENTITY()";
                            using (var cmd = new SqlCommand(sql, connection, transaction))
                            {
                                cmd.Parameters.Add(GetSqlParameter("bal_id", balans_id));
                                cmd.Parameters.Add(GetSqlParameter("file_name", file_name));
                                cmd.Parameters.Add(GetSqlParameter("file_ext", ".jpg"));
                                cmd.Parameters.Add(GetSqlParameter("user_id", "11111111-1111-1111-1111-111111111111"));
                                cmd.Parameters.Add(GetSqlParameter("create_date", new DateTime(2023,1,1)));
                                newId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            if (!Directory.Exists(ndir))
                            {
                                Directory.CreateDirectory(ndir);
                            }

                            if (newId <= 0) throw new Exception();
                            //var nfile_name = Path.Combine(ndir, file_name) + ".jpg";
                            var nfile_name = Path.Combine(ndir, "" + newId + ".jpg");
                            File.WriteAllBytes(nfile_name, newImage);
                        }

                    }
                }

                log("before commit");
                transaction.Commit();
                log("after commit");
            }
        }
        catch(Exception ex)
        {
            log("Exception:" + ex.ToString());
        }
    }


    private DataTable GetDataTable(string sql)
    {
        var factory = DbProviderFactories.GetFactory(connection);
        var dataTable = new DataTable();
        using (var cmd = factory.CreateCommand())
        {
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Transaction = transaction;
            using (var adapter = factory.CreateDataAdapter())
            {
                adapter.SelectCommand = cmd;
                adapter.Fill(dataTable);
            }
        }

        return dataTable;
    }

    private static SqlParameter GetSqlParameter(string parameterName, object value)
    {
        if (value == null)
        {
            return new SqlParameter(parameterName, DBNull.Value);
        }
        else
        {
            return new SqlParameter(parameterName, value);
        }
    }

    private void log(string text)
    {
        File.AppendAllText(@"c:\tmp\____move.log", "" + DateTime.Now + "\t\t" + (text ?? "") + "\n");
    }
}