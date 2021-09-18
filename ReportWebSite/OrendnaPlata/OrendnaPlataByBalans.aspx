<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrendnaPlataByBalans.aspx.cs" Inherits="OrendnaPlata_OrendnaPlataByBalans"
    MasterPageFile="~/NoHeader.master" Title="Орендна Плата - Звіти Балансоутримувачів" %>

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

    function GridViewRentByBalansOrgInit(s, e) {

        PrimaryGridView.PerformCallback(AddWndHeightToCallbackParam("init:"));
    }

    function GridViewRentByBalansOrgEndCallback(s, e) {

        AdjustGridSizes();
    }

    function ShowFoldersPopupControl(s, e) {

        PopupControlFolders.Show();
    }

    function ShowFieldChooserPopupControl(s, e) {

        PopupFieldChooser.Show();
    }

// ]]>

</script>

<dx:ASPxMenu ID="SectionMenu" runat="server" Width="100%" ItemAutoWidth="False" ItemStyle-HorizontalAlign="Left" Visible="false">
    <Items>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrendnaPlataByBalans.aspx" Text="Орендна плата"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByBalans.aspx" Text="Балансоутримувачі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByRenters.aspx" Text="Орендарі"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataByBuildings.aspx" Text="Приміщення"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataTotals.aspx" Text="Зведений Звіт"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataNotSubmitted.aspx" Text="Організації, Що Не Подали Звіт"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataObjectsUse.aspx" Text="Використання Неж. Фонду"></dx:MenuItem>
        <dx:MenuItem NavigateUrl="../OrendnaPlata/OrPlataVipiski.aspx" Text="Бюджет"></dx:MenuItem>
    </Items>
</dx:ASPxMenu>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentByBalansOrg" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="select * from
        (SELECT 
        row_number() over (ORDER BY org.full_name, per.rent_period_id) as npp
        ,ddd.name as 'dict_rent_occupation_name'
		,per.rent_period_id
		,per.rent_period   
           ,org.id	as organization_id
           ,[org].[full_name]  -- 0
           ,[org].[short_name]  -- 1
           ,[org].[zkpo_code] -- 2
           ,org.addr_zip_code
           ,org.addr_city
			,[dict_districts2].[name] AS 'addr_district'
           ,org.addr_street_name
           ,org.addr_nomer
           ,org.addr_korpus
           
           ,[dict_org_industry].[name] AS 'INDUSTRY' -- 3
           ,[dict_org_occupation].[name] AS 'OCCUPATION' -- 4
           ,[dict_org_status].[name] AS 'STATUS' -- 5
           ,[dict_org_form_gosp].[name] AS 'org_form' -- 6
           ,[dict_org_ownership].[name] AS 'form_of_ownership' -- 7
           ,[dict_org_ownership].[id] AS 'org_form_ownership_id' 
           ,[dict_org_gosp_struct].[name] AS 'gosp_struct' -- 8
           ,[dict_org_vedomstvo].[name] AS 'vedomstvo' -- 9
           ,[dict_org_form].[name] AS 'form_gosp' -- 10
    		,dict_districts2.id as org_district_id
       
           --,[dict_org_gosp_struct_type].[name] AS 'gosp_struct_type' -- 11
           --,[dict_org_sfera_upr].[name] AS 'sfera_upr' -- 12
--           ,[dict_org_old_organ].[name] AS 'ORG_GOSP_UPR' -- 13
           ,org.director_fio  -- 14
           ,org.director_phone  -- 15
           ,org.contact_email -- 16
--           ,org.director_title AS 'BOSS_POSADA'-- 17
           ,org.kved_code  -- 18
           ,org.koatuu
			,org.sqr_on_balance
			,org.sqr_total
			,org.num_objects
			,org.is_liquidated
			,org.liquidation_date
			
--           ,ph_street.name AS 'physAddrStreet' -- 22
           --,org.phys_addr_nomer AS 'physAddrNumber' -- 23
           --,org.phys_addr_misc AS 'physAddrMisc'-- 24
--           ,CASE WHEN org.contribution_rate IS NULL THEN 0 ELSE ORG.contribution_rate END AS 'ORG_CONTRIB_RATE' -- 25
           ,org.buhgalter_fio  -- 26
           ,org.buhgalter_phone  -- 27
--           ,org.buhgalter_email AS 'USER_EMAIL' -- 28
           ,org.budget_narah_50_uah AS 'PAY_50_NARAH' -- 29
           ,org.budget_zvit_50_uah AS 'PAY_50_PAYED'-- 30
           ,org.budget_prev_50_uah AS 'PAY_50_DEBT'-- 31
           ,org.budget_debt_30_50_uah AS 'PAY_50_DEBT_OLD' -- 32
           ,org.payment_budget_special AS 'PAY_SPECIAL'-- 33
           --,org.konkurs_payments -- 34
           --,org.unknown_payments -- 35
           ,CASE WHEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0)) > 0 THEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0)) ELSE 0 END AS 'PAY_50_DEBT_CUR'
           ,ISNULL(org.konkurs_payments, 0) + ISNULL(org.unknown_payments, 0) AS 'PAY_RECV_OTHER'
           ,[dict_otdel_gukv].name as 'otdel_gukv'
			,[dict_org_mayno].[name] AS 'mayno'
           
		--,dpt.name AS 'payment_type'
		--	  ,[org_renter].[short_name] AS 'org_renter_short_name'
		--	  ,[org_renter].[zkpo_code] AS 'org_renter_zkpo'
		--	  ,[org_giver].[short_name] AS 'org_giver_short_name'
		--	  ,[org_giver].[zkpo_code] AS 'org_giver_zkpo'
		--,arn.rent_start_date
		--,arn.rent_finish_date
		--,  purpose = (SELECT top 1 purpose_str FROM arenda_notes where arenda_id = arn.id and isnull(is_deleted, 0) = 0 and purpose_str is not null order by id)

		,ar.*
		,ap.*
		,obj.* 

