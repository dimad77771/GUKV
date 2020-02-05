<%@ control language="C#" autoeventwireup="true" inherits="RishProjectItemEditor, App_Web_rishprojectitemeditor.ascx.23e82b75" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>
<%@ Register src="RishProjectTableObjectEditor.ascx" tagname="RishProjectTableObjectEditor" tagprefix="uctl1" %>
<%@ Register src="RishProjectTableOrgFromEditor.ascx" tagname="RishProjectTableOrgFromEditor" tagprefix="uctl2" %>
<%@ Register src="RishProjectTableOrgToEditor.ascx" tagname="RishProjectTableOrgToEditor" tagprefix="uctl3" %>

<%-- Datasources that support resolution of various IDs into (read-only) display text for the edit form --%>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrganizationText" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select organizations.full_name
        from organizations 
        where organizations.id = @org_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAddressText" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select b.street_full_name + ' ' + b.addr_nomer AS 'addr'
        from view_buildings b
        where b.building_id = @building_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="building_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansText" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select CONVERT(VARCHAR(24), COALESCE(bal.sqr_total, 0)) + ' кв.м.' + COALESCE(' (' + bal.balans_obj_name + ') ', ' ') +  'на балансі ' + bal.org_full_name + ' за адресою ' + bal.street_full_name + ', ' + bal.addr_nomer AS 'balans_descr',
        bal.street_full_name + ', ' + bal.addr_nomer AS 'balans_address'
        from view_balans bal
        where bal.balans_id = @balans_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="balans_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictRights" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_obj_rights">
</mini:ProfiledSqlDataSource>

<dx:ASPxCallbackPanel ID="CPItemProperties" ClientInstanceName="CPItemProperties" runat="server" OnCallback="CPItemProperties_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent3" runat="server">

            <div class="data-field-label inline">
                <dx:ASPxLabel ID="LabelBalansObjText" ClientInstanceName="LabelBalansObjText" runat="server" Text="Об'єкт">
                </dx:ASPxLabel>
            </div>
            <div class="data-field-value inline">
                <div class="browse-button">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td> <dx:ASPxButton ID="ButtonSelectBalansObj" ClientInstanceName="ButtonSelectBalansObj" runat="server" Text="Вибрати" AutoPostBack="false" /> </td>
                            <td> &nbsp; </td>
                            <td>
                                <dx:ASPxButton ID="ButtonClearBalansObj" ClientInstanceName="ButtonClearBalansObj" runat="server" Text="Очистити" AutoPostBack="false">
                                    <ClientSideEvents Click="function (s,e) { EditBalansObjText.SetText(''); EditAddressText.SetText(''); HiddenBalansObjID.value = ''; HiddenAddrID.value = ''; HiddenArbitraryText.value = ''; CPItemProperties.PerformCallback(JSON.stringify({ BalansObjID: -1 })); }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </div>

                <dx:ASPxTextBox ID="EditBalansObjText" ClientInstanceName="EditBalansObjText" runat="server" ReadOnly="true" Width="520px" />
                <dx:ASPxTextBox ID="EditAddressText" ClientInstanceName="EditAddressText" runat="server" ReadOnly="true" Width="520px" ClientVisible="false" />
                <asp:HiddenField ID="HiddenBalansObjID" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="HiddenAddrID" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="HiddenArbitraryText" ClientIDMode="Static" runat="server" />

                <dx:ASPxPopupControl ID="PopupSelectBalansObject" runat="server" ClientInstanceName="PopupSelectBalansObject"
                    HeaderText="Вибір Об'єкту" PopupElementID="ButtonSelectBalansObj">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <uctl1:RishProjectTableObjectEditor ID="ObjEditor" runat="server"/>

                            <dx:ASPxButton ID="ButtonChooseBalansObj" ClientInstanceName="ButtonChooseBalansObj"
                                runat="server" Text="Вибрати" Width="148px" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) {
var selectedBuildingId = ComboBalansBuilding.GetValue();
if (selectedBuildingId == null
|| (typeof selectedBuildingId == 'string' &amp;&amp; selectedBuildingId.length == 0)
|| (typeof selectedBuildingId == 'number' &amp;&amp; selectedBuildingId &lt; 0)) {
selectedBuildingId = 0;
}

var selectedBalansObjId = GridViewBalansObjects.GetRowKey(GridViewBalansObjects.GetFocusedRowIndex());
if (selectedBalansObjId == null
|| (typeof selectedBalansObjId == 'string' &amp;&amp; selectedBalansObjId.length == 0) 
|| (typeof selectedBalansObjId == 'number' &amp;&amp; selectedBalansObjId &lt; 0)) {
selectedBalansObjId = 0;
}

var arbitraryText = $.trim(ArbitraryText.GetValue());

if (arbitraryText.length == 0 && selectedBuildingId == 0 && selectedBalansObjId == 0) {
alert('Для збереження необхідно заповнити хоча б одне поле!');
}
else {
PopupSelectBalansObject.Hide();
CPItemProperties.PerformCallback(JSON.stringify({
    BuildingID: selectedBuildingId, 
	BalansObjID: selectedBalansObjId, 
    ArbitraryText: ArbitraryText.GetValue()
}));
}

}" />
                            </dx:ASPxButton>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
            <br />
            <div class="data-field-label inline">
                <dx:ASPxLabel ID="LabelSelectOrgFrom" ClientInstanceName="LabelSelectOrgFrom" runat="server" Text="Від кого передається" Width="160px">
                </dx:ASPxLabel>
            </div>
            <div class="data-field-value inline">
                <div class="browse-button">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td> <dx:ASPxButton ID="ButtonSelectOrgFrom" ClientInstanceName="ButtonSelectOrgFrom" runat="server" Text="Вибрати" AutoPostBack="false" /> </td>
                            <td> &nbsp; </td>
                            <td>
                                <dx:ASPxButton ID="ButtonClearOrgFrom" ClientInstanceName="ButtonClearOrgFrom" runat="server" Text="Очистити" AutoPostBack="false">
                                    <ClientSideEvents Click="function (s,e) { EditOrgFromText.SetText(''); HiddenOrgFromID.value = ''; CPItemProperties.PerformCallback(JSON.stringify({ OrgFromID: -1 })); }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </div>

                <dx:ASPxTextBox ID="EditOrgFromText" ClientInstanceName="EditOrgFromText" runat="server" ReadOnly="true" Width="520px" />
                <asp:HiddenField ID="HiddenOrgFromID" ClientIDMode="Static" runat="server" />

                <dx:ASPxPopupControl ID="PopupSelectOrgFrom" runat="server" ClientInstanceName="PopupSelectOrgFrom"
                    HeaderText="Вибір Організації, від якої передається Об'єкт" PopupElementID="ButtonSelectOrgFrom">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                            <uctl2:RishProjectTableOrgFromEditor ID="OrgFromEditor" runat="server"/>

                            <dx:ASPxButton ID="ButtonSetOrgFrom" ClientInstanceName="ButtonSetOrgFrom" runat="server" AutoPostBack="False" Text="Вибрати" Width="90px">
                                <ClientSideEvents Click="function(s, e) {
