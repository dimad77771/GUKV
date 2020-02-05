SELECT
per.name as 'rent_period_name',
occ.name as 'occupation_name',
ap.* 
FROM [view_arenda_payments_by_balanser] ap
join dict_rent_period per on per.id = ap.rent_period_id
join organizations bal_org on bal_org.id=ap.org_balans_id
join DictSummaryReportSphere occ on occ.id=bal_org.old_occupation_id
where occ.id=1 and per.id=27