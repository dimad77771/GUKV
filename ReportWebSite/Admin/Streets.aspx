<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Streets.aspx.cs" Inherits="Admin_Streets"
    MasterPageFile="~/NoMenu.master" Title="Довідник вулиць" EnableViewState="true" %>

<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxImageGallery" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxImageSlider" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
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
    SelectCommand="SELECT id, name, name_output, kind, stan, activity, renamed_from_doc_id, renamed_to_doc_id, parent_id, modified_by, modify_date FROM [dict_streets]" 
    DeleteCommand="DELETE FROM [dict_streets] WHERE id = @id" 
    InsertCommand="
insert into [dict_streets] (id, name, name_output, kind, stan, activity, renamed_from_doc_id, renamed_to_doc_id, parent_id, modified_by, modify_date)
values (@id, @name, @name_output, @kind, 1, 1, null, null, 0, @modified_by, @modify_date)" 
    UpdateCommand="
update [dict_streets] set 
name = @name,
name_output = @name_output,
kind = @kind,
modified_by = @modified_by, 
modify_date = @modify_date
where id = @id" 
        oninserting="SqlDataSourceFreeSquare_Inserting" 
        onupdating="SqlDataSourceFreeSquare_Updating" ProviderName="System.Data.SqlClient">
    <DeleteParameters>
        <asp:Parameter Name="id" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="id" />
        <asp:Parameter Name="name" />
        <asp:Parameter Name="name_output" />
        <asp:Parameter Name="kind" />
        <asp:Parameter Name="modified_by" />
        <asp:Parameter Name="modify_date" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="name" />
        <asp:Parameter Name="name_output" />
        <asp:Parameter Name="kind" />
        <asp:Parameter Name="modified_by" />
        <asp:Parameter Name="modify_date" />
        <asp:Parameter Name="id" />
    </UpdateParameters>
</mini:ProfiledSqlDataSource>

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Довідник вулиць" CssClass="pagetitle"/>
</p>

<dx:ASPxGridView ID="ASPxGridViewFreeSquare" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceFreeSquare" KeyFieldName="id" Width="100%" 
            OnRowValidating="ASPxGridViewFreeSquare_RowValidating" 
            OnStartRowEditing="ASPxGridViewFreeSquare_StartRowEditing"
            ClientInstanceName="grid">
        <Columns>

            <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" 
                ShowInCustomizationForm="True" CellStyle-Wrap="False">
                <EditButton Visible="True">
                    <Image Url="~/Styles/EditIcon.png" />
                </EditButton>
                <NewButton Visible="True">
                    <Image Url="~/Styles/AddIcon.png" />
                </NewButton>
                <DeleteButton Visible="True">
                    <Image Url="~/Styles/DeleteIcon.png" />
                </DeleteButton>
                <CancelButton>
                    <Image Url="~/Styles/CancelIcon.png" />
                </CancelButton>
                <UpdateButton>
                    <Image Url="~/Styles/EditIcon.png" />
                </UpdateButton>
                <ClearFilterButton Visible="True">
                </ClearFilterButton>

                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn FieldName="id" Caption="id" VisibleIndex="1"  ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="name" Caption="Назва (великими)" VisibleIndex="2" ReadOnly="false" Visible="true" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="name_output" Caption="Назва" VisibleIndex="3" ReadOnly="false" Visible="true" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="kind" Caption="Тип" VisibleIndex="4" ReadOnly="false" Visible="true" >
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування" VisibleIndex="5" ReadOnly="true">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataTextColumn FieldName="modified_by" Caption="Користувач" VisibleIndex="6" ReadOnly="true">
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