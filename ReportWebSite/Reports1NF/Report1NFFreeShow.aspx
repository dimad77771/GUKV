<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFFreeShow.aspx.cs" Inherits="Reports1NF_Report1NFFreeShow"
    MasterPageFile="~/FreeShowPublicWithUser.master" Title="Перелік вільних приміщень"
    
    %>
<%--UICulture="ru" Culture="ru-UA"--%>
<%--UICulture="es-AG" Culture="es-AG"--%>
<%--UICulture="en-en" Culture="en-en"--%>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxRichEdit.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRichEdit" tagprefix="dxe" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<style type="text/css">
.blink2 {
  -webkit-animation: blink2 1s linear infinite;
  animation: blink2 5s linear infinite;
}
@-webkit-keyframes blink2 {
  100% { color: rgba(255, 132, 132, 0); }
}
@keyframes blink2 {
  100% { color: rgba(255, 132, 132, 0); }
}
</style>


<script type="text/javascript" language="javascript">

    // <![CDATA[

	var IsConnected = <%= IsConnected.ToString().ToLower() %>;

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

	function ShowPhoto(s, e) {
		if (e.buttonID == 'btnPhoto') {
			//console.log("$.cookie", $.cookie);
			//console.log("GetRowKey", s.GetRowKey(e.visibleIndex));
			$.cookie('RecordID', s.GetRowKey(e.visibleIndex));
			ASPxFileManagerPhotoFiles.Refresh();
			PopupObjectPhotos.Show();
		} else if (e.buttonID == 'btnMapShow') {
			FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnMapShowGetRowValues);
		} else if (e.buttonID == 'btnPdfBuild') {
			FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnGridPdfBuildGetRowValues);
        }
	}

	function OnGridPdfBuildGetRowValues(values) {
		//console.log(values);
		var id = values;
		window.open(
			'Report1NFFreeShowPhotosPdf.aspx?id=' + id + '&jpeg=1',
			'_blank',
		);
	}

	function OnMapShowGetRowValues(values) {
		var id = values;
		window.open(
			'Report1NFFreeMap.aspx?fs_id=' + id,
			'_blank',
		);
    }

	function AuctionZayavkaClick(s,e,rownum) {
        if (IsConnected) {
            if (confirm("Подавати заявку на аукціон ?")) {
				FreeSquareGridView.PerformCallback("auctionZayavka:" + FreeSquareGridView.GetRowKey(rownum));
            }
		} else {
			window.open('/Account/LoginInCabinet.aspx?ReturnUrl=/Reports1NF/Report1NFFreeShow.aspx', '_self'); return;
		}
    }

    function AuctionOtkazClick(s, e, rownum) {
        if (confirm("Відмовитись від договору ?")) {
			FreeSquareGridView.PerformCallback("auctionOtkaz:" + FreeSquareGridView.GetRowKey(rownum));
		}
	}

    function UploadAdogvorClick(s, e, rownum) {
        var free_square_id = FreeSquareGridView.GetRowKey(rownum)

		Upload_Adogvor_info.Set("free_square_id", free_square_id)
		Upload_Adogvor_info.Set("mode", "orendar")
		var el1 = document.getElementById("MainContent_Upload_Adogvor_TextBox0_Input");
		el1.click();
	}

    function ZayavkaClick(e) {
		//alert(IsConnected);
        //console.log("e", e)
        if (IsConnected) {
            ASPxPopupZayavkaDoc.Show();
            ZayavkaRichEdit.PerformCallback(111);
        } else {
			window.open('/Account/LoginInCabinet.aspx?ReturnUrl=/Reports1NF/Report1NFFreeShow.aspx', '_self'); return;
        }
    }

    function onAdogvorFileUploadStart(s, e) {
		console.log('onAdogvorFileUploadStart', e)
		LoadingPanel.Show()
	}

    function onAdogvorFileUploadComplete(s, e) {
        console.log('onAdogvorFileUploadComplete', e)
		if (e.isValid) {
            LoadingPanel.Hide()
			FreeSquareGridView.PerformCallback('onAdogvorFileUploadComplete')
		} else {
			LoadingPanel.Hide()
			if (e.errorText) {
				alert(e.errorText)
			} else {
				alert("Помилка")
			}
		}
	}

	window.onload = function () {
		jQuery(document).ready(function () {
			setTimeout(function () {
				AdjustGridSizes();
				$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI0_").hide();
				$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI5_IS").hide();
			})
		});
	};


    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquare" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
	fs.komis_protocol,
	fs.geodata_map_url,
 row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr) as npp     
