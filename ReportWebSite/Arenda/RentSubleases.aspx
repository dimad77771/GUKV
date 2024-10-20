﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RentSubleases.aspx.cs" Inherits="Arenda_RentSubleases" MasterPageFile="~/NoHeader.master" Title="Договори Оренди" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/SaveReportCtrl.ascx" tagname="SaveReportCtrl" tagprefix="uc1" %>
<%@ Register src="../UserControls/AddressPicker.ascx" tagname="AddressPicker" tagprefix="uc2" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register src="../UserControls/FieldFixxer.ascx" tagname="FieldFixxer" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function ShowFieldFixxerPopupControl(s, e) { PopupFieldFixxer.Show(); }

    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 185);
    }

    function GridViewArendaObjectsInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewArendaObjectsEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowFoldersPopupControl(s, e) {

        PopupControlFolders.Show();
    }

    function CheckBoxRentedObjectsComVlasn_CheckedChanged(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("bind:"));
    }

    function ShowAddressPickerPopupControl(s, e) {

        PopupAddressPicker.Show();
    }

    function ShowFieldChooserPopupControl(s, e) {

        PopupFieldChooser.Show();
    }

    function CheckBoxRentedObjectsDPZ_CheckedChanged(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("bind:"));
    }

    // ]]>

</script>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
    <Items>
        <dx:MenuItem NavigateUrl="../Arenda/RentAgreements.aspx" Text="Договори Оренди"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Arenda/RentedObjects.aspx" Text="Орендовані Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Arenda/RentOrgList.aspx" Text="Перелік Орендарів"></dx:MenuItem>
		<dx:MenuItem NavigateUrl="../Arenda/RentSubleases.aspx" Text="Договори Суборенди"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Договори Суборенди" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxCheckBox ID="CheckBoxRentedObjectsDPZ" runat="server" Checked='True' Text="Дані ДПЗ" Visible="false"
                Width="100px" ClientInstanceName="CheckBoxRentedObjectsDPZ" >
                <ClientSideEvents CheckedChanged="CheckBoxRentedObjectsDPZ_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>        
        <td>
            <dx:ASPxCheckBox ID="CheckBoxRentedObjectsComVlasn" runat="server" Checked='False' Text="Лише Ком. Власність" Visible="false"
                Width="155px" ClientInstanceName="CheckBoxRentedObjectsComVlasn" >
                <ClientSideEvents CheckedChanged="CheckBoxRentedObjectsComVlasn_CheckedChanged" />
            </dx:ASPxCheckBox>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonQuickSearchAddr1" runat="server" AutoPostBack="False" Text="" ImageSpacing="0px" AllowFocus="false" Visible="false"
                ToolTip="Щвидкий пошук за адресою">
                <Image Url="../Styles/HouseIcon.png" />
                <FocusRectPaddings Padding="1px" />
                <ClientSideEvents Click="ShowAddressPickerPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup1" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px" Visible="false">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
	        <dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px" Visible="false">
		        <ClientSideEvents Click="ShowFieldFixxerPopupControl" />
	        </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False"  Visible="false"
                Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl2" runat="server"  Visible="false"
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_ArendaObjects_SaveAs" 
                PopupElementID="ASPxButton_ArendaObjects_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton2" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_ArendaObjects_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_ArendaObjects_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_ArendaObjects_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_ArendaObjects_SaveAs" runat="server" AutoPostBack="False" Visible="false"
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="ASPxGridViewExporterArendaObjects" runat="server" 
    FileName="Оренда" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaObjects" runat="server" EnableCaching="false"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
t.id as sublease_id,
t.agreement_num as sublease_agreement_num,
t.agreement_date as sublease_agreement_date,
t.payment_type_id as sublease_payment_type_id,
t.rent_start_date as sublease_rent_start_date,
t.rent_finish_date as sublease_rent_finish_date,
t.rent_square as sublease_rent_square,
t.rent_payment_month as sublease_rent_payment_month,
t.using_possible_id as sublease_using_possible_id,
t.report_id as sublease_report_id,	
	    m.*
        ,(CASE WHEN ar.agreement_state = 1 THEN 'Договір діє' ELSE CASE WHEN ar.agreement_state = 2 THEN 'Договір закінчився, але заборгованність не погашено' ELSE CASE WHEN ar.agreement_state = 3 THEN 'Договір закінчився, оренда продовжена іншим договором' ELSE '' END END END) AS 'agreement_active_s'
        ,an1.n_cost_narah
        ,an1.n_rent_rate
        ,an1.n_rent_rate_uah
        ,an1.n_cost_expert_total
        ,an1.n_cost_agreement
        ,an1.n_rent_square
        ,an2.cost_agreement as cost_agreement_max
        ,an2.cost_narah as cost_narah_max
        ,org.[contribution_rate] 
