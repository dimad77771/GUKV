<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RishProjects.aspx.cs" Inherits="Objects_RishProjects"
    MasterPageFile="~/NoHeader.master" Title="Проекти Рішень" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>

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
        <dx:MenuItem NavigateUrl="../Objects/RishProjects.aspx" Text="Розпорядчі Документи"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<uc4:RishProjectList ID="RishProjectList1" runat="server"/>

</center>

</asp:Content>

