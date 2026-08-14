
CREATE VIEW view_obj_right_doc_links
AS
SELECT * FROM view_doc_links WHERE (slave_doc_id IN (SELECT document_id FROM object_rights))
