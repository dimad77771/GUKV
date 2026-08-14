
CREATE VIEW [view_rent_obj_grouped]
AS
SELECT
    rent_payment_id,
    SUM(sqr_total) AS 'sqr_total',
    SUM(sqr_rented) AS 'sqr_rented',
    SUM(sqr_free) AS 'sqr_free',
    SUM(sqr_korysna) AS 'sqr_korysna',
    SUM(sqr_mzk) AS 'sqr_mzk'
FROM
    rent_object
GROUP BY
    rent_payment_id
