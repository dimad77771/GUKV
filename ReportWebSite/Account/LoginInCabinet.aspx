<%@ Page Title="Log In" Language="C#" MasterPageFile="~/NoHeader.master" AutoEventWireup="true"
    CodeFile="LoginInCabinet.aspx.cs" Inherits="Account_Login" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Вхід в кабінет
    </h2>
    <p>
        Будь ласка введіть вашу електронну адресу та пароль.
    </p>
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" 
        RenderOuterTable="false" 
        FailureText="Електронна адреса або пароль не є вірним" 
        DestinationPageUrl="~/Default.aspx"
        OnLoggedIn="LoginUser_LoggedIn">
        <LayoutTemplate>
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="LoginUserValidationGroup"/>
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Вхідна інформація</legend>
                    <p>
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Електронна адреса:</asp:Label>
                        <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                             CssClass="failureNotification"
                             ErrorMessage="Для входу на сайт необхідно ввести електронну адресу користувача."
                             ToolTip="Для входу на сайт необхідно ввести електронну адресу користувача." 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Пароль:</asp:Label>
                        <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                             CssClass="failureNotification"
                             ErrorMessage="Для входу на сайт необхідно ввести пароль користувача."
                             ToolTip="Для входу на сайт необхідно ввести пароль користувача." 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:CheckBox ID="RememberMe" runat="server" Checked="true"/>
                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Запам'ятати мене</asp:Label>
                    </p>
                </fieldset>
                <p class="submitButton">
                    <dx:ASPxButton ID="LoginButton" runat="server" CommandName="Login" Text="Увійти" 
                        ValidationGroup="LoginUserValidationGroup3"></dx:ASPxButton>
                </p>
            </div>
        </LayoutTemplate>
    </asp:Login>
</asp:Content>