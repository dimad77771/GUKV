/* view_rent_debt_by_renter */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_debt_by_renter]'))
DROP VIEW [dbo].[view_rent_debt_by_renter]

GO

CREATE VIEW [view_rent_debt_by_renter]
AS
SELECT
    rent_payment_id,
    rent_renter_org_id,
    agreement_flag,
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
    rent_payment_debt
GROUP BY
    rent_payment_id,
    rent_renter_org_id,
    agreement_flag

GO
