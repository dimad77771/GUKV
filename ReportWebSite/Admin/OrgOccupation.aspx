<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgOccupation.aspx.cs" Inherits="Admin_OrgOccupation" MasterPageFile="~/NoHeader.master" Title="Сфера діяльності балансоутримувачів" EnableViewState="true" %>

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

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrganizations" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    OnUpdating="SqlDataSourceOrganizations_Updating"
    SelectCommand="select obp.*,o.zkpo_code,o.full_name,
per.name as 'rent_period_name',
occ.name as 'occupation_name'
from org_by_period obp
join organizations o on o.id=obp.org_id
join dict_rent_period per on per.id = obp.period_id
join dict_rent_occupation occ on occ.id = obp.org_occupation_id" 
    UpdateCommand="
update [org_by_period] set 
org_occupation_id = @org_occupation_id
where org_id = @org_id and period_id=@period_id" 
        ProviderName="System.Data.SqlClient">
    <UpdateParameters>
        <asp:Parameter Name="org_id" />
        <asp:Parameter Name="period_id" />
        <asp:Parameter Name="org_occupation_id" />
    </UpdateParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentPeriods" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_rent_period order by id">
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOccupation" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM [dict_rent_occupation] ORDER BY name">
</mini:ProfiledSqlDataSource>
    

<p style="font-size: 1.4em; margin: 0 0 0 0; padding: 0 0 0 0; border-bottom: 1px solid #D0D0D0;">
    <asp:Label runat="server" ID="ASPxLabel19" Text="Сфера діяльності балансоутримувачів за періодами" CssClass="pagetitle"/>
</p>

<dx:ASPxGridView ID="ASPxGridViewOrgOccupation" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceOrganizations" KeyFieldName="period_id;org_id" Width="100%" ClientInstanceName="grid">

        <Templates>
            <EditForm>
                <div style="padding-left:50px;margin:5px">
                    <dx:ASPxLabel Text="Код ЕДРПОУ" runat="server"></dx:ASPxLabel><dx:ASPxTextBox Text='<%# Eval("zkpo_code") %>' runat="server" ReadOnly="true" Width="250px" Enabled="false"></dx:ASPxTextBox>
                    <dx:ASPxLabel Text="Повна назва" runat="server"></dx:ASPxLabel><dx:ASPxTextBox Text='<%# Eval("full_name") %>' runat="server" ReadOnly="true" Width="500px" Enabled="false"></dx:ASPxTextBox>
                    <dx:ASPxLabel Text="Період" runat="server"></dx:ASPxLabel><dx:ASPxTextBox Text='<%# Eval("rent_period_name") %>' runat="server" ReadOnly="true" Width="500px" Enabled="false"></dx:ASPxTextBox>
                    <dx:ASPxLabel Text ="Сфера Діяльності" runat="server"></dx:ASPxLabel>
                    <dx:ASPxComboBox ID="ComboOccupation" runat="server" ValueType="System.Int32" TextField="name" ValueField="id" Width="500px" 
                        IncrementalFilteringMode="StartsWith" DataSourceID="SqlDataSourceOccupation" Value='<%# Eval("org_occupation_id") %>' />
                    <div style="padding-top:5px">
                        <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"  runat="server">
                        </dx:ASPxGridViewTemplateReplacement>
                    </div>
                </div>
            </EditForm>
        </Templates>

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
            <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px"
                ShowInCustomizationForm="True" CellStyle-Wrap="False" 
                    ShowEditButton="true" ShowCancelButton="true" ShowUpdateButton="true" ShowClearFilterButton="true" >
                <CellStyle Wrap="False"></CellStyle>
            </dx:GridViewCommandColumn>

<%--            <dx:GridViewDataTextColumn FieldName="id" Caption="id" VisibleIndex="1"  ReadOnly="true" Visible="false">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>--%>

            <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Код ЕДРПОУ" Width="150px">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Повна назва">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataComboBoxColumn FieldName="period_id" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Visible="True" Caption="Звітній Період">
                <PropertiesComboBox DataSourceID="SqlDataSourceRentPeriods" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

            
<%--            <dx:GridViewDataTextColumn FieldName="rent_period_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Період" Width="300px">
            </dx:GridViewDataTextColumn>--%>

            <dx:GridViewDataComboBoxColumn FieldName="org_occupation_id" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Visible="True" Caption="Сфера Діяльності">
                <PropertiesComboBox DataSourceID="SqlDataSourceOccupation" ValueField="id" TextField="name" ValueType="System.Int32" />
            </dx:GridViewDataComboBoxColumn>

<%--            <dx:GridViewDataTextColumn FieldName="occupation_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Вид Діяльності" Width="300px">
            </dx:GridViewDataTextColumn>--%>

<%--            <dx:GridViewDataDateColumn FieldName="modify_date" Caption="Дата редагування" VisibleIndex="5" ReadOnly="true" Width="150px">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataDateColumn>--%>

<%--            <dx:GridViewDataTextColumn FieldName="modified_by" Caption="Користувач" VisibleIndex="6" ReadOnly="true"  Width="250px">
                <EditFormSettings Visible="False" />
                <HeaderStyle Wrap="True" />
            </dx:GridViewDataTextColumn>--%>

        </Columns>
        <SettingsPager PageSize="20" />
        <SettingsEditing NewItemRowPosition="Bottom" />
        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" VerticalScrollableHeight="0" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard"/>
    </dx:ASPxGridView>

</asp:Content>