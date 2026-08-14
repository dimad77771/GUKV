
CREATE VIEW view_rent_pay_for_arenda
AS
SELECT
    pbr.rent_period_id,
    renter.organization_id,
    SUM(pbr.payment_narah) AS 'payment_narah',
    SUM(pbr.payment_received) AS 'payment_received'
FROM
    rent_payment_by_renter pbr
    LEFT OUTER JOIN rent_renter_org renter ON renter.id = pbr.rent_renter_org_id
GROUP BY
    pbr.rent_period_id,
    renter.organization_id
