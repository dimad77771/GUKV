<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgCard.aspx.cs" Inherits="Cards_OrgCard"
    MasterPageFile="~/NoMenu.master" Title="Картка Організації" ValidateRequest="false" %>

<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v13.1, Version=13.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" language="javascript">

    // <![CDATA[

    function ShowDocumentCard(documentId) {
    
        var cardUrl = "../Cards/DocCard.aspx?arid=" + documentId;

        window.open(cardUrl);
    }


    function ShowOrgArchiveCard(archiveId) {

        PopupArchiveStates.Hide();

        var cardUrl = "../Cards/OrgCardArchive.aspx?arid=" + archiveId;

        window.open(cardUrl);
    }

    function ShowArendaArchiveCard(archiveId) {

        PopupArendaArchiveStatesForRenter.Hide();
        PopupArendaArchiveStatesForGiver.Hide();

        var cardUrl = "../Cards/ArendaCardArchive.aspx?archid=" + archiveId;

        window.open(cardUrl);
    }

    function ShowBalansArchiveCard(archiveId) {

        PopupBalansArchiveStates.Hide();

        var cardUrl = "../Cards/BalansCardArchive.aspx?arid=" + archiveId;

        window.open(cardUrl);
    }

    function OnButtonRenterCardClick(s, e) {

        if (GridViewOrgCardArenda.GetFocusedRowIndex() >= 0) {

            var balansId = GridViewOrgCardArenda.GetRowKey(GridViewOrgCardArenda.GetFocusedRowIndex());
            var cardUrl = "../Cards/ArendaCardArchive.aspx?arid=" + balansId;

            window.open(cardUrl);
        }
        else {
            alert("Будь ласка, виберіть договір оренди для перегляду його картки.");
        }
    }

    function OnButtonArendaArchiveStatesForRenterClick(s, e) {

        if (GridViewOrgCardArenda.GetFocusedRowIndex() >= 0) {
            GridViewOrgCardRenterArchiveStates.PerformCallback(GridViewOrgCardArenda.GetRowKey(GridViewOrgCardArenda.GetFocusedRowIndex()));
        }
        else {
            alert("Будь ласка, виберіть договір оренди для перегляду його архівних станів.");
        }
    }

    function OnButtonGiverCardClick(s, e) {

        if (GridViewOrgCardArendaGiven.GetFocusedRowIndex() >= 0) {

            var balansId = GridViewOrgCardArendaGiven.GetRowKey(GridViewOrgCardArendaGiven.GetFocusedRowIndex());
            var cardUrl = "../Cards/ArendaCardArchive.aspx?arid=" + balansId;

            window.open(cardUrl);
        }
        else {
            alert("Будь ласка, виберіть договір оренди для перегляду його картки.");
        }
    }

    function OnButtonArendaArchiveStatesForGiverClick(s, e) {

        if (GridViewOrgCardArendaGiven.GetFocusedRowIndex() >= 0) {
            GridViewOrgCardGiverArchiveStates.PerformCallback(GridViewOrgCardArendaGiven.GetRowKey(GridViewOrgCardArendaGiven.GetFocusedRowIndex()));
        }
        else {
            alert("Будь ласка, виберіть договір оренди для перегляду його архівних станів.");
        }
    }

    function OnButtonBalansCardClick(s, e) {

        if (GridViewOrgCardBalans.GetFocusedRowIndex() >= 0) {

            var balansId = GridViewOrgCardBalans.GetRowKey(GridViewOrgCardBalans.GetFocusedRowIndex());
            var cardUrl = "../Cards/BalansCardArchive.aspx?balid=" + balansId;

            window.open(cardUrl);
        }
        else {
            alert("Будь ласка, виберіть об'єкт для перегляду його картки.");
        }
    }

    function OnButtonBalansArchiveStatesClick(s, e) {

        if (GridViewOrgCardBalans.GetFocusedRowIndex() >= 0) {
            GridViewOrgCardBalansArchiveStates.PerformCallback(GridViewOrgCardBalans.GetRowKey(GridViewOrgCardBalans.GetFocusedRowIndex()));
        }
        else {
            alert("Будь ласка, виберіть об'єкт для перегляду його архівних станів.");
        }
    }

    // ]]>