--        ,an1.n_cost_expert_1m

--		(select top 1 n.cost_narah from arenda_notes n where n.arenda_id = m.arenda_id order by n.modify_date desc) as n_cost_narah,
--		(select top 1 n.rent_rate from arenda_notes n where n.arenda_id = m.arenda_id order by n.modify_date desc) as n_rent_rate,
--		(select top 1 n.rent_rate_uah from arenda_notes n where n.arenda_id = m.arenda_id order by n.modify_date desc) as n_rent_rate_uah,
--		(select top 1 n.cost_expert_total from arenda_notes n where n.arenda_id = m.arenda_id order by n.modify_date desc) as n_cost_expert_total,
--		(select top 1 n.cost_agreement from arenda_notes n where n.arenda_id = m.arenda_id order by n.modify_date desc) as n_cost_agreement,
--		(select top 1 n.rent_square from arenda_notes n where n.arenda_id = m.arenda_id order by n.modify_date desc) as n_rent_square

--		(select cast(avg(n.cost_narah) as decimal(5,2)) from arenda_notes n where isnull(n.is_deleted,0)=0 and n.arenda_id = m.arenda_id ) as n_cost_narah,
--		(select sum(n.rent_rate) from arenda_notes n where isnull(n.is_deleted,0)=0 and n.arenda_id = m.arenda_id ) as n_rent_rate,
--		(select sum(n.rent_rate_uah) from arenda_notes n where isnull(n.is_deleted,0)=0 and n.arenda_id = m.arenda_id ) as n_rent_rate_uah,
--		(select sum(n.cost_expert_total) from arenda_notes n where isnull(n.is_deleted,0)=0 and n.arenda_id = m.arenda_id ) as n_cost_expert_total,
--		(select sum(n.cost_agreement) from arenda_notes n where isnull(n.is_deleted,0)=0 and n.arenda_id = m.arenda_id ) as n_cost_agreement,
--		(select sum(n.rent_square) from arenda_notes n where isnull(n.is_deleted,0)=0 and n.arenda_id = m.arenda_id ) as n_rent_square

,p.payment_narah
,p.last_year_saldo
,p.payment_received
,p.payment_nar_zvit
,p.old_debts_payed
,p.debt_total
,p.debt_zvit
,p.debt_3_month
,p.debt_12_month
,p.debt_3_years
,p.debt_over_3_years
,p.debt_v_mezhah_vitrat
,p.debt_spysano
,p.num_zahodiv_total
,p.num_zahodiv_zvit

,d.name as stanjuro

,ar.insurance_sum
,ar.insurance_start
,ar.insurance_end

