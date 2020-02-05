<%@ page title="Документація" language="C#" masterpagefile="~/NoHeader.master" autoeventwireup="true" inherits="UserGuide_UserGuide, App_Web_userguide.aspx.2515b5d7" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="LinkHelp" runat="server" Text="Інструкція Користувача" NavigateUrl="~/UserGuide/Iнструкцiя Користувача ЄIС ДКВ.pdf" /> </td>
    </tr>

    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text="Інструкція для зміни балансоутримувачів об’єктів" NavigateUrl="~/UserGuide/Инструкция по смене балансодержателей.pdf" /> </td>
    </tr>

    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" Text="Інструкція з використання програмного модуля «пошук за переліком адрес»" NavigateUrl="~/UserGuide/Инструкція з використання_Масовий вибір адрес.pdf" /> </td>
    </tr>

    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="ASPxHyperLink3" runat="server" Text="Подача звітності про використання комунального майна м. Києва (Інструкція з використання)" NavigateUrl="~/UserGuide/Інструкція для балансоутримувачів.pdf" /> </td>
    </tr>

    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="ASPxHyperLink4" runat="server" Text="Методика перегляду і перевірки звітності про використання комунального майна м. Києва (Інструкція з використання)" NavigateUrl="~/UserGuide/Інструкція по використанню зовнішньої звітності  співробітниками ДКВ.pdf" /> </td>
    </tr>

    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="ASPxHyperLink5" runat="server" Text="Єдина інформаційна система департаменту комунальної власності м. Києва (Інструкція для співробітників районних державних адміністрацій)" NavigateUrl="~/UserGuide/Інструкція по використанню зовнішньої звітності  співробітниками РДА.pdf" /> </td>
    </tr>
</table>

</asp:Content>
