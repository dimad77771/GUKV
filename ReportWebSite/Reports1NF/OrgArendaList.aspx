<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgArendaList.aspx.cs" Inherits="Reports1NF_OrgArendaList"
    MasterPageFile="~/NoMenu.master" Title="Договори Оренди" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

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

<script type="text/javascript" src="../Scripts/PageScript.js"></script>
<script type="text/javascript" language="javascript">

    document.addEventListener("DOMContentLoaded", ready);

    function ready(event) {
        var theControl = document.getElementById('<%=ASPxLabelState.ClientID %>');
        theControl.style.display = 'none';
      }

    // <![CDATA[

    window.onresize = function () { AdjustGridSize(); };

    function AdjustGridSize() {

        PrimaryGridView.SetHeight(window.innerHeight - 180);
    }

    function PrimaryGridViewEndCallback(s, e) {

        AdjustGridSize();

        CPStatus.PerformCallback('');        
    }

    function CustomButtonClick(s, e) {

        if (e.buttonID == 'btnEditAgreement') {

            var cardUrl = "../Reports1NF/OrgRentAgreement.aspx?rid=" + <%= Request.QueryString["rid"] %> + "&aid=" + PrimaryGridView.GetRowKey(e.visibleIndex);

            window.location = cardUrl;
        }

        if (e.buttonID == 'btnDeleteAgreement') {

            if (confirm("Видалити Договір Оренди?")) {
                PrimaryGridView.PerformCallback(JSON.stringify({ AgreementToDeleteID: PrimaryGridView.GetRowKey(e.visibleIndex) }));
            }
        }

        e.processOnServer = false;
    }

    function OnAddNewAgreement(s, e) {

        var agreementNum = EditAgreementNum.GetText();
        var agreementDate = EditAgreementDate.GetValue();
        var buildingId = ComboBuilding.GetValue();
        var orgBalansId = ComboBalansOrg.GetValue();
        var orgGiverId = ComboGiverOrg.GetValue();
        var orgRenterId = ComboRenterOrg.GetValue();
        var giverComment = EditGiverComment.GetText();

        if (agreementNum == "") {
            alert("Будь ласка, введіть номер Договору оренди.");
            return;
        }

        if (agreementDate == null || agreementDate == undefined) {
            alert("Будь ласка, введіть дату Договору оренди.");
            return;
        }

        if (buildingId == null || buildingId == undefined) {
            alert("Будь ласка, виберіть адресу, за якою розташовано орендоване приміщення.");
            return;
        }

        if (orgBalansId == null || orgBalansId == undefined) {
            alert("Будь ласка, виберіть організацію-балансоутримувача приміщення, що надається в оренду.");
            return;
        }

        if (orgGiverId == null || orgGiverId == undefined) {
            alert("Будь ласка, виберіть організацію-орендодавця.");
            return;
        }

        if (orgRenterId == null || orgRenterId == undefined) {
            alert("Будь ласка, виберіть організацію-орендаря.");
            return;
        }

        if (orgGiverId != 27065 && giverComment == "") {
            alert("Будь ласка, введіть коментар з приводу того, чому орендодавцем за договором не є Департамент Комунальної Власності м. Києва.");
            return;
        }

        PopupAddAgreement.Hide();

        PrimaryGridView.PerformCallback(JSON.stringify({
            AgreementNum: agreementNum,
	        AgreementDateYear: agreementDate.getFullYear(),
            AgreementDateMonth: agreementDate.getMonth(),
            AgreementDateDay: agreementDate.getDate(),
            BuildingID: buildingId,
            OrgBalansID: orgBalansId,
            OrgRenterID: orgRenterId,
            OrgGiverID: orgGiverId,
            OrgGiverComment: giverComment
        }));
    }

    // TODO!!! - УБРАТЬ ЗНАЧЕНИЯ ИДЕНТИФИКАТОРА ИЗ КОДА
    function OnComboGiverOrgSelIndexChanged(s, e) {

        var orgGiverId = ComboGiverOrg.GetValue();

        if (orgGiverId != null && orgGiverId != undefined && orgGiverId != 27065) {
            LabelGiverComment.SetVisible(true);
            EditGiverComment.SetVisible(true);
        }
        else {
            LabelGiverComment.SetVisible(false);
            EditGiverComment.SetVisible(false);
        }
    }

    function ShowSendingLog() {
        var diagnosticWindow = window.open("SendingRentingAgreementsDiagnostic.aspx?rid=<%= ReportID  %>","","width=600,height=400");        
        diagnosticWindow.focus();        
    }
    // ]]>

    function ButtonSendAllClick() {
        alert(document.getElementById('<%=ASPxLabelState.ClientID %>').innerHTML);
    }

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT CASE WHEN COUNT(*) > 0 THEN 0 ELSE 1 END AS 'report_arenda_status'
        FROM reports1nf_arenda ar
        INNER JOIN reports1nf rep ON rep.id = ar.report_id
        LEFT OUTER JOIN arenda a ON a.id = ar.id
        WHERE ar.report_id = @rep_id AND isnull(ar.is_deleted,0) = 0 AND ((ar.submit_date IS NULL OR ar.modify_date > ar.submit_date))">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceReportArenda" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT ar.id AS 'arenda_id'
