<%@ control language="C#" autoeventwireup="true" inherits="RishProjectTableEditor, App_Web_rishprojecttableeditor.ascx.23e82b75" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>
<%@ Register src="RishProjectTableObjectEditor.ascx" tagname="RishProjectTableObjectEditor" tagprefix="uc1" %>
<%@ Register src="RishProjectTableOrgFromEditor.ascx" tagname="RishProjectTableOrgFromEditor" tagprefix="uc1" %>
<%@ Register src="RishProjectTableOrgToEditor.ascx" tagname="RishProjectTableOrgToEditor" tagprefix="uc1" %>

<asp:SqlDataSource ID="SqlDataSourceColumnTypes" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_rish_table_column order by name">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceRights" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_obj_rights order by name">
</asp:SqlDataSource>

<dx:ASPxCallbackPanel ID="CPTableProperties" ClientInstanceName="CPTableProperties"
    runat="server" OnCallback="CPTableProperties_Callback">
    <PanelCollection>
        <dx:PanelContent ID="Panelcontent4" runat="server">

            <dx:ASPxRadioButton ID="RadioButtonDoc" runat="server" ClientInstanceName="RadioButtonDoc" Text="Створити таблицю використовуючи наступну інформацію:">
                <ClientSideEvents CheckedChanged="function (s,e) { CPTableProperties.PerformCallback('enabledoc:'); }" />
            </dx:ASPxRadioButton>

            <br />

            <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <dx:ASPxButton ID="BtnAddRow" ClientInstanceName="BtnAddRow" runat="server" Text="Додати рядок"
                            Width="160px" AutoPostBack="False">
                            <ClientSideEvents Click="function (s,e) { GridViewTable.PerformCallback('addrow:'); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <dx:ASPxPopupControl ID="PopupEditColumns" runat="server" HeaderText="Конфігурація Таблиці"
                            ClientInstanceName="PopupEditColumns" PopupElementID="BtnEditColumns" 
                            AllowDragging="True" PopupHorizontalAlign="WindowCenter" 
                            PopupVerticalAlign="WindowCenter">
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                                    <table border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Колонки в таблиці" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Колонки що були використані раніше" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <dx:ASPxTreeList ID="TreeListColumns" ClientInstanceName="TreeListColumns" runat="server"
                                                    AutoGenerateColumns="False" Width="300px" KeyFieldName="id"
                                                    OnCustomCallback="TreeListColumns_CustomCallback">
                                                    <Columns>
                                                        <dx:TreeListTextColumn FieldName="ordinal_pos" VisibleIndex="0" SortIndex="0" 
                                                            SortOrder="Ascending" Visible="False">
                                                        </dx:TreeListTextColumn>
                                                        <dx:TreeListTextColumn AllowSort="False" Caption="Назва Колонки" 
                                                            FieldName="column_name" ShowInCustomizationForm="True" VisibleIndex="0" 
                                                            Width="500px">
                                                        </dx:TreeListTextColumn>
                                                        <dx:TreeListCommandColumn VisibleIndex="1" ButtonType="Image">
                                                            <CustomButtons>
                                                                <dx:TreeListCommandColumnCustomButton ID="ColMoveUp" Text="Перемістити Вверх">
                                                                    <Image ToolTip="Перемістити Вверх" Url="../Styles/MoveUpIcon.png" />
                                                                </dx:TreeListCommandColumnCustomButton>
                                                                <dx:TreeListCommandColumnCustomButton ID="ColMoveDown" Text="Перемістити Вниз">
                                                                    <Image ToolTip="Перемістити Вниз" Url="../Styles/MoveDownIcon.png" />
                                                                </dx:TreeListCommandColumnCustomButton>
                                                                <dx:TreeListCommandColumnCustomButton ID="ColDelete" Text="Видалити">
                                                                    <Image ToolTip="Видалити" Url="../Styles/DeleteIcon.png" />
                                                                </dx:TreeListCommandColumnCustomButton>
                                                            </CustomButtons>
                                                        </dx:TreeListCommandColumn>
                                                    </Columns>
                                                    <Settings ShowTreeLines="False" />
                                                    <SettingsBehavior FocusNodeOnLoad="false" ExpandCollapseAction="NodeDblClick" 
                                                        AllowFocusedNode="False" />
                                                    <SettingsEditing AllowNodeDragDrop="False" Mode="PopupEditForm"></SettingsEditing>
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
                                                    <ClientSideEvents CustomButtonClick="function (s,e) {
