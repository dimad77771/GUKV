
CREATE VIEW [view_privatization_doc_objects]
AS
SELECT
    b_doc.id AS 'obj_link_id',
    b_doc.document_id AS 'slave_doc_id',
    b_doc.master_doc_id AS 'master_doc_id',
    b_doc.building_id,
    p.privatization_id,
    p.organization_id,
    p.district,
    p.street_full_name,
    p.addr_nomer,
    p.org_name,
    p.sqr_total,
    p.obj_group,
    p.privat_kind,
    p.privat_state
FROM
    building_docs AS b_doc
    INNER JOIN view_privatization AS p ON  b_doc.building_id = p.building_id
