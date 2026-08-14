
CREATE VIEW view_obj_right_docs
AS
SELECT * FROM documents WHERE (id IN (SELECT document_id FROM object_rights))
