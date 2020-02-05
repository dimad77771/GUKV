<%@ Page Title="Звіти Системи" Language="C#" MasterPageFile="~/NoHeader.master" AutoEventWireup="true" CodeFile="SysReports.aspx.cs" Inherits="SysReports_SysReports" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceCorrectedErrors" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM sys_report_corrections WHERE is_corrected = 1">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceMismatches" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM sys_report_corrections WHERE is_corrected <> 1">
</mini:ProfiledSqlDataSource>

<center>

<dx:ASPxPageControl ID="MainPageControl" ClientInstanceName="MainPageControl" runat="server" Width="98%">
    <TabPages>
        <dx:TabPage Text="Перелік Зроблених Виправлень" Name="SysReportCorrected" TabStyle-Width="98%">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server">

<%-- Content of the first tab BEGIN --%>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Перелік корегувань в БД 1НФ, що були виконані автоматично" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl1" 
                PopupElementID="ASPxButton_Corrected_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxButton ID="ASPxButton3" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Corrected_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Corrected_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Corrected_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Corrected_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewCorrectedErrorsExporter" runat="server" 
    FileName="ПерелікВиправлень" GridViewID="GridViewCorrectedErrors" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView
    ID="GridViewCorrectedErrors"
    ClientInstanceName="GridViewCorrectedErrors"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceCorrectedErrors"
    KeyFieldName="id"
    OnCustomCallback="GridViewCorrectedErrors_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewCorrectedErrors_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewCorrectedErrors_ProcessColumnAutoFilter" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="obj_descr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="0" Visible="True" Caption="Опис Об'єкту" Width="250px"> <Settings AllowHeaderFilter="False" /> </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="action_descr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="True" Caption="Внесені Зміни" Width="250px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="reason_descr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="True" Caption="Підстави для Змін" Width="250px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="modify_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="True" Caption="Дата Внесення Змін" Width="80px"></dx:GridViewDataDateColumn>
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="obj_descr" SummaryType="Count" DisplayFormat="{0} виправлень" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="False" AutoFilterRowInputDelay="2000" ColumnResizeMode="Control" />
    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="False"
        ShowFilterBar="Visible"
        ShowHeaderFilterButton="True"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.SysReports.Corrected" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<%-- Content of the first tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Перелік Знайдених Невідповідностей" Name="SysReportMismatches" TabStyle-Width="98%">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server">

<%-- Content of the second tab BEGIN --%>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle2" runat="server" Text="Перелік об'єктів, що, можливо, потребують корегування" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl2" 
                PopupElementID="ASPxButton_Mismatches_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton1" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Mismatches_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton2" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Mismatches_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton6" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Mismatches_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Mismatches_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewMismatchesExporter" runat="server" 
    FileName="ПерелікНевідповідностей" GridViewID="GridViewMismatches" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView
    ID="GridViewMismatches"
    ClientInstanceName="GridViewMismatches"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceMismatches"
    KeyFieldName="id"
    OnCustomCallback="GridViewMismatches_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewMismatches_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewMismatches_ProcessColumnAutoFilter" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="obj_descr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="0" Visible="True" Caption="Опис Об'єкту" Width="325px"> <Settings AllowHeaderFilter="False" /> </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="reason_descr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="True" Caption="Підстави для Перевірки" Width="325px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="modify_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="True" Caption="Дата Перевірки" Width="80px"></dx:GridViewDataDateColumn>
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="obj_descr" SummaryType="Count" DisplayFormat="{0} невідповідностей" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="False" AutoFilterRowInputDelay="2000" ColumnResizeMode="Control" />
    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="False"
        ShowFilterBar="Visible"
        ShowHeaderFilterButton="True"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.SysReports.Mismatches" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<%-- Content of the second tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
    </TabPages>
</dx:ASPxPageControl>

</center>

</asp:Content>
