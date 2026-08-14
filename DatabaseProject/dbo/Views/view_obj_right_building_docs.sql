
CREATE VIEW view_obj_right_building_docs
AS
SELECT * FROM view_building_docs WHERE (document_id IN (SELECT document_id FROM object_rights))
