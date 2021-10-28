using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web;
using System.Web.SessionState;
using DevExpress.Web;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using log4net;

public partial class ObjectPicker : UserControl
{
    public bool CreationOfObjectIsAllowed = false;

    public bool OnlyOurBalansObjects = false;

    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");

    private class EditorState : RishProjectDataSource.ObjectOrAddressDesc.IEditorState
    {
        private readonly RishProjectDataSource.ObjectOrAddressDesc owner;

        public int AddressStreetID { get; set; }
        public int AddressBuildingID { get; set; }
        public int BalansID { get; set; }
        public string ArbitraryText { get; set; }

        public EditorState(RishProjectDataSource.ObjectOrAddressDesc owner)
        {
            this.owner = owner;

            this.AddressBuildingID = owner.BuildingID ?? 0;
            this.BalansID = owner.BalansID ?? 0;
            this.ArbitraryText = owner.ArbitraryText;
        }

        #region IEditorState Members

        public void Commit()
        {
            if (this.AddressBuildingID == 0)
                owner.BuildingID = null;
            else
                owner.BuildingID = this.AddressBuildingID;

            if (this.BalansID == 0)
                owner.BalansID = null;
            else
                owner.BalansID = this.BalansID;

            owner.ArbitraryText = this.ArbitraryText;
        }

        #endregion

        public string GetDisplayText()
        {
            return RishProjectDataSourceItem.FormatObjectOrAddressInfo(AddressBuildingID, BalansID, ArbitraryText);
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (OnlyOurBalansObjects)
        {
            GridViewBalansObjects.DataSourceID = "";
        }

        if (!CreationOfObjectIsAllowed)
        {
            //ButtonAddBalansObj.Visible = false;
            //ButtonAddBuilding.Visible = false;
        }
        DataBind();
    }

    public override void DataBind()
    {
        RishProjectDataSource.ObjectOrAddressDesc datasource = DataSource;

        if (datasource != null && !IsInitialized)
        {
            if (BalansID != 0)
            {
                // This initializes:
                // - data grid with balans objects
                // - drop-down with street buildings
                // - drop-down with street names
                //   (and the correspoding AddressStreetID is determined)
                DataView data = (DataView)SqlDataSourceBalansByID.Select(DataSourceSelectArguments.Empty);
                DataRowView row = data.Cast<DataRowView>().FirstOrDefault();
                if (row != null)
                {
                    AddressBuildingID = (int)row["building_id"];
                    AddressStreetID = (int)row["addr_street_id"];
                }
            }
            else if (AddressBuildingID != 0)
            {
                // This initializes:
                // - drop-down with street buildings
                // - drop-down with street names
                //   (and the correspoding AddressStreetID is determined)

                DataView data = (DataView)SqlDataSourceAddressByBuilding.Select(DataSourceSelectArguments.Empty);
                AddressStreetID = data.Cast<DataRowView>().Select(x => (int)x["addr_street_id"]).FirstOrDefault();
            }

            if (BalansID != 0)
            {
                GridViewBalansObjects.DataBind();
            }
            if (AddressStreetID != 0)
            {
                ComboBalansStreet.DataBind();
                ComboBalansStreet.SelectedIndex = ComboBalansStreet.Items.IndexOfValue(AddressStreetID);
            }
            if (AddressBuildingID != 0)
            {
                ComboBalansBuilding.DataBind();
                ComboBalansBuilding.SelectedIndex = ComboBalansBuilding.Items.IndexOfValue(AddressBuildingID);
            }

            //txtArbitraryText.Text = datasource.ArbitraryText;

            IsInitialized = true;
        }
    }

    protected void SqlDataSourceAddressByBuilding_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@buildingID"].Value = AddressBuildingID;
    }

