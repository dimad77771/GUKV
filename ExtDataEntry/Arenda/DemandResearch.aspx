<%@ Page Title="ЄІС - вивчення попиту" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DemandResearch.aspx.cs" Inherits="ExtDataEntry.Arenda.DemandResearch" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxFileManager" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHiddenField" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallbackPanel" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGlobalEvents" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function OnInit(s, e) {
            AdjustSize();
        }
        function OnEndCallback(s, e) {
            AdjustSize();
        }
        function OnControlsInitialized(s, e) {
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
            });
        }
        function AdjustSize() {
            var height = Math.max(0, document.documentElement.clientHeight)-260;
            grid.SetHeight(height);
        }
        function ShowPhoto(s, e) {
            if (e.buttonID == 'btnPhoto') {
                $.cookie('RecordID', s.GetRowKey(e.visibleIndex));
                ASPxFileManager1.Refresh();
                PopupObjectPhotos.Show();
            }
        }
    </script>
    <br />
    <div style="text-align: center;">
        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="Large" Text="Оголошення про намір передати в оренду об’єкти, що належать до комунальної власності (вивчення попиту)">
        </dx:ASPxLabel>
        <hr />
        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="Small" Text="ОРЕНДОДАВЕЦЬ — ДЕПАРТАМЕНТ КОМУНАЛЬНОЇ ВЛАСНОСТІ М. КИЄВА ВИКОНАВЧОГО ОРГАНУ КИЇВСЬКОЇ МІСЬКОЇ РАДИ (КИЇВСЬКОЇ МІСЬКОЇ ДЕРЖАВНОЇ АДМІНІСТРАЦІЇ)">
        </dx:ASPxLabel>
    </div>
    <br />
    <br />
    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" AllowDragging="True" 
        ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True" 
        HeaderText="Фотографії об'єкту оренди" Modal="True" PopupHorizontalAlign="Center" 
        PopupVerticalAlign="Middle" RenderMode="Lightweight" Width="700px" 
    PopupAction="None" PopupElementID="ASPxGridView1">
        <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        DeleteMethod="Delete" InsertMethod="Insert" 
        OnDeleting="ObjectDataSource1_Deleting" 
        OnInserting="ObjectDataSource1_Inserting" 
        OnSelecting="ObjectDataSource1_Selecting" SelectMethod="Select" 
        TypeName="ExtDataEntry.Models.FileAttachment">
        <DeleteParameters>
            <asp:Parameter DefaultValue="ExtRentDemandResearch" Name="scope" 
                Type="String" />
            <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" 
                Type="Int32" />
            <asp:Parameter Name="id" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter DefaultValue="ExtRentDemandResearch" Name="scope" 
                Type="String" />
            <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" 
                Type="Int32" />
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="Image" Type="Object" />
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter DefaultValue="ExtRentDemandResearch" Name="scope" 
                Type="String" />
            <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" 
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dx:ASPxFileManager ID="ASPxFileManager1" runat="server" 
        ClientInstanceName="ASPxFileManager1" DataSourceID="ObjectDataSource1">
        <Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
        <SettingsFileList>
            <ThumbnailsViewSettings ThumbnailSize="180px" />
        </SettingsFileList>
        <SettingsEditing AllowDelete="True" />
        <SettingsFolders Visible="False" />
        <SettingsToolbar ShowDownloadButton="True" ShowPath="False" />
        <SettingsUpload UseAdvancedUploadMode="True">
            <AdvancedModeSettings EnableMultiSelect="True" />
        </SettingsUpload>
        <SettingsDataSource FileBinaryContentFieldName="Image" 
            IsFolderFieldName="IsFolder" KeyFieldName="ID" 
            LastWriteTimeFieldName="LastModified" NameFieldName="Name" 
            ParentKeyFieldName="ParentID" />
    </dx:ASPxFileManager>
    <br />
    <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
        Text="Закрити" HorizontalAlign="Center">
        <ClientSideEvents Click="function(s, e) {
	PopupObjectPhotos.Hide();
}" />
    </dx:ASPxButton>
            </dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSource1" KeyFieldName="Id" Width="100%" 
        ClientInstanceName="grid" oninitnewrow="ASPxGridView1_InitNewRow" 
        onrowupdating="ASPxGridView1_RowUpdating">
        <ClientSideEvents CustomButtonClick="ShowPhoto" Init="OnInit" EndCallback="OnEndCallback" />
    <Columns>
        <dx:GridViewBandColumn Caption="Дані про балансоутримувача" VisibleIndex="3" 
            ShowInCustomizationForm="True">
            <Columns>
                <dx:GridViewDataTextColumn Caption="Назва" FieldName="BalanceHolderName" 
                    VisibleIndex="0" ShowInCustomizationForm="True">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="№ телефон" FieldName="BalanceHolderPhone" 
                    VisibleIndex="1" ShowInCustomizationForm="True">
                </dx:GridViewDataTextColumn>
            </Columns>
        </dx:GridViewBandColumn>
        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" 
            ShowInCustomizationForm="True">
            <EditButton Visible="True">
                <Image Url="~/Styles/EditIcon.png">
                </Image>
            </EditButton>
            <NewButton Visible="True">
                <Image Url="~/Styles/AddIcon.png">
                </Image>
            </NewButton>
            <DeleteButton Visible="True">
                <Image Url="~/Styles/DeleteIcon.png">
                </Image>
            </DeleteButton>
            <CancelButton>
                <Image Url="~/Styles/CancelIcon.png">
                </Image>
            </CancelButton>
            <UpdateButton>
                <Image Url="~/Styles/EditIcon.png">
                </Image>
            </UpdateButton>
            <ClearFilterButton Visible="True">
            </ClearFilterButton>
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="btnPhoto">
                    <Image Url="~/Styles/PhotoIcon.png">
                    </Image>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
        </dx:GridViewCommandColumn>
        <dx:GridViewDataDateColumn Caption="Дата закінчення терміну" 
            FieldName="ExpirationDate" SortIndex="0" SortOrder="Descending" 
            VisibleIndex="2">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="1" 
            Caption="ID" ShowInCustomizationForm="True">
            <Settings AllowAutoFilter="False" AllowHeaderFilter="False" 
                ShowInFilterControl="False" />
            <EditFormSettings Visible="False" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewBandColumn Caption="Дані про об’єкт оренди" VisibleIndex="5" 
            ShowInCustomizationForm="True">
            <Columns>
                <dx:GridViewDataTextColumn Caption="Характеристика" FieldName="Characteristics" 
                    VisibleIndex="0" ShowInCustomizationForm="True">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Назва вулиці" FieldName="AddrStreet" 
                    VisibleIndex="1" ShowInCustomizationForm="True">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="№ будинку, літера" FieldName="AddrNumber" 
                    VisibleIndex="2" ShowInCustomizationForm="True">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Загальна площа (кв.м)" FieldName="SqrTotal" 
                    VisibleIndex="3" ShowInCustomizationForm="True">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Вартість (грн)" FieldName="Worth" 
                    VisibleIndex="4" ShowInCustomizationForm="True">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataMemoColumn Caption="Запропонована заявником мета використання приміщення" 
                    FieldName="IntendedUse" VisibleIndex="5" ShowInCustomizationForm="True">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataMemoColumn>
                <dx:GridViewDataTextColumn Caption="Строк оренди запропонований заявником" 
                    FieldName="Duration" VisibleIndex="6" ShowInCustomizationForm="True">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Орендна ставка (грн)" FieldName="RentRate" 
                    VisibleIndex="7" ShowInCustomizationForm="True">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Орендна плата за 1 кв.м (грн)" 
                    FieldName="RentPerSqrM" VisibleIndex="8" ShowInCustomizationForm="True">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Розмір місячної орендної плати (грн)" 
                    FieldName="RentTotal" VisibleIndex="9" ShowInCustomizationForm="True">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
            </Columns>
        </dx:GridViewBandColumn>
    </Columns>
    <SettingsBehavior ConfirmDelete="True" />
    <SettingsPager PageSize="5" />
    <SettingsEditing NewItemRowPosition="Bottom" />
    <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" VerticalScrollableHeight="0" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard"/>
