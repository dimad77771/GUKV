<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RishProjectForm.aspx.cs" Inherits="BPRishProject_RishProjectForm"
    MasterPageFile="~/NoMenu.master" Title="Розпорядчий Документ" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>
<%@ Register src="RishProjectItemEditor.ascx" tagname="RishProjectItemEditor" tagprefix="uc4" %>
<%@ Register src="RishProjectMainDocEditor.ascx" tagname="RishProjectMainDocEditor" tagprefix="uc5" %>
<%@ Register src="RishProjectAppendixEditor.ascx" tagname="RishProjectAppendixEditor" tagprefix="uc6" %>
<%@ Register src="RishProjectTableEditor.ascx" tagname="RishProjectTableEditor" tagprefix="uc7" %>

<%@ Register src="../UserControls/CheckComboBox.ascx" tagname="CheckComboBox" tagprefix="uc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
    .TreeListDocCmd img
    {
        visibility: hidden;
    }
    .TreeListDocRow:hover .TreeListDocCmd img
    {
        visibility: visible;
    }
    .dxtlDisabled_DevEx .TreeListDocRow:hover .TreeListDocCmd img
    {
        visibility: hidden;
    }
    .data-field-label
    {
    }
    .data-field-value
    {
        margin-left: 10px;
        margin-top: 6px;
    }
    .data-field-value .browse-button
    {
        float: right;
    }
    .data-field-label.inline
    {
        display: inline-block;
        vertical-align: bottom;
        width: 160px;
        height: 23px;
    }
    .data-field-value.inline
    {
        display: inline-block;
        width: 690px;
    }
    .data-field-label label
    {
        font-weight: bold;
    }
    .para-spacing
    {
    	padding: 0;
    	margin-top: 0;
    	margin-bottom: 4px;
    }
</style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" src="../Scripts/CommonScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    var ProjectID = <%= ProjectID %>;

    function InitializeActiveTabGrid(s, e) {

        var activeTabName = CardPageControl.GetActiveTab().name;

        if (activeTabName == 'RishCardHistory') {
            GridViewHistory.PerformCallback("");
        }
    }

    function TreeListDoc_CustomButtonClick(s, e) {

        if (e.buttonID == "DocAddItemButton") {
            TreeListDoc.PerformCallback("additem:" + e.nodeKey);
        }

        if (e.buttonID == "DocDelNodeButton") {
            if (confirm('Видалити елемент?')) {
                TreeListDoc.PerformCallback("delitem:" + e.nodeKey);
            }
        }

        if (e.buttonID == "DocAddTable") {
            TreeListDoc.PerformCallback("addtble:" + e.nodeKey);
        }
    }

    var workflowStates = {
        initialStateIDs: <%= WorkflowStates.Where(x => x.flags == 0).Select(x => x.id).ToJSON() %>,
        finalStateIDs: <%= WorkflowStates.Where(x => (x.flags & 4) == 4).Select(x => x.id).ToJSON() %>,
        intermediaryStateIDs: <%= WorkflowStates.Where(x => (x.flags & 1) == 1).Select(x => x.id).ToJSON() %>,
        coverLetterStateIDs: <%= WorkflowStates.Where(x => (x.flags & 2) == 2).Select(x => x.id).ToJSON() %>,
    };
    var originalWorkflowStates = [<%= ComboProjectState.Value %>];

    function ComboProjectState_SelectionChanged(s, e) {
        <%-- Reset selection of mutually exclusive workflow states --%>
        if (e.removedValues.length > 0) {
            if (ComboProjectState.GetSelectedValues().length == 0) {
                ComboProjectState.SelectValues(workflowStates.initialStateIDs);
            }
        }
        else if (e.addedValues.length > 0) {
            if (workflowStates.initialStateIDs.indexOf(parseInt(e.addedValues[0])) >= 0) {
                // Leave only the initial state(s) selected
                ComboProjectState.UnselectValues(
                    ComboProjectState.GetSelectedValues().filter(function (x) { 
                        return workflowStates.initialStateIDs.indexOf(parseInt(x)) < 0;
                    })
                );
            }
            else if (workflowStates.finalStateIDs.indexOf(parseInt(e.addedValues[0])) >= 0) {
                // Leave only the final state selected
                ComboProjectState.UnselectValues(
                    ComboProjectState.GetSelectedValues().filter(function (x) {
                        return x != parseInt(e.addedValues[0]);
                    })
                );
            }
            else {
                // Leave only the intermediary state(s) selected
                ComboProjectState.UnselectValues(
                    ComboProjectState.GetSelectedValues().filter(function (x) {
                        return workflowStates.intermediaryStateIDs.indexOf(parseInt(x)) < 0;
                    })
                );
            }
        }
        ComboProjectStateCallback.PerformCallback(JSON.stringify(e));
    }

    var overwriteDocument = false;

    function ButtonSaveRishProject_Validate(s, e) {
        if (CoverLettersGrid.cpMissingRequiredData) {
            alert('Будь ласка заповніть обов\'язкові поля супровідних документів');

            e.cancel = true;
            e.processOnServer = false;
        }

        if (!overwriteDocument
            && (TextBoxRishNum.IsValueChanged() || DateEditRishDate.IsValueChanged())
            && TextBoxRishNum.GetText().length > 0 
            && DateEditRishDate.GetText().length > 0) {

            e.cancel = true;
            e.processOnServer = false;

            CallbackCheckDocumentIdentity.PerformCallback(JSON.stringify({
                TextBoxRishNum: TextBoxRishNum.GetText(),
                DateEditRishDate: DateEditRishDate.GetDate()
            }));
        }
    }

    function CallbackCheckDocumentIdentity_Complete(s, e) {
        var result = JSON.parse(e.result);
        if (result.AlreadyExists) {
            if (!confirm('Документ за номером ' + TextBoxRishNum.GetText() + ' датований ' 
                + DateEditRishDate.GetText() + '\nвже існує. Перезаписати існуючий документ?')) {
                return;
            }
        }
        overwriteDocument = true;
        ButtonSaveRishProject.DoClick();
    }

    var ObjectRightsConfig = {
        EnableOrgFrom: <%= ObjectRights.Where(x => x.enable_org_from).Select(x => x.id).ToArray().ToJSON() %>,
        EnableOrgTo: <%= ObjectRights.Where(x => x.enable_org_to).Select(x => x.id).ToArray().ToJSON() %>,
    };

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRishStates" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_rish_project_state WHERE (flags & 8) = 0 ORDER BY id" >
    
    </mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceHistory" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select bp_rish_project_state.*, aspnet_Users.UserName entered_by_username, 
	dict_rish_project_state.name state_name
