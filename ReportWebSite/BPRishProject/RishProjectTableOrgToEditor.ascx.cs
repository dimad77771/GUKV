using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;
using FirebirdSql.Data.FirebirdClient;

public partial class RishProjectTableOrgToEditor : System.Web.UI.UserControl
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

                    ComboOrgTo.DataBind();
                    ComboOrgTo.SelectedIndex = ComboOrgTo.Items.IndexOfValue(datasource.OrganizationID.Value.ToString());
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
            return Page.GetEditedCustomData<RishProjectDataSource.OrganizationDesc>("RishProjectTableOrgToEditor_DataSource");
        }
        set
        {
            if (value != null)
                value.EditorState = new EditorState(value);

            Page.SetEditedCustomData("RishProjectTableOrgToEditor_DataSource", value);

            IsInitialized = false;
            DataBind();
        }
    }

    private bool IsInitialized
    {
        get { return Page.GetEditorFormInitialized("RishProjectTableOrgToEditor_Initialized"); }
        set { Page.SetEditorFormInitialized(value, "RishProjectTableOrgToEditor_Initialized"); }
    }

    protected string OrgSearchZkpoPattern
    {
        set
        {
            Page.SetEditedCustomData("OrgToSearchZkpoPattern", value);
        }

        get
        {
            return Page.GetEditedCustomData<string>("OrgToSearchZkpoPattern") ?? string.Empty;
        }
    }

    protected string OrgSearchNamePattern
    {
        set
        {
            Page.SetEditedCustomData("OrgToSearchNamePattern", value);
        }

        get
        {
            return Page.GetEditedCustomData<string>("OrgToSearchNamePattern") ?? string.Empty;
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

    protected void CPOrgToEditor_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("create:"))
        {
            string zkpo = TextBoxZkpoCodeTo.Text;
            string fullName = TextBoxFullNameTo.Text;

            //FbConnection connection = Utils.ConnectTo1NF();

            //if (connection != null)
            //{
                string errorMessage = "";

                int newOrdId = RishProjectExport.CreateNew1NFOrganization(
                    //connection,
                    fullName,
                    TextBoxShortNameTo.Text,
                    zkpo,
                    ComboBoxIndustryTo.Value is int ? (int)ComboBoxIndustryTo.Value : -1,
                    ComboBoxOccupationTo.Value is int ? (int)ComboBoxOccupationTo.Value : -1,
					-1,
					-1,
                    ComboBoxFormVlasnTo.Value is int ? (int)ComboBoxFormVlasnTo.Value : -1,
                    ComboBoxStatusTo.Value is int ? (int)ComboBoxStatusTo.Value : -1,
                    ComboBoxFormGospTo.Value is int ? (int)ComboBoxFormGospTo.Value : -1,
                    -1,
                    ComboBoxVedomstvoTo.Value is int ? (int)ComboBoxVedomstvoTo.Value : -1,
                    TextBoxDirectorFioTo.Text,
                    TextBoxDirectorPhoneTo.Text,
                    "", //EMAIL
                    TextBoxBuhgalterFioTo.Text,
                    TextBoxBuhgalterPhoneTo.Text,
                    "", //fax
                    TextBoxKvedCodeTo.Text,
                    ComboBoxDistrictTo.Value is int ? (int)ComboBoxDistrictTo.Value : -1,
                    ComboBoxStreetNameTo.Text,
                    TextBoxAddrNomerTo.Text,
                    TextBoxAddrKorpusTo.Text,
                    TextBoxAddrZipCodeTo.Text,
                    //true,
                    out errorMessage);

                //connection.Close();

                if (newOrdId > 0)
                {
                    EditOrgToZKPO.Text = zkpo;
                    EditOrgToName.Text = fullName;

                    OrgSearchNamePattern = "";
                    OrgSearchZkpoPattern = zkpo;

                    ComboOrgTo.DataBind();
                    ComboOrgTo.SelectedItem = ComboOrgTo.Items.FindByValue(newOrdId);
                }

                LabelOrgToCreationError.Text = errorMessage;
                LabelOrgToCreationError.ClientVisible = (errorMessage.Length > 0);
             //}
        }
    }
}