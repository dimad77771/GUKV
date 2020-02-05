<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrPlataObjectsUse.aspx.cs" Inherits="OrendnaPlata_OrPlataObjectsUse"
    MasterPageFile="~/NoHeader.master" Title="Орендна Плата - Використання Неж. Фонду" %>

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

    function ButtonObjectsUsePeriodsApplyClick(s, e) {

        ASPxPopupControl_ObjectsUse_Periods.Hide();

        // Combine all selected values into calback parameter
        var param = "selperiods:";
        var values = ListBoxObjectsUsePeriods.GetSelectedValues();

        for (var i = 0; i < values.length; i++) {

            if (i > 0) {
                param = param + "?";
            }

            param = param + values[i];
        }

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam(param));
    }

    function ButtonObjectsUseFieldsApplyClick(s, e) {

        ASPxPopupControl_ObjectsUse_Fields.Hide();

        // Enumerate all selected primary columns
        var primaryFields = "";
        var values1 = ListBoxObjectsUseColumns.GetSelectedValues();

        for (var i = 0; i < values1.length; i++) {

            if (i > 0) {
                primaryFields = primaryFields + "?";
            }

            primaryFields = primaryFields + values1[i];
        }

        // Enumerate all selected additional columns
        var secondaryFields = "";
        var values2 = ListBoxObjectsUseFields.GetSelectedValues();

        for (var j = 0; j < values2.length; j++) {

            if (j > 0) {
                secondaryFields = secondaryFields + "?";
            }

            secondaryFields = secondaryFields + values2[j];
        }

        // Combine all selected values into calback parameter
        var param = "selfields:" + primaryFields + "#" + secondaryFields;

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam(param));
    }

    function GridViewRentObjectsUseInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewRentObjectsUseEndCallback(s, e) {

        AdjustGridSizes();
    }

    function OnButtonObjectsUseCompareClick(s, e) {

        PopupCompare.Hide();

        var val1 = "" + ComboCompareField1.GetValue();
        var val2 = "" + ComboCompareField2.GetValue();

        if (val1 != null && val1 != undefined &&
            val2 != null && val2 != undefined) {

            var param = "compare:" + val1 + ":" + val2;

            if (CheckCompareByPercent.GetChecked()) {
                param = param + "%";
            }

            PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam(param));
        }
        else {
            alert("Будь ласка, виберіть два показники для порівняння.");
        }
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

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentObjectsUse" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
        rbo.organization_id,
        org.full_name,
        org.short_name,
        org.zkpo_code,
        dict_org_ownership.name AS 'org_ownership',
        dict_rent_occupation.name AS 'rent_occupation'
    FROM
        rent_balans_org rbo
        LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = rbo.rent_occupation_id
        LEFT OUTER JOIN organizations org ON org.id = rbo.organization_id and (org.is_deleted is null or org.is_deleted = 0)
        LEFT OUTER JOIN dict_org_ownership ON org.form_ownership_id = dict_org_ownership.id
    WHERE
        (@p_rda_district_id = 0 OR (org.form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND org.addr_distr_new_id = @p_rda_district_id))"
    OnSelecting="SqlDataSourceRentObjectsUse_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentObjectsUsePeriods" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_rent_period ORDER BY period_start">
