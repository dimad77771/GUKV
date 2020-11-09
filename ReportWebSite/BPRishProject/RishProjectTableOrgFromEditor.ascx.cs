using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;
using FirebirdSql.Data.FirebirdClient;

public partial class RishProjectTableOrgFromEditor : System.Web.UI.UserControl
{
    private class EditorState : RishProjectDataSource.OrganizationDesc.IEditorState
    {
        private readonly RishProjectDataSource.OrganizationDesc owner;

        public int OrganizationID { get; set; }

        public EditorState(RishProjectDataSource.OrganizationDesc owner)
        {
            this.owner = owner;

            this.OrganizationID = owner.OrganizationID ?? 0;
        }

        #region IEditorState Members

        public void Commit()
        {
            if (this.OrganizationID == 0)
                owner.OrganizationID = null;
            else
                owner.OrganizationID = this.OrganizationID;
        }

        #endregion

        public string GetDisplayText()
        {
            return RishProjectDataSourceItem.FormatOrganizationInfo(OrganizationID);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DataBind();
    }

    public override void DataBind()
    {
        RishProjectDataSource.OrganizationDesc datasource = DataSource;
        if (datasource != null && !IsInitialized)
        {
            if (datasource.OrganizationID != null && datasource.OrganizationID.Value != 0)
            {
                DB.organizationsRow orgRow = (new DBTableAdapters.organizationsTableAdapter())
                    .GetDataByID(datasource.OrganizationID.Value).FirstOrDefault();

                if (orgRow != null)
                {
                    if (!orgRow.Iszkpo_codeNull() && orgRow.zkpo_code.Length > 0)
                    {
                        OrgSearchZkpoPattern = orgRow.zkpo_code;
                    }
                    else if (!orgRow.Isfull_nameNull() && orgRow.full_name.Length > 0)
                    {
                        OrgSearchNamePattern = orgRow.full_name;
                    }

                    ComboOrgFrom.DataBind();
                    ComboOrgFrom.SelectedIndex = ComboOrgFrom.Items.IndexOfValue(datasource.OrganizationID.Value.ToString());
                }
            }

            IsInitialized = true;
        }
    }

    public void UpdateEditorState(int organizationID)
    {
        RishProjectDataSource.OrganizationDesc datasource = DataSource;
        if (datasource != null)
        {
            ((EditorState)datasource.EditorState).OrganizationID = organizationID;
        }
    }

    public RishProjectDataSource.OrganizationDesc DataSource
    {
        get
        {
            return Page.GetEditedCustomData<RishProjectDataSource.OrganizationDesc>("RishProjectTableOrgFromEditor_DataSource");
        }
        set
        {
            if (value != null)
                value.EditorState = new EditorState(value);

            Page.SetEditedCustomData("RishProjectTableOrgFromEditor_DataSource", value);

            IsInitialized = false;
            DataBind();
        }
    }

    private bool IsInitialized
    {
        get { return Page.GetEditorFormInitialized("RishProjectTableOrgFromEditor_Initialized"); }
        set { Page.SetEditorFormInitialized(value, "RishProjectTableOrgFromEditor_Initialized"); }
    }

    protected string OrgSearchZkpoPattern
    {
        set
        {
            Page.SetEditedCustomData("OrgFromSearchZkpoPattern", value);
        }

        get
        {
            return Page.GetEditedCustomData<string>("OrgFromSearchZkpoPattern") ?? string.Empty;
        }
    }

    protected string OrgSearchNamePattern
    {
        set
        {
            Page.SetEditedCustomData("OrgFromSearchNamePattern", value);
        }

        get
        {
            return Page.GetEditedCustomData<string>("OrgFromSearchNamePattern") ?? string.Empty;
        }
    }

    protected void ComboOrgSearch_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] parts = e.Parameter.Split(new char[] { '|' });

        if (parts.Length == 2)
        {
            OrgSearchZkpoPattern = parts[0].Trim();
            OrgSearchNamePattern = parts[1].Trim().ToUpper();

            (sender as ASPxComboBox).DataBind();
        }
    }

    protected void SqlDataSourceOrgSearch_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string name = OrgSearchNamePattern;
        string zkpo = OrgSearchZkpoPattern;

        if (name.Length == 0 && zkpo.Length == 0)
        {
            e.Command.Parameters["@zkpo"].Value = "^";
            e.Command.Parameters["@fname"].Value = "^";
        }
        else
        {
            e.Command.Parameters["@zkpo"].Value = zkpo.Length > 0 ? "%" + zkpo + "%" : "%";
            e.Command.Parameters["@fname"].Value = name.Length > 0 ? "%" + name + "%" : "%";
        }
    }

    protected void CPOrgFromEditor_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("create:"))
        {
            string zkpo = TextBoxZkpoCodeFrom.Text;
            string fullName = TextBoxFullNameFrom.Text;

            //FbConnection connection = Utils.ConnectTo1NF();

            //if (connection != null)
            //{
                string errorMessage = "";

                int newOrdId = RishProjectExport.CreateNew1NFOrganization(
                    //connection,
                    fullName,
                    TextBoxShortNameFrom.Text,
                    zkpo,
                    ComboBoxIndustryFrom.Value is int ? (int)ComboBoxIndustryFrom.Value : -1,
                    ComboBoxOccupationFrom.Value is int ? (int)ComboBoxOccupationFrom.Value : -1,
                    ComboBoxFormVlasnFrom.Value is int ? (int)ComboBoxFormVlasnFrom.Value : -1,
                    ComboBoxStatusFrom.Value is int ? (int)ComboBoxStatusFrom.Value : -1,
                    ComboBoxFormGospFrom.Value is int ? (int)ComboBoxFormGospFrom.Value : -1,
                    -1,
                    ComboBoxVedomstvoFrom.Value is int ? (int)ComboBoxVedomstvoFrom.Value : -1,
                    TextBoxDirectorFioFrom.Text,
                    TextBoxDirectorPhoneFrom.Text,
                    "", //email
                    TextBoxBuhgalterFioFrom.Text,
                    TextBoxBuhgalterPhoneFrom.Text,
                    "", //fax
                    TextBoxKvedCodeFrom.Text,
                    ComboBoxDistrictFrom.Value is int ? (int)ComboBoxDistrictFrom.Value : -1,
                    ComboBoxStreetNameFrom.Text,
                    TextBoxAddrNomerFrom.Text,
                    TextBoxAddrKorpusFrom.Text,
                    TextBoxAddrZipCodeFrom.Text,
                    //true,
                    out errorMessage);

                //connection.Close();

                if (newOrdId > 0)
                {
                    EditOrgFromZKPO.Text = zkpo;
                    EditOrgFromName.Text = fullName;

                    OrgSearchNamePattern = "";
                    OrgSearchZkpoPattern = zkpo;

                    ComboOrgFrom.DataBind();
                    ComboOrgFrom.SelectedItem = ComboOrgFrom.Items.FindByValue(newOrdId);
                }

                LabelOrgFromCreationError.Text = errorMessage;
                LabelOrgFromCreationError.ClientVisible = (errorMessage.Length > 0);
            //}
        }
    }
}
