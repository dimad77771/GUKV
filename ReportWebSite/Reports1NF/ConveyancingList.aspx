<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConveyancingList.aspx.cs" Inherits="Reports1NF_ConveyancingList" MasterPageFile="~/NoHeader.master" Title="Зміна балансоутримувачів об'єктів" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script type="text/javascript">
		var isBigBossUser = <%=IsBigBossUser%>

		function CustomizeBigBoss() {
			if (isBigBossUser) {
				console.log("PrimaryGridView", PrimaryGridView.GetMainElement());
				console.log("PrimaryGridView2", $(PrimaryGridView.GetMainElement()));
				$(PrimaryGridView.GetMainElement()).find('td.bigBossUserStyle').css('display', 'none');
			}
		}

		function OnInit(s, e) {
			console.log("isBigBossUser", isBigBossUser);
			AdjustSize();
			CustomizeBigBoss();
		}

		function OnEndCallback(s, e) {
			AdjustSize();
			CustomizeBigBoss();
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceConveyancingForConfirmation" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
        SELECT req.*, 'Введено' AS text_status,
        CASE WHEN req.conveyancing_type = 1 THEN 'Передача об’єкту з балансу на баланс'
             WHEN req.conveyancing_type = 2 THEN 'Передача об’єкту з балансу на баланс'
             WHEN req.conveyancing_type = 3 THEN 'Списання об’єкту з балансу шляхом зносу'
             WHEN req.conveyancing_type = 4 THEN 'Списання об’єкту з балансу шляхом приватизації'
             WHEN req.conveyancing_type = 5 THEN 'Постановка  новозбудованого об’єкту на баланс'
             ELSE 'Невідомий'
        END AS type_of_conveyancing, 
        org_from.zkpo_code org_from_zkpo, org_from.full_name org_from_name, org_to.zkpo_code org_to_zkpo, org_to.full_name org_to_name,
        vb.district, vb.street_full_name, vb.addr_nomer, vb.sqr_total, 
        acc.has_account
        FROM transfer_requests req   
        LEFT JOIN organizations org_from ON req.org_from_id = org_from.id and (org_from.is_deleted is null or org_from.is_deleted = 0)
        LEFT JOIN organizations org_to ON req.org_to_id = org_to.id and (org_to.is_deleted is null or org_to.is_deleted = 0)
        LEFT JOIN 
	        (
		        SELECT organization_id,
		        CAST(CASE WHEN count(*) > 0 THEN 1
			         ELSE 0 
		        END AS BIT) AS has_account
		        FROM reports1nf_accounts
		        group by organization_id
	        ) AS acc
        ON req.org_id_for_confirm = acc.organization_id	
        LEFT JOIN view_balans_all vb ON req.balans_id = vb.balans_id
        WHERE (req.org_id_for_confirm is null
        OR acc.has_account <> 1 OR acc.has_account is null)
        AND (req.status = 1) AND (req.is_object_exists = 1) 
        
        UNION 

        SELECT req.*, 'Введено' AS text_status,
        CASE WHEN req.conveyancing_type = 1 THEN 'Передача об’єкту з балансу на баланс'
             WHEN req.conveyancing_type = 2 THEN 'Передача об’єкту з балансу на баланс'
             WHEN req.conveyancing_type = 3 THEN 'Списання об’єкту з балансу шляхом зносу'
             WHEN req.conveyancing_type = 4 THEN 'Списання об’єкту з балансу шляхом приватизації'
             WHEN req.conveyancing_type = 5 THEN 'Постановка  новозбудованого об’єкту на баланс'
             ELSE 'Невідомий'
        END AS type_of_conveyancing, 
        org_from.zkpo_code org_from_zkpo, org_from.full_name org_from_name, org_to.zkpo_code org_to_zkpo, org_to.full_name org_to_name,
        district.name AS district, bu.street_full_name AS street_full_name, bu.addr_nomer AS addr_nomer, ub.sqr_total, 0
        FROM transfer_requests req   
        LEFT JOIN organizations org_from ON req.org_from_id = org_from.id  and (org_from.is_deleted is null or org_from.is_deleted = 0)
        LEFT JOIN organizations org_to ON req.org_to_id = org_to.id and (org_to.is_deleted is null or org_to.is_deleted = 0)
        LEFT JOIN unverified_balans ub ON req.balans_id = ub.id
        LEFT JOIN buildings bu ON ub.building_id = bu.id
        LEFT JOIN dict_districts2 district ON bu.addr_distr_new_id = district.id
        WHERE (req.status = 1) AND (req.is_object_exists = 0)   
        "
        >
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFAccounts.aspx" Text="Облікові Записи"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFNotifications.aspx" Text="Налаштування Повідомлень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>        
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFFreeSquare.aspx" Text="Перелік вільних приміщень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFPrivatisatSquare.aspx" Text="Об'єкти приватизації"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFDogContinue.aspx" Text="Продовження договорів"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<p><asp:Label runat="server" ID="ASPxLabel19" Text="Зміна балансоутримувачів об'єктів" CssClass="reporttitle"/></p>

<dx:ASPxGridView ID="PrimaryGridView" ClientInstanceName="PrimaryGridView" runat="server" AutoGenerateColumns="False" KeyFieldName="request_id" Width="100%" DataSourceID="SqlDataSourceConveyancingForConfirmation" >
    <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" />
    <Columns>    
        <dx:GridViewDataTextColumn FieldName="balans_id" ShowInCustomizationForm="False" Caption="Редагування">
            <DataItemTemplate>
                <%# "<center>" 
						+ "<a href=\"ConveyancingConfirmation.aspx?reqid=" + Eval("request_id") + "\"><img border='0' src='../Styles/EditIcon.png'/>" 
		                +  " "
				        + "<a title=\"Файли розпорядчих документів\" href=\"#\" onClick=\"showAttachDocuments(" + Eval("request_id") + ", 'rish')\"><img border='0' src='../Styles/PdfReportIcon.png'/></a>"
						+  " "
						+ "<a title=\"Файли акту\" href=\"#\" onClick=\"showAttachDocuments(" + Eval("request_id") + ", 'akt')\"><img border='0' src='../Styles/current_stage_pdf.png'/></a>"
						+ "</a></center>"%>
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
    <SettingsCookies CookiesID="GUKV.Reports1NF.ConveyancingList" Enabled="True" Version="A4" />
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

