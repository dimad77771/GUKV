<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreeCycle.aspx.cs" Inherits="Reports1NF_FreeCycle"
    MasterPageFile="~/NoMenu.master" Title="Картка Договору Оренди" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
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
		var scrollTop_div_data_table;
		var has_form_change = false;
		var freecycle_id = <%= freecycle_id %>;
		var free_square_id = <%= free_square_id %>;
		var is_read_only = <%= is_read_only.ToString().ToLower() %>;
		var list_freecycle_orendar = <%= Converter.JsonConvertSerializeObject(list_freecycle_orendar.ToArray()) %>;
		var list_freecycle_step = <%= Converter.JsonConvertSerializeObject(list_freecycle_step.ToArray()) %>;
		

        document.addEventListener("DOMContentLoaded", ready);

		function ready(event) {
			setTimeout(AdjustSize, 50);
		}

		function showDocuments(freecycle_step_id) {
			$.cookie('RecordID', freecycle_step_id);
            for (var i = 0; i < list_freecycle_step.length; i++) {
				if (list_freecycle_step[i].freecycle_step_id == freecycle_step_id) break;
			}
			var modify_date = list_freecycle_step[i].modify_date;
			var modified_by = list_freecycle_step[i].modified_by;
			if (formatDate(modify_date) == "") {
				var modified_str = "";
			} else {
				var modified_str = formatDate(modify_date) + " / " + modified_by;
			}
			//alert(modified_str);
			LabelUserModifiedInfo.SetText(modified_str);

			ASPxFileManagerPhotoFiles.Refresh();

			PopupObjectPhotos.Show();
		}

		function deleteOrendar(freecycle_orendar_id) {
			if (checkNeedSave()) return;

            for (var i = 0; i < list_freecycle_orendar.length; i++) {
				if (list_freecycle_orendar[i].freecycle_orendar_id == freecycle_orendar_id) break;
			}
			if (confirm('Видалити потенцийного орендаря "' + list_freecycle_orendar[i].full_name + ' з картки ?')) {
				CPDeleteOrendar.PerformCallback(freecycle_orendar_id);
			}
		}

		function moveToArhiv() {
			if (confirm('Закрити картку і перевести її в архів ?"')) {
				CPMoveToArhiv.PerformCallback();
			}
		}

		function editOrendar(freecycle_orendar_id) {
			if (checkNeedSave()) return;

			PopupEditRenterOrg.SetHeaderText("Редагування Організації");
			PopupEditRenterOrg.SetContentUrl("FreeCyclePopupEditRenterOrg.aspx?freecycle_orendar_id=" + freecycle_orendar_id + "&freecycle_id=" + freecycle_id);
			PopupEditRenterOrg.RefreshContentUrl();
			PopupEditRenterOrg.Show();
		}

		function addOrendar() {
			PopupPickRenterOrg.SetContentUrl("FreeCyclePopupPickRenterOrg.aspx");
			PopupPickRenterOrg.RefreshContentUrl();
			PopupPickRenterOrg.Show();
		}

		function doCreateNewOrg(zkpo) {
			PopupPickRenterOrg.Hide();
			PopupEditRenterOrg.SetHeaderText("Створення Організації");
			PopupEditRenterOrg.SetContentUrl("FreeCyclePopupEditRenterOrg.aspx?freecycle_orendar_id=-1" + "&freecycle_id=" + freecycle_id + "&zkpo=" + zkpo);
			PopupEditRenterOrg.RefreshContentUrl();
			PopupEditRenterOrg.Show();
		}

		function doAddExistingOrg(org_orendar_id) {
			if (org_orendar_id == null) return;
			PopupPickRenterOrg.Hide();
			CPAddExistingOrendar.PerformCallback(org_orendar_id);
		}

		function doActionOnArhivTable(s, e) {
			var freecycle_id = s.GetRowKey(e.visibleIndex);

			if (e.buttonID == 'btnViewArhivCard') {
				var url = "FreeCycle.aspx?freecycle_id=" + freecycle_id;
				window.location.assign(url);
			} else if (e.buttonID == 'btnDeleteArhivCard') {
				if (confirm('Видалити архівну картку ?')) {
					CPDeleteArhivCard.PerformCallback(freecycle_id);
				}
			}
		}

		function checkNeedSave() {
			if (has_form_change) {
				alert("Необхідно спочатку зберегти зміни");
				return true;
			} else {
				return false;
			}
		}

		function formatDate(date) {
			if (!isNotEmpty(date)) {
				return '';
			} else {
				var str = date.substring(8, 10) + "." + date.substring(5, 7) + "." + date.substring(0, 4) + " " + date.substring(11, 13) + ":" + date.substring(14, 16);
				return str;
			}
		}
		function isNotEmpty(arg) {
			return (arg != null && arg != "");
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
			var hh = Math.max(0, document.documentElement.clientHeight) - 270;
			$("#div_data_table").height(hh);

			if (!is_read_only) {
				$("#bottomTable").show();
				$("#bottomArhivTable").hide();
			} else {
				$("#bottomTable").hide();
				$("#bottomArhivTable").show();
			}
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

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

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreecycleRejectionDict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select rejection_cod + ' - ' + rejection_name as rejection_full_name, rejection_id, step_ord from freecycle_rejection_dict union select '', null, 9999 order by step_ord"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRenter" runat="server" ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT org.* FROM organizations org
                    LEFT JOIN freecycle_orendar agr
                    ON agr.org_orendar_id = org.id
                    WHERE agr.freecycle_orendar_id = @freecycle_orendar_id"
    OnSelecting="SqlDataSourceRenter_Selecting">         
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="freecycle_orendar_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceArhivCycles" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
select
*
from
(
	select
	A.freecycle_id,
	A.free_square_id,
	A.cycle_num,
	A.reg_number,	-- Реєстраційний номер
	B.step_date as first_step_date,
	(select Q.s_name from view_organizations_name Q where Q.organization_id = B.org_orendar_id) as first_org_orendar_name,
	(select Q.step_name from freecycle_step_dict Q where Q.step_id = B.step_id) as first_step_name,
	C.step_date as last_step_date,
	(select Q.s_name from view_organizations_name Q where Q.organization_id = C.org_orendar_id) as last_org_orendar_name,
	(select Q.step_name from freecycle_step_dict Q where Q.step_id = C.step_id) as last_step_name,

	Stuff(
	  (
		 select '<br/>' + CHAR(13) + CHAR(10) + '- ' + Q2.s_name
		 from freecycle_orendar Q
		 join view_organizations_name Q2 on Q2.organization_id = Q.org_orendar_id
		 where Q.freecycle_id = A.freecycle_id
		 FOR XML PATH(''),TYPE)
		.value('text()[1]','nvarchar(max)'),1,7,N''
	  ) as org_orendar_name_set,

	case when row_number() over(order by A.cycle_num desc) = 1 then 1 else 0 end is_current_row

	from freecycle A
	outer apply
	(
		select 
			top 1 
			Q.*,
			Q2.org_orendar_id
		from freecycle_step Q 
		join freecycle_orendar Q2 on Q2.freecycle_orendar_id = Q.freecycle_orendar_id and Q2.is_deleted = 0
		where Q.freecycle_id = A.freecycle_id and Q.step_date is not null and Q.is_deleted = 0 
		order by Q.step_date
	) B
	outer apply
	(
		select 
			top 1 
			Q.*,
			Q2.org_orendar_id
		from freecycle_step Q 
		join freecycle_orendar Q2 on Q2.freecycle_orendar_id = Q.freecycle_orendar_id and Q2.is_deleted = 0
		where Q.freecycle_id = A.freecycle_id and Q.step_date is not null and Q.is_deleted = 0 
		order by Q.step_date desc
	) C
	where 1=1
	and A.is_deleted = 0
	and A.free_square_id = @free_square_id
) as T
where is_current_row = 0
order by T.cycle_num desc
"
    OnSelecting="SqlDataSourceArhivCycles_Selecting"
	OnSelected="SqlDataSourceArhivCycles_Selected">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="free_square_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Cabinet.aspx" Text="Стан"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgInfo.aspx" Text="Загальна Інформація"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansList.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansDeletedList.aspx" Text="Відчужені Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgArendaList.aspx" Text="Договори використання приміщень "></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgRentedList.aspx" Text="Договори Орендування"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/Logout.aspx" Text="Вийти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/ChangePasswordNoMenu.aspx" Text="Пароль"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="http://kmda.iisd.com.ua" Text="Класифікатор майна"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabelHeader" Text="" CssClass="pagetitle"/>
	<asp:Label runat="server" ID="ASPxLabelHeader2" Text="" CssClass="pagetitle" />
</p>


<dx:ASPxCallbackPanel ID="CPDeleteOrendar" ClientInstanceName="CPDeleteOrendar" runat="server" OnCallback="CPDeleteOrendar_Callback" Height="0px">
	<ClientSideEvents EndCallback="function (s, e) { window.location.reload(false); }" />
</dx:ASPxCallbackPanel>

<dx:ASPxCallbackPanel ID="CPDeleteArhivCard" ClientInstanceName="CPDeleteArhivCard" runat="server" OnCallback="CPDeleteArhivCard_Callback" Height="0px">
	<ClientSideEvents EndCallback="function (s, e) { window.location.reload(false); }" />
</dx:ASPxCallbackPanel>

<dx:ASPxCallbackPanel ID="CPAddExistingOrendar" ClientInstanceName="CPAddExistingOrendar" runat="server" OnCallback="CPAddExistingOrendar_Callback" Height="0px" >
	<ClientSideEvents EndCallback="function (s, e) { window.location.reload(false); }" />
</dx:ASPxCallbackPanel>

<dx:ASPxCallbackPanel ID="CPMoveToArhiv" ClientInstanceName="CPMoveToArhiv" runat="server" OnCallback="CPMoveToArhiv_Callback" Height="0px">
	<ClientSideEvents EndCallback="function (s, e) { window.location.assign('FreeCycle.aspx?free_square_id=' + free_square_id + ''); }" />
</dx:ASPxCallbackPanel>

<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" runat="server" OnCallback="CPMainPanel_Callback">
	<ClientSideEvents EndCallback="function (s, e) { has_form_change = false; $('#div_data_table').scrollTop(scrollTop_div_data_table); AdjustSize(); }" />
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent5" runat="server">

			<dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare" runat="server" AllowDragging="True" 
				ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True" 
				HeaderText="Документ" Modal="True" 
				PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
				PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

						<asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server" 
							DeleteMethod="Delete" InsertMethod="Insert" 
							OnInserting="ObjectDataSourcePhotoFiles_Inserting" 
							SelectMethod="Select" 
							TypeName="ExtDataEntry.Models.FileAttachment">
							<DeleteParameters>
								<asp:Parameter DefaultValue="freecycle_step_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
								<asp:Parameter Name="id" Type="String" />
							</DeleteParameters>
							<InsertParameters>
								<asp:Parameter DefaultValue="freecycle_step_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
								<asp:Parameter Name="Name" Type="String" />
								<asp:Parameter Name="Image" Type="Object" />
							</InsertParameters>
							<SelectParameters>
								<asp:Parameter DefaultValue="freecycle_step_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
							</SelectParameters>
						</asp:ObjectDataSource>

						<dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles" runat="server" 
							ClientInstanceName="ASPxFileManagerPhotoFiles" DataSourceID="ObjectDataSourcePhotoFiles">
							<Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
							<SettingsFileList>
								<ThumbnailsViewSettings ThumbnailSize="180px" />
							</SettingsFileList>
							<SettingsEditing AllowDelete="True" />
							<SettingsFolders Visible="False" />
							<SettingsToolbar ShowDownloadButton="True" ShowPath="False" />
							<SettingsUpload UseAdvancedUploadMode="True">
								<AdvancedModeSettings EnableMultiSelect="True" />
							</SettingsUpload>

							<SettingsDataSource FileBinaryContentFieldName="Image" 
								IsFolderFieldName="IsFolder" KeyFieldName="ID" 
								LastWriteTimeFieldName="LastModified" NameFieldName="Name" 
								ParentKeyFieldName="ParentID" />
						</dx:ASPxFileManager>

						<br />

						<table width="100%">
							<tr>
								<td>
									<dx:ASPxButton ID="ASPxButtonClose" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
										<ClientSideEvents Click="function(s, e) { PopupObjectPhotos.Hide(); }" />
									</dx:ASPxButton>
								</td>
								<td align="right">
									<dx:ASPxLabel runat="server" ID="LabelUserModifiedInfo" ClientInstanceName="LabelUserModifiedInfo" Text="05.12.2019 08:33 / Синенко" />
								</td>
							</tr>
						</table>
					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>

			<dx:ASPxPopupControl ID="PopupEditRenterOrg" runat="server" ClientInstanceName="PopupEditRenterOrg" Modal="true"
				HeaderText="Створення Організації" PopupElementID="BtnEditRenterOrg"  CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
				Width="673" Height="650" ScrollBars="None" >
    
			</dx:ASPxPopupControl>

			<dx:ASPxPopupControl ID="PopupPickRenterOrg" runat="server" ClientInstanceName="PopupPickRenterOrg" Modal="true"
				HeaderText="Выбор Організації" PopupElementID="BtnEditRenterOrg"  CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
				Width="1050" Height="250" ScrollBars="None" >
    
			</dx:ASPxPopupControl>

			<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" ActiveTabIndex="0" Width="100%">
				<ClientSideEvents ActiveTabChanged="function(s, e) {
					if (s.activeTabIndex == 0) {
						$('#bottomTable').show();
					} else {
						$('#bottomTable').hide();
					}
				}" />

				<TabPages>
					<dx:TabPage Text="Картка" Name="Tab1">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl1" runat="server">
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>

					<dx:TabPage Text="Архів" Name="Tab2">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl5" runat="server">

								<dx:ASPxCallbackPanel ID="CPArhivCycles" ClientInstanceName="CPArhivCycles" runat="server" OnCallback="CPArhivCycles_Callback">
									<PanelCollection>
										<dx:panelcontent ID="Panelcontent2" runat="server">

											<dx:ASPxGridView ID="GridViewArhivCycles" ClientInstanceName="GridViewArhivCycles" runat="server" AutoGenerateColumns="False" 
												DataSourceID="SqlDataSourceArhivCycles" KeyFieldName="freecycle_id" Width="100%" >
												<ClientSideEvents CustomButtonClick="doActionOnArhivTable" />

												<Columns>
													<dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="60px">
														<CustomButtons>
															<dx:GridViewCommandColumnCustomButton ID="btnViewArhivCard" Text="Перегляд"> 
																<Image Url="~/Styles/EditIcon.png"/>
															</dx:GridViewCommandColumnCustomButton>
															<dx:GridViewCommandColumnCustomButton ID="btnDeleteArhivCard" Text="Видалити"> 
																<Image Url="~/Styles/DeleteIcon.png"/>
															</dx:GridViewCommandColumnCustomButton>
														</CustomButtons>
													</dx:GridViewCommandColumn>
													<dx:GridViewDataTextColumn FieldName="reg_number" VisibleIndex="10" Caption="Реєстра- ційний номер" Width="80px"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn FieldName="first_step_date" VisibleIndex="20" Caption="Дата початку" Width="80px">
														<PropertiesTextEdit DisplayFormatString="dd.MM.yyyy" />  
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn FieldName="first_step_name" VisibleIndex="30" Caption="Початковий cтан процесу" Width="220px"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn FieldName="first_org_orendar_name" VisibleIndex="40" Caption="Початковий потенційний орендар" Width="220px"></dx:GridViewDataTextColumn>
													
													<dx:GridViewDataTextColumn FieldName="last_step_date" VisibleIndex="120" Caption="Дата останнього стану" Width="80px">
														<PropertiesTextEdit DisplayFormatString="dd.MM.yyyy" />  
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn FieldName="last_step_name" VisibleIndex="130" Caption="Останній cтан процесу" Width="220px"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn FieldName="last_org_orendar_name" VisibleIndex="140" Caption="Останній потенційний орендар" Width="220px"></dx:GridViewDataTextColumn>

													<dx:GridViewDataTextColumn FieldName="org_orendar_name_set" VisibleIndex="500" Caption="Потенційні орендарі" Width="320px">
														<DataItemTemplate>
															<literal><%# Eval("org_orendar_name_set") %></literal>
														</DataItemTemplate>
													</dx:GridViewDataTextColumn>
												</Columns>

												<SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
												<Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" ShowHeaderFilterButton="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
												<SettingsPager Mode="ShowAllRecords" PageSize="10" />
												<SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
												<Styles Header-Wrap="True" >
													<Header Wrap="True"></Header>
												</Styles>
											</dx:ASPxGridView>

										</dx:panelcontent>
									</PanelCollection>
								</dx:ASPxCallbackPanel>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>

				</TabPages>		
			</dx:ASPxPageControl>

			<p class="SpacingPara"/>
			<table border="0" cellspacing="0" cellpadding="0" id="bottomTable" style="display:none" >
				<tr>
					<td>
						<dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
							<ClientSideEvents Click="function (s,e) { scrollTop_div_data_table = $('#div_data_table').scrollTop(); CPMainPanel.PerformCallback('save:'); }" />
						</dx:ASPxButton>
					</td>

					<td style="width:80px"> &nbsp; </td>
					<td>
						<dx:ASPxButton ID="ButtonAddNewOrandar" runat="server" Text="Додати потенційного орендаря" AutoPostBack="false" CausesValidation="false">
							<ClientSideEvents Click="function (s,e) { if (checkNeedSave()) return; addOrendar(); }" />
						</dx:ASPxButton>
					</td>

					<td style="width:80px"> &nbsp; </td>
					<td align="right">
						<dx:ASPxButton ID="ButtonSendToArhiv" runat="server" Text="Закрити картку і перевести її в архів" AutoPostBack="false" CausesValidation="false">
							<ClientSideEvents Click="function (s,e) { if (checkNeedSave()) return; moveToArhiv(); }" />
						</dx:ASPxButton>
					</td>

        

				</tr>
			</table>

			<table border="0" cellspacing="0" cellpadding="0" id="bottomArhivTable" style="display:none" >
				<tr>
					<td>
						<dx:ASPxButton ID="ButtonGoBack" runat="server" Text="Повернутися назад до поточної картки" AutoPostBack="false" CausesValidation="false">
							<ClientSideEvents Click="function (s,e) { window.history.back(); }" />
						</dx:ASPxButton>
					</td>
				</tr>
			</table>

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>


</asp:Content>
