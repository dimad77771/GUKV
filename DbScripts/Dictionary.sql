-- ƒÓ·‡‚ÎˇÂÏ ≤Õÿ≈ ‚Ó ‚ÒÂ ÒÔ‡‚Ó˜ÌËÍË 1NF
INSERT INTO dict_1nf_balans_purpose (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_balans_purpose UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_balans_purpose) S), '≤Õÿ≈')
INSERT INTO dict_1nf_balans_purpose_group (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_balans_purpose_group UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_balans_purpose_group) S), '≤Õÿ≈')
INSERT INTO dict_1nf_districts2 (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_districts2 UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_districts2) S), '≤Õÿ≈')
INSERT INTO dict_1nf_doc_kind (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_doc_kind UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_doc_kind) S), '≤Õÿ≈')
INSERT INTO dict_1nf_history (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_history UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_history) S), '≤Õÿ≈')
INSERT INTO dict_1nf_object_kind (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_object_kind UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_object_kind) S), '≤Õÿ≈')
INSERT INTO dict_1nf_object_type (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_object_type UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_object_type) S), '≤Õÿ≈')
INSERT INTO dict_1nf_org_form_gosp (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_org_form_gosp UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_org_form_gosp) S), '≤Õÿ≈')
INSERT INTO dict_1nf_org_industry (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_org_industry UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_org_industry) S), '≤Õÿ≈')
INSERT INTO dict_1nf_org_occupation (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_org_occupation UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_org_occupation) S), '≤Õÿ≈')
INSERT INTO dict_1nf_org_ownership (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_org_ownership UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_org_ownership) S), '≤Õÿ≈')
INSERT INTO dict_1nf_org_status (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_org_status UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_org_status) S), '≤Õÿ≈')
INSERT INTO dict_1nf_org_vedomstvo (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_org_vedomstvo UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_org_vedomstvo) S), '≤Õÿ≈')
INSERT INTO dict_1nf_tech_state (id, name) values ((SELECT MAX(S.MAXID) FROM (SELECT MAX(ID) + 1 AS MAXID FROM dict_tech_state UNION SELECT MAX(ID) + 1 AS MAXID  FROM dict_1nf_tech_state) S), '≤Õÿ≈')

-- ƒÓ·‡‚ÎˇÂÏ ≤Õÿ≈ ‚Ó ‚ÒÂ ÒÔ‡‚Ó˜ÌËÍË SQL
INSERT INTO dict_balans_purpose (id, name) SELECT id,name from dict_1nf_balans_purpose WHERE ID = (SELECT MAX(id) FROM dict_1nf_balans_purpose)
INSERT INTO dict_balans_purpose_group (id, name) SELECT id,name from dict_1nf_balans_purpose_group WHERE ID = (SELECT MAX(id) FROM dict_1nf_balans_purpose_group)
INSERT INTO dict_districts2 (id, name) SELECT id,name from dict_1nf_districts2 WHERE ID = (SELECT MAX(id) FROM dict_1nf_districts2)
INSERT INTO dict_doc_kind (id, name) SELECT id,name from dict_1nf_doc_kind WHERE ID = (SELECT MAX(id) FROM dict_1nf_doc_kind)
INSERT INTO dict_history (id, name) SELECT id,name from dict_1nf_history WHERE ID = (SELECT MAX(id) FROM dict_1nf_history)
INSERT INTO dict_object_kind (id, name) SELECT id,name from dict_1nf_object_kind WHERE ID = (SELECT MAX(id) FROM dict_1nf_object_kind)
INSERT INTO dict_object_type (id, name) SELECT id,name from dict_1nf_object_type WHERE ID = (SELECT MAX(id) FROM dict_1nf_object_type)
INSERT INTO dict_org_form_gosp (id, name) SELECT id,name from dict_1nf_org_form_gosp WHERE ID = (SELECT MAX(id) FROM dict_1nf_org_form_gosp)
INSERT INTO dict_org_industry (id, name) SELECT id,name from dict_1nf_org_industry WHERE ID = (SELECT MAX(id) FROM dict_1nf_org_industry)
INSERT INTO dict_org_occupation (id, name) SELECT id,name from dict_1nf_org_occupation WHERE ID = (SELECT MAX(id) FROM dict_1nf_org_occupation)
INSERT INTO dict_org_ownership (id, name) SELECT id,name from dict_1nf_org_ownership WHERE ID = (SELECT MAX(id) FROM dict_1nf_org_ownership)
INSERT INTO dict_org_status (id, name) SELECT id,name from dict_1nf_org_status WHERE ID = (SELECT MAX(id) FROM dict_1nf_org_status)
INSERT INTO dict_org_vedomstvo (id, name) SELECT id,name from dict_1nf_org_vedomstvo WHERE ID = (SELECT MAX(id) FROM dict_1nf_org_vedomstvo)
INSERT INTO dict_tech_state (id, name) SELECT id,name from dict_1nf_tech_state WHERE ID = (SELECT MAX(id) FROM dict_1nf_tech_state)