var selectedOrgId = ComboOrgFrom.GetValue();
if ((typeof selectedOrgId == 'string' &amp;&amp; selectedOrgId.length &gt; 0)
|| (typeof selectedOrgId == 'number' &amp;&amp; selectedOrgId &gt; 0)) {
PopupSelectOrgFrom.Hide();
CPItemProperties.PerformCallback(JSON.stringify({
OrgFromID: selectedOrgId
}));
}
}" />
                            </dx:ASPxButton>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
            <br />
            <div class="data-field-label inline">
                <dx:ASPxLabel ID="LabelSelectOrgTo" ClientInstanceName="LabelSelectOrgTo" runat="server" Text="Кому передається" />
            </div>
            <div class="data-field-value inline">
                <div class="browse-button">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td> <dx:ASPxButton ID="ButtonSelectOrgTo" ClientInstanceName="ButtonSelectOrgTo" runat="server" Text="Вибрати" AutoPostBack="false" /> </td>
                            <td> &nbsp; </td>
                            <td>
                                <dx:ASPxButton ID="ButtonClearOrgTo" ClientInstanceName="ButtonClearOrgTo" runat="server" Text="Очистити" AutoPostBack="false">
                                    <ClientSideEvents Click="function (s,e) { EditOrgToText.SetText(''); HiddenOrgToID.value = ''; CPItemProperties.PerformCallback(JSON.stringify({ OrgToID: -1 })); }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </div>

                <dx:ASPxTextBox ID="EditOrgToText" ClientInstanceName="EditOrgToText" runat="server" ReadOnly="true" Width="520px" />
                <asp:HiddenField ID="HiddenOrgToID" ClientIDMode="Static" runat="server" />

                <dx:ASPxPopupControl ID="PopupSelectOrgTo" runat="server" ClientInstanceName="PopupSelectOrgTo"
                    HeaderText="Вибір Організації, якій передається Об'єкт" PopupElementID="ButtonSelectOrgTo">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <uctl3:RishProjectTableOrgToEditor ID="OrgToEditor" runat="server"/>

                            <dx:ASPxButton ID="ButtonSetOrgTo" ClientInstanceName="ButtonSetOrgTo" runat="server" AutoPostBack="False" Text="Вибрати" Width="90px">
                                <ClientSideEvents Click="function(s, e) {
