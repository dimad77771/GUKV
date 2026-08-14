
CREATE VIEW view_obj_right_org_from
AS
SELECT * FROM view_organizations WHERE (organization_id IN (SELECT org_from_id FROM object_rights))
