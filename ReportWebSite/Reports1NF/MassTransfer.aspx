<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MassTransfer.aspx.cs" Inherits="Reports1NF_MassTransfer" MasterPageFile="~/NoMenu.master" Title="Массова зміна балансоутримувачів об'єктів" EnableSessionState="True" EnableViewState="true" %>

<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

    <script type="text/javascript" language="javascript">

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

    </script>

    <script type="text/javascript">
        function OnSelectAllRowsLinkClick1() {
            gridView1.SelectRows();
        }
        function OnUnselectAllRowsLinkClick1() {
            gridView1.UnselectRows();
        }
        function OnGridViewInit1() {
            UpdateTitlePanel1();
        }
        function OnGridViewSelectionChanged1() {
            UpdateTitlePanel1();
        }
        function OnGridViewEndCallback1() {
            UpdateTitlePanel1();
        }
        function UpdateTitlePanel1() {
            var text = "Кількість виділених об'єктів: <b>" + gridView1.GetSelectedRowCount() + "</b>. ";
            lblInfo1.SetText(text);
            lnkClearSelection1.SetVisible(gridView1.GetSelectedRowCount() > 0);
        }
    </script>

    <script type="text/javascript">
        function OnSelectAllRowsLinkClick2() {
            gridView2.SelectRows();
        }
        function OnUnselectAllRowsLinkClick2() {
            gridView2.UnselectRows();
        }
        function OnGridViewInit2() {
            UpdateTitlePanel2();
        }
        function OnGridViewSelectionChanged2() {
            UpdateTitlePanel2();
        }
        function OnGridViewEndCallback2() {
            UpdateTitlePanel2();
        }
        function UpdateTitlePanel2() {
            var text = "Кількість виділених об'єктів: <b>" + gridView2.GetSelectedRowCount() + "</b>. ";
            lblInfo2.SetText(text);
            lnkClearSelection2.SetVisible(gridView2.GetSelectedRowCount() > 0);
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceReport" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT * FROM view_reports1nf WHERE report_id = @report_id"
    OnSelecting="SqlDataSourceReport_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="report_id" />
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

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" />

<mini:ProfiledSqlDataSource ID="SqlDataSourceProjectTypes" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_doc_kind d
        INNER JOIN rozp_doc_kinds r ON d.id = r.kind_id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRights" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_balans_ownership_type d INNER JOIN transfer_actual_ownership_types a ON d.id = a.ownership_type_id" />

<table cellpadding="2px">
<tr>
    <td style="width:680px">

        <asp:FormView ID="formViewMain" DataSourceID="SqlDataSourceReport" runat="server" Width="100%">
        <ItemTemplate>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Балансоутримувач" Width="100%" >
                <ContentPaddings Padding="4px" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">

                        <table border="0" cellspacing="0" cellpadding="4px" width="100%">
                        <tr>
                            <td height="25px"> <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Код ЄДРПОУ та назва організації"/> </td>
                        </tr>
                        
                        <tr>
                            <td><dx:ASPxTextBox ID="ASPxTextBox3" ClientInstanceName="EditOrgFromName" runat="server" Width="100%" ReadOnly="true" Title="Назва організації" Text='<%# String.Format("{0} | {1}", Eval("zkpo_code"), Eval("full_name")) %>'></dx:ASPxTextBox></td>
                        </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
        </dx:ASPxRoundPanel>

        </ItemTemplate>
        </asp:FormView>

    </td>
    <td style="width:680px">
    
        <dx:ASPxRoundPanel ID="ASPxRoundPanelOrgFrom" runat="server" HeaderText="Балансоутримувач" Width="100%">
                <ContentPaddings Padding="4px" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent41" runat="server">
                    <table border="0" cellspacing="0" cellpadding="4px" width="100%">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Код ЄДРПОУ:"/></td>
                            <td><dx:ASPxTextBox ID="EditOrgFromZKPO" ClientInstanceName="EditOrgFromZKPO" runat="server" Title="Код ЄДРПОУ організації"/></td>
                            <td align="right"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Назва:" /></td>
                            <td><dx:ASPxTextBox ID="EditOrgFromName" ClientInstanceName="EditOrgFromName" runat="server" Title="Назва організації" Width="100%" /></td>
                            <td align="right" style="width:120px">
                                <dx:ASPxButton ID="BtnFindOrgFrom" ClientInstanceName="BtnFindOrgFrom" runat="server" AutoPostBack="False" Text="Знайти" CausesValidation="false" Width="110px">
                                    <ClientSideEvents Click="function (s, e) { ComboOrgFrom.PerformCallback(EditOrgFromZKPO.GetText() + '|' + EditOrgFromName.GetText()); }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Організація" /> 
                            </td>
                            <td colspan="4">
                                <dx:ASPxComboBox ID="ComboOrgFrom" ClientInstanceName="ComboOrgFrom" runat="server" 
                                    DropDownStyle="DropDownList" 
                                    Width="100%" DataSourceID="SqlDataSourceSearchOrgFrom" ValueField="id" 
                                    ValueType="System.Int32" TextField="search_name" EnableSynchronization="True" 
                                    OnCallback="ComboOrg_Callback" Title="Організація" 
                                    OnValueChanged="ComboOrgFrom_ValueChanged">
                                    <ClientSideEvents ValueChanged="function (s, e) { ContentCallback.PerformCallback('loadbalans:' + ComboOrgFrom.Value); }" />    
                                </dx:ASPxComboBox> 
                                
                            </td>
                        </tr>
                    </table>
                    </dx:PanelContent>                            
                </PanelCollection>
            </dx:ASPxRoundPanel>

    </td>
</tr>
<tr>
    <td colspan="2">
            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Об'єкти" Width="100%" >
                <ContentPaddings Padding="4px" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">

                        <dx:ASPxCallbackPanel ID="ContentCallback" ClientInstanceName="ContentCallback" 
                            runat="server" OnCallback="ContentCallback_Callback" >

                            <PanelCollection>
                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                               
                        <table>
                            <tr valign="top">
                                <td style="width:680px">
                                    <table width="100%">
                                        <tr>
                                            <td>
<%--                                                <dx:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False" Width="110px"
                                                    CausesValidation="False" ClientInstanceName="BtnFindOrgFrom" Text="Вибрати все" Wrap="False">
                                                    <ClientSideEvents Click="function (s, e) { ContentCallback.PerformCallback('selectall1:'); }" />
                                                </dx:ASPxButton>--%>

                                                
                                                <dx:ASPxHyperLink ID="lnkSelectAllRows1" ClientInstanceName="lnkSelectAllRows1" 
                                                    Text="Виділити все" runat="server" Cursor="pointer" 
                                                    ClientSideEvents-Click="OnSelectAllRowsLinkClick1" >
                                                    <ClientSideEvents Click="OnSelectAllRowsLinkClick1"></ClientSideEvents>
                                                </dx:ASPxHyperLink>
                                                |
                                                <dx:ASPxHyperLink ID="lnkClearSelection1" ClientInstanceName="lnkClearSelection1" 
                                                    Text="Зняти виділення" runat="server" Cursor="pointer" 
                                                    ClientSideEvents-Click="OnUnselectAllRowsLinkClick1" ClientVisible="false"  >
                                                    <ClientSideEvents Click="OnUnselectAllRowsLinkClick1"></ClientSideEvents>
                                                </dx:ASPxHyperLink>
                                                &nbsp;
                                                <dx:ASPxLabel ID="lblInfo1" ClientInstanceName="lblInfo1" runat="server" />
                                            </td>
                                            <td align="right">
                                                <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Width="150px"
                                                    CausesValidation="False" ClientInstanceName="BtnFindOrgFrom" 
                                                    Text="Передати об'єкти &gt;&gt;" Wrap="False">
                                                    <ClientSideEvents Click="function (s, e) { ContentCallback.PerformCallback('sendselected1:'); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" valign="top">
                                                <dx:ASPxGridView ID="gridView1" runat="server" AutoGenerateColumns="False" 
                                                    ClientInstanceName="gridView1" KeyFieldName="BalansId" Width="100%" 
                                                    OnHtmlRowPrepared="gridView1_HtmlRowPrepared">
                                                    <Columns>
                                                        <%--<dx:GridViewCommandColumn ShowInCustomizationForm="True" 
                                                            ShowSelectCheckbox="True" VisibleIndex="0">
                                                        </dx:GridViewCommandColumn>--%>
                                                        <dx:GridViewCommandColumn 
                                                            ShowSelectCheckbox="True" VisibleIndex="0"  />
                                                        <dx:GridViewDataTextColumn FieldName="OrganizationId" Visible="false"
                                                            ShowInCustomizationForm="True" VisibleIndex="1">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="BalansId" ShowInCustomizationForm="True" Visible="false"
                                                            VisibleIndex="2">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Street" Caption="Вулиця" ShowInCustomizationForm="True" 
                                                            VisibleIndex="3">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Number" Caption="Номер" ShowInCustomizationForm="True" 
                                                            VisibleIndex="4">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="SqrTotal" Caption="Площа" ShowInCustomizationForm="True" 
                                                            VisibleIndex="5">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Purpose" Caption="Призначення" ShowInCustomizationForm="True" 
                                                            VisibleIndex="6">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="ReportId" ShowInCustomizationForm="True" Visible="false"
                                                            VisibleIndex="7">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <Settings ShowFilterRow="true" ShowFilterBar="Auto" />
                                                    <SettingsBehavior AllowSelectByRowClick="True" />
                                                    <ClientSideEvents Init="OnGridViewInit1" SelectionChanged="OnGridViewSelectionChanged1" EndCallback="OnGridViewEndCallback1" />
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width:680px">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Width="150px"
                                                    CausesValidation="False" ClientInstanceName="BtnFindOrgFrom" 
                                                    Text="&lt;&lt; Прийняти об'єкти" Wrap="False">
                                                    <ClientSideEvents Click="function (s, e) { ContentCallback.PerformCallback('sendselected2:'); }" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td align="right">
<%--                                                <dx:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="False" Width="110px"
                                                    CausesValidation="False" ClientInstanceName="BtnFindOrgFrom" Text="Вибрати все" Wrap="False">
                                                    <ClientSideEvents Click="function (s, e) { ContentCallback.PerformCallback('selectall2:'); }" />
                                                </dx:ASPxButton>--%>

                                                <dx:ASPxLabel ID="lblInfo2" ClientInstanceName="lblInfo2" runat="server" />
                                                &nbsp;
                                                <dx:ASPxHyperLink ID="lnkClearSelection2" ClientInstanceName="lnkClearSelection2" 
                                                    Text="Зняти виділення" runat="server" Cursor="pointer" 
                                                    ClientSideEvents-Click="OnUnselectAllRowsLinkClick2" ClientVisible="false"  >
                                                    <ClientSideEvents Click="OnUnselectAllRowsLinkClick2"></ClientSideEvents>
                                                </dx:ASPxHyperLink>
                                                |
                                                <dx:ASPxHyperLink ID="lnkSelectAllRows2" ClientInstanceName="lnkSelectAllRows2" 
                                                    Text="Виділити все" runat="server" Cursor="pointer" 
                                                    ClientSideEvents-Click="OnSelectAllRowsLinkClick2" >
                                                    <ClientSideEvents Click="OnSelectAllRowsLinkClick2"></ClientSideEvents>
                                                </dx:ASPxHyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxGridView ID="gridView2" runat="server" AutoGenerateColumns="False" 
                                                    ClientInstanceName="gridView2" KeyFieldName="BalansId" Width="100%" 
                                                    OnHtmlRowPrepared="gridView2_HtmlRowPrepared">

	                                                <SettingsCommandButton>
		                                                <EditButton>
			                                                <Image Url="~/Styles/EditIcon.png" />
		                                                </EditButton>
		                                                <CancelButton>
			                                                <Image Url="~/Styles/CancelIcon.png" />
		                                                </CancelButton>
		                                                <UpdateButton>
			                                                <Image Url="~/Styles/SaveIcon.png" />
		                                                </UpdateButton>
		                                                <DeleteButton>
			                                                <Image Url="~/Styles/DeleteIcon.png" />
		                                                </DeleteButton>
		                                                <NewButton>
			                                                <Image Url="~/Styles/AddIcon.png" />
		                                                </NewButton>
		                                                <ClearFilterButton Text="Очистити" RenderMode="Link" />
	                                                </SettingsCommandButton>

                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowInCustomizationForm="True" 
                                                            ShowSelectCheckbox="True" VisibleIndex="0">
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataTextColumn FieldName="OrganizationId" Visible="false"
                                                            ShowInCustomizationForm="True" VisibleIndex="1">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="BalansId" ShowInCustomizationForm="True" Visible="false"
                                                            VisibleIndex="2">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Street" Caption="Вулиця" ShowInCustomizationForm="True" 
                                                            VisibleIndex="3">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Number" Caption="Номер" ShowInCustomizationForm="True" 
                                                            VisibleIndex="4">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="SqrTotal" Caption="Площа" ShowInCustomizationForm="True" 
                                                            VisibleIndex="5">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Purpose" Caption="Призначення" ShowInCustomizationForm="True" 
                                                            VisibleIndex="6">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="ReportId" ShowInCustomizationForm="True" Visible="false"
                                                            VisibleIndex="7">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <Settings ShowFilterRow="true" ShowFilterBar="Auto" />
                                                    <SettingsBehavior AllowSelectByRowClick="True" />
                                                    <ClientSideEvents Init="OnGridViewInit2" SelectionChanged="OnGridViewSelectionChanged2" EndCallback="OnGridViewEndCallback2" />
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                                
                                </dx:PanelContent>
                            </PanelCollection>

                        </dx:ASPxCallbackPanel>

                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
    </td>
</tr>
<tr valign="top">
    <td>
            

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel11" runat="server" HeaderText="Розпорядчі документи" Width="100%">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server">
                                    <dx:ASPxCallbackPanel ID="CPCreateRozpDoc" ClientInstanceName="CPCreateRozpDoc" runat="server" OnCallback="CPCreateRozpDoc_Callback">
                                    <ClientSideEvents EndCallback="function (s, e) { PopupCreateRozp.Hide(); }" />
                                    <PanelCollection>
                                        <dx:PanelContent>

                                        <table width="100%" cellpadding="4px">
                                            <tr>
                                                <td align="right" width="120px">
                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Номер документу">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td colspan="3">

                                                    <dx:ASPxComboBox ID="ComboRozpDoc" runat="server" ClientInstanceName="ComboRozpDoc"
                                                        DropDownStyle="DropDown" ValueType="System.Int32"
                                                        ValueField="id" Width="100%" IncrementalFilteringMode="Contains"
                                                        EnableCallbackMode="True" CallbackPageSize="10" 
                                                        TextFormatString="{0}" FilterMinLength="1"
                                                        OnItemRequestedByValue="ComboRozpDoc_OnItemRequestedByValue" 
                                                        OnItemsRequestedByFilterCondition="ComboRozpDoc_OnItemsRequestedByFilterCondition"
                                                        TextField="doc_num" Value='<%# Eval("rishrozp_doc_id") %>' OnValidation="OnValidation" 
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
                                                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" 
                                                            >
                                                            <RequiredField IsRequired="true" ErrorText="Номер документу має бути заповнений" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <dx:ASPxButton ID="BtnCreateRozp" runat="server" Text="Створити розпорядчий документ" CausesValidation="false" AutoPostBack="false" Wrap="False"></dx:ASPxButton>
                                                </td>
                                                <td align="right">
                                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Дата прийняття" Wrap="False"></dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="DateEditRishDate" runat="server" 
                                                        ClientInstanceName="DateEditRishDate" ReadOnly="True" 
                                                        Value='<%# Eval("rishrozp_date") %>' Width="100%" 
                                                        OnValidation="OnValidation">
                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                            >
                                                            <RequiredField ErrorText="Дата прийняття має бути заповнена" />
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Назва документу" />
                                                </td>
                                                <td colspan="3">
                                                    <dx:ASPxMemo ID="TextBoxRishName" ClientInstanceName="TextBoxRishName" 
                                                        runat="server" Width="100%" Height="60px" Value='<%# Eval("rishrozp_name") %>' 
                                                        ReadOnly="true" OnValidation="OnValidation">
                                                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField ErrorText="Назва документу має бути заповнена" />
                                                        </ValidationSettings>
                                                    </dx:ASPxMemo>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Тип Документу">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ComboRishRozpType" runat="server" 
                                                        ClientInstanceName="ComboRishRozpType" DataSourceID="SqlDataSourceProjectTypes" 
                                                        TextField="name" ValueField="id" ValueType="System.Int32" Width="100%" 
                                                        OnValidation="OnValidation" ReadOnly="True">
                                                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField ErrorText="Тип Документу має бути заповнена" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td align="right">
                                                    <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Право">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ASPxComboBoxRight" runat="server" 
                                                        DataSourceID="SqlDataSourceRights" TextField="name" ValueField="id" 
                                                        ValueType="System.Int32" Width="100%" OnValidation="OnValidation">
                                                        <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" >
                                                            <RequiredField IsRequired="true" ErrorText="Право має бути заповнений" />
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>



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

                                        </dx:PanelContent>
                                    </PanelCollection>
                                    </dx:ASPxCallbackPanel>                               
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

    </td>
    <td>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanelAkt" runat="server" HeaderText="Акти прийому-передачі" Width="100%">
                            <ContentPaddings Padding="4px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server">
                                    <table width="100%" cellpadding="4px">
                                        <tr><td colspan="4" height="4px"/></tr>
                                        <tr>
                                            <td valign="top" align="right" width="100px"><dx:ASPxLabel ID="ASPxLabel62" runat="server" Text="Номер акту" /></td>
                                            <td>
                                                <dx:ASPxTextBox ID="ActNumber" runat="server" Width="100%" 
                                                    OnValidation="OnValidation">
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" 
                                                        >
                                                        <RequiredField IsRequired="true" ErrorText="Номер акту має бути заповнен" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>                                            
                                            </td>
                                            <td align="right" width="100px">
                                                <dx:ASPxLabel ID="ASPxLabel63" runat="server" Text="Дата акту">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="ActDate" runat="server" 
                                                    Width="100%" OnValidation="OnValidation" >
                                                    <ValidationSettings SetFocusOnError="true" ErrorDisplayMode="ImageWithTooltip" 
                                                        >
                                                        <RequiredField IsRequired="true" ErrorText="Дата акту має бути заповнена" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr><td colspan="4" height="8px"/></tr>
<%--                                        <tr>
                                            <td valign="top" align="right">
                                                <dx:ASPxLabel ID="ASPxLabel64" runat="server" Text="Сума за актом" />
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="ActSum" runat="server" Width="100%" 
                                                    DisplayFormatString="C" DecimalPlaces="2" >
                                                    <ClientSideEvents NumberChanged="OnNumberChanged" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <td align="right">
                                                <dx:ASPxLabel ID="ASPxLabel65" runat="server" Text="Залишкова сума" />
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="ActResidualSum" runat="server" Width="100%" 
                                                    DisplayFormatString="C" DecimalPlaces="2" >
                                                    <ClientSideEvents NumberChanged="OnNumberChanged" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                        </tr>
                                        <tr><td colspan="4" height="4px"/></tr>--%>
                                    </table>    
                                </dx:PanelContent>                            
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        <br />
                        <table width="100%" cellpadding="4px">
                        <tr>
                            <td>
                                <dx:ASPxButton runat="server" AutoPostBack="False" ClientInstanceName="ButtonChooseBalansObj" 
                                    CausesValidation="true" Text="Виконати передачу об'єктів" Width="200px" 
                                    ID="ASPxButton6">
                                    <ClientSideEvents Click="function (s, e) { ContentCallback.PerformCallback('transfer:'); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>
                            <td align="right">
                                <dx:ASPxButton runat="server" AutoPostBack="False" ClientInstanceName="ButtonChooseBalansObj" 
                                    CausesValidation="False" Text="Відмінити" Width="200px" ID="ASPxButton7">
                                    <ClientSideEvents Click="function (s, e) { ContentCallback.PerformCallback('cancel:'); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>
                        </tr>
                        </table>


    </td>
</tr>
</table>


</asp:Content>