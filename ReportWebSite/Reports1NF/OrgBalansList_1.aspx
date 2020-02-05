<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgBalansList.aspx.cs" Inherits="Reports1NF_OrgBalansList"
    MasterPageFile="~/NoMenu.master" Title="Об'єкти на Балансі" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<style type="text/css">
    .SpacingPara
    {
        font-size: 10px;
        margin-top: 4px;
        margin-bottom: 0px;
        padding-top: 0px;
        padding-bottom: 0px;
    }
</style>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSize(); };

    function AdjustGridSize() {

        PrimaryGridView.SetHeight(window.innerHeight - 180);
    }

    function PrimaryGridViewEndCallback(s, e) {

        AdjustGridSize();

        CPStatus.PerformCallback('');
    }

    function ShowReportBalansObjCard(reportId, balansId) {

        var cardUrl = "../Reports1NF/OrgBalansObject.aspx?rid=" + reportId + "&bid=" + balansId;

        window.location = cardUrl;
    }

    function ShowSendingLog() {
        var diagnosticWindow = window.open("SendingBalanceObjectsDiagnostic.aspx?rid=<%= ReportID  %>", "", "width=600,height=400");
        diagnosticWindow.focus();
    }
    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT CASE WHEN COUNT(*) > 0 THEN 0 ELSE 1 END AS 'report_balans_status'
        FROM reports1nf_balans bal INNER JOIN reports1nf rep ON rep.id = bal.report_id
        WHERE bal.report_id = @rep_id AND ((bal.submit_date IS NULL OR bal.modify_date > bal.submit_date))">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceReportBalans" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
        bal.id AS 'balans_id', 
        dict_districts2.name AS 'district', 
        bld.addr_street_name,
        (COALESCE(LTRIM(RTRIM(bld.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(bld.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(bld.addr_nomer3)), '')) AS 'addr_nomer',
        bal.sqr_total,
        COALESCE(bal.purpose_str, dict_balans_purpose.name) AS 'purpose', bal.is_deleted, bal.modify_date, bal.submit_date, dict_own.name AS ownership_type,
        CASE WHEN (((bal.is_valid = 0) OR (bal.is_valid IS NULL)) OR ((bal.submit_date IS NULL OR bal.modify_date > bal.submit_date))) THEN N'НІ' ELSE N'ТАК' END AS 'is_submitted',
        bal.modified_by,
        sum(fs.total_free_sqr) as 'total_free_sqr'
        FROM reports1nf_balans bal
        INNER JOIN reports1nf rep ON rep.id = bal.report_id
        LEFT OUTER JOIN reports1nf_buildings bld ON bld.unique_id = bal.building_1nf_unique_id
        LEFT OUTER JOIN dict_balans_purpose ON dict_balans_purpose.id = bal.purpose_id
        LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = bld.addr_distr_new_id
        LEFT OUTER JOIN dict_org_ownership dict_own ON bal.form_ownership_id = dict_own.id
        left join reports1nf_balans_free_square fs on fs.balans_id = bal.id and fs.report_id = bal.report_id
        WHERE bal.report_id = @rep_id
        group by bal.id , 
        dict_districts2.name, 
        bld.addr_street_name,
        (COALESCE(LTRIM(RTRIM(bld.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(bld.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(bld.addr_nomer3)), '')),
        bal.sqr_total,
        COALESCE(bal.purpose_str, dict_balans_purpose.name), bal.is_deleted, bal.modify_date, bal.submit_date, dict_own.name,
        CASE WHEN (((bal.is_valid = 0) OR (bal.is_valid IS NULL)) OR ((bal.submit_date IS NULL OR bal.modify_date > bal.submit_date))) THEN N'НІ' ELSE N'ТАК' END,
        bal.modified_by" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Cabinet.aspx" Text="Стан"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgInfo.aspx" Text="Загальна Інформація"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansList.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansDeletedList.aspx" Text="Відчужені Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgArendaList.aspx" Text="Договори використання приміщень "></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgRentedList.aspx" Text="Договори Орендування"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingRequestsList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/Logout.aspx" Text="Вийти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/ChangePasswordNoMenu.aspx" Text="Пароль"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Help.aspx" Text="?"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<p class="SpacingPara"/>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Перелік об'єктів, що знаходяться на балансі організації" CssClass="pagetitle"/>
</p>

<table border="0" cellspacing="0" cellpadding="2">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonSendAll" runat="server" Text="Надіслати всі зміни в ДКВ" AutoPostBack="false"></dx:ASPxButton>

            <dx:ASPxTimer ID="ProgressTimer" runat="server" ClientInstanceName="ProgressTimer" Enabled="False" Interval="3000">
                <ClientSideEvents Tick="function(s, e) { CPProgress.PerformCallback('timer:'); }" />
            </dx:ASPxTimer>

            <dx:ASPxPopupControl ID="PopupSendAll" runat="server" ClientInstanceName="PopupSendAll"
                HeaderText="Надсилання до ДКВ усіх змінених об'єктів на балансі" PopupElementID="ButtonSendAll">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">

                        <dx:ASPxCallbackPanel ID="CPProgress" ClientInstanceName="CPProgress" runat="server" OnCallback="CPProgress_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent1" runat="server">

                                    <dx:ASPxLabel ID="LabelProgress" ClientInstanceName="LabelProgress" runat="server" Width="600px" />

                                    <dx:ASPxProgressBar ID="ProgressSendAll" ClientInstanceName="ProgressSendAll" runat="server" Width="600px" Height="24px" Minimum="0" Maximum="100" />

                                </dx:panelcontent>
                            </PanelCollection>

                            <ClientSideEvents EndCallback="function (s,e) {
                                        var id = '' + CPProgress.cpWorkItemId;
                                        if (id.length > 0)
                                        {
                                            ProgressTimer.SetEnabled(true);
                                        }
                                        else
                                        {
                                            ProgressTimer.SetEnabled(false);
                                            alert('Обробку завершено.');
                                            PopupSendAll.Hide();
                                            PrimaryGridView.PerformCallback('init:');
                                        }
                                    }" />
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents PopUp="function (s,e) { CPProgress.PerformCallback('init:'); ShowSendingLog();  }" />
            </dx:ASPxPopupControl>

        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_BalansObjects_SaveAs" 
                PopupElementID="ASPxButton_BalansObjects_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton2" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_BalansObjects_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_BalansObjects_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxButton ID="ASPxButton_BalansObjects_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>        
        <td>
            <dx:ASPxCallbackPanel ID="CPStatus" ClientInstanceName="CPStatus" runat="server" OnCallback="CPStatus_Callback">
                <PanelCollection>
                    <dx:panelcontent ID="Panelcontent6" runat="server">

                        <asp:FormView runat="server" BorderStyle="None" ID="BalansStatusForm" DataSourceID="SqlDataSourceBalansStatus" EnableViewState="False">
                            <ItemTemplate>

                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Необхідно надіслати внесені зміни до ДКВ" ClientVisible='<%# 0.Equals(Eval("report_balans_status")) %>' ForeColor="Red" />
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Усі зміни надіслано до ДКВ" ClientVisible='<%# 1.Equals(Eval("report_balans_status")) %>' />

                            </ItemTemplate>
                        </asp:FormView>

                    </dx:panelcontent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </td>
    </tr>
</table>


<dx:ASPxGridViewExporter ID="ASPxGridViewExporterBalansObjects" runat="server" 
    FileName="Balance" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView ID="PrimaryGridView" ClientInstanceName="PrimaryGridView" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceReportBalans" KeyFieldName="balans_id" Width="100%"
    OnCustomCallback="PrimaryGridView_CustomCallback"
    OnHtmlDataCellPrepared="PrimaryGridView_HtmlDataCellPrepared"
    OnCustomColumnSort="PrimaryGridView_CustomColumnSort" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="balans_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="True" Caption="Картка Об'єкту" Width="50px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowReportBalansObjCard(" + Request.QueryString["rid"] + "," + Eval("balans_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False" AllowAutoFilter="False" AllowHeaderFilter="False" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="1" Caption="Район" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" VisibleIndex="2" Caption="Вулиця" Width="150px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="3" Caption="Номер"><Settings SortMode="Custom" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total" VisibleIndex="4" Caption="Площа На Балансі"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="total_free_sqr" VisibleIndex="5" Caption="Площа вільних приміщень"></dx:GridViewDataTextColumn>
        

        <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="6" Caption="Призначення" Width="200px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="ownership_type" VisibleIndex="7" Caption="Форма Власності Об‘єкту"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="8" Caption="Коли Змінено"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="submit_date" VisibleIndex="9" Caption="Коли Надіслано в ДКВ"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="is_submitted" VisibleIndex="10" Caption="Надіслано в ДКВ"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="modified_by" VisibleIndex="11" Caption="Користувач"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="balans_id" SummaryType="Count" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="total_free_sqr" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowFilterBar="Visible"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="20" />
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.Reports1NF.BalansList" Enabled="True" Version="A2" />

    <ClientSideEvents
        Init="function (s,e) { PrimaryGridView.PerformCallback('init:'); }"
        EndCallback="PrimaryGridViewEndCallback" />
</dx:ASPxGridView>

</asp:Content>
