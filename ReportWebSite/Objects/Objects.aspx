<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Objects.aspx.cs" Inherits="ObjRights_ObjRightTransfer"
    MasterPageFile="~/NoHeader.master" Title="Передача Прав" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
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

        PrimaryGridView.SetHeight(window.innerHeight - 200);
    }

    function GridViewTransferInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewTransferEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ApplyObjTransferPreFilter(s, e) {

        PopupPreFilter.Hide();
        ButtonPreFilter.SetChecked(true);

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("applypre:"));
    }

    function ClearObjTransferPreFilter(s, e) {

        ComboPreFilterRight.SetSelectedIndex(-1);
        ListPreFilterOrgOwnership.UnselectAll();

        PopupPreFilter.Hide();
        ButtonPreFilter.SetChecked(false);

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("clearpre:"));
    }

    function PopupPreFilterClosing(s, e) {

        var selectedItems = ListPreFilterOrgOwnership.GetSelectedItems();

        if (selectedItems == null || selectedItems == undefined) {

            ButtonPreFilter.SetChecked(false);
        }
        else {
            if (ComboPreFilterRight.GetSelectedIndex() >= 0 && selectedItems.length > 0) {

                ButtonPreFilter.SetChecked(true);
            }
            else {
                ButtonPreFilter.SetChecked(false);
            }
        }
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

<mini:ProfiledSqlDataSource ID="SqlDataSourceAllRights" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_obj_rights WHERE LTRIM(RTRIM(name)) <> ''">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAllOrgOwnershipTypes" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_org_ownership WHERE LTRIM(RTRIM(name)) <> ''">
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Objects/RishProjects.aspx" Text="Розпорядчі Документи"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle2" runat="server" Text="Передача прав на об'єкти" CssClass="reporttitle"></asp:Label>
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
            <dx:ASPxPopupControl ID="PopupPreFilter" runat="server" 
                HeaderText="Попередній фільтр" 
                ClientInstanceName="PopupPreFilter" 
                PopupElementID="ButtonPreFilter">

                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">

                        <dx:ASPxRadioButton ID="RadioButtonLeaveObjects" runat="server" GroupName="PreFilter" Checked="True"
                            Text="Залишити об'єкти, що передавались з наступними показниками:"></dx:ASPxRadioButton>
                        <br/>
                        <dx:ASPxRadioButton ID="RadioButtonExcludeObjects" runat="server" GroupName="PreFilter"
                            Text="Вилучити об'єкти, що передавались з наступними показниками:"></dx:ASPxRadioButton>
                        <br/>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Право:"></dx:ASPxLabel>

                        <dx:ASPxComboBox ID="ComboPreFilterRight" runat="server" ValueType="System.Int32" Width="300px"
                            ClientInstanceName="ComboPreFilterRight"
                            DataSourceID="SqlDataSourceAllRights" TextField="name" ValueField="id"></dx:ASPxComboBox>
                        <br/>
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Форма власності організації, до якої передано об'єкт:"></dx:ASPxLabel>

                        <dx:ASPxListBox Width="300px" Height="260px" ID="ListPreFilterOrgOwnership" ClientInstanceName="ListPreFilterOrgOwnership"
                            SelectionMode="CheckColumn" runat="server" DataSourceID="SqlDataSourceAllOrgOwnershipTypes" ValueField="id" TextField="name" />
                        <br/>

                        <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <dx:ASPxButton ID="ButtonApplyPreFilter" runat="server" AutoPostBack="False" Text="Застосувати фільтр" Width="148px">
                                    <ClientSideEvents Click="ApplyObjTransferPreFilter" />
                                </dx:ASPxButton>
                            </td>
                            <td width="4px">&nbsp;</td>
                            <td>
                                <dx:ASPxButton ID="ButtonClearPreFilter" runat="server" AutoPostBack="False" Text="Скинути фільтр" Width="148px">
                                    <ClientSideEvents Click="ClearObjTransferPreFilter" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                        </table>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents Closing="PopupPreFilterClosing"/>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ButtonPreFilter" ClientInstanceName="ButtonPreFilter" GroupName="A"
                runat="server" AutoPostBack="False" Text="Попередній фільтр" Width="148px"></dx:ASPxButton>
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
                PopupElementID="ASPxButton_Transfer_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="ASPxButton_Transfer_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Transfer_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Transfer_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Transfer_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Transfer_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Transfer_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Transfer_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjectTransfer" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    OnSelecting="SqlDataSourceObjectTransfer_Selecting"
    SelectCommand="SELECT * FROM m_view_object_rights WHERE
        (@p_rda_district_id = 0) OR
        (org_to_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND org_to_district_id = @p_rda_district_id) OR
        (org_from_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND org_from_district_id = @p_rda_district_id)">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjectTransfer_InclusivePreFilter" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    OnSelecting="SqlDataSourceObjectTransfer_InclusivePreFilter_Selecting"
    SelectCommand="SELECT vor.* 
                    FROM m_view_object_rights vor 
                    WHERE EXISTS(
	                    SELECT 0 FROM object_rights r
	                    INNER JOIN organizations org_to ON org_to.id = r.org_to_id
                        WHERE r.right_id = @rid AND CHARINDEX( '+' + CAST(org_to.form_ownership_id AS VARCHAR(64)) + '+', @foid) > 0 AND
                        r.building_id = vor.building_id AND r.name = vor.name AND
                        (r.sqr_transferred = vor.sqr_transferred OR r.len_transferred = vor.len_transferred OR
                        (r.sqr_transferred IS NULL AND vor.sqr_transferred IS NULL AND
                        r.len_transferred IS NULL AND vor.len_transferred IS NULL)))
                        AND
                        ((@p_rda_district_id = 0) OR
                        (vor.org_to_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND vor.org_to_district_id = @p_rda_district_id) OR
                        (vor.org_from_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND vor.org_from_district_id = @p_rda_district_id))">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rid" />
        <asp:Parameter DbType="String" DefaultValue="0" Name="foid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjectTransfer_ExclusivePreFilter" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    OnSelecting="SqlDataSourceObjectTransfer_ExclusivePreFilter_Selecting"
    SelectCommand="SELECT vor.* 
                    FROM m_view_object_rights vor 
                    WHERE NOT EXISTS(
	                    SELECT 0 FROM object_rights r
	                    INNER JOIN organizations org_to ON org_to.id = r.org_to_id
                        WHERE r.right_id = @rid AND CHARINDEX( '+' + CAST(org_to.form_ownership_id AS VARCHAR(64)) + '+', @foid) > 0 AND
                        r.building_id = vor.building_id AND r.name = vor.name AND
                        (r.sqr_transferred = vor.sqr_transferred OR r.len_transferred = vor.len_transferred OR
                        (r.sqr_transferred IS NULL AND vor.sqr_transferred IS NULL AND
                        r.len_transferred IS NULL AND vor.len_transferred IS NULL)))
                        AND
                        ((@p_rda_district_id = 0) OR
                        (vor.org_to_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND vor.org_to_district_id = @p_rda_district_id) OR
                        (vor.org_from_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND vor.org_from_district_id = @p_rda_district_id))">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rid" />
        <asp:Parameter DbType="String" DefaultValue="0" Name="foid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterTransfer" runat="server" 
    FileName="ПередачаПрав" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID = "SqlDataSourceObjectTransfer"
    KeyFieldName="transfer_id"
    Width="100%" 
    OnCustomCallback="GridViewTransfer_CustomCallback"
    OnCustomSummaryCalculate="GridViewTransfer_CustomSummaryCalculate"
    OnCustomFilterExpressionDisplayText = "GridViewTransfer_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewTransfer_ProcessColumnAutoFilter"
    OnCustomColumnSort="GridViewTransfer_CustomColumnSort" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="transfer_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="False" Caption="ID"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="building_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="1" Visible="False" Caption="ID Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="False" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="True" Caption="Назва Вулиці" Width="140px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCardSimple(" + Eval("building_id") + ")\">" + Eval("street_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Caption="Номер Будинку" Width="80px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCardSimple(" + Eval("building_id") + ")\">" + Eval("addr_nomer") + "</a>"%>
            </DataItemTemplate>
            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="condition" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="False" Caption="Технічний Стан"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_type" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="False" Caption="Тип Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="False" Caption="Вид Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_group" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="False" Caption="Група призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="False" Caption="Призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_from_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10" Visible="False" Caption="Від Кого - ID"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_from_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="11" Visible="False" Caption="Від Кого - Повна Назва" Width="160px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_from_id") + ")\">" + Eval("org_from_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_from_short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="12" Visible="True" Caption="Від Кого - Коротка Назва" Width="160px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_from_id") + ")\">" + Eval("org_from_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_from_zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="13" Visible="False" Caption="Від Кого - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_from_industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="14" Visible="False" Caption="Від Кого - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_from_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="15" Visible="False" Caption="Від Кого - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_from_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="16" Visible="False" Caption="Від Кого - Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_from_form_gosp" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="17" Visible="False" Caption="Від Кого - Форма Господарювання"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_to_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="18" Visible="False" Caption="Кому - ID"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_to_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="19" Visible="False" Caption="Кому - Повна Назва" Width="160px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_to_id") + ")\">" + Eval("org_to_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_to_short_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="20" Visible="True" Caption="Кому - Коротка Назва" Width="160px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_to_id") + ")\">" + Eval("org_to_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_to_zkpo_code" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="21" Visible="False" Caption="Кому - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_to_industry" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="22" Visible="False" Caption="Кому - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_to_occupation" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="23" Visible="False" Caption="Кому - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_to_ownership" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="24" Visible="False" Caption="Кому - Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_to_form_gosp" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="25" Visible="False" Caption="Кому - Форма Господарювання"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="right_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="26" Visible="True" Caption="Право"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="transfer_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="27" Visible="False" Caption="Рік Передачі"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="transfer_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="28" Visible="False" Caption="Квартал Передачі"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="29" Visible="False" Caption="ID Акту Приймання-передачі"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="akt_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="30" Visible="False" Caption="Дата Акту Приймання-передачі"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="akt_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="31" Visible="True" Caption="Номер Акту Приймання-передачі" Width="80px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("akt_id") + ")\">" + Eval("akt_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_topic" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="32" Visible="False" Caption="Назва Акту Приймання-передачі">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("akt_id") + ")\">" + Eval("akt_topic") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_search_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="33" Visible="False" Caption="Акт Приймання-передачі">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("akt_id") + ")\">" + Eval("akt_search_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rozp_doc_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="34" Visible="False" Caption="ID Розпорядження"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rozp_doc_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="35" Visible="True" Caption="Номер Розпорядження / Рішення" Width="80px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("rozp_doc_id") + ")\">" + Eval("rozp_doc_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="rozp_doc_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="36" Visible="False" Caption="Дата Розпорядження / Рішення"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="rozp_doc_topic" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="37" Visible="False" Caption="Назва Розпорядження / Рішення">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("rozp_doc_id") + ")\">" + Eval("rozp_doc_topic") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rozp_doc_search_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="38" Visible="False" Caption="Розпорядження / Рішення">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("rozp_doc_id") + ")\">" + Eval("rozp_doc_search_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="39" Visible="True" Caption="Назва Об'єкту" Width="140px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="characteristic" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="40" Visible="False" Caption="Характеристика"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="misc_info" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="41" Visible="False" Caption="Додаткова Інформація"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sum_balans" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="42" Visible="False" Caption="Первісна Вартість (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sum_zalishkova" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="43" Visible="False" Caption="Залишкова Вартість (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_transferred" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="44" Visible="False" Caption="Передана Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="len_transferred" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="45" Visible="False" Caption="Передана Довжина (м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_from_vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="46" Visible="False" Caption="Від Кого - Орган Управління"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_to_vedomstvo" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="47" Visible="False" Caption="Кому - Орган Управління"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_date_year" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="48" Visible="False" Caption="Дата Акту Приймання-передачі - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="akt_date_quarter" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="49" Visible="False" Caption="Дата Акту Приймання-передачі - Квартал"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sum_balans" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sum_zalishkova" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_transferred" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="len_transferred" SummaryType="Custom" DisplayFormat="{0}" />
    </TotalSummary>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
        <dx:ASPxSummaryItem FieldName="sum_balans" SummaryType="Custom" DisplayFormat="Первісна Вартість = {0} грн." />
        <dx:ASPxSummaryItem FieldName="sum_zalishkova" SummaryType="Custom" DisplayFormat="Залишкова Вартість = {0} грн." />
    </GroupSummary>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager AlwaysShowPager="true" PageSize="25"> </SettingsPager>
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
    <SettingsCookies CookiesID="GUKV.Transfer" Version="A2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewTransferInit" EndCallback="GridViewTransferEndCallback" />
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
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>
