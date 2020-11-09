<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrPlataByRenters.aspx.cs" Inherits="OrendnaPlata_OrPlataByRenters"
    MasterPageFile="~/NoHeader.master" Title="Орендна Плата - Орендарі" %>

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

    function GridViewRentByRentersInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewRentByRentersEndCallback(s, e) {

        AdjustGridSizes();
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
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrendnaPlataByBalans.aspx" Text="Орендна плата"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByBalans.aspx" Text="Балансоутримувачі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByRenters.aspx" Text="Орендарі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByBuildings.aspx" Text="Приміщення"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataTotals.aspx" Text="Зведений Звіт"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataNotSubmitted.aspx" Text="Організації, Що Не Подали Звіт"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataObjectsUse.aspx" Text="Використання Неж. Фонду"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataVipiski.aspx" Text="Виписки"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentByRenters" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_rent_by_renters WHERE @p_rda_district_id = 0 OR
        (bal_org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND bal_org_district_id = @p_rda_district_id) OR
        (giver_org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND giver_org_district_id = @p_rda_district_id)"
    OnSelecting="SqlDataSourceRentByRenters_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
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
            <asp:Label ID="LabelReportTitle2" runat="server" Text="Орендна плата в розрізі орендарів" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
		<td>
			<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px">
				<ClientSideEvents Click="ShowFieldFixxerPopupControl" />
			</dx:ASPxButton>
		</td>
        <td>
            <dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_RentByRenters_SaveAs" 
                PopupElementID="ASPxButton_RentByRenters_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton7" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_RentByRenters_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton8" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_RentByRenters_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton9" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_RentByRenters_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_RentByRenters_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewRentByRentersExporter" runat="server" 
    FileName="Орендарi" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID="SqlDataSourceRentByRenters"
    KeyFieldName="rent_id"
    OnCustomCallback="GridViewRentByRenters_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewRentByRenters_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewRentByRenters_ProcessColumnAutoFilter" >

    <Columns>
        <dx:GridViewDataComboBoxColumn FieldName="rent_period_id" VisibleIndex="0" Caption="Звітній Період">
            <PropertiesComboBox DataSourceID="SqlDataSourceRentPeriods" ValueField="id" TextField="name" ValueType="System.Int32" />
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataTextColumn FieldName="renter_name" VisibleIndex="1" Visible="True" Caption="Назва Орендаря" Width="180px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("renter_organization_id"), Eval("renter_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="renter_zkpo" VisibleIndex="2" Visible="True" Caption="Код ЄДРПОУ Орендаря"/>
        <dx:GridViewDataTextColumn FieldName="bal_org_full_name" VisibleIndex="3" Visible="True" Caption="Повна Назва Балансоутримувача" Width="180px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("bal_org_id"), Eval("bal_org_full_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_short_name" VisibleIndex="4" Visible="False" Caption="Коротка Назва Балансоутримувача">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("bal_org_id"), Eval("bal_org_short_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_zkpo" VisibleIndex="5" Visible="False" Caption="Код ЄДРПОУ Балансоутримувача"/>
        <dx:GridViewDataTextColumn FieldName="bal_org_occupation" VisibleIndex="6" Visible="True" Caption="Сфера Діяльності Балансоутримувача"/>
        <dx:GridViewDataTextColumn FieldName="sqr_total_rent" VisibleIndex="7" Visible="True" Caption="Орендована Площа, загальна (кв.м.)"/>
        <dx:GridViewDataTextColumn FieldName="sqr_payed_by_percent" VisibleIndex="8" Visible="False" Caption="Орендована Площа З Відсотковою Орендною Платою. (кв.м.)"/>
        <dx:GridViewDataTextColumn FieldName="sqr_payed_by_1uah" VisibleIndex="9" Visible="False" Caption="Орендована Площа З Орендною Платою 1 грн. (кв.м.)"/>

        <dx:GridViewDataTextColumn FieldName="sqr_payed_hourly" VisibleIndex="10" Visible="False" Caption="Орендована Площа З Погодинною Орендною Платою. (кв.м.)"/>
        <dx:GridViewDataTextColumn FieldName="payment_narah" VisibleIndex="11" Visible="False" Caption="Нараховано Коштів За Оренду (грн.)"/>
        <dx:GridViewDataTextColumn FieldName="payment_received" VisibleIndex="12" Visible="False" Caption="Отримано Коштів За Оренду (грн.)"/>
        <dx:GridViewDataTextColumn FieldName="payment_budget_50_uah" VisibleIndex="13" Visible="False" Caption="Відрахування 50% До Бюджету (грн.)"/>
        <dx:GridViewDataTextColumn FieldName="debt_total" VisibleIndex="14" Visible="True" Caption="Заборгованість, всього (грн.)"/>
        <dx:GridViewDataTextColumn FieldName="debt_3_month" VisibleIndex="15" Visible="False" Caption="Заборгованість Поточна До 3 Місяців (грн.)"/>
        <dx:GridViewDataTextColumn FieldName="debt_12_month" VisibleIndex="16" Visible="False" Caption="Заборгованість прострочена від 4 до 12 місяців (грн.)"/>
        <dx:GridViewDataTextColumn FieldName="debt_3_years" VisibleIndex="17" Visible="False" Caption="Заборгованість Прострочена Від 1 До 3 Років (грн.)"/>
        <dx:GridViewDataTextColumn FieldName="debt_over_3_years" VisibleIndex="18" Visible="False" Caption="Заборгованість Безнадійна (Більше 3 Років) (грн.)"/>
        <dx:GridViewDataTextColumn FieldName="debt_spysano" VisibleIndex="19" Visible="False" Caption="Списано Заборгованості (грн.)"/>

        <dx:GridViewDataTextColumn FieldName="num_zahodiv_total" VisibleIndex="20" Visible="False" Caption="Кількість Заходів (попереджень, приписів і т.п.), всього"/>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_zvit" VisibleIndex="21" Visible="False" Caption="Кількість Заходів (попереджень, приписів і т.п.), у звітньому періоді"/>
        <dx:GridViewDataTextColumn FieldName="num_pozov_total" VisibleIndex="22" Visible="False" Caption="Кількість Позовів До Суду, всього"/>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zvit" VisibleIndex="23" Visible="False" Caption="Кількість Позовів До Суду, у звітньому періоді"/>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_total" VisibleIndex="24" Visible="False" Caption="Задоволено Позовів, всього"/>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_zvit" VisibleIndex="25" Visible="False" Caption="Задоволено Позовів, у звітньому періоді"/>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_total" VisibleIndex="26" Visible="False" Caption="Відкрито Виконавчих Впроваджень, всього"/>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_zvit" VisibleIndex="27" Visible="False" Caption="Відкрито Виконавчих Впроваджень, у звітньому періоді"/>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_total" VisibleIndex="28" Visible="False" Caption="Погашено Заборгованості, всього (грн.)"/>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_zvit" VisibleIndex="29" Visible="False" Caption="Погашено Заборгованості, у звітньому періоді (грн.)"/>

        <dx:GridViewDataTextColumn FieldName="debt_v_mezhah_vitrat" VisibleIndex="30" Visible="False" Caption="Заборгованість З Орендної Плати, Розмір Якої Встановлено В Межах Витрат На Утримання (грн.)"/>
        <dx:GridViewDataTextColumn FieldName="rent_agreement_active" VisibleIndex="31" Visible="False" Caption="Договір Діючий"/>

        <dx:GridViewDataTextColumn FieldName="giver_org_full_name" VisibleIndex="32" Visible="False" Caption="Повна Назва Орендодавця" Width="180px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("giver_org_id"), Eval("giver_org_full_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="giver_org_short_name" VisibleIndex="33" Visible="False" Caption="Коротка Назва Орендодавця">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("giver_org_id"), Eval("giver_org_short_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="giver_org_zkpo" VisibleIndex="34" Visible="False" Caption="Код ЄДРПОУ Орендодавця"/>
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
    <SettingsCookies CookiesID="GUKV.OrendnaPlata.ByRenters" Version="A2_2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewRentByRentersInit" EndCallback="GridViewRentByRentersEndCallback" />
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
