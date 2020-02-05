<%@ page language="C#" autoeventwireup="true" inherits="Reports1NF_Report1NFAccounts, App_Web_report1nfaccounts.aspx.5d94abc0" masterpagefile="~/NoHeader.master" title="Облікові записи балансоутримувачів" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 180);
    }

    function GridViewAccountsInit(s, e) {

        PrimaryGridView.PerformCallback("init:");
    }

    function GridViewAccountsEndCallback(s, e) {

        AdjustGridSizes();
    }

    function OnAddNewBalansAccount(s, e) {

        var fisrtName = EditFirstNameOrg.GetText();
        var lastName = EditLastNameOrg.GetText();
        var email = EditEmailOrg.GetText();
        var organizationId = ComboBalansOrg.GetValue();

        if (fisrtName == "") {
            alert("Будь ласка, введіть ім'я нового користувача.");
            return;
        }

        if (lastName == "") {
            alert("Будь ласка, введіть прізвище нового користувача.");
            return;
        }

        if (email == "") {
            alert("Будь ласка, вкажіть електронну адресу користувача.");
            return;
        }

        if (organizationId == null || organizationId == undefined) {
            alert("Будь ласка, виберіть організацію, до якої належить користувач.");
            return;
        }

        CPAddBalansAcc.PerformCallback('');
    }

    function OnAddNewRdaAccount(s, e) {

        var fisrtName = EditFirstNameRda.GetText();
        var lastName = EditLastNameRda.GetText();
        var email = EditEmailRda.GetText();
        var organizationId = ComboRdaOrg.GetValue();

        if (fisrtName == "") {
            alert("Будь ласка, введіть ім'я нового користувача.");
            return;
        }

        if (lastName == "") {
            alert("Будь ласка, введіть прізвище нового користувача.");
            return;
        }

        if (email == "") {
            alert("Будь ласка, вкажіть електронну адресу користувача.");
            return;
        }

        if (organizationId == null || organizationId == undefined) {
            alert("Будь ласка, виберіть районну адміністрацію, до якої належить користувач.");
            return;
        }

        CPAddRdaAcc.PerformCallback('');
    }

    function CustomButtonClick(s, e) {

        if (e.buttonID == 'btnDeleteAcc') {

            if (confirm("Видалити Обліковий Запис?")) {
                PrimaryGridView.PerformCallback("delete:" + PrimaryGridView.GetRowKey(e.visibleIndex));
            }
        }

        e.processOnServer = false;
    }

    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceAccounts" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT
       acc.id,
       usr.UserName,
       mem.Email,
       org.id AS 'organization_id',
       org.full_name,
       org.zkpo_code,
       CASE WHEN rda.name IS NULL THEN N'НІ' ELSE N'ТАК' END AS 'is_rda',
       rda.name AS 'rda_district'
    FROM reports1nf_accounts acc
    INNER JOIN aspnet_Users usr ON usr.UserId = acc.UserId
    INNER JOIN aspnet_Membership mem ON mem.UserId = acc.UserId
    INNER JOIN organizations org ON org.id = acc.organization_id
    LEFT OUTER JOIN dict_districts2 rda ON rda.id = rda_district_id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgSearchBalans" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, zkpo_code + ' - ' + full_name AS 'search_name' FROM organizations WHERE zkpo_code LIKE @zkpo AND full_name LIKE @fname AND
        (is_deleted IS NULL OR is_deleted = 0) AND master_org_id IS NULL AND id < 300000"
    OnSelecting="SqlDataSourceOrgSearchBalans_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="String" DefaultValue="%" Name="zkpo" />
        <asp:Parameter DbType="String" DefaultValue="%" Name="fname" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRda" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select id, full_name from organizations where id IN (8515, 8400, 1728, 8109, 9826, 15130, 141824, 137676, 8566, 13128) order by full_name">
</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFAccounts.aspx" Text="Облікові Записи"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFNotifications.aspx" Text="Налаштування Повідомлень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem> 
    </Items>
