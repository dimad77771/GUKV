using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using System.Data;
using System.IO;
using DevExpress.Web;

public partial class RishProjectItemEditor : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RishProjectDataSourceItem item = Page.GetEditedTreeItem<RishProjectDataSourceItem>();
        if (item == null)
            return;

        HtmlEditorCssFile cssFile = new HtmlEditorCssFile("~/CSS/ASPxHtmlEditor.css");
        MemoPunktOutro.CssFiles.Add(cssFile);
        MemoPunktExplanation.CssFiles.Add(cssFile);

        if (!this.IsInitialized)
        {
            if (!item.isTable)
            {
                // Initialize standard item editing controls
                // MemoPunktIntro.Html = item.introText;
                MemoPunktOutro.Html = item.outroText.Length > 0 ? item.outroText : "<p style=\"font-family: Times New Roman; font-size: 14pt;\">&nbsp;</p>";
                MemoPunktExplanation.Html = item.explanation.Length > 0 ? item.explanation : "<p style=\"font-family: Times New Roman; font-size: 14pt;\">&nbsp;</p>";

                bool enableOrgFrom = false, enableOrgTo = false;
                if (item.rightId > 0)
                {
                    ComboRight.DataBind();
                    ComboRight.SelectedIndex = ComboRight.Items.IndexOfValue(item.rightId);

                    DB.dict_obj_rightsRow rightsRow = item.DataSource.Context.dict_obj_rights
                        .FirstOrDefault(x => x.id == item.rightId);
                    if (rightsRow != null)
                    {
                        enableOrgFrom = rightsRow.enable_org_from;
                        enableOrgTo = rightsRow.enable_org_to;
                    }
                }
                ButtonSelectOrgFrom.Enabled =
                    ButtonClearOrgFrom.Enabled =
                    EditOrgFromText.Enabled = enableOrgFrom;
                ButtonSelectOrgTo.Enabled =
                    ButtonClearOrgTo.Enabled =
                    EditOrgToText.Enabled = enableOrgTo;

                CPItemProperties_SetOrgFromID(item.organizationFromId, string.Empty);
                CPItemProperties_SetOrgToID(item.organizationToId, string.Empty);
            }

            this.IsInitialized = true;
        }

        // Utils.HideUnnecessaryHtmlEditorButtons(MemoPunktIntro, true);
        Utils.HideUnnecessaryHtmlEditorButtons(MemoPunktOutro, true);
        Utils.HideUnnecessaryHtmlEditorButtons(MemoPunktExplanation, true);

        // Add required pop-up commands to the primary item text editor
        MemoPunktOutro.ContextMenuItems.Clear();
        MemoPunktOutro.ContextMenuItems.Add(Resources.Strings.HtmlEditorInsertOrgFrom, "InsertOrgFromLink");
        MemoPunktOutro.ContextMenuItems.Add(Resources.Strings.HtmlEditorInsertOrgTo, "InsertOrgToLink");
    }

    private bool IsInitialized
    {
        get { return Page.GetEditorFormInitialized(); }
        set { Page.SetEditorFormInitialized(value); }
    }

    private class CPItemCallbackData
    {
        public int BuildingID { get; set; }
        public int BalansObjID { get; set; }
        public int OrgFromID { get; set; }
        public int OrgToID { get; set; }
        public string ArbitraryText { get; set; }

        public string DataIdentifier { get; set; }
    }

    protected void CPItemProperties_Callback(object sender, CallbackEventArgsBase e)
    {
        if (string.IsNullOrEmpty(e.Parameter))
            return;

        CPItemCallbackData data = e.Parameter.FromJSON<CPItemCallbackData>();

        CPItemProperties_SetOrgFromID(data.OrgFromID, string.Empty);
        CPItemProperties_SetOrgToID(data.OrgToID, string.Empty);

        RishProjectDataSourceItem item = Page.GetEditedTreeItem<RishProjectDataSourceItem>();
        if (ComboRight.SelectedItem != null && item != null)
        {
            DB.dict_obj_rightsRow rightsRow = item.DataSource.Context.dict_obj_rights
                .FirstOrDefault(x => x.id == (int)ComboRight.SelectedItem.Value);
            if (rightsRow != null)
            {
                ButtonSelectOrgFrom.Enabled =
                    ButtonClearOrgFrom.Enabled =
                    EditOrgFromText.Enabled = rightsRow.enable_org_from;

                //if (!rightsRow.enable_org_from)
                //{
                //    EditOrgFromText.Text = "";
                //    HiddenOrgFromID.Value = "";
                //    CPItemProperties_Callback(CPItemProperties, new CallbackEventArgsBase(@"{""OrgFromID"":-1}\"));
                //}

                ButtonSelectOrgTo.Enabled =
                    ButtonClearOrgTo.Enabled =
                    EditOrgToText.Enabled = rightsRow.enable_org_to;

                //if (!rightsRow.enable_org_to)
                //{
                //    EditOrgToText.Text = "";
                //    HiddenOrgToID.Value = "";
                //    CPItemProperties_Callback(CPItemProperties, new CallbackEventArgsBase(@"{""OrgToID"":-1}\"));
                //}
            }
        }

        // Update the text of links
        string html = MemoPunktOutro.Html;

        Utils.ProcessHyperlinks(ref html, OnUpdateHyperlink, data);

        MemoPunktOutro.Html = html;
    }

    private void CPItemProperties_SetOrgFromID(int orgFromId, string orgFromName)
    {
        if (orgFromId <= 0)
            return;

        HiddenOrgFromID.Value = orgFromId.ToString();
        EditOrgFromText.Text = string.IsNullOrWhiteSpace(orgFromName)
            ? RetrieveDisplayText(SqlDataSourceOrganizationText, "org_id", orgFromId)
            : orgFromName;
    }

    private void CPItemProperties_SetOrgToID(int orgToId, string orgToName)
    {
        if (orgToId <= 0)
            return;

        HiddenOrgToID.Value = orgToId.ToString();
        EditOrgToText.Text = string.IsNullOrWhiteSpace(orgToName)
            ? RetrieveDisplayText(SqlDataSourceOrganizationText, "org_id", orgToId)
            : orgToName;
    }

    #region Saving of modified data

    public void SaveChanges()
    {
        RishProjectDataSourceItem item = Page.GetEditedTreeItem<RishProjectDataSourceItem>();
        if (item == null)
            return;

        // item.introText = MemoPunktIntro.Html;
        item.outroText = MemoPunktOutro.Html;
        item.explanation = MemoPunktExplanation.Html;

        if (HiddenOrgFromID.Value.Length > 0)
            item.organizationFromId = Convert.ToInt32(HiddenOrgFromID.Value);
        else
            item.organizationFromId = 0;

        if (HiddenOrgToID.Value.Length > 0)
            item.organizationToId = Convert.ToInt32(HiddenOrgToID.Value);
        else
            item.organizationToId = 0;

        if (ComboRight.Value != null && ComboRight.Value.ToString().All(char.IsDigit))
            item.rightId = Convert.ToInt32(ComboRight.Value);
        else
        {
            item.rightId = 0;
            item.organizationFromId = 0;
            item.organizationToId = 0;
        }
    }

    #endregion (Saving of modified data)

    #region Utility methods

    private string RetrieveDisplayText(SqlDataSource datasource, string parameterName, object parameterValue, 
        string columnName = null)
    {
        datasource.SelectParameters[parameterName].DefaultValue = 
            (parameterValue == null ? null : parameterValue.ToString());
        DataView queryResult = datasource.Select(new DataSourceSelectArguments()) as DataView;
        if (queryResult == null)
            return string.Empty;
        DataTable table = queryResult.ToTable();
        if (table.Rows.Count == 0)
            return string.Empty;

        object value;
        if (columnName != null)
            value = table.Rows[0][columnName];
        else
            value = table.Rows[0][0];
        return value is DBNull ? string.Empty : value.ToString();
    }

    public bool OnUpdateHyperlink(ref string openingTag, ref string innerText, ref string closingTag, object parameter)
    {
        bool canUpdateAddress = false;
        bool canUpdateOrgFrom = false;
        bool canUpdateOrgTo = false;

        bool deleteAddress = false;
        bool deleteOrgFrom = false;
        bool deleteOrgTo = false;

        if (parameter is CPItemCallbackData)
        {
            CPItemCallbackData data = parameter as CPItemCallbackData;

            canUpdateAddress = data.BuildingID > 0 || data.BalansObjID > 0 || !string.IsNullOrEmpty(data.ArbitraryText);
            canUpdateOrgFrom = data.OrgFromID > 0;
            canUpdateOrgTo = data.OrgToID > 0;

            deleteAddress = data.BalansObjID == -1;
            deleteOrgFrom = data.OrgFromID == -1;
            deleteOrgTo = data.OrgToID == -1;
        }

        // Check the value of 'rel' attribute
        string relValue = Utils.GetRelAttributeFromHyperlink(openingTag);

        if (relValue == "gukv_org_from")
        {
            if (deleteOrgFrom)
                return false;

            if (canUpdateOrgFrom)
                innerText = EditOrgFromText.Text.ToLower();
        }
        else if (relValue == "gukv_org_to")
        {
            if (deleteOrgTo)
                return false;

            if (canUpdateOrgTo)
                innerText = EditOrgToText.Text.ToLower();
        }

        return true;
    }

    #endregion (Utility methods)
}