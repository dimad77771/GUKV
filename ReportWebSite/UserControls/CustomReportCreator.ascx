<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CustomReportCreator.ascx.cs" Inherits="UserControls_CustomReportCreator" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    var updateReportsTreeAfterGridCallback = false;

    function ButtonSaveReportClick(s, e) {

        // Check if report name is entered
        var reportName = "" + EditReportName.GetText();

        if (reportName == "") {

            alert("Будь ласка, введіть назву звіту");
        }
        else {

            // Check if folder is selected
            var folderId = ComboFolders.GetValue();

            if (folderId == null || folderId == undefined) {

                alert("Будь ласка, виберіть папку для збереження звіту");
            }
            else {

                // Check if report name is already used
                var do_continue = true;

                if (ReportTreeNodeExists(reportName)) {
                    do_continue = confirm("Звіт або папка з таким ім'ям вже існує. Перезаписати його?");
                }

                if (do_continue) {

                    // Get the active tab of the tab control
                    var tabControl;
                    var activeTab;

                    tabControl = MainPageControl;

                    if (tabControl != null && tabControl != undefined) {

                        activeTab = tabControl.GetActiveTab();

                        if (activeTab != null && activeTab != undefined) {

                            // Format the callback string. It must include ID of the folder, and report name
                            var param = "addreport:" + folderId + ":" + reportName;

                            // Determine the destination Grid by the name of the active tab
                            var activeTabName = "" + activeTab.name;

                            if (activeTabName == "ObjectsMainGrid") {
                                GridViewBuildings.PerformCallback(param);
                            }
                            else if (activeTabName == "ObjectTransferGrid") {
                                GridViewTransfer.PerformCallback(param);
                            }
                            else if (activeTabName == "BalansMainGrid") {
                                GridViewBalans.PerformCallback(param);
                            }
                            else if (activeTabName == "BalansObjects") {
                                GridViewBalansObjects.PerformCallback(param);
                            }
                            else if (activeTabName == "BalansArenda") {
                                GridViewBalansArenda.PerformCallback(param);
                            }
                            else if (activeTabName == "ArendaMainGrid") {
                                GridViewArenda.PerformCallback(param);
                            }
                            else if (activeTabName == "RentedObjects") {
                                GridViewArendaObjects.PerformCallback(param);
                            }
                            else if (activeTabName == "PrivativationDocGrid") {
                                GridViewPrivatization.PerformCallback(param);
                            }
                            else if (activeTabName == "PrivativationObjGrid") {
                                GridViewPrivObjects.PerformCallback(param);
                            }
                            else if (activeTabName == "Documents") {
                                GridViewDocuments.PerformCallback(param);
                            }
                            else if (activeTabName == "DocumentObjGrid") {
                                GridViewDocObjects.PerformCallback(param);
                            }
                            else if (activeTabName == "FinanceReport") {
                                GridViewFinance.PerformCallback(param);
                            }
                            else if (activeTabName == "CatalogueBuildingsGrid") {
                                GridViewAllBuildings.PerformCallback(param);
                            }
                            else if (activeTabName == "CatalogueOrganizationsGrid") {
                                GridViewAllOrganizations.PerformCallback(param);
                            }
                            else if (activeTabName == "RentByBalansOrg") {
                                GridViewRentByBalansOrg.PerformCallback(param);
                            }
                            else if (activeTabName == "RentByRenters") {
                                GridViewRentByRenters.PerformCallback(param);
                            }
                            else if (activeTabName == "RentByBuildings") {
                                GridViewRentByBuildings.PerformCallback(param);
                            }
                            else if (activeTabName == "RentTotals") {
                                GridViewRentTotals.PerformCallback(param);
                            }
                            else if (activeTabName == "RentNotSubmitted") {
                                GridViewRentNotSubmitted.PerformCallback(param);
                            }
                            else if (activeTabName == "AssessmentObjects") {
                                GridViewAssessmentObjects.PerformCallback(param);
                            }

                            // We need to update the tree of user-defined reports after callback finishes
                            updateReportsTreeAfterGridCallback = true;
                        }
                    }
                }
            }
        }
    }

    // ]]>

</script>

<asp:SqlDataSource ID="SqlDataSourceUserFolders" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, folder_name FROM user_folders WHERE usr_id = @uid" 
    OnSelecting="SqlDataSourceUserFolders_Selecting">
    <SelectParameters>
        <asp:Parameter Name="uid" />
    </SelectParameters>
</asp:SqlDataSource>

<dx:ASPxRoundPanel ID="PanelReportCreation" runat="server" HeaderText="Щоб зберегти налаштування звіту, введіть назву для звіту, оберіть папку для збереження звіту та натисніть 'Зберегти звіт'">
    <PanelCollection>
        <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">

            <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="EditReportName" ClientInstanceName="EditReportName" runat="server" Width="585"></dx:ASPxTextBox>
                    </td>
                    <td width="16px"></td>
                    <td>
                        <dx:ASPxButton ID="ButtonSaveReport" ClientInstanceName="ButtonSaveReport" runat="server"
                                Text="Зберегти звіт" AutoPostBack="false">
                            <ClientSideEvents Click="ButtonSaveReportClick" />
                        </dx:ASPxButton>
                    </td>
                    <td width="16px"></td>
                    <td>
                        <dx:ASPxLabel ID="Label1" runat="server" Text="у папку"></dx:ASPxLabel>
                    </td>
                    <td width="16px"></td>
                    <td>

                        <dx:ASPxCallbackPanel ID="CallbackPanelUserFolders" ClientInstanceName="CallbackPanelUserFolders"
                            runat="server" OnCallback="CallbackPanelUserFolders_Callback">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">

                                    <dx:ASPxComboBox ID="ComboFolders" ClientInstanceName="ComboFolders" runat="server"
                                        Width="160px"
                                        SelectedIndex="0"
                                        DataSourceID="SqlDataSourceUserFolders"
                                        TextField="folder_name"
                                        ValueField="id"
                                        ValueType="System.Int32" >
                                    </dx:ASPxComboBox>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>
                                
                    </td>
                </tr>
            </table>

        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>
