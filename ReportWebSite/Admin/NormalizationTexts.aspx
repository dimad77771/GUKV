<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NormalizationTexts.aspx.cs" Inherits="Admin_NormalizationTexts"
    MasterPageFile="~/NoHeader.master" Title="Нормалізація імен" EnableViewState="true" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server" EnableViewState="true">

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

    <mini:ProfiledSqlDataSource ID="SqlDataSourceNormalizationTexts" runat="server"
        ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
        ProviderName="System.Data.SqlClient"
        SelectCommand="SELECT txt FROM [NormalizationTexts] ORDER BY txt"
        InsertCommand="INSERT INTO [NormalizationTexts] (txt) VALUES (@txt)"
        UpdateCommand="UPDATE [NormalizationTexts] SET txt = @txt WHERE txt = @original_txt"
        DeleteCommand="DELETE FROM [NormalizationTexts] WHERE txt = @original_txt"
        OldValuesParameterFormatString="original_{0}"
        OnInserted="SqlDataSourceNormalizationTexts_Changed"
        OnUpdated="SqlDataSourceNormalizationTexts_Changed"
        OnDeleted="SqlDataSourceNormalizationTexts_Changed">

        <InsertParameters>
            <asp:Parameter Name="txt" Type="String" Size="8000" />
        </InsertParameters>

        <UpdateParameters>
            <asp:Parameter Name="txt" Type="String" Size="8000" />
            <asp:Parameter Name="original_txt" Type="String" Size="8000" />
        </UpdateParameters>

        <DeleteParameters>
            <asp:Parameter Name="original_txt" Type="String" Size="8000" />
        </DeleteParameters>
    </mini:ProfiledSqlDataSource>

    <p style="font-size: 1.4em; margin: 0; padding: 0; border-bottom: 1px solid #D0D0D0;">
        <asp:Label runat="server" ID="PageTitleLabel" Text="Нормалізація імен" CssClass="pagetitle" />
    </p>

    <dx:ASPxGridView ID="ASPxGridViewNormalizationTexts" runat="server"
        AutoGenerateColumns="False"
        DataSourceID="SqlDataSourceNormalizationTexts"
        KeyFieldName="txt"
        Width="100%"
        ClientInstanceName="grid"
        OnRowValidating="ASPxGridViewNormalizationTexts_RowValidating">

        <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" />

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
            <dx:GridViewCommandColumn VisibleIndex="0"
                ButtonType="Image"
                CellStyle-Wrap="False"
                ShowEditButton="true"
                ShowNewButton="true"
                ShowDeleteButton="true"
                ShowCancelButton="true"
                ShowUpdateButton="true"
                ShowClearFilterButton="true">
                <CellStyle Wrap="False" />
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn FieldName="txt"
                Caption="Нормалізований текст"
                VisibleIndex="1"
                ReadOnly="false">
                <PropertiesTextEdit MaxLength="8000" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>
        </Columns>

        <SettingsBehavior ConfirmDelete="True" />
        <SettingsPager PageSize="20" />
        <SettingsEditing NewItemRowPosition="Bottom" />
        <Settings ShowFilterRow="True"
            ShowFilterBar="Auto"
            ShowFilterRowMenu="True"
            VerticalScrollableHeight="0"
            VerticalScrollBarMode="Hidden"
            VerticalScrollBarStyle="Standard" />
    </dx:ASPxGridView>

</asp:Content>
