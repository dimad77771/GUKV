<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReportCommentViewer.ascx.cs" Inherits="Reports1NF_ReportCommentViewer" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceExistingComments" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, comment, comment_date, comment_user, control_title FROM reports1nf_comments
        WHERE report_id = @rep_id AND
        (organization_id = @org_id OR @org_id = 0) AND
        (balans_id = @bal_id OR @bal_id = 0) AND
        (balans_deleted_id = @bal_del_id OR @bal_del_id = 0) AND
        (arenda_id = @rent_id OR @rent_id = 0) AND
        (arenda_rented_id = @rent_obj_id OR @rent_obj_id = 0)
        ORDER BY comment_date DESC"
    OnSelecting="SqlDataSourceExistingComments_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rep_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bal_del_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rent_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="rent_obj_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<dx:ASPxCallbackPanel ID="CPCommentPanel" ClientInstanceName="CPCommentPanel" runat="server" OnCallback="CPCommentPanel_Callback">
    <PanelCollection>
        <dx:panelcontent ID="Panelcontent5" runat="server">

            <dx:ASPxGridView
                ID="GridViewComments"
                ClientInstanceName="GridViewComments"
                runat="server"
                AutoGenerateColumns="False"
                Width="550px"
                DataSourceID="SqlDataSourceExistingComments"
                KeyFieldName="id" >

                <Columns>
                    <dx:GridViewDataTextColumn FieldName="control_title" ReadOnly="True" VisibleIndex="0" Caption="Інформація, що коментується" Width="120px" />
                    <dx:GridViewDataTextColumn FieldName="comment" ReadOnly="True" VisibleIndex="1" Caption="Коментар" Width="230px" />
                    <dx:GridViewDataTextColumn FieldName="comment_user" ReadOnly="True" VisibleIndex="2" Caption="Хто коментує" Width="100px" />
                    <dx:GridViewDataDateColumn FieldName="comment_date" ReadOnly="True" VisibleIndex="3" Caption="Коли коментує" Width="100px" />
                </Columns>

                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="control_title" SummaryType="Count" DisplayFormat="{0} коментарів" />
                </TotalSummary>

                <SettingsBehavior EnableCustomizationWindow="False" AutoFilterRowInputDelay="2000" ColumnResizeMode="Control" />
                <SettingsPager Mode="ShowPager" PageSize="5"></SettingsPager>
                <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
                <Settings
                    ShowFilterRow="False"
                    ShowFilterRowMenu="False"
                    ShowGroupPanel="False"
                    ShowFilterBar="Auto"
                    ShowFooter="True"
                    VerticalScrollBarMode="Hidden"
                    VerticalScrollBarStyle="Standard" />
                <SettingsCookies CookiesID="GUKV.Reports1NF.Comments" Version="1" Enabled="False" />
                <Styles Header-Wrap="True" >
                    <Header Wrap="True"></Header>
                </Styles>
            </dx:ASPxGridView>

            <br/>

            <dx:ASPxLabel ID="LabelCommentTarget" runat="server" Text="Інформація, що коментується" />
            <dx:ASPxTextBox ID="EditTarget" runat="server" Width="550px" />

            <p style="font-size: 4px;"/>

            <dx:ASPxCheckBox ID="CheckWrongData" runat="server" Text="Інформація, що міститься в цьому полі, є некоректною" Width="550px" />

            <p style="font-size: 4px;"/>

            <dx:ASPxMemo ID="MemoComment" runat="server" Width="550px" Height="60px" />

            <p style="font-size: 4px;"/>

            <dx:ASPxButton runat="server" ID="ButtonAddComment" Text="Додати коментар" AutoPostBack="false" CausesValidation="false">
                <ClientSideEvents Click="function (s,e) { CPCommentPanel.PerformCallback('' + lastFocusedControlId + ';' + lastFocusedControlTitle); }" />
            </dx:ASPxButton>

        </dx:panelcontent>
    </PanelCollection>
</dx:ASPxCallbackPanel>