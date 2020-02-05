<%@ Page Title="Перелік Необхідних Уточнень" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ReportErrors.aspx.cs" Inherits="Reports1NF_ReportErrors" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript" language="javascript">

// <![CDATA[

    function OnAllowAllErrors(s, e) {

        GridViewErrors.PerformCallback("allowall:");
    }

    function OnDenyAllErrors(s, e) {

        GridViewErrors.PerformCallback("denyall:");
    }

// ]]>

</script>

<asp:SqlDataSource ID="SqlDataSourceErrors" runat="server" 
    ConnectionString="<%$ ConnectionStrings:Import1NFConnectionString %>" 
    SelectCommand="SELECT * FROM view_validation_errors WHERE report_id = @rid ORDER BY is_critical DESC, error_kind">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rid" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceReportDetails" runat="server" 
    ConnectionString="<%$ ConnectionStrings:Import1NFConnectionString %>" 
    SelectCommand="SELECT * FROM view_reports WHERE report_id = @rid">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rid" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:FormView runat="server" BorderStyle="None" ID="ViewReportDetails" DataSourceID="SqlDataSourceReportDetails" EnableViewState="False">
    <ItemTemplate>
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text='Організація:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="LabelOrgName" runat="server" Text='<%# Eval("full_name") %>' /> </td>
                <td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
                <td> <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text='Код ЄДРПОУ:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="LabelOrgZKPO" runat="server" Text='<%# Eval("zkpo_code") %>' /> </td>
            </tr>
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text='Стан:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="LabelReportState" runat="server" Text='<%# Eval("report_status") %>' /> </td>
                <td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
                <td> <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text='Директор:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="LabelDirectorName" runat="server" Text='<%# Eval("director_fio") %>' /> </td>
            </tr>
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text='Кількість необхідних уточнень:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="LabelTotalErrCount" runat="server" Text='<%# Eval("error_count") %>' /> </td>
                <td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
                <td> <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text='Телефон Директора:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="LabelDirectorPhone" runat="server" Text='<%# Eval("director_phone") %>' /> </td>
            </tr>
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text='Кількість дозволених уточнень:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="LabelNumOverriddenErrors" runat="server" Text='<%# Eval("overridden_error_count") %>' /> </td>
                <td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
                <td> <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text='Бухгалтер:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="LabelBuhgalterName" runat="server" Text='<%# Eval("buhgalter_fio") %>' /> </td>
            </tr>
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text='Ким збережено:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text='<%# Eval("modified_by") %>' /> </td>
                <td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
                <td> <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text='Телефон Бухгалтера:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="LabelBuhgalterPhone" runat="server" Text='<%# Eval("buhgalter_phone") %>' /> </td>
            </tr>
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text='Коли збережено:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text='<%# Eval("modify_date") %>' /> </td>
                <td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
                <td> <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text='Електронна Адреса:' /> </td>
                <td>&nbsp;</td>
                <td> <dx:ASPxHyperLink ID="LinkEmail" runat="server" Text='<%# Eval("contact_email") %>' NavigateUrl='<%# string.Format("mailto:{0}", Eval("contact_email")) %>' /> </td>
            </tr>
        </table>
    </ItemTemplate>
</asp:FormView>

<br/>

<dx:ASPxGridViewExporter ID="GridViewExporterErrors" runat="server" 
    FileName="Перелiк Необхiдних Уточнень 1НФ" GridViewID="GridViewErrors" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView
    ID="GridViewErrors"
    ClientInstanceName="GridViewErrors"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    KeyFieldName="id"
    OnCustomCallback="GridViewErrors_CustomCallback" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="error_kind_descr" ReadOnly="True" VisibleIndex="0" Caption="Категорія Запису" Width="120px" />
        <dx:GridViewDataTextColumn FieldName="object_descr" ReadOnly="True" VisibleIndex="1" Caption="Запис у Звіті" Width="500px" />
        <dx:GridViewDataTextColumn FieldName="error_msg" ReadOnly="True" VisibleIndex="2" Caption="Потрібне Уточнення" Width="500px" />
        <dx:GridViewDataCheckColumn FieldName="is_overridden" VisibleIndex="3" Caption="Дозволити Імпорт Без Уточнення">
            <DataItemTemplate>
                <dx:ASPxCheckBox ID="chk" runat="server"
                    AutoPostBack="True"
                    ValueType="System.Int32"
                    ValueChecked="1"
                    ValueUnchecked="0"
                    Value='<%# Eval("is_overridden") %>'
                    OnInit="chk_Init"
                    OnCheckedChanged="chk_Checked" />
            </DataItemTemplate>
        </dx:GridViewDataCheckColumn>
        <dx:GridViewDataTextColumn FieldName="modified_by" ReadOnly="True" VisibleIndex="4" Caption="Ким Змінено" Width="100px" />
        <dx:GridViewDataDateColumn FieldName="modify_date" ReadOnly="True" VisibleIndex="5" Caption="Дата Зміни" Width="100px" />
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="error_kind_descr" SummaryType="Count" DisplayFormat="{0} необхідних уточнень" />
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
    <SettingsCookies CookiesID="GUKV.Import1NF.ObjectErrors" Version="1" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<br/>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonSaveChanges" runat="server" 
                Text="Зберегти зміни" 
                OnClick="ButtonSaveChanges_Click"
                Width="180px">
            </dx:ASPxButton>
        </td>
        <td>&nbsp;</td>
        <td>
            <dx:ASPxButton ID="ButtonExportErrors" runat="server" 
                Text="Експортувати в Excel" 
                OnClick="ButtonExportErrors_Click"
                Width="180px">
            </dx:ASPxButton>
        </td>
        <td>&nbsp;</td>
        <td>
            <dx:ASPxButton ID="ButtonAllowAll" runat="server" AutoPostBack="False"
                ClientInstanceName="ButtonAllowAll"
                Text="Дозволити всі уточнення" 
                Width="180px">
                <ClientSideEvents Click="OnAllowAllErrors" />
            </dx:ASPxButton>
        </td>
        <td>&nbsp;</td>
        <td>
            <dx:ASPxButton ID="ButtonDenyAll" runat="server"  AutoPostBack="False"
                ClientInstanceName="ButtonDenyAll"
                Text="Заборонити всі уточнення" 
                Width="190px">
                <ClientSideEvents Click="OnDenyAllErrors" />
            </dx:ASPxButton>
        </td>
    </tr>
</table>

</asp:Content>