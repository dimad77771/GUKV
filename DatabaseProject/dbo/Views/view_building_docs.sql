
CREATE VIEW [view_building_docs]
AS
SELECT
    bd.id AS 'link_id',
    bd.building_id,
    bd.document_id,
    /* bd.master_doc_id, */
    dict_tech_state.name AS 'condition',
    dict_object_type.name AS 'object_type',
    dict_object_kind.name AS 'object_kind',
    dict_balans_purpose_group.name AS 'purpose_group',
    dict_balans_purpose.name AS 'purpose',
    bd.purpose_group_id,
    bd.purpose_id,
    bd.obj_name,
    bd.obj_description,
    bd.obj_build_year,
    bd.obj_expl_enter_year,
    bd.obj_length,
    bd.note,
    bd.cost_balans,
    bd.cost_znos,
    bd.cost_zalishkova,
    bd.cost_expert,
    bd.num_rooms,
    bd.num_floors,
    bd.obj_location,
    bd.sqr_obj,
    bd.sqr_free,
    bd.sqr_habit,
    bd.sqr_non_habit,
    bd.is_not_in_work,
    bd.modified_by,
    bd.modify_date,
    bd.doc_dodatok,
    bd.doc_tab,
    bd.doc_pos,
    dict_doc_change_type.name AS 'doc_change_type',
    bd.doc_change_num,
    bd.doc_change_date,
    bd.doc_change_doc_id,
    dict_org_ownership.name AS 'form_ownership',
    bd.is_on_balans,
    bd.org_id,
    bd.last_org_id,
    bd.date_expert,
    bd.date_priv,
    bd.ozn_priv_id,
    bd.pipe_diameter,
    bd.pipe_material
FROM
    building_docs bd
    LEFT OUTER JOIN dict_tech_state ON bd.tech_condition_id = dict_tech_state.id
    LEFT OUTER JOIN dict_object_kind ON bd.object_kind_id = dict_object_kind.id
    LEFT OUTER JOIN dict_object_type ON bd.object_type_id = dict_object_type.id
    LEFT OUTER JOIN dict_balans_purpose_group ON bd.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON bd.purpose_id = dict_balans_purpose.id
    LEFT OUTER JOIN dict_org_ownership ON bd.form_ownership_id = dict_org_ownership.id
    LEFT OUTER JOIN dict_doc_change_type ON bd.doc_change_type_id = dict_doc_change_type.id
