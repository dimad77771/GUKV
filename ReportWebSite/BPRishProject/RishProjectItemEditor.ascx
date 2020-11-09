<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RishProjectItemEditor.ascx.cs" Inherits="RishProjectItemEditor" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>
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
    SelectCommand="SELECT id, name FROM dict_obj_rights WHERE is_active = 1 ORDER BY name">
</mini:ProfiledSqlDataSource>

<dx:ASPxCallbackPanel ID="CPItemProperties" ClientInstanceName="CPItemProperties" runat="server" OnCallback="CPItemProperties_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent3" runat="server">

            <div class="data-field-label inline">
                <dx:ASPxLabel ID="LabelSelectOrgFrom" ClientInstanceName="LabelSelectOrgFrom" runat="server" Text="Від кого передається" Width="160px">
                </dx:ASPxLabel>
            </div>
            <div class="data-field-value inline">
                <div class="browse-button">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td> 
                                <dx:ASPxButton ID="ButtonSelectOrgFrom" 
                                    ClientInstanceName="ButtonSelectOrgFrom" runat="server" Text="Вибрати" 
                                    AutoPostBack="false" EnableClientSideAPI="True" > 
                                <ClientSideEvents Click="function(s, e) {
	if (ButtonSelectOrgFrom.GetEnabled()) {
		PopupSelectOrgFrom.ShowAtElementByID(ButtonSelectOrgFrom.GetButtonCell().id);
	}
}" />
<ClientSideEvents Click="function(s, e) {
	if (ButtonSelectOrgFrom.GetEnabled()) {
		PopupSelectOrgFrom.ShowAtElementByID(ButtonSelectOrgFrom.GetButtonCell().id);
	}
}"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>
                            <td> &nbsp; </td>
                            <td>
                                <dx:ASPxButton ID="ButtonClearOrgFrom" ClientInstanceName="ButtonClearOrgFrom" runat="server" Text="Очистити" AutoPostBack="false">
                                    <ClientSideEvents Click="function (s,e) { EditOrgFromText.SetText(''); HiddenOrgFromID.value = ''; CPItemProperties.PerformCallback(JSON.stringify({ OrgFromID: -1 })); }" />
<ClientSideEvents Click="function (s,e) { EditOrgFromText.SetText(&#39;&#39;); HiddenOrgFromID.value = &#39;&#39;; CPItemProperties.PerformCallback(JSON.stringify({ OrgFromID: -1 })); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </div>

                <dx:ASPxTextBox ID="EditOrgFromText" ClientInstanceName="EditOrgFromText" runat="server" ReadOnly="true" Width="505px" />
                <asp:HiddenField ID="HiddenOrgFromID" ClientIDMode="Static" runat="server" />

                <dx:ASPxPopupControl ID="PopupSelectOrgFrom" runat="server" ClientInstanceName="PopupSelectOrgFrom"
                    HeaderText="Вибір Організації, від якої передається Об'єкт" 
                    EnableClientSideAPI="True">
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
<ClientSideEvents Click="function(s, e) {
var selectedOrgId = ComboOrgFrom.GetValue();
if ((typeof selectedOrgId == &#39;string&#39; &amp;&amp; selectedOrgId.length &gt; 0)
|| (typeof selectedOrgId == &#39;number&#39; &amp;&amp; selectedOrgId &gt; 0)) {
PopupSelectOrgFrom.Hide();
CPItemProperties.PerformCallback(JSON.stringify({
OrgFromID: selectedOrgId
}));
}
}"></ClientSideEvents>
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
                            <td> <dx:ASPxButton ID="ButtonSelectOrgTo" ClientInstanceName="ButtonSelectOrgTo" 
                                    runat="server" Text="Вибрати" AutoPostBack="false" 
                                    EnableClientSideAPI="True" > 
                                <ClientSideEvents Click="function(s, e) {
	if (ButtonSelectOrgTo.GetEnabled()) {
		PopupSelectOrgTo.ShowAtElementByID(ButtonSelectOrgTo.GetButtonCell().id);
	}
}" />
<ClientSideEvents Click="function(s, e) {
	if (ButtonSelectOrgTo.GetEnabled()) {
		PopupSelectOrgTo.ShowAtElementByID(ButtonSelectOrgTo.GetButtonCell().id);
	}
}"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>
                            <td> &nbsp; </td>
                            <td>
                                <dx:ASPxButton ID="ButtonClearOrgTo" ClientInstanceName="ButtonClearOrgTo" runat="server" Text="Очистити" AutoPostBack="false">
                                    <ClientSideEvents Click="function (s,e) { EditOrgToText.SetText(''); HiddenOrgToID.value = ''; CPItemProperties.PerformCallback(JSON.stringify({ OrgToID: -1 })); }" />
