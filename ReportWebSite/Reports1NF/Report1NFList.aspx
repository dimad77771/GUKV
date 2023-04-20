<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report1NFList.aspx.cs" Inherits="Reports1NF_Report1NFList"
    MasterPageFile="~/NoHeader.master" Title="Перелік балансоутримувачів" %>

<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v20.1, Version=20.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Export" tagprefix="dx" %>
<%@ Register src="../UserControls/FieldChooser.ascx" tagname="FieldChooser" tagprefix="uc3" %>
<%@ Register Namespace="MiniProfilerHelpers" TagPrefix="mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<script type="text/javascript" src="../Scripts/PageScript.js"></script>

<script type="text/javascript" language="javascript">

    // <![CDATA[

    window.onresize = function () { AdjustGridSizes(); };

    function AdjustGridSizes() {

        PrimaryGridView.SetHeight(window.innerHeight - 140);
    }

    function GridViewReports1NFInit(s, e) {

        PrimaryGridView.PerformCallback("init:");
    }

    function GridViewReports1NFEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowReportCard(reportId) {

        var cardUrl = "../Reports1NF/Cabinet.aspx?rid=" + reportId;

        window.open(cardUrl);
	}

    function OnDropDown(comboBox){
		//_aspxMakeScollableArea(comboBox);
	}

    function _aspxMakeScollableArea(comboBox) {  
        var listBox = comboBox.GetListBoxControl();  
        if (_aspxIsExists(listBox)) {  
            var lsScrollableDiv =  listBox.GetScrollDivElement();  
            if (_aspxIsExists(lsScrollableDiv)) {  
				var browserWidth = (_aspxGetDocumentClientWidth() - 10) + 'px';
				//alert(browserWidth);
                _aspxSetAttribute(lsScrollableDiv.style, "width", browserWidth);  
				_aspxSetAttribute(lsScrollableDiv.style, "overflow-x", "scroll");  
				_aspxSetAttribute(lsScrollableDiv.style, "overflow-y", "scroll");
            }  
        }  
	}

	function ShowPhoto(s, e) {
		if (e.buttonID == 'bnt_show_photo') {
			$.cookie('RecordID', s.GetRowKey(e.visibleIndex));
			ASPxFileManagerPhotoFiles.Refresh();
			PopupObjectPhotos.Show();
        }
		else if (e.buttonID == 'bnt_show_inventar') {
			$.cookie('RecordID', s.GetRowKey(e.visibleIndex));
			ASPxFileManagerInventarFiles.Refresh();
			PopupObjectInventars.Show();
		}
	}


    // ]]>

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<mini:ProfiledSqlDataSource ID="SqlDataSourceDictStanRecieve" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT stan_recieve_id, stan_recieve_name, step_ord FROM dict_stan_recieve ORDER BY step_ord">
</mini:ProfiledSqlDataSource>


<mini:ProfiledSqlDataSource ID="SqlDataSourceReports" runat="server"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT 
        isnull(ddd.name, 'Невідомо') as 'dict_rent_occupation_name',
        (SELECT Q.stan_recieve_name FROM dict_stan_recieve Q where Q.stan_recieve_id = rep.stan_recieve_id) stan_recieve_name,
		rep.*,
		ar.NumOfSubmAgr, 
		ar.NumOfAgr, 
		obj.NumOfSubmObj, 
		obj.NumOfObj,
        (SELECT MAX(sdt) FROM (VALUES
        (rep.bal_max_submit_date),
        (rep.bal_del_max_submit_date),
        (rep.arenda_max_submit_date),
        (rep.arenda_rented_max_submit_date),
        (rep.org_max_submit_date)) AS AllMaxSubmitDates(sdt)) AS 'max_submit_date',
        CASE WHEN rep.is_reviewed = 0 THEN N'НI' ELSE N'ТАК' END AS 'review_performed'
        
        --,ReportDates.*
        
        ,ObjectAndRentTotals1.*
        ,ObjectAndRentTotals2.*
        ,ObjectAndRentTotals3.*
        ,ObjectAndRentTotals4.*
		,OrganizationProperties.*
		,RentPaymentProperties1.*
		,RentPaymentProperties2.*

        ,isnull(PAY_RECV_ZVIT,0) + isnull(PAY_RECV_OTHER,0) - isnull(PAY_RETURN_ALL_OREND_PAYED,0) as PAY_RECV_ZVIT_new
        
        FROM view_reports1nf rep
        LEFT JOIN (SELECT sum(CASE WHEN (r1a.submit_date IS NULL OR r1a.modify_date IS NULL OR r1a.modify_date > r1a.submit_date) THEN 0 ELSE 1 END) as NumOfSubmAgr, 
			Count(r1a.ID) AS NumOfAgr, report_id
        FROM reports1nf_arenda r1a LEFT JOIN arenda a
        ON r1a.id = a.id WHERE a.is_deleted IS NULL OR a.is_deleted = 0
        GROUP BY report_id) ar on rep.report_id = ar.report_id
        
        LEFT JOIN (SELECT sum(CASE WHEN (b.submit_date IS NULL OR b.modify_date IS NULL OR b.modify_date > b.submit_date) THEN 0 ELSE 1 END) AS NumOfSubmObj,
        Count(ID) AS NumOfObj,
        report_id
        FROM reports1nf_balans b
        WHERE is_deleted IS NULL OR is_deleted = 0
        GROUP BY report_id) obj on rep.report_id = obj.report_id
        
  --      LEFT JOIN (
		--SELECT 
	 --       (SELECT MAX(org.submit_date) FROM reports1nf_org_info org WHERE org.report_id = r.id) AS 'org_date',
	 --       (SELECT MAX(bal.submit_date) FROM reports1nf_balans bal WHERE bal.report_id = r.id AND (bal.is_deleted IS NULL OR bal.is_deleted = 0)) AS 'bal_date',
	 --       (SELECT MAX(bd.submit_date) FROM reports1nf_balans_deleted bd WHERE bd.report_id = r.id) AS 'bal_deleted_date',
	 --       (SELECT MAX(ar.submit_date) FROM reports1nf_arenda ar WHERE ar.report_id = r.id AND (ar.is_deleted IS NULL OR ar.is_deleted = 0)) AS 'arenda_date',
	 --       (SELECT MAX(arr.submit_date) FROM reports1nf_arenda_rented arr WHERE arr.report_id = r.id AND (arr.is_deleted IS NULL OR arr.is_deleted = 0)) AS 'arenda_rented_date',
	 --       id as report_id
  --      FROM reports1nf r GROUP BY r.id) ReportDates on ReportDates.report_id = rep.report_id
        
        LEFT JOIN (
        SELECT
             SUM(bal.sqr_total) AS 'SQR_TOTAL_BAL'
            ,SUM(bal.sqr_kor) AS 'SQR_KOR'
			,SUM(bal.sqr_vlas_potreb) AS 'SQR_VLAS_POTREB'
	        ,SUM(case when bal.sqr_vlas_potreb > 0 then 1 else 0 end) AS 'SQR_VLAS_POTREB_COUNT'
--            ,SUM(CASE WHEN bal.is_free_sqr = 1 THEN bal.free_sqr_useful ELSE 0 END) AS 'SQR_FREE'
--            ,SUM(bal.free_sqr_useful) AS 'SQR_FREE'
              ,sum(fs.sqr_free) AS 'SQR_FREE'
			  ,sum(fs.sqr_free_count) AS 'SQR_FREE_COUNT'
            ,COUNT(*) AS 'NUM_BALANS'
            ,bal.report_id
        FROM reports1nf_balans bal
        outer apply (select sum(fs.total_free_sqr) as sqr_free, count(*) as sqr_free_count from reports1nf_balans_free_square fs where fs.balans_id = bal.id and fs.report_id = bal.report_id and fs.is_included = 1 and fs.total_free_sqr > 0) fs  
        WHERE (bal.is_deleted IS NULL OR bal.is_deleted = 0)
        GROUP BY bal.report_id) ObjectAndRentTotals1 ON ObjectAndRentTotals1.report_id = rep.report_id
        
        LEFT JOIN (
        SELECT
            SUM(bal.sqr_total) AS 'SQR_VIDCH',
            COUNT(*) AS 'NUM_VIDCH',
            bal.report_id
            FROM reports1nf_balans_deleted bal
            WHERE 
            bal.sqr_total > 0 and
            (year(bal.vidch_doc_date) = @period_year or bal.vidch_doc_date is null)
            GROUP BY bal.report_id) ObjectAndRentTotals2 ON ObjectAndRentTotals2.report_id = rep.report_id
            
         LEFT JOIN (
         SELECT R.report_id, SUM(R.row_count) AS 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN', COUNT(distinct org_renter_id) AS 'NUM_RENTER'
--        SELECT R.report_id, SUM(R.row_count) AS 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN'
--         SELECT R.report_id, SUM(NUM_GIVEN)  as 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN'
         FROM (
         	SELECT 
		SUM(ar.rent_square) AS 'SQR_GIVEN'
           		,COUNT(ar.id) AS 'NUM_GIVEN'
           		,1 AS 'row_count'
            		,ar.report_id
		,ar.org_renter_id
		FROM reports1nf_arenda ar
		WHERE (ar.is_deleted IS NULL OR ar.is_deleted = 0)
			AND NOT EXISTS(SELECT id FROM arenda a WHERE a.id = ar.id AND ISNULL(a.is_deleted, 0) = 1)
			AND ar.agreement_state = 1
		GROUP BY ar.report_id,ar.id /*ar.agreement_num,ar.agreement_date,ar.rent_start_date,ar.rent_finish_date*/,ar.org_renter_id) R GROUP BY R.report_id /*, R.row_count*/) ObjectAndRentTotals3 ON ObjectAndRentTotals3.report_id = rep.report_id
