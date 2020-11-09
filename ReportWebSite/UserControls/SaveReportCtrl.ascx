<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SaveReportCtrl.ascx.cs" Inherits="UserControls_SaveReportCtrl" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function StringTrim(s) {

        var str = "" + s;

        while (str.length > 0 && str.charAt(0) == " ") {
            str = str.substring(1);
        }

        while (str.length > 0 && str.charAt(str.length - 1) == " ") {
            str = str.substring(0, str.length - 1);
        }

        return str;
    }

    function ReportNameExists(reportName) {

        reportName = "" + StringTrim(reportName);
        reportName = reportName.toUpperCase();

        for (var i = 0; i < TreeViewUserFolders.cpReportNameCount; i++) {

            var existingReportName = "" + TreeViewUserFolders.cpReportNames[i];

            existingReportName = "" + StringTrim(existingReportName);
            existingReportName = existingReportName.toUpperCase();

            if (reportName == existingReportName) {
                return true;
            }
        }

        return false;
    }

    function ButtonAddUserFolderClick(s, e) {

        var folderName = "" + EditNewFolderName.GetText();

        if (folderName == "") {
            alert("Будь ласка, введіть назву нової папки.");
        }
        else {
            var selNode = TreeViewUserFolders.GetSelectedNode();

            if (selNode != null && selNode != undefined) {

                // User can add folders only to the folders that have no parent
                var parentNode = selNode.parent;
                var nodeName = "" + selNode.name;

                if (parentNode == null || parentNode == undefined) {
                    CallbackPanelTreeViewUserFolders.PerformCallback("addfolder:" + nodeName + ":" + folderName);
                }
                else {
                    alert("Будь ласка, виберіть папку першого рівня для створення в ній нової папки.");
                }
            }
            else {
                alert("Будь ласка, виберіть папку першого рівня для створення в ній нової папки.");
            }
        }
    }

    function ButtonDeleteUserFolderClick(s, e) {

        var selNode = TreeViewUserFolders.GetSelectedNode();

        if (selNode != null && selNode != undefined) {

            // User can delete only folders that have a parent folder
            var parentNode = selNode.parent;
            var nodeName = "" + selNode.name;

            if (parentNode != null && parentNode != undefined) {

                // Ask user for confirmation
                if (confirm("Усі звіти, що збережені в цю папку, також будуть видалені. Продовжувати?")) {

                    CallbackPanelTreeViewUserFolders.PerformCallback("delfolder:" + nodeName);
                }
            }
            else {
                alert("Папки першого рівня не можуть бути видалені.");
            }
        }
        else {
            alert("Для видалення папки необхідно вибрати папку в дереві.");
        }
    }

    function ButtonSaveReportClick(s, e) {

        // Check if report name is entered
        var reportName = "" + EditNewReportName.GetText();

        if (reportName == "") {

            alert("Будь ласка, введіть назву звіту");
        }
        else {

            // Check if folder is selected
            var selNode = TreeViewUserFolders.GetSelectedNode();

            if (selNode == null || selNode == undefined) {

                alert("Будь ласка, виберіть папку для збереження звіту");
            }
            else {

                var nodeName = "" + selNode.name;

                // Check if report name is already used
                var do_continue = true;

                if (ReportNameExists(reportName)) {
                    do_continue = confirm("Звіт з таким ім'ям вже існує. Перезаписати його?");
                }

                if (do_continue) {

                    PopupControlFolders.Hide();

                    // Format the callback string. It must include ID of the folder (node name), and report name
                    var param = "addreport:" + nodeName + ":" + reportName;

                    PrimaryGridView.PerformCallback(param);

                    /*
                    // Get the active tab of the tab control
                    var tabControl;
                    var activeTab;

                    tabControl = MainPageControl;

                    if (tabControl != null && tabControl != undefined) {

                        activeTab = tabControl.GetActiveTab();

                        if (activeTab != null && activeTab != undefined) {

                            // Format the callback string. It must include ID of the folder (node name), and report name
                            var param = "addreport:" + nodeName + ":" + reportName;

                            // Determine the destination Grid by the name of the active tab
                            var activeTabName = "" + activeTab.name;

                            // Perform grid callback to save the grid layout
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
                            else if (activeTabName == "RentObjectsUse") {
                                GridViewRentObjectsUse.PerformCallback(param);
                            }

                            PopupControlFolders.Hide();
                        }
                    }
                    */
                }
            }
        }
    }

    // ]]>

</script>

<dx:ASPxRoundPanel ID="PanelFolders" runat="server" HeaderText="Папки користувача">
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">

            <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="EditNewFolderName" ClientInstanceName="EditNewFolderName" runat="server" Width="360px"> </dx:ASPxTextBox>
                    </td>
                    <td width="8px">&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="ButtonAddFolder" ClientInstanceName="ButtonAddFolder" runat="server" Width="130px"
                            Text="Створити папку" AutoPostBack="false">
                            <ClientSideEvents Click="ButtonAddUserFolderClick" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>

            <br/>

            <dx:ASPxCallbackPanel ID="CallbackPanelTreeViewUserFolders"
                ClientInstanceName="CallbackPanelTreeViewUserFolders"
                runat="server"
                OnCallback="CallbackPanelTreeViewUserFolders_Callback">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">

                        <dx:ASPxTreeView ID="TreeViewUserFolders"
                            ClientInstanceName="TreeViewUserFolders"
                            Width="500px"
                            runat="server"
                            AllowSelectNode="True"
                            AutoPostBack="False"
                            EnableClientSideAPI="True" 
                            OnCustomJSProperties="TreeViewUserFolders_CustomJSProperties" >

                            <NodeTextTemplate>
                                <div id="div_tree_node_<%# Container.Node.Name %>">
                                    <%# Container.Node.Text %>
                                </div>
                            </NodeTextTemplate>

                            <ClientSideEvents Init="function(s, e) { TreeViewUserFolders.ExpandAll(); }" />
                        </dx:ASPxTreeView>

                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>

            <br/>

            <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="500px" align="right">
                        <dx:ASPxButton ID="ButtonDelFolder" runat="server" Text="Видалити папку" Width="130px" AutoPostBack="false">
                            <ClientSideEvents Click="ButtonDeleteUserFolderClick" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>

        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>

<br/>

<dx:ASPxRoundPanel ID="PanelReportName" runat="server" HeaderText="Назва звіту">
    <PanelCollection>
        <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
            <dx:ASPxTextBox ID="EditNewReportName" ClientInstanceName="EditNewReportName" runat="server" Width="500px"> </dx:ASPxTextBox>

            <br/>

            <dx:ASPxButton ID="ButtonSaveReport" ClientInstanceName="ButtonSaveReport" runat="server"
                    Text="Зберегти звіт" AutoPostBack="false">
                <ClientSideEvents Click="ButtonSaveReportClick" />
            </dx:ASPxButton>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>
