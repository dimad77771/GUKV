<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ObjCard.aspx.cs" Inherits="Cards_ObjCard"
    MasterPageFile="~/NoHeader.master" Title="Картка Об'єкту" ValidateRequest="false" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function ShowObjectArchiveCard(archiveId) {

        PopupArchiveStates.Hide();

        var cardUrl = "../Cards/ObjCardArchive.aspx?arid=" + archiveId;

        window.open(cardUrl);
    }

    function ShowArendaArchiveCard(archiveId) {

        PopupArendaArchiveStates.Hide();

        var cardUrl = "../Cards/ArendaCardArchive.aspx?archid=" + archiveId;

        window.open(cardUrl);
    }

    function ShowBalansArchiveCard(archiveId) {

        PopupBalansArchiveStates.Hide();

        var cardUrl = "../Cards/BalansCardArchive.aspx?arid=" + archiveId;

        window.open(cardUrl);
    }

    function OnButtonArendaCardClick(s, e) {

        if (GridViewObjCardArenda.GetFocusedRowIndex() >= 0) {

            var arendaId = GridViewObjCardArenda.GetRowKey(GridViewObjCardArenda.GetFocusedRowIndex());
            var cardUrl = "../Cards/ArendaCardArchive.aspx?arid=" + arendaId;

            window.open(cardUrl);
        }
        else {
            alert("Будь ласка, виберіть договір оренди для перегляду його картки.");
        }
    }

    function OnButtonArendaArchiveStatesClick(s, e) {

        if (GridViewObjCardArenda.GetFocusedRowIndex() >= 0) {
            GridViewObjCardArendaArchiveStates.PerformCallback(GridViewObjCardArenda.GetRowKey(GridViewObjCardArenda.GetFocusedRowIndex()));
        }
        else {
            alert("Будь ласка, виберіть договір оренди для перегляду його архівних станів.");
        }
    }

    function OnButtonBalansCardClick(s, e) {

        if (GridViewObjCardBalans.GetFocusedRowIndex() >= 0) {

            var balansId = GridViewObjCardBalans.GetRowKey(GridViewObjCardBalans.GetFocusedRowIndex());
            var cardUrl = "../Cards/BalansCardArchive.aspx?balid=" + balansId;

            window.open(cardUrl);
        }
        else {
            alert("Будь ласка, виберіть об'єкт для перегляду його картки.");
        }
    }

    function OnButtonBalansArchiveStatesClick(s, e) {

        if (GridViewObjCardBalans.GetFocusedRowIndex() >= 0) {
            GridViewObjCardBalansArchiveStates.PerformCallback(GridViewObjCardBalans.GetRowKey(GridViewObjCardBalans.GetFocusedRowIndex()));
        }
        else {
            alert("Будь ласка, виберіть об'єкт для перегляду його архівних станів.");
        }
    }

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjCardProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT *, sqr_object_mk + sqr_object_other AS 'sqr_other' FROM view_buildings WHERE building_id = @bid" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjCardBalans" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT balans_id, organization_id, org_full_name, sqr_total, form_ownership, modify_date, is_deleted FROM view_balans_all
        WHERE building_id = @bid AND (@show_del = 1 OR (@show_del = 0 AND (is_deleted IS NULL OR is_deleted = 0 OR is_not_accepted = 1)))"
    OnSelecting="SqlDataSourceObjCardBalans_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="show_del" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjCardArenda" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT arenda_id, org_renter_full_name, org_renter_id, org_giver_full_name, org_giver_id, rent_square, agreement_num,
        rent_start_date, rent_finish_date, is_subarenda, purpose, object_name, agreement_active_int
    ,(CASE WHEN agreement_state = 1 THEN 'Договір діє' ELSE CASE WHEN agreement_state = 2 THEN 'Договір закінчився, але заборгованність не погашено' ELSE CASE WHEN agreement_state = 3 THEN 'Договір закінчився, оренда продовжена іншим договором' ELSE '' END END END) AS 'agreement_state'
    FROM view_arenda_agreements
        WHERE building_id = @bid AND (@show_del = 1 OR (@show_del = 0 AND agreement_active_int = 1))"
    OnSelecting="SqlDataSourceObjCardArenda_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bid" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="show_del" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjCardPurchase" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT arenda_id, org_renter_full_name, org_renter_id, org_giver_full_name, org_giver_id, rent_square, agreement_num, agreement_kind,
        agreement_date, object_name FROM view_arenda_privat_docs WHERE building_id = @bid" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjCardPrivat" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT * FROM view_privatization WHERE building_id = @bid" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjCardTransfer" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT * FROM m_view_object_rights WHERE building_id = @bid" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceObjArchive" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT archive_id, street_full_name, addr_nomer, sqr_total, modify_date, modified_by,
        sqr_object_mk + sqr_object_dk AS 'sqr_kom_total' FROM view_arch_buildings WHERE building_id = @bid AND
        (NOT modified_by IS NULL) AND (NOT modify_date IS NULL) ORDER BY modify_date, archive_id" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="bid" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="0" cellpadding="0" width="100%">

