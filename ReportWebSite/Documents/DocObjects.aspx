<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocObjects.aspx.cs" Inherits="Documents_DocObjects" MasterPageFile="~/NoHeader.master" Title="Об'єкти за Документами" %>

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

    function GridViewDocObjectsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewDocObjectsEndCallback(s, e) {

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

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Documents/DocRelations.aspx" Text="Документи"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Documents/DocObjects.aspx" Text="Об'єкти за Документами"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle2" runat="server" Text="Документи та об'єкти" CssClass="reporttitle"></asp:Label>
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
            <dx:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Transfer_SaveAs" 
                PopupElementID="ASPxButton_DocObjects_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton_Transfer_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_DocObjects_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Transfer_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_DocObjects_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Transfer_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_DocObjects_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_DocObjects_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocObjects" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM m_view_docs_objects_akts">
</mini:ProfiledSqlDataSource>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterDocObjects" runat="server" 
    FileName="ОбєктиЗаДокументами" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID="SqlDataSourceDocObjects" 
    KeyFieldName="link_id;document_id"
    Width="100%" 
    OnCustomCallback="GridViewDocObjects_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewDocObjects_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewDocObjects_ProcessColumnAutoFilter"
    OnCustomColumnSort="GridViewDocObjects_CustomColumnSort" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="document_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="False" Caption="ID Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="doc_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="False" Caption="Вид Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="general_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="False" Caption="Узагальнений Вид Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="doc_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="True" Caption="Номер Документу">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("document_id") + ")\">" + Eval("doc_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="doc_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Caption="Дата Документу"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="doc_date_year" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="False" Caption="Дата Документу - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="doc_date_quarter" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="False" Caption="Дата Документу - Квартал"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="topic" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="True" Caption="Назва Документу" Width="195px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("document_id") + ")\">" + Eval("topic") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="doc_note" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="False" Caption="Примітка Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="search_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="False" Caption="Документ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="receive_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="False" Caption="Дата Отримання Документу"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="commission_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Коміссія"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="doc_source" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="False" Caption="Джерело Походження Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="doc_state" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="13" Visible="False" Caption="Стан Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="summa" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="False" Caption="Сума За Документом (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="summa_zalishkova" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="15" Visible="False" Caption="Залишкова Сума За Документом (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="building_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="16" Visible="False" Caption="ID Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="17" Visible="False" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="18" Visible="True" Caption="Назва Вулиці" Width="150px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCardSimple(" + Eval("building_id") + ")\">" + Eval("street_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="True" Caption="Номер Будинку">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCardSimple(" + Eval("building_id") + ")\">" + Eval("addr_nomer") + "</a>"%>
            </DataItemTemplate>
            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="True" Caption="Назва Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_description" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="Характеристика"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_build_year" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="False" Caption="Рік Побудови"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_length" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="False" Caption="Довжина Об'єкту (м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_note" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="False" Caption="Примітка Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_balans" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="25" Visible="False" Caption="Балансова Вартість (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_znos" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="26" Visible="False" Caption="Знос (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_zalishkova" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="27" Visible="False" Caption="Залишкова Вартість (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="28" Visible="False" Caption="Оціночна Вартість (тис.грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_rooms" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="29" Visible="False" Caption="Кількість Кімнат"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_floors" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="30" Visible="False" Caption="Кількість Поверхів"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_location" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="31" Visible="False" Caption="Розташування Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_group" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="32" Visible="False" Caption="Група Призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="33" Visible="False" Caption="Призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="condition" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="34" Visible="False" Caption="Технічний Стан"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="35" Visible="True" Caption="Вид Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_type" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="36" Visible="True" Caption="Тип Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_obj" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="37" Visible="False" Caption="Загальна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="38" Visible="False" Caption="Вільна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_habit" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="39" Visible="False" Caption="Житлова Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_non_habit" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="40" Visible="False" Caption="Нежитлова Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="41" Visible="False" Caption="Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="pipe_diameter" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="42" Visible="False" Caption="Діаметер Труб (мм.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="pipe_material" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="43" Visible="False" Caption="Матеріал Труб"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_exists" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="44" Visible="False" Caption="Акт Існує"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="45" Visible="False" Caption="Акт - ID"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="46" Visible="False" Caption="Акт - Номер">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("akt_id") + ")\">" + Eval("akt_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="akt_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="47" Visible="False" Caption="Акт - Дата"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="akt_date_year" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="48" Visible="False" Caption="Акт - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_date_quarter" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="49" Visible="False" Caption="Акт - Квартал"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_topic" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="50" Visible="False" Caption="Акт - Назва" Width="195px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("akt_id") + ")\">" + Eval("akt_topic") + "</a>"%>
            </DataItemTemplate>
         </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_search_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="51" Visible="False" Caption="Акт Приймання-Передачі"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="doc_is_rozp" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="52" Visible="False" Caption="Розпорядчий Документ"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="summa" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="summa_zalishkova" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="obj_length" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_balans" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_znos" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_zalishkova" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_expert" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_obj" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_habit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_non_habit" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
        <dx:ASPxSummaryItem FieldName="sqr_obj" SummaryType="Sum" DisplayFormat="Загальна Площа = {0} кв.м." />
        <dx:ASPxSummaryItem FieldName="cost_balans" SummaryType="Sum" DisplayFormat="Балансова Вартість = {0} тис.грн." />
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
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.Documents" Version="A2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewDocObjectsInit" EndCallback="GridViewDocObjectsEndCallback" />
</dx:ASPxGridView>

</center>

<iframe id="docTextFrame" src="" style="visibility:hidden" width="0px" height="0px"></iframe>

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
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
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

