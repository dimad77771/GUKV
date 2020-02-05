<%@ page language="C#" autoeventwireup="true" inherits="Reports1NF_OrgArendaList, App_Web_orgarendalist.aspx.5d94abc0" masterpagefile="~/NoMenu.master" title="Договори Оренди" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
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

<script type="text/javascript" language="javascript">

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

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT CASE WHEN COUNT(*) > 0 THEN 0 ELSE 1 END AS 'report_arenda_status'
        FROM reports1nf_arenda ar
        INNER JOIN reports1nf rep ON rep.id = ar.report_id
        LEFT OUTER JOIN arenda a ON a.id = ar.id
        WHERE ar.report_id = @rep_id AND (a.is_deleted IS NULL OR a.is_deleted = 0) AND (ar.modify_date > rep.create_date AND (ar.submit_date IS NULL OR ar.modify_date > ar.submit_date))">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceReportArenda" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT ar.id AS 'arenda_id', ar.agreement_num, ar.agreement_date, ar.rent_start_date, ar.rent_finish_date,
        org.full_name AS 'renter_name', dict_districts2.name AS 'district', bld.addr_street_name, bld.addr_nomer, ar.rent_square,
        COALESCE(ar.purpose_str, dict_balans_purpose.name) AS 'purpose', ar.is_deleted, ar.modify_date, ar.submit_date,
        a.id AS 'id_in_eis', a.is_deleted AS 'is_deleted_in_eis', org_giver.full_name AS 'giver_name',
        CASE WHEN (ar.modify_date > rep.create_date AND (ar.submit_date IS NULL OR ar.modify_date > ar.submit_date)) THEN N'НІ' ELSE N'ТАК' END AS 'is_submitted',
        ar.modified_by
        FROM reports1nf_arenda ar
        INNER JOIN reports1nf rep ON rep.id = ar.report_id
        LEFT OUTER JOIN arenda a ON a.id = ar.id
        LEFT OUTER JOIN organizations org ON org.id = ar.org_renter_id
        LEFT OUTER JOIN organizations org_giver ON org_giver.id = ar.org_giver_id
        LEFT OUTER JOIN reports1nf_buildings bld ON bld.unique_id = ar.building_1nf_unique_id
        LEFT OUTER JOIN dict_balans_purpose ON dict_balans_purpose.id = ar.purpose_id
        LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = bld.addr_distr_new_id
        WHERE ar.report_id = @rep_id AND (a.is_deleted IS NULL OR a.is_deleted = 0) ORDER BY ar.modify_date DESC" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStreets" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_streets where (not name is null) and (RTRIM(LTRIM(name)) <> '')">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictBuildings" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, LTRIM(RTRIM(addr_nomer)) AS 'nomer' from buildings where (id < 100000) AND
        (is_deleted IS NULL OR is_deleted = 0) AND
        (master_building_id IS NULL) AND
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
        (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL AND id < 300000"
    OnSelecting="SqlDataSourceOrgSearchBalans_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearchGiver" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 20 id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations WHERE
        (is_deleted IS NULL OR is_deleted = 0) AND (master_org_id IS NULL) AND (id < 300000) AND
        ((@balans_org = 0 AND zkpo_code LIKE @zkpo AND full_name LIKE @fname) OR
         (@balans_org > 0 AND id IN (@balans_org, 27065, 8515, 8400, 1728, 8109, 9826, 15130, 141824, 137676, 8566, 13128)))
        ORDER BY organizations.full_name"
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
        (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL AND id < 300000"
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
                                            <td> <dx:ASPxTextBox ID="EditAgreementNum" ClientInstanceName="EditAgreementNum" runat="server" Width="100%" /> </td>
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
                                            <td> <dx:ASPxTextBox ID="EditRenterOrgName" ClientInstanceName="EditRenterOrgName" runat="server" Width="180px" /> </td>
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

                <ClientSideEvents PopUp="function (s,e) { CPProgress.PerformCallback('init:'); ShowSendingLog(); }" />
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
    </tr>
</table>
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
        <dx:GridViewDataTextColumn FieldName="giver_name" VisibleIndex="6" Caption="Орендодавець" Width="220px"></dx:GridViewDataTextColumn>        
        <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="7" Caption="Район" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" VisibleIndex="8" Caption="Назва вулиці" Width="150px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="9" Caption="Номер будинку"><Settings SortMode="Custom" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="10" Caption="Площа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="11" Caption="Коли Змінено"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="submit_date" VisibleIndex="12" Caption="Коли Надіслано в ДКВ"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="is_submitted" VisibleIndex="13" Caption="Надіслано в ДКВ"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="modified_by" VisibleIndex="13" Caption="Користувач"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="agreement_num" ShowInColumn="CmdColumn" SummaryType="Count" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowFilterBar="Visible"
        VerticalScrollBarMode="Visible"
        VerticalScrollBarStyle="Virtual" />
    <SettingsPager PageSize="10" AlwaysShowPager="true" />
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.Reports1NF.ArendaList" Enabled="True" Version="A2" />

    <ClientSideEvents
        Init="function (s,e) { PrimaryGridView.PerformCallback('init:'); }"
        CustomButtonClick="CustomButtonClick"
        EndCallback="PrimaryGridViewEndCallback" />
</dx:ASPxGridView>

</asp:Content>
