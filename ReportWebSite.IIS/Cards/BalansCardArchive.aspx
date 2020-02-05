<%@ page language="C#" autoeventwireup="true" inherits="Cards_BalansCardArchive, App_Web_balanscardarchive.aspx.f411966a" masterpagefile="~/NoMenu.master" title="Архівний Стан Об'єкту На Балансі" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function ShowBalansArchiveCard(archiveId) {

        PopupArchiveStates.Hide();

        var cardUrl = "../Cards/BalansCardArchive.aspx?arid=" + archiveId;

        window.open(cardUrl);
    }

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansCardProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 * FROM view_balans_all WHERE balans_id = @balid">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="balid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansCardPropertiesArch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 *, NULL AS 'sqr_free_total', NULL AS 'sqr_free_korysna', NULL AS 'sqr_free_mzk', NULL AS 'free_sqr_floors', NULL AS 'free_sqr_purpose'
        FROM view_arch_balans WHERE archive_id = @arid">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="arid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansCardDocs" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT bdoc.id AS 'row_id', doc.kind, doc.topic, doc.doc_num, doc.doc_date, bd.sqr_obj, bd.document_id FROM
        balans_docs bdoc
        LEFT OUTER JOIN building_docs bd ON bd.id = bdoc.building_docs_id
        LEFT OUTER JOIN view_documents doc ON doc.id = bd.document_id
        WHERE bdoc.balans_id = @balid AND NOT (bd.document_id IS NULL) ORDER BY doc.doc_date">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="balid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansCardDocsArch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT bdoc.archive_id AS 'row_id', doc.kind, doc.topic, doc.doc_num, doc.doc_date, bd.sqr_obj, bd.document_id FROM
        arch_balans_docs bdoc
        INNER JOIN arch_balans ab ON ab.archive_link_code = bdoc.archive_balans_link_code
        LEFT OUTER JOIN building_docs bd ON bd.id = bdoc.building_docs_id
        LEFT OUTER JOIN view_documents doc ON doc.id = bd.document_id
        WHERE ab.archive_id = @arid AND NOT (bd.document_id IS NULL) ORDER BY doc.doc_date">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="arid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceBalansArchive" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT archive_id, street_full_name, addr_nomer, org_full_name, sqr_total, form_ownership, modify_date, modified_by
        FROM view_arch_balans WHERE balans_id = @balid AND (NOT modified_by IS NULL) AND (NOT modify_date IS NULL) ORDER BY modify_date, archive_id" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="balid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="0" cellpadding="0">

