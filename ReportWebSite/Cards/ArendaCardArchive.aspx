<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArendaCardArchive.aspx.cs" Inherits="Cards_ArendaCardArchive"
    MasterPageFile="~/NoMenu.master" Title="Архівний Стан Договору Оренди" %>

<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function ShowArendaArchiveCard(archiveId) {

        PopupArchiveStates.Hide();

        var cardUrl = "../Cards/ArendaCardArchive.aspx?archid=" + archiveId;

        window.open(cardUrl);
    }

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaCardProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 * FROM m_view_arenda WHERE arenda_id = @arid">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="arid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaCardPropertiesArch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 * FROM view_arch_arenda WHERE archive_id = @archid">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="archid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaCardNotes" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_arenda_notes WHERE arenda_id = @arid AND (is_deleted IS NULL OR is_deleted = 0)">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="arid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaCardNotesArch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT notes.* FROM view_arch_arenda_notes notes INNER JOIN arch_arenda ar ON ar.archive_link_code = notes.archive_arenda_link_code WHERE ar.archive_id = @archid">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="archid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaCardReasons" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_arenda_link_2_decisions WHERE arenda_id = @arid">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="arid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaCardReasonsArch" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT links.* FROM view_arch_arenda_link_2_decisions links INNER JOIN arch_arenda ar ON ar.archive_link_code = links.archive_arenda_link_code WHERE ar.archive_id = @archid">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="archid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaArchive" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT archive_id, org_renter_full_name, object_name, agreement_num, agreement_date, rent_start_date, rent_finish_date, rent_square, modified_by, modify_date
        FROM view_arch_arenda WHERE arenda_id = @arid AND (NOT modified_by IS NULL) AND (NOT modify_date IS NULL) ORDER BY modify_date, archive_id" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="arid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<asp:ObjectDataSource ID="ObjectDataSourceBalansPhoto" runat="server" SelectMethod="SelectFromTempFolder" 
    TypeName="ExtDataEntry.Models.FileAttachment">
    <SelectParameters>
        <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
        <asp:Parameter DefaultValue="" Name="recordID" Type="Int32" />
        <asp:Parameter DefaultValue="" Name="tempGuid" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>



<table border="0" cellspacing="0" cellpadding="0">

<tr>
<td>
<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server">
    <TabPages>
        <dx:TabPage Text="Архівний Стан Договору Оренди" Name="ArendaArchCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server">

<%-- Content of the first tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="ArendaArchDetails" DataSourceID="SqlDataSourceArendaCardProperties" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Адреса Будинку">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server">
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
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Поверх"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox35" runat="server" ReadOnly="True" Text='<%# Eval("floor_number") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel42" runat="server" Text="Група Призначення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox40" runat="server" ReadOnly="True" Text='<%# Eval("purpose_group") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel43" runat="server" Text="Призначення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox41" runat="server" ReadOnly="True" Text='<%# Eval("purpose") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Використання Приміщення"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ReadOnly="True" Text='<%# Eval("object_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel40" runat="server" Text="Примітки"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox38" runat="server" ReadOnly="True" Text='<%# Eval("object_note") %>' Width="700px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Договір Оренди">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Балансоутримувач"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox9" runat="server" ReadOnly="True" Text='<%# Eval("org_balans_full_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Орендодавець"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox10" runat="server" ReadOnly="True" Text='<%# Eval("org_giver_full_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Орендар"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox11" runat="server" ReadOnly="True" Text='<%# Eval("org_renter_full_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Номер Договору"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("agreement_num") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Дата Договору"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" ReadOnly="True" Value='<%# Eval("agreement_date") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Початок Оренди"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" ReadOnly="True" Value='<%# Eval("rent_start_date") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Закінчення Оренди"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit3" runat="server" ReadOnly="True" Value='<%# Eval("rent_finish_date") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Факт. Закінчення Оренди"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit4" runat="server" ReadOnly="True" Value='<%# Eval("rent_actual_finish_date") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Площа (кв.м.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ReadOnly="True" Text='<%# Eval("rent_square") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td colspan="7">
                                <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" ReadOnly="True" Checked='<%# (1.Equals(Eval("is_subarenda_int"))) ? true : false %>' Text="Суборенда" />
                                <dx:ASPxCheckBox ID="ASPxCheckBox2" runat="server" ReadOnly="True" Checked='<%# (1.Equals(Eval("agreement_active_int"))) ? true : false %>' Text="Договір Діючий" />
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" HeaderText="Стан">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent5" runat="server">
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
                            
                                            <dx:ASPxGridView ID="GridViewArendaArchiveStates" ClientInstanceName="GridViewArendaArchiveStates" runat="server"
                                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceArendaArchive" KeyFieldName="archive_id" Width="780px">

                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="archive_id" VisibleIndex="0" Caption="Картка Архівного Стану">
                                                        <DataItemTemplate>
                                                            <%# "<center><a href=\"javascript:ShowArendaArchiveCard(" + Eval("archive_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
                                                        </DataItemTemplate>
                                                        <Settings ShowInFilterControl="False"/>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="org_renter_full_name" VisibleIndex="1" Caption="Орендар" Width="150px"/>
                                                    <dx:GridViewDataTextColumn FieldName="object_name" VisibleIndex="2" Caption="Використання Приміщення" Width="150px"/>
                                                    <dx:GridViewDataTextColumn FieldName="agreement_num" VisibleIndex="3" Caption="Номер Договору"/>
                                                    <dx:GridViewDataDateColumn FieldName="agreement_date" VisibleIndex="4" Caption="Дата Договору"/>
                                                    <dx:GridViewDataDateColumn FieldName="rent_start_date" VisibleIndex="5" Caption="Початок Оренди"/>
                                                    <dx:GridViewDataDateColumn FieldName="rent_finish_date" VisibleIndex="6" Caption="Закінчення Оренди"/>
                                                    <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="7" Caption="Площа (кв.м.)"/>
                                                    <dx:GridViewDataTextColumn FieldName="modified_by" VisibleIndex="8" Caption="Ким Внесені Зміни"></dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="9" Caption="Дата Внесення Змін"></dx:GridViewDataDateColumn>
                                                </Columns>

                                                <SettingsBehavior ColumnResizeMode="Control" EnableCustomizationWindow="False" />
                                                <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                                                <SettingsPager PageSize="10" />
                                                <Styles Header-Wrap="True" />
                                                <SettingsCookies CookiesID="GUKV.ArendaCard.ArchiveStates" Enabled="False" Version="A2_1" />

                                                <ClientSideEvents EndCallback="function (s,e) { GridViewArendaArchiveStates.SetHeight(500); }"/>
                                            </dx:ASPxGridView>

                                        </dx:PopupControlContentControl>
                                    </ContentCollection>

                                    <ClientSideEvents PopUp="function (s,e) { GridViewArendaArchiveStates.SetHeight(500); }"/>
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

        <dx:TabPage Text="Додаткові Відомості" Name="ArendaArchCardRates">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server">

