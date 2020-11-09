<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrPlataVipiski.aspx.cs" Inherits="OrendnaPlata_OrPlataVipiski"
    MasterPageFile="~/NoHeader.master" Title="Орендна Плата - Виписки з Казначейства" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/SaveReportCtrl.ascx" tagname="SaveReportCtrl" tagprefix="uc1" %>
<%@ Register src="../UserControls/AddressPicker.ascx" tagname="AddressPicker" tagprefix="uc2" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

// <![CDATA[

    function OnButtonGenerateVipiskiClick(s, e) {

        var startDate = DateEditFilterByStartDate.GetValue();
        var endDate = DateEditFilterByEndDate.GetValue();

        if (startDate == null || startDate == undefined ||
            endDate == null || endDate == undefined) {

            alert("Будь ласка, введіть початкову та кінцеву дату періоду, за який необхідно згенерувати звіт.");

            e.processOnServer = false;
        }
        else {
            e.processOnServer = true;
        }
    }

// ]]>

</script>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByBalans.aspx" Text="Балансоутримувачі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByRenters.aspx" Text="Орендарі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByBuildings.aspx" Text="Приміщення"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataTotals.aspx" Text="Зведений Звіт"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataNotSubmitted.aspx" Text="Організації, Що Не Подали Звіт"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataObjectsUse.aspx" Text="Використання Неж. Фонду"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataVipiski.aspx" Text="Виписки"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentOccupations" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_rent_occupation">
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="4" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxCheckBox ID="CheckFilterByZKPO" ClientInstanceName="CheckFilterByZKPO" runat="server" Checked='False' Text="Фільтрувати За Кодом ЄДРПОУ" >
                <ClientSideEvents CheckedChanged="function (s,e) { EditFilterByZKPO.SetEnabled(CheckFilterByZKPO.GetChecked()); }" />
            </dx:ASPxCheckBox>
        </td>

        <td> &nbsp; </td>

        <td>
            <dx:ASPxTextBox ID="EditFilterByZKPO" ClientInstanceName="EditFilterByZKPO" runat="server" Width="320px" ClientEnabled="False"></dx:ASPxTextBox>
        </td>
    </tr>

    <tr>
        <td>
            <dx:ASPxCheckBox ID="CheckFilterByOrgName" ClientInstanceName="CheckFilterByOrgName" runat="server" Checked='False' Text="Фільтрувати За Назвою Організації" >
                <ClientSideEvents CheckedChanged="function (s,e) { EditFilterByOrgName.SetEnabled(CheckFilterByOrgName.GetChecked()); }" />
            </dx:ASPxCheckBox>
        </td>

        <td> &nbsp; </td>

        <td>
            <dx:ASPxTextBox ID="EditFilterByOrgName" ClientInstanceName="EditFilterByOrgName" runat="server" Width="320px" ClientEnabled="False"></dx:ASPxTextBox>
        </td>
    </tr>

    <tr>
        <td valign="top">
            <dx:ASPxCheckBox ID="CheckFilterByOccupation" ClientInstanceName="CheckFilterByOccupation" runat="server" Checked='False' Text="Фільтрувати За Сферою Діяльності" >
                <ClientSideEvents CheckedChanged="function (s,e) { ListBoxFilterByOccupation.SetEnabled(CheckFilterByOccupation.GetChecked()); }" />
            </dx:ASPxCheckBox>
        </td>

        <td> &nbsp; </td>

        <td>
            <dx:ASPxListBox ID="ListBoxFilterByOccupation" ClientInstanceName="ListBoxFilterByOccupation" runat="server"
                DataSourceID="SqlDataSourceRentOccupations" TextField="name" ValueField="id" ValueType="System.Int32"
                Width="320px" Height="250px" SelectionMode="CheckColumn" ClientEnabled="False" >
            </dx:ASPxListBox>
        </td>
    </tr>

    <tr>
        <td>
            <dx:ASPxLabel ID="LabelFilterByStartDate" runat="server" Text="Початкова Дата:"></dx:ASPxLabel>
        </td>

        <td> &nbsp; </td>

        <td>
            <dx:ASPxDateEdit ID="DateEditFilterByStartDate" ClientInstanceName="DateEditFilterByStartDate" runat="server" Width="320px" />
        </td>
    </tr>

    <tr>
        <td>
            <dx:ASPxLabel ID="LabelFilterByEndDate" runat="server" Text="Кінцева Дата:"></dx:ASPxLabel>
        </td>

        <td> &nbsp; </td>

        <td>
            <dx:ASPxDateEdit ID="DateEditFilterByEndDate" ClientInstanceName="DateEditFilterByEndDate" runat="server" Width="320px" />
        </td>
    </tr>

    <tr>
        <td colspan="3">
            <br/>

            <dx:ASPxButton ID="ButtonGenerateVipiski" runat="server" AutoPostBack="True" Text="Згенерувати звіт" Width="148px"
                OnClick="ButtonGenerateVipiski_Click">
                <ClientSideEvents Click="OnButtonGenerateVipiskiClick" />
            </dx:ASPxButton>
        </td>
    </tr>
</table>

</asp:Content>
