﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFDogContinue.aspx.cs" Inherits="Reports1NF_Report1NFDogContinue"
    MasterPageFile="~/NoHeader.master" Title="Перелік вільних приміщень" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register src="../UserControls/FieldFixxer.ascx" tagname="FieldFixxer" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<style>
    .command-column-class {
        white-space:normal !important;
    }
</style>

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function AdjustGridSizes() {
		FreeSquareGridView.SetHeight(window.innerHeight - 180);
    }

    function GridViewFreeSquareInit(s, e) {

        FreeSquareGridView.PerformCallback("init:");
    }

    function GridViewFreeSquareEndCallback(s, e) {

        AdjustGridSizes();
	}

	function ShowFieldChooserPopupControl(s, e) {

        PrimaryGridView = FreeSquareGridView;
		PopupFieldChooser.Show();
	}


	function ShowPhoto(s, e) {
		console.log(e.buttonID);
		if (e.buttonID == 'btnPdfBuild') {
			FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnGridPdfBuildGetRowValues);
		} else if (e.buttonID == 'bnt_current_stage_pdf') {
			$.cookie('RecordID', s.GetRowKey(e.visibleIndex));
			ASPxFileManagerPhotoFiles.Refresh();
			PopupObjectPhotos.Show();
		} else if (e.buttonID == 'btnMapShow') {
			FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnMapShowGetRowValues);
		} else if (e.buttonID == 'btnFreeCycle') {
			FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnFreeCycleGetRowValues);
		} else if (e.buttonID == 'btnOrgBalansObject') {
			FreeSquareGridView.GetRowValues(e.visibleIndex, 'id;arenda_id;report_id', OnClickOrgBalansObject);
        } else if (e.buttonID == 'btnCopyFullDescription') {
            var cols = "orendar_name;orendar_zkpo;org_name;zkpo_code;balanutr_addr_street;balanutr_addr_nomer;giver_name;giver_zkpo;giver_addr_street;giver_addr_nomer;agreement_date;rent_finish_date;srok_dog;";
            cols += "include_in_perelik;zal_balans_vartist;perv_balans_vartist;free_object_type_name;prop_srok_orands;punkt_metod_rozrahunok;invest_solution;";
			cols += "zgoda_control;district;street_name;addr_nomer;total_free_sqr;free_sql_usefull;";
			cols += "floor;condition;water;heating;gas;power_text;history;zgoda_renter;nomer_derzh_reestr_neruh;reenum_derzh_reestr_neruh;info_priznach_nouse;info_rahunok_postach;orend_plat_borg;stanom_na;";
            cols += "has_perevazh_pravo;polipshanya_vartist;polipshanya_finish_date";
			FreeSquareGridView.GetRowValues(e.visibleIndex, cols, OnCopyFullDescription);
		}
    }

	function OnCopyFullDescription(values) {
        var headers = [
			"Найменування орендаря - ",
			"Код ЕДРПОУ орендаря - ",
			"Найменування балансоутримувача - ",
			"Код ЕДРПОУ балансоутримувача - ",
			"Адреса балансоутримувача(вулиця) - ",
			"Адреса балансоутримувача(номер дому) - ",
			"Найменування орендодавця - ",
			"Код ЕДРПОУ орендодавця - ",
			"Адреса орендодавця(вулиця) - ",
			"Адреса орендодавця(номер дому) - ",
			"Дата укладання договору - ",
			"Дата закінчення договору - ",
			"Строк оренди(роки) - ",

            "Включено до переліку № - ",
            "Залишкова балансова вартість, тис. грн. – ",
            "Первісна балансова вартість, тис. грн. - ",
            "Тип об’єкта - ",
            "Пропонований строк оренди (у роках) – ",
            "Пункт Методики розрахунку орендної плати (якщо об’єкт пропонується для включення до Переліку другого типу) - ",
            "Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації - ",
                
            "Погодження органу управління балансоутримувача – ",
            "Район – ",
            "Назва Вулиці - ",
            "Номер Будинку - ",
            "Загальна площа об’єкта, кв.м - ",
			"Корисна площа об’єкта, кв.м – ",
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
			"Заборгованість по орендній платі, грн. (без ПДВ) – ",
			"Станом на – ",

			"Має переважне право на продовження – ",
			"Вартість здійснених чинним орендарем невід’ємних поліпшень – ",
			"Дата завершення здійснених чинним орендарем невід’ємних поліпшень – ",
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
            } else if (Object.prototype.toString.call(vv) === '[object Date]') {
				vv = formatDate(vv);
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

	function formatDate(date) {
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();

        return (day < 10 ? "0" : "") + day + "." + (month < 10 ? "0" : "") + month + "." + year;
	}


	function OnMapShowGetRowValues(values) {
		var id = values;
		window.open(
			'Report1NFProdlenMap.aspx?fs_id=' + id,
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
			'BalansDogContinuePhotosPdf.aspx?id=' + id,
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

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquare" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
	fs.komis_protocol,
    fs.prozoro_number,
	fs.using_possible_id,
	fs.geodata_map_points,
	fs.include_in_perelik,
	fs.current_stage_id,
	fs.freecycle_step_dict_id,
	fs.current_stage_docdate,
	fs.current_stage_docnum,
	fs.modify_date2,
	fs.modified_by2,
    fs.zal_balans_vartist,
    fs.perv_balans_vartist,
    fs.punkt_metod_rozrahunok,
    fs.prop_srok_orands,
    fs.nomer_derzh_reestr_neruh,
    fs.reenum_derzh_reestr_neruh,
    fs.info_priznach_nouse,
    fs.info_rahunok_postach,
    fs.priznach_before,
    fs.orend_plat_borg,
    fs.period_nouse,
    fs.stanom_na,
    fs.osoba_use_before,
 row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr) as npp     
,fs.id
,fs.arenda_id
,org.short_name as org_name
,org.zkpo_code
,org.report_id
,org.director_title as vidpov_osoba
,(select Q.name from dict_streets Q where Q.id = org.addr_street_id) as balanutr_addr_street
,org.addr_nomer as balanutr_addr_nomer

,b.district
,b.street_full_name as street_name
,(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)), '')) as addr_nomer

