<%@ Page Title="ЄІС - Конкурс" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contest.aspx.cs" Inherits="ExtDataEntry.Arenda.Contest" %>
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
            var height = Math.max(0, document.documentElement.clientHeight)-210;
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
        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="Large" Text="Конкурс на право оренди нерухомого майна, що належить до комунальної власності територіальної громади міста Києва">
        </dx:ASPxLabel>
    </div>
    <br />
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSource1" KeyFieldName="Id" Width="100%" 
        ClientInstanceName="grid" 
        onrowupdating="ASPxGridView1_RowUpdating">
        <ClientSideEvents CustomButtonClick="ShowPhoto" Init="OnInit" EndCallback="OnEndCallback"/>
    <Columns>
        <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
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
        </dx:GridViewCommandColumn>
        <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Caption="ID" VisibleIndex="1">
            <Settings AllowAutoFilter="False" />
            <EditFormSettings Visible="False"></EditFormSettings>
        </dx:GridViewDataTextColumn>
        <dx:GridViewBandColumn Caption="Дані про балансоутримувача	" VisibleIndex="2">
            <Columns>
                <dx:GridViewDataTextColumn Caption="Назва" FieldName="BalanceHolderName" 
                    VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="№ телефон" FieldName="BalanceHolderPhone" 
                    VisibleIndex="1">
                </dx:GridViewDataTextColumn>
            </Columns>
        </dx:GridViewBandColumn>
        <dx:GridViewBandColumn Caption="Дані про об’єкт оренди" VisibleIndex="4">
            <Columns>
                <dx:GridViewDataTextColumn Caption="Назва вулиці" FieldName="AddrStreet" 
                    VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="№ будинку, літера" FieldName="AddrNumber" 
                    VisibleIndex="1">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Характеристика" FieldName="Characteristics" 
                    VisibleIndex="2">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Площа, кв.м." FieldName="SqrRent" 
                    VisibleIndex="3">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Поверх" FieldName="RentFloor" 
                    VisibleIndex="4">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataMemoColumn Caption="Мета використання" FieldName="IntendedUse"
                    VisibleIndex="5">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataMemoColumn>
                <dx:GridViewDataTextColumn Caption="Стартовий розмір орендної плати (без ПДВ), грн." 
                    FieldName="RentTotal" VisibleIndex="6">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Розмір авансової орендної плати, грн" 
                    FieldName="RentPrepay" VisibleIndex="7">
                    <HeaderStyle Wrap="True" />
                </dx:GridViewDataTextColumn>
            </Columns>
        </dx:GridViewBandColumn>
        <dx:GridViewDataTextColumn FieldName="BalanceHolderBankInfo" VisibleIndex="12" 
            
            Caption="Реквізити (балансоутримувача) для сплати авансової орендної плати">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn Caption="Дата проведення конкурсу" 
            FieldName="ContestBeginsOn" 
            VisibleIndex="13">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn Caption="Кінцева дата подання документів" 
            FieldName="ContestEndsOn" VisibleIndex="14">
            <HeaderStyle Wrap="True" />
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="GeocodingId" Visible="False" 
            VisibleIndex="15">
        </dx:GridViewDataTextColumn>
    </Columns>
    <SettingsPager PageSize="5" />
    <SettingsBehavior ConfirmDelete="True" />
    <SettingsEditing NewItemRowPosition="Bottom" />
    <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" VerticalScrollableHeight="0" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
</dx:ASPxGridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:SCPConnectionString %>" 
    
    SelectCommand="SELECT Id, BalanceHolderName, BalanceHolderPhone, AddrStreet, AddrNumber, Characteristics, SqrRent, RentFloor, IntendedUse, RentTotal, RentPrepay, BalanceHolderBankInfo, ContestBeginsOn, ContestEndsOn, GeocodingId, LastModifiedOn, LastModifiedBy FROM ExtRentContest" 
    DeleteCommand="DELETE FROM ExtRentContest WHERE Id=@Id" InsertCommand="INSERT INTO ExtRentContest
    (BalanceHolderName, BalanceHolderPhone, AddrStreet, AddrNumber, Characteristics, SqrRent, RentFloor, IntendedUse, RentTotal, RentPrepay, BalanceHolderBankInfo, ContestBeginsOn, ContestEndsOn, GeocodingId, LastModifiedOn, LastModifiedBy)