--		,(SELECT MAX(sdt) FROM (VALUES (rep.bal_max_submit_date), (rep.bal_del_max_submit_date), (rep.arenda_max_submit_date), (rep.arenda_rented_max_submit_date) ,(rep.org_max_submit_date)) AS AllMaxSubmitDates(sdt)) AS 'max_submit_date'
--        ,CASE WHEN rep.is_reviewed = 0 THEN N'НI' ELSE N'ТАК' END AS 'review_performed'
        
        ,objvid.*
        ,ObjectAndRentTotals3.*
        ,ObjectAndRentTotals4.*
        ,RentPaymentProperties2.*
        
		FROM dbo.organizations org

		join  (select distinct bal.organization_id, bal.rent_period_id, per.name as rent_period from arch_balans_m bal
				join [dbo].[dict_rent_period] per on bal.rent_period_id = per.id
				--where bal.rent_period_id > 0 and bal.archive_id = (select top 1 t.archive_id from arch_balans t where bal.organization_id = t.organization_id and bal.rent_period_id = t.rent_period_id order by t.modify_date desc)
				) per on org.id = per.organization_id
        
        LEFT JOIN (SELECT 	
			 org_balans_id 
			,Count(a.ID) AS NumOfAgr
--            ,SUM(a.rent_square) AS 'SQR_GIVEN'
			,a.rent_period_id as ar_period_id
        FROM arch_arenda_m a
        GROUP BY org_balans_id, rent_period_id) ar on org.id = ar.org_balans_id and ar.ar_period_id = per.rent_period_id
 
        LEFT JOIN (SELECT 
        arn.org_balans_id as org_id1
        ,arn.rent_period_id as pay_period_id
        ,count(ap.id) as NumOfAgrbyPeriod
		,sum(ap.sqr_total_rent) sqr_total_rent	
		,sum(ap.sqr_payed_by_percent) sqr_payed_by_percent	
		,sum(ap.sqr_payed_by_1uah) sqr_payed_by_1uah	
		,sum(ap.sqr_payed_hourly) sqr_payed_hourly	
		,sum(ap.payment_narah) PAY_NARAH_ZVIT	
		,sum(ap.last_year_saldo) last_year_saldo	
		,sum(ap.payment_received) PAY_RECV_ZVIT	
		,sum(ap.payment_nar_zvit) PAY_RECV_NARAH
		,sum(ap.debt_total) PAY_DEBT_TOTAL	
		,sum(ap.debt_zvit) PAY_DEBT_ZVIT	
		,sum(ap.debt_3_month) debt_3_month	
		,sum(ap.debt_12_month) debt_12_month	
		,sum(ap.debt_3_years) debt_3_years	
		,sum(ap.debt_over_3_years) debt_over_3_years	
		,sum(ap.debt_v_mezhah_vitrat) PAY_DEBT_V_MEZH	
		,sum(ap.debt_spysano) debt_spysano	
		,sum(ap.num_zahodiv_total) num_zahodiv_total	
		,sum(ap.num_zahodiv_zvit) num_zahodiv_zvit	
		,sum(ap.num_pozov_total) num_pozov_total	
		,sum(ap.num_pozov_zvit) num_pozov_zvit	
		,sum(ap.num_pozov_zadov_total) num_pozov_zadov_total	
		,sum(ap.num_pozov_zadov_zvit) num_pozov_zadov_zvit	
		,sum(ap.num_pozov_vikon_total) num_pozov_vikon_total	
		,sum(ap.num_pozov_vikon_zvit) num_pozov_vikon_zvit	
		,sum(ap.debt_pogasheno_total) debt_pogasheno_total	
		,sum(ap.debt_pogasheno_zvit) debt_pogasheno_zvit
		,sum(ap.old_debts_payed) PAY_LAST_PER
--		, case when ap.rent_period_id is null then '*'  else '' end as notpayed
		FROM  dbo.arch_arenda_m arn 
				join dbo.arenda_payments ap on ap.arenda_id = arn.id and arn.rent_period_id = ap.rent_period_id and ap.id = (select top 1 id from dbo.arenda_payments t where t.arenda_id = ap.arenda_id and t.rent_period_id = ap.rent_period_id order by t.modify_date desc)
		GROUP BY arn.org_balans_id, arn.rent_period_id) ap on org.id = ap.org_id1 and per.rent_period_id = ap.pay_period_id

        LEFT JOIN (select 
						obj_org_id
						,period_id1 = period_id
						,COUNT(*) AS NumOfObj 
						,SUM(sqr_total) AS 'SQR_TOTAL_BAL'
						,SUM(sqr_kor) AS 'SQR_KOR'
						,SUM(free_sqr_useful) AS 'SQR_FREE'
					from
			(SELECT organization_id as obj_org_id
             ,bal.id
            ,bal.sqr_total
            ,bal.sqr_kor
            ,bal.free_sqr_useful
			--,period_id = rp.id
			,period_id = bal.rent_period_id

	        FROM arch_balans_m bal
		    WHERE isnull(bal.is_deleted, 0) = 0
			) z
			GROUP BY obj_org_id, period_id ) obj on org.id = obj.obj_org_id and per.rent_period_id = obj.period_id1

        LEFT JOIN (select 
				vid_org_id
				,period_id2 = period_id
				,COUNT(*) AS NUM_VIDCH 
				,SUM(sqr_total) AS 'SQR_VIDCH'
					from
			(SELECT organization_id as vid_org_id
			,bal.sqr_total
			,period_id = bal.rent_period_id
	        FROM arch_balans_m bal
		    WHERE bal.is_deleted > 0
			and bal.sqr_total > 0 
			and bal.vidch_doc_date is not null) z
			GROUP BY vid_org_id, period_id ) objvid on org.id = objvid.vid_org_id and per.rent_period_id = objvid.period_id2

         LEFT JOIN (select
			org_id2
			,period_id21 = period_id
	        ,COUNT(*) AS 'NUM_GIVEN'
            ,SUM(rent_square) AS 'SQR_GIVEN'
			from
		    (SELECT 
			sum(ar.rent_square) rent_square
			,ar.org_balans_id as org_id2
   			--,period_id = isnull(rp.id , 0)
   			,period_id = ar.rent_period_id
   			,ar.id
			FROM arch_arenda_m ar
			--WHERE 
			--AND ar.agreement_state = 1
			--and exists (SELECT id FROM reports1nf_arenda WHERE id = ar.id )	
			GROUP BY ar.org_balans_id, ar.rent_period_id, ar.id) z
			GROUP BY org_id2, period_id) ObjectAndRentTotals3 ON org.id = ObjectAndRentTotals3.org_id2 and per.rent_period_id = ObjectAndRentTotals3.period_id21
		
		LEFT JOIN (select
			org_id3
			,period_id3 = period_id
	        ,COUNT(*) AS 'NUM_RENTED'
            ,SUM(rent_square) AS 'SQR_RENTED'
			from
		(SELECT arr.org_renter_id as org_id3
            ,arr.rent_square
   			,period_id = arr.rent_period_id
			FROM arch_arenda_rented arr
			WHERE isnull(arr.is_deleted, 0) = 0 
			and arr.rent_period_id > 0 
			and arr.archive_entry_id = (select top 1 t.archive_entry_id from arch_arenda_rented t where arr.id = t.id and arr.agreement_num = t.agreement_num and arr.rent_period_id = t.rent_period_id order by t.modify_date desc)
			) z
			GROUP BY org_id3, period_id) ObjectAndRentTotals4 ON org.id = ObjectAndRentTotals4.org_id3 and per.rent_period_id = ObjectAndRentTotals4.period_id3

		LEFT JOIN (select
			 org_id4
			,period_id4 = period_id
	        ,COUNT(*) AS 'NUM_CMK'
            ,SUM(cmk_sqr_rented) as 'PAY_CMK_SQR'
            ,SUM(cmk_payment_narah) as 'PAY_CMK_NARAH'
            ,SUM(cmk_payment_to_budget) as 'PAY_CMK_BUDGET'
            ,SUM(cmk_rent_debt) as 'PAY_CMK_DEBT'
			from
		(SELECT arr.org_renter_id as org_id4
            ,arr.cmk_sqr_rented
            ,arr.cmk_payment_narah
            ,arr.cmk_payment_to_budget
            ,arr.cmk_rent_debt
			,period_id = arr.rent_period_id
			FROM arch_arenda_rented arr
			WHERE arr.is_cmk > 0 
			and arr.rent_period_id > 0 
			and arr.archive_entry_id = (select top 1 t.archive_entry_id from arch_arenda_rented t where arr.id = t.id and arr.agreement_num = t.agreement_num and arr.rent_period_id = t.rent_period_id order by t.modify_date desc)
			) z

			GROUP BY org_id4, period_id) RentPaymentProperties2 ON org.id = RentPaymentProperties2.org_id4 and per.rent_period_id = RentPaymentProperties2.period_id4

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
--		LEFT OUTER JOIN dict_org_old_organ ON org.old_organ_id = dict_org_old_organ.id
--		LEFT OUTER JOIN dict_streets jr_street ON org.addr_street_id = jr_street.id
--		LEFT OUTER JOIN dict_streets ph_street ON org.phys_addr_street_id = ph_street.id
		LEFT OUTER JOIN dict_districts2 ON org.addr_distr_new_id = dict_districts2.id
	    LEFT OUTER JOIN dict_org_mayno ON org.mayno_id = dict_org_mayno.id

		LEFT OUTER JOIN (
			select obp.org_id
			, occ.name
			, occ.id
			, obp.period_id as obp_period_id
			from org_by_period obp
			join dict_rent_occupation occ on occ.id = obp.org_occupation_id
				) DDD ON DDD.org_id = org.id and per.rent_period_id = DDD.obp_period_id
            
    where  isnull(NumOfAgrbyPeriod,0) + isnull(NUM_VIDCH,0) + isnull(NUM_GIVEN,0) + isnull(NUM_RENTED,0) + isnull(NUM_CMK,0) > 0 
          ) t
    WHERE (@p_rda_district_id = 0 OR (org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND org_district_id = @p_rda_district_id))
        order by full_name, rent_period_id"
    OnSelecting="SqlDataSourceRentByBalansOrg_Selecting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" DefaultValue="0" Name="p_rda_district_id" />
    </SelectParameters>
