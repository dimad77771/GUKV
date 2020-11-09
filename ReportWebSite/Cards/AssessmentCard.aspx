<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssessmentCard.aspx.cs" Inherits="Cards_AssessmentCard"
    MasterPageFile="~/NoMenu.master" Title="Картка Об'єкту Оцінки" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT * FROM view_assessment WHERE valuation_id = @vid" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentInputDoc" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT indoc.id AS 'doc_id', indoc.doc_date, indoc.doc_num, indoc.control_date, korr.name AS 'korr', indoc.rezenz_name
        FROM expert_input_doc indoc LEFT OUTER JOIN dict_expert_korr korr ON korr.id = indoc.korrespondent_id
        WHERE indoc.expert_note_id = @vid AND indoc.is_deleted <> 1" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentOutputDoc" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT outdoc.id AS 'doc_id', outdoc.doc_date, outdoc.doc_num, rt.name AS 'rezenz_type'
        FROM expert_output_doc outdoc LEFT OUTER JOIN dict_expert_rezenz_type rt ON rt.id = outdoc.rezenz_type_id
        WHERE outdoc.expert_note_id = @vid AND outdoc.is_deleted <> 1" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceAssessmentDetails" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT d.id AS 'detail_id', d.obj_square, d.cost_1_usd, d.valuation_date, d.floors, d.purpose, d.note
        FROM expert_note_detail d WHERE d.expert_note_id = @vid AND d.is_deleted <> 1" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="vid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="0" cellpadding="0">
<tr>
<td>

<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" ActiveTabIndex="0">
    <TabPages>

        <dx:TabPage Text="Відомості про об'єкт" Name="AssessmentCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">

<asp:FormView runat="server" BorderStyle="None" ID="ObjDetails" DataSourceID="SqlDataSourceAssessmentProperties" EnableViewState="False">
    <ItemTemplate>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Адреса Будинку">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Район"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditOrgId" runat="server" ReadOnly="True" Text='<%# Eval("district") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Вид Об'єкту"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox23" runat="server" ReadOnly="True" Text='<%# Eval("expert_obj_type") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Назва Вулиці"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ReadOnly="True" Text='<%# Eval("street_full_name") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Номер Будинку"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox22" runat="server" ReadOnly="True" Text='<%# Eval("addr_nomer") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <br/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Організації">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Балансоутримувач"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("balans_org_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Орендар"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ReadOnly="True" Text='<%# Eval("renter_name") %>' Width="700px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <br/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="Оцінка">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="СОД"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox5" runat="server" ReadOnly="True" Text='<%# Eval("expert_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Рецензент"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox6" runat="server" ReadOnly="True" Text='<%# Eval("rezenz_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Вид оцінки"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox7" runat="server" ReadOnly="True" Text='<%# Eval("valuation_kind") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Дата Проведення Оцінки"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" ReadOnly="True" Value='<%# Eval("valuation_date") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Площа Об'єкту"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox8" runat="server" ReadOnly="True" Text='<%# Eval("obj_square") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Вартість Об'єкту (грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox9" runat="server" ReadOnly="True" Text='<%# Eval("cost_prim") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

        <br/>

        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Архівний Стан">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td colspan="7" align="left">
                                <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" ReadOnly="True" Checked='<%# (1.Equals(Eval("is_archived"))) ? true : false %>' Text="Переміщено в Архів" />
                            </td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Архівний Номер"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Text='<%# Eval("arch_num") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Дата Затвердження"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" ReadOnly="True" Value='<%# Eval("final_date") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>

    </ItemTemplate>
</asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Вхідна Інформація" Name="AssessmentCardInputDocs">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">

                    <dx:ASPxGridView ID="GridViewAssessmentInputDocs" runat="server" 
                        ClientInstanceName="GridViewAssessmentInputDocs"
                        AutoGenerateColumns="False"
                        DataSourceID="SqlDataSourceAssessmentInputDoc" 
                        KeyFieldName="doc_id"
                        Width="840px" >

                        <GroupSummary>
                            <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                        </GroupSummary>

                        <Columns>
                            <dx:GridViewDataDateColumn FieldName="doc_date" VisibleIndex="0" Caption="Дата Документа"></dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="doc_num" VisibleIndex="1" Caption="Номер Документа"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="korr" VisibleIndex="2" Caption="Кореспондент"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="control_date" VisibleIndex="3" Caption="Контрольна Дата"></dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn FieldName="rezenz_name" VisibleIndex="4" Caption="Рецензент"></dx:GridViewDataDateColumn>
                        </Columns>

                        <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
                        <SettingsPager PageSize="15">
                        </SettingsPager>
                        <Settings
                            VerticalScrollBarMode="Hidden"
                            VerticalScrollBarStyle="Standard"
                            HorizontalScrollBarMode="Visible"
                            ShowFooter="True" />
                        <SettingsCookies CookiesID="GUKV.AssessmentCard.Input" Version="A2" Enabled="False" />
                        <Styles Header-Wrap="True" >
                            <Header Wrap="True"></Header>
                        </Styles>
                    </dx:ASPxGridView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Характеристика Об'єкта" Name="AssessmentCardDetails">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">

                    <dx:ASPxGridView ID="GridViewAssessmentDetails" runat="server" 
                        ClientInstanceName="GridViewAssessmentDetails"
                        AutoGenerateColumns="False"
                        DataSourceID="SqlDataSourceAssessmentDetails" 
                        KeyFieldName="detail_id"
                        Width="840px" >

                        <GroupSummary>
                            <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                        </GroupSummary>

                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="floors" VisibleIndex="0" Caption="Поверх"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="obj_square" VisibleIndex="1" Caption="Площа"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="2" Caption="НЕВ"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="cost_1_usd" VisibleIndex="3" Caption="Вартість 1 кв.м., $" Visible="false"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="valuation_date" VisibleIndex="4" Caption="Дата Оцінки"></dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="note" VisibleIndex="5" Caption="Примітка"></dx:GridViewDataTextColumn>
                        </Columns>

                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="obj_square" SummaryType="Sum" DisplayFormat="{0}" />
                            <dx:ASPxSummaryItem FieldName="cost_1_usd" SummaryType="Sum" DisplayFormat="{0}" />
                        </TotalSummary>

                        <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
                        <SettingsPager PageSize="15">
                        </SettingsPager>
                        <Settings
                            VerticalScrollBarMode="Hidden"
                            VerticalScrollBarStyle="Standard"
                            HorizontalScrollBarMode="Visible"
                            ShowFooter="True" />
                        <SettingsCookies CookiesID="GUKV.AssessmentCard.Output" Version="A2_1" Enabled="False" />
                        <Styles Header-Wrap="True" >
                            <Header Wrap="True"></Header>
                        </Styles>
                    </dx:ASPxGridView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Вихідна Інформація" Name="AssessmentCardOutputDocs">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">

                    <dx:ASPxGridView ID="GridViewAssessmentOutputDocs" runat="server" 
                        ClientInstanceName="GridViewAssessmentOutputDocs"
                        AutoGenerateColumns="False"
                        DataSourceID="SqlDataSourceAssessmentOutputDoc" 
                        KeyFieldName="doc_id"
                        Width="840px" >

                        <GroupSummary>
                            <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
                        </GroupSummary>

                        <Columns>
                            <dx:GridViewDataDateColumn FieldName="doc_date" VisibleIndex="0" Caption="Дата Документа"></dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="doc_num" VisibleIndex="1" Caption="Номер Документа"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="rezenz_type" VisibleIndex="2" Caption="Вид Рецензії"></dx:GridViewDataTextColumn>
                        </Columns>

                        <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
                        <SettingsPager PageSize="15">
                        </SettingsPager>
                        <Settings
                            VerticalScrollBarMode="Hidden"
                            VerticalScrollBarStyle="Standard"
                            HorizontalScrollBarMode="Visible"
                            ShowFooter="True" />
                        <SettingsCookies CookiesID="GUKV.AssessmentCard.Output" Version="A2" Enabled="False" />
                        <Styles Header-Wrap="True" >
                            <Header Wrap="True"></Header>
                        </Styles>
                    </dx:ASPxGridView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Примітка" Name="AssessmentCardNote">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">

                    <asp:FormView runat="server" BorderStyle="None" ID="FormView1" DataSourceID="SqlDataSourceAssessmentProperties" EnableViewState="False">
                        <ItemTemplate>
                            <dx:ASPxMemo ID="AssessmentMemo" runat="server" ClientInstanceName="AssessmentMemo" Width="840px" Text='<%# Eval("note_text") %>' Height="200px" ></dx:ASPxMemo>
                        </ItemTemplate>
                    </asp:FormView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

    </TabPages>
</dx:ASPxPageControl>

</td>
</tr>
</table>

</asp:Content>
