
CREATE VIEW [view_privatization]
AS
SELECT
    privatization.id AS 'privatization_id',
    privatization.building_id,
    privatization.balans_id,
    privatization.organization_id,
    dict_privat_subordination.code AS 'subord_code',
    dict_privat_subordination.name AS 'subordination',
    b.district,
    b.street_full_name,
    b.addr_nomer,
    privatization.obj_name,
    org.short_name AS 'org_name',
        COALESCE(org.addr_city + ', ', '') +
        COALESCE(org.addr_street_name + ', ', '') +
        COALESCE(org.addr_nomer, '') +
        CASE WHEN (RTRIM(LTRIM(COALESCE(org.addr_korpus, ''))) <> '') THEN ', ' + org.addr_korpus ELSE '' END
        AS 'org_address',
    org.director_fio,
    org.director_phone,
    dict_privat_complex.name AS 'complex',
    privatization.sqr_total,
    dict_privat_obj_group.name AS 'obj_group',
    dict_privat_kind.name AS 'privat_kind',
    dict_privat_state.name AS 'privat_state',
    dict_balans_purpose_group.name AS 'purpose_group',
    COALESCE(dict_object_kind.name, b.object_kind) AS 'object_kind',
    COALESCE(dict_object_type.name, b.object_type) AS 'object_type',
    COALESCE(dict_history.name, b.history) AS 'object_history',
    privatization.obj_floor,
    privatization.cost,
    privatization.cost_expert,
    privatization.expert_date,
    privatization.note,
    privatization.rishen_doc_id
FROM
    privatization
    INNER JOIN view_buildings AS b ON privatization.building_id = b.building_id
    LEFT OUTER JOIN organizations AS org ON privatization.organization_id = org.id
    LEFT OUTER JOIN dict_privat_obj_group ON privatization.obj_group_id = dict_privat_obj_group.id
    LEFT OUTER JOIN dict_privat_kind ON privatization.privat_kind_id = dict_privat_kind.id
    LEFT OUTER JOIN dict_privat_state ON privatization.privat_state_id = dict_privat_state.id
    LEFT OUTER JOIN dict_privat_complex ON privatization.complex_id = dict_privat_complex.id
    LEFT OUTER JOIN dict_privat_subordination ON privatization.subordination_id = dict_privat_subordination.id
    LEFT OUTER JOIN dict_balans_purpose_group ON privatization.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_object_kind ON privatization.object_kind_id = dict_object_kind.id
    LEFT OUTER JOIN dict_object_type ON privatization.object_type_id = dict_object_type.id
    LEFT OUTER JOIN dict_history ON privatization.history_id = dict_history.id
