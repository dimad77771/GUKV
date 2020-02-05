<%@ Page Title="Документація" Language="C#" MasterPageFile="~/NoHeader.master" AutoEventWireup="true" CodeFile="UserGuide.aspx.cs" Inherits="UserGuide_UserGuide" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <br/>

    <p class="SpacingPara"/>

    <p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
        <asp:Label runat="server" ID="ASPxLabel19" Text="Довідка" CssClass="pagetitle"/>
    </p>
    <br/>
<table border="0" cellspacing="0" cellpadding="2px">
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

    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="ASPxHyperLink6" runat="server" Text="Відображення вільних приміщень" NavigateUrl="~/UserGuide/Відображення вільних приміщень в ЄІС ДКВ.pdf" /> </td>
    </tr>

    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="ASPxHyperLink7" runat="server" Text="Пакетна пердача обєктів" NavigateUrl="~/UserGuide/Пакетна пердача обєктів.pdf" /> </td>
    </tr>

    
    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="ASPxHyperLink8" runat="server" Text="Опис передачі договорів під час передачі обєкту" NavigateUrl="~/UserGuide/Опис передачі договорів під час передачі обєкту.pdf" /> </td>
    </tr>

    
    <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="ASPxHyperLink9" runat="server" Text="Відображення фотофіксації та планів" NavigateUrl="~/UserGuide/Відображення фотофіксації та планів в ЄІС ДКВ.pdf" /> </td>
    </tr>

        <tr>
        <td> &nbsp; </td>

        <td valign="bottom"> <img src="../Styles/HelpIcon.png" alt="Довідка" /> </td>

        <td> &nbsp; </td>

        <td valign="top"> <dx:ASPxHyperLink ID="ASPxHyperLink11" runat="server" Text="Інструкція користувача - розпорядчі документи" NavigateUrl="~/UserGuide/Інструкція користувача - розпорядчі документи.pdf" /> </td>
    </tr>

    <tr><td colspan="4"> &nbsp; </td></tr>

    <tr><td colspan="4"><dx:ASPxHyperLink ID="ASPxHyperLink10" runat="server" Text="Зміни та новини проекту" NavigateUrl="~/Account/FinAdd.aspx" /></td></tr>

</table>

</asp:Content>
