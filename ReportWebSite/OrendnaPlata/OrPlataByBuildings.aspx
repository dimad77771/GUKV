<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrPlataByBuildings.aspx.cs" Inherits="OrendnaPlata_OrPlataByBuildings"
    MasterPageFile="~/NoHeader.master" Title="Орендна Плата - Приміщення" %>

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

    function GridViewRentByBuildingsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewRentByBuildingsEndCallback(s, e) {

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

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentByBuildings" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_rent_by_buildings WHERE (@p_rda_district_id = 0 OR (bal_org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND bal_org_district_id = @p_rda_district_id))"
    OnSelecting="SqlDataSourceRentByBuildings_Selecting">
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
            <asp:Label ID="LabelReportTitle3" runat="server" Text="Орендна плата в розрізі приміщень" CssClass="reporttitle"></asp:Label>
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
            <dx:ASPxButton ID="ASPxButton12" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl3" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_RentByBuildings_SaveAs" 
                PopupElementID="ASPxButton_RentByBuildings_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                        <dx:ASPxButton ID="ASPxButton13" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_RentByBuildings_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton14" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_RentByBuildings_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton15" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_RentByBuildings_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_RentByBuildings_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewRentByBuildingsExporter" runat="server" 
    FileName="Примiщення" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID="SqlDataSourceRentByBuildings"
    KeyFieldName="rent_object_id"
    OnCustomCallback="GridViewRentByBuildings_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewRentByBuildings_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewRentByBuildings_ProcessColumnAutoFilter"
    OnCustomColumnSort="GridViewRentByBuildings_CustomColumnSort" >

    <Columns>
        <dx:GridViewDataComboBoxColumn FieldName="rent_period_id" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0" Visible="True" Caption="Звітній Період">
            <PropertiesComboBox DataSourceID="SqlDataSourceRentPeriods" ValueField="id" TextField="name" ValueType="System.Int32" />
        </dx:GridViewDataComboBoxColumn>

        <dx:GridViewDataTextColumn FieldName="district" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Visible="True" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Visible="True" Caption="Назва Вулиці">
            <DataItemTemplate>
                <%# EvaluateObjectLink(Eval("building_id"), Eval("addr_street")) %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_type" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3" Visible="True" Caption="Тип Вулиці"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_num" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Visible="True" Caption="Номер Будинку (літера, корпус)">
            <DataItemTemplate>
                <%# EvaluateObjectLink(Eval("building_id"), Eval("addr_num"))%>
            </DataItemTemplate>
            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Visible="True" Caption="Загальна Площа На Балансі (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_rented" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="6" Visible="True" Caption="Площа, Надана В Оренду (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="7" Visible="False" Caption="Вільна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_korysna" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="8" Visible="False" Caption="Вільна Площа, корисна (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_mzk" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="9" Visible="False" Caption="Вільна Площа, МЗК (кв.м.)"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="floors" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="10" Visible="False" Caption="Місце Розташування"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="tech_state" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="11" Visible="False" Caption="Технічний Стан"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_energo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="12" Visible="False" Caption="Наявність енергозабезпечення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_vodo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="13" Visible="False" Caption="Наявність водозабезпечення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_teplo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="14" Visible="False" Caption="Наявність теплозабезпечення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="15" Visible="False" Caption="Можливе Використання Приміщення"></dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="rent_note" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="16" Visible="False" Caption="Примітки"></dx:GridViewDataTextColumn>--%>

        <dx:GridViewDataTextColumn FieldName="bal_org_full_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="17" Visible="True" Caption="Повна Назва Балансоутримувача" Width="180px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("bal_org_id"), Eval("bal_org_full_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_short_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="18" Visible="False" Caption="Коротка Назва Балансоутримувача">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("bal_org_id"), Eval("bal_org_short_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_zkpo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="19" Visible="False" Caption="Код ЄДРПОУ Балансоутримувача"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_occupation" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="20" Visible="False" Caption="Сфера Діяльності Балансоутримувача"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_phone" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="21" Visible="False" Caption="Телефон Балансоутримувача"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_email" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="22" Visible="False" Caption="Електронна Адреса Балансоутримувача"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_responsible_fio" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="23" Visible="False" Caption="Відповідальна Особа Балансоутримувача"></dx:GridViewDataTextColumn>
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_rented" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_korysna" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_mzk" SummaryType="Sum" DisplayFormat="{0}" />
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
    <SettingsCookies CookiesID="GUKV.OrendnaPlata.ByBuildings" Version="A2_2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewRentByBuildingsInit" EndCallback="GridViewRentByBuildingsEndCallback" />
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
