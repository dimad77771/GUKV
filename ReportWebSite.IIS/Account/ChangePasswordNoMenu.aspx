<%@ page title="Change Password" language="C#" masterpagefile="~/NoMenu.master" autoeventwireup="true" inherits="Account_ChangePassword, App_Web_changepasswordnomenu.aspx.dae9cef9" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

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
            <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingRequestsList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>
            <dx:MenuItem NavigateUrl="../Account/Logout.aspx" Text="Вийти"></dx:MenuItem>
            <dx:MenuItem NavigateUrl="~/Account/ChangePasswordNoMenu.aspx" Text="Пароль"></dx:MenuItem>
        </Items>
    </dx:ASPxMenu>
    <h2>
        Зміна паролю
    </h2>
    <p>
        Для зміни паролю, будь ласка, заповніть цю форму.
    </p>
    <p>
        Мінімальна припустима довжина нового паролю складає <%= Membership.MinRequiredPasswordLength %> символів.
    </p>
    <asp:ChangePassword ID="ChangeUserPassword" runat="server" 
        CancelDestinationPageUrl="~/" EnableViewState="false" RenderOuterTable="false" 
         SuccessPageUrl="ChangePasswordSuccessNoMenu.aspx" 
        ChangePasswordFailureText="Введений пароль не є достатньо стійким. Мінімальна припустима довжина паролю складає {0} символів.">
        <ChangePasswordTemplate>
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="ChangeUserPasswordValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="ChangeUserPasswordValidationGroup"/>
            <div class="accountInfo">
                <fieldset class="changePassword">
                    <legend>Зміна паролю</legend>
                    <p>
                        <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Існуючий пароль:</asp:Label>
                        <asp:TextBox ID="CurrentPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword" 
                             CssClass="failureNotification"
                             ErrorMessage="Необхідно ввести Ваш існуючий пароль."
                             ToolTip="Необхідно ввести Ваш існуючий пароль." 
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">Новий пароль:</asp:Label>
                        <asp:TextBox ID="NewPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword" 
                             CssClass="failureNotification"
                             ErrorMessage="Необхідно ввести новий пароль."
                             ToolTip="Необхідно ввести новий пароль." 
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Підтвердження нового паролю:</asp:Label>
                        <asp:TextBox ID="ConfirmNewPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword" 
                             CssClass="failureNotification" Display="Dynamic"
                             ErrorMessage="Необхідно ввести підтвердження нового паролю."
                             ToolTip="Необхідно ввести підтвердження нового паролю."
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                             CssClass="failureNotification" Display="Dynamic"
                             ErrorMessage="Підтвердження нового паролю має бути ідентичним Вашому новому паролю."
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:CompareValidator>
                    </p>
                </fieldset>
                <p class="submitButton">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td><dx:ASPxButton ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword" Text="Змінити пароль" 
                                ValidationGroup="ChangeUserPasswordValidationGroup"></dx:ASPxButton></td>
                            <td width="8px"></td>
                            <td><dx:ASPxButton ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Відмінити">
                                </dx:ASPxButton></td>
                        </tr>
                    </table>
                </p>
            </div>
        </ChangePasswordTemplate>
    </asp:ChangePassword>
</asp:Content>