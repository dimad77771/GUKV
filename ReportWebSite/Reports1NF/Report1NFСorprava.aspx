﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFСorprava.aspx.cs" Inherits="Reports1NF_Report1NFСorprava"
    MasterPageFile="~/NoHeader.master" Title="Перелік об'єктів приватизації" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register src="../UserControls/FieldFixxer.ascx" tagname="FieldFixxer" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<style>
    .command-column-class {
        white-space:normal !important;
    }

    a.dxbButton_DevEx {
        margin:0px !important;
    }
</style>

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function AdjustGridSizes() {

		СorpravaGridView.SetHeight(window.innerHeight - 180);
		console.log("AAAa");
    }

    function GridViewFreeSquareInit(s, e) {

        СorpravaGridView.PerformCallback("init:");
    }

    function GridViewFreeSquareEndCallback(s, e) {

        AdjustGridSizes();
	}

	function ShowFieldChooserPopupControl(s, e) {

        PrimaryGridView = СorpravaGridView;
		PopupFieldChooser.Show();
	}


	function ShowPhoto(s, e) {
		console.log(e.buttonID);
		if (e.buttonID == 'btnPdfBuild') {
			СorpravaGridView.GetRowValues(e.visibleIndex, 'id', OnGridPdfBuildGetRowValues);
		} else if (e.buttonID == 'bnt_current_stage_pdf') {
			$.cookie('RecordID', s.GetRowKey(e.visibleIndex));
			ASPxFileManagerPhotoFiles.Refresh();
			PopupObjectPhotos.Show();
		} else if (e.buttonID == 'btnMapShow') {
			СorpravaGridView.GetRowValues(e.visibleIndex, 'id', OnMapShowGetRowValues);
		} else if (e.buttonID == 'btnFreeCycle') {
			СorpravaGridView.GetRowValues(e.visibleIndex, 'id', OnFreeCycleGetRowValues);
		} else if (e.buttonID == 'btnOrgBalansObject') {
			СorpravaGridView.GetRowValues(e.visibleIndex, 'id;balans_id;report_id', OnClickOrgBalansObject);
        } else if (e.buttonID == 'btnCopyFullDescription') {
			var cols = "include_in_perelik;zal_balans_vartist;perv_balans_vartist;free_object_type_name;prop_srok_orands;punkt_metod_rozrahunok;invest_solution;";
			cols += "zgoda_control;district;street_name;addr_nomer;total_free_sqr;free_sql_usefull;";
			cols += "floor;condition;water;heating;gas;power_text;history;zgoda_renter;nomer_derzh_reestr_neruh;reenum_derzh_reestr_neruh;info_priznach_nouse;info_rahunok_postach;priznach_before;period_nouse;osoba_use_before"
			СorpravaGridView.GetRowValues(e.visibleIndex, cols, OnCopyFullDescription);
		}
    }

    function onZkpoCodeChanged(s, e) {
        //console.log("s", s);
		//console.log("e", e);
		//grid.GetEditor("org_name").PerformCallback(s.GetValue());
		var editor = СorpravaGridView.GetEditor("org_info_id");
		var table = $(editor.inputElement).parents("#MainContent_СorpravaGridView_DXEditingRow");
        var child = table.find("td.dxgv").eq(4).children("span");
        console.log("editor", editor);
        console.log("table", table);
        console.log("child", child);
        var text = editor.GetText();
        text = text.substring(text.indexOf("-") + 2);
        console.log("text", text);
		child.text(text);
	}

	function OnCopyFullDescription(values) {
        var headers = [
            "Включено до переліку № - ",
            "Залишкова балансова вартість – ",
            "Первісна балансова вартість - ",
            "Тип об’єкта - ",
            "Пропонований строк оренди (у роках) – ",
            "Пункт Методики розрахунку орендної плати (якщо об’єкт пропонується для включення до Переліку другого типу) - ",
            "Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації - ",
                
            "Погодження органу управління балансоутримувача – ",
            "Район – ",
            "Назва Вулиці - ",
            "Номер Будинку - ",
            "Загальна площа об’єкта - ",
			"Корисна площа об’єкта – ",
			"Характеристика об’єкта оренди(будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі(надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів) – ",
			"Технічний стан – ",
			"Водопостачання – ",
			"Теплопостачання – ",
			"Газопостачання – ",
			"Електропостачання – ", 
			"Пам’ятка культурної спадщини - ",
			"Погодження органу охорони культурної спадщини - ",
			"Номер запису про право власності у Реєстрація у Державному реєстрі речових прав на нерухоме майно – ",
			"Реєстраційний номер об'єкту нерухомого майна у Реєстрація у Державному реєстрі речових прав на нерухоме майно – ",
			"Інформація про цільове призначення об’єкта оренди – ",
			"Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг - ",
			"Цільове призначення об’єкта, за яким об’єкт використовувався перед тим, як він став вакантним – ",
			"Період часу, протягом якого об’єкт не використовується – ",
			"Інформацію про особу, яка використовувала об’єкт перед тим, як він став вакантним – ",
        ];

		console.log("values", values);

        var txt = "";
        for (var i = 0; i < headers.length; i++) {
            var vv = values[i];
			if (vv === null) {
                vv = "";
            } else if (vv === true) {
                vv = "так";
			} else if (vv === false) {
				vv = "ні";
            }

			txt += (i == 0 ? "" : "\n") + headers[i] +  vv;
		}
        
        //console.log("txt", txt);

        $("#inpit-for-copy-clipboard").val(txt);
        $("#inpit-for-copy-clipboard").select();
        document.execCommand("copy");
        return;

        navigator.clipboard.writeText(txt).then(function () {
			alert("Опис скопійовано в буфер обміну");
		}, function () {
		    alert("Не можу записати буфер обміну");
		});
	}


	function OnMapShowGetRowValues(values) {
		var id = values;
        window.open(
			'Report1NFPrivatisatMap.aspx?fs_id=' + id,
			'_blank',
		);
	}

	function OnFreeCycleGetRowValues(values) {
		var id = values;
		window.open(
			'FreeCycle.aspx?free_square_id=' + id,
			'_blank',
		);
    }

    function OnClickOrgBalansObject(values) {
		window.location = 'OrgBalansObject.aspx?rid=' + values[2] + '&bid=' + values[1] + '&edit_free_square_id=' + values[0];
	}


	function OnGridPdfBuildGetRowValues(values) {
		console.log(values);
		var id = values;
		window.open(
			'BalansPrivatisatPhotosPdf.aspx?id=' + id,
			'_blank',
		);
	}

    function OnDropDown(comboBox){
		//SetDropDownWidth(comboBox, "368px");
		_aspxMakeScollableArea(comboBox);
	}

    function SetDropDownWidth(comboBox, width){
        var listBox = comboBox.GetListBoxControl();
        var scrollDiv = listBox.GetScrollDivElement();
        //scrollDiv.style.overflowX = "auto";
        //scrollDiv.style.width = width;

		scrollDiv.style.overflowY = "scroll";
		console.log("scrollDiv.style.overflowY", scrollDiv.style.overflowY);

        var popupControl = comboBox.GetPopupControl();
        //popupControl.SetSize("0", "0");
	}


    function _aspxMakeScollableArea(comboBox) {  
        var listBox = comboBox.GetListBoxControl();  
        if (_aspxIsExists(listBox)) {  
            var lsScrollableDiv =  listBox.GetScrollDivElement();  
            if (_aspxIsExists(lsScrollableDiv)) {  
				var browserWidth = (_aspxGetDocumentClientWidth() - 10) + 'px';
				//alert(browserWidth);
                _aspxSetAttribute(lsScrollableDiv.style, "width", browserWidth);  
				_aspxSetAttribute(lsScrollableDiv.style, "overflow-x", "scroll");  
				_aspxSetAttribute(lsScrollableDiv.style, "overflow-y", "scroll");
            }  
        }  
    }

    // ]]>

