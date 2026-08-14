
CREATE VIEW view_obj_right_akts
AS
SELECT * FROM documents WHERE (id IN (SELECT akt_id FROM object_rights))
