/* view_rent_by_renters */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_by_renters]'))
DROP VIEW [dbo].[view_rent_by_renters]

GO

CREATE VIEW [view_rent_by_renters]
AS
SELECT
    pbr.id AS 'rent_id',
    pbr.rent_payment_id,
    pbr.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    pbr.rent_renter_org_id,
    pbr.renter_organization_id,
    renter.name AS 'renter_name',
    renter.zkpo_code AS 'renter_zkpo',
    bal_org.id AS 'bal_org_id',
    bal_org.full_name AS 'bal_org_full_name',
    bal_org.short_name AS 'bal_org_short_name',
    bal_org.zkpo_code AS 'bal_org_zkpo',
    bal_org.sfera_upr_id AS 'bal_org_sfera_upr_id',
    bal_org.form_ownership_id AS 'bal_org_form_ownership_id',
    bal_org.addr_distr_new_id AS 'bal_org_district_id',
    rent_balans_org.rent_occupation_id AS 'bal_org_occupation_id',
    dict_rent_occupation.name AS 'bal_org_occupation',
    giver_org.id AS 'giver_org_id',
    giver_org.full_name AS 'giver_org_full_name',
    giver_org.short_name AS 'giver_org_short_name',
    giver_org.zkpo_code AS 'giver_org_zkpo',
    giver_org.sfera_upr_id AS 'giver_org_sfera_upr_id',
    giver_org.form_ownership_id AS 'giver_org_form_ownership_id',
    giver_org.addr_distr_new_id AS 'giver_org_district_id',
    pbr.sqr_total_rent,
    pbr.sqr_payed_by_percent,
    pbr.sqr_payed_by_1uah,
    pbr.sqr_payed_hourly,
    pbr.payment_narah,
    pbr.payment_received,
    pbr.payment_budget_50_uah,

    0 as 'debt_total',
    0 as 'debt_3_month',
    0 as 'debt_12_month',
    0 as 'debt_3_years',
    0 as 'debt_over_3_years',
    0 as 'debt_spysano',
    0 as 'num_zahodiv_total',
    0 as 'num_zahodiv_zvit',
    0 as 'num_pozov_total',
    0 as 'num_pozov_zvit',
    0 as 'num_pozov_zadov_total',
    0 as 'num_pozov_zadov_zvit',
    0 as 'num_pozov_vikon_total',
    0 as 'num_pozov_vikon_zvit',
    0 as 'debt_pogasheno_total',
    0 as 'debt_pogasheno_zvit',
    0 as 'debt_v_mezhah_vitrat',
	
	--debt.debt_total,
    --debt.debt_3_month,
    --debt.debt_12_month,
    --debt.debt_3_years,
    --debt.debt_over_3_years,
    --debt.debt_spysano,
    --debt.num_zahodiv_total,
    --debt.num_zahodiv_zvit,
    --debt.num_pozov_total,
    --debt.num_pozov_zvit,
    --debt.num_pozov_zadov_total,
    --debt.num_pozov_zadov_zvit,
    --debt.num_pozov_vikon_total,
    --debt.num_pozov_vikon_zvit,
    --debt.debt_pogasheno_total,
    --debt.debt_pogasheno_zvit,
    --debt.debt_v_mezhah_vitrat,
    CASE WHEN pbr.agreement_flag = 1 THEN N'“¿ ' ELSE N'Õ≤' END AS 'rent_agreement_active',
    pbr.agreement_flag AS 'rent_agreement_active_int'
FROM
    rent_payment_by_renter pbr
    INNER JOIN rent_payment pay ON pay.id = pbr.rent_payment_id
    LEFT OUTER JOIN organizations bal_org ON bal_org.id = pay.org_balans_id
    LEFT OUTER JOIN organizations giver_org ON giver_org.id = pbr.giver_organization_id
    LEFT OUTER JOIN rent_balans_org ON rent_balans_org.organization_id = pay.org_balans_id
    LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = rent_balans_org.rent_occupation_id
    LEFT OUTER JOIN dict_rent_period ON dict_rent_period.id = pbr.rent_period_id
    LEFT OUTER JOIN rent_renter_org renter ON renter.id = pbr.rent_renter_org_id
  --  LEFT OUTER JOIN view_rent_debt_by_renter debt ON
  --      debt.rent_payment_id = pbr.rent_payment_id AND
		--debt.rent_renter_org_id = pbr.rent_renter_org_id AND
		--debt.agreement_flag = pbr.agreement_flag