,isnull(ddd.name, 'Невідомо') as sphera_dialnosti
,priznachennya = dc.doc_display_name

        FROM reports1nf_arenda_subleases t
		join view_arenda_Agreements m on m.arenda_id = t.arenda_id 
        join arenda ar on ar.id = m.arenda_id
        outer apply (select cast(avg(isnull(n.cost_narah,0)) as decimal(10,2)) as n_cost_narah, sum(isnull(n.rent_rate,0)) as n_rent_rate,sum(isnull(n.rent_rate_uah,0)) as n_rent_rate_uah,sum(isnull(n.cost_expert_total,0)) as n_cost_expert_total,sum(isnull(n.cost_agreement,0)) as n_cost_agreement,sum(isnull(n.rent_square,0)) as n_rent_square from arenda_notes n where m.arenda_id = n.arenda_id and isnull(n.is_deleted,0)=0 ) an1  
        outer apply (SELECT top 1 n.arenda_id, n.cost_agreement, n.cost_narah, n.payment_type_id 
               from [dbo].[arenda_notes] n where n.arenda_id = ar.id and isnull(n.is_deleted, 0) = 0 order by n.cost_agreement desc) an2
		join dbo.organizations org on m.org_balans_id = org.id
		outer apply (select top 1 * from arenda_payments where arenda_id = ar.id order by id desc) p 
		outer apply (select top 1 doc_display_name from view_arenda_link_2_decisions ld where ld.arenda_id = ar.id order by ld.link_id) dc 
		left join [dbo].[dict_otdel_gukv] d on org.otdel_gukv_id = d.id 
                    LEFT OUTER JOIN (select obp.org_id,occ.name from org_by_period obp
                                      join dict_rent_occupation occ on occ.id = obp.org_occupation_id
                                      where obp.period_id = (select top 1 id from dict_rent_period order by id desc)) DDD ON DDD.org_id = m.org_balans_id

        WHERE isnull(ar.is_deleted, 0) = 0 and 
 	    ((@p_dpz_filter = 0) OR (@p_dpz_filter <> 0 AND ar.id in (select b.id from dbo.reports1nf_arenda b where b.org_balans_id = ar.org_balans_id and ISNULL(b.is_deleted, 0) = 0 /*and b.agreement_state = 1*/ ) )) AND
        ((@p_com_filter = 0) OR (@p_com_filter <> 0 AND (m.balans_form_ownership_int IN (32,33,34) OR m.balans_org_ownership_int IN (32,33,34)))) AND
        (   (@p_rda_district_id = 0) OR
            (m.org_balans_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND m.org_balans_district_id = @p_rda_district_id) OR
            (m.org_giver_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND m.org_giver_district_id = @p_rda_district_id) OR
            (m.org_renter_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND m.org_renter_district_id = @p_rda_district_id))"
    OnSelecting="SqlDataSourceArendaObjects_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="1" Name="p_dpz_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_com_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceUsingPossible" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, left(full_name, 150) as name, rental_rate, 1 as ordrow FROM dict_rental_rate union select null, '<пусто>', null, 2 as ordrow ORDER BY ordrow, name">
</mini:ProfiledSqlDataSource>


