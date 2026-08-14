
CREATE VIEW [view_docs_objects_akts_base]
AS
SELECT
    doc.id AS 'document_id',
    doc.doc_date,
    doc.doc_num,
    doc.topic,
    doc.note AS 'doc_note',
    doc.kind_id,
    doc.general_kind_id,
    doc.commission_id,
    doc.source_id,
    doc.state_id,
    doc.search_name,
    doc.receive_date,
    doc.summa,
    doc.summa_zalishkova,
    doc.is_text_exists AS 'doc_text_exists',
    bd.building_id,
    bd.link_id,
    bd.obj_name,
    bd.obj_description,
    bd.obj_build_year,
    bd.obj_length,
    bd.note AS 'object_note',
    bd.cost_balans,
    bd.cost_znos,
    bd.cost_zalishkova,
    bd.cost_expert,
    bd.num_rooms,
    bd.num_floors,
    bd.obj_location,
    bd.purpose_group,
    bd.purpose,
    bd.condition,
    bd.object_kind,
    bd.object_type,
    bd.sqr_obj,
    bd.sqr_free,
    bd.sqr_habit,
    bd.sqr_non_habit,
    bd.form_ownership,
    bd.pipe_diameter,
    bd.pipe_material,
    'ТАК' AS 'akt_exists',
    base1.slave_doc_id AS 'akt_id',
    base1.child_num AS 'akt_num',
    base1.child_date AS 'akt_date',
    base1.child_topic AS 'akt_topic',
    base1.child_search_name AS 'akt_search_name',
    base1.child_text_exists AS 'akt_text_exists'
FROM
    view_building_docs bd
    INNER JOIN documents doc ON doc.id = bd.document_id
    INNER JOIN view_docs_objects_akts_base1 base1 ON
        base1.akt_building_id = bd.building_id AND
        base1.master_doc_id = bd.document_id
UNION ALL
SELECT
    doc.id AS 'document_id',
    doc.doc_date,
    doc.doc_num,
    doc.topic,
    doc.note AS 'doc_note',
    doc.kind_id,
    doc.general_kind_id,
    doc.commission_id,
    doc.source_id,
    doc.state_id,
    doc.search_name,
    doc.receive_date,
    doc.summa,
    doc.summa_zalishkova,
    doc.is_text_exists AS 'doc_text_exists',
    bdp.building_id,
    bdp.link_id,
    bdp.obj_name,
    bdp.obj_description,
    bdp.obj_build_year,
    bdp.obj_length,
    bdp.note AS 'object_note',
    bdp.cost_balans,
    bdp.cost_znos,
    bdp.cost_zalishkova,
    bdp.cost_expert,
    bdp.num_rooms,
    bdp.num_floors,
    bdp.obj_location,
    bdp.purpose_group,
    bdp.purpose,
    bdp.condition,
    bdp.object_kind,
    bdp.object_type,
    bdp.sqr_obj,
    bdp.sqr_free,
    bdp.sqr_habit,
    bdp.sqr_non_habit,
    bdp.form_ownership,
    bdp.pipe_diameter,
    bdp.pipe_material,
    'НІ' AS 'akt_exists',
    NULL AS 'akt_id',
    NULL AS 'akt_num',
    NULL AS 'akt_date',
    NULL AS 'akt_topic',
    NULL AS 'akt_search_name',
    0 AS 'akt_text_exists'
FROM
    view_building_docs bdp
    INNER JOIN documents doc ON doc.id = bdp.document_id
WHERE
    (doc.kind_id <> 3) AND
    (doc.kind_id <> 36) AND
    (doc.kind_id <> 47) AND
    (NOT EXISTS (
        SELECT link_id FROM view_doc_links WHERE
            (master_doc_id = doc.id) AND
            ((child_kind = 3) OR (child_kind = 36) OR (child_kind = 47))
    ))
