
CREATE VIEW [view_org_balans_totals]
AS
SELECT
    organization_id,
    COUNT(*) AS 'balans_obj_count',
    SUM(sqr_total) AS 'sqr_balans_total',
    SUM(sqr_non_habit) AS 'sqr_balans_non_habit'
FROM
    view_balans
GROUP BY
    organization_id
