<%@ page language="C#" autoeventwireup="true" inherits="Reports1NF_Help, App_Web_help.aspx.5d94abc0" masterpagefile="~/NoMenu.master" title="Довідка" %>


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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
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

    <br/>

    <p class="SpacingPara"/>

    <p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
        <asp:Label runat="server" ID="ASPxLabel19" Text="Довідка" CssClass="pagetitle"/>
    </p>
    
    <br/>
    <table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="LinkHelp1" runat="server" Text="Завантажити Інструкцію для балансоутримувачів" NavigateUrl="~/Reports1NF/UserGuide/Інструкція для балансоутримувачів.pdf" /> </td>
    </tr>

    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="LinkHelp2" runat="server" Text="Інструкція для зміни балансоутримувачів об’єктів" NavigateUrl="~/Reports1NF/UserGuide/Инструкция по смене балансодержателей.pdf" /> </td>
    </tr>
    </table>
    
    

</asp:Content>

