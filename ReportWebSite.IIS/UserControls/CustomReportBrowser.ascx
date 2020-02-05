<%@ control language="C#" autoeventwireup="true" inherits="UserControls_CustomReportBrowser, App_Web_customreportbrowser.ascx.6bb32623" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    var refreshFolderDropDownAfterTreeCallback = false;

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

    function ReportTreeNodeExists(reportName) {

        reportName = "" + reportName.toUpperCase();

        for (var i = 0; i < TreeViewUserReports.GetNodeCount(); i++) {

            var node = TreeViewUserReports.GetNode(i);
            var divName = "div_tree_node_" + node.name;

            var nodeText = "" + document.getElementById(divName).innerHTML;
            nodeText = StringTrim(nodeText);
            nodeText = nodeText.toUpperCase();

            if (nodeText == reportName) {
                return true;
            }

            // Search the child nodes as well
            for (var j = 0; j < node.GetNodeCount(); j++) {

                var subNode = node.GetNode(i);
                divName = "div_tree_node_" + subNode.name;

                nodeText = "" + document.getElementById(divName).innerHTML;
                nodeText = StringTrim(nodeText);
                nodeText = nodeText.toUpperCase();

                if (nodeText == reportName) {
                    return true;
                }
            }
        }

        return false;
    }

    function ButtonAddFolderClick(s, e) {

        CallbackPanelTreeViewUserReports.PerformCallback("addfolder:" + EditFolderName.GetText());

        // We need to refresh the drop-down of folders in CustomReportCreator control
        refreshFolderDropDownAfterTreeCallback = true;
    }

    function ButtonDeleteFolderClick(s, e) {

        var selNode;
        var nodeName;

        selNode = TreeViewUserReports.GetSelectedNode();

        if (selNode != null && selNode != undefined) {

            nodeName = "" + selNode.name;

            // The folder nodes have their name set to "FolderX"
            var token = "" + nodeName.substr(0, 6);

            if (token == "Folder") {

                // Ask user for confirmation
                if (confirm("Усі звіти, що збережені в цю папку, також будуть видалені. Продовжувати?")) {

                    CallbackPanelTreeViewUserReports.PerformCallback("delfolder:" + nodeName);

                    // We need to refresh the drop-down of folders in CustomReportCreator control
                    refreshFolderDropDownAfterTreeCallback = true;
                }
            }
        }
    }

    function ButtonDeleteReportClick(s, e) {

        var selNode;
        var nodeName;

        selNode = TreeViewUserReports.GetSelectedNode();

        if (selNode != null && selNode != undefined) {

            nodeName = "" + selNode.name;

            // The report nodes have their name set to "ReportX"
            var token = "" + nodeName.substr(0, 6);

            if (token == "Report") {

                if (confirm("Видалити збережений звіт?")) {

                    CallbackPanelTreeViewUserReports.PerformCallback("delreport:" + nodeName);
                }
            }
        }
    }

    function CallbackPanelTreeViewUserReports_EndCallback(s, e) {

        if (refreshFolderDropDownAfterTreeCallback) {

            // This callback belongs to CustomReportCreator control. The CustomReportCreator
            // control must always be present on the page, if CustomReportBrowser is present
            CallbackPanelUserFolders.PerformCallback("");

            refreshFolderDropDownAfterTreeCallback = false;
        }
    }

    function OnHtmlDocumentLoad() {

        // Check if we have the name of selected report
        var reportNodeName = "" + EditSelectedReportName.GetText();
        var folderNodeName = "" + EditSelectedFolderName.GetText();

        if (reportNodeName != "" && folderNodeName != "") {

            var folderNode = TreeViewUserReports.GetNodeByName(folderNodeName);

            if (folderNode != null && folderNode != undefined) {

                folderNode.SetExpanded(true);

                var reportNode = folderNode.GetNodeByName(reportNodeName);

                if (reportNode != null && reportNode != undefined) {

                    reportNode.SetSelected(true);
                }
            }
        }
    }

    // ]]>

</script>

<dx:ASPxRoundPanel ID="PanelFolders" runat="server" HeaderText="Звіти користувача">
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">

            <dx:ASPxTextBox ID="EditFolderName" ClientInstanceName="EditFolderName" runat="server" Width="160px">
            </dx:ASPxTextBox>

            <br/>

            <dx:ASPxButton ID="ButtonAddFolder" ClientInstanceName="ButtonAddFolder" 
                    runat="server" Width="160px" Text="Створити папку" AutoPostBack="false">
                <ClientSideEvents Click="ButtonAddFolderClick" />
            </dx:ASPxButton>

            <br/>

            <dx:ASPxCallbackPanel ID="CallbackPanelTreeViewUserReports" ClientInstanceName="CallbackPanelTreeViewUserReports"
                runat="server" OnCallback="CallbackPanelTreeViewUserReports_Callback">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">

                        <dx:ASPxTreeView ID="TreeViewUserReports" Width="160px" runat="server"
                            ClientInstanceName="TreeViewUserReports"
                            AllowSelectNode="True"
                            AutoPostBack="False"
                            EnableClientSideAPI="True"
                            SyncSelectionMode="CurrentPathAndQuery" >

                            <NodeTextTemplate>
                                <div id="div_tree_node_<%# Container.Node.Name %>" style="white-space: normal; width: 105px">
                                    <%# Container.Node.Text %>
                                </div>
                            </NodeTextTemplate>
                        </dx:ASPxTreeView>

                    </dx:PanelContent>
                </PanelCollection>

                <ClientSideEvents EndCallback="CallbackPanelTreeViewUserReports_EndCallback" />
            </dx:ASPxCallbackPanel>

            <br/>

            <dx:ASPxButton ID="ButtonDelReport" runat="server" Text="Видалити звіт" Width="160px" AutoPostBack="false">
                <ClientSideEvents Click="ButtonDeleteReportClick" />
            </dx:ASPxButton>

            <br/>

            <dx:ASPxButton ID="ButtonDelFolder" runat="server" Text="Видалити папку" Width="160px" AutoPostBack="false">
                <ClientSideEvents Click="ButtonDeleteFolderClick" />
            </dx:ASPxButton>

        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>

<dx:ASPxTextBox ID="EditSelectedReportName" ClientInstanceName="EditSelectedReportName" runat="server" ClientVisible="false">
</dx:ASPxTextBox>

<dx:ASPxTextBox ID="EditSelectedFolderName" ClientInstanceName="EditSelectedFolderName" runat="server" ClientVisible="false">
</dx:ASPxTextBox>
