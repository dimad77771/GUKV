<%@ Page Title="" Language="C#" MasterPageFile="~/NoMenu.master" AutoEventWireup="true" CodeFile="NewAct.aspx.cs" Inherits="BPRishProject_NewAct" %>

<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxFormLayout" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .row:nth-child(n+2)
        {
        	margin-top: 20px;
        }
        
        .form-cell 
        {
        	display: inline-block;
        	vertical-align: middle;
        }
        
        .form-cell.whole
        {
        	width: 810px;
        }
        .form-cell.whole .field-label
        {
        	display: inline-block;
        	width: 120px;
        	vertical-align: middle;
        }
        .form-cell.whole .form-field
        {
        	display: inline-block;
        	vertical-align: middle;
        	width: 684px;
        }
        
        .form-cell.half
        {
        	width: 370px;
        }
        .form-cell.half:nth-child(n+2)
        {
        	margin-left: 70px;
        }
        .form-cell.half .field-label
        {
        	display: inline-block;
        	width: 120px;
        	vertical-align: middle;
        }
        .form-cell.half .form-field
        {
        	display: inline-block;
        	vertical-align: middle;
        	width: 240px;
        }
        
        .form-cell.third
        {
        	width: 250px;
        }
        .form-cell.third:nth-child(n+2)
        {
        	margin-left: 28px;
        }
        .form-cell.third .field-label
        {
        	display: block;
        }
        .form-cell.third .form-field
        {
        	display: block;
        	width: 250px;
        }
        
        .field-label.top
        {
        	vertical-align: top !important;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <script type="text/javascript" src="../Scripts/CommonScript.js"></script>
    <script type="text/javascript" language="javascript">

    // <![CDATA[

    var ProjectID = <%= ProjectID %>;

    function ButtonCreateAct_Click(s, e) {
        var noObjectSelection = GridViewObjects.GetSelectedRowCount() == 0;
        PanelValidationErrors.SetVisible(noObjectSelection);
        if (!TextBoxActNumber.GetIsValid() 
            || !DateEditActDate.GetIsValid()
            || noObjectSelection) {
            e.cancel = true;
            e.processOnServer = false;
        }
    }

    // ]]>

    </script>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" SelectCommand="select bp_rish_project_item.id, bp_rish_project_item.intro_text,
	 org_from.id org_from_id, org_from.full_name org_from, org_to.id org_to_id, org_to.full_name org_to, dict_obj_rights.id right_id, dict_obj_rights.name right_name, 
     bp_rish_project_item.table_type, case when bp_rish_project_item.table_type = 0 then 'Об’єкти нерухомості нежитлового фонду' else 'Інші об’єкти комунальної власності' end table_type_name
from bp_rish_project_item
inner join bp_rish_project_item parent_item on parent_item.id = bp_rish_project_item.parent_item_id
left join organizations org_from on org_from.id = parent_item.org_from_id
left join organizations org_to on org_to.id = parent_item.org_to_id
left join dict_obj_rights on dict_obj_rights.id = parent_item.right_id
where bp_rish_project_item.project_id = @projectID
	and bp_rish_project_item.is_table = 1
	and exists (select * from bp_rish_project_table 
		where bp_rish_project_table.bp_rish_project_item_id = bp_rish_project_item.id
			and isnull(is_acted_on, 0) = 0)
" onselecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="projectID" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
        
        SelectCommand="SELECT * FROM bp_rish_project_table WHERE bp_rish_project_item_id = @itemID and ISNULL(is_acted_on, 0) = 0" 
        onselecting="SqlDataSource2_Selecting">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="itemID" />
        </SelectParameters>
    </asp:SqlDataSource>

    <dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server"
        ActiveTabIndex="0">
        <TabPages>
            <dx:TabPage Text="Акт Прийому-Передачі" Name="CardProperties">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl1" runat="server">

                        <div class="row">
                            <div class="form-cell third">
                                <div class="field-label">
                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Номер розпорядчого документу">
                                    </dx:ASPxLabel>
                                </div>
                                <div class="form-field">
                                    <dx:ASPxLabel ID="LabelRishNumber" runat="server" Font-Bold="true" Text="немає даних">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                            <div class="form-cell third">
                                <div class="field-label">
                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Дата затвердження розпорядчого документу">
                                    </dx:ASPxLabel>
                                </div>
                                <div class="form-field">
                                    <dx:ASPxLabel ID="LabelRishDate" runat="server" Font-Bold="true" Text="немає даних">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                            <div class="form-cell third">
                                <div class="field-label">
                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Тип документу">
                                    </dx:ASPxLabel>
                                </div>
                                <div class="form-field">
                                    <dx:ASPxLabel ID="LabelRishType" runat="server" Font-Bold="true" Text="немає даних">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="form-cell whole">
                                <div class="field-label top">
                                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Назва документу">
                                    </dx:ASPxLabel>
                                </div>
                                <div class="form-field">
                                    <dx:ASPxMemo ID="MemoRishName" runat="server" Width="100%" Height="80px" 
                                        ReadOnly="true" Text="немає даних" >
                                        <Border BorderStyle="None" />
                                    </dx:ASPxMemo>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <hr />
                        </div>

                        <div class="row">
                            <div class="form-cell half">
                                <div class="field-label">
                                    <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Номер Акту">
                                    </dx:ASPxLabel>
                                </div>
                                <div class="form-field">
                                    <dx:ASPxTextBox ID="TextBoxActNumber" ClientInstanceName="TextBoxActNumber" runat="server"
                                        Width="100%" >
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </div>
                            </div>
                            <div class="form-cell half">
                                <div class="field-label">
                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Дата Акту">
                                    </dx:ASPxLabel>
                                </div>
                                <div class="form-field">
                                    <dx:ASPxDateEdit ID="DateEditActDate" ClientInstanceName="DateEditActDate" runat="server"
                                        Width="100%" >
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row"></div>

                        <dx:ASPxCallbackPanel ID="CPAct" runat="server" Width="814px" 
                            ClientInstanceName="CPAct" OnCallback="CPAct_Callback">
                            <PanelCollection>
                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">

                                    <div class="row">
                                        <div class="form-cell half">
                                            <div class="field-label">
                                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Додаток">
                                                </dx:ASPxLabel>
                                            </div>
                                            <div class="form-field">
                                                <dx:ASPxComboBox ID="ComboBoxAppendix" ClientInstanceName="ComboBoxAppendix" runat="server"
                                                    Width="100%" DataSourceID="SqlDataSource1" TextField="intro_text" 
                                                    TextFormatString="{0}" ValueField="id" ValueType="System.Int32" >
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	CPAct.PerformCallback();
}" />
                                                    <Columns>
                                                        <dx:ListBoxColumn FieldName="id" Visible="False" />
                                                        <dx:ListBoxColumn FieldName="intro_text" Caption="Назва додатку" 
                                                            Width="200px" />
                                                        <dx:ListBoxColumn FieldName="org_from_id" Visible="false" />
                                                        <dx:ListBoxColumn FieldName="org_from" Caption="Від організації" 
                                                            Width="200px" />
                                                        <dx:ListBoxColumn FieldName="org_to_id" Visible="false" />
                                                        <dx:ListBoxColumn FieldName="org_to" Caption="До організації" Width="200px" />
                                                        <dx:ListBoxColumn FieldName="right_id" Visible="false" />
                                                        <dx:ListBoxColumn Caption="Право" FieldName="right_name" Width="200px" />
                                                        <dx:ListBoxColumn FieldName="table_type" Visible="false" />
                                                        <dx:ListBoxColumn Caption="Тип об'єктів" FieldName="table_type_name" Width="200px" />
                                                    </Columns>
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="form-cell half">
                                            <div class="field-label">
                                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Право">
                                                </dx:ASPxLabel>
                                            </div>
                                            <div class="form-field">
                                                <dx:ASPxLabel ID="LabelActRight" runat="server" Text="немає даних" Font-Bold="true">
                                                </dx:ASPxLabel>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="form-cell whole">
                                            <dx:ASPxGridView ID="GridViewObjects" runat="server" ClientInstanceName="GridViewObjects"
                                                Width="100%" DataSourceID="SqlDataSource2" KeyFieldName="id" 
                                                OnHtmlDataCellPrepared="GridViewObjects_HtmlDataCellPrepared">
                                                <Columns>
                                                    <dx:GridViewCommandColumn 
                                                        ShowSelectCheckbox="True" VisibleIndex="0"  />
                                                    <dx:GridViewDataTextColumn Visible="false" FieldName="id">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="1" Visible="true" FieldName="name" 
                                                        Caption="Найменування об'єкта" Width="200px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="1" Visible="true" FieldName="address" 
                                                        Caption="Адреса" Width="200px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="2" Visible="true" FieldName="addr_street_name" 
                                                        Caption="Назва вулиці" Width="200px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="3" Visible="true" FieldName="addr_nomer" 
                                                        Caption="Номер будинку" Width="80px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="4" Visible="true" FieldName="addr_misc" 
                                                        Caption="Додаткова адреса" Width="80px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="5" Visible="true" FieldName="addr_distr" 
                                                        Caption="Район" Width="80px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="6" Visible="true" FieldName="obj_kind" 
                                                        Caption="Вид будинку" Width="80px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="7" Visible="true" FieldName="obj_type" 
                                                        Caption="Тип будинку" Width="80px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="8" Visible="true" FieldName="year_built"
                                                        Caption="Рік завершення будівництва">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="9" Visible="true" FieldName="sqr_total"
                                                        Caption="Площа, кв.м">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="10" Visible="true" FieldName="location" 
                                                        Caption="Місце розташування об’єкта" Width="200px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="11" Visible="true" FieldName="inv_number"
                                                        Caption="Інвентарний номер" Width="130px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn VisibleIndex="12" Visible="true" FieldName="commissioned_date"
                                                        Caption="Дата введення в експлуатацію">
                                                        <PropertiesTextEdit DisplayFormatString="d">
                                                        </PropertiesTextEdit>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewBandColumn VisibleIndex="13" Caption="Вартість (грн.)">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="initial_cost" Caption="Первісна">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn VisibleIndex="1" FieldName="remaining_cost" Caption="Залишкова">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                    </dx:GridViewBandColumn>
                                                </Columns>
                                                <SettingsBehavior EnableCustomizationWindow="False" AutoFilterRowInputDelay="2500"
                                                    ColumnResizeMode="Control" AllowDragDrop="false" ConfirmDelete="True" EnableRowHotTrack="True" />
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
                                        </div>
                                    </div>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>

    <br />
    
    <dx:ASPxPanel ID="PanelValidationErrors" runat="server" Width="810px" ClientInstanceName="PanelValidationErrors"
        ClientVisible="false">
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxLabel ID="ASPxLabel17" runat="server" 
                    Text="Будь ласка оберіть об'єкти що належать до цього акту" Font-Bold="True" 
                    ForeColor="Red">
                </dx:ASPxLabel>
                <br />
                <br />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>

    <div style="margin-left: 60px;">
        <dx:ASPxRoundPanel ID="PanelServerErrors" runat="server" Width="710px" ClientVisible="False"
            HeaderText="Помилки">
            <PanelCollection>
                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                    <dx:ASPxLabel ID="LabelServerErrors" runat="server" Font-Bold="True" ForeColor="Red">
                    </dx:ASPxLabel>
                    <br />
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br />
    </div>

    <dx:ASPxCallbackPanel ID="CPFormButtons" ClientInstanceName="CPFormButtons" runat="server">
        <PanelCollection>
            <dx:PanelContent ID="Panelcontent2" runat="server">
                <table border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ButtonCreateAct" ClientInstanceName="ButtonCreateAct"
                                runat="server" Text="Створити акт" Width="148px" 
                                OnClick="ButtonCreateAct_Click">
                                <ClientSideEvents Click="ButtonCreateAct_Click" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