if (e.buttonID == 'ColMoveUp') { CPTableProperties.PerformCallback('moveup:' + e.nodeKey); }
if (e.buttonID == 'ColMoveDown') { CPTableProperties.PerformCallback('movedown:' + e.nodeKey); }
if (e.buttonID == 'ColDelete') {
    if (confirm('Вилучити колонку з таблиці?')) {
        CPTableProperties.PerformCallback('delcol:' + e.nodeKey);
    }
}
}" />
                                                </dx:ASPxTreeList>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxButton ID="BtnAddColumnToTable" ClientInstanceName="BtnAddColumnToTable"
                                                    runat="server" Text="<<" AutoPostBack="False" 
                                                    ToolTip="Додати вибрану колонку до таблиці">
                                                    <ClientSideEvents Click="function (s,e) { 
var selectedColumn = ListBoxColumnType.GetSelectedItem();
if (selectedColumn != null) {
    CPTableProperties.PerformCallback('addcol:' + selectedColumn.value); 
}
}" />
                                                </dx:ASPxButton>
                                                <br />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <table border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxTextBox ID="TextBoxColumnName" runat="server" Width="270px" ClientInstanceName="TextBoxColumnName">
                                                                <ClientSideEvents KeyUp="function(s, e) {
var filterString = TextBoxColumnName.GetText();

if (filterString != ListBoxColumnType.cpFilterString) {
	ListBoxColumnType.PerformCallback('filter:' + filterString);
}
}" />
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <dx:ASPxButton ID="BtnAddNewColumn" ClientInstanceName="BtnAddNewColumn" runat="server"
                                                                Text="+" AutoPostBack="False" 
                                                                ToolTip="Створити нову колонку та додати ії до таблиці">
                                                                <ClientSideEvents Click="function (s,e) {
var newColumnName = TextBoxColumnName.GetText().replace(/^\s+|\s+$/g, '');

if (newColumnName.length == 0) {
	alert('Введіть назву колонки для створення');
	return;
}

