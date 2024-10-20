﻿<%@ page language="C#" autoeventwireup="true" inherits="Reports1NF_OrgBalansObject, App_Web_orgbalansobject.aspx.5d94abc0" masterpagefile="~/NoMenu.master" title="Картка Об'єкту на Балансі" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
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

    var lastFocusedControlId = "";
    var lastFocusedControlTitle = "";

    function EnableBtiDocumentationControls(s, e) {

        var condition = ComboObjBTICondition.GetValue();

        // If condition has any value other that 'Documentation exists', do not validate
        if (condition != null && condition != undefined && condition == 1) {
            EditObjBtiCode.SetEnabled(true);
            EditObjBtiDate.SetEnabled(true);
        }
        else {
            EditObjBtiCode.SetEnabled(false);
            EditObjBtiDate.SetEnabled(false);
        }
    }

    function PerformAllValidations() {

        EditBuildingSqrTotalComment.SetIsValid(true);
        EditBuildingSqrNonHabitComment.SetIsValid(true);
        EditBuildingSqrHabitComment.SetIsValid(true);
        EditObjSqrTotalComment.SetIsValid(true);
        EditObjSqrKorComment.SetIsValid(true);
        EditObjSqrFreeComment.SetIsValid(true);

        var errorsFound = false;
        var activeTabIndex = CardPageControl.GetActiveTabIndex();
        var tabPageCount = CardPageControl.GetTabCount();

        for (var i = 0; i < tabPageCount; i++) {

            CardPageControl.SetActiveTabIndex(i);

            if (!ASPxClientEdit.ValidateEditorsInContainer(CardPageControl.GetMainElement())) {
                errorsFound = true;
                break;
            }
        }

        if (!errorsFound) {
            CardPageControl.SetActiveTabIndex(activeTabIndex);
        }

        return !errorsFound;
    }

    function RemoveAllSpaces(s) {

        var res = "";

        for (var i = 0; i < s.length; i++) {

            if (("" + s[i]) != " ") {

                res = res + s[i];
            }
        }

        return res;
    }

    function IsLiteraPresent(s) {

        var upper = "" + s;
        upper = upper.toUpperCase();

        if (upper.indexOf("ЛІТ") >= 0) {
            return true;
        }

        return false;
    }

    function OnValidateObjectBtiCondition(s, e) {

        var litera = "" + EditBuildingNum2.GetText();
        litera = RemoveAllSpaces(litera);

        var condition = ComboObjBTICondition.GetValue();

        if (IsLiteraPresent(litera) > 0 && (condition == null || condition == undefined)) {
            e.isValid = false;
            e.errorText = "Необхідно заповнити поле 'Документація БТІ'";
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateObjectBtiInvNo(s, e) {

        var condition = ComboObjBTICondition.GetValue();

        // If condition has any value other that 'Documentation exists', do not validate
        if ("" + condition == "1") {
            var litera = "" + EditBuildingNum2.GetText();
            litera = RemoveAllSpaces(litera);

            var invNo = "" + EditObjBtiCode.GetText();
            invNo = RemoveAllSpaces(invNo);

            if (IsLiteraPresent(litera) && invNo.length == 0) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Реєстраційний № в БТІ'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateObjectBtiDate(s, e) {

        var condition = ComboObjBTICondition.GetValue();

        // If condition has any value other that 'Documentation exists', do not validate
        if ("" + condition == "1") {
            var litera = "" + EditBuildingNum2.GetText();
            litera = RemoveAllSpaces(litera);

            var invDate = EditObjBtiDate.GetValue();

            if (IsLiteraPresent(litera) && (invDate == null || invDate == undefined)) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Дата Реєстрації в БТІ'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateBuildingSqrBasement(s, e) {

        if (CheckBasementExists.GetChecked()) {

            var val = EditBuildingSqrBasement.GetValue();

            if (val == null || val == undefined || val <= 0) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Площа підвалу'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateBuildingSqrLoft(s, e) {

        if (CheckLoftExists.GetChecked()) {

            var val = EditBuildingSqrLoft.GetValue();

            if (val == null || val == undefined || val <= 0) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Площа горища'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateEditObjFreeSquare(s, e) {

        if (CheckFreeSquareExists.GetChecked()) {

            var total = EditObjSqrTotal.GetValue();
            var val = EditObjFreeSquare.GetValue();

            if (val == null || val == undefined || val <= 0) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Корисна площа вільного приміщення'";
            }
            else {
                if (total == null || total == undefined) total = 0;
                if (val == null || val == undefined) val = 0;

                if (val > total) {
                    e.isValid = false;
                    e.errorText = 'Корисна площа вільного приміщення не може перевищувати загальну площу на балансі.';
                }
                else {
                    e.isValid = true;
                }
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateComboFreeSqrTechState(s, e) {

        if (CheckFreeSquareExists.GetChecked()) {

            var val = ComboFreeSqrTechState.GetValue();

            if (val == null || val == undefined) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Технічний стан вільного приміщення'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    function OnValidateEditObjFreeSqrLocation(s, e) {

        if (CheckFreeSquareExists.GetChecked()) {

            var val = EditObjFreeSqrLocation.GetValue();

            if (val == null || val == undefined) {
                e.isValid = false;
                e.errorText = "Необхідно заповнити поле 'Розташування вільного приміщення (поверх)'";
            }
            else {
                e.isValid = true;
            }
        }
        else {
            e.isValid = true;
        }
    }

    // Object square validation

    var buildingTotalSqrInitial = 0;
    var buildingNonHabitSqrInitial = 0;
    var buildingHabitSqrInitial = 0;

    var objectTotalSqrInitial = 0;
    var objectKorSqrInitial = 0;
    var objectFreeSqrInitial = 0;

    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceBuilding" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 b.* FROM reports1nf_balans bal INNER JOIN reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
        WHERE bal.id = @bal_id AND bal.report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansObject" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 bal.* FROM reports1nf_balans bal WHERE bal.id = @bal_id AND bal.report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansObjectStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT CASE WHEN submit_date IS NULL OR modify_date IS NULL OR modify_date > submit_date THEN 0 ELSE 1 END AS 'record_status' FROM reports1nf_balans WHERE id = @bal_id AND report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_districts2 ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceStreet" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_streets">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFacade" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_facade">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjKind" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_object_kind">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_object_type">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceTechState" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_tech_state">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceHistory" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_history">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocKind" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_doc_kind ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePurpose" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_balans_purpose ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePurposeGroup" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_balans_purpose_group ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFormOwnership" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_ownership ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOwnershipType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, RTRIM(name) AS 'name' FROM dict_balans_ownership_type ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBTICondition" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_balans_bti ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_balans_obj_status ORDER BY id">
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
    </Items>
</dx:ASPxMenu>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Картка об'єкту на балансі" CssClass="pagetitle"/>
</p>

<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" runat="server" OnCallback="CPMainPanel_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent2" runat="server">

<asp:FormView runat="server" BorderStyle="None" ID="StatusForm" DataSourceID="SqlDataSourceBalansObjectStatus" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Необхідно надіслати внесені зміни до ДКВ" ClientVisible='<%# 0.Equals(Eval("record_status")) %>' ForeColor="Red" />
        <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Зміни надіслано до ДКВ" ClientVisible='<%# 1.Equals(Eval("record_status")) %>' />
    </ItemTemplate>
</asp:FormView>

<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server">
    <TabPages>

        <dx:TabPage Text="Характеристика будинку" Name="Tab1">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server">

                    <asp:FormView runat="server" BorderStyle="None" ID="AddressForm" DataSourceID="SqlDataSourceBuilding" EnableViewState="False">
                        <ItemTemplate>

                            <dx:ASPxRoundPanel ID="PanelAddress" runat="server" HeaderText="Адреса будинку">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent4" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                            <tr>
                                                <td> <dx:ASPxLabel ID="ASPxLabel40" runat="server" Text="Район" Width="125px"/> </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboAddrDistrict" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDistrict" Value='<%# Eval("addr_distr_new_id") %>' ReadOnly="true"
                                                        Title="Адреса будинку - Район" />
                                                </td>
                                                <td> <dx:ASPxLabel ID="ASPxLabel41" runat="server" Text="Назва вулиці" Width="125px" /> </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboAddrStreet" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceStreet" Value='<%# Eval("addr_street_id") %>' ReadOnly="true"
                                                        Title="Адреса будинку - Назва вулиці" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td> <dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Номер будинку, літери" Width="125px"/> </td>
                                                <td>
                                                    <table border="0" cellspacing="0" cellpadding="0" width="270px">
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxTextBox ID="EditBuildingNum1" runat="server" Text='<%# EvaluateTrimStr(Eval("addr_nomer1")) %>' Width="70px" ReadOnly="true"
                                                                    Title="Адреса - Номер будинку" MaxLength="9" />
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="EditBuildingNum2" ClientInstanceName="EditBuildingNum2" runat="server" Text='<%# EvaluateTrimStr(Eval("addr_nomer2")) %>' Width="200px"
                                                                    Title="Адреса - Номер будинку (літери)" MaxLength="18" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td> <dx:ASPxLabel ID="ASPxLabel38" runat="server" Text="Корпус"/> </td>
                                                <td> <dx:ASPxTextBox ID="EditBuildingNum3" runat="server" Text='<%# EvaluateTrimStr(Eval("addr_nomer3")) %>' Width="270px" Title="Адреса - Номер будинку (корпус)" MaxLength="10"/> </td>
                                            </tr>
                                            <tr>
                                                <td> <dx:ASPxLabel ID="ASPxLabel42" runat="server" Text="Додаткова адреса"/> </td>
                                                <td> <dx:ASPxTextBox ID="EditMiscAddr" runat="server" Text='<%# EvaluateTrimStr(Eval("addr_misc")) %>' Width="270px" Title="Адреса будинку - Додаткова адреса" MaxLength="100" /> </td>
                                                <td> <dx:ASPxLabel ID="ASPxLabel43" runat="server" Text="Поштовий індекс"/> </td>
                                                <td> <dx:ASPxTextBox ID="EditZipCode" runat="server" Text='<%# EvaluateTrimStr(Eval("addr_zip_code")) %>' Width="270px" Title="Адреса будинку - Поштовий індекс" MaxLength="10" /> </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelBuildingProps" runat="server" HeaderText="Характеристики будинку">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Рік будівництва" Width="125px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditBuildYear" runat="server" NumberType="Integer" Value='<%# Eval("construct_year") %>' Width="270px" Title="Рік будівництва будинку" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Рік здачі в експлуатацію" Width="125px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditExplYear" runat="server" NumberType="Integer" Value='<%# Eval("expl_enter_year") %>' Width="270px" Title="Рік здачі будинку в експлуатацію" /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Фасадний / Не фасадний"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBuildingFacade" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceFacade" Value='<%# Eval("facade_id") %>'
                                                        Title="Фасадний / Не фасадний будинок" />
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Кількість поверхів загальна"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox ID="EditBuildingNumFloors" runat="server" Text='<%# Eval("num_floors") %>' Width="270px" Title="Загальна кількість поверхів будинку" MaxLength="18" /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Вид будинку"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBuildingKind" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceObjKind" Value='<%# Eval("object_kind_id") %>'
                                                        Title="Вид будинку" />
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Тип будинку"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBuildingType" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceObjType" Value='<%# Eval("object_type_id") %>'
                                                        Title="Тип будинку" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Технічний стан"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBuildingTechState" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceTechState" Value='<%# Eval("tech_condition_id") %>'
                                                        Title="Технічний стан будинку" />
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Пам'ятка архітектури (історії)"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBuildingHistory" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceHistory" Value='<%# Eval("history_id") %>'
                                                        Title="Будинок - Пам'ятка архітектури (історії)" />
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelBuildingSquare" runat="server" HeaderText="Площа будинку">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Загальна площа будинку, кв.м" Width="125px"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditBuildingSqrTotal" ClientInstanceName="EditBuildingSqrTotal" runat="server" NumberType="Float" Value='<%# Eval("sqr_total") %>' Width="270px" Title="Загальна площа будинку" OnValidation="OnValidation">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true" ErrorText="Необхідно вказати загальну площу будинку"></ValidationSettings>
                                                        <ClientSideEvents
                                                            Init="function (s,e) { buildingTotalSqrInitial = s.GetValue(); }"
                                                            NumberChanged="function (s,e) {
                                                                var val = s.GetValue();
                                                                EditBuildingSqrTotalComment.SetVisible(val != null && val != undefined && (val - buildingTotalSqrInitial >= 20 || val - buildingTotalSqrInitial <= -20));
                                                                LabelBuildingSqrTotalComment.SetVisible(val != null && val != undefined && (val - buildingTotalSqrInitial >= 20 || val - buildingTotalSqrInitial <= -20));
                                                                e.processOnServer = false; }"
                                                            />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxLabel ID="LabelBuildingSqrTotalComment" ClientInstanceName="LabelBuildingSqrTotalComment" Width="100%" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни загальної площи будинку, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditBuildingSqrTotalComment" ClientInstanceName="EditBuildingSqrTotalComment" runat="server" Width="100%"
                                                        Title="Коментар щодо зміни загальної площі будинку" ClientVisible="false">
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни загальної площі будинку'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel39" runat="server" Text="Площа нежилих приміщень, кв.м" Width="125px"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditBuildingSqrNonHabit" runat="server" NumberType="Float" Value='<%# Eval("sqr_non_habit") %>' Width="270px" Title="Площа нежилих приміщень будинку">
                                                        <ValidationSettings Display=None></ValidationSettings>
                                                        <ClientSideEvents
                                                            Init="function (s,e) { buildingNonHabitSqrInitial = s.GetValue(); }"
                                                            Validation="function (s,e) {
                                                                var total = EditBuildingSqrTotal.GetValue();
                                                                var val = s.GetValue();

                                                                if (total == null || total == undefined) total = 0;
                                                                if (val == null || val == undefined) val = 0;

                                                                if (val > total) { e.isValid = false; e.errorText = 'Площа нежилих приміщень будинку не може перевищувати загальну площу будинку.'; } else { e.isValid = true; }
                                                                }"
                                                            NumberChanged="function (s,e) {
                                                                var val = s.GetValue();
                                                                EditBuildingSqrNonHabitComment.SetVisible(val != null && val != undefined && (val - buildingNonHabitSqrInitial >= 20 || val - buildingNonHabitSqrInitial <= -20));
                                                                LabelBuildingSqrNonHabitComment.SetVisible(val != null && val != undefined && (val - buildingNonHabitSqrInitial >= 20 || val - buildingNonHabitSqrInitial <= -20));
                                                                e.processOnServer = false; }"
                                                            />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Площа житлового фонду, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditBuildingSqrHabit" runat="server" NumberType="Float" Value='<%# Eval("sqr_habit") %>' Width="270px" Title="Площа житлового фонду будинку">
                                                        <ValidationSettings Display=None></ValidationSettings>
                                                        <ClientSideEvents
                                                            Init="function (s,e) { buildingHabitSqrInitial = s.GetValue(); }"
                                                            Validation="function (s,e) {
                                                                var total = EditBuildingSqrTotal.GetValue();
                                                                var val = s.GetValue();

                                                                if (total == null || total == undefined) total = 0;
                                                                if (val == null || val == undefined) val = 0;

                                                                if (val > total) { e.isValid = false; e.errorText = 'Площа житлового фонду будинку не може перевищувати загальну площу будинку.'; } else { e.isValid = true; }
                                                                }"
                                                            NumberChanged="function (s,e) {
                                                                var val = s.GetValue();
                                                                EditBuildingSqrHabitComment.SetVisible(val != null && val != undefined && (val - buildingHabitSqrInitial >= 20 || val - buildingHabitSqrInitial <= -20));
                                                                LabelBuildingSqrHabitComment.SetVisible(val != null && val != undefined && (val - buildingHabitSqrInitial >= 20 || val - buildingHabitSqrInitial <= -20));
                                                                e.processOnServer = false; }"
                                                            />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxLabel ID="LabelBuildingSqrNonHabitComment" ClientInstanceName="LabelBuildingSqrNonHabitComment" Width="100%" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни нежитлової площи будинку, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditBuildingSqrNonHabitComment" ClientInstanceName="EditBuildingSqrNonHabitComment" runat="server" Width="100%"
                                                        Title="Коментар щодо зміни нежитлової площі будинку" ClientVisible="false">
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни нежитлової площі будинку'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>

                                                    <dx:ASPxLabel ID="LabelBuildingSqrHabitComment" ClientInstanceName="LabelBuildingSqrHabitComment" Width="100%" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни житлової площи будинку, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditBuildingSqrHabitComment" ClientInstanceName="EditBuildingSqrHabitComment" runat="server" Width="100%"
                                                        Title="Коментар щодо зміни житлової площі будинку" ClientVisible="false">
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни житлової площі будинку'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel45" runat="server" Text="Площа підвалу, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditBuildingSqrBasement" ClientInstanceName="EditBuildingSqrBasement" runat="server" NumberType="Float" Value='<%# Eval("sqr_pidval") %>' Width="270px" Title="Площа підвалу будинку">
                                                        <ValidationSettings Display=None></ValidationSettings>
                                                        <ClientSideEvents Validation="OnValidateBuildingSqrBasement" />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Площа горища, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditBuildingSqrLoft" ClientInstanceName="EditBuildingSqrLoft" runat="server" NumberType="Float" Value='<%# Eval("sqr_loft") %>' Width="270px" Title="Площа горища будинку">
                                                        <ValidationSettings Display=None></ValidationSettings>
                                                        <ClientSideEvents Validation="OnValidateBuildingSqrLoft" />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <dx:ASPxCheckBox ID="CheckBasementExists" ClientInstanceName="CheckBasementExists" runat="server" Text="Підвал існує" Value='<%# 1.Equals(Eval("is_basement_exists")) %>' Title="Підвал існує">
                                                        <ClientSideEvents CheckedChanged="function (s,e) { EditBuildingSqrBasement.SetEnabled(CheckBasementExists.GetChecked()); }" />
                                                    </dx:ASPxCheckBox>
                                                </td>
                                                <td colspan="2">
                                                    <dx:ASPxCheckBox ID="CheckLoftExists" ClientInstanceName="CheckLoftExists" runat="server" Text="Горище існує" Value='<%# 1.Equals(Eval("is_loft_exists")) %>' Title="Горище існує">
                                                        <ClientSideEvents CheckedChanged="function (s,e) { EditBuildingSqrLoft.SetEnabled(CheckLoftExists.GetChecked()); }" />
                                                    </dx:ASPxCheckBox>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                        </ItemTemplate>
                    </asp:FormView>

                    <p class="SpacingPara"/>

                    <asp:FormView runat="server" BorderStyle="None" ID="FormViewState" DataSourceID="SqlDataSourceBalansObject" EnableViewState="False">
                        <ItemTemplate>
                            <dx:ASPxRoundPanel ID="StatePanel" runat="server" HeaderText="Стан">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent7" runat="server">
                                        <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text='<%# EvaluateSignature(Eval("modified_by"), Eval("modify_date")) %>'></dx:ASPxLabel></td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </ItemTemplate>
                    </asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Загальна характеристика об'єкту" Name="Tab2">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl5" runat="server">

                    <asp:FormView runat="server" BorderStyle="None" ID="BalansObjForm" DataSourceID="SqlDataSourceBalansObject" EnableViewState="False">
                        <ItemTemplate>

                            <dx:ASPxRoundPanel ID="PanelObjProps" runat="server" HeaderText="Характеристики об'єкту">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Вид об'єкту"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjKind" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="250px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceObjKind" Value='<%# Eval("object_kind_id") %>'
                                                        Title="Вид об'єкту" >
                                                        <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Характеристики об'єкту: необхідно вибрати вид об'єкту" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Тип об'єкту"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjType" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="250px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceObjType" Value='<%# Eval("object_type_id") %>'
                                                        Title="Тип об'єкту" >
                                                        <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Характеристики об'єкту: необхідно вибрати тип об'єкту" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Технічний стан"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjTechState" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="250px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceTechState" Value='<%# Eval("tech_condition_id") %>'
                                                        Title="Технічний стан об'єкту">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        </dx:ASPxComboBox>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Інвентарний номер об'єкту"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditObjBtiReestrNo" runat="server" Text='<%# Eval("reestr_no") %>' Width="250px" Title="Інвентарний номер об'єкту" MaxLength="18">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true" ErrorText="Характеристики об'єкту: необхідно вказати інвентарний номер об'єкту"></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Розташування приміщення (поверх)"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox ID="EditObjFloors" runat="server" Text='<%# Eval("floors") %>' Width="250px" Title="Розташування приміщення (поверх)" MaxLength="100" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel46" runat="server" Text="Поточне використання приміщення"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjStatus" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="250px" 
                                                        DataSourceID="SqlDataSourceObjStatus" Value='<%# Eval("obj_status_id") %>' Title="Поточне використання приміщення" 
                                                        OnValidation="OnValidation">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" ErrorText="Характеристики об'єкту: необхідно вказати поточне використання приміщення"> <RequiredField IsRequired="false" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelObjSquare" runat="server" HeaderText="Площа об'єкту">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent5" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Площа нежилих приміщень на балансі, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditObjSqrTotal" ClientInstanceName="EditObjSqrTotal" runat="server" NumberType="Float" Value='<%# Eval("sqr_total") %>' Width="100px" Title="Площа нежилих приміщень на балансі">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup"> <RequiredField IsRequired="false" ErrorText="Площа об'єкту: необхідно вказати площу нежилих приміщень на балансі" /> </ValidationSettings>
                                                        <ClientSideEvents
                                                            Init="function (s,e) { objectTotalSqrInitial = s.GetValue(); }"
                                                            NumberChanged="function (s,e) {
                                                                var val = s.GetValue();
                                                                EditObjSqrTotalComment.SetVisible(val != null && val != undefined && (val - objectTotalSqrInitial >= 20 || val - objectTotalSqrInitial <= -20));
                                                                LabelObjSqrTotalComment.SetVisible(val != null && val != undefined && (val - objectTotalSqrInitial >= 20 || val - objectTotalSqrInitial <= -20));
                                                                e.processOnServer = false; }"
                                                            />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="- в т.ч. корисна площа, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditObjSqrKor" runat="server" NumberType="Float" Value='<%# Eval("sqr_kor") %>' Width="100px" Title="Корисна площа на балансі">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents
                                                            Init="function (s,e) { objectKorSqrInitial = s.GetValue(); }"
                                                            Validation="function (s,e) {
                                                                var total = EditObjSqrTotal.GetValue();
                                                                var val = s.GetValue();

                                                                if (total == null || total == undefined) total = 0;
                                                                if (val == null || val == undefined) val = 0;

                                                                if (val > total) { e.isValid = false; e.errorText = 'Корисна площа на балансі не може перевищувати загальну площу на балансі.'; } else { e.isValid = true; }
                                                                }"
                                                            NumberChanged="function (s,e) {
                                                                var val = s.GetValue();
                                                                EditObjSqrKorComment.SetVisible(val != null && val != undefined && (val - objectKorSqrInitial >= 20 || val - objectKorSqrInitial <= -20));
                                                                LabelObjSqrKorComment.SetVisible(val != null && val != undefined && (val - objectKorSqrInitial >= 20 || val - objectKorSqrInitial <= -20));
                                                                e.processOnServer = false; }"
                                                            />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxLabel ID="LabelObjSqrTotalComment" ClientInstanceName="LabelObjSqrTotalComment" Width="100%" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни площі нежилих приміщень на балансі, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditObjSqrTotalComment" ClientInstanceName="EditObjSqrTotalComment" runat="server" Width="100%"
                                                        Title="Коментар щодо зміни площі нежилих приміщень на балансі" ClientVisible="false">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни площі нежилих приміщень на балансі'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>

                                                    <dx:ASPxLabel ID="LabelObjSqrKorComment" ClientInstanceName="LabelObjSqrKorComment" Width="100%" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни корисної площі нежилих приміщень на балансі, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditObjSqrKorComment" ClientInstanceName="EditObjSqrKorComment" runat="server" Width="100%"
                                                        Title="Коментар щодо зміни корисної площі нежилих приміщень на балансі" ClientVisible="false">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни корисної площі нежилих приміщень на балансі'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Площа для власних потреб, кв.м"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjSqrVlasn" runat="server" NumberType="Float" Value='<%# Eval("sqr_vlas_potreb") %>' Width="100px" Title="Площа для власних потреб" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel32" runat="server" Text="- в т.ч. техніко-інженерних потреб, кв.м"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditObjSqrEng" runat="server" NumberType="Float" Value='<%# Eval("sqr_engineering") %>' Width="100px" Title="Площа для техніко-інженерних потреб">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel33" runat="server" Text="Кількість договорів оренди"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjNumRentAgreements" runat="server" NumberType="Integer" Value='<%# Eval("num_rent_agr") %>' Width="100px" Title="Кількість договорів оренди" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Площа надана в оренду, кв.м"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjSqrInRent" runat="server" NumberType="Float" Value='<%# Eval("sqr_in_rent") %>' Width="100px" Title="Площа надана в оренду" /></td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelFreeSquare" runat="server" HeaderText="Вільні приміщення">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent3" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                            <tr>
                                                <td colspan="2">
                                                    <dx:ASPxCheckBox ID="CheckFreeSquareExists" ClientInstanceName="CheckFreeSquareExists" runat="server" Text="Наявність вільного приміщення" Value='<%# 1.Equals(Eval("is_free_sqr")) %>' Title="Наявність вільного приміщення">
                                                        <ClientSideEvents 
                                                        CheckedChanged="function (s,e) { EditObjFreeSquare.SetEnabled(CheckFreeSquareExists.GetChecked()); ComboFreeSqrTechState.SetEnabled(CheckFreeSquareExists.GetChecked()); EditObjFreeSqrLocation.SetEnabled(CheckFreeSquareExists.GetChecked()); }" />
                                                    </dx:ASPxCheckBox>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Корисна площа вільного приміщення"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="EditObjFreeSquare" ClientInstanceName="EditObjFreeSquare" runat="server" NumberType="Float" Value='<%# Eval("free_sqr_useful") %>' Width="250px" Title="Корисна площа вільного приміщення">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents
                                                            Validation="OnValidateEditObjFreeSquare"
                                                            Init="function (s,e) { objectFreeSqrInitial = s.GetValue(); }"
                                                            NumberChanged="function (s,e) {
                                                                var val = s.GetValue();
                                                                EditObjSqrFreeComment.SetVisible(val != null && val != undefined && (val - objectFreeSqrInitial >= 20 || val - objectFreeSqrInitial <= -20));
                                                                LabelObjSqrFreeComment.SetVisible(val != null && val != undefined && (val - objectFreeSqrInitial >= 20 || val - objectFreeSqrInitial <= -20));
                                                                e.processOnServer = false; }" />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxLabel ID="LabelObjSqrFreeComment" ClientInstanceName="LabelObjSqrFreeComment" ClientVisible="false"
                                                        runat="server" Text="Будь ласка, вкажіть причину зміни корисної площі вільного приміщення, та реквізити підтверджуючого документу"/>

                                                    <dx:ASPxTextBox ID="EditObjSqrFreeComment" ClientInstanceName="EditObjSqrFreeComment" runat="server"
                                                        Title="Коментар щодо зміни корисної площі вільного приміщення" ClientVisible="false">
                                                        <ClientSideEvents Validation="function (s,e) { if (s.GetVisible() && s.GetText().length == 0) { e.isValid = false; e.errorText = 'Необхідно заповнити коментар щодо зміни корисної площі вільного приміщення'; } else { e.isValid = true; } }" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Технічний стан вільного приміщення"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboFreeSqrTechState" ClientInstanceName="ComboFreeSqrTechState" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="250px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceTechState" Value='<%# Eval("free_sqr_condition_id") %>'
                                                        Title="Технічний стан вільного приміщення">
                                                        <ClientSideEvents Validation="OnValidateComboFreeSqrTechState" />
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Розташування вільного приміщення (поверх)"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditObjFreeSqrLocation" ClientInstanceName="EditObjFreeSqrLocation" runat="server" Text='<%# Eval("free_sqr_location") %>' Width="250px" MaxLength="100" Title="Розташування вільного приміщення (поверх)">
                                                        <ClientSideEvents Validation="OnValidateEditObjFreeSqrLocation" />
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                        </ItemTemplate>
                    </asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Техніко-економічна характеристика об'єкту" Name="Tab3">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server">

                    <asp:FormView runat="server" BorderStyle="None" ID="BalansDocsForm" DataSourceID="SqlDataSourceBalansObject" EnableViewState="False">
                        <ItemTemplate>

                            <dx:ASPxRoundPanel ID="PanelObjRights" runat="server" HeaderText="Права на об'єкт">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent4" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">

                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Форма власності об’єкту"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjOwnership" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceFormOwnership" Value='<%# Eval("form_ownership_id") %>'
                                                        Title="Форма власності об’єкту">
                                                        <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Техніко-економічна характеристика об'єкту: необхідно вибрати форму власності об’єкту" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Право користування об’єктом"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboObjRight" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceOwnershipType" Value='<%# Eval("ownership_type_id") %>'
                                                        Title="Право користування об’єктом" />
                                                </td>
                                            </tr>

                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelOwnershipDoc" runat="server" HeaderText="Документ, що встановлює право користування об’єктом">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent3" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Тип документу:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboOwnershipDocKind" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="350px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDocKind" Value='<%# Eval("ownership_doc_type") %>'
                                                        Title="Тип документу, що встановлює право користування об’єктом" OnValidation="OnValidation">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" ErrorText="Документ, що встановлює право користування об’єктом: необхідно вибрати тип документу" > <RequiredField IsRequired="false" ErrorText="Документ, що встановлює право користування об’єктом: необхідно вибрати тип документу" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                
                                                <td><dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Номер:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditOwnershipDocNum" runat="server" Text='<%# Eval("ownership_doc_num") %>' Width="100px" Title="Номер документу, що встановлює право користування об’єктом" MaxLength="24" OnValidation="OnValidation">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" ErrorText="Документ, що встановлює право користування об’єктом: необхідно вказати номер документу"></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>

                                                <td><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Від:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="EditOwnershipDocDate" runat="server" Value='<%# Eval("ownership_doc_date") %>' Width="100px" Title="Дата документу, що встановлює право користування об’єктом" OnValidation="OnValidation">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" ErrorText="Документ, що встановлює право користування об’єктом: необхідно вказати дату документу"></ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelBalanspDoc" runat="server" HeaderText="Документ, що підтверджує передачу об’єкту на баланс">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent6" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Тип документу:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboBalansDocKind" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="350px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDocKind" Value='<%# Eval("balans_doc_type") %>'
                                                        Title="Тип документу, що підтверджує передачу об’єкту на баланс" OnValidation="OnValidation">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" ErrorText="Документ, що підтверджує передачу об’єкту на баланс: необхідно вибрати тип документу"></ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                
                                                <td><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Номер:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditBalansDocNum" runat="server" Text='<%# Eval("balans_doc_num") %>' Width="100px" Title="Номер документу, що підтверджує передачу об’єкту на баланс" MaxLength="24" OnValidation="OnValidation">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" ErrorText="Документ, що підтверджує передачу об’єкту на баланс: необхідно вказати номер документу"></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>

                                                <td><dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="Від:"></dx:ASPxLabel></td>
                                                <td>&nbsp; &nbsp;</td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="EditBalansDocDate" runat="server" Value='<%# Eval("balans_doc_date") %>' Width="100px" Title="Дата документу, що підтверджує передачу об’єкту на баланс" OnValidation="OnValidation">
                                                        <ValidationSettings Display="None" ValidationGroup="MainGroup" ErrorText="Документ, що підтверджує передачу об’єкту на баланс: необхідно вказати дату документу"></ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelObjPurpose" runat="server" HeaderText="Призначення за проектною документацією">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent7" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Група призначення" width="200px"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboPurposeGroup" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="600px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourcePurposeGroup" Value='<%# Eval("purpose_group_id") %>'
                                                        Title="Група призначення">
                                                        <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Призначення за проектною документацією: необхідно вибрати групу призначення" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="Призначення за класифікацією" width="200px"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboPurpose" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="600px" 
                                                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourcePurpose" Value='<%# Eval("purpose_id") %>'
                                                        Title="Призначення за класифікацією">
                                                        <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Призначення за проектною документацією: необхідно вибрати призначення за класифікацією" /> </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Призначення за уточненням балансоутримувача" width="200px"></dx:ASPxLabel></td>
                                                <td valign="top">
                                                    <dx:ASPxTextBox ID="EditPurposeStr" runat="server" Text='<%# Eval("purpose_str") %>' Width="600px" Title="Призначення за уточненням балансоутримувача" MaxLength="255">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelObjBTI" runat="server" HeaderText="Реєстрація в Укрдержреєстрі (колишньому БТІ)">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent8" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="2" width="810px">

                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Документація БТІ"></dx:ASPxLabel></td>
                                                <td colspan="3">
                                                    <dx:ASPxComboBox ID="ComboObjBTICondition" ClientInstanceName="ComboObjBTICondition" runat="server" ValueType="System.Int32"
                                                        TextField="name" ValueField="id" Width="100%" IncrementalFilteringMode="StartsWith"
                                                        DataSourceID="SqlDataSourceBTICondition" Value='<%# Eval("bti_id") %>' Title="Документація БТІ">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents Validation="OnValidateObjectBtiCondition" SelectedIndexChanged="EnableBtiDocumentationControls" />
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Реєстраційний № в БТІ" width="150px"></dx:ASPxLabel></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="EditObjBtiCode" ClientInstanceName="EditObjBtiCode" runat="server" Text='<%# Eval("obj_bti_code") %>' Width="240px" MaxLength="18"
                                                        Title="Реєстраційний № в БТІ">
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                        <ClientSideEvents Validation="OnValidateObjectBtiInvNo" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Дата Реєстрації в БТІ" width="150px"></dx:ASPxLabel></td>
                                                <td align="right">
                                                    <dx:ASPxDateEdit ID="EditObjBtiDate" ClientInstanceName="EditObjBtiDate" runat="server" Value='<%# Eval("date_bti") %>' Width="240px"
                                                        Title="Дата Реєстрації в БТІ">
                                                        <ValidationSettings Display='None'></ValidationSettings>
                                                        <ClientSideEvents Validation="OnValidateObjectBtiDate" />
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>

                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                        </ItemTemplate>
                    </asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Вартісна оцінка" Name="Tab4">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server">

                    <asp:FormView runat="server" BorderStyle="None" ID="BalansCostForm" DataSourceID="SqlDataSourceBalansObject" EnableViewState="False">
                        <ItemTemplate>

                            <dx:ASPxRoundPanel ID="PanelObjCost" runat="server" HeaderText="Вартісні показники">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent5" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Первісна (переоцінена) вартість, тис. грн." width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjCostBalans" runat="server" NumberType="Float" Value='<%# Eval("cost_balans") %>' Width="190px" Title="Первісна (переоцінена) вартість" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="Залишкова вартість, тис. грн." width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjCostZalishkova" runat="server" NumberType="Float" Value='<%# Eval("cost_zalishkova") %>' Width="190px" Title="Залишкова вартість" /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Знос, тис. грн." width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditObjZnos" runat="server" NumberType="Float" Value='<%# Eval("znos") %>' Width="190px" Title="Знос" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel32" runat="server" Text="станом на" width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxDateEdit ID="EditObjZnosDate" runat="server" Value='<%# Eval("znos_date") %>' Width="190px" Title="Знос станом на" /></td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                            <p class="SpacingPara"/>

                            <dx:ASPxRoundPanel ID="PanelExpertCost" runat="server" HeaderText="Ринкова вартість">
                                <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent9" runat="server">

                                        <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                            <tr>
                                                <td><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Ринкова вартість, тис. грн." width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxSpinEdit ID="EditCostExpertTotal" runat="server" NumberType="Float" Value='<%# Eval("cost_expert_total") %>' Width="190px" Title="Ринкова вартість" /></td>
                                                <td><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="станом на" width="200px"></dx:ASPxLabel></td>
                                                <td><dx:ASPxDateEdit ID="EditDateExpert" runat="server" Value='<%# Eval("date_expert") %>' Width="190px" Title="Ринкова вартість станом на" /></td>
                                            </tr>
                                        </table>

                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>

                        </ItemTemplate>
                    </asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

    </TabPages>
