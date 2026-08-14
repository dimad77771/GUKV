
CREATE VIEW [view_rent_by_balans_org]
AS
SELECT
    p.id AS 'payment_id',
    p.org_balans_id,
    org.full_name AS 'org_full_name',
    org.short_name AS 'org_short_name',
    org.sfera_upr_id AS 'org_sfera_upr_id',
    org.form_ownership_id AS 'org_form_ownership_id',
    org.addr_distr_new_id AS 'org_district_id',
    rbo.rent_dkk_id,
    UPPER(dict_rent_dkk.name) AS 'rent_dkk_name',
    dict_rent_dkk.short_name AS 'rent_dkk_short_name',
    rbo.rent_occupation_id,
    UPPER(dict_rent_occupation.name) AS 'org_rent_occupation',
    p.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    p.sqr_balans,
    p.payment_zvit_uah,
    p.received_zvit_uah,
    p.received_zvit_uah * 0.5 AS 'rozrah_50_uah',
    p.received_nar_zvit_uah,
    p.debt_total_uah,
    p.debt_zvit_uah,
    p.budget_narah_50_uah,
    p.budget_zvit_50_uah,
    p.budget_prev_50_uah,
    p.budget_debt_50_uah,
    p.budget_debt_30_50_uah,
    p.sqr_cmk,
    p.budget_narah_50_cmk_uah,
    p.budget_zvit_50_cmk_uah,
    p.budget_debt_50_cmk_uah,
    p.budget_zvit_50_in_uah,
    p.zkpo_code,
    p.email,
    p.phone,
    p.responsible_fio,
    p.director_fio,
    p.buhgalter_fio,
    p.num_agreements,
    --(select SUM(
    p.num_renters,
    p.debt_v_mezhah_vitrat,
    p.budget_zvit_50_old,
    p.post_address,
    p.pererahunok_50_b,
    debt.debt_total,
    debt.debt_3_month,
    debt.debt_12_month,
    debt.debt_3_years,
    debt.debt_over_3_years,
    debt.debt_spysano,
    debt.num_zahodiv_total,
    debt.num_zahodiv_zvit,
    debt.num_pozov_total,
    debt.num_pozov_zvit,
    debt.num_pozov_zadov_total,
    debt.num_pozov_zadov_zvit,
    debt.num_pozov_vikon_total,
    debt.num_pozov_vikon_zvit,
    debt.debt_pogasheno_total,
    debt.debt_pogasheno_zvit,
    obj.sqr_total AS 'obj_sqr_total',
    obj.sqr_rented AS 'obj_sqr_rented',
    obj.sqr_free AS 'obj_sqr_free',
    obj.sqr_korysna AS 'obj_sqr_korysna',
    obj.sqr_mzk AS 'obj_sqr_mzk'
FROM
    rent_payment p
    LEFT OUTER JOIN dict_rent_period ON dict_rent_period.id = p.rent_period_id
    LEFT OUTER JOIN organizations org ON org.id = p.org_balans_id
    LEFT OUTER JOIN rent_balans_org rbo ON rbo.organization_id = p.org_balans_id
    LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = rbo.rent_occupation_id
    LEFT OUTER JOIN dict_rent_dkk ON dict_rent_dkk.id = rbo.rent_dkk_id
    LEFT OUTER JOIN view_rent_debt_grouped debt ON debt.rent_payment_id = p.id
    LEFT OUTER JOIN view_rent_obj_grouped obj ON obj.rent_payment_id = p.id


UNION ALL

SELECT
    -p.id AS 'payment_id',
    p.org_balans_id,
    org.full_name AS 'org_full_name',
    org.short_name AS 'org_short_name',
    org.sfera_upr_id AS 'org_sfera_upr_id',
    org.form_ownership_id AS 'org_form_ownership_id',
    org.addr_distr_new_id AS 'org_district_id',
    NULL rent_dkk_id,
    NULL AS 'rent_dkk_name',
    NULL AS 'rent_dkk_short_name',
    org.occupation_id,
    UPPER(dict_org_occupation.name) AS 'org_rent_occupation',
    p.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    b.sqr_total, --p.sqr_balans,
    p.payment_narah, --p.payment_zvit_uah,
    p.payment_received, --p.received_zvit_uah,
    p.payment_received * 0.5 AS 'rozrah_50_uah',
    p.payment_nar_zvit, --p.received_nar_zvit_uah,
    p.debt_total, --p.debt_total_uah,
    p.debt_zvit, --p.debt_zvit_uah,
	org_arc.budget_narah_50_uah,
    org_arc.budget_zvit_50_uah,
    org_arc.budget_prev_50_uah,
    p.budget_debt_50_uah,
    org_arc.budget_debt_30_50_uah,
    cmk.sqr_cmk,
    cmk.budget_narah_50_cmk_uah,
    cmk.budget_zvit_50_cmk_uah,
    cmk.budget_debt_50_cmk_uah,
    NULL, --p.budget_zvit_50_in_uah,
    org.zkpo_code,
    org.director_email,
    org.director_phone,
    p.modified_by,
    org.director_fio,
    org.buhgalter_fio,
    --p.num_agreements,
    (select SUM(s.num_agreements) from
		(SELECT COUNT(DISTINCT A1.agreement_num) as num_agreements
		FROM arenda a1
		LEFT OUTER JOIN arenda_payments p1 ON a1.id = p1.arenda_id AND p1.rent_period_id = p.rent_period_id
		where a1.agreement_state = 1 AND (A1.is_deleted IS NULL OR A1.is_deleted = 0) AND 
		a1.org_balans_id = p.org_balans_id
		GROUP BY a1.agreement_num,a1.agreement_date,a1.rent_start_date,a1.rent_finish_date) s
    
    ) as num_agreements,
    
    
    p.num_renters,
    p.debt_v_mezhah_vitrat,
    0, --p.budget_zvit_50_old, --not used
	isnull(org.addr_street_name + ', ', '')
	+ isnull(org.addr_nomer + ' ', '')
	+ isnull(org.addr_nomer2 + ' ', '')
	+ isnull(org.addr_korpus, '')
		'post_address',
    org_arc.budget_narah_50_uah, --p.pererahunok_50_b,
    p.debt_total,
    p.debt_3_month,
    p.debt_12_month,
    p.debt_3_years,
    p.debt_over_3_years,
    p.debt_spysano,
    p.num_zahodiv_total,
    p.num_zahodiv_zvit,
    p.num_pozov_total,
    p.num_pozov_zvit,
    p.num_pozov_zadov_total,
    p.num_pozov_zadov_zvit,
    p.num_pozov_vikon_total,
    p.num_pozov_vikon_zvit,
    p.debt_pogasheno_total,
    p.debt_pogasheno_zvit,
    b.sqr_total, --obj.sqr_total AS 'obj_sqr_total', --not used
    p.sqr_total_rent AS 'obj_sqr_rented',
    b.sqr_free, --obj.sqr_free AS 'obj_sqr_free',
    b.sqr_kor, --obj.sqr_korysna AS 'obj_sqr_korysna',
    b.sqr_free_mzk --obj.sqr_mzk AS 'obj_sqr_mzk'
FROM
	(SELECT a.org_balans_id
			,p.rent_period_id
			,max(p.id) id
			,max(p.modified_by) modified_by
			,sum(sqr_total_rent) sqr_total_rent
			,sum(payment_narah) payment_narah
			,sum(payment_received) payment_received
			,sum(payment_nar_zvit) payment_nar_zvit
			,sum(debt_total) debt_total
			,sum(debt_zvit) debt_zvit
			,sum(debt_3_month) debt_3_month
			,sum(debt_12_month) debt_12_month
			,sum(debt_3_years) debt_3_years
			,sum(debt_over_3_years) debt_over_3_years
			,sum(debt_v_mezhah_vitrat) debt_v_mezhah_vitrat
			,sum(debt_spysano) debt_spysano
			,sum(num_zahodiv_total) num_zahodiv_total
			,sum(num_zahodiv_zvit) num_zahodiv_zvit
			,sum(num_pozov_total) num_pozov_total
			,sum(num_pozov_zvit) num_pozov_zvit
			,sum(num_pozov_zadov_total) num_pozov_zadov_total
			,sum(num_pozov_zadov_zvit) num_pozov_zadov_zvit
			,sum(num_pozov_vikon_total) num_pozov_vikon_total
			,sum(num_pozov_vikon_zvit) num_pozov_vikon_zvit
			,sum(debt_pogasheno_total) debt_pogasheno_total
			,sum(debt_pogasheno_zvit) debt_pogasheno_zvit
			,sum(budget_debt_50_uah) budget_debt_50_uah
			--,count(distinct a.id) num_agreements
			,count(distinct a.org_renter_id) num_renters
		FROM [arenda_payments] p
		INNER JOIN arenda a ON a.id = p.arenda_id --and a.agreement_state = 1
		GROUP BY a.org_balans_id, p.rent_period_id) p
	LEFT JOIN 
		(
		SELECT balans.organization_id, 
			SUM(balans.sqr_total) sqr_total, 
			SUM(balans.sqr_free) sqr_free,
			SUM(balans.sqr_kor) sqr_kor,
			SUM(balans.sqr_free_mzk) sqr_free_mzk
		FROM view_balans balans
		GROUP BY balans.organization_id
		) b ON b.organization_id = p.org_balans_id
    INNER JOIN dict_rent_period ON dict_rent_period.id = p.rent_period_id
    INNER JOIN organizations org ON org.id = p.org_balans_id
	CROSS APPLY
		(
			SELECT TOP 1
			   x.[id]
			  ,x.[master_org_id]
			  ,x.[last_state]
			  ,x.[occupation_id]
			  ,x.[status_id]
			  ,x.[form_gosp_id]
			  ,x.[form_ownership_id]
			  ,x.[gosp_struct_id]
			  ,x.[organ_id]
			  ,x.[industry_id]
			  ,x.[nomer_obj]
			  ,x.[zkpo_code]
			  ,x.[addr_distr_old_id]
			  ,x.[addr_distr_new_id]
			  ,x.[addr_street_name]
			  ,x.[addr_street_id]
			  ,x.[addr_nomer]
			  ,x.[addr_nomer2]
			  ,x.[addr_korpus]
			  ,x.[addr_zip_code]
			  ,x.[addr_misc]
			  ,x.[director_fio]
			  ,x.[director_phone]
			  ,x.[director_fio_kogo]
			  ,x.[director_title]
			  ,x.[director_title_kogo]
			  ,x.[director_doc]
			  ,x.[director_doc_kogo]
			  ,x.[director_email]
			  ,x.[buhgalter_fio]
			  ,x.[buhgalter_phone]
			  ,x.[buhgalter_email]
			  ,x.[num_buildings]
			  ,x.[full_name]
			  ,x.[short_name]
			  ,x.[priznak_id]
			  ,x.[title_form_id]
			  ,x.[form_1nf_id]
			  ,x.[vedomstvo_id]
			  ,x.[title_id]
			  ,x.[form_id]
			  ,x.[gosp_struct_type_id]
			  ,x.[search_name]
			  ,x.[name_komu]
			  ,x.[fax]
			  ,x.[registration_auth]
			  ,x.[registration_num]
			  ,x.[registration_date]
			  ,x.[registration_svidot]
			  ,x.[l_year]
			  ,x.[date_l_year]
			  ,x.[sqr_on_balance]
			  ,x.[sqr_manufact]
			  ,x.[sqr_non_manufact]
			  ,x.[sqr_free_for_rent]
			  ,x.[sqr_total]
			  ,x.[sqr_rented]
			  ,x.[sqr_privat]
			  ,x.[sqr_given_for_rent]
			  ,x.[sqr_znyata_z_balansu]
			  ,x.[sqr_prodaj]
			  ,x.[sqr_spisani_zneseni]
			  ,x.[sqr_peredana]
			  ,x.[num_objects]
			  ,x.[kved_code]
			  ,x.[date_stat_spravka]
			  ,x.[koatuu]
			  ,x.[modified_by]
			  ,x.[modify_date]
			  ,x.[share_type_id]
			  ,x.[share]
			  ,x.[bank_name]
			  ,x.[bank_mfo]
			  ,x.[is_deleted]
			  ,x.[del_date]
			  ,x.[otdel_gukv_id]
			  ,x.[arch_id]
			  ,x.[arch_flag]
			  ,x.[is_liquidated]
			  ,x.[liquidation_date]
			  ,x.[pidp_rda]
			  ,x.[is_arend]
			  ,x.[beg_state_date]
			  ,x.[end_state_date]
			  ,x.[mayno_id]
			  ,x.[contact_email]
			  ,x.[contact_posada_id]
			  ,x.[nadhodjennya_id]
			  ,x.[vibuttya_id]
			  ,x.[privat_status_id]
			  ,x.[cur_state_id]
			  ,x.[sfera_upr_id]
			  ,x.[plan_zone_id]
			  ,x.[registr_org_id]
			  ,x.[nadhodjennya_date]
			  ,x.[vibuttya_date]
			  ,x.[chastka]
			  ,x.[registration_rish]
			  ,x.[registration_dov_date]
			  ,x.[registration_corp]
			  ,x.[strok_start_date]
			  ,x.[strok_end_date]
			  ,x.[stat_fond]
			  ,x.[size_plus]
			  ,x.[addr_zip_code_3]
			  ,x.[old_industry_id]
			  ,x.[old_occupation_id]
			  ,x.[old_organ_id]
			  ,x.[form_vlasn_vibuttya_id]
			  ,x.[addr_city]
			  ,x.[addr_flat_num]
			  ,x.[povnovajennia]
			  ,x.[povnov_osoba_fio]
			  ,x.[povnov_passp_seria]
			  ,x.[povnov_passp_num]
			  ,x.[povnov_passp_auth]
			  ,x.[povnov_passp_date]
			  ,x.[director_passp_seria]
			  ,x.[director_passp_num]
			  ,x.[director_passp_auth]
			  ,x.[director_passp_date]
			  ,x.[registration_svid_date]
			  ,x.[origin_db]
			  ,x.[budg_payments_rate]
			  ,x.[is_under_closing]
			  ,x.[phys_addr_street_id]
			  ,x.[phys_addr_district_id]
			  ,x.[phys_addr_nomer]
			  ,x.[phys_addr_zip_code]
			  ,x.[phys_addr_misc]
			  ,x.[contribution_rate]
			  ,x.[budget_narah_50_uah]
			  ,x.[budget_zvit_50_uah]
			  ,x.[budget_prev_50_uah]
			  ,x.[budget_debt_30_50_uah]
			  ,x.[is_special_organization]
			  ,x.[payment_budget_special]
			  ,x.[konkurs_payments]
			  ,x.[unknown_payments]
			  ,x.[unknown_payment_note]
			FROM (
				SELECT CAST(GETDATE() AS date) effective_date
					  ,[id]
					  ,[master_org_id]
					  ,[last_state]
					  ,[occupation_id]
					  ,[status_id]
					  ,[form_gosp_id]
					  ,[form_ownership_id]
					  ,[gosp_struct_id]
					  ,[organ_id]
					  ,[industry_id]
					  ,[nomer_obj]
					  ,[zkpo_code]
					  ,[addr_distr_old_id]
					  ,[addr_distr_new_id]
					  ,[addr_street_name]
					  ,[addr_street_id]
					  ,[addr_nomer]
					  ,[addr_nomer2]
					  ,[addr_korpus]
					  ,[addr_zip_code]
					  ,[addr_misc]
					  ,[director_fio]
					  ,[director_phone]
					  ,[director_fio_kogo]
					  ,[director_title]
					  ,[director_title_kogo]
					  ,[director_doc]
					  ,[director_doc_kogo]
					  ,[director_email]
					  ,[buhgalter_fio]
					  ,[buhgalter_phone]
					  ,[buhgalter_email]
					  ,[num_buildings]
					  ,[full_name]
					  ,[short_name]
					  ,[priznak_id]
					  ,[title_form_id]
					  ,[form_1nf_id]
					  ,[vedomstvo_id]
					  ,[title_id]
					  ,[form_id]
					  ,[gosp_struct_type_id]
					  ,[search_name]
					  ,[name_komu]
					  ,[fax]
					  ,[registration_auth]
					  ,[registration_num]
					  ,[registration_date]
					  ,[registration_svidot]
					  ,[l_year]
					  ,[date_l_year]
					  ,[sqr_on_balance]
					  ,[sqr_manufact]
					  ,[sqr_non_manufact]
					  ,[sqr_free_for_rent]
					  ,[sqr_total]
					  ,[sqr_rented]
					  ,[sqr_privat]
					  ,[sqr_given_for_rent]
					  ,[sqr_znyata_z_balansu]
					  ,[sqr_prodaj]
					  ,[sqr_spisani_zneseni]
					  ,[sqr_peredana]
					  ,[num_objects]
					  ,[kved_code]
					  ,[date_stat_spravka]
					  ,[koatuu]
					  ,[modified_by]
					  ,[modify_date]
					  ,[share_type_id]
					  ,[share]
					  ,[bank_name]
					  ,[bank_mfo]
					  ,[is_deleted]
					  ,[del_date]
					  ,[otdel_gukv_id]
					  ,[arch_id]
					  ,[arch_flag]
					  ,[is_liquidated]
					  ,[liquidation_date]
					  ,[pidp_rda]
					  ,[is_arend]
					  ,[beg_state_date]
					  ,[end_state_date]
					  ,[mayno_id]
					  ,[contact_email]
					  ,[contact_posada_id]
					  ,[nadhodjennya_id]
					  ,[vibuttya_id]
					  ,[privat_status_id]
					  ,[cur_state_id]
					  ,[sfera_upr_id]
					  ,[plan_zone_id]
					  ,[registr_org_id]
					  ,[nadhodjennya_date]
					  ,[vibuttya_date]
					  ,[chastka]
					  ,[registration_rish]
					  ,[registration_dov_date]
					  ,[registration_corp]
					  ,[strok_start_date]
					  ,[strok_end_date]
					  ,[stat_fond]
					  ,[size_plus]
					  ,[addr_zip_code_3]
					  ,[old_industry_id]
					  ,[old_occupation_id]
					  ,[old_organ_id]
					  ,[form_vlasn_vibuttya_id]
					  ,[addr_city]
					  ,[addr_flat_num]
					  ,[povnovajennia]
					  ,[povnov_osoba_fio]
					  ,[povnov_passp_seria]
					  ,[povnov_passp_num]
					  ,[povnov_passp_auth]
					  ,[povnov_passp_date]
					  ,[director_passp_seria]
					  ,[director_passp_num]
					  ,[director_passp_auth]
					  ,[director_passp_date]
					  ,[registration_svid_date]
					  ,[origin_db]
					  ,[budg_payments_rate]
					  ,[is_under_closing]
					  ,[phys_addr_street_id]
					  ,[phys_addr_district_id]
					  ,[phys_addr_nomer]
					  ,[phys_addr_zip_code]
					  ,[phys_addr_misc]
					  ,[contribution_rate]
					  ,[budget_narah_50_uah]
					  ,[budget_zvit_50_uah]
					  ,[budget_prev_50_uah]
					  ,[budget_debt_30_50_uah]
					  ,[is_special_organization]
					  ,[payment_budget_special]
					  ,[konkurs_payments]
					  ,[unknown_payments]
					  ,[unknown_payment_note]
				FROM [organizations], (select max(id) rent_period_id from dict_rent_period) dict_rent_period
				UNION ALL
				SELECT archive_create_date effective_date
					  ,[id]
					  ,[master_org_id]
					  ,[last_state]
					  ,[occupation_id]
					  ,[status_id]
					  ,[form_gosp_id]
					  ,[form_ownership_id]
					  ,[gosp_struct_id]
					  ,[organ_id]
					  ,[industry_id]
					  ,[nomer_obj]
					  ,[zkpo_code]
					  ,[addr_distr_old_id]
					  ,[addr_distr_new_id]
					  ,[addr_street_name]
					  ,[addr_street_id]
					  ,[addr_nomer]
					  ,[addr_nomer2]
					  ,[addr_korpus]
					  ,[addr_zip_code]
					  ,[addr_misc]
					  ,[director_fio]
					  ,[director_phone]
					  ,[director_fio_kogo]
					  ,[director_title]
					  ,[director_title_kogo]
					  ,[director_doc]
					  ,[director_doc_kogo]
					  ,[director_email]
					  ,[buhgalter_fio]
					  ,[buhgalter_phone]
					  ,[buhgalter_email]
					  ,[num_buildings]
					  ,[full_name]
					  ,[short_name]
					  ,[priznak_id]
					  ,[title_form_id]
					  ,[form_1nf_id]
					  ,[vedomstvo_id]
					  ,[title_id]
					  ,[form_id]
					  ,[gosp_struct_type_id]
					  ,[search_name]
					  ,[name_komu]
					  ,[fax]
					  ,[registration_auth]
					  ,[registration_num]
					  ,[registration_date]
					  ,[registration_svidot]
					  ,[l_year]
					  ,[date_l_year]
					  ,[sqr_on_balance]
					  ,[sqr_manufact]
					  ,[sqr_non_manufact]
					  ,[sqr_free_for_rent]
					  ,[sqr_total]
					  ,[sqr_rented]
					  ,[sqr_privat]
					  ,[sqr_given_for_rent]
					  ,[sqr_znyata_z_balansu]
					  ,[sqr_prodaj]
					  ,[sqr_spisani_zneseni]
					  ,[sqr_peredana]
					  ,[num_objects]
					  ,[kved_code]
					  ,[date_stat_spravka]
					  ,[koatuu]
					  ,[modified_by]
					  ,[modify_date]
					  ,[share_type_id]
					  ,[share]
					  ,[bank_name]
					  ,[bank_mfo]
					  ,[is_deleted]
					  ,[del_date]
					  ,[otdel_gukv_id]
					  ,[arch_id]
					  ,[arch_flag]
					  ,[is_liquidated]
					  ,[liquidation_date]
					  ,[pidp_rda]
					  ,[is_arend]
					  ,[beg_state_date]
					  ,[end_state_date]
					  ,[mayno_id]
					  ,[contact_email]
					  ,[contact_posada_id]
					  ,[nadhodjennya_id]
					  ,[vibuttya_id]
					  ,[privat_status_id]
					  ,[cur_state_id]
					  ,[sfera_upr_id]
					  ,[plan_zone_id]
					  ,[registr_org_id]
					  ,[nadhodjennya_date]
					  ,[vibuttya_date]
					  ,[chastka]
					  ,[registration_rish]
					  ,[registration_dov_date]
					  ,[registration_corp]
					  ,[strok_start_date]
					  ,[strok_end_date]
					  ,[stat_fond]
					  ,[size_plus]
					  ,[addr_zip_code_3]
					  ,[old_industry_id]
					  ,[old_occupation_id]
					  ,[old_organ_id]
					  ,[form_vlasn_vibuttya_id]
					  ,[addr_city]
					  ,[addr_flat_num]
					  ,[povnovajennia]
					  ,[povnov_osoba_fio]
					  ,[povnov_passp_seria]
					  ,[povnov_passp_num]
					  ,[povnov_passp_auth]
					  ,[povnov_passp_date]
					  ,[director_passp_seria]
					  ,[director_passp_num]
					  ,[director_passp_auth]
					  ,[director_passp_date]
					  ,[registration_svid_date]
					  ,[origin_db]
					  ,[budg_payments_rate]
					  ,[is_under_closing]
					  ,[phys_addr_street_id]
					  ,[phys_addr_district_id]
					  ,[phys_addr_nomer]
					  ,[phys_addr_zip_code]
					  ,[phys_addr_misc]
					  ,[contribution_rate]
					  ,[budget_narah_50_uah]
					  ,[budget_zvit_50_uah]
					  ,[budget_prev_50_uah]
					  ,[budget_debt_30_50_uah]
					  ,[is_special_organization]
					  ,[payment_budget_special]
					  ,[konkurs_payments]
					  ,[unknown_payments]
					  ,[unknown_payment_note]
				FROM arch_organizations
			) x WHERE x.id = p.org_balans_id
				AND DATEADD(d, 20, dict_rent_period.period_end) >= x.effective_date
			ORDER BY x.effective_date DESC
		) as org_arc
    LEFT OUTER JOIN dict_org_occupation ON dict_org_occupation.id = org_arc.occupation_id
	LEFT OUTER JOIN
		(SELECT
			arenda_rented.org_renter_id,
			arenda_payments_cmk.rent_period_id,
			SUM(arenda_payments_cmk.cmk_sqr_rented) sqr_cmk,
			SUM(arenda_payments_cmk.cmk_payment_to_budget) budget_zvit_50_cmk_uah,
			SUM(arenda_payments_cmk.cmk_payment_narah) budget_narah_50_cmk_uah,
			SUM(arenda_payments_cmk.cmk_rent_debt) budget_debt_50_cmk_uah
		FROM
			arenda_rented
			INNER JOIN arenda_payments_cmk on arenda_payments_cmk.arenda_rented_id = arenda_rented.id
		WHERE
			ISNULL(arenda_rented.is_deleted, 0) = 0
		GROUP BY
			arenda_rented.org_renter_id,
			arenda_payments_cmk.rent_period_id) cmk ON cmk.org_renter_id = p.org_balans_id AND cmk.rent_period_id = p.rent_period_id
WHERE NOT EXISTS (SELECT 1 FROM rent_payment WHERE rent_payment.org_balans_id = p.org_balans_id AND rent_payment.rent_period_id = p.rent_period_id)
