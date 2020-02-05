<%@ page language="C#" autoeventwireup="true" inherits="Reports1NF_Report1NFList, App_Web_report1nflist.aspx.5d94abc0" masterpagefile="~/NoHeader.master" title="Перелік звітів 1НФ" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 180);
    }

    function GridViewReports1NFInit(s, e) {

        PrimaryGridView.PerformCallback("init:");
    }

    function GridViewReports1NFEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowReportCard(reportId) {

        var cardUrl = "../Reports1NF/Cabinet.aspx?rid=" + reportId;

        window.open(cardUrl);
    }

    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceReports" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT rep.*, ar.NumOfSubmAgr, ar.NumOfAgr, obj.NumOfSubmObj, obj.NumOfObj,
        (SELECT MAX(sdt) FROM (VALUES
        (rep.bal_max_submit_date),
        (rep.bal_del_max_submit_date),
        (rep.arenda_max_submit_date),
        (rep.arenda_rented_max_submit_date),
        (rep.org_max_submit_date)) AS AllMaxSubmitDates(sdt)) AS 'max_submit_date',
        CASE WHEN rep.is_reviewed = 0 THEN N'НI' ELSE N'ТАК' END AS 'review_performed'
        FROM view_reports1nf rep
        LEFT JOIN 
        (SELECT sum(CASE WHEN (r1a.submit_date IS NULL OR r1a.modify_date IS NULL OR r1a.modify_date > r1a.submit_date) THEN 0 ELSE 1 END) as NumOfSubmAgr, 
        Count(r1a.ID) AS NumOfAgr,
        report_id
        FROM reports1nf_arenda r1a LEFT JOIN arenda a
        ON r1a.id = a.id WHERE a.is_deleted IS NULL OR a.is_deleted = 0
        GROUP BY report_id) ar        
        on rep.report_id = ar.report_id
        LEFT JOIN
        (SELECT sum(CASE WHEN (b.submit_date IS NULL OR b.modify_date IS NULL OR b.modify_date > b.submit_date) THEN 0 ELSE 1 END) AS NumOfSubmObj,
        Count(ID) AS NumOfObj,
        report_id
        FROM reports1nf_balans b
        WHERE is_deleted IS NULL OR is_deleted = 0
        GROUP BY report_id) obj
        on rep.report_id = obj.report_id
        WHERE (@p_rda_district_id = 0 OR (rep.org_sfera_upr_id = 4 AND rep.org_district_id = @p_rda_district_id))"
    OnSelecting="SqlDataSourceReports_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFAccounts.aspx" Text="Облікові Записи"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFNotifications.aspx" Text="Налаштування Повідомлень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>        
    </Items>
</dx:ASPxMenu>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Звіти Балансоутримувачів" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="function (s,e) { PopupFieldChooser.Show(); }" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_Reports1NF_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Reports1NF_SaveAs" 
                PopupElementID="ASPxButton_Reports1NF_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton_Reports1NF_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Reports1NF_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Reports1NF_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Reports1NF_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Reports1NF_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Reports1NF_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Reports1NF_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewReports1NFExporter" runat="server" 
    FileName="ПерелікЗвітів" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView
    ID="PrimaryGridView"
    ClientInstanceName="PrimaryGridView"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceReports"
    KeyFieldName="report_id"
    OnCustomCallback="GridViewReports1NF_CustomCallback"
    OnCustomFilterExpressionDisplayText="GridViewReports1NF_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter="GridViewReports1NF_ProcessColumnAutoFilter" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="report_id" ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="0" Caption="Картка Звіту">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowReportCard(" + Eval("report_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False" AllowHeaderFilter="False" AllowAutoFilter="False" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Caption="Назва Організації" Width="300px">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="short_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Коротка Назва Організації" Width="300px" Visible="false">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Код ЄДРПОУ">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("zkpo_code") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cur_state" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Стан Звіту" />
        <dx:GridViewDataDateColumn FieldName="max_submit_date" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Дата Останнього Надсилання">
            <Settings AllowHeaderFilter="False" />
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="review_performed" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="6" Caption="Звіт Перевірено" />    

        <dx:GridViewDataTextColumn FieldName="addr_district" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="True" Caption="Район" Width="150px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="True" Caption="Назва Вулиці" Width="200px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="True" Caption="Номер Будинку"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_korpus" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="False" Caption="Номер Будинку - Корпус"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_zip_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Поштовий Індекс"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="False" Caption="Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="13" Visible="False" Caption="Вид Діяльності" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="False" Caption="Галузь (Баланс)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="15" Visible="False" Caption="Вид Діяльності (Баланс)" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_gosp" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="16" Visible="False" Caption="Форма фінансування"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_of_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="17" Visible="False" Caption="Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="gosp_struct" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="False" Caption="Госп. Структура"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="False" Caption="Орган управління"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_form" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="False" Caption="Орг.-правова форма госп."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_organ" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="Орган госп. упр."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="status" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="False" Caption="Фіз. / Юр. Особа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_fio" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="False" Caption="ФІО Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_phone" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="False" Caption="Тел. Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_fio" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="25" Visible="False" Caption="ФІО Бухгалтера"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_phone" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="26" Visible="False" Caption="Тел. Бухгалтера"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="fax" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="27" Visible="False" Caption="Факс"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_auth" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="28" Visible="False" Caption="Реєстраційний Орган"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="29" Visible="False" Caption="Номер Запису про Реєстрацію"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="30" Visible="False" Caption="Дата Реєстрації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_svidot" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="31" Visible="False" Caption="Номер Свідоцтва про Реєстрацію"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="kved_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="32" Visible="False" Caption="КВЕД"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="koatuu" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="33" Visible="False" Caption="ОАТУУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="34" Visible="False" Caption="Відділ ДКВ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="mayno" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="35" Visible="False" Caption="Правовий режим майна"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_liquidated" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="36" Visible="False" Caption="Ліквідовано"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="liquidation_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="37" Visible="False" Caption="Дата Ліквідації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_email" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="38" Visible="False" Caption="Ел. Адреса"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_posada" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="39" Visible="False" Caption="Контактна Особа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sfera_upr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="40" Visible="False" Caption="Сфера Управління"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="NumOfObj" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="41" Caption="Кількість об'єктів на балансі" />
        <dx:GridViewDataTextColumn FieldName="NumOfSubmObj" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="42" Caption="Кількість об'єктів, інформація за якими буда надіслана" />
        <dx:GridViewDataTextColumn FieldName="NumOfAgr" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="43" Caption="Кількість договорів" />
        <dx:GridViewDataTextColumn FieldName="NumOfSubmAgr" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="44" Caption="Кількість договорів, інформація за якими буда надіслана" />
    </Columns>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager AlwaysShowPager="true" PageSize="25"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="True"
        ShowFilterBar="Visible"
        ShowHeaderFilterButton="True"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        VerticalScrollBarMode="Visible"
        VerticalScrollBarStyle="Virtual" />
    <SettingsCookies CookiesID="GUKV.Reports1NF.ReportList" Version="A2_3" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewReports1NFInit" EndCallback="GridViewReports1NFEndCallback" />
</dx:ASPxGridView>

<dx:ASPxPopupControl ID="PopupFieldChooser" runat="server" 
    HeaderText="Додаткові Колонки" 
    ClientInstanceName="PopupFieldChooser" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>
