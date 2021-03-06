﻿/******************************************************************************/
/*                     Drop all the foreign keys first                        */
/******************************************************************************/

/* Buildings foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_buildings_distr_new]') AND parent_object_id = OBJECT_ID(N'[dbo].[buildings]'))
--ALTER TABLE [buildings] DROP CONSTRAINT [fk_buildings_distr_new]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_buildings_distr_old]') AND parent_object_id = OBJECT_ID(N'[dbo].[buildings]'))
--ALTER TABLE [buildings] DROP CONSTRAINT [fk_buildings_distr_old]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_buildings_history]') AND parent_object_id = OBJECT_ID(N'[dbo].[buildings]'))
--ALTER TABLE [buildings] DROP CONSTRAINT [fk_buildings_history]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_buildings_obj_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[buildings]'))
--ALTER TABLE [buildings] DROP CONSTRAINT [fk_buildings_obj_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_buildings_obj_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[buildings]'))
--ALTER TABLE [buildings] DROP CONSTRAINT [fk_buildings_obj_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_buildings_tech_stan]') AND parent_object_id = OBJECT_ID(N'[dbo].[buildings]'))
--ALTER TABLE [buildings] DROP CONSTRAINT [fk_buildings_tech_stan]

--GO

/* Organizations foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_form_vlasn_vibuttya]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_form_vlasn_vibuttya]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_occupation]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_occupation]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_status]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_status]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_form_gosp]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_form_gosp]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_ownership]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_ownership]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_gosp_struct]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_gosp_struct]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_organ]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_organ]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_industry]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_distr_old]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_distr_old]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_distr_new]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_distr_new]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_priznak]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_priznak]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_title_form]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_title_form]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_form_1nf]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_form_1nf]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_vedomstvo]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_vedomstvo]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_title]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_title]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_form]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_form]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_gosp_struct_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_gosp_struct_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_share_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_share_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_otdel]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT [fk_organizations_otdel]

--GO

/* Organizations foreign keys from the Balans database */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_mayno]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_mayno

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_posada]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_posada

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_vibuttya]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_vibuttya

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_nadhodzhennya]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_nadhodzhennya

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_cur_state]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_cur_state

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_sfera_upr]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_sfera_upr

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_plan_zone]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_plan_zone

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_privat_status]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_privat_status

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_registr_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_registr_org

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_old_industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_old_industry

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_old_occupation]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_old_occupation

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_organizations_old_organ]') AND parent_object_id = OBJECT_ID(N'[dbo].[organizations]'))
--ALTER TABLE [organizations] DROP CONSTRAINT fk_organizations_old_organ

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_dict_org_old_occupation_old_industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[dict_org_old_occupation]'))
--ALTER TABLE [dict_org_old_occupation] DROP CONSTRAINT fk_dict_org_old_occupation_old_industry

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_dict_org_old_organ_old_industry]') AND parent_object_id = OBJECT_ID(N'[dbo].[dict_org_old_organ]'))
--ALTER TABLE dict_org_old_organ DROP CONSTRAINT fk_dict_org_old_organ_old_industry

--GO

/* Balans foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_object]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_object]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_o26]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_o26]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_bti]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_bti]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_form_ownership]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_form_ownership]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_object_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_object_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_history]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_history]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_tech_condition]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_tech_condition]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_object_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_object_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_purpose_group]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_purpose_group]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_purpose]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_purpose]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_ownership_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_ownership_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_form_giver]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_form_giver]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_org_maintain_id]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_org_maintain_id]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_update_src]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_update_src]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_otdel]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_otdel]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_vidch_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_vidch_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_vidch_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_vidch_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_obj_status]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans]'))
--ALTER TABLE [balans] DROP CONSTRAINT [fk_balans_obj_status]

--GO

/* Arenda foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_object]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_object]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_org_balans]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_org_balans]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_org_renter]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_org_renter]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_org_giver]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_org_giver]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_balans]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_balans]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_object_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_object_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_purpose_group]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_purpose_group]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_purpose]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_purpose]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_update_src]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_update_src]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_agreement_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_agreement_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_privat_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_privat_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_payment_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda]'))
--ALTER TABLE [arenda] DROP CONSTRAINT [fk_arenda_payment_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_notes_arenda]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_notes]'))
--ALTER TABLE [arenda_notes] DROP CONSTRAINT [fk_arenda_notes_arenda]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_notes_purpose_group]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_notes]'))
--ALTER TABLE [arenda_notes] DROP CONSTRAINT [fk_arenda_notes_purpose_group]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_notes_purpose]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_notes]'))
--ALTER TABLE [arenda_notes] DROP CONSTRAINT [fk_arenda_notes_purpose]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_notes_payment_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_notes]'))
--ALTER TABLE [arenda_notes] DROP CONSTRAINT [fk_arenda_notes_payment_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_notes_status]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_notes]'))
--ALTER TABLE [arenda_notes] DROP CONSTRAINT [fk_arenda_notes_status]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_payments_arenda]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_payments]'))
--ALTER TABLE [arenda_payments] DROP CONSTRAINT [fk_arenda_payments_arenda]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_payments_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_payments]'))
--ALTER TABLE [arenda_payments] DROP CONSTRAINT [fk_arenda_payments_period]

--GO

/* Document foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_documents_doc_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[documents]'))
--ALTER TABLE [documents] DROP CONSTRAINT [fk_documents_doc_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_documents_source]') AND parent_object_id = OBJECT_ID(N'[dbo].[documents]'))
--ALTER TABLE [documents] DROP CONSTRAINT [fk_documents_source]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_documents_state]') AND parent_object_id = OBJECT_ID(N'[dbo].[documents]'))
--ALTER TABLE [documents] DROP CONSTRAINT [fk_documents_state]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_doc_depend_master]') AND parent_object_id = OBJECT_ID(N'[dbo].[doc_dependencies]'))
--ALTER TABLE [doc_dependencies] DROP CONSTRAINT [fk_doc_depend_master]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_doc_depend_slave]') AND parent_object_id = OBJECT_ID(N'[dbo].[doc_dependencies]'))
--ALTER TABLE [doc_dependencies] DROP CONSTRAINT [fk_doc_depend_slave]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_doc_depend_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[doc_dependencies]'))
--ALTER TABLE [doc_dependencies] DROP CONSTRAINT [fk_doc_depend_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_document]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_document]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_master_doc]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_master_doc]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_purpose_group]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_purpose_group]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_purpose]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_purpose]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_tech_condition]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_tech_condition]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_object_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_object_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_object_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_object_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_form_ownership]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_form_ownership]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_change_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_change_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_changed_doc]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_changed_doc]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_building_docs_last_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[building_docs]'))
--ALTER TABLE [building_docs] DROP CONSTRAINT [fk_building_docs_last_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_doc_appendix_kind_otdel]') AND parent_object_id = OBJECT_ID(N'[dbo].[dict_doc_appendix_kind]'))
--ALTER TABLE [dict_doc_appendix_kind] DROP CONSTRAINT [fk_doc_appendix_kind_otdel]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_appendices_doc]') AND parent_object_id = OBJECT_ID(N'[dbo].[doc_appendices]'))
--ALTER TABLE [doc_appendices] DROP CONSTRAINT [fk_appendices_doc]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_appendices_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[doc_appendices]'))
--ALTER TABLE [doc_appendices] DROP CONSTRAINT [fk_appendices_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_appendices_stadiya]') AND parent_object_id = OBJECT_ID(N'[dbo].[doc_appendices]'))
--ALTER TABLE [doc_appendices] DROP CONSTRAINT [fk_appendices_stadiya]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_docs_balans]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans_docs]'))
--ALTER TABLE [balans_docs] DROP CONSTRAINT [fk_balans_docs_balans]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_docs_bdoc]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans_docs]'))
--ALTER TABLE [balans_docs] DROP CONSTRAINT [fk_balans_docs_bdoc]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_docs_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans_docs]'))
--ALTER TABLE [balans_docs] DROP CONSTRAINT [fk_balans_docs_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_balans_docs_doc]') AND parent_object_id = OBJECT_ID(N'[dbo].[balans_docs]'))
--ALTER TABLE [balans_docs] DROP CONSTRAINT [fk_balans_docs_doc]

--GO

--/* Organization documents foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_org_docs_organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[org_docs]'))
--ALTER TABLE [org_docs] DROP CONSTRAINT [fk_org_docs_organization]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_org_docs_document]') AND parent_object_id = OBJECT_ID(N'[dbo].[org_docs]'))
--ALTER TABLE [org_docs] DROP CONSTRAINT [fk_org_docs_document]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_org_docs_link_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[org_docs]'))
--ALTER TABLE [org_docs] DROP CONSTRAINT [fk_org_docs_link_kind]

--/* Arenda documents foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_applications_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_applications]'))
--ALTER TABLE [arenda_applications] DROP CONSTRAINT [fk_rent_applications_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_applications_renter]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_applications]'))
--ALTER TABLE [arenda_applications] DROP CONSTRAINT [fk_rent_applications_renter]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_applications_giver]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_applications]'))
--ALTER TABLE [arenda_applications] DROP CONSTRAINT [fk_rent_applications_giver]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_applications_decision]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_applications]'))
--ALTER TABLE [arenda_applications] DROP CONSTRAINT [fk_rent_applications_decision]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_applications_stadiya]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_applications]'))
--ALTER TABLE [arenda_applications] DROP CONSTRAINT [fk_rent_applications_stadiya]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_applications_appendix]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_applications]'))
--ALTER TABLE [arenda_applications] DROP CONSTRAINT [fk_rent_applications_appendix]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_decisions_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decisions]'))
--ALTER TABLE [arenda_decisions] DROP CONSTRAINT [fk_rent_decisions_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_decisions_renter]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decisions]'))
--ALTER TABLE [arenda_decisions] DROP CONSTRAINT [fk_rent_decisions_renter]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_decisions_giver]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decisions]'))
--ALTER TABLE [arenda_decisions] DROP CONSTRAINT [fk_rent_decisions_giver]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_decisions_balans]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decisions]'))
--ALTER TABLE [arenda_decisions] DROP CONSTRAINT [fk_rent_decisions_balans]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_decisions_decision]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decisions]'))
--ALTER TABLE [arenda_decisions] DROP CONSTRAINT [fk_rent_decisions_decision]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_decisions_stadiya]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decisions]'))
--ALTER TABLE [arenda_decisions] DROP CONSTRAINT [fk_rent_decisions_stadiya]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_decisions_appendix]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decisions]'))
--ALTER TABLE [arenda_decisions] DROP CONSTRAINT [fk_rent_decisions_appendix]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_decisions_protocol]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decisions]'))
--ALTER TABLE [arenda_decisions] DROP CONSTRAINT [fk_rent_decisions_protocol]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_decisions_raspor]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decisions]'))
--ALTER TABLE [arenda_decisions] DROP CONSTRAINT [fk_rent_decisions_raspor]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_rishen_arenda]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_rishen]'))
--ALTER TABLE [arenda_rishen] DROP CONSTRAINT [fk_arenda_rishen_arenda]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_rishen_appl]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_rishen]'))
--ALTER TABLE [arenda_rishen] DROP CONSTRAINT [fk_arenda_rishen_appl]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_rishen_decision]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_rishen]'))
--ALTER TABLE [arenda_rishen] DROP CONSTRAINT [fk_arenda_rishen_decision]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_rishen_raspor]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_rishen]'))
--ALTER TABLE [arenda_rishen] DROP CONSTRAINT [fk_arenda_rishen_raspor]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda2decision_arenda]') AND parent_object_id = OBJECT_ID(N'[dbo].[link_arenda_2_decisions]'))
--ALTER TABLE link_arenda_2_decisions DROP CONSTRAINT [fk_arenda2decision_arenda]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda2decision_appl]') AND parent_object_id = OBJECT_ID(N'[dbo].[link_arenda_2_decisions]'))
--ALTER TABLE link_arenda_2_decisions DROP CONSTRAINT [fk_arenda2decision_appl]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda2decision_decision]') AND parent_object_id = OBJECT_ID(N'[dbo].[link_arenda_2_decisions]'))
--ALTER TABLE link_arenda_2_decisions DROP CONSTRAINT [fk_arenda2decision_decision]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda2decision_raspor]') AND parent_object_id = OBJECT_ID(N'[dbo].[link_arenda_2_decisions]'))
--ALTER TABLE link_arenda_2_decisions DROP CONSTRAINT [fk_arenda2decision_raspor]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_appl_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_applications]'))
--ALTER TABLE arenda_applications DROP CONSTRAINT [fk_arenda_appl_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_arenda_appl_renter]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_applications]'))
--ALTER TABLE arenda_applications DROP CONSTRAINT [fk_arenda_appl_renter]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_ado_appl]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decision_objects]'))
--ALTER TABLE arenda_decision_objects DROP CONSTRAINT [fk_ado_appl]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_ado_rishen]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decision_objects]'))
--ALTER TABLE arenda_decision_objects DROP CONSTRAINT [fk_ado_rishen]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_ado_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[arenda_decision_objects]'))
--ALTER TABLE arenda_decision_objects DROP CONSTRAINT [fk_ado_building]

--GO

--/* 'Privatizations' foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_obj_group]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_obj_group]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_privat_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_privat_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_privat_state]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_privat_state]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_complex]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_complex]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_subordination]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_subordination]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_obj_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_obj_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_obj_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_obj_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_purpose_group]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_purpose_group]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_history]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_history]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_privatization_district]') AND parent_object_id = OBJECT_ID(N'[dbo].[privatization]'))
--ALTER TABLE [privatization] DROP CONSTRAINT [fk_privatization_district]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_priv_docs_privatization]') AND parent_object_id = OBJECT_ID(N'[dbo].[priv_object_docs]'))
--ALTER TABLE [priv_object_docs] DROP CONSTRAINT [fk_priv_docs_privatization]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_priv_docs_document]') AND parent_object_id = OBJECT_ID(N'[dbo].[priv_object_docs]'))
--ALTER TABLE [priv_object_docs] DROP CONSTRAINT [fk_priv_docs_document]

--GO

--/* 'Rosporadjennia' foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_obj_rights_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights]'))
--ALTER TABLE [object_rights] DROP CONSTRAINT [fk_obj_rights_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_obj_rights_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights]'))
--ALTER TABLE [object_rights] DROP CONSTRAINT [fk_obj_rights_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_obj_rights_right]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights]'))
--ALTER TABLE [object_rights] DROP CONSTRAINT [fk_obj_rights_right]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_obj_rights_akt_doc]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights]'))
--ALTER TABLE [object_rights] DROP CONSTRAINT [fk_obj_rights_akt_doc]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_obj_rights_rozp_doc]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights]'))
--ALTER TABLE [object_rights] DROP CONSTRAINT [fk_obj_rights_rozp_doc]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_obj_rights_doc]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights]'))
--ALTER TABLE [object_rights] DROP CONSTRAINT [fk_obj_rights_doc]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_obj_rights_org_from]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights]'))
--ALTER TABLE [object_rights] DROP CONSTRAINT [fk_obj_rights_org_from]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_obj_rights_org_to]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights]'))
--ALTER TABLE [object_rights] DROP CONSTRAINT [fk_obj_rights_org_to]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_restrictions_doc]') AND parent_object_id = OBJECT_ID(N'[dbo].[restrictions]'))
--ALTER TABLE [restrictions] DROP CONSTRAINT [fk_restrictions_doc]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_restrictions_restriction]') AND parent_object_id = OBJECT_ID(N'[dbo].[restrictions]'))
--ALTER TABLE [restrictions] DROP CONSTRAINT [fk_restrictions_restriction]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_restrictions_restr_group]') AND parent_object_id = OBJECT_ID(N'[dbo].[restrictions]'))
--ALTER TABLE [restrictions] DROP CONSTRAINT [fk_restrictions_restr_group]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_orbd_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights_building_docs]'))
--ALTER TABLE [object_rights_building_docs] DROP CONSTRAINT [fk_orbd_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_orbd_document]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights_building_docs]'))
--ALTER TABLE [object_rights_building_docs] DROP CONSTRAINT [fk_orbd_document]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_orbd_purpose_group]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights_building_docs]'))
--ALTER TABLE [object_rights_building_docs] DROP CONSTRAINT [fk_orbd_purpose_group]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_orbd_purpose]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights_building_docs]'))
--ALTER TABLE [object_rights_building_docs] DROP CONSTRAINT [fk_orbd_purpose]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_orbd_tech_condition]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights_building_docs]'))
--ALTER TABLE [object_rights_building_docs] DROP CONSTRAINT [fk_orbd_tech_condition]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_orbd_object_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights_building_docs]'))
--ALTER TABLE [object_rights_building_docs] DROP CONSTRAINT [fk_orbd_object_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_orbd_object_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[object_rights_building_docs]'))
--ALTER TABLE [object_rights_building_docs] DROP CONSTRAINT [fk_orbd_object_type]

--GO

--/* Financial report foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_formulae_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_formulae]'))
--ALTER TABLE [fin_report_formulae] DROP CONSTRAINT [fk_fin_formulae_period]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_values_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_values]'))
--ALTER TABLE [fin_report_values] DROP CONSTRAINT fk_fin_values_org

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_values_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_values]'))
--ALTER TABLE [fin_report_values] DROP CONSTRAINT [fk_fin_values_period]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_formula_values_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_formula_values]'))
--ALTER TABLE [fin_report_formula_values] DROP CONSTRAINT [fk_fin_formula_values_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_formula_values_formula]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_formula_values]'))
--ALTER TABLE [fin_report_formula_values] DROP CONSTRAINT [fk_fin_formula_values_formula]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_formula_values_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_formula_values]'))
--ALTER TABLE [fin_report_formula_values] DROP CONSTRAINT [fk_fin_formula_values_period]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_form_kind]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_forms]'))
--ALTER TABLE [fin_report_forms] DROP CONSTRAINT [fk_fin_form_kind]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_form_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_forms]'))
--ALTER TABLE [fin_report_forms] DROP CONSTRAINT [fk_fin_form_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_form_report_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_forms]'))
--ALTER TABLE [fin_report_forms] DROP CONSTRAINT [fk_fin_form_report_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_form_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_forms]'))
--ALTER TABLE [fin_report_forms] DROP CONSTRAINT [fk_fin_form_period]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_row_form]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_rows]'))
--ALTER TABLE [fin_report_rows] DROP CONSTRAINT [fk_fin_row_form]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_col_form]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_columns]'))
--ALTER TABLE [fin_report_columns] DROP CONSTRAINT [fk_fin_col_form]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_cell_form]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_report_cells]'))
--ALTER TABLE [fin_report_cells] DROP CONSTRAINT [fk_fin_cell_form]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_form_changes_form]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_form_changes]'))
--ALTER TABLE [fin_form_changes] DROP CONSTRAINT [fk_fin_form_changes_form]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_fin_form_changes_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_form_changes]'))
--ALTER TABLE [fin_form_changes] DROP CONSTRAINT [fk_fin_form_changes_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_budget_rate_by_ind_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_budget_rate_by_industry]'))
--ALTER TABLE [fin_budget_rate_by_industry] DROP CONSTRAINT [fk_budget_rate_by_ind_period]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_budget_rate_by_ind_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_budget_rate_by_industry]'))
--ALTER TABLE [fin_budget_rate_by_industry] DROP CONSTRAINT [fk_budget_rate_by_ind_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_budget_rate_by_ind_occ]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_budget_rate_by_industry]'))
--ALTER TABLE [fin_budget_rate_by_industry] DROP CONSTRAINT [fk_budget_rate_by_ind_occ]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_budget_rate_by_org_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_budget_rate_by_org]'))
--ALTER TABLE [fin_budget_rate_by_org] DROP CONSTRAINT [fk_budget_rate_by_org_period]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_budget_rate_by_org_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[fin_budget_rate_by_org]'))
--ALTER TABLE [fin_budget_rate_by_org] DROP CONSTRAINT [fk_budget_rate_by_org_org]

--GO

--/* 'Orendna Plata' foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_payment_org_balans]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment]'))
--ALTER TABLE [rent_payment] DROP CONSTRAINT [fk_rent_payment_org_balans]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_payment_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment]'))
--ALTER TABLE [rent_payment] DROP CONSTRAINT [fk_rent_payment_period]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_balans_org_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_balans_org]'))
--ALTER TABLE [rent_balans_org] DROP CONSTRAINT [fk_rent_balans_org_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_balans_org_occupation]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_balans_org]'))
--ALTER TABLE [rent_balans_org] DROP CONSTRAINT [fk_rent_balans_org_occupation]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_balans_org_dkk]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_balans_org]'))
--ALTER TABLE [rent_balans_org] DROP CONSTRAINT [fk_rent_balans_org_dkk]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_renter_org_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_renter_org]'))
--ALTER TABLE [rent_renter_org] DROP CONSTRAINT [fk_rent_renter_org_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_object_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_object]'))
--ALTER TABLE [rent_object] DROP CONSTRAINT [fk_rent_object_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_object_payment]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_object]'))
--ALTER TABLE [rent_object] DROP CONSTRAINT [fk_rent_object_payment]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_object_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_object]'))
--ALTER TABLE [rent_object] DROP CONSTRAINT [fk_rent_object_period]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_object_district]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_object]'))
--ALTER TABLE [rent_object] DROP CONSTRAINT [fk_rent_object_district]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_object_note]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_object]'))
--ALTER TABLE [rent_object] DROP CONSTRAINT [fk_rent_object_note]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_payment_by_renter_payment]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment_by_renter]'))
--ALTER TABLE [rent_payment_by_renter] DROP CONSTRAINT [fk_payment_by_renter_payment]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_payment_by_renter_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment_by_renter]'))
--ALTER TABLE [rent_payment_by_renter] DROP CONSTRAINT [fk_payment_by_renter_period]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_payment_by_renter_org_renter]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment_by_renter]'))
--ALTER TABLE [rent_payment_by_renter] DROP CONSTRAINT [fk_payment_by_renter_org_renter]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_payment_by_renter_org_giver]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment_by_renter]'))
--ALTER TABLE [rent_payment_by_renter] DROP CONSTRAINT [fk_payment_by_renter_org_giver]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_payment_by_renter_renter]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment_by_renter]'))
--ALTER TABLE [rent_payment_by_renter] DROP CONSTRAINT [fk_payment_by_renter_renter]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_payment_debt_payment]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment_debt]'))
--ALTER TABLE [rent_payment_debt] DROP CONSTRAINT [fk_payment_debt_payment]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_payment_debt_renter]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment_debt]'))
--ALTER TABLE [rent_payment_debt] DROP CONSTRAINT [fk_payment_debt_renter]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_payment_debt_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment_debt]'))
--ALTER TABLE [rent_payment_debt] DROP CONSTRAINT [fk_payment_debt_period]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_payment_debt_org_giver]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_payment_debt]'))
--ALTER TABLE [rent_payment_debt] DROP CONSTRAINT [fk_payment_debt_org_giver]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_vipiski_org_balans]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_vipiski]'))
--ALTER TABLE [rent_vipiski] DROP CONSTRAINT [fk_rent_vipiski_org_balans]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_vipiski_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_vipiski]'))
--ALTER TABLE [rent_vipiski] DROP CONSTRAINT [fk_rent_vipiski_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_vipiski_payment_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_vipiski]'))
--ALTER TABLE [rent_vipiski] DROP CONSTRAINT [fk_rent_vipiski_payment_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_free_square_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_free_square]'))
--ALTER TABLE [rent_free_square] DROP CONSTRAINT [fk_rent_free_square_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_free_square_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_free_square]'))
--ALTER TABLE [rent_free_square] DROP CONSTRAINT [fk_rent_free_square_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_rent_free_square_period]') AND parent_object_id = OBJECT_ID(N'[dbo].[rent_free_square]'))
--ALTER TABLE [rent_free_square] DROP CONSTRAINT [fk_rent_free_square_period]

--GO

--/* 'Nezalezhna Otsinka' foreign keys */

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_dict_expert_district]') AND parent_object_id = OBJECT_ID(N'[dbo].[dict_expert]'))
--ALTER TABLE [dict_expert] DROP CONSTRAINT [fk_dict_expert_district]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_note_renter]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_note]'))
--ALTER TABLE [expert_note] DROP CONSTRAINT [fk_expert_note_renter]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_note_bal_org]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_note]'))
--ALTER TABLE [expert_note] DROP CONSTRAINT [fk_expert_note_bal_org]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_note_building]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_note]'))
--ALTER TABLE [expert_note] DROP CONSTRAINT [fk_expert_note_building]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_note_balans]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_note]'))
--ALTER TABLE [expert_note] DROP CONSTRAINT [fk_expert_note_balans]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_note_district]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_note]'))
--ALTER TABLE [expert_note] DROP CONSTRAINT [fk_expert_note_district]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_note_obj_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_note]'))
--ALTER TABLE [expert_note] DROP CONSTRAINT [fk_expert_note_obj_type]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_note_expert]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_note]'))
--ALTER TABLE [expert_note] DROP CONSTRAINT [fk_expert_note_expert]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_note_rezenz]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_note]'))
--ALTER TABLE [expert_note] DROP CONSTRAINT [fk_expert_note_rezenz]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_note_detail_note]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_note_detail]'))
--ALTER TABLE [expert_note_detail] DROP CONSTRAINT [fk_expert_note_detail_note]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_input_doc_note]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_input_doc]'))
--ALTER TABLE [expert_input_doc] DROP CONSTRAINT [fk_expert_input_doc_note]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_input_doc_korr]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_input_doc]'))
--ALTER TABLE [expert_input_doc] DROP CONSTRAINT [fk_expert_input_doc_korr]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_output_doc_note]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_output_doc]'))
--ALTER TABLE [expert_output_doc] DROP CONSTRAINT [fk_expert_output_doc_note]

--IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_expert_output_doc_rezenz_type]') AND parent_object_id = OBJECT_ID(N'[dbo].[expert_output_doc]'))
--ALTER TABLE [expert_output_doc] DROP CONSTRAINT [fk_expert_output_doc_rezenz_type]

--GO

/******************************************************************************/
/*                  A simple dictionary with YES/NO values                    */
/******************************************************************************/

IF OBJECT_ID('dict_yes_no') IS NULL
BEGIN
	CREATE TABLE dict_yes_no (
		id       INTEGER PRIMARY KEY,
		name     VARCHAR(12)
	);
	
	INSERT INTO dict_yes_no (id, name) VALUES (0, 'НІ')
	INSERT INTO dict_yes_no (id, name) VALUES (1, 'ТАК')
END

GO

/******************************************************************************/
/*            Objects (buildings) and the required dictionaries               */
/******************************************************************************/

/*    dict_streets    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_streets]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_streets]

--CREATE TABLE dict_streets /* SUL */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(160) DEFAULT ' ', /* NAME */
--    name_output        VARCHAR(160) DEFAULT ' ', /* NAME_OUTPUT */
--    kind               VARCHAR(50) DEFAULT ' ', /* KIND_NAME */
--    stan               INTEGER NOT NULL, /* STAN */
--    activity           INTEGER, /* ACTIVITY */
--    renamed_from_doc_id INTEGER, /* DOK_ID_FROM */
--    renamed_to_doc_id  INTEGER, /* DOK_ID_TO */
--    parent_id          INTEGER, /* PARENT_CODE */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_streets ADD PRIMARY KEY (id, stan);

/*    dict_kved    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_kved]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_kved]

--CREATE TABLE dict_kved /* S_KVED */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    code               VARCHAR(16), /* KOD_KVED */
--    name               VARCHAR(250), /* NU */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_kved ADD PRIMARY KEY (id);

/*    dict_districts    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_districts]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_districts]

--CREATE TABLE dict_districts /* S_RAYON */ (
--    id                 INTEGER NOT NULL, /* KOD_RAYON */
--    name               VARCHAR(64) DEFAULT ' ', /* NAME_RAYON */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_districts ADD PRIMARY KEY (id);

/*    dict_districts2    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_districts2]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_districts2]

--CREATE TABLE dict_districts2 /* S_RAYON2 */ (
--    id                 INTEGER NOT NULL, /* KOD_RAYON2 */
--    name               VARCHAR(64) DEFAULT ' ', /* NAME_RAYON2 */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_districts2 ADD PRIMARY KEY (id);

/*    dict_tech_state    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_tech_state]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_tech_state]

--CREATE TABLE dict_tech_state /* STEXSTAN */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(64) DEFAULT ' ', /* NAME */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_tech_state ADD PRIMARY KEY (id);

/*    dict_history    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_history]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_history]

--CREATE TABLE dict_history /* SHISTORY */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(64) DEFAULT ' ', /* NAME */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_history ADD PRIMARY KEY (id);

/*    dict_object_type    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_object_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_object_type]

--CREATE TABLE dict_object_type /* STYPEOBJ */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(64) DEFAULT ' ', /* NAME */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_object_type ADD PRIMARY KEY (id);

/*    dict_object_kind    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_object_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_object_kind]

--CREATE TABLE dict_object_kind /* SKINDOBJ */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(64) DEFAULT ' ', /* NAME */
--    mnemonic           VARCHAR(8) DEFAULT ' ', /* MNEMONIC */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_object_kind ADD PRIMARY KEY (id);

/*    dict_oatuu    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_oatuu]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_oatuu]

--CREATE TABLE dict_oatuu /* S_OATUU */ (
--    id                 VARCHAR(12) NOT NULL, /* KOD */
--    name               VARCHAR(128) DEFAULT ' ', /* NAZVA */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_oatuu ADD PRIMARY KEY (id);

/*    dict_facade    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_facade]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_facade]

--CREATE TABLE dict_facade /* S_OZNAKA_BUD */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(64) DEFAULT ' ', /* NAME */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_facade ADD PRIMARY KEY (id);

/*    buildings    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[buildings]') AND type in (N'U'))
--DROP TABLE [dbo].[buildings]

--CREATE TABLE buildings /* OBJECT_1NF */ (
--    id                 INTEGER NOT NULL, /* OBJECT_KOD */
--    master_building_id INTEGER, /* If this ID is not NULL, it means that this address is a duplicate of the 'master' address */
--    addr_street_name   VARCHAR(128) DEFAULT ' ', /* ULNAME */
--    addr_street_id     INTEGER, /* ULKOD - TABLE SUL */
--    addr_street_name2  VARCHAR(128), /* ULNAME2 */
--    addr_street_id2    INTEGER,      /* ULKOD2 - TABLE SUL */
--    street_full_name   VARCHAR(128), /* FULL_ULNAME */
--    addr_distr_old_id  INTEGER, /* DISTR - TABLE S_RAYON */
--    addr_distr_new_id  INTEGER, /* NEWDISTR - TABLE S_RAYON2 */
--    addr_nomer1        VARCHAR(32) DEFAULT ' ', /* NOMER1 */
--    addr_nomer2        VARCHAR(32) DEFAULT ' ', /* NOMER2 */
--    addr_nomer3        VARCHAR(32) DEFAULT ' ', /* NOMER3 */
--    addr_nomer         AS (LTRIM(RTRIM(ISNULL(addr_nomer1, ''))) + ' ' + LTRIM(RTRIM(ISNULL(addr_nomer2, '')))+ ' ' + LTRIM(RTRIM(ISNULL(addr_nomer3, '')))),
--    addr_misc          VARCHAR(128) DEFAULT ' ', /* ADRDOP */
--    addr_korpus_flag   INTEGER, /* KORPUS */
--    addr_korpus        VARCHAR(255), /* ADDRESS_NO_KORPUS */
--    addr_zip_code      VARCHAR(24), /* IKOD */
--    addr_address       AS (ISNULL(street_full_name, '') + ' ' + LTRIM(RTRIM(ISNULL(addr_nomer1, ''))) + ' ' + LTRIM(RTRIM(ISNULL(addr_nomer2, '')))+ ' ' + LTRIM(RTRIM(ISNULL(addr_nomer3, ''))) + ' ' + ISNULL(addr_misc, '')),
--    tech_condition_id  INTEGER,  /* TEXSTAN - TABLE STEXSTAN */
--    date_begin         DATE, /* DT_BEG */
--    date_end           DATE, /* DT_END */
--    num_floors         VARCHAR(24), /* PNUM */
--    construct_year     INTEGER, /* BUDYEAR */
--    condition_year     INTEGER, /* STAN_YEAR */
--    is_condition_valid INTEGER, /* REALSTAN */
--    bti_code           VARCHAR(24), /* KODBTI  */
--    history_id         INTEGER, /* HISTORY - TABLE SHISTORY */
--    object_type_id     INTEGER, /* TYPOBJ - TABLE STYPEOBJ */
--    object_kind_id     INTEGER, /* VIDOBJ - TABLE SKINDOBJ */
--    is_land            INTEGER, /* ZEMLIA */
--    modify_date        DATE, /* DT */
--    modified_by        VARCHAR(128), /* ISP */
--    kadastr_code       VARCHAR(32), /* KADASTR */
--    cost_balans        NUMERIC(15,3), /* BALANS_VARTIST */
--    sqr_total          NUMERIC(9,2), /* SZAG */
--    sqr_pidval         NUMERIC(9,2), /* SPIDV */
--    sqr_mk             NUMERIC(9,2), /* V_MK */
--    sqr_dk             NUMERIC(9,2), /* V_DK */
--    sqr_rk             NUMERIC(9,2), /* V_RK */
--    sqr_other          NUMERIC(9,2), /* V_IN */
--    sqr_rented         NUMERIC(9,2), /* V_AR */
--    sqr_zagal          NUMERIC(9,2), /* V_OB */
--    sqr_for_rent       NUMERIC(9,2), /* V_PR */
--    sqr_habit          NUMERIC(9,2), /* V_JIL */
--    sqr_non_habit      NUMERIC(9,2), /* V_NEJ */
--    additional_info    VARCHAR(252), /* DODAT_VID */
--    is_deleted         INTEGER, /* DELETED */
--    del_date           DATE, /* DTDELETE */
--    oatuu_id           VARCHAR(10), /* KOD_OATUU - TABLE S_OATUU */
--    updpr              INTEGER, /* UPDPR */
--    arch_id            INTEGER, /* ARCH_KOD */
--    arch_flag          INTEGER DEFAULT 0, /* IN_ARCH */
--    facade_id          INTEGER, /* OZNAKA_BUD - TABLE S_OZNAKA_BUD */
--    nomer_int          INTEGER, /* NOMER_INT */
--    characteristics    VARCHAR(MAX), /* CHARACTERISTIC */
--    expl_enter_year    INTEGER, /* Год ввода в єксплуатацию (не хранится в 1НФ) */
--    is_basement_exists INTEGER, /* Существует ли подвал (не хранится в 1НФ) */
--    is_loft_exists     INTEGER,  /* Существует ли чердак (не хранится в 1НФ) */
--    sqr_loft           NUMERIC(9,2) /* Площадь чердака (не хранится в 1НФ) */
--);

--ALTER TABLE buildings ADD PRIMARY KEY (id);

--ALTER TABLE buildings ADD CONSTRAINT fk_buildings_history FOREIGN KEY (history_id) REFERENCES dict_history (id);
--ALTER TABLE buildings ADD CONSTRAINT fk_buildings_distr_old FOREIGN KEY (addr_distr_old_id) REFERENCES dict_districts (id);
--ALTER TABLE buildings ADD CONSTRAINT fk_buildings_distr_new FOREIGN KEY (addr_distr_new_id) REFERENCES dict_districts2 (id);
--ALTER TABLE buildings ADD CONSTRAINT fk_buildings_tech_stan FOREIGN KEY (tech_condition_id) REFERENCES dict_tech_state (id);
--ALTER TABLE buildings ADD CONSTRAINT fk_buildings_obj_type FOREIGN KEY (object_type_id) REFERENCES dict_object_type (id);
--ALTER TABLE buildings ADD CONSTRAINT fk_buildings_obj_kind FOREIGN KEY (object_kind_id) REFERENCES dict_object_kind (id);

--GO

/******************************************************************************/
/*                Organizations and the required dictionaries                 */
/******************************************************************************/

/* dict_org_occupation */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_occupation]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_occupation]

--CREATE TABLE dict_org_occupation /* S_VID_DIAL */ (
--    id             INTEGER NOT NULL, /* KOD_VID_DIAL */
--    name           VARCHAR(50), /* NAME_VID_DIAL */
--    branch_code    INTEGER, /* KOD_GALUZ */
--    modified_by    VARCHAR(128), /* ISP */
--    modify_date    DATE /* DT */
--);

--ALTER TABLE dict_org_occupation ADD PRIMARY KEY (id);

/* dict_org_status */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_status]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_status]

--CREATE TABLE dict_org_status /* S_STATUS */(
--    id             INTEGER NOT NULL, /* KOD_STATUS */
--    name           VARCHAR(20), /* NAME_STATUS */
--    modified_by    VARCHAR(128), /* ISP */
--    modify_date    DATE /* DT */
--);

--ALTER TABLE dict_org_status ADD PRIMARY KEY (id);

/* dict_org_form_gosp */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_form_gosp]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_form_gosp]

--CREATE TABLE dict_org_form_gosp /* S_FORM_GOSP */ (
--    id             INTEGER NOT NULL, /* KOD_FORM_GOSP */
--    name           VARCHAR(40), /* NAME_FORM_GOSP */
--    modified_by    VARCHAR(128), /* ISP */
--    modify_date    DATE /* DT */
--);

