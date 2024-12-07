declare @period_year int = case when MONTH(GETDATE()) = 1 then YEAR(GETDATE()) - 1 else YEAR(GETDATE()) end

select
full_name as [Назва Організації],
zkpo_code as [Код ЄДРПОУ],
cur_state as [Стан актуализации данных],
max_submit_date as [Дата актуализации данных],
stan_recieve_id as [Поточний стан звіту],
stan_recieve_description as [Примітки],
prim_balanc as [Примітки балансоутримувача],
stan_recieve_date as [Дата останнього прийому],
inventar_recieve_date as [Дата останнього прийому акту інвентаризації],
dict_rent_occupation_name as [Сфера діяльності],
addr_district as [Район],
addr_street_name as [Назва Вулиці],
addr_nomer as [Номер Будинку],
occupation as [Вид Діяльності],
form_gosp as [Форма фінансування],
form_of_ownership as [Форма Власності],
gosp_struct as [Госп. Структура],
org_form as [Орг.-правова форма госп.],
old_organ as [Орган госп. упр.],
registration_svidot as [Номер Свідоцтва про Реєстрацію],
kved_code as [КВЕД],
otdel_gukv as [Стан юр.особи],
NumOfObj as [Кількість об'єктів на балансі],
ORG_CONTRIB_RATE as [Ставка відрахувань до бюджету (%)],
SQR_TOTAL_BAL as [Загальна площа, що знаходиться на балансі, кв.м.],
SQR_KOR as [Корисна площа, що знаходиться на балансі, кв.м.],
SQR_VLAS_POTREB as [Загальна площа об'єктів для власних потреб, кв.м.],
SQR_VLAS_POTREB_COUNT as [Кількість об'єктів що мають площу для власних потреб],
SQR_RENTED as [Площа, що орендується, кв.м.],
SQR_GIVEN as [Загальна площа, що надається в оренду, кв.м.],
NUM_GIVEN as [Кількість договорів оренди],
NUM_RENTER as [Кількість орендарів],
NUM_RENTED as [Кількість договорів орендування],
SQR_FREE as [Загальна вільна площа, що може бути надана в оренду, кв.м.],
SQR_FREE_COUNT as [Кількість об'єктів що мають вільну площу],
PAY_NARAH_ZVIT as [Нараховано орендної плати за звітний період, грн. (без ПДВ)],
PAY_ZNYATO_NADMIRNO_NARAH as [- у тому числі, знято надмірно нарахованої за звітний період],
PAY_PEREPLATA as [Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)],
PAY_ZABEZDEPOZ_PRISHLO as [Надходження авансової орендної плати у звітному періоді, грн. (без ПДВ)],
PAY_RECV_ZVIT_new as [Надходження орендної плати за звітний період, всього, грн. (без ПДВ)],
PAY_RECV_NARAH as [- у тому числі, з нарахованої за звітний період (без боргів та переплат)],
PAY_LAST_PER as [- у тому числі, погашення заборгованості минулих періодів, грн.],
PAY_RETURN_OREND_PAYED as [- у тому числі, переплата орендної плати за звітний період, грн.],
PAY_PEREPLATA_ALL as [Переплата орендної плати на кінець звітного періоду, грн. (без ПДВ)],
PAY_RETURN_ALL_OREND_PAYED as [Повернення переплати орендної плати всього у звітному періоді, грн. (без ПДВ)],
PAY_NARAH_ZVIT_NORMAL as [Нараховано орендної плати без урахування надмірно нарахованої плати, грн. (без ПДВ)],
PAY_AVANCE_DEBT as [Заборгованість з нарахованої авансової орендної плати, грн. (без ПДВ)],
PAY_RECV_OTHER as [Отримано орендної плати в тому числі інші платежі],
PAY_DEBT_TOTAL as [Загальна заборгованість по орендній платі, грн. (без ПДВ)],
PAY_DEBT_ZVIT as [- в тому числі заборгованість по орендній платі за звітний період,  грн. (без ПДВ)],
PAY_DEBT_V_MEZH as [- в тому числі (із загальної заборгованості) заборгованість з орендної плати, розмір якої встановлено в межах витрат на утрим.],
PAY_50_NARAH as [Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період, грн. (без ПДВ)],
PAY_UNKNOWN_PAYMENTS as [Сальдо платежів до бюджету (переплата на початок року), грн.],
PAY_50_PAYED as [Перераховано коштів до бюджету, у звітному періоді "КАЗНАЧЕЙСТВО", грн. (без ПДВ)],
PAY_50_DEBT as [- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки, грн. (без ПДВ)],
PAY_50_DEBT_CUR as [Заборгованість зі сплати % до бюджету від оренди майна за  звітний період, грн. (без ПДВ)],
PAY_50_DEBT_OLD as [Заборгованість зі сплати % до бюджету від оренди майна минулих років, грн. (без ПДВ)],
PAY_SPECIAL as [Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')],
planuvania_1 as [Прогнозовані надходження за місяць, грн.],
planuvania_2 as [Прогнозовані надходження за рік, грн.],
planuvania_3 as [Прогнозовані надходження  до бюджету за рік, грн.],
planuvania_4 as [Планові надходження орендної плати за рік згідно фін. плану, грн.],
planuvania_5 as [Планові надходження орендної плати до бюджету за рік згідно фін. плану, грн.]
from
(
	SELECT 
			isnull(ddd.name, 'Невідомо') as 'dict_rent_occupation_name',
			(SELECT Q.stan_recieve_name FROM dict_stan_recieve Q where Q.stan_recieve_id = rep.stan_recieve_id) stan_recieve_name,

			--rep.*,
			full_name,
			zkpo_code,
			cur_state,
			--max_submit_date,
			stan_recieve_id,
			stan_recieve_description,
			--prim_balanc,
			stan_recieve_date,
			inventar_recieve_date,
			--dict_rent_occupation_name,
			addr_district,
			addr_street_name,
			addr_nomer,
			occupation,
			form_gosp,
			form_of_ownership,
			--gosp_struct,
			org_form,
			old_organ,
			registration_svidot,
			kved_code,
			--otdel_gukv,
			--NumOfObj,
			--ORG_CONTRIB_RATE,
			SQR_TOTAL_BAL,
			SQR_KOR,
			SQR_VLAS_POTREB,
			SQR_VLAS_POTREB_COUNT,
			SQR_RENTED,
			SQR_GIVEN,
			NUM_GIVEN,
			NUM_RENTER,
			NUM_RENTED,
			SQR_FREE,
			SQR_FREE_COUNT,
			PAY_NARAH_ZVIT,
			PAY_ZNYATO_NADMIRNO_NARAH,
			PAY_PEREPLATA,
			PAY_ZABEZDEPOZ_PRISHLO,
			--PAY_RECV_ZVIT_new,
			PAY_RECV_NARAH,
			PAY_LAST_PER,
			PAY_RETURN_OREND_PAYED,
			PAY_PEREPLATA_ALL,
			PAY_RETURN_ALL_OREND_PAYED,
			PAY_NARAH_ZVIT_NORMAL,
			PAY_AVANCE_DEBT,
			--PAY_RECV_OTHER,
			PAY_DEBT_TOTAL,
			PAY_DEBT_ZVIT,
			PAY_DEBT_V_MEZH,
			--PAY_50_NARAH,
			--PAY_UNKNOWN_PAYMENTS,
			--PAY_50_PAYED,
			--PAY_50_DEBT,
			--PAY_50_DEBT_CUR,
			--PAY_50_DEBT_OLD,
			--PAY_SPECIAL,
			--planuvania_1,
			--planuvania_2,
			--planuvania_3,
			--planuvania_4,
			--planuvania_5,

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
        
			--,ObjectAndRentTotals1.*
			--,ObjectAndRentTotals2.*
			--,ObjectAndRentTotals3.*
			--,ObjectAndRentTotals4.*
			,OrganizationProperties.*
			--,RentPaymentProperties1.*
			--,RentPaymentProperties2.*

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
				(0 = 0 OR (rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = 0))
					AND
				(0 = 0 OR rep.old_organ_id = 0)
					AND
				( (0 = 0) OR (0 = 1 and obj.NumOfObj > 0) OR (0 = 2 and isnull(obj.NumOfObj,0) <= 0) )
) T