-- ƒÓ·‡‚ÎˇÂÏ ÍÓÎÓÌÍË ‰Îˇ ÒÚ‡˚ı ÁÌ‡˜ÂÌËÈ
--ALTER TABLE arenda_notes ADD purpose_id_OLD int NULL
--ALTER TABLE arenda_notes ADD purpose_group_id_OLD int NULL
--ALTER TABLE arenda ADD object_kind_id_OLD int NULL
--ALTER TABLE arenda ADD purpose_id_OLD int NULL
--ALTER TABLE arenda ADD purpose_group_id_OLD int NULL
--ALTER TABLE balans ADD form_ownership_id_OLD int NULL
--ALTER TABLE balans ADD history_id_OLD int NULL
--ALTER TABLE balans ADD object_kind_id_OLD int NULL
--ALTER TABLE balans ADD object_type_id_OLD int NULL
--ALTER TABLE balans ADD purpose_id_OLD int NULL
--ALTER TABLE balans ADD purpose_group_id_OLD int NULL
--ALTER TABLE balans ADD tech_condition_id_OLD int NULL
--ALTER TABLE building_docs ADD form_ownership_id_OLD int NULL
--ALTER TABLE building_docs ADD object_kind_id_OLD int NULL
--ALTER TABLE building_docs ADD object_type_id_OLD int NULL
--ALTER TABLE building_docs ADD purpose_id_OLD int NULL
--ALTER TABLE building_docs ADD purpose_group_id_OLD int NULL
--ALTER TABLE building_docs ADD tech_condition_id_OLD int NULL
--ALTER TABLE buildings ADD addr_distr_new_id_OLD int NULL
--ALTER TABLE buildings ADD history_id_OLD int NULL
--ALTER TABLE buildings ADD object_kind_id_OLD int NULL
--ALTER TABLE buildings ADD object_type_id_OLD int NULL
--ALTER TABLE buildings ADD tech_condition_id_OLD int NULL
--ALTER TABLE dict_expert ADD addr_district_id_OLD int NULL
--ALTER TABLE documents ADD kind_id_OLD int NULL
--ALTER TABLE expert_note ADD addr_district_id_OLD int NULL
--ALTER TABLE object_rights_building_docs ADD object_kind_id_OLD int NULL
--ALTER TABLE object_rights_building_docs ADD object_type_id_OLD int NULL
--ALTER TABLE object_rights_building_docs ADD purpose_id_OLD int NULL
--ALTER TABLE object_rights_building_docs ADD purpose_group_id_OLD int NULL
--ALTER TABLE object_rights_building_docs ADD tech_condition_id_OLD int NULL
--ALTER TABLE organizations ADD addr_distr_new_id_OLD int NULL
--ALTER TABLE organizations ADD form_gosp_id_OLD int NULL
--ALTER TABLE organizations ADD industry_id_OLD int NULL
--ALTER TABLE organizations ADD occupation_id_OLD int NULL
--ALTER TABLE organizations ADD form_ownership_id_OLD int NULL
--ALTER TABLE organizations ADD status_id_OLD int NULL
--ALTER TABLE organizations ADD vedomstvo_id_OLD int NULL
--ALTER TABLE privatization ADD addr_district_id_OLD int NULL
--ALTER TABLE privatization ADD history_id_OLD int NULL
--ALTER TABLE privatization ADD object_kind_id_OLD int NULL
--ALTER TABLE privatization ADD object_type_id_OLD int NULL
--ALTER TABLE privatization ADD purpose_group_id_OLD int NULL
--ALTER TABLE rent_object ADD district_id_OLD int NULL

