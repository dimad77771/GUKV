<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgRentedObject.aspx.cs" Inherits="Reports1NF_OrgRentedObject"
    MasterPageFile="~/NoHeader.master" Title="Картка Договору Орендування" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
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

        function EnableCmkControls(s, e) {

            var isEnable = CheckIsCmk.GetChecked();

            EditCmkSqrRented.SetEnabled(isEnable);
            EditCmkPaymentNarah.SetEnabled(isEnable);
            EditCmkPaymentToBudget.SetEnabled(isEnable);
            EditCmkRentDebt.SetEnabled(isEnable);
        }

    // ]]>

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceBuilding" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 b.* FROM reports1nf_arenda_rented ar INNER JOIN reports1nf_buildings b ON b.unique_id = ar.building_1nf_unique_id
        WHERE ar.id = @aid AND ar.report_id = @rep_id"
    OnSelecting="SqlDataSource_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentedObj" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 * FROM reports1nf_arenda_rented WHERE id = @aid AND report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAgreementStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT CASE WHEN submit_date IS NULL OR modify_date IS NULL OR modify_date > submit_date THEN 0 ELSE 1 END AS 'record_status' FROM reports1nf_arenda_rented WHERE id = @aid AND report_id = @rep_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="aid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourcePaymentType" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_arenda_payment_type ORDER BY name">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDistrict" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_1nf_districts2 ORDER BY name">
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
    <asp:Label runat="server" ID="ASPxLabel19" Text="Картка договору орендування об'єкту у іншої організації" CssClass="pagetitle"/>
</p>

<dx:ASPxCallbackPanel ID="CPMainPanel" ClientInstanceName="CPMainPanel" runat="server" OnCallback="CPMainPanel_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent2" runat="server">

<asp:FormView runat="server" BorderStyle="None" ID="StatusForm" DataSourceID="SqlDataSourceAgreementStatus" EnableViewState="False">
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

                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Район"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxComboBox ID="ComboRentedAddrDistrict" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="270px" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceDistrict" ReadOnly="true" Value='<%# Eval("addr_distr_new_id") %>'
                                        Title="Адреса будинку - район" />
                            </td>
                            <td width="8px">&nbsp;</td>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Назва вулиці"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td>
                                <dx:ASPxTextBox ID="EditRentedAddrStreet" runat="server" ReadOnly="true" Text='<%# Eval("addr_street_name") %>' Width="270px"  Title="Адреса будинку - Назва вулиці" />
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Номер будинку"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditRentedBuildingNum" runat="server" ReadOnly="true" Text='<%# Eval("addr_nomer") %>' Width="270px" Title="Адреса - номер будинку" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Поштовий індекс"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditRentedZipCode" runat="server" ReadOnly="true" Text='<%# Eval("addr_zip_code") %>' Width="270px" Title="Адреса будинку - Поштовий індекс" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Додаткова адреса"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="EditRentedMiscAddr" ReadOnly="true" runat="server" Text='<%# Eval("addr_misc") %>' Width="680px" Title="Адреса будинку - Додаткова адреса" /></td>
                        </tr>
                    </table>

                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

    </ItemTemplate>
</asp:FormView>

<p class="SpacingPara"/>