,(select q.name from dict_1nf_tech_stane q where q.id = fs.free_sqr_condition_id) as condition 
,b.object_type 
,b.object_kind
,b.sqr_total
,b.sqr_for_rent

,fs.total_free_sqr 
--,null as free_sql_usefull
--,null as mzk
--,rfs.sqr_free_korysna as free_sql_usefull
--,rfs.sqr_free_mzk as mzk
,fs.free_sqr_korysna as free_sql_usefull

,fs.is_included
,fs.floor
,fs.water
,fs.heating
,(select q.name from dict_1nf_power_info q where q.id = fs.power_info_id) as power_text
,fs.gas

--,(select left(qq.full_name, 150) as name from view_dict_rental_rate qq where qq.id = fs.using_possible_id) as possible_using
,fs.possible_using

,(select qq.name from dict_free_object_type qq where qq.id = fs.free_object_type_id) as free_object_type_name


,fs.modify_date
,fs.note

,invest_solution = (select qq.name from dict_1nf_invest_solution qq where qq.id = fs.invest_solution_id)
--, solution = fs.is_solution

, fs.initiator
, zg2.name as zgoda_control
, zg.name as zgoda_renter

,st.kind
,rep.form_of_ownership
,rep.old_organ

--,b.object_kind as vydbudynku
,history = case when isnull(b.history, 'НІ') = 'НІ' then '' else 'ТАК' end 
, isnull(ddd.name, 'Невизначені') as sf_upr
, @baseurl + '/Reports1NF/BalansFreeSquarePhotosPdf.aspx?id=' + cast(fs.id as varchar(100)) as pdfurl
, case when exists (select 1 from reports1nf_arenda_dogcontinue_photos qq where qq.free_square_id = fs.id) then 1 else 0 end as isexistsphoto

,org_renter.zkpo_code as orendar_zkpo
,org_renter.full_name as orendar_name

,org_giver.zkpo_code as giver_zkpo
,org_giver.full_name as giver_name
,(select Q.name from dict_streets Q where Q.id = org_giver.addr_street_id) as giver_addr_street
,org_giver.addr_nomer as giver_addr_nomer

,bal.agreement_date
,bal.rent_finish_date
,cast(round(DATEDIFF ( month, bal.agreement_date, bal.rent_finish_date ) / 12.0, 0) as int) as srok_dog

,fs.orend_plat_last_month
,fs.has_perevazh_pravo
,fs.polipshanya_vartist
,fs.polipshanya_finish_date

FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
--left join (select * from dbo.reports1nf_arenda_dogcontinue where id = (select top 1 id from dbo.reports1nf_arenda_dogcontinue where arenda_id = bal.id)) fs on fs.arenda_id = bal.id
join reports1nf_org_info org on org.id = bal.org_balans_id
left join [dbo].[dict_streets] st on b.addr_street_id = st.id
left join dbo.dict_zgoda_renter zg on fs.zgoda_renter_id = zg.id
left join dbo.dict_zgoda_renter zg2 on fs.zgoda_control_id = zg2.id
left join organizations org_renter on org_renter.id = bal.org_renter_id
left outer join organizations org_giver ON org_giver.id = bal.org_giver_id and (org_giver.is_deleted is null or org_giver.is_deleted = 0)

--OUTER APPLY (SELECT TOP 1 * FROM rent_free_square rfs
--		WHERE rfs.building_id = bal.building_id AND
--		      rfs.organization_id = bal.organization_id order by rfs.rent_period_id DESC) rfs

LEFT JOIN (
			select obp.org_id
			, occ.name
			, occ.id
			, per.name as period 
			from org_by_period obp
			join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
			join dict_rent_occupation occ on occ.id = obp.org_occupation_id
				) DDD ON DDD.org_id = rep.organization_id

        WHERE (@p_rda_district_id = 0 OR (rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = @p_rda_district_id))

    order by org_name, street_name, addr_nomer, total_free_sqr   "
    OnSelecting="SqlDataSourceFreeSquare_Selecting"

UpdateCommand="UPDATE [reports1nf_arenda_dogcontinue]
SET
    [komis_protocol] = @komis_protocol,
	[geodata_map_points] = @geodata_map_points,
	[include_in_perelik] = @include_in_perelik,
	[freecycle_step_dict_id] = @freecycle_step_dict_id,
	[current_stage_docdate] = @current_stage_docdate,
	[current_stage_docnum] = @current_stage_docnum,
    [prozoro_number] = @prozoro_number,
	[is_included] = @is_included,
	[modify_date2] = @modify_date2,
	[modified_by2] = @modified_by2
WHERE id = @id" 
	onupdating="SqlDataSourceFreeSquare_Updating"
	>
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="period_year" />
		<asp:Parameter DbType="String" DefaultValue="" Name="baseurl" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name, cod, ord, istitle FROM dict_free_square_stage union select null, '<пусто>', '00', -1, 0 ORDER BY ord">
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
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Перелік вільних приміщень" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px">
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
				PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" RenderMode="Lightweight" 
				PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

						<asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server" 
							DeleteMethod="Delete" InsertMethod="Insert" 
							OnInserting="ObjectDataSourcePhotoFiles_Inserting" 
							SelectMethod="Select" 
							TypeName="ExtDataEntry.Models.FileAttachment">
							<DeleteParameters>
								<asp:Parameter DefaultValue="reports1nf_arenda_dogcontinue_current_stage_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
								<asp:Parameter Name="id" Type="String" />
							</DeleteParameters>
							<InsertParameters>
								<asp:Parameter DefaultValue="reports1nf_arenda_dogcontinue_current_stage_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
								<asp:Parameter Name="Name" Type="String" />
								<asp:Parameter Name="Image" Type="Object" />
							</InsertParameters>
							<SelectParameters>
								<asp:Parameter DefaultValue="reports1nf_arenda_dogcontinue_current_stage_documents" Name="scope" Type="String" />
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
    FileName="ВільніПлощі" GridViewID="FreeSquareGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

    <%--
<dx:ASPxGridView
    ID="PrimaryGridView"
    ClientInstanceName="PrimaryGridView"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceReports"
    KeyFieldName="report_id"
    OnCustomCallback="GridViewReports1NF_CustomCallback"
    OnCustomFilterExpressionDisplayText="GridViewReports1NF_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter="GridViewReports1NF_ProcessColumnAutoFilter" >