--ALTER TABLE dict_org_form_gosp ADD PRIMARY KEY (id);

/* dict_org_ownership */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_ownership]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_ownership]

--CREATE TABLE dict_org_ownership /* S_FORM_VLASN */ (
--    id               INTEGER NOT NULL, /* KOD_FORM_VLASN */
--    name             VARCHAR(40), /* NAME_FORM_VLASN */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE, /* DT */
--    display_name     VARCHAR(40), /* NAME2 */
--    is_rda		   BIT NOT NULL
--);

--ALTER TABLE dict_org_ownership ADD PRIMARY KEY (id);
--ALTER TABLE [dbo].[dict_org_ownership] ADD  CONSTRAINT [DF_dict_org_ownership_is_rda]  DEFAULT ((0)) FOR [is_rda]

/* dict_org_gosp_struct */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_gosp_struct]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_gosp_struct]

--CREATE TABLE dict_org_gosp_struct /* S_GOSP_STRUKT */ (
--    id               INTEGER NOT NULL, /* KOD_GOSP_STRUKT */
--    name             VARCHAR(32), /* NAME_GOSP_STRUKT */
--    industry_code    INTEGER, /* KOD_GALUZ */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE, /* DT */
--    display_name     AS (STR(industry_code) + '-' + name)
--);

--ALTER TABLE dict_org_gosp_struct ADD PRIMARY KEY (id);

/* dict_org_organ */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_organ]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_organ]

--CREATE TABLE dict_org_organ /* S_ORGAN */ (
--    id               INTEGER NOT NULL, /* KOD_ORGAN */
--    name             VARCHAR(50), /* NAME_ORGAN */
--    industry_code    INTEGER, /* KOD_GALUZ */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE /* DT */
--);

--ALTER TABLE dict_org_organ ADD PRIMARY KEY (id);

/* dict_org_industry */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_industry]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_industry]

--CREATE TABLE dict_org_industry /* S_GALUZ */ (
--    id               INTEGER NOT NULL, /* KOD_GALUZ */
--    name             VARCHAR(40), /* NAME_GALUZ */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE /* DT */
--);

--ALTER TABLE dict_org_industry ADD PRIMARY KEY (id);

/* dict_org_priznak */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_priznak]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_priznak]

--CREATE TABLE dict_org_priznak /* S_ADDITION_PRIZNAK */ (
--    id               INTEGER NOT NULL, /* KOD_ADDITION_PRIZNAK */
--    name             VARCHAR(30), /* NAME_ADDITION_PRIZNAK */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE /* DT */
--);

--ALTER TABLE dict_org_priznak ADD PRIMARY KEY (id);

/* dict_org_title_form */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_title_form]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_title_form]

--CREATE TABLE dict_org_title_form /* S_PRIKMET */ (
--    id               INTEGER NOT NULL, /* KOD_PRIKMET */
--    name_ukr_male    VARCHAR(52), /* NAME_PRIKM_UKR_M */
--    name_ukr_fem     VARCHAR(52), /* NAME_PRIKM_UKR_W */
--    name_ukr_single  VARCHAR(52), /* NAME_PRIKM_UKR_O */
--    name_ukr_mult    VARCHAR(52), /* NAME_PRIKM_UKR_MN */
--    name_rus_male    VARCHAR(52), /* NAME_PRIKM_RUS_M */
--    name_rus_fem     VARCHAR(52), /* NAME_PRIKM_RUS_W */
--    name_rus_single  VARCHAR(52), /* NAME_PRIKM_RUS_O */
--    name_rus_mult    VARCHAR(52), /* NAME_PRIKM_RUS_MN */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE, /* DT */
--    display_name     AS (name_ukr_male)
--);

--ALTER TABLE dict_org_title_form ADD PRIMARY KEY (id);

/* dict_org_form_1nf */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_form_1nf]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_form_1nf]

--CREATE TABLE dict_org_form_1nf /* S_PRIZNAK_1 */ (
--    id               INTEGER NOT NULL, /* KOD_PRIZNAK_1 */
--    name             VARCHAR(32), /* NAME_PRIZNAK_1 */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE /* DT */
--);

--ALTER TABLE dict_org_form_1nf ADD PRIMARY KEY (id);

/* dict_org_vedomstvo */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_vedomstvo]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_vedomstvo]

--CREATE TABLE dict_org_vedomstvo /* S_VIDOM_NAL */ (
--    id               INTEGER NOT NULL, /* KOD_VIDOM_NAL */
--    name             VARCHAR(255), /* NAME_VIDOM_NAL */
--    full_name        VARCHAR(255), /* FULL_NAME */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE /* DT */
--);

--ALTER TABLE dict_org_vedomstvo ADD PRIMARY KEY (id);

/* dict_org_title */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_title]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_title]

--CREATE TABLE dict_org_title /* S_IMEN */ (
--    id               INTEGER NOT NULL, /* KOD_IMEN */
--    name_rus         VARCHAR(40), /* NAME_IMEN_RUS */
--    name_ukr         VARCHAR(40), /* NAME_IMEN_UKR */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE, /* DT */
--    display_name     AS (name_ukr)
--);

--ALTER TABLE dict_org_title ADD PRIMARY KEY (id);

/* dict_org_form */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_form]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_form]

--CREATE TABLE dict_org_form /* S_ORG_FORM */ (
--    id               INTEGER NOT NULL, /* KOD_ORG_FORM */
--    name             VARCHAR(160) NOT NULL, /* NAME_ORG_FORM */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE, /* DT */
--    stat_code        INTEGER /* KOD_ORG_FORM_STAT */
--);

--ALTER TABLE dict_org_form ADD PRIMARY KEY (id);

/* dict_org_gosp_struct_type */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_gosp_struct_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_gosp_struct_type]

--CREATE TABLE dict_org_gosp_struct_type /* S_VID_GOSP_STR */ (
--    id               INTEGER NOT NULL, /* KOD_VID_GOSP_STR */
--    name             VARCHAR(30), /* NAME_VID_GOSP_STR */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE /* DT */
--);

--ALTER TABLE dict_org_gosp_struct_type ADD PRIMARY KEY (id);

/* dict_org_share_type */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_share_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_share_type]

--CREATE TABLE dict_org_share_type /* S_AKCIA */ (
--    id               INTEGER NOT NULL, /* KOD_AKCIA */
--    name             VARCHAR(24), /* NAME_AKCIA */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE /* DT */
--);

--ALTER TABLE dict_org_share_type ADD PRIMARY KEY (id);

/* dict_otdel_gukv */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_otdel_gukv]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_otdel_gukv]

--CREATE TABLE dict_otdel_gukv /* S_VIDDIL */ (
--    id               INTEGER NOT NULL, /* KOD */
--    name             VARCHAR(60), /* NAME */
--    display_name     VARCHAR(100), /* NAME_OUTPUT */
--    short_name       VARCHAR(20) /* SHORT_NAME_OUTPUT */
--);

--ALTER TABLE dict_otdel_gukv ADD PRIMARY KEY (id);

/*
   Organization dictionaries from the 'Balans' database
*/

/* dict_org_mayno */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_mayno]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_mayno]

--CREATE TABLE dict_org_mayno /* S_MAYNO */ (
--    id                 INTEGER NOT NULL, /* KOD_MAYNO */
--    name               VARCHAR(50), /* MAYNO */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_org_mayno ADD PRIMARY KEY (id);

/* dict_org_contact_posada */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_contact_posada]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_contact_posada]

--CREATE TABLE dict_org_contact_posada /* S_POSADA */ (
--    id                 INTEGER NOT NULL, /* KOD_POSADA */
--    name               VARCHAR(100), /* POSADA */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_org_contact_posada ADD PRIMARY KEY (id);

/* dict_org_nadhodjennya */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_nadhodjennya]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_nadhodjennya]

--CREATE TABLE dict_org_nadhodjennya /* S_PRIZ_NADH */ (
--    id                 INTEGER NOT NULL, /* KOD_PRIZ_NADH */
--    name               VARCHAR(100), /* NAME_PRIZ_NAMDH */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_org_nadhodjennya ADD PRIMARY KEY (id);

/* dict_org_vibuttya */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_vibuttya]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_vibuttya]

--CREATE TABLE dict_org_vibuttya /* S_PRIZ_VIB */ (
--    id                 INTEGER NOT NULL, /* KOD_PRIZ_VIB */
--    name               VARCHAR(100), /* NAME_PRIZ_VIB */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_org_vibuttya ADD PRIMARY KEY (id);

/* dict_org_privat_status */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_privat_status]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_privat_status]

--CREATE TABLE dict_org_privat_status /* S_PRIZ_PR_KOM */ (
--    id                 INTEGER NOT NULL, /* KOD_PRIZ_PR_KOM */
--    name               VARCHAR(20), /* NAME_PRIZ_PR_KOM */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_org_privat_status ADD PRIMARY KEY (id);

/* dict_org_cur_state */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_cur_state]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_cur_state]

--CREATE TABLE dict_org_cur_state /* S_POTOCH_STAN */ (
--    id                 INTEGER NOT NULL, /* KOD_POTOCH_STAN */
--    name               VARCHAR(30), /* NAME_POTOCH_STAN */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_org_cur_state ADD PRIMARY KEY (id);

/* dict_org_sfera_upr */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_sfera_upr]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_sfera_upr]

--CREATE TABLE dict_org_sfera_upr /* S_SFERA_UPR */ (
--    id                 INTEGER NOT NULL, /* KOD_SFERA_UPR */
--    name               VARCHAR(200), /* NAME_SFERA_UPR */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_org_sfera_upr ADD PRIMARY KEY (id);

/* dict_org_plan_zone */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_plan_zone]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_plan_zone]

--CREATE TABLE dict_org_plan_zone /* S_EK_PLAN_ZONA */ (
--    id                 INTEGER NOT NULL, /* KOD_EK_PLAN_ZONA */
--    name               VARCHAR(20), /* NAME_EK_PLAN_ZONA */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_org_plan_zone ADD PRIMARY KEY (id);

/* dict_org_registr_org */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_registr_org]') AND type in (N'U'))
--DROP TABLE [dbo].dict_org_registr_org

--CREATE TABLE dict_org_registr_org /* S_REESORG */ (
--    id                 INTEGER NOT NULL, /* KOD_REESORG */
--    name               VARCHAR(100), /* REESORG */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--)

--ALTER TABLE dict_org_registr_org ADD PRIMARY KEY (id);

/* dict_org_old_industry */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_old_industry]') AND type in (N'U'))
--DROP TABLE [dbo].dict_org_old_industry

--CREATE TABLE dict_org_old_industry /* S_OLD_GALUZ */ (
--	id                 INTEGER NOT NULL, /* KOD_OLD_GALUZ */
--    name               VARCHAR(200), /* NAME_OLD_GALUZ */
--    modified_by        VARCHAR(128),
--    modify_date        DATE,
    
--    /* A special coefficient that allows to calculate budget payments for this industry */
--    budg_payments_rate NUMERIC(5,2) /* Deprecated; financial coefficients are imported into 'fin_budget_rate_by_industry' table */
--);

--ALTER TABLE dict_org_old_industry ADD PRIMARY KEY (id);

/* dict_org_old_occupation */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_old_occupation]') AND type in (N'U'))
--DROP TABLE [dbo].dict_org_old_occupation

--CREATE TABLE dict_org_old_occupation /* S_OLD_VID_DIAL */ (
--	id                 INTEGER NOT NULL, /* KOD_OLD_VID_DIAL */
--    name               VARCHAR(200), /* NAME_OLD_VID_DIAL */
--    old_industry_id    INTEGER, /* KOD_OLD_GALUZ */
--    modified_by        VARCHAR(128),
--    modify_date        DATE
--);

--ALTER TABLE dict_org_old_occupation ADD PRIMARY KEY (id);

--ALTER TABLE dict_org_old_occupation ADD CONSTRAINT fk_dict_org_old_occupation_old_industry FOREIGN KEY (old_industry_id) REFERENCES dict_org_old_industry (id);

/* dict_org_old_organ */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_old_organ]') AND type in (N'U'))
--DROP TABLE [dbo].dict_org_old_organ

--CREATE TABLE dict_org_old_organ /* S_OLD_ORGAN */ (
--	id                 INTEGER NOT NULL, /* KOD_OLD_ORGAN */
--    name               VARCHAR(200), /* NAME_OLD_ORGAN */
--    old_industry_id    INTEGER, /* KOD_OLD_GALUZ */
--    modified_by        VARCHAR(128),
--    modify_date        DATE
--);

--ALTER TABLE dict_org_old_organ ADD PRIMARY KEY (id);

--ALTER TABLE dict_org_old_organ ADD CONSTRAINT fk_dict_org_old_organ_old_industry FOREIGN KEY (old_industry_id) REFERENCES dict_org_old_industry (id);

/* dict_form_vlasn_vibuttya */
--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_form_vlasn_vibuttya]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_form_vlasn_vibuttya]

--CREATE TABLE [dict_form_vlasn_vibuttya] /* S_VLASN_VIB */ (
--	id					INTEGER NOT NULL, /* KOD_VLASN_VIB */
--    name				VARCHAR(32), /* NAME_VLASN_VIB */
--);

--ALTER TABLE [dict_form_vlasn_vibuttya] ADD PRIMARY KEY (id);

/* organizations */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[organizations]') AND type in (N'U'))
--DROP TABLE [dbo].[organizations]

--CREATE TABLE organizations /* SORG_1NF */ (
--    id                    INTEGER NOT NULL, /* KOD_OBJ */
--    master_org_id         INTEGER, /* If this ID is not NULL, it means that this organization is a duplicate of the 'master' organization */
--    last_state            INTEGER, /* LAST_SOST */
--    occupation_id         INTEGER, /* KOD_VID_DIAL - TABLE  S_VID_DIAL */
--    status_id             INTEGER, /* KOD_STATUS - TABLE  S_STATUS */
--    form_gosp_id          INTEGER, /* KOD_FORM_GOSP - TABLE  S_FORM_GOSP */
--    form_ownership_id     INTEGER, /* KOD_FORM_VLASN - TABLE  S_FORM_VLASN */
--    gosp_struct_id        INTEGER, /* KOD_GOSP_STRUKT - TABLE  S_GOSP_STRUKT */
--    organ_id              INTEGER, /* KOD_ORGAN - TABLE  S_ORGAN */
--    industry_id           INTEGER, /* KOD_GALUZ - TABLE  S_GALUZ */
--    nomer_obj             INTEGER, /* NOMER_OBJ */
--    zkpo_code             VARCHAR(16), /* KOD_ZKPO */
--    addr_distr_old_id     INTEGER, /* KOD_RAYON - TABLE  S_RAYON */
--    addr_distr_new_id     INTEGER, /* KOD_RAYON2 - TABLE  S_RAYON2 */
--    addr_street_name      VARCHAR(100), /* NAME_UL */
--    addr_street_id        INTEGER,
--    addr_nomer            VARCHAR(30), /* NOMER_DOMA */
--    addr_nomer2           VARCHAR(24), /* NOMER_DOMA2 */
--    addr_korpus           VARCHAR(20), /* NOMER_KORPUS */
--    addr_zip_code         VARCHAR(24), /* POST_INDEX */
--    addr_misc             VARCHAR(150), /* ADDITIONAL_ADRESS */
--    director_fio          VARCHAR(70), /* FIO_BOSS */
--    director_phone        VARCHAR(24), /* TEL_BOSS */
--    director_fio_kogo     VARCHAR(70), /* FIO_BOSS_R_VIDM */
--    director_title        VARCHAR(70), /* POST_BOSS */
--    director_title_kogo   VARCHAR(70), /* POST_BOSS_R_VIDM */
--    director_doc          VARCHAR(100), /* DOC_BOSS */
--    director_doc_kogo     VARCHAR(100), /* DOC_BOSS_R_VIDM */
--    director_email        VARCHAR(100),
--    buhgalter_fio         VARCHAR(70), /* FIO_BUH */
--    buhgalter_phone       VARCHAR(24), /* TEL_BUH */
--    buhgalter_email       VARCHAR(100),
--    num_buildings         INTEGER, /* KOLVO_BUD */
--    full_name             VARCHAR(255), /* FULL_NAME_OBJ */
--    short_name            VARCHAR(255), /* SHORT_NAME_OBJ */
--    priznak_id            INTEGER, /* KOD_ADDITION_PRIZNAK - TABLE  S_ADDITION_PRIZNAK */
--    title_form_id         INTEGER, /* KOD_PRIKMET - TABLE  S_PRIKMET */
--    form_1nf_id           INTEGER, /* KOD_PRIZNAK_1 - TABLE  S_PRIZNAK_1 */
--    vedomstvo_id          INTEGER, /* KOD_VIDOM_NAL - TABLE  S_VIDOM_NAL */
--    title_id              INTEGER, /* KOD_IMEN - TABLE  S_IMEN */
--    form_id               INTEGER, /* KOD_ORG_FORM - TABLE  S_ORG_FORM */
--    gosp_struct_type_id   INTEGER, /* KOD_VID_GOSP_STR - TABLE  S_VID_GOSP_STR */
--    search_name           VARCHAR(100), /* FIND_NAME_OBJ */
--    name_komu             VARCHAR(150), /* NAME_DAVAL_VIDM */
--    fax                   VARCHAR(24), /* TELEFAX */
--    registration_auth     VARCHAR(100), /* REESORG */
--    registration_num      VARCHAR(24), /* REESNO */
--    registration_date     DATE, /* REESDT */
--    registration_svidot   VARCHAR(10), /* NO_SVIDOT */
--    l_year                INTEGER, /* LYEAR */
--    date_l_year           DATE, /* DTLYEAR */
--    sqr_on_balance        NUMERIC(15,2), /* PL_BALANS */
--    sqr_manufact          NUMERIC(15,2), /* PL_VIROB_PRIZN */
--    sqr_non_manufact      NUMERIC(15,2), /* PL_NOT_VIROB_PRIZN */
--    sqr_free_for_rent     NUMERIC(15,2), /* PL_ARENDA_VILNA */
--    sqr_total             NUMERIC(15,2), /* PL_ZAGAL */
--    sqr_rented            NUMERIC(15,2), /* PL_ARENDUETSYA */
--    sqr_privat            NUMERIC(15,2), /* PL_PRIVAT */
--    sqr_given_for_rent    NUMERIC(15,2), /* PL_NADANA_V_A */
--    sqr_znyata_z_balansu  NUMERIC(15,2), /* PL_ZNYATA_Z_BALANSU */
--    sqr_prodaj            NUMERIC(15,2), /* PL_PRODAJ */
--    sqr_spisani_zneseni   NUMERIC(15,2), /* PL_SPISANI_ZNESENI */
--    sqr_peredana          NUMERIC(15,2), /* PL_PEREDANA_INSHUM */
--    num_objects           INTEGER, /* KOLVO_OBJ */
--    kved_code             VARCHAR(8), /* KOD_KVED */
--    date_stat_spravka     DATE, /* DT_STAT_DOVIDKA */
--    koatuu                VARCHAR(16), /* KOATUU */
--    modified_by           VARCHAR(128), /* ISP */
--    modify_date           DATETIME, /* DT */
--    share_type_id         INTEGER, /* KOD_VID_AKCIA - TABLE  S_AKCIA */
--    share                 VARCHAR(50), /* PERSENT_AKCIA */
--    bank_name             VARCHAR(16), /* NAME_BANK */
--    bank_mfo              VARCHAR(200), /* MFO */
--    is_deleted            INTEGER DEFAULT 0, /* DELETED */
--    del_date              DATE, /* DTDELETE */
--    otdel_gukv_id         INTEGER, /* VIDDIL - TABLE  S_VIDDIL */
--    arch_id               INTEGER, /* ARCH_KOD */
--    arch_flag             INTEGER DEFAULT 0, /* IN_ARCH */
--    is_liquidated         INTEGER default 0, /* LIKVID */
--    liquidation_date      INTEGER, /* LIKVID_DATE */
--    pidp_rda              INTEGER default 0, /* PIDP_RDA */
--    is_arend              INTEGER, /* IS_AREND */
--    beg_state_date        DATE, /* DT_BEG_STAN */
--    end_state_date        DATE, /* DT_END_STAN */
    
--    /* The following fields are added from the Balans database (SORG table) */
    
--    mayno_id              INTEGER, /* KOD_MAYNO */
--    contact_email         VARCHAR(50), /* EMAIL */
--    contact_posada_id     INTEGER, /* KOD_POSADA */
--    nadhodjennya_id       INTEGER, /* KOD_PRIZ_NADH */
--    vibuttya_id           INTEGER, /* KOD_PRIZ_VIB */
--    privat_status_id      INTEGER, /* KOD_PRIZ_PR_KOM */
--    cur_state_id          INTEGER, /* KOD_POTOCH_STAN */
--    sfera_upr_id          INTEGER, /* KOD_SFERA_UPR */
--    plan_zone_id          INTEGER, /* KOD_EK_PLAN_ZONA */
--    registr_org_id        INTEGER, /* KOD_REESORG */
--    nadhodjennya_date     DATE, /* DATE_NADHOD */
--    vibuttya_date         DATE, /* DATE_VIBUT */
--    chastka               NUMERIC(15,4), /* CHASTKA */
--    registration_rish     VARCHAR(10), /* REES_RISH */
--    registration_dov_date DATE, /* REES_DOV */
--    registration_corp     VARCHAR(10), /* REESNO_CORP */        
--    /* full_name_detailed    VARCHAR(MAX), FULL_NAME_OBJBLOB - do we really need a THIRD name for organization? */
--    strok_start_date      DATE, /* BEGIN_STROK */
--    strok_end_date        DATE, /* END_STROK */
--    stat_fond             NUMERIC(15,2), /* STAT_FOND */
--    size_plus             VARCHAR(10), /* SIZE_PLUS */
--    addr_zip_code_3       INTEGER, /* POST_INDEX3 */

--    old_industry_id       INTEGER, /* KOD_OLD_GALUZ */
--    old_occupation_id     INTEGER, /* KOD_OLD_VID_DIAL */
--    old_organ_id          INTEGER, /* KOD_OLD_ORGAN */
--    form_vlasn_vibuttya_id	INTEGER, /* KOD_VLASN_VIB */

--    /* The following fields are added from the Privatization database (SZKPO table) */

--    addr_city             VARCHAR(100), /* CITY */
--    addr_flat_num         INTEGER, /* KVARTIRA_FIZ */
--    povnovajennia         VARCHAR(100), /* POVNOV */
--    povnov_osoba_fio      VARCHAR(50), /* FIO_OSOBA */
--    povnov_passp_seria    VARCHAR(15), /* PASP_SER */
--    povnov_passp_num      VARCHAR(15), /* PASP_NOM */
--    povnov_passp_auth     VARCHAR(160), /* PASP_VID */
--    povnov_passp_date     DATE, /* PASP_DT */
--    director_passp_seria  VARCHAR(15), /* PASP_SER_FIZ */
--    director_passp_num    VARCHAR(15), /* PASP_NOM_FIZ */
--    director_passp_auth   VARCHAR(160), /* PASP_VID_FIZ */
--    director_passp_date   DATE, /* PASP_DT_FIZ */
--    registration_svid_date DATE, /* SVIDDT */
    
--    /* The Database that this organization was imported from */
--    origin_db             INTEGER DEFAULT 1, /* 1 - 1NF, 2 - Balans, 3 - NJF, 4 - Privatizaciya */

--    /* A special coefficient that allows to calculate budget payments for this industry */    
--    budg_payments_rate    NUMERIC(5,2), /* Deprecated; financial coefficients are imported into 'fin_budget_rate_by_industry' table */
--    is_under_closing      INTEGER, /* KOD_VID_PREDPR == 15 */
    
--    /* Physical address */
--    phys_addr_street_id   INTEGER,
--    phys_addr_district_id INTEGER,
--    phys_addr_nomer       VARCHAR(60),
--    phys_addr_zip_code    VARCHAR(24),
--    phys_addr_misc        VARCHAR(150),
--    contribution_rate	  INTEGER,
    
--    /* Rent payments (totals for the organization): moved here from the 'arenda_payments' table */
--    budget_narah_50_uah     NUMERIC(15,3), /* Нарахована сума до бюджету 50% від загальної суми надходжень орендної плати за звітний період, грн. (без ПДВ) */
--	budget_zvit_50_uah      NUMERIC(15,3), /* Перераховано до бюджету 50% за звітний період всього з 1 січня поточного року, грн. (без ПДВ) */
--	budget_prev_50_uah      NUMERIC(15,3), /* - в тому числі перераховано до бюджету 50% боргів у звітному періоді з 1 січня поточного року за попередні роки, грн. (без ПДВ) */
--	budget_debt_30_50_uah   NUMERIC(15,3), /* Заборгованість зі сплати 50% до бюджету від оренди майна минулих років, грн. (без ПДВ) */
--	is_special_organization INTEGER, /* 'Київенерго' та 'Водоканал' */
--	payment_budget_special  NUMERIC(15,3), /* Перераховано до бюджету за користування комунальним майном, грн (без ПДВ) ("Київенерго" та "Водоканал") */
	
--	/* Additional payments */
--	konkurs_payments        NUMERIC(15,3), /* Авансові платежі за участь у конкурсі */
--	unknown_payments        NUMERIC(15,3), /* Облік нез'ясованих сум */
--	unknown_payment_note    VARCHAR(510) /* Коментар щодо нез'ясованих сум */
--);

--ALTER TABLE organizations ADD PRIMARY KEY (id);