var selectedOrgId = ComboOrgTo.GetValue();
if ((typeof selectedOrgId == 'string' &amp;&amp; selectedOrgId.length &gt; 0)
|| (typeof selectedOrgId == 'number' &amp;&amp; selectedOrgId &gt; 0)) {
PopupSelectOrgTo.Hide();
CPItemProperties.PerformCallback(JSON.stringify({
OrgToID: selectedOrgId
}));
}
}" />
                            </dx:ASPxButton>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
            <br />
            <div class="data-field-label inline">
                <dx:ASPxLabel ID="LabelRight" ClientInstanceName="LabelRight" runat="server" Text="Право"/>
            </div>
            <div class="data-field-value inline">
                <div class="browse-button">
                    <dx:ASPxButton ID="ButtonClearRight" ClientInstanceName="ButtonClearRight" runat="server" Text="Очистити" AutoPostBack="false">
                        <ClientSideEvents Click="function (s,e) { ComboRight.SetSelectedIndex(-1); }" />
                    </dx:ASPxButton>
                </div>

                <dx:ASPxComboBox ID="ComboRight" ClientInstanceName="ComboRight" runat="server" Width="520px"
                    DataSourceID="SqlDataSourceDictRights" ValueField="id" ValueType="System.Int32" TextField="name">
                    <ClientSideEvents SelectedIndexChanged="function (s,e) { }" />
                </dx:ASPxComboBox>
            </div>
            <br />
            <div class="data-field-label">
                <dx:ASPxLabel ID="LabelOutro" ClientInstanceName="LabelOutro" runat="server" Text="Текст пункту">
                </dx:ASPxLabel>
            </div>
            <div class="data-field-value">
                <dx:ASPxHtmlEditor ID="MemoPunktOutro" ClientInstanceName="MemoPunktOutro" runat="server"
                    Width="860px" Height="200px">
                    <Settings AllowHtmlView="false" AllowPreview="False" />
                    <ClientSideEvents CustomCommand="function (s, e) {

var selection = MemoPunktOutro.GetSelection();

var tooltipAddress = String.fromCharCode(34) + 'Адреса об' + String.fromCharCode(39) + 'єкту, що передається' + String.fromCharCode(34);
var tooltipObject = String.fromCharCode(34) + 'Назва об' + String.fromCharCode(39) + 'єкту, що передається' + String.fromCharCode(34);
var tooltipOrgFrom = String.fromCharCode(34) + 'Організація, від якої передається об' + String.fromCharCode(39) + 'єкт' + String.fromCharCode(34);
var tooltipOrgTo = String.fromCharCode(34) + 'Організація, до якої передається об' + String.fromCharCode(39) + 'єкт' + String.fromCharCode(34);

if (e.commandName == 'InsertAddressLink') {
    selection.SetHtml('<a href=# rel=gukv_addr title=' + tooltipAddress + '>' + EditAddressText.GetText().toLowerCase() + '</a>', true);
}
else if (e.commandName == 'InsertObjectLink') {
    selection.SetHtml('<a href=# rel=gukv_obj title=' + tooltipObject + '>' + EditBalansObjText.GetText().toLowerCase() + '</a>', true);
}
else if (e.commandName == 'InsertOrgFromLink') {
    selection.SetHtml('<a href=# rel=gukv_org_from title=' + tooltipOrgFrom + '>' + EditOrgFromText.GetText().toLowerCase() + '</a>', true);
}
else if (e.commandName == 'InsertOrgToLink') {
    selection.SetHtml('<a href=# rel=gukv_org_to title=' + tooltipOrgTo + '>' + EditOrgToText.GetText().toLowerCase() + '</a>', true);
}

}" />
                </dx:ASPxHtmlEditor>
            </div>
            <br />
            <div class="data-field-label">
                <dx:ASPxLabel ID="LabelExplanation" ClientInstanceName="LabelExplanation" runat="server" Text="Примітка">
                </dx:ASPxLabel>
            </div>
            <div class="data-field-value">
                <dx:ASPxHtmlEditor ID="MemoPunktExplanation" ClientInstanceName="MemoPunktExplanation"
                    runat="server" Width="860px" Height="100px">
                    <Settings AllowHtmlView="false" AllowPreview="False" />
                </dx:ASPxHtmlEditor>
            </div>
            <br />

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<dx:ASPxTreeListTemplateReplacement ID="Replacement1" runat="server" ReplacementType="UpdateButton" />
<dx:ASPxTreeListTemplateReplacement ID="Replacement2" runat="server" ReplacementType="CancelButton" />

