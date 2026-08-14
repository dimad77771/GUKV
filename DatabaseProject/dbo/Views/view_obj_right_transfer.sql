
CREATE VIEW [view_obj_right_transfer]
AS
SELECT
    r.id as 'transfer_id',
    r.building_id,
    r.organization_id,
    b.district,
    b.street_full_name,
    b.addr_nomer,
    bdoc.condition,
    bdoc.object_type,
    bdoc.object_kind,
    bdoc.purpose_group,
    bdoc.purpose,
    org.full_name AS 'org_full_name',
    org.short_name AS 'org_short_name',
    org.zkpo_code AS 'org_zkpo_code',
    org.industry AS 'org_industry',
    org.occupation AS 'org_occupation',
    org.form_of_ownership AS 'org_form_of_ownership',
    org.form_gosp AS 'org_form_gosp',
    dict_obj_rights.name AS 'right_name',
    r.right_add_date AS 'transfer_date',
    r.right_add_year AS 'transfer_year',
    r.right_add_quarter AS 'transfer_quarter',
    r.right_add_doc_id AS 'akt_id',
    doc_add.doc_date AS 'akt_date',
    doc_add.doc_num AS 'akt_num',
    doc_add.topic AS 'akt_topic',
    doc_add.search_name AS 'akt_search_name',
    lnk.master_doc_id AS 'rozp_doc_id',
    lnk.parent_num AS 'rozp_doc_num',
    lnk.parent_date AS 'rozp_doc_date',
    lnk.parent_topic AS 'rozp_doc_topic',
    lnk.parent_search_name AS 'rozp_doc_search_name',
    r.modified_by,
    r.modify_date,
    r.name AS 'object_name',
    r.characteristic,
    r.misc_info,
    r.sum_balans,
    r.sum_zalishkova,
    r.sqr_transferred,
    r.len_transferred
FROM
    object_rights r
    LEFT OUTER JOIN view_buildings AS b ON r.building_id = b.building_id
    LEFT OUTER JOIN view_organizations AS org ON r.organization_id = org.organization_id
    LEFT OUTER JOIN dict_obj_rights ON r.right_id = dict_obj_rights.id
    LEFT OUTER JOIN documents doc_add ON r.right_add_doc_id = doc_add.id
    LEFT OUTER JOIN view_doc_links lnk ON r.right_add_doc_id = lnk.slave_doc_id
    LEFT OUTER JOIN view_building_docs bdoc ON (r.building_id = bdoc.building_id) AND (r.right_add_doc_id = bdoc.document_id)
WHERE
    (NOT r.right_add_doc_id IS NULL) AND (r.right_rem_doc_id IS NULL)
