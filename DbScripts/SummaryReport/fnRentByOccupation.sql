/* fnRentByOccupation - stored procedure that mimics the 'view_rent_by_occupation' view, plus additional logic */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnRentByOccupation]'))
	DROP PROCEDURE [dbo].[fnRentByOccupation]
GO

CREATE PROCEDURE dbo.fnRentByOccupation
(	
	@RDA_DISTRICT_ID INTEGER,
	@RENT_AGREEMENT_ACTIVE INTEGER /* 0 - don't care, 1 - active, 2 - inactive */
)
AS
	Declare @temp_table table
	(
		rent_period_id INTEGER,
		rent_period VARCHAR(64),
		bal_org_occupation_id INTEGER,
		bal_org_occupation  VARCHAR(128),
		sqr_total_rent NUMERIC(15,3),
		sqr_payed_by_percent NUMERIC(15,3),
		sqr_payed_by_1uah NUMERIC(15,3),
		sqr_payed_hourly NUMERIC(15,3),
		payment_narah NUMERIC(15,3),
		payment_received NUMERIC(15,3),
		payment_budget_50_uah NUMERIC(15,3),
		debt_total NUMERIC(15,3),
		debt_3_month NUMERIC(15,3),
		debt_12_month NUMERIC(15,3),
		debt_3_years NUMERIC(15,3),
		debt_over_3_years NUMERIC(15,3),
		debt_spysano NUMERIC(15,3),
		num_zahodiv_total NUMERIC(15,3),
		num_zahodiv_zvit NUMERIC(15,3),
		num_pozov_total NUMERIC(15,3),
		num_pozov_zvit NUMERIC(15,3),
		num_pozov_zadov_total NUMERIC(15,3),
		num_pozov_zadov_zvit NUMERIC(15,3),
		num_pozov_vikon_total NUMERIC(15,3),
		num_pozov_vikon_zvit NUMERIC(15,3),
		debt_pogasheno_total NUMERIC(15,3),
		debt_pogasheno_zvit NUMERIC(15,3),
		debt_v_mezhah_vitrat NUMERIC(15,3)
	)

	INSERT INTO @temp_table
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
		( @RDA_DISTRICT_ID = 0 OR
          (bal_org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND bal_org_district_id = @RDA_DISTRICT_ID) OR
          (giver_org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND giver_org_district_id = @RDA_DISTRICT_ID) )
        AND
        ( @RENT_AGREEMENT_ACTIVE = 0 OR
          (@RENT_AGREEMENT_ACTIVE = 1 AND rent_agreement_active_int = 1) OR
          (@RENT_AGREEMENT_ACTIVE = 2 AND rent_agreement_active_int <> 1) )
	GROUP BY
		rent_period_id,
		rent_period,
		bal_org_occupation_id,
		bal_org_occupation
		
    /**/
    
    Declare @temp_table2 table
	(
		rent_period_id INTEGER,
		rent_occupation_id INTEGER,
		total_num_renters INTEGER
	)
	
	INSERT INTO @temp_table2
    SELECT
		pay.rent_period_id,
		bal_org.rent_occupation_id,
		SUM(pay.num_renters) AS 'total_num_renters'
	FROM
		rent_payment pay
		LEFT OUTER JOIN rent_balans_org bal_org ON bal_org.organization_id = pay.org_balans_id
		LEFT OUTER JOIN organizations org ON org.id = pay.org_balans_id
	WHERE
		@RDA_DISTRICT_ID = 0 OR (org.form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND org.addr_distr_new_id = @RDA_DISTRICT_ID)
	GROUP BY
		pay.rent_period_id,
		bal_org.rent_occupation_id
    
    /**/
    
	SELECT
		rbo.*,
		tr.total_num_renters
	FROM
		@temp_table rbo
        LEFT OUTER JOIN @temp_table2 tr ON tr.rent_period_id = rbo.rent_period_id AND tr.rent_occupation_id = rbo.bal_org_occupation_id
GO
