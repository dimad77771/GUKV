<%@ control language="C#" autoeventwireup="true" inherits="UserControls_FieldFixxer, App_Web_fieldfixxer.ascx.6bb32623" %>


<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function OnFieldFixxerApply(s, e) {

        PopupFieldFixxer.Hide();

        // Get the list of selected nodes in Tree View
        var columnList = "#";

        for (var i = 0; i < ListBoxGridColumns1.GetItemCount(); i++) {

            var item = ListBoxGridColumns1.GetItem(i);
            var itemValue = "" + item.value;

            if (item.selected) {
                columnList = columnList + itemValue + "++#";
            }
            else {
                // columnList = columnList + itemValue + "--#";
            }
        }

        $('#hdnSelectedFieldsUnderFix_FieldFixxer').val("columnsFixxer:" + columnList);

        e.processOnServer = true;
    }

    function OnFieldFixxerFindColumn(s, e) {

        CPGridColumns1.PerformCallback(EditColumnNamePattern1.GetText());
    }
    
    // ]]>

</script>

<table border="0" cellspacing="0" cellpadding="0">
    <asp:HiddenField runat="server" ID="hdnSelectedFieldsUnderFix_FieldFixxer" ClientIDMode="Static"/>
    <tr>
        <td><dx:ASPxTextBox ID="EditColumnNamePattern1" ClientInstanceName="EditColumnNamePattern1" runat="server" Text="" Width="300px" /></td>

        <td>&nbsp;</td>

        <td>
            <dx:ASPxButton ID="ButtonFindColumnInFixxer" runat="server" AutoPostBack="False" Text="Знайти" Width="95px">
                <ClientSideEvents Click="OnFieldFixxerFindColumn" />
            </dx:ASPxButton>
            
        </td>
    </tr>
</table>

<br/>

<dx:ASPxCallbackPanel ID="CPGridColumns1" runat="server" ClientInstanceName="CPGridColumns1" OnCallback="CallbackPanelGridColumns_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent0" runat="server">
            <dx:ASPxListBox ID="ListBoxGridColumns1" ClientInstanceName="ListBoxGridColumns1" runat="server"  
                Width="400px" Height="350px" SelectionMode="CheckColumn" ValueType="System.String" EnableSynchronization="True">
            </dx:ASPxListBox>
        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<br/>

<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <dx:ASPxButton ID="ButtonFieldFixxerApply" runat="server" Text="Вибрати" Width="148px" AutoPostBack="False" ClientInstanceName="ButtonFieldFixxerApply" OnClick="SaveFixxedColumns_Click">
                <ClientSideEvents Click="OnFieldFixxerApply" />
            </dx:ASPxButton>
        </td>

        <td>&nbsp;</td>

        <td><dx:ASPxButton ID="ButtonFieldFixxerClose" runat="server" AutoPostBack="False" Text="Відмінити" Width="148px">
            <ClientSideEvents Click="function (s,e) { PopupFieldFixxer.Hide(); }" />
        </dx:ASPxButton></td>
    </tr>
</table>
