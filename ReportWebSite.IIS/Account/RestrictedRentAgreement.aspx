<%@ page language="C#" autoeventwireup="true" inherits="Account_RestrictedRentAgreement, App_Web_restrictedrentagreement.aspx.dae9cef9" masterpagefile="~/NoMenu.master" title="Доступ обмежено" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Неможливо відкрити картку договору оренди
    </h2>
    <p>
        Можливості Вашого облікового запису в системі не дозволяють переглядати картку даного договору оренди. Будь ласка, увійдіть в систему як інший користувач, або продовжуйте працювати з системою з початкової сторінки.
    </p>
    <p>
        <dx:ASPxHyperLink ID="LinkLogin" runat="server" Text="Увійти в систему як інший користувач" NavigateUrl="~/Account/Login.aspx" />
    </p>
    <p>
        <dx:ASPxHyperLink ID="LinkReturn" runat="server" Text="Повернутись на головну сторінку" NavigateUrl="~/Default.aspx" />
    </p>
</asp:Content>
