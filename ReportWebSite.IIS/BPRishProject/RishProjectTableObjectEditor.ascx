<%@ control language="C#" autoeventwireup="true" inherits="RishProjectTableObjectEditor, App_Web_rishprojecttableobjecteditor.ascx.23e82b75" %>

<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStreets" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_streets where (not name is null) and (RTRIM(LTRIM(name)) <> '')">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictBuildings" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, LTRIM(RTRIM(addr_nomer)) AS 'nomer' from buildings where
        (is_deleted IS NULL OR is_deleted = 0) AND
        (master_building_id IS NULL) AND
        addr_street_id = @street_id AND
        (RTRIM(LTRIM(addr_nomer)) <> '') ORDER BY RTRIM(LTRIM(addr_nomer))"
    OnSelecting="SqlDataSourceDictBuildings_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="street_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAddressByBuilding" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    
    SelectCommand="select addr_street_id from buildings where id = @buildingID" 
    onselecting="SqlDataSourceAddressByBuilding_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="buildingID" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansSearch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT balans_id, org_full_name, sqr_total, COALESCE(balans_obj_name, purpose) AS 'purpose' FROM view_balans WHERE building_id = @bid"
    OnSelecting="SqlDataSourceBalansSearch_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansByID" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select balans.building_id, buildings.addr_street_id from balans inner join buildings on buildings.id = balans.building_id where balans.id = @balansID"
    OnSelecting="SqlDataSourceBalansByID_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="balansID" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearch2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 20 id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations WHERE zkpo_code LIKE @zkpo AND full_name LIKE @fname AND
        (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL"
    OnSelecting="SqlDataSourceOrgSearch2_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictDistricts2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT [id], [name] FROM [dict_1nf_districts2] WHERE len(name) > 0 ORDER BY [name]" 
    EnableCaching="True">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictObjectKind" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_object_kind WHERE len(name) > 0 ORDER BY name"
    EnableCaching="True">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictObjectType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_object_type WHERE len(name) > 0 ORDER BY name"
    EnableCaching="True">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgOwnership" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_ownership WHERE len(name) > 0 ORDER BY name"
    EnableCaching="True">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictBalansPurposeGroup" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_balans_purpose_group WHERE len(name) > 0 ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictBalansPurpose" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_balans_purpose WHERE len(name) > 0 ORDER BY name"
    EnableCaching="true">
</mini:ProfiledSqlDataSource>

<dx:ASPxCallbackPanel ID="CPObjectEditor" ClientInstanceName="CPObjectEditor" runat="server" OnCallback="CPObjectEditor_Callback">
    <ClientSideEvents EndCallback="function(s, e) {
	if (typeof window.CPObjectEditorCallbackType == 'string') {
        if (window.CPObjectEditorCallbackType == 'createbuilding') {
            if (!LabelBuildingCreationError.GetVisible()) {
                PopupAddBuilding.Hide();
            }
        }
        else if (window.CPObjectEditorCallbackType == 'createbalans') {
            if (!LabelBalansCreationError.GetVisible()) {
                PopupAddBalansObj.Hide();
            }
        }
    }
}" />
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent3" runat="server">