</dx:ASPxGridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:SCPConnectionString %>" 
    
    SelectCommand="SELECT Id, BalanceHolderName, BalanceHolderPhone, Characteristics, AddrStreet, AddrNumber, SqrTotal, Worth, IntendedUse, Duration, RentRate, RentPerSqrM, RentTotal, ExpirationDate, GeocodingId FROM ExtRentDemandResearch" 
    DeleteCommand="DELETE FROM ExtRentDemandResearch WHERE Id=@Id" InsertCommand="INSERT INTO ExtRentDemandResearch
    (BalanceHolderName, BalanceHolderPhone, Characteristics, AddrStreet, AddrNumber, SqrTotal, Worth, IntendedUse, Duration, RentRate, RentPerSqrM, RentTotal, ExpirationDate, LastModifiedOn, LastModifiedBy, GeocodingId)
VALUES
    (@BalanceHolderName, @BalanceHolderPhone, @Characteristics, @AddrStreet, @AddrNumber, @SqrTotal, @Worth, @IntendedUse, @Duration, @RentRate, @RentPerSqrM, @RentTotal, @ExpirationDate, @LastModifiedOn, @LastModifiedBy, @GeocodingId);
SELECT SCOPE_IDENTITY()" UpdateCommand="UPDATE ExtRentDemandResearch
SET
    BalanceHolderName=@BalanceHolderName,
    BalanceHolderPhone=@BalanceHolderPhone, 
    Characteristics=@Characteristics, 
    AddrStreet=@AddrStreet,
    AddrNumber=@AddrNumber,
    SqrTotal=@SqrTotal,
    Worth=@Worth,
    IntendedUse=@IntendedUse,
    Duration=@Duration,
    RentRate=@RentRate,
    RentPerSqrM=@RentPerSqrM,
    RentTotal=@RentTotal,
    ExpirationDate=@ExpirationDate,
    LastModifiedOn=@LastModifiedOn,
    LastModifiedBy=@LastModifiedBy,
    GeocodingId=@GeocodingId