</script>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceСorprava" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
SELECT 
      (select Q.full_name from reports1nf_org_info Q where Q.report_id = A.report_id) as rep_full_name,
      (select Q.zkpo_code from reports1nf_org_info Q where Q.report_id = A.report_id) as rep_zkpo_code,
      *
  FROM [reports1nf_org_corprava] A
    order by 1,2
"
    OnSelecting="SqlDataSourceСorprava_Selecting"

DeleteCommand="DELETE FROM [privatisat] WHERE id = @id" 

UpdateCommand="UPDATE [privatisat]
SET
    [privat_year] = @privat_year
    ,[kmr_number] = @kmr_number
    ,[kmr_date] = @kmr_date
    ,[org_info_id] = @org_info_id
    ,[organizator] = @organizator
    ,[obj_name] = @obj_name
    ,[addr_street_id] = @addr_street_id
    ,[addr_distr_new_id] = @addr_distr_new_id
    ,[addr_nomer] = @addr_nomer
    ,[geodata_map_points] = @geodata_map_points
    ,[orendar] = @orendar
    ,[total_free_sqr] = @total_free_sqr
    ,[sposib_privat] = @sposib_privat
    ,[document_privat] = @document_privat
    ,[obj_price] = @obj_price
    ,[buyer_name] = @buyer_name
    ,[buyer_adr_street] = @buyer_adr_street
    ,[buyer_adr_nomer] = @buyer_adr_nomer
    ,[prozoro_number] = @prozoro_number
    ,[modify_date2] = @modify_date2
    ,[modified_by2] = @modified_by2