</dx:ASPxPageControl>

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<p class="SpacingPara"/>

<dx:ASPxValidationSummary ID="ValidationSummary" ValidationGroup="MainGroup" runat="server" RenderMode="BulletedList" Width="800px" ClientInstanceName="ValidationSummary">
</dx:ASPxValidationSummary>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('save:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonSend" runat="server" Text="Надіслати" AutoPostBack="false" ValidationGroup="MainGroup" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('send:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonClear" runat="server" Text="Відмінити зміни" CausesValidation="false" AutoPostBack="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('clear:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonComments" runat="server" Text="Коментарі" CausesValidation="false" AutoPostBack="false" />

            <dx:ASPxPopupControl ID="PopupComments" runat="server" 
                HeaderText="Коментарі" 
                ClientInstanceName="PopupComments" 
                PopupElementID="ButtonComments" >
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                        
                        <dx:ASPxCallbackPanel ID="CPCommentViewerPanel" ClientInstanceName="CPCommentViewerPanel" runat="server" OnCallback="CPCommentViewerPanel_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent6" runat="server">

                                    <uc1:ReportCommentViewer ID="ReportCommentViewer1" runat="server"/>

                                </dx:panelcontent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents
                    PopUp="function (s,e) { CPCommentViewerPanel.PerformCallback('' + lastFocusedControlId + ';' + lastFocusedControlTitle); }"
                    CloseUp="function (s,e) { CPMainPanel.PerformCallback('clear:'); }" />
            </dx:ASPxPopupControl>
        </td>
    </tr>
</table>

</asp:Content>
