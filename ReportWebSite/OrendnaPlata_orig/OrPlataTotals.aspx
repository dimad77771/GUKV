<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrPlataTotals.aspx.cs" Inherits="OrendnaPlata_OrPlataTotals"
    MasterPageFile="~/NoHeader.master" Title="Зведений Звіт з Орендної Плати" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/SaveReportCtrl.ascx" tagname="SaveReportCtrl" tagprefix="uc1" %>
<%@ Register src="../UserControls/AddressPicker.ascx" tagname="AddressPicker" tagprefix="uc2" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register src="../UserControls/FieldFixxer.ascx" tagname="FieldFixxer" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

// <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function ShowFieldFixxerPopupControl(s, e) { PopupFieldFixxer.Show(); }

    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 185);
    }

    function GridViewRentTotalsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewRentTotalsEndCallback(s, e) {

        AdjustGridSizes();
    }

    function OnRentTotalsApplyPreferences(s, e) {

        PopupReportParameters.Hide();

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("rebind:"));
    }

    function ShowFoldersPopupControl(s, e) {

        PopupControlFolders.Show();
    }

    function ShowFieldChooserPopupControl(s, e) {

        PopupFieldChooser.Show();
    }

// ]]>

</script>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByBalans.aspx" Text="Балансоутримувачі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByRenters.aspx" Text="Орендарі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByBuildings.aspx" Text="Приміщення"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataTotals.aspx" Text="Зведений Звіт"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataNotSubmitted.aspx" Text="Організації, Що Не Подали Звіт"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataObjectsUse.aspx" Text="Використання Неж. Фонду"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataVipiski.aspx" Text="Виписки"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentTotalsAll" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="fnRentByOccupation"
    SelectCommandType="StoredProcedure"
    OnSelecting="SqlDataSourceRentTotalsAll_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="RDA_DISTRICT_ID" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="RENT_AGREEMENT_ACTIVE" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentTotalsActive" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="fnRentByOccupation"
    SelectCommandType="StoredProcedure"
    OnSelecting="SqlDataSourceRentTotalsActive_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="RDA_DISTRICT_ID" />
        <asp:Parameter DbType="Int32" DefaultValue="1" Name="RENT_AGREEMENT_ACTIVE" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentTotalsInactive" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="fnRentByOccupation"
    SelectCommandType="StoredProcedure"
    OnSelecting="SqlDataSourceRentTotalsInactive_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="RDA_DISTRICT_ID" />
        <asp:Parameter DbType="Int32" DefaultValue="2" Name="RENT_AGREEMENT_ACTIVE" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentPeriods" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_rent_period">
