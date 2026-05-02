<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFDogContinue.aspx.cs" Inherits="Reports1NF_Report1NFDogContinue"
    MasterPageFile="~/NoHeader.master" Title="Реєстр продовження договорів" %>

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
        } else if (e.buttonID == 'btnDocx1Build') {
            FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnGridDocx1BuildGetRowValues);
        } else if (e.buttonID == 'btnDocx2Build') {
            FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnGridDocx2BuildGetRowValues);
        } else if (e.buttonID == 'btnCommissionLetterBuild') {
            FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnCommissionLetterBuild);
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
        }
        else if (e.buttonID == 'btnCopyFullDescription') {
            var cols = "orendar_name;orendar_zkpo;org_name;zkpo_code;balanutr_addr_street;balanutr_addr_nomer;giver_name;giver_zkpo;giver_addr_street;giver_addr_nomer;agreement_date;rent_finish_date;srok_dog;";
            cols += "include_in_perelik;zal_balans_vartist;perv_balans_vartist;free_object_type_name;prop_srok_orands;punkt_metod_rozrahunok;invest_solution;";
            cols += "zgoda_control;district;street_name;addr_nomer;total_free_sqr;free_sql_usefull;";
            cols += "floor;condition;water;heating;gas;power_text;history;zgoda_renter;nomer_derzh_reestr_neruh;reenum_derzh_reestr_neruh;possible_using;info_rahunok_postach;orend_plat_last_month;orend_plat_borg;stanom_na;";
            cols += "has_perevazh_pravo;polipshanya_vartist;polipshanya_finish_date;rozmir_vidshkoduv;zalbalansvartist_date;primitki;id";
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
            "Залишкова балансова вартість, грн. – ",
            "Первісна балансова вартість, грн. - ",
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
            "Місячна орендна плата за останній місяць(проіндексована) – ",
            "Заборгованість по орендній платі, грн. (без ПДВ) – ",
            "Станом на – ",

            "Має переважне право на продовження – ",
            "Вартість здійснених чинним орендарем невід’ємних поліпшень – ",
            "Дата завершення здійснених чинним орендарем невід’ємних поліпшень – ",
            "Розмір відшкодування земельного податку та інших - ",
            "Дата формування залишкової вартості - ",
            "Примітки – ",
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

            txt += (i == 0 ? "" : "\n") + headers[i] + vv;
        }

        var id = values[values.length - 1];
        txt += "\n" + "Фото - https://dkv.kyivcity.gov.ua/Reports1NF/BalansDogContinuePhotosPdf.aspx?id=" + id + '&jpeg=1';

        myCopyToClipboard(txt);
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
        window.location = 'OrgRentAgreement.aspx?rid=' + values[2] + '&aid=' + values[1] + '&edit_free_square_id=' + values[0];
    }


    function OnGridPdfBuildGetRowValues(values) {
        console.log(values);
        var id = values;
        window.open(
            'BalansDogContinuePhotosPdf.aspx?id=' + id,
            '_blank',
        );
    }

    function OnGridDocx1BuildGetRowValues(values) {
        console.log(values);
        var id = values;
        window.open(
            'BalansDogContinuePhotosDocx.aspx?repmode=1&id=' + id,
            '_blank',
        );
    }

    function OnGridDocx2BuildGetRowValues(values) {
        console.log(values);
        var id = values;
        window.open(
            'BalansDogContinuePhotosDocx.aspx?repmode=2&id=' + id,
            '_blank',
        );
    }

    function OnCommissionLetterBuild(values) {
        console.log(values);
        var id = values;
        window.open(
            'CommissionLetter.aspx?id=' + id,
            '_blank',
        );
    }

    function OnDropDown(comboBox) {
        //SetDropDownWidth(comboBox, "368px");
        _aspxMakeScollableArea(comboBox);
    }

    function SetDropDownWidth(comboBox, width) {
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
            var lsScrollableDiv = listBox.GetScrollDivElement();
            if (_aspxIsExists(lsScrollableDiv)) {
                var browserWidth = (_aspxGetDocumentClientWidth() - 10) + 'px';
                //alert(browserWidth);
                _aspxSetAttribute(lsScrollableDiv.style, "width", browserWidth);
                _aspxSetAttribute(lsScrollableDiv.style, "overflow-x", "scroll");
                _aspxSetAttribute(lsScrollableDiv.style, "overflow-y", "scroll");
            }
        }
    }

    function CheckBoxBalansObjectsShowNeziznacheni_CheckedChanged(s, e) {

        FreeSquareGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }


    function OnContextMenuItemClick(s, e) {
        var id = s.GetRowKey(e.elementIndex)
        window.open(
            'Report2.aspx?id=' + id,
            '_blank',
        );
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
,org.full_name as org_name
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
,fs.modified_by
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

,bal.agreement_num
,bal.agreement_date
,bal.rent_finish_date
,cast(round(DATEDIFF ( month, bal.agreement_date, bal.rent_finish_date ) / 12.0, 0) as int) as srok_dog

,fs.orend_plat_last_month
,fs.orend_plat_dogovor
,fs.has_perevazh_pravo
,(select Q.name from dict_may_pravo_prodov Q where Q.id = fs.may_pravo_prodov) as may_pravo_prodov_text
,fs.polipshanya_vartist
,fs.polipshanya_finish_date
,fs.primitki
,fs.zalbalansvartist_date
,fs.osoba_oznakoml
,fs.rozmir_vidshkoduv
,fs.building_type
,fs.category
,fs.rental_rate_percent
,fs.rental_type
,fs.rental_term
,fs.commission_note
,fs.additional_info
,fs.speaker_name
,fs.incoming_doc_num
,fs.incoming_doc_date
,fs.letter_appendix_num
,fs.letter_appendix_date
,fs.commission_id
,dc.commission_num
,fs.commission_result
,fs.protocol_question_num
,fs.outgoing_doc_num
,fs.outgoing_doc_date
,fs.slukhali_text
,fs.virishyly_text
,fs.golosovanie

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
left join dbo.dogcontinue_commission dc on dc.id = fs.commission_id

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
            AND ( (@p_show_neziznacheni = 0) OR (@p_show_neziznacheni = 1 AND (fs.is_included = 1 and fs.komis_protocol <> '' and fs.geodata_map_points <> '')) )
            AND ( (@bal_organization_id <= 0) OR ( org.zkpo_code in (select q.zkpo_code from view_reports1nf q where q.organization_id = @bal_organization_id)) )

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
    [may_pravo_prodov] = @may_pravo_prodov_text,
	[is_included] = @is_included,
	[building_type] = @building_type,
	[category] = @category,
	[rental_rate_percent] = @rental_rate_percent,
	[rental_type] = @rental_type,
	[rental_term] = @rental_term,
	[commission_note] = @commission_note,
	[additional_info] = @additional_info,
	[speaker_name] = @speaker_name,
	[incoming_doc_num] = @incoming_doc_num,
	[incoming_doc_date] = @incoming_doc_date,
	[letter_appendix_num] = @letter_appendix_num,
	[letter_appendix_date] = @letter_appendix_date,
	[commission_id] = @commission_id,
	[commission_result] = @commission_result,
	[protocol_question_num] = @protocol_question_num,
	[outgoing_doc_num] = @outgoing_doc_num,
	[outgoing_doc_date] = @outgoing_doc_date,
	[slukhali_text] = @slukhali_text,
	[virishyly_text] = @virishyly_text,
	[golosovanie] = @golosovanie,
	[modify_date2] = @modify_date2,
	[modified_by2] = @modified_by2
WHERE id = @id" 
	onupdating="SqlDataSourceFreeSquare_Updating"
	>
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="period_year" />
		<asp:Parameter DbType="String" DefaultValue="" Name="baseurl" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_show_neziznacheni" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_organization_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name, cod, ord, istitle FROM dict_free_square_stage union select null, '<пусто>', '00', -1, 0 ORDER BY ord">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreecycleStepDict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT step_id, lookup_name as step_name, step_ord FROM dogcontinue_proc_step_dict union select null, '<пусто>', 0 ORDER BY step_ord">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceUsingPossible" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, left(full_name, 150) as name, rental_rate, 1 as ordrow FROM dict_rental_rate union select null, '<пусто>', null, 2 as ordrow ORDER BY ordrow, name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceMayPravoProdov" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name, ordnum from dict_may_pravo_prodov union select null, '',  9999 as ordrow ORDER BY ordnum, name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceIncludeInPerelik" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT '1' id, '1' name, 1 as ordrow union SELECT '2' id, '2' name, 1 as ordrow union select null, '',  2 as ordrow ORDER BY ordrow, name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceCommission" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT [id], [commission_num], [commission_date], [commission_status], [deputies_list], [dkv_head], [district_representatives], [modify_date], [modified_by] FROM [dogcontinue_commission] ORDER BY CASE WHEN [commission_date] IS NULL THEN 1 ELSE 0 END, [commission_date] DESC, [commission_num]"
    DeleteCommand="DELETE dc FROM [dogcontinue_commission] dc WHERE dc.[id] = @id AND NOT EXISTS (SELECT 1 FROM [reports1nf_arenda_dogcontinue] fs WHERE fs.[commission_id] = dc.[id])"
    InsertCommand="INSERT INTO [dogcontinue_commission] ([commission_num], [commission_date], [commission_status], [deputies_list], [dkv_head], [district_representatives], [modify_date], [modified_by]) VALUES (@commission_num, @commission_date, @commission_status, @deputies_list, @dkv_head, @district_representatives, @modify_date, @modified_by) SELECT SCOPE_IDENTITY()"
    UpdateCommand="UPDATE [dogcontinue_commission] SET [commission_num] = @commission_num, [commission_date] = @commission_date, [commission_status] = @commission_status, [deputies_list] = @deputies_list, [dkv_head] = @dkv_head, [district_representatives] = @district_representatives, [modify_date] = @modify_date, [modified_by] = @modified_by WHERE [id] = @id"
    OnInserting="SqlDataSourceCommission_Inserting"
    OnUpdating="SqlDataSourceCommission_Updating"
    OnDeleting="SqlDataSourceCommission_Deleting"
    ProviderName="System.Data.SqlClient">
    <DeleteParameters>
        <asp:Parameter Name="id" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="commission_num" Type="String" />
        <asp:Parameter Name="commission_date" Type="DateTime" />
        <asp:Parameter Name="commission_status" Type="String" />
        <asp:Parameter Name="deputies_list" Type="String" />
        <asp:Parameter Name="dkv_head" Type="String" />
        <asp:Parameter Name="district_representatives" Type="String" />
        <asp:Parameter Name="modify_date" Type="DateTime" />
        <asp:Parameter Name="modified_by" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="id" Type="Int32" />
        <asp:Parameter Name="commission_num" Type="String" />
        <asp:Parameter Name="commission_date" Type="DateTime" />
        <asp:Parameter Name="commission_status" Type="String" />
        <asp:Parameter Name="deputies_list" Type="String" />
        <asp:Parameter Name="dkv_head" Type="String" />
        <asp:Parameter Name="district_representatives" Type="String" />
        <asp:Parameter Name="modify_date" Type="DateTime" />
        <asp:Parameter Name="modified_by" Type="String" />
    </UpdateParameters>
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
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Реєстр продовження договорів" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxCheckBox ID="CheckBoxBalansObjectsShowNeziznacheni" runat="server" Checked='False' Text="Публічні" ToolTip="Показувати лише публічні об'єкти"
                Width="80px" ClientInstanceName="CheckBoxBalansObjectsShowNeziznacheni" >
                <ClientSideEvents CheckedChanged="CheckBoxBalansObjectsShowNeziznacheni_CheckedChanged" />
            </dx:ASPxCheckBox>
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

        </td>
        <td>
            <dx:ASPxPopupControl ID="PopupCommissions" runat="server"
                HeaderText="Комісії"
                ClientInstanceName="PopupCommissions"
                Width="1200px"
                PopupElementID="ASPxButtonCommissions"
                PopupAction="None"
                PopupHorizontalAlign="Center"
                PopupVerticalAlign="Middle"
                PopupAnimationType="Slide">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentCommissions" runat="server">
                        <dx:ASPxGridView ID="GridViewCommission" ClientInstanceName="GridViewCommission" runat="server"
                            AutoGenerateColumns="False" DataSourceID="SqlDataSourceCommission" KeyFieldName="id" 
                            OnCustomErrorText="GridViewCommission_CustomErrorText">
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
                                <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowInCustomizationForm="True" CellStyle-Wrap="False"
                                    ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowClearFilterButton="true" ShowEditButton="true" ShowNewButton="true">
                                    <CellStyle Wrap="False"></CellStyle>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="commission_num" Caption="Номер комісії" Width="120px" />
                                <dx:GridViewDataDateColumn FieldName="commission_date" Caption="Дата комісії" Width="120px" />
                                <dx:GridViewDataTextColumn FieldName="commission_status" Caption="Статус комісії" Width="140px" />
                                <dx:GridViewDataMemoColumn FieldName="deputies_list" Caption="Список депутатів, що голосують" Width="320px" Name="colDeputiesList">
                                    <EditItemTemplate>
                                        <table cellpadding="0" cellspacing="0" style="width:100%;">
                                            <tr>
                                                <td style="padding-right:6px; vertical-align:top;">
                                                    <dx:ASPxMemo ID="EditDeputiesListText" runat="server" Width="100%" Height="70px" ReadOnly="true"
                                                        Text='<%# Bind("deputies_list") %>' />
                                                </td>
                                                <td style="width:110px; vertical-align:top;">
                                                    <dx:ASPxButton ID="ButtonEditDeputiesList" runat="server" Text="Редагувати"
                                                        OnClick="ButtonEditDeputiesList_Click" Width="100px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                </dx:GridViewDataMemoColumn>
                                <dx:GridViewDataTextColumn FieldName="dkv_head" Caption="Начальник відділу використання майна ДКВ" Width="220px" />
                                <dx:GridViewDataMemoColumn FieldName="district_representatives" Caption="Представники районів" Width="320px" Name="colDistrictRepresentatives">
                                    <EditItemTemplate>
                                        <table cellpadding="0" cellspacing="0" style="width:100%;">
                                            <tr>
                                                <td style="padding-right:6px; vertical-align:top;">
                                                    <dx:ASPxMemo ID="EditDistrictRepresentativesText" runat="server" Width="100%" Height="70px" ReadOnly="true"
                                                        Text='<%# Bind("district_representatives") %>' />
                                                </td>
                                                <td style="width:110px; vertical-align:top;">
                                                    <dx:ASPxButton ID="ButtonEditDistrictRepresentatives" runat="server" Text="Редагувати"
                                                        OnClick="ButtonEditDistrictRepresentatives_Click" Width="100px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                </dx:GridViewDataMemoColumn>
                            </Columns>

                            <SettingsBehavior ColumnResizeMode="Control" EnableCustomizationWindow="False" ConfirmDelete="True" />
                            <Settings HorizontalScrollBarMode="Auto" ShowFooter="false" VerticalScrollBarMode="Auto" VerticalScrollBarStyle="Standard" />
                            <SettingsEditing NewItemRowPosition="Top" Mode="Inline" />
                            <SettingsPager PageSize="10" />
                            <Styles Header-Wrap="True" />
                            <ClientSideEvents EndCallback="function (s,e) { GridViewCommission.SetHeight(500); }" />
                        </dx:ASPxGridView>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <ClientSideEvents PopUp="function (s,e) { GridViewCommission.SetHeight(500); }" />
            </dx:ASPxPopupControl>

            <dx:ASPxPopupControl ID="PopupDeputiesEditor" runat="server"
                HeaderText="Список депутатів, що голосують"
                ClientInstanceName="PopupDeputiesEditor"
                CloseAction="CloseButton"
                Modal="True"
                Width="700px"
                PopupAction="None"
                PopupHorizontalAlign="Center"
                PopupVerticalAlign="Middle"
                PopupAnimationType="Slide">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentDeputiesEditor" runat="server">
                        <dx:ASPxGridView ID="GridViewDeputiesEditor" runat="server"
                            AutoGenerateColumns="False" KeyFieldName="id" Width="100%"
                            OnDataBinding="GridViewDeputiesEditor_DataBinding"
                            OnRowInserting="GridViewDeputiesEditor_RowInserting"
                            OnRowUpdating="GridViewDeputiesEditor_RowUpdating"
                            OnRowDeleting="GridViewDeputiesEditor_RowDeleting">
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
                            </SettingsCommandButton>
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowInCustomizationForm="True" CellStyle-Wrap="False"
                                    ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowEditButton="true" ShowNewButton="true">
                                    <CellStyle Wrap="False"></CellStyle>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="deputy_name" Caption="ПІБ депутата" Width="520px" />
                            </Columns>
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing NewItemRowPosition="Top" Mode="Inline" />
                            <SettingsPager Mode="ShowAllRecords" />
                        </dx:ASPxGridView>

                        <div style="margin-top:10px; text-align:right;">
                            <dx:ASPxButton ID="ButtonDeputiesEditorOk" runat="server" Text="OK" OnClick="ButtonDeputiesEditorOk_Click" Width="90px" />
                            <dx:ASPxButton ID="ButtonDeputiesEditorCancel" runat="server" Text="Скасувати" OnClick="ButtonDeputiesEditorCancel_Click" Width="90px" style="margin-left:8px;" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxPopupControl ID="PopupDistrictRepresentativesEditor" runat="server"
                HeaderText="Представники районів"
                ClientInstanceName="PopupDistrictRepresentativesEditor"
                CloseAction="CloseButton"
                Modal="True"
                Width="850px"
                PopupAction="None"
                PopupHorizontalAlign="Center"
                PopupVerticalAlign="Middle"
                PopupAnimationType="Slide">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentDistrictRepresentativesEditor" runat="server">
                        <dx:ASPxGridView ID="GridViewDistrictRepresentativesEditor" runat="server"
                            AutoGenerateColumns="False" KeyFieldName="id" Width="100%"
                            OnDataBinding="GridViewDistrictRepresentativesEditor_DataBinding"
                            OnRowUpdating="GridViewDistrictRepresentativesEditor_RowUpdating">
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
                            </SettingsCommandButton>
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowInCustomizationForm="True" CellStyle-Wrap="False"
                                    ShowDeleteButton="False" ShowCancelButton="true" ShowUpdateButton="true" ShowEditButton="true" ShowNewButton="false">
                                    <CellStyle Wrap="False"></CellStyle>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="district_name" Caption="Район" Width="220px" ReadOnly="True" />
                                <dx:GridViewDataTextColumn FieldName="representative_name" Caption="Представник району" Width="500px" />
                            </Columns>
                            <SettingsBehavior ConfirmDelete="False" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsPager Mode="ShowAllRecords" />
                        </dx:ASPxGridView>

                        <div style="margin-top:10px; text-align:right;">
                            <dx:ASPxButton ID="ButtonDistrictRepresentativesEditorOk" runat="server" Text="OK" OnClick="ButtonDistrictRepresentativesEditorOk_Click" Width="90px" />
                            <dx:ASPxButton ID="ButtonDistrictRepresentativesEditorCancel" runat="server" Text="Скасувати" OnClick="ButtonDistrictRepresentativesEditorCancel_Click" Width="90px" style="margin-left:8px;" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxPopupControl ID="PopupVotingEditor" runat="server"
                HeaderText="Голосування"
                ClientInstanceName="PopupVotingEditor"
                CloseAction="CloseButton"
                Modal="True"
                Width="900px"
                PopupAction="None"
                PopupHorizontalAlign="Center"
                PopupVerticalAlign="Middle"
                PopupAnimationType="Slide">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentVotingEditor" runat="server">
                        <dx:ASPxGridView ID="GridViewVotingEditor" runat="server"
                            AutoGenerateColumns="False" KeyFieldName="id" Width="100%"
                            OnDataBinding="GridViewVotingEditor_DataBinding"
                            OnRowUpdating="GridViewVotingEditor_RowUpdating">
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
                            </SettingsCommandButton>
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowInCustomizationForm="True" CellStyle-Wrap="False"
                                    ShowDeleteButton="False" ShowCancelButton="true" ShowUpdateButton="true" ShowEditButton="true" ShowNewButton="false">
                                    <CellStyle Wrap="False"></CellStyle>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="deputy_name" Caption="Депутат" Width="320px" ReadOnly="True" />
                                <dx:GridViewDataComboBoxColumn FieldName="vote_value" Caption="Голос" Width="220px">
                                    <PropertiesComboBox DropDownStyle="DropDownList" NullText="">
                                        <Items>
                                            <dx:ListEditItem Text="За" Value="За" />
                                            <dx:ListEditItem Text="Проти" Value="Проти" />
                                            <dx:ListEditItem Text="Утримався" Value="Утримався" />
                                            <dx:ListEditItem Text="Не голосував" Value="Не голосував" />
                                        </Items>
                                    </PropertiesComboBox>
                                </dx:GridViewDataComboBoxColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="False" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsPager Mode="ShowAllRecords" />
                        </dx:ASPxGridView>

                        <div style="margin-top:10px; text-align:right;">
                            <dx:ASPxButton ID="ButtonVotingEditorOk" runat="server" Text="OK" OnClick="ButtonVotingEditorOk_Click" Width="90px" />
                            <dx:ASPxButton ID="ButtonVotingEditorCancel" runat="server" Text="Скасувати" OnClick="ButtonVotingEditorCancel_Click" Width="90px" style="margin-left:8px;" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButtonCommissions" ClientInstanceName="ASPxButtonCommissions" runat="server" Text="Комісії" AutoPostBack="false" Width="100px">
                <ClientSideEvents Click="function (s,e) { PopupCommissions.Show(); }" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButtonCommissionProrydok" runat="server" Text="Порядок денний (таб.)"
                Width="170px" OnClick="ASPxButtonCommissionProrydok_Click">
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButtonCommissionProrydokText" runat="server" Text="Порядок денний (текст)"
                Width="180px" OnClick="ASPxButtonCommissionProrydokText_Click">
            </dx:ASPxButton>
        </td>
        <td>
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
        OnProcessColumnAutoFilter="GridViewFreeSquare_ProcessColumnAutoFilter"
        OnFillContextMenuItems="FreeSquareGridView_FillContextMenuItems" >
	   <ClientSideEvents CustomButtonClick="ShowPhoto" ContextMenuItemClick="OnContextMenuItemClick" />

       <SettingsContextMenu Enabled="true"/>

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
        <dx:GridViewCommandColumn VisibleIndex="0" Width="85px" ButtonType="Image" CellStyle-Wrap="True" FixedStyle="Left" CellStyle-CssClass="command-column-class" 
            ShowCancelButton="true" ShowUpdateButton="true" ShowEditButton="true" >
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="btnPdfBuild" Text="Pdf"> 
					<Image Url="~/Styles/PdfReportIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnMapShow" Text="Показати на мапі"> 
					<Image Url="~/Styles/MapShowIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnOrgBalansObject" Text="Змінити картку"> 
					<Image Url="~/Styles/EditTextIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnFreeCycle" Text="Картка процесу передачі в оренду вільного приміщення" Visibility="Invisible"> 
					<Image Url="~/Styles/ReportDocument18.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnCopyFullDescription" Text="Опис об'єкта до буфера обміну"> 
					<Image Url="~/Styles/CopyIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnDocx1Build" Text="Оголошення про продовження договорів оренди на аукціоні"> 
					<Image Url="~/Styles/ribbonicon_help_4.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnDocx2Build" Text="Проект договору оренди"> 
					<Image Url="~/Styles/report_101.png"/>
                </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnCommissionLetterBuild" Text="Лист на комісію"> 
					<Image Url="~/Styles/letter18.png"/>
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

        <dx:GridViewDataTextColumn FieldName="agreement_num" Caption="Номер договору" VisibleIndex="1" Width="80px" ReadOnly="true" CellStyle-HorizontalAlign="Left">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("agreement_num") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
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

        <dx:GridViewDataCheckColumn FieldName="is_included" Caption="Включено до переліку продовження договорів" VisibleIndex="4" Width ="40px">
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
                <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування Б" VisibleIndex="13" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("modify_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataDateColumn>
				<dx:GridViewDataTextColumn FieldName="modified_by" Caption="Користувач Б" VisibleIndex="13" Width="80px" ReadOnly="true">
					<EditItemTemplate>
						<dx:ASPxLabel runat="server" Text='<%# Eval("modified_by") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
					</EditItemTemplate>
                </dx:GridViewDataTextColumn>
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

        <dx:GridViewDataTextColumn FieldName="orend_plat_dogovor" Caption="Місячна орендна плата за договором" VisibleIndex="24" Width="80px"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("orend_plat_dogovor") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
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

        <%--<dx:GridViewDataTextColumn FieldName="may_pravo_prodov_text" Caption="Цільове використання" VisibleIndex="24" Width="230px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("may_pravo_prodov_text") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        	<dx:GridViewDataComboBoxColumn FieldName="may_pravo_prodov_text" Caption="Цільове використання" VisibleIndex="24" Width="230px">
            <PropertiesComboBox 
				DataSourceID="SqlDataSourceMayPravoProdov"
				DropDownStyle="DropDownList"
				DropDownWidth="500px"
				TextField="name"  
				ValueField="id">
            </PropertiesComboBox>  
        </dx:GridViewDataComboBoxColumn>

        <dx:GridViewDataTextColumn FieldName="polipshanya_vartist" Caption="Вартість здійснених чинним орендарем невід’ємних поліпшень" VisibleIndex="24" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("polipshanya_vartist") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="rozmir_vidshkoduv" Caption="Розмір відшкодування земельного податку та інших" VisibleIndex="24" Width="100px"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("rozmir_vidshkoduv") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>


        <dx:GridViewDataColumn FieldName="polipshanya_finish_date" Caption="Дата завершення здійснених чинним орендарем невід’ємних поліпшень" VisibleIndex="24" Width="80px" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
                <dx:ASPxLabel runat="server" Text='<%# Eval("polipshanya_finish_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataColumn>

		<dx:GridViewDataTextColumn FieldName="primitki" Caption="Примітки" VisibleIndex="24" Width="120px" ReadOnly="true" >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("primitki") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>


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
        <dx:GridViewDataTextColumn FieldName="modified_by2" Caption="Користувач О" VisibleIndex="510" Width="100px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("modified_by2") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="zal_balans_vartist" Caption="Залишкова балансова вартість, грн." VisibleIndex="1110" Visible="false" >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zal_balans_vartist") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="perv_balans_vartist" Caption="Первісна балансова вартість, грн." VisibleIndex="1120" Visible="false" >
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

        <dx:GridViewDataDateColumn FieldName="zalbalansvartist_date" Caption="Дата формування залишкової вартості" VisibleIndex="1340" Visible="false" Width="100px"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("zalbalansvartist_date") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataDateColumn>

        <dx:GridViewDataTextColumn FieldName="osoba_oznakoml" Caption="Особа відповідальна за ознайомлення з об’єктом" VisibleIndex="1350" Visible="false" Width="220px"  >
            <HeaderStyle Wrap="True" />
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("osoba_oznakoml") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn FieldName="building_type" Caption="Тип будинку" VisibleIndex="1360" Width="180px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="category" Caption="Категорія" VisibleIndex="1370" Width="180px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataSpinEditColumn FieldName="rental_rate_percent" Caption="Орендна ставка, %" VisibleIndex="1380" Width="120px" CellStyle-HorizontalAlign="Right">
            <HeaderStyle Wrap="True" />
            <PropertiesSpinEdit NumberType="Float" DecimalPlaces="2" DisplayFormatString="g29" />
        </dx:GridViewDataSpinEditColumn>

        <dx:GridViewDataTextColumn FieldName="rental_type" Caption="Тип оренди" VisibleIndex="1390" Width="180px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="rental_term" Caption="Строк / термін оренди" VisibleIndex="1400" Width="180px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="commission_note" Caption="Примітка" VisibleIndex="1410" Width="220px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="additional_info" Caption="Додаткова інформація" VisibleIndex="1420" Width="220px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="speaker_name" Caption="Доповідач" VisibleIndex="1430" Width="180px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="incoming_doc_num" Caption="Вхідний номер звернення" VisibleIndex="1440" Width="180px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="incoming_doc_date" Caption="Дата вхідного звернення" VisibleIndex="1450" Width="120px" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataDateColumn>

        <dx:GridViewDataTextColumn FieldName="letter_appendix_num" Caption="Додаток до листа. Номер" VisibleIndex="1460" Width="180px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="letter_appendix_date" Caption="Додаток до листа. Дата" VisibleIndex="1470" Width="120px" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataDateColumn>

        <dx:GridViewDataComboBoxColumn FieldName="commission_id" Caption="Номер комісії" VisibleIndex="1480" Width="140px" Name="colCommissionId">
            <HeaderStyle Wrap="True" />
            <PropertiesComboBox
                DataSourceID="SqlDataSourceCommission"
                TextField="commission_num"
                ValueField="id"
                ValueType="System.Int32"
                DropDownStyle="DropDownList"
                NullText="">
            </PropertiesComboBox>
            <DataItemTemplate>
                <asp:Label ID="LabelCommissionNum" runat="server" Text='<%# Eval("commission_num") %>' />
            </DataItemTemplate>
            <EditItemTemplate>
                <dx:ASPxComboBox ID="EditCommissionId" runat="server" Width="100%"
                    DataSourceID="SqlDataSourceCommission"
                    TextField="commission_num"
                    ValueField="id"
                    ValueType="System.Int32"
                    DropDownStyle="DropDownList"
                    NullText=""
                    OnInit="EditCommissionId_Init"
                    Value='<%# Bind("commission_id") %>'>
                </dx:ASPxComboBox>
            </EditItemTemplate>
        </dx:GridViewDataComboBoxColumn>

        <dx:GridViewDataTextColumn FieldName="commission_result" Caption="Результат" VisibleIndex="1490" Width="120px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataSpinEditColumn FieldName="protocol_question_num" Caption="№ питання у протоколі" VisibleIndex="1500" Width="120px" CellStyle-HorizontalAlign="Right">
            <HeaderStyle Wrap="True" />
            <PropertiesSpinEdit NumberType="Integer" DisplayFormatString="g" />
        </dx:GridViewDataSpinEditColumn>

        <dx:GridViewDataTextColumn FieldName="outgoing_doc_num" Caption="Вихідний номер звернення" VisibleIndex="1510" Width="180px">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="outgoing_doc_date" Caption="Дата вихідного звернення" VisibleIndex="1520" Width="120px" CellStyle-HorizontalAlign="Center">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataDateColumn>

        <dx:GridViewDataMemoColumn FieldName="slukhali_text" Caption="СЛУХАЛИ" VisibleIndex="1530" Width="280px">
            <HeaderStyle Wrap="True" />
            <PropertiesMemoEdit Rows="3" />
        </dx:GridViewDataMemoColumn>

        <dx:GridViewDataMemoColumn FieldName="virishyly_text" Caption="ВИРІШИЛИ" VisibleIndex="1540" Width="280px">
            <HeaderStyle Wrap="True" />
            <PropertiesMemoEdit Rows="3" />
        </dx:GridViewDataMemoColumn>

        <dx:GridViewDataMemoColumn FieldName="golosovanie" Caption="Голосування" VisibleIndex="1550" Width="320px" Name="colGolosovanie">
            <HeaderStyle Wrap="True" />
            <EditItemTemplate>
                <table cellpadding="0" cellspacing="0" style="width:100%;">
                    <tr>
                        <td style="padding-right:6px; vertical-align:top;">
                            <dx:ASPxMemo ID="EditGolosovanieText" runat="server" Width="100%" Height="90px" ReadOnly="true"
                                Text='<%# Bind("golosovanie") %>' />
                        </td>
                        <td style="width:110px; vertical-align:top;">
                            <dx:ASPxButton ID="ButtonEditGolosovanie" runat="server" Text="Редагувати"
                                OnClick="ButtonEditGolosovanie_Click" Width="100px" />
                        </td>
                    </tr>
                </table>
            </EditItemTemplate>
        </dx:GridViewDataMemoColumn>




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
    <SettingsCookies CookiesID="GUKV.Reports1NF.Report1NFDogContinue" Version="A3_44" Enabled="true" />
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