</script>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgCardProperties" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT TOP 1 * FROM view_organizations WHERE organization_id = @org_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgCardBalans" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
        v.balans_id, 
        v.district, 
        v.street_full_name, 
        v.addr_nomer, 
        v.sqr_total, 
        v.purpose, 
        v.history, 
        v.sqr_in_rent, 
        v.is_deleted,
        sum(fs.total_free_sqr) as 'total_free_sqr'
        FROM view_balans_all v
        left join balans_free_square fs on fs.balans_id = v.balans_id
        WHERE (v.organization_id = @org_id) AND (@show_del = 1 OR (@show_del = 0 AND (v.is_deleted IS NULL OR v.is_deleted = 0)))
        group by 
        v.balans_id, 
        v.district, 
        v.street_full_name, 
        v.addr_nomer, 
        v.sqr_total, 
        v.purpose, 
        v.history, 
        v.sqr_in_rent, 
        v.is_deleted"
    OnSelecting="SqlDataSourceOrgCardBalans_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="show_del" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<%--???--%>
<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgCardArenda" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT arenda_id, district, street_full_name, addr_nomer, org_giver_full_name, rent_square, agreement_num, rent_start_date, rent_finish_date, is_subarenda,
        purpose, object_name, agreement_active_int FROM m_view_arenda_agreements WHERE org_renter_id = @org_id AND (@show_del = 1 OR (@show_del = 0 AND agreement_active_int = 1))"
    OnSelecting="SqlDataSourceOrgCardArenda_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="show_del" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgCardArendaGiven" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT arenda_id, district, street_full_name, addr_nomer, org_renter_full_name, rent_square, agreement_num, rent_start_date, rent_finish_date, is_subarenda,
        purpose, object_name, agreement_active_int FROM m_view_arenda_agreements WHERE ((org_balans_id = @org_id) OR (org_giver_id = @org_id)) AND (@show_del = 1 OR (@show_del = 0 AND agreement_active_int = 1))"
    OnSelecting="SqlDataSourceOrgCardArendaGiven_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="show_del" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgCardPrivat" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT * FROM view_privatization WHERE organization_id = @org_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgCardOrendnaPlataByBalans" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT v.* FROM view_rent_by_balans_org v 
    WHERE v.org_balans_id = @org_id  AND v.rent_period_id = (SELECT max(rbb.rent_period_id) FROM view_rent_by_balans_org rbb WHERE rbb.org_balans_id = @org_id ) 
--AND NOT EXISTS (SELECT rbb.payment_id FROM view_rent_by_balans_org rbb WHERE rbb.org_balans_id = @org_id AND rbb.rent_period_id > v.rent_period_id)">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgCardOrendnaPlataByRenter" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT v.* FROM view_rent_by_renters v
    WHERE renter_organization_id = @org_id   AND v.rent_period_id = (SELECT max(rbr.rent_period_id) FROM view_rent_by_renters rbr WHERE rbr.renter_organization_id = @org_id ) 
-- AND NOT EXISTS (SELECT rbr.rent_id FROM view_rent_by_renters rbr WHERE rbr.renter_organization_id = @org_id AND rbr.rent_period_id > v.rent_period_id)">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceOrgArchive" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>"
    SelectCommand="SELECT archive_id, full_name, addr_street_name, addr_nomer, director_fio, industry, occupation, form_gosp,
        form_of_ownership, modified_by, modify_date FROM view_arch_organizations WHERE organization_id = @org_id AND
        (NOT modified_by IS NULL) AND (NOT modify_date IS NULL) ORDER BY modify_date, archive_id" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceNormativniDocumenti" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT od.*, d.* FROM view_documents d JOIN org_docs od ON od.document_id = d.id WHERE od.organization_id = @org_id">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="org_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="0" cellpadding="0" width="100%">

<tr>
<td>
<dx:ASPxPageControl ID="CardPageControl" ClientInstanceName="CardPageControl" 
        runat="server" Width="100%" ActiveTabIndex="0">
    <TabPages>
        <dx:TabPage Text="Відомості" Name="OrgCardProperties">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the first tab BEGIN --%>

