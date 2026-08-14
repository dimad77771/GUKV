
CREATE VIEW [view_docs_objects_akts]
AS
SELECT
    ab.document_id,
    ab.doc_date,
    YEAR(ab.doc_date) AS 'doc_date_year',
    DATEPART(Quarter, ab.doc_date)  AS 'doc_date_quarter',
    ab.doc_num,
    ab.topic,
    ab.doc_note,
    dict_doc_kind.name AS 'doc_kind',
    CASE WHEN ((dict_doc_kind.name LIKE 'РОЗПОРЯДЖЕННЯ%') OR (dict_doc_kind.name LIKE 'РІШЕННЯ%')) THEN N'ТАК' ELSE N'НІ' END AS 'doc_is_rozp',
    dict_doc_general_kind.name AS 'general_kind',
    dict_doc_commission.name AS 'commission_name',
    dict_doc_source.name AS 'doc_source',
    dict_doc_state.name AS 'doc_state',
    ab.search_name,
    ab.receive_date,
    ab.summa,
    ab.summa_zalishkova,
    ab.doc_text_exists,
    ab.link_id,
    ab.building_id,
    b.district,
    b.street_full_name,
    b.addr_nomer,
    ab.obj_name,
    ab.obj_description,
    ab.obj_build_year,
    ab.obj_length,
    ab.object_note,
    ab.cost_balans,
    ab.cost_znos,
    ab.cost_zalishkova,
    ab.cost_expert,
    ab.num_rooms,
    ab.num_floors,
    ab.obj_location,
    ab.purpose_group,
    ab.purpose,
    ab.condition,
    ab.object_kind,
    ab.object_type,
    ab.sqr_obj,
    ab.sqr_free,
    ab.sqr_habit,
    ab.sqr_non_habit,
    ab.form_ownership,
    ab.pipe_diameter,
    ab.pipe_material,
    ab.akt_exists,
    ab.akt_id,
    ab.akt_num,
    ab.akt_date,
    YEAR(ab.akt_date) AS 'akt_date_year',
    DATEPART(Quarter, ab.akt_date)  AS 'akt_date_quarter',
    ab.akt_topic,
    ab.akt_search_name,
    ab.akt_text_exists
FROM
    view_docs_objects_akts_base ab
    LEFT OUTER JOIN dict_doc_kind ON dict_doc_kind.id = ab.kind_id
    LEFT OUTER JOIN dict_doc_general_kind ON dict_doc_general_kind.id = ab.general_kind_id
    LEFT OUTER JOIN dict_doc_commission ON dict_doc_commission.id = ab.commission_id
    LEFT OUTER JOIN dict_doc_source ON dict_doc_source.id = ab.source_id
    LEFT OUTER JOIN dict_doc_state ON dict_doc_state.id = ab.state_id
    LEFT OUTER JOIN view_buildings b ON ab.building_id = b.building_id
