<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConveyancingRequestsList.aspx.cs" Inherits="Reports1NF_ConveyancingRequestsList" MasterPageFile="~/NoHeader.master" Title="Зміна балансоутримувачів об'єктів" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .buttons-panel 
        {
            margin: 5px 5px 5px 0; height: 25px;
        }    
        .action-button-wrapper
        {
            float: left; padding: 0 5px 5px 0;
        }
        .action-button-wrapper:last-child 
        {
            padding-bottom: 5px;
        }
        .title-block
        {
            font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;
        }
    </style>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript">
        var reportId = <%= ReportID %>;

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
            var height = Math.max(0, document.documentElement.clientHeight - 170);
            PrimaryGridView.SetHeight(height);
        }

        function showAttachDocuments(request_id, mode) {
            $.cookie('RecordID', request_id);
            if (mode == 'rish') {
                ASPxFileManagerPhotoFiles1.Refresh();
                PopupObjectPhotos1.Show();
			} else if (mode == 'akt') {
                ASPxFileManagerPhotoFiles2.Refresh();
                PopupObjectPhotos2.Show();
            }
        }
	</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceConveyancingRequests" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="
        SELECT req.*, 
        CASE WHEN req.status = 1 THEN 'Введено'
             WHEN req.status = 2 THEN 'Відхилено'
             WHEN req.status = 3 THEN 'Підтверджено'
             ELSE 'Невідомий'
        END AS text_status,
        CASE WHEN req.conveyancing_type = 1 THEN 'Передача об’єкту з балансу на баланс'
             WHEN req.conveyancing_type = 2 THEN 'Передача об’єкту з балансу на баланс'
             WHEN req.conveyancing_type = 3 THEN 'Списання об’єкту з балансу шляхом зносу'
             WHEN req.conveyancing_type = 4 THEN 'Списання об’єкту з балансу шляхом приватизації'
             WHEN req.conveyancing_type = 5 THEN 'Постановка  новозбудованого об’єкту на баланс'
             ELSE 'Невідомий'
        END AS type_of_conveyancing, 
        vb.district, vb.street_full_name, vb.addr_nomer, vb.sqr_total, 
        org_from.zkpo_code org_from_zkpo, org_from.full_name org_from_name, org_to.zkpo_code org_to_zkpo, org_to.full_name org_to_name
        FROM transfer_requests req LEFT JOIN view_balans_all vb ON req.balans_id = vb.balans_id 
        LEFT JOIN organizations org_from ON req.org_from_id = org_from.id and (org_from.is_deleted is null or org_from.is_deleted = 0)
        LEFT JOIN organizations org_to ON req.org_to_id = org_to.id and (org_to.is_deleted is null or org_to.is_deleted = 0)
        WHERE (req.org_from_id = @our_org_id OR req.org_id_for_confirm = @our_org_id OR (req.org_to_id = @our_org_id AND req.conveyancing_type IN(1, 5))) AND (req.status <> 3) AND (req.is_object_exists = 1) 
        
        UNION

        SELECT req.*, 
        CASE WHEN req.status = 1 THEN 'Введено'
             WHEN req.status = 2 THEN 'Відхилено'
             WHEN req.status = 3 THEN 'Підтверджено'
             ELSE 'Невідомий'
        END AS text_status,
        CASE WHEN req.conveyancing_type = 1 THEN 'Передача об’єкту з балансу на баланс'
             WHEN req.conveyancing_type = 2 THEN 'Передача об’єкту з балансу на баланс'
             WHEN req.conveyancing_type = 3 THEN 'Списання об’єкту з балансу шляхом зносу'
             WHEN req.conveyancing_type = 4 THEN 'Списання об’єкту з балансу шляхом приватизації'
             WHEN req.conveyancing_type = 5 THEN 'Постановка  новозбудованого об’єкту на баланс'
             ELSE 'Невідомий'
        END AS type_of_conveyancing, 
        b.district, b.street_full_name, b.addr_nomer, ub.sqr_total,
        org_from.zkpo_code org_from_zkpo, org_from.full_name org_from_name, org_to.zkpo_code org_to_zkpo, org_to.full_name org_to_name
        FROM transfer_requests req LEFT JOIN unverified_balans ub ON req.balans_id = ub.id 
        LEFT JOIN organizations org_from ON req.org_from_id = org_from.id AND (org_from.is_deleted is null or org_from.is_deleted = 0)
        LEFT JOIN organizations org_to ON req.org_to_id = org_to.id AND (org_to.is_deleted is null or org_to.is_deleted = 0)
        LEFT JOIN view_buildings b ON b.building_id = ub.building_id
        WHERE (req.org_from_id = @our_org_id OR req.org_id_for_confirm = @our_org_id OR (req.org_to_id = @our_org_id AND req.conveyancing_type IN(1, 5))) AND (req.status <> 3) AND (req.is_object_exists <> 1) 
        
        " OnSelecting="SqlDataSourceConveyancingRequests_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="our_org_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left"  Visible="false" >
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Cabinet.aspx" Text="Стан"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgInfo.aspx" Text="Загальна Інформація"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansList.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansDeletedList.aspx" Text="Відчужені Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgArendaList.aspx" Text="Договори використання приміщень "></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgRentedList.aspx" Text="Договори Орендування"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingRequestsList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/Logout.aspx" Text="Вийти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/ChangePasswordNoMenu.aspx" Text="Пароль"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Help.aspx" Text="?"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<p class="title-block">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Зміна балансоутримувачів об'єктів" CssClass="pagetitle"/>
</p>

