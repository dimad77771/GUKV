<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFCorporprav.aspx.cs" Inherits="Reports1NF_Report1NFCorporprav"
    MasterPageFile="~/NoHeader.master" Title="Перелік корпоративних прав" %>

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

		PrivatisatGridView.SetHeight(window.innerHeight - 140);
		console.log("AAAa");
    }

    function GridViewFreeSquareInit(s, e) {

        PrivatisatGridView.PerformCallback("init:");
    }

    function GridViewFreeSquareEndCallback(s, e) {

        AdjustGridSizes();
	}

	function ShowFieldChooserPopupControl(s, e) {

        PrimaryGridView = PrivatisatGridView;
		PopupFieldChooser.Show();
	}


	function ShowPhoto(s, e) {
		console.log(e.buttonID);
		if (e.buttonID == 'btnPdfBuild') {
			PrivatisatGridView.GetRowValues(e.visibleIndex, 'id', OnGridPdfBuildGetRowValues);
		} else if (e.buttonID == 'btnJpegBuild') {
			PrivatisatGridView.GetRowValues(e.visibleIndex, 'id', OnGridJpegBuildGetRowValues);
		} else if (e.buttonID == 'bnt_current_stage_pdf') {
			$.cookie('RecordID', s.GetRowKey(e.visibleIndex));
			ASPxFileManagerPhotoFiles.Refresh();
			PopupObjectPhotos.Show();
		} else if (e.buttonID == 'btnMapShow') {
			PrivatisatGridView.GetRowValues(e.visibleIndex, 'id', OnMapShowGetRowValues);
		} else if (e.buttonID == 'btnFreeCycle') {
			PrivatisatGridView.GetRowValues(e.visibleIndex, 'id', OnFreeCycleGetRowValues);
		} else if (e.buttonID == 'btnOrgBalansObject') {
			PrivatisatGridView.GetRowValues(e.visibleIndex, 'id;balans_id;report_id', OnClickOrgBalansObject);
        } else if (e.buttonID == 'btnCopyFullDescription2') {
			var cols = "include_in_perelik;zal_balans_vartist;perv_balans_vartist;free_object_type_name;prop_srok_orands;punkt_metod_rozrahunok;invest_solution;";
			cols += "zgoda_control;district;street_name;addr_nomer;total_free_sqr;free_sql_usefull;";
			cols += "floor;condition;water;heating;gas;power_text;history;zgoda_renter;nomer_derzh_reestr_neruh;reenum_derzh_reestr_neruh;info_priznach_nouse;info_rahunok_postach;priznach_before;period_nouse;osoba_use_before"
			PrivatisatGridView.GetRowValues(e.visibleIndex, cols, OnCopyFullDescription2);
		} else if (e.buttonID == 'btnCopyFullDescription') {
			var cols = "id";
			PrivatisatGridView.GetRowValues(e.visibleIndex, cols, OnCopyFullDescription);
		}
    }

    function onZkpoCodeChanged(s, e) {
        //console.log("s", s);
		//console.log("e", e);
		//grid.GetEditor("org_name").PerformCallback(s.GetValue());
		var editor = PrivatisatGridView.GetEditor("org_info_id");
		var table = $(editor.inputElement).parents("#MainContent_PrivatisatGridView_DXEditingRow");
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
		//for (var i = 0; i < headers.length; i++) {
		//	var vv = values[i];
		//	if (vv === null) {
		//		vv = "";
		//	} else if (vv === true) {
		//		vv = "так";
		//	} else if (vv === false) {
		//		vv = "ні";
		//	}

		//	txt += (i == 0 ? "" : "\n") + headers[i] + vv;
		//}

		//var id = values[values.length - 1];
		var id = values;
		txt += "Фото - http://eis.gukv.gov.ua/gukv/Reports1NF/BalansPrivatisatPhotosPdf.aspx?id=" + id + '&jpeg=1';

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

	function OnCopyFullDescription2(values) {
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

	function OnGridJpegBuildGetRowValues(values) {
		console.log(values);
		var id = values;
		window.open(
			'BalansPrivatisatPhotosPdf.aspx?id=' + id + '&jpeg=1',
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

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgOwnership" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_1nf_org_ownership where len(name) > 0 order by name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourcePrivatisat" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
SELECT 
*
FROM [corporprav] A
"
OnSelecting="SqlDataSourcePrivatisat_Selecting"

DeleteCommand="DELETE FROM [corporprav] WHERE id = @id" 

UpdateCommand="UPDATE [corporprav]
SET
	[uridname] = @uridname
	,[uridzkpo] = @uridzkpo
	,[vlasname] = @vlasname
	,[vlasicod] = @vlasicod
	,[vlaszkpo] = @vlaszkpo
	,[vlaskopfg] = @vlaskopfg
	,[vlasaddr] = @vlasaddr
	,[isvlas] = @isvlas
	,[akckol] = @akckol
	,[akcvart] = @akcvart
	,[akcpart] = @akcpart
	,[akcform] = @akcform
	,[ownershipvid] = @ownershipvid
	,[nomvart] = @nomvart
	,[stanobj] = @stanobj
	,[modify_date2] = @modify_date2
	,[modified_by2] = @modified_by2
WHERE id = @id" 
	onupdating="SqlDataSourcePrivatisat_Updating"

    InsertCommand="INSERT INTO [corporprav]
    ([uridname]
	,[uridzkpo]
	,[vlasname]
	,[vlasicod]
	,[vlaszkpo]
	,[vlaskopfg]
	,[vlasaddr]
	,[isvlas]
	,[akckol]
	,[akcvart]
	,[akcpart]
	,[akcform]
	,[ownershipvid]
	,[nomvart]
	,[stanobj]
	,[modify_date2]
	,[modified_by2]
    ) 
    VALUES
    (@uridname
	,@uridzkpo
	,@vlasname
	,@vlasicod
	,@vlaszkpo
	,@vlaskopfg
	,@vlasaddr
	,@isvlas
	,@akckol
	,@akcvart
	,@akcpart
	,@akcform
	,@ownershipvid
	,@nomvart
	,@stanobj
	,@modify_date2
	,@modified_by2
    );
SELECT SCOPE_IDENTITY()" 
    oninserting="SqlDataSourcePrivatisat_Inserting" 

	>
    <SelectParameters>
		<asp:Parameter DbType="String" DefaultValue="" Name="baseurl" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<textarea rows="2" cols="2" id="inpit-for-copy-clipboard" style="display:none2;width:1px;height:1px;position:absolute;top:1px;right:1px;z-index:-1" ></textarea>

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
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Перелік корпоративних прав" CssClass="reporttitle"></asp:Label>
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

            <dx:ASPxButton ID="ASPxButton_FreeSquare_SaveAs" runat="server" AutoPostBack="False" visible="true"
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
    FileName="Корпоративні права" GridViewID="PrivatisatGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

   <dx:ASPxGridView ID="PrivatisatGridView" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourcePrivatisat" KeyFieldName="id" Width="100%" 
        ClientInstanceName="PrivatisatGridView" 
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
            ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowEditButton="true" ShowNewButton="true" >
            <CustomButtons>
                <%--<dx:GridViewCommandColumnCustomButton ID="btnPdfBuild" Text="Pdf"> 
					<Image Url="~/Styles/PdfReportIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnJpegBuild" Text="Jpeg" Visibility="Invisible"> 
					<Image Url="~/Styles/PhotoIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnMapShow" Text="Показати на мапі"> 
					<Image Url="~/Styles/MapShowIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
				<dx:GridViewCommandColumnCustomButton ID="btnCopyFullDescription" Text="Опис об'єкта до буфера обміну"> 
					<Image Url="~/Styles/CopyIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>--%>
            </CustomButtons>
            <CellStyle Wrap="False"></CellStyle>
        </dx:GridViewCommandColumn>

        <dx:GridViewDataTextColumn FieldName="uridname" Caption="Назва юр. особи" Width="160px" CellStyle-HorizontalAlign="Left">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="uridzkpo" Caption="Код ЕДРПОУ юр. особи" Width="90px" CellStyle-HorizontalAlign="Center">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="vlasname" Caption="Назва / ФІО власника корпправа" Width="150px" CellStyle-HorizontalAlign="Left">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="vlasicod" Caption="ІНН власника корпправа" Width="90px" CellStyle-HorizontalAlign="Center">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="vlaszkpo" Caption="ЕДРПОУ власника корпправ" Width="90px" CellStyle-HorizontalAlign="Center">
        </dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="vlaskopfg" Caption="КОПФГ власника корп.права" Width="80px" CellStyle-HorizontalAlign="Center">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="vlasaddr" Caption="Юр. Адреса власника корп.права" Width="150px" CellStyle-HorizontalAlign="Left">
        </dx:GridViewDataTextColumn>

		<dx:GridViewDataCheckColumn FieldName="isvlas" Caption="Є засно-вником" Width="50px" CellStyle-HorizontalAlign="Center">
        </dx:GridViewDataCheckColumn>


        <dx:GridViewDataSpinEditColumn FieldName="akckol" Caption="Кількість акцій у власника, шт." Width="100px">
            <PropertiesSpinEdit NumberType="Float" NumberFormat="Number">
                <SpinButtons Enabled="false" ClientVisible="false" ></SpinButtons>
            </PropertiesSpinEdit>
        </dx:GridViewDataSpinEditColumn>

        <dx:GridViewDataSpinEditColumn FieldName="akcvart" Caption="Частка власника, грн." Width="100px">
            <PropertiesSpinEdit NumberType="Float" NumberFormat="Number">
                <SpinButtons Enabled="false" ClientVisible="false" ></SpinButtons>
            </PropertiesSpinEdit>
        </dx:GridViewDataSpinEditColumn>

        <dx:GridViewDataSpinEditColumn FieldName="akcpart" Caption="Частка власника, %" Width="70px">
            <PropertiesSpinEdit NumberType="Float" NumberFormat="Number">
                <SpinButtons Enabled="false" ClientVisible="false" ></SpinButtons>
            </PropertiesSpinEdit>
        </dx:GridViewDataSpinEditColumn>

		<dx:GridViewDataTextColumn FieldName="akcform" Caption="Форма існування акцій" Width="120px" CellStyle-HorizontalAlign="Left">
        </dx:GridViewDataTextColumn>

		<dx:GridViewDataComboBoxColumn FieldName="ownershipvid" Caption="Форма власності" Width="140px" CellStyle-HorizontalAlign="Left">
            <PropertiesComboBox DataSourceID="SqlDataSourceDictOrgOwnership"
                                ValueType="System.String"
                                ValueField="name"
                                TextField="name" />
        </dx:GridViewDataComboBoxColumn>


        <dx:GridViewDataSpinEditColumn FieldName="nomvart" Caption="Номінальна вартість акцій, грн" Width="100px">
            <PropertiesSpinEdit NumberType="Float" NumberFormat="Number">
                <SpinButtons Enabled="false" ClientVisible="false" ></SpinButtons>
            </PropertiesSpinEdit>
        </dx:GridViewDataSpinEditColumn>


		<dx:GridViewDataTextColumn FieldName="stanobj" Caption="Стан реєстрації об'єкту корпоративної власності" Width="120px" CellStyle-HorizontalAlign="Left">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="modify_date2" Caption="Дата редагу-вання"  Width="75px">
			<HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("modify_date2") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="modified_by2" Caption="Користувач"  Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("modified_by2") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

    </Columns>

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
    <SettingsCookies CookiesID="GUKV.Reports1NF.CorporpravSquare" Version="A1_17" Enabled="false" />
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
