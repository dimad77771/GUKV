<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DicOrgOccupation.aspx.cs" Inherits="Admin_DicOrgOccupation" MasterPageFile="~/NoMenu.master" Title="Довідник видів діяльності" EnableViewState="true" %>

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

<mini:ProfiledSqlDataSource ID="SqlDataSourceDicOrgOccupation" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM [dict_org_occupation]" 
    DeleteCommand="DELETE FROM [dict_org_occupation] WHERE id = @id" 
    InsertCommand="
insert into [dict_org_occupation] (id, name, branch_code, modified_by, modify_date)
values (@id, @name, @branch_code, @modified_by, @modify_date)" 
    UpdateCommand="
update [dict_org_occupation] set 
name = @name,
branch_code = @branch_code,
modified_by = @modified_by, 
modify_date = @modify_date
where id = @id" 
        OnInserting="SqlDataSourceDicOrgOccupation_Inserting" 
        OnUpdating="SqlDataSourceDicOrgOccupation_Updating" ProviderName="System.Data.SqlClient">
    <DeleteParameters>
        <asp:Parameter Name="id" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="id" />
        <asp:Parameter Name="name" />
        <asp:Parameter Name="branch_code" />
        <asp:Parameter Name="modified_by" />
        <asp:Parameter Name="modify_date" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="name" />
        <asp:Parameter Name="branch_code" />
        <asp:Parameter Name="modified_by" />
        <asp:Parameter Name="modify_date" />
        <asp:Parameter Name="id" />
    </UpdateParameters>
</mini:ProfiledSqlDataSource>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Довідник видів діяльності" CssClass="pagetitle"/>
</p>

<dx:ASPxGridView ID="ASPxGridViewDicOrgOccupation" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceDicOrgOccupation" KeyFieldName="id" Width="100%" 
            OnRowValidating="ASPxGridViewDicOrgOccupation_RowValidating" 
            OnStartRowEditing="ASPxGridViewDicOrgOccupation_StartRowEditing"
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
                ShowInCustomizationForm="True" CellStyle-Wrap="False" ShowEditButton="true" 
                ShowNewButton="true" ShowDeleteButton="True" ShowCancelButton="true" ShowUpdateButton="true" ShowClearFilterButton="true" >

                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn FieldName="id" Caption="id" VisibleIndex="1"  ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="name" Caption="Назва" VisibleIndex="2" ReadOnly="false" Visible="true" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="branch_code" Caption="Код галузі" VisibleIndex="3" ReadOnly="false" Visible="true" Width="100px" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування" VisibleIndex="5" ReadOnly="true" Width="200px">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataTextColumn FieldName="modified_by" Caption="Користувач" VisibleIndex="6" ReadOnly="true" Width="200px">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

        </Columns>
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsPager PageSize="20" />
        <SettingsEditing NewItemRowPosition="Bottom" />
        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" VerticalScrollableHeight="0" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard"/>
    </dx:ASPxGridView>

</asp:Content>