<tr>
<td>
<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server">
    <TabPages>
        <dx:TabPage Text="Архівний Стан Об'єкту На Балансі" Name="BalansCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the first tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="BalansDetails" DataSourceID="SqlDataSourceBalansCardProperties" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Адреса Будинку">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Район"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditOrgId" runat="server" ReadOnly="True" Text='<%# Eval("district") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Назва Вулиці"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox34" runat="server" ReadOnly="True" Text='<%# Eval("street_full_name") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Номер Будинку"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox33" runat="server" ReadOnly="True" Text='<%# Eval("addr_nomer") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Додаткова Адреса"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox35" runat="server" ReadOnly="True" Text='<%# Eval("addr_misc") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Кількість Поверхів"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ReadOnly="True" Text='<%# Eval("num_floors") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Розташування (Поверх)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("floors") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Документація БТІ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ReadOnly="True" Text='<%# Eval("bti_condition") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Інвентарний Номер в БТІ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Text='<%# Eval("invent_no_bti") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Дата Реєстрації в БТІ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit4" runat="server" ReadOnly="True" Value='<%# Eval("date_bti") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Реєстраційний № в БТІ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox6" runat="server" ReadOnly="True" Text='<%# Eval("obj_bti_code") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Балансоутримувач">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Балансоутримувач"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox10" runat="server" ReadOnly="True" Text='<%# Eval("org_full_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Право"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox12" runat="server" ReadOnly="True" Text='<%# Eval("ownership_type") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Форма Власності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox13" runat="server" ReadOnly="True" Text='<%# Eval("org_ownership") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Утримуюча Організація"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox11" runat="server" ReadOnly="True" Text='<%# Eval("org_maintainer_full_name") %>' Width="700px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Додаткові Відомості">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Вид Обєкта"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox7" runat="server" ReadOnly="True" Text='<%# Eval("object_kind") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Тип Обєкта"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox8" runat="server" ReadOnly="True" Text='<%# Eval("object_type") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Група Призначення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox14" runat="server" ReadOnly="True" Text='<%# Eval("purpose_group") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Призначення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox15" runat="server" ReadOnly="True" Text='<%# Eval("purpose") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Технічний Стан"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox16" runat="server" ReadOnly="True" Text='<%# Eval("condition") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Форма Власності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox17" runat="server" ReadOnly="True" Text='<%# Eval("form_ownership") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Історична Цінність"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox18" runat="server" ReadOnly="True" Text='<%# Eval("history") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Відділ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox19" runat="server" ReadOnly="True" Text='<%# Eval("otdel_gukv") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Назва Об'єкту"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox5" runat="server" ReadOnly="True" Text='<%# Eval("balans_obj_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Примітка"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox9" runat="server" ReadOnly="True" Text='<%# Eval("note") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td colspan="7">
                                <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" ReadOnly="True" Checked='<%# (1.Equals(Eval("priznak_1nf"))) ? true : false %>' Text="Введено по 1НФ" />
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" HeaderText="Стан">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel33" runat="server" Text='<%# EvaluateSignature(Eval("modified_by"), Eval("modify_date")) %>'></dx:ASPxLabel></td>
                            <td align="right">
                                <dx:ASPxPopupControl ID="PopupArchiveStates" runat="server" 
                                    HeaderText="Архівні стани" 
                                    ClientInstanceName="PopupArchiveStates" 
                                    PopupElementID="CardPageControl"
                                    PopupAction="None"
                                    PopupHorizontalAlign="Center"
                                    PopupVerticalAlign="Middle"
                                    PopupAnimationType="Slide">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            
                                            <dx:ASPxGridView ID="GridViewBalansArchiveStates" ClientInstanceName="GridViewBalansArchiveStates" runat="server"
                                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceBalansArchive" KeyFieldName="archive_id" Width="780px">

                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="archive_id" VisibleIndex="0" Caption="Картка Архівного Стану">
                                                        <DataItemTemplate>
                                                            <%# "<center><a href=\"javascript:ShowBalansArchiveCard(" + Eval("archive_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
                                                        </DataItemTemplate>
                                                        <Settings ShowInFilterControl="False"/>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="1" Caption="Назва Вулиці" Width="120px"/>
                                                    <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="2" Caption="Номер Будинку"/>
                                                    <dx:GridViewDataTextColumn FieldName="org_full_name" VisibleIndex="3" Caption="Балансоутримувач" Width="180px"/>
                                                    <dx:GridViewDataTextColumn FieldName="sqr_total" VisibleIndex="4" Caption="Площа На Балансі (кв.м.)"/>
                                                    <dx:GridViewDataTextColumn FieldName="form_ownership" VisibleIndex="5" Caption="Форма Власності"/>
                                                    <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="6" Caption="Коли Внесені Зміни"/>
                                                    <dx:GridViewDataTextColumn FieldName="modified_by" VisibleIndex="7" Caption="Ким Внесені Зміни"/>
                                                </Columns>

                                                <SettingsBehavior ColumnResizeMode="Control" EnableCustomizationWindow="False" />
                                                <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Visible" VerticalScrollBarStyle="Virtual" />
                                                <SettingsPager PageSize="10" />
                                                <Styles Header-Wrap="True" />
                                                <SettingsCookies CookiesID="GUKV.BalansCard.ArchiveStates" Enabled="False" Version="A2_1" />

                                                <ClientSideEvents EndCallback="function (s,e) { GridViewBalansArchiveStates.SetHeight(500); }"/>
                                            </dx:ASPxGridView>

                                        </dx:PopupControlContentControl>
                                    </ContentCollection>

                                    <ClientSideEvents PopUp="function (s,e) { GridViewBalansArchiveStates.SetHeight(500); }"/>
                                </dx:ASPxPopupControl>

                                <dx:ASPxButton ID="ASPxButtonArchive" ClientInstanceName="ASPxButtonArchive" runat="server" Text="Архівні Стани" AutoPostBack="false"
                                    ClientVisible='<%# IsHistoryButtonVisible() %>' >
                                    <ClientSideEvents Click="function (s,e) { PopupArchiveStates.Show(); }" />
                                </dx:ASPxButton>
                            </td>
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

        <dx:TabPage Text="Інформація щодо площ" Name="BalansCardSqr">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the second tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="BalansDetailsPage2" DataSourceID="SqlDataSourceBalansCardProperties" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Інформація щодо площ">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Площа Нежилих Приміщень На Балансі (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditOrgId" runat="server" ReadOnly="True" Text='<%# Eval("sqr_total") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Корисна Площа Нежилих Приміщень На Балансі (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox34" runat="server" ReadOnly="True" Text='<%# Eval("sqr_kor") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Площа Власних Потреб (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox33" runat="server" ReadOnly="True" Text='<%# Eval("sqr_vlas_potreb") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Площа Підвалів (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox35" runat="server" ReadOnly="True" Text='<%# Eval("sqr_pidval") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td colspan="5"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Площа Власних Потреб, Яка Не Може Бути Надана В Оренду (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("sqr_not_for_rent") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="24px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Площа Надана В Оренду (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ReadOnly="True" Text='<%# Eval("sqr_in_rent") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Площа Приватизована (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Text='<%# Eval("sqr_privatizov") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Кількість Договорів Оренди"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox20" runat="server" ReadOnly="True" Text='<%# Eval("num_rent_agr") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Кількість Приватизованих Приміщень"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox6" runat="server" ReadOnly="True" Text='<%# Eval("num_privat_apt") %>' Width="140px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel8" runat="server" HeaderText="Вільні Приміщення">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent8" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel40" runat="server" Text="Вільні Приміщення: Загальна Площа (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox32" runat="server" ReadOnly="True" Text='<%# Eval("sqr_free_total") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel41" runat="server" Text="Вільні Приміщення: Корисна Площа (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox36" runat="server" ReadOnly="True" Text='<%# Eval("sqr_free_korysna") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel42" runat="server" Text="Вільні Приміщення МЗК (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox37" runat="server" ReadOnly="True" Text='<%# Eval("sqr_free_mzk") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel43" runat="server" Text="Місце Розташування Вільного Приміщення (поверх)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox38" runat="server" ReadOnly="True" Text='<%# Eval("free_sqr_floors") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel38" runat="server" Text="Можливе Використання Вільного Приміщення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox30" runat="server" ReadOnly="True" Text='<%# Eval("free_sqr_purpose") %>' Width="549px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </ItemTemplate>
