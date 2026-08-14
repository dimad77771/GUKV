
CREATE VIEW [view_rent_total_renters]
AS
SELECT
    pay.rent_period_id,
    bal_org.rent_occupation_id,
    SUM(pay.num_renters) AS 'total_num_renters'
FROM
    rent_payment pay
    LEFT OUTER JOIN rent_balans_org bal_org ON bal_org.organization_id = pay.org_balans_id
GROUP BY
    pay.rent_period_id,
    bal_org.rent_occupation_id