--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_occupation FOREIGN KEY (occupation_id) REFERENCES dict_org_occupation (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_status FOREIGN KEY (status_id) REFERENCES dict_org_status (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_form_gosp FOREIGN KEY (form_gosp_id) REFERENCES dict_org_form_gosp (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_ownership FOREIGN KEY (form_ownership_id) REFERENCES dict_org_ownership (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_gosp_struct FOREIGN KEY (gosp_struct_id) REFERENCES dict_org_gosp_struct (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_organ FOREIGN KEY (organ_id) REFERENCES dict_org_organ (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_industry FOREIGN KEY (industry_id) REFERENCES dict_org_industry (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_distr_old FOREIGN KEY (addr_distr_old_id) REFERENCES dict_districts (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_distr_new FOREIGN KEY (addr_distr_new_id) REFERENCES dict_districts2 (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_priznak FOREIGN KEY (priznak_id) REFERENCES dict_org_priznak (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_title_form FOREIGN KEY (title_form_id) REFERENCES dict_org_title_form (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_form_1nf FOREIGN KEY (form_1nf_id) REFERENCES dict_org_form_1nf (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_vedomstvo FOREIGN KEY (vedomstvo_id) REFERENCES dict_org_vedomstvo (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_title FOREIGN KEY (title_id) REFERENCES dict_org_title (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_form FOREIGN KEY (form_id) REFERENCES dict_org_form (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_gosp_struct_type FOREIGN KEY (gosp_struct_type_id) REFERENCES dict_org_gosp_struct_type (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_share_type FOREIGN KEY (share_type_id) REFERENCES dict_org_share_type (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_otdel FOREIGN KEY (otdel_gukv_id) REFERENCES dict_otdel_gukv (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_old_industry FOREIGN KEY (old_industry_id) REFERENCES dict_org_old_industry (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_old_occupation FOREIGN KEY (old_occupation_id) REFERENCES dict_org_old_occupation (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_old_organ FOREIGN KEY (old_organ_id) REFERENCES dict_org_old_organ (id);

/* New Organizations foreign keys from the Balans database */

--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_mayno FOREIGN KEY (mayno_id) REFERENCES dict_org_mayno (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_posada FOREIGN KEY (contact_posada_id) REFERENCES dict_org_contact_posada (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_vibuttya FOREIGN KEY (vibuttya_id) REFERENCES dict_org_vibuttya (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_nadhodzhennya FOREIGN KEY (nadhodjennya_id) REFERENCES dict_org_nadhodjennya (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_cur_state FOREIGN KEY (cur_state_id) REFERENCES dict_org_cur_state (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_sfera_upr FOREIGN KEY (sfera_upr_id) REFERENCES dict_org_sfera_upr (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_plan_zone FOREIGN KEY (plan_zone_id) REFERENCES dict_org_plan_zone (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_privat_status FOREIGN KEY (privat_status_id) REFERENCES dict_org_privat_status (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_registr_org FOREIGN KEY (registr_org_id) REFERENCES dict_org_registr_org (id);
--ALTER TABLE organizations ADD CONSTRAINT fk_organizations_form_vlasn_vibuttya FOREIGN KEY (form_vlasn_vibuttya_id) REFERENCES dict_form_vlasn_vibuttya (id);
--GO

/******************************************************************************/
/*                   "Balans" and the required dictionaries                   */
/******************************************************************************/

/* dict_balans_o26 */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_balans_o26]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_balans_o26]

--CREATE TABLE dict_balans_o26 /* SO26 */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              VARCHAR(60), /* NAME */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE /* DT */
--);

--ALTER TABLE dict_balans_o26 ADD PRIMARY KEY (id);

/* dict_balans_bti */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_balans_bti]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_balans_bti]

--CREATE TABLE dict_balans_bti /* SOBTI */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              VARCHAR(60), /* NAME */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE /* DT */
--);

--ALTER TABLE dict_balans_bti ADD PRIMARY KEY (id);

/* dict_balans_purpose_group */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_balans_purpose_group]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_balans_purpose_group]

--CREATE TABLE dict_balans_purpose_group /* SGRPURPOSE */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              VARCHAR(160), /* NAME */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE /* DT */
--);

--ALTER TABLE dict_balans_purpose_group ADD PRIMARY KEY (id);

/* dict_balans_purpose */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_balans_purpose]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_balans_purpose]

--CREATE TABLE dict_balans_purpose /* SPURPOSE */ (
--    id                INTEGER NOT NULL, /* KOD */
--    group_code        INTEGER, /* GR */
--    name              VARCHAR(160), /* NAME */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE, /* DT */
--    display_name      AS (STR(group_code) + '-' + name)
--);

--ALTER TABLE dict_balans_purpose ADD PRIMARY KEY (id);

/* dict_balans_ownership_type */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_balans_ownership_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_balans_ownership_type]

--CREATE TABLE dict_balans_ownership_type /* SPRAVO */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              VARCHAR(50), /* NAME */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE /* DT */
--);

--ALTER TABLE dict_balans_ownership_type ADD PRIMARY KEY (id);

/* dict_balans_form_giver */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_balans_form_giver]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_balans_form_giver]

--CREATE TABLE dict_balans_form_giver /* S_FORM_GIV */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              VARCHAR(60), /* NAME */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE /* DT */
--);

--ALTER TABLE dict_balans_form_giver ADD PRIMARY KEY (id);

/* dict_balans_update_src */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_balans_update_src]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_balans_update_src]

--CREATE TABLE dict_balans_update_src /* S_UPD_SOURCE */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              VARCHAR(60), /* NAME */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE /* DT */
--);

--ALTER TABLE dict_balans_update_src ADD PRIMARY KEY (id);

/* dict_balans_vidch_type */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_balans_vidch_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_balans_vidch_type]

--CREATE TABLE dict_balans_vidch_type /* S_VIDCH_TYPE */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              VARCHAR(60), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_balans_vidch_type ADD PRIMARY KEY (id);

/* dict_balans_obj_status - Не импортируется из 1НФ */

IF OBJECT_ID('dict_balans_obj_status') IS NULL
BEGIN
	CREATE TABLE dict_balans_obj_status (
		id       INTEGER PRIMARY KEY,
		name     VARCHAR(64)
	);
	
	INSERT INTO dict_balans_obj_status (id, name) VALUES (0, 'ПРИМІЩЕННЯ ВІЛЬНЕ')

	INSERT INTO dict_balans_obj_status (id, name) VALUES (1, 'ПРИМІЩЕННЯ ЗАЙНЯТЕ ПО ДОГОВОРУ ОРЕНДИ')

	INSERT INTO dict_balans_obj_status (id, name) VALUES (2, 'ПРИМІЩЕННЯ ЗАЙНЯТЕ З ІНШИХ ПРИЧИН')

END

/* balans */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[balans]') AND type in (N'U'))
--DROP TABLE [dbo].[balans]

--CREATE TABLE balans /* BALANS_1NF */ (
--    id                INTEGER NOT NULL, /* ID */
--    year_balans       INTEGER, /* LYEAR */
--    building_id       INTEGER, /* OBJECT */
--    organization_id   INTEGER, /* ORG */
--    sqr_total         NUMERIC(9,2), /* SQR_ZAG */
--    sqr_pidval        NUMERIC(9,2), /* SQR_PIDV */
--    sqr_vlas_potreb   NUMERIC(9,2), /* SQR_VLAS */
--    sqr_free          NUMERIC(9,2), /* SQR_FREE */
--    sqr_in_rent       NUMERIC(9,2), /* V_SQR_ORENDA */
--    sqr_privatizov    NUMERIC(9,2), /* SQR_PRIV */
--    sqr_not_for_rent  NUMERIC(9,2), /* SQR_VLASN_N */
--    sqr_gurtoj        NUMERIC(9,2), /* SQR_GURTOJ */
--    sqr_non_habit     NUMERIC(9,2), /* SQR_NEJ */
--    sqr_kor           NUMERIC(9,2), /* SQR_KOR */
--    cost_balans       NUMERIC(15,3), /* BALANS_VARTIST */
--    cost_fair         NUMERIC(15,3), /* SPRAV_VARTIST */
--    cost_expert_1m    NUMERIC(15,3), /* VARTIST_EXP */
--    cost_expert_total NUMERIC(15,3), /* VARTIST_EXP_V */
--    cost_rent_narah   NUMERIC(15,3), /* OREND_NARAH */
--    cost_rent_payed   NUMERIC(15,3), /* OREND_SPLACH */
--    cost_debt         NUMERIC(15,3), /* BORG */
--    cost_zalishkova   NUMERIC(15,3), /* ZAL_VART */
--    cost_rinkova      NUMERIC(15,3), /* VARTIST_RINK */
--    cost_fair_1m      NUMERIC(15,3), /* SPR_VART_1KVM */
--    num_people        INTEGER, /* PPL_NUM */
--    num_rent_agr      INTEGER, /* V_DOG_ORENDA */
--    num_privat_apt    INTEGER, /* PRIV_COUNT */
--    o26_id            INTEGER, /* O26 - TABLE  SO26 */
--    bti_id            INTEGER, /* OBTI */
--    approval_by       VARCHAR(100), /* RISHEN */
--    approval_num      VARCHAR(18), /* RNOMER */
--    approval_date     DATE, /* RDATA */
--    form_ownership_id INTEGER, /* FORM_VLASN - TABLE S_FORM_VLASN */ /* DUPLICATION */
--    object_kind_id    INTEGER, /* KINDOBJ - TABLE SKINDOBJ */ /* DUPLICATION */
--    object_type_id    INTEGER, /* TYPEOBJ - TABLE STYPEOBJ */ /* DUPLICATION */
--    history_id        INTEGER, /* HISTORY - TABLE SHISTORY */ /* DUPLICATION */
--    tech_condition_id INTEGER, /* TEXSTAN - TABLE STEXSTAN */ /* DUPLICATION */
--    purpose_group_id  INTEGER, /* GRPURP - TABLE SGRPURPOSE */
--    purpose_id        INTEGER, /* PURPOSE - TABLE SPURPOSE */
--    purpose_str       VARCHAR(255), /* PURP_STR */
--    floors            VARCHAR(100), /* FLOATS */
--    priznak_1nf       INTEGER, /* PRIZNAK1NF */
--    ownership_type_id INTEGER, /* PRAVO - TABLE SPRAVO */
--    obj_street_name   VARCHAR(100), /* OBJ_ULNAME */ /* DUPLICATION */
--    obj_street_id     INTEGER, /* OBJ_KODUL */ /* DUPLICATION */
--    obj_street_name2  VARCHAR(100), /* ULNAME2 */ /* DUPLICATION */
--    obj_street_id2    INTEGER, /* ULKOD2 */ /* DUPLICATION */
--    obj_nomer1        VARCHAR(10), /* OBJ_NOMER1 */ /* DUPLICATION */
--    obj_nomer2        VARCHAR(18), /* OBJ_NOMER2 */ /* DUPLICATION */
--    obj_nomer3        VARCHAR(10), /* OBJ_NOMER3 */ /* DUPLICATION */
--    obj_nomer         AS (LTRIM(RTRIM(obj_nomer1)) + ' ' + LTRIM(RTRIM(obj_nomer2))+ ' ' + LTRIM(RTRIM(obj_nomer3))), /* DUPLICATION */
--    obj_bti_code      VARCHAR(18), /* OBJ_KODBTI */ /* DUPLICATION */
--    obj_addr_misc     VARCHAR(100), /* OBJ_ADRDOP */ /* DUPLICATION */
--    obj_street_misc   VARCHAR(255), /* OBJ_ULADRDOP */ /* DUPLICATION */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATETIME, /* DT */
--    form_giver_id     INTEGER, /* FORM_GIV */
--    org_maintain_id   INTEGER, /* ORG_EXPL */
--    update_src_id     INTEGER, /* UPD_SOURCE - TABLE S_UPD_SOURCE */
--    is_deleted        INTEGER, /* DELETED */
--    del_date          DATE, /* DTDELETE */
--    znos              NUMERIC(15,3), /* ZNOS */
--    znos_date         DATE, /* ZNOS_DATE */
--    date_expert       DATE, /* DTEXP */
--    reestr_no         VARCHAR(18), /* REESNO */
--    fair_cost_date    DATE, /* DTSPRAV */
--    otdel_gukv_id     INTEGER, /* VIDDIL - TABLE  S_VIDDIL */
--    date_bti          DATE, /* DATE_BTI */
--    arch_id           INTEGER, /* ARCH_KOD */
--    arch_flag         INTEGER DEFAULT 0, /* IN_ARCH */
--    date_cost_rinkova DATE, /* RINK_VART_DT */
--    memo              VARCHAR(MAX), /* MEMO */
--    note              VARCHAR(MAX), /* PRYMITKA */
--    /*doc_list        VARCHAR(MAX), */ /* All documents related to this Balans entry are listed in this field */
--    /* Free square properties */
--    is_free_sqr       INTEGER,
--    free_sqr_useful   NUMERIC(9,2),
--    free_sqr_condition_id INTEGER,
--    free_sqr_location VARCHAR(100),
--    /* Документ, подтверждающий право собственности на объект */
--    ownership_doc_type INTEGER,
--    ownership_doc_num VARCHAR(24),
--    ownership_doc_date DATETIME,
--    /* Документ, подтверждающий передачу объекта на баланс */
--    balans_doc_type   INTEGER,
--    balans_doc_num    VARCHAR(24),
--    balans_doc_date   DATETIME,
--    /* Документ, подтверждающий отчуждение объекта (продажа, снос, и т.д.) */
--    vidch_doc_type    INTEGER,
--    vidch_doc_num     VARCHAR(24),
--    vidch_doc_date    DATETIME,
--    vidch_type_id     INTEGER, /* Способ отчуждения - dict_balans_vidch_type */
--    vidch_org_id      INTEGER, /* Организация, которой передан объект */
--    vidch_cost        NUMERIC(15,3), /* Стоимость отчуждения */
--    /**/
--    sqr_engineering   NUMERIC(9,2), /* Площа технiко-iнженерних потреб (не хранится в 1НФ) */
--    obj_status_id     INTEGER
--);

--ALTER TABLE balans ADD PRIMARY KEY (id);

--ALTER TABLE balans ADD CONSTRAINT fk_balans_org FOREIGN KEY (organization_id) REFERENCES organizations (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_object FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_o26 FOREIGN KEY (o26_id) REFERENCES dict_balans_o26 (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_bti FOREIGN KEY (bti_id) REFERENCES dict_balans_bti (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_form_ownership FOREIGN KEY (form_ownership_id) REFERENCES dict_org_ownership (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_object_kind FOREIGN KEY (object_kind_id) REFERENCES dict_object_kind (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_history FOREIGN KEY (history_id) REFERENCES dict_history (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_tech_condition FOREIGN KEY (tech_condition_id) REFERENCES dict_tech_state (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_object_type FOREIGN KEY (object_type_id) REFERENCES dict_object_type (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_purpose_group FOREIGN KEY (purpose_group_id) REFERENCES dict_balans_purpose_group (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_purpose FOREIGN KEY (purpose_id) REFERENCES dict_balans_purpose (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_ownership_type FOREIGN KEY (ownership_type_id) REFERENCES dict_balans_ownership_type (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_form_giver FOREIGN KEY (form_giver_id) REFERENCES dict_balans_form_giver (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_org_maintain_id FOREIGN KEY (org_maintain_id) REFERENCES organizations (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_update_src FOREIGN KEY (update_src_id) REFERENCES dict_balans_update_src (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_otdel FOREIGN KEY (otdel_gukv_id) REFERENCES dict_otdel_gukv (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_vidch_type FOREIGN KEY (vidch_type_id) REFERENCES dict_balans_vidch_type (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_vidch_org FOREIGN KEY (vidch_org_id) REFERENCES organizations (id);
--ALTER TABLE balans ADD CONSTRAINT fk_balans_obj_status FOREIGN KEY (obj_status_id) REFERENCES dict_balans_obj_status (id);

--GO

/******************************************************************************/
/*                   'Arenda' and the required dictionaries                   */
/******************************************************************************/

/* dict_arenda_agreement_kind */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_arenda_agreement_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_arenda_agreement_kind]

--CREATE TABLE dict_arenda_agreement_kind /* SKINDDOG */ (
--    id                  INTEGER NOT NULL, /* KOD */
--    name                VARCHAR(64) DEFAULT ' ', /* NAME */
--    modified_by         VARCHAR(128), /* ISP */
--    modify_date         DATE /* DT */
--);

--ALTER TABLE dict_arenda_agreement_kind ADD PRIMARY KEY (id);

/* dict_arenda_privat_kind */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_arenda_privat_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_arenda_privat_kind]

--CREATE TABLE dict_arenda_privat_kind /* S_WAYPRIVAT */ (
--    id                  INTEGER NOT NULL, /* KOD */
--    name                VARCHAR(64) DEFAULT ' ' /* NAME */
--);

--ALTER TABLE dict_arenda_privat_kind ADD PRIMARY KEY (id);

/* dict_arenda_payment_type */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_arenda_payment_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_arenda_payment_type]

--CREATE TABLE dict_arenda_payment_type /* S_VIDPLATA*/  (
--    id                  INTEGER NOT NULL, /* KOD */
--    name                VARCHAR(64) DEFAULT ' ' /* NAME */
--);

--ALTER TABLE dict_arenda_payment_type ADD PRIMARY KEY (id);

/* dict_rent_period */

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_rent_period]') AND type in (N'U'))
BEGIN

	CREATE TABLE dict_rent_period /* ZAV_S_PERIOD_OR_PLATA */ (
		id                      INTEGER NOT NULL, /* S_PERIOD_OR_PLATA_ID */
		name                    VARCHAR(255), /* NAME */
		period_start            DATE, /* DT_START */
		period_end              DATE, /* DT_END */
		period_quarter          INTEGER, /* KVART */
		period_year             INTEGER, /* SYEAR */
		is_active               INTEGER DEFAULT 0 /* PRIZNAK */
	);

	ALTER TABLE dict_rent_period ADD PRIMARY KEY (id);

END;

/* arenda */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[arenda]') AND type in (N'U'))
--DROP TABLE [dbo].[arenda]

--CREATE TABLE arenda /* ARENDA1NF */ (
--    id                  INTEGER NOT NULL, /* ID */
--    building_id         INTEGER, /* OBJECT_KOD */
--    org_balans_id       INTEGER, /* BALANS_KOD */
--    org_renter_id       INTEGER, /* ORG_KOD */
--    org_giver_id        INTEGER, /* ORGIV */
--    balans_id           INTEGER, /* BALID */
--    rent_year           INTEGER, /* LYEAR */
--    object_kind_id      INTEGER, /* OBJKIND - TABLE SKINDOBJ */ /* DUPLICATION */
--    purpose_group_id    INTEGER, /* GRPURPOSE - TABLE SGRPURPOSE */
--    purpose_id          INTEGER, /* PURPOSE - TABLE SPURPOSE */
--    purpose_str         VARCHAR(252), /* PURP_STR */
--    name                VARCHAR(252), /* NAME */
--    is_privat           INTEGER, /* PRIVAT */
--    update_src_id       INTEGER, /* UPDT_SOURCE */
--    agreement_kind_id   INTEGER, /* DOGKIND */
--    agreement_date      DATE, /* DOGDATE */
--    agreement_num       VARCHAR(18), /* DOGNUM */
--    agreement_str       VARCHAR(252), /* DOGSTR */
--    floor_number        VARCHAR(100), /* POVERH */
--    num_people          INTEGER, /* PPL_CNT */
--    cost_narah          NUMERIC(15,3), /* PLATA_NARAH */
--    cost_payed          NUMERIC(15,3), /* PLATA_SPLACH */
--    cost_debt           NUMERIC(15,3), /* PLATA_BORG */
--    cost_agreement      NUMERIC(15,3), /* PLATA_DOGOVOR */
--    cost_expert_1m      NUMERIC(15,3), /* VARTIST_EXP_V */
--    cost_expert_total   NUMERIC(15,3), /* VARTIST_EXP */
--    pidstava            VARCHAR(252), /* PIDSTAVA_STR */
--    pidstava_date       DATE, /* PIDSTAVA_DATE */
--    pidstava_num        VARCHAR(18), /* PIDSTAVA_NUM */
--    pidstava_fact       VARCHAR(200), /* PIDSTAVA_FACT */
--    pidstava2           VARCHAR(252), /* PIDSTAVA_STR2 */
--    pidstava_num2       VARCHAR(18), /* PIDSTAVA_NUM2 */
--    pidstava_date2      DATE, /* PIDSTAVA_DATE2 */
--    pidstava_display    VARCHAR(MAX), /* combined information from the ARRISH table */
--    rent_start_date     DATE, /* START_ORENDA */
--    rent_finish_date    DATE, /* FINISH_ORENDA */
--    rent_actual_finish_date DATE, /* FINISH_ORENDA_FACT */
--    rent_rate           NUMERIC(15,3), /* AR_STAVKA */
--    rent_rate_uah       NUMERIC(15,3), /* AR_STAVKA_UAH */
--    rent_square         NUMERIC(9,2), /* SQUARE */
--    priznak_1nf         INTEGER, /* PRIZNAK_1NF */
--    debt_timespan       VARCHAR(18), /* SROK_BORG */
--    order_num           VARCHAR(18), /* ORDER_NO */
--    order_date          DATE, /* ORDER_DATE */
--    order_no2           VARCHAR(18), /* ORDER_NO2 */
--    rishennya_id        INTEGER, /* RISHEN */
--    is_inactive         INTEGER, /* NOACTIVE */
--    inactive_date       DATE, /* NOACTIVE_DT */
--    is_deleted          INTEGER, /* DELETED */
--    del_date            DATE, /* DTDELETE */
--    date_expert         DATE, /* DTEXP */
--    is_subarenda        INTEGER, /* SUBAR */
--    privat_kind_id      INTEGER, /* WAYPRIVAT - TABLE S_WAYPRIVAT */
--    num_primirnikiv     INTEGER, /* KILK_PRIM */
--    date_expl_enter     DATE, /* DATE_EXP */
--    num_akt             VARCHAR(18), /* NUM_AKT */
--    date_akt            DATE, /* DATE_AKT */
--    num_bti             VARCHAR(18), /* NUM_BTI */
--    date_bti            DATE, /* DATE_BTI */
--    svidotstvo_serial   VARCHAR(3), /* SER_SVID */
--    svidotstvo_num      VARCHAR(18), /* NUM_SVID */
--    svidotstvo_date     DATE, /* DATE_SVID */
--    payment_type_id     INTEGER, /* KOD_VIDPLATA */
--    arch_id             INTEGER, /* ARCH_KOD */
--    modified_by         VARCHAR(128), /* ISP */
--    modify_date         DATETIME, /* DT */
--    note                VARCHAR(MAX), /* DESCRIPTION */
--    agreement_state     INTEGER DEFAULT 1, /* 0 - закрыт, 1 - активен, 2 - закрыт, но ведется претензийная работа (не хранится в 1НФ) 3 - Договір закінчився, оренда продовжена іншим договором */
--    is_insured			INTEGER, /* check-box  «Страхування» (так або ні) */
--    insurance_start		DATE, /* Період страхування. Начало */
--    insurance_end		DATE, /* Період страхування. Конец */
--    insurance_sum		NUMERIC(15,2), /* Сума  страхування */
--    is_loan_agreement	INTEGER /* договір позички */
--);

--ALTER TABLE arenda ADD PRIMARY KEY (id);

--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_object FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_org_balans FOREIGN KEY (org_balans_id) REFERENCES organizations (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_org_renter FOREIGN KEY (org_renter_id) REFERENCES organizations (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_org_giver FOREIGN KEY (org_giver_id) REFERENCES organizations (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_balans FOREIGN KEY (balans_id) REFERENCES balans (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_object_kind FOREIGN KEY (object_kind_id) REFERENCES dict_object_kind (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_purpose_group FOREIGN KEY (purpose_group_id) REFERENCES dict_balans_purpose_group (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_purpose FOREIGN KEY (purpose_id) REFERENCES dict_balans_purpose (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_update_src FOREIGN KEY (update_src_id) REFERENCES dict_balans_update_src (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_agreement_kind FOREIGN KEY (agreement_kind_id) REFERENCES dict_arenda_agreement_kind (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_privat_kind FOREIGN KEY (privat_kind_id) REFERENCES dict_arenda_privat_kind (id);
--ALTER TABLE arenda ADD CONSTRAINT fk_arenda_payment_type FOREIGN KEY (payment_type_id) REFERENCES dict_arenda_payment_type (id);

/* dict_arenda_note_status - Не импортируется из 1НФ */

IF OBJECT_ID('dict_arenda_note_status') IS NULL
BEGIN
	CREATE TABLE dict_arenda_note_status (
		id       INTEGER PRIMARY KEY,
		name     VARCHAR(64)
	);
	
	INSERT INTO dict_arenda_note_status (id, name) VALUES (0, 'ВИКОРИСТОВУЄТЬСЯ ЗА ПРИЗНАЧЕННЯМ')

	INSERT INTO dict_arenda_note_status (id, name) VALUES (1, 'ВИКОРИСТОВУЄТЬСЯ НЕ ЗА ПРИЗНАЧЕННЯМ')

	INSERT INTO dict_arenda_note_status (id, name) VALUES (2, 'НЕ ВИКОРИСТОВУЄТЬСЯ')

END

/* arenda_notes */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[arenda_notes]') AND type in (N'U'))
--DROP TABLE [dbo].[arenda_notes]

--CREATE TABLE arenda_notes /* ARENDA_PRIM */ (
--    id                  INTEGER IDENTITY(1,1), /* ID - not imported, because it is never referenced */
--    arenda_id           INTEGER, /* ARENDA */
--    purpose_group_id    INTEGER, /* GRPURP */
--    purpose_id          INTEGER, /* PURP - TABLE SPURPOSE */
--    purpose_str         VARCHAR(252), /* PURP_STR */
--    rent_square         NUMERIC(9,2), /* SQUARE */
--    modify_date         DATE, /* DT */
--    modified_by         VARCHAR(128), /* ISP */
--    note                VARCHAR(252), /* DESCRIPT */
--    rent_rate           NUMERIC(15,3), /* AR_STAVKA */
--    rent_rate_uah       NUMERIC(15,3), /* PRICE */
--    cost_narah          NUMERIC(15,3),
--    cost_agreement      NUMERIC(15,3), /* NARAH */
--    is_deleted          INTEGER, /* DELETED */
--    del_date            DATE, /* DTDELETE */
--    cost_expert_total   NUMERIC(15,3), /* VARTIST_EXP */
--    date_expert         DATE, /* DTEXP */
--    payment_type_id     INTEGER, /* KOD_VIDPLATA */
--    invent_no           VARCHAR(128), /* Инвентарный номер объекта (не импортируется из 1НФ) */
--    note_status_id      INTEGER /* (Не импортируется из 1НФ) */
--);

--ALTER TABLE arenda_notes ADD PRIMARY KEY (id);

--ALTER TABLE arenda_notes ADD CONSTRAINT fk_arenda_notes_arenda FOREIGN KEY (arenda_id) REFERENCES arenda (id);
--ALTER TABLE arenda_notes ADD CONSTRAINT fk_arenda_notes_purpose_group FOREIGN KEY (purpose_group_id) REFERENCES dict_balans_purpose_group (id);
--ALTER TABLE arenda_notes ADD CONSTRAINT fk_arenda_notes_purpose FOREIGN KEY (purpose_id) REFERENCES dict_balans_purpose (id);
--ALTER TABLE arenda_notes ADD CONSTRAINT fk_arenda_notes_payment_type FOREIGN KEY (payment_type_id) REFERENCES dict_arenda_payment_type (id);
--ALTER TABLE arenda_notes ADD CONSTRAINT fk_arenda_notes_status FOREIGN KEY (note_status_id) REFERENCES dict_arenda_note_status (id);

/* arenda_payments - Записи нового отчета по арендной плате, с привязкой к договорам аренды и кварталам */

IF OBJECT_ID('arenda_payments') IS NULL
BEGIN
	CREATE TABLE arenda_payments (
		id                      INTEGER IDENTITY(1,1),
		arenda_id               INTEGER, /* foreign key to the 'arenda' table */
		rent_period_id          INTEGER, /* foreign key to the 'dict_rent_period' table */
		sqr_total_rent          NUMERIC(15,3), /* Площа, надана в оренду, всього, кв.м  */
		sqr_payed_by_percent    NUMERIC(15,3), /* - на яку нараховується орендна плата у відсотках від вартості */
		sqr_payed_by_1uah       NUMERIC(15,3), /* - на яку нараховується орендна плата в розмірі 1 грн */
		sqr_payed_hourly        NUMERIC(15,3), /* Площа, надана в погодинну оренду, чи відповідно до угод про співпрацю, кв.м */
		payment_narah           NUMERIC(15,3), /* Нараховано орендної плати за звітний період, грн (без ПДВ) */
		last_year_saldo         NUMERIC(15,3), /* Переплачено орендної плати на початок звітного періоду, грн (без ПДВ) */
		payment_received        NUMERIC(15,3), /* Надходження орендної плати за звітний період, всього, грн (без ПДВ) */
		payment_nar_zvit        NUMERIC(15,3), /* - в тому числі нарахованої за звітний період (без боргів та переплат) */
		payment_budget_special  NUMERIC(15,3), /* Перераховано до бюджету за користування комунальним майном, грн (без ПДВ) ("Київенерго" та "Водоканал") */
		debt_total              NUMERIC(15,3), /* Заборгованість по орендній платі, всього,  грн (без ПДВ) */
		debt_zvit               NUMERIC(15,3), /* Заборгованість по орендній платі, за звітний період,  грн (без ПДВ) */
		debt_3_month            NUMERIC(15,3), /* Заборгованість по орендній платі, поточна до 3-х місяців,  грн (без ПДВ) */
		debt_12_month           NUMERIC(15,3), /* Заборгованість по орендній платі, прострочена від 4 до 12 місяців,  грн (без ПДВ) */
		debt_3_years            NUMERIC(15,3), /* Заборгованість по орендній платі, прострочена від 1 до 3 років,  грн (без ПДВ) */
		debt_over_3_years       NUMERIC(15,3), /* Заборгованість по орендній платі, безнадійна більше 3-х років,  грн (без ПДВ) */
		debt_v_mezhah_vitrat    NUMERIC(15,3), /* Заборгованість з орендної плати (із загальної заборгованості), розмір якої встановлено в межах витрат на утримання, грн (без ПДВ) */
		debt_spysano            NUMERIC(15,3), /* Списано заборгованості з орендної плати у звітному періоді, грн (без ПДВ) */
		num_zahodiv_total       INTEGER, /* Кількість заходів (попереджень, приписів і т.п.), всього */
		num_zahodiv_zvit        INTEGER, /* Кількість заходів (попереджень, приписів і т.п.), звітній період */
		num_pozov_total         INTEGER, /* Кількість позовів до суду, всього */
		num_pozov_zvit          INTEGER, /* Кількість позовів до суду, звітній період */
		num_pozov_zadov_total   INTEGER, /* Задоволено позовів, всього */
		num_pozov_zadov_zvit    INTEGER, /* Задоволено позовів, звітній період */
		num_pozov_vikon_total   INTEGER, /* Відкрито виконавчих впроваджень, всього */
		num_pozov_vikon_zvit    INTEGER, /* Відкрито виконавчих впроваджень, звітній період */
		debt_pogasheno_total    NUMERIC(15,3), /* Погашено заборгованості за результатами вжитих заходів, всього, грн (без ПДВ) */
		debt_pogasheno_zvit     NUMERIC(15,3), /* Погашено заборгованості за результатами вжитих заходів, за звітний період, грн (без ПДВ) */
		budget_narah_50_uah     NUMERIC(15,3), /* Нарахована сума до бюджету 50% від загальної суми надходжень орендної плати за звітний період, грн. (без ПДВ) */
		budget_zvit_50_uah      NUMERIC(15,3), /* Перераховано до бюджету 50% за звітний період всього з 1 січня поточного року, грн. (без ПДВ) */
		budget_prev_50_uah      NUMERIC(15,3), /* - в тому числі перераховано до бюджету 50% боргів у звітному періоді з 1 січня поточного року за попередні роки, грн. (без ПДВ) */
		budget_debt_50_uah      NUMERIC(15,3), /* Заборгованість зі сплати 50% до бюджету від оренди майна за звітний період, грн. (без ПДВ) */
		budget_debt_30_50_uah   NUMERIC(15,3), /* Заборгованість зі сплати 50% до бюджету від оренди майна минулих років, грн. (без ПДВ) */
		is_renter_out           INTEGER DEFAULT 0, /* 1 ~ Орендар виїхав */
		is_debt_exists          INTEGER DEFAULT 0, /* 1 ~ Існує заборгованість */
		modify_date             DATETIME,
		modified_by             VARCHAR(128),
		is_special_organization INTEGER, /* 'Київенерго' та 'Водоканал' */
		old_debts_payed         NUMERIC(15,3), /* Погашення заборгованості минулих періодів */
	);

	ALTER TABLE arenda_payments ADD PRIMARY KEY (id);
	
	/* ALTER TABLE arenda_payments ADD CONSTRAINT fk_arenda_payments_arenda FOREIGN KEY (arenda_id) REFERENCES arenda (id);
	ALTER TABLE arenda_payments ADD CONSTRAINT fk_arenda_payments_period FOREIGN KEY (rent_period_id) REFERENCES dict_rent_period (id); */
END

/* arenda_payments_cmk - Записи нового отчета по арендной плате, касающиеся ЦМК; с привязкой к балансодержателю и кварталу */

IF OBJECT_ID('arenda_payments_cmk') IS NULL
BEGIN
	CREATE TABLE arenda_payments_cmk (
		id                      INTEGER IDENTITY(1,1),
		arenda_rented_id        INTEGER NOT NULL, /* Foreign key to the 'arenda_rented' table */
		rent_period_id          INTEGER, /* Foreign key to the 'dict_rent_period' table */
		cmk_sqr_rented          NUMERIC(15,3), /* ЦМК Площа в оренді, кв.м */
	    cmk_payment_narah       NUMERIC(15,3), /* ЦМК Нарахована орендна плата, грн (без ПДВ) */
	    cmk_payment_to_budget   NUMERIC(15,3), /* ЦМК Перераховано до бюджету, грн (без ПДВ) */
	    cmk_rent_debt           NUMERIC(15,3), /* ЦМК Заборгованість по орендній платі,  грн (без ПДВ) */
	    modify_date             DATETIME,
		modified_by             VARCHAR(128)
	);

	ALTER TABLE arenda_payments_cmk ADD PRIMARY KEY (id);
END

/* arenda_rented - Перечень договоров аренды со стороны арендатора */

IF OBJECT_ID('arenda_rented') IS NULL
BEGIN
	CREATE TABLE arenda_rented (
		id                  INTEGER IDENTITY(1,1),
		building_id         INTEGER,
		org_renter_id       INTEGER,
		org_giver_id        INTEGER,
		payment_type_id     INTEGER,
		agreement_date      DATE,
		agreement_num       VARCHAR(18),
		rent_start_date     DATE,
		rent_finish_date    DATE,
		rent_square         NUMERIC(9,2),
		is_subarenda        INTEGER,
		is_cmk              INTEGER,
		cmk_sqr_rented      NUMERIC(15,3),
		cmk_payment_narah   NUMERIC(15,3),
		cmk_payment_to_budget NUMERIC(15,3),
		cmk_rent_debt       NUMERIC(15,3),
		modify_date         DATETIME,
		modified_by         VARCHAR(128),
		is_deleted          INTEGER DEFAULT 0,
		del_date            DATETIME
	);
	
	ALTER TABLE arenda_rented ADD PRIMARY KEY (id);
	
	/* Force the 'arch_arenda_rented' table to be re-created */
	--IF NOT (OBJECT_ID('arch_arenda_rented') IS NULL)
	--BEGIN
	--	DROP TABLE arch_arenda_rented
	--END
END	

IF OBJECT_ID('arch_arenda_rented') IS NULL
BEGIN
	SELECT TOP 0 * INTO arch_arenda_rented FROM arenda_rented

	ALTER TABLE arch_arenda_rented DROP COLUMN id

	ALTER TABLE arch_arenda_rented ADD id INTEGER NOT NULL
	ALTER TABLE arch_arenda_rented ADD archive_entry_id INTEGER IDENTITY(1,1) PRIMARY KEY
	ALTER TABLE arch_arenda_rented ADD archive_entry_timestamp DATETIME

	ALTER TABLE dbo.arch_arenda_rented ADD CONSTRAINT FK_arch_arenda_rented_arenda FOREIGN KEY
		(id) REFERENCES dbo.arenda_rented (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

GO

/* fnPushRentedObjectToArchive - generates an archive entry for the 'arenda_rented' table entry */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnPushRentedObjectToArchive]'))
	DROP PROCEDURE [dbo].[fnPushRentedObjectToArchive]
GO

CREATE PROCEDURE dbo.fnPushRentedObjectToArchive
(	
	@ARENDA_RENTED_ID INTEGER
)
AS
	INSERT INTO arch_arenda_rented
	(
		id,
		building_id,
		org_renter_id,
		org_giver_id,
		payment_type_id,
		agreement_date,
		agreement_num,
		rent_start_date,
		rent_finish_date,
		rent_square,
		is_subarenda,
		is_cmk,
		cmk_sqr_rented,
		cmk_payment_narah,
		cmk_payment_to_budget,
		cmk_rent_debt,
		modify_date,
		modified_by,
		is_deleted,
		del_date,
		archive_entry_timestamp
	)
	SELECT
		id,
		building_id,
		org_renter_id,
		org_giver_id,
		payment_type_id,
		agreement_date,
		agreement_num,
		rent_start_date,
		rent_finish_date,
		rent_square,
		is_subarenda,
		is_cmk,
		cmk_sqr_rented,
		cmk_payment_narah,
		cmk_payment_to_budget,
		cmk_rent_debt,
		modify_date,
		modified_by,
		is_deleted,
		del_date,
		GETDATE() AS 'archive_entry_timestamp'
	FROM arenda_rented WHERE id = @ARENDA_RENTED_ID
GO

GO

/******************************************************************************/
/*                   Documents and the required dictionaries                  */
/******************************************************************************/

/* dict_doc_kind */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_doc_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_doc_kind]

--CREATE TABLE dict_doc_kind /* SKINDDOK */ (
--    id              INTEGER NOT NULL, /* ID */
--    name            VARCHAR(60), /* NAME */
--    modified_by     VARCHAR(128), /* ISP */
--    modify_date     DATE /* DT */
--);

--ALTER TABLE dict_doc_kind ADD PRIMARY KEY (id);

/* dict_doc_general_kind */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_doc_general_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_doc_general_kind]

--CREATE TABLE dict_doc_general_kind /* S_UZAG_DOC */ (
--    id              INTEGER NOT NULL, /* ID */
--    name            VARCHAR(50), /* NAZVA */
--    modified_by     VARCHAR(128),
--    modify_date     DATE
--);

--ALTER TABLE dict_doc_general_kind ADD PRIMARY KEY (id);

/* dict_doc_depend_kind */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_doc_depend_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_doc_depend_kind]

--CREATE TABLE dict_doc_depend_kind /* S_DEPEND_KIND */ (
--    id              INTEGER NOT NULL, /* KOD */
--    name            VARCHAR(60) /* NAME */
--);

--ALTER TABLE dict_doc_depend_kind ADD PRIMARY KEY (id);

/* dict_doc_commission */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_doc_commission]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_doc_commission]

--CREATE TABLE dict_doc_commission /* SVKOMIS */ (
--    id              INTEGER NOT NULL, /* KOD */
--    name            VARCHAR(40), /* NAME */
--    modified_by     VARCHAR(128), /* ISP */
--    modify_date     DATE /* DT */
--);

--ALTER TABLE dict_doc_commission ADD PRIMARY KEY (id);

/* dict_doc_appendix_kind */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_doc_appendix_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_doc_appendix_kind]

--CREATE TABLE dict_doc_appendix_kind /* SVDOD */ (
--    id                    INTEGER NOT NULL, /* KOD */
--    name                  VARCHAR(60), /* NAME */
--    modified_by           VARCHAR(128), /* ISP */
--    modify_date           DATE, /* DT */
--    otdel_gukv_id         INTEGER /* VIDDIL - TABLE S_VIDDIL */
--);

--ALTER TABLE dict_doc_appendix_kind ADD PRIMARY KEY (id);
--ALTER TABLE dict_doc_appendix_kind ADD CONSTRAINT fk_doc_appendix_kind_otdel FOREIGN KEY (otdel_gukv_id) REFERENCES dict_otdel_gukv (id);

/* dict_doc_stadiya */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_doc_stadiya]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_doc_stadiya]

--CREATE TABLE dict_doc_stadiya /* SSTADIYA */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(60), /* NAME */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE /* DT */
--);

--ALTER TABLE dict_doc_stadiya ADD PRIMARY KEY (id);

/* dict_doc_source */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_doc_source]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_doc_source]

--CREATE TABLE dict_doc_source /* SPIDROZDIL from the database 'Rosporadjennia' */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(60), /* NAME */
--    modified_by        VARCHAR(128), /* not migrated */
--    modify_date        DATE /* not migrated */
--);

--ALTER TABLE dict_doc_source ADD PRIMARY KEY (id);

/* dict_doc_state */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_doc_state]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_doc_state]

--CREATE TABLE dict_doc_state /* SSTATUS from the database 'Rosporadjennia' */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(60), /* NAME */
--    modified_by        VARCHAR(128), /* not migrated */
--    modify_date        DATE /* not migrated */
--);

--ALTER TABLE dict_doc_state ADD PRIMARY KEY (id);

/* dict_rent_decisions */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_rent_decisions]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_rent_decisions]

--CREATE TABLE dict_rent_decisions /* SOZR */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(40), /* NAME */
--    name_abbr          VARCHAR(2), /* NAMEK */
--    modified_by        VARCHAR(128), /* ISP */
--    modify_date        DATE, /* DT */
--    name_report_form   VARCHAR(255), /* NAMEREPORT */
--    name_order_form    VARCHAR(40) /* NAME_EXP */
--);

--ALTER TABLE dict_rent_decisions ADD PRIMARY KEY (id);

/* documents */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[documents]') AND type in (N'U'))
--DROP TABLE [dbo].[documents]

--CREATE TABLE documents /* ROZP_DOK */ (
--    id              INTEGER NOT NULL, /* DOK_ID */
--    kind_id         INTEGER, /* DOKKIND */
--    general_kind_id INTEGER, /* ID_UZAG_DOC - S_UZAG_DOC from the Balans database */
--    doc_date        DATE, /* DOKDATA */
--    doc_num         VARCHAR(20), /* DOKNUM */
--    topic           VARCHAR(255), /* DOKTEMA */
--    serial_num      VARCHAR(24), /* SERIA */
--    note            VARCHAR(MAX), /* KOMENTAR - from the SPROTOK and SRASPOR tables of the 1NF database */
--    modified_by     VARCHAR(128), /* ISP */
--    modify_date     DATE, /* DT */
--    display_name    AS (N'Документ N ' + RTRIM(doc_num) + N' вiд ' + CONVERT(VARCHAR(32), doc_date, 104)),
--    search_name     AS
--        CASE WHEN doc_num IS NULL THEN (COALESCE(topic, '') + N' Б/Н')
--        ELSE (N'№ ' + RTRIM(doc_num) + N' вiд ' + COALESCE(CONVERT(VARCHAR(32),doc_date,104), '') + N': ' + COALESCE(topic, ''))
--        END,
--    is_not_in_work  INTEGER DEFAULT 0, /* WORKED */
--    is_archived     INTEGER DEFAULT 0, /* ARXIV - from the SPROTOK and SRASPOR tables of the 1NF database */
--    is_reconstruct  INTEGER, /* IS_REC */
--    is_text_exists  INTEGER, /* ISTEXT column from the ROZP_DOC table of the NJF database */
--    is_priv_rishen  INTEGER, /* Indicates that this is a primary document from 'Privatizacia' database */
--    receive_date    DATE, /* DATA_NADH */
--    commission_id   INTEGER, /* VKOMIS (SVKOMIS) from the SPROTOK table of the 1NF database */
--    source_id       INTEGER, /* PIDROZDIL (SPIDROZDIL) from the ROZP_DOC table of the NJF database */
--    state_id        INTEGER, /* STATUS (SSTATUS) from the ROZP_DOC table of the NJF database */
--    priv_date       DATE, /* DATE_PRIV */
--    summa           NUMERIC(15,3), /* SUMMA from the ROZP_DOC table of the NJF database */
--    summa_zalishkova NUMERIC(15,3), /* SUMMA_ZAL from the ROZP_DOC table of the NJF database */
--    extern_doc_id   INTEGER /* RDOC_ID column from the ROZP_DOC table of the NJF database */
--);

--ALTER TABLE documents ADD PRIMARY KEY (id);

--ALTER TABLE documents ADD CONSTRAINT fk_documents_doc_kind FOREIGN KEY (kind_id) REFERENCES dict_doc_kind (id);
--ALTER TABLE documents ADD CONSTRAINT fk_documents_source FOREIGN KEY (source_id) REFERENCES dict_doc_source (id);
--ALTER TABLE documents ADD CONSTRAINT fk_documents_state FOREIGN KEY (state_id) REFERENCES dict_doc_state (id);

/* doc_appendices */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[doc_appendices]') AND type in (N'U'))
--DROP TABLE [dbo].[doc_appendices]

--CREATE TABLE doc_appendices /* RASPOR, PROTOK */ (
--    id              INTEGER NOT NULL, /* KOD */
--    app_date        DATE, /* DT */
--    app_doc_num     VARCHAR(15), /* NOM */
--    app_num         VARCHAR(2), /* DODATOK */
--    app_num_user    VARCHAR(2), /* DODATOK_USER */
--    is_archived     VARCHAR(1), /* ARXIV VARCHAR(1) */
--    kind_id         INTEGER, /* VDOD */
--    modified_by     VARCHAR(128), /* ISP */
--    modify_date     DATE, /* DTISP */
--    doc_id          INTEGER, /* SPKOD, SRKOD */
--    doc_date        DATE, /* DTP, DTR */
--    doc_num         VARCHAR(15), /* NOMP, NOMR */
--    stadiya_id      INTEGER /* STADIYA - TABLE SSTADIYA */
--);

--ALTER TABLE doc_appendices ADD PRIMARY KEY (id);

--ALTER TABLE doc_appendices ADD CONSTRAINT fk_appendices_doc FOREIGN KEY (doc_id) REFERENCES documents (id);
--ALTER TABLE doc_appendices ADD CONSTRAINT fk_appendices_kind FOREIGN KEY (kind_id) REFERENCES dict_doc_appendix_kind (id);
--ALTER TABLE doc_appendices ADD CONSTRAINT fk_appendices_stadiya FOREIGN KEY (stadiya_id) REFERENCES dict_doc_stadiya (id);

/* arenda_applications */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[arenda_applications]') AND type in (N'U'))
--DROP TABLE [dbo].[arenda_applications]

--CREATE TABLE arenda_applications /* ZAMOV */ (
--    id               INTEGER NOT NULL, /* KOD */
--    renter_name      VARCHAR(100), /* ARNAME */
--    rent_goal        VARCHAR(252), /* NAZ */
--    rent_term        VARCHAR(252), /* STROK */
--    building_id      INTEGER, /* OBJ */
--    appl_letter_num  VARCHAR(9), /* NOMVLU */
--    appl_letter_date DATE, /* DTVLU */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE, /* DTISP */
--    appl_sqr         NUMERIC(15,2), /* S_ZAMOV */
--    org_renter_id    INTEGER, /* SYSTEM_AR */
--    subarenda_id     INTEGER /* OZNSUBOREND */
--);

--ALTER TABLE arenda_applications ADD PRIMARY KEY (id);

--ALTER TABLE arenda_applications ADD CONSTRAINT fk_arenda_appl_building FOREIGN KEY (building_id) REFERENCES documents (id);
--ALTER TABLE arenda_applications ADD CONSTRAINT fk_arenda_appl_renter FOREIGN KEY (org_renter_id) REFERENCES documents (id);

/* arenda_decisions */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[arenda_decisions]') AND type in (N'U'))
--DROP TABLE [dbo].[arenda_decisions]

--CREATE TABLE arenda_decisions /* RISHEN */ (
--    id               INTEGER NOT NULL, /* KOD */
--    decision_id      INTEGER, /* OZR - TABLE SOZR */
--    etr              VARCHAR(2), /* ETR */
--    rent_term        VARCHAR(252), /* STROK */
--    rent_term_note   VARCHAR(252), /* PR_STROK */
--    title            VARCHAR(252), /* NAZVA */
--    rent_purpose     VARCHAR(252), /* RDNM */
--    nov_prot         VARCHAR(1) DEFAULT '', /* NOVPROT */
--    building_id      INTEGER, /* OBJ - TABLE OBJECT_1NF */
--    org_renter_id    INTEGER, /* SYSTEM_AR - TABLE SORG_1NF */
--    org_balans_id    INTEGER, /* SYSTEM_ARQ - TABLE SORG_1NF */
--    org_giver_id     INTEGER, /* ORGIV */
--    balans_id        INTEGER, /* BALANS_ID - TABLE BALANS_1NF */
--    apply_date       DATE, /* DTILU */
--    apply_letter_id  INTEGER DEFAULT 0, /* KODILU */
--    apply_letter_num VARCHAR(9) DEFAULT 0, /* NOMILU */
--    appendix_prot_id INTEGER, /* KODP */
--    appendix_rasp_id INTEGER, /* KODR */
--    application_id   INTEGER, /* KODZ */
--    num_rooms        INTEGER DEFAULT 0, /* K_KOMN */
--    sqr_applied      NUMERIC(15,2), /* S_ZAMOV */
--    sqr_robocha      NUMERIC(15,2), /* S_ROB */
--    cost_1m          NUMERIC(15,2), /* VART_1M */
--    cost_rent        NUMERIC(15,2), /* PLATA */
--    is_subarenda     INTEGER, /* OZNSUBOREND */
--    stadiya_id       INTEGER, /* STADIYA - TABLE SSTADIYA */
--    rishen_punkt     INTEGER, /* NPP */
--    modified_by      VARCHAR(128), /* ISP */
--    modify_date      DATE DEFAULT GETDATE(), /* DTISP */
--    note_rish_etap1  VARCHAR(MAX), /* TRISH */
--    note_rish2_etap1 VARCHAR(MAX), /* TRISHP */
--    note_nazva       VARCHAR(MAX), /* NAZ */
--    note_rish_etap2  VARCHAR(MAX), /* TRISH1 */
--    note_rish2_etap2 VARCHAR(MAX), /* TRISHP1 */
--);

--ALTER TABLE arenda_decisions ADD PRIMARY KEY (id);

--ALTER TABLE arenda_decisions ADD CONSTRAINT fk_rent_decisions_building FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE arenda_decisions ADD CONSTRAINT fk_rent_decisions_renter FOREIGN KEY (org_renter_id) REFERENCES organizations (id);
--ALTER TABLE arenda_decisions ADD CONSTRAINT fk_rent_decisions_giver FOREIGN KEY (org_giver_id) REFERENCES organizations (id);
--ALTER TABLE arenda_decisions ADD CONSTRAINT fk_rent_decisions_balans FOREIGN KEY (org_balans_id) REFERENCES organizations (id);
--ALTER TABLE arenda_decisions ADD CONSTRAINT fk_rent_decisions_decision FOREIGN KEY (decision_id) REFERENCES dict_rent_decisions (id);
--ALTER TABLE arenda_decisions ADD CONSTRAINT fk_rent_decisions_stadiya FOREIGN KEY (stadiya_id) REFERENCES dict_doc_stadiya (id);
--ALTER TABLE arenda_decisions ADD CONSTRAINT fk_rent_decisions_protocol FOREIGN KEY (appendix_prot_id) REFERENCES doc_appendices (id);
--ALTER TABLE arenda_decisions ADD CONSTRAINT fk_rent_decisions_raspor FOREIGN KEY (appendix_rasp_id) REFERENCES doc_appendices (id);

/* link_arenda_2_decisions */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[link_arenda_2_decisions]') AND type in (N'U'))
--DROP TABLE [dbo].[link_arenda_2_decisions]

--CREATE TABLE link_arenda_2_decisions /* ARRISH */ (
--    id              INTEGER IDENTITY(1,1), /* ID - Not imported */
--    arenda_id       INTEGER, /* ARENDA */
--    rishen_id       INTEGER, /* RISHEN */
--    ord             INTEGER, /* ORD */
--    modified_by     VARCHAR(128), /* ISP */
--    modify_date     DATE, /* DT */
--    doc_num         VARCHAR(18), /* NOM */
--    doc_date        DATE, /* DT_DOC */
--    doc_dodatok     VARCHAR(2), /* DODATOK */
--    doc_punkt       INTEGER, /* PUNKT */
--    purpose_str     VARCHAR(255), /* PURP_STR */
--    rent_square     NUMERIC(15,2), /* SQUARE */
--    /* KIND_PIDSTAVA  INTEGER, /* KIND_PIDSTAVA - Not imported */ */
--    decision_id     INTEGER, /* OZR */
--    doc_raspor_id   INTEGER, /* RASPOR */
--    pidstava        VARCHAR(255) /* PIDSTAVA */
--);

--ALTER TABLE link_arenda_2_decisions ADD PRIMARY KEY (id);

--ALTER TABLE link_arenda_2_decisions ADD CONSTRAINT fk_arenda2decision_arenda FOREIGN KEY (arenda_id) REFERENCES arenda (id);
--ALTER TABLE link_arenda_2_decisions ADD CONSTRAINT fk_arenda2decision_appl FOREIGN KEY (rishen_id) REFERENCES arenda_decisions (id);
--ALTER TABLE link_arenda_2_decisions ADD CONSTRAINT fk_arenda2decision_decision FOREIGN KEY (decision_id) REFERENCES dict_rent_decisions (id);
--ALTER TABLE link_arenda_2_decisions ADD CONSTRAINT fk_arenda2decision_raspor FOREIGN KEY (doc_raspor_id) REFERENCES documents (id);

/* arenda_decision_objects */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[arenda_decision_objects]') AND type in (N'U'))
--DROP TABLE [dbo].[arenda_decision_objects]

--CREATE TABLE arenda_decision_objects /* RISHOBJ */ (
--    id             INTEGER IDENTITY(1,1), /* KOD - Not imported */
--    name           VARCHAR(MAX), /* NAZ */
--    location       VARCHAR(MAX), /* ROZTASH */
--    application_id INTEGER, /* KODZ */
--    rishen_id      INTEGER, /* RISHEN */
--    building_id    INTEGER, /* OBJ */
--    poverh         INTEGER, /* POVERCH */
--    modified_by    VARCHAR(128), /* ISP */
--	modify_date    DATE, /* DTISP */
--    obj_square     NUMERIC(15,2), /* PL */
--    stavka         VARCHAR(80), /* STAVKA */
--    stavka_m       VARCHAR(80) /* STAVKAM */
--);

--ALTER TABLE arenda_decision_objects ADD PRIMARY KEY (id);

--ALTER TABLE arenda_decision_objects ADD CONSTRAINT fk_ado_appl FOREIGN KEY (application_id) REFERENCES arenda_applications (id);
--ALTER TABLE arenda_decision_objects ADD CONSTRAINT fk_ado_rishen FOREIGN KEY (rishen_id) REFERENCES arenda_decisions (id);
--ALTER TABLE arenda_decision_objects ADD CONSTRAINT fk_ado_building FOREIGN KEY (building_id) REFERENCES buildings (id);

/* doc_dependencies */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[doc_dependencies]') AND type in (N'U'))
--DROP TABLE [dbo].[doc_dependencies]

--CREATE TABLE doc_dependencies /* DOK_DEPEND */ (
--    id              INTEGER IDENTITY(1,1), /* not imported */
--    master_doc_id   INTEGER, /* MASTER */
--    slave_doc_id    INTEGER, /* SLAVE */
--    depend_kind_id  INTEGER, /* VID */
--    modified_by     VARCHAR(128), /* ISP */
--    modify_date     DATE
--);

--ALTER TABLE doc_dependencies ADD PRIMARY KEY (id);

--ALTER TABLE doc_dependencies ADD CONSTRAINT fk_doc_depend_master FOREIGN KEY (master_doc_id) REFERENCES documents (id);
--ALTER TABLE doc_dependencies ADD CONSTRAINT fk_doc_depend_slave FOREIGN KEY (slave_doc_id) REFERENCES documents (id);
--ALTER TABLE doc_dependencies ADD CONSTRAINT fk_doc_depend_kind FOREIGN KEY (depend_kind_id) REFERENCES dict_doc_depend_kind (id);

/* dict_doc_change_type */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_doc_change_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_doc_change_type]

--CREATE TABLE dict_doc_change_type /* S_DOK_CHANGE */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              VARCHAR(60) /* NAME */
--);

--ALTER TABLE dict_doc_change_type ADD PRIMARY KEY (id);


/* dict_org_doc_link_kind */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_org_doc_link_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_org_doc_link_kind]

--CREATE TABLE dict_org_doc_link_kind (
--    id                INTEGER NOT NULL,
--    name              VARCHAR(64),
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_org_doc_link_kind ADD PRIMARY KEY (id);


/* building_docs */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[building_docs]') AND type in (N'U'))
--DROP TABLE [dbo].[building_docs]

--CREATE TABLE building_docs /* OBJECT_DOKS_PROPERTIES */ (
--    id                INTEGER IDENTITY(1,1), /* ID is migrated into 'old_id' field */
--    building_id       INTEGER NOT NULL, /* OBJECT_KOD - TABLE OBJECT_1NF */
--    /* master_doc_id  INTEGER, /* Only 'privatization' DB contains hierarchical relations between documents and objects */ */
--    document_id       INTEGER NOT NULL, /* DOK_ID - TABLE ROZP_DOK */
--    obj_name          VARCHAR(255), /* NAME */
--    obj_description   VARCHAR(100), /* CHARACTERISTIC */
--    obj_build_year    INTEGER, /* YEAR_BUILD */
--    obj_expl_enter_year INTEGER, /* YEAR_EXPL */
--    obj_length        NUMERIC(9,2), /* LEN */
--    note              VARCHAR(MAX), /* DESCRIPTION */
--    cost_balans       NUMERIC(15,3), /* SUMMA_BALANS */
--    cost_znos         NUMERIC(15,3), /* SUMMA_ZNOS */
--    cost_zalishkova   NUMERIC(15,3), /* SUMMA_ZAL */
--    cost_expert       NUMERIC(15,3), /* EXP_VART */
--    num_rooms         INTEGER, /* ROOM_COUNT */
--    num_floors        INTEGER, /* FLOORS */
--    obj_location      VARCHAR(60), /* ARRANGEMENT */
--    purpose_group_id  INTEGER, /* GRPURP - TABLE SGRPURPOSE */
--    purpose_id        INTEGER, /* PURPOSE - TABLE SPURPOSE */
--    tech_condition_id INTEGER, /* TEXSTAN - TABLE STEXSTAN */
--    object_kind_id    INTEGER DEFAULT 0, /* OBJKIND - TABLE SKINDOBJ */
--    object_type_id    INTEGER, /* OBJTYPE - TABLE STYPEOBJ */
--    sqr_obj           NUMERIC(9,2), /* SQUARE */
--    sqr_free          NUMERIC(9,2), /* CLR_SQUARE */
--    sqr_habit         NUMERIC(9,2), /* LSQUARE */
--    sqr_non_habit     NUMERIC(9,2), /* NLSQARE */
--    is_not_in_work    INTEGER DEFAULT 0, /* WORKED */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE, /* DT */
--    doc_dodatok       VARCHAR(8), /* DODATOK */
--    doc_tab           VARCHAR(8), /* TAB */
--    doc_pos           VARCHAR(12), /* POS */
--    /*
--    addr_street_name  VARCHAR(100), /* OBJ_ULNAME */ /* DUPLICATION */
--    addr_street_name2 VARCHAR(100), /* OBJ_ULNAME2 */ /* DUPLICATION */
--    addr_nomer1       VARCHAR(10), /* OBJ_NOMER1 */ /* DUPLICATION */
--    addr_nomer2       VARCHAR(18), /* OBJ_NOMER2 */ /* DUPLICATION */
--    addr_nomer3       VARCHAR(10), /* OBJ_NOMER3 */ /* DUPLICATION */
--    addr_misc         VARCHAR(100), /* ADRDOP */ /* DUPLICATION */
--    */
--    doc_change_type_id INTEGER, /* DOK_CHANGE  - TABLE S_DOK_CHANGE */
--    doc_change_num    VARCHAR(20), /* DOK_CH_NUM */
--    doc_change_date   DATE, /* DOK_CH_DATE */
--    doc_change_doc_id INTEGER, /* CH_DOK_ID - TABLE ROZP_DOK */
--    form_ownership_id INTEGER, /* FORM_VLASN - TABLE S_FORM_VLASN */ /* DUPLICATION */
--    is_on_balans      INTEGER, /* BAL_FLAG */
--    org_id            INTEGER, /* ORG - TABLE SORG_1NF */
--    last_org_id       INTEGER, /* LAST_ORG */
--    date_expert       DATE, /* EXP_DATE */
--    date_priv         DATE, /* DATE_PRIV */
--    ozn_priv_id       INTEGER, /* ID_OZN_PRIV */
    
--    /* The following fields are added from the 'Rosporadjennia' database (OBJECT_DOKS_PROPERTIES table) */
    
--    pipe_diameter     VARCHAR(20), /* DIAM_TRUB */
--    pipe_material     VARCHAR(20) /* MAT_TRUB*/
--);

--ALTER TABLE building_docs ADD PRIMARY KEY (id);

--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_building FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_document FOREIGN KEY (document_id) REFERENCES documents (id);
/*ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_master_doc FOREIGN KEY (master_doc_id) REFERENCES documents (id);*/
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_purpose_group FOREIGN KEY (purpose_group_id) REFERENCES dict_balans_purpose_group (id);
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_purpose FOREIGN KEY (purpose_id) REFERENCES dict_balans_purpose (ID);
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_tech_condition FOREIGN KEY (tech_condition_id) REFERENCES dict_tech_state (id);
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_object_kind FOREIGN KEY (object_kind_id) REFERENCES dict_object_kind (id);
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_object_type FOREIGN KEY (object_type_id) REFERENCES dict_object_type (id);
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_form_ownership FOREIGN KEY (form_ownership_id) REFERENCES dict_org_ownership (id);
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_change_type FOREIGN KEY (doc_change_type_id) REFERENCES dict_doc_change_type (id);
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_changed_doc FOREIGN KEY (doc_change_doc_id) REFERENCES documents (id);
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_org FOREIGN KEY (org_id) REFERENCES organizations (id);
--ALTER TABLE building_docs ADD CONSTRAINT fk_building_docs_last_org FOREIGN KEY (last_org_id) REFERENCES organizations (id);

/* balans_docs */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[balans_docs]') AND type in (N'U'))
--DROP TABLE [dbo].[balans_docs]

--CREATE TABLE balans_docs /* BALANS_DOK_PROP */ (
--    id                INTEGER IDENTITY(1,1), /* ID - not imported */
--    balans_id         INTEGER, /* BALID */
--    building_id       INTEGER, /* OBJECT_KOD */
--    link_kind         INTEGER, /* FLAG */
--    building_docs_id  INTEGER, /* DOK_ID */
--    sort_field        INTEGER /* SORT_FLD */
--);

--ALTER TABLE balans_docs ADD PRIMARY KEY (id);

--ALTER TABLE balans_docs ADD CONSTRAINT fk_balans_docs_balans FOREIGN KEY (balans_id) REFERENCES balans (id);
--ALTER TABLE balans_docs ADD CONSTRAINT fk_balans_docs_building FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE balans_docs ADD CONSTRAINT fk_balans_docs_bdoc FOREIGN KEY (building_docs_id) REFERENCES building_docs (id);

/* object_rights_building_docs */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[object_rights_building_docs]') AND type in (N'U'))
--DROP TABLE [dbo].[object_rights_building_docs]

--CREATE TABLE object_rights_building_docs /* OBJECT_DOKS_PROPERTIES from the 'Rosporadjennia' database */ (
--    id                INTEGER IDENTITY(1,1), /* ID - not migrated */
--    building_id       INTEGER NOT NULL, /* OBJECT_KOD - TABLE OBJECT_1NF */
--    document_id       INTEGER NOT NULL, /* DOK_ID - TABLE ROZP_DOK */
--    obj_name          VARCHAR(255), /* NAME */
--    obj_description   VARCHAR(100), /* CHARACTERISTIC */
--    obj_build_year    INTEGER, /* YEAR_BUILD */
--    obj_expl_enter_year INTEGER, /* YEAR_EXPL */
--    obj_length        NUMERIC(9,2), /* LEN */
--    note              VARCHAR(MAX), /* DESCRIPTION */
--    cost_balans       NUMERIC(15,3), /* SUMMA_BALANS */
--    cost_znos         NUMERIC(15,3), /* SUMMA_ZNOS */
--    cost_zalishkova   NUMERIC(15,3), /* SUMMA_ZAL */
--    num_rooms         INTEGER, /* ROOM_COUNT */
--    num_floors        INTEGER, /* FLOORS */
--    obj_location      VARCHAR(60), /* ARRANGEMENT */
--    purpose_group_id  INTEGER, /* GRPURP - TABLE SGRPURPOSE */
--    purpose_id        INTEGER, /* PURPOSE - TABLE SPURPOSE */
--    tech_condition_id INTEGER, /* TEXSTAN - TABLE STEXSTAN */
--    object_kind_id    INTEGER DEFAULT 0, /* OBJKIND - TABLE SKINDOBJ */
--    object_type_id    INTEGER, /* OBJTYPE - TABLE STYPEOBJ */
--    sqr_obj           NUMERIC(9,2), /* SQUARE */
--    sqr_free          NUMERIC(9,2), /* CLR_SQUARE */
--    sqr_habit         NUMERIC(9,2), /* LSQUARE */
--    sqr_non_habit     NUMERIC(9,2), /* NLSQARE */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE, /* DT */
--    doc_pos           VARCHAR(12), /* POS */
--    pipe_diameter     VARCHAR(20), /* DIAM_TRUB */
--    pipe_material     VARCHAR(20) /* MAT_TRUB*/
--);
        
--ALTER TABLE object_rights_building_docs ADD PRIMARY KEY (id);

--ALTER TABLE object_rights_building_docs ADD CONSTRAINT fk_orbd_building FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE object_rights_building_docs ADD CONSTRAINT fk_orbd_document FOREIGN KEY (document_id) REFERENCES documents (id);
--ALTER TABLE object_rights_building_docs ADD CONSTRAINT fk_orbd_purpose_group FOREIGN KEY (purpose_group_id) REFERENCES dict_balans_purpose_group (id);
--ALTER TABLE object_rights_building_docs ADD CONSTRAINT fk_orbd_purpose FOREIGN KEY (purpose_id) REFERENCES dict_balans_purpose (ID);
--ALTER TABLE object_rights_building_docs ADD CONSTRAINT fk_orbd_tech_condition FOREIGN KEY (tech_condition_id) REFERENCES dict_tech_state (id);
--ALTER TABLE object_rights_building_docs ADD CONSTRAINT fk_orbd_object_kind FOREIGN KEY (object_kind_id) REFERENCES dict_object_kind (id);
--ALTER TABLE object_rights_building_docs ADD CONSTRAINT fk_orbd_object_type FOREIGN KEY (object_type_id) REFERENCES dict_object_type (id);

/* org_docs */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[org_docs]') AND type in (N'U'))
--DROP TABLE [dbo].[org_docs]

--CREATE TABLE org_docs /* DOC_SORG */ (
--    organization_id   INTEGER NOT NULL, /* KOD_OBJ */
--    document_id       INTEGER NOT NULL, /* KOD_DOC */
--    link_kind_id      INTEGER,
--    modified_by       VARCHAR(128), /* not imported */
--    modify_date       DATE /* not imported */
--);

--ALTER TABLE org_docs ADD PRIMARY KEY (organization_id, document_id);

--ALTER TABLE org_docs ADD CONSTRAINT fk_org_docs_organization FOREIGN KEY (organization_id) REFERENCES organizations (id);
--ALTER TABLE org_docs ADD CONSTRAINT fk_org_docs_document FOREIGN KEY (document_id) REFERENCES documents (id);
--ALTER TABLE org_docs ADD CONSTRAINT fk_org_docs_link_kind FOREIGN KEY (link_kind_id) REFERENCES dict_org_doc_link_kind (id);

--GO

/******************************************************************************/
/*                        'Privatizacia' database                             */
/******************************************************************************/

/* dict_privat_state */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_privat_state]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_privat_state]

--CREATE TABLE dict_privat_state /* S_OZN_PRIV */ (
--    id                INTEGER NOT NULL, /* ID */
--    name              VARCHAR(50), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_privat_state ADD PRIMARY KEY (id);

/* dict_privat_obj_group */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_privat_obj_group]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_privat_obj_group]

--CREATE TABLE dict_privat_obj_group /* SKLGR */ (
--    id                INTEGER NOT NULL, /* KOD_KLGR */
--    name              VARCHAR(1), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_privat_obj_group ADD PRIMARY KEY (id);

/* dict_privat_kind */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_privat_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_privat_kind]

--CREATE TABLE dict_privat_kind /* SSPOSPRIV */ (
--    id                INTEGER NOT NULL, /* KOD_SPOSPRIV */
--    name              VARCHAR(20), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_privat_kind ADD PRIMARY KEY (id);

/* dict_privat_complex */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_privat_complex]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_privat_complex]

--CREATE TABLE dict_privat_complex /* SOZNOBJ */ (
--    id                INTEGER NOT NULL, /* KOD_OZNOBJ */
--    name              VARCHAR(50), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_privat_complex ADD PRIMARY KEY (id);

/* dict_privat_subordination */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_privat_subordination]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_privat_subordination]

--CREATE TABLE dict_privat_subordination /* SSFERUPR */ (
--    id                INTEGER NOT NULL, /* ID */
--    name              VARCHAR(255), /* FULLNAME */
--    code              VARCHAR(50), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_privat_subordination ADD PRIMARY KEY (id);

/* privatization */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[privatization]') AND type in (N'U'))
--DROP TABLE [dbo].[privatization]

--CREATE TABLE privatization /* PRIVOBJ */ (
--    id                INTEGER NOT NULL, /* ID */
--    building_id       INTEGER NOT NULL, /* ID_MOBJ */
--    organization_id   INTEGER, /* ID_ZKPO */
--    balans_id         INTEGER, /* BALID */
--    obj_group_id      INTEGER, /* KOD_KLGR */
--    privat_kind_id    INTEGER, /* KOD_SPOSPRIV */
--    privat_state_id   INTEGER, /* ID_OZN_PRIV */
--    sqr_total         NUMERIC(9,2), /* S_ZAG */
--    obj_name          VARCHAR(100), /* NAZVA */
--    complex_id        INTEGER, /* KOD_OZNOBJ - TABLE SOZNOBJ */
--    object_kind_id    INTEGER, /* KOD_VIDOBJ */
--    object_type_id    INTEGER, /* KOD_TIPOBJ */
--    purpose_group_id  INTEGER, /* KOD_GRNAZN */
--    history_id        INTEGER, /* KOD_ARHIT */
--    obj_floor         INTEGER, /* KOD_POVERH */
--    note              VARCHAR(100), /* MISCE */
--    cost              NUMERIC(15,2), /* CINA */
--    cost_expert       NUMERIC(15,2), /* EK_VART */
--    expert_date       DATE, /* EK_VARTD */
--    subordination_id  INTEGER, /* ID_SFERUPR - TABLE SSFERUPR */
--    rishen_doc_id     INTEGER, /**/
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE, /* DTISP */
--    addr_street_name  VARCHAR(100), /* NUL */
--	addr_number       VARCHAR(18), /* NOMER */
--	addr_misc         VARCHAR(30), /* ADRDOP */
--    addr_district_id  INTEGER, /* NEWDISTR */
--    addr_zip_code     VARCHAR(15) /* POSTIND */
--);

--ALTER TABLE privatization ADD PRIMARY KEY (id);

--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_building FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_org FOREIGN KEY (organization_id) REFERENCES organizations (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_obj_group FOREIGN KEY (obj_group_id) REFERENCES dict_privat_obj_group (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_privat_kind FOREIGN KEY (privat_kind_id) REFERENCES dict_privat_kind (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_privat_state FOREIGN KEY (privat_state_id) REFERENCES dict_privat_state (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_complex FOREIGN KEY (complex_id) REFERENCES dict_privat_complex (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_subordination FOREIGN KEY (subordination_id) REFERENCES dict_privat_subordination (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_obj_type FOREIGN KEY (object_type_id) REFERENCES dict_object_type (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_obj_kind FOREIGN KEY (object_kind_id) REFERENCES dict_object_kind (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_purpose_group FOREIGN KEY (purpose_group_id) REFERENCES dict_balans_purpose_group (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_history FOREIGN KEY (history_id) REFERENCES dict_history (id);
--ALTER TABLE privatization ADD CONSTRAINT fk_privatization_district FOREIGN KEY (addr_district_id) REFERENCES dict_districts2 (id);

/* priv_object_docs */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[priv_object_docs]') AND type in (N'U'))
--DROP TABLE [dbo].[priv_object_docs]

--CREATE TABLE priv_object_docs (
--    privatization_id  INTEGER NOT NULL,
--    master_doc_id     INTEGER,
--    document_id       INTEGER NOT NULL,
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE priv_object_docs ADD PRIMARY KEY (privatization_id, document_id);

--ALTER TABLE priv_object_docs ADD CONSTRAINT fk_priv_docs_privatization FOREIGN KEY (privatization_id) REFERENCES privatization (id);
--ALTER TABLE priv_object_docs ADD CONSTRAINT fk_priv_docs_document FOREIGN KEY (document_id) REFERENCES documents (id);

--GO

/******************************************************************************/
/*                        'Rosporadjennia' database                           */
/******************************************************************************/

/* dict_obj_rights */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_obj_rights]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_obj_rights]

--CREATE TABLE dict_obj_rights /* SPRAVO */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              VARCHAR(50), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_obj_rights ADD PRIMARY KEY (id);

/* dict_restriction_group */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_restriction_group]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_restriction_group]

--CREATE TABLE dict_restriction_group /* SGRRESTRICTIONS */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              CHAR(50), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_restriction_group ADD PRIMARY KEY (id);

/* dict_restriction */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_restriction]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_restriction]

--CREATE TABLE dict_restriction /* SRESTRICTION */ (
--    id                INTEGER NOT NULL, /* KOD */
--    group_id          INTEGER, /* GR */
--    name              VARCHAR(70), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_restriction ADD PRIMARY KEY (id);

/* restrictions */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[restrictions]') AND type in (N'U'))
--DROP TABLE [dbo].[restrictions]

--CREATE TABLE restrictions /* RESTRICTIONS */ (
--    id                INTEGER IDENTITY(1,1),
--    document_id       INTEGER NOT NULL, /* DOK_ID */
--    restriction_id    INTEGER NOT NULL, /* RESTRICTION */
--    restr_group_id    INTEGER NOT NULL, /* GR */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE /* DT */
--);

--ALTER TABLE restrictions ADD PRIMARY KEY (id);

--ALTER TABLE restrictions ADD CONSTRAINT fk_restrictions_doc FOREIGN KEY (document_id) REFERENCES documents (id);
--ALTER TABLE restrictions ADD CONSTRAINT fk_restrictions_restriction FOREIGN KEY (restriction_id) REFERENCES dict_restriction (id);
--ALTER TABLE restrictions ADD CONSTRAINT fk_restrictions_restr_group FOREIGN KEY (restr_group_id) REFERENCES dict_restriction_group (id);

/* object_rights */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[object_rights]') AND type in (N'U'))
--DROP TABLE [dbo].[object_rights]

--CREATE TABLE object_rights /* OBJECT_PRAVO */ (
--    id                INTEGER IDENTITY(1,1), /* not imported */
--    building_id       INTEGER NOT NULL, /* OBJECT_KOD */
--    org_from_id       INTEGER, /* ZKPO */
--    org_to_id         INTEGER, /* ZKPO */
--    right_id          INTEGER, /* PRAVO */
--    transfer_date     DATE, /* START_DATE or END_DATE*/
--    transfer_year     AS YEAR(transfer_date),
--    transfer_quarter  AS DATEPART(Quarter, transfer_date),
--    akt_id            INTEGER, /* START_DOK or END_DOK */
--    rozp_id           INTEGER, /* Calculated during migration, not imported */
--    modified_by       VARCHAR(128), /* ISP */
--    modify_date       DATE, /* DT */

--    /* The following properties are imported from the OBJECT_PRAVO_PROPERTIES table */
    
--    name              VARCHAR(255), /* NAME */
--    characteristic    VARCHAR(MAX), /* CHARACTERISTIC */
--    misc_info         VARCHAR(MAX), /* ARRANGEMENT */
--    sum_balans        NUMERIC(15,3), /* SUMMA_BALANS */
--    sum_zalishkova    NUMERIC(15,3), /* SUMMA_ZAL */
--    sqr_transferred   NUMERIC(9,3), /* SQUARE */
--    len_transferred   NUMERIC(9,2) /* LEN */
--);

--ALTER TABLE object_rights ADD PRIMARY KEY (id);

--ALTER TABLE object_rights ADD CONSTRAINT fk_obj_rights_building FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE object_rights ADD CONSTRAINT fk_obj_rights_org_from FOREIGN KEY (org_from_id) REFERENCES organizations (id);
--ALTER TABLE object_rights ADD CONSTRAINT fk_obj_rights_org_to FOREIGN KEY (org_to_id) REFERENCES organizations (id);
--ALTER TABLE object_rights ADD CONSTRAINT fk_obj_rights_right FOREIGN KEY (right_id) REFERENCES dict_obj_rights (id);
--ALTER TABLE object_rights ADD CONSTRAINT fk_obj_rights_akt_doc FOREIGN KEY (akt_id) REFERENCES documents (id);
--ALTER TABLE object_rights ADD CONSTRAINT fk_obj_rights_rozp_doc FOREIGN KEY (rozp_id) REFERENCES documents (id);

/******************************************************************************/
/*                    Financial reporting (Balans database)                   */
/******************************************************************************/

/* dict_fin_report_type */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_fin_report_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_fin_report_type]

--CREATE TABLE dict_fin_report_type /* TYPE_ZVITNOST */ (
--    id                INTEGER NOT NULL, /* KOD_TYPE_ZVITNOST */
--    name              VARCHAR(30), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_fin_report_type ADD PRIMARY KEY (id);

/* dict_fin_form_type */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_fin_form_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_fin_form_type]

--CREATE TABLE dict_fin_form_type /* S_TYPE_FORM */ (
--    id                INTEGER NOT NULL, /* KOD_TYPE_FORM */
--    name              VARCHAR(30), /* NAME_TYPE_FORM */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_fin_form_type ADD PRIMARY KEY (id);

/* dict_fin_form_kind */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_fin_form_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_fin_form_kind]

--CREATE TABLE dict_fin_form_kind /* S_PRIZNAKFORM */ (
--    id                INTEGER NOT NULL, /* PRIZNAKFORMID */
--    name              VARCHAR(30), /* NAME */
--    modified_by       VARCHAR(128),
--    modify_date       DATE
--);

--ALTER TABLE dict_fin_form_kind ADD PRIMARY KEY (id);

/* fin_report_periods */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_report_periods]') AND type in (N'U'))
--DROP TABLE [dbo].[fin_report_periods]

--CREATE TABLE fin_report_periods /* DANPER */ (
--    id                INTEGER NOT NULL, /* KOD_DANPER */
--    name              VARCHAR(30), /* PERIOD */
--    period_start      DATE, /* NACHALO */
--    modified_by       VARCHAR(128), /* USER_KOREG */
--    modify_date       DATE, /* DATE_KOREG */
--    is_fin_plan       INTEGER, /* FIN_PLAN */
--    parent_id         INTEGER, /* PARENT_KOD */
--    is_deleted        INTEGER /* IS_DEL */
--);

--ALTER TABLE fin_report_periods ADD PRIMARY KEY (id);

/* fin_report_forms */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_report_forms]') AND type in (N'U'))
--DROP TABLE [dbo].[fin_report_forms]

--CREATE TABLE fin_report_forms /* S_FORM */ (
--    id                 INTEGER NOT NULL, /* KOD_FORM */
--    is_active          VARCHAR(1), /* PRIZ_ACTIVE */
--    report_type_id     INTEGER NOT NULL, /* KOD_TYPE_ZVITNOST */
--    form_type_id       INTEGER NOT NULL, /* KOD_TYPE_FORM */
--    form_name          VARCHAR(200), /* NAME_FORM */
--    modified_by        VARCHAR(128), /* USER_KOREG */
--    modify_date        DATE, /* DATE_KOREG */
--    form_number        VARCHAR(20), /* NOMER_FORM */
--    file_name1         VARCHAR(250), /* FILE_NAME */
--    period_id          INTEGER NOT NULL, /* KOD_DANPER */
--    form_kind_id       INTEGER DEFAULT 1 NOT NULL, /* PRIZNAK_BALANS */
--    file_name2         VARCHAR(50), /* FILE_NAME2 */
--    num_fixed_cols     INTEGER, /* FIXCOLS */
--    num_fixed_rows     INTEGER /* FIXROWS */
--);

--ALTER TABLE fin_report_forms ADD PRIMARY KEY (id);

--ALTER TABLE fin_report_forms ADD CONSTRAINT fk_fin_form_kind FOREIGN KEY (form_kind_id) REFERENCES dict_fin_form_kind (id);
--ALTER TABLE fin_report_forms ADD CONSTRAINT fk_fin_form_type FOREIGN KEY (form_type_id) REFERENCES dict_fin_form_type (id);
--ALTER TABLE fin_report_forms ADD CONSTRAINT fk_fin_form_report_type FOREIGN KEY (report_type_id) REFERENCES dict_fin_report_type (id);
--ALTER TABLE fin_report_forms ADD CONSTRAINT fk_fin_form_period FOREIGN KEY (period_id) REFERENCES fin_report_periods (id);

/* fin_report_rows */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_report_rows]') AND type in (N'U'))
--DROP TABLE [dbo].[fin_report_rows]

--CREATE TABLE fin_report_rows /* S_ROW */ (
--    id             INTEGER IDENTITY(1,1), /* not imported */
--    row_index      INTEGER NOT NULL, /* NOMER_ROW */
--    form_id        INTEGER NOT NULL, /* KOD_FORM */
--    is_disabled    INTEGER, /* KOD_PRIZN */
--    modified_by    VARCHAR(128), /* USER_KOREG */
--    modify_date    DATE, /* DATE_KOREG */
--    name           VARCHAR(150) /* NAME_OUTPUT */
--);

--ALTER TABLE fin_report_rows ADD PRIMARY KEY (id);

--ALTER TABLE fin_report_rows ADD CONSTRAINT fk_fin_row_form FOREIGN KEY (form_id) REFERENCES fin_report_forms (id);

/* fin_report_columns */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_report_columns]') AND type in (N'U'))
--DROP TABLE [dbo].[fin_report_columns]

--CREATE TABLE fin_report_columns /* S_COLUMN */ (
--    id             INTEGER IDENTITY(1,1), /* not imported */
--    col_index      INTEGER NOT NULL, /* NOMER_COL */
--    form_id        INTEGER NOT NULL, /* KOD_FORM */
--    is_disabled    INTEGER, /* KOD_PRIZN */
--    modified_by    VARCHAR(128), /* USER_KOREG */
--    modify_date    DATE, /* DATE_KOREG */
--);

--ALTER TABLE fin_report_columns ADD PRIMARY KEY (id);

--ALTER TABLE fin_report_columns ADD CONSTRAINT fk_fin_col_form FOREIGN KEY (form_id) REFERENCES fin_report_forms (id);

/* fin_report_cells */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_report_cells]') AND type in (N'U'))
--DROP TABLE [dbo].[fin_report_cells]

--CREATE TABLE fin_report_cells /* DISABLE_CELL */ (
--    id             INTEGER IDENTITY(1,1), /* not imported */
--    form_id        INTEGER NOT NULL, /* KOD_FORM */
--    row_index      INTEGER NOT NULL, /* NOMER_ROW */
--    col_index      INTEGER NOT NULL, /* NOMER_COL */
--    is_disabled    INTEGER NOT NULL, /* KOD_PRIZN */
--    cell_text      VARCHAR(100) /* NAME_CELL */
--);

--ALTER TABLE fin_report_cells ADD PRIMARY KEY (id);

--ALTER TABLE fin_report_cells ADD CONSTRAINT fk_fin_cell_form FOREIGN KEY (form_id) REFERENCES fin_report_forms (id);

/* fin_report_formulae */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_report_formulae]') AND type in (N'U'))
--DROP TABLE [dbo].[fin_report_formulae]

--CREATE TABLE fin_report_formulae /* REP_FORMULA */ (
--    id                INTEGER NOT NULL, /* RFORMUALID */
--    period_id         INTEGER, /* KOD_DANPER */
--    name              VARCHAR(255), /* RFORMULA_NAME */
--    formula           VARCHAR(1000), /* RFORMULALINE */
--    modified_by       VARCHAR(128),
--    modify_date       DATE /* DATE_KOREG */
--);

--ALTER TABLE fin_report_formulae ADD PRIMARY KEY (id);

--ALTER TABLE fin_report_formulae ADD CONSTRAINT fk_fin_formulae_period FOREIGN KEY (period_id) REFERENCES fin_report_periods (id);

/* fin_report_values */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_report_values]') AND type in (N'U'))
--DROP TABLE [dbo].[fin_report_values]

--CREATE TABLE fin_report_values /* ZNACHENIE */ (
--    id                INTEGER IDENTITY(1,1), /* not imported */
--    organization_id   INTEGER, /* KOD_OBJ */
--    form_id           INTEGER, /* KOD_FORM */
--    period_id         INTEGER, /* KOD_DANPER */
--    num_row           INTEGER, /* NOMER_ROW */
--    num_col           INTEGER, /* NOMER_COL */
--    expense_code      INTEGER, /* KOD_KLASIFIK */
--    value             NUMERIC(18,4) /* ZNACHENIE */
--);

--ALTER TABLE fin_report_values ADD PRIMARY KEY (id);

--ALTER TABLE fin_report_values ADD CONSTRAINT fk_fin_values_org FOREIGN KEY (organization_id) REFERENCES organizations (id);
--ALTER TABLE fin_report_values ADD CONSTRAINT fk_fin_values_period FOREIGN KEY (period_id) REFERENCES fin_report_periods (id);

/* fin_report_formula_values */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_report_formula_values]') AND type in (N'U'))
--DROP TABLE [dbo].[fin_report_formula_values]

--CREATE TABLE fin_report_formula_values (
--    id                INTEGER IDENTITY(1,1),
--    organization_id   INTEGER,
--    formula_id        INTEGER,
--    period_id         INTEGER,
--    value             NUMERIC(15,2)
--);

--ALTER TABLE fin_report_formula_values ADD PRIMARY KEY (id);

--ALTER TABLE fin_report_formula_values ADD CONSTRAINT fk_fin_formula_values_org FOREIGN KEY (organization_id) REFERENCES organizations (id);
--ALTER TABLE fin_report_formula_values ADD CONSTRAINT fk_fin_formula_values_formula FOREIGN KEY (formula_id) REFERENCES fin_report_formulae (id);
--ALTER TABLE fin_report_formula_values ADD CONSTRAINT fk_fin_formula_values_period FOREIGN KEY (period_id) REFERENCES fin_report_periods (id);

/* fin_form_changes */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_form_changes]') AND type in (N'U'))
--DROP TABLE [dbo].[fin_form_changes]

