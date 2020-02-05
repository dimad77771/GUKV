<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreeCyclePopupPickRenterOrg.aspx.cs" Inherits="Reports1NF_FreeCyclePopupPickRenterOrg"
    MasterPageFile="~/Empty.master" Title="Картка Договору Оренди" %>

<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxImageGallery" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>
<%@ Register src="ReportCommentViewer.ascx" tagname="ReportCommentViewer" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    <style type="text/css">
        .SpacingPara
        {
            font-size: 10px;
            margin-top: 4px;
            margin-bottom: 0px;
            padding-top: 0px;
            padding-bottom: 0px;
        }
    </style>

    <script type="text/javascript" language="javascript">

    // <![CDATA[
        var period;
        document.addEventListener("DOMContentLoaded", ready);

		function ready(event) {
			AdjustSize();
		}
     // ]]>

    </script>

	<script type="text/javascript" language="javascript">
    </script>

    <script type="text/javascript">
        function OnInit(s, e) {
            AdjustSize();
        }
        function OnEndCallback(s, e) {
            AdjustSize();
        }
        function OnControlsInitialized(s, e) {
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
            });
        }
        function AdjustSize() {
			//var hh = Math.max(0, document.documentElement.clientHeight) - 240;
			//$("#div_data_table").height(hh);
			//console.log("height=", height);
			//console.log("height=", $("#div_data_table").height());
            //grid.SetHeight(height);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearchRenter" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations org
        WHERE 
        	(@zkpo != '^' AND @fname != '^' AND org.id IN (SELECT TOP 200 id FROM organizations WHERE isactive = 1 AND (zkpo_code = @zkpo OR @zkpo = '%') AND full_name LIKE @fname AND (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL))
        ORDER BY org.full_name"
    OnSelecting="SqlDataSourceOrgSearchRenter_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>


	<dx:ASPxCallbackPanel ID="CPPopUp" ClientInstanceName="CPPopUp" runat="server" OnCallback="CPPopUp_Callback" ScrollBars="None">
		<ClientSideEvents EndCallback="function (s, e) { if (LabelOrgEditError.clientVisible) return; window.parent.PopupEditRenterOrg.Hide(); }" />
		<PanelCollection>
			<dx:panelcontent ID="Panelcontent1" runat="server" >
				<asp:FormView runat="server" BorderStyle="None" ID="OrganizationsForm" EnableViewState="False" >
					<ItemTemplate>
						<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Орендар/Позичальник">
							<ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
								<PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
										<table border="0" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td> <dx:ASPxLabel ID="ASPxLabel58" runat="server" Text="Код ЄДРПОУ:" Width="90px" /> </td>
                                                <td> <dx:ASPxTextBox ID="EditRenterOrgZKPO" ClientInstanceName="EditRenterOrgZKPO" runat="server" Width="200px" Title="Код ЄДРПОУ Орендаря/позичальника"/> </td>
                                                <td align="right"> <dx:ASPxLabel ID="ASPxLabel59" runat="server" Text="Назва:" Width="50px" /> </td>
                                                <td align="right"> <dx:ASPxTextBox ID="EditRenterOrgName" ClientInstanceName="EditRenterOrgName" runat="server" Width="300px" Title="Назва Орендаря/позичальника"/> </td>
                                                <td align="right">
                                                    <dx:ASPxButton ID="BtnFindRenterOrg" ClientInstanceName="BtnFindRenterOrg" runat="server" AutoPostBack="False" Text="Знайти потенційного орендаря" Width="250px">
                                                        <ClientSideEvents Click="function (s, e) { ComboRenterOrg.PerformCallback(EditRenterOrgZKPO.GetText() + '|' + EditRenterOrgName.GetText()); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
											<tr>
                                                <td colspan="4" valign="top">
                                                    <dx:ASPxComboBox ID="ComboRenterOrg" ClientInstanceName="ComboRenterOrg" runat="server" DropDownStyle="DropDownList"
                                                        Width="700px" DataSourceID="SqlDataSourceOrgSearchRenter" ValueField="id" ValueType="System.Int32"
                                                        TextField="search_name" EnableSynchronization="True" OnCallback="ComboRenterOrg_Callback"
                                                        Title="Орендар/Позичальник" Value='<%# Eval("org_renter_id") %>'>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td align="right">
                                                    <dx:ASPxButton ID="BtnAddRenterOrg" ClientInstanceName="BtnAddRenterOrg" runat="server" AutoPostBack="False" Text="Додати потенційного орендаря" Width="250px">
                                                        <ClientSideEvents Click="function (s, e) { window.parent.doAddExistingOrg(ComboRenterOrg.GetValue()); }" />
                                                    </dx:ASPxButton>
                                                </td>
											</tr>
											<tr style="height:50px">
												<td colspan="5" align="right" valign="bottom">
                                                    <dx:ASPxButton ID="BtnCreateNewOrg" ClientInstanceName="BtnCreateNewOrg" runat="server" AutoPostBack="False" Text="Створити нову особу і додати її як потенційного орендаря" Width="500px">
                                                        <ClientSideEvents Click="function (s, e) { window.parent.doCreateNewOrg(EditRenterOrgZKPO.GetText()); }" />
                                                    </dx:ASPxButton>
                                                </td>
											</tr>
										</table>
                                    </dx:PanelContent>
								</PanelCollection>
						</dx:ASPxRoundPanel>
					</ItemTemplate>
				</asp:FormView>
			</dx:panelcontent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>




</asp:Content>