<tr>
<td>
<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" runat="server" ActiveTabIndex="0" Width="100%">
    <TabPages>
        <dx:TabPage Text="Будинок" Name="ObjCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server">

<%-- Content of the first tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="ObjDetails" DataSourceID="SqlDataSourceObjCardProperties" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Адреса Будинку">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server">
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

        <%--<br/>--%>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Експлуатаційні Характеристики" Visible="false">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="320px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Кількість Поверхів"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox9" runat="server" ReadOnly="True" Text='<%# Eval("num_floors") %>' Width="70px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="320px"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Загальна Площа"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox10" runat="server" ReadOnly="True" Text='<%# Eval("sqr_total") %>' Width="70px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="320px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Площа Нежилих Приміщень"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox13" runat="server" ReadOnly="True" Text='<%# Eval("sqr_non_habit") %>' Width="70px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="320px"><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Площа Житлового Фонду"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox14" runat="server" ReadOnly="True" Text='<%# Eval("sqr_habit") %>' Width="70px" /></td>
                        </tr>

                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="320px"><dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Площа Міської Комунальної Власності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox16" runat="server" ReadOnly="True" Text='<%# Eval("sqr_object_mk") %>' Width="70px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="320px"><dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Площа Державної Форми Власності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox17" runat="server" ReadOnly="True" Text='<%# Eval("sqr_object_dk") %>' Width="70px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="320px"><dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Площа Міської Комунальної Власності Надана в Оренду"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox18" runat="server" ReadOnly="True" Text='<%# Eval("sqr_rented") %>' Width="70px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="320px"><dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Площа Інших Форм Власності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox19" runat="server" ReadOnly="True" Text='<%# Eval("sqr_other") %>' Width="70px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                    </table>
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="150px"><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Додаткові Відомості"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox21" runat="server" ReadOnly="True" Text='<%# Eval("additional_info") %>' Width="650px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="PanelObjState" runat="server" HeaderText="Стан">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text='<%# EvaluateSignature(Eval("modified_by"), Eval("modify_date")) %>'></dx:ASPxLabel></td>
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
                            
                                            <dx:ASPxGridView ID="GridViewObjCardArchiveStates" ClientInstanceName="GridViewObjCardArchiveStates" runat="server"
                                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceObjArchive" KeyFieldName="archive_id" Width="720px">

                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="archive_id" VisibleIndex="0" Caption="Картка Архівного Стану">
                                                        <DataItemTemplate>
                                                            <%# "<center><a href=\"javascript:ShowObjectArchiveCard(" + Eval("archive_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
                                                        </DataItemTemplate>
                                                        <Settings ShowInFilterControl="False"/>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="1" Caption="Назва Вулиці" Width="120px"/>
                                                    <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="2" Caption="Номер Будинку" Width="80px"/>
                                                    <dx:GridViewDataTextColumn FieldName="sqr_total" VisibleIndex="3" Caption="Загальна Площа"/>
                                                    <dx:GridViewDataTextColumn FieldName="sqr_kom_total" VisibleIndex="4" Caption="Площа Комунальної Форми Власності"/>
                                                    <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="5" Caption="Дата Внесення Змін"></dx:GridViewDataDateColumn>
                                                    <dx:GridViewDataTextColumn FieldName="modified_by" VisibleIndex="6" Caption="Ким Внесені Зміни"></dx:GridViewDataTextColumn>
                                                </Columns>

                                                <SettingsBehavior ColumnResizeMode="Control" EnableCustomizationWindow="False" />
                                                <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                                                <SettingsPager PageSize="10" />
                                                <Styles Header-Wrap="True" />
                                                <SettingsCookies CookiesID="GUKV.ObjCard.ArchiveStates" Enabled="False" Version="A2_1" />

                                                <ClientSideEvents EndCallback="function (s,e) { GridViewObjCardArchiveStates.SetHeight(500); }"/>
                                            </dx:ASPxGridView>

                                        </dx:PopupControlContentControl>
                                    </ContentCollection>

                                    <ClientSideEvents PopUp="function (s,e) { GridViewObjCardArchiveStates.SetHeight(500); }"/>
                                </dx:ASPxPopupControl>

                                <dx:ASPxButton ID="ASPxButtonArchive" ClientInstanceName="ASPxButtonArchive" runat="server" Text="Архівні Стани" AutoPostBack="false">
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

        <dx:TabPage Text="Балансоутримувачі" Name="ObjCardBalans">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl6" runat="server">