<div class="buttons-panel">
    <div class="action-button-wrapper">
        <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Прийняти об'єкт на баланс" 
            AutoPostBack="False">
            <ClientSideEvents Click="function (s, e) { window.location.href  = 'TransferRequestAdd.aspx?rid=' + reportId + '&t=1'; }" />
        </dx:ASPxButton>
    </div>
    <div class="action-button-wrapper">
        <dx:ASPxButton ID="ASPxButton4" runat="server" Text="Передати об'єкт з балансу" AutoPostBack="false">
            <ClientSideEvents Click="function (s, e) { window.location.href  = 'TransferRequestAdd.aspx?rid=' + reportId + '&t=2'; }" />
        </dx:ASPxButton>
    </div>
    <div class="action-button-wrapper">
        <dx:ASPxButton ID="ASPxButton5" runat="server" Text="Списати об'єкту з балансу шляхом зносу" AutoPostBack="false">
            <ClientSideEvents Click="function (s, e) { window.location.href  = 'TransferRequestAdd.aspx?rid=' + reportId + '&t=3'; }" />
        </dx:ASPxButton>
    </div>
    <div class="action-button-wrapper">
        <dx:ASPxButton ID="ASPxButton6" runat="server" Text="Списати об'єкту з балансу шляхом приватизації" AutoPostBack="false">
            <ClientSideEvents Click="function (s, e) { window.location.href  = 'TransferRequestAdd.aspx?rid=' + reportId + '&t=4'; }" />
        </dx:ASPxButton>
    </div>
    <div class="action-button-wrapper">
        <dx:ASPxButton ID="ASPxButton7" runat="server" Text="Постановка  новозбудованого об'єкту на баланс" AutoPostBack="false">
            <ClientSideEvents Click="function (s, e) { window.location.href  = 'TransferRequestAdd.aspx?rid=' + reportId + '&t=5'; }" />
        </dx:ASPxButton>
    </div>

    <div class="action-button-wrapper">
        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Пакетна передача об'єктів з балансу на баланс" AutoPostBack="false">
            <ClientSideEvents Click="function (s, e) { window.location.href  = 'MassTransfer.aspx'; }" />
        </dx:ASPxButton>
    </div>
</div>

<dx:ASPxGridView ID="PrimaryGridView" ClientInstanceName="PrimaryGridView" runat="server" AutoGenerateColumns="False" KeyFieldName="request_id" Width="100%" DataSourceID="SqlDataSourceConveyancingRequests" >
    <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" />
    <Columns>    
        <dx:GridViewDataTextColumn FieldName="balans_id" ShowInCustomizationForm="False" Caption="Редагування">
            <DataItemTemplate>