--		GROUP BY ar.report_id ,ar.agreement_num /*,ar.agreement_date,ar.rent_start_date,ar.rent_finish_date,ar.org_giver_id*/) R GROUP BY R.report_id /*, R.row_count*/) ObjectAndRentTotals3 ON ObjectAndRentTotals3.report_id = rep.report_id
			
         LEFT JOIN (
	SELECT
             SUM(ar.rent_square) AS 'SQR_RENTED'
            ,COUNT(*) AS 'NUM_RENTED',
            ar.report_id
			FROM reports1nf_arenda_rented ar
			WHERE (ar.is_deleted IS NULL OR ar.is_deleted = 0)
			GROUP BY ar.report_id) ObjectAndRentTotals4 ON ObjectAndRentTotals4.report_id = rep.report_id
			
		LEFT JOIN (
		SELECT
            [org].[full_name] AS 'SUBMITTER_FULL_NAME' -- 0
           ,[org].[short_name] AS 'SUBMITTER_SHORT_NAME' -- 1
           ,[org].[zkpo_code] AS 'SUBMITTER_ZKPO'-- 2
           ,[dict_org_industry].[name] AS 'ORG_INDUSTRY' -- 3
           ,[dict_org_occupation].[name] AS 'ORG_OCCUPATION' -- 4
           ,[dict_org_status].[name] AS 'ORG_STATUS' -- 5
           ,[dict_org_form_gosp].[name] AS 'ORG_FIN_FORM' -- 6
           ,[dict_org_ownership].[name] AS 'ORG_OWNERSHIP' -- 7
           ,[dict_org_gosp_struct].[name] AS 'gosp_struct' -- 8
           ,[dict_org_vedomstvo].[name] AS 'ORG_VEDOMSTVO' -- 9
           ,[dict_org_form].[name] AS 'ORG_GOSP_FORM' -- 10
           --,[dict_org_gosp_struct_type].[name] AS 'gosp_struct_type' -- 11
           --,[dict_org_sfera_upr].[name] AS 'sfera_upr' -- 12
           ,[dict_org_old_organ].[name] AS 'ORG_GOSP_UPR' -- 13
           ,[director_fio] AS 'BOSS_FIO' -- 14
           ,[director_phone] AS 'BOSS_TEL' -- 15
           ,[director_email] AS 'BOSS_EMAIL'-- 16
           ,[director_title] AS 'BOSS_POSADA'-- 17
           ,[kved_code] AS 'ORG_KVED' -- 18
           --,jr_street.name AS 'addr_street' -- 19
           --,org.addr_nomer -- 20
           --,org.addr_misc -- 21
           ,ph_street.name AS 'physAddrStreet' -- 22
           ,org.phys_addr_nomer AS 'physAddrNumber' -- 23
           ,org.phys_addr_misc AS 'physAddrMisc'-- 24
           ,CASE WHEN org.contribution_rate IS NULL THEN 0 ELSE ORG.contribution_rate END AS 'ORG_CONTRIB_RATE' -- 25
           ,org.buhgalter_fio AS 'USER_FIO' -- 26
           ,org.buhgalter_phone AS 'USER_TEL' -- 27
           ,org.buhgalter_email AS 'USER_EMAIL' -- 28
           ,org.budget_narah_50_uah AS 'PAY_50_NARAH' -- 29

           --,org.budget_zvit_50_uah AS 'PAY_50_PAYED'-- 30
           --,dbo.[get_kazna_total](org.zkpo_code,null,null) AS 'PAY_50_PAYED'-- 30 
           ,kazna.pay_sum AS 'PAY_50_PAYED'

           ,org.budget_prev_50_uah AS 'PAY_50_DEBT'-- 31
           ,org.budget_debt_30_50_uah AS 'PAY_50_DEBT_OLD' -- 32
           ,org.payment_budget_special AS 'PAY_SPECIAL'-- 33
           --,org.konkurs_payments -- 34
           --,org.unknown_payments -- 35
           --,CASE WHEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0)) > 0 THEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0)) ELSE 0 END AS 'PAY_50_DEBT_CUR'
           ,CASE WHEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(kazna.pay_sum, 0) + ISNULL(org.budget_prev_50_uah, 0)) - ISNULL(org.unknown_payments,0) < 0 THEN 0 Else (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(kazna.pay_sum, 0) + ISNULL(org.budget_prev_50_uah, 0)) - ISNULL(org.unknown_payments,0) END AS 'PAY_50_DEBT_CUR'
           ,org.konkurs_payments AS 'PAY_RECV_OTHER'
           ,[org].[report_id]
           ,[dict_otdel_gukv].name as 'otdel_gukv'
           ,[org].[prim_balanc]
           ,org.unknown_payments AS 'PAY_UNKNOWN_PAYMENTS'

            ,org.planuvania_1
            ,org.planuvania_2
            ,org.planuvania_3
            ,org.planuvania_4
            ,org.planuvania_5

            ,[dbo].[get_conveyancingRequests_count]([org].[report_id]) AS conveyancingRequests_count

        FROM
            reports1nf_org_info org
            LEFT OUTER JOIN kazna_total_info(null, null) kazna on kazna.ident_bal_zkpo = org.zkpo_code
            LEFT OUTER JOIN dict_otdel_gukv ON org.otdel_gukv_id = dict_otdel_gukv.id
            LEFT OUTER JOIN dict_org_industry ON org.industry_id = dict_org_industry.id
            LEFT OUTER JOIN dict_org_occupation ON org.occupation_id = dict_org_occupation.id
            LEFT OUTER JOIN dict_org_status ON org.status_id = dict_org_status.id
            LEFT OUTER JOIN dict_org_form_gosp ON org.form_gosp_id = dict_org_form_gosp.id
            LEFT OUTER JOIN dict_org_ownership ON org.form_ownership_id = dict_org_ownership.id
            LEFT OUTER JOIN dict_org_gosp_struct ON org.gosp_struct_id = dict_org_gosp_struct.id
            LEFT OUTER JOIN dict_org_gosp_struct_type ON org.gosp_struct_type_id = dict_org_gosp_struct_type.id
            LEFT OUTER JOIN dict_org_vedomstvo ON org.vedomstvo_id = dict_org_vedomstvo.id
            LEFT OUTER JOIN dict_org_form ON org.form_id = dict_org_form.id
            LEFT OUTER JOIN dict_org_sfera_upr ON org.sfera_upr_id = dict_org_sfera_upr.id
            LEFT OUTER JOIN dict_org_old_organ ON org.old_organ_id = dict_org_old_organ.id
            LEFT OUTER JOIN dict_streets jr_street ON org.addr_street_id = jr_street.id
            LEFT OUTER JOIN dict_streets ph_street ON org.phys_addr_street_id = ph_street.id) OrganizationProperties ON OrganizationProperties.report_id = rep.report_id
            
		LEFT JOIN (SELECT report_id,MAX(rent_period_id) AS 'max_rent_period_id' FROM reports1nf_arenda_payments group by report_id) mrp on mrp.report_id = rep.report_id
            		
		LEFT JOIN (
		SELECT
                 --SUM(pay.sqr_total_rent) as 'sqr_total_rent' -- 0
                --,SUM(pay.sqr_payed_by_percent) as 'sqr_payed_by_percent' -- 1
                --,SUM(pay.sqr_payed_by_1uah) as 'sqr_payed_by_1uah' -- 2
                --,SUM(pay.sqr_payed_hourly) as 'sqr_payed_hourly' -- 3
                isnull(SUM(pay.payment_narah),0) - isnull(SUM(pay.znyato_nadmirno_narah),0) as 'PAY_NARAH_ZVIT' -- 4
                ,SUM(pay.last_year_saldo) as 'PAY_PEREPLATA' -- 5
                ,SUM(pay.zabezdepoz_prishlo) as 'PAY_ZABEZDEPOZ_PRISHLO' -- 5
                ,SUM(pay.payment_received) as 'PAY_RECV_ZVIT' -- 6
                ,SUM(pay.payment_nar_zvit) as 'PAY_RECV_NARAH' -- 7
                --,SUM(pay.payment_budget_special) as 'payment_budget_special' -- 8
                ,SUM(pay.debt_total) as 'PAY_DEBT_TOTAL' -- 9
                ,SUM(pay.debt_zvit) as 'PAY_DEBT_ZVIT' -- 10
                --,SUM(pay.debt_3_month) as 'debt_3_month' -- 11
                --,SUM(pay.debt_12_month) as 'debt_12_month' -- 12
                --,SUM(pay.debt_3_years) as 'debt_3_years' -- 13
                --,SUM(pay.debt_over_3_years) as 'debt_over_3_years' -- 14
                ,SUM(pay.debt_v_mezhah_vitrat) as 'PAY_DEBT_V_MEZH' -- 15
                ,SUM(pay.debt_spysano) as 'debt_spysano' -- 16
                --,SUM(budget_narah_50_uah) as 'budget_narah_50_uah' -- 17
                --,SUM(budget_zvit_50_uah) as 'budget_zvit_50_uah' -- 18
                --,SUM(budget_prev_50_uah) as 'budget_prev_50_uah' -- 19
                --,SUM(budget_debt_50_uah) as 'budget_debt_50_uah' -- 20
                --,SUM(budget_debt_30_50_uah) as 'budget_debt_30_50_uah' -- 21
                ,SUM(pay.old_debts_payed) as 'PAY_LAST_PER' -- 22
                ,SUM( isnull(pay.payment_narah,0) - isnull(pay.znyato_nadmirno_narah,0) ) as 'PAY_NARAH_ZVIT_NORMAL' -- 23
                ,SUM(pay.znyato_nadmirno_narah) as 'PAY_ZNYATO_NADMIRNO_NARAH' -- 24
                ,SUM(pay.zabezdepoz_saldo) as 'PAY_AVANCE_SALDO' -- 25
                ,SUM(pay.avance_plat) as 'PAY_AVANCE_PLAT' -- 26
                ,SUM(pay.total_pereplata) as 'PAY_PEREPLATA_ALL' -- 27
                ,SUM(pay.avance_debt) as 'PAY_AVANCE_DEBT' -- 28
                ,SUM(pay.avance_paymentnar) as 'PAY_AVANCE_PAYMENTNAR' -- 28
                ,SUM(pay.return_orend_payed) as 'PAY_RETURN_OREND_PAYED' -- 28
                ,SUM(pay.znyato_from_avance) as 'PAY_ZNYATO_FROM_AVANCE' -- 28
                ,SUM(pay.return_all_orend_payed) as 'PAY_RETURN_ALL_OREND_PAYED' -- 28
				,report_id
				,rent_period_id
            FROM reports1nf_arenda_payments pay
            WHERE NOT EXISTS(SELECT id FROM arenda a WHERE a.id = pay.arenda_id AND ISNULL(a.is_deleted, 0) = 1)  and pay.arenda_id > 0
            GROUP BY pay.report_id,pay.rent_period_id) RentPaymentProperties1 ON RentPaymentProperties1.report_id = rep.report_id and RentPaymentProperties1.rent_period_id = mrp.max_rent_period_id
            
        LEFT JOIN (
        SELECT
            SUM(pay.cmk_sqr_rented) as 'PAY_CMK_SQR'
            ,SUM(pay.cmk_payment_narah) as 'PAY_CMK_NARAH'
            ,SUM(pay.cmk_payment_to_budget) as 'PAY_CMK_BUDGET'
            ,SUM(pay.cmk_rent_debt) as 'PAY_CMK_DEBT'
            ,pay.report_id
        FROM reports1nf_arenda_rented pay
		WHERE pay.is_cmk > 0 and ISNULL(pay.is_deleted, 0) = 0
        GROUP BY pay.report_id) RentPaymentProperties2 ON RentPaymentProperties2.report_id = rep.report_id

		LEFT OUTER JOIN (
select obp.org_id,occ.name from org_by_period obp
join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
join dict_rent_occupation occ on occ.id = obp.org_occupation_id
		) DDD ON DDD.org_id = rep.organization_id


        WHERE 
            (@p_rda_district_id = 0 OR (rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = @p_rda_district_id))
                AND
            (@p_misto_id = 0 OR rep.old_organ_id = @p_misto_id)
				AND
			( (@smode = 0) OR (@smode = 1 and obj.NumOfObj > 0) OR (@smode = 2 and isnull(obj.NumOfObj,0) <= 0) )"
    OnSelecting="SqlDataSourceReports_Selecting"

