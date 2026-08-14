
CREATE VIEW view_obj_right_organizations
AS
SELECT * FROM view_organizations WHERE
    (organization_id IN (SELECT org_from_id FROM object_rights)) OR
    (organization_id IN (SELECT org_to_id FROM object_rights))