if (confirm('Ця дія створить нову колонку за ім\'ям &quot;' + TextBoxColumnName.GetText() + '&quot;.\nПродовжити?')) {
	CPTableProperties.PerformCallback('newcol:' + newColumnName);
}
}" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" height="4px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <dx:ASPxListBox ID="ListBoxColumnType" runat="server" ValueType="System.Int32" TextField="name"
                                                                ValueField="id" Width="100%" Height="250px" EnableCallbackMode="True" EnableClientSideAPI="True"
                                                                OnCallback="ListBoxColumnType_Callback" ClientInstanceName="ListBoxColumnType">
                                                            </dx:ASPxListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="OK" AutoPostBack="False">
                                        <ClientSideEvents Click="function (s, e) { PopupEditColumns.Hide(); }" />
                                    </dx:ASPxButton>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxButton ID="BtnEditColumns" ClientInstanceName="BtnEditColumns" runat="server"
                            Text="Редагувати колонки" Width="160px" AutoPostBack="False" />

                        <dx:ASPxPopupControl ID="PopupNewColumn" runat="server" HeaderText="Нова колонка"
                            PopupElementID="BtnCreateNewColumn">
                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Назва:">
                                    </dx:ASPxLabel>
                                    <dx:ASPxTextBox ID="TextBoxNewColumnName" runat="server" ClientInstanceName="TextBoxNewColumnName"
                                        Width="170px">
                                    </dx:ASPxTextBox>
                                    <br />
                                    <div style="float: right;">
                                        <dx:ASPxButton ID="BtnCreateNewColumnSubmit" runat="server" Text="Створити" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) { CPTableProperties.PerformCallback('newcol:' + TextBoxNewColumnName.GetText()); }" />
                                        </dx:ASPxButton>
                                    </div>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>
                    </td>
                </tr>
            </table>

            <br />

            <dx:ASPxGridView ID="GridViewTable" runat="server" ClientInstanceName="GridViewTable"
                AutoGenerateColumns="True" KeyFieldName="id" Width="920px" 
                EnableRowsCache="False" OnDataBound="GridViewTable_DataBound" 
                OnRowDeleting="GridViewTable_RowDeleting" 
                OnRowUpdating="GridViewTable_RowUpdating" 
                OnStartRowEditing="GridViewTable_StartRowEditing" 
                OnCancelRowEditing="GridViewTable_CancelRowEditing" 
                OnCellEditorInitialize="GridViewTable_CellEditorInitialize" 
                OnCustomCallback="GridViewTable_CustomCallback">
                <SettingsBehavior EnableCustomizationWindow="False" AutoFilterRowInputDelay="2500"
                    ColumnResizeMode="Control" AllowDragDrop="false" ConfirmDelete="True" 
                    EnableRowHotTrack="True" />
                <SettingsPager PageSize="10">
                </SettingsPager>
                <SettingsPopup>
                    <HeaderFilter Width="200" Height="300" />
                </SettingsPopup>
                <Settings ShowFilterRow="False" ShowFilterRowMenu="False" ShowGroupPanel="False"
                    ShowFilterBar="Hidden" ShowHeaderFilterButton="False" HorizontalScrollBarMode="Visible"
                    ShowFooter="False" VerticalScrollBarMode="Hidden" />
                <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                <SettingsCookies CookiesID="GUKV.RishProject.TableEditor" Version="A2_1" />
                <Styles Header-Wrap="True">
                    <Header Wrap="True">
                    </Header>
                </Styles>
            </dx:ASPxGridView>

            <br/>

            <dx:ASPxRadioButton ID="RadioButtonFile" runat="server" ClientInstanceName="RadioButtonFile" Text="Підключити зовнішній документ:">
                <ClientSideEvents CheckedChanged="function (s,e) { CPTableProperties.PerformCallback('enableattach:'); }" />
            </dx:ASPxRadioButton>

            <br />

            <div class="data-field-label" style="display: inline;">
                <dx:ASPxLabel ID="LabelFile" ClientInstanceName="LabelFile" runat="server" Text="Зовнішній документ:"/>
            </div>

            <dx:ASPxHyperLink ID="LinkFileName" ClientInstanceName="LinkFileName" runat="server" Text="не завантажено" Target="_blank" NavigateUrl="" />

            <br />

            <dx:ASPxUploadControl ID="UploadFile" ClientInstanceName="UploadFile" runat="server" Width="920px"
                ShowProgressPanel="True" OnFileUploadComplete="UploadFile_FileUploadComplete" ShowClearFileSelectionButton="False">
                <ValidationSettings AllowedFileExtensions=".docx"></ValidationSettings>
                <ClientSideEvents
FileUploadComplete="function(s, e) {
	var data = $.parseJSON(e.callbackData);
    if (data != null) {
	    LinkFileName.SetText(data.OriginalFileName);
        LinkFileName.SetNavigateUrl(data.ViewDocumentUrl);
        $('#OrigFileName').val(data.OriginalFileName);
        $('#TempFileName').val(data.TempFileName);
    } else {
	    LinkFileName.SetText('не завантажено');
        LinkFileName.SetNavigateUrl('');
        $('#OrigFileName').val('');
        $('#TempFileName').val('');
    }
}"
TextChanged="function(s, e) { UploadFile.Upload(); }"
                />
            </dx:ASPxUploadControl>

            <asp:HiddenField ID="OrigFileName" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="TempFileName" ClientIDMode="Static" runat="server" />

            <br />

        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<dx:ASPxTreeListTemplateReplacement ID="Replacement1" runat="server" ReplacementType="UpdateButton" />
<dx:ASPxTreeListTemplateReplacement ID="Replacement2" runat="server" ReplacementType="CancelButton" />