</mini:ProfiledSqlDataSource>

<mini:ProfiledSqlDataSource ID="SqlDataSourceRentPeriods" runat="server" EnableCaching="true"
    ConnectionString="<%$ ConnectionStrings:GUKVConnectionString %>" 
    SelectCommand="SELECT id, name FROM dict_rent_period">
</mini:ProfiledSqlDataSource>

<center>

<table border="0" cellspacing="4" cellpadding="0" width="100%">
    <tr>
        <td style="width: 100%;">
            <asp:Label ID="LabelReportTitle1" runat="server" Text="Орендна плата в розрізі балансоутримувачів та звітних періодів" CssClass="reporttitle"></asp:Label>
        </td>
        <td>
            <dx:ASPxButton ID="ButtonShowFoldersPopup1" runat="server" AutoPostBack="False" Text="Зберегти звіт" Width="148px">
                <ClientSideEvents Click="ShowFoldersPopupControl" />
            </dx:ASPxButton>
        </td>
		<td>
			<dx:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="False" Text="Закріпити Колонки" Width="148px">
				<ClientSideEvents Click="ShowFieldFixxerPopupControl" />
			</dx:ASPxButton>
		</td>
        <td>
            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Додаткові Колонки" Width="148px">
                <ClientSideEvents Click="ShowFieldChooserPopupControl" />
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                HeaderText="Збереження у Файлі" 
                ClientInstanceName="ASPxPopupControl_RentByBalansOrg_SaveAs" 
                PopupElementID="ASPxButton_RentByBalansOrg_SaveAs">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxButton ID="ASPxButton3" runat="server" 
                            Text="XLS - Microsoft Excel&reg;" 
                            OnClick="ASPxButton_RentByBalansOrg_ExportXLS_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton4" runat="server" 
                            Text="PDF - Adobe Acrobat&reg;" 
                            OnClick="ASPxButton_RentByBalansOrg_ExportPDF_Click" Width="180px">
                        </dx:ASPxButton>
                        <br />
                        <dx:ASPxButton ID="ASPxButton5" runat="server" 
                            Text="CSV - значення, розділені комами" 
                            OnClick="ASPxButton_RentByBalansOrg_ExportCSV_Click" Width="180px">
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <dx:ASPxButton ID="ASPxButton_RentByBalansOrg_SaveAs" runat="server" AutoPostBack="False" 
                Text="Зберегти у Файлі" Width="148px">
            </dx:ASPxButton>
        </td>
    </tr>