from bp_rish_project_state
inner join aspnet_Users on aspnet_Users.UserId = bp_rish_project_state.entered_by
inner join dict_rish_project_state on dict_rish_project_state.id = bp_rish_project_state.state_id
where project_id=@projid
order by entered_on desc
" >
    
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="projid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceProjectTypes" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_rish_project_type ORDER BY id">
    
    </mini:ProfiledSqlDataSource>

<dx:ASPxCallback ID="CallbackCheckDocumentIdentity" runat="server" 
        ClientInstanceName="CallbackCheckDocumentIdentity" 
        oncallback="CallbackCheckDocumentIdentity_Callback">
    <ClientSideEvents CallbackComplete="CallbackCheckDocumentIdentity_Complete" />
</dx:ASPxCallback>

<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" 
        runat="server" ActiveTabIndex="0">
    <TabPages>
        <dx:TabPage Text="Розпорядчий Документ" Name="RishCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server">

<%-- Content of the first tab BEGIN --%>

<table border="0" cellspacing="0" cellpadding="0" width="810px">
    <tr>
        <td width="100px" valign="top"><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Назва Документа"></dx:ASPxLabel></td>
        <td width="8px">&nbsp;</td>
        <td colspan="5"><dx:ASPxMemo ID="TextBoxRishName" ClientInstanceName="TextBoxRishName" runat="server" Width="700px" Height="80px" /></td>
    </tr>
    <tr><td colspan="7" height="4px"/></tr>
    <tr>
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel0" runat="server" Text="Номер Затвердженого Документа"></dx:ASPxLabel></td>
        <td width="8px">&nbsp;</td>
        <td><dx:ASPxTextBox ID="TextBoxRishNum" ClientInstanceName="TextBoxRishNum" runat="server" Width="290px" /></td>
        <td width="8px">&nbsp;</td>
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel00" runat="server" Text="Дата Затвердження Документа"></dx:ASPxLabel></td>
        <td width="8px">&nbsp;</td>
        <td><dx:ASPxDateEdit ID="DateEditRishDate" ClientInstanceName="DateEditRishDate" runat="server" Width="290px" /></td>
    </tr>
    <tr><td colspan="7" height="4px"/></tr>
    <tr>
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Стан"></dx:ASPxLabel></td>
        <td width="8px">&nbsp;</td>
        <td>
            <dx:ASPxCallback ID="ComboProjectStateCallback" runat="server" 
                ClientInstanceName="ComboProjectStateCallback" 
                OnCallback="ComboProjectStateCallback_Callback">
                <ClientSideEvents CallbackComplete="function(s, e) {
    var result = JSON.parse(e.result);
    if (result.RefreshCoverLetterGrid) {
        CoverLettersGridPanel.SetVisible(result.ShowCoverLetterGrid);
        CoverLettersGrid.Refresh();
    }
}" />
            </dx:ASPxCallback>
            <uc1:CheckComboBox ID="ComboProjectState" ClientInstanceName="ComboProjectState" runat="server" Width="290px"
                DataSourceID="SqlDataSourceRishStates" ValueField="id" ValueType="System.Int32" TextField="name" 
                DropDownHeight="300px">
                <%-- 
                    The following code works well at run-time. It is only a problem with the designer.
                    If you would like use the designer, comment the ClientSideEvents element out, use
                    the designer, and then place ClientSideEvents back into the markup.
                    ------------------------------------------------------------------------------------
                    Visual Studio 2010 has a problem with UserControls having content properties.
                    IT IS A WELL-KNOWN *UNRESOLVED* ISSUE, DON'T WASTE YOUR TIME TRYING TO FIND A WORKAROUND!
                    http://stackoverflow.com/questions/11658053/how-to-add-a-templating-to-a-usercontrol
                --%>
                <ClientSideEvents SelectionChanged="ComboProjectState_SelectionChanged" />
            </uc1:CheckComboBox>
        </td>
        <td width="8px">&nbsp;</td>
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Тип Документу" /></td>
        <td width="8px">&nbsp;</td>
        <td>
            <dx:ASPxComboBox ID="ComboProjectType" ClientInstanceName="ComboProjectType" runat="server"
                DataSourceID="SqlDataSourceProjectTypes" TextField="name" ValueField="id" ValueType="System.Int32"
                Width="290px">
            </dx:ASPxComboBox>
        </td>
    </tr>
