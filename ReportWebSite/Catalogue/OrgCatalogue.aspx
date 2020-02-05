<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgCatalogue.aspx.cs" Inherits="Catalogue_OrgCatalogue" MasterPageFile="~/NoHeader.master" Title="Каталог Організацій" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register src="../UserControls/SaveReportCtrl.ascx" tagname="SaveReportCtrl" tagprefix="uc1" %>
<%@ Register src="../UserControls/AddressPicker.ascx" tagname="AddressPicker" tagprefix="uc2" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register src="../UserControls/FieldFixxer.ascx" tagname="FieldFixxer" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function ShowFieldFixxerPopupControl(s, e) { PopupFieldFixxer.Show(); }

    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 185);
    }

    function GridViewAllOrganizationsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewAllOrganizationsEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowFoldersPopupControl(s, e) {

        PopupControlFolders.Show();
    }

    function ShowAddressPickerPopupControl(s, e) {

        PopupAddressPicker.Show();
    }

    function ShowFieldChooserPopupControl(s, e) {

        PopupFieldChooser.Show();
    }

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAllOrganizations" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_organizations WHERE ((org_deleted IS NULL) OR (org_deleted = 0)) AND LEN(COALESCE(full_name, '')) > 0 AND (@p_rda_district_id = 0 OR (form_of_ownership_int in (select id from dict_org_ownership where is_rda = 1) AND addr_distr_new_id = @p_rda_district_id) )"
    OnSelecting="SqlDataSourceAllOrganizations_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Catalogue/AddrCatalogue.aspx" Text="Каталог Адрес"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Catalogue/OrgCatalogue.aspx" Text="Каталог Організацій"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<%-- Content of the second tab BEGIN --%>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle2" runat="server" Text="Повний перелік організацій" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup2" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
		<td>
			<dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px">
				<ClientSideEvents Click="ShowFieldFixxerPopupControl" />
			</dx:ASPxButton>
		</td>
        <td>
            <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_AllOrganizations_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_AllOrganizations_SaveAs" 
                PopupElementID="ASPxButton_AllOrganizations_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton_AllOrganizations_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_AllOrganizations_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_AllOrganizations_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_AllOrganizations_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_AllOrganizations_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_AllOrganizations_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_AllOrganizations_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewAllOrganizationsExporter" runat="server" 
    FileName="ПерелікОрганізацій" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID="SqlDataSourceAllOrganizations"
    KeyFieldName="organization_id"
    OnCustomCallback="GridViewAllOrganizations_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewAllOrganizations_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewAllOrganizations_ProcessColumnAutoFilter" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="0" Visible="True" Caption="Повна назва" Width="250px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="False" Caption="Коротка Назва" Width="250px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="True" Caption="Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="True" Caption="Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Caption="Вид Діяльності" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="False" Caption="Галузь (Баланс)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="False" Caption="Вид Діяльності (Баланс)" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_gosp" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="False" Caption="Форма фінансування"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_of_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="False" Caption="Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="gosp_struct" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="False" Caption="Госп. Структура"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="False" Caption="Орган управління"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_form" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Орг.-правова форма госп."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_organ" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="False" Caption="Орган госп. упр."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="status" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="13" Visible="False" Caption="Фіз. / Юр. Особа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_district" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="True" Caption="Район" Width="150px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="15" Visible="True" Caption="Назва Вулиці" Width="200px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="16" Visible="True" Caption="Номер Будинку"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_korpus" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="17" Visible="False" Caption="Номер Будинку - Корпус"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_zip_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="False" Caption="Поштовий Індекс"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_fio" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="False" Caption="ФІО Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_phone" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="False" Caption="Тел. Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_fio" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="ФІО Бухгалтера"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_phone" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="False" Caption="Тел. Бухгалтера"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="fax" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="False" Caption="Факс"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_auth" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="False" Caption="Реєстраційний Орган"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="25" Visible="False" Caption="Номер Запису про Реєстрацію"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="26" Visible="False" Caption="Дата Реєстрації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_svidot" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="27" Visible="False" Caption="Номер Свідоцтва про Реєстрацію"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="kved_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="28" Visible="False" Caption="КВЕД"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="koatuu" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="29" Visible="False" Caption="ОАТУУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="30" Visible="False" Caption="Відділ ДКВ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="mayno" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="31" Visible="False" Caption="Правовий режим майна"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_liquidated" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="32" Visible="False" Caption="Ліквідовано"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="liquidation_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="33" Visible="False" Caption="Дата Ліквідації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_email" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="34" Visible="False" Caption="Ел. Адреса"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_posada" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="35" Visible="False" Caption="Контактна Особа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sfera_upr" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="36" Visible="False" Caption="Сфера Управління"></dx:GridViewDataTextColumn>
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
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.Catalogue.Organizations" Version="A2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewAllOrganizationsInit" EndCallback="GridViewAllOrganizationsEndCallback" />
</dx:ASPxGridView>

<%-- Content of the second tab END --%>

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

