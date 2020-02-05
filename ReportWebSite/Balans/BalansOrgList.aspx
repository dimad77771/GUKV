<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BalansOrgList.aspx.cs" Inherits="Balans_BalansOrgList" MasterPageFile="~/NoHeader.master" Title="Перелік Балансоутримувачів" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/SaveReportCtrl.ascx" tagname="SaveReportCtrl" tagprefix="uc1" %>
<%@ Register src="../UserControls/AddressPicker.ascx" tagname="AddressPicker" tagprefix="uc2" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 185);
    }

    function GridViewBalansInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewBalansEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowFoldersPopupControl(s, e) {

        PopupControlFolders.Show();
    }

    function CheckBoxBalansHoldersComVlasn_CheckedChanged(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function CheckBoxRentedObjectsDPZ_CheckedChanged(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function ShowAddressPickerPopupControl(s, e) {

        PopupAddressPicker.Show();
    }

    function ShowFieldChooserPopupControl(s, e) {

        PopupFieldChooser.Show();
    }

    // ]]>

</script>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Balans/BalansObjects.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
<%--         <dx:MenuItem NavigateUrl="../Balans/BalansArenda.aspx" Text="Договори Оренди по Об'єктах"></dx:MenuItem>    --%>
        <dx:MenuItem NavigateUrl="../Balans/BalansOrgList.aspx" Text="Перелік Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Balans/BalansOther.aspx" Text="Інші Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Balans/BalansInventarizations.aspx" Text="Об'єкти Інвентарізації"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="http://kmda.iisd.com.ua" Text="Класифікатор майна"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle2" runat="server" Text="Загальна інформація по балансоутримувачам" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxCheckBox ID="CheckBoxRentedObjectsDPZ" runat="server" Checked='True' Text="Дані ДПЗ"
                Width="100px" ClientInstanceName="CheckBoxRentedObjectsDPZ" >
                <ClientSideEvents CheckedChanged="CheckBoxRentedObjectsDPZ_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>        
        <td>
            <dx:ASPxCheckBox ID="CheckBoxBalansHoldersComVlasn" runat="server" Checked='False' Text="Лише Ком. Власність"
                Width="155px" ClientInstanceName="CheckBoxBalansHoldersComVlasn" >
                <ClientSideEvents CheckedChanged="CheckBoxBalansHoldersComVlasn_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup2" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_Balans_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Balans_SaveAs" 
                PopupElementID="ASPxButton_Balans_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton_Balans_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Balans_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Balans_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Balans_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Balans_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Balans_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Balans_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterBalans" runat="server" 
    FileName="Балансоутримувачi" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansOrganizations" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT vo.*, tot.balans_obj_count, tot.sqr_balans_total, tot.sqr_balans_non_habit,
                    rent.payment_zvit_uah, rent.received_zvit_uah, rent.debt_total_uah 
                   ,isnull(ddd.name, 'Невідомо') as sphera_dialnosti
                    FROM view_organizations vo
                    INNER JOIN view_org_balans_totals tot ON tot.organization_id = vo.organization_id
                    OUTER APPLY (SELECT TOP 1 payment_zvit_uah, received_zvit_uah, debt_total_uah FROM rent_payment rp
                        WHERE rp.org_balans_id = vo.organization_id ORDER BY rent_period_id DESC) rent
                    LEFT OUTER JOIN (select obp.org_id,occ.name from org_by_period obp
                                      join dict_rent_occupation occ on occ.id = obp.org_occupation_id
                                      where obp.period_id = (select top 1 id from dict_rent_period order by id desc)) DDD ON DDD.org_id = vo.organization_id

                    WHERE ((@p_dpz_filter = 0) OR (@p_dpz_filter <> 0 AND exists (select b.organization_id from dbo.reports1nf_balans b where b.organization_id = vo.organization_id and isnull(b.is_deleted,0) = 0) )) and
                        ((@p_com_filter = 0) OR (@p_com_filter <> 0 AND vo.form_of_ownership_int IN (32,33,34))) AND
                        ((@p_rda_district_id = 0) OR (vo.form_of_ownership_int in (select id from dict_org_ownership where is_rda = 1) AND vo.addr_distr_new_id = @p_rda_district_id))"
    OnSelecting="SqlDataSourceBalansOrganizations_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="1" Name="p_dpz_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_com_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxGridView ID="PrimaryGridView" runat="server" 
    ClientInstanceName="PrimaryGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceBalansOrganizations" 
    KeyFieldName="organization_id"
    Width="100%"
    OnCustomCallback="GridViewBalans_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewBalans_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewBalans_ProcessColumnAutoFilter"
    OnCustomColumnSort="GridViewBalans_CustomColumnSort" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="organization_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="False" Caption="ID Балансоутримувача">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("organization_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="True" Caption="Повна Назва" Width="220px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="False" Caption="Коротка Назва">
        <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("short_name") + "</a>"%>
            </DataItemTemplate></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="True" Caption="Код ЄДРПОУ" Width="70px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Caption="Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="True" Caption="Вид Діяльності" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="status" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="True" Caption="Фіз. / Юр. Особа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_district" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="True" Caption="Адреса - Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="True" Caption="Адреса - Вулиця" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="True" Caption="Адреса - Номер" Width="80px"><Settings SortMode="Custom" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_gosp" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="False" Caption="Форма фінансування"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_of_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_form" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="False" Caption="Орг.-правова форма госп."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="gosp_struct_type" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="13" Visible="False" Caption="Госп. Структура"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_fio" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="False" Caption="Директор"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_phone" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="15" Visible="False" Caption="Телефон Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="kved_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="16" Visible="False" Caption="КВЕД"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="koatuu" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="17" Visible="False" Caption="КОАТУУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="False" Caption="Стан юр. особи"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="balans_obj_count" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="False" Caption="Кількість об'єктів на балансі"></dx:GridViewDataTextColumn>
         <dx:GridViewDataTextColumn FieldName="sqr_balans_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="False" Caption="Сукупна площа на балансі"></dx:GridViewDataTextColumn>      
<%--        <dx:GridViewDataTextColumn FieldName="sqr_balans_non_habit" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="Сукупна нежитлова площа на балансі"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="payment_zvit_uah" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="False" Caption="Нараховано Орендної Плати За Останній Звітній Період (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="received_zvit_uah" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="False" Caption="Отримано Орендної Плати За Останній Звітній Період (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_total_uah" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="False" Caption="Заборгованість По Орендній Платі, всього (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sphera_dialnosti" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="25" Visible="True" Caption="Сфера діяльності"></dx:GridViewDataTextColumn>

    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="payment_zvit_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="received_zvit_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_total_uah" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True"
        AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
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
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.Balans" Version="A2" Enabled="True" />
    <SettingsDetail ShowDetailRow="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <Templates>
        <DetailRow>
            <dx:ASPxGridView ID="ASPxGridViewBalansDetail" runat="server" AutoGenerateColumns="False" 
                DataSourceID="SqlDataSourceBalansDetail" KeyFieldName="balans_id"
                onbeforeperformdataselect="ASPxGridViewBalansDetail_BeforePerformDataSelect" 
                Width="100%">

                <Columns>
                    <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="0" Caption="Район"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="1" Caption="Вулиця" Width="140px">
                        <DataItemTemplate>
                            <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("street_full_name") + "</a>"%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="2" Caption="Номер">
                        <DataItemTemplate>
                            <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("addr_nomer") + "</a>"%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="sqr_total" VisibleIndex="3" Caption="Площа на Балансі (кв.м.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="sqr_pidval" VisibleIndex="4" Caption="Підвальна Площа (кв.м.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="sqr_in_rent" VisibleIndex="5" Caption="Площа надана в Оренду (кв.м.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="sqr_privatizov" VisibleIndex="6" Caption="Приватизована Площа (кв.м.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_balans" VisibleIndex="7" Caption="Балансова Вартість (тис.грн.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_expert_1m" VisibleIndex="8" Caption="Оціночна Вартість За Кв.м. (грн.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_expert_total" VisibleIndex="9" Caption="Оціночна Вартість (грн.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_zalishkova" VisibleIndex="10" Caption="Залишкова Вартість (грн.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="o26_code" VisibleIndex="11" Caption="О26" Visible="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="num_floors" VisibleIndex="12" Caption="Кількість Поверхів"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="form_ownership" VisibleIndex="13" Caption="Форма Власності Об'єкту"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ownership_type" VisibleIndex="14" Caption="Право"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="object_type" VisibleIndex="15" Caption="Тип Об'єкту"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="object_kind" VisibleIndex="16" Caption="Вид Об'єкту"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="condition" VisibleIndex="17" Caption="Стан"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="purpose_group" VisibleIndex="18" Caption="Група Призначення"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="19" Caption="Призначення"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="history" VisibleIndex="20" Caption="Історична Цінність"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="input_date" VisibleIndex="21" Caption="Дата Актуальності"></dx:GridViewDataDateColumn>
                </Columns>

                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="sqr_pidval" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="sqr_in_rent" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="sqr_privatizov" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_balans" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_expert_1m" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_expert_total" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_zalishkova" SummaryType="Sum" DisplayFormat="{0}" />
                </TotalSummary>

                <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" 
                    EnableCustomizationWindow="True" />
                <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                <Styles Header-Wrap="True" />
                <SettingsCookies CookiesID="GUKV.Balans.BalansDetail" Enabled="False" Version="A2" />
            </dx:ASPxGridView>

            <mini:ProfiledSqlDataSource ID="SqlDataSourceBalansDetail" runat="server" 
                ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                OnSelecting="SqlDataSourceBalansDetail_Selecting" 
                SelectCommand="SELECT balans_id, building_id, organization_id, district, street_full_name, addr_nomer,
                               sqr_total, sqr_pidval, sqr_in_rent, sqr_privatizov, cost_balans, cost_expert_1m, cost_expert_total,
                               cost_zalishkova, o26_code, num_floors, form_ownership, ownership_type, object_kind, object_type,
                               condition, purpose_group, purpose, history, input_date FROM view_balans WHERE (organization_id = @organization_id)">
                <SelectParameters>
                    <asp:Parameter DbType="Int32" DefaultValue="0" Name="organization_id" />
                </SelectParameters>
            </mini:ProfiledSqlDataSource>

        </DetailRow>
    </Templates>

    <ClientSideEvents Init="GridViewBalansInit" EndCallback="GridViewBalansEndCallback" />
</dx:ASPxGridView>

</center>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" SupportsDisabledAttribute="True">
            <uc1:SaveReportCtrl ID="SaveReportCtrl2" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="PopupAddressPicker" runat="server" 
    HeaderText="Швидкий Пошук За Адресою" 
    ClientInstanceName="PopupAddressPicker" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
            <uc2:AddressPicker ID="AddressPicker2" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="PopupFieldChooser" runat="server" 
    HeaderText="Додаткові Колонки" 
    ClientInstanceName="PopupFieldChooser" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>