</table>

<br/>
<dx:ASPxPanel runat="server" Width="810px" ID="CoverLettersGridPanel" 
                        ClientInstanceName="CoverLettersGridPanel" EnableClientSideAPI="True">
    <PanelCollection>
        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
            <dx:ASPxGridView ID="CoverLettersGrid" runat="server" Width="100%" 
                AutoGenerateColumns="False" ClientInstanceName="CoverLettersGrid" 
                KeyFieldName="id" OnRowUpdating="CoverLettersGrid_RowUpdating" 
                OnHtmlDataCellPrepared="CoverLettersGrid_HtmlDataCellPrepared">

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
                    <dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="0" 
                        ShowEditButton="true" >
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn FieldName="id" ShowInCustomizationForm="True" 
                        Visible="False" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Стан" FieldName="state_name" 
                        ReadOnly="True" ShowInCustomizationForm="True" SortIndex="0" 
                        SortOrder="Ascending" VisibleIndex="2" Width="350px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Номер супровідного листа" 
                        FieldName="cover_letter_no" ShowInCustomizationForm="True" VisibleIndex="3">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="Дата супровідного листа" 
                        FieldName="cover_letter_date" ShowInCustomizationForm="True" 
                        VisibleIndex="4" Width="120px">
                    </dx:GridViewDataDateColumn>
                </Columns>
                <SettingsBehavior AllowSort="False" />
                <SettingsPager Mode="ShowAllRecords">
                </SettingsPager>
                <SettingsEditing Mode="Inline" />
            </dx:ASPxGridView>
            <br/>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>


