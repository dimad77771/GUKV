<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransferRequestAdd.aspx.cs" Inherits="Reports1NF_TransferRequestAdd" MasterPageFile="~/NoHeader.master" Title="Зміна балансоутримувачів об'єктів - Новий запрос" %>

<%@ Register src="ObjectPicker.ascx" tagname="ObjectPicker" tagprefix="uctl1" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v20.1" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v20.1" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v20.1" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v20.1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .SpacingPara
        {
            font-size: 10px;
            margin-top: 4px;
            margin-bottom: 0px;
            padding-top: 0px;
            padding-bottom: 0px;
        }
    </style>

    <script type="text/javascript" language="javascript">
    // <![CDATA[
        var reportId = <%= ReportID %>;

        function OnNumberChanged(s, e) {
            var test = s.GetNumber();
            if (test < 0) {
                s.SetNumber(0);
            }
        }

        function convertStringToDate(strDate) {
            var arrValues = strDate.split(".");
            var jsDate = new Date(arrValues[2], arrValues[1] - 1, arrValues[0]);
            return jsDate;
        }

        function ComboRozpDoc_OnSelectedIndexChanged(s, e) {
            //console.log(ComboRozpDoc);
            if (ComboRozpDoc.GetSelectedIndex() != -1) {

                //console.log(ComboRozpDoc.GetSelectedItem().GetColumnText('kind_id'));
                TextBoxRishName.SetValue(ComboRozpDoc.GetSelectedItem().GetColumnText('topic'));
                DateEditRishDate.SetValue(convertStringToDate(ComboRozpDoc.GetSelectedItem().GetColumnText('doc_date')));
                ComboRishRozpType.SetValue(ComboRozpDoc.GetSelectedItem().GetColumnText('kind_id'));
            } else {
                //TextBoxRishName.SetValue(null);
                //DateEditRishDate.SetDate(null);
                //ComboRishRozpType.SetValue(null);
            }
        }

        function ValidateForm(s, e) {
            $('#objerr').hide();
            if (HdnObjectId.value == "" || HdnObjectId.value == "0" ) {
                $('#objerr').show();
            }
            if (!ASPxClientEdit.ValidateEditorsInContainer(ConveyancingForm)) {
                e.processOnServer = false;
                return;
            }

            s.SetEnabled(false);
            CPMainPanel.PerformCallback('save:');
        }

     
    // ]]>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" />

