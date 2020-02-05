<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PrivatizationView.aspx.cs" Inherits="Privatization_PrivatizationView" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v12.1, Version=12.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v12.1, Version=12.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register src="../UserControls/GoogleMapsSnapIn.ascx" tagname="GoogleMapsSnapIn" tagprefix="uc2" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v12.1.Export, Version=12.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript" language="javascript">

    // <![CDATA[
    var ParentDocGridAdjustRequired = true;
    var ObjectsToAddGridAdjustRequired = true;
    var ObjectGridSelRowIndex = -1;

    function ParentDocDropDownHandler(s, e) {

        if (ParentDocGridAdjustRequired) {

            GridViewParentDocuments.AdjustControl();
        }

        ParentDocGridAdjustRequired = false;
    }

    function ParentDocGridRowClickHandler(s, e) {

        DropDownParentDoc.SetKeyValue(GridViewParentDocuments.cpDocIds[e.visibleIndex]);
        DropDownParentDoc.SetText(GridViewParentDocuments.cpDocDisplayNames[e.visibleIndex]);
        DropDownParentDoc.HideDropDown();
    }

    function ObjectToAddDropDownHandler(s, e) {

        if (ObjectsToAddGridAdjustRequired) {

            GridViewObjectsToAdd.AdjustControl();
        }

        ObjectsToAddGridAdjustRequired = false;
    }

    function ObjectsToAddGridRowClickHandler(s, e) {

        DropDownObjectToAdd.SetKeyValue(GridViewObjectsToAdd.cpBalansIds[e.visibleIndex]);
        DropDownObjectToAdd.SetText(GridViewObjectsToAdd.cpBalansDisplayNames[e.visibleIndex]);
        DropDownObjectToAdd.HideDropDown();
    }

    function ObjectGridRowClickHandler(s, e) {

        // Unselect all rows
        GridViewObjects._selectAllRowsOnPage(false);

        // Select the row
        GridViewObjects.SelectRow(e.visibleIndex, true);

        // Save the selected row
        ObjectGridSelRowIndex = e.visibleIndex;

        // Display the address at google maps
        GridViewObjects.GetRowValues(e.visibleIndex, "street;number;organization;sqr_balans;geo_lat;geo_lng", OnGetObjectGridRowValues);
    }

    function OnGetObjectGridRowValues(values) {

        // This function is a callback invoked from ObjectGridRowClickHandler -> GridViewObjects.GetRowValues()

        var street = "" + values[0];
        var number = "" + values[1];
        var organization = "" + values[2];
        var sqr_balans = "" + values[3];
        var geo_lat = values[4];
        var geo_lng = values[5];

        if (geo_lat > 0 && geo_lng > 0) {
            ShowGeolocation(geo_lat, geo_lng, "" + street + " " + number,
                "<strong>" + street + " " + number + "</strong><br /><br />"
                + "Балансоутримувач:<br /><strong>" + organization + "</strong><br />"
                + "Площа на балансі: <strong>" + sqr_balans + "</strong> кв.м.");
        }
    }

    function AddObjectButtonClickHandler(s, e) {

        // Send a callback to the Grid to insert the row
        GridViewObjects.PerformCallback("insert");
    }

    function RemoveObjectButtonClickHandler(s, e) {

        // Send a callback to the Grid to delete the row
        GridViewObjects.PerformCallback("delete: " + ObjectGridSelRowIndex);
    }

    function ShowAllObjectsButtonClickHandler(s, e) {
        var allCoordinates = $.parseJSON(GridViewObjects.cpAllCoordinates);
        var center = $.parseJSON(GridViewObjects.cpAllCoordinatesCenter);

        ShowMultipleLocations(center, allCoordinates);
    }

    function ButtonPrivatizationFieldChooserClick(s, e) {

        if (GridViewPrivatization.IsCustomizationWindowVisible())
            GridViewPrivatization.HideCustomizationWindow();
        else
            GridViewPrivatization.ShowCustomizationWindow();
    }

    ///////////////////////////////////////////////////////////////////////////
    // Timer support for the drop-down of parent documents

    function ParentDocTimerTick() {

        if (DropDownParentDoc.GetText() != '') {
            GridViewParentDocuments.PerformCallback(DropDownParentDoc.GetText());
        }

        TimerParentDoc.SetEnabled(false);
    }

    function ParentDocKeyDown() {

        TimerParentDoc.SetEnabled(false);
    }

    function ParentDocKeyUp() {

        TimerParentDoc.SetEnabled(true);
    }

    ///////////////////////////////////////////////////////////////////////////
    // Timer support for the drop-down of objects

    function ObjectToAddTimerTick() {

        if (DropDownObjectToAdd.GetText() != '') {
            GridViewObjectsToAdd.PerformCallback(DropDownObjectToAdd.GetText());
        }

        TimerObjectToAdd.SetEnabled(false);
    }

    function ObjectToAddKeyDown() {

        TimerObjectToAdd.SetEnabled(false);
    }

    function ObjectToAddKeyUp() {

        TimerObjectToAdd.SetEnabled(true);
    }

    // ]]>