,fs.id
,fs.include_in_perelik
,org.short_name as org_name
,org.zkpo_code
,org.prozoro_title as vidpov_osoba

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

,(SELECT Q.public_name FROM free_proc_step_dict Q where Q.step_id = fs.freecycle_step_dict_id) freecycle_step_name

,fs.zal_balans_vartist
,fs.perv_balans_vartist
,fs.prop_srok_orands
,fs.punkt_metod_rozrahunok
,invest_solution = (select qq.name from dict_1nf_invest_solution qq where qq.id = fs.invest_solution_id)
,(select q.name from dict_1nf_power_info q where q.id = fs.power_info_id) as power_text
,fs.nomer_derzh_reestr_neruh
,fs.reenum_derzh_reestr_neruh
,fs.info_priznach_nouse
,fs.info_rahunok_postach
,fs.priznach_before
,fs.period_nouse
,fs.osoba_use_before
,fs.floor
,water = case when fs.water = 1 then 1 when fs.water = 0 then 0 end
,heating = case when fs.heating = 1 then 1 when fs.heating = 0 then 0 end
,power = case when fs.power = 1 then 1 when fs.power = 0 then 0 end
,gas = case when fs.gas = 1 then 1 when fs.gas = 0 then 0 end

--,(select qq.name2 from view_dict_rental_rate qq where qq.id = fs.using_possible_id) as possible_using
,fs.possible_using

,(select qq.name from dict_free_object_type qq where qq.id = fs.free_object_type_id) as free_object_type_name

,fs.modify_date
,fs.note
, solution = fs.is_solution
, fs.initiator
, zg2.name as zgoda_control
, zg.name as zgoda_renter
,fs.prozoro_number

,st.kind
,rep.form_of_ownership
--,(select Q.full_name from organizations Q where Q.id = bal.org_giver_id) as orandodatel
,dbo.get_reports1NF_orandodatel(b.district, rep.form_of_ownership) as orandodatel
,rep.old_organ

--,b.object_kind as vydbudynku
,history = case when isnull(b.history, 'НІ') = 'НІ' then '' else 'ТАК' end 
, isnull(ddd.name, 'Невизначені') as sf_upr
, @baseurl + '/Reports1NF/BalansFreeSquarePhotosPdf.aspx?id=' + cast(fs.id as varchar(100)) as pdfurl
, @baseurl + '/Reports1NF/Report1NFFreeShowPhotosPdf.aspx?id=' + cast(fs.id as varchar(100)) + '&jpeg=1' as jpegurl
, case when exists (select 1 from reports1nf_balans_free_square_photos qq where qq.free_square_id = fs.id) then 1 else 0 end as isexistsphoto

,case 
       when org.zkpo_code = '05433011' then '1' 
       when org.zkpo_code = '40538421' then '2' 
        else '0' 
    end as show_btn_load_dogovor

,(SELECT top 1 Q.zayavka_date FROM auction_uchasnik Q where Q.free_square_id = fs.id and Q.is_arhiv = 0 order by Q.zayavka_date desc) as zayavka_date
,dbo.[CabinetOrendarStage](fs.id, @userId) as cabinetOrendarStage