<asp:FormView runat="server" BorderStyle="None" ID="OrgDetails" DataSourceID="SqlDataSourceOrgCardProperties" EnableViewState="False">
    <ItemTemplate>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Базові відомості">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="Код ЄДРПОУ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox22" runat="server" ReadOnly="True" Text='<%# Eval("zkpo_code") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Повна Назва"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox2" runat="server" ReadOnly="True" Text='<%# Eval("full_name") %>' Width="700px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Коротка Назва"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox3" runat="server" ReadOnly="True" Text='<%# Eval("short_name") %>' Width="700px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Юридична адреса">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Місто"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox4" runat="server" ReadOnly="True" Text='<%# Eval("addr_city") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Район"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox5" runat="server" ReadOnly="True" Text='<%# Eval("addr_district") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="Назва Вулиці"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox6" runat="server" ReadOnly="True" Text='<%# Eval("addr_street_name") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="Номер Будинку"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox7" runat="server" ReadOnly="True" Text='<%# Eval("addr_nomer") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Корпус"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox8" runat="server" ReadOnly="True" Text='<%# Eval("addr_korpus") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Поштовий Індекс"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox9" runat="server" ReadOnly="True" Text='<%# Eval("addr_zip_code") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Контакти">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Директор"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox10" runat="server" ReadOnly="True" Text='<%# Eval("director_fio") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Тел. Директора"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox11" runat="server" ReadOnly="True" Text='<%# Eval("director_phone") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="Бухгалтер"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox12" runat="server" ReadOnly="True" Text='<%# Eval("buhgalter_fio") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Тел. Бухгалтера"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox13" runat="server" ReadOnly="True" Text='<%# Eval("buhgalter_phone") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="Факс"></dx:ASPxLabel></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox18" runat="server" ReadOnly="True" Text='<%# Eval("fax") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Email"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox19" runat="server" ReadOnly="True" Text='<%# Eval("contact_email") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" HeaderText="Додаткові відомості">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Фіз. / Юр. Особа"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox14" runat="server" ReadOnly="True" Text='<%# Eval("status") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Орган управління"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox15" runat="server" ReadOnly="True" Text='<%# Eval("vedomstvo") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Галузь"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox16" runat="server" ReadOnly="True" Text='<%# Eval("industry") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Вид Діяльності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox17" runat="server" ReadOnly="True" Text='<%# Eval("occupation") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Галузь (Баланс)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ReadOnly="True" Text='<%# Eval("old_industry") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Вид Діяльності (Баланс)"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox23" runat="server" ReadOnly="True" Text='<%# Eval("old_occupation") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Форма фінансування"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox20" runat="server" ReadOnly="True" Text='<%# Eval("form_gosp") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Форма Власності"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox21" runat="server" ReadOnly="True" Text='<%# Eval("form_of_ownership") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel25" runat="server" Text="Госп. Структура"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox24" runat="server" ReadOnly="True" Text='<%# Eval("gosp_struct") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="Вид Госп. Структури"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox25" runat="server" ReadOnly="True" Text='<%# Eval("gosp_struct_type") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel29" runat="server" Text="КВЕД"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox28" runat="server" ReadOnly="True" Text='<%# Eval("kved_code") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel30" runat="server" Text="ОАТУУ"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox29" runat="server" ReadOnly="True" Text='<%# Eval("koatuu") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel24" runat="server" Text="Іменник"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox27" runat="server" ReadOnly="True" Text='<%# Eval("title") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel28" runat="server" Text="Прикметник"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox30" runat="server" ReadOnly="True" Text='<%# Eval("title_form") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel31" runat="server" Text="Орг.-правова форма госп."></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox31" runat="server" ReadOnly="True" Text='<%# Eval("org_form") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel32" runat="server" Text="Сфера управління"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox32" runat="server" ReadOnly="True" Text='<%# Eval("sfera_upr") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel27" runat="server" Text="Орган госп. упр."></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox26" runat="server" ReadOnly="True" Text='<%# Eval("old_organ") %>' Width="290px" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxRoundPanel>
        <br/>
        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" HeaderText="Інформація щодо перебування в комунальній власності">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server" SupportsDisabledAttribute="True">
                    <table border="0" cellspacing="0" cellpadding="0" width="810px">
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel35" runat="server" Text="Ознака надходження"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox33" runat="server" ReadOnly="True" Text='<%# Eval("nadhodjennya") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel36" runat="server" Text="Ознака вибуття"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxTextBox ID="ASPxTextBox34" runat="server" ReadOnly="True" Text='<%# Eval("vibuttya") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel39" runat="server" Text="Дата надходження"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" ReadOnly="True" Value='<%# Eval("nadhodjennya_date") %>' Width="290px" /></td>
                            <td width="8px">&nbsp;</td>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel40" runat="server" Text="Дата вибуття"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td><dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" ReadOnly="True" Value='<%# Eval("vibuttya_date") %>' Width="290px" /></td>
                        </tr>
                        <tr><td colspan="7" height="4px"/></tr>
                        <tr>
                            <td width="100px"><dx:ASPxLabel ID="ASPxLabel37" runat="server" Text="Власність після вибуття"></dx:ASPxLabel></td>
                            <td width="8px">&nbsp;</td>
                            <td colspan="5"><dx:ASPxTextBox ID="ASPxTextBox35" runat="server" ReadOnly="True" Text='<%# Eval("form_vlasn_vibuttya") %>' Width="290px" /></td>
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
                            <td><dx:ASPxLabel ID="ASPxLabel34" runat="server" Text='<%# EvaluateSignature(Eval("modified_by"), Eval("modify_date")) %>'></dx:ASPxLabel></td>
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
                            
                                            <dx:ASPxGridView ID="GridViewOrgCardArchiveStates" ClientInstanceName="GridViewOrgCardArchiveStates" runat="server"
                                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceOrgArchive" KeyFieldName="archive_id" Width="780px">

                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="archive_id" VisibleIndex="0" Caption="Картка Архівного Стану">
                                                        <DataItemTemplate>
                                                            <%# "<center><a href=\"javascript:ShowOrgArchiveCard(" + Eval("archive_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
                                                        </DataItemTemplate>
                                                        <Settings ShowInFilterControl="False"/>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="full_name" VisibleIndex="1" Caption="Повна Назва" Width="150px"/>
                                                    <dx:GridViewDataTextColumn FieldName="addr_street_name" VisibleIndex="2" Caption="Назва Вулиці" Width="120px"/>
                                                    <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="3" Caption="Номер Будинку"/>
                                                    <dx:GridViewDataTextColumn FieldName="director_fio" VisibleIndex="4" Caption="Директор"/>
                                                    <dx:GridViewDataTextColumn FieldName="industry" VisibleIndex="5" Caption="Галузь" Width="90px"/>
                                                    <dx:GridViewDataTextColumn FieldName="occupation" VisibleIndex="6" Caption="Вид Діяльності" Width="90px"/>
                                                    <dx:GridViewDataTextColumn FieldName="form_gosp" VisibleIndex="7" Caption="Форма Фінансування"/>
                                                    <dx:GridViewDataTextColumn FieldName="form_of_ownership" VisibleIndex="8" Caption="Форма Власності"/>
                                                    <dx:GridViewDataTextColumn FieldName="modified_by" VisibleIndex="9" Caption="Ким Внесені Зміни"></dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataDateColumn FieldName="modify_date" VisibleIndex="10" Caption="Дата Внесення Змін"></dx:GridViewDataDateColumn>
                                                </Columns>

                                                <SettingsBehavior ColumnResizeMode="Control" EnableCustomizationWindow="False" />
                                                <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
                                                <SettingsPager PageSize="10" />
                                                <Styles Header-Wrap="True" />
                                                <SettingsCookies CookiesID="GUKV.OrgCard.ArchiveStates" Enabled="False" Version="A2_1" />

                                                <ClientSideEvents EndCallback="function (s,e) { GridViewOrgCardArchiveStates.SetHeight(500); }"/>
                                            </dx:ASPxGridView>

                                        </dx:PopupControlContentControl>
                                    </ContentCollection>

                                    <ClientSideEvents PopUp="function (s,e) { GridViewOrgCardArchiveStates.SetHeight(500); }"/>
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

        <dx:TabPage Text="На балансі" Name="OrgCardBalans">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl2" runat="server">

