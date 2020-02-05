<%@ page language="C#" autoeventwireup="true" inherits="Cards_DocCard, App_Web_doccard.aspx.f411966a" masterpagefile="~/NoMenu.master" title="Картка Документу" validaterequest="false" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" Width="100%">
    <TabPages>
        <dx:TabPage Text="Реквізити документу" Name="DocCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the first tab BEGIN --%>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocCardProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 id, kind, general_kind, doc_date, doc_num, topic, note, receive_date, commission, [source],
                   [state], summa, summa_zalishkova, is_text_exists FROM view_documents WHERE id = @doc_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="doc_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<asp:FormView runat="server" BorderStyle="None" ID="DocDetails" DataSourceID="SqlDataSourceDocCardProperties" EnableViewState="False" Width="100%">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Реквізити документу">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Номер Документу"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditOrgId" runat="server" ReadOnly="True" Text='<%# Eval("doc_num") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Дата Документу"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" ReadOnly="True" Value='<%# Eval("doc_date") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Назва Документу"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("topic") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Вид Документу"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ReadOnly="True" Text='<%# Eval("kind") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Узагальнення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox22" runat="server" ReadOnly="True" Text='<%# Eval("general_kind") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Додаткові відомості">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Дата Отримання"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Text='<%# Eval("receive_date") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Джерело Походження"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox5" runat="server" ReadOnly="True" Text='<%# Eval("source") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Статус"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox6" runat="server" ReadOnly="True" Text='<%# Eval("state") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Комісія, що розглядала документ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox7" runat="server" ReadOnly="True" Text='<%# Eval("commission") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Сума за документом"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox8" runat="server" ReadOnly="True" Text='<%# Eval("summa") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Залишкова сума"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox9" runat="server" ReadOnly="True" Text='<%# Eval("summa_zalishkova") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Примітка"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox10" runat="server" ReadOnly="True" Text='<%# Eval("note") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td colspan="7"><dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" ReadOnly="True"
                                Checked='<%# (1.Equals(Eval("is_text_exists"))) ? true : false %>' Text="Текст Існує" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </ItemTemplate>
</asp:FormView>

<%-- Content of the first tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Підпорядковані документи" Name="DocCardChildDocs">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server">

<%-- Content of the second tab BEGIN --%>

<dx:ASPxGridView ID="GridViewDocCardChildDocs" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceDocCardChildDocs" KeyFieldName="link_id" Width="100%">

    <Columns>
        <dx:GridViewDataDateColumn FieldName="child_date" VisibleIndex="0" Caption="Дата Підпорядкованого Документу" Width="120px">
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="child_num" VisibleIndex="1" Caption="Номер Підпорядкованого Документу" Width="120px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("slave_doc_id") + ")\">" + Eval("child_num") + "</a>"%>
            </DataItemTemplate> 
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="child_topic" VisibleIndex="2" Caption="Назва Підпорядкованого Документу" Width="570px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("slave_doc_id") + ")\">" + Eval("child_topic") + "</a>"%>
            </DataItemTemplate> 
        </dx:GridViewDataTextColumn>
    </Columns>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" 
        EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" />
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.DocCard.ChildDocs" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocCardChildDocs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT link_id, slave_doc_id, child_num, child_date, child_topic, child_kind FROM view_doc_links WHERE master_doc_id = @m_doc_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="m_doc_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<%-- Content of the second tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Об'єкти за документом" Name="DocCardObjects">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the third tab BEGIN --%>

<dx:ASPxGridView ID="GridViewDocCardObjects" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceDocCardObjects" KeyFieldName="link_id" Width="100%">

    <Columns>
        <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="0" Caption="Назва Вулиці" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="1" Caption="Номер Будинку" Width="100px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_name" VisibleIndex="2" Caption="Назва Об'єкту" Width="160px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_group" VisibleIndex="3" Caption="Група Призначення" Width="160px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="4" Caption="Призначення" Width="160px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_obj" VisibleIndex="5" Caption="Площа" Width="100px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_balans" VisibleIndex="6" Caption="Балансова Вартість" Width="100px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_zalishkova" VisibleIndex="7" Caption="Залишкова Вартість" Width="100px"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_obj" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_balans" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_zalishkova" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" 
        EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" />
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.DocCard.Objects" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<mini:ProfiledSqlDataSource ID="SqlDataSourceDocCardObjects" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT bd.link_id, bd.building_id, bd.document_id, b.district, b.street_full_name, b.addr_nomer, bd.purpose_group,
                   bd.purpose, bd.obj_name, bd.cost_balans, bd.cost_zalishkova, bd.sqr_obj FROM view_building_docs bd
                   INNER JOIN view_buildings b ON b.building_id = bd.building_id WHERE bd.document_id = @doc_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="doc_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<%-- Content of the third tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Текст" Name="DocCardText">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the fourth tab BEGIN --%>

<iframe id="docCardTextFrame" src="../Documents/DoTextEmbedded.aspx?docid=<%= Request.QueryString["docid"] %>"
    frameborder="0" width="100%" height="650px"></iframe>

<%-- Content of the fourth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
    </TabPages>
</dx:ASPxPageControl>

</asp:Content>