<div style="padding: 20px;">

    <table border="0" cellspacing="0" cellpadding="2">
        <tr>
            <td> <dx:ASPxLabel ID="LabelAddrStreet" runat="server" Text="Назва вулиці:" Width="80px" /> </td>
            <td>
                <dx:ASPxComboBox ID="ComboBalansStreet" runat="server" ClientInstanceName="ComboBalansStreet"
                    DataSourceID="SqlDataSourceDictStreets" DropDownStyle="DropDownList" ValueType="System.Int32"
                    TextField="name" ValueField="id" Width="220px" IncrementalFilteringMode="StartsWith"
                    FilterMinLength="3" EnableCallbackMode="True" CallbackPageSize="50" EnableViewState="False"
                    EnableSynchronization="False">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) { ComboBalansBuilding.PerformCallback(ComboBalansStreet.GetValue().toString()); }" />
                </dx:ASPxComboBox>
            </td>
            <td> <dx:ASPxLabel ID="LabelAddrPickerNumber" runat="server" Text="Номер будинку:" Width="95px"/> </td>
            <td>
                <dx:ASPxComboBox runat="server" ID="ComboBalansBuilding" ClientInstanceName="ComboBalansBuilding"
                    DataSourceID="SqlDataSourceDictBuildings" DropDownStyle="DropDownList" TextField="nomer"
                    ValueField="id" ValueType="System.Int32" Width="100px" IncrementalFilteringMode="StartsWith"
                    EnableSynchronization="False" OnCallback="ComboAddressBuilding_Callback">
                    <ClientSideEvents SelectedIndexChanged="function (s,e) { GridViewBalansObjects.PerformCallback(ComboBalansBuilding.GetValue().toString()); }" />
                </dx:ASPxComboBox>
            </td>
            <td>
                <dx:ASPxPopupControl ID="PopupAddBuilding" runat="server" ClientInstanceName="PopupAddBuilding"
                    HeaderText="Створення нової адреси" PopupElementID="ButtonAddBuilding">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">

                            <table border="0" cellspacing="0" cellpadding="2">
                                <tr>
                                    <td> <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Район:" Width="110px" /> </td>
                                    <td colspan="3"> <dx:ASPxComboBox ID="ComboBoxDistrict" runat="server" ClientInstanceName="ComboBoxDistrict" 
                                        ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                            IncrementalFilteringMode="StartsWith" 
                                            DataSourceID="SqlDataSourceDictDistricts2" /> </td>
                                </tr>
                                <tr>
                                    <td> <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Номер Будинку:" Width="110px" /> </td>
                                    <td> <dx:ASPxTextBox ID="TextBoxNumber1" ClientInstanceName="TextBoxNumber1" runat="server" Width="100px" /> </td>
                                    <td> <dx:ASPxTextBox ID="TextBoxNumber2" ClientInstanceName="TextBoxNumber2" runat="server" Width="100px" /> </td>
                                    <td> <dx:ASPxTextBox ID="TextBoxNumber3" ClientInstanceName="TextBoxNumber3" runat="server" Width="100px" /> </td>
                                </tr>
                                <tr>
                                    <td> <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Додаткова Адреса:" Width="110px" /> </td>
                                    <td colspan="3"> <dx:ASPxTextBox ID="TextBoxMiscAddr" ClientInstanceName="TextBoxMiscAddr" runat="server" Width="100%" /> </td>
                                </tr>
                                <tr>
                                    <td> <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Вид Об'єкту:" Width="110px" /> </td>
                                    <td colspan="3"> <dx:ASPxComboBox ID="ComboBoxBuildingKind" runat="server" ClientInstanceName="ComboBoxBuildingKind" 
                                        ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                            IncrementalFilteringMode="StartsWith" 
                                            DataSourceID="SqlDataSourceDictObjectKind" /> </td>
                                </tr>
                                <tr>
                                    <td> <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Тип Об'єкту:" Width="110px" /> </td>
                                    <td colspan="3"> <dx:ASPxComboBox ID="ComboBoxBuildingType" runat="server" ClientInstanceName="ComboBoxBuildingType" 
                                        ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                            IncrementalFilteringMode="StartsWith" 
                                            DataSourceID="SqlDataSourceDictObjectType" /> </td>
                                </tr>
                            </table>

                            <br/>
                            <dx:ASPxLabel ID="LabelBuildingCreationError" ClientInstanceName="LabelBuildingCreationError" runat="server" Text="" ClientVisible="false" ForeColor="Red" />
                            <br />

                            <dx:ASPxButton ID="ButtonDoAddBuilding" ClientInstanceName="ButtonDoAddBuilding" runat="server" AutoPostBack="False" Text="Створити">
                                <ClientSideEvents Click="function (s, e) {
    window.CPObjectEditorCallbackType = 'createbuilding';
    CPObjectEditor.PerformCallback('createbuilding:');
}" />
                            </dx:ASPxButton>

                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>

                <dx:ASPxButton ID="ButtonAddBuilding" ClientInstanceName="ButtonAddBuilding" runat="server" AutoPostBack="False" Text="Створити">
                    <ClientSideEvents Click="function (s, e) { }" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    
    <br />

    <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Об'єкт:" />

    <dx:ASPxGridView ID="GridViewBalansObjects" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="GridViewBalansObjects" Width="100%" DataSourceID="SqlDataSourceBalansSearch"
        KeyFieldName="balans_id" 
        OnCustomCallback="GridViewBalansObjects_CustomCallback" 
        ondatabound="GridViewBalansObjects_DataBound">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="org_full_name" Caption="Балансоутримувач" VisibleIndex="0"
                Width="200px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="sqr_total" Caption="Площа, кв.м." VisibleIndex="1"
                Width="100px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="purpose" Caption="Призначення" VisibleIndex="2"
                Width="200px">
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPager PageSize="5">
        </SettingsPager>
        <Settings ShowFilterRow="False" ShowFilterRowMenu="False" VerticalScrollBarMode="Visible"
            VerticalScrollBarStyle="Virtual" />
        <SettingsBehavior AutoFilterRowInputDelay="1500" AllowFocusedRow="true" />
        <Styles Header-Wrap="True">
            <Header Wrap="True">
            </Header>
        </Styles>
    </dx:ASPxGridView>

    <br />

    <dx:ASPxPopupControl ID="PopupAddBalansObj" runat="server" ClientInstanceName="PopupAddBalansObj"
        HeaderText="Створення нового об'єкту на балансі" PopupElementID="ButtonAddBalansObj">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">

                <table border="0" cellspacing="0" cellpadding="2">
                    <tr>
                        <td colspan="5">
                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Балансоутримувач:" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Код ЄДРПОУ:" Width="85px" />
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="EditOrgZKPO" ClientInstanceName="EditOrgZKPO" runat="server" Width="100px" />
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Назва:" Width="40px" />
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="EditOrgName" ClientInstanceName="EditOrgName" runat="server" Width="150px" />
                        </td>
                        <td>
                            <dx:ASPxButton ID="ButtonFindOrg" ClientInstanceName="ButtonFindOrg" runat="server" AutoPostBack="False" Text="Знайти">
                                <ClientSideEvents Click="function (s, e) { ComboBalansOrg.PerformCallback(EditOrgZKPO.GetText() + '|' + EditOrgName.GetText()); }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <dx:ASPxComboBox ID="ComboBalansOrg" ClientInstanceName="ComboBalansOrg" runat="server"
                                Width="100%" DataSourceID="SqlDataSourceOrgSearch2" ValueField="id" ValueType="System.Int32"
                                TextField="search_name" EnableSynchronization="True" OnCallback="ComboBalansOrg_Callback">
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                </table>

                <br/>

                <table border="0" cellspacing="0" cellpadding="2">
                    <tr>
                        <td> <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Площа Об'єкту:" Width="120px" /> </td>
                        <td> <dx:ASPxTextBox ID="EditBalansObjSquare" ClientInstanceName="EditBalansObjSquare" runat="server" Width="330px" /> </td>
                    </tr>
                    <tr>
                        <td> <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Балансова Вартість:" Width="120px" /> </td>
                        <td> <dx:ASPxTextBox ID="EditBalansObjCost" ClientInstanceName="EditBalansObjCost" runat="server" Width="330px" /> </td>
                    </tr>
                    <tr>
                        <td> <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Форма Власності:" Width="120px" /> </td>
                        <td> 
                            <dx:ASPxComboBox ID="ComboBoxBalansOwnership" runat="server" ClientInstanceName="ComboBoxBalansOwnership" 
                            ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                IncrementalFilteringMode="StartsWith" 
                                DataSourceID="SqlDataSourceDictOrgOwnership" /></td>
                    </tr>
                    <tr>
                        <td> <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Вид Об'єкту:" Width="120px" /> </td>
                        <td> <dx:ASPxComboBox ID="ComboBoxBalansObjKind" runat="server" ClientInstanceName="ComboBoxBalansObjKind" 
                            ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                IncrementalFilteringMode="StartsWith" 
                                DataSourceID="SqlDataSourceDictObjectKind" /></td>
                    </tr>
                    <tr>
                        <td> <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Тип Об'єкту:" Width="120px" /> </td>
                        <td> <dx:ASPxComboBox ID="ComboBoxBalansObjType" runat="server" ClientInstanceName="ComboBoxBalansObjType" 
                            ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                IncrementalFilteringMode="StartsWith" 
                                DataSourceID="SqlDataSourceDictObjectType" /></td>
                    </tr>
                    <tr>
                        <td> <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Група Призначення:" Width="120px" /> </td>
                        <td> 
                            <dx:ASPxComboBox ID="ComboBoxBalansPurposeGroup" runat="server" ClientInstanceName="ComboBoxBalansPurposeGroup" 
                            ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                IncrementalFilteringMode="StartsWith" 
                                DataSourceID="SqlDataSourceDictBalansPurposeGroup" /></td>
                    </tr>
                    <tr>
                        <td> <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Призначення:" Width="120px" /> </td>
                        <td> 
                            <dx:ASPxComboBox ID="ComboBoxBalansPurpose" runat="server" ClientInstanceName="ComboBoxBalansPurpose" 
                            ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                IncrementalFilteringMode="StartsWith" 
                                DataSourceID="SqlDataSourceDictBalansPurpose" /></td>
                    </tr>
                    <tr>
                        <td> <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Використання:" Width="120px" /> </td>
                        <td> <dx:ASPxTextBox ID="EditBalansObjUse" ClientInstanceName="EditBalansObjUse" runat="server" Width="330px" /> </td>
                    </tr>
                </table>

                <br/>
                <dx:ASPxLabel ID="LabelBalansCreationError" ClientInstanceName="LabelBalansCreationError" runat="server" Text="" ClientVisible="false" ForeColor="Red" />
                <br />

                <dx:ASPxButton ID="ButtonDoAddBalansObj" ClientInstanceName="ButtonDoAddBalansObj" runat="server" AutoPostBack="False" Text="Створити">
                    <ClientSideEvents Click="function (s, e) {
    window.CPObjectEditorCallbackType = 'createbalans';
    CPObjectEditor.PerformCallback('createbalans:');
}" />
                </dx:ASPxButton>
                
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxButton ID="ButtonAddBalansObj" ClientInstanceName="ButtonAddBalansObj" runat="server" AutoPostBack="False" Text="Створити Об'єкт">
        <ClientSideEvents Click="function (s, e) { }" />
    </dx:ASPxButton>

    <br />

    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Додаткові відомості по адресі:" />
    <dx:ASPxTextBox ID="txtArbitraryText" ClientInstanceName="ArbitraryText" runat="server" Width="100%" />

    <br />

    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="OK" AutoPostBack="False">
        <ClientSideEvents Click="function (s, e) {  }" />
    </dx:ASPxButton>

</div>

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>
