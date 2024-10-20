﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendingBalanceObjectsDiagnostic.aspx.cs" Inherits="Reports1NF_SendingBalanceObjectsDiagnostic" MasterPageFile="~/NoHeader.master" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="padding-bottom:20px;">Журнал відправлення об'єктів на балансі:</div>

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
    <asp:Label ID="lblSuccess" runat="server" Visible="false">Всі об'єкти на балансі були успішно відправлені.</asp:Label>
</asp:Content>