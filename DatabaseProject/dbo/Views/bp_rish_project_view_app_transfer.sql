
CREATE VIEW bp_rish_project_view_app_transfer
AS
SELECT
    tr.id AS 'transfer_id',
    tr.appendix_id AS 'appendix_id',
    org_from.full_name AS 'org_from_name',
    org_to.full_name AS 'org_to_name',
    dict_obj_rights.name AS 'right_name'
FROM
    bp_rish_project_app_transfer tr
    LEFT OUTER JOIN organizations org_from ON org_from.id = tr.org_from_id
    LEFT OUTER JOIN organizations org_to ON org_to.id = tr.org_to_id
    LEFT OUTER JOIN dict_obj_rights ON tr.right_id = dict_obj_rights.id
