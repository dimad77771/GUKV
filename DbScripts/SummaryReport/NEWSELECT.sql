select 
ap.rent_period_id,
per.name as 'rent_period_name',
occ.id as 'occupation_id',
occ.name as 'occupation_name',
count(ap.id) as 'arenda_count',
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
from arenda_payments ap
join arch_arenda a on ap.arenda_id=a.id and a.archive_link_code = ap.arenda_archive_link_code and a.id = ap.arenda_id
join org_by_period obp on obp.period_id = ap.rent_period_id and obp.org_id = a.org_balans_id
join dict_rent_period per on per.id = ap.rent_period_id
join dict_rent_occupation occ on occ.id=obp.org_occupation_id
where 
(a.agreement_state = 1 ) and (a.is_deleted is null OR a.is_deleted = 0)
and a.org_balans_id = 1 and ap.rent_period_id = 29 
group by
ap.rent_period_id,
per.name,
occ.id,
occ.name