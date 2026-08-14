
CREATE VIEW view_priv_object_docs
AS
SELECT
    pod.master_doc_id,
    pod.document_id,
    p.privatization_id,
    p.building_id, 
    p.balans_id, 
    p.organization_id, 
    p.subord_code, 
    p.subordination, 
    p.district, 
    p.street_full_name, 
    p.addr_nomer, 
    p.obj_name, 
    p.org_name, 
    p.org_address, 
    p.director_fio, 
    p.director_phone, 
    p.complex, 
    p.sqr_total, 
    p.obj_group, 
    p.privat_kind, 
    p.privat_state, 
    p.purpose_group, 
    p.object_kind, 
    p.object_type, 
    p.object_history, 
    p.obj_floor, 
    p.cost, 
    p.cost_expert, 
    p.expert_date, 
    p.note
FROM
    priv_object_docs pod
    INNER JOIN view_privatization p ON pod.privatization_id = p.privatization_id