</script>

<asp:SqlDataSource ID="SqlDataSourceGUKVDocTypes" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_doc_kind">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceGUKVAllDocs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, kind_id, general_kind_id, doc_date, doc_num, topic, search_name FROM documents">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceGUKVViewBalans" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT balans_id, org_short_name, sqr_balans, district, street_full_name, addr_nomer, sqr_total,
                   object_name, object_group, object_kind FROM view_org_balans_privat WHERE sqr_balans <> 0">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceViewPrivatization" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT link_id, master_doc_id, slave_doc_id,
                   parent_num, parent_date, parent_topic, child_num, child_date, child_topic
                   FROM view_privatization_doc_links">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceViewPrivatDetail" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    onselecting="SqlDataSourceViewPrivatDetail_Selecting" 
    SelectCommand="SELECT obj_link_id, privatization_id, building_id, organization_id, slave_doc_id, master_doc_id,
                   district, street_full_name, addr_nomer, org_name, sqr_total, obj_group, privat_kind, privat_state
                   FROM view_privatization_doc_objects WHERE (master_doc_id = @p_master_doc_id) AND (slave_doc_id = @p_slave_doc_id)">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_master_doc_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_slave_doc_id" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataGUKVObjectGroups" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT DISTINCT name FROM dict_privat_obj_group">
</asp:SqlDataSource>

<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1">
            <TabPages>
                <dx:TabPage Text="Об'єкти">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl2" runat="server">