UpdateCommand="UPDATE [reports1nf]
SET
    [stan_recieve_id] = @stan_recieve_id,
	[stan_recieve_date] = @stan_recieve_date,
	[stan_recieve_description] = @stan_recieve_description
WHERE id = @report_id" 
	onupdating="SqlDataSourceReports_Updating"
	
	>
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="period_year" />
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_misto_id" />
		<asp:Parameter DbType="Int32" DefaultValue="0" Name="smode" />
    </SelectParameters>



</mini:ProfiledSqlDataSource>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFAccounts.aspx" Text="Облікові Записи"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFNotifications.aspx" Text="Налаштування Повідомлень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/ConveyancingList.aspx" Text="Зміна балансоутримувачів об'єктів"></dx:MenuItem> 
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFFreeSquare.aspx" Text="Перелік вільних приміщень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFPrivatisatSquare.aspx" Text="Об'єкти приватизації"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFDogContinue.aspx" Text="Продовження договорів"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<dx:ASPxMenu ID="SectionMenuForRDARole" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
    <Items>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFList.aspx" Text="Звіти Балансоутримувачів"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFFreeSquare.aspx" Text="Перелік вільних приміщень"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFPrivatisatSquare.aspx" Text="Об'єкти приватизації"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Reports1NF/Report1NFDogContinue.aspx" Text="Продовження договорів"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<dx:ASPxMenu ID="SectionMenuForSmallMode" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
    <Items>
        <dx:MenuItem NavigateUrl="../Balans/BalansObjects.aspx" Text="Об'єкти на Балансі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Balans/BalansOrgList.aspx" Text="Перелік Балансоутримувачів" Selected="true"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Balans/BalansOther.aspx" Text="Інші Об'єкти"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../Balans/BalansInventarizations.aspx" Text="Об'єкти Інвентарізації"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="http://kmda.iisd.com.ua" Text="Класифікатор майна"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>


