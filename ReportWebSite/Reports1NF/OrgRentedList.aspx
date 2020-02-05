<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgRentedList.aspx.cs" Inherits="Reports1NF_OrgRentedList"
    MasterPageFile="~/NoMenu.master" Title="Договори Орендування" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

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

    window.onresize = function () { AdjustGridSize(); };

    function AdjustGridSize() {

        PrimaryGridView.SetHeight(window.innerHeight - 180);
    }

    function PrimaryGridViewEndCallback(s, e) {

        AdjustGridSize();

        CPStatus.PerformCallback('');
    }

    function CustomButtonClick(s, e) {

        if (e.buttonID == 'btnEditAgreement') {

            var cardUrl = "../Reports1NF/OrgRentedObject.aspx?rid=" + <%= Request.QueryString["rid"] %> + "&aid=" + PrimaryGridView.GetRowKey(e.visibleIndex);

            window.location = cardUrl;
        }

        if (e.buttonID == 'btnDeleteAgreement') {

            if (confirm("Видалити Договір Орендування?")) {
                PrimaryGridView.PerformCallback(JSON.stringify({ AgreementToDeleteID: PrimaryGridView.GetRowKey(e.visibleIndex) }));
            }
        }

        e.processOnServer = false;
    }

    function OnAddNewAgreement(s, e) {

        var agreementNum = EditAgreementNum.GetText();
        var agreementDate = EditAgreementDate.GetValue();
        var agreementStart = EditAgreementStart.GetValue();
        var agreementFinish = EditAgreementFinish.GetValue();
        var buildingId = ComboBuilding.GetValue();

        if (agreementNum == "") {
            alert("Будь ласка, введіть номер Договору оренди.");
            return;
        }

        if (agreementDate == null || agreementDate == undefined) {
            alert("Будь ласка, введіть дату Договору оренди.");
            return;
        }

        if (agreementStart == null || agreementStart == undefined) {
            alert("Будь ласка, введіть дату початку оренди за Договором.");
            return;
        }

        if (agreementFinish == null || agreementFinish == undefined) {
            alert("Будь ласка, введіть дату закінчення оренди за Договором.");
            return;
        }

        if (buildingId == null || buildingId == undefined) {
            alert("Будь ласка, виберіть адресу, за якою розташовано орендоване приміщення.");
            return;
        }

        PopupAddAgreement.Hide();

        PrimaryGridView.PerformCallback(JSON.stringify({
            AgreementNum: agreementNum,
	        AgreementDateYear: agreementDate.getFullYear(),
            AgreementDateMonth: agreementDate.getMonth(),
            AgreementDateDay: agreementDate.getDate(),

            AgreementStartYear: agreementStart.getFullYear(),
            AgreementStartMonth: agreementStart.getMonth(),
            AgreementStartDay: agreementStart.getDate(),

            AgreementFinishYear: agreementFinish.getFullYear(),
            AgreementFinishMonth: agreementFinish.getMonth(),
            AgreementFinishDay: agreementFinish.getDate(),

            BuildingID: buildingId,
        }));
    }

    function ShowSendingLog() {
        var diagnosticWindow = window.open("SendingRentedObjectsDiagnostic.aspx?rid=<%= ReportID  %>","","width=600,height=400");        
        diagnosticWindow.focus();        
    }

    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaStatus" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT CASE WHEN COUNT(*) > 0 THEN 0 ELSE 1 END AS 'report_arenda_status'
        FROM reports1nf_arenda_rented ar
        INNER JOIN reports1nf rep ON rep.id = ar.report_id
        LEFT OUTER JOIN arenda_rented a ON a.id = ar.arenda_rented_id
        WHERE ar.report_id = @rep_id AND (a.is_deleted IS NULL OR a.is_deleted = 0) AND (ar.modify_date > rep.create_date AND (ar.submit_date IS NULL OR ar.modify_date > ar.submit_date))">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceReportRented" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT ar.id AS 'arenda_id', ar.agreement_num, ar.agreement_date, ar.rent_start_date, ar.rent_finish_date, ar.rent_square, ar.modify_date, ar.submit_date, ar.is_deleted,
        a.id AS 'id_in_eis', a.is_deleted AS 'is_deleted_in_eis',
        dict_districts2.name AS 'district', bld.addr_street_name, bld.addr_nomer,
        CASE
            WHEN ar.is_cmk = 1 AND ar.is_subarenda = 1 THEN 'СУБОРЕНДА, ЦМК' 
            WHEN ar.is_cmk = 1 THEN 'ЦМК'
            WHEN ar.is_subarenda = 1 THEN 'СУБОРЕНДА'
            ELSE '' 
        END as agreement_type,
        CASE WHEN (((ar.is_valid = 0) OR (ar.is_valid IS NULL)) OR ((ar.submit_date IS NULL OR ar.modify_date > ar.submit_date))) THEN N'НІ' ELSE N'ТАК' END AS 'is_submitted',
        ar.modified_by
        FROM reports1nf_arenda_rented ar
        INNER JOIN reports1nf rep ON rep.id = ar.report_id
        LEFT OUTER JOIN reports1nf_buildings bld ON bld.unique_id = ar.building_1nf_unique_id
        LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = bld.addr_distr_new_id
        LEFT OUTER JOIN arenda_rented a ON a.id = ar.arenda_rented_id
        WHERE ar.report_id = @rep_id AND (a.is_deleted IS NULL OR a.is_deleted = 0) ORDER BY ar.modify_date DESC" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStreets" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, name from dict_streets where (not name is null) and (RTRIM(LTRIM(name)) <> '')">