<mini:ProfiledSqlDataSource ID="SqlDataSourceTransferRequest" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="
    SELECT  TOP 1 req.*, org_to.full_name org_to_name, org_from.full_name org_from_name,
    CASE WHEN is_object_exists = 1 THEN bal.sqr_total ELSE ub.sqr_total END AS sqr_total,
    CASE WHEN is_object_exists = 1 THEN bui.addr_address ELSE bui_un.addr_address END AS full_addr,
    CASE WHEN is_object_exists = 1 THEN bal.purpose_str ELSE ub.purpose_str END AS purpose_str,
    CASE WHEN req.conveyancing_type = 1 THEN 'Передача об’єкту з балансу на баланс'
        WHEN req.conveyancing_type = 2 THEN 'Передача об’єкту з балансу на баланс'
        WHEN req.conveyancing_type = 3 THEN 'Списання об’єкту з балансу шляхом зносу'
        WHEN req.conveyancing_type = 4 THEN 'Списання об’єкту з балансу шляхом приватизації'
        WHEN req.conveyancing_type = 5 THEN 'Постановка  новозбудованого об’єкту на баланс'
        ELSE 'Невідомий'
    END AS type_of_conveyancing
    FROM transfer_requests req
    LEFT JOIN unverified_balans ub ON req.balans_id = ub.id
    LEFT JOIN organizations org_to ON req.org_to_id = org_to.id
    LEFT JOIN organizations org_from ON req.org_from_id = org_from.id
    LEFT JOIN balans bal ON req.balans_id = bal.id
    LEFT JOIN buildings bui ON bal.building_id = bui.id
    LEFT JOIN buildings bui_un ON ub.building_id = bui_un.id
    WHERE request_id = @req_id"
    OnSelecting="SqlDataSourceTransferRequest_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="req_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:profiledsqldatasource ID="SqlDataSourceSearchOrgFrom" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations org
        WHERE
            id = @org_id
            OR
        	(@zkpo = '^' AND @fname = '^' AND (org.id IN (SELECT ar.org_renter_id FROM reports1nf_arenda ar WHERE ar.id = @aid AND ar.report_id = @rep_id))
	        OR
        	(@zkpo != '^' AND @fname != '^' AND org.id IN (SELECT TOP 20 id FROM organizations WHERE zkpo_code LIKE @zkpo AND full_name LIKE @fname AND (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL)))
        ORDER BY org.full_name"
    OnSelecting="SqlDataSourceSearchOrgFrom_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:profiledsqldatasource>

<mini:profiledsqldatasource ID="SqlDataSourceSearchOrgTo" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations org
        WHERE
            id = @org_id
            OR
        	(@zkpo = '^' AND @fname = '^' AND (org.id IN (SELECT ar.org_renter_id FROM reports1nf_arenda ar WHERE ar.id = @aid AND ar.report_id = @rep_id))
	        OR
        	(@zkpo != '^' AND @fname != '^' AND org.id IN (SELECT TOP 20 id FROM organizations WHERE zkpo_code LIKE @zkpo AND full_name LIKE @fname AND (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL)))
        ORDER BY org.full_name"
    OnSelecting="SqlDataSourceSearchOrgTo_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:profiledsqldatasource>

<mini:profiledsqldatasource ID="SqlDataSourceOrgFrom" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, zkpo_code + ' - ' + full_name AS presentation FROM organizations org
        WHERE id = @org_id"
    OnSelecting="SqlDataSourceOrgFrom_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
    </SelectParameters>
</mini:profiledsqldatasource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceProjectTypes" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_doc_kind d
        INNER JOIN rozp_doc_kinds r ON d.id = r.kind_id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="ProfiledSqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT balans_id, org_full_name, sqr_total, COALESCE(balans_obj_name, purpose) AS 'purpose' FROM view_balans WHERE organization_id = @org_id"
    OnSelecting="SqlDataSourceBalansOur_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRights" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_balans_ownership_type d INNER JOIN transfer_actual_ownership_types a ON d.id = a.ownership_type_id" />


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


<mini:ProfiledSqlDataSource ID="SqlDataSourceDictOrgOwnership" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_org_ownership WHERE len(name) > 0 ORDER BY name"
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

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStreets" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_streets where (name is not null) and (RTRIM(LTRIM(name)) <> '') order by name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictDistricts2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT [id], [name] FROM [dict_1nf_districts2] WHERE len(name) > 0 ORDER BY [name]" 
    EnableCaching="True">
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

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" >
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Cabinet.aspx" Text="Стан"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgInfo.aspx" Text="Загальна Інформація"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansList.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansDeletedList.aspx" Text="Відчужені Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgArendaList.aspx" Text="Договори використання приміщень "></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgRentedList.aspx" Text="Договори Орендування"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingRequestsList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/Logout.aspx" Text="Вийти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/ChangePasswordNoMenu.aspx" Text="Пароль"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Help.aspx" Text="?"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>
    
<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel20" Text="Зміна балансоутримувачів об'єктів" CssClass="pagetitle"/>  
</p>

<asp:HiddenField ID="ConveyancingType" runat="server" />
<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" runat="server" OnCallback="CPMainPanel_Callback">
    <ClientSideEvents 
        EndCallback="function (s, e) { if (s.cp_status == 'OK') { window.location.href  = 'ConveyancingRequestsList.aspx?rid=' + reportId;  } else { alert(s.cp_status); ASPxButtonSubmit.SetEnabled(true); } }"
        />
</dx:ASPxCallbackPanel>
    <asp:FormView runat="server" BorderStyle="None" ID="ConveyancingForm" 
        DataSourceID="SqlDataSourceTransferRequest" EnableViewState="False" 
        OnDataBound="ConveyancingForm_DataBound" ClientIDMode="Static"
        OnItemCreated="ConveyancingForm_OnItemCreated">
    <EditItemTemplate>
        <p><asp:Label runat="server" ID="ASPxLabel19" Text='<%# Eval("type_of_conveyancing") %>' CssClass="reporttitle"/></p>
        <dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" ActiveTabIndex="0">
            <TabPages>
                <dx:TabPage Text="Проект акту" Name="RishCardProperties">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl1" runat="server">
                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Інформація про об'єкт">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server">
                                    <dx:ASPxCallbackPanel ID="CPObjSel" ClientInstanceName="CPObjSel" runat="server" OnCallback="CPObjSel_Callback">
                                        <ClientSideEvents EndCallback="function (s, e) { 
                                            console.log(s.cp_status);

                                            if (s.cp_status == 'createbuildingok') {
                                                ButtonDoAddBuilding.SetEnabled(true);
                                                PopupAddBuilding.Hide();
                                                if ($('#LabelBuildingCreationError').text() == '') {
                                                } else {
                                                    PopupAddBuilding.ShowAtElement(ButtonAddBuilding.GetMainElement());
                                                }
                                                //ComboBalansBuildingNewObj.PerformCallback(ComboBalansStreetNewObj.GetValue().toString());
                                            } 
                                            else if(s.cp_status == 'selobjok') {
                                                PopupSelectBalansObject.Hide();
                                            }
                                            else if (s.cp_status == 'createbalansok')
                                            {
                                                PopupAddBalansObj.Hide();
                                            }                                       
                                        }" />
                                    <PanelCollection>

                                    <dx:PanelContent>
                                        <asp:HiddenField ID="HdnObjectId" ClientIDMode="Static" Value='<%# Eval("balans_id") %>' runat="server" />
                                        <asp:HiddenField ID="IsExistingObject" ClientIDMode="Static" Value='<%# Eval("is_object_exists") %>' runat="server" />
                                        <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                            <tr>
                                                <td colspan="3">
                                                    <table>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="ButtonSelectBalansObj" ClientInstanceName="ButtonSelectBalansObj" runat="server" Text="Вибрати" CausesValidation="False" AutoPostBack="false" />
                                                        </td>
                                                        <td>   
                                                            <dx:ASPxButton ID="ButtonCreateBalansObj" ClientInstanceName="ButtonCreateBalansObj" runat="server" Text="Створити" CausesValidation="False" AutoPostBack="false" Visible='<%# ConveyancingType.Value == "5" %>' />
                                                        </td>
                                                        <td>
                                                            <div id="objerr" style="width: 300px; color: #FF0000; font-weight: bold; display: none;" >Необхідно вказати об'єкт</div>     
                                                        </td>
                                                    </tr>
                                                    </table>
                                                    <dx:ASPxPopupControl ID="PopupSelectBalansObject" runat="server" ClientInstanceName="PopupSelectBalansObject"
                                                        HeaderText="Вибір Об'єкту" PopupElementID="ButtonSelectBalansObj" AllowDragging="True" Width="500px" ShowPageScrollbarWhenModal="true">
                                                        <ContentCollection>
                                                            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                                                <uctl1:ObjectPicker ID="ObjectPicker" runat="server" />
                                                                <dx:ASPxButton ID="ButtonChooseBalansObj" ClientInstanceName="ButtonChooseBalansObj"
                                                                    runat="server" Text="Вибрати" Width="148px" AutoPostBack="False" CausesValidation="False">
                                                                    <ClientSideEvents Click="function(s, e) {
                                                                        var selectedBalansObjId = GridViewBalansObjects.GetRowKey(GridViewBalansObjects.GetFocusedRowIndex());
                                                                        if (selectedBalansObjId == null
                                                                            || (typeof selectedBalansObjId == 'string' &amp;&amp; selectedBalansObjId.length == 0) 
                                                                            || (typeof selectedBalansObjId == 'number' &amp;&amp; selectedBalansObjId &lt; 0)) {
                                                                            selectedBalansObjId = 0;
                                                                        }
                                                                        //PopupSelectBalansObject.Hide();
                                                                        CPObjSel.PerformCallback('sel_obj:'+selectedBalansObjId);
                                                                        }" />
                                                                </dx:ASPxButton>
                                                            </dx:PopupControlContentControl>
                                                        </ContentCollection>
                                                    </dx:ASPxPopupControl> 
                                                    <dx:ASPxPopupControl ID="PopupAddBalansObj" runat="server" ClientInstanceName="PopupAddBalansObj" Modal="True" CloseAction="CloseButton" ShowPageScrollbarWhenModal="true"
                                                        HeaderText="Створення нового об'єкту на балансі" PopupElementID="ButtonCreateBalansObj" AllowDragging="True">
                                                        <ContentCollection>
                                                            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">

                                                                <table border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
            <td> <dx:ASPxLabel ID="LabelAddrStreet" runat="server" Text="Назва вулиці:" Width="80px" /> </td>
