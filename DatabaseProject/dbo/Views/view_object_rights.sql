
CREATE VIEW view_object_rights
AS
SELECT
    r.id AS 'transfer_id',
    r.building_id,
    b.district,
    b.street_full_name,
    b.addr_nomer,
    dict_tech_state.name AS 'condition',
    bdoc.tech_condition_id AS 'condition_int',
    dict_object_type.name AS 'object_type',
    bdoc.object_type_id AS 'object_type_int',
    dict_object_kind.name AS 'object_kind',
    bdoc.object_kind_id AS 'object_kind_int',
    dict_balans_purpose_group.name AS 'purpose_group',
    bdoc.purpose_group_id AS 'purpose_group_int',
    dict_balans_purpose.name AS 'purpose',
    bdoc.purpose_id AS 'purpose_int',
    r.org_from_id,
    org_from.full_name AS 'org_from_full_name',
    org_from.short_name AS 'org_from_short_name',
    org_from.zkpo_code AS 'org_from_zkpo_code',
    org_from.industry AS 'org_from_industry',
    org_from.occupation AS 'org_from_occupation',
    org_from.form_of_ownership AS 'org_from_ownership',
    org_from.form_gosp AS 'org_from_form_gosp',
    org_from.vedomstvo AS 'org_from_vedomstvo',
    org_from.sfera_upr_id AS 'org_from_sfera_upr_id',
    org_from.form_of_ownership_int AS 'org_from_form_ownership_id',
    org_from.addr_distr_new_id AS 'org_from_district_id',
    r.org_to_id,
    org_to.full_name AS 'org_to_full_name',
    org_to.short_name AS 'org_to_short_name',
    org_to.zkpo_code AS 'org_to_zkpo_code',
    org_to.industry AS 'org_to_industry',
    org_to.occupation AS 'org_to_occupation',
    org_to.form_of_ownership AS 'org_to_ownership',
    org_to.form_gosp AS 'org_to_form_gosp',
    org_to.vedomstvo AS 'org_to_vedomstvo',
    org_to.sfera_upr_id AS 'org_to_sfera_upr_id',
    org_to.form_of_ownership_int AS 'org_to_form_ownership_id',
    org_to.addr_distr_new_id AS 'org_to_district_id',
    r.right_id,
    dict_obj_rights.name AS 'right_name',
    r.transfer_date,
    r.transfer_year,
    r.transfer_quarter,
    r.akt_id AS 'akt_id',
    akts.doc_num AS 'akt_num',
    akts.doc_date AS 'akt_date',
    YEAR(akts.doc_date) AS 'akt_date_year',
    DATEPART(Quarter, akts.doc_date)  AS 'akt_date_quarter',
    akts.topic AS 'akt_topic',
    akts.search_name AS 'akt_search_name',
    akts.is_text_exists AS 'akt_text_exists',
    r.rozp_id AS 'rozp_doc_id',
    rozp.doc_num AS 'rozp_doc_num',
    rozp.doc_date AS 'rozp_doc_date',
    rozp.topic AS 'rozp_doc_topic',
    rozp.search_name AS 'rozp_doc_search_name',
    rozp.is_text_exists AS 'rozp_text_exists',
    r.modified_by,
    r.modify_date,
    r.name,
    r.characteristic,
    r.misc_info,
    COALESCE(bdoc.cost_balans, r.sum_balans) AS 'sum_balans',
    COALESCE(bdoc.cost_zalishkova, r.sum_zalishkova) AS 'sum_zalishkova',
    COALESCE(bdoc.sqr_obj, r.sqr_transferred) AS 'sqr_transferred',
    COALESCE(bdoc.obj_length, r.len_transferred) AS 'len_transferred'
FROM
    object_rights r
    LEFT OUTER JOIN dict_obj_rights ON r.right_id = dict_obj_rights.id
    LEFT OUTER JOIN view_buildings AS b ON r.building_id = b.building_id
    LEFT OUTER JOIN view_obj_right_akts AS akts ON r.akt_id = akts.id
    LEFT OUTER JOIN view_obj_right_rozp AS rozp ON r.rozp_id = rozp.id
    LEFT OUTER JOIN view_obj_right_org_from AS org_from ON r.org_from_id = org_from.organization_id
    LEFT OUTER JOIN view_obj_right_org_to AS org_to ON r.org_to_id = org_to.organization_id
    LEFT OUTER JOIN object_rights_building_docs bdoc ON
        (r.building_id = bdoc.building_id) AND
        (r.akt_id = bdoc.document_id OR (r.akt_id IS NULL AND r.rozp_id = bdoc.document_id)) AND
        (r.name = bdoc.obj_name OR (r.name IS NULL AND bdoc.obj_name IS NULL))
    LEFT OUTER JOIN dict_tech_state ON bdoc.tech_condition_id = dict_tech_state.id
    LEFT OUTER JOIN dict_object_kind ON bdoc.object_kind_id = dict_object_kind.id
    LEFT OUTER JOIN dict_object_type ON bdoc.object_type_id = dict_object_type.id
    LEFT OUTER JOIN dict_balans_purpose_group ON bdoc.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON bdoc.purpose_id = dict_balans_purpose.id        
