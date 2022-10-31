<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BuildingPicker.ascx.cs" Inherits="ObjectPicker" %>

<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStreets" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_streets where (name is not null) and (RTRIM(LTRIM(name)) <> '') order by name">
<%--    SelectCommand="select s.id, s.name as sname, r.name as rname, ISNULL(r.name + ' - ', '') + s.name as name from dict_streets s left join dict_regions r on r.id = s.region_id where (not s.name is null) and (RTRIM(LTRIM(s.name)) <> '')">     --%>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictBuildings" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, LTRIM(RTRIM(addr_nomer)) AS 'nomer' from buildings where
        --(is_deleted IS NULL OR is_deleted = 0) AND
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
    SelectCommand="SELECT balans_id, sqr_total, COALESCE(balans_obj_name, purpose) AS 'purpose' FROM view_balans WHERE building_id = @bid"
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
        </tr>
    </table>
    
    <br />

    <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Існуючі об'єкти за цією адресою:" />

    <dx:ASPxGridView ID="GridViewBalansObjects" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="GridViewBalansObjects" Width="100%" DataSourceID="SqlDataSourceBalansSearch"
        KeyFieldName="balans_id" 
        OnCustomCallback="GridViewBalansObjects_CustomCallback" 
        ondatabound="GridViewBalansObjects_DataBound">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="sqr_total" Caption="Площа, кв.м." VisibleIndex="1"
                Width="100px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="purpose" Caption="Призначення" VisibleIndex="2"
                Width="200px">
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPager PageSize="5" >
        </SettingsPager>
        <Settings ShowFilterRow="False" ShowFilterRowMenu="False" VerticalScrollBarMode="Hidden"
            VerticalScrollBarStyle="Standard" />
        <SettingsBehavior AutoFilterRowInputDelay="1500" AllowFocusedRow="false" />
        <Styles Header-Wrap="True">
            <Header Wrap="True">
            </Header>
        </Styles>
    </dx:ASPxGridView>
</div>