</mini:ProfiledSqlDataSource>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle6" runat="server" Text="Використання житлового фонду міста та районів" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxPopupControl ID="PopupCompare" runat="server" 
                ClientInstanceName="PopupCompare" 
                HeaderText="Порівняння показників" 
                PopupElementID="ButtonCompare">

                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl11" runat="server">

                        <dx:ASPxCallbackPanel ID="CallbackPanelCompare"
                            ClientInstanceName="CallbackPanelCompare"
                            runat="server" >
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">

                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Перший показник для порівняння:"></dx:ASPxLabel>
                                    <dx:ASPxComboBox ID="ComboCompareField1" runat="server" ValueType="System.Int32" Width="300px" ClientInstanceName="ComboCompareField1"
                                        OnCallback="ComboCompareField_Callback" >
                                    </dx:ASPxComboBox>
                                    <br/>

                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Другий показник для порівняння:"></dx:ASPxLabel>
                                    <dx:ASPxComboBox ID="ComboCompareField2" runat="server" ValueType="System.Int32" Width="300px" ClientInstanceName="ComboCompareField2"
                                        OnCallback="ComboCompareField_Callback" >
                                    </dx:ASPxComboBox>
                                    <br/>

                                    <dx:ASPxCheckBox ID="CheckCompareByPercent" ClientInstanceName="CheckCompareByPercent" runat="server" Checked='False' Text="Порівняти У Відсотках" />
                                    <br/>

                                    <dx:ASPxButton ID="ButtonObjectsUseCompare" runat="server" AutoPostBack="False" Text="Порівняти" Width="148px">
                                        <ClientSideEvents Click="OnButtonObjectsUseCompareClick" />
                                    </dx:ASPxButton>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents PopUp="function (s, e) { ComboCompareField1.PerformCallback(''); ComboCompareField2.PerformCallback(''); }" />
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ButtonCompare" ClientInstanceName="ButtonCompare"
                runat="server" AutoPostBack="False" Text="Порівняння" Width="148px"></dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_ObjectsUse_Periods" runat="server" 
                HeaderText="Вибір Періодів" 
                ClientInstanceName="ASPxPopupControl_ObjectsUse_Periods" 
                PopupElementID="ASPxButton_ObjectsUse_Periods">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server">
                        
                        <dx:ASPxCallbackPanel ID="CPObjectsUsePeriods" runat="server" ClientInstanceName="CPObjectsUsePeriods" OnCallback="CallbackPanelObjectsUsePeriods_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent0" runat="server">

                                    <dx:ASPxListBox ID="ListBoxObjectsUsePeriods" ClientInstanceName="ListBoxObjectsUsePeriods" runat="server"
                                        DataSourceID="SqlDataSourceRentObjectsUsePeriods" TextField="name" ValueField="id" ValueType="System.Int32"
                                        Width="180px" Height="220px" SelectionMode="CheckColumn" >
                                    </dx:ASPxListBox>

                                </dx:panelcontent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                        <br/>

                        <dx:ASPxButton ID="ButtonObjectsUsePeriodsApply" runat="server" AutoPostBack="False" Text="Вибрати" Width="120px">
                            <ClientSideEvents Click="ButtonObjectsUsePeriodsApplyClick" />
                        </dx:ASPxButton>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents PopUp="function (s, e) { CPObjectsUsePeriods.PerformCallback(''); }" />
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_ObjectsUse_Periods" runat="server" AutoPostBack="False" Text="Періоди" Width="148px"></dx:ASPxButton>
        </td>
		<td>
			<dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px">
				<ClientSideEvents Click="ShowFieldFixxerPopupControl" />
			</dx:ASPxButton>
		</td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_ObjectsUse_Fields" runat="server" 
                HeaderText="Вибір Колонок" 
                ClientInstanceName="ASPxPopupControl_ObjectsUse_Fields" 
                PopupElementID="ASPxButton_ObjectsUse_Fields">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server">
                        
                        <dx:ASPxCallbackPanel ID="CPObjectsUseFields" runat="server" ClientInstanceName="CPObjectsUseFields" OnCallback="CallbackPanelObjectsUseFields_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent1" runat="server">

                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Основні колонки"></dx:ASPxLabel>

                                    <dx:ASPxListBox ID="ListBoxObjectsUseColumns" ClientInstanceName="ListBoxObjectsUseColumns" runat="server"
                                        ValueType="System.Int32" Width="300px" Height="150px" SelectionMode="CheckColumn" >
                                        <Items>
                                            <dx:ListEditItem Text="Повна Назва Балансоутримувача" Value="0" />
                                            <dx:ListEditItem Text="Коротка Назва Балансоутримувача" Value="1" />
                                            <dx:ListEditItem Text="Код ЄДРПОУ Балансоутримувача" Value="2" />
                                            <dx:ListEditItem Text="Форма Власності Балансоутримувача" Value="3" />
                                            <dx:ListEditItem Text="Сфера Діяльності Балансоутримувача" Value="4" />
                                        </Items>
                                    </dx:ASPxListBox>

                                    <br/>

                                    <dx:ASPxLabel ID="LabelObjectsUseFields2" runat="server" Text="Додаткові колонки"></dx:ASPxLabel>

                                    <dx:ASPxListBox ID="ListBoxObjectsUseFields" ClientInstanceName="ListBoxObjectsUseFields" runat="server"
                                        TextField="name" ValueField="id" ValueType="System.String"
                                        Width="300px" Height="200px" SelectionMode="CheckColumn" >
                                    </dx:ASPxListBox>

                                </dx:panelcontent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                        <br/>

                        <dx:ASPxButton ID="ButtonObjectsUseFieldsApply" runat="server" AutoPostBack="False" Text="Вибрати" Width="120px">
                            <ClientSideEvents Click="ButtonObjectsUseFieldsApplyClick" />
                        </dx:ASPxButton>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents PopUp="function (s, e) { CPObjectsUseFields.PerformCallback(''); }" />
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_ObjectsUse_Fields" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="148px"></dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl6" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_RentNotSubmitted_SaveAs" 
                PopupElementID="ASPxButton_ObjectsUse_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                        <dx:ASPxButton ID="ASPxButton22" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_ObjectsUse_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton28" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_ObjectsUse_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton29" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_ObjectsUse_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_ObjectsUse_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewRentObjectsUseExporter" runat="server" 
    FileName="ВикористанняНежФонду" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    EnableRowsCache="False"
    Width="100%"
    DataSourceID="SqlDataSourceRentObjectsUse"
    KeyFieldName="organization_id"
    OnCustomCallback="GridViewRentObjectsUse_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewRentObjectsUse_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewRentObjectsUse_ProcessColumnAutoFilter"
    OnCustomUnboundColumnData="GridViewRentObjectsUse_CustomUnboundColumnData" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="0" Visible="True" Caption="Повна Назва Балансоутримувача" Width="250px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("organization_id"), Eval("full_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="False" Caption="Коротка Назва Балансоутримувача" Width="250px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("organization_id"), Eval("short_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="True" Caption="Код ЄДРПОУ Балансоутримувача"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="True" Caption="Форма Власності Балансоутримувача"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Caption="Сфера Діяльності Балансоутримувача"></dx:GridViewDataTextColumn>
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
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
    <SettingsCookies CookiesID="GUKV.OrendnaPlata.ObjectsUse" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewRentObjectsUseInit" EndCallback="GridViewRentObjectsUseEndCallback" />
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
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
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
