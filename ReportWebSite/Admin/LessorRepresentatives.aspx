<%@ Page Language="C#" MasterPageFile="~/NoHeader.master" Title="Представники орендодавця" %>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (!System.Web.Security.Roles.IsUserInRole(Utils.DictionartyAdmin))
            Response.Redirect(ResolveUrl("~/Account/Restricted.aspx"));
    }
</script>
<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <h1 class="pagetitle">Представники орендодавця</h1>
    <asp:SqlDataSource ID="Representatives" runat="server"
        ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
        SelectCommand="SELECT name, name AS original_name FROM dbo.dict_lessor_representatives ORDER BY name"
        InsertCommand="INSERT INTO dbo.dict_lessor_representatives (name) VALUES (@name)"
        UpdateCommand="UPDATE TOP (1) dbo.dict_lessor_representatives SET name = @name WHERE name = @original_name OR (name IS NULL AND @original_name IS NULL)"
        DeleteCommand="DELETE TOP (1) FROM dbo.dict_lessor_representatives WHERE name = @original_name OR (name IS NULL AND @original_name IS NULL)" />
    <asp:ListView ID="Names" runat="server" DataSourceID="Representatives"
        DataKeyNames="original_name" InsertItemPosition="LastItem">
        <LayoutTemplate>
            <table><tr><th>Представник орендодавця</th><th></th></tr>
                <tr runat="server" id="itemPlaceholder"></tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr><td><%# Server.HtmlEncode(Convert.ToString(Eval("name"))) %></td><td>
                <asp:Button runat="server" CommandName="Edit" Text="Редагувати" />
                <asp:Button runat="server" CommandName="Delete" Text="Видалити" />
            </td></tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr><td><asp:TextBox runat="server" ID="EditedName" Width="32em" MaxLength="500" Text='<%# Bind("name") %>' /></td><td>
                <asp:Button runat="server" CommandName="Update" Text="Зберегти" />
                <asp:Button runat="server" CommandName="Cancel" Text="Скасувати" />
            </td></tr>
        </EditItemTemplate>
        <InsertItemTemplate>
            <tr><td><asp:TextBox runat="server" ID="NewName" Width="32em" MaxLength="500" Text='<%# Bind("name") %>' /></td><td>
                <asp:Button runat="server" CommandName="Insert" Text="Додати" />
            </td></tr>
        </InsertItemTemplate>
    </asp:ListView>
</asp:Content>
