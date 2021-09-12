<%@ Page Title="" Language="C#" MasterPageFile="~/NoHeader.master" AutoEventWireup="true" CodeFile="BalansInventarizations.aspx.cs" Inherits="Balans_BalansInventarizations" %>


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

    function GridViewInventarizationsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewInventarizationsEndCallback(s, e) {

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

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
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
            <asp:Label ID="LabelReportTitle3" runat="server" Text="Об'єкти Інвентарізації" CssClass="reporttitle"></asp:Label>
        </td>
<%-- 
               <td>
            <dx:ASPxComboBox ID="ASPxComboBoxPeriod" runat="server" TextField="name" 
                ValueField="begin_date">
                <ClientSideEvents SelectedIndexChanged="function(s, e) {PrimaryGridView.Refresh();}" />
            </dx:ASPxComboBox>
        </td>
        <td>
            <dx:ASPxCheckBox ID="CheckBoxShowChangesOnly" runat="server" Text="Лише Зміни за Період"
                Width="155px" ClientInstanceName="CheckBoxShowChangesOnly" 
                CheckState="Unchecked" >
                <ClientSideEvents CheckedChanged="function(s, e) {PrimaryGridView.Refresh();}" />
            </dx:ASPxCheckBox>
        </td>
--%> 
    
           <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Inventarizations_SaveAs" 
                PopupElementID="ASPxButton_BalansInventarizations_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton8" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_BalansInventarizations_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton9" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_BalansInventarizations_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton10" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_BalansInventarizations_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_BalansInventarizations_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterBalansInventarizations" runat="server" 
    FileName="Об'єкти Інвентарізації" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<mini:ProfiledSqlDataSource ID="SqlDataSourceInventarizations" runat="server" EnableCaching="True"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
    inv.id
    ,inv.yer_period
    ,inv.zkpo_code
    ,inv.name
    ,inv.inv_number
    ,inv.sqr_total
    ,inv.commissioned_date
    ,inv.initial_cost
    ,inv.znos
    ,inv.remaining_cost
    ,inv.doc_number_on
    ,inv.doc_date_on
    ,inv.comments
    ,inv.doc_number_off
    ,inv.doc_date_off
     
    ,org.full_name org_full_name

/*	datepart(month, inv.commissioned_date) commissioned_date_month,
	datepart(year, inv.commissioned_date) commissioned_date_year,
	datepart(month, inv.decommissioned_date) decommissioned_date_month,
	datepart(year, inv.decommissioned_date) decommissioned_date_year,
	datepart(month, inv.doc_date_on) document_date_month,
	datepart(year, inv.doc_date_on) document_date_year,
	datepart(month, inv.on_balance_date) on_balance_date_month,
	datepart(year, inv.on_balance_date) on_balance_date_year
*/
FROM inventarization inv
INNER JOIN organizations org ON org.id = inv.org_id and isnull(org.is_deleted,0) = 0
--WHERE (@changes_only = 0
--AND (inv.on_balance_date is null or inv.on_balance_date <= @period_end_date)
--AND (inv.decommissioned_date IS NULL OR inv.decommissioned_date >= @period_begin_date))
--OR (@changes_only = 1
--AND ((inv.on_balance_date is null or inv.on_balance_date >= @period_begin_date AND inv.on_balance_date <= @period_end_date)
--OR (inv.decommissioned_date is null or inv.decommissioned_date >= @period_begin_date AND inv.decommissioned_date <= @period_begin_date)))
" onselecting="SqlDataSourceInventarizations_Selecting" >
    <SelectParameters>
        <asp:Parameter  DbType="Int32" DefaultValue="0" Name="changes_only" />
        <asp:Parameter DefaultValue="2999.01.01" Name="period_end_date" />
        <asp:Parameter DefaultValue="1900.01.01" Name="period_begin_date" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxGridView ID="PrimaryGridView" runat="server" 
    ClientInstanceName="PrimaryGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceInventarizations"
    Width="100%"
    OnCustomCallback="GridViewBalansInventarizations_CustomCallback" >

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
        <dx:GridViewDataTextColumn Caption="ID" FieldName="id" VisibleIndex="0" Width="50px" Visible ="false">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Звітний рік" FieldName="yer_period" VisibleIndex="1" Width="80px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Код ЕДРПОУ" FieldName="zkpo_code" VisibleIndex="2" Width="80px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Балансоутримувач" FieldName="org_full_name" VisibleIndex="3" Width="300px">
        </dx:GridViewDataTextColumn>
<%-- 
        <dx:GridViewDataTextColumn Caption="Місце розташування об'єкта" FieldName="location" VisibleIndex="4" Width="300px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Адреса об'єкта" FieldName="addr_name" VisibleIndex="5" Width="200px">
        </dx:GridViewDataTextColumn>
--%>
        <dx:GridViewDataTextColumn Caption="Назва основних засобів" FieldName="name" VisibleIndex="6" Width="200px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Інвентарний номер" FieldName="inv_number" VisibleIndex="7" Width="160px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Площа, яка враховується на балансі" FieldName="sqr_total" VisibleIndex="8" Width="120px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn Caption="Дата введення в експлуатацію" FieldName="commissioned_date" VisibleIndex="9" Width="100px">
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn Caption="Первісна вартість, тис. грн." FieldName="initial_cost" VisibleIndex="10" Width="120px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Знос, тис. грн." FieldName="znos" VisibleIndex="11" Width="120px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Залишкова вартість, тис. грн." FieldName="remaining_cost" VisibleIndex="11" Width="120px">
        </dx:GridViewDataTextColumn>

<%--  
        <dx:GridViewBandColumn Caption="Дата введення в експлуатацію" VisibleIndex="11">
            <Columns>
                <dx:GridViewDataTextColumn Caption="рік" FieldName="commissioned_date_year" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataComboBoxColumn Caption="місяць" FieldName="commissioned_date_month" Name="ColumnCommissionedDateMonth" VisibleIndex="1">
                    <PropertiesComboBox TextField="name" ValueField="id" ValueType="System.Int32">
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
            </Columns>
        </dx:GridViewBandColumn>

        <dx:GridViewBandColumn Caption="Дата списання" HeaderStyle-HorizontalAlign="Center" VisibleIndex="12">
            <Columns>
                <dx:GridViewDataTextColumn Caption="рік" FieldName="decommissioned_date_year" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataComboBoxColumn Caption="місяць" FieldName="decommissioned_date_month" Name="ColumnDecommissionedDateMonth" 
                    VisibleIndex="1">
                    <PropertiesComboBox TextField="name" ValueField="id" ValueType="System.Int32">
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
            </Columns>
        </dx:GridViewBandColumn>
--%>
        <dx:GridViewBandColumn Caption="Документ, що встановлює право користування об’єктом" HeaderStyle-HorizontalAlign="Center" VisibleIndex="13">
            <Columns>
                <dx:GridViewDataTextColumn Caption="Номер" FieldName="doc_number_on" VisibleIndex="1" Width="100px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="Дата" FieldName="doc_date_on" VisibleIndex="2" Width="100px">
                </dx:GridViewDataDateColumn>

            </Columns>
        </dx:GridViewBandColumn>
<%--
        <dx:GridViewBandColumn Caption="Дата постановки на баланс" HeaderStyle-HorizontalAlign="Center" VisibleIndex="15">
            <Columns>
                <dx:GridViewDataTextColumn Caption="рік" FieldName="on_balance_date_year" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataComboBoxColumn Caption="місяць" FieldName="on_balance_date_month" Name="ColumnOnBalanceDateMonth" VisibleIndex="1">
                    <PropertiesComboBox TextField="name" ValueField="id" ValueType="System.Int32">
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
           </Columns>
        </dx:GridViewBandColumn>
--%>
        <dx:GridViewDataTextColumn Caption="Примітка, якщо не використовується" FieldName="comments" VisibleIndex="14" Width="200px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewBandColumn Caption="Документ, що припиняє право користування об’єктом" HeaderStyle-HorizontalAlign="Center" VisibleIndex="15">
            <Columns>
                <dx:GridViewDataTextColumn Caption="Номер" FieldName="doc_number_off" VisibleIndex="1" Width="100px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="Дата" FieldName="doc_date_off" VisibleIndex="2" Width="100px">
                </dx:GridViewDataDateColumn>

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
    <SettingsCookies CookiesID="GUKV.BalansInventarizations" Version="A1" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewInventarizationsInit" EndCallback="GridViewInventarizationsEndCallback" />
</dx:ASPxGridView>

</center>

</asp:Content>