FROM view_reports1nf rep
join reports1nf_balans bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_balans_free_square fs on fs.balans_id = bal.id and fs.report_id = rep.report_id
--left join (select * from dbo.reports1nf_balans_free_square where id = (select top 1 id from dbo.reports1nf_balans_free_square where balans_id = bal.id)) fs on fs.balans_id = bal.id
join reports1nf_org_info org on org.id = bal.organization_id
left join [dbo].[dict_streets] st on b.addr_street_id = st.id
left join dbo.dict_zgoda_renter zg on fs.zgoda_renter_id = zg.id
left join dbo.dict_zgoda_renter zg2 on fs.zgoda_control_id = zg2.id

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
				and (is_included = 1)
				and (@fs_id = -1 OR fs.id = @fs_id)
				and (@mode50 = 0 OR fs.freecycle_step_dict_id = 31)
    order by 
        case when (SELECT Q.public_name FROM free_proc_step_dict Q where Q.step_id = fs.freecycle_step_dict_id) <> '' then 1 else 2 end, 
        case when (SELECT Q.public_name FROM free_proc_step_dict Q where Q.step_id = fs.freecycle_step_dict_id) = 'аукціон оголошено' then 1 else 2 end, 
        org_name, street_name, addr_nomer, total_free_sqr   "
    OnSelecting="SqlDataSourceFreeSquare_Selecting"

UpdateCommand="UPDATE [reports1nf_balans_free_square]
SET
    [komis_protocol] = @komis_protocol,
	[geodata_map_url] = @geodata_map_url
