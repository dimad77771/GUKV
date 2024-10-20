﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RentAgreementsShow.aspx.cs" Inherits="Reports1NF_RentAgreementsShow"
    MasterPageFile="~/RentAgreementsShowPublic.master" Title="Договори Оренди" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function AdjustGridSizes() {
        //console.log("AdjustGridSizes", window.innerHeight - 180);
        //FreeSquareGridView.SetHeight(600);
        FreeSquareGridView.SetHeight(window.innerHeight - 180);
		//console.log("AdjustGridSizes2", FreeSquareGridView.GetHeight());
    }

	function GridViewArendaObjectsInit(s, e) {

		FreeSquareGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
	}

	function GridViewArendaObjectsEndCallback(s, e) {

		AdjustGridSizes();
	}


	function ShowPhoto(s, e) {
		if (e.buttonID == 'btnPhoto') {
			//console.log("$.cookie", $.cookie);
			//console.log("GetRowKey", s.GetRowKey(e.visibleIndex));
			$.cookie('RecordID', s.GetRowKey(e.visibleIndex));
			ASPxFileManagerPhotoFiles.Refresh();
			PopupObjectPhotos.Show();
		} else if (e.buttonID == 'btnMapShow') {
			FreeSquareGridView.GetRowValues(e.visibleIndex, 'id', OnMapShowGetRowValues);
		} else if (e.buttonID == 'btnJpegBuild') {
			FreeSquareGridView.GetRowValues(e.visibleIndex, 'arenda_id', OnGridJpegBuildGetRowValues);
		}
	}

	function OnGridJpegBuildGetRowValues(values) {
		var id = values;
		window.open(
			'OrgRentAgreementPhotosPdfRun.aspx?arid=' + id,
			'_blank',
		);
	}

	function OnMapShowGetRowValues(values) {
		var id = values;
		window.open(
			'Report1NFFreeMap.aspx?fs_id=' + id,
			'_blank',
		);
	}

	window.onload = function () {
		jQuery(document).ready(function () {
			setTimeout(function () {
				AdjustGridSizes();
				$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI0_").hide();
				$("#MainContent_ASPxPopupControlFreeSquare_ASPxFileManagerPhotoFiles_Splitter_Toolbar_DXI5_IS").hide();
			})
		});
	};


    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceArendaObjects" runat="server" EnableCaching="false"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT m.*
        ,(CASE WHEN ar.agreement_state = 1 THEN 'Договір діє' ELSE CASE WHEN ar.agreement_state = 2 THEN 'Договір закінчився, але заборгованність не погашено' ELSE CASE WHEN ar.agreement_state = 3 THEN 'Договір закінчився, оренда продовжена іншим договором' ELSE '' END END END) AS 'agreement_active_s'
        ,an1.n_cost_narah
        ,an1.n_rent_rate
        ,an1.n_rent_rate_uah
        ,an1.n_cost_expert_total
        ,cast(case when cast(an1.n_cost_agreement as decimal(18,2)) in (0, 1.2, 1, 0.08) then 0.08 else an1.n_cost_agreement end as decimal(18,2)) as n_cost_agreement2
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
,p.return_orend_payed
,p.return_all_orend_payed
,p.use_calc_debt
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
,p.avance_plat

,p.is_discount
,p.zvilneno_percent
,p.zvilneno_date1
,p.zvilneno_date2
,p.povidoleno1_date
,p.povidoleno1_num
,p.povidoleno2_date
,p.povidoleno2_num
,p.povidoleno3_date
,p.povidoleno3_num
,p.povidoleno4_date
,p.povidoleno4_num
,p.zvilbykmp_percent
,p.zvilbykmp_date1
,p.zvilbykmp_date2


,d.name as stanjuro

,ar.insurance_sum
,ar.insurance_start
,ar.insurance_end