--CREATE TABLE fin_form_changes /* ZMIN */ (
--    id                 INTEGER IDENTITY(1,1), /* KOD_ZM - We do not need it; use an IDENTITY instead */
--    change_date        DATE, /* DATE_ZM */
--    order_num          VARCHAR(20), /* NOMER_NAKAZ */
--    modified_by        VARCHAR(128), /* USER_KOREG */
--    modify_date        DATE, /* DATE_KOREG */
--    form_id            INTEGER, /* KOD_FORM */
--    organization_id    INTEGER, /* KOD_OBJ */
--    note               VARCHAR(126), /* PRIM */
--    is_active          INTEGER, /* ACTIVATE */
--    form_status        INTEGER, /* STATUS */
--    save_date          DATE /* DATE_SAVE */
--);

--ALTER TABLE fin_form_changes ADD PRIMARY KEY (id);

--ALTER TABLE fin_form_changes ADD CONSTRAINT fk_fin_form_changes_form FOREIGN KEY (form_id) REFERENCES fin_report_forms (id);
--ALTER TABLE fin_form_changes ADD CONSTRAINT fk_fin_form_changes_org FOREIGN KEY (organization_id) REFERENCES organizations (id);

/* fin_budget_rate_by_industry */

IF OBJECT_ID('fin_budget_rate_by_industry') IS NULL
BEGIN
	CREATE TABLE fin_budget_rate_by_industry (
		id                 INTEGER IDENTITY(1,1),
		period_id          INTEGER,
		old_industry_id    INTEGER,
		old_occupation_id  INTEGER DEFAULT 0,
		payments_rate      NUMERIC(5,2)
	);

	ALTER TABLE fin_budget_rate_by_industry ADD PRIMARY KEY (id);

	ALTER TABLE fin_budget_rate_by_industry ADD CONSTRAINT fk_budget_rate_by_ind_period FOREIGN KEY (period_id) REFERENCES fin_report_periods (id);
	ALTER TABLE fin_budget_rate_by_industry ADD CONSTRAINT fk_budget_rate_by_ind_org FOREIGN KEY (old_industry_id) REFERENCES dict_org_old_industry (id);
	ALTER TABLE fin_budget_rate_by_industry ADD CONSTRAINT fk_budget_rate_by_ind_occ FOREIGN KEY (old_occupation_id) REFERENCES dict_org_old_occupation (id);