--%>
   <dx:ASPxGridView ID="FreeSquareGridView" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceFreeSquare" KeyFieldName="id" Width="100%" 
        ClientInstanceName="FreeSquareGridView" 
        OnCustomCallback="GridViewFreeSquare_CustomCallback"
        OnCustomFilterExpressionDisplayText="GridViewFreeSquare_CustomFilterExpressionDisplayText"
        OnProcessColumnAutoFilter="GridViewFreeSquare_ProcessColumnAutoFilter" >
	   <ClientSideEvents CustomButtonClick="ShowPhoto" />
    <Columns>

        <dx:GridViewCommandColumn VisibleIndex="0" Width="50px" ButtonType="Image" CellStyle-Wrap="True" FixedStyle="Left" CellStyle-CssClass="command-column-class" >
             <EditButton Visible="True">
				<Image Url="~/Styles/EditIcon.png" />
             </EditButton>
             <CancelButton>
				<Image Url="~/Styles/CancelIcon.png" />
            </CancelButton>
            <UpdateButton>
				<Image Url="~/Styles/SaveIcon.png" />
			</UpdateButton>
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="btnPdfBuild" Text="Pdf"> 
					<Image Url="~/Styles/PdfReportIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnMapShow" Text="Показати на мапі"> 
					<Image Url="~/Styles/MapShowIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnOrgBalansObject" Text="Змінити картку"  Visibility="Invisible"> 
					<Image Url="~/Styles/EditTextIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnFreeCycle" Text="Картка процесу передачі в оренду вільного приміщення" Visibility="Invisible"> 
					<Image Url="~/Styles/ReportDocument18.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnCopyFullDescription" Text="Опис об'єкта до буфера обміну"> 
					<Image Url="~/Styles/CopyIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <CellStyle Wrap="False"></CellStyle>
        </dx:GridViewCommandColumn>

        <dx:GridViewDataTextColumn FieldName="orendar_name" Caption="Найменування орендаря" VisibleIndex="0" Width="300px" ReadOnly="true">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("orendar_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="orendar_zkpo" Caption="Код ЕДРПОУ орендаря" VisibleIndex="0" Width="100px" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("orendar_zkpo") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn FieldName="org_name" Caption="Найменування балансоутримувача" VisibleIndex="0"  Width="300px" ReadOnly="true" >
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrgInfo(" + Eval("report_id") + ")\">" + Eval("org_name") + "</a>"%>
            </DataItemTemplate>
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("org_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="zkpo_code" Caption="Код ЕДРПОУ балансоутримувача" VisibleIndex="1" Width="100px" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zkpo_code") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="balanutr_addr_street" Caption="Адреса балансоутримувача (вулиця)" VisibleIndex="1" Width="120px" ReadOnly="true">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("balanutr_addr_street") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="balanutr_addr_nomer" Caption="Адреса балансоутримувача (номер дому)" VisibleIndex="1" Width="50px" ReadOnly="true" CellStyle-HorizontalAlign="Left">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("balanutr_addr_nomer") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="giver_name" Caption="Найменування орендодавця" VisibleIndex="1" Width="300px" ReadOnly="true">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("giver_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="giver_zkpo" Caption="Код ЕДРПОУ орендодавця" VisibleIndex="1" Width="100px" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("giver_zkpo") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="giver_addr_street" Caption="Адреса орендодавця (вулиця)" VisibleIndex="1" Width="120px" ReadOnly="true">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("giver_addr_street") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="giver_addr_nomer" Caption="Адреса орендодавця (номер дому)" VisibleIndex="1" Width="50px" ReadOnly="true" CellStyle-HorizontalAlign="Left">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("giver_addr_nomer") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataColumn FieldName="agreement_date" Caption="Дата укладання договору" VisibleIndex="1" Width="80px" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
                <dx:ASPxLabel runat="server" Text='<%# Eval("agreement_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataColumn>

        <dx:GridViewDataColumn FieldName="rent_finish_date" Caption="Дата закінчення договору" VisibleIndex="1" Width="80px" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
                <dx:ASPxLabel runat="server" Text='<%# Eval("rent_finish_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataColumn>

        <dx:GridViewDataTextColumn FieldName="srok_dog" Caption="Строк оренди (роки)" VisibleIndex="1" Width="50px" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("srok_dog") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>


		<dx:GridViewDataCheckColumn FieldName="isexistsphoto" Caption="Наявність фото" VisibleIndex="1" Width="30px" ReadOnly="true">
		</dx:GridViewDataCheckColumn>
        <dx:GridViewDataTextColumn FieldName="district" Caption="Район" VisibleIndex="2" Width="120px" ReadOnly="true">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("district") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_name" Caption="Назва Вулиці" VisibleIndex="3" Width="150px" ReadOnly="true" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("street_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" Caption="Номер Будинку" VisibleIndex="4" Width="80px" ReadOnly="true" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("addr_nomer") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataCheckColumn FieldName="is_included" Caption="Включено до переліку вільних приміщень" VisibleIndex="4" Width ="40px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataCheckColumn>


        <dx:GridViewDataTextColumn FieldName="id" Caption="Реєстра-ційний №" VisibleIndex="4" Width ="60px"  >
			<CellStyle HorizontalAlign="Center" />
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("id") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <%--<dx:GridViewDataTextColumn FieldName="include_in_perelik" Caption="Включено до переліку №" VisibleIndex="4" Width="50px">
        </dx:GridViewDataTextColumn>--%>
		<dx:GridViewDataComboBoxColumn FieldName="include_in_perelik" VisibleIndex="4" Width = "50px" Visible="True" Caption="Включено до переліку №">
			<HeaderStyle Wrap="True" />
			<PropertiesComboBox DataSourceID="SqlDataSourceIncludeInPerelik" ValueField="id" TextField="name" ValueType="System.String" />
		</dx:GridViewDataComboBoxColumn>


        <dx:GridViewDataTextColumn FieldName="komis_protocol" Caption="Погодження орендодавця" VisibleIndex="4" Width="100px">
			<%--<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("komis_protocol") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>--%>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="geodata_map_points" Caption="Координати на мапі" VisibleIndex="4" Width="100px">
        </dx:GridViewDataTextColumn>


        <dx:GridViewBandColumn Caption="Вільні приміщення"  HeaderStyle-HorizontalAlign="Center" > 
           <Columns>
                <dx:GridViewDataTextColumn FieldName="floor" Caption="Характеристика об’єкта оренди" VisibleIndex="5" Width="80px" ReadOnly="true" ToolTip="Характеристика об’єкта оренди (будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі (надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("floor") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="sqr_for_rent" Caption="Загальна площа приміщень, що перебувають в орендному користуванні, кв.м." VisibleIndex="6" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("sqr_for_rent") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="total_free_sqr" Caption="Загальна площа об’єкта"  VisibleIndex="7" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("total_free_sqr") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="free_sql_usefull" Caption="Корисна площа об’єкта" VisibleIndex="8" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("free_sql_usefull") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
         </dx:GridViewBandColumn>
 
        <dx:GridViewBandColumn Caption="Наявність комунікацій"  HeaderStyle-HorizontalAlign="Center" > 
                <Columns>
                    <dx:GridViewDataCheckColumn FieldName="water" Caption="Водопостачання" VisibleIndex="9" Width="50px" ReadOnly="true">
						<%--<EditItemTemplate>
							<dx:ASPxLabel runat="server" Text='<%# Eval("water") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
						</EditItemTemplate>--%>
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataCheckColumn FieldName="heating" Caption="Теплопостачання" VisibleIndex="10" Width="50px" ReadOnly="true">
						<%--<EditItemTemplate>
							<dx:ASPxLabel runat="server" Text='<%# Eval("heating") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
						</EditItemTemplate>--%>
                    </dx:GridViewDataCheckColumn>

                    <dx:GridViewDataTextColumn FieldName="power_text" Caption="Потужність електромережі" VisibleIndex="11" Width="70px" ReadOnly="true" >
                        <HeaderStyle Wrap="True" />
			            <EditItemTemplate>
				            <dx:ASPxLabel runat="server" Text='<%# Eval("power_text") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			            </EditItemTemplate>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataCheckColumn FieldName="gas" Caption="Газопостачання" VisibleIndex="12" Width="50px" ReadOnly="true">
						<%--<EditItemTemplate>
							<dx:ASPxLabel runat="server" Text='<%# Eval("gas") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
						</EditItemTemplate>--%>
                    </dx:GridViewDataCheckColumn>
                </Columns>
            </dx:GridViewBandColumn>

        <dx:GridViewBandColumn Caption="Додаткові"  HeaderStyle-HorizontalAlign="Center"> 
            <Columns>
                <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування" VisibleIndex="13" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("modify_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="condition" Caption="Технічний стан об’єкта" VisibleIndex="14" Width="80px">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("condition") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>

				<dx:GridViewDataTextColumn FieldName="possible_using" Caption="Можливе використання вільного приміщення" VisibleIndex="15" Width="300px">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("possible_using") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>

<%--				<dx:GridViewDataComboBoxColumn FieldName="using_possible_id" VisibleIndex="15" Width = "320px" Visible="True" Caption="Можливе використання вільного приміщення">
					<HeaderStyle Wrap="True" />
					<PropertiesComboBox DataSourceID="SqlDataSourceUsingPossible" ValueField="id" TextField="name" ValueType="System.Int32" />
				</dx:GridViewDataComboBoxColumn>--%>

            </Columns>
         </dx:GridViewBandColumn>
        <dx:GridViewDataTextColumn FieldName="form_of_ownership" Caption="Форма Власності" VisibleIndex="16" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("form_of_ownership") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_organ" Caption="Орган госп. упр." VisibleIndex="17" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("old_organ") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

<%--        <dx:GridViewDataCheckColumn FieldName="solution" Caption="Наявність рішень про проведення інвестиційного конкурсу" VisibleIndex="18" Width="50px" ReadOnly="true">
        </dx:GridViewDataCheckColumn>--%>
        <dx:GridViewDataTextColumn FieldName="invest_solution" Caption="Наявність рішень про проведення інвестиційного конкурсу" VisibleIndex="18" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("invest_solution") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="initiator" Caption="Ініціатор оренди" VisibleIndex="19" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("initiator") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zgoda_control" Caption="Погодження органу управління балансоутримувача" VisibleIndex="20" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zgoda_control") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zgoda_renter" Caption="Погодження органу охорони культурної спадщини" VisibleIndex="21" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zgoda_renter") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_object_type_name" Caption="Тип об’єкта" VisibleIndex="22" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("free_object_type_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="history" Caption="Пам’ятка культурної спадщини" VisibleIndex="23" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("history") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="sf_upr" Caption="Сфера управління" VisibleIndex="24" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("sf_upr") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="orend_plat_last_month" Caption="Місячна орендна плата за останній місяць(проіндексована)" VisibleIndex="24" Width="80px"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("orend_plat_last_month") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="orend_plat_borg" Caption="Заборгованість по орендній платі, грн. (без ПДВ)" VisibleIndex="24" Width="80px"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("orend_plat_borg") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataColumn FieldName="stanom_na" Caption="Станом на" VisibleIndex="24" Width="80px" CellStyle-HorizontalAlign="Center"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("stanom_na", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataColumn>



		<dx:GridViewDataCheckColumn FieldName="has_perevazh_pravo" Caption="Має переважне право на продовження" VisibleIndex="24" Width="60px" ReadOnly="true">
		</dx:GridViewDataCheckColumn>

        <dx:GridViewDataTextColumn FieldName="polipshanya_vartist" Caption="Вартість здійснених чинним орендарем невід’ємних поліпшень" VisibleIndex="24" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("polipshanya_vartist") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataColumn FieldName="polipshanya_finish_date" Caption="Дата завершення здійснених чинним орендарем невід’ємних поліпшень" VisibleIndex="24" Width="80px" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
                <dx:ASPxLabel runat="server" Text='<%# Eval("polipshanya_finish_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataColumn>


        <dx:GridViewDataTextColumn FieldName="prozoro_number" Caption="Унікальний код обєкту у ЕТС Прозорро-продажі" VisibleIndex="24" Width="150px">
            <DataItemTemplate>
                <%# "<a target=\"_blank\" href=\"https://prozorro.sale/auction/" + Eval("prozoro_number") + "\">" + Eval("prozoro_number") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="pdfurl" Caption="Pdf" VisibleIndex="25" Width="100px" Visible="false">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("pdfurl") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
		</dx:GridViewDataTextColumn>




<%--        <dx:GridViewDataComboBoxColumn FieldName="current_stage_id" Caption="Стан процесу передачі" VisibleIndex="50" Width="300px">
            <PropertiesComboBox 
				DataSourceID="SqlDataSourceDistrict"
				DropDownStyle="DropDownList"
				DropDownWidth="500px"
				TextField="name"  
				ValueField="id">
				<ClientSideEvents DropDown="function(s, e) {
					OnDropDown(s);
				}" />
            </PropertiesComboBox>  
        </dx:GridViewDataComboBoxColumn>--%>

        <dx:GridViewDataComboBoxColumn FieldName="freecycle_step_dict_id" Caption="Стан процесу передачі" VisibleIndex="50" Width="300px">
            <PropertiesComboBox 
				DataSourceID="SqlDataSourceFreecycleStepDict"
				DropDownStyle="DropDownList"
				DropDownWidth="500px"
				TextField="step_name"  
				ValueField="step_id">
				<ClientSideEvents DropDown="function(s, e) {
					OnDropDown(s);
				}" />
            </PropertiesComboBox>  
        </dx:GridViewDataComboBoxColumn>



        <dx:GridViewDataDateColumn FieldName="current_stage_docdate" Caption="Дата документа" VisibleIndex="60" Width="100px" >
        </dx:GridViewDataDateColumn>

        <dx:GridViewDataTextColumn FieldName="current_stage_docnum" Caption="№ документа" VisibleIndex="70" Width="100px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewCommandColumn Caption="Док." VisibleIndex="80" Width="40px" ButtonType="Image" CellStyle-Wrap="False" >
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="bnt_current_stage_pdf" Text="Зображення документу PDF"> <Image Url="~/Styles/current_stage_pdf.png"> </Image>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <CellStyle Wrap="False"></CellStyle>
        </dx:GridViewCommandColumn>

        <dx:GridViewDataTextColumn FieldName="modify_date2" Caption="Дата редагу-вання" VisibleIndex="500" Width="75px">
			<HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("modify_date2") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="modified_by2" Caption="Користувач" VisibleIndex="510" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("modified_by2") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="zal_balans_vartist" Caption="Залишкова балансова вартість" VisibleIndex="1110" Visible="false" >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zal_balans_vartist") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="perv_balans_vartist" Caption="Первісна балансова вартість" VisibleIndex="1120" Visible="false" >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("perv_balans_vartist") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="punkt_metod_rozrahunok" Caption="Посилання на пункт Методики розрахунку орендної плати, яким встановлена орендна ставка для запропонованого цільового призначення" VisibleIndex="1130" Visible="false" Width="230px" >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("punkt_metod_rozrahunok") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="prop_srok_orands" Caption="Пропонований строк оренди (у роках)" VisibleIndex="1140" Visible="false" >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("prop_srok_orands") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="nomer_derzh_reestr_neruh" Caption="Номер запису про право власності у Реєстрація у Державному реєстрі речових прав на нерухоме майно" VisibleIndex="1150" Visible="false" Width="280px" >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("nomer_derzh_reestr_neruh") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="reenum_derzh_reestr_neruh" Caption="Реєстраційний номер об'єкту нерухомого майна у Реєстрація у Державному реєстрі речових прав на нерухоме майно" VisibleIndex="1160" Visible="false" Width="280px" >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("reenum_derzh_reestr_neruh") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="info_priznach_nouse" Caption="Інформація про цільове призначення об’єкта оренди у випадках неможливості використання об’єкта за будь-яким цільовим призначенням, якщо об’єкт розташований у приміщеннях, які мають відповідне соціально-економічне призначення" VisibleIndex="1170" Visible="false" Width="350px"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("info_priznach_nouse") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="info_rahunok_postach" Caption="Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг, або інформація про порядок участі орендаря у компенсації балансоутримувачу витрат на оплату комунальних послуг, якщо об'єкт оренди не має окремих особових рахунків, відкритих для нього відповідними постачальниками комунальних послуг" VisibleIndex="1180" Visible="false" Width="380px"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("info_rahunok_postach") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

<%--        <dx:GridViewDataTextColumn FieldName="priznach_before" Caption="Цільове призначення об’єкта, за яким об’єкт використовувався перед тим, як він став вакантним" VisibleIndex="1310" Visible="false" Width="380px"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("priznach_before") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>


<%--        <dx:GridViewDataTextColumn FieldName="period_nouse" Caption="Період часу, протягом якого об’єкт не використовується" VisibleIndex="1320" Visible="false" Width="380px"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("period_nouse") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>





    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="orend_plat_last_month" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="orend_plat_borg" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="total_free_sqr" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="free_sql_usefull" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

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
        ShowFooter="True"
        VerticalScrollBarMode="Auto"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.Reports1NF.Report1NFDogContinue" Version="A3_18" Enabled="true" />
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