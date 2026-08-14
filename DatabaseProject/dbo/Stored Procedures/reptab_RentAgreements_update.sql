CREATE   PROCEDURE [dbo].[reptab_RentAgreements_update]
AS
BEGIN

drop table if exists #qqq

SELECT m.*
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


,d.name as stanjuro

,ar.insurance_sum
,ar.insurance_start
,ar.insurance_end

--,ar.orandodavec_user_id
--,(select rtrim(ltrim(concat(Q2.namf,' ',Q2.nami,' ',Q2.namo))) from reports1nf Q1 join dict_orandodavec_user Q2 on Q2.id = Q1.orandodavec_user_id where Q1.organization_id = m.org_balans_id) as orandodavec_user_name2

,(select top 1 director_email from organizations Q where Q.zkpo_code = m.org_renter_zkpo) as org_renter_director_email
,isnull(ddd.name, 'Невідомо') as sphera_dialnosti
,priznachennya = dc.purpose_str
,case when exists (select 1 from reports1nf_arenda q where q.id = ar.id) then 1 else 0 end as ex_reports1nf_arenda 
,(select top 1 q.report_id from reports1nf_arenda q where q.id = ar.id) as arenda_report_id
,case when ar.id in (select b.id from dbo.reports1nf_arenda b where b.org_balans_id = ar.org_balans_id and ISNULL(b.is_deleted, 0) = 0 /*and b.agreement_state = 1*/ ) then 1 else 0 end as is_dpz_object 
,(SELECT count(*) FROM reports1nf_arenda_notes WHERE (is_deleted IS NULL OR is_deleted = 0) AND report_id = rep.rep_id AND arenda_id = m.arenda_id) as count_dogovor_objects
--,count_dogovor_objects
,case when exists (select 1 from reports1nf_arendaphotos Q where Q.arenda_id = m.arenda_id) then 1 else 0 end as has_reports1nf_photos
,old.name as old_organ
,p.num_pozov_total
,p.num_pozov_zadov_total
,p.num_pozov_vikon_total
,Stuff((
	SELECT 
		';' + CONVERT (NVARCHAR(MAX), reports1nf_arenda_notes.ref_balans_id)
	FROM reports1nf_arenda_notes WHERE (is_deleted IS NULL OR is_deleted = 0) 
	AND report_id = rep.rep_id AND arenda_id = m.arenda_id
	ORDER by 1
	FOR XML PATH(''),TYPE
).value('text()[1]','nvarchar(max)'),1,1,'') as list_dogovor_objects
--,list_dogovor_objects

,(select top 1 Q.prozoro_number from reports1nf_balans_free_square Q where Q.current_stage_docdate = m.agreement_date and Q.current_stage_docnum = m.agreement_num and m.agreement_date is not null and m.agreement_num <> '') as prozoro_number

,(select Q.name from dict_method_calc Q where Q.id = ar.method_calc_id) as method_calc_name
,ar.base_month


