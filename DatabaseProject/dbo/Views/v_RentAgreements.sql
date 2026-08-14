

CREATE view [dbo].[v_RentAgreements] as
SELECT 
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

,p.znizhka1_name	
,p.znizhka1_percent
,p.znizhka1_date1
,p.znizhka1_date2
,p.znizhka2_name	
,p.znizhka2_percent
,p.znizhka2_date1
,p.znizhka2_date2
,p.znizhka3_name	
,p.znizhka3_percent
,p.znizhka3_date1
,p.znizhka3_date2
,p.znizhka4_name	
,p.znizhka4_percent
,p.znizhka4_date1
,p.znizhka4_date2
,p.znizhka5_name	
,p.znizhka5_percent
,p.znizhka5_date1
,p.znizhka5_date2
,p.znizhka6_name	
,p.znizhka6_percent
,p.znizhka6_date1
,p.znizhka6_date2

,p.znizhka1_invnums
,p.znizhka2_invnums
,p.znizhka3_invnums
,p.znizhka4_invnums
,p.znizhka5_invnums
,p.znizhka6_invnums

,p.znizhka7_name
,p.znizhka7_percent
,p.znizhka7_date1
,p.znizhka7_date2
,p.znizhka7_invnums

,p.znizhka8_name
,p.znizhka8_percent
,p.znizhka8_date1
,p.znizhka8_date2
,p.znizhka8_invnums

,p.znizhka9_name
,p.znizhka9_percent
,p.znizhka9_date1
,p.znizhka9_date2
,p.znizhka9_invnums

,p.znizhka10_name
,p.znizhka10_percent
,p.znizhka10_date1
,p.znizhka10_date2
,p.znizhka10_invnums

,d.name as stanjuro

,ar.insurance_sum
,ar.insurance_start
,ar.insurance_end

,isnull(ddd.name, 'Невідомо') as sphera_dialnosti
,priznachennya = dc.doc_display_name
,case when exists (select 1 from reports1nf_arenda q where q.id = ar.id) then 1 else 0 end as ex_reports1nf_arenda 
,(select top 1 q.report_id from reports1nf_arenda q where q.id = ar.id) as arenda_report_id
,case when ar.id in (select b.id from dbo.reports1nf_arenda b where b.org_balans_id = ar.org_balans_id and ISNULL(b.is_deleted, 0) = 0 /*and b.agreement_state = 1*/ ) then 1 else 0 end as is_dpz_object 

		FROM view_arenda_agreements m  /*m_view_arenda_agreements m3 */
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

		WHERE 
	--isnull(ar.is_deleted, 0) = 0 and 
 		ar.id in (select b.id from dbo.reports1nf_arenda b where b.org_balans_id = ar.org_balans_id and ISNULL(b.is_deleted, 0) = 0 )