,isnull(ddd.name, 'Невідомо') as sphera_dialnosti
,priznachennya = dc.doc_display_name
,case when exists (select 1 from reports1nf_arenda q where q.id = ar.id) then 1 else 0 end as ex_reports1nf_arenda 
,(select top 1 q.report_id from reports1nf_arenda q where q.id = ar.id) as arenda_report_id
,case when ar.id in (select b.id from dbo.reports1nf_arenda b where b.org_balans_id = ar.org_balans_id and ISNULL(b.is_deleted, 0) = 0 /*and b.agreement_state = 1*/ ) then 1 else 0 end as is_dpz_object 

        FROM view_arenda_agreements m  /*m_view_arenda_agreements m2 */
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

        WHERE 1=1

		and not exists ( select 1 from arenda_notes n where m.arenda_id = n.arenda_id and isnull(n.is_deleted,0)=0 and n.purpose_group_id = 31 ) 

		and d.name = 'ДІЮЧЕ'
        and m.modify_date >= '20200101'
		and (CASE WHEN ar.agreement_state = 1 THEN 'Договір діє' ELSE CASE WHEN ar.agreement_state = 2 THEN 'Договір закінчився, але заборгованність не погашено' ELSE CASE WHEN ar.agreement_state = 3 THEN 'Договір закінчився, оренда продовжена іншим договором' ELSE '' END END END)
				= 'Договір діє'
		and isnull(m.payment_type,'') <> 'БЕЗОПЛАТНЕ КОРИСТУВАННЯ'

    
        and isnull(ar.is_deleted, 0) = 0 and 
 	    ((@p_dpz_filter = 0) OR (@p_dpz_filter <> 0 AND ar.id in (select b.id from dbo.reports1nf_arenda b where b.org_balans_id = ar.org_balans_id and ISNULL(b.is_deleted, 0) = 0 /*and b.agreement_state = 1*/ ) )) AND
        ((@p_com_filter = 0) OR (@p_com_filter <> 0 AND (m.balans_form_ownership_int IN (32,33,34) OR m.balans_org_ownership_int IN (32,33,34)))) AND
        (   (@p_rda_district_id = 0) OR
            (m.org_balans_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND m.org_balans_district_id = @p_rda_district_id) OR
            (m.org_giver_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND m.org_giver_district_id = @p_rda_district_id) OR
            (m.org_renter_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND m.org_renter_district_id = @p_rda_district_id))
	order by case when org_balans_zkpo = '03366500' then 2 else 1 end
	"
    OnSelecting="SqlDataSourceArendaObjects_Selecting" >
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="1" Name="p_dpz_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_com_filter" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Перелік договорів оренди" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl_FreeSquare_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_FreeSquare_SaveAs" 
                PopupElementID="ASPxButton_FreeSquare_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_FreeSquare_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_FreeSquare_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_FreeSquare_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_FreeSquare_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_FreeSquare_SaveAs" runat="server" AutoPostBack="False" Visible="false"
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewFreeSquareExporter" runat="server" 
    FileName="ВільніПлощі" GridViewID="FreeSquareGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView ID="FreeSquareGridView" runat="server" 
    ClientInstanceName="FreeSquareGridView"
    AutoGenerateColumns="False"
    DataSourceID="SqlDataSourceArendaObjects" 
    KeyFieldName="arenda_id"
    Width="100%"
    OnCustomCallback="GridViewArendaObjects_CustomCallback"
    OnCustomFilterExpressionDisplayText="GridViewArendaObjects_CustomFilterExpressionDisplayText"
    OnCustomSummaryCalculate="GridViewArendaObjects_CustomSummaryCalculate"
    OnProcessColumnAutoFilter = "GridViewArendaObjects_ProcessColumnAutoFilter"
    OnCustomColumnSort="GridViewArendaObjects_CustomColumnSort" >
	<ClientSideEvents CustomButtonClick="ShowPhoto" />

    <Columns>
		<dx:GridViewCommandColumn Width="30px" ButtonType="Image" CellStyle-Wrap="True" FixedStyle="Left" CellStyle-CssClass="command-column-class" 
            ShowDeleteButton="False" ShowCancelButton="False" ShowUpdateButton="False" ShowEditButton="False" ShowNewButton="False" >
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="btnJpegBuild" Text="Фото/плани"> 
					<Image Url="~/Styles/PhotoIcon.png"/>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <CellStyle Wrap="False"></CellStyle>
        </dx:GridViewCommandColumn>

        <dx:GridViewDataTextColumn FieldName="arenda_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="0" Visible="false" Caption="Картка">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowArendaCardEx(" + Eval("ex_reports1nf_arenda") + "," + Eval("arenda_id") + "," + Eval("arenda_report_id") + ")\"><img border='0' src='../Styles/" + ((int)Eval("is_dpz_object") == 1 ? "EditIcon_green.png" : "EditIcon.png") + "'/></a></center>"%>
            </DataItemTemplate>
            <Settings ShowInFilterControl="False"/>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="1" Visible="False" Caption="ID Балансоутримувача">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_full_name" ReadOnly="True"
            VisibleIndex="2" Visible="true" Caption="Балансоутримувач - Повна Назва" Width="250px">
            <%--<DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_full_name") + "</a>"%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_short_name" ReadOnly="True"
            VisibleIndex="3" Visible="False" 
            Caption="Балансоутримувач - Коротка Назва" Width="250px">
            <%--<DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_balans_id") + ")\">" + Eval("org_balans_short_name") + "</a>"%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="org_balans_zkpo" ReadOnly="True"
            VisibleIndex="4" Visible="true" Caption="Балансоутримувач - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_industry" ReadOnly="True"
            VisibleIndex="5" Visible="False" Caption="Балансоутримувач - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_balans_occupation" ReadOnly="True"
            VisibleIndex="6" Visible="False" 
            Caption="Балансоутримувач - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="7" Visible="False" Caption="ID Орендаря">
