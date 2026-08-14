CREATE VIEW [dbo].[view_balans]
AS
SELECT *
FROM
    view_balans_all
WHERE
    (is_deleted IS NULL) OR (is_deleted = 0)
