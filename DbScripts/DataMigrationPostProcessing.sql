
/******************************************************************************/
/*       Post-processing steps which are performed after data migration       */
/******************************************************************************/

/*
   '1NF' database fixes
*/

UPDATE organizations SET form_id = temp_org_kind_id WHERE (form_id IS NULL) AND (NOT temp_org_kind_id IS NULL)

GO

ALTER TABLE organizations DROP COLUMN temp_org_kind_id

GO

UPDATE arch_organizations SET form_id = temp_org_kind_id WHERE (form_id IS NULL) AND (NOT temp_org_kind_id IS NULL)

GO

ALTER TABLE arch_organizations DROP COLUMN temp_org_kind_id

GO

ALTER TABLE building_docs DROP COLUMN old_id

GO

/* Make sure that cached street names are up-to-date */

UPDATE b SET b.addr_street_name = st.name FROM buildings b INNER JOIN dict_streets st ON st.id = b.addr_street_id

GO

UPDATE b SET b.addr_street_name2 = st.name FROM buildings b INNER JOIN dict_streets st ON st.id = b.addr_street_id2

GO

UPDATE b SET b.street_full_name = st.name FROM buildings b INNER JOIN dict_streets st ON st.id = b.addr_street_id WHERE (b.addr_street_id2 IS NULL) OR (b.addr_street_id2 = 0)

GO

UPDATE buildings SET street_full_name = addr_street_name + '/' + addr_street_name2 WHERE (addr_street_id2 > 0)

GO

/* Make sure that building numbers are comparable */

UPDATE buildings SET addr_nomer1 = LTRIM(RTRIM(addr_nomer1))

/* Make sure that OATUU codes are comparable */

UPDATE buildings SET oatuu_id = RTRIM(LTRIM(oatuu_id))

GO

UPDATE dict_oatuu SET id = RTRIM(LTRIM(id))

GO

/* Make sure that all organizations have their full name and short name defined */

--UPDATE organizations
--SET full_name = full_name + ' №' + CAST(nomer_obj AS VARCHAR(24))
--WHERE
--    (NOT full_name IS NULL) AND
--    (RTRIM(LTRIM(full_name)) <> '') AND
--    (NOT nomer_obj IS NULL) AND
--    (LTRIM(RTRIM(nomer_obj)) <> '') AND
--    (LTRIM(RTRIM(nomer_obj)) <> '0')

--GO

--UPDATE organizations
--SET short_name = short_name + ' №' + CAST(nomer_obj AS VARCHAR(24))
--WHERE
--    (NOT short_name IS NULL) AND
--    (RTRIM(LTRIM(short_name)) <> '') AND
--    (NOT nomer_obj IS NULL) AND
--    (LTRIM(RTRIM(nomer_obj)) <> '') AND
--    (LTRIM(RTRIM(nomer_obj)) <> '0')

--GO

UPDATE organizations SET full_name = short_name WHERE (full_name IS NULL) OR (RTRIM(LTRIM(full_name)) = '')

GO

UPDATE organizations SET short_name = full_name WHERE (short_name IS NULL) OR (RTRIM(LTRIM(short_name)) = '')

GO

/* Make sure that all buildings have street name */

UPDATE buildings SET street_full_name = addr_street_name WHERE (street_full_name IS NULL) OR (RTRIM(LTRIM(street_full_name)) = '')

GO

UPDATE buildings SET addr_street_name = street_full_name WHERE (addr_street_name IS NULL) OR (RTRIM(LTRIM(addr_street_name)) = '')

GO

/* Make sure that dict_arenda_payment_type table does not have empty values */

/* UPDATE dict_arenda_payment_type SET name = '<НЕ ЗАДАНО>' WHERE id = 10 */

GO

UPDATE arenda_notes SET payment_type_id = 10 WHERE payment_type_id IS NULL

GO

UPDATE arenda SET payment_type_id = 10 WHERE payment_type_id IS NULL

GO

/* Update our own flag that defines the state of rent agreement */

UPDATE arenda SET agreement_state = 0 WHERE is_deleted = 1

GO

/* Make sure that arenda_notes does not have empty fields */

UPDATE arenda_notes SET purpose_str = NULL WHERE RTRIM(LTRIM(purpose_str)) = ''

GO

/*
   Drop temporary tables
*/

DROP TABLE temp_balans_org

GO

DROP TABLE temp_buildings

GO

/* Delete the duplicating value 'КОМУНАЛЬНА (сфера управління РДА)' from dictionary 'dict_org_ownership' */

--UPDATE organizations SET form_ownership_id = 34 WHERE form_ownership_id = 38

--GO

--UPDATE building_docs SET form_ownership_id = 34 WHERE form_ownership_id = 38

--GO

--UPDATE balans SET form_ownership_id = 34 WHERE form_ownership_id = 38

--GO

--DELETE FROM dict_org_ownership WHERE id = 38

--GO

/*
   'Arenda' database fixes
*/

/* Move appendix ID from two temporary columns to the actual column 'appendix_id' */

/* UPDATE arenda_decisions SET appendix_id = appendix_id_protok WHERE (NOT appendix_id_protok IS NULL) */

GO

/* UPDATE arenda_decisions SET appendix_id = appendix_id_raspor WHERE (NOT appendix_id_raspor IS NULL) */

GO

/* Convert all BLOBS to CLOBS */

UPDATE arenda_decisions SET note_rish_etap1 = cast(blob_rish_etap1 as varchar(max)) WHERE (NOT blob_rish_etap1 IS NULL)

GO

UPDATE arenda_decisions SET note_rish2_etap1 = cast(blob_rish2_etap1 as varchar(max)) WHERE (NOT blob_rish2_etap1 IS NULL)

GO

UPDATE arenda_decisions SET note_nazva = cast(blob_nazva as varchar(max)) WHERE (NOT blob_nazva IS NULL)

GO

UPDATE arenda_decisions SET note_rish_etap2 = cast(blob_rish_etap2 as varchar(max)) WHERE (NOT blob_rish_etap2 IS NULL)

