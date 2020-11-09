<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageRoles.aspx.cs" Inherits="Account_ManageRoles"
    MasterPageFile="~/Site.master" Title="Керування Правами Доступу" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Керування правами доступу
    </h2>

    <p></p>

    <table border="0" cellspacing="0" cellpadding="0">
    <tr>

    <td valign="top">
    <dx:ASPxRoundPanel ID="PanelCreateRole" runat="server" HeaderText="Створення Ролей">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent4" runat="server">

                <dx:ASPxGridView ID="GridViewRoles" runat="server" AutoGenerateColumns="False" Width="350px"
                    KeyFieldName="RoleName"
                    OnRowDeleting="GridViewRoles_RowDeleting">

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
                        <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="RoleName" ReadOnly="True" Caption="Роль">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewCommandColumn VisibleIndex="1" ShowDeleteButton="True"/>
                    </Columns>

                    <SettingsBehavior EnableCustomizationWindow="False" ColumnResizeMode="Control" />
                    <SettingsPager PageSize="10"></SettingsPager>
                    <Settings
                        ShowFilterRow="False"
                        ShowFilterRowMenu="False"
                        ShowGroupPanel="False" 
                        ShowFilterBar="Hidden"
                        ShowHeaderFilterButton="False" 
                        HorizontalScrollBarMode="Hidden"
                        ShowFooter="False" />
                </dx:ASPxGridView>
           
                <br/>

                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Назва Нової Ролі"></dx:ASPxLabel>

                <dx:ASPxTextBox ID="EditNewRoleName" runat="server" Width="350px"></dx:ASPxTextBox>

                <br/>

                <dx:ASPxButton ID="ButtonAddRole" Width="200px"
                    runat="server" Text="Створити Нову Роль" OnClick="ButtonAddRole_Click"></dx:ASPxButton>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    </td>

    <td width="16px">&nbsp;&nbsp;</td>

    <td valign="top">
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Ролі Користувача">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">

                <dx:ASPxComboBox ID="ComboUsers" runat="server" Width="350px" AutoPostBack="True"
                    OnSelectedIndexChanged="ComboUsers_SelectedIndexChanged"></dx:ASPxComboBox>

                <br/>

                <dx:ASPxGridView ID="GridViewUserRoles" runat="server" AutoGenerateColumns="False" Width="350px"
                    KeyFieldName="RoleName"
                    OnRowDeleting="GridViewUserRoles_RowDeleting">

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
                        <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="RoleName" ReadOnly="True" Caption="Роль Користувача">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewCommandColumn VisibleIndex="1" ShowDeleteButton="True"/>
                    </Columns>

                    <SettingsBehavior EnableCustomizationWindow="False" ColumnResizeMode="Control" />
                    <SettingsPager PageSize="10"></SettingsPager>
                    <Settings
                        ShowFilterRow="False"
                        ShowFilterRowMenu="False"
                        ShowGroupPanel="False" 
                        ShowFilterBar="Hidden"
                        ShowHeaderFilterButton="False" 
                        HorizontalScrollBarMode="Hidden"
                        ShowFooter="False" />
                </dx:ASPxGridView>

                <br/>

                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Можливі Ролі Користувача"></dx:ASPxLabel>

                <dx:ASPxComboBox ID="ComboRolesToAdd" runat="server" Width="350px"></dx:ASPxComboBox>

                <br/>

                <dx:ASPxButton ID="ButtonAssignRole" Width="200px"
                    runat="server" Text="Надати Роль Користувачу" OnClick="ButtonAssignRole_Click"></dx:ASPxButton>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    </td>

    </tr>
    </table>

</asp:Content>