<dx:ASPxTreeList ID="TreeListDoc" ClientInstanceName="TreeListDoc" runat="server" AutoGenerateColumns="False" Width="810px"
    KeyFieldName="ID" ParentFieldName="ParentID"
    OnProcessDragNode="TreeListDoc_ProcessDragNode"
    OnCustomCallback="TreeListDoc_CustomCallback"
    OnCommandColumnButtonInitialize="TreeListDoc_CommandColumnButtonInitialize"
    OnStartNodeEditing="TreeListDoc_StartNodeEditing"
    OnNodeUpdating="TreeListDoc_NodeUpdating" >
    <Columns>
        <dx:TreeListTextColumn FieldName="DisplayNodeType" VisibleIndex="0" Caption="Розділ" Width="100px"></dx:TreeListTextColumn>
        <dx:TreeListTextColumn FieldName="DisplayNumber" VisibleIndex="1" Caption="Номер Пункту" Width="100px"></dx:TreeListTextColumn>
        <dx:TreeListTextColumn FieldName="DisplayText" VisibleIndex="2" Caption="Вміст" Width="450px">
            <DataCellTemplate>
                <%# HttpUtility.HtmlDecode(Eval("DisplayText").ToString()) %>
            </DataCellTemplate>
        </dx:TreeListTextColumn>

        <dx:TreeListCommandColumn VisibleIndex="3" ButtonType="Image">
            <CustomButtons>
                <dx:TreeListCommandColumnCustomButton ID="DocAddItemButton" Text="Додати Пункт"> <Image ToolTip="Додати Пункт" Url="../Styles/AddIcon.png" /> </dx:TreeListCommandColumnCustomButton>
                <dx:TreeListCommandColumnCustomButton ID="DocAddTable" Text="Створити Додаток"> <Image ToolTip="Створити Додаток" Url="../Styles/AddTableIcon.png" /> </dx:TreeListCommandColumnCustomButton>
                <dx:TreeListCommandColumnCustomButton ID="DocDelNodeButton" Text="Видалити"> <Image ToolTip="Видалити" Url="../Styles/DeleteIcon.png" /> </dx:TreeListCommandColumnCustomButton>
            </CustomButtons>
            <EditButton Visible="True" Text="Редагувати"><Image Url="../Styles/EditIcon.png" ToolTip="Редагувати"></Image></EditButton>
            <UpdateButton Text="Зберегти"><Image Url="../Styles/AcceptIcon.png" ToolTip="Зберегти"></Image></UpdateButton>
            <CancelButton Text="Не зберігати"><Image Url="../Styles/CancelIcon.png" ToolTip="Не зберігати"></Image></CancelButton>
        </dx:TreeListCommandColumn>
    </Columns>
    <Settings ShowTreeLines="True" />
    <SettingsBehavior FocusNodeOnLoad="false" ExpandCollapseAction="NodeDblClick" 
        AllowFocusedNode="True" AllowSort="false" />
    <SettingsEditing AllowNodeDragDrop="True" Mode="PopupEditForm"> </SettingsEditing>
    <Styles>
        <Node CssClass="TreeListDocRow">
        </Node>
        <AlternatingNode CssClass="TreeListDocRow">
        </AlternatingNode>
        <FocusedNode CssClass="TreeListDocRow">
        </FocusedNode>
        <CommandCell CssClass="TreeListDocCmd">
        </CommandCell>
    </Styles>
    <ClientSideEvents
        CustomButtonClick="TreeListDoc_CustomButtonClick" />
</dx:ASPxTreeList>

<br/>

<table border="0" cellspacing="0" cellpadding="0" width="810px">
    <tr>
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Документ Створено"></dx:ASPxLabel></td>
        <td width="8px">&nbsp;</td>
        <td><dx:ASPxTextBox ID="TextCreatedBy" runat="server" ReadOnly="true" Width="290px" /></td>
        <td width="8px">&nbsp;</td>
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Дата Створення"></dx:ASPxLabel></td>
        <td width="8px">&nbsp;</td>
        <td><dx:ASPxDateEdit ID="DateCreatedOn" runat="server" ReadOnly="true" Width="290px" /></td>
    </tr>
    <tr><td colspan="7" height="4px"/></tr>
    <tr>
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Документ Змінено"></dx:ASPxLabel></td>
        <td width="8px">&nbsp;</td>
        <td><dx:ASPxTextBox ID="TextModifiedBy" runat="server" ReadOnly="true" Width="290px" /></td>
        <td width="8px">&nbsp;</td>
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Дата Зміни"></dx:ASPxLabel></td>
        <td width="8px">&nbsp;</td>
        <td><dx:ASPxDateEdit ID="DateModifiedOn" runat="server" ReadOnly="true" Width="290px" /></td>
    </tr>