</mini:ProfiledSqlDataSource>


<%-- убрано условие после where (id < 100000) AND--%>
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

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
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

<p class="SpacingPara"/>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Перелік договорів використання  об'єктів  інших організацій" CssClass="pagetitle"/>
</p>

<table border="0" cellspacing="0" cellpadding="2">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonAddAgreement" runat="server" Text="Додати Новий Договір" AutoPostBack="false"></dx:ASPxButton>

            <dx:ASPxPopupControl ID="PopupAddAgreement" runat="server" ClientInstanceName="PopupAddAgreement"
                HeaderText="Введення нового Договору орендування" PopupElementID="ButtonAddAgreement" Width="330px">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Реквізити договору" Width="100%">
                            <ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">

                                    <table border="0" cellspacing="0" cellpadding="2" width="100%">
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Номер Договору:" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditAgreementNum" ClientInstanceName="EditAgreementNum" runat="server" Width="100%" /> </td>
                                        </tr>
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Дата Договору:" /> </td>
                                            <td> <dx:ASPxDateEdit ID="EditAgreementDate" ClientInstanceName="EditAgreementDate" runat="server" Width="100%" /> </td>
                                        </tr>
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Початок Оренди:" /> </td>
                                            <td> <dx:ASPxDateEdit ID="EditAgreementStart" ClientInstanceName="EditAgreementStart" runat="server" Width="100%" /> </td>
                                        </tr>
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Закінчення Оренди:" /> </td>
                                            <td> <dx:ASPxDateEdit ID="EditAgreementFinish" ClientInstanceName="EditAgreementFinish" runat="server" Width="100%" /> </td>
                                        </tr>
                                    </table>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="Адреса Приміщення">
                            <ContentPaddings PaddingTop="4px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="2px" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent5" runat="server">

                                    <table border="0" cellspacing="0" cellpadding="2">
                                        <tr>
                                            <td> <dx:ASPxLabel ID="LabelAddrStreet" runat="server" Text="Назва вулиці:" Width="90px" /> </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ComboStreet" runat="server" ClientInstanceName="ComboStreet"
                                                    DataSourceID="SqlDataSourceDictStreets" DropDownStyle="DropDownList" ValueType="System.Int32"
                                                    TextField="name" ValueField="id" Width="220px" IncrementalFilteringMode="StartsWith"
                                                    FilterMinLength="3" EnableCallbackMode="True" CallbackPageSize="50" EnableViewState="False"
                                                    EnableSynchronization="False">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { ComboBuilding.PerformCallback(ComboStreet.GetValue().toString()); }" />
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td> <dx:ASPxLabel ID="LabelAddrPickerNumber" runat="server" Text="Номер будинку:" Width="95px" /> </td>
                                            <td>
                                                <dx:ASPxComboBox runat="server" ID="ComboBuilding" ClientInstanceName="ComboBuilding"
                                                    DataSourceID="SqlDataSourceDictBuildings" DropDownStyle="DropDownList" TextField="nomer"
                                                    ValueField="id" ValueType="System.Int32" Width="100px" IncrementalFilteringMode="StartsWith"
                                                    EnableSynchronization="False" OnCallback="ComboBuilding_Callback">
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                        <p class="SpacingPara"/>

                        <dx:ASPxButton ID="ButtonDoAddAgreement" ClientInstanceName="ButtonDoAddAgreement" runat="server" AutoPostBack="False" Text="Створити">
                            <ClientSideEvents Click="OnAddNewAgreement" />
                        </dx:ASPxButton>

                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonSendAll" runat="server" Text="Надіслати всі зміни в ДКВ" AutoPostBack="false"></dx:ASPxButton>

            <dx:ASPxTimer ID="ProgressTimer" runat="server" ClientInstanceName="ProgressTimer" Enabled="False" Interval="3000">
                <ClientSideEvents Tick="function(s, e) { CPProgress.PerformCallback('timer:'); }" />
            </dx:ASPxTimer>

            <dx:ASPxPopupControl ID="PopupSendAll" runat="server" ClientInstanceName="PopupSendAll"
                HeaderText="Надсилання до ДКВ усіх змінених договорів орендування" PopupElementID="ButtonSendAll">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">

                        <dx:ASPxCallbackPanel ID="CPProgress" ClientInstanceName="CPProgress" runat="server" OnCallback="CPProgress_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent1" runat="server">

                                    <dx:ASPxLabel ID="LabelProgress" ClientInstanceName="LabelProgress" runat="server" Width="650px" />

                                    <dx:ASPxProgressBar ID="ProgressSendAll" ClientInstanceName="ProgressSendAll" runat="server" Width="650px" Height="24px" Minimum="0" Maximum="100" />

                                </dx:panelcontent>
                            </PanelCollection>

                            <ClientSideEvents EndCallback="function (s,e) {
                                        var id = '' + CPProgress.cpWorkItemId;
                                        if (id.length > 0)
                                        {
                                            ProgressTimer.SetEnabled(true);
                                        }
                                        else
                                        {
                                            ProgressTimer.SetEnabled(false);
                                            alert('Обробку завершено.');
                                            PopupSendAll.Hide();
                                            PrimaryGridView.PerformCallback('init:');
                                        }
                                    }" />
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>

                <ClientSideEvents PopUp="function (s,e) { CPProgress.PerformCallback('init:'); ShowSendingLog(); }" />
            </dx:ASPxPopupControl>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_RentedObjects_SaveAs" 
                PopupElementID="ASPxButton_RentedObjects_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxButton ID="ASPxButton2" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_RentedObjects_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_RentedObjects_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxButton ID="ASPxButton_RentedObjects_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxCallbackPanel ID="CPStatus" ClientInstanceName="CPStatus" runat="server" OnCallback="CPStatus_Callback">
                <PanelCollection>
                    <dx:panelcontent ID="Panelcontent6" runat="server">

                        <asp:FormView runat="server" BorderStyle="None" ID="ArendaStatusForm" DataSourceID="SqlDataSourceArendaStatus" EnableViewState="False">
                            <ItemTemplate>

                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Необхідно надіслати внесені зміни до ДКВ" ClientVisible='<%# 0.Equals(Eval("report_arenda_status")) %>' ForeColor="Red" />
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Усі зміни надіслано до ДКВ" ClientVisible='<%# 1.Equals(Eval("report_arenda_status")) %>' />

                            </ItemTemplate>
                        </asp:FormView>

                    </dx:panelcontent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterRentedObjects" runat="server" 
    FileName="Arenda" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView ID="PrimaryGridView" ClientInstanceName="PrimaryGridView" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceReportRented" KeyFieldName="arenda_id" Width="100%"
    OnHtmlRowPrepared="PrimaryGridView_HtmlRowPrepared"
    OnCustomCallback="PrimaryGridView_CustomCallback"
    OnCustomButtonInitialize="PrimaryGridView_CustomButtonInitialize"
    OnHtmlDataCellPrepared="PrimaryGridView_HtmlDataCellPrepared"
    OnCustomColumnSort="PrimaryGridView_CustomColumnSort" >

    <Columns>
        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Name="CmdColumn">
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="btnEditAgreement"><Image  Url="../Styles/EditIcon.png" ToolTip="Редагувати Договір Орендування" /></dx:GridViewCommandColumnCustomButton>
                <dx:GridViewCommandColumnCustomButton ID="btnDeleteAgreement"><Image  Url="../Styles/DeleteIcon.png" ToolTip="Видалити Договір Орендування" /></dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
        </dx:GridViewCommandColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_num" VisibleIndex="1" Caption="Номер Договору Орендування"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="agreement_date" VisibleIndex="2" Caption="Дата Договору Орендування"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="3" Caption="Район" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" VisibleIndex="4" Caption="Назва вулиці" Width="150px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="5" Caption="Номер будинку"><Settings SortMode="Custom" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="rent_start_date" VisibleIndex="6" Caption="Початок Оренди"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="rent_finish_date" VisibleIndex="7" Caption="Закінчення Оренди"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="8" Caption="Орендована Площа, кв.м."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_type" VisibleIndex="9" Caption="Тип договору"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="10" Caption="Коли Змінено"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="submit_date" VisibleIndex="11" Caption="Коли Надіслано в ДКВ"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="is_submitted" VisibleIndex="12" Caption="Надіслано в ДКВ"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="modified_by" VisibleIndex="12" Caption="Користувач"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>        
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="agreement_num" ShowInColumn="CmdColumn" SummaryType="Count" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowFilterBar="Visible"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="15" />
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.Reports1NF.RentedObjList" Enabled="True" Version="A2_1" />

    <ClientSideEvents
        Init="function (s,e) { PrimaryGridView.PerformCallback('init:'); }"
        CustomButtonClick="CustomButtonClick"
        EndCallback="PrimaryGridViewEndCallback" />
</dx:ASPxGridView>

</asp:Content>
