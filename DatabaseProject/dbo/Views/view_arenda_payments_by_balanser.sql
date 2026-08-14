
CREATE VIEW [view_arenda_payments_by_balanser]
AS

select 
a.org_renter_id,
--a.org_renter_id,
--a.org_giver_id,
--bal_org.zkpo_code as 'balans_org_zkpo',
--bal_org.occupation_id as 'balans_org_occupation_id',
--occ.name as 'balans_org_occupation_name',
ap.rent_period_id,
--per.name as 'rent_period_name',
count(a.id) as 'arenda_count',
sum(ap.[sqr_total_rent]) as 'sqr_total_rent',
sum(ap.[sqr_payed_by_percent]) as 'sqr_payed_by_percent',
sum(ap.[sqr_payed_by_1uah]) as 'sqr_payed_by_1uah',
sum(ap.[sqr_payed_hourly]) as 'sqr_payed_hourly',
sum(ap.[payment_narah]) as 'payment_narah',
sum(ap.[last_year_saldo]) as 'last_year_saldo',
sum(ap.[payment_received]) as 'payment_received',
sum(ap.[payment_nar_zvit]) as 'payment_nar_zvit',
sum(ap.[payment_budget_special]) as 'payment_budget_special',
sum(ap.[debt_total]) as 'debt_total',
sum(ap.[debt_zvit]) as 'debt_zvit',
sum(ap.[debt_3_month]) as 'debt_3_month',
sum(ap.[debt_12_month]) as 'debt_12_month',
sum(ap.[debt_3_years]) as 'debt_3_years',
sum(ap.[debt_over_3_years]) as 'debt_over_3_years',
sum(ap.[debt_v_mezhah_vitrat]) as 'debt_v_mezhah_vitrat',
sum(ap.[debt_spysano]) as 'debt_spysano',
sum(ap.[num_zahodiv_total]) as 'num_zahodiv_total',
sum(ap.[num_zahodiv_zvit]) as 'num_zahodiv_zvit',
sum(ap.[num_pozov_total]) as 'num_pozov_total',
sum(ap.[num_pozov_zvit]) as 'num_pozov_zvit',
sum(ap.[num_pozov_zadov_total]) as 'num_pozov_zadov_total',
sum(ap.[num_pozov_zadov_zvit]) as 'num_pozov_zadov_zvit',
sum(ap.[num_pozov_vikon_total]) as 'num_pozov_vikon_total',
sum(ap.[num_pozov_vikon_zvit]) as 'num_pozov_vikon_zvit',
sum(ap.[debt_pogasheno_total]) as 'debt_pogasheno_total',
sum(ap.[debt_pogasheno_zvit]) as 'debt_pogasheno_zvit',
sum(ap.[budget_narah_50_uah]) as 'budget_narah_50_uah',
sum(ap.[budget_zvit_50_uah]) as 'budget_zvit_50_uah',
sum(ap.[budget_prev_50_uah]) as 'budget_prev_50_uah',
sum(ap.[budget_debt_50_uah]) as 'budget_debt_50_uah',
sum(ap.[budget_debt_30_50_uah]) as 'budget_debt_30_50_uah',
sum(ap.[old_debts_payed]) as 'old_debts_payed'
from arenda a
LEFT OUTER JOIN reports1nf_arenda a1 on a1.id = a.id
left outer join arenda_payments ap on ap.arenda_id=a.id
--join dict_rent_period per on per.id = ap.rent_period_id
--join organizations bal_org on bal_org.id=a.org_balans_id
--join dict_org_occupation occ on occ.id=bal_org.occupation_id
--where 
--a.org_balans_id=1
--order by ap.rent_period_id
--ap.rent_period_id=27
where 
a.agreement_state = 1 and (a1.is_deleted IS NULL OR a1.is_deleted = 0)
group by
a.org_renter_id,
--a.org_renter_id,
--a.org_giver_id,
--bal_org.zkpo_code,
--bal_org.occupation_id,
--occ.name,
ap.rent_period_id--,
--per.name
