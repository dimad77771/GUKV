<%@ page title="Register" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" inherits="Account_Register, App_Web_1hdydluz" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:CreateUserWizard ID="RegisterUser" runat="server" EnableViewState="false" 
        OnCreatedUser="RegisterUser_CreatedUser" 
        CompleteSuccessText="Користувач успішно зареєстрований." 
        ContinueButtonText="Продовжити" 
        DuplicateEmailErrorMessage="Вказана електронна адреса вже використовується іншим користувачем. Будь ласка, введіть іншу електронну адресу." 
        DuplicateUserNameErrorMessage="Користувач з таким ім'ям вже зареєстрований на сайті. Будь ласка, введіть інше ім'я користувача." 
        InvalidEmailErrorMessage="Будь ласка, введіть коректну електронну адресу." 
        InvalidPasswordErrorMessage="Мінімальна довжина паролю: {0} символів" 
        UnknownErrorMessage="Помилка при створенні нового користувача. Будь ласка, спробуйте знову." 
        ContinueDestinationPageUrl="~/Account/RegisterSuccess.aspx">
        <LayoutTemplate>
            <asp:PlaceHolder ID="wizardStepPlaceholder" runat="server"></asp:PlaceHolder>
            <asp:PlaceHolder ID="navigationPlaceholder" runat="server"></asp:PlaceHolder>
        </LayoutTemplate>
        <WizardSteps>
            <asp:CreateUserWizardStep ID="RegisterUserWizardStep" runat="server">
                <ContentTemplate>
                    <h2>
                        Реєстрація нового користувача
                    </h2>
                    <p>
                        Для реєстрації на сайті, будь ласка, заповніть цю форму.
                    </p>
                    <p>
                        Мінімальна припустима довжина паролю складає <%= Membership.MinRequiredPasswordLength %> символів.
                    </p>
                    <span class="failureNotification">
                        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                    </span>
                    <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" CssClass="failureNotification" 
                         ValidationGroup="RegisterUserValidationGroup"/>
                    <div class="accountInfo">
                        <fieldset class="register">
                            <legend>Інформація про користувача</legend>
                            <p>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Ім'я користувача:</asp:Label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести ім'я користувача."
                                     ToolTip="Для реєстрації необхідно ввести ім'я користувача." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">Електронна адреса:</asp:Label>
                                <asp:TextBox ID="Email" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести електронну адресу користувача."
                                     ToolTip="Для реєстрації необхідно ввести електронну адресу користувача." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Пароль:</asp:Label>
                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести пароль."
                                     ToolTip="Для реєстрації необхідно ввести пароль." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Підтвердження паролю:</asp:Label>
                                <asp:TextBox ID="ConfirmPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" CssClass="failureNotification"
                                     Display="Dynamic" 
                                     ErrorMessage="Для реєстрації необхідно ввести підтвердження паролю."
                                     ID="ConfirmPasswordRequired"
                                     runat="server" 
                                     ToolTip="Для реєстрації необхідно ввести підтвердження паролю."
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                     CssClass="failureNotification" Display="Dynamic"
                                     ErrorMessage="Для успішної реєстрації пароль та підтвердження паролю мають співпадати."
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:CompareValidator>
                            </p>
                        </fieldset>
                        <p class="submitButton">
                            <dx:ASPxButton ID="CreateUserButton" runat="server" CommandName="MoveNext" Text="Зареєструватись" 
                                ValidationGroup="RegisterUserValidationGroup"></dx:ASPxButton>
                        </p>
                    </div>
                </ContentTemplate>
                <CustomNavigationTemplate>
                </CustomNavigationTemplate>
            </asp:CreateUserWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>

</asp:Content>