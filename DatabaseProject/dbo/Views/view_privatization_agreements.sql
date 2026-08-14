
CREATE VIEW [view_privatization_agreements]
AS
SELECT
    pd.privatization_id,
    pd.document_id,
    doc.doc_date,
    doc.doc_num,
    doc.topic,
    doc.search_name,
    doc.is_text_exists
FROM
    priv_object_docs pd
    INNER JOIN view_documents doc ON pd.document_id = doc.id
WHERE
    doc.kind_id IN (27,35)