<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Перелік балансоутримувачів" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButtonEditColumnList" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="function (s,e) { PopupFieldChooser.Show(); }" />
            </dx:ASPxButton>
        </td>
        <td>
			<dx:ASPxPopupControl ID="ASPxPopupControlFreeSquare" runat="server" AllowDragging="True" 
				ClientInstanceName="PopupObjectPhotos" EnableClientSideAPI="True" 
				HeaderText="Документ" Modal="True" 
				PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
				PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">

						<asp:ObjectDataSource ID="ObjectDataSourcePhotoFiles" runat="server" 
							SelectMethod="Select" 
							DeleteMethod="Delete"
							TypeName="ExtDataEntry.Models.FileAttachment">
							<SelectParameters>
								<asp:Parameter DefaultValue="reports1nf_report_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
							</SelectParameters>
							<DeleteParameters>
								<asp:Parameter DefaultValue="reports1nf_report_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
								<asp:Parameter Name="id" Type="String" />
							</DeleteParameters>
						</asp:ObjectDataSource>

						<dx:ASPxFileManager ID="ASPxFileManagerPhotoFiles" runat="server" 
							ClientInstanceName="ASPxFileManagerPhotoFiles" DataSourceID="ObjectDataSourcePhotoFiles">
							<Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
<%--							<SettingsFileList>
								<ThumbnailsViewSettings ThumbnailSize="180px" />
							</SettingsFileList>--%>
                            <SettingsFileList View="Details">
                                <DetailsViewSettings AllowColumnResize="true" AllowColumnDragDrop="true" AllowColumnSort="true" ShowHeaderFilterButton="false"/>
                            </SettingsFileList>
							<SettingsEditing AllowDelete="true" AllowCreate="false" AllowDownload="true" AllowMove="false" AllowRename="false" />
							<SettingsFolders Visible="False" />
							<SettingsToolbar ShowDownloadButton="True" ShowPath="False" />
							<SettingsUpload UseAdvancedUploadMode="True" Enabled="false">
								<AdvancedModeSettings EnableMultiSelect="True" />
							</SettingsUpload>



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

			<dx:ASPxPopupControl ID="ASPxPopupControlInventar" runat="server" AllowDragging="True" 
				ClientInstanceName="PopupObjectInventars" EnableClientSideAPI="True" 
				HeaderText="Документ" Modal="True" 
				PopupHorizontalAlign="Center" PopupVerticalAlign="Middle"  
				PopupAction="None" PopupElementID="ASPxGridViewFreeSquare" Width="700px" >
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">

						<asp:ObjectDataSource ID="ObjectDataSourceInventarFiles" runat="server" 
							SelectMethod="Select" 
							DeleteMethod="Delete"
							TypeName="ExtDataEntry.Models.FileAttachment">
							<SelectParameters>
								<asp:Parameter DefaultValue="reports1nf_inventar_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
							</SelectParameters>
							<DeleteParameters>
								<asp:Parameter DefaultValue="reports1nf_inventar_documents" Name="scope" Type="String" />
								<asp:CookieParameter CookieName="RecordID" DefaultValue="" Name="recordID" Type="Int32" />
								<asp:Parameter Name="id" Type="String" />
							</DeleteParameters>
						</asp:ObjectDataSource>

						<dx:ASPxFileManager ID="ASPxFileManagerInventarFiles" runat="server" 
							ClientInstanceName="ASPxFileManagerInventarFiles" DataSourceID="ObjectDataSourceInventarFiles">
							<Settings RootFolder="~\" ThumbnailFolder="~\Thumb\" />
