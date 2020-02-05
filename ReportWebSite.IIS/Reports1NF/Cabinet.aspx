<%@ page language="C#" autoeventwireup="true" inherits="Reports1NF_Cabinet, App_Web_cabinet.aspx.5d94abc0" masterpagefile="~/NoMenu.master" title="Звіт з використання комунальної власності" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceReport" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT * FROM view_reports1nf WHERE report_id = @rep_id"
    OnSelecting="SqlDataSourceReport_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" >
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

<br/>

<asp:FormView runat="server" BorderStyle="None" ID="ReportStatus" DataSourceID="SqlDataSourceReport" EnableViewState="False">
    <ItemTemplate>

        <table border="0" cellspacing="0" cellpadding="2">
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Назва організації:" /> </td>
                <td> &nbsp; &nbsp; </td>
                <td> <dx:ASPxLabel ID="LabelOrgName" ClientInstanceName="LabelOrgName" runat="server" Text='<%# Eval("full_name") %>' /> </td>
            </tr>
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Код ЄДРПОУ організації:" /> </td>
                <td> &nbsp; &nbsp; </td>
                <td> <dx:ASPxLabel ID="LabelOrgZKPO" ClientInstanceName="LabelOrgZKPO" runat="server" Text='<%# Eval("zkpo_code") %>' /> </td>
            </tr>
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Поточний стан звіту у балансоутримувача:" /> </td>
                <td> &nbsp; &nbsp; </td>
                <td> <dx:ASPxHyperLink ID="LinkReportState" ClientInstanceName="LinkReportState" runat="server" Text='<%# Eval("cur_state") %>' Target="_blank" NavigateUrl="" /> </td>
            </tr>
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Поточний стан звіту в ДКВ:" /> </td>
                <td> &nbsp; &nbsp; </td>
                <td> <dx:ASPxHyperLink ID="LinkReportStateDKV" ClientInstanceName="LinkReportStateDKV" runat="server" Text='<%# Eval("review_state") %>' Target="_blank" NavigateUrl="" /> </td>
            </tr>
            <tr>
                <td> <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Дата останнього надсилання звіту:" /> </td>
                <td> &nbsp; &nbsp; </td>
                <td> <dx:ASPxDateEdit ID="EditReportDueDate" runat="server" ReadOnly="true" Value='<%# GetMaxDate( Eval("bal_max_submit_date"), Eval("org_max_submit_date") ) %>' /> </td>
            </tr>
        </table>

    </ItemTemplate>
</asp:FormView>

<h1 style="font-size: 2.2em; margin: 0 0 20px 0; padding: 0 0 20px 20px; border-bottom: 1px solid #DAE5F2;"></h1>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td valign="top"> <dx:ASPxHyperLink runat="server" ID="LinkOrgInfo" Text="Загальна Інформація" NavigateUrl="../Reports1NF/OrgInfo.aspx" CssClass="reporttitle"/> </td>
        <td> &nbsp; &nbsp; </td>
        <td valign="top"> <dx:ASPxLabel runat="server" ID="ASPxLabel5" Width="600px" Text="Перегляд та редагування відомостей про організацію, що подає звіт про використання об'єктів комунальної власності"/> <p style="font-size: 4px;"/> </td>
    </tr>
    <tr>
        <td valign="top"> <dx:ASPxHyperLink runat="server" ID="LinkBalansObjects" Text="Об'єкти на Балансі" NavigateUrl="../Reports1NF/OrgBalansList.aspx" CssClass="reporttitle"/> </td>
        <td> &nbsp; &nbsp; </td>
        <td valign="top"> <dx:ASPxLabel runat="server" ID="ASPxLabel3" Width="600px" Text="Перегляд та редагування відомостей про об'єкти комунальної власності, що знаходяться на балансі організації"/> <p style="font-size: 4px;"/> </td>
    </tr>
    <tr>
        <td valign="top"> <dx:ASPxHyperLink runat="server" ID="LinkBalansDeleted" Text="Відчужені Об'єкти" NavigateUrl="../Reports1NF/OrgBalansDeletedList.aspx" CssClass="reporttitle"/> </td>
        <td> &nbsp; &nbsp; </td>
        <td valign="top"><dx:ASPxLabel runat="server" ID="ASPxLabel4" Width="600px" Text="Перегляд та редагування відомостей про відчужені об'єкти комунальної власності, що знаходились на балансі організації"/> <p style="font-size: 4px;"/> </td>
    </tr>
    <tr>
        <td valign="top"> <dx:ASPxHyperLink runat="server" ID="LinkRentAgreements" Text="Договори використання приміщень " NavigateUrl="../Reports1NF/OrgArendaList.aspx" CssClass="reporttitle"/> </td>
        <td> &nbsp; &nbsp; </td>
        <td valign="top"><dx:ASPxLabel runat="server" ID="ASPxLabel6" Width="600px" Text="Перегляд та редагування відомостей про надання в оренду іншим організаціям об'єктів комунальної власності, що знаходяться на балансі організації"/> <p style="font-size: 4px;"/> </td>
    </tr>
    <tr>
        <td valign="top"> <dx:ASPxHyperLink runat="server" ID="LinkRentedObjects" Text="Договори Орендування" NavigateUrl="../Reports1NF/OrgRentedList.aspx" CssClass="reporttitle"/> </td>
        <td> &nbsp; &nbsp; </td>
        <td valign="top"><dx:ASPxLabel runat="server" ID="ASPxLabel7" Width="600px" Text="Перегляд та редагування відомостей про орендування об'єктів комунальної власності, що знаходяться на балансі інших організацій"/> </td>
    </tr>
</table>

<h1 style="font-size: 2.2em; margin: 0 0 20px 0; padding: 0 0 20px 20px; border-bottom: 1px solid #DAE5F2;"></h1>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonPrint" runat="server" Text="Зберегти у Файл" OnClick="ButtonPrint_Click" CausesValidation="false"></dx:ASPxButton>
            <dx:ASPxButton ID="ButtonVerify" runat="server" Text="Звіт Перевірено" OnClick="ButtonVerify_Click" CausesValidation="false" ClientVisible="false"></dx:ASPxButton>
        </td>
        <td> &nbsp; &nbsp; </td>
        <td> <dx:ASPxButton ID="ButtonLogout" runat="server" Text="Вийти" OnClick="ButtonLogout_Click" CausesValidation="false"></dx:ASPxButton> </td>
    </tr>
</table>

</asp:Content>
