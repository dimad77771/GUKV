
CREATE VIEW [view_rent_not_submitted]
AS
WITH period AS (SELECT TOP 5 id, name FROM dict_rent_period ORDER BY period_end DESC)
SELECT
    arenda.org_balans_id organization_id,
    org.full_name,
    org.short_name,
    org.zkpo_code,
    org.sfera_upr_id AS 'org_sfera_upr_id',
    org.form_ownership_id AS 'org_form_ownership_id',
	org.addr_distr_new_id AS 'org_district_id',
    dict_org_occupation.name AS 'rent_occupation',
	org.director_email email,
	org.director_phone phone,
	org.director_fio responsible_fio,
    period.id AS 'period_id',
    period.name AS 'period_name'
FROM
	(select distinct org_balans_id from arenda where arenda.agreement_state in (1, 2, 3) and agreement_kind_id in (1, 3, 7, 8, 11, 18)) arenda
	cross join period
	left join
	(
		select distinct arenda.org_balans_id, period.id rent_period_id
		from arenda, period
		where exists(select 1 from arenda_payments where arenda_payments.arenda_id = arenda.id and arenda_payments.rent_period_id = period.id)
	) x
	ON arenda.org_balans_id = x.org_balans_id AND period.id = x.rent_period_id
    INNER JOIN organizations org ON org.id = arenda.org_balans_id
    LEFT OUTER JOIN dict_org_occupation on dict_org_occupation.id = org.occupation_id
WHERE x.org_balans_id is null
