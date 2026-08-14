
CREATE VIEW [view_rent_by_occupation_inactive]
AS
SELECT
    rent_period_id,
    rent_period,
    bal_org_occupation_id,
    bal_org_occupation,
    SUM(sqr_total_rent) AS 'sqr_total_rent',
    SUM(sqr_payed_by_percent) AS 'sqr_payed_by_percent',
    SUM(sqr_payed_by_1uah) AS 'sqr_payed_by_1uah',
    SUM(sqr_payed_hourly) AS 'sqr_payed_hourly',
    SUM(payment_narah) AS 'payment_narah',
    SUM(payment_received) AS 'payment_received',
    SUM(payment_budget_50_uah) AS 'payment_budget_50_uah',
    SUM(debt_total) AS 'debt_total',
    SUM(debt_3_month) AS 'debt_3_month',
    SUM(debt_12_month) AS 'debt_12_month',
    SUM(debt_3_years) AS 'debt_3_years',
    SUM(debt_over_3_years) AS 'debt_over_3_years',
    SUM(debt_spysano) AS 'debt_spysano',
    SUM(num_zahodiv_total) AS 'num_zahodiv_total',
    SUM(num_zahodiv_zvit) AS 'num_zahodiv_zvit',
    SUM(num_pozov_total) AS 'num_pozov_total',
    SUM(num_pozov_zvit) AS 'num_pozov_zvit',
    SUM(num_pozov_zadov_total) AS 'num_pozov_zadov_total',
    SUM(num_pozov_zadov_zvit) AS 'num_pozov_zadov_zvit',
    SUM(num_pozov_vikon_total) AS 'num_pozov_vikon_total',
    SUM(num_pozov_vikon_zvit) AS 'num_pozov_vikon_zvit',
    SUM(debt_pogasheno_total) AS 'debt_pogasheno_total',
    SUM(debt_pogasheno_zvit) AS 'debt_pogasheno_zvit',
    SUM(debt_v_mezhah_vitrat) AS 'debt_v_mezhah_vitrat'
FROM
    view_rent_by_renters
WHERE
    rent_agreement_active_int <> 1
GROUP BY
    rent_period_id,
    rent_period,
    bal_org_occupation_id,
    bal_org_occupation