</asp:FormView>

<%-- Content of the second tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Вартісні показники" Name="BalansCardCosts">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the third tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="BalansDetailsPage3" DataSourceID="SqlDataSourceBalansCardProperties" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Вартість Об'єкту">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Балансова Вартість (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditOrgId" runat="server" ReadOnly="True" Text='<%# Eval("cost_balans") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Залишкова Вартість (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox34" runat="server" ReadOnly="True" Text='<%# Eval("cost_zalishkova") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Знос"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox33" runat="server" ReadOnly="True" Text='<%# Eval("znos") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Станом на"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" ReadOnly="True" Value='<%# Eval("znos_date") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Ринкова Вартість Приміщення (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ReadOnly="True" Text='<%# Eval("cost_rinkova") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Станом на"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" ReadOnly="True" Value='<%# Eval("date_cost_rinkova") %>' Width="140px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Експертна Оцінка">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Вартість Приміщення (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox21" runat="server" ReadOnly="True" Text='<%# Eval("cost_expert_total") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Вартість 1 кв.м."></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox22" runat="server" ReadOnly="True" Text='<%# Eval("cost_expert_1m") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Дата Експертної Оцінки"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit3" runat="server" ReadOnly="True" Value='<%# Eval("date_expert") %>' Width="140px" /></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="Справедлива Вартість">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Вартість Приміщення (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox23" runat="server" ReadOnly="True" Text='<%# Eval("cost_fair") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Вартість 1 кв.м."></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox24" runat="server" ReadOnly="True" Text='<%# Eval("cost_fair_1m") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel26" runat="server" Text=""></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit5" runat="server" ReadOnly="True" Value='<%# Eval("fair_cost_date") %>' Width="140px" /></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel7" runat="server" HeaderText="Орендна Плата">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent7" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Орендна Плата Нарахована (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox25" runat="server" ReadOnly="True" Text='<%# Eval("cost_rent_narah") %>' Width="140px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Орендна Плата Сплачена (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox26" runat="server" ReadOnly="True" Text='<%# Eval("cost_rent_payed") %>' Width="140px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="250px"><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="Заборгованість (тис.грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit6" runat="server" ReadOnly="True" Value='<%# Eval("cost_debt") %>' Width="140px" /></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
    </ItemTemplate>
</asp:FormView>

<%-- Content of the third tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Документи" Name="BalansCardDocs">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the fourth tab BEGIN --%>

<dx:ASPxGridView ID="GridViewBalansCardDocs" runat="server" 
    ClientInstanceName="GridViewBalansCardDocs"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceBalansCardDocs" 
    KeyFieldName="row_id"
    Width="840px" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="kind" VisibleIndex="0" Caption="Вид Документу" Width="120px" />
        <dx:GridViewDataTextColumn FieldName="topic" VisibleIndex="1" Caption="Назва Документу" Width="350px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("document_id") + ")\">" + Eval("topic") + "</a>"%>
            </DataItemTemplate> 
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="doc_num" VisibleIndex="2" Caption="Номер Документу">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("document_id") + ")\">" + Eval("doc_num") + "</a>"%>
            </DataItemTemplate> 
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="doc_date" VisibleIndex="3" Caption="Дата Документу"/>
        <dx:GridViewDataTextColumn FieldName="sqr_obj" VisibleIndex="4" Caption="Площа Об'єкту За Документом" />
    </Columns>

    <SettingsBehavior EnableCustomizationWindow="False" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" AllowFocusedRow="false" />
    <SettingsPager PageSize="15">
    </SettingsPager>
    <Settings
        HorizontalScrollBarMode="Visible"
        ShowFooter="True" />
    <SettingsCookies CookiesID="GUKV.BalansCardArchive.Docs" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<%-- Content of the fourth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
    </TabPages>
</dx:ASPxPageControl>
</td>
</tr>

</table>
</asp:Content>