<%--
                <%# "<center>"
                + "<a href=\"TransferRequestAdd.aspx?rid=" + ReportID + "&reqid=" + Eval("request_id") + "\"><img border='0' src='../Styles/EditIcon.png'/></a>"
                + (this.userOrgId.ToString() == (Eval("org_id_for_confirm") ?? 0).ToString()
                    ? string.Empty
                    : " "
                        + "<a class=\"delete-transfer-request\" href=\"ConveyancingRequestsList.aspx?rid=" + ReportID + "&reqid=" + Eval("request_id") + "&mode=delete\"><img border='0' src='../Styles/DeleteIcon.png'/></a>")
                + "</center>" %>
--%>
                <%# "<center>"
                + "<a href=\"TransferRequestAdd.aspx?rid=" + ReportID + "&reqid=" + Eval("request_id") + "\"><img border='0' src='../Styles/EditIcon.png'/></a>"
                +  " "
                + "<a class=\"delete-transfer-request\" href=\"ConveyancingRequestsList.aspx?rid=" + ReportID + "&reqid=" + Eval("request_id") + "&mode=delete\"><img border='0' src='../Styles/DeleteIcon.png'/></a>"
                +  " "
                + "<a title=\"Файли розпорядчих документів\" href=\"#\" onClick=\"showAttachDocuments(" + Eval("request_id") + ", 'rish')\"><img border='0' src='../Styles/PdfReportIcon.png'/></a>"
                +  " "
                + "<a title=\"Файли акту\" href=\"#\" onClick=\"showAttachDocuments(" + Eval("request_id") + ", 'akt')\"><img border='0' src='../Styles/current_stage_pdf.png'/></a>"
                + "</center>" %>            

            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>          
        <dx:GridViewDataTextColumn FieldName="text_status" Caption="Статус"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="type_of_conveyancing" Caption="Тип передачі"></dx:GridViewDataTextColumn>
        <dx:GridViewBandColumn Caption="Адреса об'єкту">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="district" Caption="Район"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="street_full_name" Caption="Вулиця"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="addr_nomer" Caption="Будинок"></dx:GridViewDataTextColumn>   
            </Columns>
        </dx:GridViewBandColumn>
        <dx:GridViewDataTextColumn FieldName="conveyancing_area" Caption="Площа, що передається"></dx:GridViewDataTextColumn>
        <dx:GridViewBandColumn Caption="Організація, від якої передається">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="org_from_zkpo" Caption="ЕДРПОУ"></dx:GridViewDataTextColumn>      
                <dx:GridViewDataTextColumn FieldName="org_from_name" Caption="Назва"></dx:GridViewDataTextColumn>   
            </Columns>
        </dx:GridViewBandColumn>
        <dx:GridViewBandColumn Caption="Організація, до якої передається">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="org_to_zkpo" Caption="ЕДРПОУ"></dx:GridViewDataTextColumn>  
                <dx:GridViewDataTextColumn FieldName="org_to_name" Caption="Назва"></dx:GridViewDataTextColumn>     
            </Columns>
        </dx:GridViewBandColumn>
        <dx:GridViewBandColumn Caption="Акт">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="new_akt_num" Caption="Номер"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="new_akt_date" Caption="Дата"></dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="new_akt_summa" Caption="Сумма"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="new_akt_summa_zalishkova" Caption="Залишкова сумма"></dx:GridViewDataTextColumn>
            </Columns>
        </dx:GridViewBandColumn>
    </Columns>

    <TotalSummary>
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowFilterBar="Visible"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="10" AlwaysShowPager="true" />
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
    <SettingsCookies CookiesID="GUKV.Reports1NF.ConveyancingRequestsList" Enabled="True" Version="A2" />
</dx:ASPxGridView>

<dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare1" runat="server" AllowDragging="True" 
	ClientInstanceName="PopupObjectPhotos1" EnableClientSideAPI="True" 
	HeaderText="Документ" Modal="True" 
	PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
	PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
	<ContentCollection>
		<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">

			<asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles1" runat="server" 
				DeleteMethod="Delete" InsertMethod="Insert" 
				OnInserting="ObjectDataSourcePhotoFiles_Inserting" 
				SelectMethod="Select" 
				TypeName="ExtDataEntry.Models.FileAttachment">
				<DeleteParameters>
					<asp:Parameter DefaultValue="transfer_requests_rish_attachfiles" Name="scope" Type="String" />
					<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
					<asp:Parameter Name="id" Type="String" />
				</DeleteParameters>
				<InsertParameters>
					<asp:Parameter DefaultValue="transfer_requests_rish_attachfiles" Name="scope" Type="String" />
					<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
					<asp:Parameter Name="Name" Type="String" />
					<asp:Parameter Name="Image" Type="Object" />
				</InsertParameters>
				<SelectParameters>
					<asp:Parameter DefaultValue="transfer_requests_rish_attachfiles" Name="scope" Type="String" />
					<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
				</SelectParameters>
			</asp:ObjectDataSource>

			<dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles1" runat="server" 
				ClientInstanceName="ASPxFileManagerPhotoFiles1" DataSourceID="ObjectDataSourcePhotoFiles1">
				<Settings RootFolder="~\" ThumbnailFolder="~\Thumb\a1\" />
				<SettingsFileList>
					<ThumbnailsViewSettings ThumbnailSize="180px" />
				</SettingsFileList>
				<SettingsEditing AllowDelete="True" AllowDownload="true" />
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

			<dx:ASPxButton ID="ASPxButtonClose1" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
				<ClientSideEvents Click="function(s, e) { PopupObjectPhotos1.Hide(); }" />
			</dx:ASPxButton>

		</dx:PopupControlContentControl>
	</ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare2" runat="server" AllowDragging="True" 
	ClientInstanceName="PopupObjectPhotos2" EnableClientSideAPI="True" 
	HeaderText="Документ" Modal="True" 
	PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
	PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
	<ContentCollection>
		<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

			<asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles2" runat="server" 
				DeleteMethod="Delete" InsertMethod="Insert" 
				OnInserting="ObjectDataSourcePhotoFiles_Inserting" 
				SelectMethod="Select" 
				TypeName="ExtDataEntry.Models.FileAttachment">
				<DeleteParameters>
					<asp:Parameter DefaultValue="transfer_requests_akt_attachfiles" Name="scope" Type="String" />
					<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
					<asp:Parameter Name="id" Type="String" />
				</DeleteParameters>
				<InsertParameters>
					<asp:Parameter DefaultValue="transfer_requests_akt_attachfiles" Name="scope" Type="String" />
					<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
					<asp:Parameter Name="Name" Type="String" />
					<asp:Parameter Name="Image" Type="Object" />
				</InsertParameters>
				<SelectParameters>
					<asp:Parameter DefaultValue="transfer_requests_akt_attachfiles" Name="scope" Type="String" />
					<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
				</SelectParameters>
			</asp:ObjectDataSource>

			<dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles2" runat="server" 
				ClientInstanceName="ASPxFileManagerPhotoFiles2" DataSourceID="ObjectDataSourcePhotoFiles2">
				<Settings RootFolder="~\" ThumbnailFolder="~\Thumb\a2\" />
				<SettingsFileList>
					<ThumbnailsViewSettings ThumbnailSize="180px" />
				</SettingsFileList>
				<SettingsEditing AllowDelete="True" AllowDownload="true" />
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

			<dx:ASPxButton ID="ASPxButtonClose2" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
				<ClientSideEvents Click="function(s, e) { PopupObjectPhotos2.Hide(); }" />
			</dx:ASPxButton>

		</dx:PopupControlContentControl>
	</ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxGlobalEvents ID="ge" runat="server">
    <ClientSideEvents ControlsInitialized="OnControlsInitialized" />
</dx:ASPxGlobalEvents>

</asp:Content>

<asp:Content ContentPlaceHolderID="FooterContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.delete-transfer-request').click(function () {
                return confirm('Підтвердіть видалення вибраного запиту на передачу прав.');
            });
        });
    </script>
</asp:Content>