<%-- Content of the first tab BEGIN --%>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <h1 style="padding: 0; margin: 0;">Загальна інформація по приватизації</h1>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" 
                Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ButtonPrivatizationFieldChooserClick" />
            </dx:ASPxButton>
        </td>
        <td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_Privatization_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Privatization_SaveAs" 
                PopupElementID="ASPxButton_Privatization_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton_Privatization_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Privatization_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Privatization_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Privatization_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Privatization_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Privatization_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Privatization_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewPrivatizationExporter" runat="server" 
    FileName="Приватизація" GridViewID="GridViewPrivatization" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView
    ID="GridViewPrivatization"
    ClientInstanceName="GridViewPrivatization"
    runat="server"
    AutoGenerateColumns="False"
    Width="1200px"
    DataSourceID="SqlDataSourceViewPrivatization"
    KeyFieldName="link_id">

    <GroupSummary><dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" /></GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn Caption="Головний документ: ID" FieldName="master_doc_id" ShowInCustomizationForm="True" VisibleIndex="0" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Головний документ: номер" FieldName="parent_num" ShowInCustomizationForm="True" VisibleIndex="1"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn Caption="Головний документ: дата" FieldName="parent_date" ShowInCustomizationForm="True" VisibleIndex="2"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn Caption="Головний документ: назва" FieldName="parent_topic" ShowInCustomizationForm="True" VisibleIndex="3">
            <DataItemTemplate>
                <a href="DocTemplates/SampleDocument.doc"><%# Container.Text %></a>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Підпорядкований документ: ID" FieldName="slave_doc_id" ShowInCustomizationForm="True" VisibleIndex="4" Visible="False"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Підпорядкований документ: номер" FieldName="child_num" ShowInCustomizationForm="True" VisibleIndex="5"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn Caption="Підпорядкований документ: дата" FieldName="child_date" ShowInCustomizationForm="True" VisibleIndex="6"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn Caption="Підпорядкований документ: назва" FieldName="child_topic" ShowInCustomizationForm="True" VisibleIndex="7">
            <DataItemTemplate>
                <a href="DocTemplates/SampleDocument.doc"><%# Container.Text %></a>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
    </Columns>

    <Templates>
        <DetailRow>
            <dx:ASPxGridView ID="ASPxGridViewPrivatDetail" runat="server" AutoGenerateColumns="False" 
                DataSourceID="SqlDataSourceViewPrivatDetail" ClientInstanceName="ASPxGridViewPrivatDetail"
                Width="1140px"
                KeyFieldName="privatization_id"
                OnBeforePerformDataSelect="GridViewPrivatizationDetail_BeforePerformDataSelect">

                <Columns>
                    <dx:GridViewDataTextColumn Caption="Район" FieldName="district" ShowInCustomizationForm="True" VisibleIndex="0" Width="80px"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Назва вулиці" FieldName="street_full_name" ShowInCustomizationForm="True" VisibleIndex="1" Width="100px"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Номер будинку" FieldName="addr_nomer" ShowInCustomizationForm="True" VisibleIndex="2" Width="80px"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Організація" FieldName="org_name" ShowInCustomizationForm="True" VisibleIndex="3" Width="120px"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Площа (кв.м.)" FieldName="sqr_total" ShowInCustomizationForm="True" VisibleIndex="4" Width="80px"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Група об'єкту" FieldName="obj_group" ShowInCustomizationForm="True" VisibleIndex="5" Width="60px"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Спосіб приватизації" FieldName="privat_kind" ShowInCustomizationForm="True" VisibleIndex="6" Width="60px"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Стан" FieldName="privat_state" ShowInCustomizationForm="True" VisibleIndex="7" Width="80px"></dx:GridViewDataTextColumn>
                </Columns>

                <SettingsPager PageSize="10"></SettingsPager>
                <SettingsBehavior AutoFilterRowInputDelay="500" />
                <Styles Header-Wrap="True" />
                <SettingsCookies CookiesID="GUKV.Privatization.Detail" Enabled="True" Version="1" />
            </dx:ASPxGridView>

        </DetailRow>
    </Templates>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="500" ColumnResizeMode="Control" />
    <SettingsPager PageSize="18"></SettingsPager>
    <SettingsDetail ShowDetailRow="True" />
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="True"
        ShowFilterBar="Auto"
        ShowHeaderFilterButton="True"
        HorizontalScrollBarMode="Visible" />
    <SettingsCookies CookiesID="GUKV.privatization" Version="1" Enabled="True" />
    <Styles Header-Wrap="True" >
<Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<%-- Content of the first tab END --%>

                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>

                <dx:TabPage Text="Проект рішення">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl1" runat="server">

<%-- Content of the second tab BEGIN --%>

<h1 style="padding: 0; margin: 0;">Приватизація: Підготовка проекту рішення</h1>

<p></p>

<table border="0" cellspacing="0" cellpadding="0">

<tr>
<td valign="top">