<%--							<SettingsFileList>
								<ThumbnailsViewSettings ThumbnailSize="180px" />
							</SettingsFileList>--%>
                            <SettingsFileList View="Details">
                                <DetailsViewSettings AllowColumnResize="true" AllowColumnDragDrop="true" AllowColumnSort="true" ShowHeaderFilterButton="false"/>
                            </SettingsFileList>
							<SettingsEditing AllowDelete="true" AllowCreate="false" AllowDownload="true" AllowMove="false" AllowRename="false" />
							<SettingsFolders Visible="False" />
							<SettingsToolbar ShowDownloadButton="True" ShowPath="False" />
							<SettingsUpload UseAdvancedUploadMode="True" Enabled="false">
								<AdvancedModeSettings EnableMultiSelect="True" />
							</SettingsUpload>



							<SettingsDataSource FileBinaryContentFieldName="Image" 
								IsFolderFieldName="IsFolder" KeyFieldName="ID" 
								LastWriteTimeFieldName="LastModified" NameFieldName="Name" 
								ParentKeyFieldName="ParentID" />
						</dx:ASPxFileManager>

						<br />

						<dx:ASPxButton ID="ASPxButtonClose2" runat="server" AutoPostBack="False" Text="Закрити" HorizontalAlign="Center">
							<ClientSideEvents Click="function(s, e) { PopupObjectInventars.Hide(); }" />
						</dx:ASPxButton>

					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>

            <dx:ASPxPopupControl ID="ASPxPopupControl_Reports1NF_SaveAs" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_Reports1NF_SaveAs" 
                PopupElementID="ASPxButton_Reports1NF_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxButton ID="ASPxButton_Reports1NF_ExportXLS" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_Reports1NF_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Reports1NF_ExportPDF" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_Reports1NF_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton_Reports1NF_ExportCSV" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_Reports1NF_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_Reports1NF_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="ASPxButtonZvedeniyBuild" runat="server" Text="Зведений Звіт" Width="148px" OnClick="ASPxButton_Zvedeniy_Build">
            </dx:ASPxButton>
        </td>
        <td>
            
            <dx:ASPxCallbackPanel ID="CPUpdatePlanuvania" ClientInstanceName="CPUpdatePlanuvania" runat="server" OnCallback="ASPxButton_UpdatePlanuvania"
                ClientSideEvents-EndCallback="function (s,e) { window.location.replace(window.location.href); }"    
                >
                
                <PanelCollection>
                    <dx:panelcontent ID="Panelcontent2" runat="server">
                        <dx:ASPxButton ID="ASPxButtonUpdatePlanuvania" runat="server" Text="Оновити прогнози" Width="148px">
                            <ClientSideEvents Click="function (s,e) { CPUpdatePlanuvania.PerformCallback(''); }" />
                        </dx:ASPxButton>
                    </dx:panelcontent>
                 </PanelCollection>
            </dx:ASPxCallbackPanel> 
        </td>

    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewReports1NFExporter" runat="server" 
    FileName="ПерелікЗвітів" GridViewID="PrimaryGridView" PaperKind="A4" 
    BottomMargin="20" LeftMargin="10" RightMargin="10" TopMargin="20">
    <Styles>
        <Default Font-Names="Calibri,Verdana,Sans Serif">
        </Default>
        <AlternatingRowCell BackColor="#E0E0E0">
        </AlternatingRowCell>
    </Styles>
</dx:ASPxGridViewExporter>

