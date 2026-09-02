<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePasswordSuccess.aspx.cs" Inherits="Account_ChangePasswordSuccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="uk">
<head id="Head1" runat="server">
    <title>&#1055;&#1072;&#1088;&#1086;&#1083;&#1100; &#1079;&#1084;&#1110;&#1085;&#1077;&#1085;&#1086;</title>
    <link href="~/Styles/Site.css?v=20260901-password-expiration-3" rel="stylesheet" type="text/css" />
</head>
<body class="password-change-success-page">
    <form id="SuccessForm" runat="server">
        <div class="password-change-success-card">
            <h1>&#1055;&#1072;&#1088;&#1086;&#1083;&#1100; &#1079;&#1084;&#1110;&#1085;&#1077;&#1085;&#1086;</h1>
            <p>&#1042;&#1072;&#1096; &#1087;&#1072;&#1088;&#1086;&#1083;&#1100; &#1091;&#1089;&#1087;&#1110;&#1096;&#1085;&#1086; &#1079;&#1084;&#1110;&#1085;&#1077;&#1085;&#1086; &#1085;&#1072; &#1085;&#1086;&#1074;&#1080;&#1081;.</p>
            <p class="password-change-success-actions">
                <asp:HyperLink ID="LoginPageLink" runat="server" NavigateUrl="~/Account/Login.aspx">&#1055;&#1077;&#1088;&#1077;&#1081;&#1090;&#1080; &#1085;&#1072; &#1089;&#1090;&#1086;&#1088;&#1110;&#1085;&#1082;&#1091; &laquo;&#1042;&#1093;&#1110;&#1076; &#1085;&#1072; &#1089;&#1072;&#1081;&#1090;&raquo;</asp:HyperLink>
            </p>
        </div>
    </form>
</body>
</html>