--!! ,W.big_month_koef
into #qqq
        FROM view_arenda_agreements m  /*m_view_arenda_agreements m3 */
        join arenda ar on ar.id = m.arenda_id
		outer apply (select top 1 q.report_id as rep_id from reports1nf_arenda q where q.id = ar.id) rep
        outer apply (select cast(avg(isnull(n.cost_narah,0)) as decimal(10,2)) as n_cost_narah, sum(isnull(n.rent_rate,0)) as n_rent_rate,sum(isnull(n.rent_rate_uah,0)) as n_rent_rate_uah,sum(isnull(n.cost_expert_total,0)) as n_cost_expert_total,sum(isnull(n.cost_agreement,0)) as n_cost_agreement,sum(isnull(n.rent_square,0)) as n_rent_square from arenda_notes n where m.arenda_id = n.arenda_id and isnull(n.is_deleted,0)=0 ) an1  
        outer apply (SELECT top 1 n.arenda_id, n.cost_agreement, n.cost_narah, n.payment_type_id 
               from [dbo].[arenda_notes] n where n.arenda_id = ar.id and isnull(n.is_deleted, 0) = 0 order by n.cost_agreement desc) an2
		join dbo.organizations org on m.org_balans_id = org.id
		left join dict_org_old_organ old on old.id = org.old_organ_id
		outer apply (select top 1 * from arenda_payments where arenda_id = ar.id order by id desc) p 
		outer apply (select top 1 doc_display_name, purpose_str from view_arenda_link_2_decisions ld where ld.arenda_id = ar.id order by ld.link_id) dc 
		left join [dbo].[dict_otdel_gukv] d on org.otdel_gukv_id = d.id 
                    LEFT OUTER JOIN (select obp.org_id,occ.name from org_by_period obp
                                      join dict_rent_occupation occ on occ.id = obp.org_occupation_id
                                      where obp.period_id = (select top 1 id from dict_rent_period order by id desc)) DDD ON DDD.org_id = m.org_balans_id
        --!! left join (select * from bigborg_arenda(case when @p_bigborg_filter = 1 then @p_bigborg_email else '-' end)) W on W.arenda_id = ar.id

		--outer apply 
		--(
		--	select 
		--		count(*) as count_dogovor_objects,
		--		STRING_AGG(CONVERT (NVARCHAR(MAX), reports1nf_arenda_notes.ref_balans_id), ';') as list_dogovor_objects
		--		--'' as list_dogovor_objects
		--	FROM reports1nf_arenda_notes 
		--	WHERE (is_deleted IS NULL OR is_deleted = 0) AND report_id = rep.rep_id AND arenda_id = m.arenda_id
		--) dogovor_objects

        WHERE 1=1

		--and
  --  --isnull(ar.is_deleted, 0) = 0 and 
 	--    ((@p_dpz_filter = 0) OR (@p_dpz_filter <> 0 AND ar.id in (select b.id from dbo.reports1nf_arenda b where b.org_balans_id = ar.org_balans_id and ISNULL(b.is_deleted, 0) = 0 /*and b.agreement_state = 1*/ ) )) AND
  --      ((@p_com_filter = 0) OR (@p_com_filter <> 0 AND (m.balans_form_ownership_int IN (32,33,34) OR m.balans_org_ownership_int IN (32,33,34)))) AND
  --      ( (@p_bigborg_filter = 0) OR (@p_bigborg_filter = 1 AND W.arenda_id is not null) ) AND
  --      ((@p_show_neziznacheni = 1) OR (@p_show_neziznacheni = 0 AND (isnull(ddd.name, 'Невідомо') <> 'Невизначені'))) AND
  --      (   (@p_rda_district_id = 0) OR
  --          (m.org_balans_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND m.org_balans_district_id = @p_rda_district_id) OR
  --          (m.org_giver_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND m.org_giver_district_id = @p_rda_district_id) OR
  --          (m.org_renter_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND m.org_renter_district_id = @p_rda_district_id))

  --      AND 
  --      (
  --          isnull(@ref_balans_id,0) <= 0
  --              OR 
  --          m.arenda_id in (select distinct Q.arenda_id from view_arenda Q where Q.ref_balans_id = @ref_balans_id and isnull(Q.is_deleted,0)=0)
  --      )


  --select arenda_id,count(*) from zzzzz20250605a group by arenda_id having count(*) > 1


alter table #qqq add unique([arenda_id])

