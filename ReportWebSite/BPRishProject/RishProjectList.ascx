<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RishProjectList.ascx.cs" Inherits="BPRishProject_RishProjectList" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<%@ Register src="../UserControls/CheckComboBox.ascx" tagname="CheckComboBox" tagprefix="uc1" %>

<style type="text/css">
    .TreeListDocCmd img
    {
        visibility: hidden;
    }
    .TreeListDocRow:hover .TreeListDocCmd img
    {
        visibility: visible;
    }
</style>
<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSize(); };

    function AdjustGridSize() {

        GridViewRishProjects.SetHeight(window.innerHeight - 200);
    }

    function GridViewRishProjectsEndCallback(s, e) {

        AdjustGridSize();
    }

    function OnAddNewProjectOrg(s, e) {
        if (typeof NewContactOrgName.GetText() != "string" || NewContactOrgName.GetText().length == 0) {
            alert("Будь ласка, введіть назву організації.");
            return false;
        }
        NewContactOrganization.PerformCallback(JSON.stringify({
            OrganizationName: NewContactOrgName.GetText()
        }));
        PopupNewProjectOrg.Hide();
        NewContactOrgName.SetValue("");
    }

    function OnAddNewProjectContact(s, e) {
        if (typeof NewContactName.GetText() != "string" || NewContactName.GetText().length == 0) {
            alert("Будь ласка, введіть ім'я відповідальної особи.");
            return false;
        }
        if (typeof NewContactOrganization.GetValue() != "string" || NewContactOrganization.GetValue().length == 0) {
            alert("Будь ласка, виберіть організацію.");
            return false;
        }
        NewRishContact.PerformCallback(JSON.stringify({
            OrganizationID: NewContactOrganization.GetValue(),
            ContactName: NewContactName.GetText(),
            ContactTitle: NewContactTitle.GetText(),
            ContactPhone: NewContactPhone.GetText()
        }));
        PopupNewProjectContact.Hide();
        NewContactOrganization.SetSelectedIndex(-1);
        NewContactName.SetValue("");
        NewContactTitle.SetValue("");
        NewContactPhone.SetValue("");
    }

    function OnAddNewRishProject(s, e) {

        if (typeof NewRishProjectName.GetText() != "string" || NewRishProjectName.GetText().length == 0) {
            alert("Будь ласка, введіть назву розпорядчого документу.");
            return;
        }
        if (typeof NewRishContact.GetValue() != "string" || NewRishContact.GetValue().length == 0) {
            alert("Будь ласка, виберіть особу відповідальну за проект розпорядчого документу.");
            return;
        }
        if (typeof NewRishProjectType.GetValue() != "string" || NewRishProjectType.GetValue().length == 0) {
            alert("Будь ласка, виберіть тип розпорядчого документу.");
            return;
        }

        GridViewRishProjects.PerformCallback(JSON.stringify({
            Name: NewRishProjectName.GetText(),
            ContactID: NewRishContact.GetValue(),
            DocTypeID: NewRishProjectType.GetValue()
        }));
        PopupAddRishProject.Hide();
        NewRishProjectName.SetText("");
        NewRishContact.SetSelectedIndex(-1);
        NewRishProjectType.SetSelectedIndex(-1);
    }

    function OnGridViewRishProjectsCustomButtonClick(s, e) {

        if (e.buttonID == "Edit") {

            var projectId = GridViewRishProjects.cpProjectIds[e.visibleIndex];

            var cardUrl = "../BPRishProject/RishProjectForm.aspx?projid=" + projectId;

            window.open(cardUrl);

            e.processOnServer = false;
        }
        else if (e.buttonID == "Delete") {

            if (confirm("Видалити проект розпорядчого документу?")) {
                e.processOnServer = true;
            }
            else {
                e.processOnServer = false;
            }
        }
        else if (e.buttonID == "Clone") {

            e.processOnServer = confirm("Зробити копію проекту розпорядчого документу?");
        }
    }

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRishProjects" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT proj.*, info.*, st.state_name AS 'project_state', t.name AS 'project_type',
                usrCrea.UserName AS 'created_usr', usrMod.UserName AS 'modified_usr',
                contact.contact_name, contact.contact_title, contact.contact_phone,
                org.name contact_org_name
        FROM bp_rish_project proj
        INNER JOIN bp_rish_project_info info ON info.project_id = proj.id
        INNER JOIN dict_rish_project_type t ON t.id = info.project_type_id
        INNER JOIN aspnet_Users usrCrea ON usrCrea.UserId = info.created_by
        INNER JOIN aspnet_Users usrMod ON usrMod.UserId = info.modified_by
        INNER JOIN dict_rish_project_org_contact contact ON contact.id = info.project_contact_id
        INNER JOIN dict_rish_project_org org ON org.id = contact.project_org_id
        LEFT JOIN bp_rish_project_view_state st ON st.project_id = proj.id
        " >
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelRishProjectListTitle" runat="server" Text="Розпорядчі Документи" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxPopupControl ID="PopupAddRishProject" runat="server" 
                HeaderText="Новий Проект Документу" 
                ClientInstanceName="PopupAddRishProject" 
                PopupElementID="ButtonShowNewRishProjectPopup" style="margin-left: 0px">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxLabel ID="LabelNewRishProjectName" runat="server" Text="Назва Документу:" />
                        <dx:ASPxTextBox ID="NewRishProjectName" ClientInstanceName="NewRishProjectName" runat="server" Width="360px" />
                        <br/>
                        <div style="float: right;">
                            <dx:ASPxLabel ID="LabelNewRishContact" runat="server" Text="Відповідальна Особа:" />
                            <dx:ASPxComboBox ID="NewRishContact" ClientInstanceName="NewRishContact" 
                                runat="server" ValueType="System.String" 
                                DataSourceID="SqlDataSourceProjectContact" TextField="contact_name" 
                                ValueField="contact_id" OnCallback="NewRishContact_Callback" 
                                TextFormatString="{1}">
                                <ClientSideEvents EndCallback="function(s, e) 
                                    { 
                                        if (s.cpErrorText == '')
                                            s.SetSelectedIndex(s.cpSelectedIndex); 
                                        else
                                            alert(s.cpErrorText);

                                    }" />
                                <Columns>
                                    <dx:ListBoxColumn Caption="Організація" FieldName="org_name" />
                                    <dx:ListBoxColumn FieldName="contact_id" Visible="False" />
                                    <dx:ListBoxColumn Caption="Відповідальна Особа" FieldName="contact_name" />
                                    <dx:ListBoxColumn Caption="Посада" FieldName="contact_title" />
                                    <dx:ListBoxColumn Caption="Телефон" FieldName="contact_phone" />
                                </Columns>
                            </dx:ASPxComboBox>
                            <asp:SqlDataSource ID="SqlDataSourceProjectContact" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                                SelectCommand="SELECT dict_rish_project_org.name AS org_name, dict_rish_project_org_contact.id AS contact_id, dict_rish_project_org_contact.contact_name, dict_rish_project_org_contact.contact_title, dict_rish_project_org_contact.contact_phone FROM dict_rish_project_org INNER JOIN dict_rish_project_org_contact ON dict_rish_project_org.id = dict_rish_project_org_contact.project_org_id">
                            </asp:SqlDataSource>
                            <div style="float: right;">
                                <dx:ASPxHyperLink ID="LinkNewRishContact" runat="server" Text="(ввести нову)">
                                </dx:ASPxHyperLink>
                            </div>
                        </div>
                        <dx:ASPxLabel ID="LabelNewRishProjectType" runat="server" Text="Тип Документу:" />
                        <dx:ASPxComboBox ID="NewRishProjectType" 
                            ClientInstanceName="NewRishProjectType" runat="server" 
                            DataSourceID="SqlDataSourceProjectTypes" TextField="name" ValueField="id">
                        </dx:ASPxComboBox>
                        <asp:SqlDataSource ID="SqlDataSourceProjectTypes" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                            SelectCommand="SELECT id, name FROM dict_rish_project_type">
                        </asp:SqlDataSource>
                        <br />
                        <dx:ASPxLabel ID="LabelNewRishState" runat="server" Text="Початковий Стан Документу:" />
                        <uc1:CheckComboBox ID="NewRishState" Width="360px"
                            ClientInstanceName="NewRishState" runat="server" ReadOnly="True"
                            TextField="name" ValueField="id" DataSourceID="SqlDataSourceProjectStates" />
                        <asp:SqlDataSource ID="SqlDataSourceProjectStates" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                            SelectCommand="SELECT [id], [name] FROM [dict_rish_project_state] ORDER BY [order]">
                        </asp:SqlDataSource>
                        <br />
                        <br />
                        <table border="0" cellspacing="4" cellpadding="0" width="100%">
                            <tr>
                                <td align="right">
                                    <dx:ASPxButton ID="ButtonCreateRishProject" runat="server" Text="Створити" AutoPostBack="false" Width="148px">
                                        <ClientSideEvents Click="OnAddNewRishProject" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                        <dx:ASPxPopupControl ID="PopupNewProjectContact" runat="server" 
                            ClientInstanceName="PopupNewProjectContact"
                            HeaderText="Нова Контактна Особа" Height="115px" 
                            PopupElementID="LinkNewRishContact" Width="313px">
                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server">
                                    <dx:ASPxLabel ID="LabelChooseOrganization" runat="server" Text="Організація:">
                                    </dx:ASPxLabel>
                                    <div style="float: right; margin-top: 16px;">
                                        <dx:ASPxHyperLink ID="LinkNewProjectOrganization" runat="server" Text="(ввести нову)">
                                        </dx:ASPxHyperLink>
                                    </div>
                                    <dx:ASPxComboBox ID="NewContactOrganization" runat="server" 
                                        DataSourceID="SqlDataSourceProjectOrganizations" TextField="name" 
                                        ValueField="id" Width="210px" ClientInstanceName="NewContactOrganization" 
                                        EnableClientSideAPI="True" OnCallback="NewContactOrganization_Callback">
                                        <ClientSideEvents EndCallback="function(s, e) {
	s.SetSelectedIndex(s.cpSelectedIndex);
}" />
                                    </dx:ASPxComboBox>
                                    <asp:SqlDataSource ID="SqlDataSourceProjectOrganizations" runat="server" 
                                        ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
                                        SelectCommand="SELECT id, name FROM dict_rish_project_org ORDER BY name">
                                    </asp:SqlDataSource>
                                    <br />
                                    <dx:ASPxLabel ID="LabelNewContactName" runat="server" Text="Відповідальна Особа:">
                                    </dx:ASPxLabel>
                                    <dx:ASPxTextBox ID="NewContactName" ClientInstanceName="NewContactName" 
                                        runat="server" Width="300px">
                                    </dx:ASPxTextBox>
                                    <br />
                                    <dx:ASPxLabel ID="LabelNewContactTitle" runat="server" Text="Посада:">
                                    </dx:ASPxLabel>
                                    <dx:ASPxTextBox ID="NewContactTitle" ClientInstanceName="NewContactTitle" 
                                        runat="server" Width="300px">
                                    </dx:ASPxTextBox>
                                    <br />
                                    <dx:ASPxLabel ID="LabelNewContactPhone" runat="server" Text="Телефон:">
                                    </dx:ASPxLabel>
                                    <dx:ASPxTextBox ID="NewContactPhone" ClientInstanceName="NewContactPhone" 
                                        runat="server" Width="300px">
                                    </dx:ASPxTextBox>
                                    <br />
                                    <br />
                                    <table border="0" cellspacing="4" cellpadding="0" width="100%">
                                        <tr>
                                            <td align="right">
                                                <dx:ASPxButton ID="ButtonAddNewProjectContact" runat="server" Text="Створити" AutoPostBack="false" Width="148px">
                                                    <ClientSideEvents Click="OnAddNewProjectContact" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <dx:ASPxPopupControl ID="PopupNewProjectOrg" runat="server" 
                                        ClientInstanceName="PopupNewProjectOrg" HeaderText="Нова Організація" 
                                        PopupElementID="LinkNewProjectOrganization">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl runat="server">
                                                <dx:ASPxLabel ID="LabelNewContactOrgName" runat="server" Text="Назва Організації:">
                                                </dx:ASPxLabel>
                                                <dx:ASPxTextBox ID="NewContactOrgName" ClientInstanceName="NewContactOrgName" 
                                                    runat="server" Width="300px">
                                                </dx:ASPxTextBox>
                                                <br />
                                                <br />
                                                <table border="0" cellspacing="4" cellpadding="0" width="100%">
                                                    <tr>
                                                        <td align="right">
                                                            <dx:ASPxButton ID="ButtonAddNewProjectOrg" runat="server" Text="Створити" AutoPostBack="false" Width="148px">
                                                                <ClientSideEvents Click="OnAddNewProjectOrg" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <table width="100%" border="0" cellspacing="0" cellpadding="4">
                <tr>
                    <td>
                        <dx:ASPxButton ID="ButtonShowNewRishProjectPopup" runat="server" AutoPostBack="False" Text="Створити Новий" Width="148px"/>
                    </td>
                    <td>
                        <dx:ASPxPopupControl ID="PopupSaveAs" runat="server" 
                            HeaderText="Збереження у Файлі" 
                            ClientInstanceName="PopupSaveAs" 
                            PopupElementID="ButtonSaveAs">
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" >
                                    <dx:ASPxButton ID="ButtonSaveAs_ExportXLS" runat="server" 
                                        Text="XLS - Microsoft Excel&reg;" 
                                        OnClick="ButtonSaveAs_ExportXLS_Click" Width="180px">
                                    </dx:ASPxButton>
                                    <br />
                                    <dx:ASPxButton ID="ButtonSaveAs_ExportPDF" runat="server" 
                                        Text="PDF - Adobe Acrobat&reg;" 
                                        OnClick="ButtonSaveAs_ExportPDF_Click" Width="180px">
                                    </dx:ASPxButton>
                                    <br />
                                    <dx:ASPxButton ID="ButtonSaveAs_ExportCSV" runat="server" 
                                        Text="CSV - значення, розділені комами" 
                                        OnClick="ButtonSaveAs_ExportCSV_Click" Width="180px">
                                    </dx:ASPxButton>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxButton ID="ButtonSaveAs" runat="server" AutoPostBack="False" Text="Зберегти у Файлі" Width="148px"/>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewRishProjectsExporter" runat="server"
    FileName="РеєстрПроектівДокументів" GridViewID="GridViewRishProjects" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView ID="GridViewRishProjects" ClientInstanceName="GridViewRishProjects" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceRishProjects" KeyFieldName="id" Width="100%"
    OnCustomCallback="GridViewRishProjects_CustomCallback"
    OnCustomButtonCallback="GridViewRishProjects_CustomButtonCallback"
    OnCustomJSProperties="GridViewRishProjects_CustomJSProperties" >

    <Columns>
        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="Edit" Text="Відкрити Картку"> <Image ToolTip="Відкрити Картку" Url="../Styles/EditIcon.png" /> </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="Delete" Text="Видалити"> <Image ToolTip="Видалити" Url="../Styles/DeleteIcon.png" /> </dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton Text="Створити Копію" ID="Clone"> <Image ToolTip="Створити Копію" Url="~/Styles/CopyIcon.png"> </Image> </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
        </dx:GridViewCommandColumn>
        <dx:GridViewDataTextColumn FieldName="project_type" VisibleIndex="2" 
            Caption="Тип Документу" Width="120px"><Settings HeaderFilterMode="CheckedList"/></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="create_date" Caption="Дата Документу" VisibleIndex="5"></dx:GridViewDataDateColumn>

        <dx:GridViewDataTextColumn Caption="Назва" FieldName="name" VisibleIndex="4" Width="200px">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# CutRishennyaName(Eval("name")) %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="15" 
            Caption="Останні Зміни"><Settings AllowHeaderFilter="False"/></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="project_state" VisibleIndex="3" Caption="Стан" Width="120px">
            <Settings HeaderFilterMode="CheckedList"></Settings>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="subject" VisibleIndex="10" 
            Caption="Тема Документу"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_org_name" VisibleIndex="6" 
            Caption="Ким Створено"><Settings HeaderFilterMode="CheckedList"/></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Відповідальна Особа" 
            FieldName="contact_name" VisibleIndex="7">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_title" VisibleIndex="8" 
            Caption="Посада Відповідальної Особи"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Телефон Відповідальної Особи" 
            FieldName="contact_phone" VisibleIndex="9">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Номер" FieldName="id" VisibleIndex="1">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Дата Затвердження Документу" 
            FieldName="document_date" VisibleIndex="11">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Номер Затвердження Документу" 
            FieldName="document_num" VisibleIndex="13">
        </dx:GridViewDataTextColumn>
    </Columns>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="False" />
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowFilterBar="Visible"
        ShowHeaderFilterButton="True"
        HorizontalScrollBarMode="Visible"
        ShowFooter="False"
        ShowGroupPanel="True"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsPager AlwaysShowPager="true" PageSize="10" />
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
        <Row CssClass="TreeListDocRow">
        </Row>
        <AlternatingRow CssClass="TreeListDocRow">
        </AlternatingRow>
        <CommandColumn CssClass="TreeListDocCmd">
        </CommandColumn>
    </Styles>
    <SettingsCookies CookiesID="GUKV.RishProjectList" Enabled="True" Version="A2" />

    <ClientSideEvents
        CustomButtonClick="OnGridViewRishProjectsCustomButtonClick"
        Init="function (s,e) { GridViewRishProjects.PerformCallback('init:'); }"
        EndCallback="GridViewRishProjectsEndCallback" />
</dx:ASPxGridView>