-- —Óı‡ÌˇÂÏ ÒÚ‡˚Â ÁÌ‡˜ÂÌËˇ
--UPDATE arenda_notes SET purpose_id_OLD = purpose_id
--UPDATE arenda_notes SET purpose_group_id_OLD = purpose_group_id
--UPDATE arenda SET object_kind_id_OLD = object_kind_id
--UPDATE arenda SET purpose_id_OLD = purpose_id
--UPDATE arenda SET purpose_group_id_OLD = purpose_group_id
--UPDATE balans SET form_ownership_id_OLD = form_ownership_id
--UPDATE balans SET history_id_OLD = history_id
--UPDATE balans SET object_kind_id_OLD = object_kind_id
--UPDATE balans SET object_type_id_OLD = object_type_id
--UPDATE balans SET purpose_id_OLD = purpose_id
--UPDATE balans SET purpose_group_id_OLD = purpose_group_id
--UPDATE balans SET tech_condition_id_OLD = tech_condition_id
--UPDATE building_docs SET form_ownership_id_OLD = form_ownership_id
--UPDATE building_docs SET object_kind_id_OLD = object_kind_id
--UPDATE building_docs SET object_type_id_OLD = object_type_id
--UPDATE building_docs SET purpose_id_OLD = purpose_id
--UPDATE building_docs SET purpose_group_id_OLD = purpose_group_id
--UPDATE building_docs SET tech_condition_id_OLD = tech_condition_id
--UPDATE buildings SET addr_distr_new_id_OLD = addr_distr_new_id
--UPDATE buildings SET history_id_OLD = history_id
--UPDATE buildings SET object_kind_id_OLD = object_kind_id
--UPDATE buildings SET object_type_id_OLD = object_type_id
--UPDATE buildings SET tech_condition_id_OLD = tech_condition_id
--UPDATE dict_expert SET addr_district_id_OLD = addr_district_id
--UPDATE documents SET kind_id_OLD = kind_id
--UPDATE expert_note SET addr_district_id_OLD = addr_district_id
--UPDATE object_rights_building_docs SET object_kind_id_OLD = object_kind_id
--UPDATE object_rights_building_docs SET object_type_id_OLD = object_type_id
--UPDATE object_rights_building_docs SET purpose_id_OLD = purpose_id
--UPDATE object_rights_building_docs SET purpose_group_id_OLD = purpose_group_id
--UPDATE object_rights_building_docs SET tech_condition_id_OLD = tech_condition_id
--UPDATE organizations SET addr_distr_new_id_OLD = addr_distr_new_id
--UPDATE organizations SET form_gosp_id_OLD = form_gosp_id
--UPDATE organizations SET industry_id_OLD = industry_id
--UPDATE organizations SET occupation_id_OLD = occupation_id
--UPDATE organizations SET form_ownership_id_OLD = form_ownership_id
--UPDATE organizations SET status_id_OLD = status_id
--UPDATE organizations SET vedomstvo_id_OLD = vedomstvo_id
--UPDATE privatization SET addr_district_id_OLD = addr_district_id
--UPDATE privatization SET history_id_OLD = history_id
--UPDATE privatization SET object_kind_id_OLD = object_kind_id
--UPDATE privatization SET object_type_id_OLD = object_type_id
--UPDATE privatization SET purpose_group_id_OLD = purpose_group_id
--UPDATE rent_object SET district_id_OLD = district_id

-- Ó·ÌÓ‚ÎˇÂÏ ‚ÒÂ ÔÓÎˇ ‚ sql Ì‡ ÁÌ‡˜ÂÌËÂ 
UPDATE arenda_notes SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE arenda_notes SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE arenda SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE arenda SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE arenda SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE balans SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE balans SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE balans SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE balans SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE balans SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE balans SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE balans SET tech_condition_id = (SELECT MAX(ID) FROM dict_tech_state) WHERE tech_condition_id NOT IN (SELECT ID FROM dict_1nf_tech_state)
UPDATE building_docs SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE building_docs SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE building_docs SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE building_docs SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE building_docs SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE building_docs SET tech_condition_id = (SELECT MAX(ID) FROM dict_tech_state) WHERE tech_condition_id NOT IN (SELECT ID FROM dict_1nf_tech_state)
UPDATE buildings SET addr_distr_new_id = (SELECT MAX(ID) FROM dict_districts2) WHERE addr_distr_new_id NOT IN (SELECT ID FROM dict_1nf_districts2)
UPDATE buildings SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE buildings SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE buildings SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE buildings SET tech_condition_id = (SELECT MAX(ID) FROM dict_tech_state) WHERE tech_condition_id NOT IN (SELECT ID FROM dict_1nf_tech_state)
UPDATE dict_expert SET addr_district_id = (SELECT MAX(ID) FROM dict_districts2) WHERE addr_district_id NOT IN (SELECT ID FROM dict_1nf_districts2)
UPDATE documents SET kind_id = (SELECT MAX(ID) FROM dict_doc_kind) WHERE kind_id NOT IN (SELECT ID FROM dict_1nf_doc_kind)
UPDATE expert_note SET addr_district_id = (SELECT MAX(ID) FROM dict_districts2) WHERE addr_district_id NOT IN (SELECT ID FROM dict_1nf_districts2)
UPDATE object_rights_building_docs SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE object_rights_building_docs SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE object_rights_building_docs SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE object_rights_building_docs SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE object_rights_building_docs SET tech_condition_id = (SELECT MAX(ID) FROM dict_tech_state) WHERE tech_condition_id NOT IN (SELECT ID FROM dict_1nf_tech_state)
UPDATE organizations SET addr_distr_new_id = (SELECT MAX(ID) FROM dict_districts2) WHERE addr_distr_new_id NOT IN (SELECT ID FROM dict_1nf_districts2)
UPDATE organizations SET form_gosp_id = (SELECT MAX(ID) FROM dict_org_form_gosp) WHERE form_gosp_id NOT IN (SELECT ID FROM dict_1nf_org_form_gosp)
UPDATE organizations SET industry_id = (SELECT MAX(ID) FROM dict_org_industry) WHERE industry_id NOT IN (SELECT ID FROM dict_1nf_org_industry)
UPDATE organizations SET occupation_id = (SELECT MAX(ID) FROM dict_org_occupation) WHERE occupation_id NOT IN (SELECT ID FROM dict_1nf_org_occupation)
UPDATE organizations SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE organizations SET status_id = (SELECT MAX(ID) FROM dict_org_status) WHERE status_id NOT IN (SELECT ID FROM dict_1nf_org_status)
UPDATE organizations SET vedomstvo_id = (SELECT MAX(ID) FROM dict_org_vedomstvo) WHERE vedomstvo_id NOT IN (SELECT ID FROM dict_1nf_org_vedomstvo)
UPDATE privatization SET addr_district_id = (SELECT MAX(ID) FROM dict_districts2) WHERE addr_district_id NOT IN (SELECT ID FROM dict_1nf_districts2)
UPDATE privatization SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE privatization SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE privatization SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE privatization SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE rent_object SET district_id = (SELECT MAX(ID) FROM dict_districts2) WHERE district_id NOT IN (SELECT ID FROM dict_1nf_districts2)

