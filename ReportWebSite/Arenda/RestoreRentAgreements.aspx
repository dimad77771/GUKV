<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RestoreRentAgreements.aspx.cs" Inherits="Arenda_RestoreRentAgreements" MasterPageFile="~/NoHeader.master" Title="Повернути з видалених" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .restore-agreements { padding: 8px; }
        .restore-search { margin: 16px 0; }
        .restore-search td { padding: 0 16px 8px 0; vertical-align: bottom; }
        .restore-search label { display: block; margin-bottom: 5px; }
        .restore-message { display: block; margin: 12px 0; }
        .restore-results-wrap { overflow-x: auto; }
        .restore-results { width: 100%; border-collapse: collapse; background: white; }
        .restore-results th, .restore-results td { border: 1px solid #cfcfd9; padding: 8px; text-align: left; }
        .restore-results th { background: #f3f3f6; font-weight: normal; }
        .restore-results td:last-child { min-width: 190px; }
        .restore-confirmation-actions { margin-top: 20px; }
        .restore-confirmation-actions input { margin-right: 12px; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="restore-agreements">
        <asp:Label ID="LabelTitle" runat="server" Text="Повернути з видалених" CssClass="reporttitle" />

        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="ButtonSearch">
            <table class="restore-search">
                <tr>
                    <td>
                        <asp:Label ID="LabelHolderCode" runat="server" AssociatedControlID="TextHolderCode" Text="Код ЄДРПОУ балансоутримувача" />
                        <asp:TextBox ID="TextHolderCode" runat="server" MaxLength="16" Width="220px" />
                    </td>
                    <td>
                        <asp:Label ID="LabelAgreementNumber" runat="server" AssociatedControlID="TextAgreementNumber" Text="Номер договору оренди" />
                        <asp:TextBox ID="TextAgreementNumber" runat="server" MaxLength="18" Width="220px" />
                    </td>
                    <td>
                        <asp:Button ID="ButtonSearch" runat="server" Text="Пошук" OnClick="ButtonSearch_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <asp:Label ID="LabelMessage" runat="server" CssClass="restore-message" Visible="false" role="status" />

        <div class="restore-results-wrap">
            <asp:GridView ID="GridAgreements" runat="server" AutoGenerateColumns="false" DataKeyNames="id"
                CssClass="restore-results" GridLines="None" Visible="false" UseAccessibleHeader="true"
                EmptyDataText="За вказаними умовами договорів не знайдено." OnRowCommand="GridAgreements_RowCommand">
                <Columns>
                    <asp:BoundField DataField="org_balans_full_name" HeaderText="Балансоутримувач - Повна Назва" />
                    <asp:BoundField DataField="org_balans_zkpo" HeaderText="Балансоутримувач - Код ЄДРПОУ" />
                    <asp:BoundField DataField="org_renter_short_name" HeaderText="Орендар - Коротка Назва" />
                    <asp:BoundField DataField="org_giver_short_name" HeaderText="Орендодавець - Коротка Назва" />
                    <asp:BoundField DataField="agreement_date" HeaderText="Дата укладання договору" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="agreement_num" HeaderText="Номер Договору Оренди" />
                    <asp:TemplateField HeaderText="Відновлення">
                        <ItemTemplate>
                            <asp:Button ID="ButtonRestore" runat="server" Text="Відновити" CommandName="RestoreAgreement"
                                CommandArgument='<%# Eval("id") %>' Visible='<%# Convert.ToBoolean(Eval("can_restore")) %>' />
                            <asp:Literal ID="LiteralStatus" runat="server" Mode="Encode" Text='<%# Eval("status_message") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <dx:ASPxPopupControl ID="PopupRestore" runat="server" HeaderText="Підтвердження відновлення"
        Modal="true" PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Width="480px" CloseAction="CloseButton" ShowCloseButton="true">
        <ContentCollection>
            <dx:PopupControlContentControl ID="RestoreConfirmationContent" runat="server">
                <p>Відновити договір оренди?</p>
                <p>Номер договору: <strong><asp:Literal ID="LiteralAgreementNumber" runat="server" Mode="Encode" /></strong></p>
                <p>Дата укладання: <strong><asp:Literal ID="LiteralAgreementDate" runat="server" Mode="Encode" /></strong></p>
                <div class="restore-confirmation-actions">
                    <asp:Button ID="ButtonConfirmRestore" runat="server" Text="Відновити" OnClick="ButtonConfirmRestore_Click" />
                    <asp:Button ID="ButtonCancelRestore" runat="server" Text="Скасувати" OnClick="ButtonCancelRestore_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
