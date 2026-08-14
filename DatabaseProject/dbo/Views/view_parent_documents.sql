
CREATE VIEW [view_parent_documents]
AS
SELECT * FROM documents WHERE (id IN (SELECT document_id FROM building_docs))
