<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExternalDocument.aspx.cs" Inherits="Documents_ExternalDocument" MasterPageFile="~/NoHeader.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
        Text="Запрошуваного документа не існує" Font-Bold="True" Font-Size="Large" 
        ForeColor="Red">
    </dx:ASPxLabel>
</asp:Content>

