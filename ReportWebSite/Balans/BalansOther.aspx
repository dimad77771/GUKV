<%@ Page Title="" Language="C#" MasterPageFile="~/NoHeader.master" AutoEventWireup="true" CodeFile="BalansOther.aspx.cs" Inherits="Balans_BalansOther" %>


<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/SaveReportCtrl.ascx" tagname="SaveReportCtrl" tagprefix="uc1" %>
<%@ Register src="../UserControls/AddressPicker.ascx" tagname="AddressPicker" tagprefix="uc2" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register src="../UserControls/FieldFixxer.ascx" tagname="FieldFixxer" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function ShowFieldFixxerPopupControl(s, e) { PopupFieldFixxer.Show(); }

    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 185);
    }

    function GridViewKievenergoInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewKievenergoEndCallback(s, e) {

        AdjustGridSizes();
    }

    function CheckBoxShowChangesOnly_CheckedChanged(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function ShowAddressPickerPopupControl(s, e) {

        PopupAddressPicker.Show();
    }

    function ShowFieldChooserPopupControl(s, e) {

        PopupFieldChooser.Show();
    }

    // ]]>

</script>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Balans/BalansObjects.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
<%--         <dx:MenuItem NavigateUrl="../Balans/BalansArenda.aspx" Text="Договори Оренди по Об'єктах"></dx:MenuItem>    --%>
        <dx:MenuItem NavigateUrl="../Balans/BalansOrgList.aspx" Text="Перелік Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Balans/BalansOther.aspx" Text="Інші Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Balans/BalansInventarizations.aspx" Text="Об'єкти Інвентарізації"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="http://kmda.iisd.com.ua" Text="Класифікатор майна"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle3" runat="server" Text="Інші Об'єкти Комунальної Власності" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxComboBox ID="ASPxComboBoxPeriod" runat="server" TextField="name" 
                ValueField="begin_date">
                <ClientSideEvents SelectedIndexChanged="function(s, e) {
	PrimaryGridView.Refresh();
}" />
            </dx:ASPxComboBox>
        </td>
        <td>
            <dx:ASPxCheckBox ID="CheckBoxShowChangesOnly" runat="server" Text="Лише Зміни за Період"
                Width="155px" ClientInstanceName="CheckBoxShowChangesOnly" 
                CheckState="Unchecked" >
                <ClientSideEvents CheckedChanged="function(s, e) {
	PrimaryGridView.Refresh();
}" />
            </dx:ASPxCheckBox>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Kievenergo_SaveAs" 
                PopupElementID="ASPxButton_BalansKievenergo_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton8" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_BalansKievenergo_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton9" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_BalansKievenergo_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton10" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_BalansKievenergo_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_BalansKievenergo_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterBalansKievenergo" runat="server" 
    FileName="Інші Об'єкти" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<mini:ProfiledSqlDataSource ID="SqlDataSourceKievenergo" runat="server" EnableCaching="True"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT b.*, 
    org.zkpo_code org_zkpo_code,
    org.full_name org_full_name,
	datepart(month, b.commissioned_date) commissioned_date_month,
	datepart(year, b.commissioned_date) commissioned_date_year,
	datepart(month, b.decommissioned_date) decommissioned_date_month,
	datepart(year, b.decommissioned_date) decommissioned_date_year,
	datepart(month, b.document_date) document_date_month,
	datepart(year, b.document_date) document_date_year,
	datepart(month, b.on_balance_date) on_balance_date_month,
	datepart(year, b.on_balance_date) on_balance_date_year