<dx:ASPxGridView ID="PrimaryGridView" runat="server" 
    ClientInstanceName="PrimaryGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceArendaObjects" 
    KeyFieldName="arenda_id"
    Width="100%"
    OnCustomCallback="GridViewArendaObjects_CustomCallback"
    OnCustomFilterExpressionDisplayText="GridViewArendaObjects_CustomFilterExpressionDisplayText"
    OnCustomSummaryCalculate="GridViewArendaObjects_CustomSummaryCalculate"
    OnProcessColumnAutoFilter = "GridViewArendaObjects_ProcessColumnAutoFilter"
    OnCustomColumnSort="GridViewArendaObjects_CustomColumnSort" >

    <Columns>
        <dx:GridViewDataTextColumn FieldName="arenda_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="True" Caption="#" Width="50px">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowSubleaseCard(" + Eval("sublease_report_id") + "," + Eval("arenda_id") + ")\"><img border='0' src='../Styles/EditIcon.png'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10" Visible="False" Caption="ID Балансоутримувача">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_full_name" ReadOnly="True"
            VisibleIndex="20" Visible="True" Caption="Балансоутримувач - Повна Назва" Width="400px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_short_name" ReadOnly="True"
            VisibleIndex="30" Visible="False" 
            Caption="Балансоутримувач - Коротка Назва" Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_zkpo" ReadOnly="True"
            VisibleIndex="40" Visible="True" Caption="Балансоутримувач - Код ЄДРПОУ" Width="130px"></dx:GridViewDataTextColumn>

	<dx:GridViewDataTextColumn FieldName="agreement_num" ReadOnly="True"
            VisibleIndex="100" Visible="True" Caption="Номер договору оренди"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="agreement_date" ReadOnly="True"
            VisibleIndex="110" Visible="True" Caption="Дата укладання договору оренди"></dx:GridViewDataDateColumn>


		<dx:GridViewDataTextColumn FieldName="sublease_agreement_num" VisibleIndex="210" Caption="Номер договору суборендування" Width="150px"></dx:GridViewDataTextColumn>
		<dx:GridViewDataDateColumn FieldName="sublease_agreement_date" VisibleIndex="220" Caption="Дата укладання договору суборендування" Width="100px"></dx:GridViewDataDateColumn>
		<dx:GridViewDataDateColumn FieldName="sublease_rent_start_date" VisibleIndex="230" Caption="Початок оренди згідно з договором суборендування"></dx:GridViewDataDateColumn>
		<dx:GridViewDataDateColumn FieldName="sublease_rent_finish_date" VisibleIndex="240" Caption="Закінчення оренди згідно з договором суборендування"></dx:GridViewDataDateColumn>
		<dx:GridViewDataTextColumn FieldName="sublease_rent_square" VisibleIndex="250" Caption="Суборендована площа, кв.м."></dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="sublease_rent_payment_month" VisibleIndex="260" Caption="Суборендна плата, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
		<dx:GridViewDataComboBoxColumn FieldName="sublease_using_possible_id" VisibleIndex="270" Width = "320px" Visible="True" Caption="Можливе використання вільного приміщення">
			<HeaderStyle Wrap="True" />
			<PropertiesComboBox DataSourceID="SqlDataSourceUsingPossible" ValueField="id" TextField="name" ValueType="System.Int32" />
		</dx:GridViewDataComboBoxColumn>



        <dx:GridViewDataTextColumn FieldName="org_balans_industry" ReadOnly="True"
            VisibleIndex="1005" Visible="False" Caption="Балансоутримувач - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_occupation" ReadOnly="True"
            VisibleIndex="1006" Visible="False" 
            Caption="Балансоутримувач - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="1007" Visible="False" Caption="ID Орендаря">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_full_name" ReadOnly="True"
            VisibleIndex="1008" Visible="False" Caption="Орендар - Повна Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_short_name" ReadOnly="True"
            VisibleIndex="1009" Visible="False" Caption="Орендар - Коротка Назва" 
            Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_zkpo" ReadOnly="True"
            VisibleIndex="10010" Visible="False" Caption="Орендар - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_industry" ReadOnly="True"
            VisibleIndex="10011" Visible="False" Caption="Орендар - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_occupation" ReadOnly="True"
            VisibleIndex="10012" Visible="False" Caption="Орендар - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10013" Visible="False" Caption="ID Орендодавця">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_full_name" ReadOnly="True"
            VisibleIndex="10014" Visible="False" Caption="Орендодавець - Повна Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_short_name" ReadOnly="True"
            VisibleIndex="10015" Visible="False" Caption="Орендодавець - Коротка Назва" 
            Width="200px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_zkpo" ReadOnly="True"
            VisibleIndex="10016" Visible="False" Caption="Орендодавець - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_industry" ReadOnly="True"
            VisibleIndex="10017" Visible="False" Caption="Орендодавець - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_occupation" ReadOnly="True"
            VisibleIndex="10018" Visible="False" Caption="Орендодавець - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" ReadOnly="True"
            VisibleIndex="10019" Visible="False" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ReadOnly="True"
            VisibleIndex="10020" Visible="False" Caption="Назва Вулиці" Width="140px">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("street_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True"
            VisibleIndex="10021" Visible="False" Caption="Номер Будинку">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("addr_nomer") + "</a>"%>
            </DataItemTemplate>
            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="object_name" ReadOnly="True"
            VisibleIndex="10022" Visible="False" Caption="Використання згідно з договором"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_note" ReadOnly="True"
            VisibleIndex="10023" Visible="False" Caption="Розташування приміщення (поверх)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_group" ReadOnly="True"
            VisibleIndex="10024" Visible="False" Caption="Група Призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" ReadOnly="True"
            VisibleIndex="10025" Visible="False" Caption="Призначення"></dx:GridViewDataTextColumn>        
        <dx:GridViewDataTextColumn FieldName="is_privat" ReadOnly="True"
            VisibleIndex="10026" Visible="False" Caption="Приватизовано"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_kind" ReadOnly="True"
            VisibleIndex="10027" Visible="False" Caption="Тип Договору Оренди"></dx:GridViewDataTextColumn>    --%>
<%--         <dx:GridViewDataTextColumn FieldName="agreement_date_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10029" Visible="False" Caption="Дата Договору Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_date_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10030" Visible="False" Caption="Дата Договору Оренди - Квартал"></dx:GridViewDataTextColumn>     --%>
        
<%--        <dx:GridViewDataTextColumn FieldName="floor_number" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10032" Visible="False" Caption="Поверх"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_narah" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10033" Visible="False" Caption="Середня Ставка за використання (%)"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="priznachennya" VisibleIndex="10033" Caption="Призначення за Документом" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="cost_payed" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10034" Visible="False" Caption="Сплачена Вартість (грн.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="cost_debt" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10035" Visible="False" Caption="Борг (грн.)"></dx:GridViewDataTextColumn>       --%>
        <dx:GridViewDataTextColumn FieldName="n_cost_agreement" ReadOnly="True"
            VisibleIndex="10036" Visible="False" Caption="Плата за використання, грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_agreement_max" ReadOnly="True"
            VisibleIndex="10037" Visible="False" Caption="Максимальна Орендна Плата за об'єкт договору (грн.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="cost_narah_max" ReadOnly="True"
            VisibleIndex="10038" Visible="False" Caption="Ставка за використання (%) об'єкту з макс.орен.платою"></dx:GridViewDataTextColumn>
          <dx:GridViewDataTextColumn FieldName="n_cost_expert_1m" ReadOnly="True" 
            VisibleIndex="10037" Visible="False" Caption="Оціночна Вартість За Кв.м. (грн.)"></dx:GridViewDataTextColumn>   --%> 
        <dx:GridViewDataDateColumn FieldName="date_expert" ReadOnly="True"
            VisibleIndex="10039" Visible="False" Caption="Дата, на яку проведена оцінка об'єкту"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert_total" ReadOnly="True"
            VisibleIndex="10040" Visible="False" Caption="Оціночна вартість приміщень за договором, грн"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_cost_expert_total" ReadOnly="True"
            VisibleIndex="10041" Visible="False" Caption="Ринкова вартість приміщень, грн"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="debt_timespan" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10042" Visible="False" Caption="Час Заборгованості"></dx:GridViewDataTextColumn>      
        <dx:GridViewDataTextColumn FieldName="pidstava_display" ReadOnly="True"
            VisibleIndex="10043" Visible="False" Caption="Підстава"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataDateColumn FieldName="rent_start_date" ReadOnly="True" 
            VisibleIndex="10044" Visible="False" Caption="Початок Оренди"></dx:GridViewDataDateColumn>
<%--        <dx:GridViewDataTextColumn FieldName="rent_start_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10045" Visible="False" Caption="Початок Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_start_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10046" Visible="False" Caption="Початок Оренди - Квартал"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataDateColumn FieldName="rent_finish_date" ReadOnly="True"
            VisibleIndex="10047" Visible="False" Caption="Закінчення Оренди"></dx:GridViewDataDateColumn>
<%--        <dx:GridViewDataTextColumn FieldName="rent_finish_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10048" Visible="False" Caption="Закінчення Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_finish_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10049" Visible="False" Caption="Закінчення Оренди - Квартал"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataDateColumn FieldName="rent_actual_finish_date" ReadOnly="True"
            VisibleIndex="10050" Visible="False" Caption="Фактичне Закінчення Оренди"></dx:GridViewDataDateColumn>
<%--        <dx:GridViewDataTextColumn FieldName="actual_finish_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10051" Visible="False" Caption="Фактичне Закінчення Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="actual_finish_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10052" Visible="False" Caption="Фактичне Закінчення Оренди - Квартал"></dx:GridViewDataTextColumn>     --%>
<%--        <dx:GridViewDataTextColumn FieldName="rent_rate_percent" ReadOnly="True"
            VisibleIndex="10052" Visible="False" Caption="Ставка %"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_rate_uah" ReadOnly="True"
            VisibleIndex="10053" Visible="False" Caption="Ставка (грн.)"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataTextColumn FieldName="rent_square" ReadOnly="True" 
            VisibleIndex="10054" Visible="False" Caption="Площа (кв.м.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="num_akt" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10055" Visible="False" Caption="Номер Акту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="date_akt" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10056" Visible="False" Caption="Дата Акту"></dx:GridViewDataDateColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="is_subarenda" ReadOnly="True"
            VisibleIndex="10057" Visible="False" Caption="Суборенда"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_type" ReadOnly="True"
            VisibleIndex="10058" Visible="False" Caption="Вид Розрахунків"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_active_s" ReadOnly="True"
            VisibleIndex="10059" Visible="False" Caption="Стан договору"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_vedomstvo" ReadOnly="True"
            VisibleIndex="10060" Visible="False" Caption="Балансоутримувач - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_vedomstvo" ReadOnly="True"
            VisibleIndex="10061" Visible="False" Caption="Орендар - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_vedomstvo" ReadOnly="True" 
            VisibleIndex="10062" Visible="False" Caption="Орендодавець - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
 <%--       <dx:GridViewDataTextColumn FieldName="balans_sqr_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10063" Visible="False" Caption="Балансоутримувач - Загальна Площа На Балансі (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="balans_num_rent_agr" ReadOnly="True"
            VisibleIndex="10064" Visible="False" Caption="Балансоутримувач - Кількість Договорів Оренди"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="stanjuro" ReadOnly="True"
            VisibleIndex="10064" Visible="False" Caption="Балансоутримувач - стан юр. особи"></dx:GridViewDataTextColumn> 

        <dx:GridViewDataTextColumn FieldName="balans_sqr_in_rent" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="10065" Visible="False" Caption="Балансоутримувач - Загальна Площа Надана В Оренду (кв.м.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="balans_form_ownership" ReadOnly="True"
            VisibleIndex="10066" Visible="False" Caption="Цільове використання майна"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="org_balans_form_ownership" ReadOnly="True"
            VisibleIndex="10067" Visible="False" Caption="Балансоутримувач - Форма Власності"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="agreement_num_int" ReadOnly="True"
            VisibleIndex="10068" Visible="False" Caption="Номер Договору Оренди (число)"></dx:GridViewDataTextColumn>     
        <dx:GridViewDataTextColumn FieldName="is_in_privat" ReadOnly="True"
            VisibleIndex="10069" Visible="False" Caption="Будинок В Програмі Приватизації"></dx:GridViewDataTextColumn>      --%>
<%--        <dx:GridViewDataTextColumn FieldName="sqr_free_total" ReadOnly="True"
            VisibleIndex="10070" Visible="False" Caption="Вільні Приміщення: Загальна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_korysna" ReadOnly="True"
            VisibleIndex="10071" Visible="False" Caption="Вільні Приміщення: Корисна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_mzk" ReadOnly="True"
            VisibleIndex="10072" Visible="False" Caption="Вільні Приміщення: МЗК (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_floors" ReadOnly="True"
            VisibleIndex="10073" Visible="False" Caption="Місце Розташування Вільного Приміщення (поверх)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_purpose" ReadOnly="True"
            VisibleIndex="10074" Visible="False" Caption="Можливе Використання Вільного Приміщення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_form_of_ownership" ReadOnly="True"
            VisibleIndex="10075" Visible="False" Caption="Орендар - Форма Власності"></dx:GridViewDataTextColumn>     --%>

        <dx:GridViewDataTextColumn FieldName="org_balans_org_form" ReadOnly="True"
            VisibleIndex="10076" Visible="False" Caption="Балансоутримувач - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_org_form" ReadOnly="True"
            VisibleIndex="10077" Visible="False" Caption="Орендодавець - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_org_form" ReadOnly="True"
            VisibleIndex="10078" Visible="False" Caption="Орендар - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contribution_rate" ReadOnly="True"
            VisibleIndex="10079" Visible="False" Caption="Ставка відрахувань до бюджету (%)"></dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="modify_date" ReadOnly="True"
            VisibleIndex="10080" Visible="False" Caption="Дата Актуальності"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="payment_narah" ReadOnly="True"
            VisibleIndex="10081" Visible="False" Caption="Нараховано орендної плати за звітний період, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="last_year_saldo" ReadOnly="True"
            VisibleIndex="10082" Visible="False" Caption="Сальдо (переплата) на початок року (незмінна впродовж року величина), грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_received" ReadOnly="True"
            VisibleIndex="10083" Visible="False" Caption="Надходження орендної плати за звітний період, всього, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_nar_zvit" ReadOnly="True"
            VisibleIndex="10084" Visible="False" Caption="- у тому числі, з нарахованої за звітний період (без боргів та переплат)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_debts_payed" ReadOnly="True"
            VisibleIndex="10085" Visible="False" Caption="Погашення заборгованості минулих періодів, грн"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="debt_total" ReadOnly="True"
            VisibleIndex="10086" Visible="False" Caption="Загальна заборгованість по орендній платі - всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_zvit" ReadOnly="True"
            VisibleIndex="10087" Visible="False" Caption="Заборгованість по орендній платі за звітний період"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_month" ReadOnly="True"
            VisibleIndex="10088" Visible="False" Caption="Заборгованість по орендній платі поточна до 3-х місяців"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_12_month" ReadOnly="True"
            VisibleIndex="10089" Visible="False" Caption="Заборгованість по орендній платі прострочена від 4 до 12 місяців"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_years" ReadOnly="True"
            VisibleIndex="10090" Visible="False" Caption="Заборгованість по орендній платі прострочена від 1 до 3 років"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_over_3_years" ReadOnly="True"
            VisibleIndex="10091" Visible="False" Caption="Заборгованість по орендній платі безнадійна більше 3-х років"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_v_mezhah_vitrat" ReadOnly="True"
            VisibleIndex="10092" Visible="False" Caption="Заборгованість з орендної плати (із загальної заборгованості), розмір якої встановлено в межах витрат на утримання, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_spysano" ReadOnly="True"
            VisibleIndex="10093" Visible="False" Caption="Списано заборгованості з орендної плати у звітному періоді, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_total" ReadOnly="True"
            VisibleIndex="10094" Visible="False" Caption="Кількість заходів (попереджень, приписів і т.п.), всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_zvit" ReadOnly="True"
            VisibleIndex="10095" Visible="False" Caption="Кількість заходів (попереджень, приписів і т.п.), за звітний період"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="insurance_sum" ReadOnly="True"
            VisibleIndex="10096" Visible="False" Caption="Вартість об'єкту страхування, грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="insurance_start" ReadOnly="True"
            VisibleIndex="10097" Visible="False" Caption="Дата початку періоду страхування"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="insurance_end" ReadOnly="True"
            VisibleIndex="10098" Visible="False" Caption="Дата закінчення періоду страхування"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="sphera_dialnosti" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="10099" Visible="False" Caption="Сфера діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_form_of_ownership" ReadOnly="True"  ShowInCustomizationForm="True"
            VisibleIndex="100100" Visible="False" Caption="Орендар - Форма власності"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="n_cost_narah" ReadOnly="True" VisibleIndex="10078" Caption="(NEW) Орендна ставка (%)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_rent_rate" ReadOnly="True" VisibleIndex="10079" Caption="(NEW) Орендна ставка (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_cost_expert_total" ReadOnly="True" VisibleIndex="10081" Caption="(NEW) Експертна вартість (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_cost_agreement" ReadOnly="True" VisibleIndex="10082" Caption="(NEW) Орендна Плата (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_rent_square" ReadOnly="True" VisibleIndex="10083" Caption="(NEW) Площа за договором"></dx:GridViewDataTextColumn>--%>


    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="cost_agreement" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="cost_expert_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="balans_sqr_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="balans_num_rent_agr" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="balans_sqr_in_rent" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free_total" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free_korysna" SummaryType="Custom" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="sqr_free_mzk" SummaryType="Custom" DisplayFormat="{0}" />
    </TotalSummary>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
        <dx:ASPxSummaryItem FieldName="rent_square" SummaryType="Custom" DisplayFormat="Загальна Орендована Площа = {0} кв.м." />
    </GroupSummary>

    <SettingsBehavior EnableCustomizationWindow="True" 
        AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
    <SettingsPager AlwaysShowPager="true" PageSize="25"></SettingsPager>
    <SettingsPopup> <HeaderFilter Width="200" Height="300" /> </SettingsPopup>
    <Settings
        ShowFilterRow="True"
        ShowFilterRowMenu="True"
        ShowGroupPanel="True" 
        ShowFilterBar="Visible"
        ShowHeaderFilterButton="True" 
        HorizontalScrollBarMode="Visible"
        ShowFooter="True"
        VerticalScrollBarMode="Hidden"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.ArendaSubleases" Version="A2_33" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewArendaObjectsInit" EndCallback="GridViewArendaObjectsEndCallback" />
</dx:ASPxGridView>

</center>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
            <uc1:SaveReportCtrl ID="SaveReportCtrl2" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="PopupAddressPicker" runat="server" 
    HeaderText="Швидкий Пошук За Адресою" 
    ClientInstanceName="PopupAddressPicker" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
            <uc2:AddressPicker ID="AddressPicker1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="PopupFieldFixxer" runat="server" 
    HeaderText="Закріпити Колонки" 
    ClientInstanceName="PopupFieldFixxer" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
            <uc3:FieldFixxer ID="FieldFixxer1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns1.PerformCallback(); }" />
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="PopupFieldChooser" runat="server" 
    HeaderText="Додаткові Колонки" 
    ClientInstanceName="PopupFieldChooser" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>