<dx:ASPxGridView
    ID="PrimaryGridView"
    ClientInstanceName="PrimaryGridView"
    runat="server"
    AutoGenerateColumns="False"
    Width="100%"
    DataSourceID="SqlDataSourceReports"
    KeyFieldName="report_id"
    OnCustomCallback="GridViewReports1NF_CustomCallback"
    OnCustomFilterExpressionDisplayText="GridViewReports1NF_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter="GridViewReports1NF_ProcessColumnAutoFilter" >
	<ClientSideEvents CustomButtonClick="ShowPhoto" />

	<SettingsCommandButton>
		<EditButton>
			<Image Url="~/Styles/EditIcon.png" />
		</EditButton>
		<CancelButton>
			<Image Url="~/Styles/CancelIcon.png" />
		</CancelButton>
		<UpdateButton>
			<Image Url="~/Styles/SaveIcon.png" />
		</UpdateButton>
		<DeleteButton>
			<Image Url="~/Styles/DeleteIcon.png" />
		</DeleteButton>
		<NewButton>
			<Image Url="~/Styles/AddIcon.png" />
		</NewButton>
		<ClearFilterButton Text="Очистити" RenderMode="Link" />
	</SettingsCommandButton>
	
    <Columns>
        <dx:GridViewDataTextColumn FieldName="report_id" ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="0" Width="45px" Caption="Картка Звіту">
            <DataItemTemplate>
                <%# "<center><a href=\"javascript:ShowReportCard(" + Eval("report_id") + ")\"><img border='0' src='../Styles/ReportDocument18.png'/></a></center>"%>
            </DataItemTemplate>
            <EditItemTemplate>
                <%# "<center><a class=\"editLabelFormStyle\" href=\"javascript:ShowReportCard(" + Eval("report_id") + ")\"><img border='0' src='../Styles/ReportDocument18.png'/></a></center>"%>
            </EditItemTemplate>
            <Settings ShowInFilterControl="False" AllowHeaderFilter="False" AllowAutoFilter="False" />
			<HeaderStyle Wrap="True" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="dict_rent_occupation_name" ReadOnly="True" ShowInCustomizationForm="True" Caption="Сфера діяльності">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("dict_rent_occupation_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="full_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Caption="Назва Організації" Width="300px">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("full_name") + "</a>"%>
            </DataItemTemplate>
            <EditItemTemplate>
                <%# "<a class=\"editLabelFormStyle\" href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("full_name") + "</a>"%>
            </EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="short_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Коротка Назва Організації" Width="300px" Visible="false">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("short_name") + "</a>"%>
            </DataItemTemplate>
            <EditItemTemplate>
                <%# "<a class=\"editLabelFormStyle\" href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("short_name") + "</a>"%>
            </EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Код ЄДРПОУ">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("zkpo_code") + "</a>"%>
            </DataItemTemplate>
            <EditItemTemplate>
                <%# "<a class=\"editLabelFormStyle\" href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("zkpo_code") + "</a>"%>
            </EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="cur_state" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Стан актуализации данных">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("cur_state") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="max_submit_date" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Дата актуализации данных">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("max_submit_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
            <Settings AllowHeaderFilter="True" />
        </dx:GridViewDataDateColumn>

        <dx:GridViewDataComboBoxColumn FieldName="stan_recieve_id" Caption="Поточний стан звіту" VisibleIndex="5" Width="300px">
            <PropertiesComboBox 
				DataSourceID="SqlDataSourceDictStanRecieve"
				DropDownStyle="DropDownList"
				DropDownWidth="200px"
				TextField="stan_recieve_name"  
				ValueField="stan_recieve_id">
				<ClientSideEvents DropDown="function(s, e) {
					OnDropDown(s);
				}" />
            </PropertiesComboBox>  
        </dx:GridViewDataComboBoxColumn>

        <dx:GridViewDataTextColumn FieldName="stan_recieve_name" ReadOnly="False" ShowInCustomizationForm="True" VisibleIndex="5" Visible="False" Caption="Поточний стан звіту(*)" Width="150px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="stan_recieve_description" ReadOnly="False" ShowInCustomizationForm="True" VisibleIndex="5" Visible="True" Caption="Примітки" Width="150px">
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="prim_balanc" ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="5" Visible="True" Caption="Примітки балансоутримувача" Width="120px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("prim_balanc") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="stan_recieve_date" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Дата останнього прийому">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("stan_recieve_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataDateColumn>

		
        <dx:GridViewCommandColumn Caption="#" VisibleIndex="5" Width="60px" ButtonType="Image" CellStyle-Wrap="False" 
            ShowCancelButton="true" ShowUpdateButton="true" ShowEditButton="true" >

            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="bnt_show_photo" Text="Звіт"> <Image Url="~/Styles/current_stage_pdf.png"> </Image>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <CellStyle Wrap="False"></CellStyle>
        </dx:GridViewCommandColumn>

        <dx:GridViewDataDateColumn FieldName="inventar_recieve_date" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Дата останнього прийому акту інвентаризації">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("inventar_recieve_date", "{0:dd.MM.yyyy}") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataDateColumn>

        <dx:GridViewCommandColumn Caption="#" VisibleIndex="5" Width="40px" ButtonType="Image" CellStyle-Wrap="False" 
            ShowCancelButton="false" ShowUpdateButton="false" ShowEditButton="false" >

            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="bnt_show_inventar" Text="Звіт"> <Image Url="~/Styles/inventar_pdf.png"> </Image>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
            <CellStyle Wrap="False"></CellStyle>
        </dx:GridViewCommandColumn>

        <%--<dx:GridViewDataTextColumn FieldName="review_performed" ReadOnly="True" ShowInCustomizationForm="True"
            VisibleIndex="6" Caption="Звіт Перевірено" />--%>

        <dx:GridViewDataTextColumn FieldName="addr_district" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="7" Visible="True" Caption="Район" Width="150px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("addr_district") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="8" Visible="True" Caption="Назва Вулиці" Width="200px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("addr_street_name") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="9" Visible="True" Caption="Номер Будинку">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("addr_nomer") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_korpus" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="10" Visible="False" Caption="Номер Будинку - Корпус">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("addr_korpus") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_zip_code" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="11" Visible="False" Caption="Поштовий Індекс">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("addr_zip_code") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="industry" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="12" Visible="False" Caption="Галузь">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("industry") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="occupation" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="13" Visible="False" Caption="Вид Діяльності" Width="120px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("occupation") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="13" Caption="Фін.звітн." Width="65px">
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a target=\"_blank\" href=\"https://balans.gukv.gov.ua/card/" + Eval("zkpo_code") + "\">" + "Фін.звітн." + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>

<%--        <dx:GridViewDataTextColumn FieldName="old_industry" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="14" Visible="False" Caption="Галузь (Баланс)">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("old_industry") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_occupation" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="15" Visible="False" Caption="Вид Діяльності (Баланс)" Width="120px">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("old_occupation") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="form_gosp" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="16" Visible="False" Caption="Форма фінансування">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("form_gosp") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_of_ownership" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="17" Visible="False" Caption="Форма Власності">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("form_of_ownership") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="gosp_struct" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="18" Visible="False" Caption="Госп. Структура">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("gosp_struct") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="vedomstvo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="19" Visible="False" Caption="Орган управління">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("vedomstvo") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_form" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="20" Visible="False" Caption="Орг.-правова форма госп.">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("org_form") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="old_organ" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="21" Visible="False" Caption="Орган госп. упр.">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("old_organ") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="status" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="22" Visible="False" Caption="Фіз. / Юр. Особа">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("status") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_fio" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="23" Visible="False" Caption="ФІО Директора">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("director_fio") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_title" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="24" Visible="False" Width="300" Caption="Відповідальна особа">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("director_title") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_fio" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="25" Visible="False" Caption="ФІО Бухгалтера">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("buhgalter_fio") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_phone" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="26" Visible="False" Caption="Тел. Бухгалтера">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("buhgalter_phone") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="fax" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="27" Visible="False" Caption="Факс">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("fax") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_auth" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="28" Visible="False" Caption="Реєстраційний Орган"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_num" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="29" Visible="False" Caption="Номер Запису про Реєстрацію"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_date" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="30" Visible="False" Caption="Дата Реєстрації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="registration_svidot" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="31" Visible="False" Caption="Номер Свідоцтва про Реєстрацію"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="kved_code" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="32" Visible="False" Caption="КВЕД"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="koatuu" ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="33" Visible="False" Caption="ОАТУУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="34" Visible="False" Caption="Стан юр.особи"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="mayno" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="35" Visible="False" Caption="Правовий режим майна"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="is_liquidated" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="36" Visible="False" Caption="Ліквідовано"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="liquidation_date" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="37" Visible="False" Caption="Дата Ліквідації"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_email" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="38" Visible="False" Caption="Ел. Адреса"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="contact_posada" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="39" Visible="False" Caption="Контактна Особа"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="sfera_upr" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="40" Visible="False" Caption="Сфера Управління"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="NumOfObj" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="41" Caption="Кількість об'єктів на балансі">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("NumOfObj") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
<%--        <dx:GridViewDataTextColumn FieldName="NumOfSubmObj" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="42" Caption="Кількість об'єктів, інформація за якими буде надіслана">
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("NumOfSubmObj") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <%--<dx:GridViewDataTextColumn FieldName="NumOfAgr" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="43" Caption="Кількість договорів" />--%>
<%--        <dx:GridViewDataTextColumn FieldName="NumOfSubmAgr" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="44" Caption="Кількість договорів, інформація за якими буде надіслана" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("NumOfSubmAgr") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>

        <dx:GridViewDataTextColumn Caption="Ставка відрахувань до бюджету (%)" FieldName="ORG_CONTRIB_RATE" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="45"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("ORG_CONTRIB_RATE") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SQR_TOTAL_BAL" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="46" Caption="Загальна площа, що знаходиться на балансі, кв.м." >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("SQR_TOTAL_BAL") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SQR_KOR" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="47" Caption="Корисна площа, що знаходиться на балансі, кв.м." >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("SQR_KOR") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="SQR_VLAS_POTREB" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="47" Caption="Загальна площа об'єктів для власних потреб, кв.м." >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("SQR_VLAS_POTREB") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

		<dx:GridViewDataTextColumn FieldName="SQR_VLAS_POTREB_COUNT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="47" Caption="Кількість об'єктів що мають площу для власних потреб" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("SQR_VLAS_POTREB_COUNT") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="SQR_VIDCH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="48" Caption="Загальна площа, що знята з балансу, кв.м." >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("SQR_VIDCH") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="NUM_VIDCH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="49" Caption="Кількість об'єктів знятих з балансу" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("NUM_VIDCH") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SQR_RENTED" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="50" Caption="Площа, що орендується, кв.м." >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("SQR_RENTED") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="SQR_GIVEN" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="51" Caption="Загальна площа, що надається в оренду, кв.м." >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("SQR_GIVEN") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="NUM_GIVEN" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="52" Caption="Кількість договорів оренди" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("NUM_GIVEN") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="NUM_RENTER" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="53" Caption="Кількість орендарів" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("NUM_RENTER") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="NUM_RENTED" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="54" Caption="Кількість договорів орендування" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("NUM_RENTED") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="SQR_FREE" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="55" Caption="Загальна вільна площа, що може бути надана в оренду, кв.м." >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("SQR_FREE") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="SQR_FREE_COUNT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="55" Caption="Кількість об'єктів що мають вільну площу" >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("SQR_FREE_COUNT") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
















    <%-- !!!!!! --%>

        <dx:GridViewDataTextColumn Caption="Авансова орендна плата (нараховано), грн." FieldName="PAY_AVANCE_PLAT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_AVANCE_PLAT") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Нараховано орендної плати за звітний період, грн. (без ПДВ)" FieldName="PAY_NARAH_ZVIT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_NARAH_ZVIT") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="- у тому числі, знято надмірно нарахованої за звітний період" FieldName="PAY_ZNYATO_NADMIRNO_NARAH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_ZNYATO_NADMIRNO_NARAH") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <%--<dx:GridViewDataTextColumn Caption="- у тому числі, використано авансової плати" FieldName="PAY_ZNYATO_FROM_AVANCE" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_ZNYATO_FROM_AVANCE") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>--%>

        <dx:GridViewDataTextColumn Caption="Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)" FieldName="PAY_PEREPLATA" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_PEREPLATA") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Надходження авансової орендної плати у звітному періоді, грн. (без ПДВ)" FieldName="PAY_ZABEZDEPOZ_PRISHLO" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_ZABEZDEPOZ_PRISHLO") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Сальдо авансової орендної плати на початок звітного періоду, грн. (без ПДВ)" FieldName="PAY_AVANCE_SALDO" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_AVANCE_SALDO") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn Caption="Надходження орендної плати за звітний період, всього, грн. (без ПДВ)" FieldName="PAY_RECV_ZVIT_new" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_RECV_ZVIT_new") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="- у тому числі, з нарахованої за звітний період (без боргів та переплат)" FieldName="PAY_RECV_NARAH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_RECV_NARAH") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="- у тому числі, з нарахованої авансової орендної плати, грн." FieldName="PAY_AVANCE_PAYMENTNAR" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_AVANCE_PAYMENTNAR") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="- у тому числі, погашення заборгованості минулих періодів, грн." FieldName="PAY_LAST_PER" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_LAST_PER") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="- у тому числі, переплата орендної плати за звітний період, грн." FieldName="PAY_RETURN_OREND_PAYED" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_RETURN_OREND_PAYED") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Переплата орендної плати на кінець звітного періоду, грн. (без ПДВ)" FieldName="PAY_PEREPLATA_ALL" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_PEREPLATA_ALL") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Повернення переплати орендної плати всього у звітному періоді, грн. (без ПДВ)" FieldName="PAY_RETURN_ALL_OREND_PAYED" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_RETURN_ALL_OREND_PAYED") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>


        

        

        









        


        <dx:GridViewDataTextColumn Caption="Нараховано орендної плати без урахування надмірно нарахованої плати, грн. (без ПДВ)" FieldName="PAY_NARAH_ZVIT_NORMAL" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_NARAH_ZVIT_NORMAL") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Заборгованість з нарахованої авансової орендної плати, грн. (без ПДВ)" FieldName="PAY_AVANCE_DEBT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="56"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_AVANCE_DEBT") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Отримано орендної плати в тому числі інші платежі" FieldName="PAY_RECV_OTHER" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="60"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_RECV_OTHER") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Загальна заборгованість по орендній платі, грн. (без ПДВ)" FieldName="PAY_DEBT_TOTAL" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="62"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_DEBT_TOTAL") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="- в тому числі заборгованість по орендній платі за звітний період,  грн. (без ПДВ)" FieldName="PAY_DEBT_ZVIT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="63"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_DEBT_ZVIT") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="- в тому числі (із загальної заборгованості) заборгованість з орендної плати, розмір якої встановлено в межах витрат на утримання, грн. (без ПДВ)" FieldName="PAY_DEBT_V_MEZH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="64"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_DEBT_V_MEZH") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        
        <dx:GridViewDataTextColumn Caption="Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період, грн. (без ПДВ)" FieldName="PAY_50_NARAH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="65"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_50_NARAH") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Сальдо платежів до бюджету (переплата на початок року), грн." FieldName="PAY_UNKNOWN_PAYMENTS" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="65"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_UNKNOWN_PAYMENTS") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn Caption="Перераховано коштів до бюджету, у звітному періоді ″КАЗНАЧЕЙСТВО″, грн. (без ПДВ)" FieldName="PAY_50_PAYED" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="66"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_50_PAYED") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки, грн. (без ПДВ)" FieldName="PAY_50_DEBT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="67"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_50_DEBT") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Заборгованість зі сплати % до бюджету від оренди майна за  звітний період, грн. (без ПДВ)" FieldName="PAY_50_DEBT_CUR" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="68"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_50_DEBT_CUR") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Заборгованість зі сплати % до бюджету від оренди майна минулих років, грн. (без ПДВ)" FieldName="PAY_50_DEBT_OLD" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="69"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_50_DEBT_OLD") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Списано заборгованості з орендної плати у звітному періоді, грн. (без ПДВ)" FieldName="debt_spysano" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="69" Visible="false"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("debt_spysano") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>


        <dx:GridViewDataTextColumn Caption="ЦМК Площа в оренді, кв.м." FieldName="PAY_CMK_SQR" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="70"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_CMK_SQR") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="ЦМК Нарахована орендна плата, грн. (без ПДВ)" FieldName="PAY_CMK_NARAH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="71"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_CMK_NARAH") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="ЦМК Перераховано до бюджету, грн. (без ПДВ)" FieldName="PAY_CMK_BUDGET" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="72"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_CMK_BUDGET") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="ЦМК Заборгованість по орендній платі, грн. (без ПДВ)" FieldName="PAY_CMK_DEBT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="73"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_CMK_DEBT") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')" FieldName="PAY_SPECIAL" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="74"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("PAY_SPECIAL") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn Caption="Прогнозовані надходження за місяць, грн." FieldName="planuvania_1" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="101"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("planuvania_1") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Прогнозовані надходження за рік, грн." FieldName="planuvania_2" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="101"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("planuvania_2") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Прогнозовані надходження  до бюджету за рік, грн." FieldName="planuvania_3" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="101"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("planuvania_3") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Планові надходження орендної плати за рік згідно фін. плану, грн." FieldName="planuvania_4" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="101"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("planuvania_4") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Планові надходження орендної плати до бюджету за рік згідно фін. плану, грн." FieldName="planuvania_5" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="101"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("planuvania_5") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Зміна балансоутримувача об'єктів" FieldName="conveyancingRequests_count" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="120"  >
			<EditItemTemplate>
				<dx:ASPxLabel runat="server" Text='<%# Eval("conveyancingRequests_count") %>' CssClass="editLabelFormStyle"></dx:ASPxLabel>
			</EditItemTemplate>
        </dx:GridViewDataTextColumn>

    </Columns>

    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="NumOfObj" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_TOTAL_BAL" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_KOR" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_VIDCH" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="NUM_VIDCH" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_RENTED" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_GIVEN" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="NUM_GIVEN" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="NUM_RENTER" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="NUM_RENTED" SummaryType="Sum" DisplayFormat="{0}" />
		<dx:ASPxSummaryItem FieldName="SQR_VLAS_POTREB" SummaryType="Sum" DisplayFormat="{0}" />
		<dx:ASPxSummaryItem FieldName="SQR_VLAS_POTREB_COUNT" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_FREE" SummaryType="Sum" DisplayFormat="{0}" />
		<dx:ASPxSummaryItem FieldName="SQR_FREE_COUNT" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_NARAH_ZVIT" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_NARAH_ZVIT_NORMAL" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_ZNYATO_NADMIRNO_NARAH" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_AVANCE_SALDO" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_AVANCE_PLAT" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_AVANCE_PAYMENTNAR" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_RETURN_OREND_PAYED" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_PEREPLATA_ALL" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_AVANCE_DEBT" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_ZNYATO_FROM_AVANCE" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_RETURN_ALL_OREND_PAYED" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_PEREPLATA" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_ZABEZDEPOZ_PRISHLO" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_RECV_ZVIT_new" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_RECV_NARAH" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_RECV_OTHER" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_LAST_PER" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_DEBT_TOTAL" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_DEBT_ZVIT" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_DEBT_V_MEZH" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_50_NARAH" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_50_PAYED" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_50_DEBT" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_50_DEBT_CUR" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_50_DEBT_OLD" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_CMK_SQR" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_CMK_NARAH" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_CMK_BUDGET" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_CMK_DEBT" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_SPECIAL" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_UNKNOWN_PAYMENTS" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="debt_spysano" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="planuvania_1" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="planuvania_2" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="planuvania_3" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="planuvania_4" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="planuvania_5" SummaryType="Sum" DisplayFormat="{0}" />
    </TotalSummary>

    <SettingsBehavior EnableCustomizationWindow="True" AutoFilterRowInputDelay="2500" ColumnResizeMode="Control" />
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
    <SettingsCookies CookiesID="GUKV.Reports1NF.ReportList" Version="A4_3" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewReports1NFInit" EndCallback="GridViewReports1NFEndCallback" />
</dx:ASPxGridView>

<dx:ASPxPopupControl ID="PopupFieldChooser" runat="server" 
    HeaderText="Додаткові Колонки" 
    ClientInstanceName="PopupFieldChooser" 
    PopupElementID="PrimaryGridView"
    PopupAction="None"
    PopupHorizontalAlign="Center"
    PopupVerticalAlign="Middle"
    PopupAnimationType="Slide" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>


