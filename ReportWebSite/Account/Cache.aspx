<%@ Page Title="" Language="C#" MasterPageFile="~/NoHeader.master" AutoEventWireup="true" CodeFile="Cache.aspx.cs" Inherits="Account_Cache" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<style type="text/css">
    .spacer
    {
        display: inline-block;
        width: 20px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Information">
    </dx:ASPxLabel>
    <br />
    <br />
    <dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" 
        NavigateUrl="~/Account/Cache.aspx?Reset" Text="Clear Cache"
        ToolTip="Clears the in-memory cache contents. Does not affect the collection of queries eligible for bulk pre-load." />
    <div class="spacer"></div>
    <dx:ASPxHyperLink ID="ASPxHyperLink3" runat="server" 
        NavigateUrl="~/Account/Cache.aspx?ForgetAll" Text="Forget All Queries"
        ToolTip="Resets the collection of queries eligible for bulk pre-load. Does not affect the in-memory cache contents." />
    <div class="spacer"></div>
    <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
        NavigateUrl="~/Account/Cache.aspx?Reload" Text="Reload Cache"
        ToolTip="Clears the in-memory cache contents and then uses the collection of queries eligible for bulk pre-load to populate the cache directly from the database." />
    <br />
    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="" ClientVisible="false" 
        Visible="false" Font-Bold="True" ForeColor="Red">
    </dx:ASPxLabel>
    <br />
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False">
        <Columns>
            <dx:GridViewDataHyperLinkColumn Caption="#" FieldName="UID" VisibleIndex="1">
                <PropertiesHyperLinkEdit NavigateUrlFormatString="~/Account/Cache.aspx?Forget={0}" 
                    Text="Forget">
                </PropertiesHyperLinkEdit>
            </dx:GridViewDataHyperLinkColumn>
            <dx:GridViewDataTextColumn FieldName="UID" VisibleIndex="0" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Query" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Parameters" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="Available?" VisibleIndex="4">
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataDateColumn FieldName="Last Access" VisibleIndex="5">
                <PropertiesDateEdit DisplayFormatString="g">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Geometry" VisibleIndex="6">
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPager Mode="ShowAllRecords">
        </SettingsPager>
    </dx:ASPxGridView>
    
</asp:Content>

