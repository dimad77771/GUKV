<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ObjCardArchive.aspx.cs" Inherits="Cards_ObjCardArchive"
    MasterPageFile="~/NoMenu.master" Title="Архівний Стан Об'єкту" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjCardProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT TOP 1 * FROM view_arch_buildings WHERE archive_id = @arid" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="arid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="0" cellpadding="0">

<tr>
<td>
<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" ActiveTabIndex="0">
    <TabPages>
        <dx:TabPage Text="Архівний Стан Об'єкту" Name="ObjCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the first tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="ObjDetails" DataSourceID="SqlDataSourceObjCardProperties" EnableViewState="False">
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
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Поштовий Індекс"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox23" runat="server" ReadOnly="True" Text='<%# Eval("addr_zip_code") %>' Width="290px" /></td>
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
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Код Області (ОАТУУ)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox15" runat="server" ReadOnly="True" Text='<%# Eval("oatuu_code") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Область"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox7" runat="server" ReadOnly="True" Text='<%# Eval("region") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Додатково"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox6" runat="server" ReadOnly="True" Text='<%# Eval("addr_misc") %>' Width="700px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Характеристики Об'єкту">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Тип Об'єкту"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Text='<%# Eval("object_type") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Вид Об'єкту"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox5" runat="server" ReadOnly="True" Text='<%# Eval("object_kind") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Технічний Стан"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox8" runat="server" ReadOnly="True" Text='<%# Eval("condition") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Реєстровий Номер БТІ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox12" runat="server" ReadOnly="True" Text='<%# Eval("bti_code") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Історична Цінність"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ReadOnly="True" Text='<%# Eval("history") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Фасадність"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("facade") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Рік Будівництва"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox11" runat="server" ReadOnly="True" Text='<%# Eval("construct_year") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px">&nbsp;</td>
                            <td width="8px">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Експлуатаційні Характеристики">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Кількість Поверхів"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox9" runat="server" ReadOnly="True" Text='<%# Eval("num_floors") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Загальна Площа"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox10" runat="server" ReadOnly="True" Text='<%# Eval("sqr_total") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Площа Нежилих Приміщень"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox13" runat="server" ReadOnly="True" Text='<%# Eval("sqr_non_habit") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Площа Житлового Фонду"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox14" runat="server" ReadOnly="True" Text='<%# Eval("sqr_habit") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Додаткові Відомості"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox18" runat="server" ReadOnly="True" Text='<%# Eval("additional_info") %>' Width="700px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="Стан">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text='<%# EvaluateSignature(Eval("modified_by"), Eval("modify_date")) %>'></dx:ASPxLabel></td>
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
    </TabPages>
</dx:ASPxPageControl>
</td>
</tr>

</table>

</asp:Content>