</table>

<dx:ASPxGridViewExporter ID="GridViewRentByBalansOrgExporter" runat="server" 
    FileName="Балансоутримувачi" GridViewID="PrimaryGridView" PaperKind="A4" 
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
    DataSourceID="SqlDataSourceRentByBalansOrg"
    KeyFieldName="npp"
    OnCustomCallback="GridViewRentByBalansOrg_CustomCallback"
    OnCustomFilterExpressionDisplayText = "GridViewRentByBalansOrg_CustomFilterExpressionDisplayText"
    OnProcessColumnAutoFilter = "GridViewRentByBalansOrg_ProcessColumnAutoFilter" >

    <Columns>
          <dx:GridViewDataTextColumn FieldName="short_name" ReadOnly="True" ShowInCustomizationForm="True" Caption="Балансоутримувач" Width="300px" >
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("short_name") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="zkpo_code" ReadOnly="True" ShowInCustomizationForm="True" Caption="Код ЄДРПОУ" >
            <Settings AllowHeaderFilter="False" />
            <DataItemTemplate>
                <%# "<a href=\"javascript:ShowOrganizationCard(" + Eval("organization_id") + ")\">" + Eval("zkpo_code") + "</a>"%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataComboBoxColumn FieldName="rent_period_id" Visible="True" Caption="Звітній Період" Width="120px">
            <PropertiesComboBox DataSourceID="SqlDataSourceRentPeriods" ValueField="id" TextField="name" ValueType="System.Int32" />
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataTextColumn FieldName="dict_rent_occupation_name" ReadOnly="True" ShowInCustomizationForm="True" Caption="Сфера діяльності" Width="220px"/>
<%-- --%>
        <dx:GridViewDataTextColumn FieldName="addr_district" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="7" Visible="True" Caption="Район" Width="150px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_street_name" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="8" Visible="True" Caption="Назва Вулиці" Width="200px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_nomer" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="9" Visible="True" Caption="Номер Будинку"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_korpus" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="10" Visible="False" Caption="Номер Будинку - Корпус"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="addr_zip_code" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="11" Visible="False" Caption="Поштовий Індекс"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="industry" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="12" Visible="False" Caption="Галузь"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="occupation" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="13" Visible="False" Caption="Вид Діяльності" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="industry" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="14" Visible="False" Caption="Галузь (Баланс)"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="occupation" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="15" Visible="False" Caption="Вид Діяльності (Баланс)" Width="120px"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="form_of_ownership" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="17" Visible="False" Caption="Форма Власності"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="gosp_struct" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="18" Visible="False" Caption="Госп. Структура"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="vedomstvo" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="19" Visible="False" Caption="Орган управління"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="org_form" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="20" Visible="False" Caption="Орг.-правова форма госп."></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_fio" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="23" Visible="False" Caption="ФІО Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="director_phone" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="24" Visible="False" Caption="Тел. Директора"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_fio" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="25" Visible="False" Caption="ФІО Бухгалтера"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="buhgalter_phone" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="26" Visible="False" Caption="Тел. Бухгалтера"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="kved_code" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="32" Visible="False" Caption="КВЕД"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="koatuu" ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="33" Visible="False" Caption="КОАТУУ"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="otdel_gukv" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="34" Visible="False" Caption="Відділ ДКВ"></dx:GridViewDataTextColumn>

        <dx:GridViewDataTextColumn FieldName="NumOfObj" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="41" Caption="Кількість об'єктів на балансі" />

        <dx:GridViewDataTextColumn FieldName="SQR_TOTAL_BAL" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="46" Caption="Загальна площа, що знаходиться на балансі, кв.м." />
        <dx:GridViewDataTextColumn FieldName="SQR_KOR" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="47" Caption="Корисна площа, що знаходиться на балансі, кв.м." />
        <dx:GridViewDataTextColumn FieldName="SQR_VIDCH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="48" Caption="Загальна площа, що знята з балансу, кв.м." />
        <dx:GridViewDataTextColumn FieldName="NUM_VIDCH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="49" Caption="Кількість об'єктів знятих з балансу" />
        <dx:GridViewDataTextColumn FieldName="SQR_RENTED" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="50" Caption="Площа, що орендується, кв.м." />
        <dx:GridViewDataTextColumn FieldName="SQR_GIVEN" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="51" Caption="Загальна площа, що надається в оренду, кв.м." />
        <dx:GridViewDataTextColumn FieldName="NUM_GIVEN" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="52" Caption="Кількість договорів оренди" />
        <dx:GridViewDataTextColumn FieldName="NUM_RENTED" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="53" Caption="Кількість договорів орендування" />
        <dx:GridViewDataTextColumn FieldName="SQR_FREE" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="54" Caption="Загальна вільна площа, що може бути надана в оренду, кв.м." />

        <dx:GridViewDataTextColumn Caption="Нараховано орендної плати за звітний період, грн. (без ПДВ)" FieldName="PAY_NARAH_ZVIT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="55"  />
        <dx:GridViewDataTextColumn Caption="Отримано орендної плати всього за звітний період, грн. (без ПДВ)" FieldName="PAY_RECV_ZVIT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="57"  />
        <dx:GridViewDataTextColumn Caption="Отримано орендної плати в тому числі інші платежі" FieldName="PAY_RECV_OTHER" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="59"  />
        <dx:GridViewDataTextColumn Caption="Погашення заборгованості минулих періодів" FieldName="PAY_LAST_PER" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="60"  />
        <dx:GridViewDataTextColumn Caption="Загальна заборгованість по орендній платі, грн. (без ПДВ)" FieldName="PAY_DEBT_TOTAL" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="61"  />
        <dx:GridViewDataTextColumn Caption="- в тому числі заборгованість по орендній платі за звітний період,  грн. (без ПДВ)" FieldName="PAY_DEBT_ZVIT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="62"  />
        <dx:GridViewDataTextColumn Caption="- в тому числі (із загальної заборгованості) заборгованість з орендної плати, розмір якої встановлено в межах витрат на утримання, грн. (без ПДВ)" FieldName="PAY_DEBT_V_MEZH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="63"  />

       
        <dx:GridViewDataTextColumn Caption="Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період, грн. (без ПДВ)" FieldName="PAY_50_NARAH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="64"  />
        <dx:GridViewDataTextColumn Caption="Перераховано до бюджету % за звітний період всього з 1 січня поточного року, грн. (без ПДВ)" FieldName="PAY_50_PAYED" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="65"  />
        <dx:GridViewDataTextColumn Caption="- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки, грн. (без ПДВ)" FieldName="PAY_50_DEBT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="66"  />
        <dx:GridViewDataTextColumn Caption="Заборгованість зі сплати % до бюджету від оренди майна за  звітний період, грн. (без ПДВ)" FieldName="PAY_50_DEBT_CUR" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="67"  />
        <dx:GridViewDataTextColumn Caption="Заборгованість зі сплати % до бюджету від оренди майна минулих років, грн. (без ПДВ)" FieldName="PAY_50_DEBT_OLD" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="68"  />

        <dx:GridViewDataTextColumn Caption="ЦМК Площа в оренді, кв.м." FieldName="PAY_CMK_SQR" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="69"  />
        <dx:GridViewDataTextColumn Caption="ЦМК Нарахована орендна плата, грн. (без ПДВ)" FieldName="PAY_CMK_NARAH" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="70"  />
        <dx:GridViewDataTextColumn Caption="ЦМК Перераховано до бюджету, грн. (без ПДВ)" FieldName="PAY_CMK_BUDGET" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="71"  />
        <dx:GridViewDataTextColumn Caption="ЦМК Заборгованість по орендній платі, грн. (без ПДВ)" FieldName="PAY_CMK_DEBT" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="72"  />