END

/* fin_budget_rate_by_org */

IF OBJECT_ID('fin_budget_rate_by_org') IS NULL
BEGIN
	CREATE TABLE fin_budget_rate_by_org (
		id                 INTEGER IDENTITY(1,1),
		period_id          INTEGER,
		organization_id    INTEGER,
		payments_rate      NUMERIC(5,2)
	);

	ALTER TABLE fin_budget_rate_by_org ADD PRIMARY KEY (id);

	ALTER TABLE fin_budget_rate_by_org ADD CONSTRAINT fk_budget_rate_by_org_period FOREIGN KEY (period_id) REFERENCES fin_report_periods (id);
	ALTER TABLE fin_budget_rate_by_org ADD CONSTRAINT fk_budget_rate_by_org_org FOREIGN KEY (organization_id) REFERENCES organizations (id);
END

/* fin_budget_formula */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_budget_formula]') AND type in (N'U'))
--DROP TABLE [dbo].[fin_budget_formula]

--CREATE TABLE fin_budget_formula (
--    formula_id         INTEGER NOT NULL
--);

--ALTER TABLE fin_budget_formula ADD PRIMARY KEY (formula_id);

/******************************************************************************/
/*                        'Orendna Plata' database                            */
/******************************************************************************/

/* dict_rent_note */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_rent_note]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_rent_note]

--CREATE TABLE dict_rent_note /* ZAV_S_PRIMITKA */ (
--    id                      INTEGER NOT NULL, /* ID */
--    name                    VARCHAR(255) /* NAZVA */
--);

--ALTER TABLE dict_rent_note ADD PRIMARY KEY (id);

/* dict_rent_dkk */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_rent_dkk]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_rent_dkk]

--CREATE TABLE dict_rent_dkk /* ZAV_DKK */ (
--    id                      INTEGER NOT NULL, /* ID */
--    name                    VARCHAR(255), /* FULL_NAME */
--    short_name              VARCHAR(16) /* SHORT_NAME */
--);

--ALTER TABLE dict_rent_dkk ADD PRIMARY KEY (id);

/* dict_rent_payment_type */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_rent_payment_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_rent_payment_type]

--CREATE TABLE dict_rent_payment_type /* ZAV_PLATA */ (
--    id                      INTEGER NOT NULL, /* ID */
--    name                    VARCHAR(100) /* PLATA */
--);

--ALTER TABLE dict_rent_payment_type ADD PRIMARY KEY (id);

/* dict_rent_occupation */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_rent_occupation]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_rent_occupation]

--CREATE TABLE dict_rent_occupation /* ZAV_S_VID_DIAL */ (
--    id                      INTEGER NOT NULL, /* VID_DIAL_ID */
--    name                    VARCHAR(255), /* VID_DIAL_NAME */
--);

--ALTER TABLE dict_rent_occupation ADD PRIMARY KEY (id);

/* rent_balans_org */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rent_balans_org]') AND type in (N'U'))
--DROP TABLE [dbo].[rent_balans_org]

--CREATE TABLE rent_balans_org /* ZAV_BALANS_ORG */ (
--    organization_id         INTEGER NOT NULL, /* KOD_OBJ */
--    rent_occupation_id      INTEGER, /* VID_DIAL_ID */
--    rent_dkk_id             INTEGER /* ID_DKK */,
--    p_energo_kanal          INTEGER DEFAULT 0, /* P_ENERGO_KANAL */
--    p_zvit                  INTEGER DEFAULT 1 /* P_ZVIT */
--);

--ALTER TABLE rent_balans_org ADD PRIMARY KEY (organization_id);

--ALTER TABLE rent_balans_org ADD CONSTRAINT fk_rent_balans_org_org FOREIGN KEY (organization_id) REFERENCES organizations (id);
--ALTER TABLE rent_balans_org ADD CONSTRAINT fk_rent_balans_org_occupation FOREIGN KEY (rent_occupation_id) REFERENCES dict_rent_occupation (id);
--ALTER TABLE rent_balans_org ADD CONSTRAINT fk_rent_balans_org_dkk FOREIGN KEY (rent_dkk_id) REFERENCES dict_rent_dkk (id);

/* rent_renter_org */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rent_renter_org]') AND type in (N'U'))
--DROP TABLE [dbo].[rent_renter_org]

--CREATE TABLE rent_renter_org /* ZAV_ORENDARI_OR_PLATA */ (
--    id                      INTEGER NOT NULL, /* ORENDARI_OR_PLATA_ID */
--    name                    VARCHAR(255), /* NAME */
--    zkpo_code               VARCHAR(64), /* EDRPOU */
--    organization_id         INTEGER
--);

--ALTER TABLE rent_renter_org ADD PRIMARY KEY (id);

--ALTER TABLE rent_renter_org ADD CONSTRAINT fk_rent_renter_org_org FOREIGN KEY (organization_id) REFERENCES organizations (id);

/* rent_payment */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rent_payment]') AND type in (N'U'))
--DROP TABLE [dbo].[rent_payment]

--CREATE TABLE rent_payment /* ZAVANTAZH_OR_PLATA */ (
--    id                      INTEGER NOT NULL, /* ZAVANTAZH_ID */
--    org_balans_id           INTEGER, /* KOD_BALANS_OR_PLATA */
--    rent_period_id          INTEGER, /* S_PERIOD_OR_PLATA_ID */
--    sqr_balans              NUMERIC(15,3), /* SQUARE_BALANS */
--    payment_zvit_uah        NUMERIC(15,3), /* PLATA_ZVIT_UAH */
--    received_zvit_uah       NUMERIC(15,3), /* OTRYM_ZVIT_UAH */
--    received_nar_zvit_uah   NUMERIC(15,3), /* OTRYM_NAR_ZVIT_UAH */
--    debt_total_uah          NUMERIC(15,3), /* BORG_ZAG_UAH */
--    debt_zvit_uah           NUMERIC(15,3), /* BORG_ZVIT_UAH */
--    budget_narah_50_uah     NUMERIC(15,3), /* BUD_NAR_50_UAH */
--    budget_zvit_50_uah      NUMERIC(15,3), /* BUD_ZVIT_50_UAH */
--    budget_prev_50_uah      NUMERIC(15,3), /* BUD_POPER_50_UAH */
--    budget_debt_50_uah      NUMERIC(15,3), /* BUD_BORG_50_UAH */
--    budget_debt_30_50_uah   NUMERIC(15,3), /* BUD_BORG_30_50_UAH */
--    sqr_cmk                 NUMERIC(15,3), /* SQUARE_CMK */
--    budget_narah_50_cmk_uah NUMERIC(15,3), /* BUD_NAR_50_CMK_UAH */
--    budget_zvit_50_cmk_uah  NUMERIC(15,3), /* BUD_ZVIT_50_CMK_UAH */
--    budget_debt_50_cmk_uah  NUMERIC(15,3), /* BUD_BORG_50_CMK_UAH */
--    budget_zvit_50_in_uah   NUMERIC(15,3), /* BUD_ZVIT_50_IN_UAH */
--    zkpo_code               VARCHAR(255), /* KOD_ZKPO */
--    modify_date             DATE, /* DT */
--    modified_by             VARCHAR(255), /* ISP */
--    email                   VARCHAR(255), /* MAIL */
--    phone                   VARCHAR(25), /* TELL */
--    responsible_fio         VARCHAR(255), /* VIDPOV */
--    director_fio            VARCHAR(255), /* DIRECTOR */
--    buhgalter_fio           VARCHAR(255), /* BUH */
--    num_agreements          INTEGER, /* KOL_DOG */
--    num_renters             INTEGER, /* KOL_ORENDAR */
--    debt_v_mezhah_vitrat    NUMERIC(15,3), /* BORG_VITRAT_NA_UTRIMAN */
--    budget_zvit_50_old      NUMERIC(15,3) DEFAULT 0, /* BUD_ZVIT_50_UAH_OLD */
--    post_address            VARCHAR(512), /* POST_ADDRESS */
--    pererahunok_50_b        NUMERIC(15,3) /* PERERAH_50_B */
--);

--ALTER TABLE rent_payment ADD PRIMARY KEY (id);

--ALTER TABLE rent_payment ADD CONSTRAINT fk_rent_payment_org_balans FOREIGN KEY (org_balans_id) REFERENCES organizations (id);
--ALTER TABLE rent_payment ADD CONSTRAINT fk_rent_payment_period FOREIGN KEY (rent_period_id) REFERENCES dict_rent_period (id);

/* rent_object */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rent_object]') AND type in (N'U'))
--DROP TABLE [dbo].[rent_object]

--CREATE TABLE rent_object /* ZAV_DANI_OBJ_OR_PLATA */ (
--    id                      INTEGER IDENTITY(1,1), /* ZAV_DANI_OBJ_OR_PLATA_ID - not imported */
--    building_id             INTEGER,
--    rent_payment_id         INTEGER, /* ZAVANTAZH_ID */
--    rent_period_id          INTEGER, /* S_PERIOD_OR_PLATA_ID */
--    addr_street             VARCHAR(255), /* OBJ_VUL */
--    addr_street_type        VARCHAR(64), /* OBJ_TYPE */
--    addr_num                VARCHAR(255), /* OBJ_NUM */
--    sqr_total               NUMERIC(15,3), /* ZAG_PLOSH */
--    sqr_rented              NUMERIC(15,3), /* ZAG_PLOSH_OREND */
--    sqr_free                NUMERIC(15,3), /* ZAG_PLOSH_VIL */
--    sqr_korysna             NUMERIC(15,3), /* PLOSH_VIL_KOR */
--    sqr_mzk                 NUMERIC(15,3), /* PLOSH_VIL_MZK */
--    floors                  VARCHAR(255), /* POVERH */
--    tech_state_id           INTEGER, /* STAN */
--    is_energo               INTEGER, /* ENERGO */
--    is_vodo                 INTEGER, /* VODO */
--    is_teplo                INTEGER, /* TEPLO */
--    district_id             INTEGER, /* KOD_RAYON */
--    purpose                 VARCHAR(255), /* VYKORISTANNJA */
--    rent_note_id            INTEGER /* PRYMITKA */
--);

--ALTER TABLE rent_object ADD PRIMARY KEY (id);

--ALTER TABLE rent_object ADD CONSTRAINT fk_rent_object_building FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE rent_object ADD CONSTRAINT fk_rent_object_payment FOREIGN KEY (rent_payment_id) REFERENCES rent_payment (id);
--ALTER TABLE rent_object ADD CONSTRAINT fk_rent_object_period FOREIGN KEY (rent_period_id) REFERENCES dict_rent_period (id);
--ALTER TABLE rent_object ADD CONSTRAINT fk_rent_object_district FOREIGN KEY (district_id) REFERENCES dict_districts2 (id);
--ALTER TABLE rent_object ADD CONSTRAINT fk_rent_object_note FOREIGN KEY (rent_note_id) REFERENCES dict_rent_note (id);

/* rent_payment_by_renter */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rent_payment_by_renter]') AND type in (N'U'))
--DROP TABLE [dbo].[rent_payment_by_renter]

--CREATE TABLE rent_payment_by_renter /* ZAV_DANI_OR_PLATA */ (
--    id                      INTEGER IDENTITY(1,1), /* DANI_OR_PLATA_ID - not imported */
--    rent_renter_org_id      INTEGER, /* ORENDARI_OR_PLATA_ID */
--    sqr_total_rent          NUMERIC(15,3), /* SQUARE_ORENDA */
--    sqr_payed_by_percent    NUMERIC(15,3), /* SQUARE_PLATA_VIDS_VART */
--    sqr_payed_by_1uah       NUMERIC(15,3), /* SQUARE_PLATA_1_UAH */
--    sqr_payed_hourly        NUMERIC(15,3), /* SQUARE_HOUR */
--    payment_narah           NUMERIC(15,3), /* PAY_NAR_UAH */
--    payment_received        NUMERIC(15,3), /* PAY_OTR_UAH */
--    payment_budget_50_uah   NUMERIC(15,3), /* BUD_50_UAH */
--    rent_payment_id         INTEGER, /* ZAVANTAZH_ID */
--    renter_organization_id  INTEGER, /* KOD_OREND_SORG_1NF */
--    rent_period_id          INTEGER, /* S_PERIOD_OR_PLATA_ID */
--    giver_organization_id   INTEGER, /* ORNDODAV_ID */
--    agreement_flag          INTEGER DEFAULT 0 /* PRIZNAK_DOG */,
--    payment_nar_zvit        NUMERIC(15,3) /* PAY_NAR_ZVIT */
--);

--ALTER TABLE rent_payment_by_renter ADD PRIMARY KEY (id);

--ALTER TABLE rent_payment_by_renter ADD CONSTRAINT fk_payment_by_renter_payment FOREIGN KEY (rent_payment_id) REFERENCES rent_payment (id);
--ALTER TABLE rent_payment_by_renter ADD CONSTRAINT fk_payment_by_renter_period FOREIGN KEY (rent_period_id) REFERENCES dict_rent_period (id);
--ALTER TABLE rent_payment_by_renter ADD CONSTRAINT fk_payment_by_renter_org_renter FOREIGN KEY (renter_organization_id) REFERENCES organizations (id);
--ALTER TABLE rent_payment_by_renter ADD CONSTRAINT fk_payment_by_renter_org_giver FOREIGN KEY (giver_organization_id) REFERENCES organizations (id);
--ALTER TABLE rent_payment_by_renter ADD CONSTRAINT fk_payment_by_renter_renter FOREIGN KEY (rent_renter_org_id) REFERENCES rent_renter_org (id);

/* rent_payment_debt */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rent_payment_debt]') AND type in (N'U'))
--DROP TABLE [dbo].[rent_payment_debt]

--CREATE TABLE rent_payment_debt /* ZAV_DANI_BORG_OR_PLATA */ (
--    id                      INTEGER IDENTITY(1,1), /* DANI_BORG_OR_PLATA_ID - not imported */
--    rent_payment_id         INTEGER, /* ZAVANTAZH_ID */
--    rent_renter_org_id      INTEGER, /* ORENDARI_OR_PLATA_ID */
--    rent_period_id          INTEGER, /* S_PERIOD_OR_PLATA_ID */
--    giver_organization_id   INTEGER, /* ORNDODAV_ID */
--    debt_total              NUMERIC(15,3), /* BORG_ALL_UAH */
--    debt_3_month            NUMERIC(15,3), /* BORG_3_UAH */
--    debt_12_month           NUMERIC(15,3), /* BORG_3_36_UAH */
--    debt_3_years            NUMERIC(15,3), /* BORG_3_36_UAH2 */
--    debt_over_3_years       NUMERIC(15,3), /* BORG_3_YEAR_UAH */
--    debt_spysano            NUMERIC(15,3), /* BORG_SPYSANO_UAH */
--    num_zahodiv_total       INTEGER DEFAULT 0, /* QUAN_ZAH */
--    num_zahodiv_zvit        INTEGER, /* QUAN_ZAH_ZVIT */
--    num_pozov_total         INTEGER DEFAULT 0, /* QUAN_POZOV */
--    num_pozov_zvit          INTEGER, /* QUAN_POZOV_ZVIT */
--    num_pozov_zadov_total   INTEGER DEFAULT 0, /* QUAN_POZOV_DOZV */
--    num_pozov_zadov_zvit    INTEGER, /* QUAN_POZOV_DOZV_ZVIT */
--    num_pozov_vikon_total   INTEGER, /* QUAN_POZOV_VIK */
--    num_pozov_vikon_zvit    INTEGER, /* QUAN_POZOV_VIK_ZVIT */
--    debt_pogasheno_total    NUMERIC(15,3), /* BORG_POGASH_UAH */
--    debt_pogasheno_zvit     NUMERIC(15,3), /* BORG_POGASH_ZVIT_UAH */
--    agreement_flag          INTEGER DEFAULT 0, /* PRIZNAK_DOG */
--    debt_v_mezhah_vitrat    NUMERIC(15,3) /* BORG_VITRAT_NA_UTRIMAN */
--);

--ALTER TABLE rent_payment_debt ADD PRIMARY KEY (id);

--ALTER TABLE rent_payment_debt ADD CONSTRAINT fk_payment_debt_payment FOREIGN KEY (rent_payment_id) REFERENCES rent_payment (id);
--ALTER TABLE rent_payment_debt ADD CONSTRAINT fk_payment_debt_renter FOREIGN KEY (rent_renter_org_id) REFERENCES rent_renter_org (id);
--ALTER TABLE rent_payment_debt ADD CONSTRAINT fk_payment_debt_period FOREIGN KEY (rent_period_id) REFERENCES dict_rent_period (id);
--ALTER TABLE rent_payment_debt ADD CONSTRAINT fk_payment_debt_org_giver FOREIGN KEY (giver_organization_id) REFERENCES organizations (id);

/* rent_vipiski */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rent_vipiski]') AND type in (N'U'))
--DROP TABLE [dbo].[rent_vipiski]

--CREATE TABLE rent_vipiski /* ZAV_KAZNA */ (
--    id                      INTEGER IDENTITY(1,1), /* ID - not imported */
--    zkpo_code               VARCHAR(13), /* KOD_INN */
--    payment_date            DATE, /* DT */
--    payment_sum             NUMERIC(15,3), /* SUMMA */
--    rent_payment_type_id    INTEGER, /* ID_PLATA */
--    org_balans_id           INTEGER /* KOD_BALANS */,
--    organization_id         INTEGER /* OBJ */
--);

--ALTER TABLE rent_vipiski ADD PRIMARY KEY (id);

--ALTER TABLE rent_vipiski ADD CONSTRAINT fk_rent_vipiski_org_balans FOREIGN KEY (org_balans_id) REFERENCES organizations (id);
--ALTER TABLE rent_vipiski ADD CONSTRAINT fk_rent_vipiski_org FOREIGN KEY (organization_id) REFERENCES organizations (id);
--ALTER TABLE rent_vipiski ADD CONSTRAINT fk_rent_vipiski_payment_type FOREIGN KEY (rent_payment_type_id) REFERENCES dict_rent_payment_type (id);

/* rent_free_square */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rent_free_square]') AND type in (N'U'))
--DROP TABLE [dbo].[rent_free_square]

--CREATE TABLE rent_free_square (
--    id                      INTEGER IDENTITY(1,1),
--    building_id             INTEGER,
--    organization_id         INTEGER,
--    rent_period_id          INTEGER,
--    sqr_free_total          NUMERIC(15,3),
--    sqr_free_korysna        NUMERIC(15,3),
--    sqr_free_mzk            NUMERIC(15,3),
--    free_sqr_floors         VARCHAR(MAX),
--    free_sqr_purpose        VARCHAR(MAX)
--);

--ALTER TABLE rent_free_square ADD PRIMARY KEY (id);

--ALTER TABLE rent_free_square ADD CONSTRAINT fk_rent_free_square_building FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE rent_free_square ADD CONSTRAINT fk_rent_free_square_org FOREIGN KEY (organization_id) REFERENCES organizations (id);
--ALTER TABLE rent_free_square ADD CONSTRAINT fk_rent_free_square_period FOREIGN KEY (rent_period_id) REFERENCES dict_rent_period (id);

--GO

/******************************************************************************/
/*                      'Nezalezhna Otsinka' database                         */
/******************************************************************************/

/* dict_expert_obj_type */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_expert_obj_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_expert_obj_type]

--CREATE TABLE dict_expert_obj_type /* S_VID_OB_EXP_OC */ (
--    id                      INTEGER NOT NULL, /* S_VID_OB_EXP_OC_ID */
--    name                    VARCHAR(100), /* NAME */
--    short_name              VARCHAR(50) /* NAME_SHORT */
--);

--ALTER TABLE dict_expert_obj_type ADD PRIMARY KEY (id);

/* dict_expert_rezenz */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_expert_rezenz]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_expert_rezenz]

--CREATE TABLE dict_expert_rezenz /* S_REZENZ */ (
--    id                      INTEGER NOT NULL, /* S_REZENZ_ID */
--    name                    VARCHAR(100), /* NAME */
--    is_deleted              INTEGER DEFAULT 0 /* DELETED */
--);

--ALTER TABLE dict_expert_rezenz ADD PRIMARY KEY (id);

/* dict_expert_korr */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_expert_korr]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_expert_korr]

--CREATE TABLE dict_expert_korr /* S_KORESPONDENT */ (
--    id                      INTEGER NOT NULL, /* S_KORESPONDENT_ID */
--    name                    VARCHAR(100) /* NAME */
--);

--ALTER TABLE dict_expert_korr ADD PRIMARY KEY (id);

/* dict_expert_rezenz_type */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_expert_rezenz_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_expert_rezenz_type]

--CREATE TABLE dict_expert_rezenz_type /* S_VID_REZENZ */ (
--    id                      INTEGER NOT NULL, /* S_VID_REZENZ_ID */
--    name                    VARCHAR(100), /* NAME */
--    color_rgb               INTEGER /* COLOR */
--);

--ALTER TABLE dict_expert_rezenz_type ADD PRIMARY KEY (id);

/* dict_expert */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_expert]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_expert]

--CREATE TABLE dict_expert /* S_EXPERT */ (
--    id                      INTEGER NOT NULL, /* KOD */
--    full_name               VARCHAR(250), /* FULL_NAME */
--    short_name              VARCHAR(100), /* SHORT_NAME */
--    modified_by             VARCHAR(128) /* ISP */,
--    modify_date             DATE /* DT */,
--    fio_boss                VARCHAR(100), /* FIO_BOSS */
--    tel_boss                VARCHAR(80), /* TEL_BOSS */
--    addr_district_id        INTEGER /* KOD_RAYON2 */,
--    addr_street_id          INTEGER /* ULKOD */,
--    addr_zip_code           VARCHAR(20), /* POST_INDEX */
--    addr_number             VARCHAR(60), /* NOMER_DOMA */
--    certificate_num         VARCHAR(15), /* SERTIFIKAT_NOM */
--    certificate_date        DATE /* SERTIFIKAT_DT */,
--    certificate_end_date    DATE /* SERTIFIKAT_FINISH_DT */,
--    case_num                INTEGER, /* SPRAVA_NOM */
--    addr_full               VARCHAR(255), /* FULL_ADDRESS */
--    note                    VARCHAR(255), /* PRIMITKA */
--    is_deleted              INTEGER DEFAULT 0 /* DELETED */,
--    del_date                DATE /* DTDELETE */
--);

--ALTER TABLE dict_expert ADD PRIMARY KEY (id);

--ALTER TABLE dict_expert ADD CONSTRAINT fk_dict_expert_district FOREIGN KEY (addr_district_id) REFERENCES dict_districts2 (id);

/* expert_note */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[expert_note]') AND type in (N'U'))
--DROP TABLE [dbo].[expert_note]

--CREATE TABLE expert_note /* NO_PRIM_EXP */ (
--    id                      INTEGER NOT NULL, /* NO_PRIM_EXP_ID */
--    org_renter_id           INTEGER, /* ORG_AR */
--    org_renter_name         VARCHAR(255), /* ORG_AR_NAME */
--    org_balans_id           INTEGER, /* ORG_BAL */
--    org_balans_name         VARCHAR(255), /* ORG_BAL_NAME */
--    building_id             INTEGER, /* OBJECT */
--    balans_id               INTEGER, /* BALID */
--    addr_district_id        INTEGER, /* KOD_RAYON2 */
--    expert_obj_type_id      INTEGER, /* S_VID_OB_EXP_OC_ID */
--    obj_square              NUMERIC(15,2), /* SQUARE */
--    expert_id               INTEGER, /* S_EXPERT_ID */
--    expert_name             VARCHAR(255), /* S_EXPERT_NAME */
--    modified_by             VARCHAR(128), /* ISP */
--    modify_date             DATE, /* DT */
--    rezenz_id               INTEGER, /* S_REZENZ_ID */
--    addr_street_name        VARCHAR(100), /* OBJECT_ULNAME */
--    cost_1_usd              NUMERIC(15,2), /* VART1_USD */
--    cost_prim               NUMERIC(15,2), /* VART_PRIM */
--    valuation_date          DATE, /* DT_EXP */
--    final_date              DATE, /* DT_ITOG */
--    arch_num                VARCHAR(10), /* ARCH_NOM */
--    arch_date               DATE, /* ARCH_DT */
--    is_archived             INTEGER DEFAULT 0, /* IN_ARCH */
--    is_stand_oc             INTEGER DEFAULT 0, /* IS_STAND_OC */
--    is_deleted              INTEGER DEFAULT 0, /* DELETED */
--    del_date                DATE /* DTDELETE */,
--    addr_number1            VARCHAR(9), /* OBJECT_NOMER1 */
--    addr_number2            VARCHAR(64), /* OBJECT_NOMER2 */
--    note_text               VARCHAR(255) /* PRIMITKA_TEXT */
--);

--ALTER TABLE expert_note ADD PRIMARY KEY (id);

--ALTER TABLE expert_note ADD CONSTRAINT fk_expert_note_renter FOREIGN KEY (org_renter_id) REFERENCES organizations (id);
--ALTER TABLE expert_note ADD CONSTRAINT fk_expert_note_bal_org FOREIGN KEY (org_balans_id) REFERENCES organizations (id);
--ALTER TABLE expert_note ADD CONSTRAINT fk_expert_note_building FOREIGN KEY (building_id) REFERENCES buildings (id);
--ALTER TABLE expert_note ADD CONSTRAINT fk_expert_note_balans FOREIGN KEY (balans_id) REFERENCES balans (id);
--ALTER TABLE expert_note ADD CONSTRAINT fk_expert_note_district FOREIGN KEY (addr_district_id) REFERENCES dict_districts2 (id);
--ALTER TABLE expert_note ADD CONSTRAINT fk_expert_note_obj_type FOREIGN KEY (expert_obj_type_id) REFERENCES dict_expert_obj_type (id);
--ALTER TABLE expert_note ADD CONSTRAINT fk_expert_note_expert FOREIGN KEY (expert_id) REFERENCES dict_expert (id);
--ALTER TABLE expert_note ADD CONSTRAINT fk_expert_note_rezenz FOREIGN KEY (rezenz_id) REFERENCES dict_expert_rezenz (id);

/* expert_note_detail */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[expert_note_detail]') AND type in (N'U'))
--DROP TABLE [dbo].[expert_note_detail]

--CREATE TABLE expert_note_detail /* NO_PRIM_EXP_DETAILS */ (
--    id                      INTEGER IDENTITY(1,1), /* NO_PRIM_EXP_DETAILS_ID - Replaced by an IDENTITY field */
--    expert_note_id          INTEGER, /* NO_PRIM_EXP_ID */
--    obj_square              NUMERIC(15,2), /* SQUARE */
--    cost_1_usd              NUMERIC(15,2), /* VART1_USD */
--    cost_1_usd_flag         INTEGER DEFAULT 0, /* VART1_USD_FLAG */
--    valuation_date          DATE, /* DT_EXP */
--    floors                  VARCHAR(100), /* POVERH */
--    purpose                 VARCHAR(100), /* NEV */
--    note                    VARCHAR(255), /* PRIMITKA */
--    modified_by             VARCHAR(128), /* ISP */
--    modify_date             DATE, /* DT */
--    is_deleted              INTEGER DEFAULT 0, /* DELETED */
--    del_date                DATE /* DTDELETE */
--);

--ALTER TABLE expert_note_detail ADD PRIMARY KEY (id);

--ALTER TABLE expert_note_detail ADD CONSTRAINT fk_expert_note_detail_note FOREIGN KEY (expert_note_id) REFERENCES expert_note (id);

/* expert_note_detail_grouped */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[expert_note_detail_grouped]') AND type in (N'U'))
--DROP TABLE [dbo].[expert_note_detail_grouped]

--CREATE TABLE expert_note_detail_grouped (
--    id                      INTEGER IDENTITY(1,1),
--    expert_note_id          INTEGER,
--    obj_squares             VARCHAR(MAX),
--    costs_1_usd             VARCHAR(MAX),
--    valuation_dates         VARCHAR(MAX),
--    floors                  VARCHAR(MAX),
--    purposes                VARCHAR(MAX)
--);

--ALTER TABLE expert_note_detail_grouped ADD PRIMARY KEY (id);

/* expert_input_doc */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[expert_input_doc]') AND type in (N'U'))
--DROP TABLE [dbo].[expert_input_doc]

--CREATE TABLE expert_input_doc /* NO_VHIDNI_DOK */ (
--    id                      INTEGER IDENTITY(1,1), /* NO_VHIDNI_DOK_ID - Replaced by an IDENTITY field */
--    expert_note_id          INTEGER, /* NO_PRIM_EXP_ID */
--    doc_date                DATE, /* DTDOK */
--    doc_num                 VARCHAR(20), /* NODOC */
--    control_date            DATE, /* DTKONTR */
--    korrespondent_id        INTEGER, /* KORESPONDENT_ID */
--    modify_date             DATE, /* DT */
--    modified_by             VARCHAR(128), /* ISP */
--    mvd_doc_id              INTEGER, /* MVD_DOK_ID */
--    is_deleted              INTEGER DEFAULT 0 /* DELETED */,
--    del_date                DATE /* DTDELETE */,
--    rezenz_name             VARCHAR(255) /* REZENZ_NAME */
--);

--ALTER TABLE expert_input_doc ADD PRIMARY KEY (id);

--ALTER TABLE expert_input_doc ADD CONSTRAINT fk_expert_input_doc_note FOREIGN KEY (expert_note_id) REFERENCES expert_note (id);
--ALTER TABLE expert_input_doc ADD CONSTRAINT fk_expert_input_doc_korr FOREIGN KEY (korrespondent_id) REFERENCES dict_expert_korr (id);

/* expert_input_doc_grouped */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[expert_input_doc_grouped]') AND type in (N'U'))
--DROP TABLE [dbo].[expert_input_doc_grouped]

--CREATE TABLE expert_input_doc_grouped (
--    id                      INTEGER IDENTITY(1,1),
--    expert_note_id          INTEGER,
--    doc_dates               VARCHAR(MAX),
--    doc_numbers             VARCHAR(MAX),
--    control_dates           VARCHAR(MAX),
--    korrespondents          VARCHAR(MAX),
--    rezenz_names            VARCHAR(MAX)
--);

--ALTER TABLE expert_input_doc_grouped ADD PRIMARY KEY (id);

/* expert_output_doc */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[expert_output_doc]') AND type in (N'U'))
--DROP TABLE [dbo].[expert_output_doc]

--CREATE TABLE expert_output_doc /* NO_VYHIDNI_DOK */ (
--    id                      INTEGER IDENTITY(1,1), /* NO_VYHIDNI_DOK_ID - Replaced by an IDENTITY field*/
--    expert_note_id          INTEGER, /* NO_PRIM_EXP_ID */
--    doc_date                DATE, /* DTDOK */
--    doc_num                 VARCHAR(20), /* NODOC */
--    rezenz_type_id          INTEGER, /* S_VID_REZENZ_ID */
--    modify_date             DATE, /* DT */
--    modified_by             VARCHAR(128), /* ISP */
--    is_deleted              INTEGER DEFAULT 0, /* DELETED */
--    del_date                DATE /* DTDELETE */
--);

--ALTER TABLE expert_output_doc ADD PRIMARY KEY (id);

--ALTER TABLE expert_output_doc ADD CONSTRAINT fk_expert_output_doc_note FOREIGN KEY (expert_note_id) REFERENCES expert_note (id);
--ALTER TABLE expert_output_doc ADD CONSTRAINT fk_expert_output_doc_rezenz_type FOREIGN KEY (rezenz_type_id) REFERENCES dict_expert_rezenz_type (id);

/* expert_output_doc_grouped */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[expert_output_doc_grouped]') AND type in (N'U'))
--DROP TABLE [dbo].[expert_output_doc_grouped]

--CREATE TABLE expert_output_doc_grouped (
--    id                      INTEGER IDENTITY(1,1),
--    expert_note_id          INTEGER,
--    doc_dates               VARCHAR(MAX),
--    doc_numbers             VARCHAR(MAX),
--    rezenz_kinds            VARCHAR(MAX),
--);

--ALTER TABLE expert_output_doc_grouped ADD PRIMARY KEY (id);

--GO

/******************************************************************************/
/*                             Archive states                                 */
/******************************************************************************/

/* arch_buildings */

--IF OBJECT_ID('arch_buildings') IS NOT NULL
--DROP TABLE arch_buildings

--GO

--SELECT * INTO arch_buildings FROM buildings

--ALTER TABLE arch_buildings ADD archive_id INTEGER IDENTITY(1,1)
--ALTER TABLE arch_buildings ADD archive_create_date DATE
--ALTER TABLE arch_buildings ADD archive_create_user VARCHAR(24)

--ALTER TABLE arch_buildings ADD PRIMARY KEY (archive_id);

--GO

/* arch_organizations */

--IF OBJECT_ID('arch_organizations') IS NOT NULL
--DROP TABLE arch_organizations

--GO

--SELECT * INTO arch_organizations FROM organizations

--ALTER TABLE arch_organizations ADD archive_id INTEGER IDENTITY(1,1)
--ALTER TABLE arch_organizations ADD archive_create_date DATE
--ALTER TABLE arch_organizations ADD archive_create_user VARCHAR(24)

--ALTER TABLE arch_organizations ADD PRIMARY KEY (archive_id);

--GO

/* arch_arenda */

--IF OBJECT_ID('arch_arenda') IS NOT NULL
--DROP TABLE arch_arenda

--GO

--SELECT * INTO arch_arenda FROM arenda

--ALTER TABLE arch_arenda ADD archive_id INTEGER IDENTITY(1,1)
--ALTER TABLE arch_arenda ADD archive_create_date DATE
--ALTER TABLE arch_arenda ADD archive_create_user VARCHAR(24)
--ALTER TABLE arch_arenda ADD archive_link_code INTEGER NOT NULL

--ALTER TABLE arch_arenda ADD PRIMARY KEY (archive_id);