UPDATE reports1nf_arenda_notes SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE reports1nf_arenda_notes SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE reports1nf_arenda SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE reports1nf_arenda SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE reports1nf_arenda SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE reports1nf_balans SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE reports1nf_balans SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE reports1nf_balans SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE reports1nf_balans SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE reports1nf_balans SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE reports1nf_balans SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE reports1nf_balans SET tech_condition_id = (SELECT MAX(ID) FROM dict_tech_state) WHERE tech_condition_id NOT IN (SELECT ID FROM dict_1nf_tech_state)
UPDATE reports1nf_buildings SET addr_distr_new_id = (SELECT MAX(ID) FROM dict_districts2) WHERE addr_distr_new_id NOT IN (SELECT ID FROM dict_1nf_districts2)
UPDATE reports1nf_buildings SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE reports1nf_buildings SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE reports1nf_buildings SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE reports1nf_buildings SET tech_condition_id = (SELECT MAX(ID) FROM dict_tech_state) WHERE tech_condition_id NOT IN (SELECT ID FROM dict_1nf_tech_state)
UPDATE reports1nf_org_info SET addr_distr_new_id = (SELECT MAX(ID) FROM dict_districts2) WHERE addr_distr_new_id NOT IN (SELECT ID FROM dict_1nf_districts2)
UPDATE reports1nf_org_info SET form_gosp_id = (SELECT MAX(ID) FROM dict_org_form_gosp) WHERE form_gosp_id NOT IN (SELECT ID FROM dict_1nf_org_form_gosp)
UPDATE reports1nf_org_info SET industry_id = (SELECT MAX(ID) FROM dict_org_industry) WHERE industry_id NOT IN (SELECT ID FROM dict_1nf_org_industry)
UPDATE reports1nf_org_info SET occupation_id = (SELECT MAX(ID) FROM dict_org_occupation) WHERE occupation_id NOT IN (SELECT ID FROM dict_1nf_org_occupation)
UPDATE reports1nf_org_info SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE reports1nf_org_info SET status_id = (SELECT MAX(ID) FROM dict_org_status) WHERE status_id NOT IN (SELECT ID FROM dict_1nf_org_status)
UPDATE reports1nf_org_info SET vedomstvo_id = (SELECT MAX(ID) FROM dict_org_vedomstvo) WHERE vedomstvo_id NOT IN (SELECT ID FROM dict_1nf_org_vedomstvo)


