<%@ page language="C#" autoeventwireup="true" inherits="Cards_ReportViewer, App_Web_reportviewer.aspx.f411966a" masterpagefile="~/NoMenu.master" title="Звіт" %>

<%@ Register assembly="DevExpress.XtraReports.v13.1.Web, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <table>
        <tr>
            <td>
                <dx:ReportToolbar ID="MainReportToolbar" runat="server" 
                    ReportViewerID="MainReportViewer" ShowDefaultButtons="False">
                    <Items>
                        <dx:ReportToolbarButton ItemKind="Search" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton ItemKind="PrintReport" />
                        <dx:ReportToolbarButton ItemKind="PrintPage" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                        <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                        <dx:ReportToolbarLabel ItemKind="PageLabel" />
                        <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px"></dx:ReportToolbarComboBox>
                        <dx:ReportToolbarLabel ItemKind="OfLabel" />
                        <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                        <dx:ReportToolbarButton ItemKind="NextPage" />
                        <dx:ReportToolbarButton ItemKind="LastPage" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                        <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                        <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                            <elements>
                                <dx:ListElement Value="pdf" />
                                <dx:ListElement Value="xls" />
                                <dx:ListElement Value="xlsx" />
                                <dx:ListElement Value="rtf" />
                                <dx:ListElement Value="mht" />
                                <dx:ListElement Value="html" />
                                <dx:ListElement Value="txt" />
                                <dx:ListElement Value="csv" />
                                <dx:ListElement Value="png" />
                            </elements>
                        </dx:ReportToolbarComboBox>
                    </Items>
                    <styles>
                        <LabelStyle><Margins MarginLeft='3px' MarginRight='3px' /></LabelStyle>
                    </styles>
                </dx:ReportToolbar>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ReportViewer ID="MainReportViewer" runat="server"></dx:ReportViewer>
            </td>
        </tr>
    </table>

</asp:Content>
