<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddressPicker.ascx.cs" Inherits="UserControls_AddressPicker" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function OnAddressPickerStreetChanged(cmbStreet) {

        ComboAddrPickerBuilding.PerformCallback(cmbStreet.GetValue().toString());
    }

    function OnAddressPickerSearch(s, e) {

        PopupAddressPicker.Hide();

        // Prepare the grid callback parameter
        var param = "building:" + ComboAddrPickerStreet.GetText() + "$" + ComboAddrPickerBuilding.GetText();

        PrimaryGridView.PerformCallback(param);
    }

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStreets" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_streets where (not name is null) and (RTRIM(LTRIM(name)) <> '')">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictBuildings" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, LTRIM(RTRIM(addr_nomer)) AS 'nomer' from buildings where
        (is_deleted IS NULL OR is_deleted = 0) AND
        master_building_id IS NULL AND
        addr_street_id = @street_id AND
        (RTRIM(LTRIM(addr_nomer)) <> '')
        ORDER BY RTRIM(LTRIM(addr_nomer))"
    OnSelecting="SqlDataSourceDictBuildings_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="street_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<br/>

<dx:ASPxLabel ID="LabelAddrPickerStreet" runat="server" Text="Назва Вулиці"></dx:ASPxLabel>

<dx:ASPxComboBox ID="ComboAddrPickerStreet" runat="server" ClientInstanceName="ComboAddrPickerStreet"
    DataSourceID="SqlDataSourceDictStreets"
    DropDownStyle="DropDownList"
    ValueType="System.Int32"
    TextField="name"
    ValueField="id"
    Width="350px"
    IncrementalFilteringMode="StartsWith"
    FilterMinLength="2"
    EnableCallbackMode="True"
    CallbackPageSize="50"
    EnableViewState="False"
    EnableSynchronization="False" >

    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnAddressPickerStreetChanged(s); }" />
</dx:ASPxComboBox>

<br/>

<dx:ASPxLabel ID="LabelAddrPickerNumber" runat="server" Text="Номер будинку"></dx:ASPxLabel>

<dx:ASPxComboBox runat="server" ID="ComboAddrPickerBuilding" ClientInstanceName="ComboAddrPickerBuilding" 
    DataSourceID="SqlDataSourceDictBuildings"
    DropDownStyle="DropDown"
    TextField="nomer"
    ValueField="id"
    ValueType="System.Int32"
    Width="350px"
    IncrementalFilteringMode="None"
    EnableSynchronization="False"
    OnCallback="ComboAddrPickerBuilding_Callback">
</dx:ASPxComboBox>

<br/>

<dx:ASPxButton ID="ButtonAddrPickerSearch" runat="server" Text="Знайти" Width="148px" AutoPostBack="False" >
    <ClientSideEvents Click="OnAddressPickerSearch" />
</dx:ASPxButton>

<br/>
