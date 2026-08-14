
CREATE VIEW [view_arenda_privat_docs]
AS
SELECT
    *
FROM
    view_arenda_agreements
WHERE
    ((is_privat_int = 1) OR
     (agreement_kind_id = 9) OR
     (agreement_kind_id = 10) OR
     (agreement_kind_id = 16) OR
     (agreement_kind_id = 17))