</mini:ProfiledSqlDataSource>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle4" runat="server" Text="Зведений звіт з орендної плати" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxPopupControl ID="PopupReportParameters" runat="server" 
                HeaderText="Параметри Звіту" 
                ClientInstanceName="PopupReportParameters" 
                PopupElementID="ButtonReportParameters">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl12" runat="server">
                        <dx:ASPxRadioButton runat="server" GroupName="RbRentTotalsGroup1" ID="RbRentTotalsAllAgreements" ClientInstanceName="RbRentTotalsAllAgreements"
                            Checked="true" AutoPostBack="false" Text="Всі Договори Оренди" Width="250px" />
                        <dx:ASPxRadioButton runat="server" GroupName="RbRentTotalsGroup1" ID="RbRentTotalsActiveAgreements" ClientInstanceName="RbRentTotalsActiveAgreements"
                            Checked="false" AutoPostBack="false" Text="Лише Діючі Договори Оренди" Width="250px" />
                        <dx:ASPxRadioButton runat="server" GroupName="RbRentTotalsGroup1" ID="RbRentTotalsInactiveAgreements" ClientInstanceName="RbRentTotalsInactiveAgreements"
                            Checked="false" AutoPostBack="false" Text="Лише Недіючі Договори Оренди" Width="250px" />
                        <br/>
                        <center>
                        <dx:ASPxButton ID="RentTotalsApplyPreferences" ClientInstanceName="RentTotalsApplyPreferences" runat="server" AutoPostBack="False" Text="Вибрати" Width="148px">
                            <ClientSideEvents Click="OnRentTotalsApplyPreferences" />
                        </dx:ASPxButton>
                        </center>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ButtonReportParameters" runat="server" AutoPostBack="False" Text="" ImageSpacing="0px" AllowFocus="false" Tooltip="Параметри Звіту">
                <Image Url="../Styles/PreferencesIcon.png" />
                <FocusRectPaddings Padding="1px" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
		<td>
			<dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px">
				<ClientSideEvents Click="ShowFieldFixxerPopupControl" />
			</dx:ASPxButton>
		</td>
        <td>
            <dx:ASPxButton ID="ASPxButton18" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl4" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_RentTotals_SaveAs" 
                PopupElementID="ASPxButton_RentTotals_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <dx:ASPxButton ID="ASPxButton19" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_RentTotals_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton20" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_RentTotals_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton21" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_RentTotals_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_RentTotals_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewRentTotalsExporter" runat="server" 
    FileName="ОренднаПлата" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID="SqlDataSourceRentTotalsAll"
    KeyFieldName="rent_period_id; bal_org_occupation_id"
    OnCustomCallback="GridViewRentTotals_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewRentTotals_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewRentTotals_ProcessColumnAutoFilter" >

    <Columns>
        <dx:GridViewDataComboBoxColumn FieldName="rent_period_id" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0" Visible="True" Caption="Звітній Період">
            <PropertiesComboBox DataSourceID="SqlDataSourceRentPeriods" ValueField="id" TextField="name" ValueType="System.Int32" />
        </dx:GridViewDataComboBoxColumn>

        <dx:GridViewDataTextColumn FieldName="bal_org_occupation" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Visible="True" Caption="Сфера Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total_rent" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Visible="True" Caption="Орендована Площа, загальна (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_payed_by_percent" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3" Visible="False" Caption="Орендована Площа З Відсотковою Орендною Платою. (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_payed_by_1uah" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Visible="False" Caption="Орендована Площа З Орендною Платою 1 грн. (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_payed_hourly" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Visible="False" Caption="Орендована Площа З Погодинною Орендною Платою. (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_narah" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="6" Visible="True" Caption="Нараховано Коштів За Оренду (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_received" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="7" Visible="True" Caption="Отримано Коштів За Оренду (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_budget_50_uah" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="8" Visible="False" Caption="Відрахування 50% До Бюджету (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_total" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="9" Visible="True" Caption="Заборгованість, всього (грн.)"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="debt_3_month" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="10" Visible="False" Caption="Заборгованість Поточна До 3 Місяців (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_12_month" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="11" Visible="False" Caption="Заборгованість прострочена від 4 до 12 місяців (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_years" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="12" Visible="False" Caption="Заборгованість Прострочена Від 1 До 3 Років (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_over_3_years" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="13" Visible="False" Caption="Заборгованість Безнадійна (Більше 3 Років) (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_spysano" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="14" Visible="False" Caption="Списано Заборгованості (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_total" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="15" Visible="False" Caption="Кількість Заходів (попереджень, приписів і т.п.), всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_zvit" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="16" Visible="False" Caption="Кількість Заходів (попереджень, приписів і т.п.), у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_total" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="17" Visible="False" Caption="Кількість Позовів До Суду, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zvit" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="18" Visible="False" Caption="Кількість Позовів До Суду, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_total" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="19" Visible="False" Caption="Задоволено Позовів, всього"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_zvit" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="20" Visible="False" Caption="Задоволено Позовів, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_total" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="21" Visible="False" Caption="Відкрито Виконавчих Впроваджень, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_zvit" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="22" Visible="False" Caption="Відкрито Виконавчих Впроваджень, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_total" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="23" Visible="False" Caption="Погашено Заборгованості, всього (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_zvit" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="24" Visible="False" Caption="Погашено Заборгованості, у звітньому періоді (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_v_mezhah_vitrat" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="25" Visible="False" Caption="Заборгованість З Орендної Плати, Розмір Якої Встановлено В Межах Витрат На Утримання (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="total_num_renters" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="26" Visible="True" Caption="Загальна Кількість Орендарів"></dx:GridViewDataTextColumn>
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total_rent" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_payed_by_percent" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_payed_by_1uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_payed_hourly" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_narah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_received" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_budget_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_3_month" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_12_month" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_3_years" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_over_3_years" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_spysano" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_zahodiv_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_zahodiv_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zadov_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zadov_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_vikon_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_vikon_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_pogasheno_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_pogasheno_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_v_mezhah_vitrat" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="total_num_renters" SummaryType="Sum" DisplayFormat="{0}" />
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
        ShowFooter="True"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.OrendnaPlata.Totals" Version="A2_1" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewRentTotalsInit" EndCallback="GridViewRentTotalsEndCallback" />
</dx:ASPxGridView>

</center>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
            <uc1:SaveReportCtrl ID="FolderBrowser" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="PopupFieldFixxer" runat="server" 
    HeaderText="Закріпити Колонки" 
    ClientInstanceName="PopupFieldFixxer" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
            <uc3:FieldFixxer ID="FieldFixxer1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns1.PerformCallback(); }" />
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
        <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>
