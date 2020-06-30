<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintOrgCard.aspx.cs" Inherits="Cards_PrintOrgCard" %>

<%@ Register assembly="DevExpress.XtraReports.v13.1.Web, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register Src="~/UserControls/ThemeSelector.ascx" TagPrefix="dx" TagName="ThemeSelector" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title>Єдина Інформаційна Система ДКВ м. Києва</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />

    <script language="javascript">

        function UpdateScaleFactor(sender) {

            document.getElementById('Scale').value = sender.GetText();
            viewer.Refresh();
        }

    </script>
</head>
<body>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/DXTheme.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/CommonScript.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-1.8.3.js") %>"></script>

    <form id="Form1" runat="server">
        <div class="page">
            <div class="narrowheader">
                <div style="float: right; padding-top: 2px;">
                    <dx:ThemeSelector ID="ThemeSelector" runat="server" />
                </div>
            <div style="float:right;color:white">
                    <strong>Технічна підтримка <a style="color:white" href="mailto:seic@gukv.gov.ua" title="Написати лист">seic@gukv.gov.ua</a> </strong>
                    0931605647 – процедура прийому звітів; <br />0672605106 – реєстрація, об’єкти, договори, вільні; 0979065119 - плата за використання
            </div>
            </div>
            <div class="main">
                <dx:ReportToolbar ID="ReportToolbar1" runat="server" 
                    ReportViewerID="ReportViewer1" ShowDefaultButtons="False">
                    <Items>
                        <dx:ReportToolbarButton ItemKind="Search" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton ItemKind="PrintReport" />
                        <dx:ReportToolbarButton ItemKind="PrintPage" />

                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarLabel Text="Масштаб:" />
                        <dx:ReportToolbarComboBox Name="ScaleFactor" Width="50px">
                            <Elements>
                                <dx:ListElement Text="100" Value="100" />
                                <dx:ListElement Text="90" Value="90" />
                                <dx:ListElement Text="80" Value="80" />
                                <dx:ListElement Text="70" Value="70" />
                                <dx:ListElement Text="60" Value="60" />
                                <dx:ListElement Text="50" Value="50" />
                            </Elements>
                        </dx:ReportToolbarComboBox>
                        <dx:ReportToolbarLabel Text="%" />

                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                        <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                        <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                            <Elements>
                                <dx:ListElement Value="pdf" />
                                <dx:ListElement Value="xls" />
                                <dx:ListElement Value="xlsx" />
                                <dx:ListElement Value="rtf" />
                                <dx:ListElement Value="mht" />
                                <dx:ListElement Value="html" />
                                <dx:ListElement Value="txt" />
                                <dx:ListElement Value="csv" />
                                <dx:ListElement Value="png" />
                            </Elements>
                        </dx:ReportToolbarComboBox>
                    </Items>
                    <Styles>
                        <LabelStyle>
                        <Margins MarginLeft="3px" MarginRight="3px" />
                        </LabelStyle>
                    </Styles>
                    <ClientSideEvents ItemValueChanged="function(s, e) { UpdateScaleFactor(s) }" />
                </dx:ReportToolbar>
                <dx:ReportViewer ID="ReportViewer1" ClientInstanceName="viewer" runat="server"
                    AutoSize="True" PageByPage="False" Width="100%" ReportName="ReportObjectCard" >
                </dx:ReportViewer>
            </div>
            <input id="Scale" runat="server" type="hidden" />
            <div class="clear"></div>
        </div>
    </form>

    <%= StackExchange.Profiling.MiniProfiler.RenderIncludes() %> 
</body>
</html>