-- ”‰‡ÎˇÂÏ ÎË¯ÌËÂ ÁÌ‡˜ÂÌËˇ ÒÔ‡‚Ó˜ÌËÍÓ‚, ÍÓÚÓ˚Â ÓÚÒÛÚÒÚ‚Û˛Ú Û ·‡Î‡ÌÒÓ‰ÂÊ‡ÚÂÎÂÈ
DELETE FROM dict_balans_purpose WHERE ID NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
DELETE FROM dict_balans_purpose_group WHERE ID NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
DELETE FROM dict_districts2 WHERE ID NOT IN (SELECT ID FROM dict_1nf_districts2)
DELETE FROM dict_doc_kind WHERE ID NOT IN (SELECT ID FROM dict_1nf_doc_kind)
DELETE FROM dict_history WHERE ID NOT IN (SELECT ID FROM dict_1nf_history)
DELETE FROM dict_object_kind WHERE ID NOT IN (SELECT ID FROM dict_1nf_object_kind)
DELETE FROM dict_object_type WHERE ID NOT IN (SELECT ID FROM dict_1nf_object_type)
DELETE FROM dict_org_form_gosp WHERE ID NOT IN (SELECT ID FROM dict_1nf_org_form_gosp)
DELETE FROM dict_org_industry WHERE ID NOT IN (SELECT ID FROM dict_1nf_org_industry)
DELETE FROM dict_org_occupation WHERE ID NOT IN (SELECT ID FROM dict_1nf_org_occupation)
DELETE FROM dict_org_ownership WHERE ID NOT IN (SELECT ID FROM dict_1nf_org_ownership)
DELETE FROM dict_org_status WHERE ID NOT IN (SELECT ID FROM dict_1nf_org_status)
DELETE FROM dict_org_vedomstvo WHERE ID NOT IN (SELECT ID FROM dict_1nf_org_vedomstvo)
DELETE FROM dict_tech_state WHERE ID NOT IN (SELECT ID FROM dict_1nf_tech_state)

-- ƒÓ·‡‚ÎˇÂÏ ÁÌ‡˜ÂÌËˇ ÒÔ‡‚Ó˜ÌËÍÓ‚, ÍÓÚÓ˚Â ÓÚÒÛÚÒÚ‚Û˛Ú ‚ ƒ ¬
INSERT INTO dict_balans_purpose (id, name) select id, name from dict_1nf_balans_purpose WHERE id not in (select id from dict_balans_purpose)
INSERT INTO dict_balans_purpose_group (id, name) select id, name from dict_1nf_balans_purpose_group WHERE id not in (select id from dict_balans_purpose_group)
INSERT INTO dict_districts2 (id, name) select id, name from dict_1nf_districts2 WHERE id not in (select id from dict_districts2)
INSERT INTO dict_doc_kind (id, name) select id, name from dict_1nf_doc_kind WHERE id not in (select id from dict_doc_kind)
INSERT INTO dict_history (id, name) select id, name from dict_1nf_history WHERE id not in (select id from dict_history)
INSERT INTO dict_object_kind (id, name) select id, name from dict_1nf_object_kind WHERE id not in (select id from dict_object_kind)
INSERT INTO dict_object_type (id, name) select id, name from dict_1nf_object_type WHERE id not in (select id from dict_object_type)
INSERT INTO dict_org_form_gosp (id, name) select id, name from dict_1nf_org_form_gosp WHERE id not in (select id from dict_org_form_gosp)
INSERT INTO dict_org_industry (id, name) select id, name from dict_1nf_org_industry WHERE id not in (select id from dict_org_industry)
INSERT INTO dict_org_occupation (id, name) select id, name from dict_1nf_org_occupation WHERE id not in (select id from dict_org_occupation)
INSERT INTO dict_org_ownership (id, name) select id, name from dict_1nf_org_ownership WHERE id not in (select id from dict_org_ownership)
INSERT INTO dict_org_status (id, name) select id, name from dict_1nf_org_status WHERE id not in (select id from dict_org_status)
INSERT INTO dict_org_vedomstvo (id, name) select id, name from dict_1nf_org_vedomstvo WHERE id not in (select id from dict_org_vedomstvo)
INSERT INTO dict_tech_state (id, name) select id, name from dict_1nf_tech_state WHERE id not in (select id from dict_tech_state)

-- Œ·ÌÓ‚ÎˇÂÏ ÌÂÒÓÓÚ‚ÂÚÒÚ‚Û˛˘ËÂ ÁÌ‡˜ÂÌËˇ
update dict_1nf_org_ownership set name = 'ƒ≈–∆¿¬Õ¿' where id = 31
update dict_org_ownership set name = 'ƒ≈–∆¿¬Õ¿' where id = 31
update dict_1nf_org_ownership set name = ' ŒÃ”Õ¿À‹Õ¿ (—‘≈–¿ ”œ–¿¬À≤ÕÕﬂ –ƒ¿)' where id = 34
update dict_org_ownership set name = ' ŒÃ”Õ¿À‹Õ¿ (—‘≈–¿ ”œ–¿¬À≤ÕÕﬂ –ƒ¿)' where id = 34
update dict_1nf_org_ownership set name = '¬À¿—Õ≤—“‹ ≤Õÿ»’ ƒ≈–∆¿¬' where id = 40
update dict_org_ownership set name = '¬À¿—Õ≤—“‹ ≤Õÿ»’ ƒ≈–∆¿¬' where id = 40
update dict_1nf_org_ownership set name = '¬À¿—Õ≤—“‹ ﬁ–»ƒ»◊Õ»’ Œ—≤¡ ≤Õÿ»’ ƒ≈–∆¿¬' where id = 50
update dict_org_ownership set name = '¬À¿—Õ≤—“‹ ﬁ–»ƒ»◊Õ»’ Œ—≤¡ ≤Õÿ»’ ƒ≈–∆¿¬' where id = 50