GO

UPDATE arenda_decisions SET note_rish2_etap2 = cast(blob_rish2_etap2 as varchar(max)) WHERE (NOT blob_rish2_etap2 IS NULL)

GO

UPDATE arenda_decision_objects SET name = cast(blob_name as varchar(max)) WHERE (NOT blob_name IS NULL)

GO

UPDATE arenda_decision_objects SET location = cast(blob_location as varchar(max)) WHERE (NOT blob_location IS NULL)

GO

/* Delete the temporary columns */

/*ALTER TABLE arenda_decisions DROP COLUMN appendix_id_protok*/

GO

/*ALTER TABLE arenda_decisions DROP COLUMN appendix_id_raspor*/

GO

ALTER TABLE arenda_decisions DROP COLUMN blob_rish_etap1

GO

ALTER TABLE arenda_decisions DROP COLUMN blob_rish2_etap1

GO

ALTER TABLE arenda_decisions DROP COLUMN blob_nazva

GO

ALTER TABLE arenda_decisions DROP COLUMN blob_rish_etap2

GO

ALTER TABLE arenda_decisions DROP COLUMN blob_rish2_etap2

GO

ALTER TABLE arenda_decision_objects DROP COLUMN blob_name

GO

ALTER TABLE arenda_decision_objects DROP COLUMN blob_location

GO

/*
   'Rosporadjennia' database fixes
*/

UPDATE dict_obj_rights SET name = RTRIM(LTRIM(name))

/*
   Archive state tables - remove temporary columns
*/

ALTER TABLE arch_buildings DROP COLUMN archive_link_code
ALTER TABLE arch_buildings DROP COLUMN archive_link_code_prev
ALTER TABLE arch_buildings DROP COLUMN archive_link_code_next

ALTER TABLE arch_organizations DROP COLUMN archive_link_code
ALTER TABLE arch_organizations DROP COLUMN archive_link_code_prev
ALTER TABLE arch_organizations DROP COLUMN archive_link_code_next

/* ALTER TABLE arch_arenda DROP COLUMN archive_link_code - this code is permanent, not temporary in 'arch_arenda' table */
ALTER TABLE arch_arenda DROP COLUMN archive_link_code_prev
ALTER TABLE arch_arenda DROP COLUMN archive_link_code_next

/* ALTER TABLE arch_balans DROP COLUMN archive_link_code INTEGER - this code is permanent, not temporary in 'arch_balans' table */
ALTER TABLE arch_balans DROP COLUMN archive_link_code_prev
ALTER TABLE arch_balans DROP COLUMN archive_link_code_next

/******************************************************************************/
/*               Make sure that system tables contain some data               */
/******************************************************************************/

/* user_folders */

IF NOT EXISTS (SELECT id FROM user_folders WHERE id = 1)
INSERT INTO user_folders (folder_name) VALUES ('Передача Прав')

GO

IF NOT EXISTS (SELECT id FROM user_folders WHERE id = 2)
INSERT INTO user_folders (folder_name) VALUES ('Об''єкти')

GO

IF NOT EXISTS (SELECT id FROM user_folders WHERE id = 3)
INSERT INTO user_folders (folder_name) VALUES ('Оренда')

GO

IF NOT EXISTS (SELECT id FROM user_folders WHERE id = 4)
INSERT INTO user_folders (folder_name) VALUES ('Документи')

GO

IF NOT EXISTS (SELECT id FROM user_folders WHERE id = 5)
INSERT INTO user_folders (folder_name) VALUES ('Приватизація')

GO

IF NOT EXISTS (SELECT id FROM user_folders WHERE id = 6)
INSERT INTO user_folders (folder_name) VALUES ('Фінансова Звітність')

GO

IF NOT EXISTS (SELECT id FROM user_folders WHERE id = 7)
INSERT INTO user_folders (folder_name) VALUES ('Орендна Плата')

GO

/* fin_periods */

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 1)
INSERT INTO fin_periods (period_name, period_code) VALUES ('Всі періоди', '')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 2)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.04.2010', '2010Q1')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 3)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.07.2010', '2010Q2')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 4)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.10.2010', '2010Q3')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 5)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.01.2011', '2010')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 6)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.04.2011', '2011Q1')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 7)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.07.2011', '2011Q2')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 8)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.10.2011', '2011Q3')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 9)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.01.2012', '2011')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 10)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.04.2012', '2012Q1')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 11)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.07.2012', '2012Q2')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 12)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.10.2012', '2012Q3')

GO

IF NOT EXISTS (SELECT id FROM fin_periods WHERE id = 13)
INSERT INTO fin_periods (period_name, period_code) VALUES ('01.01.2013', '2012')

GO


/**********************************************************************/
/* Обновление дат, которые выходят за пределы диапазона 1753 - 9999 г */
/**********************************************************************/