</dx:ASPxMenu>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Облікові Записи Балансоутримувачів та РДА" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonAddMultipleOrgAccounts" runat="server" Text="Додати Балансоутримувачів Масово" AutoPostBack="false" Width="240px"></dx:ASPxButton>

            <dx:ASPxPopupControl ID="PopupAddMultipleOrgAccounts" runat="server" ClientInstanceName="PopupAddMultipleOrgAccounts"
                HeaderText="Додати Балансоутримувачів Масово" PopupElementID="ButtonAddMultipleOrgAccounts">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxCallbackPanel ID="CPAddMultipleBalansAcc" ClientInstanceName="CPAddMultipleBalansAcc" runat="server" OnCallback="CPAddMultipleBalansAcc_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent3" runat="server">
                                    <table border="0" cellspacing="0" cellpadding="2" width="600px">
                                        <tr>
                                            <td colspan="2">
                                                Формат даних: П.І.Б.;ЕДРПОУ;Електронна адреса
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                Дані для імпорту:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxMemo ID="ASPxMemoCSVData" ClientInstanceName="ASPxMemoCSVData" runat="server" Height="400px" Width="500px">
                                                </dx:ASPxMemo>
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Імпортувати" AutoPostBack="False">
                                                    <ClientSideEvents Click="function (s, e) { CPAddMultipleBalansAcc.PerformCallback(); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                Лог імпорту:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxMemo ID="ASPxMemoLog" runat="server" Height="200px" Width="500px">
                                                </dx:ASPxMemo>                                        
                                            </td>
                                        </tr>
                                    </table>
                                </dx:panelcontent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonAddOrgAccount" runat="server" Text="Додати Балансоутримувача" AutoPostBack="false" Width="180px"></dx:ASPxButton>

            <dx:ASPxPopupControl ID="PopupAddOrgAccount" runat="server" ClientInstanceName="PopupAddOrgAccount"
                HeaderText="Додати Балансоутримувача" PopupElementID="ButtonAddOrgAccount">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">

                        <dx:ASPxCallbackPanel ID="CPAddBalansAcc" ClientInstanceName="CPAddBalansAcc" runat="server" OnCallback="CPAddBalansAcc_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent2" runat="server">

                                    <table border="0" cellspacing="0" cellpadding="2" width="100%">
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Прізвище Користувача:" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditLastNameOrg" ClientInstanceName="EditLastNameOrg" runat="server" Width="100%" /> </td>
                                        </tr>
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Ім'я Користувача:" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditFirstNameOrg" ClientInstanceName="EditFirstNameOrg" runat="server" Width="100%" /> </td>
                                        </tr>
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Електронна Адреса:" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditEmailOrg" ClientInstanceName="EditEmailOrg" runat="server" Width="100%" /> </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"> <p/> </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table border="0" cellspacing="0" cellpadding="2">
                                                    <tr>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel88" runat="server" Text="Код ЄДРПОУ:" Width="90px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="EditBalansOrgZKPO" ClientInstanceName="EditBalansOrgZKPO" runat="server" Width="120px" /> </td>
                                                        <td> <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Назва:" Width="50px" /> </td>
                                                        <td> <dx:ASPxTextBox ID="EditBalansOrgName" ClientInstanceName="EditBalansOrgName" runat="server" Width="180px" /> </td>
                                                        <td>
                                                            <dx:ASPxButton ID="BtnFindBalansOrg" ClientInstanceName="BtnFindBalansOrg" runat="server" AutoPostBack="False" Text="Знайти">
                                                                <ClientSideEvents Click="function (s, e) { ComboBalansOrg.PerformCallback(EditBalansOrgZKPO.GetText() + '|' + EditBalansOrgName.GetText()); }" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <dx:ASPxComboBox ID="ComboBalansOrg" ClientInstanceName="ComboBalansOrg" runat="server"
                                                                Width="100%" DataSourceID="SqlDataSourceOrgSearchBalans" ValueField="id" ValueType="System.Int32"
                                                                TextField="search_name" EnableSynchronization="True" OnCallback="ComboBalansOrg_Callback">
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"> <p/> </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <dx:ASPxButton ID="ButtonAddOrgAcc" ClientInstanceName="ButtonAddOrgAcc" runat="server" AutoPostBack="False" Text="Створити">
                                                    <ClientSideEvents Click="OnAddNewBalansAccount" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>

                                </dx:panelcontent>
                            </PanelCollection>

                            <ClientSideEvents EndCallback="function (s,e) { alert('Реєстрацію користувача виконано. На електронну адресу користувача надіслані його реквізити для входу на веб-сайт ДКВ.'); PopupAddOrgAccount.Hide(); }" />
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonAddRdaAccount" runat="server" Text="Додати Користувача РДА" AutoPostBack="false" Width="180px"></dx:ASPxButton>

            <dx:ASPxPopupControl ID="PopupAddRdaAccount" runat="server" ClientInstanceName="PopupAddRdaAccount"
                HeaderText="Додати Користувача РДА" PopupElementID="ButtonAddRdaAccount">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">

                        <dx:ASPxCallbackPanel ID="CPAddRdaAcc" ClientInstanceName="CPAddRdaAcc" runat="server" OnCallback="CPAddRdaAcc_Callback">
                            <PanelCollection>
                                <dx:panelcontent ID="Panelcontent1" runat="server">

                                    <table border="0" cellspacing="0" cellpadding="2">
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Прізвище Користувача:" Width="150px" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditLastNameRda" ClientInstanceName="EditLastNameRda" runat="server" Width="300px" /> </td>
                                        </tr>
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Ім'я Користувача:" Width="150px" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditFirstNameRda" ClientInstanceName="EditFirstNameRda" runat="server" Width="300px" /> </td>
                                        </tr>
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Електронна Адреса:" Width="150px" /> </td>
                                            <td> <dx:ASPxTextBox ID="EditEmailRda" ClientInstanceName="EditEmailRda" runat="server" Width="300px" /> </td>
                                        </tr>
                                        <tr>
                                            <td> <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Районна Адміністрація:" Width="150px" /> </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ComboRdaOrg" ClientInstanceName="ComboRdaOrg" runat="server"
                                                    Width="300px" DataSourceID="SqlDataSourceRda" ValueField="id" ValueType="System.Int32"
                                                    TextField="full_name" EnableSynchronization="True">
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"> <p/> </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <dx:ASPxButton ID="ButtonAddRdaAcc" ClientInstanceName="ButtonAddRdaAcc" runat="server" AutoPostBack="False" Text="Створити">
                                                    <ClientSideEvents Click="OnAddNewRdaAccount" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>

                                </dx:panelcontent>
                            </PanelCollection>

                            <ClientSideEvents EndCallback="function (s,e) { alert('Реєстрацію користувача виконано. На електронну адресу користувача надіслані його реквізити для входу на веб-сайт ДКВ.'); PopupAddRdaAccount.Hide(); }" />
                        </dx:ASPxCallbackPanel>

                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewAccountsExporter" runat="server" 
    FileName="ОбліковіЗаписи" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView
    ID="PrimaryGridView"
    ClientInstanceName="PrimaryGridView"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceAccounts"
    KeyFieldName="id"
    OnCustomCallback="GridViewAccounts_CustomCallback" >

    <Columns>
        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
            <CustomButtons>                
                <dx:GridViewCommandColumnCustomButton ID="btnDeleteAcc"><Image  Url="../Styles/DeleteIcon.png" ToolTip="Видалити Обліковий Запис" /></dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
        </dx:GridViewCommandColumn>
        <dx:GridViewDataTextColumn FieldName="UserName" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Caption="Ім'я Користувача">
            <Settings AllowHeaderFilter="False" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Email" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Електронна Адреса Користувача">
            <Settings AllowHeaderFilter="False" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Назва Організації Користувача" Width="300px">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Код ЄДРПОУ Організації Користувача">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("zkpo_code") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_rda" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Користувач Належить до РДА" />
        <dx:GridViewDataDateColumn FieldName="rda_district" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="6" Caption="Район РДА Користувача" />
    </Columns>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager AlwaysShowPager="true" PageSize="25"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="True"
        ShowFilterBar="Visible"
        ShowHeaderFilterButton="True"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        VerticalScrollBarMode="Visible"
        VerticalScrollBarStyle="Virtual" />
    <SettingsCookies CookiesID="GUKV.Reports1NF.Accounts" Version="A2_2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewAccountsInit" EndCallback="GridViewAccountsEndCallback" CustomButtonClick="CustomButtonClick" />
</dx:ASPxGridView>

</asp:Content>
