select
org_balans_full_name as [Балансоутримувач - Повна Назва],
org_balans_short_name as [Балансоутримувач - Коротка Назва],
org_balans_zkpo as [Балансоутримувач - Код ЄДРПОУ],
org_balans_industry as [Балансоутримувач - Галузь],
org_balans_occupation as [Балансоутримувач - Вид Діяльності],
org_renter_full_name as [Орендар - Повна Назва],
org_renter_short_name as [Орендар - Коротка Назва],
org_renter_zkpo as [Орендар - Код ЄДРПОУ],
org_renter_industry as [Орендар - Галузь],
org_renter_occupation as [Орендар - Вид Діяльності],
org_giver_short_name as [Орендодавець - Коротка Назва],
org_giver_zkpo as [Орендодавець - Код ЄДРПОУ],
district as [Район],
street_full_name as [Назва Вулиці],
addr_nomer as [Номер Будинку],
agreement_date as [Дата укладання договору],
agreement_num as [Номер Договору Оренди],
priznachennya as [Призначення за Документом],
n_cost_agreement as [Плата за використання, грн.],
cost_agreement_max as [Максимальна Орендна Плата за об'єкт договору (грн.)],
n_cost_expert_total as [Ринкова вартість приміщень, грн],
rent_start_date as [Початок Оренди],
rent_finish_date as [Закінчення Оренди],
rent_actual_finish_date as [Фактичне Закінчення Оренди],
rent_square as [Площа (кв.м.)],
is_subarenda as [Суборенда],
payment_type as [Вид Розрахунків],
agreement_active_s as [Стан договору],
org_balans_vedomstvo as [Балансоутримувач - Орган Управління],
org_renter_vedomstvo as [Орендар - Орган Управління],
stanjuro as [Балансоутримувач - стан юр. особи],
org_balans_form_ownership as [Балансоутримувач - Форма Власності],
org_balans_org_form as [Балансоутримувач - Організаційно-правова Форма],
org_renter_org_form as [Орендар - Організаційно-правова Форма],
contribution_rate as [Ставка відрахувань до бюджету (%)],
modify_date as [Дата Актуальності],
payment_narah as [Нараховано орендної плати за звітний період, грн. (без ПДВ)],
payment_received as [Надходження орендної плати за звітний період, всього, грн. (без ПДВ)],
payment_nar_zvit as [- у тому числі, з нарахованої за звітний період (без боргів та переплат)],
debt_total as [Загальна заборгованість по орендній платі - всього],
debt_3_month as [Заборгованість по орендній платі поточна до 3-х місяців],
debt_12_month as [Заборгованість по орендній платі прострочена від 4 до 12 місяців],
debt_3_years as [Заборгованість по орендній платі прострочена від 1 до 3 років],
sphera_dialnosti as [Сфера діяльності],
org_renter_form_of_ownership as [Орендар - Форма власності]
from
(
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
 	    ar.id in (select b.id from dbo.reports1nf_arenda b where b.org_balans_id = ar.org_balans_id and ISNULL(b.is_deleted, 0) = 0 )
) T