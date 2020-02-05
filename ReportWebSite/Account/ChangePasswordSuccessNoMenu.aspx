<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/NoMenu.master" AutoEventWireup="true"
    CodeFile="ChangePasswordSuccessNoMenu.aspx.cs" Inherits="Account_ChangePasswordSuccess" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
        <Items>
            <dx:MenuItem NavigateUrl="../Reports1NF/Cabinet.aspx" Text="Стан"></dx:MenuItem>
            <dx:MenuItem NavigateUrl="../Reports1NF/OrgInfo.aspx" Text="Загальна Інформація"></dx:MenuItem>
            <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansList.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
            <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansDeletedList.aspx" Text="Відчужені Об'єкти"></dx:MenuItem>
            <dx:MenuItem NavigateUrl="../Reports1NF/OrgArendaList.aspx" Text="Договори використання приміщень "></dx:MenuItem>
            <dx:MenuItem NavigateUrl="../Reports1NF/OrgRentedList.aspx" Text="Договори Орендування"></dx:MenuItem>
            <dx:MenuItem NavigateUrl="../Account/Logout.aspx" Text="Вийти"></dx:MenuItem>
            <dx:MenuItem NavigateUrl="~/Account/ChangePasswordNoMenu.aspx" Text="Пароль"></dx:MenuItem>
        </Items>
    </dx:ASPxMenu>
    <h2>
        Зміна паролю
    </h2>
    <p>
        Ваш пароль успішно змінено на новий.
    </p>
</asp:Content>