<%-- Content of the second tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="ArendaArchDetailsPage2" DataSourceID="SqlDataSourceArendaCardProperties" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Акт Приймання-Передачі">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text="Номер Акту"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="EditOrgId" runat="server" ReadOnly="True" Text='<%# Eval("num_akt") %>' Width="270px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Дата Акту"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit5" runat="server" ReadOnly="True" Value='<%# Eval("date_akt") %>' Width="270px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Сплата За Договором">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Ставка"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox6" runat="server" ReadOnly="True" Text='<%# Eval("rent_rate_percent") %>' Width="270px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Ставка (грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox7" runat="server" ReadOnly="True" Text='<%# Eval("rent_rate_uah") %>' Width="270px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Нарахована Вартість (грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox5" runat="server" ReadOnly="True" Text='<%# Eval("cost_narah") %>' Width="270px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Сплачена Вартість (грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox8" runat="server" ReadOnly="True" Text='<%# Eval("cost_payed") %>' Width="270px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Оціночна Вартість (грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Text='<%# Eval("cost_expert_total") %>' Width="270px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Оціночна Вартість 1 Кв.м. (грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox12" runat="server" ReadOnly="True" Text='<%# Eval("cost_expert_1m") %>' Width="270px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Орендна Плата За Договором (грн.)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("cost_agreement") %>' Width="270px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="120px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Вид Розрахунків"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox13" runat="server" ReadOnly="True" Text='<%# Eval("payment_type") %>' Width="270px" /></td>
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

        <dx:TabPage Text="Об'єкти За Договором" Name="ArendaArchCardNotes">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server">

<%-- Content of the third tab BEGIN --%>

<dx:ASPxGridView ID="GridViewArendaCardNotes" ClientInstanceName="GridViewArendaCardNotes" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceArendaCardNotes" KeyFieldName="note_id" Width="840px" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="0" Caption="Площа" Width="65px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="note" VisibleIndex="1" Caption="Розташування"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_str" VisibleIndex="2" Caption="Використання" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert_total" VisibleIndex="3" Caption="Експертна Вартість (грн.)" Width="80px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="date_expert" VisibleIndex="4" Caption="Дата Експертної Оцінки"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="payment_type" VisibleIndex="5" Caption="Вид Оплати"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_narah" VisibleIndex="6" Caption="Орендна Ставка (%)" Width="85px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_rate" VisibleIndex="7" Caption="Орендна Ставка (грн.)" Width="85px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_agreement" VisibleIndex="8" Caption="Орендна Плата (грн.)" Width="85px"></dx:GridViewDataTextColumn>
    </Columns>
    
    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_expert_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_narah" SummaryType="Average" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="rent_rate" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_agreement" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="False" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="7" />
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.ArendaCardArchive.Notes" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<%-- Content of the third tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Підстави на оренду" Name="ArendaArchCardReasons">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server">

<%-- Content of the fourth tab BEGIN --%>

<dx:ASPxGridView ID="GridViewArendaCardReasons" ClientInstanceName="GridViewArendaCardReasons" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceArendaCardReasons" KeyFieldName="link_id" Width="840px" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="doc_display_name" VisibleIndex="0" Caption="Документ" Width="200px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_str" VisibleIndex="1" Caption="Призначення" Width="200px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="2" Caption="Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="decision" VisibleIndex="3" Caption="Результат Розгляду" Width="130px"></dx:GridViewDataTextColumn>
    </Columns>
    
    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="False" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="7" />
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.ArendaCardArchive.Reasons" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<%-- Content of the fourth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

		<dx:TabPage Text="Фото / плани" Name="Tab6">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl7" runat="server">

                    <dx:ASPxRoundPanel ID="PanelPhoto" runat="server" HeaderText="Фото / плани" EnableViewState="true">
                        <ContentPaddings PaddingTop="4px" PaddingLeft="4px" PaddingRight="4px" PaddingBottom="4px" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent16" runat="server">

                                <table border="0" cellspacing="0" cellpadding="0" width="990px">
                                <tr>
                                <td>

                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanelImageGallery" EnableViewState="true"
                                    ClientInstanceName="ASPxCallbackPanelImageGallery" runat="server" 
                                    OnCallback="ASPxCallbackPanelImageGallery_Callback">

                                    <SettingsLoadingPanel Enabled="false"/>
                            
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
                                        
                                    <dx:ASPxImageGallery ID="imageGalleryDemo" runat="server" DataSourceID="ObjectDataSourceBalansPhoto"
                                        EnableViewState="false" 
                                        AlwaysShowPager="false" 
                                        PagerAlign="Center"
                                        ThumbnailHeight="190" ThumbnailWidth="190"
                                        SettingsFullscreenViewer-ShowCloseButton="true" 
                                        OnDataBound="imageGalleryDemo_DataBound" >

                                      <SettingsFolder ImageCacheFolder="~\Thumb\"  /> 
                                        
<%--                                        <ItemTextTemplate>
                                            <div class="item">
                                                <div class="item_email" style="text-align:center">
                                                    <a style="color:White;cursor:pointer" onclick="javascript:DeleteImage(<%# Container.ItemIndex %>);" title="Видалити">Видалити</a>
                                                </div>
                                            </div>
                                        </ItemTextTemplate>--%>

                                        <SettingsTableLayout RowsPerPage="2" ColumnCount="5" />

                                        <SettingsTableLayout ColumnCount="5" RowsPerPage="2"></SettingsTableLayout>

                                        <PagerSettings Position="TopAndBottom">
                                            <PageSizeItemSettings Visible="False" />
                                            <PageSizeItemSettings Visible="False"></PageSizeItemSettings>
                                        </PagerSettings>

                                    </dx:ASPxImageGallery> 

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                <dx:ASPxCallbackPanel ID="ContentCallback" runat="server" EnableViewState="true"
                                    ClientInstanceName="ContentCallback">
                                    
                                    <PanelCollection>
                                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                            
<%--                                            <dx:ASPxUploadControl ID="uplImage" runat="server" ShowUploadButton="false" 
                                                FileUploadMode="OnPageLoad"
                                                ClientInstanceName="uploader" NullText="..." 
                                                
                                                ShowProgressPanel="True" Size="35" UploadMode="Auto">
                                                <ValidationSettings AllowedFileExtensions=".jpg, .jpeg, .jpe, .gif, .png, .bmp, .pdf" 
                                                    MaxFileSize="20480000">

                                                </ValidationSettings>
                                                <ClientSideEvents FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" 
                                                    FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }" 
                                                    TextChanged="function(s, e) { UpdateUploadButton(); }" />
                                                <AdvancedModeSettings EnableMultiSelect="True" />
                                            </dx:ASPxUploadControl>--%>


                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                                </td>
                                </tr>
                                            
                                                <tr>
                                                   <td>
                                                        <dx:ASPxButton ID="btnBuildPdf" runat="server" OnClick="PdfImageBuild_Click"
                                                            ClientInstanceName="btnBuildPdf" Text="Друк" >
                                                            
                                                        </dx:ASPxButton>
                                                    </td>
                                                    </tr>
                                             
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

    </TabPages>
</dx:ASPxPageControl>
</td>
</tr>

</table>

</asp:Content>
