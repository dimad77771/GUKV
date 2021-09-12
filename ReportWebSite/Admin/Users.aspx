<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Admin_Users" MasterPageFile="~/NoHeader.master" Title="Довідник користувачів" EnableViewState="true" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server" EnableViewState="true">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <script type="text/javascript">
        function OnInit(s, e) {
            AdjustSize();
        }
        function OnEndCallback(s, e) {
            AdjustSize();
        }
        function AdjustSize() {
            var height = Math.max(0, document.documentElement.clientHeight) - 260;
            grid.SetHeight(height);
        }
    </script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceFreeSquare" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select 
u.UserId,
u.UserName,
m.Email,
m.IsLockedOut,
u.LastActivityDate,
m.CreateDate,
m.LastLoginDate,
m.IsIncludedToEmail,
m.IsBigBossUser 
from aspnet_Users u
join aspnet_Membership m on m.UserId = u.UserId" 
    UpdateCommand="
update aspnet_Membership set 
Email = @Email,
IsLockedOut = @IsLockedOut,
IsIncludedToEmail = @IsIncludedToEmail,
IsBigBossUser = @IsBigBossUser
where UserId = @UserId
update aspnet_Users set 
UserName = @UserName
where UserId = @UserId" 
        ProviderName="System.Data.SqlClient">
    <UpdateParameters>
        <asp:Parameter Name="Email" />
        <asp:Parameter Name="IsLockedOut" />
		<asp:Parameter Name="IsIncludedToEmail" />
		<asp:Parameter Name="IsBigBossUser" />
        <asp:Parameter Name="UserName" />
        <asp:Parameter Name="UserId" />
    </UpdateParameters>
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="0" cellpadding="0" width="100%">
	<tr>
		<td style="width: 100%;">
			<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
				<asp:Label runat="server" ID="ASPxLabel19" Text="Довідник користувачів" CssClass="pagetitle"/>
			</p>
		</td>
		<td>
			<dx:ASPxButton ID="ASPxButton5" runat="server" 
				Text="Сформувати розсилку" 
                OnClick="ASPxButton_AllBuildings_ExportCSV_Click" Width="180px">
            </dx:ASPxButton>
		</td>
	</tr>
</table>

<dx:ASPxGridView ID="ASPxGridViewFreeSquare" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceFreeSquare" KeyFieldName="UserId" Width="100%" 
            OnRowValidating="ASPxGridViewFreeSquare_RowValidating" 
            OnStartRowEditing="ASPxGridViewFreeSquare_StartRowEditing"
            ClientInstanceName="grid">

	    <SettingsCommandButton>
		    <EditButton>
			    <Image Url="~/Styles/EditIcon.png" />
		    </EditButton>
		    <CancelButton>
			    <Image Url="~/Styles/CancelIcon.png" />
		    </CancelButton>
		    <UpdateButton>
			    <Image Url="~/Styles/SaveIcon.png" />
		    </UpdateButton>
		    <DeleteButton>
			    <Image Url="~/Styles/DeleteIcon.png" />
		    </DeleteButton>
		    <NewButton>
			    <Image Url="~/Styles/AddIcon.png" />
		    </NewButton>
		    <ClearFilterButton Text="Очистити" RenderMode="Link" />
	    </SettingsCommandButton>


        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" 
                ShowInCustomizationForm="True" CellStyle-Wrap="False" ShowEditButton="true" ShowCancelButton="true" ShowClearFilterButton="true">
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewCommandColumn>
            

<dx:GridViewDataTextColumn FieldName="UserName" Caption="ПІБ користувача" VisibleIndex="1"><HeaderStyle Wrap="True" /></dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="Email" Caption="Електронна пошта" VisibleIndex="2"><HeaderStyle Wrap="True" /></dx:GridViewDataTextColumn>
<dx:GridViewDataCheckColumn FieldName="IsLockedOut" Caption="Заблоковано" VisibleIndex="3"><HeaderStyle Wrap="True" /></dx:GridViewDataCheckColumn>
<dx:GridViewDataCheckColumn FieldName="IsIncludedToEmail" Caption="Включено в розсилку" VisibleIndex="3"><HeaderStyle Wrap="True" /></dx:GridViewDataCheckColumn>
<dx:GridViewDataCheckColumn FieldName="IsBigBossUser" Caption="Оглядач" VisibleIndex="4"><HeaderStyle Wrap="True" /></dx:GridViewDataCheckColumn>
<dx:GridViewDataDateColumn FieldName="LastActivityDate" Caption="Остання активність" VisibleIndex="5" ReadOnly="true"><EditFormSettings Visible="False" /><HeaderStyle Wrap="True" /></dx:GridViewDataDateColumn>
<dx:GridViewDataDateColumn FieldName="CreateDate" Caption="Дата створення" VisibleIndex="6" ReadOnly="true"><EditFormSettings Visible="False" /><HeaderStyle Wrap="True" /></dx:GridViewDataDateColumn>
<dx:GridViewDataDateColumn FieldName="LastLoginDate" Caption="Останній вхід" VisibleIndex="7" ReadOnly="true"><EditFormSettings Visible="False" /><HeaderStyle Wrap="True" /></dx:GridViewDataDateColumn>

        </Columns>
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsPager PageSize="20" />
        <SettingsEditing NewItemRowPosition="Bottom" EditFormColumnCount="1" />
        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" VerticalScrollableHeight="0" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard"/>
    </dx:ASPxGridView>

</asp:Content>