, ar.agreement_num
, ar.agreement_date
, ar.rent_start_date
, ar.rent_finish_date
, org.full_name AS 'renter_name'
, dict_districts2.name AS 'district'
, bld.addr_street_name
, bld.addr_nomer
, ar.rent_square
, COALESCE(ar.purpose_str, dict_balans_purpose.name) AS 'purpose'
, ar.is_deleted
, ar.modify_date
, ar.submit_date
, a.id AS 'id_in_eis'
, a.is_deleted AS 'is_deleted_in_eis'
, org_giver.full_name AS 'giver_name'

--, CASE WHEN ((ar.submit_date IS NULL OR ar.modify_date > ar.submit_date)) THEN N'НІ' ELSE N'ТАК' END AS 'is_submitted'
, CASE WHEN (ap.rent_period_id <> apd.period_id or (ar.submit_date IS NULL OR ar.modify_date > ar.submit_date)) THEN N'НІ' ELSE N'ТАК' END AS 'is_submitted'

,(CASE WHEN ar.agreement_state = 1 THEN 'Договір діє' ELSE CASE WHEN ar.agreement_state = 2 THEN 'Договір закінчився, але заборгованність не погашено' ELSE CASE WHEN ar.agreement_state = 3 THEN 'Договір закінчився, оренда продовжена іншим договором' ELSE '' END END END) AS 'agreement_state'
, ar.modified_by

,ap.sqr_payed_by_percent	
,ap.sqr_payed_by_1uah	
,ap.sqr_payed_hourly	
,ap.payment_narah	
,ap.last_year_saldo	
,ap.payment_received	
,ap.payment_nar_zvit
,ap.debt_total	
,ap.debt_zvit	
,ap.debt_3_month	
,ap.debt_12_month	
,ap.debt_3_years	
,ap.debt_over_3_years	
,ap.debt_v_mezhah_vitrat	
,ap.debt_spysano	
,ap.num_zahodiv_total	
,ap.num_zahodiv_zvit	
,ap.num_pozov_total	
,ap.num_pozov_zvit	
,ap.num_pozov_zadov_total	
,ap.num_pozov_zadov_zvit	
,ap.num_pozov_vikon_total	
,ap.num_pozov_vikon_zvit	
,ap.debt_pogasheno_total	
,ap.debt_pogasheno_zvit
,ap.old_debts_payed
,ap.znyato_nadmirno_narah
,ap.avance_plat
,ap.use_calc_debt
,ap.return_all_orend_payed
,ap.return_orend_payed
,isnull(ap.payment_narah,0) - isnull(ap.znyato_nadmirno_narah,0) as payment_narah_normal

,dpt.name AS 'payment_type'
      ,[org].[zkpo_code] AS 'org_renter_zkpo'
      ,[org_giver].[zkpo_code] AS 'org_giver_zkpo'
    ,isnull(ad.pidstava+' ','')+ isnull(ad.doc_num+' ','')+ isnull('від '+ad.doc_date+' ','')+ad.purpose_str as priznachennya
,ar.insurance_sum
,ar.insurance_start
,ar.insurance_end,

zvilneno_percent,zvilneno_date1,zvilneno_date2,
zvilbykmp_percent,zvilbykmp_date1,zvilbykmp_date2,
povidoleno1_date,povidoleno1_num,
povidoleno2_date,povidoleno2_num,
povidoleno3_date,povidoleno3_num,
povidoleno4_date,povidoleno4_num


FROM reports1nf_arenda ar
        INNER JOIN reports1nf rep ON rep.id = ar.report_id
        LEFT OUTER JOIN arenda a ON a.id = ar.id
        LEFT OUTER JOIN organizations org ON org.id = ar.org_renter_id and (org.is_deleted is null or org.is_deleted = 0)
        LEFT OUTER JOIN organizations org_giver ON org_giver.id = ar.org_giver_id and (org_giver.is_deleted is null or org_giver.is_deleted = 0)
        LEFT OUTER JOIN reports1nf_buildings bld ON bld.unique_id = ar.building_1nf_unique_id
        LEFT OUTER JOIN dict_balans_purpose ON dict_balans_purpose.id = ar.purpose_id
        LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = bld.addr_distr_new_id
--       LEFT JOIN (SELECT report_id,MAX(rent_period_id) AS 'max_rent_period_id' FROM reports1nf_arenda_payments group by report_id) mrp on mrp.report_id = rep.id 
       LEFT JOIN (SELECT arenda_id, MAX(rent_period_id) AS 'max_rent_period_id' FROM reports1nf_arenda_payments group by arenda_id) mrp on mrp.arenda_id = a.id 

--        left join dbo.reports1NF_arenda_payments ap on ap.arenda_id = a.id and  ap.rent_period_id = (select max(rent_period_id) from dbo.reports1NF_arenda_payments where arenda_id = a.id)  /*and ap.rent_period_id = mrp.max_rent_period_id*/   /* (select top 1 id from dict_rent_period per where per.is_active = 1) */
        left join dbo.reports1NF_arenda_payments ap on ap.arenda_id = a.id and ap.rent_period_id = mrp.max_rent_period_id

        LEFT JOIN dict_arenda_payment_type dpt ON a.payment_type_id = dpt.id