<%--            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_id") + "</a>"%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_full_name" ReadOnly="True"
            VisibleIndex="8" Visible="False" Caption="Орендар - Повна Назва">
<%--            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_full_name") + "</a>"%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_short_name" ReadOnly="True"
            VisibleIndex="9" Visible="True" Caption="Орендар - Коротка Назва" 
            Width="250px">
            <%--<DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_renter_id") + ")\">" + Eval("org_renter_short_name") + "</a>"%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_zkpo" ReadOnly="True"
            VisibleIndex="10" Visible="true" Caption="Орендар - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_industry" ReadOnly="True"
            VisibleIndex="11" Visible="False" Caption="Орендар - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_occupation" ReadOnly="True"
            VisibleIndex="12" Visible="False" Caption="Орендар - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_id" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="13" Visible="False" Caption="ID Орендодавця">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_id") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_full_name" ReadOnly="True"
            VisibleIndex="14" Visible="False" Caption="Орендодавець - Повна Назва">
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_full_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_short_name" ReadOnly="True"
            VisibleIndex="15" Visible="True" Caption="Орендодавець - Коротка Назва" 
            Width="280px">
            <%--<DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("org_giver_id") + ")\">" + Eval("org_giver_short_name") + "</a>"%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_zkpo" ReadOnly="True"
            VisibleIndex="16" Visible="False" Caption="Орендодавець - Код ЄДРПОУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_industry" ReadOnly="True"
            VisibleIndex="17" Visible="False" Caption="Орендодавець - Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_occupation" ReadOnly="True"
            VisibleIndex="18" Visible="False" Caption="Орендодавець - Вид Діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="district" ReadOnly="True"
            VisibleIndex="19" Visible="False" Caption="Район"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="street_full_name" ReadOnly="True"
            VisibleIndex="20" Visible="True" Caption="Назва Вулиці" Width="140px">
            <%--<DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("street_full_name") + "</a>"%>
            </DataItemTemplate>--%>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True"
            VisibleIndex="21" Visible="True" Caption="Номер Будинку">
            <%--<DataItemTemplate>
                <%# "<a href=\"javascript:ShowObjectCard(" + Eval("balans_id") + "," + Eval("building_id") + ")\">" + Eval("addr_nomer") + "</a>"%>
            </DataItemTemplate>--%>
            <Settings SortMode="Custom" />
        </dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="object_name" ReadOnly="True"
            VisibleIndex="22" Visible="False" Caption="Використання згідно з договором"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="object_note" ReadOnly="True"
            VisibleIndex="23" Visible="True" Caption="Розташування приміщення (поверх)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose_group" ReadOnly="True"
            VisibleIndex="24" Visible="False" Caption="Група Призначення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="purpose" ReadOnly="True"
            VisibleIndex="25" Visible="False" Caption="Призначення"></dx:GridViewDataTextColumn>        
        <dx:GridViewDataTextColumn FieldName="is_privat" ReadOnly="True"
            VisibleIndex="26" Visible="False" Caption="Приватизовано"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_kind" ReadOnly="True"
            VisibleIndex="27" Visible="False" Caption="Тип Договору Оренди"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataDateColumn FieldName="agreement_date" ReadOnly="True"
            VisibleIndex="28" Visible="False" Caption="Дата укладання договору"></dx:GridViewDataDateColumn>