<%-- --%>
            <td>
                <dx:ASPxComboBox ID="ComboBalansStreetNewObj" runat="server" ClientInstanceName="ComboBalansStreetNewObj"
                    DataSourceID="SqlDataSourceDictStreets" DropDownStyle="DropDownList" ValueType="System.Int32"
                    TextField="name" ValueField="id" Width="220px" IncrementalFilteringMode="StartsWith"
                    FilterMinLength="3" EnableCallbackMode="True" CallbackPageSize="50" EnableViewState="False"
                    EnableSynchronization="False">
                    <ClientSideEvents 
                        Init="function (s, e) {
                            var value = ComboBalansStreetNewObj.GetValue();
                            ButtonAddBuilding.SetEnabled(value != null);
                        }"
                        SelectedIndexChanged="function(s, e) { 
                            var value = ComboBalansStreetNewObj.GetValue();
                            ButtonAddBuilding.SetEnabled(value != null);
                            ComboBalansBuildingNewObj.PerformCallback((value == null ? '' : value).toString()); 
                        }" />
                </dx:ASPxComboBox>
            </td>     
            <td> <dx:ASPxLabel ID="LabelAddrPickerNumber" runat="server" Text="Номер будинку:" Width="95px"/> </td>
            <td>
                <dx:ASPxComboBox runat="server" ID="ComboBalansBuildingNewObj" ClientInstanceName="ComboBalansBuildingNewObj"
                    DataSourceID="SqlDataSourceDictBuildings" DropDownStyle="DropDownList" TextField="nomer"
                    ValueField="id" ValueType="System.Int32" Width="100px" IncrementalFilteringMode="StartsWith"
                    EnableSynchronization="False" OnCallback="ComboAddressBuilding_Callback">
                    <ClientSideEvents SelectedIndexChanged="function (s,e) { console.log('ComboBalansBuildingNewObj SelectedIndexChanged'); }" />
                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="popupCreateBalObject">
                        <RequiredField IsRequired="true" ErrorText="Адреса має бути заповнена" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td>
                <dx:ASPxPopupControl ID="PopupAddBuilding" runat="server" ClientInstanceName="PopupAddBuilding" ShowPageScrollbarWhenModal="true"
                    HeaderText="Створення нової адреси" Modal="true">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">

                            <table border="0" cellspacing="0" cellpadding="2">
                                <tr>
                                    <td> <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Район:" Width="110px" /> </td>
                                    <td colspan="3"> <dx:ASPxComboBox ID="ComboBoxDistrict" runat="server" ClientInstanceName="ComboBoxDistrict" 
                                        ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                            IncrementalFilteringMode="StartsWith" 
                                            DataSourceID="SqlDataSourceDictDistricts2" /> </td>
                                </tr>
                                <tr>
                                    <td> <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Номер Будинку:" Width="110px" /> </td>
                                    <td> <dx:ASPxTextBox ID="TextBoxNumber1" ClientInstanceName="TextBoxNumber1" runat="server" Width="100px" /> </td>
                                    <td> <dx:ASPxTextBox ID="TextBoxNumber2" ClientInstanceName="TextBoxNumber2" runat="server" Width="100px" /> </td>
                                    <td> <dx:ASPxTextBox ID="TextBoxNumber3" ClientInstanceName="TextBoxNumber3" runat="server" Width="100px" /> </td>
                                </tr>
                                <tr>
                                    <td> <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Додаткова Адреса:" Width="110px" /> </td>
                                    <td colspan="3"> <dx:ASPxTextBox ID="TextBoxMiscAddr" ClientInstanceName="TextBoxMiscAddr" runat="server" Width="100%" /> </td>
                                </tr>
                                <tr>
                                    <td> <dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Вид Об'єкту:" Width="110px" /> </td>
                                    <td colspan="3"> <dx:ASPxComboBox ID="ComboBoxBuildingKind" runat="server" ClientInstanceName="ComboBoxBuildingKind" 
                                        ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                            IncrementalFilteringMode="StartsWith" 
                                            DataSourceID="SqlDataSourceDictObjectKind" /> </td>
                                </tr>
                                <tr>
                                    <td> <dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Тип Об'єкту:" Width="110px" /> </td>
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
    //window.CPObjectEditorCallbackType = 'createbuilding';
    e.performOnServer = false;
    s.SetEnabled(false);
    CPObjSel.PerformCallback('createbuilding:');
}" />
                            </dx:ASPxButton>

                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>

                <dx:ASPxButton ID="ButtonAddBuilding" ClientInstanceName="ButtonAddBuilding" runat="server" 
                    AutoPostBack="False" Text="Створити" ClientEnabled="false">
                    <ClientSideEvents Click="function (s, e) {
                        PopupAddBuilding.ShowAtElement(ButtonAddBuilding.GetMainElement());
                    }" />
                </dx:ASPxButton>
            </td>                                                                    
                                                                    </tr>

                                                                </table>

                                                                <br/>

                                                                <table border="0" cellspacing="0" cellpadding="2">
                                                                    <tr>
                                                                        <td> <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Площа Об'єкту:" Width="120px" /> </td>
                                                                        <td> 
                                                                            <dx:ASPxSpinEdit ID="EditBalansObjSquare" ClientInstanceName="EditBalansObjSquare" runat="server" Width="380px">
                                                                                <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="popupCreateBalObject">
                                                                                    <RequiredField IsRequired="true" ErrorText="Площа має бути заповнена" />
                                                                                </ValidationSettings>
                                                                            </dx:ASPxSpinEdit> 
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td> <dx:ASPxLabel ID="ASPxLabelBalValue" runat="server" Text="Балансова Вартість:" Width="120px" /> </td>
                                                                        <td> <dx:ASPxSpinEdit ID="EditBalansObjCost" ClientInstanceName="EditBalansObjCost" runat="server" Width="380px" /> </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td> <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Форма Власності:" Width="120px" /> </td>
                                                                        <td> 
                                                                            <dx:ASPxComboBox ID="ComboBoxBalansOwnership" runat="server" ClientInstanceName="ComboBoxBalansOwnership" 
                                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="380px" 
                                                                                IncrementalFilteringMode="StartsWith" 
                                                                                DataSourceID="SqlDataSourceDictOrgOwnership" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td> <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Вид Об'єкту:" Width="120px" /> </td>
                                                                        <td> <dx:ASPxComboBox ID="ComboBoxBalansObjKind" runat="server" ClientInstanceName="ComboBoxBalansObjKind" 
                                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="380px" 
                                                                                IncrementalFilteringMode="StartsWith" 
                                                                                DataSourceID="SqlDataSourceDictObjectKind" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td> <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Тип Об'єкту:" Width="120px" /> </td>
                                                                        <td> <dx:ASPxComboBox ID="ComboBoxBalansObjType" runat="server" ClientInstanceName="ComboBoxBalansObjType" 
                                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="380px" 
                                                                                IncrementalFilteringMode="StartsWith" 
                                                                                DataSourceID="SqlDataSourceDictObjectType" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td> <dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Група Призначення:" Width="120px" /> </td>
                                                                        <td> 
                                                                            <dx:ASPxComboBox ID="ComboBoxBalansPurposeGroup" runat="server" ClientInstanceName="ComboBoxBalansPurposeGroup" 
                                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="380px" 
                                                                                IncrementalFilteringMode="StartsWith" 
                                                                                DataSourceID="SqlDataSourceDictBalansPurposeGroup" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td> <dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Призначення:" Width="120px" /> </td>
                                                                        <td> 
                                                                            <dx:ASPxComboBox ID="ComboBoxBalansPurpose" runat="server" ClientInstanceName="ComboBoxBalansPurpose" 
                                                                            ValueType="System.Int32" TextField="name" ValueField="id" Width="380px" 
                                                                                IncrementalFilteringMode="StartsWith" 
                                                                                DataSourceID="SqlDataSourceDictBalansPurpose" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td> <dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="Використання:" Width="120px" /> </td>
                                                                        <td> <dx:ASPxTextBox ID="EditBalansObjUse" ClientInstanceName="EditBalansObjUse" runat="server" Width="380px" /> </td>
                                                                    </tr>
                                                                </table>

                                                                <br/>
                                                                <dx:ASPxLabel ID="LabelBalansCreationError" ClientInstanceName="LabelBalansCreationError" runat="server" Text="" ClientVisible="false" ForeColor="Red" />
                                                                <br />

                                                                <dx:ASPxButton ID="ButtonDoAddBalansObj" ClientInstanceName="ButtonDoAddBalansObj" runat="server" AutoPostBack="False" Text="Створити" CausesValidation="false">
                                                                    <ClientSideEvents Click="function (s, e) {
                                                                        if(ASPxClientEdit.ValidateGroup('popupCreateBalObject'))
                                                                        CPObjSel.PerformCallback('createbalans:');
                                                                        return false;
                                                }" />
                                                                </dx:ASPxButton>
                
                                                            </dx:PopupControlContentControl>
                                                        </ContentCollection>
                                                    </dx:ASPxPopupControl>                                                    
                                                                                               
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="14px"/></tr>   
                                            <tr>
                                                <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Адреса" Width="180px" /></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabelObjAddress" runat="server" Text='<%# Eval("full_addr") %>'></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="14px"/></tr>
                                            <tr>
                                                <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Призначення" /></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabelObjPurpose" runat="server" Text='<%# Eval("purpose_str") %>'></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="14px"/></tr>
                                            <tr>
                                                <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Загальна площа (м²)" /></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabelObjTotalArea" runat="server" Text='<%# Eval("sqr_total") %>'></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="14px"/></tr>
                                            <tr>
                                                <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Площа, що передається (м²)" /></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="AreaForTransfer" runat="server" Width="170px" DisplayFormatString="N" DecimalPlaces="2" Value='<%# Eval("conveyancing_area") %>'>
                                                        <ClientSideEvents NumberChanged="OnNumberChanged" />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                        </table> 
                                    </dx:PanelContent>
                                    </PanelCollection>   
                                    </dx:ASPxCallbackPanel>
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel11" runat="server" HeaderText="Розпорядчі документи">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <dx:ASPxCallbackPanel ID="CPCreateRozpDoc" ClientInstanceName="CPCreateRozpDoc" runat="server" OnCallback="CPCreateRozpDoc_Callback">
                                    <ClientSideEvents EndCallback="function (s, e) { PopupCreateRozp.Hide(); }" />
                                    <PanelCollection>
                                        <dx:PanelContent>
                                        <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                            <tr>
                                                <td valign="top" width="100px">
                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Номер документу">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td width="8px">
                                                    &nbsp;</td>
                                                <td>
                                                
                                                    <dx:ASPxComboBox ID="ComboRozpDoc" runat="server" ClientInstanceName="ComboRozpDoc"
                                                        DropDownStyle="DropDown" ValueType="System.Int32"
                                                        ValueField="id" Width="700px" IncrementalFilteringMode="Contains"
                                                        EnableCallbackMode="True" CallbackPageSize="10" 
                                                        TextFormatString="{0}" FilterMinLength="1"
                                                        OnItemRequestedByValue="ComboRozpDoc_OnItemRequestedByValue" 
                                                        OnItemsRequestedByFilterCondition="ComboRozpDoc_OnItemsRequestedByFilterCondition"
                                                        TextField="doc_num" Value='<%# Eval("rishrozp_doc_id") %>'
                                                        >      
                                                        <%--Value='<%# Eval("rishrozp_doc_id") %>'--%>
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Номер документу" FieldName="doc_num" Width="100px" />
                                                            <dx:ListBoxColumn Caption="Дата документу" FieldName="doc_date" Width="100px" />
                                                            <dx:ListBoxColumn Caption="Тип документу" FieldName="doc_kind" Width="200px" />
                                                            <dx:ListBoxColumn Caption="Назва документу" FieldName="topic" Width="500px" />   
                                                            <dx:ListBoxColumn FieldName="kind_id" Width="0px" Visible="true" />                      
                                                        </Columns>   
                                                        <ClientSideEvents SelectedIndexChanged="ComboRozpDoc_OnSelectedIndexChanged" />
                                                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField IsRequired="true" ErrorText="Номер документу має бути заповнений" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="4px"/></tr>                                        
                                            <tr>
                                                <td colspan="2"></td>
                                                <td>
                                                    <dx:ASPxPopupControl ID="PopupCreateRozp" runat="server" ClientInstanceName="PopupCreateRozp" CloseAction="CloseButton"
                                                            HeaderText="Створення розпорядчого документу" PopupElementID="BtnCreateRozp" AllowDragging="True" Width="500px" Modal="true" ShowPageScrollbarWhenModal="true">
                                                            <ContentCollection>
                                                                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                                                    <table>
                                                                        <tr>
                                                                            <td><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Номер документу" Width="120px"></dx:ASPxLabel></td>
                                                                            <td>
                                                                                <dx:ASPxTextBox ID="NewRozpNum" ClientInstanceName="NewRozpName" runat="server" Width="200px">
                                                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="PopupCreateRozpDoc">
                                                                                        <RequiredField IsRequired="true" ErrorText="Номер документу має бути заповнений" />
                                                                                    </ValidationSettings>
                                                                                </dx:ASPxTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Назва документу"></dx:ASPxLabel></td>
                                                                            <td>
                                                                                <dx:ASPxMemo ID="NewRozpName" ClientInstanceName="NewRozpName" runat="server" Width="700px" Height="80px">
                                                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="PopupCreateRozpDoc">
                                                                                        <RequiredField IsRequired="true" ErrorText="Назва документу має бути заповнена" />
                                                                                    </ValidationSettings>
                                                                                </dx:ASPxMemo>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Дата прийняття"></dx:ASPxLabel></td>
                                                                            <td>
                                                                                <dx:ASPxDateEdit ID="NewRozpDate" ClientInstanceName="NewRozpDate" runat="server" Width="200px">
                                                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="PopupCreateRozpDoc">
                                                                                        <RequiredField IsRequired="true" ErrorText="Дата прийняття має бути заповнена" />
                                                                                    </ValidationSettings>
                                                                                </dx:ASPxDateEdit>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Тип документу"></dx:ASPxLabel></td>
                                                                            <td>
                                                                                <dx:ASPxComboBox ID="NewRozpType" runat="server"
                                                                                    ClientInstanceName="NewRozpType" DataSourceID="SqlDataSourceProjectTypes" 
                                                                                    TextField="name" ValueField="id" ValueType="System.Int32" Width="700px">
                                                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="PopupCreateRozpDoc">
                                                                                        <RequiredField IsRequired="true" ErrorText="Тип Документу має бути заповнена" />
                                                                                    </ValidationSettings>
                                                                                </dx:ASPxComboBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                    <p class="SpacingPara"/>

                                                                    <dx:ASPxButton ID="ASPxButton5" ClientInstanceName="ButtonChooseBalansObj"
                                                                        runat="server" Text="Створити" Width="148px" AutoPostBack="False" CausesValidation="false">
                                                                        <ClientSideEvents Click="function(s, e) {
                                                                            e.performOnServer = false;
                                                                            if(ASPxClientEdit.ValidateGroup('PopupCreateRozpDoc'))
                                                                                CPCreateRozpDoc.PerformCallback();  
                                                                            }" />
                                                                    </dx:ASPxButton>
                                                                </dx:PopupControlContentControl>
                                                            </ContentCollection>
                                                        </dx:ASPxPopupControl>
                                                    <dx:ASPxButton ID="BtnCreateRozp" runat="server" Text="Створити" CausesValidation="false" AutoPostBack="false"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="4px"/></tr>                                        
                                            <tr>
                                                <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Назва документу"></dx:ASPxLabel></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxMemo ID="TextBoxRishName" ClientInstanceName="TextBoxRishName" runat="server" Width="700px" Height="80px" Value='<%# Eval("rishrozp_name") %>' ReadOnly="true">
                                                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField IsRequired="true" ErrorText="Назва документу має бути заповнена" />
                                                        </ValidationSettings>
                                                    </dx:ASPxMemo>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="4px"/></tr>
                                            <tr>
                                                <td valign="top" width="100px">
                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Дата прийняття">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td width="8px">
                                                    &nbsp;</td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="DateEditRishDate" ClientInstanceName="DateEditRishDate" runat="server" Value='<%# Eval("rishrozp_date") %>' ReadOnly="true">
                                                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField IsRequired="true" ErrorText="Дата прийняття має бути заповнена" />
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="4px"/></tr>
                                            <tr>
                                                <td valign="top" width="100px">
                                                    <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Тип Документу">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td width="8px">
                                                    &nbsp;</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboRishRozpType" runat="server" Value='<%# Eval("rish_doc_kind_id") %>' ReadOnly="true"
                                                        ClientInstanceName="ComboRishRozpType" DataSourceID="SqlDataSourceProjectTypes" 
                                                        TextField="name" ValueField="id" ValueType="System.Int32" Width="700px">
                                                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField IsRequired="true" ErrorText="Тип Документу має бути заповнена" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="4px"/></tr>
                                            <tr id="Tr1" runat="server" visible='<%# ConveyancingType.Value == "1" || ConveyancingType.Value == "2" || ConveyancingType.Value == "5" %>'>
                                                <td valign="top" width="100px">
                                                    Право</td>
                                                <td width="8px">
                                                    &nbsp;</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ASPxComboBoxRight" runat="server" Value='<%# Eval("obj_right_id") %>'
                                                        DataSourceID="SqlDataSourceRights" TextField="name" ValueField="id" ValueType="System.Int32" Width="700px">
                                                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField IsRequired="true" ErrorText="Право має бути заповнений" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                    </dx:ASPxCallbackPanel>                               
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        
                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelAkt" runat="server" HeaderText="Акт">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                        <tr>
                                            <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel62" runat="server" Text="Номер акту" /></td>
                                            <td>
                                                <dx:ASPxTextBox ID="ActNumber" runat="server" Width="170px" Value='<%# Eval("new_akt_num") %>'>
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Номер акту має бути заповнен" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>                                            
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel63" runat="server" Text="Дата акту">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="ActDate" runat="server" Value='<%# Eval("new_akt_date") %>'>
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Дата акту має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr><td colspan="5" height="4px"/></tr>
                                        <tr>
                                            <td width="100px" valign="top">
                                                <dx:ASPxLabel ID="ASPxLabel64" runat="server" Text="Сума за актом" />
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="ActSum" runat="server" Width="170px" DisplayFormatString="C" DecimalPlaces="2" Value='<%# Eval("new_akt_summa") %>'>
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Сума за актом має бути заповнена" />
                                                    </ValidationSettings>
                                                    <ClientSideEvents NumberChanged="OnNumberChanged" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel65" runat="server" Text="Залишкова сума" />
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="ActResidualSum" runat="server" Width="170px" DisplayFormatString="C" DecimalPlaces="2" Value='<%# Eval("new_akt_summa_zalishkova") %>'>
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Залишкова сума за актом має бути заповнена" />
                                                    </ValidationSettings>
                                                    <ClientSideEvents NumberChanged="OnNumberChanged" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                        </tr>                                        
                                    </table>    
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelOrgTo" runat="server" HeaderText="Кому передається" Visible='<%# ConveyancingType.Value == "2" || ConveyancingType.Value == "4" %>'>
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server">
                                <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                    <tr>
                                        <td> <dx:ASPxLabel ID="ASPxLabel58" runat="server" Text="Код ЄДРПОУ:" Width="90px" /> </td>
                                        <td> <dx:ASPxTextBox ID="EditOrgToZKPO" ClientInstanceName="EditOrgToZKPO" runat="server" Width="200px" Title="Код ЄДРПОУ організації, якій передається"/> </td>
                                        <td> <dx:ASPxLabel ID="ASPxLabel59" runat="server" Text="Назва:" Width="50px" /> </td>
                                        <td> <dx:ASPxTextBox ID="EditOrgToName" ClientInstanceName="EditOrgToName" runat="server" Width="300px" Title="Назва організації, якій передається"/> </td>
                                        <td>
                                            <dx:ASPxButton ID="BtnFindOrgTo" ClientInstanceName="BtnFindOrgTo" runat="server" AutoPostBack="False" Text="Знайти">
                                                <ClientSideEvents Click="function (s, e) { ComboOrgTo.PerformCallback(EditOrgToZKPO.GetText() + '|' + EditOrgToName.GetText()); }" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                    <tr><td colspan="5" height="4px"/></tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel61" runat="server" Text="Організація, якій передається об'єкт:" Width="90px" /> 
                                        </td>
                                        <td colspan="4" valign="top">
                                            <dx:ASPxComboBox ID="ComboOrgTo" ClientInstanceName="ComboOrgTo" runat="server" 
                                                DropDownStyle="DropDownList" Value='<%# Eval("org_to_id") %>'
                                                Width="700px" DataSourceID="SqlDataSourceSearchOrgTo" ValueField="id" 
                                                ValueType="System.Int32" TextField="search_name" EnableSynchronization="True" 
                                                OnCallback="ComboOrg_Callback" Title="Організація, якій передається об'єкт" 
                                                OnItemRequestedByValue="ComboOrg_ItemRequestedByValue">
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelOrgFrom" runat="server" HeaderText="Від кого передається" Visible='<%# ConveyancingType.Value == "1" %>'>
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent41" runat="server">
                                <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                    <tr>
                                        <td> <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Код ЄДРПОУ:" Width="90px" /> </td>
                                        <td> <dx:ASPxTextBox ID="EditOrgFromZKPO" ClientInstanceName="EditOrgFromZKPO" runat="server" Width="200px" Title="Код ЄДРПОУ організації, від якої передається"/> </td>
                                        <td> <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Назва:" Width="50px" /> </td>
                                        <td> <dx:ASPxTextBox ID="EditOrgFromName" ClientInstanceName="EditOrgFromName" runat="server" Width="300px" Title="Назва організації, від якої передається"/> </td>
                                        <td>
                                            <dx:ASPxButton ID="BtnFindOrgFrom" ClientInstanceName="BtnFindOrgFrom" runat="server" AutoPostBack="False" Text="Знайти" CausesValidation="false">
                                                <ClientSideEvents Click="function (s, e) { ComboOrgFrom.PerformCallback(EditOrgFromZKPO.GetText() + '|' + EditOrgFromName.GetText()); }" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                    <tr><td colspan="5" height="4px"/></tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Організація, від якої передається" Width="90px" /> 
                                        </td>
                                        <td colspan="4" valign="top">
                                            <dx:ASPxComboBox ID="ComboOrgFrom" ClientInstanceName="ComboOrgFrom" runat="server" 
                                                DropDownStyle="DropDownList" Value='<%# Eval("org_from_id") %>'
                                                Width="700px" DataSourceID="SqlDataSourceSearchOrgFrom" ValueField="id" 
                                                ValueType="System.Int32" TextField="search_name" EnableSynchronization="True" 
                                                OnCallback="ComboOrg_Callback" Title="Організація, від якої передається об'єкт"
                                                OnItemRequestedByValue="ComboOrg_ItemRequestedByValue">
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Коментар" Visible="<%# (requestId != 0) %>">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent6" runat="server">
                                    <dx:ASPxMemo ID="Comment" ClientInstanceName="Comment" runat="server" Width="810px" Height="80px" Value='<%# Eval("comment") %>' />
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
        </dx:ASPxPageControl>

        <dx:ASPxPanel ID="ASPxPanelInitButtons" runat="server" Width="600px">
            <PanelCollection>
                <dx:PanelContent>
                    <div>
                        <div style="margin: 5px 5px 5px 0; height: 25px;">
                            <div style="float: left; padding-right: 5px;">
                                <dx:ASPxButton ID="ASPxButton3" ClientInstanceName="ASPxButtonSubmit" runat="server" Text="Надіслати акт на підтверження" AutoPostBack="false"> 
                                    <ClientSideEvents Click="function (s,e) { ValidateForm(s, e); }" />               
                                </dx:ASPxButton>
                            </div>
                            <div style="float: left;">
                                <dx:ASPxButton ID="ASPxButtonCancel" runat="server" Text="Скасувати зміни та повернутися до списку запитів" AutoPostBack="false" CausesValidation="false">
                                    <ClientSideEvents Click="function (s, e) { window.location.href  = 'ConveyancingRequestsList.aspx?rid=' + reportId; }" />
                                </dx:ASPxButton>
                            </div>        
                        </div>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxPanel>
    </EditItemTemplate>
    <ItemTemplate>
        <p><asp:Label runat="server" ID="ASPxLabel19" Text='<%# Eval("type_of_conveyancing") %>' CssClass="reporttitle"/></p>
        <dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" ActiveTabIndex="0">
            <TabPages>
                <dx:TabPage Text="Проект акту" Name="RishCardProperties">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl1" runat="server">
                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Інформація про об'єкт">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server">
                                    <dx:ASPxCallbackPanel ID="CPObjSel" ClientInstanceName="CPObjSel" runat="server" OnCallback="CPObjSel_Callback">
                                    <PanelCollection>
                                    <dx:PanelContent>
                                        <asp:HiddenField ID="HdnObjectId" ClientIDMode="Static" Value="" runat="server"  />
                                        <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                            <tr>
                                                <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Адреса" Width="180px" /></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabelObjAddress" runat="server" Text='<%# Eval("full_addr") %>'></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="14px"/></tr>
                                            <tr>
                                                <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Призначення" /></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabelObjPurpose" runat="server" Text='<%# Eval("purpose_str") %>'></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="14px"/></tr>
                                            <tr>
                                                <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Загальна площа (м²)" /></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabelObjTotalArea" runat="server" Text='<%# Eval("sqr_total") %>'></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr><td colspan="3" height="14px"/></tr>
                                            <tr>
                                                <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Площа, що передається (м²)" /></td>
                                                <td width="8px">&nbsp;</td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="AreaForTransfer" runat="server" Width="170px" DisplayFormatString="N" DecimalPlaces="2" Value='<%# Eval("conveyancing_area") %>' ReadOnly="true">
                                                        <ClientSideEvents NumberChanged="OnNumberChanged" />
                                                    </dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                        </table> 
                                    </dx:PanelContent>
                                    </PanelCollection>   
                                    </dx:ASPxCallbackPanel>
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Розпорядчі документии">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent5" runat="server">
                                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                        <tr>
                                            <td valign="top" width="100px">
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Номер документу">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td width="8px">
                                                &nbsp;</td>
                                            <td>
                                                <dx:ASPxTextBox ID="RozpDoc" runat="server" Value='<%# Eval("rish_num") %>' ReadOnly="true" Width="700px"></dx:ASPxTextBox>                                              
                                            </td>
                                        </tr>
                                        <tr><td colspan="3" height="4px"/></tr>                                        
                                        <tr>
                                            <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Назва документу"></dx:ASPxLabel></td>
                                            <td width="8px">&nbsp;</td>
                                            <td>
                                                <dx:ASPxMemo ID="TextBoxRishName" ClientInstanceName="TextBoxRishName" runat="server" Width="700px" Height="80px" Value='<%# Eval("rishrozp_name") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Назва документу має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxMemo>
                                            </td>
                                        </tr>
                                        <tr><td colspan="3" height="4px"/></tr>
                                        <tr>
                                            <td valign="top" width="100px">
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Дата прийняття">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td width="8px">
                                                &nbsp;</td>
                                            <td>
                                                <dx:ASPxDateEdit ID="DateEditRishDate" ClientInstanceName="DateEditRishDate" runat="server" Value='<%# Eval("rishrozp_date") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Дата прийняття має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr><td colspan="3" height="4px"/></tr>
                                        <tr>
                                            <td valign="top" width="100px">
                                                <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Тип Документу">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td width="8px">
                                                &nbsp;</td>
                                            <td>
                                                <dx:ASPxComboBox ID="ComboRishRozpType" runat="server" Value='<%# Eval("rish_doc_kind_id") %>' ReadOnly="true"
                                                    ClientInstanceName="ComboRishRozpType" DataSourceID="SqlDataSourceProjectTypes" 
                                                    TextField="name" ValueField="id" ValueType="System.Int32" Width="700px">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Тип Документу має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <%
                                            if ((int)((System.Data.DataRowView)ConveyancingForm.DataItem)["obj_right_id"] > 0)
                                            {
                                        %>
                                        <tr><td colspan="3" height="4px"/></tr>
                                        <tr>
                                            <td valign="top" width="100px">
                                                Право</td>
                                            <td width="8px">
                                                &nbsp;</td>
                                            <td>
                                                <dx:ASPxComboBox ID="ASPxComboBoxRight" runat="server" Value='<%# Eval("obj_right_id") %>' ReadOnly="true"
                                                    DataSourceID="SqlDataSourceRights" TextField="name" ValueField="id" ValueType="System.Int32" Width="700px">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Право має бути заповнений" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <%
                                            }
                                         %>
                                    </table>                                    
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        
                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelAkt" runat="server" HeaderText="Акт">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                        <tr>
                                            <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel62" runat="server" Text="Номер акту" /></td>
                                            <td>
                                                <dx:ASPxTextBox ID="ActNumber" runat="server" Width="170px" Value='<%# Eval("new_akt_num") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Номер акту має бути заповнен" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>                                            
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel63" runat="server" Text="Дата акту">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="ActDate" runat="server" Value='<%# Eval("new_akt_date") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Дата акту має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr><td colspan="5" height="4px"/></tr>
                                        <tr>
                                            <td width="100px" valign="top">
                                                <dx:ASPxLabel ID="ASPxLabel64" runat="server" Text="Сума за актом" />
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="ActSum" runat="server" Width="170px" DisplayFormatString="C" DecimalPlaces="2" Value='<%# Eval("new_akt_summa") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Сума за актом має бути заповнена" />
                                                    </ValidationSettings>
                                                    <ClientSideEvents NumberChanged="OnNumberChanged" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel65" runat="server" Text="Залишкова сума" />
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="ActResidualSum" runat="server" Width="170px" DisplayFormatString="C" DecimalPlaces="2" Value='<%# Eval("new_akt_summa_zalishkova") %>' ReadOnly="true">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="true" ErrorText="Залишкова сума за актом має бути заповнена" />
                                                    </ValidationSettings>
                                                    <ClientSideEvents NumberChanged="OnNumberChanged" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                        </tr>                                        
                                    </table>    
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelOrgTo" runat="server" HeaderText="Кому передається" Visible='<%# ConveyancingType.Value != "3" %>'>
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel61" runat="server" Text="Організація, якій передається об'єкт:" /> 
                                        </td>
                                    </tr>
                                    <tr><td height="4px"/></tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Value='<%# Eval("org_to_name") %>' ReadOnly="true" Width="700px"></dx:ASPxTextBox>                                             
                                        </td>
                                    </tr>
                                </table>
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelOrgFrom" runat="server" HeaderText="Від кого передається" Visible='<%# ConveyancingType.Value != "5" %>'>
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server">
                                <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Організація, від якої передається:" /> 
                                        </td>
                                    </tr>
                                    <tr><td height="4px"/></tr>
                                    <tr> 
                                        <td>   
                                            <dx:ASPxTextBox ID="ASPxTextBox3" runat="server" Value='<%# Eval("org_from_name") %>' ReadOnly="true" Width="700px"></dx:ASPxTextBox>  
                                        </td>
                                    </tr>
                                </table>
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>
                        
                        <dx:ASPxPopupControl ID="pcWarn" runat="server" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcWarn"
                            HeaderText="Попередження" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" ShowPageScrollbarWhenModal="true">
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                    <dx:ASPxPanel ID="Panel1" runat="server">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent6" runat="server">
                                                Необхідно додати коментар.
                                            </dx:PanelContent>                                        
                                        </PanelCollection>
                                    </dx:ASPxPanel>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Коментар">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent7" runat="server">
                                    <dx:ASPxMemo ID="Comment" ClientInstanceName="Comment" runat="server" Width="810px" Height="80px" Value='<%# Eval("comment") %>'></dx:ASPxMemo>
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
        </dx:ASPxPageControl>
        
    </ItemTemplate>