<asp:Panel ID="PanelPrivAddDocMain" runat="server" Width="800px">

    <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" Width="100%" HeaderText="Статус проекту рішення">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">

                <table border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="LabelWorkflowStatus" runat="server" Text="Статус"></dx:ASPxLabel>
                        </td>
                        <td width="16px"></td>
                        <td>
                            <dx:ASPxLabel ID="LabelWorkflowStartDate" runat="server" Text="Дата створення"></dx:ASPxLabel>
                        </td>
                        <td width="16px"></td>
                        <td>
                            <dx:ASPxLabel ID="LabelWorkflowEditDate" runat="server" Text="Дата останнього редагування"></dx:ASPxLabel>
                        </td>
                        <td width="16px"></td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="ComboWorkflowStatus" runat="server" Width="200px" 
                                ValueType="System.String" SelectedIndex="0">
                                <Items>
                                    <dx:ListEditItem Selected="True" Text="Новий" Value="Новий" />
                                    <dx:ListEditItem Text="На розгляді" Value="На розгляді" />
                                    <dx:ListEditItem Text="Потребує допрацювання" Value="Потребує допрацювання" />
                                    <dx:ListEditItem Text="Затверджено" Value="Затверджено" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>
                        <td width="16px"></td>
                        <td>
                            <dx:ASPxDateEdit ID="EditWorkfowStartDate" runat="server"></dx:ASPxDateEdit>
                        </td>
                        <td width="16px"></td>
                        <td>
                            <dx:ASPxDateEdit ID="EditWorkfowEditDate" runat="server"></dx:ASPxDateEdit>
                        </td>
                        <td width="16px"></td>
                        <td>
                            <td><dx:ASPxButton ID="ASPxButton1" runat="server" Text="Затверджено"></dx:ASPxButton></td>
                        </td>
                    </tr>
                </table>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

    <br />

    <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Width="100%" 
        HeaderText="Загальні характеристики документу">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="LabelDocNum" runat="server" Text="Введіть номер документу"></dx:ASPxLabel>
                        </td>
                        <td width="16px"></td>
                        <td>
                            <dx:ASPxLabel ID="LabelDocDate" runat="server" Text="Виберіть дату документу"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxTextBox ID="EditDocNum" runat="server" Width="200px"></dx:ASPxTextBox>
                        </td>
                        <td width="16px"></td>
                        <td>
                            <dx:ASPxDateEdit ID="EditDocDate" runat="server"></dx:ASPxDateEdit>
                        </td>
                    </tr>
                </table>

                <br/>

                <dx:ASPxLabel ID="LabelDocName" runat="server" Text="Введіть назву документу"></dx:ASPxLabel>

                <dx:ASPxTextBox ID="EditDocName" runat="server" Width="100%"></dx:ASPxTextBox>

                <br/>

                <dx:ASPxLabel ID="LabelParentDoc" runat="server" Text="Виберіть головний документ"></dx:ASPxLabel>

                <dx:ASPxTimer ID="TimerParentDoc" runat="server" ClientInstanceName="TimerParentDoc" Interval="500" Enabled="False">
                    <ClientSideEvents tick="function(s, e) { ParentDocTimerTick(); }" />
                </dx:ASPxTimer>

                <dx:ASPxDropDownEdit ID="DropDownParentDoc" runat="server" ClientInstanceName="DropDownParentDoc" 
                    AllowUserInput="true" EnableAnimation="False" Width="100%">

                    <DropDownWindowStyle> <Border BorderWidth="0px" />  </DropDownWindowStyle>

                    <DropDownWindowTemplate>
                        <dx:ASPxGridView
                            ID="GridViewParentDocuments"
                            runat="server"
                            AutoGenerateColumns="False"
                            ClientInstanceName="GridViewParentDocuments"
                            Width="100%"
                            DataSourceID="SqlDataSourceGUKVAllDocs"
                            KeyFieldName="id"
                            OnCustomJSProperties="GridViewParentDocuments_CustomJSProperties"
                            OnCustomCallback="GridViewParentDocuments_CustomCallback" >

                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="id" VisibleIndex="0" Visible="False"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="doc_num" Caption = "Номер документу" VisibleIndex="0" Width="80px"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn FieldName="doc_date" Caption = "Дата документу" VisibleIndex="1" Width="80px"></dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn FieldName="topic" Caption = "Назва документу" VisibleIndex="2"></dx:GridViewDataTextColumn>
                            </Columns>

                            <ClientSideEvents
                                 RowClick="ParentDocGridRowClickHandler"
                                 EndCallback="function(s,e) { DropDownParentDoc.ShowDropDown(); }" >
                            </ClientSideEvents>

                            <SettingsPager PageSize="10"></SettingsPager>
                            <Settings
                                ShowFilterRow="True"
                                ShowFilterRowMenu="True"
                                VerticalScrollBarMode="Visible"
                                VerticalScrollBarStyle="Virtual" />
                            <SettingsBehavior AutoFilterRowInputDelay="500" />
                            <Styles Header-Wrap="True" >
                                <Header Wrap="True"></Header>
                            </Styles>
                        </dx:ASPxGridView>
                    </DropDownWindowTemplate>

                    <ClientSideEvents
                        DropDown="ParentDocDropDownHandler"
                        KeyDown="function(s, e) { ParentDocKeyDown(); }"
                        KeyUp="function(s, e) { ParentDocKeyUp(); }" >
                    </ClientSideEvents>
                </dx:ASPxDropDownEdit>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

    <br />

    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" 
        HeaderText="Об'єкти, що відносяться до рішення">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">

                <dx:ASPxGridView ID="GridViewObjects" runat="server" KeyFieldName="balans_id" 
                    AutoGenerateColumns="False" ClientInstanceName="GridViewObjects" Width="100%"
                    OnCustomCallback="GridViewObjects_CustomCallback" 
                    OnRowUpdating="GridViewObjects_RowUpdating">
                    <ClientSideEvents RowClick="ObjectGridRowClickHandler" />
                    <clientsideevents rowclick="ObjectGridRowClickHandler" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Код" FieldName="balans_id" 
                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="0" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Балансоутримувач" FieldName="organization" 
                            ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Назва району" FieldName="district" 
                            ShowInCustomizationForm="True" VisibleIndex="2" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Назва вулицi" FieldName="street" 
                            ShowInCustomizationForm="True" VisibleIndex="3" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Номер будинку" FieldName="number" 
                            ShowInCustomizationForm="True" VisibleIndex="4" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataComboBoxColumn FieldName="object_group" VisibleIndex="5" Caption="Група об'єкту"
                            ShowInCustomizationForm="True" >
                            <PropertiesComboBox DataSourceID="SqlDataGUKVObjectGroups" TextField="name" ValueField="name" ValueType="System.String">
                            </PropertiesComboBox>
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataTextColumn Caption="Площа на балансі (кв.м.)" FieldName="sqr_balans" 
                            ShowInCustomizationForm="True" VisibleIndex="6" ReadOnly="True">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Площа на приватизацію (кв.м.)" FieldName="sqr_privat" 
                            ShowInCustomizationForm="True" VisibleIndex="7">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Поверх" FieldName="floor" 
                            ShowInCustomizationForm="True" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataComboBoxColumn FieldName="privat_kind" VisibleIndex="9" Caption="Шлях приватизації"
                            ShowInCustomizationForm="True" >
                            <PropertiesComboBox>
                                <Items>
                                    <dx:ListEditItem Selected="True" Text="Викуп" Value="Викуп" />
                                    <dx:ListEditItem Text="Аукціон" Value="Аукціон" />
                                </Items>
                            </PropertiesComboBox>
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewCommandColumn ShowInCustomizationForm="False" VisibleIndex="10">
                            <EditButton visible="True"></EditButton>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn FieldName="geo_lat" ShowInCustomizationForm="True" 
                            Visible="False" VisibleIndex="11">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="geo_lng" ShowInCustomizationForm="True" 
                            Visible="False" VisibleIndex="12">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" 
                        HorizontalScrollBarMode="Visible" ShowFilterBar="Auto" />
                    <SettingsBehavior AutoFilterRowInputDelay="500" ColumnResizeMode="Control" />
                    <SettingsCookies CookiesID="GUKV.privatization.rish" Version="1" Enabled="True" />
                    <Styles Header-Wrap="True" >
                        <Header Wrap="True"></Header>
                    </Styles>
                </dx:ASPxGridView>

                <br />

                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td>
                            <dx:ASPxTimer ID="TimerObjectToAdd" runat="server" ClientInstanceName="TimerObjectToAdd" Interval="500" Enabled="False">
                                <ClientSideEvents tick="function(s, e) { ObjectToAddTimerTick(); }" />
                            </dx:ASPxTimer>

                            <dx:ASPxDropDownEdit ID="DropDownObjectToAdd" runat="server" 
                                AllowUserInput="True" ClientInstanceName="DropDownObjectToAdd" 
                                EnableAnimation="False" Width="420px">

                                <DropDownWindowTemplate>
                                    <dx:ASPxGridView ID="GridViewObjectsToAdd" runat="server" 
                                        AutoGenerateColumns="False" ClientInstanceName="GridViewObjectsToAdd" 
                                        DataSourceID="SqlDataSourceGUKVViewBalans" KeyFieldName="balans_id" 
                                        OnCustomJSProperties="GridViewObjectsToAdd_CustomJSProperties" Width="750px"
                                        OnCustomCallback="GridViewObjectsToAdd_CustomCallback">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="balans_id" Visible="False" VisibleIndex="0"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Балансоутримувач" FieldName="org_short_name" VisibleIndex="1" Width="220px"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Назва району" FieldName="district" VisibleIndex="2"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Назва вулицi" FieldName="street_full_name" VisibleIndex="3"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Номер будинку" FieldName="addr_nomer" VisibleIndex="4" Width="80px"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Група об'єкту" FieldName="object_group" VisibleIndex="5" Width="60px"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Площа на балансі (кв.м.)" FieldName="sqr_balans" VisibleIndex="6" Width="80px"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Назва об'єкту" FieldName="object_name" Visible="False" VisibleIndex="7"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Тип об'єкту" FieldName="object_kind" Visible="False" VisibleIndex="8"></dx:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents
                                             RowClick="ObjectsToAddGridRowClickHandler"
                                             EndCallback="function(s,e) { DropDownObjectToAdd.ShowDropDown(); }" >
                                        </ClientSideEvents>
                                        <SettingsPager PageSize="10"></SettingsPager>
                                        <Settings
                                            ShowFilterRow="True"
                                            ShowFilterRowMenu="True"
                                            VerticalScrollBarMode="Visible"
                                            VerticalScrollBarStyle="Virtual" />
                                        <SettingsBehavior AutoFilterRowInputDelay="500" />
                                        <Styles Header-Wrap="True" />
                                    </dx:ASPxGridView>
                                </DropDownWindowTemplate>

                                <ClientSideEvents
                                    DropDown="ObjectToAddDropDownHandler"
                                    KeyDown="function(s, e) { ObjectToAddKeyDown(); }"
                                    KeyUp="function(s, e) { ObjectToAddKeyUp(); }" >
                                </ClientSideEvents>

                                <dropdownwindowstyle>
                                    <Border BorderWidth="0px" />
                                </dropdownwindowstyle>
                            </dx:ASPxDropDownEdit>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="ComboPrivatizationKind" runat="server" 
                                ValueType="System.String" SelectedIndex="0" Width="90px">
                                <Items>
                                    <dx:ListEditItem Selected="True" Text="Викуп" Value="Викуп" />
                                    <dx:ListEditItem Text="Аукціон" Value="Аукціон" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ButtonAddDocObject" runat="server" Text="Додати об'єкт" AutoPostBack="False">
                                <ClientSideEvents Click="AddObjectButtonClickHandler" />
                            </dx:ASPxButton>
                        </td>
                        <td align="right">
                            <dx:ASPxButton ID="ButtonRemoveDocObject" runat="server" Text="Вилучити об'єкт" AutoPostBack="False">
                                <ClientSideEvents Click="RemoveObjectButtonClickHandler" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right" style="padding-top: 10px;">
                            <dx:ASPxButton ID="ButtonShowAllObjects" runat="server" Text="Показати всі об'єкти на карті" AutoPostBack="False">
                                <ClientSideEvents Click="ShowAllObjectsButtonClickHandler" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    
<p></p>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td><dx:ASPxButton ID="ButtonSaveRishen" runat="server" Text="Зберегти"></dx:ASPxButton></td>
        <td width="16px"></td>
        <td><dx:ASPxButton ID="ButtonCreateDocument" runat="server" Text="Сформувати проект рішення" OnClick="ButtonCreateDocument_Click"></dx:ASPxButton></td>
    </tr>
</table>

</asp:Panel>

</td>

<td width="32px">&nbsp;&nbsp;&nbsp;</td>

<td valign="top">

<dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="Розташування об'єкту">
    <PanelCollection>
        <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
            <uc2:GoogleMapsSnapIn ID="GoogleMapsSnapIn" runat="server" Language="uk" 
                Height="520px" 
                EmptyAddressMessage="Виберіть об'єкт з проекту рішення щоб подивитися його розташування на карті" 
                GeocodeErrorMessage="Geocode не була успішною з наступних причин" />
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>

</td>
</tr>

</table>

<%-- Content of the second tab END --%>

                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
</dx:ASPxPageControl>

</asp:Content>