VALUES
    (@BalanceHolderName, @BalanceHolderPhone, @AddrStreet, @AddrNumber, @Characteristics, @SqrRent, @RentFloor, @IntendedUse, @RentTotal, @RentPrepay, @BalanceHolderBankInfo, @ContestBeginsOn, @ContestEndsOn, @GeocodingId, @LastModifiedOn, @LastModifiedBy);
SELECT SCOPE_IDENTITY()" UpdateCommand="UPDATE ExtRentContest
SET
    BalanceHolderName=@BalanceHolderName,
    BalanceHolderPhone=@BalanceHolderPhone,
    AddrStreet=@AddrStreet,
    AddrNumber=@AddrNumber,
    Characteristics=@Characteristics,
    SqrRent=@SqrRent,
    RentFloor=@RentFloor,
    IntendedUse=@IntendedUse,
    RentTotal=@RentTotal,
    RentPrepay=@RentPrepay,
    BalanceHolderBankInfo=@BalanceHolderBankInfo,
    ContestBeginsOn=@ContestBeginsOn,
    ContestEndsOn=@ContestEndsOn,
    GeocodingId=@GeocodingId,
    LastModifiedOn=@LastModifiedOn,
    LastModifiedBy=@LastModifiedBy
WHERE
    Id=@Id" 
        oninserting="SqlDataSource1_Inserting" 
    onupdating="SqlDataSource1_Updating">
    <DeleteParameters>
        <asp:Parameter Name="Id" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="BalanceHolderName"  />
        <asp:Parameter Name="BalanceHolderPhone" />
        <asp:Parameter Name="AddrStreet" />
        <asp:Parameter Name="AddrNumber" />
        <asp:Parameter Name="Characteristics" />
        <asp:Parameter Name="SqrRent" />
        <asp:Parameter Name="RentFloor" />
        <asp:Parameter Name="IntendedUse" />
        <asp:Parameter Name="RentTotal" />
        <asp:Parameter Name="RentPrepay" />
        <asp:Parameter Name="BalanceHolderBankInfo" />
        <asp:Parameter Name="ContestBeginsOn" />
        <asp:Parameter Name="ContestEndsOn" />
        <asp:Parameter Name="GeocodingId" />
        <asp:Parameter Name="LastModifiedOn" />
        <asp:Parameter Name="LastModifiedBy" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="BalanceHolderName" />
        <asp:Parameter Name="BalanceHolderPhone" />
        <asp:Parameter Name="AddrStreet" />
        <asp:Parameter Name="AddrNumber" />
        <asp:Parameter Name="Characteristics" />
        <asp:Parameter Name="SqrRent" />
        <asp:Parameter Name="RentFloor" />
        <asp:Parameter Name="IntendedUse" />
        <asp:Parameter Name="RentTotal" />
        <asp:Parameter Name="RentPrepay" />
        <asp:Parameter Name="BalanceHolderBankInfo" />
        <asp:Parameter Name="ContestBeginsOn" />
        <asp:Parameter Name="ContestEndsOn" />
        <asp:Parameter Name="GeocodingId" />
        <asp:Parameter Name="LastModifiedOn" />
        <asp:Parameter Name="LastModifiedBy" />
        <asp:Parameter Name="Id" />
    </UpdateParameters>
</asp:SqlDataSource>
<dx:ASPxGlobalEvents ID="ge" runat="server">
    <ClientSideEvents ControlsInitialized="OnControlsInitialized" />
</dx:ASPxGlobalEvents>
</asp:Content>