</asp:FormView>

<dx:ASPxCallbackPanel ID="CPApply" ClientInstanceName="CPApply" runat="server" OnCallback="CPApply_Callback"></dx:ASPxCallbackPanel>
<dx:ASPxPanel ID="ASPxPanelApplyButtons" runat="server" Width="600px" Visible="true">
    <PanelCollection>
        <dx:PanelContent>
            <div>
                <div style="margin: 5px 5px 5px 0; height: 25px;">
                    <div style="float: left; padding-right: 5px;">
                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Підтвердити" AutoPostBack="false" CausesValidation="false"> 
                            <ClientSideEvents Click="function(s, e) { CPApply.PerformCallback('approve:'); }" />            
                        </dx:ASPxButton>
                    </div>
                    <div style="float: left;  padding-right: 5px;">
                        <dx:ASPxButton ID="ASPxButton4" runat="server" Text="Відхилити" AutoPostBack="false" CausesValidation="true">
                            <ClientSideEvents Click="function (s, e) { if(Comment.GetValue() == null || Comment.GetValue().trim() == '') pcWarn.Show(); else CPApply.PerformCallback('discard:'+Comment.GetValue().trim());  }" />
                        </dx:ASPxButton>
                    </div>  
                    <div style="float: left;">
                        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Повернутися до списку запитів" AutoPostBack="false" CausesValidation="false"> 
                            <ClientSideEvents Click="function(s, e) { window.location.href  = 'ConveyancingRequestsList.aspx?rid=' + reportId; }" />
                        </dx:ASPxButton>
                    </div>       
                </div>
            </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel> 
</asp:Content>
