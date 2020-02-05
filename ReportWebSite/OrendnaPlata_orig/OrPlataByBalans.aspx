<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrPlataByBalans.aspx.cs" Inherits="OrendnaPlata_OrPlataByBalans"
    MasterPageFile="~/NoHeader.master" Title="Орендна Плата - Звіти Балансоутримувачів" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
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

    function GridViewRentByBalansOrgInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewRentByBalansOrgEndCallback(s, e) {

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
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByBalans.aspx" Text="Балансоутримувачі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByRenters.aspx" Text="Орендарі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByBuildings.aspx" Text="Приміщення"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataTotals.aspx" Text="Зведений Звіт"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataNotSubmitted.aspx" Text="Організації, Що Не Подали Звіт"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataObjectsUse.aspx" Text="Використання Неж. Фонду"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataVipiski.aspx" Text="Виписки"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentByBalansOrg" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_rent_by_balans_org WHERE (@p_rda_district_id = 0 OR (org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND org_district_id = @p_rda_district_id))"
    OnSelecting="SqlDataSourceRentByBalansOrg_Selecting">
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
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Орендна плата в розрізі балансоутримувачів" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup1" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
		<td>
			<dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px">
				<ClientSideEvents Click="ShowFieldFixxerPopupControl" />
			</dx:ASPxButton>
		</td>
        <td>
            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_RentByBalansOrg_SaveAs" 
                PopupElementID="ASPxButton_RentByBalansOrg_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxButton ID="ASPxButton3" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_RentByBalansOrg_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_RentByBalansOrg_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_RentByBalansOrg_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_RentByBalansOrg_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewRentByBalansOrgExporter" runat="server" 
    FileName="Балансоутримувачi" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID="SqlDataSourceRentByBalansOrg"
    KeyFieldName="payment_id"
    OnCustomCallback="GridViewRentByBalansOrg_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewRentByBalansOrg_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewRentByBalansOrg_ProcessColumnAutoFilter" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="org_full_name" ReadOnly="True" 
            VisibleIndex="0" Visible="True" Caption="Повна Назва Балансоутримувача" 
            Width="180px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("org_balans_id"), Eval("org_full_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_short_name" ReadOnly="True" 
            VisibleIndex="1" Visible="False" Caption="Коротка Назва Балансоутримувача" 
            Width="180px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("org_balans_id"), Eval("org_short_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" 
            VisibleIndex="2" Visible="True" Caption="Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_dkk_name" ReadOnly="True" 
            VisibleIndex="3" Visible="False" Caption="Класифікація"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_dkk_short_name" ReadOnly="True" 
            VisibleIndex="4" Visible="False" Caption="Класифікація (скор.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_rent_occupation" ReadOnly="True" 
            VisibleIndex="5" Visible="True" Caption="Сфера Діяльності"></dx:GridViewDataTextColumn>

        <dx:GridViewDataComboBoxColumn FieldName="rent_period_id" VisibleIndex="6" 
            Visible="True" Caption="Звітній Період">
            <PropertiesComboBox DataSourceID="SqlDataSourceRentPeriods" ValueField="id" TextField="name" ValueType="System.Int32" />
        </dx:GridViewDataComboBoxColumn>

        <dx:GridViewDataTextColumn FieldName="sqr_balans" ReadOnly="True" 
            VisibleIndex="7" Visible="True" 
            Caption="Площа Нежитлових Приміщень На Балансі, всього (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_zvit_uah" ReadOnly="True" 
            VisibleIndex="8" Visible="False" 
            Caption="Нараховано Орендної Плати, за звітній період (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="received_zvit_uah" ReadOnly="True" 
            VisibleIndex="9" Visible="False" 
            Caption="Отримано Орендної Плати, за звітній період (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="received_nar_zvit_uah" ReadOnly="True" 
            VisibleIndex="10" Visible="False" 
            Caption="Отримано Орендної Плати з Нарахованої, за звітній період (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_total_uah" ReadOnly="True" 
            VisibleIndex="11" Visible="False" 
            Caption="Загальна Заборгованість По Орендній Платі (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_zvit_uah" ReadOnly="True" 
            VisibleIndex="12" Visible="False" 
            Caption="Заборгованість По Орендній Платі за звітній період (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_v_mezhah_vitrat" ReadOnly="True" 
            VisibleIndex="13" Visible="False" 
            Caption="Заборгованість З Орендної Плати В Межах Витрат На Утримання (грн.)"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="budget_narah_50_uah" ReadOnly="True" 
            VisibleIndex="14" Visible="False" 
            Caption="Нарахована Сума До Бюджету 50%, за звітній період (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_zvit_50_uah" ReadOnly="True" 
            VisibleIndex="15" Visible="False" 
            Caption="Перераховано До Бюджету 50%, у звітньому періоді всього з 1 січня (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_prev_50_uah" ReadOnly="True" 
            VisibleIndex="16" Visible="False" 
            Caption="Перераховано До Бюджету 50% Боргів, у звітньому періоді всього з 1 січня (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_debt_50_uah" ReadOnly="True" 
            VisibleIndex="17" Visible="False" 
            Caption="Заборгованість Зі Сплати 50% До Бюджету Від Оренди Майна (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_debt_30_50_uah" ReadOnly="True" 
            VisibleIndex="18" Visible="False" 
            Caption="Заборгованість Зі Сплати 30% (50%) До Бюджету Від Оренди Майна Минулих Років (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_renters" ReadOnly="True" 
            VisibleIndex="19" Visible="True" Caption="Кількість Орендарів"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_agreements" ReadOnly="True" 
            VisibleIndex="20" Visible="True" Caption="Кількість Договорів Оренди"></dx:GridViewDataTextColumn>
        
        <dx:GridViewDataTextColumn FieldName="sqr_cmk" ReadOnly="True" 
            VisibleIndex="21" Visible="False" Caption="ЦМК, Площа В Оренді (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_narah_50_cmk_uah" ReadOnly="True" 
            VisibleIndex="22" Visible="False" 
            Caption="ЦМК, Нарахована Орендна Плата (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_zvit_50_cmk_uah" ReadOnly="True" 
            VisibleIndex="23" Visible="False" Caption="ЦМК, Перераховано До Бюджету (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_debt_50_cmk_uah" ReadOnly="True" 
            VisibleIndex="24" Visible="False" 
            Caption="ЦМК, Заборгованість По Орендній Платі (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="pererahunok_50_b" ReadOnly="True" 
            VisibleIndex="25" Visible="False" 
            Caption="Перераховано До Бюджету За Користування Комунальним Майном (грн.)"></dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="budget_zvit_50_in_uah" ReadOnly="True" 
            VisibleIndex="26" Visible="False" 
            Caption="Перераховано До Бюджету 50% Від Оренди Іншого Майна (грн.)"></dx:GridViewDataTextColumn>--%>

        <dx:GridViewDataTextColumn FieldName="email" ReadOnly="True" VisibleIndex="27" 
            Visible="False" Caption="Електронна Адреса"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="phone" ReadOnly="True" VisibleIndex="28" 
            Visible="False" Caption="Контактний Телефон"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="responsible_fio" ReadOnly="True" 
            VisibleIndex="29" Visible="False" Caption="Відповідальний Виконавець"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_fio" ReadOnly="True" 
            VisibleIndex="30" Visible="False" Caption="Директор"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_fio" ReadOnly="True" 
            VisibleIndex="31" Visible="False" Caption="Бухгалтер"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="post_address" ReadOnly="True" 
            VisibleIndex="32" Visible="False" Caption="Поштова Адреса"></dx:GridViewDataTextColumn>
        
        <dx:GridViewDataTextColumn FieldName="debt_3_month" ReadOnly="True" 
            VisibleIndex="33" Visible="False" 
            Caption="Заборгованість Орендарів, поточна до 3 місяців (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_12_month" ReadOnly="True" 
            VisibleIndex="34" Visible="False" 
            Caption="Заборгованість Орендарів, прострочена від 3 до 12 місяців (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_years" ReadOnly="True" 
            VisibleIndex="35" Visible="False" 
            Caption="Заборгованість Орендарів, прострочена від 1 до 3 років (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_over_3_years" ReadOnly="True" 
            VisibleIndex="36" Visible="False" 
            Caption="Заборгованість Орендарів, безнадійна (більше 3 років) (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_spysano" ReadOnly="True" 
            VisibleIndex="37" Visible="False" 
            Caption="Списано Заборгованості Орендарів (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_total" ReadOnly="True" 
            VisibleIndex="38" Visible="False" 
            Caption="Кількість Заходів (попереджень, приписів і т.п.), всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_zvit" ReadOnly="True" 
            VisibleIndex="39" Visible="False" 
            Caption="Кількість Заходів (попереджень, приписів і т.п.), у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_total" ReadOnly="True" 
            VisibleIndex="40" Visible="False" Caption="Кількість Позовів До Суду, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zvit" ReadOnly="True" 
            VisibleIndex="41" Visible="False" 
            Caption="Кількість Позовів До Суду, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_total" ReadOnly="True" 
            VisibleIndex="42" Visible="False" Caption="Задоволено Позовів, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_zvit" ReadOnly="True" 
            VisibleIndex="43" Visible="False" 
            Caption="Задоволено Позовів, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_total" ReadOnly="True" 
            VisibleIndex="44" Visible="False" 
            Caption="Відкрито Виконавчих Впроваджень, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_zvit" ReadOnly="True" 
            VisibleIndex="45" Visible="False" 
            Caption="Відкрито Виконавчих Впроваджень, у звітньому періоді"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_total" ReadOnly="True" 
            VisibleIndex="46" Visible="False" 
            Caption="Погашено Заборгованості Орендарями, всього (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_zvit" ReadOnly="True" 
            VisibleIndex="47" Visible="False" 
            Caption="Погашено Заборгованості Орендарями, у звітньому періоді (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_sqr_rented" ReadOnly="True" 
            VisibleIndex="48" Visible="False" Caption="Площа Надана В Оренду (кв.м)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_sqr_free" ReadOnly="True" 
            VisibleIndex="49" Visible="False" Caption="Вільна Площа (кв.м)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_sqr_korysna" ReadOnly="True" 
            VisibleIndex="50" Visible="False" Caption="Корисна Площа (кв.м)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_sqr_mzk" ReadOnly="True" 
            VisibleIndex="51" Visible="False" Caption="Площа МЗК (кв.м.)"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="rozrah_50_uah" ReadOnly="True" 
            VisibleIndex="52" Visible="False" 
            Caption="Розрахований Показник Нарахованої Суми До Бюджету 50%, у звітньому періоді (грн.)"></dx:GridViewDataTextColumn>
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_balans" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_zvit_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="received_zvit_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_narah_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_zvit_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_debt_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_debt_30_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_cmk" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_narah_50_cmk_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_zvit_50_cmk_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_debt_50_cmk_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_agreements" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_renters" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_v_mezhah_vitrat" SummaryType="Sum" DisplayFormat="{0}" />
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
        <dx:ASPxSummaryItem FieldName="obj_sqr_rented" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="obj_sqr_free" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="obj_sqr_korysna" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="obj_sqr_mzk" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="rozrah_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
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
    <SettingsCookies CookiesID="GUKV.OrendnaPlata.ByBalansOrg" Version="A2_2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewRentByBalansOrgInit" EndCallback="GridViewRentByBalansOrgEndCallback" />
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