<%--
        <dx:GridViewDataTextColumn Caption="Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')" FieldName="PAY_SPECIAL" ReadOnly="true" ShowInCustomizationForm="true" VisibleIndex="73"  />
 --%>
    </Columns>

    <GroupSummary>
        <dx:ASPxSummaryItem DisplayFormat="{0} рядків" SummaryType="Count" />
    </GroupSummary>
<%-- --%>
    <TotalSummary>
<%--
        <dx:ASPxSummaryItem FieldName="NumOfObj" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_TOTAL_BAL" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_KOR" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_VIDCH" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="NUM_VIDCH" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_RENTED" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_GIVEN" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="NUM_GIVEN" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="NUM_RENTED" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="SQR_FREE" SummaryType="Sum" DisplayFormat="{0}" />

        <dx:ASPxSummaryItem FieldName="PAY_NARAH_ZVIT" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_PEREPLATA" SummaryType="Sum" DisplayFormat="{0}" />
        <dx:ASPxSummaryItem FieldName="PAY_RECV_ZVIT" SummaryType="Sum" DisplayFormat="{0}" />
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
 --%>
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
    <SettingsCookies CookiesID="GUKV.OrendnaPlataByBalans" Version="A2_2" Enabled="True" />
    <Styles Header-Wrap="True" >
        <Header Wrap="True"></Header>
    </Styles>

    <ClientSideEvents Init="GridViewRentByBalansOrgInit" EndCallback="GridViewRentByBalansOrgEndCallback" />
</dx:ASPxGridView>

</center>

<dx:ASPxPopupControl ID="PopupControlFolders" runat="server" HeaderText="Зберегти звіт"
    ClientInstanceName="PopupControlFolders" PopupElementID="PrimaryGridView" PopupAction="None"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Middle" PopupAnimationType="Slide">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
            <uc1:SaveReportCtrl ID="FolderBrowser" runat="server"/>
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
        <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
            <uc3:FieldChooser ID="FieldChooser1" runat="server"/>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function (s, e) { EditColumnNamePattern.SetText(''); CPGridColumns.PerformCallback(); }" />
</dx:ASPxPopupControl>

</asp:Content>