/*        left join (select report_id,arenda_id,purpose_str, pidstava, doc_num  from dbo.reports1nf_arenda_decisions t1 where id = (select top 1 id from dbo.reports1nf_arenda_decisions t2 where t2.arenda_id = t1.arenda_id)) ad on ad.arenda_id = ar.id and ad.report_id = rep.id */
        outer apply (select top 1 report_id,arenda_id,purpose_str, pidstava, doc_num, doc_date=convert(varchar(10),doc_date, 104)  from dbo.reports1nf_arenda_decisions t1 where t1.arenda_id = ar.id and t1.report_id = rep.id order by t1.id) ad 

		outer apply (select top 1 id as period_id from dict_rent_period per where per.is_active = 1) apd

        WHERE ar.report_id = @rep_id AND isnull(a.is_deleted, 0) = 0 ORDER BY ar.modify_date DESC" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStreets" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select s.id, s.name as sname, r.name as rname,  s.name as name from dict_streets s left join dict_regions r on r.id = s.region_id where (not s.name is null) and (RTRIM(LTRIM(s.name)) <> '')">
<%--    SelectCommand="select s.id, s.name as sname, r.name as rname, ISNULL(r.name + ' - ', '') + s.name as name from dict_streets s left join dict_regions r on r.id = s.region_id where (not s.name is null) and (RTRIM(LTRIM(s.name)) <> '')">	--%>

<%--    SelectCommand="select id, name from dict_streets where (not name is null) and (RTRIM(LTRIM(name)) <> '')">	--%>

</mini:ProfiledSqlDataSource>


