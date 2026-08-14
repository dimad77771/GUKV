
CREATE VIEW view_rent_debt_for_arenda
AS
SELECT
    d.rent_period_id,
    renter.organization_id,
    SUM(d.debt_total) AS 'debt_total'
FROM
    rent_payment_debt d
    LEFT OUTER JOIN rent_renter_org renter ON renter.id = d.rent_renter_org_id
GROUP BY
    d.rent_period_id,
    renter.organization_id
