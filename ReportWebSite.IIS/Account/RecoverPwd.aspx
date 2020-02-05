<%@ page language="C#" autoeventwireup="true" inherits="Account_RecoverPwd, App_Web_recoverpwd.aspx.dae9cef9" masterpagefile="~/Site.master" title="Відновлення паролю" %>

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
