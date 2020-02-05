<%@ page language="C#" autoeventwireup="true" inherits="Reports1NF_SendingDeletedObjectsDiagnostic, App_Web_sendingdeletedobjectsdiagnostic.aspx.5d94abc0" masterpagefile="~/NoMenu.master" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="padding-bottom:20px;">Журнал відправлення відчужених об'єктів:</div>

    <dx:ASPxGridView ID="dxGridView" runat="server">
        <SettingsBehavior AllowSort="false" AllowGroup="false" />
        <Columns>
            <dx:GridViewDataTextColumn FieldName="Message" VisibleIndex="1" Caption="Повідомлення"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Errors" VisibleIndex="2" Caption="Помилки">
                <PropertiesTextEdit EncodeHtml="false"></PropertiesTextEdit>
            </dx:GridViewDataTextColumn>            
            <dx:GridViewDataTextColumn FieldName="Link" VisibleIndex="3" Caption="Посилання">
                <PropertiesTextEdit EncodeHtml="false"></PropertiesTextEdit>
            </dx:GridViewDataTextColumn>            
        </Columns>        
    </dx:ASPxGridView>
    <asp:Label ID="lblSuccess" runat="server" Visible="false">Всі відчужені об'єкти були успішно відправлені.</asp:Label>
</asp:Content>