FROM balans_other b
INNER JOIN organizations org ON org.id = b.org_id and (org.is_deleted is null or org.is_deleted = 0)
WHERE (@changes_only = 0
AND b.on_balance_date &lt;= @period_end_date
AND (b.decommissioned_date IS NULL OR b.decommissioned_date &gt;= @period_begin_date))
OR (@changes_only = 1
AND ((b.on_balance_date &gt;= @period_begin_date AND b.on_balance_date &lt;= @period_end_date)
OR (b.decommissioned_date &gt;= @period_begin_date AND b.decommissioned_date &lt;= @period_begin_date)))
" onselecting="SqlDataSourceKievenergo_Selecting" >
    <SelectParameters>
        <asp:Parameter DefaultValue="" Name="changes_only" />
        <asp:Parameter DefaultValue="" Name="period_end_date" />
        <asp:Parameter Name="period_begin_date" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxGridView ID="PrimaryGridView" runat="server" 
    ClientInstanceName="PrimaryGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceKievenergo"
    Width="100%"
    OnCustomCallback="GridViewBalansKievenergo_CustomCallback" >

    <TotalSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="initial_cost" 
            ShowInColumn="Первісна вартість" SummaryType="Sum" />
        <dx:ASPxSummaryItem DisplayFormat="{0}" FieldName="remaining_cost" 
            ShowInColumn="Залишкова вартість" SummaryType="Sum" />
    </TotalSummary>
    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
        <dx:ASPxSummaryItem DisplayFormat="Первісна вартість = {0}" 
            FieldName="initial_cost" SummaryType="Sum" />
        <dx:ASPxSummaryItem DisplayFormat="Залишкова вартість = {0}" 
            FieldName="remaining_cost" SummaryType="Sum" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn Caption="ID" FieldName="id" VisibleIndex="0" 
            Width="50px" Visible ="false">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Код ЕДРПОУ" 
            FieldName="org_zkpo_code" VisibleIndex="1" Width="200px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Балансоутримувач" 
            FieldName="org_full_name" VisibleIndex="1" Width="200px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Місце розташування об'єкта" 
            FieldName="location" VisibleIndex="1" Width="300px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Адреса об'єкта" FieldName="address" 
            VisibleIndex="2" Width="200px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Інвентарний номер" FieldName="inv_number" 
            VisibleIndex="4" Width="160px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Найменування об’єкта" FieldName="name" 
            VisibleIndex="5" Width="200px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Первісна вартість" FieldName="initial_cost" 
            VisibleIndex="6" Width="120px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Залишкова вартість" 
            FieldName="remaining_cost" VisibleIndex="9" Width="120px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewBandColumn Caption="Дата введення в експлуатацію" VisibleIndex="10">
            <Columns>
                <dx:GridViewDataTextColumn Caption="рік" FieldName="commissioned_date_year" 
                    VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataComboBoxColumn Caption="місяць" 
                    FieldName="commissioned_date_month" Name="ColumnCommissionedDateMonth" 
                    VisibleIndex="1">
                    <PropertiesComboBox TextField="name" ValueField="id" ValueType="System.Int32">
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
            </Columns>
        </dx:GridViewBandColumn>
        <dx:GridViewBandColumn Caption="Дата списання" VisibleIndex="11">
            <Columns>
                <dx:GridViewDataTextColumn Caption="рік" FieldName="decommissioned_date_year" 
                    VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataComboBoxColumn Caption="місяць" 
                    FieldName="decommissioned_date_month" Name="ColumnDecommissionedDateMonth" 
                    VisibleIndex="1">
                    <PropertiesComboBox TextField="name" ValueField="id" ValueType="System.Int32">
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
            </Columns>
        </dx:GridViewBandColumn>
        <dx:GridViewBandColumn Caption="Правовстановчий документ" 
            VisibleIndex="13">
            <Columns>
                <dx:GridViewDataTextColumn Caption="Номер" FieldName="document_number" 
                    VisibleIndex="1" Width="50px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewBandColumn Caption="Дата" 
                    VisibleIndex="2">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="рік" FieldName="document_date_year" 
                            VisibleIndex="0">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataComboBoxColumn Caption="місяць" FieldName="document_date_month" 
                            Name="ColumnDocumentDateMonth" VisibleIndex="1">
                            <PropertiesComboBox TextField="name" ValueField="id" ValueType="System.Int32">
                            </PropertiesComboBox>
                        </dx:GridViewDataComboBoxColumn>
                    </Columns>
                </dx:GridViewBandColumn>
            </Columns>
        </dx:GridViewBandColumn>
        <dx:GridViewBandColumn Caption="Дата постановки на баланс" VisibleIndex="15">
            <Columns>
                <dx:GridViewDataTextColumn Caption="рік" FieldName="on_balance_date_year" 
                    VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataComboBoxColumn Caption="місяць" 
                    FieldName="on_balance_date_month" Name="ColumnOnBalanceDateMonth" 
                    VisibleIndex="1">
                    <PropertiesComboBox TextField="name" ValueField="id" ValueType="System.Int32">
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
            </Columns>
        </dx:GridViewBandColumn>
    </Columns>

    <SettingsBehavior EnableCustomizationWindow="True"
        AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager AlwaysShowPager="true" PageSize="25"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="True" 
        ShowFilterBar="Visible"
        ShowHeaderFilterButton="True" 
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.BalansOther" Version="A1" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewKievenergoInit" EndCallback="GridViewKievenergoEndCallback" />
</dx:ASPxGridView>

</center>

</asp:Content>
