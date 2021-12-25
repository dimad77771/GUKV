<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecoverPwd.aspx.cs" Inherits="Account_RecoverPwd" MasterPageFile="~/NoHeader.master" Title="Відновлення паролю" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<asp:PasswordRecovery ID="PasswordRecovery1" Runat="server"
    SuccessText="Новий пароль успішно згенеровано, та відправлено на електронну адресу, яку Ви вказали під час реєстрації."
    UserNameLabelText="Ім'я користувача:"
    UserNameTitleText="Створення нового паролю"
    UserNameInstructionText="Для створення нового паролю, будь ласка, введіть ваше ім'я в системі">
</asp:PasswordRecovery>

</asp:Content>
