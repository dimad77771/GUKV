CREATE VIEW [dbo].[view_arenda]
AS
SELECT
    vab.*
FROM
    view_arenda_base vab
--WHERE
--    (vab.is_privat_int <> 1 OR vab.is_privat_int IS NULL) AND
--    (vab.agreement_kind_id IS NULL OR
--    ((vab.agreement_kind_id <> 9) AND
--     (vab.agreement_kind_id <> 10) AND
--     (vab.agreement_kind_id <> 16) AND
--     (vab.agreement_kind_id <> 17)))
