<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RestrictedObject.aspx.cs" Inherits="Account_RestrictedObject"
    MasterPageFile="~/NoMenu.master" Title="Доступ обмежено" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Неможливо відкрити картку об'єкту
    </h2>
    <p>
        Можливості Вашого облікового запису в системі не дозволяють переглядати картку даного об'єкту. Будь ласка, увійдіть в систему як інший користувач, або продовжуйте працювати з системою з початкової сторінки.
    </p>
    <p>
        <dx:ASPxHyperLink ID="LinkLogin" runat="server" Text="Увійти в систему як інший користувач" NavigateUrl="~/Account/Login.aspx" />
    </p>
    <p>
        <dx:ASPxHyperLink ID="LinkReturn" runat="server" Text="Повернутись на головну сторінку" NavigateUrl="~/Default.aspx" />
    </p>
</asp:Content>
