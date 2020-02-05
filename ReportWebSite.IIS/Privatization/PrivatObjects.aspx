<%@ page language="C#" autoeventwireup="true" inherits="Privatization_PrivatObjects, App_Web_privatobjects.aspx.65b2b859" masterpagefile="~/NoHeader.master" title="Об'єкти в Програмі Приватизації" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
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

        PrimaryGridView.SetHeight(window.innerHeight - 200);
    }

    function GridViewPrivObjectsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewPrivObjectsEndCallback(s, e) {

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

<mini:ProfiledSqlDataSource ID="SqlDataSourcePrivatizationObjects" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_privatization_objects">
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Privatization/PrivatObjects.aspx" Text="Об'єкти в Програмі Приватизації"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Privatization/PrivatDocList.aspx" Text="Розпорядчі Документи з Приватизації"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Об'єкти, включені до програми приватизації" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonQuickSearchAddr1" runat="server" AutoPostBack="False" Text="" ImageSpacing="0px" AllowFocus="false"
                ToolTip="Щвидкий пошук за адресою">
                <Image Url="../Styles/HouseIcon.png" />
                <FocusRectPaddings Padding="1px" />
                <ClientSideEvents Click="ShowAddressPickerPopupControl" />
            </dx:ASPxButton>
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
            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_PrivObjects_SaveAs" 
                PopupElementID="ASPxButton_PrivObjects_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton3" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_PrivObjects_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_PrivObjects_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_PrivObjects_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_PrivObjects_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewPrivObjectsExporter" runat="server" 
    FileName="Приватизація" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID="SqlDataSourcePrivatizationObjects"
    KeyFieldName="privatization_id"
    OnCustomCallback="GridViewPrivObjects_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewPrivObjects_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewPrivObjects_ProcessColumnAutoFilter"
    OnCustomColumnSort="GridViewPrivObjects_CustomColumnSort" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="building_id" ShowInCustomizationForm="False" Caption="ID Будинку"
            VisibleIndex="0" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="organization_id" ShowInCustomizationForm="False" Caption="ID Організації"
            VisibleIndex="1" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("organization_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans" ShowInCustomizationForm="True" Caption="Балансоутримувач"
            VisibleIndex="2" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_zkpo" ShowInCustomizationForm="True" Caption="Код ЄДРПОУ Балансоутримувача"
            VisibleIndex="3" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="subord_code" ShowInCustomizationForm="True" Caption="Сфера Управління"
            VisibleIndex="4" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="subordination" ShowInCustomizationForm="True" Caption="Назва Сфери Управління"
            VisibleIndex="5" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" ShowInCustomizationForm="True" Caption="Район"
            VisibleIndex="6" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ShowInCustomizationForm="True" Caption="Назва Вулиці"
            VisibleIndex="7" Visible="True" Width="120px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCardForPrivatization('" + Eval("balans_id") + "," + Eval("building_id") + "')\">" + Eval("street_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ShowInCustomizationForm="True" Caption="Номер Будинку"
            VisibleIndex="8" Visible="True" Width="80px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCardForPrivatization('" + Eval("balans_id") + "," + Eval("building_id") + "')\">" + Eval("addr_nomer") + "</a>"%>
            </DataItemTemplate>
            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_name" ShowInCustomizationForm="True" Caption="Назва Об'єкту"
            VisibleIndex="9" Visible="True"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_name" ShowInCustomizationForm="True" Caption="Організація"
            VisibleIndex="10" Visible="True" Width="180px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("org_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_address" ShowInCustomizationForm="True" Caption="Адреса Організації"
            VisibleIndex="11" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_fio" ShowInCustomizationForm="True" Caption="ПІБ Директора"
            VisibleIndex="12" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_phone" ShowInCustomizationForm="True" Caption="Тел. Директора"
            VisibleIndex="13" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total" ShowInCustomizationForm="True" Caption="Загальна Площа (кв.м.)"
            VisibleIndex="14" Visible="True" Width="70px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_group" ShowInCustomizationForm="True" Caption="Група Об'єкту"
            VisibleIndex="15" Visible="True" Width="70px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="privat_kind" ShowInCustomizationForm="True" Caption="Спосіб Приватизації"
            VisibleIndex="16" Visible="True"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="privat_state" ShowInCustomizationForm="True" Caption="Стан"
            VisibleIndex="17" Visible="True" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_group" ShowInCustomizationForm="True" Caption="Група Призначення"
            VisibleIndex="18" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_kind" ShowInCustomizationForm="True" Caption="Вид Об'єкту"
            VisibleIndex="19" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_type" ShowInCustomizationForm="True" Caption="Тип Об'єкту"
            VisibleIndex="20" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_floor" ShowInCustomizationForm="True" Caption="Поверх"
            VisibleIndex="21" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost" ShowInCustomizationForm="True" Caption="Вартість (грн.)"
            VisibleIndex="22" Visible="True"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert" ShowInCustomizationForm="True" Caption="Оцінка"
            VisibleIndex="23" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="expert_date" ShowInCustomizationForm="True" Caption="Дата Затвердження Оцінки"
            VisibleIndex="24" Visible="False"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="note" ShowInCustomizationForm="True" Caption="Примітка"
            VisibleIndex="25" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rishennya_id" ShowInCustomizationForm="False" Caption="ID Рішення"
            VisibleIndex="26" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rishennya_num" ShowInCustomizationForm="True" Caption="Рішення - Номер"
            VisibleIndex="27" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("rishennya_id") + ")\">" + Eval("rishennya_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="rishennya_date" ShowInCustomizationForm="True" Caption="Рішення - Дата"
            VisibleIndex="28" Visible="False"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="rishennya_year" ShowInCustomizationForm="True" Caption="Рішення - Рік"
            VisibleIndex="29" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rishennya_topic" ShowInCustomizationForm="True" Caption="Рішення - Назва"
            VisibleIndex="30" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("rishennya_id") + ")\">" + Eval("rishennya_topic") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rishennya_search_name" ShowInCustomizationForm="True" Caption="Рішення"
            VisibleIndex="31" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("rishennya_id") + ")\">" + Eval("rishennya_search_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_id" ShowInCustomizationForm="False" Caption="ID Договору"
            VisibleIndex="32" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_num" ShowInCustomizationForm="True" Caption="Договір - Номер"
            VisibleIndex="33" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("agreement_id") + ")\">" + Eval("agreement_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="agreement_date" ShowInCustomizationForm="True" Caption="Договір - Дата"
            VisibleIndex="34" Visible="False"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_year" ShowInCustomizationForm="True" Caption="Договір - Рік"
            VisibleIndex="35" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_topic" ShowInCustomizationForm="True" Caption="Договір - Назва"
            VisibleIndex="36" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("agreement_id") + ")\">" + Eval("agreement_topic") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_search_name" ShowInCustomizationForm="True" Caption="Договір"
            VisibleIndex="37" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("agreement_id") + ")\">" + Eval("agreement_search_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_id" ShowInCustomizationForm="False" Caption="ID Акту"
            VisibleIndex="38" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_num" ShowInCustomizationForm="True" Caption="Акт - Номер"
            VisibleIndex="39" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("akt_id") + ")\">" + Eval("akt_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="akt_date" ShowInCustomizationForm="True" Caption="Акт - Дата"
            VisibleIndex="40" Visible="False"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="akt_year" ShowInCustomizationForm="True" Caption="Акт - Рік"
            VisibleIndex="41" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_topic" ShowInCustomizationForm="True" Caption="Акт - Назва"
            VisibleIndex="42" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("akt_id") + ")\">" + Eval("akt_topic") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_search_name" ShowInCustomizationForm="True" Caption="Акт Приймання-Передачі"
            VisibleIndex="43" Visible="False">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("akt_id") + ")\">" + Eval("akt_search_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_history" ShowInCustomizationForm="True" Caption="Історична Цінність"
            VisibleIndex="44" Visible="False"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_expert" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="Загальна Площа = {0} кв.м." />
        <dx:ASPxSummaryItem FieldName="cost" SummaryType="Sum" DisplayFormat="Загальна Вартість = {0} грн." />
    </GroupSummary>

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
    <SettingsCookies CookiesID="GUKV.Privatization.Objects" Version="A2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewPrivObjectsInit" EndCallback="GridViewPrivObjectsEndCallback" />
</dx:ASPxGridView>

</center>

<iframe id="docTextFrame" src="" style="visibility:hidden" width="0px" height="0px"></iframe>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
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
        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
            <uc2:AddressPicker ID="AddressPicker2" runat="server"/>
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
        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>