update dict_1nf_org_status set name = 'ﬁ–»ƒ»◊Õ¿' where id = 1
update dict_org_status set name = 'ﬁ–»ƒ»◊Õ¿' where id = 1
update dict_1nf_org_status set name = 'Õ≈ﬁ–»ƒ»◊Õ¿' where id = 2
update dict_org_status set name = 'Õ≈ﬁ–»ƒ»◊Õ¿' where id = 2

UPDATE organizations SET form_ownership_id = 34 WHERE form_ownership_id = 38
UPDATE building_docs SET form_ownership_id = 34 WHERE form_ownership_id = 38
UPDATE balans SET form_ownership_id = 34 WHERE form_ownership_id = 38
DELETE FROM dict_org_ownership WHERE id = 38
DELETE FROM dict_1nf_org_ownership WHERE id = 38

update dict_org_occupation set name = 'Œ–√¿Õ ”œ–¿¬ÀIÕÕﬂ' where name = 'Œ–√¿Õ ”œ–¿¬À≤ÕÕﬂ'
update dict_org_occupation set name = 'À≤ ¿–ÕﬂÕ≤ «¿ À¿ƒ»' where name = 'ÀI ¿–ÕﬂÕI «¿ À¿ƒ»'
update dict_org_occupation set name = '≤Õÿ≤ Œ¡''™ “»' where name = '≤Õÿ≤ Œ¡"™ “»'


UPDATE arenda SET purpose_group_id = 37 WHERE purpose_group_id = 27
UPDATE arenda_notes SET purpose_group_id = 37 WHERE purpose_group_id = 27
UPDATE balans SET purpose_group_id = 37 WHERE purpose_group_id = 27
UPDATE building_docs SET purpose_group_id = 37 WHERE purpose_group_id = 27
UPDATE object_rights_building_docs SET purpose_group_id = 37 WHERE purpose_group_id = 27
UPDATE privatization SET purpose_group_id = 37 WHERE purpose_group_id = 27

DELETE FROM dict_1nf_balans_purpose_group WHERE id = 27
DELETE FROM dict_balans_purpose_group WHERE id = 27

UPDATE arenda_notes SET purpose_id = 29003 WHERE purpose_id IN (27036,21042,12039)
UPDATE arenda SET purpose_id = 29003 WHERE purpose_id IN (27036,21042,12039)
UPDATE balans SET purpose_id = 29003 WHERE purpose_id IN (27036,21042,12039)
UPDATE building_docs SET purpose_id = 29003 WHERE purpose_id IN (27036,21042,12039)
UPDATE object_rights_building_docs SET purpose_id = 29003 WHERE purpose_id IN (27036,21042,12039)

DELETE FROM dict_balans_purpose WHERE id IN (27036,21042,12039)
DELETE FROM dict_1nf_balans_purpose WHERE id IN (27036,21042,12039)



-- REGENERATE SCRIPTS FOR dict_org_occupation AND dict_1nf_org_occupation AND RUN HERE




DELETE FROM dict_org_occupation WHERE ID = 91804
DELETE FROM dict_org_occupation WHERE ID = 91511

DELETE FROM dict_1nf_org_occupation WHERE ID = 91804
DELETE FROM dict_1nf_org_occupation WHERE ID = 91511