<%--         <dx:GridViewDataTextColumn FieldName="agreement_date_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="29" Visible="False" Caption="Дата Договору Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="agreement_date_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="30" Visible="False" Caption="Дата Договору Оренди - Квартал"></dx:GridViewDataTextColumn>     --%>
        <dx:GridViewDataTextColumn FieldName="agreement_num" ReadOnly="True"
            VisibleIndex="31" Visible="True" Caption="Номер Договору Оренди"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="floor_number" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="32" Visible="False" Caption="Поверх"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_narah" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="33" Visible="True" Caption="Середня Ставка за використання (%)"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="priznachennya" VisibleIndex="33" Caption="Призначення за Документом" ShowInCustomizationForm="True" Visible="False"><Settings AllowHeaderFilter="True" HeaderFilterMode="CheckedList" /></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="cost_payed" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="34" Visible="False" Caption="Сплачена Вартість (грн.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="cost_debt" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="35" Visible="False" Caption="Борг (грн.)"></dx:GridViewDataTextColumn>       --%>
        <dx:GridViewDataTextColumn FieldName="n_cost_agreement2" ReadOnly="True"
            VisibleIndex="36" Visible="True" Caption="Плата за використання, грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cost_agreement_max" ReadOnly="True"
            VisibleIndex="37" Visible="false" Caption="Максимальна Орендна Плата за об'єкт договору (грн.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="cost_narah_max" ReadOnly="True"
            VisibleIndex="38" Visible="True" Caption="Ставка за використання (%) об'єкту з макс.орен.платою"></dx:GridViewDataTextColumn>
          <dx:GridViewDataTextColumn FieldName="n_cost_expert_1m" ReadOnly="True" 
            VisibleIndex="37" Visible="True" Caption="Оціночна Вартість За Кв.м. (грн.)"></dx:GridViewDataTextColumn>   --%> 
        <dx:GridViewDataDateColumn FieldName="date_expert" ReadOnly="True"
            VisibleIndex="39" Visible="false" Caption="Дата, на яку проведена оцінка об'єкту"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="cost_expert_total" ReadOnly="True"
            VisibleIndex="40" Visible="false" Caption="Оціночна вартість приміщень за договором, грн"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_cost_expert_total" ReadOnly="True"
            VisibleIndex="41" Visible="false" Caption="Ринкова вартість приміщень, грн"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="debt_timespan" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="42" Visible="False" Caption="Час Заборгованості"></dx:GridViewDataTextColumn>      
        <dx:GridViewDataTextColumn FieldName="pidstava_display" ReadOnly="True"
            VisibleIndex="43" Visible="False" Caption="Підстава"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataDateColumn FieldName="rent_start_date" ReadOnly="True" 
            VisibleIndex="44" Visible="true" Caption="Початок Оренди"></dx:GridViewDataDateColumn>
<%--        <dx:GridViewDataTextColumn FieldName="rent_start_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="45" Visible="False" Caption="Початок Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_start_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="46" Visible="False" Caption="Початок Оренди - Квартал"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataDateColumn FieldName="rent_finish_date" ReadOnly="True"
            VisibleIndex="47" Visible="True" Caption="Закінчення Оренди"></dx:GridViewDataDateColumn>
<%--        <dx:GridViewDataTextColumn FieldName="rent_finish_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="48" Visible="False" Caption="Закінчення Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_finish_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="49" Visible="False" Caption="Закінчення Оренди - Квартал"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataDateColumn FieldName="rent_actual_finish_date" ReadOnly="True"
            VisibleIndex="50" Visible="False" Caption="Фактичне Закінчення Оренди"></dx:GridViewDataDateColumn>
<%--        <dx:GridViewDataTextColumn FieldName="actual_finish_year" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="51" Visible="False" Caption="Фактичне Закінчення Оренди - Рік"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="actual_finish_quarter" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="52" Visible="False" Caption="Фактичне Закінчення Оренди - Квартал"></dx:GridViewDataTextColumn>     --%>
<%--        <dx:GridViewDataTextColumn FieldName="rent_rate_percent" ReadOnly="True"
            VisibleIndex="52" Visible="False" Caption="Ставка %"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="rent_rate_uah" ReadOnly="True"
            VisibleIndex="53" Visible="False" Caption="Ставка (грн.)"></dx:GridViewDataTextColumn>    --%>
        <dx:GridViewDataTextColumn FieldName="rent_square" ReadOnly="True" 
            VisibleIndex="54" Visible="true" Caption="Площа (кв.м.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="num_akt" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="55" Visible="False" Caption="Номер Акту"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="date_akt" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="56" Visible="False" Caption="Дата Акту"></dx:GridViewDataDateColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="is_subarenda" ReadOnly="True"
            VisibleIndex="57" Visible="False" Caption="Суборенда"></dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="payment_type" ReadOnly="True"
            VisibleIndex="58" Visible="true" Caption="Вид Розрахунків"></dx:GridViewDataTextColumn>--%>
        <%--<dx:GridViewDataTextColumn FieldName="agreement_active_s" ReadOnly="True"
            VisibleIndex="59" Visible="true" Caption="Стан договору"></dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="org_balans_vedomstvo" ReadOnly="True"
            VisibleIndex="60" Visible="False" Caption="Балансоутримувач - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_vedomstvo" ReadOnly="True"
            VisibleIndex="61" Visible="False" Caption="Орендар - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_vedomstvo" ReadOnly="True" 
            VisibleIndex="62" Visible="False" Caption="Орендодавець - Орган Управління" Width="180px"></dx:GridViewDataTextColumn>
 <%--       <dx:GridViewDataTextColumn FieldName="balans_sqr_total" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="63" Visible="False" Caption="Балансоутримувач - Загальна Площа На Балансі (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="balans_num_rent_agr" ReadOnly="True"
            VisibleIndex="64" Visible="False" Caption="Балансоутримувач - Кількість Договорів Оренди"></dx:GridViewDataTextColumn>      --%>
        <%--<dx:GridViewDataTextColumn FieldName="stanjuro" ReadOnly="True"
            VisibleIndex="64" Visible="True" Caption="Балансоутримувач - стан юр. особи"></dx:GridViewDataTextColumn> --%>

        <dx:GridViewDataTextColumn FieldName="balans_sqr_in_rent" ReadOnly="True" ShowInCustomizationForm="False"
            VisibleIndex="65" Visible="False" Caption="Балансоутримувач - Загальна Площа Надана В Оренду (кв.м.)"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="balans_form_ownership" ReadOnly="True"
            VisibleIndex="66" Visible="False" Caption="Цільове використання майна"></dx:GridViewDataTextColumn>      --%>
        <dx:GridViewDataTextColumn FieldName="org_balans_form_ownership" ReadOnly="True"
            VisibleIndex="67" Visible="False" Caption="Балансоутримувач - Форма Власності"></dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="agreement_num_int" ReadOnly="True"
            VisibleIndex="68" Visible="False" Caption="Номер Договору Оренди (число)"></dx:GridViewDataTextColumn>     
        <dx:GridViewDataTextColumn FieldName="is_in_privat" ReadOnly="True"
            VisibleIndex="69" Visible="False" Caption="Будинок В Програмі Приватизації"></dx:GridViewDataTextColumn>      --%>
<%--        <dx:GridViewDataTextColumn FieldName="sqr_free_total" ReadOnly="True"
            VisibleIndex="70" Visible="False" Caption="Вільні Приміщення: Загальна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_korysna" ReadOnly="True"
            VisibleIndex="71" Visible="False" Caption="Вільні Приміщення: Корисна Площа (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sqr_free_mzk" ReadOnly="True"
            VisibleIndex="72" Visible="False" Caption="Вільні Приміщення: МЗК (кв.м.)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_floors" ReadOnly="True"
            VisibleIndex="73" Visible="False" Caption="Місце Розташування Вільного Приміщення (поверх)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="free_sqr_purpose" ReadOnly="True"
            VisibleIndex="74" Visible="False" Caption="Можливе Використання Вільного Приміщення"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_form_of_ownership" ReadOnly="True"
            VisibleIndex="75" Visible="False" Caption="Орендар - Форма Власності"></dx:GridViewDataTextColumn>     --%>

        <dx:GridViewDataTextColumn FieldName="org_balans_org_form" ReadOnly="True"
            VisibleIndex="76" Visible="False" Caption="Балансоутримувач - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_giver_org_form" ReadOnly="True"
            VisibleIndex="77" Visible="False" Caption="Орендодавець - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_org_form" ReadOnly="True"
            VisibleIndex="78" Visible="False" Caption="Орендар - Організаційно-правова Форма"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contribution_rate" ReadOnly="True"
            VisibleIndex="79" Visible="False" Caption="Ставка відрахувань до бюджету (%)"></dx:GridViewDataTextColumn>

        <%--<dx:GridViewDataDateColumn FieldName="modify_date" ReadOnly="True"
            VisibleIndex="80" Visible="true" Caption="Дата Актуальності"></dx:GridViewDataDateColumn>--%>
        <dx:GridViewDataTextColumn FieldName="payment_narah" ReadOnly="True"
            VisibleIndex="81" Visible="False" Caption="Нараховано орендної плати за звітний період, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="last_year_saldo" ReadOnly="True"
            VisibleIndex="82" Visible="False" Caption="Сальдо (переплата) на початок року (незмінна впродовж року величина), грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="avance_plat" ReadOnly="True"
            VisibleIndex="82" Visible="False" Caption="Авансова орендна плата, грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_received" ReadOnly="True"
            VisibleIndex="83" Visible="False" Caption="Надходження орендної плати за звітний період, всього, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="payment_nar_zvit" ReadOnly="True"
            VisibleIndex="84" Visible="False" Caption="- у тому числі, з нарахованої за звітний період (без боргів та переплат)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_debts_payed" ReadOnly="True"
            VisibleIndex="85" Visible="False" Caption="Погашення заборгованості минулих періодів, грн"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="return_orend_payed" ReadOnly="True"
            VisibleIndex="86" Visible="False" Caption="Переплата орендної плати всього, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="return_all_orend_payed" ReadOnly="True"
            VisibleIndex="86" Visible="False" Caption="Повернення переплати орендної плати всього за звітний період, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataCheckColumn FieldName="use_calc_debt" ReadOnly="True"
            VisibleIndex="86" Visible="False" Caption="Розраховувати заборгованість з орендної плати"></dx:GridViewDataCheckColumn>


        <dx:GridViewDataTextColumn FieldName="debt_total" ReadOnly="True"
            VisibleIndex="86" Visible="False" Caption="Загальна заборгованість по орендній платі - всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_zvit" ReadOnly="True"
            VisibleIndex="87" Visible="False" Caption="Заборгованість по орендній платі за звітний період"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_month" ReadOnly="True"
            VisibleIndex="88" Visible="False" Caption="Заборгованість по орендній платі поточна до 3-х місяців"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_12_month" ReadOnly="True"
            VisibleIndex="89" Visible="False" Caption="Заборгованість по орендній платі прострочена від 4 до 12 місяців"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_3_years" ReadOnly="True"
            VisibleIndex="90" Visible="False" Caption="Заборгованість по орендній платі прострочена від 1 до 3 років"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_over_3_years" ReadOnly="True"
            VisibleIndex="91" Visible="False" Caption="Заборгованість по орендній платі безнадійна більше 3-х років"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_v_mezhah_vitrat" ReadOnly="True"
            VisibleIndex="92" Visible="False" Caption="Заборгованість з орендної плати (із загальної заборгованості), розмір якої встановлено в межах витрат на утримання, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="debt_spysano" ReadOnly="True"
            VisibleIndex="93" Visible="False" Caption="Списано заборгованості з орендної плати у звітному періоді, грн. (без ПДВ)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_total" ReadOnly="True"
            VisibleIndex="94" Visible="False" Caption="Кількість заходів (попереджень, приписів і т.п.), всього"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="num_zahodiv_zvit" ReadOnly="True"
            VisibleIndex="95" Visible="False" Caption="Кількість заходів (попереджень, приписів і т.п.), за звітний період"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="insurance_sum" ReadOnly="True"
            VisibleIndex="96" Visible="False" Caption="Вартість об'єкту страхування, грн."></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="insurance_start" ReadOnly="True"
            VisibleIndex="97" Visible="False" Caption="Дата початку періоду страхування"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="insurance_end" ReadOnly="True"
            VisibleIndex="98" Visible="False" Caption="Дата закінчення періоду страхування"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="sphera_dialnosti" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="99" Visible="False" Caption="Сфера діяльності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_renter_form_of_ownership" ReadOnly="True"  ShowInCustomizationForm="True"
            VisibleIndex="100" Visible="False" Caption="Орендар - Форма власності"></dx:GridViewDataTextColumn>


        <dx:GridViewDataCheckColumn FieldName="is_discount" ReadOnly="True"
            VisibleIndex="110" Visible="False" Caption="Знижка"></dx:GridViewDataCheckColumn>
        <dx:GridViewDataTextColumn FieldName="zvilneno_percent" ReadOnly="True"
            VisibleIndex="111" Visible="False" Caption="Звільнено від сплати орендної плати на (%)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="zvilneno_date1" ReadOnly="True"
            VisibleIndex="112" Visible="False" Caption="Звільнено від сплати орендної плати на (з)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="zvilneno_date2" ReadOnly="True"
            VisibleIndex="113" Visible="False" Caption="Звільнено від сплати орендної плати на (по)"></dx:GridViewDataDateColumn>

        <dx:GridViewDataTextColumn FieldName="zvilbykmp_percent" ReadOnly="True"
            VisibleIndex="114" Visible="False" Caption="Звільнено від сплати згідно абзац 3 пункт 2 рішення КМР 253/9332 (%)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="zvilbykmp_date1" ReadOnly="True"
            VisibleIndex="115" Visible="False" Caption="Звільнено від сплати згідно абзац 3 пункт 2 рішення КМР 253/9332 (з)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="zvilbykmp_date2" ReadOnly="True"
            VisibleIndex="116" Visible="False" Caption="Звільнено від сплати згідно абзац 3 пункт 2 рішення КМР 253/9332 (по)"></dx:GridViewDataDateColumn>


        <dx:GridViewDataDateColumn FieldName="povidoleno1_date" ReadOnly="True"
            VisibleIndex="117" Visible="False" Caption="Повідомлення орендаря до балансоутримувача про неможлівість використання (дата)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno1_num" ReadOnly="True"
            VisibleIndex="118" Visible="False" Caption="Повідомлення орендаря до балансоутримувача про неможлівість використання (№)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="povidoleno2_date" ReadOnly="True"
            VisibleIndex="119" Visible="False" Caption="Повідомлення орендаря до орендодавця про неможлівість використання (дата)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno2_num" ReadOnly="True"
            VisibleIndex="120" Visible="False" Caption="Повідомлення орендаря до орендодавця про неможлівість використання (№)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="povidoleno3_date" ReadOnly="True"
            VisibleIndex="121" Visible="False" Caption="Повідомлення орендаря до балансоутримувача про намір використовувати об'єкт (дата)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno3_num" ReadOnly="True"
            VisibleIndex="122" Visible="False" Caption="Повідомлення орендаря до балансоутримувача про намір використовувати об'єкт (№)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="povidoleno4_date" ReadOnly="True"
            VisibleIndex="123" Visible="False" Caption="Повідомлення орендаря до орендодавця про намір використовувати об'єкт (дата)"></dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="povidoleno4_num" ReadOnly="True"
            VisibleIndex="124" Visible="False" Caption="Повідомлення орендаря до орендодавця про намір використовувати об'єкт (№)"></dx:GridViewDataTextColumn>


<%--        <dx:GridViewDataTextColumn FieldName="n_cost_narah" ReadOnly="True" VisibleIndex="78" Caption="(NEW) Орендна ставка (%)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_rent_rate" ReadOnly="True" VisibleIndex="79" Caption="(NEW) Орендна ставка (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_cost_expert_total" ReadOnly="True" VisibleIndex="81" Caption="(NEW) Експертна вартість (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_cost_agreement" ReadOnly="True" VisibleIndex="82" Caption="(NEW) Орендна Плата (грн)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="n_rent_square" ReadOnly="True" VisibleIndex="83" Caption="(NEW) Площа за договором"></dx:GridViewDataTextColumn>--%>


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
        VerticalScrollBarMode="Auto"
        VerticalScrollBarStyle="Standard" />
    <SettingsCookies CookiesID="GUKV.RentAgreementsShow" Version="A2_3" Enabled="false" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewArendaObjectsInit" EndCallback="GridViewArendaObjectsEndCallback" />
</dx:ASPxGridView>

<dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare" runat="server" AllowDragging="True" 
    ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True" 
    HeaderText="Фотографії об'єкту з вільним приміщенням" Modal="True" 
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
    PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

            <asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server" 
                SelectMethod="Select" 
                TypeName="ExtDataEntry.Models.FileAttachment">
                <SelectParameters>
                    <asp:Parameter DefaultValue="1NF" Name="scope" Type="String" />
                    <asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles" runat="server" 
                ClientInstanceName="ASPxFileManagerPhotoFiles" DataSourceID="ObjectDataSourcePhotoFiles">
                <Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
                <SettingsFileList>
                    <ThumbnailsViewSettings ThumbnailSize="180px" />
                </SettingsFileList>
                    <SettingsEditing AllowDelete="false" AllowCreate="false" AllowDownload="true" AllowMove="false" AllowRename="false" />
                    <SettingsFolders Visible="False" />
                    <SettingsToolbar ShowDownloadButton="true" ShowPath="true" ShowFilterBox="false" />
                    <SettingsUpload UseAdvancedUploadMode="True" Enabled="false" >
                        <AdvancedModeSettings EnableMultiSelect="True" />
                    </SettingsUpload>
<%--                <SettingsEditing AllowDelete="false" AllowCreate="false" AllowDownload="false" AllowMove="false" AllowRename="false" />
                <SettingsFolders Visible="False" />
                <SettingsToolbar ShowDownloadButton="false" ShowPath="False" />
                <SettingsUpload UseAdvancedUploadMode="True" Enabled="false">
                    <AdvancedModeSettings EnableMultiSelect="True" />
                </SettingsUpload>--%>

                <SettingsDataSource FileBinaryContentFieldName="Image" 
                    IsFolderFieldName="IsFolder" KeyFieldName="ID" 
                    LastWriteTimeFieldName="LastModified" NameFieldName="Name" 
                    ParentKeyFieldName="ParentID" />
            </dx:ASPxFileManager>

            <br />

            <dx:ASPxButton ID="ASPxButtonClose" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
                <ClientSideEvents Click="function(s, e) { PopupObjectPhotos.Hide(); }" />
            </dx:ASPxButton>

        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

</asp:Content>
