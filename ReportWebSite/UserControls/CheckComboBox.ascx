<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CheckComboBox.ascx.cs" Inherits="UserControls_CheckComboBox" %>

<%-- 
    TODO:
        The JavaScript should be wrapped into a generic container so multiple CheckComboBox 
        controls may be used on the same page at the same time.
--%>
<script type="text/javascript">
    function OnListBoxSelectionChanged(listBox, args) {
        <%
        if (ClientSideEvents.SelectionChanged != null && ClientSideEvents.SelectionChanged.Trim().Length > 0)
        {
        %>
        var values = [listBox.GetItem(args.index).value];
        (<%= ClientSideEvents.SelectionChanged %>)(
            <%= ClientInstanceName ?? "null" %>, {
                removedValues: (args.isSelected ? [] : values),
                addedValues: (args.isSelected ? values : [])
            }
        );
        <%
        }
        %>
        UpdateText();
    }
    function UpdateText() {
        var selectedItems = <%= ASPxListBox1.ClientID %>.GetSelectedItems();
        <%= ASPxDropDownEdit1.ClientID %>.SetText(GetSelectedItemsText(selectedItems));
    }
    function SynchronizeListBoxValues(dropDown, args) {
        <%= ASPxListBox1.ClientID %>.UnselectAll();
        var texts = dropDown.GetText().split('<%= ValueSeparator %>');
        var values = GetValuesByTexts(texts);
        <%= ASPxListBox1.ClientID %>.SelectValues(values);
        UpdateText();
    }
    function GetSelectedItemsText(items) {
        var texts = [];
        for (var i = 0; i < items.length; i++)
            texts.push(items[i].text);
        return texts.join('<%= ValueSeparator %>');
    }
    function GetValuesByTexts(texts) {
        var actualValues = [];
        var item;
        for (var i = 0; i < texts.length; i++) {
            item = <%= ASPxListBox1.ClientID %>.FindItemByText(texts[i]);
            if (item != null)
                actualValues.push(item.value);
        }
        return actualValues;
    }
    <%
    if (!string.IsNullOrEmpty(this.ClientInstanceName))
    {
    %>
    var <%= ClientInstanceName %> = null;
    (function () {
        function CheckBomboBoxApi() {
            var self = this;

            self.init = function(options) {
                self.options = options;
            };

            self.getListBox = function() {
                return eval(self.options.ListBox);
            };

            self.GetValue = function() {
                return self.GetSelectedValues().join(',');
            };

            self.GetSelectedValues = function() {
                return self.getListBox().GetSelectedValues();
            };

            self.UnselectValues = function(values) {
                self.getListBox().UnselectValues(values);
            };

            self.SelectValues = function(values) {
                self.getListBox().SelectValues(values);
            };
        }

        <%= ClientInstanceName %> = new CheckBomboBoxApi();
        <%= ClientInstanceName %>.init({
            DropDownEdit: '<%= ASPxDropDownEdit1.ClientID %>',
            ListBox: '<%= ASPxListBox1.ClientID %>',
            Button: '<%= ASPxButton1.ClientID %>'
        });
    })();
    <%
    }
    %>
</script>
<dx:ASPxDropDownEdit ID="ASPxDropDownEdit1" runat="server" ReadOnly="True">
    <DropDownWindowTemplate>
        <dx:ASPxListBox ID="ASPxListBox1" runat="server" SelectionMode="CheckColumn" 
            Width="100%" oncallback="ASPxListBox1_Callback">
            <Border BorderStyle="None" />
            <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
            <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" />
        </dx:ASPxListBox>
        <table style="width: 100%">
            <tr>
                <td style="padding: 4px">
                    <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close"
                        Style="float: right">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </DropDownWindowTemplate>
    <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" />
</dx:ASPxDropDownEdit>