--ALTER TABLE arenda_notes DROP COLUMN purpose_id_OLD
--ALTER TABLE arenda_notes DROP COLUMN purpose_group_id_OLD
--ALTER TABLE arenda DROP COLUMN object_kind_id_OLD
--ALTER TABLE arenda DROP COLUMN purpose_id_OLD
--ALTER TABLE arenda DROP COLUMN purpose_group_id_OLD
--ALTER TABLE balans DROP COLUMN form_ownership_id_OLD
--ALTER TABLE balans DROP COLUMN history_id_OLD
--ALTER TABLE balans DROP COLUMN object_kind_id_OLD
--ALTER TABLE balans DROP COLUMN object_type_id_OLD
--ALTER TABLE balans DROP COLUMN purpose_id_OLD
--ALTER TABLE balans DROP COLUMN purpose_group_id_OLD
--ALTER TABLE balans DROP COLUMN tech_condition_id_OLD
--ALTER TABLE building_docs DROP COLUMN form_ownership_id_OLD
--ALTER TABLE building_docs DROP COLUMN object_kind_id_OLD
--ALTER TABLE building_docs DROP COLUMN object_type_id_OLD
--ALTER TABLE building_docs DROP COLUMN purpose_id_OLD
--ALTER TABLE building_docs DROP COLUMN purpose_group_id_OLD
--ALTER TABLE building_docs DROP COLUMN tech_condition_id_OLD
--ALTER TABLE buildings DROP COLUMN addr_distr_new_id_OLD
--ALTER TABLE buildings DROP COLUMN history_id_OLD
--ALTER TABLE buildings DROP COLUMN object_kind_id_OLD
--ALTER TABLE buildings DROP COLUMN object_type_id_OLD
--ALTER TABLE buildings DROP COLUMN tech_condition_id_OLD
--ALTER TABLE dict_expert DROP COLUMN addr_district_id_OLD
--ALTER TABLE documents DROP COLUMN kind_id_OLD
--ALTER TABLE expert_note DROP COLUMN addr_district_id_OLD
--ALTER TABLE object_rights_building_docs DROP COLUMN object_kind_id_OLD
--ALTER TABLE object_rights_building_docs DROP COLUMN object_type_id_OLD
--ALTER TABLE object_rights_building_docs DROP COLUMN purpose_id_OLD
--ALTER TABLE object_rights_building_docs DROP COLUMN purpose_group_id_OLD
--ALTER TABLE object_rights_building_docs DROP COLUMN tech_condition_id_OLD
--ALTER TABLE organizations DROP COLUMN addr_distr_new_id_OLD
--ALTER TABLE organizations DROP COLUMN form_gosp_id_OLD
--ALTER TABLE organizations DROP COLUMN industry_id_OLD
--ALTER TABLE organizations DROP COLUMN occupation_id_OLD
--ALTER TABLE organizations DROP COLUMN form_ownership_id_OLD
--ALTER TABLE organizations DROP COLUMN status_id_OLD
--ALTER TABLE organizations DROP COLUMN vedomstvo_id_OLD
--ALTER TABLE privatization DROP COLUMN addr_district_id_OLD
--ALTER TABLE privatization DROP COLUMN history_id_OLD
--ALTER TABLE privatization DROP COLUMN object_kind_id_OLD
--ALTER TABLE privatization DROP COLUMN object_type_id_OLD
--ALTER TABLE privatization DROP COLUMN purpose_group_id_OLD
--ALTER TABLE rent_object DROP COLUMN district_id_OLD



UPDATE arch_arenda SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE arch_arenda_notes SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE arch_balans SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE arenda SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE arenda_notes SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE balans SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE building_docs SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE object_rights_building_docs SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE reports1nf_arenda SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE reports1nf_arenda_notes SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE reports1nf_balans SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE reports1nf_balans_deleted SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)
UPDATE unverified_balans SET purpose_id = (SELECT MAX(ID) FROM dict_balans_purpose) WHERE purpose_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose)

UPDATE arch_arenda SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE arch_arenda_notes SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE arch_balans SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE arenda SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE arenda_notes SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE balans SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE building_docs SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE object_rights_building_docs SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE privatization SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE reports1nf_arenda SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE reports1nf_arenda_notes SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE reports1nf_balans SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE reports1nf_balans_deleted SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)
UPDATE unverified_balans SET purpose_group_id = (SELECT MAX(ID) FROM dict_balans_purpose_group) WHERE purpose_group_id NOT IN (SELECT ID FROM dict_1nf_balans_purpose_group)

UPDATE rent_object SET district_id = (SELECT MAX(ID) FROM dict_districts2) WHERE district_id NOT IN (SELECT ID FROM dict_1nf_districts2)

UPDATE arch_buildings SET facade_id = (SELECT MAX(ID) FROM dict_facade) WHERE facade_id NOT IN (SELECT ID FROM dict_1nf_facade)
UPDATE buildings SET facade_id = (SELECT MAX(ID) FROM dict_facade) WHERE facade_id NOT IN (SELECT ID FROM dict_1nf_facade)
UPDATE reports1nf_buildings SET facade_id = (SELECT MAX(ID) FROM dict_facade) WHERE facade_id NOT IN (SELECT ID FROM dict_1nf_facade)

UPDATE arch_balans SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE arch_buildings SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE balans SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE buildings SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE privatization SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE reports1nf_balans SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE reports1nf_balans_deleted SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)
UPDATE reports1nf_buildings SET history_id = (SELECT MAX(ID) FROM dict_history) WHERE history_id NOT IN (SELECT ID FROM dict_1nf_history)

