<%@ Page Title="База даних ДКВ м. Києва" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<asp:LoginView ID="LoginView1" runat="server">
    <LoggedInTemplate>

        <style type="text/css">
            .dxtv-nd span
            {
                max-width: 300px;
                white-space: normal !important;
            }
            div#MainContent_LoginView1_TreeViewStdReports_CD ul li
            {
                margin-bottom: 1.0em;
            }
            .dxtv-subnd
            {
                margin-left: 24px !important;
            }
            .dxtv-ndTxt
            {
                font-family: Arial !important;
            }
        </style>

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <div style="float: right; padding-top: 2px; padding-right: 2px;">
                        <dx:ASPxHyperLink ID="LinkSysReports" runat="server" Text="Звіти з Контролю Даних" NavigateUrl="~/SysReports/SysReports.aspx"></dx:ASPxHyperLink>
                    </div>
                    
                    <div style="padding: 0 0 10px 10px; border-bottom: 1px solid #DAE5F2;">
                        <dx:ASPxLabel ID="LabelMainTitle" runat="server" Text="Звіти Інформаційної Системи" CssClass="reporttitle" />
                    </div>
                </div>

                <p />

                <table border="0" cellspacing="0" cellpadding="0" width="97%">
                <tr>
                    <td valign="top" style="width: 350px;">
                        <div style="display: inline-block; border-right: 2px solid #DAE5F2; vertical-align: top; width: 350px;">
                            <dx:ASPxTreeView ID="TreeViewStdReports"
                                ClientInstanceName="TreeViewStdReports"
                                runat="server"
                                EnableClientSideAPI="True" 
                                OnNodeDataBound="TreeViewStdReports_NodeDataBound"
                                AllowSelectNode="true"
                                ShowExpandButtons="False"
                                ShowTreeLines="False"
                                Width="350px">
                                <Styles>
                                    <Node ForeColor="#034AF3" VerticalAlign="Middle"> </Node>
                                </Styles>
                                <ClientSideEvents NodeClick="function (s, e) { GridViewStdReports.PerformCallback(e.node.name); }" />
                            </dx:ASPxTreeView>
                        </div>
                    </td>
                    <td align="left" valign="top" style="width: 100%;">
                        <div style="margin-left: 10px; display: inline-block; vertical-align: top;">
                            <div style="padding-bottom: 20px;">
                                <dx:ASPxLabel ID="LabelAktRequestsInfo" Text="" runat="server" Height="20px"></dx:ASPxLabel>
                                <dx:ASPxButton ID="BtnConveyancingConfirmation" runat="server" Text="Перейти до актів" AutoPostBack="false">
                                    <ClientSideEvents Click="function (s,e) { window.location.href  = 'Reports1NF/ConveyancingList.aspx'; }" />
                                </dx:ASPxButton>
                            </div>
                            <dx:ASPxGridView ID="GridViewStdReports"  runat="server" Width="100%"
                                ClientInstanceName="GridViewStdReports" 
                                OnCustomCallback="GridViewStdReports_CustomCallback" 
                                OnRowDeleted="GridViewStdReports_RowDeleted"
                                AutoGenerateColumns="False"
                                DataSourceID="GridViewStdReports_DataSource" 
                                KeyFieldName="id"
                                EnableCallBacks="False" >

	                            <SettingsCommandButton>
		                            <EditButton>
			                            <Image Url="~/Styles/EditIcon.png" />
		                            </EditButton>
		                            <CancelButton>
			                            <Image Url="~/Styles/CancelIcon.png" />
		                            </CancelButton>
		                            <UpdateButton>
			                            <Image Url="~/Styles/SaveIcon.png" />
		                            </UpdateButton>
		                            <DeleteButton>
			                            <Image Url="~/Styles/DeleteIcon.png" />
		                            </DeleteButton>
		                            <NewButton>
			                            <Image Url="~/Styles/AddIcon.png" />
		                            </NewButton>
		                            <ClearFilterButton Text="Очистити" RenderMode="Link" />
	                            </SettingsCommandButton>

                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" VisibleIndex="0" SortIndex="0" SortOrder="Descending">
                                        <EditFormSettings Visible="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="report_name" Visible="False" VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataHyperLinkColumn FieldName="report_link" ReadOnly="True" SortIndex="1">
                                        <PropertiesHyperLinkEdit NavigateUrlFormatString="~/{0}" TextField="report_name" Target="_blank">
                                            <Style Font-Size="1.3em"> </Style>
                                        </PropertiesHyperLinkEdit>
                                        <Settings SortMode="DisplayText" />
                                    </dx:GridViewDataHyperLinkColumn>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="3" 
                                        ShowDeleteButton="True" ShowClearFilterButton="true">
                                    </dx:GridViewCommandColumn>
                                </Columns>
                                <SettingsBehavior ConfirmDelete="True" />
                                <SettingsPager Mode="ShowAllRecords">
                                </SettingsPager>
                                <Settings GridLines="None" ShowColumnHeaders="False" ShowGroupButtons="False" 
                                    ShowHeaderFilterBlankItems="False" ShowStatusBar="Hidden" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                                <Styles>
                                    <Cell BackColor="#FFFFEF">
                                        <Paddings PaddingBottom="8px" PaddingTop="8px" />
                                    </Cell>
                                    <CommandColumn BackColor="#FFFFEF">
                                    </CommandColumn>
                                </Styles>
                                <Border BorderStyle="None" />
                            </dx:ASPxGridView>
                        </div>
                    </td>
                </tr>
                </table>

            </ContentTemplate>
        </asp:UpdatePanel>

    </LoggedInTemplate>
