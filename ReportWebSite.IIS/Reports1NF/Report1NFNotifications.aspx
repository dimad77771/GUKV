<%@ page language="C#" autoeventwireup="true" inherits="Reports1NF_Report1NFNotifications, App_Web_report1nfnotifications.aspx.5d94abc0" masterpagefile="~/NoHeader.master" title="Налаштування повідомлень" %>

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

    function GridViewNotificationsInit(s, e) {

        PrimaryGridView.PerformCallback("init:");
    }

    function GridViewNotificationsEndCallback(s, e) {

        AdjustGridSizes();
    }

    function OnSaveSettings(s, e) {

        PrimaryGridView.GetSelectedFieldValues("id", GetSelectedFieldValuesCallback);
    }

    function GetSelectedFieldValuesCallback(values) {

        var idList = "";

        for (var i = 0; i < values.length; i++) {
            idList += values[i];
            idList += "|";
        }

        PrimaryGridView.PerformCallback("set:" + idList);
    }

    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceNotifications" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT org.id, org.zkpo_code, org.full_name, uns.is_notify FROM organizations org
        LEFT OUTER JOIN user_notification_settings uns ON uns.UserId = @usrid AND uns.organization_id = org.id
        WHERE org.id IN (SELECT organization_id FROM reports1nf)"
    OnSelecting="SqlDataSourceNotifications_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Guid" Name="usrid" />
    </SelectParameters>
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
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Будь ласка, виберіть у списку ті організації, звіти яких Вам необхідно контролювати" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonSaveNotifications" runat="server" Text="Зберегти Налаштування" AutoPostBack="false" Width="180px">
                <ClientSideEvents Click="OnSaveSettings" />
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridView
    ID="PrimaryGridView"
    ClientInstanceName="PrimaryGridView"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceNotifications"
    KeyFieldName="id"
    OnCustomCallback="GridViewNotifications_CustomCallback" >

    <Columns>
        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowSelectCheckbox="true">
        </dx:GridViewCommandColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Caption="Код ЄДРПОУ Організації" Width="100px">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("id") + ")\">" + Eval("zkpo_code") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Назва Організації" Width="500px">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("id") + ")\">" + Eval("full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="3" Visible="false" Caption="ID Організації">
        </dx:GridViewDataTextColumn>
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
    <SettingsCookies CookiesID="GUKV.Reports1NF.Notifications" Version="A2_2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewNotificationsInit" EndCallback="GridViewNotificationsEndCallback" />
</dx:ASPxGridView>

</asp:Content>