WHERE id = @id" 
	onupdating="SqlDataSourceFreeSquare_Updating"
	>
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="period_year" />
		<asp:Parameter DbType="String" DefaultValue="" Name="baseurl" />
		<asp:Parameter DbType="Int32" DefaultValue="-1" Name="fs_id" />
		<asp:Parameter DbType="Int32" DefaultValue="0" Name="mode50" />
        <asp:Parameter DbType="String" DefaultValue="" Name="userId" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<%--<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFAccounts.aspx" Text="Облікові Записи"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFNotifications.aspx" Text="Налаштування Повідомлень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>        
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFFreeShow.aspx" Text="Перелік вільних приміщень"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>--%>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Перелік вільних приміщень" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_FreeSquare_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_FreeSquare_SaveAs" 
                PopupElementID="ASPxButton_FreeSquare_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_FreeSquare_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <%--<br />
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_FreeSquare_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_FreeSquare_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>--%>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_FreeSquare_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
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


	<dx:ASPxHiddenField ID="Upload_Adogvor_info" ClientInstanceName="Upload_Adogvor_info" runat="server"/>
	<div style="display:none">
		<dx:ASPxUploadControl runat="server" ID="Upload_Adogvor" ClientInstanceName="Upload_Adogvor" 
			NullText="Select files 1" AutoStartUpload="true" UploadMode="Auto" ShowUploadButton="True" ShowProgressPanel="True"
			OnFileUploadComplete="UploadControl_FileUploadComplete">
			<AdvancedModeSettings EnableMultiSelect="false" EnableFileList="True" EnableDragAndDrop="True" />
			<ValidationSettings MaxFileSize="100000000" AllowedFileExtensions=".zip">
			</ValidationSettings>
			<ClientSideEvents 
				FilesUploadStart="onAdogvorFileUploadStart"
				FileUploadComplete="onAdogvorFileUploadComplete" />
		</dx:ASPxUploadControl>
	</div>

	<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" />


   <dx:ASPxGridView ID="FreeSquareGridView" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceFreeSquare" KeyFieldName="id" Width="100%" 
        ClientInstanceName="FreeSquareGridView" 
        OnDataBound="FreeSquareGridView_DataBound"
        OnCustomCallback="GridViewFreeSquare_CustomCallback"
        OnCustomFilterExpressionDisplayText="GridViewFreeSquare_CustomFilterExpressionDisplayText"
        OnProcessColumnAutoFilter="GridViewFreeSquare_ProcessColumnAutoFilter" >
	   <ClientSideEvents CustomButtonClick="ShowPhoto" />
    <Templates>
        <DetailRow>
            <div style="margin-left:5px">
                <table style="border:1px solid; margin-top:6px; border-collapse:collapse; width:1700px">
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px; width:200px">
                            Включено до переліку №
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px; width:200px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("include_in_perelik") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px; width:200px">
                            Залишкова балансова вартість, грн.
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px; width:200px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("zal_balans_vartist") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Первісна балансова вартість, грн.
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("perv_balans_vartist") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Тип об’єкта
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("free_object_type_name") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Пропонований строк оренди (у роках)
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("prop_srok_orands") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Пункт Методики розрахунку орендної плати (якщо об’єкт пропонується для включення до Переліку другого типу)
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("punkt_metod_rozrahunok") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Наявність рішень про проведення інвестиційного конкурсу або про включення об’єкта до переліку майна, що підлягає приватизації
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("invest_solution") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Погодження органу управління балансоутримувача
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("zgoda_control") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Район
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("district") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Назва Вулиці
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("street_name") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Номер Будинку
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("addr_nomer") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Загальна площа об’єкта, кв.м
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("total_free_sqr") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Корисна площа об’єкта, кв.м
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("free_sql_usefull") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Характеристика об’єкта оренди(будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі(надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("floor") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Технічний стан
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("condition") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Водопостачання
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("water") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Теплопостачання
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("heating") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Газопостачання
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("gas") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Електропостачання
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("power_text") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Пам’ятка культурної спадщини
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("history") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Погодження органу охорони культурної спадщини
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("zgoda_renter") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Номер запису про право власності у Реєстрація у Державному реєстрі речових прав на нерухоме майно
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("nomer_derzh_reestr_neruh") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Реєстраційний номер об'єкту нерухомого майна у Реєстрація у Державному реєстрі речових прав на нерухоме майно
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("reenum_derzh_reestr_neruh") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Інформація про цільове призначення об’єкта оренди
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("info_priznach_nouse") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Інформація про наявність окремих особових рахунків на об'єкт оренди, відкритих постачальниками комунальних послуг
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("info_rahunok_postach") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Цільове призначення об’єкта, за яким об’єкт використовувався перед тим, як він став вакантним
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("priznach_before") %>' Font-Bold="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Період часу, протягом якого об’єкт не використовується (у місяцях)
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("period_nouse") %>' Font-Bold="true" />
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            Інформацію про особу, яка використовувала об’єкт перед тим, як він став вакантним
                        </td>
                        <td style="text-align:left; border:1px solid; padding:3px">
                            <dx:ASPxLabel runat="server" Text='<%# Eval("osoba_use_before") %>' Font-Bold="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </DetailRow>
    </Templates>
    <Columns>

        <dx:GridViewCommandColumn VisibleIndex="0" Width="50px" ButtonType="Image" CellStyle-Wrap="False" >
            <CustomButtons>
                <%--<dx:GridViewCommandColumnCustomButton ID="btnPhoto" Text="Фотографії об'єкту"> 
					<Image Url="~/Styles/PhotoIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>--%>
				<dx:GridViewCommandColumnCustomButton ID="btnPdfBuild" Text="Фотографії об'єкту"> 
					<Image Url="~/Styles/PhotoIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnMapShow" Text="Показати на мапі"> 
					<Image Url="~/Styles/MapShowIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <CellStyle Wrap="False"></CellStyle>
        </dx:GridViewCommandColumn>

        <dx:GridViewDataTextColumn FieldName="auction_tablo" Caption="Аукціон" VisibleIndex="0"  Width="170px" ReadOnly="true">
			<DataItemTemplate>

                <dx:ASPxButton Enabled="true" runat="server" ID="AuctionZayavkaBtn" Text="Подати заявку" AutoPostBack="false" Visible='<%# Eval("cabinetOrendarStage").ToString() == "" %>' OnInit="AuctionZayavkaBtn_Init" >
                    <ClientSideEvents Click="function(s, e) { AuctionZayavkaClick(s,e); }" />
                </dx:ASPxButton>
                
                <dx:ASPxLabel runat="server" Text='<%# "Заявка подана " + (Eval("zayavka_date") is DateTime ? ((DateTime)Eval("zayavka_date")).ToString("dd.MM.yyyy") : "") %>' Visible='<%# Eval("cabinetOrendarStage").ToString() == "zayavka_podana" %>' />
                
                <dx:ASPxButton runat="server" ID="AuctionZayavkaLoadPodpisBtn" Text="Завантажити підпис" AutoPostBack="false" Visible='<%# Eval("cabinetOrendarStage").ToString() == "podpis_balansoderzhatel" %>' OnInit="AuctionZayavkaLoadPodpisBtn_Init" >
                    <ClientSideEvents Click="function(s, e) { UploadAdogvorClick(s,e); }" />
                </dx:ASPxButton>

                <dx:ASPxButton runat="server" ID="AuctionZayavkaOtkazBtn" Text="Відмовитись від договору" AutoPostBack="false" Visible='<%# Eval("cabinetOrendarStage").ToString() == "podpis_balansoderzhatel" %>' OnInit="AuctionZayavkaOtkazBtn_Init" >
                    <ClientSideEvents Click="function(s, e) { AuctionOtkazClick(s,e); }" />
                </dx:ASPxButton>


                <dx:ASPxLabel runat="server" Text='<%# "Договір знаходиться на підпису орендодавця" %>' Visible='<%# Eval("cabinetOrendarStage").ToString() == "podpis_balansoderzhatel_orendar" %>' />

                <dx:ASPxLabel runat="server" Text='<%# "Договір підписано всіма сторонами" %>' Visible='<%# Eval("cabinetOrendarStage").ToString() == "podpis_all" %>' />

            </DataItemTemplate>
        </dx:GridViewDataTextColumn>


        

        <dx:GridViewDataTextColumn FieldName="org_name" Caption="Балансоутримувач" VisibleIndex="0"  Width="250px" ReadOnly="true">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("org_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" Caption="Код ЄДРПОУ" VisibleIndex="1" Width="85px" ReadOnly="true">
			<HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zkpo_code") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
		<dx:GridViewDataCheckColumn FieldName="isexistsphoto" Caption="Наяв- ність фото" VisibleIndex="1" Width="50px" ReadOnly="true"/>
        <dx:GridViewDataTextColumn FieldName="freecycle_step_name" Caption="Стан процесу передачі" VisibleIndex="1" Width="200px" CellStyle-ForeColor="Red" CellStyle-CssClass="blink2">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("freecycle_step_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" Caption="Район" VisibleIndex="2" Width="120px" ReadOnly="true">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("district") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_name" Caption="Назва Вулиці" VisibleIndex="3" Width="150px" ReadOnly="true">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("street_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" Caption="Номер Будинку" VisibleIndex="4" Width="80px" ReadOnly="true">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("addr_nomer") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="id" Caption="Реєстра-ційний №" VisibleIndex="4" Width ="60px"  >
			<CellStyle HorizontalAlign="Center" />
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("id") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

		<dx:GridViewDataComboBoxColumn FieldName="include_in_perelik" VisibleIndex="4" Width = "50px" Visible="True" Caption="Включено до переліку №">
			<HeaderStyle Wrap="True" />
		</dx:GridViewDataComboBoxColumn>


        <%--<dx:GridViewDataTextColumn FieldName="geodata_map_url" Caption="URL на мапі" VisibleIndex="4" Width="100px">
        </dx:GridViewDataTextColumn>--%>


        <dx:GridViewBandColumn Caption="Вільні приміщення"  HeaderStyle-HorizontalAlign="Center" > 
           <Columns>
                <dx:GridViewDataTextColumn FieldName="floor" Caption="Характеристика об’єкта оренди" VisibleIndex="5" Width="80px" ReadOnly="true" ToolTip="Характеристика об’єкта оренди (будівлі в цілому або частини будівлі із зазначенням місця розташування об’єкта в будівлі (надземний, цокольний, підвальний, технічний або мансардний поверх, номер поверху або поверхів)">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("floor") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
<%--                <dx:GridViewDataTextColumn FieldName="sqr_for_rent" Caption="Загальна площа приміщень, що перебувають в орендному користуванні, кв.м." VisibleIndex="6" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("sqr_for_rent") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>--%>
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
                    <dx:GridViewDataCheckColumn FieldName="power" Caption="Потужність електромережі" VisibleIndex="11" Width="50px" ReadOnly="true">
						<%--<EditItemTemplate>
							<dx:ASPxLabel runat="server" Text='<%# Eval("power") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
						</EditItemTemplate>--%>
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataCheckColumn FieldName="gas" Caption="Газопостачання" VisibleIndex="12" Width="50px" ReadOnly="true">
						<%--<EditItemTemplate>
							<dx:ASPxLabel runat="server" Text='<%# Eval("gas") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
						</EditItemTemplate>--%>
                    </dx:GridViewDataCheckColumn>
                </Columns>
            </dx:GridViewBandColumn>

        <dx:GridViewBandColumn Caption="Додаткові"  HeaderStyle-HorizontalAlign="Center"> 
            <Columns>
<%--                <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування" VisibleIndex="13" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("modify_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataDateColumn>--%>
                <dx:GridViewDataTextColumn FieldName="condition" Caption="Технічний стан об’єкта" VisibleIndex="14" Width="80px">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("condition") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="possible_using" Caption="Можливе використання вільного приміщення" VisibleIndex="15" Width="180px">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("possible_using") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
         </dx:GridViewBandColumn>
        <%--<dx:GridViewDataTextColumn FieldName="form_of_ownership" Caption="Форма Власності" VisibleIndex="16" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("form_of_ownership") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <%--<dx:GridViewDataTextColumn FieldName="old_organ" Caption="Орган госп. упр." VisibleIndex="17" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("old_organ") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <%--<dx:GridViewDataCheckColumn FieldName="solution" Caption="Наявність рішень про проведення інвестиційного конкурсу" VisibleIndex="18" Width="50px" ReadOnly="true">--%>
<%--			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("solution") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>--%>
        <%--</dx:GridViewDataCheckColumn>--%>
        <%--<dx:GridViewDataTextColumn FieldName="initiator" Caption="Ініціатор оренди" VisibleIndex="19" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("initiator") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <%--<dx:GridViewDataTextColumn FieldName="zgoda_control" Caption="Погодження органу управління" VisibleIndex="20" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zgoda_control") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <%--<dx:GridViewDataTextColumn FieldName="zgoda_renter" Caption="Погодження орендодавця" VisibleIndex="21" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zgoda_renter") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="orandodatel" Caption="Орендодавець" VisibleIndex="22" Width="500px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("orandodatel") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_object_type_name" Caption="Тип об’єкта" VisibleIndex="22" Width="180px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("free_object_type_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="history" Caption="Пам’ятка культурної спадщини" VisibleIndex="23" Width="70px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("history") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="prozoro_number" Caption="Унікальний код обєкту у ЕТС Прозорро-продажі" VisibleIndex="24" Width="150px">
            <DataItemTemplate>
                <%# "<a target=\"_blank\" href=\"https://prozorro.sale/auction/" + Eval("prozoro_number") + "\">" + Eval("prozoro_number") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="sf_upr" Caption="Сфера управління" VisibleIndex="24" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("sf_upr") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
		<dx:GridViewDataTextColumn FieldName="pdfurl" Caption="Pdf" VisibleIndex="25" Width="150px" Visible="false">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("pdfurl") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
		</dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="vidpov_osoba" Caption="Особа відповідальна за ознайомлення з об’єктом" VisibleIndex="25" Width="220px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("vidpov_osoba") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="jpegurl" Caption="Фото-архів" VisibleIndex="25" Width="150px" Visible="true">
			<DataItemTemplate>
                <%# "<a target=\"_blank\" href=\"" + Eval("jpegurl") + "\">" + Eval("jpegurl") + "</a>"%>
            </DataItemTemplate>
		</dx:GridViewDataTextColumn>

        

        



        <%--<dx:GridViewDataTextColumn FieldName="komis_protocol" Caption="Протокол комісії" VisibleIndex="40" Width="100px">--%>
			<%--<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("komis_protocol") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>--%>
        <%--</dx:GridViewDataTextColumn>--%>


    </Columns>

    <TotalSummary>
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
    <SettingsCookies CookiesID="GUKV.Reports1NF.Report1NFFreeShow" Version="D1_8" Enabled="false" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewFreeSquareInit" EndCallback="GridViewFreeSquareEndCallback" />
</dx:ASPxGridView>

<dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare" runat="server" AllowDragging="True" 
    ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True" 
    HeaderText="Фотографії об'єкту з вільним приміщенням" Modal="True" 
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
    PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

            <asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server" 
                SelectMethod="Select" 
                TypeName="ExtDataEntry.Models.FileAttachment">
                <SelectParameters>
                    <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
                    <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles" runat="server" 
                ClientInstanceName="ASPxFileManagerPhotoFiles" DataSourceID="ObjectDataSourcePhotoFiles">
                <Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
                <SettingsFileList>
                    <ThumbnailsViewSettings ThumbnailSize="180px" />
                </SettingsFileList>
                    <SettingsEditing AllowDelete="false" AllowCreate="false" AllowDownload="true" AllowMove="false" AllowRename="false" />
                    <SettingsFolders Visible="False" />
                    <SettingsToolbar ShowDownloadButton="true" ShowPath="true" ShowFilterBox="false" />
                    <SettingsUpload UseAdvancedUploadMode="True" Enabled="false" >
                        <AdvancedModeSettings EnableMultiSelect="True" />
                    </SettingsUpload>
<%--                <SettingsEditing AllowDelete="false" AllowCreate="false" AllowDownload="false" AllowMove="false" AllowRename="false" />
                <SettingsFolders Visible="False" />
                <SettingsToolbar ShowDownloadButton="false" ShowPath="False" />
                <SettingsUpload UseAdvancedUploadMode="True" Enabled="false">
                    <AdvancedModeSettings EnableMultiSelect="True" />
                </SettingsUpload>--%>

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

<dx:ASPxPopupControl ID="ASPxPopupZayavkaDoc" runat="server" AllowDragging="True" 
    ClientInstanceName="ASPxPopupZayavkaDoc" EnableClientSideAPI="True" 
    HeaderText="Заявка" Modal="True" 
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
    PopupAction="None" PopupElementID="ASPxGridViewFreeSquare"  >
    <ContentCollection>
        <dx:PopupControlContentControl ID="ASPxPopupZayavkaDocContentControl" runat="server" SupportsDisabledAttribute="True">
            <dxe:ASPxRichEdit ID="ZayavkaRichEdit" ClientInstanceName="ZayavkaRichEdit" runat="server" Width="1000px" Height="700px" ActiveTabIndex="0" ShowConfirmOnLosingChanges="false" OnCallback="ZayavkaRichEdit_Callback" OnInit="ZayavkaRichEdit_Init" > 
            </dxe:ASPxRichEdit>



            <br />

            <dx:ASPxButton ID="ASPxPopupZayavkaDocButtonClose" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
                <ClientSideEvents Click="function(s, e) { ASPxPopupZayavkaDoc.Hide(); }" />
            </dx:ASPxButton>

        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="ASPxPopupUser" runat="server" AllowDragging="True" 
    ClientInstanceName="ASPxPopupUser" EnableClientSideAPI="True" 
    HeaderText="Заявка" Modal="True" 
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
    PopupAction="None" PopupElementID="ASPxGridViewFreeSquare"  >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControlUser" runat="server" SupportsDisabledAttribute="True">
            <asp:CreateUserWizard ID="RegisterUser" runat="server" EnableViewState="false" 
        OnCreatedUser="RegisterUser_CreatedUser" 
        CompleteSuccessText="Користувач успішно зареєстрований." 
        ContinueButtonText="Продовжити" 
        DuplicateEmailErrorMessage="Вказана електронна адреса вже використовується іншим користувачем. Будь ласка, введіть іншу електронну адресу." 
        DuplicateUserNameErrorMessage="Користувач з таким ім'ям вже зареєстрований на сайті. Будь ласка, введіть інше ім'я користувача." 
        InvalidEmailErrorMessage="Будь ласка, введіть коректну електронну адресу." 
        InvalidPasswordErrorMessage="Мінімальна довжина паролю: {0} символів" 
        UnknownErrorMessage="Помилка при створенні нового користувача. Будь ласка, спробуйте знову." >
        <LayoutTemplate>
            <asp:PlaceHolder ID="wizardStepPlaceholder" runat="server"></asp:PlaceHolder>
            <asp:PlaceHolder ID="navigationPlaceholder" runat="server"></asp:PlaceHolder>
        </LayoutTemplate>
        <WizardSteps>
            <asp:CreateUserWizardStep ID="RegisterUserWizardStep" runat="server">
                <ContentTemplate>
                    <h2>
                        Реєстрація нового користувача
                    </h2>
                    <p>
                        Для реєстрації на сайті, будь ласка, заповніть цю форму.
                    </p>
                    <p>
                        Мінімальна припустима довжина паролю складає <%= Membership.MinRequiredPasswordLength %> символів.
                    </p>
                    <span class="failureNotification">
                        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                    </span>
                    <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" CssClass="failureNotification" 
                         ValidationGroup="RegisterUserValidationGroup"/>
                    <div class="accountInfo">
                        <fieldset class="register">
                            <legend>Інформація про користувача</legend>
                            <p>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Ім'я користувача:</asp:Label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести ім'я користувача."
                                     ToolTip="Для реєстрації необхідно ввести ім'я користувача." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">Електронна адреса:</asp:Label>
                                <asp:TextBox ID="Email" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести електронну адресу користувача."
                                     ToolTip="Для реєстрації необхідно ввести електронну адресу користувача." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Пароль:</asp:Label>
                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести пароль."
                                     ToolTip="Для реєстрації необхідно ввести пароль." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Підтвердження паролю:</asp:Label>
                                <asp:TextBox ID="ConfirmPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" CssClass="failureNotification"
                                     Display="Dynamic" 
                                     ErrorMessage="Для реєстрації необхідно ввести підтвердження паролю."
                                     ID="ConfirmPasswordRequired"
                                     runat="server" 
                                     ToolTip="Для реєстрації необхідно ввести підтвердження паролю."
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                     CssClass="failureNotification" Display="Dynamic"
                                     ErrorMessage="Для успішної реєстрації пароль та підтвердження паролю мають співпадати."
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:CompareValidator>
                            </p>
                        </fieldset>
                        <p class="submitButton">
                            <table width="100%" border="0" style="width:100%">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="CreateUserButton" runat="server" CommandName="MoveNext" Text="Зареєструватись" ValidationGroup="RegisterUserValidationGroup"></dx:ASPxButton>
                                   </td>

                                    <td>
                                        <dx:ASPxButton ID="ASPxPopupUserButtonClose" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
                                            <ClientSideEvents Click="function(s, e) { ASPxPopupUser.Hide(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </p>
                    </div>
                </ContentTemplate>
                <CustomNavigationTemplate>
                </CustomNavigationTemplate>
            </asp:CreateUserWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>

            

            

        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>


</asp:Content>