--GO

/* arch_arenda_notes */

--IF OBJECT_ID('arch_arenda_notes') IS NOT NULL
--DROP TABLE arch_arenda_notes

--GO

--SELECT * INTO arch_arenda_notes FROM arenda_notes

--ALTER TABLE arch_arenda_notes DROP COLUMN id
--ALTER TABLE arch_arenda_notes ADD archive_id INTEGER IDENTITY(1,1)
--ALTER TABLE arch_arenda_notes ADD archive_arenda_link_code INTEGER NOT NULL

--ALTER TABLE arch_arenda_notes ADD PRIMARY KEY (archive_id);

--GO

/* arch_link_arenda_2_decisions*/

--IF OBJECT_ID('arch_link_arenda_2_decisions') IS NOT NULL
--DROP TABLE arch_link_arenda_2_decisions

--GO

--SELECT * INTO arch_link_arenda_2_decisions FROM link_arenda_2_decisions

--ALTER TABLE arch_link_arenda_2_decisions DROP COLUMN id
--ALTER TABLE arch_link_arenda_2_decisions ADD archive_id INTEGER IDENTITY(1,1)
--ALTER TABLE arch_link_arenda_2_decisions ADD archive_arenda_link_code INTEGER NOT NULL

--ALTER TABLE arch_link_arenda_2_decisions ADD PRIMARY KEY (archive_id);

--GO

/* arch_balans */

--IF OBJECT_ID('arch_balans') IS NOT NULL
--DROP TABLE arch_balans

--GO

--SELECT * INTO arch_balans FROM balans

--ALTER TABLE arch_balans ADD archive_id INTEGER IDENTITY(1,1)
--ALTER TABLE arch_balans ADD archive_create_date DATE
--ALTER TABLE arch_balans ADD archive_create_user VARCHAR(24)
--ALTER TABLE arch_balans ADD archive_link_code INTEGER NOT NULL

--ALTER TABLE arch_balans ADD PRIMARY KEY (archive_id);

--GO

/* arch_balans_docs */

--IF OBJECT_ID('arch_balans_docs') IS NOT NULL
--DROP TABLE arch_balans_docs

--GO

--SELECT * INTO arch_balans_docs FROM balans_docs

--ALTER TABLE arch_balans_docs DROP COLUMN id
--ALTER TABLE arch_balans_docs ADD archive_id INTEGER IDENTITY(1,1)
--ALTER TABLE arch_balans_docs ADD archive_balans_link_code INTEGER NOT NULL

--ALTER TABLE arch_balans_docs ADD PRIMARY KEY (archive_id);

--GO

/******************************************************************************/
/* Some 1NF dictionaries must be duplicated in our database in exactly the
   same way as they exist in 1NF. This is useful when we need to create new
   addresses/objects/organizations in 1NF. Such 'copy' dictionaries are
   described in this section. */
/******************************************************************************/

/*    dict_1nf_streets    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_streets]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_streets]

--CREATE TABLE dict_1nf_streets /* SUL */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(160) DEFAULT ' ', /* NAME */
--    stan               INTEGER NOT NULL /* STAN */
--);

--ALTER TABLE dict_1nf_streets ADD PRIMARY KEY (id, stan);



/*    dict_1nf_districts2    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_districts2]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_districts2]

--CREATE TABLE dict_1nf_districts2 /* S_RAYON2 */ (
--    id                 INTEGER NOT NULL, /* KOD_RAYON2 */
--    name               VARCHAR(64) DEFAULT ' ' /* NAME_RAYON2 */
--);

--ALTER TABLE dict_1nf_districts2 ADD PRIMARY KEY (id);

/*    dict_1nf_tech_state    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_tech_state]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_tech_state]

--CREATE TABLE dict_1nf_tech_state /* STEXSTAN */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(64) DEFAULT ' ' /* NAME */
--);

--ALTER TABLE dict_1nf_tech_state ADD PRIMARY KEY (id);

/*    dict_1nf_history    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_history]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_history]

--CREATE TABLE dict_1nf_history /* SHISTORY */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(64) DEFAULT ' ' /* NAME */
--);

--ALTER TABLE dict_1nf_history ADD PRIMARY KEY (id);

/*    dict_1nf_object_kind    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_object_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_object_kind]

--CREATE TABLE dict_1nf_object_kind /* SKINDOBJ */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(64) DEFAULT ' ' /* NAME */
--);

--ALTER TABLE dict_1nf_object_kind ADD PRIMARY KEY (id);

/*    dict_1nf_object_type    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_object_type]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_object_type]

--CREATE TABLE dict_1nf_object_type /* STYPEOBJ */ (
--    id                 INTEGER NOT NULL, /* KOD */
--    name               VARCHAR(64) DEFAULT ' ' /* NAME */
--);

--ALTER TABLE dict_1nf_object_type ADD PRIMARY KEY (id);

/* dict_1nf_org_ownership */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_org_ownership]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_org_ownership]

--CREATE TABLE dict_1nf_org_ownership /* S_FORM_VLASN */ (
--    id               INTEGER NOT NULL, /* KOD_FORM_VLASN */
--    name             VARCHAR(40) /* NAME_FORM_VLASN */
--);

--ALTER TABLE dict_1nf_org_ownership ADD PRIMARY KEY (id);

/* dict_1nf_balans_purpose_group */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_balans_purpose_group]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_balans_purpose_group]

--CREATE TABLE dict_1nf_balans_purpose_group /* SGRPURPOSE */ (
--    id                INTEGER NOT NULL, /* KOD */
--    name              VARCHAR(160) /* NAME */
--);

--ALTER TABLE dict_1nf_balans_purpose_group ADD PRIMARY KEY (id);

/* dict_1nf_balans_purpose */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_balans_purpose]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_balans_purpose]

--CREATE TABLE dict_1nf_balans_purpose /* SPURPOSE */ (
--    id                INTEGER NOT NULL, /* KOD */
--    group_code        INTEGER, /* GR */
--    name              VARCHAR(160) /* NAME */
--);

--ALTER TABLE dict_1nf_balans_purpose ADD PRIMARY KEY (id);

/* dict_1nf_org_status */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_org_status]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_org_status]

--CREATE TABLE dict_1nf_org_status /* S_STATUS */(
--    id             INTEGER NOT NULL, /* KOD_STATUS */
--    name           VARCHAR(20) /* NAME_STATUS */
--);

--ALTER TABLE dict_1nf_org_status ADD PRIMARY KEY (id);

/* dict_1nf_org_form_gosp */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_org_form_gosp]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_org_form_gosp]

--CREATE TABLE dict_1nf_org_form_gosp /* S_FORM_GOSP */ (
--    id             INTEGER NOT NULL, /* KOD_FORM_GOSP */
--    name           VARCHAR(40) /* NAME_FORM_GOSP */
--);

--ALTER TABLE dict_1nf_org_form_gosp ADD PRIMARY KEY (id);

/* dict_1nf_org_industry */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_org_industry]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_org_industry]

--CREATE TABLE dict_1nf_org_industry /* S_GALUZ */ (
--    id               INTEGER NOT NULL, /* KOD_GALUZ */
--    name             VARCHAR(40) /* NAME_GALUZ */
--);

--ALTER TABLE dict_1nf_org_industry ADD PRIMARY KEY (id);

/* dict_1nf_org_occupation */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_org_occupation]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_org_occupation]

--CREATE TABLE dict_1nf_org_occupation /* S_VID_DIAL */ (
--    id             INTEGER NOT NULL, /* KOD_VID_DIAL */
--    name           VARCHAR(50), /* NAME_VID_DIAL */
--    branch_code    INTEGER /* KOD_GALUZ */
--);

--ALTER TABLE dict_1nf_org_occupation ADD PRIMARY KEY (id);

/* dict_1nf_org_vedomstvo */

--IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_org_vedomstvo]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_org_vedomstvo]

--CREATE TABLE dict_1nf_org_vedomstvo /* S_VIDOM_NAL */ (
--    id               INTEGER NOT NULL, /* KOD_VIDOM_NAL */
--    name             VARCHAR(255) /* NAME_VIDOM_NAL */
--);

--ALTER TABLE dict_1nf_org_vedomstvo ADD PRIMARY KEY (id);

/*    dict_1nf_doc_kind    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_doc_kind]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_doc_kind]

--CREATE TABLE dict_1nf_doc_kind /* SKINDDOK */ (
--    id              INTEGER NOT NULL, /* ID */
--    name            VARCHAR(60) /* NAME */
--);

--ALTER TABLE dict_1nf_doc_kind ADD PRIMARY KEY (id);

/*    dict_1nf_facade    */

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dict_1nf_facade]') AND type in (N'U'))
--DROP TABLE [dbo].[dict_1nf_facade]

--CREATE TABLE dict_1nf_facade /* SKINDDOK */ (
--    id              INTEGER NOT NULL, /* KOD */
--    name            VARCHAR(60) /* NAME */
--);

--ALTER TABLE dict_1nf_facade ADD PRIMARY KEY (id);

/******************************************************************************/
/*                Create tables for user-defined mappings                     */
/******************************************************************************/

/* Mapping between 'Balans' organizations and '1NF' organizations */

IF (NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mapping_org_balans_1nf]') AND type in (N'U')))

CREATE TABLE mapping_org_balans_1nf (
    org_id_balans     INTEGER NOT NULL,
    org_id_1nf        INTEGER NOT NULL
);

/* Mapping between 'Privatization' organizations and '1NF' organizations */

IF (NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mapping_org_privat_1nf]') AND type in (N'U')))

CREATE TABLE mapping_org_privat_1nf (
    org_id_privat     INTEGER NOT NULL,
    org_id_1nf        INTEGER NOT NULL
);


/* Street name corrections (used by the address mapping algorithm) */

IF (NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mapping_street_name_corrections]') AND type in (N'U')))

CREATE TABLE mapping_street_name_corrections (
    id                INTEGER IDENTITY(1,1) PRIMARY KEY,
    street_pattern    VARCHAR(128),
    street_name       VARCHAR(128)
);

/* This table allows DKV users to unsubscribe from web-based 1NF report notifications */

IF OBJECT_ID('user_notification_settings') IS NULL
BEGIN
	CREATE TABLE user_notification_settings (
		UserId            UNIQUEIDENTIFIER NOT NULL,
		organization_id   INTEGER NOT NULL,
		is_notify         INTEGER DEFAULT 1
	);
	
	ALTER TABLE user_notification_settings ADD PRIMARY KEY (UserId, organization_id);
END

GO

/* Insert static mapping for the 'Privatization' organizations */

--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (588, 23005)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (598, 3228)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (652, 11936)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (1572, 29687)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (1710, 19205)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (2528, 133886)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (2723, 461)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (3292, 136724)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (3296, 136724)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (3481, 139373)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (3485, 140052)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (3649, 139021)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (3957, 135239)
--INSERT INTO mapping_org_privat_1nf (org_id_privat, org_id_1nf) VALUES (4139, 3723)

--GO

IF OBJECT_ID('out_of_city_organizations') IS NULL
BEGIN
	CREATE TABLE out_of_city_organizations (
		organization_id   INTEGER NOT NULL PRIMARY KEY
	);
	
	INSERT INTO out_of_city_organizations (organization_id) VALUES (11638)
	INSERT INTO out_of_city_organizations (organization_id) VALUES (142028)
	INSERT INTO out_of_city_organizations (organization_id) VALUES (11620)
END

GO

/******************************************************************************/
/*                    Create tables for user settings                         */
/******************************************************************************/

/* User folders */

IF (NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[user_folders]') AND type in (N'U')))

CREATE TABLE user_folders (
    id                INTEGER IDENTITY(1,1) PRIMARY KEY,
    parent_folder_id  INTEGER,
    usr_id            UNIQUEIDENTIFIER,
    folder_name       VARCHAR(255)
);

/* User saved reports */

IF (NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[user_reports]') AND type in (N'U')))

CREATE TABLE user_reports (
    id                INTEGER IDENTITY(1,1) PRIMARY KEY,
    usr_id            UNIQUEIDENTIFIER,
    folder_id         INTEGER,
    report_name       VARCHAR(1024),
    category          INTEGER NOT NULL, /* Identifies the grid that shows the report */
    grid_layout       VARCHAR(MAX), /* Stored layout of the ASPxGridView */
    pre_filter        VARCHAR(MAX), 
    fixed_columns	  VARCHAR(1024) /* Stored fixed columns of the ASPxGridView on report creating/updating */
);

GO

/* Periods for financial reporting */

IF (NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fin_periods]') AND type in (N'U')))

CREATE TABLE fin_periods (
    id                INTEGER IDENTITY(1,1) PRIMARY KEY,
    period_name       VARCHAR(48),
    period_code       VARCHAR(48)
);

GO

/******************************************************************************/
/*                      Create tables for system reports                      */
/******************************************************************************/

IF (NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sys_report_corrections]') AND type in (N'U')))

CREATE TABLE sys_report_corrections (
    id                INTEGER IDENTITY(1,1) PRIMARY KEY,
    is_corrected      INTEGER,
    correction_kind   INTEGER,
    obj_descr         VARCHAR(MAX),
    action_descr      VARCHAR(MAX),
    reason_descr      VARCHAR(MAX),
    modify_date       DATE
);

GO

/******************************************************************************/
/*            Rishennya/Rozporadjennia generation business process            */
/******************************************************************************/

/* dict_rish_project_state */

IF OBJECT_ID('dict_rish_project_state') IS NULL
BEGIN
	CREATE TABLE dict_rish_project_state (
		id                INTEGER IDENTITY(1,1) PRIMARY KEY,
		name              VARCHAR(128)
	);
	
	INSERT INTO dict_rish_project_state (name) VALUES ('ПРОЕКТ')
	INSERT INTO dict_rish_project_state (name) VALUES ('У КЕРІВНИКА ДЕПАРТАМЕНТУ')
	INSERT INTO dict_rish_project_state (name) VALUES ('НА ПОГОДЖЕННІ В РДА')
	INSERT INTO dict_rish_project_state (name) VALUES ('НА ПОГОДЖЕННІ ГОЛОВИ КМДА')
	INSERT INTO dict_rish_project_state (name) VALUES ('НА ПОГОДЖЕННІ В КМДА')
	INSERT INTO dict_rish_project_state (name) VALUES ('НА ПОГОДЖЕННІ В КИЇВРАДІ')
	INSERT INTO dict_rish_project_state (name) VALUES ('НА ПОГОДЖЕННІ В ІНШИХ ПІДПРИЄМСТВАХ')
	INSERT INTO dict_rish_project_state (name) VALUES ('НА РОЗГЛЯДІ ПОСТІЙНОЇ КОМІСІЇ')
	INSERT INTO dict_rish_project_state (name) VALUES ('НА РОЗГЛЯДІ В ПРЕЗИДІЇ')
	INSERT INTO dict_rish_project_state (name) VALUES ('ВИНЕСЕНО НА СЕСІЮ КИЇВРАДИ')
	INSERT INTO dict_rish_project_state (name) VALUES ('ПРОЕКТ ВІДХИЛЕНО')
	INSERT INTO dict_rish_project_state (name) VALUES ('ПРОЕКТ ПРИЙНЯТИЙ')
	INSERT INTO dict_rish_project_state (name) VALUES ('В ЮРИДИЧНОМУ ВІДДІЛІ ДКВ')
	INSERT INTO dict_rish_project_state (name) VALUES ('НА ПІДПИСІ У КОРНІЙЦЯ')
	INSERT INTO dict_rish_project_state (name) VALUES ('НА ПІДПИСІ У ГУДЗЯ')
	INSERT INTO dict_rish_project_state (name) VALUES ('ЗАТВЕРДЖЕНО')
	INSERT INTO dict_rish_project_state (name) VALUES ('ПОТРЕБУЄ ДООПРАЦЮВАННЯ')
END

IF COLUMNPROPERTY(OBJECT_ID('dict_rish_project_state'), 'is_final', 'ColumnId') IS NULL
BEGIN

	--IF OBJECT_ID('bp_rish_project_info') IS NOT NULL
	--BEGIN
		--ALTER TABLE dbo.bp_rish_project_info
			--DROP FK_bp_rish_project_info_dict_rish_project_state;
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_bp_rish_project_info_dict_rish_project_state]') AND parent_object_id = OBJECT_ID(N'[dbo].[bp_rish_project_info]'))
ALTER TABLE [dbo].[bp_rish_project_info] DROP CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_state]			
	--END;
END;
GO

IF COLUMNPROPERTY(OBJECT_ID('dict_rish_project_state'), 'is_final', 'ColumnId') IS NULL
BEGIN
	--TRUNCATE TABLE dict_rish_project_state;
	DELETE FROM dict_rish_project_state;

	BEGIN
		if not exists(select * from syscolumns where name = 'order' and id = object_id('dict_rish_project_state'))
		BEGIN
			ALTER TABLE dict_rish_project_state ADD [order] INT NOT NULL;
		END;
		if not exists(select * from syscolumns where name = 'flags' and id = object_id('dict_rish_project_state'))	
		BEGIN
			ALTER TABLE dict_rish_project_state ADD [flags] INT NOT NULL;
		END;
	END;
END;
GO

IF NOT EXISTS(SELECT * FROM dict_rish_project_state)
BEGIN
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('Проект', 0, 0);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('В юридичному відділі ДКВ', 10, 1);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('У керівника ДКВ', 20, 1);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('Наказ підписано', 30, 4);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('На погоджені в інших підприємствах', 40, 3);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('На погодженні в РДА', 50, 3);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('На погоджені у заступника голови КМДА', 60, 3);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('На погоджені в юридичному управлінні КМДА', 70, 3);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('Передано в секретаріат Київради', 80, 3);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('На розгляді постійної комісії', 90, 3);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('На погоджені в юридичному управлінні Київради', 100, 1);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('На розгляді в президії', 110, 1);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('Винесено на сесію Київради', 120, 1);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('На підписі у голови КМДА', 130, 1);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('Розпорядження підписано', 140, 4);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('Проект рішення відхилено', 150, 4);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('Рішення прийнято', 160, 4);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('На підписі у міського голови', 170, 1);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('Рішення підписано', 180, 4);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('Надані зауваження', 190, 1);
	INSERT INTO dict_rish_project_state (name, [order], [flags]) VALUES ('Виконано', 200, 8);
END;
GO

--???
--IF OBJECT_ID('bp_rish_project_info') IS NOT NULL AND OBJECT_ID('[FK_bp_rish_project_info_dict_rish_project_state]') IS NULL
--BEGIN
--	ALTER TABLE [dbo].[bp_rish_project_info]  WITH CHECK ADD  CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_state] FOREIGN KEY([state_id])
--	REFERENCES [dbo].[dict_rish_project_state] ([id])
--	ON UPDATE CASCADE
--	ON DELETE CASCADE;

--	ALTER TABLE [dbo].[bp_rish_project_info] CHECK CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_state];
--END;
--GO

/* dict_rish_project_type - document draft type (rishennya, rosporyadzhennya) */

IF OBJECT_ID('dict_rish_project_type') IS NULL
BEGIN
	CREATE TABLE [dbo].[dict_rish_project_type] (
		[id] [int] IDENTITY(1,1) NOT NULL,
		[name] [varchar](128) NULL,
		CONSTRAINT [PK_dict_rish_project_type] PRIMARY KEY CLUSTERED ([id] ASC),
		CONSTRAINT [UC_dict_rish_project_type] UNIQUE NONCLUSTERED ([name] ASC)
	);
END;
GO

IF NOT EXISTS (SELECT TOP 1 id FROM [dict_rish_project_type] WHERE name LIKE '%РІШЕННЯ%')
	INSERT INTO [dict_rish_project_type] (name) VALUES ('РІШЕННЯ');

IF NOT EXISTS (SELECT TOP 1 id FROM [dict_rish_project_type] WHERE name LIKE '%РОЗПОРЯДЖЕННЯ%')
	INSERT INTO [dict_rish_project_type] (name) VALUES ('РОЗПОРЯДЖЕННЯ');

IF NOT EXISTS (SELECT TOP 1 id FROM [dict_rish_project_type] WHERE name LIKE '%НАКАЗ%')
	INSERT INTO [dict_rish_project_type] (name) VALUES ('НАКАЗ');

/* dict_rish_project_org - organizations that serve as origination points for document drafts */

IF OBJECT_ID('dict_rish_project_org') IS NULL
BEGIN
	CREATE TABLE [dbo].[dict_rish_project_org] (
		[id] [int] IDENTITY(1,1) NOT NULL,
		[name] [varchar](128) NULL,
		CONSTRAINT [PK_dict_rish_project_org] PRIMARY KEY CLUSTERED ([id] ASC),
		CONSTRAINT [UC_dict_rish_project_org] UNIQUE NONCLUSTERED ([name] ASC)
	);
END

/* dict_rish_project_org_contact - contact persons within originating organizations for document drafts */

IF OBJECT_ID('dict_rish_project_org_contact') IS NULL
BEGIN
	CREATE TABLE [dbo].[dict_rish_project_org_contact](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[project_org_id] [int] NOT NULL,
		[contact_name] [varchar](128) NOT NULL,
		[contact_title] [varchar](128) NULL,
		[contact_phone] [varchar](128) NOT NULL,
		CONSTRAINT [PK_dict_rish_project_org_contact] PRIMARY KEY CLUSTERED ([id] ASC),
		CONSTRAINT [UC_dict_rish_project_org_contact] UNIQUE NONCLUSTERED ([project_org_id] ASC, [contact_name] ASC)
	);

	ALTER TABLE [dbo].[dict_rish_project_org_contact]  WITH CHECK ADD  CONSTRAINT [FK_dict_rish_project_org_contact_dict_rish_project_org] FOREIGN KEY([project_org_id])
	REFERENCES [dbo].[dict_rish_project_org] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[dict_rish_project_org_contact] CHECK CONSTRAINT [FK_dict_rish_project_org_contact_dict_rish_project_org];
END


IF OBJECT_ID('external_document') IS NULL
BEGIN
	CREATE TABLE dbo.external_document
		(
		id int NOT NULL IDENTITY (1, 1),
		name varchar(512) NOT NULL,
		unique_filename varchar(512) NOT NULL,
		CONSTRAINT [PK_external_document] PRIMARY KEY CLUSTERED (id)
		);
END

/* bp_rish_project - all document drafts and their appendixes (the subset of data that is shared with appendixes) */

IF OBJECT_ID('bp_rish_project') IS NULL
BEGIN
	CREATE TABLE [dbo].[bp_rish_project] (
		[id] [int] IDENTITY(1,1) NOT NULL,
		[name] [varchar](max) NOT NULL,
		[intro_text] [varchar](max) NULL,
		[outro_text] [varchar](max) NULL,
		[app_for_project_id] [int] NULL,
		[ordinal_pos] int NULL,
		[external_document_id] int NULL,
		[use_external_document] bit NULL,
		CONSTRAINT [PK__bp_rish___3213E83F61D381D5] PRIMARY KEY CLUSTERED ([id] ASC)
	);
	
	ALTER TABLE [dbo].[bp_rish_project]  WITH CHECK ADD  CONSTRAINT [FK_bp_rish_project_bp_rish_project] FOREIGN KEY([app_for_project_id])
	REFERENCES [dbo].[bp_rish_project] ([id]);

	ALTER TABLE [dbo].[bp_rish_project] CHECK CONSTRAINT [FK_bp_rish_project_bp_rish_project];
END

IF COL_LENGTH('bp_rish_project', 'ordinal_pos') IS NULL
BEGIN
	ALTER TABLE dbo.bp_rish_project ADD
		ordinal_pos int NULL;
END

IF COL_LENGTH('bp_rish_project', 'external_document_id') IS NULL
BEGIN
	ALTER TABLE dbo.bp_rish_project ADD
		external_document_id int NULL;
		
	ALTER TABLE dbo.bp_rish_project 
	ADD CONSTRAINT [FK_bp_rish_project_external_document] FOREIGN KEY
		(external_document_id) REFERENCES dbo.external_document(id);
END

IF COL_LENGTH('bp_rish_project', 'use_external_document') IS NULL
BEGIN
	ALTER TABLE dbo.bp_rish_project ADD
		use_external_document bit NULL;
END

/* bp_rish_project_info - document draft information (the subset of data that is not shared with document appendixes) */

IF OBJECT_ID('bp_rish_project_info') IS NULL
BEGIN
	CREATE TABLE [dbo].[bp_rish_project_info] (
		[id] [int] IDENTITY(1,1) NOT NULL,
		[project_id] [int] NOT NULL,
		[document_num] [varchar](64) NULL,
		[document_date] [date] NULL,
		[state_id] [int] NOT NULL,
		[created_by] [uniqueidentifier] NOT NULL,
		[create_date] [datetime] NOT NULL,
		[modified_by] [uniqueidentifier] NOT NULL,
		[modify_date] [datetime] NOT NULL,
		[project_type_id] [int] NOT NULL,
		[project_contact_id] [int] NOT NULL,
		[subject] [varchar](max) NULL,
		[is_exported] [int] NULL DEFAULT 0,
		CONSTRAINT [PK_bp_rish_project_info] PRIMARY KEY CLUSTERED ([id] ASC),
		CONSTRAINT [UC_bp_rish_project_info] UNIQUE NONCLUSTERED ([project_id] ASC)
	);

	ALTER TABLE [dbo].[bp_rish_project_info]  WITH CHECK ADD  CONSTRAINT [FK_bp_rish_project_info_bp_rish_project] FOREIGN KEY([project_id])
	REFERENCES [dbo].[bp_rish_project] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[bp_rish_project_info] CHECK CONSTRAINT [FK_bp_rish_project_info_bp_rish_project];

	ALTER TABLE [dbo].[bp_rish_project_info]  WITH CHECK ADD  CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_org_contact] FOREIGN KEY([project_contact_id])
	REFERENCES [dbo].[dict_rish_project_org_contact] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[bp_rish_project_info] CHECK CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_org_contact];

	ALTER TABLE [dbo].[bp_rish_project_info]  WITH CHECK ADD  CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_state] FOREIGN KEY([state_id])
	REFERENCES [dbo].[dict_rish_project_state] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[bp_rish_project_info] CHECK CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_state];

	ALTER TABLE [dbo].[bp_rish_project_info]  WITH CHECK ADD  CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_type] FOREIGN KEY([project_type_id])
	REFERENCES [dbo].[dict_rish_project_type] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[bp_rish_project_info] CHECK CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_type];
END

/* bp_rish_project_history - all changes made to the bp_rish_project table */

IF OBJECT_ID('bp_rish_project_history') IS NULL
BEGIN
	CREATE TABLE [dbo].[bp_rish_project_history] (
		[id] [int] IDENTITY(1,1) NOT NULL,
		[project_id] [int] NOT NULL,
		[change_text] [varchar](max) NOT NULL,
		[modified_by] [uniqueidentifier] NOT NULL,
		[modify_date] [datetime] NOT NULL,
		CONSTRAINT [PK__bp_rish___3213E83F678C5B2B] PRIMARY KEY CLUSTERED ([id] ASC)
	);

	ALTER TABLE [dbo].[bp_rish_project_history]  WITH CHECK ADD  CONSTRAINT [fk_bp_rish_project_history_proj] FOREIGN KEY([project_id])
	REFERENCES [dbo].[bp_rish_project] ([id]);

	ALTER TABLE [dbo].[bp_rish_project_history] CHECK CONSTRAINT [fk_bp_rish_project_history_proj];
END

/* bp_rish_project_item - individual items of Rishennya projects */

IF OBJECT_ID('bp_rish_project_item') IS NULL
BEGIN
	CREATE TABLE [dbo].[bp_rish_project_item] (
		[id] [int] IDENTITY(1,1) NOT NULL,
		[project_id] [int] NOT NULL,
		[parent_item_id] [int] NULL,
		[ordinal_pos] [int] NULL,
		[intro_text] [varchar](max) NULL,
		[outro_text] [varchar](max) NULL,
		[building_id] [int] NULL,
		[balans_id] [int] NULL,
		[org_from_id] [int] NULL,
		[org_to_id] [int] NULL,
		[right_id] [int] NULL,
		[explanation] [varchar](max) NULL,
		[is_table] [bit] NOT NULL,
		[arbitrary_text] [varchar](max),
		[external_document_id] int NULL,
		[use_external_document] int NULL,
		CONSTRAINT [PK__bp_rish___3213E83F7115C565] PRIMARY KEY CLUSTERED ([id] ASC)
	);

	ALTER TABLE [dbo].[bp_rish_project_item]  WITH CHECK ADD  CONSTRAINT [fk_bp_rish_project_item_proj] FOREIGN KEY([project_id])
	REFERENCES [dbo].[bp_rish_project] ([id]);

	ALTER TABLE [dbo].[bp_rish_project_item] CHECK CONSTRAINT [fk_bp_rish_project_item_proj];
END
ELSE
BEGIN
	IF OBJECT_ID('UC_bp_rish_project_item') IS NOT NULL
	BEGIN
		ALTER TABLE dbo.bp_rish_project_item
			DROP CONSTRAINT UC_bp_rish_project_item;
	END
END

IF COL_LENGTH('bp_rish_project_item', 'external_document_id') IS NULL
BEGIN
	ALTER TABLE dbo.[bp_rish_project_item] ADD
		external_document_id int NULL;
		
	ALTER TABLE dbo.[bp_rish_project_item] 
	ADD CONSTRAINT [FK_bp_rish_project_item_external_document] FOREIGN KEY
		(external_document_id) REFERENCES dbo.external_document(id);
END

IF COL_LENGTH('bp_rish_project_item', 'use_external_document') IS NULL
BEGIN
	ALTER TABLE dbo.[bp_rish_project_item] ADD
		use_external_document bit NULL;
END

/* bp_rish_project_signature - document draft and document appendix signatures */

IF OBJECT_ID('bp_rish_project_signature') IS NULL
BEGIN
	CREATE TABLE [dbo].[bp_rish_project_signature] (
		[id] [int] IDENTITY(1,1) NOT NULL,
		[project_id] [int] NOT NULL,
		[person_name] [varchar](128) NOT NULL,
		[person_title] [varchar](128) NOT NULL,
		[signed_on] [date] NULL,
		[ordinal_pos] [int] NOT NULL,
		CONSTRAINT [PK_bp_rish_project_signature] PRIMARY KEY CLUSTERED ([id] ASC)
	);

	ALTER TABLE [dbo].[bp_rish_project_signature]  WITH CHECK ADD  CONSTRAINT [FK_bp_rish_project_signature_bp_rish_project] FOREIGN KEY([project_id])
	REFERENCES [dbo].[bp_rish_project] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[bp_rish_project_signature] CHECK CONSTRAINT [FK_bp_rish_project_signature_bp_rish_project];
END

/* dict_rish_table_column - system-wide registry of all columns that can be used in document draft tables */

--IF OBJECT_ID('dict_rish_table_column') IS NULL
--BEGIN
--	CREATE TABLE dbo.dict_rish_table_column
--		(
--		id int NOT NULL IDENTITY (1, 1),
--		name varchar(256) NOT NULL,
--		semantic_type int NOT NULL
--		);
--	ALTER TABLE dbo.dict_rish_table_column ADD CONSTRAINT
--		PK_dict_rish_table_column PRIMARY KEY CLUSTERED 
--		(
--		id
--		);
--	ALTER TABLE dbo.dict_rish_table_column ADD CONSTRAINT
--		UC_dict_rish_table_column UNIQUE NONCLUSTERED 
--		(
--		name
--		);
		
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Адреса (Об''єкт)', 1)
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Від кого передається', 2)
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Кому передається', 3)
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Право', 4)
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Рік побудови', 5)
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Рік введення в експлуатацію', 6)
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Балансова вартість первинна', 7)
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Балансова вартість залишкова', 8)
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Площа, кв.м.', 9)
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Довжина, м.', 10)
--	INSERT INTO dbo.dict_rish_table_column (name, semantic_type) VALUES ('Діаметр, мм.', 11)
--END

/* bp_rish_project_table_column - specific table configuration (column set, ordering of columns) */

--IF OBJECT_ID('bp_rish_project_table_column') IS NULL
--BEGIN
--	CREATE TABLE dbo.bp_rish_project_table_column
--		(
--		id int NOT NULL IDENTITY (1, 1),
--		table_id int NOT NULL,
--		column_id int NOT NULL,
--		ordinal_pos int NULL
--		);
--	ALTER TABLE dbo.bp_rish_project_table_column ADD CONSTRAINT
--		PK_bp_rish_project_table_column PRIMARY KEY CLUSTERED 
--		(
--		id
--		);
--	ALTER TABLE dbo.bp_rish_project_table_column ADD CONSTRAINT
--		FK_bp_rish_project_table_column_dict_rish_table_column FOREIGN KEY
--		(
--		column_id
--		) REFERENCES dbo.dict_rish_table_column
--		(
--		id
--		) ON UPDATE  CASCADE 
--		 ON DELETE  CASCADE;
--	ALTER TABLE dbo.bp_rish_project_table_column ADD CONSTRAINT
--		FK_bp_rish_project_table_column_bp_rish_project_item FOREIGN KEY
--		(
--		table_id
--		) REFERENCES dbo.bp_rish_project_item
--		(
--		id
--		) ON UPDATE  CASCADE 
--		 ON DELETE  CASCADE;
--END

/* bp_rish_project_table_value - values for the dynamic table columns */

--IF OBJECT_ID('bp_rish_project_table_value') IS NULL
--BEGIN
--	CREATE TABLE dbo.bp_rish_project_table_value
--		(
--		id int NOT NULL IDENTITY (1, 1),
--		table_item_id int NOT NULL,
--		table_column_id int NOT NULL,
--		value varchar(MAX) NULL
--		);
--	ALTER TABLE dbo.bp_rish_project_table_value ADD CONSTRAINT
--		PK_bp_rish_project_table_value PRIMARY KEY CLUSTERED 
--		(
--		id
--		);
--	ALTER TABLE dbo.bp_rish_project_table_value ADD CONSTRAINT
--		FK_bp_rish_project_table_value_bp_rish_project_table_column FOREIGN KEY
--		(
--		table_column_id
--		) REFERENCES dbo.bp_rish_project_table_column
--		(
--		id
--		) ON UPDATE  CASCADE 
--		 ON DELETE  CASCADE;
--	ALTER TABLE dbo.bp_rish_project_table_value ADD CONSTRAINT
--		FK_bp_rish_project_table_value_bp_rish_project_item FOREIGN KEY
--		(
--		table_item_id
--		) REFERENCES dbo.bp_rish_project_item
--		(
--		id
--		);
--END
--GO

IF OBJECT_ID('bp_rish_project_state') IS NULL
BEGIN
	CREATE TABLE [dbo].[bp_rish_project_state](
		[id] [int] NOT NULL PRIMARY KEY CLUSTERED IDENTITY(1,1),
		[project_id] [int] NOT NULL,
		[state_id] [int] NOT NULL,
		[entered_on] [datetime] NOT NULL,
		[entered_by] [uniqueidentifier] NOT NULL,
		[exited_on] [datetime] NULL,
		[exited_by] [uniqueidentifier] NULL,
		[cover_letter_no] [varchar](128) NULL,
		[cover_letter_date] [date] NULL
	)
END
GO

IF OBJECT_ID('FK_bp_rish_project_state_bp_rish_project') IS NULL
BEGIN
	ALTER TABLE [dbo].[bp_rish_project_state]  WITH CHECK ADD  CONSTRAINT [FK_bp_rish_project_state_bp_rish_project] FOREIGN KEY([project_id])
	REFERENCES [dbo].[bp_rish_project] ([id])
	ON UPDATE CASCADE ON DELETE CASCADE;

	ALTER TABLE [dbo].[bp_rish_project_state] CHECK CONSTRAINT [FK_bp_rish_project_state_bp_rish_project];
END
GO

IF OBJECT_ID('FK_bp_rish_project_state_dict_rish_project_state') IS NULL
BEGIN
	ALTER TABLE [dbo].[bp_rish_project_state]  WITH CHECK ADD  CONSTRAINT [FK_bp_rish_project_state_dict_rish_project_state] FOREIGN KEY([state_id])
	REFERENCES [dbo].[dict_rish_project_state] ([id])
	ON UPDATE CASCADE ON DELETE CASCADE;

	ALTER TABLE [dbo].[bp_rish_project_state] CHECK CONSTRAINT [FK_bp_rish_project_state_dict_rish_project_state];
END
GO

IF COLUMNPROPERTY(OBJECT_ID('bp_rish_project_info'), 'state_id', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE dbo.bp_rish_project_info
		DROP CONSTRAINT FK_bp_rish_project_info_dict_rish_project_state;

	ALTER TABLE bp_rish_project_info DROP COLUMN state_id;

	INSERT INTO bp_rish_project_state (project_id, state_id, entered_by, entered_on)
	SELECT project_id, 1, created_by, create_date FROM bp_rish_project_info
END
GO


/* fnTokenizeStringList - transform the delimited list of values into a table of values */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnTokenizeStringList]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fnTokenizeStringList]
GO