UNION ALL

SELECT
    -pbr.id AS 'rent_id',
    -pbr.id rent_payment_id,
    pbr.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    pbr.org_renter_id rent_renter_org_id,
    pbr.org_renter_id renter_organization_id,
    renter_org.full_name AS 'renter_name',
    renter_org.zkpo_code AS 'renter_zkpo',
    bal_org.id AS 'bal_org_id',
    bal_org.full_name AS 'bal_org_full_name',
    bal_org.short_name AS 'bal_org_short_name',
    bal_org.zkpo_code AS 'bal_org_zkpo',
    bal_org.sfera_upr_id AS 'bal_org_sfera_upr_id',
    bal_org.form_ownership_id AS 'bal_org_form_ownership_id',
    bal_org.addr_distr_new_id AS 'bal_org_district_id',
    bal_org.occupation_id AS 'bal_org_occupation_id',
    dict_org_occupation.name AS 'bal_org_occupation',
    giver_org.id AS 'giver_org_id',
    giver_org.full_name AS 'giver_org_full_name',
    giver_org.short_name AS 'giver_org_short_name',
    giver_org.zkpo_code AS 'giver_org_zkpo',
    giver_org.sfera_upr_id AS 'giver_org_sfera_upr_id',
    giver_org.form_ownership_id AS 'giver_org_form_ownership_id',
    giver_org.addr_distr_new_id AS 'giver_org_district_id',
    pbr.sqr_total_rent,
    pbr.sqr_payed_by_percent,
    pbr.sqr_payed_by_1uah,
    pbr.sqr_payed_hourly,
    pbr.payment_narah,
    pbr.payment_received,
	NULL, --renter_org.budget_narah_50_uah, --pbr.payment_budget_50_uah,
    pbr.debt_total,
    pbr.debt_3_month,
    pbr.debt_12_month,
    pbr.debt_3_years,
    pbr.debt_over_3_years,
    pbr.debt_spysano,
    pbr.num_zahodiv_total,
    pbr.num_zahodiv_zvit,
    pbr.num_pozov_total,
    pbr.num_pozov_zvit,
    pbr.num_pozov_zadov_total,
    pbr.num_pozov_zadov_zvit,
    pbr.num_pozov_vikon_total,
    pbr.num_pozov_vikon_zvit,
    pbr.debt_pogasheno_total,
    pbr.debt_pogasheno_zvit,
    pbr.debt_v_mezhah_vitrat,
    CASE WHEN pbr.agreement_state = 1 THEN N'“¿ ' ELSE N'Õ≤' END AS 'rent_agreement_active',
    pbr.agreement_state AS 'rent_agreement_active_int'
FROM
	(SELECT a.org_balans_id
			,a.org_giver_id
			,a.org_renter_id
			,p.rent_period_id
			,max(p.id) id
			,max(p.modified_by) modified_by
			,sum(sqr_total_rent) sqr_total_rent
			,sum(sqr_payed_by_percent) sqr_payed_by_percent
			,sum(sqr_payed_by_1uah) sqr_payed_by_1uah
			,sum(sqr_payed_hourly) sqr_payed_hourly
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
			,max(a.agreement_state) agreement_state
		FROM arenda_payments p
		INNER JOIN arenda a ON a.id = p.arenda_id
		GROUP BY a.org_balans_id, a.org_giver_id, a.org_renter_id, p.rent_period_id) pbr
    LEFT OUTER JOIN organizations bal_org ON bal_org.id = pbr.org_balans_id
    LEFT OUTER JOIN organizations giver_org ON giver_org.id = pbr.org_giver_id
    LEFT OUTER JOIN organizations renter_org ON renter_org.id = pbr.org_renter_id
	LEFT JOIN dict_org_occupation ON dict_org_occupation.id = bal_org.occupation_id
    --LEFT OUTER JOIN rent_balans_org ON rent_balans_org.organization_id = pbr.org_balans_id
    --LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = rent_balans_org.rent_occupation_id
    LEFT OUTER JOIN dict_rent_period ON dict_rent_period.id = pbr.rent_period_id

GO