WHERE
    Id=@Id" ondeleted="SqlDataSource1_Deleted" 
        oninserting="SqlDataSource1_Inserting" onupdating="SqlDataSource1_Updating">
    <DeleteParameters>
        <asp:Parameter Name="Id" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="BalanceHolderName" />
        <asp:Parameter Name="BalanceHolderPhone" />
        <asp:Parameter Name="Characteristics" />
        <asp:Parameter Name="AddrStreet" />
        <asp:Parameter Name="AddrNumber" />
        <asp:Parameter Name="SqrTotal" />
        <asp:Parameter Name="Worth" />
        <asp:Parameter Name="IntendedUse" />
        <asp:Parameter Name="Duration" />
        <asp:Parameter Name="RentRate" />
        <asp:Parameter Name="RentPerSqrM" />
        <asp:Parameter Name="RentTotal" />
        <asp:Parameter Name="ExpirationDate" />
        <asp:Parameter Name="LastModifiedOn" />
        <asp:Parameter Name="LastModifiedBy" />
        <asp:Parameter Name="GeocodingId" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="BalanceHolderName" />
        <asp:Parameter Name="BalanceHolderPhone" />
        <asp:Parameter Name="Characteristics" />
        <asp:Parameter Name="AddrStreet" />
        <asp:Parameter Name="AddrNumber" />
        <asp:Parameter Name="SqrTotal" />
        <asp:Parameter Name="Worth" />
        <asp:Parameter Name="IntendedUse" />
        <asp:Parameter Name="Duration" />
        <asp:Parameter Name="RentRate" />
        <asp:Parameter Name="RentPerSqrM" />
        <asp:Parameter Name="RentTotal" />
        <asp:Parameter Name="ExpirationDate" />
        <asp:Parameter Name="LastModifiedOn" />
        <asp:Parameter Name="LastModifiedBy" />
        <asp:Parameter Name="GeocodingId" />
        <asp:Parameter Name="Id" />
    </UpdateParameters>
</asp:SqlDataSource>
</asp:Content>
