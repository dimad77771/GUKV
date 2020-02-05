<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RentOrgList.aspx.cs" Inherits="Arenda_RentOrgList"
    MasterPageFile="~/NoHeader.master" Title="Перелік Орендарів" %>

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

    function GridViewArendaInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewArendaEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowFoldersPopupControl(s, e) {

        PopupControlFolders.Show();
    }

    function CheckBoxRentersComVlasn_CheckedChanged(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("bind:"));
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
        <dx:MenuItem NavigateUrl="../Arenda/RentAgreements.aspx" Text="Договори Оренди"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Arenda/RentedObjects.aspx" Text="Орендовані Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Arenda/RentOrgList.aspx" Text="Перелік Орендарів"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle2" runat="server" Text="Загальна інформація по орендарям" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxCheckBox ID="CheckBoxRentedObjectsDPZ" runat="server" Checked='True' Text="Дані ДПЗ"
                Width="100px" ClientInstanceName="CheckBoxRentedObjectsDPZ" >
                <ClientSideEvents CheckedChanged="CheckBoxRentedObjectsDPZ_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>        
        <td>
            <dx:ASPxCheckBox ID="CheckBoxRentersComVlasn" runat="server" Checked='False' Text="Лише Ком. Власність"
                Width="155px" ClientInstanceName="CheckBoxRentersComVlasn" >
                <ClientSideEvents CheckedChanged="CheckBoxRentersComVlasn_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup2" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Arenda_SaveAs" 
                PopupElementID="ASPxButton_Arenda_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxButton ID="ASPxButton_Arenda_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Arenda_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Arenda_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Arenda_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Arenda_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Arenda_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Arenda_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaOrganizations" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT vo.*, pay.payment_narah, pay.payment_received, debt.debt_total
                    FROM view_organizations vo
                    OUTER APPLY (SELECT TOP 1 mpba.payment_narah, mpba.payment_received FROM m_view_rent_pay_for_arenda mpba
                        WHERE mpba.organization_id = vo.organization_id ORDER BY mpba.rent_period_id DESC) pay
                    OUTER APPLY (SELECT TOP 1 mdba.debt_total FROM m_view_rent_debt_for_arenda mdba
                        WHERE mdba.organization_id = vo.organization_id ORDER BY mdba.rent_period_id DESC) debt
                    WHERE (vo.organization_id IN (SELECT org_renter_id FROM arenda)) AND
                 	    ((@p_dpz_filter = 0) OR (@p_dpz_filter <> 0 AND exists (select a.org_renter_id from dbo.reports1nf_arenda a where a.org_renter_id = vo.organization_id and ISNULL(a.is_deleted,0) = 0) )) AND 
                        ((@p_com_filter = 0) OR (@p_com_filter <> 0 AND vo.form_of_ownership_int IN (32,33,34))) AND
                        ((@p_rda_district_id = 0) OR (vo.form_of_ownership_int in (select id from dict_org_ownership where is_rda = 1) AND vo.addr_distr_new_id = @p_rda_district_id))"
    OnSelecting="SqlDataSourceArendaOrganizations_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="1" Name="p_dpz_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_com_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterArenda" runat="server" 
    FileName="Орендарi" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView ID="PrimaryGridView" runat="server" 
    ClientInstanceName="PrimaryGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceArendaOrganizations" 
    KeyFieldName="organization_id"
    Width="100%" 
    OnCustomCallback="GridViewArenda_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewArenda_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewArenda_ProcessColumnAutoFilter"
    OnCustomColumnSort="GridViewArenda_CustomColumnSort" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="organization_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="False" Caption="ID Орендаря">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("organization_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="True" Caption="Повна Назва Орендаря" Width="220px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="False" Caption="Коротка Назва Орендаря">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
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
            VisibleIndex="17" Visible="False" Caption="ОАТУУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="False" Caption="Відділ ДКВ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_narah" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="False" Caption="Нараховано Орендної Плати За Останній Звітній Період (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_received" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="False" Caption="Надходження Орендної Плати За Останній Звітній Період (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="Заборгованість По Орендній Платі, всього (грн.)"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="payment_narah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_received" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_total" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

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
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard"
        ShowFooter="True" />
    <SettingsCookies CookiesID="GUKV.Arenda" Version="A2" Enabled="True" />
    <SettingsDetail ShowDetailRow="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <Templates>
        <DetailRow>
            <dx:ASPxGridView ID="ASPxGridViewArendaDetail" runat="server" AutoGenerateColumns="False" 
                DataSourceID="SqlDataSourceArendaDetail" KeyFieldName="arenda_id"
                OnBeforePerformDataSelect="ASPxGridViewArendaDetail_BeforePerformDataSelect" 
                OnCustomSummaryCalculate="ASPxGridViewArendaDetail_CustomSummaryCalculate"
                Width="100%">

                <Columns>
                    <dx:GridViewDataTextColumn FieldName="org_giver_full_name" VisibleIndex="0" Caption="Орендодавець" Width="200px">
                        <DataItemTemplate>
                            <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_full_name") + "</a>"%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="org_giver_zkpo" VisibleIndex="1" Caption="Код ЄДРПОУ Орендодавця"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="org_balans_full_name" VisibleIndex="2" Caption="Балансоутримувач" Width="200px">
                        <DataItemTemplate>
                            <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_full_name") + "</a>"%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="org_balans_zkpo" VisibleIndex="3" Caption="Код ЄДРПОУ Балансоутримувача"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="4" Caption="Район"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="5" Caption="Вулиця" Width="160px">
                        <DataItemTemplate>
                            <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("street_full_name") + "</a>"%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="6" Caption="Номер Будинку">
                        <DataItemTemplate>
                            <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("addr_nomer") + "</a>"%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="object_name" VisibleIndex="7" Caption="Назва Об'єкту"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="8" Caption="Орендована Площа (кв.м.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="purpose_group" VisibleIndex="9" Caption="Група Призначення"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="10" Caption="Призначення"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="agreement_kind" VisibleIndex="11" Caption="Вид Договору"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="agreement_date" VisibleIndex="12" Caption="Дата Договору"></dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="agreement_num" VisibleIndex="13"  Caption="Номер Договору"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_narah" VisibleIndex="14" Caption="Нараховано (грн.)" Visible="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_payed" VisibleIndex="15" Caption="Сплачено (грн.)" Visible="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_debt" VisibleIndex="16" Caption="Борг (грн.)" Visible="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_agreement" VisibleIndex="17" Caption="Орендна Плата по Договору (грн.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_expert_1m" VisibleIndex="18" Caption="Оціночна Вартість За Кв.м. (грн.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="cost_expert_total" VisibleIndex="19" Caption="Оціночна Вартість (грн.)"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="pidstava_display" VisibleIndex="20" Caption="Підстава"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="rent_start_date" VisibleIndex="21" Caption="Початок Оренди"></dx:GridViewDataDateColumn>
                    <dx:GridViewDataDateColumn FieldName="rent_finish_date" VisibleIndex="22" Caption="Закінчення Оренди"></dx:GridViewDataDateColumn>
                    <dx:GridViewDataDateColumn FieldName="rent_actual_finish_date" VisibleIndex="23" Caption="Фактичне Закінчення Оренди"></dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="is_subarenda" VisibleIndex="24" Caption="Суборенда"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="payment_type" VisibleIndex="25" Caption="Вид Розрахунків"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="agreement_active" VisibleIndex="26" Caption="Договір Діючий"></dx:GridViewDataTextColumn>
                </Columns>

                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_narah" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_payed" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_debt" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_agreement" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_expert_1m" SummaryType="Sum" DisplayFormat="{0}" />
                    <dx:ASPxSummaryItem FieldName="cost_expert_total" SummaryType="Sum" DisplayFormat="{0}" />
                </TotalSummary>

                <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" 
                    EnableCustomizationWindow="True" />
                <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                <Styles Header-Wrap="True" />
                <SettingsCookies CookiesID="GUKV.Arenda.ArendaDetail" Enabled="False" Version="A2" />
            </dx:ASPxGridView>

            <mini:ProfiledSqlDataSource ID="SqlDataSourceArendaDetail" runat="server" 
                ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                onselecting="SqlDataSourceArendaDetail_Selecting" 
                SelectCommand="SELECT * FROM m_view_arenda WHERE (org_renter_id = @organization_id)">
                <SelectParameters>
                    <asp:Parameter DbType="Int32" DefaultValue="0" Name="organization_id" />
                </SelectParameters>
            </mini:ProfiledSqlDataSource>

        </DetailRow>
    </Templates>

    <ClientSideEvents Init="GridViewArendaInit" EndCallback="GridViewArendaEndCallback" />
</dx:ASPxGridView>

</center>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
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
        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
            <uc2:AddressPicker ID="AddressPicker1" runat="server"/>
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
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>

