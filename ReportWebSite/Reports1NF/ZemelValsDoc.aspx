<%@ Page Title="Лист" Language="C#" MasterPageFile="~/NoHeader.master" AutoEventWireup="true"
    CodeFile="ZemelValsDoc.aspx.cs" Inherits="Account_Register" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2 style="display: none">Реєстрація нового користувача
    </h2>
    <p style="display: none">
        Для реєстрації на сайті, будь ласка, заповніть цю форму.
    </p>

    <div class="accountInfo">
        <fieldset class="register">
            <%--<legend>Інформація про користувача</legend>--%>
            <p>
                <asp:Label runat="server" AssociatedControlID="Column_1">Лист Департаменту земельних ресурсів (від):</asp:Label>
                <asp:TextBox ID="Column_1" runat="server" CssClass="textEntry"></asp:TextBox>
            </p>

            <p>
                <asp:Label runat="server" AssociatedControlID="Column_2">Лист Департаменту земельних ресурсів (номер):</asp:Label>
                <asp:TextBox ID="Column_2" runat="server" CssClass="textEntry"></asp:TextBox>
            </p>

            <p>
                <asp:Label runat="server" AssociatedControlID="Column_3">Земельній ділянці з кадастровим номером:</asp:Label>
                <asp:TextBox ID="Column_3" runat="server" CssClass="textEntry"></asp:TextBox>
            </p>

            <p>
                <asp:Label runat="server" AssociatedControlID="Column_4">Адреса (вулиця):</asp:Label>
                <asp:TextBox ID="Column_4" runat="server" CssClass="textEntry"></asp:TextBox>
            </p>

            <p>
                <asp:Label runat="server" AssociatedControlID="Column_5">Адреса (номер):</asp:Label>
                <asp:TextBox ID="Column_5" runat="server" CssClass="textEntry"></asp:TextBox>
            </p>

            <p>
                <asp:Label runat="server" AssociatedControlID="Column_6">У районі м. Києва:</asp:Label>
                <asp:TextBox ID="Column_6" runat="server" CssClass="textEntry"></asp:TextBox>
            </p>

            <p>
                <asp:Label runat="server" AssociatedControlID="Column_7">Шаблон пошуку:</asp:Label>
                <asp:TextBox ID="Column_7" runat="server" CssClass="textEntry"></asp:TextBox>
            </p>



        </fieldset>
        <p class="submitButton">
            <dx:ASPxButton runat="server" Text="Створити лист" OnClick="Unnamed_Click">
            </dx:ASPxButton>
        </p>
    </div>




</asp:Content>




