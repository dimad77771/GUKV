<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PasswordExpirationNotice.ascx.cs" Inherits="UserControls_PasswordExpirationNotice" %>

<asp:Panel ID="NoticePanel" runat="server" Visible="false" role="alert">
    <asp:Literal ID="NoticeText" runat="server" />
    <span class="password-expiration-notice__links">
        <asp:HyperLink ID="ChangePasswordLink" runat="server" />
        <span aria-hidden="true">|</span>
        <asp:HyperLink ID="VideoLink" runat="server" Target="_blank" />
    </span>
</asp:Panel>