CREATE FUNCTION [dbo].[fnTokenizeStringList]
(	
	@STRINGLIST AS VARCHAR(MAX),
	@DELIMETER AS VARCHAR(MAX)
)
RETURNS @OUTPUT TABLE (STRINGS VARCHAR(MAX))
AS
BEGIN
	DECLARE @TOKEN AS VARCHAR(MAX)
	DECLARE @VALUES AS VARCHAR(MAX)

	WHILE LEN(@STRINGLIST) > 0
	BEGIN

		SET @VALUES = CHARINDEX(@DELIMETER, @STRINGLIST)
		IF @VALUES > 0
		BEGIN
			SET @TOKEN = SUBSTRING(@STRINGLIST, 1, @VALUES - 1)
			SET @STRINGLIST = SUBSTRING(@STRINGLIST, @VALUES + 1, LEN(@STRINGLIST))
		END
		IF @VALUES = 0
		BEGIN
			SET @TOKEN = @STRINGLIST
			SET @STRINGLIST = ''
		END

		INSERT INTO @OUTPUT (STRINGS) VALUES (CAST(@TOKEN AS varchar(max)) )

	END
	RETURN
END

GO

/******************************************************************************/
/*                  The new reporting system for organizations                */
/******************************************************************************/

/* The table below relates user accounts to 1NF organization IDs */

IF OBJECT_ID('reports1nf_accounts') IS NULL
BEGIN
	CREATE TABLE dbo.reports1nf_accounts
		(
		id INTEGER NOT NULL IDENTITY (1, 1) PRIMARY KEY,
		UserId UNIQUEIDENTIFIER NOT NULL,
		organization_id INTEGER NOT NULL,
		rda_district_id INTEGER,
		fin_add_showed INTEGER
		);
END

/* Reports prepared by organizations */

IF OBJECT_ID('reports1nf') IS NULL
BEGIN
	CREATE TABLE dbo.reports1nf
		(
		id INTEGER NOT NULL IDENTITY (1, 1) PRIMARY KEY,
		organization_id INTEGER NOT NULL,
		is_reviewed INTEGER DEFAULT 0,
		create_date DATETIME NOT NULL
		);
END

/* Information about organization - submitter of the report */

