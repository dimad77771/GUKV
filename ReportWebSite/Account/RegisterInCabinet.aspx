<%@ Page Title="Реєстрація нового користувача" Language="C#" MasterPageFile="~/NoHeader.master" AutoEventWireup="true"
    CodeFile="RegisterInCabinet.aspx.cs" Inherits="Account_Register" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:CreateUserWizard ID="RegisterUser" runat="server" EnableViewState="false" 
        OnCreatedUser="RegisterUser_CreatedUser" 
        CompleteSuccessText="Користувач успішно зареєстрований." 
        ContinueButtonText="Продовжити" 
        RequireEmail="false"
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
                        Для реєстрації в Кабінеті, будь ласка, заповніть цю форму.
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
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Електронна адреса:</asp:Label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести електронну адресу користувача."
                                     ToolTip="Для реєстрації необхідно ввести електронну адресу користувача."
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="NameFLabel" runat="server" AssociatedControlID="NameF">Прізвище користувача:</asp:Label>
                                <asp:TextBox ID="NameF" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="NameFRequired" runat="server" ControlToValidate="NameF" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести прізвище користувача."
                                     ToolTip="Для реєстрації необхідно ввести прізвище користувача." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
			                <p>
                                <asp:Label ID="NameILabel" runat="server" AssociatedControlID="NameI">Ім'я користувача:</asp:Label>
                                <asp:TextBox ID="NameI" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="NameIRequired" runat="server" ControlToValidate="NameI" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести ім'я користувача."
                                     ToolTip="Для реєстрації необхідно ввести ім'я користувача." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>                            
			                <p>
                                <asp:Label ID="NameOLabel" runat="server" AssociatedControlID="NameO">По батькові користувача:</asp:Label>
                                <asp:TextBox ID="NameO" runat="server" CssClass="textEntry"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="NameORequired" runat="server" ControlToValidate="NameO" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести ім'я користувача."
                                     ToolTip="Для реєстрації необхідно ввести ім'я користувача." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>--%>
                            </p>
                            <p>
                                <asp:Label ID="PhoneLabel" runat="server" AssociatedControlID="Phone">Телефон:</asp:Label>
                                <asp:TextBox ID="Phone" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PhoneRequired" runat="server" ControlToValidate="Phone" 
                                     CssClass="failureNotification"
                                     ErrorMessage="Для реєстрації необхідно ввести телефон користувача."
                                     ToolTip="Для реєстрації необхідно ввести телефон користувача." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Пароль:</asp:Label>
                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password" AutoCompleteType="None"></asp:TextBox>
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
    
    <dx:ASPxHyperLink ID="LinkManageRoles" runat="server" Text="Керування правами доступу" NavigateUrl="ManageRoles.aspx" Visible="false" />

</asp:Content>