<%-- убрано условие после where (id < 100000) AND--%>
<mini:ProfiledSqlDataSource ID="SqlDataSourceDictBuildings" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, LTRIM(RTRIM(addr_nomer)) AS 'nomer' from buildings where 
        (is_deleted IS NULL OR is_deleted = 0) AND
       /* (master_building_id IS NULL) AND*/
        addr_street_id = @street_id AND
        (RTRIM(LTRIM(addr_nomer)) <> '') ORDER BY RTRIM(LTRIM(addr_nomer))"
    OnSelecting="SqlDataSourceDictBuildings_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="street_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearchBalans" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 20 id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations WHERE zkpo_code LIKE @zkpo AND full_name LIKE @fname AND
        (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL"
    OnSelecting="SqlDataSourceOrgSearchBalans_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearchGiver" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 20 id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations WHERE
        (is_deleted IS NULL OR is_deleted = 0) AND (master_org_id IS NULL) AND
        ((@balans_org = 0 AND zkpo_code LIKE @zkpo AND full_name LIKE @fname) OR
         (@balans_org > 0 AND id IN (@balans_org, 27065, 8515,8400,1728,8109,9826,15130,141824,137676,8566,308543, 99405315)))
        ORDER BY full_name"
    OnSelecting="SqlDataSourceOrgSearchGiver_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="balans_org" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearchRenter" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 20 id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations WHERE zkpo_code LIKE @zkpo AND full_name LIKE @fname AND
        (is_deleted IS NULL OR is_deleted = 0) --AND master_org_id IS NULL"
    OnSelecting="SqlDataSourceOrgSearchRenter_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_status ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgFormGosp" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_form_gosp ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgOwnership" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_org_ownership where len(name) > 0 order by name"
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

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictDistricts2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_districts2 ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
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

<p class="SpacingPara"/>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Перелік договорів  надання об'єктів іншим організаціям" CssClass="pagetitle"/>
</p>

<table border="0" cellspacing="0" cellpadding="2">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonAddAgreement" runat="server" Text="Додати Новий Договір" AutoPostBack="false"></dx:ASPxButton>

            <dx:ASPxPopupControl ID="PopupAddAgreement" runat="server" ClientInstanceName="PopupAddAgreement"
                HeaderText="Введення нового Договору надання в оренду" PopupElementID="ButtonAddAgreement">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Реквізити Договору Оренди" Width="100%">
                            <ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server">

                                    <table border="0" cellspacing="0" cellpadding="2" width="100%">
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Номер Договору:" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditAgreementNum" ClientInstanceName="EditAgreementNum" runat="server" Width="100%" MaxLength ="18"/> </td>
                                        </tr>
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Дата Договору:" /> </td>
                                            <td> <dx:ASPxDateEdit ID="EditAgreementDate" ClientInstanceName="EditAgreementDate" runat="server" Width="100%" /> </td>
                                        </tr>
                                    </table>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="Адреса Приміщення">
                            <ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent5" runat="server">

                                    <table border="0" cellspacing="0" cellpadding="2">
                                        <tr>
                                            <td> <dx:ASPxLabel ID="LabelAddrStreet" runat="server" Text="Назва вулиці:" Width="90px" /> </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ComboStreet" runat="server" ClientInstanceName="ComboStreet"
                                                    DataSourceID="SqlDataSourceDictStreets" DropDownStyle="DropDownList" ValueType="System.Int32"
                                                    TextField="name" ValueField="id" Width="220px" IncrementalFilteringMode="StartsWith"
                                                    FilterMinLength="3" EnableCallbackMode="True" CallbackPageSize="50" EnableViewState="False"
                                                    EnableSynchronization="False">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { ComboBuilding.PerformCallback(ComboStreet.GetValue().toString()); }" />
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td> <dx:ASPxLabel ID="LabelAddrPickerNumber" runat="server" Text="Номер будинку:" Width="95px" /> </td>
                                            <td>
                                                <dx:ASPxComboBox runat="server" ID="ComboBuilding" ClientInstanceName="ComboBuilding"
                                                    DataSourceID="SqlDataSourceDictBuildings" DropDownStyle="DropDownList" TextField="nomer"
                                                    ValueField="id" ValueType="System.Int32" Width="100px" IncrementalFilteringMode="StartsWith"
                                                    EnableSynchronization="False" OnCallback="ComboBuilding_Callback">
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="PanelOrgBalans" runat="server" HeaderText="Балансоутримувач">
                            <ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server">

                                    <table border="0" cellspacing="0" cellpadding="2">
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel88" runat="server" Text="Код ЄДРПОУ:" Width="90px" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditBalansOrgZKPO" ClientInstanceName="EditBalansOrgZKPO" runat="server" Width="120px" /> </td>
                                            <td> <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Назва:" Width="50px" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditBalansOrgName" ClientInstanceName="EditBalansOrgName" runat="server" Width="180px" /> </td>
                                            <td>
                                                <dx:ASPxButton ID="BtnFindBalansOrg" ClientInstanceName="BtnFindBalansOrg" runat="server" AutoPostBack="False" Text="Знайти">
                                                    <ClientSideEvents Click="function (s, e) { ComboBalansOrg.PerformCallback(EditBalansOrgZKPO.GetText() + '|' + EditBalansOrgName.GetText()); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <dx:ASPxComboBox ID="ComboBalansOrg" ClientInstanceName="ComboBalansOrg" runat="server"
                                                    Width="100%" DataSourceID="SqlDataSourceOrgSearchBalans" ValueField="id" ValueType="System.Int32"
                                                    TextField="search_name" EnableSynchronization="True" OnCallback="ComboBalansOrg_Callback">
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Орендодавець">
                            <ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">

                                    <table border="0" cellspacing="0" cellpadding="2">
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Код ЄДРПОУ:" Width="90px" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditGiverOrgZKPO" ClientInstanceName="EditGiverOrgZKPO" runat="server" Width="120px" /> </td>
                                            <td> <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Назва:" Width="50px" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditGiverOrgName" ClientInstanceName="EditGiverOrgName" runat="server" Width="180px" /> </td>
                                            <td>
                                                <dx:ASPxButton ID="BtnFindGiverOrg" ClientInstanceName="BtnFindGiverOrg" runat="server" AutoPostBack="False" Text="Знайти">
                                                    <ClientSideEvents Click="function (s, e) { ComboGiverOrg.PerformCallback(EditGiverOrgZKPO.GetText() + '|' + EditGiverOrgName.GetText()); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <dx:ASPxComboBox ID="ComboGiverOrg" ClientInstanceName="ComboGiverOrg" runat="server"
                                                    Width="100%" DataSourceID="SqlDataSourceOrgSearchGiver" ValueField="id" ValueType="System.Int32"
                                                    TextField="search_name" EnableSynchronization="True" OnCallback="ComboGiverOrg_Callback">
                                                    <ClientSideEvents SelectedIndexChanged="OnComboGiverOrgSelIndexChanged" />
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <dx:ASPxLabel ID="LabelGiverComment" ClientInstanceName="LabelGiverComment" runat="server" Text="Будь ласка, прокоментуйте причину вибору іншого орендодавця:" Width="100%" ClientVisible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <dx:ASPxTextBox ID="EditGiverComment" ClientInstanceName="EditGiverComment" runat="server" Width="100%" ClientVisible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
В договорах оренди укладених після 01.10.2011 згідно з Рішенням №34/6250 орендодавцем виступає: <br/>
<br/>
а) ДКВ (для майна, що перебуває в сфері управління міста),<br/>
б) РДА (для майна, що перебуває в сфері управління району),<br/>
в) Балансоутримувач, за умови, що площа на балансі не перевищує 200 кв.м.
                                            </td>
                                        </tr>
                                    </table>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Орендар">
                            <ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                <dx:ASPxCallbackPanel ID="CPOrgEditor" ClientInstanceName="CPOrgEditor" runat="server" OnCallback="CPOrgEditor_Callback">
                                                    <PanelCollection>
                                                    <dx:panelcontent ID="Panelcontent8" runat="server">
                                    <table border="0" cellspacing="0" cellpadding="2">
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Код ЄДРПОУ:" Width="90px" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditRenterOrgZKPO" ClientInstanceName="EditRenterOrgZKPO" runat="server" Width="120px" /> </td>
                                            <td> <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Назва:" Width="50px" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditRenterOrgName" ClientInstanceName="EditRenterOrgName" runat="server" Width="180px" ReadOnly="true" /> </td>
                                            <td align="right">
                                                <dx:ASPxButton ID="BtnFindRenterOrg" ClientInstanceName="BtnFindRenterOrg" runat="server" AutoPostBack="False" Text="Знайти">
                                                    <ClientSideEvents Click="function (s, e) { ComboRenterOrg.PerformCallback(EditRenterOrgZKPO.GetText() + '|' + EditRenterOrgName.GetText()); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" align="right">
                                                
                                                        <dx:ASPxPopupControl ID="PopupAddRenterOrg" runat="server" ClientInstanceName="PopupAddRenterOrg"
                                                        HeaderText="Створення Організації" PopupElementID="BtnCreateRenterOrg">
                                                        <ContentCollection>
                                                            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">     
                                                            
                                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" HeaderText="Основні відомості" Width="100%">
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent9" runat="server">
                                                                        <table border="0" cellspacing="0" cellpadding="2">
                                                                            <tr>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Повна Назва:" Width="85px" /> </td>
                                                                                <td colspan="3"> <dx:ASPxTextBox ID="TextBoxFullNameOrg" ClientInstanceName="TextBoxFullNameOrg" runat="server" Width="100%" /> </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Код ЄДРПОУ:" Width="85px" /> </td>
                                                                                <td> <dx:ASPxTextBox ID="TextBoxZkpoCodeOrg" ClientInstanceName="TextBoxZkpoCodeOrg" runat="server" Width="90px" /> </td>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Коротка Назва" Width="85px" /> </td>
                                                                                <td> <dx:ASPxTextBox ID="TextBoxShortNameOrg" runat="server" Width="290px" ClientInstanceName="TextBoxShortNameOrg" /> </td>
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
                                                                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="120px" 
                                                                                        IncrementalFilteringMode="StartsWith" 
                                                                                        DataSourceID="SqlDataSourceDictDistricts2" /> </td>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Назва Вулиці" Width="84px" /> </td>
                                                                                <td colspan="3"> 
                                                                                    <dx:ASPxTextBox ID="TextBoxStreetNameOrg" runat="server" ClientInstanceName="TextBoxStreetNameOrg"
                                                                                        Width="100%" Title="" Value='<%# Eval("addr_street_name") %>' />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Номер Будинку" /> </td>
                                                                                <td> <dx:ASPxTextBox ID="TextBoxAddrNomerOrg" runat="server" Text="" Width="120px" ClientInstanceName="TextBoxAddrNomerOrg" /> </td>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabelCorp" runat="server" Text="Корпус" /> </td>
                                                                                <td> <dx:ASPxTextBox ID="TextBoxAddrKorpusFrom" runat="server" Text="" Width="80px" ClientInstanceName="TextBoxAddrKorpusFrom" /> </td>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabelZip" runat="server" Text="Пошт. Індекс" Width="85px" /> </td>
                                                                                <td> <dx:ASPxTextBox ID="TextBoxAddrZipCodeOrg" runat="server" Width="80px" ClientInstanceName="TextBoxAddrZipCodeOrg" /> </td>
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
                                                                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" 
                                                                                        IncrementalFilteringMode="StartsWith" 
                                                                                        DataSourceID="SqlDataSourceDictOrgStatus" /> </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Форма Власності" Width="160px" /> </td>
                                                                                <td> <dx:ASPxComboBox ID="ComboBoxFormVlasnOrg" runat="server" ClientInstanceName="ComboBoxFormVlasnOrg" 
                                                                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" 
                                                                                        IncrementalFilteringMode="StartsWith" 
                                                                                        DataSourceID="SqlDataSourceDictOrgOwnership" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Галузь" Width="160px" /> </td>
                                                                                <td> <dx:ASPxComboBox ID="ComboBoxIndustryFrom" runat="server" ClientInstanceName="ComboBoxIndustryFrom" 
                                                                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" 
                                                                                        IncrementalFilteringMode="StartsWith" 
                                                                                        DataSourceID="SqlDataSourceDictOrgIndustry" /> </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Вид Діяльності" Width="160px" /> </td>
                                                                                <td> <dx:ASPxComboBox ID="ComboBoxOccupationFrom" runat="server" ClientInstanceName="ComboBoxOccupationFrom" 
                                                                                    ValueType="System.Int32" TextField="name" ValueField="id" Width="400px" 
                                                                                        IncrementalFilteringMode="StartsWith" 
                                                                                        DataSourceID="SqlDataSourceDictOrgOccupation" /> </td>
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
                                                                                <td> <dx:ASPxTextBox ID="TextBoxDirectorFioOrg" runat="server" ClientInstanceName="TextBoxDirectorFioOrg" Width="250px" /> </td>
                                                                                <td> &nbsp; </td>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Тел. Директора" Width="100px" /> </td>
                                                                                <td> <dx:ASPxTextBox ID="TextBoxDirectorPhoneOrg" runat="server" ClientInstanceName="TextBoxDirectorPhoneOrg" Width="100px" /> </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Email Директора" Width="95px" /> </td>
                                                                                <td> <dx:ASPxTextBox ID="TextBoxDirectorEmailOrg" runat="server" ClientInstanceName="TextBoxDirectorEmailOrg" Width="250px" /> </td>
                                                                                <td colspan="3"> &nbsp; </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="ПІБ Бухгалтера" Width="95px" /> </td>
                                                                                <td> <dx:ASPxTextBox ID="TextBoxBuhgalterFioFrom" runat="server" ClientInstanceName="TextBoxBuhgalterFioFrom" Width="250px" /> </td>
                                                                                <td> &nbsp; </td>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Тел. Бухгалтера" Width="100px" /> </td>
                                                                                <td> <dx:ASPxTextBox ID="TextBoxBuhgalterPhoneFrom" runat="server" ClientInstanceName="TextBoxBuhgalterPhoneFrom" Width="100px" /> </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td> <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Факс" Width="95px" /> </td>
                                                                                <td> <dx:ASPxTextBox ID="TextBoxFax" runat="server" ClientInstanceName="TextBoxFax" Width="250px" /> </td>
                                                                                <td colspan="3"> &nbsp; </td>
                                                                            </tr>
                                                                        </table>                                                                    
                                                                    </dx:PanelContent>
                                                                </PanelCollection>                                                        
                                                                </dx:ASPxRoundPanel>
                                                                <p class="SpacingPara"/>
                                                                <dx:ASPxLabel ID="LabelOrgCreationError" ClientInstanceName="LabelOrgCreationError" runat="server" Text="" ClientVisible="false" ForeColor="Red" />
                                                                <p class="SpacingPara"/>
                                                                <dx:ASPxButton ID="ButtonDoAddOrgFrom" ClientInstanceName="ButtonDoAddOrgFrom" runat="server" AutoPostBack="False" Text="Створити">
                                                                    <ClientSideEvents Click="function (s, e) { CPOrgEditor.PerformCallback('create_org:'); }" />
                                                                </dx:ASPxButton>

                                                            </dx:PopupControlContentControl>
                                                        </ContentCollection>
                                                    </dx:ASPxPopupControl>                                                
                                                        
                                                    
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <dx:ASPxComboBox ID="ComboRenterOrg" ClientInstanceName="ComboRenterOrg" runat="server"
                                                    Width="100%" DataSourceID="SqlDataSourceOrgSearchRenter" ValueField="id" ValueType="System.Int32"
                                                    TextField="search_name" EnableSynchronization="True" OnCallback="ComboRenterOrg_Callback">
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="BtnCreateRenterOrg" ClientInstanceName="BtnCreateRenterOrg" runat="server" AutoPostBack="False" Text="Створити">
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                    </dx:panelcontent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="function (s, e) { if (!LabelOrgCreationError.GetVisible()) { PopupAddRenterOrg.Hide(); }; }" />
                                                </dx:ASPxCallbackPanel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxButton ID="ButtonDoAddAgreement" ClientInstanceName="ButtonDoAddAgreement" runat="server" AutoPostBack="False" Text="Створити">
                            <ClientSideEvents Click="OnAddNewAgreement" />                            
                        </dx:ASPxButton>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents PopUp="function (s, e) { ComboGiverOrg.PerformCallback('set_default_giver'); }" />
            </dx:ASPxPopupControl>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonSendAll_Doble" runat="server" Text="Надіслати всі зміни в ДКВ" AutoPostBack="false" ClientSideEvents-Click="ButtonSendAllClick" Visible="false"></dx:ASPxButton>
            <dx:ASPxButton ID="ButtonSendAll" runat="server" Text="Надіслати всі зміни в ДКВ" AutoPostBack="false"></dx:ASPxButton>

            <dx:ASPxTimer ID="ProgressTimer" runat="server" ClientInstanceName="ProgressTimer" Enabled="False" Interval="3000">
                <ClientSideEvents Tick="function(s, e) { CPProgress.PerformCallback('timer:'); }" />
            </dx:ASPxTimer>

            <dx:ASPxPopupControl ID="PopupSendAll" runat="server" ClientInstanceName="PopupSendAll"
                HeaderText="Надсилання до ДКВ усіх змінених договорів надання в оренду" PopupElementID="ButtonSendAll">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">

                        <dx:ASPxCallbackPanel ID="CPProgress" ClientInstanceName="CPProgress" runat="server" OnCallback="CPProgress_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent7" runat="server">

                                    <dx:ASPxLabel ID="LabelProgress" ClientInstanceName="LabelProgress" runat="server" Width="650px" />

                                    <dx:ASPxProgressBar ID="ProgressSendAll" ClientInstanceName="ProgressSendAll" runat="server" Width="650px" Height="24px" Minimum="0" Maximum="100" />

                                </dx:panelcontent>
                            </PanelCollection>

                            <ClientSideEvents EndCallback="function (s,e) {                                        
                                        var id = '' + CPProgress.cpWorkItemId;
                                        if (id.length > 0)
                                        {
                                            ProgressTimer.SetEnabled(true);
                                        }
                                        else
                                        {
                                        
                                            ProgressTimer.SetEnabled(false);
                                            alert('Обробку завершено.');
                                            PopupSendAll.Hide();
                                            PrimaryGridView.PerformCallback('init:');
                                            
                                        }
                                    }" />
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents PopUp="function (s,e) { CPProgress.PerformCallback('init:'); ShowSendingLog();}" />
            </dx:ASPxPopupControl>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_ArendaObjects_SaveAs" 
                PopupElementID="ASPxButton_ArendaObjects_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxButton ID="ASPxButton2" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_ArendaObjects_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_ArendaObjects_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxButton ID="ASPxButton_ArendaObjects_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxCallbackPanel ID="CPStatus" ClientInstanceName="CPStatus" runat="server" OnCallback="CPStatus_Callback">
                <PanelCollection>
                    <dx:panelcontent ID="Panelcontent6" runat="server">

                        <asp:FormView runat="server" BorderStyle="None" ID="ArendaStatusForm" DataSourceID="SqlDataSourceArendaStatus" EnableViewState="False">
                            <ItemTemplate>

                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Необхідно надіслати внесені зміни до ДКВ" ClientVisible='<%# 0.Equals(Eval("report_arenda_status")) %>' ForeColor="Red" />
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Усі зміни надіслано до ДКВ" ClientVisible='<%# 1.Equals(Eval("report_arenda_status")) %>' />

                            </ItemTemplate>
                        </asp:FormView>

                    </dx:panelcontent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </td>
        <td>
        </td>    
        <td width = "500" align ="right">
            <dx:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="160px">
                <ClientSideEvents Click="function (s,e) { PopupFieldChooser.Show(); }" />
            </dx:ASPxButton>
        </td>    
    </tr>
