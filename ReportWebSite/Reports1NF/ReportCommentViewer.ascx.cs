using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using System.Data.SqlClient;

public partial class Reports1NF_ReportCommentViewer : System.Web.UI.UserControl
{
    private int reportId = 0;

    private int organizationId = 0;
    private int balansId = 0;
    private int balansDeletedId = 0;
    private int rentAgreementId = 0;
    private int rentedObjectId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public int ReportId
    {
        get { return reportId; }
        set { reportId = value; }
    }

    public int OrganizationId
    {
        get { return organizationId; }
        set { organizationId = value; }
    }

    public int BalansId
    {
        get { return balansId; }
        set { balansId = value; }
    }

    public int BalansDeletedId
    {
        get { return balansDeletedId; }
        set { balansDeletedId = value; }
    }

    public int RentAgreementId
    {
        get { return rentAgreementId; }
        set { rentAgreementId = value; }
    }

    public int RentedObjectId
    {
        get { return rentedObjectId; }
        set { rentedObjectId = value; }
    }

    public void AddNumberOfCommentsToButton(DevExpress.Web.ASPxButton button)
    {
        if (button != null && !button.Text.Contains('('))
        {
            object view = SqlDataSourceExistingComments.Select(new DataSourceSelectArguments());

            if (view is DataView)
            {
                DataTable dt = (view as DataView).ToTable();
                int rowCount = dt.Rows.Count;

                if (rowCount > 0)
                {
                    button.Text += " (" + rowCount.ToString() + ")";
                    button.Checked = true;
                }
            }
        }
    }

    public void SetCommentTargetText(string target)
    {
        EditTarget.Text = target;
    }

    protected void SqlDataSourceExistingComments_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@rep_id"].Value = reportId;
        e.Command.Parameters["@org_id"].Value = organizationId;
        e.Command.Parameters["@bal_id"].Value = balansId;
        e.Command.Parameters["@bal_del_id"].Value = balansDeletedId;
        e.Command.Parameters["@rent_id"].Value = rentAgreementId;
        e.Command.Parameters["@rent_obj_id"].Value = rentedObjectId;
    }

    protected void CPCommentPanel_Callback(object sender, CallbackEventArgsBase e)
    {
        // Get the commented control ID and Title
        int dividerPos = e.Parameter.IndexOf(';');
        string controlId = "";
        string controlTitle = "";

        if (dividerPos > 0)
        {
            controlId = e.Parameter.Substring(0, dividerPos);
            controlTitle = e.Parameter.Substring(dividerPos + 1);
        }

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            Reports1NFUtils.AddComment(connection, ReportId, MemoComment.Text.Trim(),
                organizationId, balansId, balansDeletedId, rentAgreementId, rentedObjectId,
                controlId, EditTarget.Text, CheckWrongData.Checked, true);

            connection.Close();

            MemoComment.Text = "";
            EditTarget.Text = "";
            CheckWrongData.Checked = false;
        }

        GridViewComments.DataBind();
    }
}
