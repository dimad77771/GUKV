<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RDAReportList.aspx.cs" Inherits="Reports1NF_RDAReportList"
    MasterPageFile="~/NoMenu.master" Title="Перелік звітів 1НФ підпорядкованих організацій" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function ShowReportCard(reportId) {

        var cardUrl = "../Reports1NF/Cabinet.aspx?rid=" + reportId;

        window.open(cardUrl);
    }

    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceRdaReports" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT rep.*,
        (SELECT MAX(sdt) FROM (VALUES
            (rep.bal_max_submit_date),
            (rep.bal_del_max_submit_date),
            (rep.arenda_max_submit_date),
            (rep.arenda_rented_max_submit_date),
            (rep.org_max_submit_date)) AS AllMaxSubmitDates(sdt)) AS 'max_submit_date',
        CASE WHEN rep.is_reviewed = 0 THEN N'НI' ELSE N'ТАК' END AS 'review_performed'
        FROM view_reports1nf rep WHERE rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = @distr_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="distr_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxGridViewExporter ID="GridViewRdaReports1NFExporter" runat="server" 
    FileName="ПерелікЗвітів" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView
    ID="PrimaryGridView"
    ClientInstanceName="PrimaryGridView"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceRdaReports"
    KeyFieldName="id" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="0" Caption="Картка Звіту">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowReportCard(" + Eval("report_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False" AllowHeaderFilter="False" AllowAutoFilter="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Caption="Назва Організації" Width="300px">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_zkpo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Код ЄДРПОУ">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("zkpo_code") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cur_state" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Стан Звіту" />
        <dx:GridViewDataDateColumn FieldName="max_submit_date" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Дата Останнього Надсилання до ДКВ">
            <Settings AllowHeaderFilter="False" />
        </dx:GridViewDataDateColumn>
    </Columns>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager PageSize="25"></SettingsPager>
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
    <SettingsCookies CookiesID="GUKV.Reports1NF.RdaReportList" Version="A2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

</asp:Content>