if exists 
(
	select
	1
	from #qqq A
	full join reptab_RentAgreements B on A.[arenda_id] = B.[arenda_id]
	where 1=2
	or case when (A.[arenda_id] = B.[arenda_id]) or (A.[arenda_id] is null and B.[arenda_id] is null) then 1 else 0 end = 0
	or case when (A.[building_id] = B.[building_id]) or (A.[building_id] is null and B.[building_id] is null) then 1 else 0 end = 0
	or case when (A.[balans_id] = B.[balans_id]) or (A.[balans_id] is null and B.[balans_id] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_id] = B.[org_balans_id]) or (A.[org_balans_id] is null and B.[org_balans_id] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_full_name] = B.[org_balans_full_name]) or (A.[org_balans_full_name] is null and B.[org_balans_full_name] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_short_name] = B.[org_balans_short_name]) or (A.[org_balans_short_name] is null and B.[org_balans_short_name] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_zkpo] = B.[org_balans_zkpo]) or (A.[org_balans_zkpo] is null and B.[org_balans_zkpo] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_industry] = B.[org_balans_industry]) or (A.[org_balans_industry] is null and B.[org_balans_industry] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_occupation] = B.[org_balans_occupation]) or (A.[org_balans_occupation] is null and B.[org_balans_occupation] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_vedomstvo] = B.[org_balans_vedomstvo]) or (A.[org_balans_vedomstvo] is null and B.[org_balans_vedomstvo] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_org_form] = B.[org_balans_org_form]) or (A.[org_balans_org_form] is null and B.[org_balans_org_form] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_sfera_upr_id] = B.[org_balans_sfera_upr_id]) or (A.[org_balans_sfera_upr_id] is null and B.[org_balans_sfera_upr_id] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_form_ownership_id] = B.[org_balans_form_ownership_id]) or (A.[org_balans_form_ownership_id] is null and B.[org_balans_form_ownership_id] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_form_ownership] = B.[org_balans_form_ownership]) or (A.[org_balans_form_ownership] is null and B.[org_balans_form_ownership] is null) then 1 else 0 end = 0
	or case when (A.[org_balans_district_id] = B.[org_balans_district_id]) or (A.[org_balans_district_id] is null and B.[org_balans_district_id] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_id] = B.[org_renter_id]) or (A.[org_renter_id] is null and B.[org_renter_id] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_full_name] = B.[org_renter_full_name]) or (A.[org_renter_full_name] is null and B.[org_renter_full_name] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_short_name] = B.[org_renter_short_name]) or (A.[org_renter_short_name] is null and B.[org_renter_short_name] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_zkpo] = B.[org_renter_zkpo]) or (A.[org_renter_zkpo] is null and B.[org_renter_zkpo] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_industry] = B.[org_renter_industry]) or (A.[org_renter_industry] is null and B.[org_renter_industry] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_occupation] = B.[org_renter_occupation]) or (A.[org_renter_occupation] is null and B.[org_renter_occupation] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_vedomstvo] = B.[org_renter_vedomstvo]) or (A.[org_renter_vedomstvo] is null and B.[org_renter_vedomstvo] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_director_fio] = B.[org_renter_director_fio]) or (A.[org_renter_director_fio] is null and B.[org_renter_director_fio] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_director_phone] = B.[org_renter_director_phone]) or (A.[org_renter_director_phone] is null and B.[org_renter_director_phone] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_form_of_ownership] = B.[org_renter_form_of_ownership]) or (A.[org_renter_form_of_ownership] is null and B.[org_renter_form_of_ownership] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_org_form] = B.[org_renter_org_form]) or (A.[org_renter_org_form] is null and B.[org_renter_org_form] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_sfera_upr_id] = B.[org_renter_sfera_upr_id]) or (A.[org_renter_sfera_upr_id] is null and B.[org_renter_sfera_upr_id] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_form_ownership_id] = B.[org_renter_form_ownership_id]) or (A.[org_renter_form_ownership_id] is null and B.[org_renter_form_ownership_id] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_district_id] = B.[org_renter_district_id]) or (A.[org_renter_district_id] is null and B.[org_renter_district_id] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_id] = B.[org_giver_id]) or (A.[org_giver_id] is null and B.[org_giver_id] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_full_name] = B.[org_giver_full_name]) or (A.[org_giver_full_name] is null and B.[org_giver_full_name] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_short_name] = B.[org_giver_short_name]) or (A.[org_giver_short_name] is null and B.[org_giver_short_name] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_zkpo] = B.[org_giver_zkpo]) or (A.[org_giver_zkpo] is null and B.[org_giver_zkpo] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_industry] = B.[org_giver_industry]) or (A.[org_giver_industry] is null and B.[org_giver_industry] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_occupation] = B.[org_giver_occupation]) or (A.[org_giver_occupation] is null and B.[org_giver_occupation] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_vedomstvo] = B.[org_giver_vedomstvo]) or (A.[org_giver_vedomstvo] is null and B.[org_giver_vedomstvo] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_org_form] = B.[org_giver_org_form]) or (A.[org_giver_org_form] is null and B.[org_giver_org_form] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_sfera_upr_id] = B.[org_giver_sfera_upr_id]) or (A.[org_giver_sfera_upr_id] is null and B.[org_giver_sfera_upr_id] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_form_ownership_id] = B.[org_giver_form_ownership_id]) or (A.[org_giver_form_ownership_id] is null and B.[org_giver_form_ownership_id] is null) then 1 else 0 end = 0
	or case when (A.[org_giver_district_id] = B.[org_giver_district_id]) or (A.[org_giver_district_id] is null and B.[org_giver_district_id] is null) then 1 else 0 end = 0
	or case when (A.[district] = B.[district]) or (A.[district] is null and B.[district] is null) then 1 else 0 end = 0
	or case when (A.[street_full_name] = B.[street_full_name]) or (A.[street_full_name] is null and B.[street_full_name] is null) then 1 else 0 end = 0
	or case when (A.[addr_nomer] = B.[addr_nomer]) or (A.[addr_nomer] is null and B.[addr_nomer] is null) then 1 else 0 end = 0
	or case when (A.[is_in_privat] = B.[is_in_privat]) or (A.[is_in_privat] is null and B.[is_in_privat] is null) then 1 else 0 end = 0
	or case when (A.[sqr_free_total] = B.[sqr_free_total]) or (A.[sqr_free_total] is null and B.[sqr_free_total] is null) then 1 else 0 end = 0
	or case when (A.[sqr_free_korysna] = B.[sqr_free_korysna]) or (A.[sqr_free_korysna] is null and B.[sqr_free_korysna] is null) then 1 else 0 end = 0
	or case when (A.[sqr_free_mzk] = B.[sqr_free_mzk]) or (A.[sqr_free_mzk] is null and B.[sqr_free_mzk] is null) then 1 else 0 end = 0
	or case when (A.[free_sqr_floors] = B.[free_sqr_floors]) or (A.[free_sqr_floors] is null and B.[free_sqr_floors] is null) then 1 else 0 end = 0
	or case when (A.[free_sqr_purpose] = B.[free_sqr_purpose]) or (A.[free_sqr_purpose] is null and B.[free_sqr_purpose] is null) then 1 else 0 end = 0
	or case when (A.[object_name] = B.[object_name]) or (A.[object_name] is null and B.[object_name] is null) then 1 else 0 end = 0
	or case when (A.[object_note] = B.[object_note]) or (A.[object_note] is null and B.[object_note] is null) then 1 else 0 end = 0
	or case when (A.[name] = B.[name]) or (A.[name] is null and B.[name] is null) then 1 else 0 end = 0
	or case when (A.[purpose_group] = B.[purpose_group]) or (A.[purpose_group] is null and B.[purpose_group] is null) then 1 else 0 end = 0
	or case when (A.[purpose] = B.[purpose]) or (A.[purpose] is null and B.[purpose] is null) then 1 else 0 end = 0
	or case when (A.[is_privat] = B.[is_privat]) or (A.[is_privat] is null and B.[is_privat] is null) then 1 else 0 end = 0
	or case when (A.[is_privat_int] = B.[is_privat_int]) or (A.[is_privat_int] is null and B.[is_privat_int] is null) then 1 else 0 end = 0
	or case when (A.[agreement_kind_id] = B.[agreement_kind_id]) or (A.[agreement_kind_id] is null and B.[agreement_kind_id] is null) then 1 else 0 end = 0
	or case when (A.[agreement_kind] = B.[agreement_kind]) or (A.[agreement_kind] is null and B.[agreement_kind] is null) then 1 else 0 end = 0
	or case when (A.[agreement_date] = B.[agreement_date]) or (A.[agreement_date] is null and B.[agreement_date] is null) then 1 else 0 end = 0
	or case when (A.[agreement_date_year] = B.[agreement_date_year]) or (A.[agreement_date_year] is null and B.[agreement_date_year] is null) then 1 else 0 end = 0
	or case when (A.[agreement_date_quarter] = B.[agreement_date_quarter]) or (A.[agreement_date_quarter] is null and B.[agreement_date_quarter] is null) then 1 else 0 end = 0
	or case when (A.[agreement_num] = B.[agreement_num]) or (A.[agreement_num] is null and B.[agreement_num] is null) then 1 else 0 end = 0
	or case when (A.[agreement_num_int] = B.[agreement_num_int]) or (A.[agreement_num_int] is null and B.[agreement_num_int] is null) then 1 else 0 end = 0
	or case when (A.[agreement_state] = B.[agreement_state]) or (A.[agreement_state] is null and B.[agreement_state] is null) then 1 else 0 end = 0
	or case when (A.[floor_number] = B.[floor_number]) or (A.[floor_number] is null and B.[floor_number] is null) then 1 else 0 end = 0
	or case when (A.[cost_narah] = B.[cost_narah]) or (A.[cost_narah] is null and B.[cost_narah] is null) then 1 else 0 end = 0
	or case when (A.[cost_payed] = B.[cost_payed]) or (A.[cost_payed] is null and B.[cost_payed] is null) then 1 else 0 end = 0
	or case when (A.[cost_debt] = B.[cost_debt]) or (A.[cost_debt] is null and B.[cost_debt] is null) then 1 else 0 end = 0
	or case when (A.[cost_agreement] = B.[cost_agreement]) or (A.[cost_agreement] is null and B.[cost_agreement] is null) then 1 else 0 end = 0
	or case when (A.[cost_expert_1m] = B.[cost_expert_1m]) or (A.[cost_expert_1m] is null and B.[cost_expert_1m] is null) then 1 else 0 end = 0
	or case when (A.[cost_expert_total] = B.[cost_expert_total]) or (A.[cost_expert_total] is null and B.[cost_expert_total] is null) then 1 else 0 end = 0
	or case when (A.[debt_timespan] = B.[debt_timespan]) or (A.[debt_timespan] is null and B.[debt_timespan] is null) then 1 else 0 end = 0
	or case when (A.[pidstava] = B.[pidstava]) or (A.[pidstava] is null and B.[pidstava] is null) then 1 else 0 end = 0
	or case when (A.[pidstava_date] = B.[pidstava_date]) or (A.[pidstava_date] is null and B.[pidstava_date] is null) then 1 else 0 end = 0
	or case when (A.[pidstava_num] = B.[pidstava_num]) or (A.[pidstava_num] is null and B.[pidstava_num] is null) then 1 else 0 end = 0
	or case when (A.[pidstava_display] = B.[pidstava_display]) or (A.[pidstava_display] is null and B.[pidstava_display] is null) then 1 else 0 end = 0
	or case when (A.[rent_start_date] = B.[rent_start_date]) or (A.[rent_start_date] is null and B.[rent_start_date] is null) then 1 else 0 end = 0
	or case when (A.[rent_start_year] = B.[rent_start_year]) or (A.[rent_start_year] is null and B.[rent_start_year] is null) then 1 else 0 end = 0
	or case when (A.[rent_start_quarter] = B.[rent_start_quarter]) or (A.[rent_start_quarter] is null and B.[rent_start_quarter] is null) then 1 else 0 end = 0
	or case when (A.[rent_finish_date] = B.[rent_finish_date]) or (A.[rent_finish_date] is null and B.[rent_finish_date] is null) then 1 else 0 end = 0
	or case when (A.[rent_finish_year] = B.[rent_finish_year]) or (A.[rent_finish_year] is null and B.[rent_finish_year] is null) then 1 else 0 end = 0
	or case when (A.[rent_finish_quarter] = B.[rent_finish_quarter]) or (A.[rent_finish_quarter] is null and B.[rent_finish_quarter] is null) then 1 else 0 end = 0
	or case when (A.[rent_actual_finish_date] = B.[rent_actual_finish_date]) or (A.[rent_actual_finish_date] is null and B.[rent_actual_finish_date] is null) then 1 else 0 end = 0
	or case when (A.[actual_finish_year] = B.[actual_finish_year]) or (A.[actual_finish_year] is null and B.[actual_finish_year] is null) then 1 else 0 end = 0
	or case when (A.[actual_finish_quarter] = B.[actual_finish_quarter]) or (A.[actual_finish_quarter] is null and B.[actual_finish_quarter] is null) then 1 else 0 end = 0
	or case when (A.[rent_rate_percent] = B.[rent_rate_percent]) or (A.[rent_rate_percent] is null and B.[rent_rate_percent] is null) then 1 else 0 end = 0
	or case when (A.[rent_rate_uah] = B.[rent_rate_uah]) or (A.[rent_rate_uah] is null and B.[rent_rate_uah] is null) then 1 else 0 end = 0
	or case when (A.[rent_square] = B.[rent_square]) or (A.[rent_square] is null and B.[rent_square] is null) then 1 else 0 end = 0
	or case when (A.[rishennya_code] = B.[rishennya_code]) or (A.[rishennya_code] is null and B.[rishennya_code] is null) then 1 else 0 end = 0
	or case when (A.[num_akt] = B.[num_akt]) or (A.[num_akt] is null and B.[num_akt] is null) then 1 else 0 end = 0
	or case when (A.[date_akt] = B.[date_akt]) or (A.[date_akt] is null and B.[date_akt] is null) then 1 else 0 end = 0
	or case when (A.[num_bti] = B.[num_bti]) or (A.[num_bti] is null and B.[num_bti] is null) then 1 else 0 end = 0
	or case when (A.[date_bti] = B.[date_bti]) or (A.[date_bti] is null and B.[date_bti] is null) then 1 else 0 end = 0
	or case when (A.[modified_by] = B.[modified_by]) or (A.[modified_by] is null and B.[modified_by] is null) then 1 else 0 end = 0
	or case when (A.[modify_date] = B.[modify_date]) or (A.[modify_date] is null and B.[modify_date] is null) then 1 else 0 end = 0
	or case when (A.[is_subarenda_int] = B.[is_subarenda_int]) or (A.[is_subarenda_int] is null and B.[is_subarenda_int] is null) then 1 else 0 end = 0
	or case when (A.[is_subarenda] = B.[is_subarenda]) or (A.[is_subarenda] is null and B.[is_subarenda] is null) then 1 else 0 end = 0
	or case when (A.[payment_type] = B.[payment_type]) or (A.[payment_type] is null and B.[payment_type] is null) then 1 else 0 end = 0
	or case when (A.[agreement_active] = B.[agreement_active]) or (A.[agreement_active] is null and B.[agreement_active] is null) then 1 else 0 end = 0
	or case when (A.[agreement_active_int] = B.[agreement_active_int]) or (A.[agreement_active_int] is null and B.[agreement_active_int] is null) then 1 else 0 end = 0
	or case when (A.[is_deleted] = B.[is_deleted]) or (A.[is_deleted] is null and B.[is_deleted] is null) then 1 else 0 end = 0
	or case when (A.[balans_sqr_total] = B.[balans_sqr_total]) or (A.[balans_sqr_total] is null and B.[balans_sqr_total] is null) then 1 else 0 end = 0
	or case when (A.[balans_num_rent_agr] = B.[balans_num_rent_agr]) or (A.[balans_num_rent_agr] is null and B.[balans_num_rent_agr] is null) then 1 else 0 end = 0
	or case when (A.[balans_sqr_in_rent] = B.[balans_sqr_in_rent]) or (A.[balans_sqr_in_rent] is null and B.[balans_sqr_in_rent] is null) then 1 else 0 end = 0
	or case when (A.[balans_org_ownership] = B.[balans_org_ownership]) or (A.[balans_org_ownership] is null and B.[balans_org_ownership] is null) then 1 else 0 end = 0
	or case when (A.[balans_org_ownership_int] = B.[balans_org_ownership_int]) or (A.[balans_org_ownership_int] is null and B.[balans_org_ownership_int] is null) then 1 else 0 end = 0
	or case when (A.[balans_form_ownership] = B.[balans_form_ownership]) or (A.[balans_form_ownership] is null and B.[balans_form_ownership] is null) then 1 else 0 end = 0
	or case when (A.[balans_form_ownership_int] = B.[balans_form_ownership_int]) or (A.[balans_form_ownership_int] is null and B.[balans_form_ownership_int] is null) then 1 else 0 end = 0
	or case when (A.[date_expert] = B.[date_expert]) or (A.[date_expert] is null and B.[date_expert] is null) then 1 else 0 end = 0
	or case when (A.[form_gosp] = B.[form_gosp]) or (A.[form_gosp] is null and B.[form_gosp] is null) then 1 else 0 end = 0
	or case when (A.[agreement_active_s] = B.[agreement_active_s]) or (A.[agreement_active_s] is null and B.[agreement_active_s] is null) then 1 else 0 end = 0
	or case when (A.[n_cost_narah] = B.[n_cost_narah]) or (A.[n_cost_narah] is null and B.[n_cost_narah] is null) then 1 else 0 end = 0
	or case when (A.[n_rent_rate] = B.[n_rent_rate]) or (A.[n_rent_rate] is null and B.[n_rent_rate] is null) then 1 else 0 end = 0
	or case when (A.[n_rent_rate_uah] = B.[n_rent_rate_uah]) or (A.[n_rent_rate_uah] is null and B.[n_rent_rate_uah] is null) then 1 else 0 end = 0
	or case when (A.[n_cost_expert_total] = B.[n_cost_expert_total]) or (A.[n_cost_expert_total] is null and B.[n_cost_expert_total] is null) then 1 else 0 end = 0
	or case when (A.[n_cost_agreement] = B.[n_cost_agreement]) or (A.[n_cost_agreement] is null and B.[n_cost_agreement] is null) then 1 else 0 end = 0
	or case when (A.[n_rent_square] = B.[n_rent_square]) or (A.[n_rent_square] is null and B.[n_rent_square] is null) then 1 else 0 end = 0
	or case when (A.[cost_agreement_max] = B.[cost_agreement_max]) or (A.[cost_agreement_max] is null and B.[cost_agreement_max] is null) then 1 else 0 end = 0
	or case when (A.[cost_narah_max] = B.[cost_narah_max]) or (A.[cost_narah_max] is null and B.[cost_narah_max] is null) then 1 else 0 end = 0
	or case when (A.[contribution_rate] = B.[contribution_rate]) or (A.[contribution_rate] is null and B.[contribution_rate] is null) then 1 else 0 end = 0
	or case when (A.[payment_narah] = B.[payment_narah]) or (A.[payment_narah] is null and B.[payment_narah] is null) then 1 else 0 end = 0
	or case when (A.[last_year_saldo] = B.[last_year_saldo]) or (A.[last_year_saldo] is null and B.[last_year_saldo] is null) then 1 else 0 end = 0
	or case when (A.[payment_received] = B.[payment_received]) or (A.[payment_received] is null and B.[payment_received] is null) then 1 else 0 end = 0
	or case when (A.[payment_nar_zvit] = B.[payment_nar_zvit]) or (A.[payment_nar_zvit] is null and B.[payment_nar_zvit] is null) then 1 else 0 end = 0
	or case when (A.[old_debts_payed] = B.[old_debts_payed]) or (A.[old_debts_payed] is null and B.[old_debts_payed] is null) then 1 else 0 end = 0
	or case when (A.[return_orend_payed] = B.[return_orend_payed]) or (A.[return_orend_payed] is null and B.[return_orend_payed] is null) then 1 else 0 end = 0
	or case when (A.[return_all_orend_payed] = B.[return_all_orend_payed]) or (A.[return_all_orend_payed] is null and B.[return_all_orend_payed] is null) then 1 else 0 end = 0
	or case when (A.[use_calc_debt] = B.[use_calc_debt]) or (A.[use_calc_debt] is null and B.[use_calc_debt] is null) then 1 else 0 end = 0
	or case when (A.[debt_total] = B.[debt_total]) or (A.[debt_total] is null and B.[debt_total] is null) then 1 else 0 end = 0
	or case when (A.[debt_zvit] = B.[debt_zvit]) or (A.[debt_zvit] is null and B.[debt_zvit] is null) then 1 else 0 end = 0
	or case when (A.[debt_3_month] = B.[debt_3_month]) or (A.[debt_3_month] is null and B.[debt_3_month] is null) then 1 else 0 end = 0
	or case when (A.[debt_12_month] = B.[debt_12_month]) or (A.[debt_12_month] is null and B.[debt_12_month] is null) then 1 else 0 end = 0
	or case when (A.[debt_3_years] = B.[debt_3_years]) or (A.[debt_3_years] is null and B.[debt_3_years] is null) then 1 else 0 end = 0
	or case when (A.[debt_over_3_years] = B.[debt_over_3_years]) or (A.[debt_over_3_years] is null and B.[debt_over_3_years] is null) then 1 else 0 end = 0
	or case when (A.[debt_v_mezhah_vitrat] = B.[debt_v_mezhah_vitrat]) or (A.[debt_v_mezhah_vitrat] is null and B.[debt_v_mezhah_vitrat] is null) then 1 else 0 end = 0
	or case when (A.[debt_spysano] = B.[debt_spysano]) or (A.[debt_spysano] is null and B.[debt_spysano] is null) then 1 else 0 end = 0
	or case when (A.[num_zahodiv_total] = B.[num_zahodiv_total]) or (A.[num_zahodiv_total] is null and B.[num_zahodiv_total] is null) then 1 else 0 end = 0
	or case when (A.[num_zahodiv_zvit] = B.[num_zahodiv_zvit]) or (A.[num_zahodiv_zvit] is null and B.[num_zahodiv_zvit] is null) then 1 else 0 end = 0
	or case when (A.[avance_plat] = B.[avance_plat]) or (A.[avance_plat] is null and B.[avance_plat] is null) then 1 else 0 end = 0
	or case when (A.[is_discount] = B.[is_discount]) or (A.[is_discount] is null and B.[is_discount] is null) then 1 else 0 end = 0
	or case when (A.[zvilneno_percent] = B.[zvilneno_percent]) or (A.[zvilneno_percent] is null and B.[zvilneno_percent] is null) then 1 else 0 end = 0
	or case when (A.[zvilneno_date1] = B.[zvilneno_date1]) or (A.[zvilneno_date1] is null and B.[zvilneno_date1] is null) then 1 else 0 end = 0
	or case when (A.[zvilneno_date2] = B.[zvilneno_date2]) or (A.[zvilneno_date2] is null and B.[zvilneno_date2] is null) then 1 else 0 end = 0
	or case when (A.[povidoleno1_date] = B.[povidoleno1_date]) or (A.[povidoleno1_date] is null and B.[povidoleno1_date] is null) then 1 else 0 end = 0
	or case when (A.[povidoleno1_num] = B.[povidoleno1_num]) or (A.[povidoleno1_num] is null and B.[povidoleno1_num] is null) then 1 else 0 end = 0
	or case when (A.[povidoleno2_date] = B.[povidoleno2_date]) or (A.[povidoleno2_date] is null and B.[povidoleno2_date] is null) then 1 else 0 end = 0
	or case when (A.[povidoleno2_num] = B.[povidoleno2_num]) or (A.[povidoleno2_num] is null and B.[povidoleno2_num] is null) then 1 else 0 end = 0
	or case when (A.[povidoleno3_date] = B.[povidoleno3_date]) or (A.[povidoleno3_date] is null and B.[povidoleno3_date] is null) then 1 else 0 end = 0
	or case when (A.[povidoleno3_num] = B.[povidoleno3_num]) or (A.[povidoleno3_num] is null and B.[povidoleno3_num] is null) then 1 else 0 end = 0
	or case when (A.[povidoleno4_date] = B.[povidoleno4_date]) or (A.[povidoleno4_date] is null and B.[povidoleno4_date] is null) then 1 else 0 end = 0
	or case when (A.[povidoleno4_num] = B.[povidoleno4_num]) or (A.[povidoleno4_num] is null and B.[povidoleno4_num] is null) then 1 else 0 end = 0
	or case when (A.[zvilbykmp_percent] = B.[zvilbykmp_percent]) or (A.[zvilbykmp_percent] is null and B.[zvilbykmp_percent] is null) then 1 else 0 end = 0
	or case when (A.[zvilbykmp_date1] = B.[zvilbykmp_date1]) or (A.[zvilbykmp_date1] is null and B.[zvilbykmp_date1] is null) then 1 else 0 end = 0
	or case when (A.[zvilbykmp_date2] = B.[zvilbykmp_date2]) or (A.[zvilbykmp_date2] is null and B.[zvilbykmp_date2] is null) then 1 else 0 end = 0
	or case when (A.[stanjuro] = B.[stanjuro]) or (A.[stanjuro] is null and B.[stanjuro] is null) then 1 else 0 end = 0
	or case when (A.[insurance_sum] = B.[insurance_sum]) or (A.[insurance_sum] is null and B.[insurance_sum] is null) then 1 else 0 end = 0
	or case when (A.[insurance_start] = B.[insurance_start]) or (A.[insurance_start] is null and B.[insurance_start] is null) then 1 else 0 end = 0
	or case when (A.[insurance_end] = B.[insurance_end]) or (A.[insurance_end] is null and B.[insurance_end] is null) then 1 else 0 end = 0
	or case when (A.[org_renter_director_email] = B.[org_renter_director_email]) or (A.[org_renter_director_email] is null and B.[org_renter_director_email] is null) then 1 else 0 end = 0
	or case when (A.[sphera_dialnosti] = B.[sphera_dialnosti]) or (A.[sphera_dialnosti] is null and B.[sphera_dialnosti] is null) then 1 else 0 end = 0
	or case when (A.[priznachennya] = B.[priznachennya]) or (A.[priznachennya] is null and B.[priznachennya] is null) then 1 else 0 end = 0
	or case when (A.[ex_reports1nf_arenda] = B.[ex_reports1nf_arenda]) or (A.[ex_reports1nf_arenda] is null and B.[ex_reports1nf_arenda] is null) then 1 else 0 end = 0
	or case when (A.[arenda_report_id] = B.[arenda_report_id]) or (A.[arenda_report_id] is null and B.[arenda_report_id] is null) then 1 else 0 end = 0
	or case when (A.[is_dpz_object] = B.[is_dpz_object]) or (A.[is_dpz_object] is null and B.[is_dpz_object] is null) then 1 else 0 end = 0
	or case when (A.[count_dogovor_objects] = B.[count_dogovor_objects]) or (A.[count_dogovor_objects] is null and B.[count_dogovor_objects] is null) then 1 else 0 end = 0
	or case when (A.[has_reports1nf_photos] = B.[has_reports1nf_photos]) or (A.[has_reports1nf_photos] is null and B.[has_reports1nf_photos] is null) then 1 else 0 end = 0
	or case when (A.[old_organ] = B.[old_organ]) or (A.[old_organ] is null and B.[old_organ] is null) then 1 else 0 end = 0
	or case when (A.[num_pozov_total] = B.[num_pozov_total]) or (A.[num_pozov_total] is null and B.[num_pozov_total] is null) then 1 else 0 end = 0
	or case when (A.[num_pozov_zadov_total] = B.[num_pozov_zadov_total]) or (A.[num_pozov_zadov_total] is null and B.[num_pozov_zadov_total] is null) then 1 else 0 end = 0
	or case when (A.[num_pozov_vikon_total] = B.[num_pozov_vikon_total]) or (A.[num_pozov_vikon_total] is null and B.[num_pozov_vikon_total] is null) then 1 else 0 end = 0
	or case when (A.[list_dogovor_objects] = B.[list_dogovor_objects]) or (A.[list_dogovor_objects] is null and B.[list_dogovor_objects] is null) then 1 else 0 end = 0
	or case when (A.[prozoro_number] = B.[prozoro_number]) or (A.[prozoro_number] is null and B.[prozoro_number] is null) then 1 else 0 end = 0
	or case when (A.[method_calc_name] = B.[method_calc_name]) or (A.[method_calc_name] is null and B.[method_calc_name] is null) then 1 else 0 end = 0
	or case when (A.[base_month] = B.[base_month]) or (A.[base_month] is null and B.[base_month] is null) then 1 else 0 end = 0
)
begin
	truncate TABLE [dbo].[reptab_RentAgreements]
	insert into [dbo].[reptab_RentAgreements] select * from #qqq A
end


	
END


-- exec [dbo].[reptab_RentAgreements_update]
-- select count_dogovor_objects,list_dogovor_objects,* from [reptab_RentAgreements] where list_dogovor_objects <> '' and count_dogovor_objects > 1
-- select prozoro_number,count_dogovor_objects,list_dogovor_objects,* from [reptab_RentAgreements] where prozoro_number <> ''
-- select * from [reptab_RentAgreements] where count_dogovor_objects > 0
-- select distinct method_calc_name from [reptab_RentAgreements]
-- select distinct base_month from [reptab_RentAgreements]
-- delete from reptab_RentAgreements