    protected void SqlDataSourceBalansByID_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@balansID"].Value = BalansID;
    }

    public RishProjectDataSource.ObjectOrAddressDesc DataSource
    {
        get
        {
            return Page.GetEditedCustomData<RishProjectDataSource.ObjectOrAddressDesc>("RishProjectTableObjectEditor_DataSource");
        }
        set
        {
            if (value != null)
                value.EditorState = new EditorState(value);

            Page.SetEditedCustomData("RishProjectTableObjectEditor_DataSource", value);
            
            IsInitialized = false;
            DataBind();
        }
    }

    private bool IsInitialized
    {
        get { return Page.GetEditorFormInitialized("RishProjectTableObjectEditor_Initialized"); }
        set { Page.SetEditorFormInitialized(value, "RishProjectTableObjectEditor_Initialized"); }
    }

    private int AddressStreetID
    {
        get
        {
            RishProjectDataSource.ObjectOrAddressDesc datasource = DataSource;

            if (datasource != null)
            {
                return ((EditorState)datasource.EditorState).AddressStreetID;
            }
            else
            {
                return Page.GetEditedCustomData<int>("RishProjectTableObjectEditor_AddressStreetID");
            }
        }
        set
        {
            RishProjectDataSource.ObjectOrAddressDesc datasource = DataSource;

            if (datasource != null)
            {
                ((EditorState)datasource.EditorState).AddressStreetID = value;
            }
            else
            {
                Page.SetEditedCustomData("RishProjectTableObjectEditor_AddressStreetID", value);
            }
        }
    }

    protected int AddressBuildingID
    {
        get
        {
            RishProjectDataSource.ObjectOrAddressDesc datasource = DataSource;

            if (datasource != null)
            {
                return ((EditorState)datasource.EditorState).AddressBuildingID;
            }
            else
            {
                return Page.GetEditedCustomData<int>("RishProjectTableObjectEditor_AddressBuildingID");
            }
        }
        set
        {
            RishProjectDataSource.ObjectOrAddressDesc datasource = DataSource;

            if (datasource != null)
            {
                ((EditorState)datasource.EditorState).AddressBuildingID = value;
            }
            else
            {
                Page.SetEditedCustomData("RishProjectTableObjectEditor_AddressBuildingID", value);
            }
        }
    }

    protected int BalansID
    {
        get
        {
            RishProjectDataSource.ObjectOrAddressDesc datasource = DataSource;

            if (datasource != null)
            {
                return ((EditorState)datasource.EditorState).BalansID;
            }
            else
            {
                return Page.GetEditedCustomData<int>("RishProjectTableObjectEditor_BalansID");
            }
        }
        set
        {
            RishProjectDataSource.ObjectOrAddressDesc datasource = DataSource;

            if (datasource != null)
            {
                ((EditorState)datasource.EditorState).BalansID = value;
            }
            else
            {
                Page.SetEditedCustomData("RishProjectTableObjectEditor_BalansID", value);
            }
        }
    }

    protected string OrgSearchZkpoPattern
    {
        set
        {
            Page.SetEditedCustomData("OrgSearchZkpoPattern", value);
        }

        get
        {
            return Page.GetEditedCustomData<string>("OrgSearchZkpoPattern") ?? string.Empty;
        }
    }

    protected string OrgSearchNamePattern
    {
        set
        {
            Page.SetEditedCustomData("OrgSearchNamePattern", value);
        }

        get
        {
            return Page.GetEditedCustomData<string>("OrgSearchNamePattern") ?? string.Empty;
        }
    }

    protected void ComboAddressBuilding_Callback(object source, CallbackEventArgsBase e)
    {
        try
        {
            AddressBuildingID = 0;

            int streetId = int.Parse(e.Parameter);

            AddressStreetID = streetId;

            (source as ASPxComboBox).DataBind();
        }
        finally
        {
        }
    }

    protected void SqlDataSourceDictBuildings_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@street_id"].Value = AddressStreetID;
    }

    protected void SqlDataSourceBalansSearch_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@bid"].Value = AddressBuildingID;
    }

    protected void GridViewBalansObjects_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        try
        {
            AddressBuildingID = int.Parse(e.Parameters);

            GridViewBalansObjects.DataBind();
        }
        finally
        {
        }
    }

    public void UpdateEditorState(int streetID, int buildingID, int balansID, string arbitraryText)
    {
        RishProjectDataSource.ObjectOrAddressDesc datasource = DataSource;
        if (datasource == null)
            return;
        ((EditorState)datasource.EditorState).AddressStreetID = streetID;
        ((EditorState)datasource.EditorState).AddressBuildingID = buildingID;
        ((EditorState)datasource.EditorState).BalansID = balansID;
        ((EditorState)datasource.EditorState).ArbitraryText = arbitraryText;
    }

    protected void GridViewBalansObjects_DataBound(object sender, EventArgs e)
    {
        
        bool validBalansID = false;

        if (BalansID != 0)
        {
            if (GridViewBalansObjects.MakeRowVisible(BalansID))
            {
                int visibleIndex = GridViewBalansObjects.FindVisibleIndexByKeyValue(BalansID);
                if (visibleIndex >= 0)
                {
                    GridViewBalansObjects.FocusedRowIndex = visibleIndex;
                    validBalansID = true;
                }
            }
        }

        if (!validBalansID)
        {
            GridViewBalansObjects.FocusedRowIndex = -1;

            if (BalansID != 0)
                BalansID = 0;
        }
    }

    protected void SqlDataSourceOrgSearch2_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
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

    protected void ComboBalansOrg_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] parts = e.Parameter.Split(new char[] { '|' });

        if (parts.Length == 2)
        {
            OrgSearchZkpoPattern = parts[0].Trim();
            OrgSearchNamePattern = parts[1].Trim().ToUpper();

            (sender as ASPxComboBox).DataBind();
        }
    }
}