</table>
 <dx:ASPxLabel ID="ASPxLabelState"  runat="server" Width="650px" ForeColor="White" />
<dx:ASPxGridViewExporter ID="ASPxGridViewExporterArendaObjects" runat="server" 
    FileName="Arenda" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView ID="PrimaryGridView" ClientInstanceName="PrimaryGridView" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceReportArenda" KeyFieldName="arenda_id" Width="100%"
    OnHtmlRowPrepared="PrimaryGridView_HtmlRowPrepared"
    OnCustomCallback="PrimaryGridView_CustomCallback"
    OnCustomButtonInitialize="PrimaryGridView_CustomButtonInitialize"
    OnHtmlDataCellPrepared="PrimaryGridView_HtmlDataCellPrepared"
    OnCustomColumnSort="PrimaryGridView_CustomColumnSort" >

    <Columns>
        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Name="CmdColumn">
            <CustomButtons>                
                <dx:GridViewCommandColumnCustomButton ID="btnEditAgreement"><Image  Url="../Styles/EditIcon.png" ToolTip="Редагувати Договір Оренди" /></dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnDeleteAgreement"><Image  Url="../Styles/DeleteIcon.png" ToolTip="Видалити Договір Оренди" /></dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
        </dx:GridViewCommandColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_num" VisibleIndex="1" Caption="Номер Договору"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="agreement_date" VisibleIndex="2" Caption="Дата Договору"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="rent_start_date" VisibleIndex="3" Caption="Початок Використання"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="rent_finish_date" VisibleIndex="4" Caption="Закінчення Використання"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="renter_name" VisibleIndex="5" Caption="Орендар" Width="220px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="giver_name" VisibleIndex="6" Caption="Орендодавець" ShowInCustomizationForm="True" Visible="False" Width="220px"></dx:GridViewDataTextColumn>        
        <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="7" Caption="Район" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" VisibleIndex="8" Caption="Назва вулиці" Width="150px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="9" Caption="Номер будинку"><Settings SortMode="Custom" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="10" Caption="Площа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="11" Caption="Коли Змінено"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="submit_date" VisibleIndex="12" Caption="Коли Надіслано в ДКВ"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="is_submitted" VisibleIndex="13" Caption="Надіслано в ДКВ"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_state" VisibleIndex="14" Caption="Стан договору"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="modified_by" VisibleIndex="15" Caption="Користувач"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="sqr_payed_by_percent" VisibleIndex="16" Caption="на яку нараховується плата за використання у відсотках від вартості" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_payed_by_1uah" VisibleIndex="17" Caption="на яку нараховується плата за використання в розмірі 1 грн" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_payed_hourly" VisibleIndex="18" Caption="надана в погодинну оренду, чи відповідно до угод про співпрацю" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_narah_normal" VisibleIndex="19" Caption="Нараховано орендної плати без урахування надмірно нарахованої плати" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_narah" VisibleIndex="19" Caption="Нараховано орендної плати" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="znyato_nadmirno_narah" VisibleIndex="19" Caption="- у тому числі, знято надмірно нарахованої за звітний період" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="last_year_saldo" VisibleIndex="20" Caption="Сальдо на початок року" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_received" VisibleIndex="21" Caption="Надходження орендної плати за звітний період, всього" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_nar_zvit" VisibleIndex="22" Caption="Надходження орендної плати, у тому числі за звітний період без боргів та переплат" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_total" VisibleIndex="23" Caption="Заборгованість по орендній платі" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_zvit" VisibleIndex="24" Caption="у т.ч. з нарахованої у звітному періоді" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_month" VisibleIndex="25" Caption="Заборгованість з орендної плати поточна до 3-х місяців" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_12_month" VisibleIndex="26" Caption="Заборгованість з орендної плати поточна до 12-х місяців"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_years" VisibleIndex="27" Caption="Заборгованість з орендної плати поточна до 3-х років" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_v_mezhah_vitrat" VisibleIndex="28" Caption="Заборгованість з орендної плати (із загальної заборгованості), розмір якої встановлено в межах витрат на утримання" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_over_3_years" VisibleIndex="29" Caption="Заборгованість з орендної плати безнадійна більше 3-х років" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_total" VisibleIndex="30" Caption="Кількість заходів (попереджень, приписів і т.п.)"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_zvit" VisibleIndex="31" Caption="Кількість заходів (попереджень, приписів і т.п.), за звітний період" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_total" VisibleIndex="32" Caption="кількість позовів до суду, всього"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zvit" VisibleIndex="33" Caption="кількість позовів до суду, за звітний період" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_total" VisibleIndex="34" Caption="задоволено позовів, всього" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_zvit" VisibleIndex="35" Caption="задоволено позовів, за звітний період" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_total" VisibleIndex="36" Caption="відкрито виконавчих впроваджень, всього" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_zvit" VisibleIndex="37" Caption="відкрито виконавчих впроваджень, за звітний період" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_total" VisibleIndex="38" Caption="погашено заборгованості за результатами вжитих заходів за звітний період, всього" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_zvit" VisibleIndex="39" Caption="погашено заборгованості за результатами вжитих заходів за звітний період, за звітний період" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_debts_payed" VisibleIndex="40" Caption="Погашення заборгованості минулих періодів" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_type" VisibleIndex="41" Caption="Вид оплати"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="priznachennya" VisibleIndex="42" Caption="Призначення за Документом" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="43" Caption="Використання згідно з договором" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_zkpo" VisibleIndex="44" Caption="Код ЄДРПОУ Орендаря" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_zkpo" VisibleIndex="45" Caption="Код ЄДРПОУ Орендодавця" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_spysano" VisibleIndex="46" Caption="Списано заборгованості з орендної плати у звітному періоді, грн. (без ПДВ)" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="insurance_sum" VisibleIndex="47" Caption="Вартість об'єкту страхування" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="insurance_start" VisibleIndex="48" Caption="Дата початку періоду страхування" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="insurance_end" VisibleIndex="49" Caption="Дата закінчення періоду страхування" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataDateColumn>

        <dx:GridViewDataTextColumn FieldName="avance_plat" VisibleIndex="51" Caption="Авансова орендна плата, грн." ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataCheckColumn FieldName="use_calc_debt" VisibleIndex="52" Caption="Розраховувати заборгованість з орендної плати" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataCheckColumn>
        <dx:GridViewDataTextColumn FieldName="return_all_orend_payed" VisibleIndex="53" Caption="Повернення переплати орендної плати всього за звітний період, грн. (без ПДВ)" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="return_orend_payed" VisibleIndex="54" Caption="Переплата орендної плати всього, грн. (без ПДВ)" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="zvilneno_percent" VisibleIndex="101" Caption="Звільнено від сплати орендної плати на, %" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="zvilneno_date1" VisibleIndex="102" Caption="Звільнено від сплати орендної плати на, з" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="zvilneno_date2" VisibleIndex="103" Caption="Звільнено від сплати орендної плати на, по" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataDateColumn>

        <dx:GridViewDataTextColumn FieldName="zvilbykmp_percent" VisibleIndex="111" Caption="Звільнено від сплати згідно абзац 3 пункт 2 рішення КМР 253/9332, %" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="zvilbykmp_date1" VisibleIndex="112" Caption="Звільнено від сплати згідно абзац 3 пункт 2 рішення КМР 253/9332, з" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="zvilbykmp_date2" VisibleIndex="113" Caption="Звільнено від сплати згідно абзац 3 пункт 2 рішення КМР 253/9332, по" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataDateColumn>

        <dx:GridViewDataDateColumn FieldName="povidoleno1_date" VisibleIndex="121" Caption="Повідомлення орендаря до балансоутримувача про неможлівість використання, дата" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno1_num" VisibleIndex="122" Caption="Повідомлення орендаря до балансоутримувача про неможлівість використання, №" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="povidoleno2_date" VisibleIndex="131" Caption="Повідомлення орендаря до орендодавця про неможлівість використання, дата" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno2_num" VisibleIndex="132" Caption="Повідомлення орендаря до орендодавця про неможлівість використання, №" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="povidoleno3_date" VisibleIndex="141" Caption="Повідомлення орендаря до балансоутримувача про намір використовувати об'єкт, дата" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno3_num" VisibleIndex="142" Caption="Повідомлення орендаря до балансоутримувача про намір використовувати об'єкт, №" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="povidoleno4_date" VisibleIndex="151" Caption="Повідомлення орендаря до орендодавця про намір використовувати об'єкт, дата" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno4_num" VisibleIndex="152" Caption="Повідомлення орендаря до орендодавця про намір використовувати об'єкт, №" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>


    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="agreement_num" ShowInColumn="CmdColumn" SummaryType="Count" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Sum" DisplayFormat="{0}" />

        <dx:ASPxSummaryItem FieldName="sqr_payed_by_percent" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_payed_by_1uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_payed_hourly" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_narah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="last_year_saldo" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_received" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_nar_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_3_month" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_12_month" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_3_years" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_v_mezhah_vitrat" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_over_3_years" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_zahodiv_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_zahodiv_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zadov_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zadov_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_vikon_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_vikon_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_pogasheno_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_pogasheno_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="old_debts_payed" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_spysano" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="insurance_sum" SummaryType="Sum" DisplayFormat="{0}" />

        <dx:ASPxSummaryItem FieldName="znyato_nadmirno_narah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="avance_plat" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="return_all_orend_payed" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="return_orend_payed" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_narah_normal" SummaryType="Sum" DisplayFormat="{0}" />
        

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
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.Reports1NF.ArendaList" Enabled="True" Version="B4" />

    <ClientSideEvents
        Init="function (s,e) { PrimaryGridView.PerformCallback('init:'); }"
        CustomButtonClick="CustomButtonClick"
        EndCallback="PrimaryGridViewEndCallback" />
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
        <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>
