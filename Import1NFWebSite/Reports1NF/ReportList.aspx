<%@ Page Title="Перелік Звітів 1НФ" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ReportList.aspx.cs" Inherits="Reports1NF_ReportList" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<asp:SqlDataSource ID="SqlDataSourceReports" runat="server" 
    ConnectionString="<%$ ConnectionStrings:Import1NFConnectionString %>" 
    SelectCommand="SELECT * FROM view_reports">
</asp:SqlDataSource>

<dx:ASPxGridView
    ID="GridViewReports"
    ClientInstanceName="GridViewReports"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceReports"
    KeyFieldName="report_id" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" VisibleIndex="0" Caption="Код ЄДРПОУ" Width="80px" />
        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" VisibleIndex="1" Caption="Назва Організації" Width="400px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowReportErrorList(" + Eval("report_id") + ")\">" + Eval("full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="import_date" ReadOnly="True" VisibleIndex="2" Caption="Дата Імпорту Звіту" />
        <dx:GridViewDataTextColumn FieldName="folder_path" ReadOnly="True" VisibleIndex="3" Caption="Назва Папки Звіту" Width="400px" />
        <dx:GridViewDataTextColumn FieldName="report_status" ReadOnly="True" VisibleIndex="4" Caption="Стан" />
        <dx:GridViewDataTextColumn FieldName="error_count" ReadOnly="True" VisibleIndex="5" Caption="Кількість Необхідних Уточнень" />
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="zkpo_code" SummaryType="Count" DisplayFormat="{0} звітів" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="False" AutoFilterRowInputDelay="2000" ColumnResizeMode="Control" />
    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="False"
        ShowFilterBar="Auto"
        ShowHeaderFilterButton="True"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True" />
    <SettingsCookies CookiesID="GUKV.Import1NF.Reports" Version="1" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

</asp:Content>