WHERE id = @id" 
	onupdating="SqlDataSourceСorprava_Updating"

    InsertCommand="INSERT INTO [privatisat]
    ([privat_year]
      ,[kmr_number]
      ,[kmr_date]
      ,[org_info_id]
      ,[organizator]
      ,[obj_name]
      ,[addr_street_id]
      ,[addr_distr_new_id]
      ,[addr_nomer]
      ,[geodata_map_points]
      ,[orendar]
      ,[total_free_sqr]
      ,[sposib_privat]
      ,[document_privat]
      ,[obj_price]
      ,[buyer_name]
      ,[buyer_adr_street]
      ,[buyer_adr_nomer]
      ,[prozoro_number]
      ,[modify_date2]
      ,[modified_by2]
    ) 
    VALUES
    (@privat_year
    ,@kmr_number
    ,@kmr_date
    ,@org_info_id
    ,@organizator
    ,@obj_name
    ,@addr_street_id
    ,@addr_distr_new_id
    ,@addr_nomer
    ,@geodata_map_points
    ,@orendar
    ,@total_free_sqr
    ,@sposib_privat
    ,@document_privat
    ,@obj_price
    ,@buyer_name
    ,@buyer_adr_street
    ,@buyer_adr_nomer
    ,@prozoro_number
    ,@modify_date2
    ,@modified_by2
    );
SELECT SCOPE_IDENTITY()" 
    oninserting="SqlDataSourceСorprava_Inserting" 

	>
    <SelectParameters>
		<asp:Parameter DbType="String" DefaultValue="" Name="baseurl" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgInfo" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select report_id, zkpo_code, zkpo_code + ' - ' + isnull(short_name,'') as nam from reports1nf_org_info order by zkpo_code">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_districts2 where id < 400 ORDER BY name">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceStreet" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_streets where (name is not null) and (RTRIM(LTRIM(name)) <> '') order by name">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceFreecycleStepDict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT step_id, step_name, step_cod, step_ord, istitle FROM freecycle_step_dict union select null, '<пусто>', '00', -1, 0 ORDER BY step_ord">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceUsingPossible" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, left(full_name, 150) as name, rental_rate, 1 as ordrow FROM dict_rental_rate union select null, '<пусто>', null, 2 as ordrow ORDER BY ordrow, name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceIncludeInPerelik" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT '1' id, '1' name, 1 as ordrow union SELECT '2' id, '2' name, 1 as ordrow union select null, '',  2 as ordrow ORDER BY ordrow, name">
</mini:ProfiledSqlDataSource>

<textarea rows="2" cols="2" id="inpit-for-copy-clipboard" style="display:none2;width:1px;height:1px;position:absolute;top:1px;right:1px;z-index:-1" ></textarea>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
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