<asp:FormView runat="server" BorderStyle="None" ID="RentedObjectForm" DataSourceID="SqlDataSourceRentedObj" EnableViewState="False">
    <ItemTemplate>

        <dx:ASPxRoundPanel ID="PanelRentedObject" runat="server" HeaderText="Відомості щодо договору орендування">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server">

                    <table border="0" cellspacing="0" cellpadding="2" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Номер договору орендування"></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxTextBox ID="EditAgrNum" runat="server" Text='<%# Eval("agreement_num") %>' Width="190px" MaxLength="18" Title="Номер договору орендування">
                                    <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true"></ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td> &nbsp; </td>
                            <td><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Дата укладання договору"></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxDateEdit ID="EditAgrDate" runat="server" Value='<%# Eval("agreement_date") %>' Width="100%" Title="Дата укладання договору">
                                    <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true"></ValidationSettings>
                                </dx:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Початок оренди згідно з договором"></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxDateEdit ID="EditAgrStartDate" runat="server" Value='<%# Eval("rent_start_date") %>' Width="190px" Title="Початок оренди згідно з договором">
                                    <ValidationSettings Display="None" ValidationGroup="MainGroup" EnableCustomValidation="true"></ValidationSettings>
                                </dx:ASPxDateEdit>
                            </td>
                            <td> &nbsp; </td>
                            <td><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Закінчення оренди згідно з договором"></dx:ASPxLabel></td>
                            <td><dx:ASPxDateEdit ID="EditAgrFinishDate" runat="server" Value='<%# Eval("rent_finish_date") %>' Width="100%"  Title="Закінчення оренди згідно з договором" /></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Вид оплати"></dx:ASPxLabel></td>
                            <td colspan="4">
                                <dx:ASPxComboBox ID="ComboPaymentTypeRented" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="100%" 
                                    IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourcePaymentType" Value='<%# Eval("payment_type_id") %>'
                                     Title="Вид оплати">
                                    <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Необхідно вибрати вид оплати за договором" /> </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Орендована площа, кв.м."></dx:ASPxLabel></td>
                            <td><dx:ASPxSpinEdit ID="EditRentedSquare" runat="server" NumberType="Float" Value='<%# Eval("rent_square") %>' Width="190px"  Title="Орендована площа" /></td>
                            <td> &nbsp; </td>
                            <td colspan="2">
                                <dx:ASPxCheckBox ID="CheckRentedSubarenda" runat="server" Text="Суборенда" Checked='<%# 1.Equals(Eval("is_subarenda")) %>'  Title="Суборенда" />
                            </td>
                        </tr>
                    </table>

                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <p class="SpacingPara"/>

        <dx:ASPxRoundPanel ID="PanelCmk" runat="server" HeaderText="Відомості щодо користування цілісним майновим комплексом">
            <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">

                    <table border="0" cellspacing="0" cellpadding="2" width="810px">
                        <tr>
                            <td colspan="2">
                                <dx:ASPxCheckBox ID="CheckIsCmk" ClientInstanceName="CheckIsCmk" runat="server" Text="Користування цілісним майновим комплексом" Checked='<%# 1.Equals(Eval("is_cmk")) %>'
                                     Title="Користування цілісним майновим комплексом">
                                    <ClientSideEvents CheckedChanged="EnableCmkControls" />
                                </dx:ASPxCheckBox>
                            </td>
                            <td width="360px"></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="ЦМК Площа в оренді, кв.м."></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxSpinEdit ID="EditCmkSqrRented" ClientInstanceName="EditCmkSqrRented" runat="server" NumberType="Float" Value='<%# Eval("cmk_sqr_rented") %>' Width="100px"
                                     Title="ЦМК Площа в оренді">
                                    <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Необхідно заповнити площу ЦМК в оренді" /> </ValidationSettings>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="ЦМК Нарахована орендна плата, грн. (без ПДВ)"></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxSpinEdit ID="EditCmkPaymentNarah" ClientInstanceName="EditCmkPaymentNarah" runat="server" NumberType="Float" Value='<%# Eval("cmk_payment_narah") %>' Width="100px"
                                     Title="ЦМК Нарахована плата за використання">
                                     <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Необхідно заповнити ЦМК Нарахована плата за використання" /> </ValidationSettings>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="ЦМК Перераховано до бюджету, грн. (без ПДВ)"></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxSpinEdit ID="EditCmkPaymentToBudget" ClientInstanceName="EditCmkPaymentToBudget" runat="server" NumberType="Float" Value='<%# Eval("cmk_payment_to_budget") %>' Width="100px"
                                     Title="ЦМК Перераховано до бюджету">
                                     <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Необхідно заповнити ЦМК Перераховано до бюджету" /> </ValidationSettings>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="ЦМК Заборгованість по орендній платі, грн. (без ПДВ)"></dx:ASPxLabel></td>
                            <td>
                                <dx:ASPxSpinEdit ID="EditCmkRentDebt" ClientInstanceName="EditCmkRentDebt" runat="server" NumberType="Float" Value='<%# Eval("cmk_rent_debt") %>' Width="100px"
                                    Title="ЦМК Заборгованість по орендній платі">
                                    <ValidationSettings Display="None"> <RequiredField IsRequired="True" ErrorText="Необхідно заповнити ЦМК Заборгованість по орендній платі" /> </ValidationSettings>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td></td>
                        </tr>
                    </table>

                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        

    </ItemTemplate>
</asp:FormView>

        <p class="SpacingPara"/>
        <asp:FormView runat="server" BorderStyle="None" ID="FormViewState" DataSourceID="SqlDataSourceRentedObj" EnableViewState="False">
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

<asp:FormView ID="errorForm" runat="server" OnDataBound="errorForm_DataBound">
    <ItemTemplate>
        <div style="padding:10px;">
        <div style="color:Red;font-weight:bold">Наступні поля мають бути заповнені перед надсиланням до ДКВ:</div>
        <ul>
            <asp:Repeater ID="errorList" runat="server">
                <ItemTemplate>
                    <li style="color:Red"><%# Eval("error_message") %> </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
        </div>
    </ItemTemplate>
</asp:FormView>

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<p class="SpacingPara"/>

<%--<dx:ASPxValidationSummary ID="ValidationSummary" runat="server" RenderMode="BulletedList" Width="800px" ClientInstanceName="ValidationSummary">
</dx:ASPxValidationSummary>--%>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonSave" runat="server" Text="Зберегти" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPMainPanel.PerformCallback('save:'); }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonSend" runat="server" Text="Надіслати" AutoPostBack="false" ValidationGroup="MainGroup">
                <ClientSideEvents Click="function (s,e) { /*if (ASPxClientEdit.ValidateEditorsInContainer(null))*/ { CPMainPanel.PerformCallback('send:'); } }" />
            </dx:ASPxButton>
        </td>
        <td> &nbsp; </td>
        <td>
            <dx:ASPxButton ID="ButtonClear" runat="server" Text="Відмінити зміни" CausesValidation="false" AutoPostBack="false">
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
