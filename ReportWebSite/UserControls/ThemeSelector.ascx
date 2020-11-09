<%@ Control Language="C#" %>

<script runat="server">
    
    protected void Page_Load(object sender, EventArgs e) {
        ThemesContainerRepeater.DataBind();            
    }
    protected void Menu_DataBinding(object sender, EventArgs e) {
        ASPxMenu menu = (ASPxMenu)sender;
        RepeaterItem item = menu.NamingContainer as RepeaterItem;
        if(item != null) {
            ThemeGroupModel group = item.DataItem as ThemeGroupModel;
            foreach(ThemeModel theme in group.Themes) {
                DevExpress.Web.MenuItem menuItem = menu.Items.Add(theme.Title, theme.Name);
                menuItem.Image.SpriteProperties.CssClass = "DemoSprite " + theme.SpriteCssClass;
                menuItem.Selected = (theme.Name == Utils.CurrentTheme);
            }
        }
    }
</script>

<script type="text/javascript">
    DXTheme.CurrentThemeCookieKey = "<%= Utils.CurrentThemeCookieKey %>";
</script>
<a class="DemoSprite Button" id="ThemeSelectorButton" onclick="DXTheme.ShowThemeSelector()">Вигляд</a>
<dx:ASPxPopupControl ID="ThemeSelectorPopup" 
    ClientInstanceName="ThemeSelectorPopup" CssClass="ThemeSelectorPopup" runat="server"  EnableTheming="false"
    PopupElementID="ThemeSelectorButton" PopupHorizontalAlign="LeftSides" ShowShadow="true" PopupAnimationType="None"
    PopupAction="LeftMouseClick" 
    PopupVerticalAlign="Below" PopupVerticalOffset="1" ShowHeader="False" >
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <div id="ThemesContainer">
                <asp:Repeater runat="server" ID="ThemesContainerRepeater" EnableViewState="false" DataSource="<%# ThemesModel.Current.Groups %>">
                    <ItemTemplate>
                        <dx:ASPxMenu ID="ThemeGroupMenu" CssClass="ThemeGroupMenu" runat="server" EnableTheming="false"
                            EnableViewState="false"
                              ItemImagePosition="Top"
                            OnDataBinding="Menu_DataBinding" >
                            <ClientSideEvents ItemClick="function(s,e){ DXTheme.SetCurrentTheme(e.item.name); }" />
                        </dx:ASPxMenu>
                        <b class="Clear"></b>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="DXTheme.ThemeSelectorPopupPopUp" CloseUp="DXTheme.ThemeSelectorPopupCloseUp" />
</dx:ASPxPopupControl>