IF OBJECT_ID('reports1nf_org_info') IS NULL
BEGIN
	SELECT TOP 0 * INTO reports1nf_org_info FROM organizations
	
	ALTER TABLE reports1nf_org_info ADD report_id INTEGER NOT NULL
	ALTER TABLE reports1nf_org_info ADD submit_date DATETIME
	
	ALTER TABLE reports1nf_org_info ADD PRIMARY KEY (report_id);
	
	ALTER TABLE dbo.reports1nf_org_info ADD CONSTRAINT FK_reports1nf_org_info_reports1nf FOREIGN KEY
		(report_id) REFERENCES dbo.reports1nf (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

/* Information about buildings. This is a temporary storage for relating building information to balans objects */

IF OBJECT_ID('reports1nf_buildings') IS NULL
BEGIN
	SELECT TOP 0 * INTO reports1nf_buildings FROM buildings

	ALTER TABLE reports1nf_buildings ADD unique_id INTEGER IDENTITY(1,1)
	ALTER TABLE reports1nf_buildings ADD report_id INTEGER NOT NULL

	ALTER TABLE reports1nf_buildings ADD PRIMARY KEY (unique_id);
	
	ALTER TABLE dbo.reports1nf_buildings ADD CONSTRAINT FK_reports1nf_buildings_reports1nf FOREIGN KEY
		(report_id) REFERENCES dbo.reports1nf (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

/* Information about objects on balans for particular organization */

IF OBJECT_ID('reports1nf_balans') IS NULL
BEGIN
	SELECT TOP 0 * INTO reports1nf_balans FROM balans
	
	ALTER TABLE reports1nf_balans ADD report_id INTEGER NOT NULL
	ALTER TABLE reports1nf_balans ADD building_1nf_unique_id INTEGER
	ALTER TABLE reports1nf_balans ADD submit_date DATETIME
	ALTER TABLE reports1nf_balans ADD is_valid INTEGER
	ALTER TABLE reports1nf_balans ADD validation_errors TEXT

	ALTER TABLE reports1nf_balans ADD PRIMARY KEY (report_id, id);
	
	ALTER TABLE dbo.reports1nf_balans ADD CONSTRAINT FK_reports1nf_balans_reports1nf FOREIGN KEY
		(report_id) REFERENCES dbo.reports1nf (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

/* Information about deleted objects on balans for particular organization */

IF OBJECT_ID('reports1nf_balans_deleted') IS NULL
BEGIN
	SELECT TOP 0 * INTO reports1nf_balans_deleted FROM balans
	
	ALTER TABLE reports1nf_balans_deleted ADD report_id INTEGER NOT NULL
	ALTER TABLE reports1nf_balans_deleted ADD submit_date DATETIME
	ALTER TABLE reports1nf_balans_deleted ADD is_valid INTEGER
	ALTER TABLE reports1nf_balans_deleted ADD validation_errors TEXT

	ALTER TABLE reports1nf_balans_deleted ADD PRIMARY KEY (report_id, id);
	
	ALTER TABLE dbo.reports1nf_balans_deleted ADD CONSTRAINT FK_reports1nf_balans_deleted_reports1nf FOREIGN KEY
		(report_id) REFERENCES dbo.reports1nf (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

/* Information about rent agreements for particular organization */

IF OBJECT_ID('reports1nf_arenda') IS NULL
BEGIN
	SELECT TOP 0 * INTO reports1nf_arenda FROM arenda
	
	ALTER TABLE reports1nf_arenda ADD report_id INTEGER NOT NULL
	ALTER TABLE reports1nf_arenda ADD building_1nf_unique_id INTEGER
	ALTER TABLE reports1nf_arenda ADD submit_date DATETIME
	ALTER TABLE reports1nf_arenda ADD is_valid INTEGER
	ALTER TABLE reports1nf_arenda ADD validation_errors TEXT
	
	ALTER TABLE reports1nf_arenda ADD PRIMARY KEY (report_id, id);
	
	ALTER TABLE dbo.reports1nf_arenda ADD CONSTRAINT FK_reports1nf_arenda_reports1nf FOREIGN KEY
		(report_id) REFERENCES dbo.reports1nf (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

/* Individual objects within rent agreements */

IF OBJECT_ID('reports1nf_arenda_notes') IS NULL
BEGIN
	SELECT TOP 0 * INTO reports1nf_arenda_notes FROM arenda_notes
	
	ALTER TABLE reports1nf_arenda_notes ADD report_id INTEGER NOT NULL
	ALTER TABLE reports1nf_arenda_notes ADD PRIMARY KEY (report_id, id);

	ALTER TABLE dbo.reports1nf_arenda_notes ADD CONSTRAINT FK_reports1nf_arenda_notes_reports1nf FOREIGN KEY
		(report_id) REFERENCES dbo.reports1nf (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

/* Documents that confirm validity of the rent agreements */

IF OBJECT_ID('reports1nf_arenda_decisions') IS NULL
BEGIN
	SELECT TOP 0 * INTO reports1nf_arenda_decisions FROM link_arenda_2_decisions
		
	ALTER TABLE reports1nf_arenda_decisions ADD report_id INTEGER NOT NULL
	ALTER TABLE reports1nf_arenda_decisions ADD PRIMARY KEY (report_id, id);

	ALTER TABLE dbo.reports1nf_arenda_decisions ADD CONSTRAINT FK_reports1nf_arenda_decisions_reports1nf FOREIGN KEY
		(report_id) REFERENCES dbo.reports1nf (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

/* Rent agreements from the point of view of the renter */

IF OBJECT_ID('reports1nf_arenda_rented') IS NULL
BEGIN
	SELECT TOP 0 * INTO reports1nf_arenda_rented FROM arenda_rented
		
	ALTER TABLE reports1nf_arenda_rented ADD report_id INTEGER NOT NULL
	ALTER TABLE reports1nf_arenda_rented ADD arenda_rented_id INTEGER
	ALTER TABLE reports1nf_arenda_rented ADD building_1nf_unique_id INTEGER
	ALTER TABLE reports1nf_arenda_rented ADD submit_date DATETIME
	ALTER TABLE reports1nf_arenda_rented ADD is_valid INTEGER
	ALTER TABLE reports1nf_arenda_rented ADD validation_errors TEXT
	
	ALTER TABLE reports1nf_arenda_rented ADD PRIMARY KEY (report_id, id);

	ALTER TABLE dbo.reports1nf_arenda_rented ADD CONSTRAINT FK_reports1nf_arenda_rented_reports1nf FOREIGN KEY
		(report_id) REFERENCES dbo.reports1nf (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

/* Payment information related to the rent agreement */

IF OBJECT_ID('reports1nf_arenda_payments') IS NULL
BEGIN
	SELECT TOP 0 * INTO reports1nf_arenda_payments FROM arenda_payments

	ALTER TABLE reports1nf_arenda_payments DROP COLUMN id

	ALTER TABLE reports1nf_arenda_payments ADD id INTEGER IDENTITY(1,1)
	ALTER TABLE reports1nf_arenda_payments ADD report_id INTEGER NOT NULL
	ALTER TABLE reports1nf_arenda_payments ADD PRIMARY KEY (id);

	ALTER TABLE dbo.reports1nf_arenda_payments ADD CONSTRAINT FK_reports1nf_arenda_payments_reports1nf FOREIGN KEY
		(report_id) REFERENCES dbo.reports1nf (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

/* Submitter and/or GUKV personnel comments about the report */

IF OBJECT_ID('reports1nf_comments') IS NULL
BEGIN
	CREATE TABLE dbo.reports1nf_comments
		(
		id INTEGER NOT NULL IDENTITY (1, 1) PRIMARY KEY,
		report_id INTEGER NOT NULL,
		organization_id INTEGER, /* If comment is related to organization information, this field must be set */
		balans_id INTEGER, /* If comment is related to balans object, this field must be set */
		balans_deleted_id INTEGER, /* If comment is related to a deleted balans object, this field must be set */
		arenda_id INTEGER, /* If comment is related to a rent agreement, this field must be set */
		arenda_rented_id INTEGER, /* If comment is related to a rented object, this field must be set */
		control_id VARCHAR(64),
		control_title VARCHAR(256),
		comment VARCHAR(MAX),
		is_wrong_data INTEGER DEFAULT 0,
		comment_date DATETIME NOT NULL,
		comment_user VARCHAR(64)
		);

	ALTER TABLE dbo.reports1nf_comments ADD CONSTRAINT FK_reports1nf_comments_reports1nf FOREIGN KEY
		(report_id) REFERENCES dbo.reports1nf (id)
		 ON UPDATE CASCADE 
		 ON DELETE CASCADE;
END

/* платіжні квітанції */
IF OBJECT_ID('reports1nf_payment_documents') IS NULL
BEGIN
	CREATE TABLE dbo.reports1nf_payment_documents
		(
		id INTEGER NOT NULL IDENTITY (1, 1) PRIMARY KEY,
		report_id INTEGER NOT NULL,
		arenda_id INTEGER, 
		payment_date DATE NOT NULL,
		payment_number VARCHAR(64),
		payment_sum NUMERIC(15,2),
		payment_purpose VARCHAR(256) NULL,
		modify_date DATE NULL,
		modified_by VARCHAR(128) NULL,
		rent_period_id INTEGER NOT NULL  
		);
END


/* Фотографії / плани */
IF OBJECT_ID('reports1nf_photos') IS NULL
BEGIN
	CREATE TABLE [dbo].[reports1nf_photos]
		(
		[id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[bal_id] [int] NOT NULL,
		[file_name] [varchar](255) NOT NULL,
		[file_ext] [varchar](255) NOT NULL,
		[user_id] [uniqueidentifier] NOT NULL,
		[create_date] [datetime] NOT NULL
		);
END


GO

/* fnGenerateInitial1NFReport - generates the first report for an organization, using the existing information from EIS */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnGenerateInitial1NFReport]'))
	DROP PROCEDURE [dbo].[fnGenerateInitial1NFReport]
GO

CREATE PROCEDURE dbo.fnGenerateInitial1NFReport
(	
	@ORG_ID INTEGER,
	@REPORT_ID INTEGER OUTPUT
)
AS
	/* Generate a new report */
	DECLARE @TmpTableReportId TABLE (report_id INTEGER)
	
	INSERT INTO reports1nf (organization_id, create_date)
		OUTPUT INSERTED.id INTO @TmpTableReportId
		VALUES (@ORG_ID, GETDATE())
	
	SET @REPORT_ID = (select report_id from @TmpTableReportId)
	
	/* Copy general information about organization to the 'report1nf_org_info' table */
	INSERT INTO reports1nf_org_info
	SELECT
	   org.[id]
      ,org.[master_org_id]
      ,org.[last_state]
      ,org.[occupation_id]
      ,org.[status_id]
      ,org.[form_gosp_id]
      ,org.[form_ownership_id]
      ,org.[gosp_struct_id]
      ,org.[organ_id]
      ,org.[industry_id]
      ,org.[nomer_obj]
      ,org.[zkpo_code]
      ,org.[addr_distr_old_id]
      ,org.[addr_distr_new_id]
      ,org.[addr_street_name]
      ,org.[addr_street_id]
      ,org.[addr_nomer]
      ,org.[addr_nomer2]
      ,org.[addr_korpus]
      ,org.[addr_zip_code]
      ,org.[addr_misc]
      ,org.[director_fio]
      ,org.[director_phone]
      ,org.[director_fio_kogo]
      ,org.[director_title]
      ,org.[director_title_kogo]
      ,org.[director_doc]
      ,org.[director_doc_kogo]
      ,org.[director_email]
      ,org.[buhgalter_fio]
      ,org.[buhgalter_phone]
      ,org.[num_buildings]
      ,org.[full_name]
      ,org.[short_name]
      ,org.[priznak_id]
      ,org.[title_form_id]
      ,org.[form_1nf_id]
      ,org.[vedomstvo_id]
      ,org.[title_id]
      ,org.[form_id]
      ,org.[gosp_struct_type_id]
      ,org.[search_name]
      ,org.[name_komu]
      ,org.[fax]
      ,org.[registration_auth]
      ,org.[registration_num]
      ,org.[registration_date]
      ,org.[registration_svidot]
      ,org.[l_year]
      ,org.[date_l_year]
      ,org.[sqr_on_balance]
      ,org.[sqr_manufact]
      ,org.[sqr_non_manufact]
      ,org.[sqr_free_for_rent]
      ,org.[sqr_total]
      ,org.[sqr_rented]
      ,org.[sqr_privat]
      ,org.[sqr_given_for_rent]
      ,org.[sqr_znyata_z_balansu]
      ,org.[sqr_prodaj]
      ,org.[sqr_spisani_zneseni]
      ,org.[sqr_peredana]
      ,org.[num_objects]
      ,org.[kved_code]
      ,org.[date_stat_spravka]
      ,org.[koatuu]
      ,org.[modified_by]
      ,org.[modify_date]
      ,org.[share_type_id]
      ,org.[share]
      ,org.[bank_name]
      ,org.[bank_mfo]
      ,org.[is_deleted]
      ,org.[del_date]
      ,org.[otdel_gukv_id]
      ,org.[arch_id]
      ,org.[arch_flag]
      ,org.[is_liquidated]
      ,org.[liquidation_date]
      ,org.[pidp_rda]
      ,org.[is_arend]
      ,org.[beg_state_date]
      ,org.[end_state_date]
      ,org.[mayno_id]
      ,org.[contact_email]
      ,org.[contact_posada_id]
      ,org.[nadhodjennya_id]
      ,org.[vibuttya_id]
      ,org.[privat_status_id]
      ,org.[cur_state_id]
      ,org.[sfera_upr_id]
      ,org.[plan_zone_id]
      ,org.[registr_org_id]
      ,org.[nadhodjennya_date]
      ,org.[vibuttya_date]
      ,org.[chastka]
      ,org.[registration_rish]
      ,org.[registration_dov_date]
      ,org.[registration_corp]
      ,org.[strok_start_date]
      ,org.[strok_end_date]
      ,org.[stat_fond]
      ,org.[size_plus]
      ,org.[addr_zip_code_3]
      ,org.[old_industry_id]
      ,org.[old_occupation_id]
      ,org.[old_organ_id]
      ,org.[form_vlasn_vibuttya_id]
      ,org.[addr_city]
      ,org.[addr_flat_num]
      ,org.[povnovajennia]
      ,org.[povnov_osoba_fio]
      ,org.[povnov_passp_seria]
      ,org.[povnov_passp_num]
      ,org.[povnov_passp_auth]
      ,org.[povnov_passp_date]
      ,org.[director_passp_seria]
      ,org.[director_passp_num]
      ,org.[director_passp_auth]
      ,org.[director_passp_date]
      ,org.[registration_svid_date]
      ,org.[origin_db]
      ,org.[budg_payments_rate]
      ,org.[is_under_closing]
      ,org.[phys_addr_street_id]
      ,org.[phys_addr_district_id]
      ,org.[phys_addr_nomer]
      ,org.[phys_addr_zip_code]
      ,org.[phys_addr_misc]
      ,org.[contribution_rate]
      ,@REPORT_ID AS 'report_id'
      ,NULL AS 'submit_date'
      ,org.[budget_narah_50_uah]
      ,org.[budget_zvit_50_uah]
      ,org.[budget_prev_50_uah]
      ,org.[budget_debt_30_50_uah]
      ,org.[is_special_organization]
      ,org.[payment_budget_special]
      ,org.buhgalter_email
      ,org.konkurs_payments
      ,org.unknown_payments
      ,org.unknown_payment_note
	FROM organizations org
	WHERE org.id = @ORG_ID
		
	/* Update the street ID (sometimes only street name is entered) */
	DECLARE @STREET_NAME VARCHAR(128)
	SET @STREET_NAME = (select ltrim(rtrim(addr_street_name)) FROM reports1nf_org_info WHERE report_id = @REPORT_ID)
	
	IF LEN(@STREET_NAME) > 0
	BEGIN
		DECLARE @STREET_ID INTEGER
		SET @STREET_ID = (select id from dict_streets WHERE ltrim(rtrim(name)) = @STREET_NAME)
		
		IF @STREET_ID > 0
		BEGIN
			UPDATE reports1nf_org_info SET addr_street_id = @STREET_ID WHERE report_id = @REPORT_ID
		END
	END
	
	/* Copy the legal address to the physical address (because physical address is empty by default) */
	UPDATE reports1nf_org_info SET
		phys_addr_street_id = addr_street_id,
		phys_addr_district_id = addr_distr_new_id,
		phys_addr_nomer = addr_nomer,
		phys_addr_zip_code = addr_zip_code
	WHERE report_id = @REPORT_ID
	
	/* Copy information about balans objects to the 'report1nf_balans' table */
	INSERT INTO reports1nf_balans (
	
	[id]
      ,[year_balans]
      ,[building_id]
      ,[organization_id]
      ,[sqr_total]
      ,[sqr_pidval]
      ,[sqr_vlas_potreb]
      ,[sqr_free]
      ,[sqr_in_rent]
      ,[sqr_privatizov]
      ,[sqr_not_for_rent]
      ,[sqr_gurtoj]
      ,[sqr_non_habit]
      ,[sqr_kor]
      ,[cost_balans]
      ,[cost_fair]
      ,[cost_expert_1m]
      ,[cost_expert_total]
      ,[cost_rent_narah]
      ,[cost_rent_payed]
      ,[cost_debt]
      ,[cost_zalishkova]
      ,[cost_rinkova]
      ,[cost_fair_1m]
      ,[num_people]
      ,[num_rent_agr]
      ,[num_privat_apt]
      ,[o26_id]
      ,[bti_id]
      ,[approval_by]
      ,[approval_num]
      ,[approval_date]
      ,[form_ownership_id]
      ,[object_kind_id]
      ,[object_type_id]
      ,[history_id]
      ,[tech_condition_id]
      ,[purpose_group_id]
      ,[purpose_id]
      ,[purpose_str]
      ,[floors]
      ,[priznak_1nf]
      ,[ownership_type_id]
      ,[obj_street_name]
      ,[obj_street_id]
      ,[obj_street_name2]
      ,[obj_street_id2]
      ,[obj_nomer1]
      ,[obj_nomer2]
      ,[obj_nomer3]
      --,[obj_nomer]
      ,[obj_bti_code]
      ,[obj_addr_misc]
      ,[obj_street_misc]
      ,[modified_by]
      ,[modify_date]
      ,[form_giver_id]
      ,[org_maintain_id]
      ,[update_src_id]
      ,[is_deleted]
      ,[del_date]
      ,[znos]
      ,[znos_date]
      ,[date_expert]
      ,[reestr_no]
      ,[fair_cost_date]
      ,[otdel_gukv_id]
      ,[date_bti]
      ,[arch_id]
      ,[arch_flag]
      ,[date_cost_rinkova]
      ,[memo]
      ,[note]
      ,[is_free_sqr]
      ,[free_sqr_useful]
      ,[free_sqr_condition_id]
      ,[free_sqr_location]
      ,[ownership_doc_type]
      ,[ownership_doc_num]
      ,[ownership_doc_date]
      ,[balans_doc_type]
      ,[balans_doc_num]
      ,[balans_doc_date]
      ,[vidch_doc_type]
      ,[vidch_doc_num]
      ,[vidch_doc_date]
      ,[vidch_type_id]
      ,[vidch_org_id]
      ,[vidch_cost]
      ,[sqr_engineering]
      ,[obj_status_id]
      ,[report_id]
      ,[building_1nf_unique_id]
      ,[submit_date]
      ,[is_valid]
      ,[validation_errors]
	)
	
	SELECT 
	
	bal.[id]
      ,bal.[year_balans]
      ,bal.[building_id]
      ,bal.[organization_id]
      ,bal.[sqr_total]
      ,bal.[sqr_pidval]
      ,bal.[sqr_vlas_potreb]
      ,bal.[sqr_free]
      ,bal.[sqr_in_rent]
      ,bal.[sqr_privatizov]
      ,bal.[sqr_not_for_rent]
      ,bal.[sqr_gurtoj]
      ,bal.[sqr_non_habit]
      ,bal.[sqr_kor]
      ,bal.[cost_balans]
      ,bal.[cost_fair]
      ,bal.[cost_expert_1m]
      ,bal.[cost_expert_total]
      ,bal.[cost_rent_narah]
      ,bal.[cost_rent_payed]
      ,bal.[cost_debt]
      ,bal.[cost_zalishkova]
      ,bal.[cost_rinkova]
      ,bal.[cost_fair_1m]
      ,bal.[num_people]
      ,bal.[num_rent_agr]
      ,bal.[num_privat_apt]
      ,bal.[o26_id]
      ,bal.[bti_id]
      ,bal.[approval_by]
      ,bal.[approval_num]
      ,bal.[approval_date]
      ,bal.[form_ownership_id]
      ,bal.[object_kind_id]
      ,bal.[object_type_id]
      ,bal.[history_id]
      ,bal.[tech_condition_id]
      ,bal.[purpose_group_id]
      ,bal.[purpose_id]
      ,bal.[purpose_str]
      ,bal.[floors]
      ,bal.[priznak_1nf]
      ,bal.[ownership_type_id]
      ,bal.[obj_street_name]
      ,bal.[obj_street_id]
      ,bal.[obj_street_name2]
      ,bal.[obj_street_id2]
      ,bal.[obj_nomer1]
      ,bal.[obj_nomer2]
      ,bal.[obj_nomer3]
      --,[obj_nomer]
      ,bal.[obj_bti_code]
      ,bal.[obj_addr_misc]
      ,bal.[obj_street_misc]
      ,bal.[modified_by]
      ,bal.[modify_date]
      ,bal.[form_giver_id]
      ,bal.[org_maintain_id]
      ,bal.[update_src_id]
      ,bal.[is_deleted]
      ,bal.[del_date]
      ,bal.[znos]
      ,bal.[znos_date]
      ,bal.[date_expert]
      ,bal.[reestr_no]
      ,bal.[fair_cost_date]
      ,bal.[otdel_gukv_id]
      ,bal.[date_bti]
      ,bal.[arch_id]
      ,bal.[arch_flag]
      ,bal.[date_cost_rinkova]
      ,bal.[memo]
      ,bal.[note]
      ,bal.[is_free_sqr]
      ,bal.[free_sqr_useful]
      ,bal.[free_sqr_condition_id]
      ,bal.[free_sqr_location]
      ,bal.[ownership_doc_type]
      ,bal.[ownership_doc_num]
      ,bal.[ownership_doc_date]
      ,bal.[balans_doc_type]
      ,bal.[balans_doc_num]
      ,bal.[balans_doc_date]
      ,bal.[vidch_doc_type]
      ,bal.[vidch_doc_num]
      ,bal.[vidch_doc_date]
      ,bal.[vidch_type_id]
      ,bal.[vidch_org_id]
      ,bal.[vidch_cost]
      ,bal.[sqr_engineering]
      ,bal.[obj_status_id]
	
	
		,@REPORT_ID AS 'report_id',
		NULL AS 'building_1nf_unique_id',
		NULL AS 'submit_date',
		0 as 'is_valid',
		'' AS 'validation_errors'
	FROM balans bal
	WHERE bal.organization_id = @ORG_ID AND (bal.is_deleted IS NULL OR bal.is_deleted = 0)
		
	/* Copy information about deleted balans objects to the 'report1nf_balans_deleted' table */
	INSERT INTO reports1nf_balans_deleted
	SELECT bal.*,
		@REPORT_ID AS 'report_id',
		NULL AS 'submit_date',
		0 as 'is_valid',
		'' AS 'validation_errors'
	FROM balans bal
	WHERE bal.organization_id = @ORG_ID AND bal.is_deleted > 0
		
	/* Copy building information for each balans object entry */
	DECLARE @BALANS_ID INTEGER
	DECLARE @BALANS_BUILDING_ID INTEGER
	DECLARE @BUILDING_UNIQUE_ID INTEGER
	DECLARE @TmpTableBuildingUniqueId TABLE (unique_id INTEGER)

	DECLARE Balans_Obj_Cursor CURSOR FOR SELECT id, building_id FROM reports1nf_balans WHERE report_id = @REPORT_ID;

	OPEN Balans_Obj_Cursor;

	FETCH NEXT FROM Balans_Obj_Cursor INTO @BALANS_ID, @BALANS_BUILDING_ID;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		/* Copy a single row to the 'reports1nf_buildings' table */
		DELETE FROM @TmpTableBuildingUniqueId

		INSERT INTO reports1nf_buildings OUTPUT INSERTED.unique_id INTO @TmpTableBuildingUniqueId
			SELECT b.*, @REPORT_ID AS 'report_id' FROM buildings b WHERE b.id = @BALANS_BUILDING_ID

		SET @BUILDING_UNIQUE_ID = (select unique_id from @TmpTableBuildingUniqueId)

		/* Update the balans record: save the unique building id */
		UPDATE reports1nf_balans SET building_1nf_unique_id = @BUILDING_UNIQUE_ID
			WHERE report_id = @REPORT_ID AND id = @BALANS_ID

		/* Go to the next balans record */
		FETCH NEXT FROM Balans_Obj_Cursor INTO @BALANS_ID, @BALANS_BUILDING_ID;
	END;

	CLOSE Balans_Obj_Cursor;
	DEALLOCATE Balans_Obj_Cursor;
	
	/* Copy information about rent agreements to the 'report1nf_arenda' table */
	INSERT INTO reports1nf_arenda SELECT
		 ar.[id]
		,ar.[building_id]
		,ar.[org_balans_id]
		,ar.[org_renter_id]
		,ar.[org_giver_id]
		,ar.[balans_id]
		,ar.[rent_year]
		,ar.[object_kind_id]
		,ar.[purpose_group_id]
		,ar.[purpose_id]
		,ar.[purpose_str]
		,ar.[name]
		,ar.[is_privat]
		,ar.[update_src_id]
		,ar.[agreement_kind_id]
		,ar.[agreement_date]
		,ar.[agreement_num]
		,ar.[agreement_str]
		,ar.[floor_number]
		,ar.[num_people]
		,ar.[cost_narah]
		,ar.[cost_payed]
		,ar.[cost_debt]
		,ar.[cost_agreement]
		,ar.[cost_expert_1m]
		,ar.[cost_expert_total]
		,ar.[pidstava]
		,ar.[pidstava_date]
		,ar.[pidstava_num]
		,ar.[pidstava_fact]
		,ar.[pidstava2]
		,ar.[pidstava_num2]
		,ar.[pidstava_date2]
		,ar.[pidstava_display]
		,ar.[rent_start_date]
		,ar.[rent_finish_date]
		,ar.[rent_actual_finish_date]
		,ar.[rent_rate]
		,ar.[rent_rate_uah]
		,ar.[rent_square]
		,ar.[priznak_1nf]
		,ar.[debt_timespan]
		,ar.[order_num]
		,ar.[order_date]
		,ar.[order_no2]
		,ar.[rishennya_id]
		,ar.[is_inactive]
		,ar.[inactive_date]
		,ar.[is_deleted]
		,ar.[del_date]
		,ar.[date_expert]
		,ar.[is_subarenda]
		,ar.[privat_kind_id]
		,ar.[num_primirnikiv]
		,ar.[date_expl_enter]
		,ar.[num_akt]
		,ar.[date_akt]
		,ar.[num_bti]
		,ar.[date_bti]
		,ar.[svidotstvo_serial]
		,ar.[svidotstvo_num]
		,ar.[svidotstvo_date]
		,ar.[payment_type_id]
		,ar.[arch_id]
		,ar.[modified_by]
		,ar.[modify_date]
		,ar.[note]
		,ar.[agreement_state]
		,ar.[is_insured]
		,ar.[insurance_start]
		,ar.[insurance_end]
		,ar.[insurance_sum]
	    ,@REPORT_ID AS 'report_id'
	    ,NULL AS 'submit_date'
	    ,NULL AS 'building_1nf_unique_id'
	    ,ar.[is_loan_agreement]
	    ,0 as 'is_valid'
	    ,'' AS 'validation_errors'
	FROM arenda ar
	WHERE ar.org_balans_id = @ORG_ID AND (ar.is_deleted IS NULL OR ar.is_deleted = 0) AND (ar.is_privat IS NULL OR ar.is_privat = 0)
				
	/* Copy building information and all the related objects (notes, decisions, payments) for each rent agreement */
	DECLARE @ARENDA_ID INTEGER
	DECLARE @ARENDA_BUILDING_ID INTEGER
	DECLARE Arenda_Obj_Cursor CURSOR FOR SELECT id, building_id FROM reports1nf_arenda WHERE report_id = @REPORT_ID;

	OPEN Arenda_Obj_Cursor;

	FETCH NEXT FROM Arenda_Obj_Cursor INTO @ARENDA_ID, @ARENDA_BUILDING_ID;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		/* Copy a single row to the 'reports1nf_buildings' table */
		DELETE FROM @TmpTableBuildingUniqueId

		INSERT INTO reports1nf_buildings OUTPUT INSERTED.unique_id INTO @TmpTableBuildingUniqueId
			SELECT b.*, @REPORT_ID AS 'report_id' FROM buildings b WHERE b.id = @ARENDA_BUILDING_ID

		SET @BUILDING_UNIQUE_ID = (select unique_id from @TmpTableBuildingUniqueId)

		/* Update the rent agreement record: save the unique building id */
		UPDATE reports1nf_arenda SET building_1nf_unique_id = @BUILDING_UNIQUE_ID
			WHERE report_id = @REPORT_ID AND id = @ARENDA_ID

		/* Create a new entry in the 'reports1nf_arenda_payments' table */
		INSERT INTO reports1nf_arenda_payments (report_id, arenda_id) VALUES (@REPORT_ID, @ARENDA_ID)

		/* Copy all the notes for this rent agreement */
		INSERT INTO reports1nf_arenda_notes
			SELECT arenda_id, purpose_group_id, purpose_id, purpose_str, rent_square, modify_date, modified_by, note,
				rent_rate, rent_rate_uah, cost_narah, cost_agreement, is_deleted, del_date, cost_expert_total,
				date_expert, payment_type_id, invent_no, note_status_id, @REPORT_ID AS 'report_id'
				FROM arenda_notes WHERE arenda_id = @ARENDA_ID AND (is_deleted IS NULL OR is_deleted = 0)

		/* Copy all decisions for this rent agreement */
		INSERT INTO reports1nf_arenda_decisions
			SELECT arenda_id, rishen_id, ord, modified_by, modify_date, doc_num, doc_date, doc_dodatok, doc_punkt, purpose_str,
				rent_square, decision_id, doc_raspor_id, pidstava, @REPORT_ID AS 'report_id'
				FROM link_arenda_2_decisions WHERE arenda_id = @ARENDA_ID

		/* Go to the next rent agreement */
		FETCH NEXT FROM Arenda_Obj_Cursor INTO @ARENDA_ID, @ARENDA_BUILDING_ID;
	END;

	CLOSE Arenda_Obj_Cursor;
	DEALLOCATE Arenda_Obj_Cursor;
	
	/* Erase some outdated values from 'dict_arenda_payment_type' dictionary */
	UPDATE reports1nf_arenda SET payment_type_id = 10 WHERE (report_id = @REPORT_ID) AND NOT (payment_type_id IN (3, 7, 8, 10, 11))
	UPDATE reports1nf_arenda_notes SET payment_type_id = 10 WHERE (report_id = @REPORT_ID) AND NOT (payment_type_id IN (3, 7, 8, 10, 11))
	
	/* Copy information about rent agreements (from the point of view of the renter) to the 'report1nf_arenda_rented' table */
	DELETE FROM arenda_rented WHERE org_renter_id = @ORG_ID
	
	INSERT INTO arenda_rented
		(building_id, org_renter_id, payment_type_id, agreement_date, agreement_num, rent_start_date, rent_finish_date,
		is_subarenda, modified_by, modify_date, is_deleted, del_date, rent_square)
	SELECT ar.building_id, ar.org_renter_id, ar.payment_type_id, ar.agreement_date, ar.agreement_num, ar.rent_start_date, ar.rent_finish_date,
		ar.is_subarenda, ar.modified_by, ar.modify_date, ar.is_deleted, ar.del_date, ar.rent_square
		 FROM arenda ar
	WHERE ar.org_renter_id = @ORG_ID AND (ar.is_deleted IS NULL OR ar.is_deleted = 0) AND (ar.is_privat IS NULL OR ar.is_privat = 0)
	
	INSERT INTO reports1nf_arenda_rented
		(building_id, org_renter_id, payment_type_id, agreement_date, agreement_num, rent_start_date, rent_finish_date,
		is_subarenda, modified_by, modify_date, is_deleted, del_date, rent_square,
		report_id, arenda_rented_id, submit_date, is_valid, validation_errors)
	SELECT ar.building_id, ar.org_renter_id, ar.payment_type_id, ar.agreement_date, ar.agreement_num, ar.rent_start_date, ar.rent_finish_date,
		ar.is_subarenda, ar.modified_by, ar.modify_date, ar.is_deleted, ar.del_date, ar.rent_square,
		@REPORT_ID AS 'report_id', ar.id as 'arenda_rented_id', NULL AS 'submit_date', 0 AS 'is_valid', '' AS 'validation_errors' FROM arenda_rented ar
	WHERE ar.org_renter_id = @ORG_ID
	
	DECLARE Rented_Obj_Cursor CURSOR FOR SELECT id, building_id FROM reports1nf_arenda_rented WHERE report_id = @REPORT_ID;

	OPEN Rented_Obj_Cursor;

	FETCH NEXT FROM Rented_Obj_Cursor INTO @ARENDA_ID, @ARENDA_BUILDING_ID;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		/* Copy a single row to the 'reports1nf_buildings' table */
		DELETE FROM @TmpTableBuildingUniqueId

		INSERT INTO reports1nf_buildings OUTPUT INSERTED.unique_id INTO @TmpTableBuildingUniqueId
			SELECT b.*, @REPORT_ID AS 'report_id' FROM buildings b WHERE b.id = @ARENDA_BUILDING_ID

		SET @BUILDING_UNIQUE_ID = (select unique_id from @TmpTableBuildingUniqueId)

		/* Update the rented object record: save the unique building id */
		UPDATE reports1nf_arenda_rented SET building_1nf_unique_id = @BUILDING_UNIQUE_ID
			WHERE report_id = @REPORT_ID AND id = @ARENDA_ID

		/* Go to the next rented object */
		FETCH NEXT FROM Rented_Obj_Cursor INTO @ARENDA_ID, @ARENDA_BUILDING_ID;
	END;

	CLOSE Rented_Obj_Cursor;
	DEALLOCATE Rented_Obj_Cursor;
GO

/* fnCopy1NFReport - creates a copy of 1NF report */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnCopy1NFReport]'))
	DROP PROCEDURE [dbo].[fnCopy1NFReport]
GO

/* fnCreateRentedObject - generates an empty entry in the 'arenda_rented' table */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnCreateRentedObject]'))
	DROP PROCEDURE [dbo].[fnCreateRentedObject]
GO

CREATE PROCEDURE dbo.fnCreateRentedObject
(	
	@ARENDA_RENTED_ID INTEGER OUTPUT
)
AS
	DECLARE @TmpTable TABLE (arenda_rented_id INTEGER)
	
	INSERT INTO arenda_rented (is_cmk) OUTPUT INSERTED.id INTO @TmpTable VALUES (0)

	SET @ARENDA_RENTED_ID = (select arenda_rented_id from @TmpTable)
GO

/* fnCreateRentAgreement - generates an empty entry in the 'arenda' table */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnCreateRentAgreement]'))
	DROP PROCEDURE [dbo].[fnCreateRentAgreement]
GO

CREATE PROCEDURE dbo.fnCreateRentAgreement
(	
	@ARENDA_ID INTEGER OUTPUT
)
AS
	SET @ARENDA_ID = (select MAX(id) + 1 from arenda)
	
	INSERT INTO arenda (id, is_deleted) VALUES (@ARENDA_ID, 0)
GO

/* fnCreateUniqueBuilding - copies a single row from the 'buildings' table to the 'reports1nf_buildings' table */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnCreateUniqueBuilding]'))
	DROP PROCEDURE [dbo].[fnCreateUniqueBuilding]
GO

CREATE PROCEDURE dbo.fnCreateUniqueBuilding
(	
	@BUILDING_ID INTEGER,
	@REPORT_ID INTEGER,
	@BUILDING_UNIQUE_ID INTEGER OUTPUT
)
AS
	DECLARE @TmpTable TABLE (unique_id INTEGER)
	
	INSERT INTO reports1nf_buildings OUTPUT INSERTED.unique_id INTO @TmpTable
		SELECT b.*, @REPORT_ID AS 'report_id' FROM buildings b WHERE b.id = @BUILDING_ID

	SET @BUILDING_UNIQUE_ID = (select unique_id from @TmpTable)
GO

/* fnAddBalansObjectToReport - adds a single balans object from EIS to the user report */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnAddBalansObjectToReport]'))
	DROP PROCEDURE [dbo].[fnAddBalansObjectToReport]
GO

CREATE PROCEDURE dbo.fnAddBalansObjectToReport
(	
	@BALANS_ID INTEGER
)
AS
	/* Check if report exists for this organization, before doing anything else */
	IF EXISTS (SELECT rep.id FROM reports1nf rep INNER JOIN balans bal ON bal.organization_id = rep.organization_id WHERE bal.id = @BALANS_ID)
	BEGIN
		/* If this balans object is already present in the report, do nothing */
		IF NOT EXISTS (SELECT id FROM reports1nf_balans WHERE id = @BALANS_ID) 
		BEGIN
			/* Get a report ID */
			DECLARE @REPORT_ID INTEGER
			DECLARE @BALANS_BUILDING_ID INTEGER
			
			SELECT @REPORT_ID = rep.id, @BALANS_BUILDING_ID = bal.building_id
				FROM reports1nf rep
				INNER JOIN balans bal ON bal.organization_id = rep.organization_id
				WHERE bal.id = @BALANS_ID

			/* Copy information about balans objects to the 'report1nf_balans' table */
			INSERT INTO reports1nf_balans
			([id]
      ,[year_balans]
      ,[building_id]
      ,[organization_id]
      ,[sqr_total]
      ,[sqr_pidval]
      ,[sqr_vlas_potreb]
      ,[sqr_free]
      ,[sqr_in_rent]
      ,[sqr_privatizov]
      ,[sqr_not_for_rent]
      ,[sqr_gurtoj]
      ,[sqr_non_habit]
      ,[sqr_kor]
      ,[cost_balans]
      ,[cost_fair]
      ,[cost_expert_1m]
      ,[cost_expert_total]
      ,[cost_rent_narah]
      ,[cost_rent_payed]
      ,[cost_debt]
      ,[cost_zalishkova]
      ,[cost_rinkova]
      ,[cost_fair_1m]
      ,[num_people]
      ,[num_rent_agr]
      ,[num_privat_apt]
      ,[o26_id]
      ,[bti_id]
      ,[approval_by]
      ,[approval_num]
      ,[approval_date]
      ,[form_ownership_id]
      ,[object_kind_id]
      ,[object_type_id]
      ,[history_id]
      ,[tech_condition_id]
      ,[purpose_group_id]
      ,[purpose_id]
      ,[purpose_str]
      ,[floors]
      ,[priznak_1nf]
      ,[ownership_type_id]
      ,[obj_street_name]
      ,[obj_street_id]
      ,[obj_street_name2]
      ,[obj_street_id2]
      ,[obj_nomer1]
      ,[obj_nomer2]
      ,[obj_nomer3]
      ,[obj_bti_code]
      ,[obj_addr_misc]
      ,[obj_street_misc]
      ,[modified_by]
      ,[modify_date]
      ,[form_giver_id]
      ,[org_maintain_id]
      ,[update_src_id]
      ,[is_deleted]
      ,[del_date]
      ,[znos]
      ,[znos_date]
      ,[date_expert]
      ,[reestr_no]
      ,[fair_cost_date]
      ,[otdel_gukv_id]
      ,[date_bti]
      ,[arch_id]
      ,[arch_flag]
      ,[date_cost_rinkova]
      ,[memo]
      ,[note]
      ,[is_free_sqr]
      ,[free_sqr_useful]
      ,[free_sqr_condition_id]
      ,[free_sqr_location]
      ,[ownership_doc_type]
      ,[ownership_doc_num]
      ,[ownership_doc_date]
      ,[balans_doc_type]
      ,[balans_doc_num]
      ,[balans_doc_date]
      ,[vidch_doc_type]
      ,[vidch_doc_num]
      ,[vidch_doc_date]
      ,[vidch_type_id]
      ,[vidch_org_id]
      ,[vidch_cost]
      ,[sqr_engineering]
      ,[obj_status_id]
      ,[report_id]
      ,[building_1nf_unique_id]
      ,[submit_date]
      ,[is_valid]
      ,[validation_errors])
			
			
			SELECT bal.[id]
      ,bal.[year_balans]
      ,bal.[building_id]
      ,bal.[organization_id]
      ,bal.[sqr_total]
      ,bal.[sqr_pidval]
      ,bal.[sqr_vlas_potreb]
      ,bal.[sqr_free]
      ,bal.[sqr_in_rent]
      ,bal.[sqr_privatizov]
      ,bal.[sqr_not_for_rent]
      ,bal.[sqr_gurtoj]
      ,bal.[sqr_non_habit]
      ,bal.[sqr_kor]
      ,bal.[cost_balans]
      ,bal.[cost_fair]
      ,bal.[cost_expert_1m]
      ,bal.[cost_expert_total]
      ,bal.[cost_rent_narah]
      ,bal.[cost_rent_payed]
      ,bal.[cost_debt]
      ,bal.[cost_zalishkova]
      ,bal.[cost_rinkova]
      ,bal.[cost_fair_1m]
      ,bal.[num_people]
      ,bal.[num_rent_agr]
      ,bal.[num_privat_apt]
      ,bal.[o26_id]
      ,bal.[bti_id]
      ,bal.[approval_by]
      ,bal.[approval_num]
      ,bal.[approval_date]
      ,bal.[form_ownership_id]
      ,bal.[object_kind_id]
      ,bal.[object_type_id]
      ,bal.[history_id]
      ,bal.[tech_condition_id]
      ,bal.[purpose_group_id]
      ,bal.[purpose_id]
      ,bal.[purpose_str]
      ,bal.[floors]
      ,bal.[priznak_1nf]
      ,bal.[ownership_type_id]
      ,bal.[obj_street_name]
      ,bal.[obj_street_id]
      ,bal.[obj_street_name2]
      ,bal.[obj_street_id2]
      ,bal.[obj_nomer1]
      ,bal.[obj_nomer2]
      ,bal.[obj_nomer3]
      ,bal.[obj_bti_code]
      ,bal.[obj_addr_misc]
      ,bal.[obj_street_misc]
      ,bal.[modified_by]
      ,bal.[modify_date]
      ,bal.[form_giver_id]
      ,bal.[org_maintain_id]
      ,bal.[update_src_id]
      ,bal.[is_deleted]
      ,bal.[del_date]
      ,bal.[znos]
      ,bal.[znos_date]
      ,bal.[date_expert]
      ,bal.[reestr_no]
      ,bal.[fair_cost_date]
      ,bal.[otdel_gukv_id]
      ,bal.[date_bti]
      ,bal.[arch_id]
      ,bal.[arch_flag]
      ,bal.[date_cost_rinkova]
      ,bal.[memo]
      ,bal.[note]
      ,bal.[is_free_sqr]
      ,bal.[free_sqr_useful]
      ,bal.[free_sqr_condition_id]
      ,bal.[free_sqr_location]
      ,bal.[ownership_doc_type]
      ,bal.[ownership_doc_num]
      ,bal.[ownership_doc_date]
      ,bal.[balans_doc_type]
      ,bal.[balans_doc_num]
      ,bal.[balans_doc_date]
      ,bal.[vidch_doc_type]
      ,bal.[vidch_doc_num]
      ,bal.[vidch_doc_date]
      ,bal.[vidch_type_id]
      ,bal.[vidch_org_id]
      ,bal.[vidch_cost]
      ,bal.[sqr_engineering]
      ,bal.[obj_status_id]
			,
				@REPORT_ID AS 'report_id',
				NULL AS 'building_1nf_unique_id',
				NULL AS 'submit_date',
				0 AS 'is_valid',
				'' AS 'validation_errors'
			FROM balans bal
			WHERE bal.id = @BALANS_ID

			/* Copy building information */
			DECLARE @BUILDING_UNIQUE_ID INTEGER
			DECLARE @TmpTableBuildingUniqueId TABLE (unique_id INTEGER)
			
			INSERT INTO reports1nf_buildings OUTPUT INSERTED.unique_id INTO @TmpTableBuildingUniqueId
				SELECT b.*, @REPORT_ID AS 'report_id' FROM buildings b WHERE b.id = @BALANS_BUILDING_ID

			SET @BUILDING_UNIQUE_ID = (SELECT unique_id FROM @TmpTableBuildingUniqueId)

			UPDATE reports1nf_balans SET building_1nf_unique_id = @BUILDING_UNIQUE_ID
				WHERE report_id = @REPORT_ID AND id = @BALANS_ID
		END
	END
GO

/* fnDeleteBalansObjectInReport - moves the balans object from 'reports1nf_balans' table to 'reports1nf_balans_deleted' table */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnDeleteBalansObjectInReport]'))
	DROP PROCEDURE [dbo].[fnDeleteBalansObjectInReport]
GO

CREATE PROCEDURE dbo.fnDeleteBalansObjectInReport
(	
	@BALANS_ID INTEGER,
	@COPY_TO_DELETED BIT,
	@VIDCH_TYPE_ID INTEGER,
	@VIDCH_DOC_NUM VARCHAR(24),
	@VIDCH_DOC_DATE datetime,
	@VIDCH_ORG_ID INTEGER
)
AS
	IF EXISTS (SELECT id FROM reports1nf_balans WHERE id = @BALANS_ID)
	BEGIN
		/* Get some properties of the primary balans object (which is already deleted) */
		DECLARE @MODIFIED_BY VARCHAR(128)
		DECLARE @MODIFY_DATE DATETIME
		DECLARE @DEL_DATE DATE
		
		SELECT
			@MODIFIED_BY = bal.modified_by,
			@MODIFY_DATE = bal.modify_date,
			@DEL_DATE = bal.del_date
		FROM balans bal
		WHERE bal.id = @BALANS_ID

		IF (@COPY_TO_DELETED = 1)
		BEGIN
			/* Copy the object to the 'reports1nf_balans_deleted' table */
			INSERT INTO reports1nf_balans_deleted
			SELECT 
				 [id]
				,[year_balans]
				,[building_id]
				,[organization_id]
				,[sqr_total]
				,[sqr_pidval]
				,[sqr_vlas_potreb]
				,[sqr_free]
				,[sqr_in_rent]
				,[sqr_privatizov]
				,[sqr_not_for_rent]
				,[sqr_gurtoj]
				,[sqr_non_habit]
				,[sqr_kor]
				,[cost_balans]
				,[cost_fair]
				,[cost_expert_1m]
				,[cost_expert_total]
				,[cost_rent_narah]
				,[cost_rent_payed]
				,[cost_debt]
				,[cost_zalishkova]
				,[cost_rinkova]
				,[cost_fair_1m]
				,[num_people]
				,[num_rent_agr]
				,[num_privat_apt]
				,[o26_id]
				,[bti_id]
				,[approval_by]
				,[approval_num]
				,[approval_date]
				,[form_ownership_id]
				,[object_kind_id]
				,[object_type_id]
				,[history_id]
				,[tech_condition_id]
				,[purpose_group_id]
				,[purpose_id]
				,[purpose_str]
				,[floors]
				,[priznak_1nf]
				,[ownership_type_id]
				,[obj_street_name]
				,[obj_street_id]
				,[obj_street_name2]
				,[obj_street_id2]
				,[obj_nomer1]
				,[obj_nomer2]
				,[obj_nomer3]
				,[obj_nomer]
				,[obj_bti_code]
				,[obj_addr_misc]
				,[obj_street_misc]
				,@MODIFIED_BY AS 'modified_by'
				,@MODIFY_DATE AS 'modify_date'
				,[form_giver_id]
				,[org_maintain_id]
				,[update_src_id]
				,1 AS 'is_deleted'
				,@DEL_DATE AS 'del_date'
				,[znos]
				,[znos_date]
				,[date_expert]
				,[reestr_no]
				,[fair_cost_date]
				,[otdel_gukv_id]
				,[date_bti]
				,[arch_id]
				,[arch_flag]
				,[date_cost_rinkova]
				,[memo]
				,[note]
				,[is_free_sqr]
				,[free_sqr_useful]
				,[free_sqr_condition_id]
				,[free_sqr_location]
				,[ownership_doc_type]
				,[ownership_doc_num]
				,[ownership_doc_date]
				,[balans_doc_type]
				,[balans_doc_num]
				,[balans_doc_date]
				--,[vidch_doc_type]
				,3 as 'vidch_doc_type'
				--,[vidch_doc_num]
				,@VIDCH_DOC_NUM as 'vidch_doc_num'
				--,[vidch_doc_date]
				,@VIDCH_DOC_DATE as 'vidch_doc_date'
				--,[vidch_type_id]
				,@VIDCH_TYPE_ID AS 'vidch_type_id'
				--,[vidch_org_id]
				,CASE WHEN @VIDCH_ORG_ID > 0 THEN @VIDCH_ORG_ID ELSE [vidch_org_id] END 'vidch_org_id'
				,[vidch_cost]
				,[sqr_engineering]
				,[obj_status_id]
				,[report_id]
				,NULL
				,0 AS 'is_valid'
				,'' AS 'validation_errors'
			FROM reports1nf_balans WHERE id = @BALANS_ID
		END;

		/* Delete the object from the 'reports1nf_balans' table */
		DELETE FROM reports1nf_balans WHERE id = @BALANS_ID
	END
GO




IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[reports1nf_balans_free_square]') AND type in (N'U'))
BEGIN

CREATE TABLE dbo.reports1nf_balans_free_square
	(
	id int NOT NULL IDENTITY (1, 1),
	balans_id int NOT NULL,
	total_free_sqr numeric(9, 2) NOT NULL,
	free_sqr_condition_id int NOT NULL,
	floor varchar(128) NULL,
	possible_using varchar(255) NULL,
	water bit NOT NULL,
	heating bit NOT NULL,
	power bit NOT NULL,
	gas bit NOT NULL,
	modify_date datetime NOT NULL,
	modified_by varchar(128) NOT NULL,
	report_id int NOT NULL	
	)  ON [PRIMARY]

ALTER TABLE dbo.reports1nf_balans_free_square ADD CONSTRAINT
	DF_reports1nf_balans_free_square_total_free_sqrr DEFAULT 0 FOR total_free_sqr

ALTER TABLE dbo.reports1nf_balans_free_square ADD CONSTRAINT
	DF_reports1nf_balans_free_square_floor DEFAULT 0 FOR floor

ALTER TABLE dbo.reports1nf_balans_free_square ADD CONSTRAINT
	DF_reports1nf_balans_free_square_water DEFAULT 0 FOR water

ALTER TABLE dbo.reports1nf_balans_free_square ADD CONSTRAINT
	DF_reports1nf_balans_free_square_heating DEFAULT 0 FOR heating

ALTER TABLE dbo.reports1nf_balans_free_square ADD CONSTRAINT
	DF_reports1nf_balans_free_square_power DEFAULT 0 FOR power

ALTER TABLE dbo.reports1nf_balans_free_square ADD CONSTRAINT
	PK_Treports1nf_balans_free_square PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

ALTER TABLE dbo.reports1nf_balans_free_square SET (LOCK_ESCALATION = TABLE)

END;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[balans_free_square]') AND type in (N'U'))
BEGIN

CREATE TABLE dbo.balans_free_square
	(
	id int NOT NULL IDENTITY (1, 1),
	balans_id int NOT NULL,
	total_free_sqr numeric(9, 2) NOT NULL,
	free_sqr_condition_id int NOT NULL,
	floor varchar(128) NULL,
	possible_using varchar(255) NULL,
	water bit NOT NULL,
	heating bit NOT NULL,
	power bit NOT NULL,
	gas bit NOT NULL,
	modify_date datetime NOT NULL,
	modified_by varchar(128) NOT NULL,
	original_id int NOT NULL
	)  ON [PRIMARY]

ALTER TABLE dbo.balans_free_square ADD CONSTRAINT
	DF_balans_free_square_total_free_sqr DEFAULT 0 FOR total_free_sqr

ALTER TABLE dbo.balans_free_square ADD CONSTRAINT
	DF_balans_free_square_floor DEFAULT 0 FOR floor

ALTER TABLE dbo.balans_free_square ADD CONSTRAINT
	DF_balans_free_square_water DEFAULT 0 FOR water

ALTER TABLE dbo.balans_free_square ADD CONSTRAINT
	DF_balans_free_square_heating DEFAULT 0 FOR heating

ALTER TABLE dbo.balans_free_square ADD CONSTRAINT
	DF_balans_free_square_power DEFAULT 0 FOR power

ALTER TABLE dbo.balans_free_square ADD CONSTRAINT
	PK_balans_free_square PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

ALTER TABLE dbo.balans_free_square SET (LOCK_ESCALATION = TABLE)

END;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[reports1nf_balans_free_square_photos]') AND type in (N'U'))
BEGIN

CREATE TABLE [reports1nf_balans_free_square_photos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[free_square_id] [int] NOT NULL,
	[file_name] [varchar](255) NOT NULL,
	[file_ext] [varchar](255) NOT NULL,
	[modify_date] datetime NOT NULL,
	[modified_by] varchar(128) NOT NULL
 CONSTRAINT [PK_reports1nf_balans_free_square_photos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE dbo.reports1nf_balans_free_square_photos ADD CONSTRAINT
	FK_reports1nf_balans_free_square_photos_reports1nf_balans_free_square FOREIGN KEY
	(
	free_square_id
	) REFERENCES dbo.reports1nf_balans_free_square
	(
	id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 

END;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[balans_free_square_photos]') AND type in (N'U'))
BEGIN

CREATE TABLE balans_free_square_photos(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[free_square_id] [int] NOT NULL,
	[file_name] [varchar](255) NOT NULL,
	[file_ext] [varchar](255) NOT NULL,
	modify_date datetime NOT NULL,
	modified_by varchar(128) NOT NULL,
	original_id int NOT NULL
 CONSTRAINT [PK_balans_free_square_photos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE dbo.balans_free_square_photos ADD CONSTRAINT
	FK_balans_free_square_photos_balans_free_square FOREIGN KEY
	(
	free_square_id
	) REFERENCES dbo.balans_free_square
	(
	id
	) ON UPDATE  CASCADE 
	 ON DELETE  CASCADE 

END;
GO

IF OBJECT_ID('Concatenate') IS NOT NULL
BEGIN
	DROP AGGREGATE [Concatenate];
END
GO

IF EXISTS(select * from sys.assemblies where name = 'ITG.EIS.SqlServerExtensions')
BEGIN
	DROP ASSEMBLY [ITG.EIS.SqlServerExtensions];
END
GO

CREATE ASSEMBLY [ITG.EIS.SqlServerExtensions]
    AUTHORIZATION [dbo]
    FROM 0x4D5A90000300000004000000FFFF0000B800000000000000400000000000000000000000000000000000000000000000000000000000000000000000800000000E1FBA0E00B409CD21B8014CCD21546869732070726F6772616D2063616E6E6F742062652072756E20696E20444F53206D6F64652E0D0D0A2400000000000000504500004C010300C4E693560000000000000000E00002210B010B00000C00000006000000000000DE2A0000002000000040000000000010002000000002000004000000000000000400000000000000008000000002000000000000030040850000100000100000000010000010000000000000100000000000000000000000882A000053000000004000009003000000000000000000000000000000000000006000000C000000502900001C0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000200000080000000000000000000000082000004800000000000000000000002E74657874000000E40A000000200000000C000000020000000000000000000000000000200000602E72737263000000900300000040000000040000000E0000000000000000000000000000400000402E72656C6F6300000C0000000060000000020000001200000000000000000000000000004000004200000000000000000000000000000000C02A00000000000048000000020005003C2100001408000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000003202731000000A7D020000042ACE027B020000046F1100000A163111027B0200000472010000706F1200000A26027B020000040F01281300000A6F1200000A262A0000000330020042000000000000000F017B020000046F1100000A163132027B020000046F1100000A163111027B0200000472010000706F1200000A26027B020000040F017B020000046F1400000A262A46027B020000046F1500000A281600000A2A6A036F1700000A2D1102036F1800000A731900000A7D020000042AA6027B020000042D0803176F1A00000A2A03166F1A00000A03027B020000046F1500000A6F1B00000A2A00000042534A4201000100000000000C00000076322E302E35303732370000000005006C000000B0020000237E00001C030000C003000023537472696E677300000000DC0600000800000023555300E4060000100000002347554944000000F40600002001000023426C6F620000000000000002000001571F00000900000000FA253300160000010000001900000002000000020000000600000004000000010000001B000000010000000C000000010000000200000000000A00010000000000060046003F000A0077005C000600A20096000A00D100BC0006000001F60006001201F6000600470135010600640135010600810135010600A00135010600B90135010600D20135010600ED01350106000802350106002102350106003A02350106006A02570247007E0200000600AD028D020600CD028D020A0007035C000A0028035C0006004E032F03060064032F0306008B033F000000000001000000000001000100090110002A000000050001000100568088000A000100B00010005020000000008600B700140001005D20000000008600DB00180001009420000000008600E6001E000200E220000000008600EC0024000300F42000000000E6010D01290003000F2100000000E6011F012F000400000001002501000001002B010000010031010000010033010200090039005E01350041005E01350049005E01350051005E01350059005E01350061005E01350069005E01350071005E01350079005E01350081005E01350089005E013A0099005E014000A1005E011400A9005E014500B9005E01660019005E01140019006F036C0019007A03700021008103760019007A037A00C9009203760021009B0380002900A70386002900B303760019005E01350031001F018A0031001F0135000E0004000D002E002B00B7002E000B008F002E001300B1002E001B00B1002E002300B1002E005300E0002E006B00FF002E003B00B1002E003300C4002E005B00ED002E006300F600430073004B000480000001000000DE16E242000000000000EB020000020000000000000000000000010036000000000002000000000000000000000001005000000000000000003C4D6F64756C653E004954472E4549532E53716C536572766572457874656E73696F6E732E646C6C00436F6E636174656E617465006D73636F726C69620053797374656D0056616C7565547970650053797374656D2E44617461004D6963726F736F66742E53716C5365727665722E536572766572004942696E61727953657269616C697A65004974656D44656C696D697465720053797374656D2E5465787400537472696E674275696C6465720076616C75657300496E69740053797374656D2E446174612E53716C54797065730053716C537472696E6700416363756D756C617465004D65726765005465726D696E6174650053797374656D2E494F0042696E61727952656164657200526561640042696E6172795772697465720057726974650056616C75650047726F7570007200770053797374656D2E5265666C656374696F6E00417373656D626C795469746C65417474726962757465002E63746F7200417373656D626C794465736372697074696F6E41747472696275746500417373656D626C79436F6E66696775726174696F6E41747472696275746500417373656D626C79436F6D70616E7941747472696275746500417373656D626C7950726F6475637441747472696275746500417373656D626C79436F7079726967687441747472696275746500417373656D626C7954726164656D61726B41747472696275746500417373656D626C7943756C7475726541747472696275746500417373656D626C7956657273696F6E41747472696275746500417373656D626C7946696C6556657273696F6E4174747269627574650053797374656D2E446961676E6F73746963730044656275676761626C6541747472696275746500446562756767696E674D6F6465730053797374656D2E52756E74696D652E436F6D70696C6572536572766963657300436F6D70696C6174696F6E52656C61786174696F6E734174747269627574650052756E74696D65436F6D7061746962696C697479417474726962757465004954472E4549532E53716C536572766572457874656E73696F6E730053716C55736572446566696E656441676772656761746541747472696275746500466F726D61740053797374656D2E52756E74696D652E496E7465726F705365727669636573005374727563744C61796F7574417474726962757465004C61796F75744B696E64006765745F4C656E67746800417070656E64006765745F56616C7565004F626A65637400546F537472696E67006F705F496D706C696369740052656164426F6F6C65616E0052656164537472696E6700000000032C00000000004404BC3217240148AB37947568E1D57A0008B77A5C561934E08902060E022C000306120D032000010520010111110520010111080420001111052001011215052001011219042001010E05200101114904200101080520010111591A010002000000010054080B4D61784279746553697A65FFFFFFFF05200101116103200008052001120D0E0320000E052001120D1C05000111110E0320000204200101022101001C495447204549532053716C53657276657220457874656E73696F6E7300000501000000000C0100074954472045495300001B010016436F707972696768742028632920323031352049544700000C010007312E302E302E3000000801000200000000000801000800000000001E01000100540216577261704E6F6E457863657074696F6E5468726F777301000000000000C4E6935600000000020000001C0100006C2900006C0B0000525344530444CB333D0DDC4D9D776D0F1A6FB53E04000000633A5C776F726B5C47554B565C4954472E4549532E53716C536572766572457874656E73696F6E735C4954472E4549532E53716C536572766572457874656E73696F6E735C6F626A5C52656C656173655C4954472E4549532E53716C536572766572457874656E73696F6E732E70646200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000B02A00000000000000000000CE2A0000002000000000000000000000000000000000000000000000C02A000000000000000000000000000000005F436F72446C6C4D61696E006D73636F7265652E646C6C0000000000FF2500200010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000100100000001800008000000000000000000000000000000100010000003000008000000000000000000000000000000100000000004800000058400000380300000000000000000000380334000000560053005F00560045005200530049004F004E005F0049004E0046004F0000000000BD04EFFE00000100000001000000000000000100000000003F000000000000000400000002000000000000000000000000000000440000000100560061007200460069006C00650049006E0066006F00000000002400040000005400720061006E0073006C006100740069006F006E00000000000000B00498020000010053007400720069006E006700460069006C00650049006E0066006F00000074020000010030003000300030003000340062003000000064001D000100460069006C0065004400650073006300720069007000740069006F006E000000000049005400470020004500490053002000530071006C00530065007200760065007200200045007800740065006E00730069006F006E00730000000000300008000100460069006C006500560065007200730069006F006E000000000031002E0030002E0030002E003000000060002000010049006E007400650072006E0061006C004E0061006D00650000004900540047002E004500490053002E00530071006C0053006500720076006500720045007800740065006E00730069006F006E0073002E0064006C006C0000005400170001004C006500670061006C0043006F007000790072006900670068007400000043006F0070007900720069006700680074002000280063002900200032003000310035002000490054004700000000006800200001004F0072006900670069006E0061006C00460069006C0065006E0061006D00650000004900540047002E004500490053002E00530071006C0053006500720076006500720045007800740065006E00730069006F006E0073002E0064006C006C000000300008000100500072006F0064007500630074004E0061006D0065000000000049005400470020004500490053000000340008000100500072006F006400750063007400560065007200730069006F006E00000031002E0030002E0030002E003000000048000F00010041007300730065006D0062006C0079002000560065007200730069006F006E00000031002E0030002E0035003800350034002E00310037003100320032000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002000000C000000E03A00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000;
GO

CREATE AGGREGATE [dbo].[Concatenate](@Value NVARCHAR (MAX))
    RETURNS NVARCHAR (MAX)
    EXTERNAL NAME [ITG.EIS.SqlServerExtensions].[Concatenate];
GO


--
-- NOTE:
--
-- Although the view references the CRL Aggregate function, SQL Server allows to
-- drop and recreate the Aggregate without a complaint.
--

IF OBJECT_ID('bp_rish_project_view_state') IS NOT NULL
BEGIN
	DROP VIEW [bp_rish_project_view_state];
END
GO

CREATE VIEW [dbo].[bp_rish_project_view_state]
AS
SELECT dbo.bp_rish_project_state.project_id, dbo.Concatenate(dbo.dict_rish_project_state.id) AS state_id, dbo.Concatenate(dbo.dict_rish_project_state.name) AS state_name
FROM dbo.bp_rish_project_state
INNER JOIN dbo.dict_rish_project_state ON dbo.dict_rish_project_state.id = dbo.bp_rish_project_state.state_id
WHERE bp_rish_project_state.exited_on IS NULL
GROUP BY dbo.bp_rish_project_state.project_id
GO

IF OBJECT_ID('bp_rish_project_table_value') IS NOT NULL
BEGIN
	DROP TABLE bp_rish_project_table_value;
END
GO

IF OBJECT_ID('bp_rish_project_table_column') IS NOT NULL
BEGIN
	DROP TABLE bp_rish_project_table_column;
END
GO

IF OBJECT_ID('dict_rish_table_column') IS NOT NULL
BEGIN
	DROP TABLE dict_rish_table_column;
END
GO


IF COLUMNPROPERTY(OBJECT_ID('bp_rish_project_item'), 'table_type', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE dbo.bp_rish_project_item ADD table_type INT NULL;
END
GO

IF OBJECT_ID('bp_rish_project_table') IS NULL
BEGIN

	CREATE TABLE [dbo].[bp_rish_project_table](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[bp_rish_project_item_id] [int] NOT NULL,
		[name] [varchar](max) NULL,
		[address] [varchar](512) NULL,
		[addr_street_name] [varchar](128) NULL,
		[addr_nomer] [varchar](32) NULL,
		[addr_misc] [varchar](128) NULL,
		[addr_distr] [varchar](64) NULL,
		[obj_type] [varchar](64) NULL,
		[obj_kind] [varchar](64) NULL,
		[year_built] [int] NULL,
		[sqr_total] [numeric](18, 2) NULL,
		[inv_number] [varchar](32) NULL,
		[initial_cost] [decimal](18, 2) NULL,
		[remaining_cost] [decimal](18, 2) NULL,
		[location] [varchar](512) NULL,
		[commissioned_date] [date] NULL,
		CONSTRAINT [PK_bp_rish_project_table] PRIMARY KEY CLUSTERED 
		(
			[id] ASC
		)
	);

	ALTER TABLE [dbo].[bp_rish_project_table]  WITH CHECK ADD  CONSTRAINT [FK_bp_rish_project_table_bp_rish_project_item] FOREIGN KEY([bp_rish_project_item_id])
	REFERENCES [dbo].[bp_rish_project_item] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[bp_rish_project_table] CHECK CONSTRAINT [FK_bp_rish_project_table_bp_rish_project_item];

END
GO




IF COLUMNPROPERTY(OBJECT_ID('dict_obj_rights'), 'is_active', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE dbo.dict_obj_rights ADD
		is_active bit NULL,
		enable_org_from bit NULL,
		enable_org_to bit NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM dict_obj_rights WHERE name='СПИСАННЯ ШЛЯХОМ ПРОДАЖУ')
BEGIN
	-- Mark all dictionary entries obsolete; some will be mapped onto the new one and reused shortly
	UPDATE dict_obj_rights SET is_active = 0, enable_org_from = 0, enable_org_to = 0;

	-- Upgrade existing dictionary entries
	UPDATE dict_obj_rights SET is_active = 1, enable_org_from = 1, enable_org_to = 1, name = 'ОПЕРАТИВНЕ УПРАВЛІННЯ' WHERE name = 'ОПЕРАТИВНОГО УПРАВЛІННЯ';
	UPDATE dict_obj_rights SET is_active = 1, enable_org_from = 1, enable_org_to = 1 WHERE name = 'ДО СФЕРИ УПРАВЛІННЯ';
	UPDATE dict_obj_rights SET is_active = 1, enable_org_from = 0, enable_org_to = 1, name = 'ОРЕНДА' WHERE name = 'ОРЕНДИ';
	UPDATE dict_obj_rights SET is_active = 1, enable_org_from = 0, enable_org_to = 1, name = 'ПОЗИЧКА' WHERE name = 'ПОЗИЧКУ';
	UPDATE dict_obj_rights SET is_active = 1, enable_org_from = 0, enable_org_to = 1 WHERE name = 'ВОЛОДІННЯ ТА КОРИСТУВАННЯ';

	DECLARE @id INT = (SELECT MAX(id) FROM dict_obj_rights);

	-- Create new dictionary entries
	INSERT INTO dict_obj_rights (id, name, is_active, enable_org_from, enable_org_to)
		VALUES (@id + 1, 'СПИСАННЯ ШЛЯХОМ ПРОДАЖУ', 1, 1, 0),
			(@id + 2, 'СПИСАННЯ ШЛЯХОМ ЛІКВІДАЦІЇ', 1, 1, 0),
			(@id + 3, 'ЗАРАХУВАННЯ', 1, 0, 1),
			(@id + 4, 'ПРИЙНЯТТЯ', 1, 0, 1),
			(@id + 5, 'ПЕРЕДАЧА У ВЛАСНІСТЬ', 1, 1, 1),
			(@id + 6, 'ГОСПОДАРСЬКЕ ВІДАННЯ', 1, 1, 1),
			(@id + 7, 'СПИСАННЯ ШЛЯХОМ ПЕРЕДАЧІ', 1, 1, 1);
END
GO


IF COLUMNPROPERTY(OBJECT_ID('bp_rish_project_info'), 'document_id', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE dbo.bp_rish_project_info ADD
		document_id int NULL;
END
GO

IF OBJECT_ID('FK_bp_rish_project_info_documents') IS NULL
BEGIN
	ALTER TABLE dbo.bp_rish_project_info ADD CONSTRAINT
		FK_bp_rish_project_info_documents FOREIGN KEY
		(
		document_id
		) REFERENCES dbo.documents
		(
		id
		) ON UPDATE  CASCADE 
		 ON DELETE  CASCADE 
END	
GO


IF COLUMNPROPERTY(OBJECT_ID('bp_rish_project_table'), 'is_acted_on', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE bp_rish_project_table 
		ADD is_acted_on BIT
END
GO


IF OBJECT_ID('balans_other') IS NULL
BEGIN
	CREATE TABLE [dbo].[balans_other](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[org_id] [int] NOT NULL,
		[name] [varchar](max) NULL,
		[address] [varchar](512) NULL,
		[inv_number] [varchar](32) NULL,
		[initial_cost] [decimal](18, 2) NULL,
		[remaining_cost] [decimal](18, 2) NULL,
		[location] [varchar](512) NULL,
		[commissioned_date] [date] NULL,
		[decommissioned_date] [date] NULL,
		[document_number] [varchar](20) NULL,
		[document_date] [date] NULL,
		[on_balance_date] [date] NULL,
		CONSTRAINT [PK_balans_other] PRIMARY KEY CLUSTERED 
		(
			[id] ASC
		)
	)
END
GO


IF OBJECT_ID('FK_balans_other_organizations') IS NULL
BEGIN
	ALTER TABLE dbo.balans_other ADD CONSTRAINT
		FK_balans_other_organizations FOREIGN KEY
		(
		org_id
		) REFERENCES dbo.organizations
		(
		id
		) ON UPDATE  CASCADE 
		 ON DELETE  CASCADE 
END
GO


IF OBJECT_ID('balans_kievenergo') IS NOT NULL
BEGIN

	-- 461: ПУБЛІЧНЕ АКЦІОНЕРНЕ ТОВАРИСТВО "КИЇВЕНЕРГО"

	INSERT INTO balans_other 
		(org_id, name, address, inv_number, initial_cost, remaining_cost, location, commissioned_date, 
		decommissioned_date, document_number, document_date, on_balance_date)
	SELECT 461, name, address, inv_number, initial_cost, remaining_cost, location, commissioned_date, 
		decommissioned_date, document_number, document_date, on_balance_date
	FROM balans_kievenergo;

	DROP TABLE balans_kievenergo;

END
GO




IF OBJECT_ID('balans_other_docs') IS NULL
BEGIN
	CREATE TABLE [dbo].[balans_other_docs](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[balans_other_id] [int] NOT NULL,
		[document_id] [int] NOT NULL,
		CONSTRAINT [PK_balans_other_docs] PRIMARY KEY CLUSTERED 
		(
			[id] ASC
		)
	)
END
GO


IF OBJECT_ID('FK_balans_other_docs_balans_other') IS NULL
BEGIN
	ALTER TABLE [dbo].[balans_other_docs]  WITH CHECK ADD  CONSTRAINT [FK_balans_other_docs_balans_other] FOREIGN KEY([balans_other_id])
	REFERENCES [dbo].[balans_other] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[balans_other_docs] CHECK CONSTRAINT [FK_balans_other_docs_balans_other]
END
GO

IF OBJECT_ID('FK_balans_other_docs_documents') IS NULL
BEGIN
	ALTER TABLE [dbo].[balans_other_docs]  WITH CHECK ADD  CONSTRAINT [FK_balans_other_docs_documents] FOREIGN KEY([document_id])
	REFERENCES [dbo].[documents] ([id])
	ON UPDATE CASCADE
	ON DELETE CASCADE;

	ALTER TABLE [dbo].[balans_other_docs] CHECK CONSTRAINT [FK_balans_other_docs_documents]
END
GO