<%-- Content of the second tab BEGIN --%>

<dx:ASPxGridView ID="GridViewOrgCardBalans" ClientInstanceName="GridViewOrgCardBalans" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceOrgCardBalans" KeyFieldName="balans_id" Width="100%"
    OnCustomCallback="GridViewOrgCardBalans_CustomCallback"
    OnHtmlRowPrepared="GridViewOrgCardBalans_HtmlRowPrepared" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="balans_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="True" Caption="Картка" Width="50px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowBalansCard(" + Eval("balans_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="1" Caption="Район" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="2" Caption="Вулиця" Width="130px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="3" Caption="Номер"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total" VisibleIndex="4" Caption="Площа На Балансі"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="5" Caption="Призначення" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="history" VisibleIndex="6" Caption="Історична Цінність"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_in_rent" VisibleIndex="7" Caption="Площа Надана В Оренду"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="total_free_sqr" VisibleIndex="8" Caption="Площа вільних приміщень"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_in_rent" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle=Standard />
    <SettingsPager PageSize="10" />
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.OrgCard.Balans" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<br/>

<dx:ASPxCheckBox ID="CheckBalansShowDeleted" ClientInstanceName="CheckBalansShowDeleted" runat="server" Checked='False' Text="Відображати Видалені Об'єкти">
    <ClientSideEvents CheckedChanged='function(s, e) { GridViewOrgCardBalans.PerformCallback(""); }' />
</dx:ASPxCheckBox>

<%-- Content of the second tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Орендовано" Name="OrgCardArenda" >
            <ContentCollection>
                <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the third tab BEGIN --%>

<dx:ASPxGridView ID="GridViewOrgCardArenda" ClientInstanceName="GridViewOrgCardArenda" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceOrgCardArenda" KeyFieldName="arenda_id" Width="100%"
    OnCustomCallback="GridViewObjCardArenda_CustomCallback"
    OnHtmlRowPrepared="GridViewOrgCardArenda_HtmlRowPrepared" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="arenda_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="True" Caption="Картка" Width="50px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowArendaCard(" + Eval("arenda_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="1" Caption="Район" Width="110px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="2" Caption="Назва Вулиці" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="3" Caption="Номер Будинку"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_full_name" VisibleIndex="4" Caption="Орендодавець" Width="300px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="5" Caption="Орендована Площа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_num" VisibleIndex="6"  Caption="Номер Договору"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="rent_start_date" VisibleIndex="7" Caption="Початок Оренди"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="rent_finish_date" VisibleIndex="8" Caption="Закінчення Оренди"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="is_subarenda" VisibleIndex="9" Caption="Суборенда"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="10" Caption="Призначення" Width="100px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_name" VisibleIndex="11" Caption="Використання Приміщення" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="10" />
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.OrgCard.Arenda" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<br/>

<dx:ASPxCheckBox ID="CheckArendaShowDeleted" ClientInstanceName="CheckArendaShowDeleted" runat="server" Checked='False' Text="Відображати Видалені Договори Оренди">
    <ClientSideEvents CheckedChanged='function(s, e) { GridViewOrgCardArenda.PerformCallback(""); }' />
</dx:ASPxCheckBox>

<%-- Content of the third tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Надано в оренду" Name="OrgCardArendaGiven">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the fourth tab BEGIN --%>

<dx:ASPxGridView ID="GridViewOrgCardArendaGiven" ClientInstanceName="GridViewOrgCardArendaGiven" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceOrgCardArendaGiven" KeyFieldName="arenda_id" Width="100%"
    OnCustomCallback="GridViewOrgCardArendaGiven_CustomCallback"
    OnHtmlRowPrepared="GridViewOrgCardArendaGiven_HtmlRowPrepared" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="arenda_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="True" Caption="Картка" Width="50px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowArendaCard(" + Eval("arenda_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="1" Caption="Район" Width="110px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="2" Caption="Назва Вулиці" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="3" Caption="Номер Будинку"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_full_name" VisibleIndex="4" Caption="Орендар" Width="300px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_square" VisibleIndex="5" Caption="Орендована Площа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_num" VisibleIndex="6"  Caption="Номер Договору"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="rent_start_date" VisibleIndex="7" Caption="Початок Оренди"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="rent_finish_date" VisibleIndex="8" Caption="Закінчення Оренди"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="is_subarenda" VisibleIndex="9" Caption="Суборенда"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" VisibleIndex="10" Caption="Призначення" Width="100px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_name" VisibleIndex="11" Caption="Використання Приміщення" Width="120px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="10" />
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.OrgCard.ArendaGiven" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<br/>

<dx:ASPxCheckBox ID="CheckArendaGivenShowDeleted" ClientInstanceName="CheckArendaGivenShowDeleted" runat="server" Checked='False' Text="Відображати Видалені Договори Оренди">
    <ClientSideEvents CheckedChanged='function(s, e) { GridViewOrgCardArendaGiven.PerformCallback(""); }' />
</dx:ASPxCheckBox>

<%-- Content of the fourth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Приватизовано" Name="OrgCardPrivat">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">

<%-- Content of the fifth tab BEGIN --%>

<dx:ASPxGridView ID="GridViewOrgCardPrivat" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceOrgCardPrivat" KeyFieldName="privatization_id" Width="100%">

    <Columns>
        <dx:GridViewDataTextColumn FieldName="district" VisibleIndex="1" Caption="Район" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" VisibleIndex="2" Caption="Назва Вулиці" Width="160px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" VisibleIndex="3" Caption="Номер Будинку"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_name" VisibleIndex="4" Caption="Назва Об'єкту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total" VisibleIndex="5" Caption="Площа" Width="65px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="privat_kind" VisibleIndex="6" Caption="Спосіб Приватизації" Width="80px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="privat_state" VisibleIndex="7" Caption="Стан" Width="110px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost" VisibleIndex="8" Caption="Вартість"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_expert" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" 
        EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="7" />
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.OrgCard.Privat" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<%-- Content of the fifth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Орендна плата" Name="OrgCardOrendnaPlata">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl6" runat="server">

<%-- Content of the sixth tab BEGIN --%>

<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Отримано Орендної Плати Від Орендарів:"></dx:ASPxLabel>

<dx:ASPxGridView ID="GridViewOrgCardOrendnaPlataByBalans" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceOrgCardOrendnaPlataByBalans" KeyFieldName="payment_id" Width="100%">

    <Columns>
        <dx:GridViewDataTextColumn FieldName="rent_period" VisibleIndex="0" Caption="Звітній Період"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_balans" VisibleIndex="1" Caption="Нежитлова Площа На Балансі, всього (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_zvit_uah" VisibleIndex="2" Caption="Нараховано орендної плати, у звітньому періоді (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="received_zvit_uah" VisibleIndex="3" Caption="Отримано орендної плати, у звітньому періоді (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_narah_50_uah" VisibleIndex="4" Caption="Нарахована Сума До Бюджету 50%, у звітньому періоді (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_zvit_50_uah" VisibleIndex="5" Caption="Перераховано До Бюджету 50%, у звітньому періоді (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_debt_50_uah" VisibleIndex="6" Caption="Заборгованість Зі Сплати 50% До Бюджету Від Оренди Майна, (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_debt_30_50_uah" VisibleIndex="7" Caption="Заборгованість Зі Сплати 30% (50%) До Бюджету Від Оренди Майна Минулих Років, (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_cmk" VisibleIndex="8" Caption="Площа Цілісних Майнових Комплексів (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_narah_50_cmk_uah" VisibleIndex="9" Caption="Нараховано 50% До Бюджету Від Оренди ЦМК, всього (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_zvit_50_cmk_uah" VisibleIndex="10" Caption="Нарахована Сума До Бюджету 50% Від Оренди ЦМК, у звітньому періоді (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="budget_debt_50_cmk_uah" VisibleIndex="11" Caption="Заборгованість Зі Сплати 50% До Бюджету Від Оренди ЦМК, (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_agreements" VisibleIndex="12" Caption="Кількість Договорів Оренди"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_renters" VisibleIndex="13" Caption="Кількість Орендарів"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_v_mezhah_vitrat" VisibleIndex="14" Caption="Заборгованість Орендарів В Межах Витрат На Утримання (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_total" VisibleIndex="15" Caption="Заборгованість Орендарів, всього (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_month" VisibleIndex="16" Caption="Заборгованість Орендарів, поточна до 3 місяців (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_12_month" VisibleIndex="17" Caption="Заборгованість Орендарів, прострочена від 4 до 12 місяців (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_years" VisibleIndex="18" Caption="Заборгованість Орендарів, прострочена від 1 до 3 років (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_over_3_years" VisibleIndex="19" Caption="Заборгованість Орендарів, безнадійна (більше 3 років) (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_spysano" VisibleIndex="20" Caption="Списано Заборгованості Орендарів (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_total" VisibleIndex="21" Caption="Кількість Заходів (попереджень, приписів і т.п.), всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_zvit" VisibleIndex="22" Caption="Кількість Заходів (попереджень, приписів і т.п.), у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_total" VisibleIndex="23" Caption="Кількість Позовів До Суду, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zvit" VisibleIndex="24" Caption="Кількість Позовів До Суду, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_total" VisibleIndex="25" Caption="Задоволено Позовів, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_zvit" VisibleIndex="26" Caption="Задоволено Позовів, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_total" VisibleIndex="27" Caption="Відкрито Виконавчих Впроваджень, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_zvit" VisibleIndex="28" Caption="Відкрито Виконавчих Впроваджень, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_total" VisibleIndex="29" Caption="Погашено Заборгованості Орендарями, всього (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_zvit" VisibleIndex="30" Caption="Погашено Заборгованості Орендарями, у звітньому періоді (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_sqr_rented" VisibleIndex="31" Caption="Площа Надана В Оренду (кв.м)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_sqr_free" VisibleIndex="32" Caption="Вільна Площа (кв.м)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_sqr_korysna" VisibleIndex="33" Caption="Корисна Площа (кв.м)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="obj_sqr_mzk" VisibleIndex="34" Caption="Площа МЗК (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rozrah_50_uah" VisibleIndex="35" Caption="Розрахований Показник Нарахованої Суми До Бюджету 50%, у звітньому періоді (грн.)"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_balans" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_zvit_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="received_zvit_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_narah_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_zvit_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_debt_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_debt_30_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_cmk" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_narah_50_cmk_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_zvit_50_cmk_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="budget_debt_50_cmk_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_agreements" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_renters" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_v_mezhah_vitrat" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_3_month" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_12_month" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_3_years" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_over_3_years" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_spysano" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_zahodiv_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_zahodiv_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zadov_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zadov_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_vikon_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_vikon_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_pogasheno_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_pogasheno_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="obj_sqr_rented" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="obj_sqr_free" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="obj_sqr_korysna" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="obj_sqr_mzk" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="rozrah_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="7" />
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.OrgCard.OrendnaPlataByBalans" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<br/>

<dx:ASPxLabel ID="ASPxLabel33" runat="server" Text="Сплачено Орендної Плати Балансоутримувачам:"></dx:ASPxLabel>

<dx:ASPxGridView ID="GridViewOrgCardOrendnaPlataByRenter" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceOrgCardOrendnaPlataByRenter" KeyFieldName="rent_id" Width="100%">

    <Columns>
        <dx:GridViewDataTextColumn FieldName="rent_period" VisibleIndex="0" Caption="Звітній Період"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_full_name" VisibleIndex="1" Caption="Повна Назва Балансоутримувача" Width="180px">
            <DataItemTemplate>
                <%# EvaluateOrganizationLink(Eval("bal_org_id"), Eval("bal_org_full_name"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_zkpo" VisibleIndex="2" Caption="Код ЄДРПОУ Балансоутримувача"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="bal_org_occupation" VisibleIndex="3" Caption="Сфера Діяльності Балансоутримувача"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_total_rent" VisibleIndex="4" Caption="Орендована Площа, загальна (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_payed_by_percent" VisibleIndex="5" Caption="Орендована Площа З Відсотковою Орендною Платою. (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_payed_by_1uah" VisibleIndex="6" Caption="Орендована Площа З Орендною Платою 1 грн. (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_payed_hourly" VisibleIndex="7" Caption="Орендована Площа З Погодинною Орендною Платою. (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_narah" VisibleIndex="8" Caption="Нараховано Коштів За Оренду (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_received" VisibleIndex="9" Caption="Отримано Коштів За Оренду (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_budget_50_uah" VisibleIndex="10" Caption="Відрахування 50% До Бюджету (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_total" VisibleIndex="11" Caption="Заборгованість, всього (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_month" VisibleIndex="12" Caption="Заборгованість Поточна До 3 Місяців (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_12_month" VisibleIndex="13" Caption="Заборгованість прострочена від 4 до 12 місяців (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_years" VisibleIndex="14" Caption="Заборгованість Прострочена Від 1 До 3 Років (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_over_3_years" VisibleIndex="15" Caption="Заборгованість Безнадійна (Більше 3 Років) (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_spysano" VisibleIndex="16" Caption="Списано Заборгованості (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_total" VisibleIndex="17" Caption="Кількість Заходів (попереджень, приписів і т.п.), всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_zvit" VisibleIndex="18" Caption="Кількість Заходів (попереджень, приписів і т.п.), у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_total" VisibleIndex="19" Caption="Кількість Позовів До Суду, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zvit" VisibleIndex="20" Caption="Кількість Позовів До Суду, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_total" VisibleIndex="21" Caption="Задоволено Позовів, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_zadov_zvit" VisibleIndex="22" Caption="Задоволено Позовів, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_total" VisibleIndex="23" Caption="Відкрито Виконавчих Впроваджень, всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_pozov_vikon_zvit" VisibleIndex="24" Caption="Відкрито Виконавчих Впроваджень, у звітньому періоді"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_total" VisibleIndex="25" Caption="Погашено Заборгованості, всього (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_pogasheno_zvit" VisibleIndex="26" Caption="Погашено Заборгованості, у звітньому періоді (грн.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_v_mezhah_vitrat" VisibleIndex="27" Caption="Заборгованість З Орендної Плати, Розмір Якої Встановлено В Межах Витрат На Утримання (грн)"></dx:GridViewDataTextColumn>
    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total_rent" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_payed_by_percent" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_payed_by_1uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_payed_hourly" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_narah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_received" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="payment_budget_50_uah" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_3_month" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_12_month" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_3_years" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_over_3_years" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_spysano" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_zahodiv_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_zahodiv_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zadov_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_zadov_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_vikon_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="num_pozov_vikon_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_pogasheno_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_pogasheno_zvit" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_v_mezhah_vitrat" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="7" />
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.OrgCard.OrendnaPlataByRenter" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<%-- Content of the sixth tab END --%>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
        
        <dx:TabPage Text="Нормативні документи" Name="OrgCardNormativniDocumenti">
            <ContentCollection>
                <dx:ContentControl ID="ContentControl7" runat="server">

<%-- Content of the seventh tab BEGIN --%>

<dx:ASPxGridView ID="GridViewNormativniDocumenti" ClientInstanceName="GridViewNormativniDocumenti" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSourceNormativniDocumenti" KeyFieldName="ogranization_id" Width="100%"
    OnCustomCallback="GridViewNormativniDocumenti_CustomCallback">

    <Columns>
        <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="True" Caption="Картка" Width="50px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowDocumentCard(" + Eval("id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="doc_date" VisibleIndex="1" Caption="Дата Документу" Width="90px"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="doc_num" VisibleIndex="2" Caption="Номер Документу" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="topic" VisibleIndex="3" Caption="Назва Документу" Width="400px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="kind" VisibleIndex="4" Caption="Вид Документу" Width="150px"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="general_kind" VisibleIndex="5" Caption="Узагальнення" ></dx:GridViewDataTextColumn>
        
        <dx:GridViewDataDateColumn FieldName="receive_date" VisibleIndex="6" Caption="Дата Отримання" ReadOnly="True" Width="90px"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="source" VisibleIndex="7" Caption="Джерело Походження" ></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="state" VisibleIndex="8" Caption="Статус" ></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="commission" VisibleIndex="9" Caption="Комісія, що розглядала документ" ></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="summa" VisibleIndex="10" Caption="Сума за документом" ></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="summa_zalishkova" VisibleIndex="11" Caption="Залишкова сума" ></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="note" VisibleIndex="12" Caption="Примітка" ></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_text_exists" VisibleIndex="13" Caption="Текст Існує" ></dx:GridViewDataTextColumn>--%>
    </Columns>

    <%--<TotalSummary>
        <dx:ASPxSummaryItem FieldName="sqr_total" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_in_rent" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>--%>

    <SettingsBehavior AutoFilterRowInputDelay="1250" ColumnResizeMode="Control" EnableCustomizationWindow="True" />
    <Settings HorizontalScrollBarMode="Visible" ShowFooter="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterBar="Visible" VerticalScrollBarMode="Hidden" VerticalScrollBarStyle="Standard" />
    <SettingsPager PageSize="10" />
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Styles Header-Wrap="True" />
    <SettingsCookies CookiesID="GUKV.OrgCard.NormativniDocumenti" Enabled="False" Version="A2" />
</dx:ASPxGridView>

<%-- Content of the seventh tab END --%>

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
    <dx:ASPxButton ID="ASPxButtonPrint" runat="server" Text="Роздрукувати" onclick="ASPxButtonPrint_Click"></dx:ASPxButton>
</td>
</tr>

</table>

</asp:Content>