<%-- Content of the second tab BEGIN --%>

<dx:ASPxGridView ID="GridViewObjCardBalans" runat="server" 
    ClientInstanceName="GridViewObjCardBalans"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceObjCardBalans" 
    KeyFieldName="balans_id"
    Width="100%"
    OnCustomCallback="GridViewObjCardBalans_CustomCallback"
    OnHtmlRowPrepared="GridViewObjCardBalans_HtmlRowPrepared" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="balans_id" VisibleIndex="0" Visible="True" Caption="Картка Об'єкту на Балансі" Width="80px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowBalansCard(" + Eval("balans_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_full_name" VisibleIndex="1" Visible="True" Caption="Балансоутримувач" Width="300px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("org_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total" VisibleIndex="2" Visible="True" Caption="Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_ownership" VisibleIndex="3" Visible="True" Caption="Форма Власності Об'єкту" Width="170px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="4" Visible="True" Caption="Дата Актуальності"></dx:GridViewDataDateColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager PageSize="10"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.ObjCard.Balans" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<br/>

<dx:ASPxCheckBox ID="CheckBalansShowDeleted" ClientInstanceName="CheckBalansShowDeleted" runat="server" Checked='False' Text="Відображати Видалені Об'єкти">
    <ClientSideEvents CheckedChanged='function(s, e) { GridViewObjCardBalans.PerformCallback(""); }' />
</dx:ASPxCheckBox>

<%-- Content of the second tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Договори Оренди" Name="ObjCardArenda">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server">

<%-- Content of the third tab BEGIN --%>

<dx:ASPxGridView ID="GridViewObjCardArenda" runat="server" 
    ClientInstanceName="GridViewObjCardArenda"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceObjCardArenda" 
    KeyFieldName="arenda_id"
    Width="100%"
    OnCustomCallback="GridViewObjCardArenda_CustomCallback"
    OnHtmlRowPrepared="GridViewObjCardArenda_HtmlRowPrepared"
    OnCustomSummaryCalculate="GridViewObjCardArenda_CustomSummaryCalculate" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="arenda_id" VisibleIndex="0" Visible="True" Caption="Картка Договору Оренди" Width="70px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowArendaCard(" + Eval("arenda_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_full_name" VisibleIndex="1" Visible="True" Caption="Орендар" Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_full_name" VisibleIndex="2" Visible="True" Caption="Орендодавець" Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="3" Visible="True" Caption="Орендована Площа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_num" VisibleIndex="4" Visible="True" Caption="Номер Договору Оренди" Width="80px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="rent_start_date" VisibleIndex="5" Visible="True" Caption="Початок Оренди" Width="80px"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="rent_finish_date" VisibleIndex="6" Visible="True" Caption="Закінчення Оренди" Width="80px"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="is_subarenda" VisibleIndex="7" Caption="Суборенда" Width="90px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="8" Caption="Призначення" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_name" VisibleIndex="9" Caption="Використання Приміщення" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_state" VisibleIndex="10" Caption="Стан договору"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
 
           </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Custom" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager PageSize="10"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.ObjCard.Arenda" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<br/>

<dx:ASPxCheckBox ID="CheckArendaShowDeleted" ClientInstanceName="CheckArendaShowDeleted" runat="server" Checked='False' Text="Відображати Видалені Договори Оренди">
    <ClientSideEvents CheckedChanged='function(s, e) { GridViewObjCardArenda.PerformCallback(""); }' />
</dx:ASPxCheckBox>

<%-- Content of the third tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Договори Купівлі-Продажу" Name="ObjCardPurchase">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl5" runat="server">

<%-- Content of the fourth tab BEGIN --%>

<dx:ASPxGridView ID="GridViewObjCardPurchase" runat="server" 
    ClientInstanceName="GridViewObjCardPurchase"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceObjCardPurchase" 
    KeyFieldName="arenda_id"
    Width="100%" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="org_renter_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="0" Visible="True" Caption="Покупець" Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_full_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="True" Caption="Продавець" Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="True" Caption="Назва Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_square" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="True" Caption="Площа" Width="75px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Caption="Тип Договору"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="agreement_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="True" Caption="Дата Договору" Width="80px"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_num" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="True" Caption="Номер Договору" Width="80px"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True" 
        AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager PageSize="15">
    </SettingsPager>
    <Settings
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True" />
    <SettingsCookies CookiesID="GUKV.ObjCard.Arenda" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<%-- Content of the fourth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Приватизація" Name="ObjCardPrivat">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server">

<%-- Content of the fifth tab BEGIN --%>

<dx:ASPxGridView ID="GridViewObjCardPrivat" runat="server" 
    ClientInstanceName="GridViewObjCardPrivat"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceObjCardPrivat" 
    KeyFieldName="privatization_id"
    Width="100%" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="org_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="0" Visible="True" Caption="Організація" Width="180px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("org_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_address" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="1" Visible="True" Caption="Юридична Адреса" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_name" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="2" Visible="True" Caption="Назва Об'єкту" Width="200px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="3" Visible="True" Caption="Площа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_group" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="4" Visible="True" Caption="Група Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="privat_kind" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="5" Visible="True" Caption="Спосіб Приватизації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="privat_state" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Visible="True" Caption="Стан"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="7" Visible="True" Caption="Вартість"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="8" Visible="True" Caption="Оціночна Вартість"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="expert_date" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="9" Visible="True" Caption="Дата Затвердження Оцінки"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="note" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10" Visible="True" Caption="Примітка"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_expert" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True" 
        AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager PageSize="15">
    </SettingsPager>
    <Settings
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True" />
    <SettingsCookies CookiesID="GUKV.ObjCard.Privat" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<%-- Content of the fifth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Передача Прав" Name="ObjCardTransfer">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server">

<%-- Content of the sixs tab BEGIN --%>

<dx:ASPxGridView ID="GridViewObjCardTransfer" runat="server" 
    ClientInstanceName="GridViewObjCardTransfer"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceObjCardTransfer" 
    KeyFieldName="transfer_id"
    Width="100%" >

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>

    <Columns>
        <dx:GridViewDataTextColumn FieldName="org_from_short_name" VisibleIndex="0" Caption="Від Кого" Width="160px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_from_id") + ")\">" + Eval("org_from_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_to_short_name" VisibleIndex="1" Caption="Кому" Width="160px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_to_id") + ")\">" + Eval("org_to_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="right_name" VisibleIndex="2" Caption="Право"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_transferred" VisibleIndex="3" Caption="Передана Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="len_transferred" VisibleIndex="4" Caption="Передана Довжина (м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="name" VisibleIndex="5" Caption="Назва Об'єкту" Width="140px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rozp_doc_num" VisibleIndex="6" Caption="Номер Розпорядження" Width="80px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("rozp_doc_id") + ")\">" + Eval("rozp_doc_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="rozp_doc_date" VisibleIndex="7" Caption="Дата Розпорядження"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="akt_num" VisibleIndex="8" Caption="Номер Акту Приймання-передачі" Width="80px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowDocumentCard(" + Eval("akt_id") + ")\">" + Eval("akt_num") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="akt_date" VisibleIndex="9" Caption="Дата Акту Приймання-передачі"></dx:GridViewDataDateColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sum_balans" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sum_zalishkova" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_transferred" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="len_transferred" SummaryType="Custom" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True" 
        AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager PageSize="15">
    </SettingsPager>
    <Settings
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard"
        HorizontalScrollBarMode="Visible"
        ShowFooter="True" />
    <SettingsCookies CookiesID="GUKV.ObjCard.Transfer" Version="A2" Enabled="False" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>
</dx:ASPxGridView>

<%-- Content of the sixs tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
    </TabPages>
</dx:ASPxPageControl>
</td>
</tr>

<tr><td height="4px"></td></tr>

<tr>
<td>
	<table cellspacing="0" cellpadding="0" width="810px">
		<tr>
			<td>
				<dx:ASPxButton ID="ASPxButtonPrint" runat="server" Text="Роздрукувати" onclick="ASPxButtonPrint_Click"></dx:ASPxButton>
			</td>
			<td align="right">
				<dx:ASPxButton ID="ASPxButtonDelete" runat="server" Text="Вилучити" onclick="ASPxButtonPrint_Delete" Visible="true">
					<ClientSideEvents Click="function(s, e) {
							var r = confirm('Ви дійсно хочете вилучити адресу ?');
							if (r == true)
							{
								e.processOnServer = true;
							}
							else
							{
								e.processOnServer = false;
							}
						 }" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
</td>
</tr>

</table>

</asp:Content>

