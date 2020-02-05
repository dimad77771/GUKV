<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreeCyclePopupEditRenterOrg.aspx.cs" Inherits="Reports1NF_FreeCyclePopupEditRenterOrg"
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

<mini:ProfiledSqlDataSource ID="SqlDataSourceRenter" runat="server" ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT org.* FROM organizations org
                    RIGHT JOIN freecycle_orendar agr
                    ON agr.org_orendar_id = org.id
                    WHERE agr.freecycle_orendar_id = @freecycle_orendar_id"
    OnSelecting="SqlDataSourceRenter_Selecting">         
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="freecycle_orendar_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>  

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictDistricts2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_districts2 ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_status ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgOwnership" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_1nf_org_ownership where len(name) > 0 order by name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgIndustry" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_industry ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgOccupation" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_occupation ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>


<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" >
	<dx:ASPxCallbackPanel ID="CPPopUp" ClientInstanceName="CPPopUp" runat="server" OnCallback="CPPopUp_Callback">
		<ClientSideEvents EndCallback="function (s, e) { if (LabelOrgEditError.clientVisible) return; window.parent.PopupEditRenterOrg.Hide(); window.parent.window.location.reload(false); }" />
		<PanelCollection>
			<dx:panelcontent ID="Panelcontent1" runat="server" >
				<asp:FormView runat="server" BorderStyle="None" ID="FormView1" DataSourceID="SqlDataSourceRenter" EnableViewState="False" >
					<ItemTemplate>
						<dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" HeaderText="Основні відомості" Width="100%">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent9" runat="server" >
								<table border="0" cellspacing="0" cellpadding="2">
									<tr>
										<td>
											<asp:HiddenField ID="OldOrgId" runat="server" ClientIDMode="static" Value='<%# Eval("id") %>' /> 
											<dx:ASPxLabel ID="ASPxLabel47" runat="server" Text="Повна Назва:" Width="110px" /> 
										</td>
										<td colspan="3"> 
											<dx:ASPxTextBox ID="TextBoxFullNameOrg" ClientInstanceName="TextBoxFullNameOrg" runat="server" Width="100%" Title=""
												Value='<%# Eval("full_name") %>' />                                                                 
										</td>
									</tr>
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel48" runat="server" Text="Код ЄДРПОУ/ІНН:" Width="110px" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxZkpoCodeOrg" ClientInstanceName="TextBoxZkpoCodeOrg" runat="server" Width="90px" Title="" Value='<%# Eval("zkpo_code") %>' /> </td>
										<td> <dx:ASPxLabel ID="ASPxLabel49" runat="server" Text="Коротка Назва" Width="85px" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxShortNameOrg" runat="server" Width="290px" ClientInstanceName="TextBoxShortNameOrg" Title="" Value='<%# Eval("short_name") %>' /> </td>
									</tr>
								</table>                                                                    
							</dx:PanelContent>
						</PanelCollection>                                                        
						</dx:ASPxRoundPanel>
						<p class="SpacingPara"/>
						<dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Юридична адреса" Width="100%">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent10" runat="server">
								<table border="0" cellspacing="0" cellpadding="2">
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Район" Width="95px" /> </td>
										<td> <dx:ASPxComboBox ID="ComboBoxDistrictOrg" runat="server" ClientInstanceName="ComboBoxDistrictOrg" 
											ValueType="System.Int32" TextField="name" ValueField="id" Width="120px" Title="" 
												IncrementalFilteringMode="StartsWith" 
												DataSourceID="SqlDataSourceDictDistricts2" Value='<%# Eval("addr_distr_new_id") %>' /> </td>
										<td> <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Назва Вулиці" Width="84px" /> </td>
										<td colspan="3">
											<dx:ASPxTextBox ID="TextBoxStreetNameOrg" runat="server" ClientInstanceName="TextBoxStreetNameOrg"
												Width="100%" Title="" Value='<%# Eval("addr_street_name") %>' />
										</td>
									</tr>
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Номер Будинку" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxAddrNomerOrg" runat="server" Text="" Width="120px" ClientInstanceName="TextBoxAddrNomerOrg" Title="" Value='<%# Eval("addr_nomer") %>' /> </td>
										<td> <dx:ASPxLabel ID="ASPxLabelCorp" runat="server" Text="Корпус" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxAddrKorpusFrom" runat="server" Text="" Width="80px" ClientInstanceName="TextBoxAddrKorpusFrom" Title="" Value='<%# Eval("addr_korpus") %>' /> </td>
										<td> <dx:ASPxLabel ID="ASPxLabelZip" runat="server" Text="Пошт. Індекс" Width="85px" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxAddrZipCodeOrg" runat="server" Width="80px" ClientInstanceName="TextBoxAddrZipCodeOrg" Title="" Value='<%# Eval("addr_zip_code") %>' /> </td>
									</tr>
								</table>                                                                    
							</dx:PanelContent>
						</PanelCollection>                                                        
						</dx:ASPxRoundPanel>
						<p class="SpacingPara"/>
						<dx:ASPxRoundPanel ID="ASPxRoundPanel7" runat="server" HeaderText="Додаткові відомості" Width="100%">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent11" runat="server">
								<table border="0" cellspacing="0" cellpadding="2">
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Статус (фіз./юр. особа)" Width="160px"/> </td>
										<td> <dx:ASPxComboBox ID="ComboBoxStatusOrg" runat="server" ClientInstanceName="ComboBoxStatusOrg" 
											ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title=""
												IncrementalFilteringMode="StartsWith" 
												DataSourceID="SqlDataSourceDictOrgStatus" Value='<%# Eval("status_id") %>' /> </td>
									</tr>
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Форма Власності" Width="160px" /> </td>
										<td> <dx:ASPxComboBox ID="ComboBoxFormVlasnOrg" runat="server" ClientInstanceName="ComboBoxFormVlasnOrg" 
											ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title="" 
												IncrementalFilteringMode="StartsWith" 
												DataSourceID="SqlDataSourceDictOrgOwnership" Value='<%# Eval("form_ownership_id") %>' /></td>
									</tr>
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel50" runat="server" Text="Галузь" Width="160px" /> </td>
										<td> <dx:ASPxComboBox ID="ComboBoxIndustryOrg" runat="server" ClientInstanceName="ComboBoxIndustryOrg" 
											ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title="" 
												IncrementalFilteringMode="StartsWith" 
												DataSourceID="SqlDataSourceDictOrgIndustry" Value='<%# Eval("industry_id") %>' /> </td>
									</tr>
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel51" runat="server" Text="Вид Діяльності" Width="160px" /> </td>
										<td> <dx:ASPxComboBox ID="ComboBoxOccupationFrom" runat="server" ClientInstanceName="ComboBoxOccupationFrom" 
											ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" Title="" 
												IncrementalFilteringMode="StartsWith" 
												DataSourceID="SqlDataSourceDictOrgOccupation" Value='<%# Eval("occupation_id") %>' /> </td>
									</tr>
								</table>                                                                    
							</dx:PanelContent>
						</PanelCollection>                                                        
						</dx:ASPxRoundPanel>
						<p class="SpacingPara"/>
						<dx:ASPxRoundPanel ID="ASPxRoundPanel8" runat="server" HeaderText="Контактна інформація" Width="100%">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent12" runat="server">
								<table border="0" cellspacing="0" cellpadding="2">
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="ПІБ Директора" Width="95px" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxDirectorFioOrg" runat="server" ClientInstanceName="TextBoxDirectorFioOrg" Width="250px" Title="" Value='<%# Eval("director_fio") %>' /> </td>
										<td> &nbsp; </td>
										<td> <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Тел. Директора" Width="100px" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxDirectorPhoneOrg" runat="server" ClientInstanceName="TextBoxDirectorPhoneOrg" Width="100px" Title="" Value='<%# Eval("director_phone") %>' /> </td>
									</tr>
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Email Директора" Width="95px" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxDirectorEmailOrg" runat="server" ClientInstanceName="TextBoxDirectorEmailOrg" Width="250px" Title="" Value='<%# Eval("director_email") %>' /> </td>
										<td colspan="3"> &nbsp; </td>
									</tr>
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="ПІБ Бухгалтера" Width="95px" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxBuhgalterFioOrg" runat="server" ClientInstanceName="TextBoxBuhgalterFioOrg" Width="250px" Title="" Value='<%# Eval("buhgalter_fio") %>' /> </td>
										<td> &nbsp; </td>
										<td> <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Тел. Бухгалтера" Width="100px" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxBuhgalterPhoneOrg" runat="server" ClientInstanceName="TextBoxBuhgalterPhoneOrg" Width="100px" Title="" Value='<%# Eval("buhgalter_phone") %>' /> </td>
									</tr>
									<tr>
										<td> <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Факс" Width="95px" /> </td>
										<td> <dx:ASPxTextBox ID="TextBoxFax" runat="server" ClientInstanceName="TextBoxFax" Width="250px" Title="" Value='<%# Eval("fax") %>' /> </td>
										<td colspan="3"> &nbsp; </td>
									</tr>
								</table>                                                                    
							</dx:PanelContent>
						</PanelCollection>                                                        
						</dx:ASPxRoundPanel>
						<p class="SpacingPara"/>
						<dx:ASPxLabel ID="LabelOrgEditError" ClientInstanceName="LabelOrgEditError" runat="server" Text="" ClientVisible="false" ForeColor="Red" />
						<asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="static" Value='<%# Eval("id") %>' /> 
						<p class="SpacingPara"/>
						<dx:ASPxButton ID="ButtonDoAddOrgFrom" ClientInstanceName="ButtonDoAddOrgFrom" runat="server" AutoPostBack="False" Text="Зберегти">
							<ClientSideEvents Click="function (s, e) { console.log(CPPopUp); CPPopUp.PerformCallback('edit_org:'); }" />
						</dx:ASPxButton>       
					</ItemTemplate>
				</asp:FormView>  
			</dx:panelcontent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
</dx:PopupControlContentControl>



</asp:Content>