</asp:LoginView>

<mini:ProfiledSqlDataSource ID="GridViewStdReports_DataSource" runat="server" 
        ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
        OnSelecting="GridViewStdReports_DataSource_Selecting" 
        SelectCommand="SELECT id, report_name, 
                        CASE 
	                        WHEN category IN (1 , 2) THEN 
		                        'Objects/Objects.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
	                        WHEN category = 4 THEN 
		                        'Balans/BalansOrgList.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 5 THEN 
		                        'Balans/BalansObjects.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 13 THEN 
		                        'Balans/BalansArenda.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
	                        WHEN category = 6 THEN 
		                        'Arenda/RentOrgList.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 7 THEN 
		                        'Arenda/RentedObjects.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
	                        WHEN category = 8 THEN 
		                        'Documents/DocRelations.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 9 THEN 
		                        'Documents/DocObjects.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
	                        WHEN category = 10 THEN 
		                        'Privatization/PrivatDocList.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 11 THEN 
		                        'Privatization/PrivatObjects.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
	                        WHEN category = 12 THEN 
		                        'Finance/FinanceView.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 14 THEN 
		                        'Catalogue/AddrCatalogue.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 15 THEN 
		                        'Catalogue/OrgCatalogue.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 18 THEN 
		                        'OrendnaPlata/OrPlataByBalans.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 19 THEN 
		                        'OrendnaPlata/OrPlataByRenters.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 20 THEN 
		                        'OrendnaPlata/OrPlataByBuildings.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 21 THEN 
		                        'OrendnaPlata/SummaryReport.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 22 THEN 
		                        'OrendnaPlata/OrPlataNotSubmitted.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 23 THEN 
		                        'OrendnaPlata/OrPlataObjectsUse.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 24 THEN 
		                        'Assessment/Assessment.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
                            WHEN category = 26 THEN 
		                        'Arenda/RentAgreements.aspx?grid=' + CAST(category AS VARCHAR(10)) + '&amp;report=' + CAST(id AS VARCHAR(10)) 
	                        END AS report_link 
                        FROM user_reports WHERE (folder_id = @FolderID) ORDER BY id"

        DeleteCommand="DELETE FROM user_reports WHERE id = @id" >
    <DeleteParameters>
        <asp:Parameter Name="id" />
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter Name="FolderID" />
    </SelectParameters>
    </mini:ProfiledSqlDataSource>

</asp:Content>

