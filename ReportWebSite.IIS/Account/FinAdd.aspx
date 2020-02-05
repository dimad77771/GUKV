<%@ page language="C#" autoeventwireup="true" inherits="Account_FinAdd, App_Web_finadd.aspx.dae9cef9" masterpagefile="~/NoMenu.master" title="Інформація" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Увага
    </h2>
    <p style="width: 600px;">       
        Шановні балансоутримувачі,
        повідомляємо Вам, що новий метод подання звіту щодо використання комунального майна, у майбутньому буде поширений також на фінансову звітність. Про терміни впровадження зовнішньої подачі фінансової звітності буде повідомлено пізніше.
        Пропозиції та зауваження чекаємо на адресу: 
    </p>
    <p>
        <dx:ASPxHyperLink ID="LinkToCabinet" runat="server" Text="Продовжити" NavigateUrl="~/Reports1NF/Cabinet.aspx" />
    </p>
</asp:Content>
  