update sys_report_corrections set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update fin_report_periods set period_start = null where period_start < '1753-01-01' or period_start > '9999-12-31'
update fin_report_periods set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_plan_zone set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_history set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_arenda_payments set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_doc_link_kind set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_rent_period set period_start = null where period_start < '1753-01-01' or period_start > '9999-12-31'
update dict_rent_period set period_end = null where period_end < '1753-01-01' or period_end > '9999-12-31'
update fin_report_forms set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_registr_org set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_object_type set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update building_docs set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update building_docs set doc_change_date = null where doc_change_date < '1753-01-01' or doc_change_date > '9999-12-31'
update building_docs set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update building_docs set date_priv = null where date_priv < '1753-01-01' or date_priv > '9999-12-31'
update arenda set agreement_date = null where agreement_date < '1753-01-01' or agreement_date > '9999-12-31'
update arenda set pidstava_date = null where pidstava_date < '1753-01-01' or pidstava_date > '9999-12-31'
update arenda set pidstava_date2 = null where pidstava_date2 < '1753-01-01' or pidstava_date2 > '9999-12-31'
update arenda set rent_start_date = null where rent_start_date < '1753-01-01' or rent_start_date > '9999-12-31'
update arenda set rent_finish_date = null where rent_finish_date < '1753-01-01' or rent_finish_date > '9999-12-31'
update arenda set rent_actual_finish_date = null where rent_actual_finish_date < '1753-01-01' or rent_actual_finish_date > '9999-12-31'
update arenda set order_date = null where order_date < '1753-01-01' or order_date > '9999-12-31'
update arenda set inactive_date = null where inactive_date < '1753-01-01' or inactive_date > '9999-12-31'
update arenda set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update arenda set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update arenda set date_expl_enter = null where date_expl_enter < '1753-01-01' or date_expl_enter > '9999-12-31'
update arenda set date_akt = null where date_akt < '1753-01-01' or date_akt > '9999-12-31'
update arenda set date_bti = null where date_bti < '1753-01-01' or date_bti > '9999-12-31'
update arenda set svidotstvo_date = null where svidotstvo_date < '1753-01-01' or svidotstvo_date > '9999-12-31'
update arenda set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arenda set insurance_start = null where insurance_start < '1753-01-01' or insurance_start > '9999-12-31'
update arenda set insurance_end = null where insurance_end < '1753-01-01' or insurance_end > '9999-12-31'
update arenda_rishen set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arenda_rishen set doc_date = null where doc_date < '1753-01-01' or doc_date > '9999-12-31'
update dict_org_old_industry set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_object_kind set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_old_occupation set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update fin_report_rows set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_payment_documents set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_oatuu set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_old_organ set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update fin_report_columns set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_expert set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_expert set certificate_date = null where certificate_date < '1753-01-01' or certificate_date > '9999-12-31'
update dict_expert set certificate_end_date = null where certificate_end_date < '1753-01-01' or certificate_end_date > '9999-12-31'
update dict_expert set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update dict_facade set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arenda_notes set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arenda_notes set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update arenda_notes set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update organizations set registration_date = null where registration_date < '1753-01-01' or registration_date > '9999-12-31'
update organizations set date_l_year = null where date_l_year < '1753-01-01' or date_l_year > '9999-12-31'
update organizations set date_stat_spravka = null where date_stat_spravka < '1753-01-01' or date_stat_spravka > '9999-12-31'
update organizations set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update organizations set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update organizations set beg_state_date = null where beg_state_date < '1753-01-01' or beg_state_date > '9999-12-31'
update organizations set end_state_date = null where end_state_date < '1753-01-01' or end_state_date > '9999-12-31'
update organizations set nadhodjennya_date = null where nadhodjennya_date < '1753-01-01' or nadhodjennya_date > '9999-12-31'
update organizations set vibuttya_date = null where vibuttya_date < '1753-01-01' or vibuttya_date > '9999-12-31'
update organizations set registration_dov_date = null where registration_dov_date < '1753-01-01' or registration_dov_date > '9999-12-31'
update organizations set strok_start_date = null where strok_start_date < '1753-01-01' or strok_start_date > '9999-12-31'
update organizations set strok_end_date = null where strok_end_date < '1753-01-01' or strok_end_date > '9999-12-31'
update organizations set povnov_passp_date = null where povnov_passp_date < '1753-01-01' or povnov_passp_date > '9999-12-31'
update organizations set director_passp_date = null where director_passp_date < '1753-01-01' or director_passp_date > '9999-12-31'
update organizations set registration_svid_date = null where registration_svid_date < '1753-01-01' or registration_svid_date > '9999-12-31'
update expert_note set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update expert_note set valuation_date = null where valuation_date < '1753-01-01' or valuation_date > '9999-12-31'
update expert_note set final_date = null where final_date < '1753-01-01' or final_date > '9999-12-31'
update expert_note set arch_date = null where arch_date < '1753-01-01' or arch_date > '9999-12-31'
update expert_note set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update buildings set date_begin = null where date_begin < '1753-01-01' or date_begin > '9999-12-31'
update buildings set date_end = null where date_end < '1753-01-01' or date_end > '9999-12-31'
update buildings set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update buildings set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update fin_report_formulae set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update object_rights_building_docs set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_doc_kind set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arenda_payments set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_doc_general_kind set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update expert_note_detail set valuation_date = null where valuation_date < '1753-01-01' or valuation_date > '9999-12-31'
update expert_note_detail set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update expert_note_detail set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update arenda_payments_cmk set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_occupation set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update org_docs set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update fin_form_changes set change_date = null where change_date < '1753-01-01' or change_date > '9999-12-31'
update fin_form_changes set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update fin_form_changes set save_date = null where save_date < '1753-01-01' or save_date > '9999-12-31'
update arenda_rented set agreement_date = null where agreement_date < '1753-01-01' or agreement_date > '9999-12-31'
update arenda_rented set rent_start_date = null where rent_start_date < '1753-01-01' or rent_start_date > '9999-12-31'
update arenda_rented set rent_finish_date = null where rent_finish_date < '1753-01-01' or rent_finish_date > '9999-12-31'
update arenda_rented set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arenda_rented set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update dict_org_status set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_doc_commission set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_form_gosp set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_privat_state set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_arenda_rented set agreement_date = null where agreement_date < '1753-01-01' or agreement_date > '9999-12-31'
update arch_arenda_rented set rent_start_date = null where rent_start_date < '1753-01-01' or rent_start_date > '9999-12-31'
update arch_arenda_rented set rent_finish_date = null where rent_finish_date < '1753-01-01' or rent_finish_date > '9999-12-31'
update arch_arenda_rented set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_arenda_rented set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update arch_arenda_rented set archive_entry_timestamp = null where archive_entry_timestamp < '1753-01-01' or archive_entry_timestamp > '9999-12-31'
update dict_doc_appendix_kind set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update expert_input_doc set doc_date = null where doc_date < '1753-01-01' or doc_date > '9999-12-31'
update expert_input_doc set control_date = null where control_date < '1753-01-01' or control_date > '9999-12-31'
update expert_input_doc set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update expert_input_doc set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update dict_org_ownership set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_privat_obj_group set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_doc_stadiya set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_gosp_struct set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_privat_kind set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_doc_source set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_organ set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_privat_complex set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update expert_output_doc set doc_date = null where doc_date < '1753-01-01' or doc_date > '9999-12-31'
update expert_output_doc set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update expert_output_doc set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update dict_doc_state set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_balans_o26 set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_industry set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_privat_subordination set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_rent_decisions set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_balans_bti set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_priznak set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update privatization set expert_date = null where expert_date < '1753-01-01' or expert_date > '9999-12-31'
update privatization set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update documents set doc_date = null where doc_date < '1753-01-01' or doc_date > '9999-12-31'
update documents set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update documents set receive_date = null where receive_date < '1753-01-01' or receive_date > '9999-12-31'
update documents set priv_date = null where priv_date < '1753-01-01' or priv_date > '9999-12-31'
update dict_balans_purpose_group set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_buildings set date_begin = null where date_begin < '1753-01-01' or date_begin > '9999-12-31'
update arch_buildings set date_end = null where date_end < '1753-01-01' or date_end > '9999-12-31'
update arch_buildings set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_buildings set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update arch_buildings set archive_create_date = null where archive_create_date < '1753-01-01' or archive_create_date > '9999-12-31'
update dict_org_title_form set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_balans_purpose set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_organizations set registration_date = null where registration_date < '1753-01-01' or registration_date > '9999-12-31'
update arch_organizations set date_l_year = null where date_l_year < '1753-01-01' or date_l_year > '9999-12-31'
update arch_organizations set date_stat_spravka = null where date_stat_spravka < '1753-01-01' or date_stat_spravka > '9999-12-31'
update arch_organizations set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_organizations set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update arch_organizations set beg_state_date = null where beg_state_date < '1753-01-01' or beg_state_date > '9999-12-31'
update arch_organizations set end_state_date = null where end_state_date < '1753-01-01' or end_state_date > '9999-12-31'
update arch_organizations set nadhodjennya_date = null where nadhodjennya_date < '1753-01-01' or nadhodjennya_date > '9999-12-31'
update arch_organizations set vibuttya_date = null where vibuttya_date < '1753-01-01' or vibuttya_date > '9999-12-31'
update arch_organizations set registration_dov_date = null where registration_dov_date < '1753-01-01' or registration_dov_date > '9999-12-31'
update arch_organizations set strok_start_date = null where strok_start_date < '1753-01-01' or strok_start_date > '9999-12-31'
update arch_organizations set strok_end_date = null where strok_end_date < '1753-01-01' or strok_end_date > '9999-12-31'
update arch_organizations set povnov_passp_date = null where povnov_passp_date < '1753-01-01' or povnov_passp_date > '9999-12-31'
update arch_organizations set director_passp_date = null where director_passp_date < '1753-01-01' or director_passp_date > '9999-12-31'
update arch_organizations set registration_svid_date = null where registration_svid_date < '1753-01-01' or registration_svid_date > '9999-12-31'
update arch_organizations set archive_create_date = null where archive_create_date < '1753-01-01' or archive_create_date > '9999-12-31'
update dict_org_form_1nf set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update doc_appendices set app_date = null where app_date < '1753-01-01' or app_date > '9999-12-31'
update doc_appendices set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update doc_appendices set doc_date = null where doc_date < '1753-01-01' or doc_date > '9999-12-31'
update dict_balans_ownership_type set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_arenda set agreement_date = null where agreement_date < '1753-01-01' or agreement_date > '9999-12-31'
update arch_arenda set pidstava_date = null where pidstava_date < '1753-01-01' or pidstava_date > '9999-12-31'
update arch_arenda set pidstava_date2 = null where pidstava_date2 < '1753-01-01' or pidstava_date2 > '9999-12-31'
update arch_arenda set rent_start_date = null where rent_start_date < '1753-01-01' or rent_start_date > '9999-12-31'
update arch_arenda set rent_finish_date = null where rent_finish_date < '1753-01-01' or rent_finish_date > '9999-12-31'
update arch_arenda set rent_actual_finish_date = null where rent_actual_finish_date < '1753-01-01' or rent_actual_finish_date > '9999-12-31'
update arch_arenda set order_date = null where order_date < '1753-01-01' or order_date > '9999-12-31'
update arch_arenda set inactive_date = null where inactive_date < '1753-01-01' or inactive_date > '9999-12-31'
update arch_arenda set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update arch_arenda set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update arch_arenda set date_expl_enter = null where date_expl_enter < '1753-01-01' or date_expl_enter > '9999-12-31'
update arch_arenda set date_akt = null where date_akt < '1753-01-01' or date_akt > '9999-12-31'
update arch_arenda set date_bti = null where date_bti < '1753-01-01' or date_bti > '9999-12-31'
update arch_arenda set svidotstvo_date = null where svidotstvo_date < '1753-01-01' or svidotstvo_date > '9999-12-31'
update arch_arenda set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_arenda set insurance_start = null where insurance_start < '1753-01-01' or insurance_start > '9999-12-31'
update arch_arenda set insurance_end = null where insurance_end < '1753-01-01' or insurance_end > '9999-12-31'
update arch_arenda set archive_create_date = null where archive_create_date < '1753-01-01' or archive_create_date > '9999-12-31'
update dict_org_vedomstvo set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update rent_payment set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_balans_form_giver set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_arenda_notes set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_arenda_notes set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update arch_arenda_notes set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update dict_org_title set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update priv_object_docs set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arenda_applications set appl_letter_date = null where appl_letter_date < '1753-01-01' or appl_letter_date > '9999-12-31'
update arenda_applications set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_balans_update_src set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_link_arenda_2_decisions set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_link_arenda_2_decisions set doc_date = null where doc_date < '1753-01-01' or doc_date > '9999-12-31'
update dict_org_form set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_obj_rights set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arenda_decisions set apply_date = null where apply_date < '1753-01-01' or apply_date > '9999-12-31'
update arenda_decisions set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_balans_vidch_type set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_balans set approval_date = null where approval_date < '1753-01-01' or approval_date > '9999-12-31'
update arch_balans set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arch_balans set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update arch_balans set znos_date = null where znos_date < '1753-01-01' or znos_date > '9999-12-31'
update arch_balans set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update arch_balans set fair_cost_date = null where fair_cost_date < '1753-01-01' or fair_cost_date > '9999-12-31'
update arch_balans set date_bti = null where date_bti < '1753-01-01' or date_bti > '9999-12-31'
update arch_balans set date_cost_rinkova = null where date_cost_rinkova < '1753-01-01' or date_cost_rinkova > '9999-12-31'
update arch_balans set ownership_doc_date = null where ownership_doc_date < '1753-01-01' or ownership_doc_date > '9999-12-31'
update arch_balans set balans_doc_date = null where balans_doc_date < '1753-01-01' or balans_doc_date > '9999-12-31'
update arch_balans set vidch_doc_date = null where vidch_doc_date < '1753-01-01' or vidch_doc_date > '9999-12-31'
update arch_balans set archive_create_date = null where archive_create_date < '1753-01-01' or archive_create_date > '9999-12-31'
update dict_org_gosp_struct_type set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_org_info set registration_date = null where registration_date < '1753-01-01' or registration_date > '9999-12-31'
update reports1nf_org_info set date_l_year = null where date_l_year < '1753-01-01' or date_l_year > '9999-12-31'
update reports1nf_org_info set date_stat_spravka = null where date_stat_spravka < '1753-01-01' or date_stat_spravka > '9999-12-31'
update reports1nf_org_info set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_org_info set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update reports1nf_org_info set beg_state_date = null where beg_state_date < '1753-01-01' or beg_state_date > '9999-12-31'
update reports1nf_org_info set end_state_date = null where end_state_date < '1753-01-01' or end_state_date > '9999-12-31'
update reports1nf_org_info set nadhodjennya_date = null where nadhodjennya_date < '1753-01-01' or nadhodjennya_date > '9999-12-31'
update reports1nf_org_info set vibuttya_date = null where vibuttya_date < '1753-01-01' or vibuttya_date > '9999-12-31'
update reports1nf_org_info set registration_dov_date = null where registration_dov_date < '1753-01-01' or registration_dov_date > '9999-12-31'
update reports1nf_org_info set strok_start_date = null where strok_start_date < '1753-01-01' or strok_start_date > '9999-12-31'
update reports1nf_org_info set strok_end_date = null where strok_end_date < '1753-01-01' or strok_end_date > '9999-12-31'
update reports1nf_org_info set povnov_passp_date = null where povnov_passp_date < '1753-01-01' or povnov_passp_date > '9999-12-31'
update reports1nf_org_info set director_passp_date = null where director_passp_date < '1753-01-01' or director_passp_date > '9999-12-31'
update reports1nf_org_info set registration_svid_date = null where registration_svid_date < '1753-01-01' or registration_svid_date > '9999-12-31'
update reports1nf_org_info set submit_date = null where submit_date < '1753-01-01' or submit_date > '9999-12-31'
update dict_restriction_group set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update balans set approval_date = null where approval_date < '1753-01-01' or approval_date > '9999-12-31'
update balans set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update balans set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update balans set znos_date = null where znos_date < '1753-01-01' or znos_date > '9999-12-31'
update balans set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update balans set fair_cost_date = null where fair_cost_date < '1753-01-01' or fair_cost_date > '9999-12-31'
update balans set date_bti = null where date_bti < '1753-01-01' or date_bti > '9999-12-31'
update balans set date_cost_rinkova = null where date_cost_rinkova < '1753-01-01' or date_cost_rinkova > '9999-12-31'
update balans set ownership_doc_date = null where ownership_doc_date < '1753-01-01' or ownership_doc_date > '9999-12-31'
update balans set balans_doc_date = null where balans_doc_date < '1753-01-01' or balans_doc_date > '9999-12-31'
update balans set vidch_doc_date = null where vidch_doc_date < '1753-01-01' or vidch_doc_date > '9999-12-31'
update dict_org_share_type set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_restriction set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_buildings set date_begin = null where date_begin < '1753-01-01' or date_begin > '9999-12-31'
update reports1nf_buildings set date_end = null where date_end < '1753-01-01' or date_end > '9999-12-31'
update reports1nf_buildings set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_buildings set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update restrictions set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update m_view_docs_objects_akts set doc_date = null where doc_date < '1753-01-01' or doc_date > '9999-12-31'
update m_view_docs_objects_akts set receive_date = null where receive_date < '1753-01-01' or receive_date > '9999-12-31'
update m_view_docs_objects_akts set akt_date = null where akt_date < '1753-01-01' or akt_date > '9999-12-31'
update reports1nf_balans set approval_date = null where approval_date < '1753-01-01' or approval_date > '9999-12-31'
update reports1nf_balans set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_balans set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update reports1nf_balans set znos_date = null where znos_date < '1753-01-01' or znos_date > '9999-12-31'
update reports1nf_balans set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update reports1nf_balans set fair_cost_date = null where fair_cost_date < '1753-01-01' or fair_cost_date > '9999-12-31'
update reports1nf_balans set date_bti = null where date_bti < '1753-01-01' or date_bti > '9999-12-31'
update reports1nf_balans set date_cost_rinkova = null where date_cost_rinkova < '1753-01-01' or date_cost_rinkova > '9999-12-31'
update reports1nf_balans set ownership_doc_date = null where ownership_doc_date < '1753-01-01' or ownership_doc_date > '9999-12-31'
update reports1nf_balans set balans_doc_date = null where balans_doc_date < '1753-01-01' or balans_doc_date > '9999-12-31'
update reports1nf_balans set vidch_doc_date = null where vidch_doc_date < '1753-01-01' or vidch_doc_date > '9999-12-31'
update reports1nf_balans set submit_date = null where submit_date < '1753-01-01' or submit_date > '9999-12-31'
update m_view_object_rights set transfer_date = null where transfer_date < '1753-01-01' or transfer_date > '9999-12-31'
update m_view_object_rights set akt_date = null where akt_date < '1753-01-01' or akt_date > '9999-12-31'
update m_view_object_rights set rozp_doc_date = null where rozp_doc_date < '1753-01-01' or rozp_doc_date > '9999-12-31'
update m_view_object_rights set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_mayno set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update m_report_balans_and_arenda set approval_date = null where approval_date < '1753-01-01' or approval_date > '9999-12-31'
update m_report_balans_and_arenda set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update m_report_balans_and_arenda set znos_date = null where znos_date < '1753-01-01' or znos_date > '9999-12-31'
update m_report_balans_and_arenda set input_date = null where input_date < '1753-01-01' or input_date > '9999-12-31'
update m_report_balans_and_arenda set agreement_date = null where agreement_date < '1753-01-01' or agreement_date > '9999-12-31'
update m_report_balans_and_arenda set pidstava_date = null where pidstava_date < '1753-01-01' or pidstava_date > '9999-12-31'
update m_report_balans_and_arenda set rent_start_date = null where rent_start_date < '1753-01-01' or rent_start_date > '9999-12-31'
update m_report_balans_and_arenda set rent_finish_date = null where rent_finish_date < '1753-01-01' or rent_finish_date > '9999-12-31'
update m_report_balans_and_arenda set rent_actual_finish_date = null where rent_actual_finish_date < '1753-01-01' or rent_actual_finish_date > '9999-12-31'
update m_report_balans_and_arenda set date_akt = null where date_akt < '1753-01-01' or date_akt > '9999-12-31'
update m_view_arenda set agreement_date = null where agreement_date < '1753-01-01' or agreement_date > '9999-12-31'
update m_view_arenda set pidstava_date = null where pidstava_date < '1753-01-01' or pidstava_date > '9999-12-31'
update m_view_arenda set rent_start_date = null where rent_start_date < '1753-01-01' or rent_start_date > '9999-12-31'
update m_view_arenda set rent_finish_date = null where rent_finish_date < '1753-01-01' or rent_finish_date > '9999-12-31'
update m_view_arenda set rent_actual_finish_date = null where rent_actual_finish_date < '1753-01-01' or rent_actual_finish_date > '9999-12-31'
update m_view_arenda set date_akt = null where date_akt < '1753-01-01' or date_akt > '9999-12-31'
update m_view_arenda set date_bti = null where date_bti < '1753-01-01' or date_bti > '9999-12-31'
update m_view_arenda set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_streets set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update m_view_arenda_agreements set agreement_date = null where agreement_date < '1753-01-01' or agreement_date > '9999-12-31'
update m_view_arenda_agreements set pidstava_date = null where pidstava_date < '1753-01-01' or pidstava_date > '9999-12-31'
update m_view_arenda_agreements set rent_start_date = null where rent_start_date < '1753-01-01' or rent_start_date > '9999-12-31'
update m_view_arenda_agreements set rent_finish_date = null where rent_finish_date < '1753-01-01' or rent_finish_date > '9999-12-31'
update m_view_arenda_agreements set rent_actual_finish_date = null where rent_actual_finish_date < '1753-01-01' or rent_actual_finish_date > '9999-12-31'
update m_view_arenda_agreements set date_akt = null where date_akt < '1753-01-01' or date_akt > '9999-12-31'
update m_view_arenda_agreements set date_bti = null where date_bti < '1753-01-01' or date_bti > '9999-12-31'
update m_view_arenda_agreements set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update transfer_requests set create_date = null where create_date < '1753-01-01' or create_date > '9999-12-31'
update transfer_requests set new_akt_date = null where new_akt_date < '1753-01-01' or new_akt_date > '9999-12-31'
update transfer_requests set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update transfer_requests set rishrozp_date = null where rishrozp_date < '1753-01-01' or rishrozp_date > '9999-12-31'
update dict_org_contact_posada set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update bp_rish_project_info set document_date = null where document_date < '1753-01-01' or document_date > '9999-12-31'
update link_arenda_2_decisions set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update link_arenda_2_decisions set doc_date = null where doc_date < '1753-01-01' or doc_date > '9999-12-31'
update reports1nf_balans_deleted set approval_date = null where approval_date < '1753-01-01' or approval_date > '9999-12-31'
update reports1nf_balans_deleted set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_balans_deleted set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update reports1nf_balans_deleted set znos_date = null where znos_date < '1753-01-01' or znos_date > '9999-12-31'
update reports1nf_balans_deleted set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update reports1nf_balans_deleted set fair_cost_date = null where fair_cost_date < '1753-01-01' or fair_cost_date > '9999-12-31'
update reports1nf_balans_deleted set date_bti = null where date_bti < '1753-01-01' or date_bti > '9999-12-31'
update reports1nf_balans_deleted set date_cost_rinkova = null where date_cost_rinkova < '1753-01-01' or date_cost_rinkova > '9999-12-31'
update reports1nf_balans_deleted set ownership_doc_date = null where ownership_doc_date < '1753-01-01' or ownership_doc_date > '9999-12-31'
update reports1nf_balans_deleted set balans_doc_date = null where balans_doc_date < '1753-01-01' or balans_doc_date > '9999-12-31'
update reports1nf_balans_deleted set vidch_doc_date = null where vidch_doc_date < '1753-01-01' or vidch_doc_date > '9999-12-31'
update reports1nf_balans_deleted set submit_date = null where submit_date < '1753-01-01' or submit_date > '9999-12-31'
update object_rights set transfer_date = null where transfer_date < '1753-01-01' or transfer_date > '9999-12-31'
update object_rights set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_nadhodjennya set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_kved set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_arenda set agreement_date = null where agreement_date < '1753-01-01' or agreement_date > '9999-12-31'
update reports1nf_arenda set pidstava_date = null where pidstava_date < '1753-01-01' or pidstava_date > '9999-12-31'
update reports1nf_arenda set pidstava_date2 = null where pidstava_date2 < '1753-01-01' or pidstava_date2 > '9999-12-31'
update reports1nf_arenda set rent_start_date = null where rent_start_date < '1753-01-01' or rent_start_date > '9999-12-31'
update reports1nf_arenda set rent_finish_date = null where rent_finish_date < '1753-01-01' or rent_finish_date > '9999-12-31'
update reports1nf_arenda set rent_actual_finish_date = null where rent_actual_finish_date < '1753-01-01' or rent_actual_finish_date > '9999-12-31'
update reports1nf_arenda set order_date = null where order_date < '1753-01-01' or order_date > '9999-12-31'
update reports1nf_arenda set inactive_date = null where inactive_date < '1753-01-01' or inactive_date > '9999-12-31'
update reports1nf_arenda set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update reports1nf_arenda set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update reports1nf_arenda set date_expl_enter = null where date_expl_enter < '1753-01-01' or date_expl_enter > '9999-12-31'
update reports1nf_arenda set date_akt = null where date_akt < '1753-01-01' or date_akt > '9999-12-31'
update reports1nf_arenda set date_bti = null where date_bti < '1753-01-01' or date_bti > '9999-12-31'
update reports1nf_arenda set svidotstvo_date = null where svidotstvo_date < '1753-01-01' or svidotstvo_date > '9999-12-31'
update reports1nf_arenda set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_arenda set insurance_start = null where insurance_start < '1753-01-01' or insurance_start > '9999-12-31'
update reports1nf_arenda set insurance_end = null where insurance_end < '1753-01-01' or insurance_end > '9999-12-31'
update reports1nf_arenda set submit_date = null where submit_date < '1753-01-01' or submit_date > '9999-12-31'
update dict_org_vibuttya set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update arenda_decision_objects set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_districts set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_arenda_notes set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_arenda_notes set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update reports1nf_arenda_notes set date_expert = null where date_expert < '1753-01-01' or date_expert > '9999-12-31'
update dict_fin_report_type set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_arenda_agreement_kind set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_privat_status set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update rent_vipiski set payment_date = null where payment_date < '1753-01-01' or payment_date > '9999-12-31'
update dict_districts2 set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update bp_rish_project_signature set signed_on = null where signed_on < '1753-01-01' or signed_on > '9999-12-31'
update dict_fin_form_type set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update doc_dependencies set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_arenda_decisions set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_arenda_decisions set doc_date = null where doc_date < '1753-01-01' or doc_date > '9999-12-31'
update dict_org_cur_state set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_tech_state set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_fin_form_kind set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update dict_org_sfera_upr set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_arenda_rented set agreement_date = null where agreement_date < '1753-01-01' or agreement_date > '9999-12-31'
update reports1nf_arenda_rented set rent_start_date = null where rent_start_date < '1753-01-01' or rent_start_date > '9999-12-31'
update reports1nf_arenda_rented set rent_finish_date = null where rent_finish_date < '1753-01-01' or rent_finish_date > '9999-12-31'
update reports1nf_arenda_rented set modify_date = null where modify_date < '1753-01-01' or modify_date > '9999-12-31'
update reports1nf_arenda_rented set del_date = null where del_date < '1753-01-01' or del_date > '9999-12-31'
update reports1nf_arenda_rented set submit_date = null where submit_date < '1753-01-01' or submit_date > '9999-12-31'




