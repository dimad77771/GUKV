<%@ page language="C#" autoeventwireup="true" inherits="Reports1NF_OrgBalansDeletedObject, App_Web_orgbalansdeletedobject.aspx.5d94abc0" masterpagefile="~/NoMenu.master" title="Картка Відчуженого Об'єкту" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>
<%@ Register src="ReportCommentViewer.ascx" tagname="ReportCommentViewer" tagprefix="uc1" %>

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

        var lastFocusedControlId = "";
        var lastFocusedControlTitle = "";

    // ]]>

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceBuilding" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 b.* FROM reports1nf_balans_deleted bal INNER JOIN buildings b ON b.id = bal.building_id
        WHERE bal.id = @bal_id AND bal.report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansObject" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 * FROM reports1nf_balans_deleted WHERE id = @bal_id AND report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansObjectStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT CASE WHEN submit_date IS NULL OR modify_date IS NULL OR modify_date > submit_date THEN 0 ELSE 1 END AS 'record_status' FROM reports1nf_balans_deleted WHERE id = @bal_id AND report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_districts2 ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceVidchType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_balans_vidch_type ORDER BY id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocKind" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_doc_kind ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations WHERE zkpo_code LIKE @zkpo AND full_name LIKE @fname AND
        (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL"
    OnSelecting="SqlDataSourceOrgSearch_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Cabinet.aspx" Text="Стан"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgInfo.aspx" Text="Загальна Інформація"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansList.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgBalansDeletedList.aspx" Text="Відчужені Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgArendaList.aspx" Text="Договори використання приміщень "></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/OrgRentedList.aspx" Text="Договори Орендування"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/Logout.aspx" Text="Вийти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Account/ChangePasswordNoMenu.aspx" Text="Пароль"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Картка відчуженого об'єкту" CssClass="pagetitle"/>
</p>

<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" runat="server" OnCallback="CPMainPanel_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent2" runat="server">

<asp:FormView runat="server" BorderStyle="None" ID="StatusForm" DataSourceID="SqlDataSourceBalansObjectStatus" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Необхідно надіслати внесені зміни до ДКВ" ClientVisible='<%# 0.Equals(Eval("record_status")) %>' ForeColor="Red" />
        <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Зміни надіслано до ДКВ" ClientVisible='<%# 1.Equals(Eval("record_status")) %>' />
    </ItemTemplate>
</asp:FormView>

<asp:FormView runat="server" BorderStyle="None" ID="AddressForm" DataSourceID="SqlDataSourceBuilding" EnableViewState="False">
    <ItemTemplate>

        <dx:ASPxRoundPanel ID="PanelAddress" runat="server" HeaderText="Адреса будинку">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server">

                    <table border="0" cellspacing="0" cellpadding="2" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Район"></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxComboBox ID="ComboAddrDistrictDeleted" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDistrict" Value='<%# Eval("addr_distr_new_id") %>' ReadOnly="true"
                                    Title="Адреса будинку - Район" />
                            </td>
                            <td><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Назва вулиці"></dx:ASPxLabel></td>
                            <td> &nbsp; &nbsp; &nbsp; &nbsp; </td>
                            <td align="right"><dx:ASPxTextBox ID="EditStreetNameDeleted" runat="server" Text='<%# Eval("addr_street_name") %>' Width="270px" ReadOnly="true" Title="Адреса будинку - Назва вулиці" /></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Номер будинку"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="EditBuildingNumDeleted" runat="server" Text='<%# Eval("addr_nomer") %>' Width="270px" ReadOnly="true" Title="Адреса - Номер будинку" /></td>
                            <td><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Поштовий індекс"></dx:ASPxLabel></td>
                            <td></td>
                            <td align="right"><dx:ASPxTextBox ID="EditZipCodeDeleted" runat="server" Text='<%# Eval("addr_zip_code") %>' Width="270px" ReadOnly="true" Title="Адреса будинку - Поштовий індекс" /></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Додаткова адреса"></dx:ASPxLabel></td>
                            <td colspan="4"><dx:ASPxTextBox ID="EditMiscAddrDeleted" runat="server" Text='<%# Eval("addr_misc") %>' Width="100%" ReadOnly="true" Title="Адреса будинку - Додаткова адреса" /></td>
                        </tr>
                    </table>

                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <p class="SpacingPara"/>

    </ItemTemplate>
</asp:FormView>

<asp:FormView runat="server" BorderStyle="None" ID="DelObjectForm" DataSourceID="SqlDataSourceBalansObject" EnableViewState="False">
    <ItemTemplate>

        <dx:ASPxRoundPanel ID="PanelBuildingProps" runat="server" HeaderText="Характеристики відчуженого об'єкту">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">

                    <table border="0" cellspacing="0" cellpadding="2" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Спосіб відчуження"></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxComboBox ID="ComboVidchType" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceVidchType" Value='<%# Eval("vidch_type_id") %>'
                                    Title="Спосіб відчуження об'єкту" OnValidation="OnValidation">
                                    <ValidationSettings Display="None" ValidationGroup="MainGroup" ErrorText="Необхідно вибрати спосіб відчуження об'єкту"> <RequiredField IsRequired="false" ErrorText="Необхідно вибрати спосіб відчуження об'єкту" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                            <td><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Розташування приміщення (поверх)"></dx:ASPxLabel></td>
                            <td align="right"><dx:ASPxTextBox ID="EditVidchFloor" runat="server" Text='<%# Eval("floors") %>' Width="270px" Title="Розташування приміщення (поверх)" MaxLength="100" /></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Площа відчуження, кв.м."></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxSpinEdit ID="EditVidchSquare" runat="server" NumberType="Float" Value='<%# Eval("sqr_total") %>' Width="270px" Title="Площа відчуження" OnValidation="OnValidation">
                                    <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true" ErrorText="Необхідно вказати площу відчуження"></ValidationSettings>
                                </dx:ASPxSpinEdit>
                            </td>

                            <td><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Вартість відчуженого приміщення, грн."></dx:ASPxLabel></td>
                            <td align="right"><dx:ASPxSpinEdit ID="EditVidchCost" runat="server" NumberType="Float" Value='<%# Eval("vidch_cost") %>' Width="270px" Title="Вартість відчуженого приміщення" /></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Призначення об‘єкту"></dx:ASPxLabel></td>
                            <td><dx:ASPxTextBox ID="EditVidchPurpose" runat="server" Text='<%# Eval("purpose_str") %>' Width="270px" Title="Призначення об‘єкту" MaxLength="255" /></td>
                            <td><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Інвентарний номер об'єкту"></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxTextBox ID="EditObjBtiReestrNoDeleted" runat="server" Text='<%# Eval("reestr_no") %>' Width="270px" Title="Інвентарний номер об'єкту" MaxLength="18" OnValidation="OnValidation">
                                    <ValidationSettings Display="None" ValidationGroup="MainGroup" ErrorText="Необхідно вказати інвентарний номер об'єкту"></ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                        </tr>
                    </table>

                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="PanelOrganizations" runat="server" HeaderText="Кому передано на баланс">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server">

                    <table border="0" cellspacing="0" cellpadding="2">
                        <tr>
                            <td> <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Код ЄДРПОУ:" Width="90px" /> </td>
                            <td> <dx:ASPxTextBox ID="EditVidchOrgZKPO" ClientInstanceName="EditVidchOrgZKPO" runat="server" Width="180px" Title="" /> </td>
                            <td> <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Назва:" Width="50px" /> </td>
                            <td> <dx:ASPxTextBox ID="EditVidchOrgName" ClientInstanceName="EditVidchOrgName" runat="server" Width="380px" Title="" /> </td>
                            <td>
                                <dx:ASPxButton ID="BtnFindVidchOrg" ClientInstanceName="BtnFindVidchOrg" runat="server" AutoPostBack="False" Text="Знайти" CausesValidation="false" Width="90px">
                                    <ClientSideEvents Click="function (s, e) { ComboVidchOrg.PerformCallback(EditVidchOrgZKPO.GetText() + '|' + EditVidchOrgName.GetText()); }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <dx:ASPxComboBox ID="ComboVidchOrg" ClientInstanceName="ComboVidchOrg" runat="server"
                                    Width="100%" DataSourceID="SqlDataSourceOrgSearch" ValueField="id" ValueType="System.Int32"
                                    TextField="search_name" EnableSynchronization="True" OnCallback="ComboVidchOrg_Callback"
                                    Value='<%# Eval("vidch_org_id") %>' Title="Кому передано на баланс" >
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                    </table>

                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="PanelVidchDoc" runat="server" HeaderText="Документ, що підтверджує відчуження об'єкту">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server">

                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Тип документу:"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboVidchDocKind" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="350px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDocKind" Value='<%# Eval("vidch_doc_type") %>'
                                    Title="Тип документу, що підтверджує відчуження об'єкту" OnValidation="OnValidation">
                                    <ValidationSettings Display="None" ValidationGroup="MainGroup" ErrorText="Документ, що підтверджує відчуження об'єкту: необхідно вибрати тип документу"> <RequiredField IsRequired="false" ErrorText="Документ, що підтверджує відчуження об'єкту: необхідно вибрати тип документу" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                                                
                            <td><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Номер:"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxTextBox ID="EditVidchDocNum" runat="server" Text='<%# Eval("vidch_doc_num") %>' Width="100px" Title="Номер документу, що підтверджує відчуження об'єкту" MaxLength="24" OnValidation="OnValidation">
                                    <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true" ErrorText="Документ, що підтверджує відчуження об'єкту: необхідно вказати номер документу"></ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>

                            <td><dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="Від:"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxDateEdit ID="EditVidchDocDate" runat="server" Value='<%# Eval("vidch_doc_date") %>' Width="100px" Title="Дата документу, що підтверджує відчуження об'єкту" OnValidation="OnValidation">
                                    <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true" ErrorText="Документ, що підтверджує відчуження об'єкту: необхідно вказати дату документу"></ValidationSettings>
                                </dx:ASPxDateEdit>
                            </td>
                        </tr>
                    </table>

                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        

    </ItemTemplate>
</asp:FormView>

        <p class="SpacingPara"/>

        <asp:FormView runat="server" BorderStyle="None" ID="FormViewState" DataSourceID="SqlDataSourceBalansObject" EnableViewState="False">
            <ItemTemplate>
                <dx:ASPxRoundPanel ID="StatePanel" runat="server" HeaderText="Стан">
                    <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="0px" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent7" runat="server">
                            <table border="0" cellspacing="0" cellpadding="0" width="810px">
                                <tr>
                                    <td><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text='<%# EvaluateSignature(Eval("modified_by"), Eval("modify_date")) %>'></dx:ASPxLabel></td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </ItemTemplate>
        </asp:FormView>

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<p class="SpacingPara"/>

<dx:ASPxValidationSummary ID="ValidationSummary" ValidationGroup="MainGroup" runat="server" RenderMode="BulletedList" Width="800px" ClientInstanceName="ValidationSummary">
</dx:ASPxValidationSummary>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('save:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonSend" runat="server" Text="Надіслати" AutoPostBack="false" ValidationGroup="MainGroup" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { if (ASPxClientEdit.ValidateEditorsInContainer(null)) { CPMainPanel.PerformCallback('send:'); } }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonClear" runat="server" Text="Відмінити зміни" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('clear:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonComments" runat="server" Text="Коментарі" CausesValidation="false" AutoPostBack="false" />

            <dx:ASPxPopupControl ID="PopupComments" runat="server" 
                HeaderText="Коментарі" 
                ClientInstanceName="PopupComments" 
                PopupElementID="ButtonComments" >
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                        
                        <dx:ASPxCallbackPanel ID="CPCommentViewerPanel" ClientInstanceName="CPCommentViewerPanel" runat="server" OnCallback="CPCommentViewerPanel_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent6" runat="server">

                                    <uc1:ReportCommentViewer ID="ReportCommentViewer1" runat="server"/>

                                </dx:panelcontent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents
                    PopUp="function (s,e) { CPCommentViewerPanel.PerformCallback('' + lastFocusedControlId + ';' + lastFocusedControlTitle); }"
                    CloseUp="function (s,e) { CPMainPanel.PerformCallback('clear:'); }" />
            </dx:ASPxPopupControl>
        </td>
    </tr>
</table>

</asp:Content>
