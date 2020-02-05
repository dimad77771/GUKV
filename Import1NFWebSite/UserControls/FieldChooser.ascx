<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FieldChooser.ascx.cs" Inherits="UserControls_FieldChooser" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function OnFieldChooserApply(s, e) {

        PopupFieldChooser.Hide();

        // Get the list of selected nodes in Tree View
        var columnList = "#";

        for (var i = 0; i < ListBoxGridColumns.GetItemCount(); i++) {

            var item = ListBoxGridColumns.GetItem(i);
            var itemValue = "" + item.value;

            columnList = columnList + itemValue;

            if (item.selected) {
                columnList = columnList + "++#";
            }
            else {
                columnList = columnList + "--#";
            }
        }

        var callbackParam = "columns:" + columnList;

        GridViewSubmitters.PerformCallback(callbackParam);
    }

    function OnFieldChooserFindColumn(s, e) {

        CPGridColumns.PerformCallback(EditColumnNamePattern.GetText());
    }

    // ]]>

</script>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td><dx:ASPxTextBox ID="EditColumnNamePattern" ClientInstanceName="EditColumnNamePattern" runat="server" Text="" Width="300px" /></td>

        <td>&nbsp;</td>

        <td><dx:ASPxButton ID="ButtonFindColumnInChooser" runat="server" AutoPostBack="False" Text="Знайти" Width="95px">
            <ClientSideEvents Click="OnFieldChooserFindColumn" />
        </dx:ASPxButton></td>
    </tr>
</table>

<br/>

<dx:ASPxCallbackPanel ID="CPGridColumns" runat="server" ClientInstanceName="CPGridColumns" OnCallback="CallbackPanelGridColumns_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent0" runat="server">
            <dx:ASPxListBox ID="ListBoxGridColumns" ClientInstanceName="ListBoxGridColumns" runat="server"
                Width="400px" Height="350px" SelectionMode="CheckColumn" ValueType="System.String" >
            </dx:ASPxListBox>
        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<br/>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td><dx:ASPxButton ID="ButtonFieldChooserApply" runat="server" AutoPostBack="False" Text="Вибрати" Width="148px">
            <ClientSideEvents Click="OnFieldChooserApply" />
        </dx:ASPxButton></td>

        <td>&nbsp;</td>

        <td><dx:ASPxButton ID="ButtonFieldChooserClose" runat="server" AutoPostBack="False" Text="Відмінити" Width="148px">
            <ClientSideEvents Click="function (s,e) { PopupFieldChooser.Hide(); }" />
        </dx:ASPxButton></td>
    </tr>
</table>