UPDATE arch_arenda SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE arch_balans SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE arch_buildings SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE arenda SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE balans SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE building_docs SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE buildings SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE object_rights_building_docs SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE privatization SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE reports1nf_arenda SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE reports1nf_balans SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE reports1nf_balans_deleted SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE reports1nf_buildings SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)
UPDATE unverified_balans SET object_kind_id = (SELECT MAX(ID) FROM dict_object_kind) WHERE object_kind_id NOT IN (SELECT ID FROM dict_1nf_object_kind)

UPDATE arch_balans SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE arch_buildings SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE balans SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE building_docs SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE buildings SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE object_rights_building_docs SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE privatization SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE reports1nf_balans SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE reports1nf_balans_deleted SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE reports1nf_buildings SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)
UPDATE unverified_balans SET object_type_id = (SELECT MAX(ID) FROM dict_object_type) WHERE object_type_id NOT IN (SELECT ID FROM dict_1nf_object_type)

UPDATE arch_organizations SET form_gosp_id = (SELECT MAX(ID) FROM dict_org_form_gosp) WHERE form_gosp_id NOT IN (SELECT ID FROM dict_1nf_org_form_gosp)
UPDATE organizations SET form_gosp_id = (SELECT MAX(ID) FROM dict_org_form_gosp) WHERE form_gosp_id NOT IN (SELECT ID FROM dict_1nf_org_form_gosp)
UPDATE reports1nf_org_info SET form_gosp_id = (SELECT MAX(ID) FROM dict_org_form_gosp) WHERE form_gosp_id NOT IN (SELECT ID FROM dict_1nf_org_form_gosp)

UPDATE arch_organizations SET industry_id = (SELECT MAX(ID) FROM dict_org_industry) WHERE industry_id NOT IN (SELECT ID FROM dict_1nf_org_industry)
UPDATE organizations SET industry_id = (SELECT MAX(ID) FROM dict_org_industry) WHERE industry_id NOT IN (SELECT ID FROM dict_1nf_org_industry)
UPDATE reports1nf_org_info SET industry_id = (SELECT MAX(ID) FROM dict_org_industry) WHERE industry_id NOT IN (SELECT ID FROM dict_1nf_org_industry)

UPDATE arch_organizations SET occupation_id = (SELECT MAX(ID) FROM dict_org_occupation) WHERE occupation_id NOT IN (SELECT ID FROM dict_1nf_org_occupation)
UPDATE organizations SET occupation_id = (SELECT MAX(ID) FROM dict_org_occupation) WHERE occupation_id NOT IN (SELECT ID FROM dict_1nf_org_occupation)
UPDATE reports1nf_org_info SET occupation_id = (SELECT MAX(ID) FROM dict_org_occupation) WHERE occupation_id NOT IN (SELECT ID FROM dict_1nf_org_occupation)

UPDATE arch_balans SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE arch_organizations SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE balans SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE building_docs SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE organizations SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE reports1nf_balans SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE reports1nf_balans_deleted SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE reports1nf_org_info SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)
UPDATE unverified_balans SET form_ownership_id = (SELECT MAX(ID) FROM dict_org_ownership) WHERE form_ownership_id NOT IN (SELECT ID FROM dict_1nf_org_ownership)

UPDATE arch_organizations SET status_id = (SELECT MAX(ID) FROM dict_org_status) WHERE status_id NOT IN (SELECT ID FROM dict_1nf_org_status)
UPDATE organizations SET status_id = (SELECT MAX(ID) FROM dict_org_status) WHERE status_id NOT IN (SELECT ID FROM dict_1nf_org_status)
UPDATE reports1nf_org_info SET status_id = (SELECT MAX(ID) FROM dict_org_status) WHERE status_id NOT IN (SELECT ID FROM dict_1nf_org_status)

UPDATE arch_organizations SET vedomstvo_id = (SELECT MAX(ID) FROM dict_org_vedomstvo) WHERE vedomstvo_id NOT IN (SELECT ID FROM dict_1nf_org_vedomstvo)
UPDATE organizations SET vedomstvo_id = (SELECT MAX(ID) FROM dict_org_vedomstvo) WHERE vedomstvo_id NOT IN (SELECT ID FROM dict_1nf_org_vedomstvo)
UPDATE reports1nf_org_info SET vedomstvo_id = (SELECT MAX(ID) FROM dict_org_vedomstvo) WHERE vedomstvo_id NOT IN (SELECT ID FROM dict_1nf_org_vedomstvo)

UPDATE rent_object SET tech_state_id = (SELECT MAX(ID) FROM dict_tech_state) WHERE tech_state_id NOT IN (SELECT ID FROM dict_1nf_tech_state)