
CREATE VIEW view_obj_right_org_to
AS
SELECT * FROM view_organizations WHERE (organization_id IN (SELECT org_to_id FROM object_rights))
