<%@ page language="C#" autoeventwireup="true" inherits="Objects_RishProjects, App_Web_rishprojects.aspx.a8c63d73" masterpagefile="~/NoHeader.master" title="Проекти Рішень" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>

<%@ Register src="../BPRishProject/RishProjectList.ascx" tagname="RishProjectList" tagprefix="uc4" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

// <![CDATA[

// ]]>

</script>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left">
    <Items>
        <dx:MenuItem NavigateUrl="../Objects/Objects.aspx" Text="Передача Прав на Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Objects/RishProjects.aspx" Text="Проекти Рішень"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<uc4:RishProjectList ID="RishProjectList1" runat="server"/>

</center>

</asp:Content>