/*

   Sync data from [organizations] table into [reports1nf_org_info] table to reflect changes
   that have occurred in the [Balans] database

*/

UPDATE [dbo].[reports1nf_org_info]
   SET [master_org_id] = o.[master_org_id]
      ,[last_state] = o.[last_state]
      ,[occupation_id] = o.[occupation_id]
      ,[status_id] = o.[status_id]
      ,[form_gosp_id] = o.[form_gosp_id]
      ,[form_ownership_id] = o.[form_ownership_id]
      ,[gosp_struct_id] = o.[gosp_struct_id]
      ,[organ_id] = o.[organ_id]
      ,[industry_id] = o.[industry_id]
      ,[nomer_obj] = o.[nomer_obj]
      ,[zkpo_code] = o.[zkpo_code]
      ,[addr_distr_old_id] = o.[addr_distr_old_id]
      ,[addr_distr_new_id] = o.[addr_distr_new_id]
      ,[addr_street_name] = o.[addr_street_name]
      ,[addr_street_id] = o.[addr_street_id]
      ,[addr_nomer] = o.[addr_nomer]
      ,[addr_nomer2] = o.[addr_nomer2]
      ,[addr_korpus] = o.[addr_korpus]
      ,[addr_zip_code] = o.[addr_zip_code]
      ,[addr_misc] = o.[addr_misc]
      ,[director_fio] = o.[director_fio]
      ,[director_phone] = o.[director_phone]
      ,[director_fio_kogo] = o.[director_fio_kogo]
      ,[director_title] = o.[director_title]
      ,[director_title_kogo] = o.[director_title_kogo]
      ,[director_doc] = o.[director_doc]
      ,[director_doc_kogo] = o.[director_doc_kogo]
      ,[director_email] = o.[director_email]
      ,[buhgalter_fio] = o.[buhgalter_fio]
      ,[buhgalter_phone] = o.[buhgalter_phone]
      ,[num_buildings] = o.[num_buildings]
      ,[full_name] = o.[full_name]
      ,[short_name] = o.[short_name]
      ,[priznak_id] = o.[priznak_id]
      ,[title_form_id] = o.[title_form_id]
      ,[form_1nf_id] = o.[form_1nf_id]
      ,[vedomstvo_id] = o.[vedomstvo_id]
      ,[title_id] = o.[title_id]
      ,[form_id] = o.[form_id]
      ,[gosp_struct_type_id] = o.[gosp_struct_type_id]
      ,[search_name] = o.[search_name]
      ,[name_komu] = o.[name_komu]
      ,[fax] = o.[fax]
      ,[registration_auth] = o.[registration_auth]
      ,[registration_num] = o.[registration_num]
      ,[registration_date] = o.[registration_date]
      ,[registration_svidot] = o.[registration_svidot]
      ,[l_year] = o.[l_year]
      ,[date_l_year] = o.[date_l_year]
      ,[sqr_on_balance] = o.[sqr_on_balance]
      ,[sqr_manufact] = o.[sqr_manufact]
      ,[sqr_non_manufact] = o.[sqr_non_manufact]
      ,[sqr_free_for_rent] = o.[sqr_free_for_rent]
      ,[sqr_total] = o.[sqr_total]
      ,[sqr_rented] = o.[sqr_rented]
      ,[sqr_privat] = o.[sqr_privat]
      ,[sqr_given_for_rent] = o.[sqr_given_for_rent]
      ,[sqr_znyata_z_balansu] = o.[sqr_znyata_z_balansu]
      ,[sqr_prodaj] = o.[sqr_prodaj]
      ,[sqr_spisani_zneseni] = o.[sqr_spisani_zneseni]
      ,[sqr_peredana] = o.[sqr_peredana]
      ,[num_objects] = o.[num_objects]
      ,[kved_code] = o.[kved_code]
      ,[date_stat_spravka] = o.[date_stat_spravka]
      ,[koatuu] = o.[koatuu]
      ,[modified_by] = o.[modified_by]
      ,[modify_date] = o.[modify_date]
      ,[share_type_id] = o.[share_type_id]
      ,[share] = o.[share]
      ,[bank_name] = o.[bank_name]
      ,[bank_mfo] = o.[bank_mfo]
      ,[is_deleted] = o.[is_deleted]
      ,[del_date] = o.[del_date]
      ,[otdel_gukv_id] = o.[otdel_gukv_id]
      ,[arch_id] = o.[arch_id]
      ,[arch_flag] = o.[arch_flag]
      ,[is_liquidated] = o.[is_liquidated]
      ,[liquidation_date] = o.[liquidation_date]
      ,[pidp_rda] = o.[pidp_rda]
      ,[is_arend] = o.[is_arend]
      ,[beg_state_date] = o.[beg_state_date]
      ,[end_state_date] = o.[end_state_date]
      ,[mayno_id] = o.[mayno_id]
      ,[contact_email] = o.[contact_email]
      ,[contact_posada_id] = o.[contact_posada_id]
      ,[nadhodjennya_id] = o.[nadhodjennya_id]
      ,[vibuttya_id] = o.[vibuttya_id]
      ,[privat_status_id] = o.[privat_status_id]
      ,[cur_state_id] = o.[cur_state_id]
      ,[sfera_upr_id] = o.[sfera_upr_id]
      ,[plan_zone_id] = o.[plan_zone_id]
      ,[registr_org_id] = o.[registr_org_id]
      ,[nadhodjennya_date] = o.[nadhodjennya_date]
      ,[vibuttya_date] = o.[vibuttya_date]
      ,[chastka] = o.[chastka]
      ,[registration_rish] = o.[registration_rish]
      ,[registration_dov_date] = o.[registration_dov_date]
      ,[registration_corp] = o.[registration_corp]
      ,[strok_start_date] = o.[strok_start_date]
      ,[strok_end_date] = o.[strok_end_date]
      ,[stat_fond] = o.[stat_fond]
      ,[size_plus] = o.[size_plus]
      ,[addr_zip_code_3] = o.[addr_zip_code_3]
      ,[old_industry_id] = o.[old_industry_id]
      ,[old_occupation_id] = o.[old_occupation_id]
      ,[old_organ_id] = o.[old_organ_id]
      ,[form_vlasn_vibuttya_id] = o.[form_vlasn_vibuttya_id]
      ,[addr_city] = o.[addr_city]
      ,[addr_flat_num] = o.[addr_flat_num]
      ,[povnovajennia] = o.[povnovajennia]
      ,[povnov_osoba_fio] = o.[povnov_osoba_fio]
      ,[povnov_passp_seria] = o.[povnov_passp_seria]
      ,[povnov_passp_num] = o.[povnov_passp_num]
      ,[povnov_passp_auth] = o.[povnov_passp_auth]
      ,[povnov_passp_date] = o.[povnov_passp_date]
      ,[director_passp_seria] = o.[director_passp_seria]
      ,[director_passp_num] = o.[director_passp_num]
      ,[director_passp_auth] = o.[director_passp_auth]
      ,[director_passp_date] = o.[director_passp_date]
      ,[registration_svid_date] = o.[registration_svid_date]
      ,[origin_db] = o.[origin_db]
      ,[budg_payments_rate] = o.[budg_payments_rate]
      ,[is_under_closing] = o.[is_under_closing]
      ,[phys_addr_street_id] = o.[phys_addr_street_id]
      ,[phys_addr_district_id] = o.[phys_addr_district_id]
      ,[phys_addr_nomer] = o.[phys_addr_nomer]
      ,[phys_addr_zip_code] = o.[phys_addr_zip_code]
      ,[phys_addr_misc] = o.[phys_addr_misc]
      ,[contribution_rate] = o.[contribution_rate]
      --,[budget_narah_50_uah] = o.[budget_narah_50_uah]
      --,[budget_zvit_50_uah] = o.[budget_zvit_50_uah]
      --,[budget_prev_50_uah] = o.[budget_prev_50_uah]
      --,[budget_debt_30_50_uah] = o.[budget_debt_30_50_uah]
      ,[is_special_organization] = o.[is_special_organization]
      --,[payment_budget_special] = o.[payment_budget_special]
      ,[buhgalter_email] = o.[buhgalter_email]
      --,[konkurs_payments] = o.[konkurs_payments]
      --,[unknown_payments] = o.[unknown_payments]
      --,[unknown_payment_note] = o.[unknown_payment_note]
FROM [organizations] o
INNER JOIN [reports1nf] r ON r.organization_id = o.id
WHERE r.id = [reports1nf_org_info].report_id;

--UPDATE balans SET is_deleted = 1 WHERE sqr_total = 0 AND ISNULL(is_deleted, 0) = 0;


UPDATE dict_org_ownership SET IS_RDA = 0
UPDATE dict_org_ownership SET IS_RDA = 1 WHERE ID = 34