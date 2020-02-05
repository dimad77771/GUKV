<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinForm.aspx.cs" Inherits="Finance_FinForm" MasterPageFile="~/NoHeader.master" Title="Форма" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register assembly="DevExpress.XtraReports.v13.1.Web, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelFormTitle" runat="server" Text="Фінансова звітність підприємств" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Form_SaveAs" 
                PopupElementID="ASPxButton_Form_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton2" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Form_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Form_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Form_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Form_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterFinForm" runat="server" 
    FileName="ФiнансовийЗвiт" GridViewID="GridViewFinForm" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif"></Default>
        <AlternatingRowCell BackColor="#E0E0E0"></AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView ID="GridViewFinForm" runat="server" 
    ClientInstanceName="GridViewFinForm"
    AutoGenerateColumns="True"
    Width="100%"
    EnableRowsCache="False"
    OnDataBound="GridViewFinForm_DataBound" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <SettingsBehavior EnableCustomizationWindow="False" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager Mode="ShowAllRecords"> </SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="False"
        ShowFilterRowMenu="False"
        ShowGroupPanel="False" 
        ShowFilterBar="Hidden"
        ShowHeaderFilterButton="False" 
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.Finance.Form" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

</asp:Content>
