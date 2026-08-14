
CREATE VIEW view_obj_right_rozp
AS
SELECT * FROM documents WHERE (id IN (SELECT rozp_id FROM object_rights))