<dx:ASPxMenu ID="SectionMenuForRDARole" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFFreeSquare.aspx" Text="Перелік вільних приміщень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFPrivatisatSquare.aspx" Text="Об'єкти приватизації"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFDogContinue.aspx" Text="Продовження договорів"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Реєстр корпоративних прав" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px" Visible="false">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
			<dx:ASPxPopupControl
				ID="ASPxPopupControl_FreeSquare_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_FreeSquare_SaveAs" 
                PopupElementID="ASPxButton_FreeSquare_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_FreeSquare_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_FreeSquare_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_FreeSquare_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_FreeSquare_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>

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
								<asp:Parameter DefaultValue="privatisat_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
								<asp:Parameter Name="id" Type="String" />
							</DeleteParameters>
							<InsertParameters>
								<asp:Parameter DefaultValue="privatisat_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
								<asp:Parameter Name="Name" Type="String" />
								<asp:Parameter Name="Image" Type="Object" />
							</InsertParameters>
							<SelectParameters>
								<asp:Parameter DefaultValue="privatisat_documents" Name="scope" Type="String" />
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

						<dx:ASPxButton ID="ASPxButtonClose" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
							<ClientSideEvents Click="function(s, e) { PopupObjectPhotos.Hide(); }" />
						</dx:ASPxButton>

					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewFreeSquareExporter" runat="server" 
    FileName="Приватизація" GridViewID="СorpravaGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

   <dx:ASPxGridView ID="СorpravaGridView" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceСorprava" KeyFieldName="id" Width="100%" 
        ClientInstanceName="СorpravaGridView" 
        OnCustomCallback="GridViewFreeSquare_CustomCallback"
        OnCustomFilterExpressionDisplayText="GridViewFreeSquare_CustomFilterExpressionDisplayText"
        OnProcessColumnAutoFilter="GridViewFreeSquare_ProcessColumnAutoFilter" >
	   <ClientSideEvents CustomButtonClick="ShowPhoto" />

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
        <dx:GridViewCommandColumn Width="70px" ButtonType="Image" CellStyle-Wrap="True" FixedStyle="Left" CellStyle-CssClass="command-column-class" 
            ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowEditButton="true" ShowNewButton="true" Visible="false" >
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="btnPdfBuild" Text="Pdf"> 
					<Image Url="~/Styles/PdfReportIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnMapShow" Text="Показати на мапі"> 
					<Image Url="~/Styles/MapShowIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <CellStyle Wrap="False"></CellStyle>
        </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn FieldName="id" Caption="ID" VisibleIndex="1"  ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="report_id" Caption="report_id" VisibleIndex="2" ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="primitki" Caption="primitki" VisibleIndex="2" ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>


            <dx:GridViewDataTextColumn FieldName="rep_full_name" Caption="Назва юр. особи" VisibleIndex="3" Width="250" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="rep_zkpo_code" Caption="ЕДРПОУ юр.особи" VisibleIndex="3" Width="70" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>


            <dx:GridViewDataTextColumn FieldName="vlasn_name" Caption="Назва/ФІО власника корпправа" VisibleIndex="3" Width="250" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="vlasn_zkpo" Caption="ЕДРПОУ/ІНН власника корпправа" VisibleIndex="3" Width="85" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="vlasn_kopfg" Caption="КОПФГ власника корпправа" VisibleIndex="3" Width="70" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="vlasn_uradr" Caption="Юр. Адреса власника корпправа" VisibleIndex="3" Width="250" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataCheckColumn FieldName="is_zasnobn" Caption="Є засновником" VisibleIndex="3" Width="40">
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataCheckColumn>

            <dx:GridViewDataTextColumn FieldName="kol_akcia" Caption="Кількість акцій у власника, шт." VisibleIndex="3" Width="100" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="total_price" Caption="Частка власника, грн." VisibleIndex="3" Width="100" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="part_price" Caption="Частка власника, %" VisibleIndex="3" Width="80" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="stat_found" Caption="СК юр. особи, грн." VisibleIndex="3" Width="100" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="form_akcia" Caption="Форма існування акцій" VisibleIndex="3" Width="100" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="nominal_vart" Caption="Номінальна вартість акцій, грн." VisibleIndex="3" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="stan_vlasn" Caption="Стан реєстрації об'єкту корпоративної власності" VisibleIndex="3" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>


    </Columns>

<%--    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="total_free_sqr" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="free_sql_usefull" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>--%>

    <SettingsBehavior ConfirmDelete="True" />
    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager AlwaysShowPager="true" PageSize="25"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="True"
        ShowFilterBar="Visible"
        ShowHeaderFilterButton="True"
        HorizontalScrollBarMode="Auto"
        ShowFooter="false"
        VerticalScrollBarMode="Auto"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.Reports1NF.Сorprava" Version="A1_18" Enabled="true" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewFreeSquareInit" EndCallback="GridViewFreeSquareEndCallback" />
</dx:ASPxGridView>

<dx:ASPxPopupControl ID="PopupFieldChooser" runat="server" 
    HeaderText="Додаткові Колонки" 
    ClientInstanceName="PopupFieldChooser" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" >
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>


</asp:Content>