<ClientSideEvents Click="function (s,e) { EditOrgToText.SetText(&#39;&#39;); HiddenOrgToID.value = &#39;&#39;; CPItemProperties.PerformCallback(JSON.stringify({ OrgToID: -1 })); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </div>

                <dx:ASPxTextBox ID="EditOrgToText" ClientInstanceName="EditOrgToText" runat="server" ReadOnly="true" Width="505px" />
                <asp:HiddenField ID="HiddenOrgToID" ClientIDMode="Static" runat="server" />

                <dx:ASPxPopupControl ID="PopupSelectOrgTo" runat="server" ClientInstanceName="PopupSelectOrgTo"
                    HeaderText="Вибір Організації, якій передається Об'єкт">
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
<ClientSideEvents Click="function(s, e) {
	var selectedOrgId = ComboOrgTo.GetValue();
	if ((typeof selectedOrgId == &#39;string&#39; &amp;&amp; selectedOrgId.length &gt; 0)
		|| (typeof selectedOrgId == &#39;number&#39; &amp;&amp; selectedOrgId &gt; 0)) {
		PopupSelectOrgTo.Hide();
		CPItemProperties.PerformCallback(JSON.stringify({
			OrgToID: selectedOrgId
		}));
	}
}"></ClientSideEvents>
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
                        <ClientSideEvents Click="function (s,e) { ComboRight.SetSelectedIndex(-1); }"></ClientSideEvents>
                    </dx:ASPxButton>
                </div>

                <dx:ASPxComboBox ID="ComboRight" ClientInstanceName="ComboRight" runat="server" Width="505px"
                    DataSourceID="SqlDataSourceDictRights" ValueField="id" ValueType="System.Int32" TextField="name">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
/*
    var rightId = parseInt(ComboRight.GetSelectedItem().value);

    var enableOrgFrom = (ObjectRightsConfig.EnableOrgFrom.indexOf(rightId) &gt;= 0);
    ButtonSelectOrgFrom.SetEnabled(enableOrgFrom);
    ButtonClearOrgFrom.SetEnabled(enableOrgFrom);
    EditOrgFromText.SetEnabled(enableOrgFrom);

    var enableOrgTo = (ObjectRightsConfig.EnableOrgTo.indexOf(rightId) &gt;= 0);
    ButtonSelectOrgTo.SetEnabled(enableOrgTo);
    ButtonClearOrgTo.SetEnabled(enableOrgTo);
    EditOrgToText.SetEnabled(enableOrgTo);
*/
	CPItemProperties.PerformCallback(JSON.stringify({}));
}
" />
<ClientSideEvents SelectedIndexChanged="function(s, e) {
/*
    var rightId = parseInt(ComboRight.GetSelectedItem().value);

    var enableOrgFrom = (ObjectRightsConfig.EnableOrgFrom.indexOf(rightId) &gt;= 0);
    ButtonSelectOrgFrom.SetEnabled(enableOrgFrom);
    ButtonClearOrgFrom.SetEnabled(enableOrgFrom);
    EditOrgFromText.SetEnabled(enableOrgFrom);

    var enableOrgTo = (ObjectRightsConfig.EnableOrgTo.indexOf(rightId) &gt;= 0);
    ButtonSelectOrgTo.SetEnabled(enableOrgTo);
    ButtonClearOrgTo.SetEnabled(enableOrgTo);
    EditOrgToText.SetEnabled(enableOrgTo);
*/
	CPItemProperties.PerformCallback(JSON.stringify({}));
}
"></ClientSideEvents>
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

var tooltipOrgFrom = String.fromCharCode(34) + 'Організація, від якої передається об' + String.fromCharCode(39) + 'єкт' + String.fromCharCode(34);
var tooltipOrgTo = String.fromCharCode(34) + 'Організація, до якої передається об' + String.fromCharCode(39) + 'єкт' + String.fromCharCode(34);

if (e.commandName == 'InsertOrgFromLink') {
    selection.SetHtml('&lt;a href=# rel=gukv_org_from title=' + tooltipOrgFrom + '&gt;' + EditOrgFromText.GetText().toLowerCase() + '&lt;/a&gt;', true);
}
else if (e.commandName == 'InsertOrgToLink') {
    selection.SetHtml('&lt;a href=# rel=gukv_org_to title=' + tooltipOrgTo + '&gt;' + EditOrgToText.GetText().toLowerCase() + '&lt;/a&gt;', true);
}

}" />
<ClientSideEvents CustomCommand="function (s, e) {

var selection = MemoPunktOutro.GetSelection();

var tooltipOrgFrom = String.fromCharCode(34) + &#39;Організація, від якої передається об&#39; + String.fromCharCode(39) + &#39;єкт&#39; + String.fromCharCode(34);
var tooltipOrgTo = String.fromCharCode(34) + &#39;Організація, до якої передається об&#39; + String.fromCharCode(39) + &#39;єкт&#39; + String.fromCharCode(34);

if (e.commandName == &#39;InsertOrgFromLink&#39;) {
    selection.SetHtml(&#39;&lt;a href=# rel=gukv_org_from title=&#39; + tooltipOrgFrom + &#39;&gt;&#39; + EditOrgFromText.GetText().toLowerCase() + &#39;&lt;/a&gt;&#39;, true);
}
else if (e.commandName == &#39;InsertOrgToLink&#39;) {
    selection.SetHtml(&#39;&lt;a href=# rel=gukv_org_to title=&#39; + tooltipOrgTo + &#39;&gt;&#39; + EditOrgToText.GetText().toLowerCase() + &#39;&lt;/a&gt;&#39;, true);
}

}"></ClientSideEvents>

<Settings AllowHtmlView="False" AllowPreview="False"></Settings>
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
<Settings AllowHtmlView="False" AllowPreview="False"></Settings>
                </dx:ASPxHtmlEditor>
            </div>
            <br />

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<dx:ASPxTreeListTemplateReplacement ID="Replacement1" runat="server" ReplacementType="UpdateButton" />
<dx:ASPxTreeListTemplateReplacement ID="Replacement2" runat="server" ReplacementType="CancelButton" />

