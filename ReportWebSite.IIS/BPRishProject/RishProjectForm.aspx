<%@ page language="C#" autoeventwireup="true" inherits="BPRishProject_RishProjectForm, App_Web_rishprojectform.aspx.23e82b75" masterpagefile="~/NoMenu.master" title="Проект Рішення" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>
<%@ Register src="RishProjectItemEditor.ascx" tagname="RishProjectItemEditor" tagprefix="uc4" %>
<%@ Register src="RishProjectMainDocEditor.ascx" tagname="RishProjectMainDocEditor" tagprefix="uc5" %>
<%@ Register src="RishProjectAppendixEditor.ascx" tagname="RishProjectAppendixEditor" tagprefix="uc6" %>
<%@ Register src="RishProjectTableEditor.ascx" tagname="RishProjectTableEditor" tagprefix="uc7" %>

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

        if (e.buttonID == "DocAddAppendixButton") {
            TreeListDoc.PerformCallback("addappx:" + e.nodeKey);
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

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRishStates" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_rish_project_state ORDER BY id" >
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceHistory" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT hist.*, usrMod.UserName AS 'modified_usr'
        FROM bp_rish_project_history hist
        LEFT OUTER JOIN aspnet_Users usrMod ON usrMod.UserId = hist.modified_by
        WHERE hist.project_id = @projid ORDER BY hist.id" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="projid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceProjectTypes" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_rish_project_type ORDER BY id">
</mini:ProfiledSqlDataSource>

<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" ActiveTabIndex="0">
    <TabPages>
        <dx:TabPage Text="Проект Розпорядчого Документу" Name="RishCardProperties">
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
            <dx:ASPxComboBox ID="ComboProjectState" ClientInstanceName="ComboProjectState" runat="server" Width="290px"
                DataSourceID="SqlDataSourceRishStates" ValueField="id" ValueType="System.Int32" TextField="name" />
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
                <dx:TreeListCommandColumnCustomButton ID="DocAddAppendixButton" Text="Створити Додаток"> <Image ToolTip="Створити Додаток" Url="../Styles/AddAttachmentIcon.png" /> </dx:TreeListCommandColumnCustomButton>
                <dx:TreeListCommandColumnCustomButton ID="DocAddTable" Text="Створити Таблицю"> <Image ToolTip="Створити Таблицю" Url="../Styles/AddTableIcon.png" /> </dx:TreeListCommandColumnCustomButton>
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
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Проект Створено"></dx:ASPxLabel></td>
        <td width="8px">&nbsp;</td>
        <td><dx:ASPxTextBox ID="TextCreatedBy" runat="server" ReadOnly="true" Width="290px" /></td>
        <td width="8px">&nbsp;</td>
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Дата Створення"></dx:ASPxLabel></td>
        <td width="8px">&nbsp;</td>
        <td><dx:ASPxDateEdit ID="DateCreatedOn" runat="server" ReadOnly="true" Width="290px" /></td>
    </tr>
    <tr><td colspan="7" height="4px"/></tr>
    <tr>
        <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Проект Змінено"></dx:ASPxLabel></td>
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

        <dx:TabPage Text="Історія Змін" Name="RishCardHistory" ClientVisible="False">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl5" runat="server">

<%-- Content of the fourth tab BEGIN --%>

<dx:ASPxGridView ID="GridViewHistory" ClientInstanceName="GridViewHistory" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceHistory" KeyFieldName="id" Width="810px" OnCustomCallback="GridViewHistory_CustomCallback" >

    <Columns>
        <dx:GridViewDataDateColumn FieldName="modified_usr" VisibleIndex="0" Caption="Дата Внесення Змін" Width="100px"></dx:GridViewDataDateColumn >
        <dx:GridViewDataMemoColumn FieldName="modified_by" VisibleIndex="1" Caption="Ким Внесені Зміни" Width="100px"></dx:GridViewDataMemoColumn >
        <dx:GridViewDataMemoColumn FieldName="change_text" VisibleIndex="2" Caption="Перелік Змін" Width="600px"></dx:GridViewDataMemoColumn >
    </Columns>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="False" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="False" />
    <SettingsPager PageSize="20" Mode="ShowAllRecords" />
    <Styles Header-Wrap="True" >
<Header Wrap="True"></Header>
    </Styles>
    <SettingsCookies CookiesID="GUKV.RishProjectHistory" Enabled="False" Version="A2" />
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
            <dx:ASPxButton ID="ButtonSaveRishProject" ClientInstanceName="ButtonSaveRishProject" runat="server" Text="Зберегти Зміни" Width="148px"
                OnClick="ButtonSaveRishProject_Click" />
        </td>
        <td width="8px">&nbsp;</td>
        <td>
            <dx:ASPxButton ID="ButtonExportRishProject" ClientInstanceName="ButtonExportRishProject" runat="server" Text="Зберегти у Файл" Width="148px"
                OnClick="ButtonExportRishProject_Click" />
        </td>
        <td width="8px">&nbsp;</td>
        <td>
            <dx:ASPxButton ID="ButtonExportToNJF" ClientInstanceName="ButtonExportToNJF" runat="server" Text="Занести в БД 'Розпорядження'" Width="230px" AutoPostBack="false">
                <ClientSideEvents Click="function (s,e) { CPFormButtons.PerformCallback('export:'); }" />
            </dx:ASPxButton>
        </td>
    </tr>
</table>

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>



</asp:Content>