</table>

<%-- Content of the first tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Історія Змін" Name="RishCardHistory">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl5" runat="server">

<%-- Content of the fourth tab BEGIN --%>

<dx:ASPxGridView ID="GridViewHistory" ClientInstanceName="GridViewHistory" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceHistory" KeyFieldName="id" Width="810px" OnCustomCallback="GridViewHistory_CustomCallback" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" 
            ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
            <EditFormSettings Visible="False" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="project_id" 
            ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="state_id" ShowInCustomizationForm="True" 
            Visible="False" VisibleIndex="2">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="entered_on" VisibleIndex="9" 
            Caption="Дата зміни" SortIndex="0" SortOrder="Descending">
        </dx:GridViewDataDateColumn >
        <dx:GridViewDataTextColumn FieldName="entered_by" 
            ShowInCustomizationForm="True" Visible="False" VisibleIndex="4">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="exited_on" ShowInCustomizationForm="True" 
            Visible="False" VisibleIndex="5">
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="exited_by" ShowInCustomizationForm="True" 
            Visible="False" VisibleIndex="6">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Номер супровідного листа" 
            FieldName="cover_letter_no" ShowInCustomizationForm="True" VisibleIndex="7">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn Caption="Дата супровідного листа" 
            FieldName="cover_letter_date" ShowInCustomizationForm="True" VisibleIndex="8">
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn Caption="Змінено користувачем" 
            FieldName="entered_by_username" ShowInCustomizationForm="True" 
            VisibleIndex="10">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Стан" FieldName="state_name" 
            ShowInCustomizationForm="True" VisibleIndex="3" Width="350px">
        </dx:GridViewDataTextColumn>
    </Columns>

    <SettingsBehavior AutoFilterRowInputDelay="2500" 
        EnableCustomizationWindow="False" />
    <SettingsPager PageSize="20" Mode="ShowAllRecords" />
    <Styles Header-Wrap="True" >
<Header Wrap="True"></Header>
    </Styles>
    <SettingsCookies CookiesID="GUKV.RishProjectHistory" Enabled="False" 
        Version="A3" />
</dx:ASPxGridView>

<%-- Content of the fourth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
    </TabPages>

    <ClientSideEvents ActiveTabChanged="InitializeActiveTabGrid" />
</dx:ASPxPageControl>

<br/>

<dx:ASPxCallbackPanel ID="CPFormButtons" ClientInstanceName="CPFormButtons" runat="server" OnCallback="CPFormButtons_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent2" runat="server">

<dx:ASPxLabel ID="LabelNJFExportError" ClientInstanceName="LabelNJFExportError" runat="server" Text="" ClientVisible="false" ForeColor="Red" />

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonSaveRishProject" 
                ClientInstanceName="ButtonSaveRishProject" runat="server" Text="Зберегти Зміни" Width="148px"
                OnClick="ButtonSaveRishProject_Click" >
                <ClientSideEvents Click="ButtonSaveRishProject_Validate" />
            </dx:ASPxButton>
        </td>
        <td width="8px">&nbsp;</td>
        <td>
            <dx:ASPxButton ID="ButtonExportRishProject" ClientInstanceName="ButtonExportRishProject" runat="server" Text="Зберегти у Файл" Width="148px"
                OnClick="ButtonExportRishProject_Click" />
        </td>
        <td width="8px">&nbsp;</td>
        <td>
            <dx:ASPxButton ID="ButtonCreateNewAct" ClientInstanceName="ButtonCreateNewAct" runat="server" 
                Text="Створити Акт" Width="148px" AutoPostBack="False" >
                <ClientSideEvents Click="function(s, e) {
	window.location.assign('NewAct.aspx?projid=' + ProjectID);
}" />
            </dx:ASPxButton>
<%--            <dx:ASPxButton ID="ButtonExportToNJF" ClientInstanceName="ButtonExportToNJF" runat="server" Text="Занести в БД 'Розпорядження'" Width="230px" AutoPostBack="false">
                <ClientSideEvents Click="function (s,e) { CPFormButtons.PerformCallback('export:'); }" />
            </dx:ASPxButton>--%>
        </td>
    </tr>
</table>

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>



</asp:Content>
