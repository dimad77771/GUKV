
/******************************************************************************/
/*                                  Views                                     */
/******************************************************************************/

/* view_buildings - displays detailed information about building */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_buildings]'))
DROP VIEW [dbo].[view_buildings]

GO

CREATE VIEW [view_buildings]
AS
SELECT [buildings].[id] AS 'building_id'
      ,[buildings].[street_full_name]
      ,[dict_districts2].[name] AS 'district'
      ,[buildings].[addr_nomer]
      ,[buildings].[addr_nomer1]
      ,[buildings].[addr_zip_code]
      ,[buildings].[addr_misc]
      ,[dict_tech_state].[name] AS 'condition'
      ,[buildings].[num_floors]
      ,[buildings].[construct_year]
      ,[buildings].[bti_code]
      ,[dict_history].[name] AS 'history'
      ,[dict_object_type].[name] AS 'object_type'
      ,[dict_object_kind].[name] AS 'object_kind'
      ,[buildings].[cost_balans]
      ,[buildings].[sqr_total]
      ,[buildings].[sqr_habit]
      ,[buildings].[sqr_rented]
      ,[buildings].[sqr_zagal]
      ,[buildings].[sqr_for_rent]
      ,[buildings].[sqr_non_habit]
      ,[buildings].[sqr_pidval]
      ,[buildings].[additional_info]
      ,[buildings].[oatuu_id] AS 'oatuu_code'
      ,[dict_facade].[name] AS 'facade'
      ,[dict_oatuu].[name] AS 'region'
      ,[buildings].sqr_dk AS 'sqr_object_dk'
      ,[buildings].sqr_mk AS 'sqr_object_mk'
      ,[buildings].sqr_rk AS 'sqr_object_rk'
      ,[buildings].sqr_other AS 'sqr_object_other'
      ,[buildings].is_deleted AS 'building_deleted'
      ,CASE WHEN priv.id IS NULL THEN N'НІ' ELSE N'ТАК' END AS 'is_in_privat'
      ,[buildings].modified_by
      ,[buildings].modify_date
      ,[buildings].addr_street_id
      ,[buildings].addr_street_id2
      ,[buildings].addr_distr_new_id
FROM
    buildings
    OUTER APPLY (SELECT TOP 1 id FROM privatization p WHERE p.building_id = buildings.id AND p.privat_state_id = 1) priv
    LEFT OUTER JOIN dict_districts2 ON buildings.addr_distr_new_id = dict_districts2.id
    LEFT OUTER JOIN dict_tech_state ON buildings.tech_condition_id = dict_tech_state.id
    LEFT OUTER JOIN dict_object_kind ON buildings.object_kind_id = dict_object_kind.id
    LEFT OUTER JOIN dict_object_type ON buildings.object_type_id = dict_object_type.id
    LEFT OUTER JOIN dict_facade ON buildings.facade_id = dict_facade.id
    LEFT OUTER JOIN dict_history ON buildings.history_id = dict_history.id
    LEFT OUTER JOIN dict_oatuu ON buildings.oatuu_id = dict_oatuu.id
WHERE
	(master_building_id IS NULL)

GO

/* view_arch_buildings - displays detailed information about building archive state */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arch_buildings]'))
DROP VIEW [dbo].[view_arch_buildings]

GO

CREATE VIEW [view_arch_buildings]
AS
SELECT [arch_buildings].[archive_id],
       [arch_buildings].[id] AS 'building_id'
      ,[arch_buildings].[street_full_name]
      ,[dict_districts2].[name] AS 'district'
      ,(LTRIM(RTRIM(isnull(arch_buildings.addr_nomer1, ''))) + ' ' +
        LTRIM(RTRIM(isnull(arch_buildings.addr_nomer2, ''))) + ' ' +
        LTRIM(RTRIM(isnull(arch_buildings.addr_nomer3, '')))) AS 'addr_nomer'
      ,LTRIM(RTRIM(isnull(arch_buildings.addr_nomer1, ''))) AS 'addr_nomer1'
      ,[arch_buildings].[addr_zip_code]
      ,[arch_buildings].[addr_misc]
      ,[dict_tech_state].[name] AS 'condition'
      ,[arch_buildings].[num_floors]
      ,[arch_buildings].[construct_year]
      ,[arch_buildings].[bti_code]
      ,[dict_history].[name] AS 'history'
      ,[dict_object_type].[name] AS 'object_type'
      ,[dict_object_kind].[name] AS 'object_kind'
      ,[arch_buildings].[cost_balans]
      ,[arch_buildings].[sqr_total]
      ,[arch_buildings].[sqr_habit]
      ,[arch_buildings].[sqr_rented]
      ,[arch_buildings].[sqr_zagal]
      ,[arch_buildings].[sqr_for_rent]
      ,[arch_buildings].[sqr_non_habit]
      ,[arch_buildings].[sqr_pidval]
      ,[arch_buildings].[additional_info]
      ,[arch_buildings].[oatuu_id] AS 'oatuu_code'
      ,[dict_facade].[name] AS 'facade'
      ,[dict_oatuu].[name] AS 'region'
      ,[arch_buildings].sqr_dk AS 'sqr_object_dk'
      ,[arch_buildings].sqr_mk AS 'sqr_object_mk'
      ,[arch_buildings].sqr_rk AS 'sqr_object_rk'
      ,[arch_buildings].sqr_other AS 'sqr_object_other'
      ,[arch_buildings].is_deleted AS 'building_deleted'
      ,CASE WHEN arch_buildings.is_deleted = 1 THEN N'ТАК' ELSE N'НІ' END AS 'building_deleted_txt'
      ,[arch_buildings].modified_by
      ,[arch_buildings].modify_date
      ,[arch_buildings].addr_distr_new_id
FROM
    arch_buildings
    LEFT OUTER JOIN dict_districts2 ON arch_buildings.addr_distr_new_id = dict_districts2.id
    LEFT OUTER JOIN dict_tech_state ON arch_buildings.tech_condition_id = dict_tech_state.id
    LEFT OUTER JOIN dict_object_kind ON arch_buildings.object_kind_id = dict_object_kind.id
    LEFT OUTER JOIN dict_object_type ON arch_buildings.object_type_id = dict_object_type.id
    LEFT OUTER JOIN dict_facade ON arch_buildings.facade_id = dict_facade.id
    LEFT OUTER JOIN dict_history ON arch_buildings.history_id = dict_history.id
    LEFT OUTER JOIN dict_oatuu ON arch_buildings.oatuu_id = dict_oatuu.id
WHERE
	(master_building_id IS NULL)

GO

/* view_organizations - displays detailed information about organizations */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_organizations]'))
DROP VIEW [dbo].[view_organizations]

GO

CREATE VIEW [view_organizations]
AS
SELECT
    [organizations].[id] AS 'organization_id'
   ,[organizations].[full_name]
   ,[organizations].[short_name]
   ,[organizations].[zkpo_code]
   ,[dict_org_industry].[name] AS 'industry'
   ,[dict_org_occupation].[name] AS 'occupation'
   ,[dict_org_status].[name] AS 'status'
   ,[dict_org_form_gosp].[name] AS 'form_gosp'
   ,[dict_org_ownership].[name] AS 'form_of_ownership'
   ,[organizations].[form_ownership_id] AS 'form_of_ownership_int'
   ,[dict_org_gosp_struct].[name] AS 'gosp_struct'
   ,[dict_org_vedomstvo].[name] AS 'vedomstvo'
   ,[dict_org_title].[display_name] AS 'title'
   ,[dict_org_title_form].[display_name] AS 'title_form'
   ,[dict_org_form].[name] AS 'org_form'
   ,[dict_org_gosp_struct_type].[name] AS 'gosp_struct_type'
   ,[dict_org_sfera_upr].[name] AS 'sfera_upr'
   ,[dict_org_old_industry].[name] AS 'old_industry'
   ,[dict_org_old_occupation].[name] AS 'old_occupation'
   ,[dict_org_old_organ].[name] AS 'old_organ'
   ,[organizations].[addr_city]
   ,[dict_districts2].[name] AS 'addr_district'
   ,[organizations].[addr_street_name]
   ,[organizations].[addr_nomer]
   ,[organizations].[addr_korpus]
   ,[organizations].[addr_zip_code]
   ,[director_fio]
   ,[director_phone]
   ,[buhgalter_fio]
   ,[buhgalter_phone]
   ,[num_buildings]
   ,[fax]
   ,[registration_auth]
   ,[registration_num]
   ,[registration_date]
   ,[registration_svidot]
   ,[sqr_on_balance]
   ,[sqr_manufact]
   ,[sqr_non_manufact]
   ,[sqr_free_for_rent]
   ,[organizations].[sqr_total]
   ,[sqr_rented]
   ,[sqr_privat]
   ,[sqr_given_for_rent]
   ,[sqr_znyata_z_balansu]
   ,[sqr_prodaj]
   ,[sqr_spisani_zneseni]
   ,[sqr_peredana]
   ,[num_objects]
   ,[kved_code]
   ,[koatuu]
   ,[dict_otdel_gukv].[name] AS 'otdel_gukv'
   ,[dict_org_mayno].[name] AS 'mayno'
   ,CASE WHEN [is_liquidated] = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_liquidated'
   ,[liquidation_date]
   ,[contact_email]
   ,[dict_org_contact_posada].[name] AS 'contact_posada'
   ,[dict_org_nadhodjennya].[name] AS 'nadhodjennya'
   ,[dict_org_vibuttya].[name] AS 'vibuttya'
   ,COALESCE([dict_org_registr_org].[name], registration_auth) AS 'registr_org'
   ,[nadhodjennya_date]
   ,[vibuttya_date]
   ,[origin_db]
   ,COALESCE([organizations].[budg_payments_rate], [dict_org_old_industry].[budg_payments_rate]) AS 'budg_payments_rate'
   ,organizations.last_state
   ,organizations.beg_state_date
   ,organizations.end_state_date
   ,organizations.is_deleted AS 'org_deleted'
   ,organizations.modified_by
   ,organizations.modify_date
   ,organizations.is_under_closing
   ,[dict_form_vlasn_vibuttya].name AS 'form_vlasn_vibuttya'
   ,organizations.sfera_upr_id
   ,organizations.addr_distr_new_id
   /*
   ,[dict_org_plan_zone].[name] AS 'plan_zone'
   ,[dict_org_share_type].[name] AS 'share_type'
   ,[dict_org_privat_status].[name] AS 'privat_status'
   ,[dict_org_cur_state].[name] AS 'cur_state'
   ,[dict_org_organ].[name] AS 'organ'
   ,[dict_org_priznak].[name] AS 'priznak'
   */
FROM
    [dbo].[organizations]
    LEFT OUTER JOIN dict_org_industry ON organizations.industry_id = dict_org_industry.id
    LEFT OUTER JOIN dict_org_occupation ON organizations.occupation_id = dict_org_occupation.id
    LEFT OUTER JOIN dict_org_status ON organizations.status_id = dict_org_status.id
    LEFT OUTER JOIN dict_org_form_gosp ON organizations.form_gosp_id = dict_org_form_gosp.id
    LEFT OUTER JOIN dict_org_ownership ON organizations.form_ownership_id = dict_org_ownership.id
    LEFT OUTER JOIN dict_org_gosp_struct ON organizations.gosp_struct_id = dict_org_gosp_struct.id
    LEFT OUTER JOIN dict_org_gosp_struct_type ON organizations.gosp_struct_type_id = dict_org_gosp_struct_type.id
    LEFT OUTER JOIN dict_org_vedomstvo ON organizations.vedomstvo_id = dict_org_vedomstvo.id
    LEFT OUTER JOIN dict_org_title ON organizations.title_id = dict_org_title.id
    LEFT OUTER JOIN dict_org_title_form ON organizations.title_form_id = dict_org_title_form.id
    LEFT OUTER JOIN dict_org_form ON organizations.form_id = dict_org_form.id
    LEFT OUTER JOIN dict_otdel_gukv ON organizations.otdel_gukv_id = dict_otdel_gukv.id
    LEFT OUTER JOIN dict_org_mayno ON organizations.mayno_id = dict_org_mayno.id
    LEFT OUTER JOIN dict_org_contact_posada ON organizations.contact_posada_id = dict_org_contact_posada.id
    LEFT OUTER JOIN dict_org_nadhodjennya ON organizations.nadhodjennya_id = dict_org_nadhodjennya.id
    LEFT OUTER JOIN dict_org_vibuttya ON organizations.vibuttya_id = dict_org_vibuttya.id
    LEFT OUTER JOIN dict_org_sfera_upr ON organizations.sfera_upr_id = dict_org_sfera_upr.id
    LEFT OUTER JOIN dict_org_registr_org ON organizations.registr_org_id = dict_org_registr_org.id
    LEFT OUTER JOIN dict_org_old_industry ON organizations.old_industry_id = dict_org_old_industry.id
    LEFT OUTER JOIN dict_org_old_occupation ON organizations.old_occupation_id = dict_org_old_occupation.id
    LEFT OUTER JOIN dict_org_old_organ ON organizations.old_organ_id = dict_org_old_organ.id
    LEFT OUTER JOIN dict_districts2 ON organizations.addr_distr_new_id = dict_districts2.id
    LEFT OUTER JOIN [dict_form_vlasn_vibuttya] ON organizations.form_vlasn_vibuttya_id = [dict_form_vlasn_vibuttya].id
    /* The following information seems not used */
    /*
    LEFT OUTER JOIN dict_org_organ ON organizations.organ_id = dict_org_organ.id
    LEFT OUTER JOIN dict_org_priznak ON organizations.priznak_id = dict_org_priznak.id
    LEFT OUTER JOIN dict_org_share_type ON organizations.share_type_id = dict_org_share_type.id
    LEFT OUTER JOIN dict_org_privat_status ON organizations.privat_status_id = dict_org_privat_status.id
    LEFT OUTER JOIN dict_org_cur_state ON organizations.cur_state_id = dict_org_cur_state.id
    LEFT OUTER JOIN dict_org_plan_zone ON organizations.plan_zone_id = dict_org_plan_zone.id
    */
WHERE
    master_org_id IS NULL    

GO

/* view_arch_organizations - displays detailed information about organization archive state */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arch_organizations]'))
DROP VIEW [dbo].[view_arch_organizations]

GO

CREATE VIEW [view_arch_organizations]
AS
SELECT
    [arch_organizations].[archive_id]
   ,[arch_organizations].[id] AS 'organization_id'
   ,[arch_organizations].[full_name]
   ,[arch_organizations].[short_name]
   ,[arch_organizations].[zkpo_code]
   ,[dict_org_industry].[name] AS 'industry'
   ,[dict_org_occupation].[name] AS 'occupation'
   ,[dict_org_status].[name] AS 'status'
   ,[dict_org_form_gosp].[name] AS 'form_gosp'
   ,[dict_org_ownership].[name] AS 'form_of_ownership'
   ,[arch_organizations].[form_ownership_id] AS 'form_of_ownership_int'
   ,[dict_org_gosp_struct].[name] AS 'gosp_struct'
   ,[dict_org_vedomstvo].[name] AS 'vedomstvo'
   ,[dict_org_title].[display_name] AS 'title'
   ,[dict_org_title_form].[display_name] AS 'title_form'
   ,[dict_org_form].[name] AS 'org_form'
   ,[dict_org_gosp_struct_type].[name] AS 'gosp_struct_type'
   ,[dict_org_sfera_upr].[name] AS 'sfera_upr'
   ,[dict_org_old_industry].[name] AS 'old_industry'
   ,[dict_org_old_occupation].[name] AS 'old_occupation'
   ,[dict_org_old_organ].[name] AS 'old_organ'
   ,[arch_organizations].[addr_city]
   ,[dict_districts2].[name] AS 'addr_district'
   ,[arch_organizations].[addr_street_name]
   ,[arch_organizations].[addr_nomer]
   ,[arch_organizations].[addr_korpus]
   ,[arch_organizations].[addr_zip_code]
   ,[director_fio]
   ,[director_phone]
   ,[buhgalter_fio]
   ,[buhgalter_phone]
   ,[num_buildings]
   ,[fax]
   ,[registration_auth]
   ,[registration_num]
   ,[registration_date]
   ,[registration_svidot]
   ,[sqr_on_balance]
   ,[sqr_manufact]
   ,[sqr_non_manufact]
   ,[sqr_free_for_rent]
   ,[arch_organizations].[sqr_total]
   ,[sqr_rented]
   ,[sqr_privat]
   ,[sqr_given_for_rent]
   ,[sqr_znyata_z_balansu]
   ,[sqr_prodaj]
   ,[sqr_spisani_zneseni]
   ,[sqr_peredana]
   ,[num_objects]
   ,[kved_code]
   ,[koatuu]
   ,[dict_otdel_gukv].[name] AS 'otdel_gukv'
   ,[dict_org_mayno].[name] AS 'mayno'
   ,CASE WHEN [is_liquidated] = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_liquidated'
   ,[liquidation_date]
   ,[contact_email]
   ,[dict_org_contact_posada].[name] AS 'contact_posada'
   ,[dict_org_nadhodjennya].[name] AS 'nadhodjennya'
   ,[dict_org_vibuttya].[name] AS 'vibuttya'
   ,COALESCE([dict_org_registr_org].[name], registration_auth) AS 'registr_org'
   ,[nadhodjennya_date]
   ,[vibuttya_date]
   ,[origin_db]
   ,COALESCE([arch_organizations].[budg_payments_rate], [dict_org_old_industry].[budg_payments_rate]) AS 'budg_payments_rate'
   ,arch_organizations.last_state
   ,arch_organizations.beg_state_date
   ,arch_organizations.end_state_date
   ,arch_organizations.is_deleted AS 'org_deleted'
   ,CASE WHEN arch_organizations.is_deleted = 1 THEN N'ТАК' ELSE N'НІ' END AS 'org_deleted_txt'
   ,arch_organizations.modified_by
   ,arch_organizations.modify_date
   ,CASE WHEN arch_organizations.is_under_closing = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_under_closing'
   ,[dict_form_vlasn_vibuttya].name AS 'form_vlasn_vibuttya' 
   ,arch_organizations.sfera_upr_id
   ,arch_organizations.addr_distr_new_id
FROM
    [dbo].[arch_organizations]
    LEFT OUTER JOIN dict_org_industry ON arch_organizations.industry_id = dict_org_industry.id
    LEFT OUTER JOIN dict_org_occupation ON arch_organizations.occupation_id = dict_org_occupation.id
    LEFT OUTER JOIN dict_org_status ON arch_organizations.status_id = dict_org_status.id
    LEFT OUTER JOIN dict_org_form_gosp ON arch_organizations.form_gosp_id = dict_org_form_gosp.id
    LEFT OUTER JOIN dict_org_ownership ON arch_organizations.form_ownership_id = dict_org_ownership.id
    LEFT OUTER JOIN dict_org_gosp_struct ON arch_organizations.gosp_struct_id = dict_org_gosp_struct.id
    LEFT OUTER JOIN dict_org_gosp_struct_type ON arch_organizations.gosp_struct_type_id = dict_org_gosp_struct_type.id
    LEFT OUTER JOIN dict_org_vedomstvo ON arch_organizations.vedomstvo_id = dict_org_vedomstvo.id
    LEFT OUTER JOIN dict_org_title ON arch_organizations.title_id = dict_org_title.id
    LEFT OUTER JOIN dict_org_title_form ON arch_organizations.title_form_id = dict_org_title_form.id
    LEFT OUTER JOIN dict_org_form ON arch_organizations.form_id = dict_org_form.id
    LEFT OUTER JOIN dict_otdel_gukv ON arch_organizations.otdel_gukv_id = dict_otdel_gukv.id
    LEFT OUTER JOIN dict_org_mayno ON arch_organizations.mayno_id = dict_org_mayno.id
    LEFT OUTER JOIN dict_org_contact_posada ON arch_organizations.contact_posada_id = dict_org_contact_posada.id
    LEFT OUTER JOIN dict_org_nadhodjennya ON arch_organizations.nadhodjennya_id = dict_org_nadhodjennya.id
    LEFT OUTER JOIN dict_org_vibuttya ON arch_organizations.vibuttya_id = dict_org_vibuttya.id
    LEFT OUTER JOIN dict_org_sfera_upr ON arch_organizations.sfera_upr_id = dict_org_sfera_upr.id
    LEFT OUTER JOIN dict_org_registr_org ON arch_organizations.registr_org_id = dict_org_registr_org.id
    LEFT OUTER JOIN dict_org_old_industry ON arch_organizations.old_industry_id = dict_org_old_industry.id
    LEFT OUTER JOIN dict_org_old_occupation ON arch_organizations.old_occupation_id = dict_org_old_occupation.id
    LEFT OUTER JOIN dict_org_old_organ ON arch_organizations.old_organ_id = dict_org_old_organ.id
    LEFT OUTER JOIN dict_districts2 ON arch_organizations.addr_distr_new_id = dict_districts2.id
    LEFT OUTER JOIN [dict_form_vlasn_vibuttya] ON arch_organizations.form_vlasn_vibuttya_id = [dict_form_vlasn_vibuttya].id
WHERE
    master_org_id IS NULL

GO

/* view_documents - information from the 'documents' table */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_documents]'))
DROP VIEW [dbo].[view_documents]

GO

CREATE VIEW [view_documents]
AS
SELECT [doc].[id]
      ,[doc].[kind_id]
      ,[dict_doc_kind].[name] AS 'kind'
      ,[doc].[general_kind_id]
      ,[dict_doc_general_kind].[name] AS 'general_kind'
      ,[doc].[doc_date]
      ,[doc].[doc_num]
      ,[doc].[topic]
      ,[doc].[note]
      ,[doc].[search_name]
      ,[doc].[receive_date]
      ,[dict_doc_commission].[name] AS 'commission'
      ,[dict_doc_source].[name] AS 'source'
      ,[dict_doc_state].[name] AS 'state'
      ,[doc].[summa]
      ,[doc].[summa_zalishkova]
      ,[doc].[is_text_exists]
      ,[doc].[is_priv_rishen]
      ,[doc].[extern_doc_id]
      ,COALESCE(dict_doc_kind.name, N'Документ') + N' № ' + COALESCE(RTRIM(doc_num), N'Б/Н') +
       COALESCE(N' вiд ' + CONVERT(VARCHAR(32), doc_date, 104), N'') AS 'display_name'
FROM
      documents doc
      LEFT OUTER JOIN dict_doc_kind ON dict_doc_kind.id = doc.kind_id
      LEFT OUTER JOIN dict_doc_general_kind ON dict_doc_general_kind.id = doc.general_kind_id
      LEFT OUTER JOIN dict_doc_commission ON dict_doc_commission.id = doc.commission_id
      LEFT OUTER JOIN dict_doc_source ON dict_doc_source.id = doc.source_id
      LEFT OUTER JOIN dict_doc_state ON dict_doc_state.id = doc.state_id

GO

/* view_balans_all - displays detailed information about 'balans' entries, includingthe deleted ones */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_balans_all]'))
DROP VIEW [dbo].[view_balans_all]

GO

CREATE VIEW [view_balans_all]
AS
SELECT [balans].[id] AS 'balans_id'
      ,[balans].[building_id]
      ,[balans].[organization_id]
      ,[org_holder].[full_name] AS 'org_full_name'
      ,[org_holder].[short_name] AS 'org_short_name'
      ,[org_holder].[zkpo_code] AS 'org_zkpo_code'
      ,[org_holder].[industry] AS 'org_industry'
      ,[org_holder].[occupation] AS 'org_occupation'
      ,[org_holder].[old_industry] AS 'org_old_industry'
      ,[org_holder].[old_occupation] AS 'org_old_occupation'
      ,[org_holder].[vedomstvo] AS 'org_vedomstvo'
      ,[org_holder].[form_of_ownership] AS 'org_ownership'
      ,[org_holder].[form_of_ownership_int] AS 'org_ownership_int'
      ,[org_holder].[sfera_upr_id] AS 'org_sfera_upr_id'
      ,[org_holder].[form_of_ownership_int] AS 'org_form_ownership_id'
      ,[org_holder].[addr_distr_new_id] AS 'org_district_id'
      ,[b].[district]      
      ,[b].[street_full_name]
      ,[b].[addr_nomer]
      ,[b].[addr_misc]
      ,[balans].[sqr_total]
      ,[balans].[sqr_pidval]
      ,[balans].[sqr_vlas_potreb]
      ,[balans].[sqr_free]
      ,[balans].[sqr_in_rent]
      ,[balans].[sqr_privatizov]
      ,[balans].[sqr_not_for_rent]
      ,[balans].[sqr_gurtoj]
      ,[balans].[sqr_non_habit]
      ,[balans].[sqr_kor]
      ,[balans].[cost_balans]
      ,[balans].[cost_expert_1m]
      ,[balans].[cost_expert_total]
      ,[balans].[cost_zalishkova]
      ,[balans].[num_rent_agr]
      ,[balans].[num_privat_apt]
      ,[balans].[approval_by]
      ,[balans].[approval_num]
      ,[balans].[approval_date]
      ,[dict_balans_o26].[name] AS 'o26_code'
      ,[dict_balans_bti].[name] AS 'bti_condition'
      --,[balans].[floors] AS 'num_floors'
	  ,[b].[num_floors]
      ,[balans].[org_maintain_id]
      ,[org_maintainer].[full_name] AS 'org_maintainer_full_name'
      ,[org_maintainer].[short_name] AS 'org_maintainer_short_name'
      ,[org_maintainer].[zkpo_code] AS 'org_maintainer_zkpo_code'
      ,[dict_otdel_gukv].[name] AS 'otdel_gukv'
      ,[dict_org_ownership].[name] AS 'form_ownership'
      ,[balans].[form_ownership_id] AS 'form_ownership_int'
      ,[dict_balans_ownership_type].[name] AS 'ownership_type'
      ,[dict_object_kind].[name] AS 'object_kind'
      ,[dict_object_type].[name] AS 'object_type'
      ,[dict_tech_state].[name] AS 'condition'
      ,[dict_balans_purpose_group].[name] AS 'purpose_group'
      ,[dict_balans_purpose].[name] AS 'purpose'
      ,[dict_history].[name] AS 'history'
      ,[balans].[date_expert]
      ,[balans].[floors]
      ,[balans].[memo]
      ,[balans].[note]
      ,[balans].[znos]
      ,[balans].[znos_date]
      ,[balans].[modify_date] AS 'input_date' 
      ,[balans].[purpose_str] AS 'balans_obj_name'
      ,[balans].[is_deleted]
      ,CASE WHEN [balans].[is_deleted] > 0 THEN N'НІ' ELSE N'ТАК' END AS 'is_on_balans'
      ,[balans].modified_by
      ,[balans].modify_date
      ,[balans].cost_fair
      ,[balans].cost_fair_1m
      ,[balans].cost_rent_narah
      ,[balans].cost_rent_payed
      ,[balans].cost_debt
      ,[balans].cost_rinkova
      ,[balans].date_cost_rinkova
      ,[balans].fair_cost_date
      ,[balans].[obj_bti_code]
      ,[balans].[date_bti]
      ,[balans].[reestr_no] AS 'invent_no_bti'
      ,[balans].priznak_1nf
      ,[b].[addr_zip_code]
      ,[b].[construct_year]
      ,[b].[oatuu_code]
      ,[b].[facade]
      ,[b].[is_in_privat]
      ,free_sqr.sqr_free_total
      ,free_sqr.sqr_free_korysna
      ,free_sqr.sqr_free_mzk
      ,free_sqr.free_sqr_floors
      ,free_sqr.free_sqr_purpose
      ,CASE WHEN twin.id IS NULL AND balans.is_deleted > 0 AND balans.modified_by = 'Auto-import' THEN 1 ELSE 0 END AS 'is_not_accepted'
FROM
    [dbo].[balans]
    LEFT OUTER JOIN dict_balans_o26 ON balans.o26_id = dict_balans_o26.id
    LEFT OUTER JOIN dict_balans_bti ON balans.bti_id = dict_balans_bti.id
    LEFT OUTER JOIN dict_balans_purpose_group ON balans.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON balans.purpose_id = dict_balans_purpose.id
    LEFT OUTER JOIN dict_balans_ownership_type ON balans.ownership_type_id = dict_balans_ownership_type.id
    LEFT OUTER JOIN dict_otdel_gukv ON balans.otdel_gukv_id = dict_otdel_gukv.id
    LEFT OUTER JOIN dict_org_ownership ON balans.form_ownership_id = dict_org_ownership.id
    LEFT OUTER JOIN dict_tech_state ON balans.tech_condition_id = dict_tech_state.id
    LEFT OUTER JOIN dict_object_kind ON balans.object_kind_id = dict_object_kind.id
    LEFT OUTER JOIN dict_object_type ON balans.object_type_id = dict_object_type.id
    LEFT OUTER JOIN dict_history ON balans.history_id = dict_history.id
    LEFT OUTER JOIN view_organizations org_holder ON balans.organization_id = org_holder.organization_id
    LEFT OUTER JOIN organizations org_maintainer ON balans.org_maintain_id = org_maintainer.id
    LEFT OUTER JOIN view_buildings b ON balans.building_id = b.building_id
    OUTER APPLY (SELECT TOP 1 rp.rent_period_id FROM rent_payment rp WHERE rp.org_balans_id = balans.organization_id ORDER BY rp.rent_period_id DESC) rent_report
    OUTER APPLY (SELECT TOP 1 * FROM rent_free_square rfs
		WHERE rfs.building_id = balans.building_id AND
		      rfs.organization_id = balans.organization_id AND
		      rfs.rent_period_id = rent_report.rent_period_id) free_sqr
	OUTER APPLY
	(
		SELECT TOP 1 bal2.id FROM balans bal2 INNER JOIN buildings b2 ON b2.id = bal2.building_id WHERE
			b2.addr_street_id = b.addr_street_id and
			b2.addr_nomer1 = b.addr_nomer1 and
			bal2.id != balans.id and
			bal2.sqr_total = balans.sqr_total and
			ISNULL(bal2.is_deleted, 0) = 0
	) twin
WHERE
    (NOT [balans].[building_id] IS NULL) AND
    (NOT [balans].[organization_id] IS NULL)

GO

/* view_balans - displays detailed information about 'balans' entries */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_balans]'))
DROP VIEW [dbo].[view_balans]

GO

CREATE VIEW [view_balans]
AS
SELECT *
FROM
    view_balans_all
WHERE
    (is_deleted IS NULL) OR (is_deleted = 0)

GO

/* view_arch_balans - displays archive states of the 'balans' entries */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arch_balans]'))
DROP VIEW [dbo].[view_arch_balans]

GO

CREATE VIEW [view_arch_balans]
AS
SELECT [ab].archive_id
      ,[ab].[id] AS 'balans_id'
      ,[ab].[building_id]
      ,[ab].[organization_id]
      ,[org_holder].[full_name] AS 'org_full_name'
      ,[org_holder].[short_name] AS 'org_short_name'
      ,[org_holder].[zkpo_code] AS 'org_zkpo_code'
      ,[org_holder].[industry] AS 'org_industry'
      ,[org_holder].[occupation] AS 'org_occupation'
      ,[org_holder].[old_industry] AS 'org_old_industry'
      ,[org_holder].[old_occupation] AS 'org_old_occupation'
      ,[org_holder].[vedomstvo] AS 'org_vedomstvo'
      ,[org_holder].[form_of_ownership] AS 'org_ownership'
      ,[org_holder].[form_of_ownership_int] AS 'org_ownership_int'
      ,[org_holder].[sfera_upr_id] AS 'org_sfera_upr_id'
      ,[org_holder].[addr_distr_new_id] AS 'org_district_id'
      ,[b].[district]      
      ,[b].[street_full_name]
      ,[b].[addr_nomer]
      ,[b].[addr_misc]
      ,[ab].[sqr_total]
      ,[ab].[sqr_pidval]
      ,[ab].[sqr_vlas_potreb]
      ,[ab].[sqr_free]
      ,[ab].[sqr_in_rent]
      ,[ab].[sqr_privatizov]
      ,[ab].[sqr_not_for_rent]
      ,[ab].[sqr_gurtoj]
      ,[ab].[sqr_non_habit]
      ,[ab].[sqr_kor]
      ,[ab].[cost_balans]
      ,[ab].[cost_expert_1m]
      ,[ab].[cost_expert_total]
      ,[ab].[cost_zalishkova]
      ,[ab].[num_rent_agr]
      ,[ab].[num_privat_apt]
      ,[ab].[approval_by]
      ,[ab].[approval_num]
      ,[ab].[approval_date]
      ,[dict_balans_o26].[name] AS 'o26_code'
      ,[dict_balans_bti].[name] AS 'bti_condition'
      ,[ab].[floors] AS 'num_floors'
      ,[ab].[org_maintain_id]
      ,[org_maintainer].[full_name] AS 'org_maintainer_full_name'
      ,[org_maintainer].[short_name] AS 'org_maintainer_short_name'
      ,[org_maintainer].[zkpo_code] AS 'org_maintainer_zkpo_code'
      ,[dict_otdel_gukv].[name] AS 'otdel_gukv'
      ,[dict_org_ownership].[name] AS 'form_ownership'
      ,[ab].[form_ownership_id] AS 'form_ownership_int'
      ,[dict_balans_ownership_type].[name] AS 'ownership_type'
      ,[dict_object_kind].[name] AS 'object_kind'
      ,[dict_object_type].[name] AS 'object_type'
      ,[dict_tech_state].[name] AS 'condition'
      ,[dict_balans_purpose_group].[name] AS 'purpose_group'
      ,[dict_balans_purpose].[name] AS 'purpose'
      ,[dict_history].[name] AS 'history'
      ,[ab].[date_expert]
      ,[ab].[floors]
      ,[ab].[obj_bti_code]
      ,[ab].[date_bti]
      ,[ab].[reestr_no] AS 'invent_no_bti'
      ,[ab].[memo]
      ,[ab].[note]
      ,[ab].[znos]
      ,[ab].[znos_date]
      ,[ab].[modify_date] AS 'input_date' 
      ,[ab].[purpose_str] AS 'balans_obj_name'
      ,[ab].[is_deleted]
      ,[ab].priznak_1nf
      ,[b].[addr_zip_code]
      ,[b].[construct_year]
      ,[b].[oatuu_code]
      ,[b].[facade]
      ,[b].[is_in_privat]
      ,[ab].modified_by
      ,[ab].modify_date
      ,[ab].cost_fair
      ,[ab].cost_fair_1m
      ,[ab].cost_rent_narah
      ,[ab].cost_rent_payed
      ,[ab].cost_debt
      ,[ab].cost_rinkova
      ,[ab].date_cost_rinkova
      ,[ab].fair_cost_date
FROM
    [dbo].[arch_balans] ab
    LEFT OUTER JOIN dict_balans_o26 ON ab.o26_id = dict_balans_o26.id
    LEFT OUTER JOIN dict_balans_bti ON ab.bti_id = dict_balans_bti.id
    LEFT OUTER JOIN dict_balans_purpose_group ON ab.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON ab.purpose_id = dict_balans_purpose.id
    LEFT OUTER JOIN dict_balans_ownership_type ON ab.ownership_type_id = dict_balans_ownership_type.id
    LEFT OUTER JOIN dict_otdel_gukv ON ab.otdel_gukv_id = dict_otdel_gukv.id
    LEFT OUTER JOIN dict_org_ownership ON ab.form_ownership_id = dict_org_ownership.id
    LEFT OUTER JOIN dict_tech_state ON ab.tech_condition_id = dict_tech_state.id
    LEFT OUTER JOIN dict_object_kind ON ab.object_kind_id = dict_object_kind.id
    LEFT OUTER JOIN dict_object_type ON ab.object_type_id = dict_object_type.id
    LEFT OUTER JOIN dict_history ON ab.history_id = dict_history.id
    LEFT OUTER JOIN view_organizations org_holder ON ab.organization_id = org_holder.organization_id
    LEFT OUTER JOIN organizations org_maintainer ON ab.org_maintain_id = org_maintainer.id
    LEFT OUTER JOIN view_buildings b ON ab.building_id = b.building_id
WHERE
    (NOT [ab].[building_id] IS NULL) AND
    (NOT [ab].[organization_id] IS NULL)

GO

/* view_org_balans_totals - calculates total summaries for the 'view_balans' view */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_org_balans_totals]'))
DROP VIEW [dbo].[view_org_balans_totals]

GO

CREATE VIEW [view_org_balans_totals]
AS
SELECT
    organization_id,
    COUNT(*) AS 'balans_obj_count',
    SUM(sqr_total) AS 'sqr_balans_total',
    SUM(sqr_non_habit) AS 'sqr_balans_non_habit'
FROM
    view_balans
GROUP BY
    organization_id

GO

/* view_arenda_agreements - displays detailed information about 'arenda' table */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arenda_agreements]'))
DROP VIEW [dbo].[view_arenda_agreements]

GO

CREATE VIEW [view_arenda_agreements]
AS
SELECT [an].[id] AS 'arenda_id'
      ,[an].[building_id]
      ,[an].[balans_id]
      ,[an].[org_balans_id]
      ,[org_holder].[full_name] AS 'org_balans_full_name'
      ,[org_holder].[short_name] AS 'org_balans_short_name'
      ,[org_holder].[zkpo_code] AS 'org_balans_zkpo'
      ,[org_holder].[industry] AS 'org_balans_industry'
      ,[org_holder].[occupation] AS 'org_balans_occupation'
      ,[org_holder].[vedomstvo] AS 'org_balans_vedomstvo'
      ,[org_holder].[org_form] AS 'org_balans_org_form'
      ,[org_holder].sfera_upr_id AS 'org_balans_sfera_upr_id'
      ,[org_holder].form_of_ownership_int as 'org_balans_form_ownership_id'
      ,[org_holder].form_of_ownership as 'org_balans_form_ownership'
      ,[org_holder].addr_distr_new_id AS 'org_balans_district_id'
      ,[an].[org_renter_id]
      ,[org_renter].[full_name] AS 'org_renter_full_name'
      ,[org_renter].[short_name] AS 'org_renter_short_name'
      ,[org_renter].[zkpo_code] AS 'org_renter_zkpo'
      ,[org_renter].[industry] AS 'org_renter_industry'
      ,[org_renter].[occupation] AS 'org_renter_occupation'
      ,[org_renter].[vedomstvo] AS 'org_renter_vedomstvo'
      ,[org_renter].[director_fio] AS 'org_renter_director_fio'
      ,[org_renter].[director_phone] AS 'org_renter_director_phone'
      ,[org_renter].[form_of_ownership] AS 'org_renter_form_of_ownership'
      ,[org_renter].[org_form] AS 'org_renter_org_form'
      ,[org_renter].sfera_upr_id AS 'org_renter_sfera_upr_id'
      ,[org_renter].form_of_ownership_int as 'org_renter_form_ownership_id'
      ,[org_renter].addr_distr_new_id AS 'org_renter_district_id'
      ,[an].[org_giver_id]
      ,[org_giver].[full_name] AS 'org_giver_full_name'
      ,[org_giver].[short_name] AS 'org_giver_short_name'
      ,[org_giver].[zkpo_code] AS 'org_giver_zkpo'
      ,[org_giver].[industry] AS 'org_giver_industry'
      ,[org_giver].[occupation] AS 'org_giver_occupation'
      ,[org_giver].[vedomstvo] AS 'org_giver_vedomstvo'
      ,[org_giver].[org_form] AS 'org_giver_org_form'
      ,[org_giver].sfera_upr_id AS 'org_giver_sfera_upr_id'
      ,[org_giver].form_of_ownership_int as 'org_giver_form_ownership_id'
      ,[org_giver].addr_distr_new_id AS 'org_giver_district_id'
      ,[b].[district]      
      ,[b].[street_full_name]
      ,[b].[addr_nomer]
      ,[b].[is_in_privat] 
      ,[free_sqr].[sqr_free_total]
      ,[free_sqr].[sqr_free_korysna]
      ,[free_sqr].[sqr_free_mzk]
      ,[free_sqr].[free_sqr_floors]
      ,[free_sqr].[free_sqr_purpose]
      ,[an].[purpose_str] AS 'object_name'
      ,[an].[note] AS 'object_note'
      ,[an].[name]
      ,[dict_balans_purpose_group].[name] AS 'purpose_group'
      ,[dict_balans_purpose].[name] AS 'purpose'
      ,CASE WHEN [an].[is_privat] = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_privat'
      ,[an].[is_privat] AS 'is_privat_int'
      ,[an].[agreement_kind_id]
      ,[dict_arenda_agreement_kind].[name] AS 'agreement_kind'
      ,[an].[agreement_date]
      ,YEAR(an.agreement_date) AS 'agreement_date_year'
      ,DATEPART(Quarter, an.agreement_date)  AS 'agreement_date_quarter'
      ,[an].[agreement_num]
      ,CASE WHEN NOT (RTRIM(LTRIM(an.agreement_num)) LIKE '%[^0-9]%') THEN CAST(RTRIM(LTRIM(an.agreement_num)) AS bigint) ELSE NULL END AS 'agreement_num_int'
      ,[an].[floor_number]
      ,[an].[cost_narah]
      ,[an].[cost_payed]
      ,[an].[cost_debt]
      ,[an].[cost_agreement]
      ,[an].[cost_expert_1m]
      ,[an].[cost_expert_total]
      ,[an].[debt_timespan]
      ,[an].[pidstava]
      ,[an].[pidstava_date]
      ,[an].[pidstava_num]
      ,[an].[pidstava_display]
      ,[an].[rent_start_date]
      ,YEAR(an.rent_start_date) AS 'rent_start_year'
      ,DATEPART(Quarter, an.rent_start_date)  AS 'rent_start_quarter'
      ,[an].[rent_finish_date]
      ,YEAR(an.rent_finish_date) AS 'rent_finish_year'
      ,DATEPART(Quarter, an.rent_finish_date)  AS 'rent_finish_quarter'
      ,[an].[rent_actual_finish_date]
      ,YEAR(an.rent_actual_finish_date) AS 'actual_finish_year'
      ,DATEPART(Quarter, an.rent_actual_finish_date)  AS 'actual_finish_quarter'
      ,[an].[rent_rate] AS 'rent_rate_percent'
      --,[an].[rent_rate_uah] AS 'rent_rate_uah' -- Замена при отображении а также для импорта на портал
	  ,[an].[rent_rate] AS 'rent_rate_uah'
      ,[an].[rent_square]
      ,[an].[rishennya_id] AS 'rishennya_code'
      ,[an].[num_akt]
      ,[an].[date_akt]
      ,[an].[num_bti]
      ,[an].[date_bti]
      ,[an].[modified_by]
      ,[an].[modify_date]
      ,[an].[is_subarenda] AS 'is_subarenda_int'
      ,CASE WHEN [an].[is_subarenda] = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_subarenda'
      ,[dict_arenda_payment_type].[name] AS 'payment_type'
      ,CASE WHEN (an.is_deleted = 1) THEN N'НІ' ELSE N'ТАК' END AS 'agreement_active'
      ,CASE WHEN (an.is_deleted = 1) THEN 0 ELSE 1 END AS 'agreement_active_int'
      ,[an].[is_deleted]
      
      --,[bal].[sqr_total] AS 'balans_sqr_total'
      ,bal_total.sqr_total AS 'balans_sqr_total'
      --,[bal].[num_rent_agr] AS 'balans_num_rent_agr'
      --,an_total.num_rent_agr AS 'balans_num_rent_agr'
      ,(SELECT count(*) FROM arenda an_total
		WHERE an_total.building_id = an.building_id AND
			an_total.org_balans_id = an.org_balans_id AND 
			(an_total.is_deleted IS NULL OR an_total.is_deleted = 0) AND
			an_total.agreement_state = 1
			) as 'balans_num_rent_agr'      
      
      --,[bal].[sqr_in_rent] AS 'balans_sqr_in_rent'
      ,an_total.rent_square AS 'balans_sqr_in_rent'
  --    ,(SELECT SUM(an_total.rent_square) FROM arenda an_total
		--WHERE an_total.building_id = an.building_id AND
		--	an_total.org_balans_id = an.org_balans_id AND 
		--	(an_total.is_deleted IS NULL OR an_total.is_deleted = 0) AND
		--	an_total.agreement_state = 1
		--	) as 'balans_sqr_in_rent'      
      
      ,[bal].[org_ownership] AS 'balans_org_ownership'
      ,[bal].[org_ownership_int] AS 'balans_org_ownership_int'
      ,[bal].[form_ownership] AS 'balans_form_ownership'
      ,[bal].[form_ownership_int] AS 'balans_form_ownership_int'
FROM
    [dbo].[arenda] an
    LEFT OUTER JOIN dict_balans_purpose_group ON an.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON an.purpose_id = dict_balans_purpose.id
    LEFT OUTER JOIN dict_arenda_agreement_kind ON an.agreement_kind_id = dict_arenda_agreement_kind.id
    LEFT OUTER JOIN dict_arenda_payment_type ON an.payment_type_id = dict_arenda_payment_type.id
    LEFT OUTER JOIN view_organizations org_holder ON an.org_balans_id = org_holder.organization_id
    LEFT OUTER JOIN view_organizations org_giver ON an.org_giver_id = org_giver.organization_id
    LEFT OUTER JOIN view_organizations org_renter ON an.org_renter_id = org_renter.organization_id
    LEFT OUTER JOIN view_buildings b ON an.building_id = b.building_id
    LEFT OUTER JOIN view_balans_all bal ON an.balans_id = bal.balans_id
    OUTER APPLY (SELECT TOP 1 rp.rent_period_id FROM rent_payment rp WHERE rp.org_balans_id = an.org_balans_id ORDER BY rp.rent_period_id DESC) rent_report
    OUTER APPLY (SELECT TOP 1 * FROM rent_free_square rfs
		WHERE rfs.building_id = an.building_id AND
		      rfs.organization_id = an.org_balans_id AND
		      rfs.rent_period_id = rent_report.rent_period_id) free_sqr	
	
	OUTER APPLY (SELECT SUM(bal_total.sqr_total) AS 'sqr_total'  FROM balans bal_total
		WHERE bal_total.building_id = an.building_id AND
			bal_total.organization_id = an.org_balans_id AND 
			(bal_total.is_deleted IS NULL OR bal_total.is_deleted = 0)) bal_total
			
	OUTER APPLY (SELECT SUM(an_total.rent_square) AS 'rent_square', COUNT(*) AS 'num_rent_agr'  FROM arenda an_total
		WHERE an_total.building_id = an.building_id AND
			an_total.org_balans_id = an.org_balans_id AND 
			(an_total.is_deleted IS NULL OR an_total.is_deleted = 0) 
			--AND an_total.agreement_state = 1
			) an_total		
WHERE
    (NOT [an].[building_id] IS NULL) AND
    (NOT [an].[org_renter_id] IS NULL)

GO

/* view_arenda_and_notes - displays detailed information about 'arenda' joined with 'arenda_notes' table */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arenda_and_notes]'))
DROP VIEW [dbo].[view_arenda_and_notes]

GO

CREATE VIEW [view_arenda_and_notes]
AS
SELECT 
       [arenda].[id]
      ,[notes].[id] AS 'arenda_note_id'
      ,[arenda].[building_id]
      ,[arenda].[org_balans_id]
      ,[arenda].[org_renter_id]
      ,[arenda].[org_giver_id]
      ,[arenda].[balans_id]
      ,[arenda].[rent_year]
      ,[arenda].[object_kind_id]
      ,COALESCE(notes.purpose_group_id, arenda.purpose_group_id) AS 'purpose_group_id'
      ,COALESCE(notes.purpose_id, arenda.purpose_id) AS 'purpose_id'
      ,COALESCE(notes.purpose_str, arenda.purpose_str) AS 'purpose_str'
      ,[arenda].[name]
      ,[arenda].[is_privat]
      ,[arenda].[update_src_id]
      ,[arenda].[agreement_kind_id]
      ,[arenda].[agreement_date]
      ,[arenda].[agreement_num]
      ,CASE WHEN NOT (RTRIM(LTRIM(arenda.agreement_num)) LIKE '%[^0-9]%') THEN CAST(RTRIM(LTRIM(arenda.agreement_num)) AS bigint) ELSE NULL END AS 'agreement_num_int'
      ,[arenda].[agreement_str]
      ,[arenda].[floor_number]
      ,[arenda].[num_people]
      ,COALESCE(notes.cost_narah, arenda.cost_narah) AS 'cost_narah'
      ,[arenda].[cost_payed]
      ,[arenda].[cost_debt]
      ,COALESCE(notes.cost_agreement, arenda.cost_agreement) AS 'cost_agreement'
      ,[arenda].[cost_expert_1m]
      ,COALESCE(notes.cost_expert_total, arenda.cost_expert_total) AS 'cost_expert_total'
      ,[arenda].[pidstava]
      ,[arenda].[pidstava_date]
      ,[arenda].[pidstava_num]
      ,[arenda].[pidstava_fact]
      ,[arenda].[pidstava2]
      ,[arenda].[pidstava_num2]
      ,[arenda].[pidstava_date2]
      ,[arenda].[pidstava_display]
      ,[arenda].[rent_start_date]
      ,[arenda].[rent_finish_date]
      ,[arenda].[rent_actual_finish_date]
      ,COALESCE(notes.rent_rate, arenda.rent_rate) AS 'rent_rate'
      --,COALESCE(notes.rent_rate_uah, arenda.rent_rate_uah) AS 'rent_rate_uah' -- Замена при отображении а также для импорта на портал
	  ,COALESCE(notes.rent_rate, arenda.rent_rate) AS 'rent_rate_uah'
      ,COALESCE(notes.rent_square, arenda.rent_square) AS 'rent_square'
      ,[arenda].[priznak_1nf]
      ,[arenda].[debt_timespan]
      ,[arenda].[order_num]
      ,[arenda].[order_date]
      ,[arenda].[order_no2]
      ,[arenda].[rishennya_id]
      ,[arenda].[is_inactive]
      ,[arenda].[inactive_date]
      ,[arenda].[is_deleted]
      ,[arenda].[del_date]
      ,COALESCE(notes.date_expert, arenda.date_expert) AS 'date_expert'
      ,[arenda].[is_subarenda]
      ,[arenda].[privat_kind_id]
      ,[arenda].[num_primirnikiv]
      ,[arenda].[date_expl_enter]
      ,[arenda].[num_akt]
      ,[arenda].[date_akt]
      ,[arenda].[num_bti]
      ,[arenda].[date_bti]
      ,[arenda].[svidotstvo_serial]
      ,[arenda].[svidotstvo_num]
      ,[arenda].[svidotstvo_date]
      ,COALESCE(notes.payment_type_id, arenda.payment_type_id) AS 'payment_type_id'
      ,[arenda].[arch_id]
      ,[arenda].[modified_by]
      ,[arenda].[modify_date]
      ,COALESCE(notes.note, arenda.note) AS 'note'
FROM
       arenda
       LEFT OUTER JOIN (SELECT * FROM arenda_notes WHERE is_deleted = 0) notes ON notes.arenda_id = arenda.id

GO

/* view_arenda_base - displays detailed information about 'arenda' entries */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arenda_base]'))
DROP VIEW [dbo].[view_arenda_base]

GO

CREATE VIEW [view_arenda_base]
AS
SELECT [an].[id] AS 'arenda_id'
      ,[an].[arenda_note_id]
      ,[an].[building_id]
      ,[an].[balans_id]
      ,[an].[org_balans_id]
      ,[org_holder].[full_name] AS 'org_balans_full_name'
      ,[org_holder].[short_name] AS 'org_balans_short_name'
      ,[org_holder].[zkpo_code] AS 'org_balans_zkpo'
      ,[org_holder].[industry] AS 'org_balans_industry'
      ,[org_holder].[occupation] AS 'org_balans_occupation'
      ,[org_holder].[vedomstvo] AS 'org_balans_vedomstvo'
      ,[org_holder].[org_form] AS 'org_balans_org_form'
      ,[org_holder].sfera_upr_id AS 'org_balans_sfera_upr_id'
      ,[org_holder].form_of_ownership_int AS 'org_balans_form_ownership_id'
      ,[org_holder].form_of_ownership AS 'org_balans_form_ownership'
      ,[org_holder].addr_distr_new_id AS 'org_balans_district_id'
      ,[an].[org_renter_id]
      ,[org_renter].[full_name] AS 'org_renter_full_name'
      ,[org_renter].[short_name] AS 'org_renter_short_name'
      ,[org_renter].[zkpo_code] AS 'org_renter_zkpo'
      ,[org_renter].[industry] AS 'org_renter_industry'
      ,[org_renter].[occupation] AS 'org_renter_occupation'
      ,[org_renter].[vedomstvo] AS 'org_renter_vedomstvo'
      ,[org_renter].[director_fio] AS 'org_renter_director_fio'
      ,[org_renter].[director_phone] AS 'org_renter_director_phone'
      ,[org_renter].[form_of_ownership] AS 'org_renter_form_of_ownership'
      ,[org_renter].[org_form] AS 'org_renter_org_form'
      ,[org_renter].sfera_upr_id AS 'org_renter_sfera_upr_id'
      ,[org_renter].form_of_ownership_int AS 'org_renter_form_ownership_id'
      ,[org_renter].addr_distr_new_id AS 'org_renter_district_id'
      ,[an].[org_giver_id]
      ,[org_giver].[full_name] AS 'org_giver_full_name'
      ,[org_giver].[short_name] AS 'org_giver_short_name'
      ,[org_giver].[zkpo_code] AS 'org_giver_zkpo'
      ,[org_giver].[industry] AS 'org_giver_industry'
      ,[org_giver].[occupation] AS 'org_giver_occupation'
      ,[org_giver].[vedomstvo] AS 'org_giver_vedomstvo'
      ,[org_giver].[org_form] AS 'org_giver_org_form'
      ,[org_giver].sfera_upr_id AS 'org_giver_sfera_upr_id'
      ,[org_giver].form_of_ownership_int AS 'org_giver_form_ownership_id'
      ,[org_giver].addr_distr_new_id AS 'org_giver_district_id'
      ,[b].[district]      
      ,[b].[street_full_name]
      ,[b].[addr_nomer]
      ,[b].[is_in_privat] 
      ,[free_sqr].[sqr_free_total]
      ,[free_sqr].[sqr_free_korysna]
      ,[free_sqr].[sqr_free_mzk]
      ,[free_sqr].[free_sqr_floors]
      ,[free_sqr].[free_sqr_purpose]
      ,[an].[purpose_str] AS 'object_name'
      ,[an].[note] AS 'object_note'
      ,[an].[name]
      ,[dict_balans_purpose_group].[name] AS 'purpose_group'
      ,[dict_balans_purpose].[name] AS 'purpose'
      ,CASE WHEN [an].[is_privat] = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_privat'
      ,[an].[is_privat] AS 'is_privat_int'
      ,[an].[agreement_kind_id]
      ,[dict_arenda_agreement_kind].[name] AS 'agreement_kind'
      ,[an].[agreement_date]
      ,YEAR(an.agreement_date) AS 'agreement_date_year'
      ,DATEPART(Quarter, an.agreement_date)  AS 'agreement_date_quarter'
      ,[an].[agreement_num]
      ,[an].[agreement_num_int]
      ,[an].[floor_number]
      ,[an].[cost_narah]
      ,[an].[cost_payed]
      ,[an].[cost_debt]
      ,[an].[cost_agreement]
      ,[an].[cost_expert_1m]
      ,[an].[cost_expert_total]
      ,[an].[debt_timespan]
      ,[an].[pidstava]
      ,[an].[pidstava_date]
      ,[an].[pidstava_num]
      ,[an].[pidstava_display]
      ,[an].[rent_start_date]
      ,YEAR(an.rent_start_date) AS 'rent_start_year'
      ,DATEPART(Quarter, an.rent_start_date)  AS 'rent_start_quarter'
      ,[an].[rent_finish_date]
      ,YEAR(an.rent_finish_date) AS 'rent_finish_year'
      ,DATEPART(Quarter, an.rent_finish_date)  AS 'rent_finish_quarter'
      ,[an].[rent_actual_finish_date]
      ,YEAR(an.rent_actual_finish_date) AS 'actual_finish_year'
      ,DATEPART(Quarter, an.rent_actual_finish_date)  AS 'actual_finish_quarter'
      ,[an].[rent_rate] AS 'rent_rate_percent'
      --,[an].[rent_rate_uah] AS 'rent_rate_uah' -- Замена при отображении а также для импорта на портал
	  ,[an].[rent_rate] AS 'rent_rate_uah'
      ,[an].[rent_square]
      ,[an].[rishennya_id] AS 'rishennya_code'
      ,[an].[num_akt]
      ,[an].[date_akt]
      ,[an].[num_bti]
      ,[an].[date_bti]
      ,[an].[modified_by]
      ,[an].[modify_date]
      ,[an].[is_subarenda] AS 'is_subarenda_int'
      ,CASE WHEN [an].[is_subarenda] = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_subarenda'
      ,[dict_arenda_payment_type].[name] AS 'payment_type'
      ,CASE WHEN
          (an.is_deleted = 1) /* OR (an.rent_start_date > GETDATE()) OR
          ((NOT an.rent_finish_date IS NULL) AND (an.rent_finish_date < GETDATE())) */
          THEN N'НІ' ELSE N'ТАК' END AS 'agreement_active'
      ,CASE WHEN
          (an.is_deleted = 1) /* OR (an.rent_start_date > GETDATE()) OR
          ((NOT an.rent_finish_date IS NULL) AND (an.rent_finish_date < GETDATE())) */
          THEN 0 ELSE 1 END AS 'agreement_active_int'
      ,[an].[is_deleted]
      ,[bal].[sqr_total] AS 'balans_sqr_total'
      ,[bal].[num_rent_agr] AS 'balans_num_rent_agr'
      ,[bal].[sqr_in_rent] AS 'balans_sqr_in_rent'
      ,[bal].[org_ownership] AS 'balans_org_ownership'
      --,[bal].[org_ownership_int] AS 'balans_org_ownership_int'
	  ,[org_holder].form_of_ownership_int AS 'balans_org_ownership_int'
      ,[bal].[form_ownership] AS 'balans_form_ownership'
      ,[bal].[form_ownership_int] AS 'balans_form_ownership_int'
FROM
    [dbo].[view_arenda_and_notes] an
    LEFT OUTER JOIN dict_balans_purpose_group ON an.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON an.purpose_id = dict_balans_purpose.id
    LEFT OUTER JOIN dict_arenda_agreement_kind ON an.agreement_kind_id = dict_arenda_agreement_kind.id
    LEFT OUTER JOIN dict_arenda_payment_type ON an.payment_type_id = dict_arenda_payment_type.id
    LEFT OUTER JOIN view_organizations org_holder ON an.org_balans_id = org_holder.organization_id
    LEFT OUTER JOIN view_organizations org_giver ON an.org_giver_id = org_giver.organization_id
    LEFT OUTER JOIN view_organizations org_renter ON an.org_renter_id = org_renter.organization_id
    LEFT OUTER JOIN view_buildings b ON an.building_id = b.building_id
    LEFT OUTER JOIN view_balans_all bal ON an.balans_id = bal.balans_id
    OUTER APPLY (SELECT TOP 1 rp.rent_period_id FROM rent_payment rp WHERE rp.org_balans_id = an.org_balans_id ORDER BY rp.rent_period_id DESC) rent_report
    OUTER APPLY (SELECT TOP 1 * FROM rent_free_square rfs
		WHERE rfs.building_id = an.building_id AND
		      rfs.organization_id = an.org_balans_id AND
		      rfs.rent_period_id = rent_report.rent_period_id) free_sqr	
WHERE
    (NOT [an].[building_id] IS NULL) AND
    (NOT [an].[org_renter_id] IS NULL)

GO

/* view_arenda - displays detailed information about 'arenda' entries */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arenda]'))
DROP VIEW [dbo].[view_arenda]

GO

CREATE VIEW [view_arenda]
AS
SELECT
    vab.*
FROM
    view_arenda_base vab
WHERE
    (vab.is_privat_int <> 1 OR vab.is_privat_int IS NULL) AND
    (vab.agreement_kind_id IS NULL OR
    ((vab.agreement_kind_id <> 9) AND
     (vab.agreement_kind_id <> 10) AND
     (vab.agreement_kind_id <> 16) AND
     (vab.agreement_kind_id <> 17)))

GO

/* view_arenda_privat_docs - displays only privatization documents from the 'arenda' table */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arenda_privat_docs]'))
DROP VIEW [dbo].[view_arenda_privat_docs]

GO

CREATE VIEW [view_arenda_privat_docs]
AS
SELECT
    *
FROM
    view_arenda_agreements
WHERE
    ((is_privat_int = 1) OR
     (agreement_kind_id = 9) OR
     (agreement_kind_id = 10) OR
     (agreement_kind_id = 16) OR
     (agreement_kind_id = 17))

GO

/* view_arenda_notes - displays 'arenda' notes */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arenda_notes]'))
DROP VIEW [dbo].[view_arenda_notes]

GO

CREATE VIEW [view_arenda_notes]
AS
SELECT
       an.id AS 'note_id'
      ,an.[arenda_id]
      ,dict_balans_purpose_group.name AS 'purpose_group'
      ,dict_balans_purpose.name AS 'purpose'
      ,an.[purpose_str]
      ,an.[rent_square]
      ,an.[modify_date]
      ,an.[modified_by]
      ,an.[note]
      ,an.[rent_rate]
      ,an.[cost_narah]
      ,an.[cost_agreement]
      ,an.[is_deleted]
      ,an.[del_date]
      ,an.[cost_expert_total]
      ,an.[date_expert]
      ,dict_arenda_payment_type.name AS 'payment_type'
FROM
    arenda_notes an
    LEFT OUTER JOIN dict_balans_purpose_group ON an.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON an.purpose_id = dict_balans_purpose.id
    LEFT OUTER JOIN dict_arenda_payment_type ON an.payment_type_id = dict_arenda_payment_type.id

GO

/* view_arenda_link_2_decisions - displays 'arenda' documents */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arenda_link_2_decisions]'))
DROP VIEW [dbo].[view_arenda_link_2_decisions]

GO

CREATE VIEW [view_arenda_link_2_decisions]
AS
SELECT
    link.id AS 'link_id',
    link.arenda_id,
    link.rishen_id,
    link.ord,
    link.modified_by,
    link.modify_date,
    link.doc_num,
    link.doc_date,
    link.doc_dodatok,
    link.doc_punkt,
    link.purpose_str,
    link.rent_square,
    dict_rent_decisions.name AS 'decision',
    link.doc_raspor_id,
    link.pidstava,
    COALESCE(pidstava, N'Документ') + N' № ' + COALESCE(RTRIM(doc_num), N'Б/Н') +
        COALESCE(N' вiд ' + CONVERT(VARCHAR(32), doc_date, 104), N'') AS 'doc_display_name'
FROM
    link_arenda_2_decisions link
    LEFT OUTER JOIN dict_rent_decisions ON dict_rent_decisions.id = link.decision_id

GO

/* view_arch_arenda - displays archive states of the 'arenda' entries */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arch_arenda]'))
DROP VIEW [dbo].[view_arch_arenda]

GO

CREATE VIEW [view_arch_arenda]
AS
SELECT 
       [aa].[archive_id]
      ,[aa].archive_link_code
      ,[aa].[id] AS 'arenda_id'
      ,[aa].[building_id]
      ,[aa].[org_balans_id]
      ,[org_holder].[full_name] AS 'org_balans_full_name'
      ,[org_holder].[short_name] AS 'org_balans_short_name'
      ,[org_holder].[zkpo_code] AS 'org_balans_zkpo'
      ,[org_holder].[industry] AS 'org_balans_industry'
      ,[org_holder].[occupation] AS 'org_balans_occupation'
      ,[org_holder].[vedomstvo] AS 'org_balans_vedomstvo'
      ,[aa].[org_renter_id]
      ,[org_renter].[full_name] AS 'org_renter_full_name'
      ,[org_renter].[short_name] AS 'org_renter_short_name'
      ,[org_renter].[zkpo_code] AS 'org_renter_zkpo'
      ,[org_renter].[industry] AS 'org_renter_industry'
      ,[org_renter].[occupation] AS 'org_renter_occupation'
      ,[org_renter].[vedomstvo] AS 'org_renter_vedomstvo'
      ,[org_renter].[director_fio] AS 'org_renter_director_fio'
      ,[org_renter].[director_phone] AS 'org_renter_director_phone'
      ,[aa].[org_giver_id]
      ,[org_giver].[full_name] AS 'org_giver_full_name'
      ,[org_giver].[short_name] AS 'org_giver_short_name'
      ,[org_giver].[zkpo_code] AS 'org_giver_zkpo'
      ,[org_giver].[industry] AS 'org_giver_industry'
      ,[org_giver].[occupation] AS 'org_giver_occupation'
      ,[org_giver].[vedomstvo] AS 'org_giver_vedomstvo'
      ,[b].[district]      
      ,[b].[street_full_name]
      ,[b].[addr_nomer]
      ,[b].[is_in_privat]
      ,[aa].[balans_id]
      ,[aa].[rent_year]
      ,[aa].[object_kind_id]
      ,[dict_balans_purpose_group].[name] AS 'purpose_group'
      ,[dict_balans_purpose].[name] AS 'purpose'
      ,[aa].purpose_str AS 'object_name'
      ,[aa].[name]
      ,CASE WHEN [aa].[is_privat] = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_privat'
      ,[aa].[is_privat] AS 'is_privat_int'
      ,[aa].[agreement_kind_id]
      ,[dict_arenda_agreement_kind].[name] AS 'agreement_kind'
      ,[aa].[agreement_date]
      ,[aa].[agreement_num]
      ,[aa].[agreement_str]
      ,[aa].[floor_number]
      ,[aa].[num_people]
      ,[aa].cost_narah
      ,[aa].[cost_payed]
      ,[aa].[cost_debt]
      ,[aa].cost_agreement
      ,[aa].[cost_expert_1m]
      ,[aa].cost_expert_total
      ,[aa].[pidstava]
      ,[aa].[pidstava_date]
      ,[aa].[pidstava_num]
      ,[aa].[pidstava_fact]
      ,[aa].[pidstava2]
      ,[aa].[pidstava_num2]
      ,[aa].[pidstava_date2]
      ,[aa].[pidstava_display]
      ,[aa].[rent_start_date]
      ,[aa].[rent_finish_date]
      ,[aa].[rent_actual_finish_date]
      ,[aa].rent_rate AS 'rent_rate_percent'
      --,[aa].[rent_rate_uah] AS 'rent_rate_uah' -- Замена при отображении а также для импорта на портал
	  ,[aa].[rent_rate] AS 'rent_rate_uah'
      ,[aa].rent_square
      ,[aa].[priznak_1nf]
      ,[aa].[debt_timespan]
      ,[aa].[order_num]
      ,[aa].[order_date]
      ,[aa].[order_no2]
      ,[aa].[rishennya_id] AS 'rishennya_code'
      ,[aa].[is_inactive]
      ,[aa].[inactive_date]
      ,CASE WHEN (aa.is_deleted = 1) THEN N'НІ' ELSE N'ТАК' END AS 'agreement_active'
      ,CASE WHEN (aa.is_deleted = 1) THEN 0 ELSE 1 END AS 'agreement_active_int'
      ,[aa].[is_deleted]
      ,[aa].[del_date]
      ,[aa].date_expert
      ,[aa].[is_subarenda] AS 'is_subarenda_int'
      ,CASE WHEN [aa].[is_subarenda] = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_subarenda'
      ,[aa].[privat_kind_id]
      ,[aa].[num_primirnikiv]
      ,[aa].[date_expl_enter]
      ,[aa].[num_akt]
      ,[aa].[date_akt]
      ,[aa].[num_bti]
      ,[aa].[date_bti]
      ,[aa].[svidotstvo_serial]
      ,[aa].[svidotstvo_num]
      ,[aa].[svidotstvo_date]
      ,[dict_arenda_payment_type].[name] AS 'payment_type'
      ,[aa].[modified_by]
      ,[aa].[modify_date]
      ,[aa].note AS 'object_note'
FROM
       arch_arenda aa
       LEFT OUTER JOIN dict_balans_purpose_group ON aa.purpose_group_id = dict_balans_purpose_group.id
       LEFT OUTER JOIN dict_balans_purpose ON aa.purpose_id = dict_balans_purpose.id
       LEFT OUTER JOIN dict_arenda_agreement_kind ON aa.agreement_kind_id = dict_arenda_agreement_kind.id
       LEFT OUTER JOIN dict_arenda_payment_type ON aa.payment_type_id = dict_arenda_payment_type.id
       LEFT OUTER JOIN view_organizations org_holder ON aa.org_balans_id = org_holder.organization_id
       LEFT OUTER JOIN view_organizations org_giver ON aa.org_giver_id = org_giver.organization_id
       LEFT OUTER JOIN view_organizations org_renter ON aa.org_renter_id = org_renter.organization_id
       LEFT OUTER JOIN view_buildings b ON aa.building_id = b.building_id

GO

/* view_arch_arenda_notes - displays archive states of the 'arenda' notes */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arch_arenda_notes]'))
DROP VIEW [dbo].[view_arch_arenda_notes]

GO

CREATE VIEW [view_arch_arenda_notes]
AS
SELECT
       aan.[archive_id] AS 'note_id'
      ,aan.archive_arenda_link_code
      ,aan.[arenda_id]
      ,dict_balans_purpose_group.name AS 'purpose_group'
      ,dict_balans_purpose.name AS 'purpose'
      ,aan.[purpose_str]
      ,aan.[rent_square]
      ,aan.[modify_date]
      ,aan.[modified_by]
      ,aan.[note]
      ,aan.[rent_rate]
      ,aan.[cost_narah]
      ,aan.[cost_agreement]
      ,aan.[is_deleted]
      ,aan.[del_date]
      ,aan.[cost_expert_total]
      ,aan.[date_expert]
      ,dict_arenda_payment_type.name AS 'payment_type'
FROM
    arch_arenda_notes aan
    LEFT OUTER JOIN dict_balans_purpose_group ON aan.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON aan.purpose_id = dict_balans_purpose.id
    LEFT OUTER JOIN dict_arenda_payment_type ON aan.payment_type_id = dict_arenda_payment_type.id

GO

/* view_arch_arenda_link_2_decisions - displays archive states of the 'arenda' documents */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arch_arenda_link_2_decisions]'))
DROP VIEW [dbo].[view_arch_arenda_link_2_decisions]

GO

CREATE VIEW [view_arch_arenda_link_2_decisions]
AS
SELECT
    link.arenda_id AS 'link_id',
    link.rishen_id,
    link.ord,
    link.modified_by,
    link.modify_date,
    link.doc_num,
    link.doc_date,
    link.doc_dodatok,
    link.doc_punkt,
    link.purpose_str,
    link.rent_square,
    dict_rent_decisions.name AS 'decision',
    link.doc_raspor_id,
    link.pidstava,
    link.archive_id,
    link.archive_arenda_link_code,
    COALESCE(pidstava, N'Документ') + N' № ' + COALESCE(RTRIM(doc_num), N'Б/Н') +
        COALESCE(N' вiд ' + CONVERT(VARCHAR(32), doc_date, 104), N'') AS 'doc_display_name'
FROM
    arch_link_arenda_2_decisions link
    LEFT OUTER JOIN dict_rent_decisions ON dict_rent_decisions.id = link.decision_id

GO

/* view_1nf_report_all_org  - The list of organizations that must submit the 1NF report */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_1nf_report_all_org]'))
DROP VIEW [dbo].[view_1nf_report_all_org]

GO

CREATE VIEW [view_1nf_report_all_org]
AS
SELECT
    DISTINCT organization_id
FROM
    view_balans
WHERE
    org_ownership_int IN (32,33,34) OR form_ownership_int IN (32,33,34)
UNION
SELECT
    DISTINCT ar.org_renter_id AS 'organization_id'
FROM
    view_arenda ar
    INNER JOIN organizations org ON org.id = ar.org_renter_id
WHERE
    balans_form_ownership_int IN (32,33,34) AND
    org.form_ownership_id IN (32,33,34)

GO

/* view_building_docs - flat view of the 'building_docs' table */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_building_docs]'))
DROP VIEW [dbo].[view_building_docs]

GO

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

GO

/*
   view_doc_links - represents the parent-child relationship between all documents
*/

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_doc_links]'))
DROP VIEW [dbo].[view_doc_links]

GO

CREATE VIEW [view_doc_links]
AS
SELECT
    dep.id AS 'link_id',
    dep.master_doc_id,
    dep.slave_doc_id,
    parent.doc_num AS 'parent_num',
    parent.doc_date AS 'parent_date',
    parent.topic AS 'parent_topic',
    parent.search_name AS 'parent_search_name',
    parent.kind_id AS 'parent_kind',
    parent.general_kind_id AS 'parent_general_kind',
    parent.is_text_exists AS 'parent_text_exists',
    parent.is_priv_rishen AS 'parent_priv_rishen',
    child.doc_num AS 'child_num',
    child.doc_date AS 'child_date',
    child.topic AS 'child_topic',
    child.search_name AS 'child_search_name',
    child.kind_id AS 'child_kind',
    child.general_kind_id AS 'child_general_kind',
    child.is_text_exists AS 'child_text_exists'
FROM
    doc_dependencies AS dep
    INNER JOIN documents AS parent ON dep.master_doc_id = parent.id
    INNER JOIN documents AS child ON dep.slave_doc_id = child.id
WHERE    
    (NOT dep.master_doc_id IS NULL) AND
    (NOT dep.slave_doc_id IS NULL)

GO

/******************************************************************************/
/*                              Privatization                                 */
/******************************************************************************/

/*
   view_privatization_rishen - filters the 'priv_object_docs' table to display only
   'rishennya' documents from the 'privatization' database
*/

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_privatization_rishen]'))
DROP VIEW [dbo].[view_privatization_rishen]

GO

CREATE VIEW [view_privatization_rishen]
AS
SELECT
    pd.privatization_id,
    pd.document_id,
    doc.doc_date,
    doc.doc_num,
    doc.topic,
    doc.search_name,
    doc.is_text_exists
FROM
    priv_object_docs pd
    INNER JOIN view_documents doc ON pd.document_id = doc.id
WHERE
    /* doc.kind_id IN (SELECT id FROM dict_doc_kind WHERE (name LIKE 'РІШЕННЯ%') OR (name LIKE 'РОЗПОРЯДЖЕННЯ%')) */
    doc.is_priv_rishen = 1

GO

/*
   view_privatization_akts - filters the 'priv_object_docs' table to display only
   'Akt' documents from the 'privatization' database
*/

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_privatization_akts]'))
DROP VIEW [dbo].[view_privatization_akts]

GO

CREATE VIEW [view_privatization_akts]
AS
SELECT
    pd.privatization_id,
    pd.document_id,
    doc.doc_date,
    doc.doc_num,
    doc.topic,
    doc.search_name,
    doc.is_text_exists
FROM
    priv_object_docs pd
    INNER JOIN view_documents doc ON pd.document_id = doc.id
WHERE
    doc.kind_id IN (3,47)
GO

/*
   view_privatization_agreements - filters the 'priv_object_docs' table to display only
   'Agreement' documents from the 'privatization' database
*/

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_privatization_agreements]'))
DROP VIEW [dbo].[view_privatization_agreements]

GO

CREATE VIEW [view_privatization_agreements]
AS
SELECT
    pd.privatization_id,
    pd.document_id,
    doc.doc_date,
    doc.doc_num,
    doc.topic,
    doc.search_name,
    doc.is_text_exists
FROM
    priv_object_docs pd
    INNER JOIN view_documents doc ON pd.document_id = doc.id
WHERE
    doc.kind_id IN (27,35)
GO

/* view_privatization_doc_rel - links between documents from the provatization table */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_privatization_doc_rel]'))
DROP VIEW [dbo].[view_privatization_doc_rel]

GO

/*
CREATE VIEW [view_privatization_doc_rel]
AS
SELECT * FROM view_doc_links
WHERE
    (master_doc_id IN (SELECT document_id FROM priv_object_docs)) AND
    (slave_doc_id IN (SELECT document_id FROM priv_object_docs))
*/

GO

/* view_privatization  - presents information about objects from the 'privatization' table */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_privatization]'))
DROP VIEW [dbo].[view_privatization]

GO

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

GO

/*
   view_privatization_doc_links - represents the parent-child relationship between
   documents from the 'privatization' database
*/

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_privatization_doc_links]'))
DROP VIEW [dbo].[view_privatization_doc_links]

GO

CREATE VIEW [view_privatization_doc_links]
AS
SELECT
    dl.link_id, 
    dl.master_doc_id, 
    dl.slave_doc_id, 
    dl.parent_num, 
    dl.parent_date,
    YEAR(dl.parent_date) AS 'parent_date_year',
    DATEPART(Quarter, dl.parent_date)  AS 'parent_date_quarter',
    dl.parent_topic, 
    dl.parent_search_name, 
    dl.parent_kind AS 'parent_kind_id',
    p_kind.name AS 'parent_kind',
    dl.parent_text_exists, 
    dl.child_num, 
    dl.child_date, 
    YEAR(dl.child_date) AS 'child_date_year',
    DATEPART(Quarter, dl.child_date)  AS 'child_date_quarter',
    dl.child_topic, 
    dl.child_search_name, 
    dl.child_kind AS 'child_kind_id',
    c_kind.name AS 'child_kind',
    dl.child_text_exists
FROM
    view_doc_links dl
    LEFT OUTER JOIN dict_doc_kind p_kind ON p_kind.id = dl.parent_kind
    LEFT OUTER JOIN dict_doc_kind c_kind ON c_kind.id = dl.child_kind
WHERE
    (dl.parent_priv_rishen = 1) AND
    ((c_kind.name LIKE 'РІШЕННЯ%') OR (c_kind.name LIKE 'РОЗПОРЯДЖЕННЯ%'))

GO

/*
   view_privatization_objects - represents the detailed information about objects
   from the 'privatization' database
*/

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_privatization_objects]'))
DROP VIEW [dbo].[view_privatization_objects]

GO

CREATE VIEW [view_privatization_objects]
AS
SELECT
    p.privatization_id,
    p.building_id,
    p.balans_id,
    p.organization_id,
    bal.organization_id AS 'org_balans_id',
    bal.org_full_name AS 'org_balans',
    bal.org_zkpo_code AS 'org_balans_zkpo',
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
    COALESCE(p.purpose_group, bal.purpose_group) AS 'purpose_group',
    p.object_kind,
    p.object_type,
    p.object_history,
    p.obj_floor,
    p.cost,
    p.cost_expert,
    p.expert_date,
    p.note,
    rish.id AS 'rishennya_id',
    rish.doc_num AS 'rishennya_num',
    rish.doc_date AS 'rishennya_date',
    YEAR(rish.doc_date) AS 'rishennya_year',
    rish.topic AS 'rishennya_topic',
    rish.search_name AS 'rishennya_search_name',
    rish.is_text_exists AS 'rishennya_text_exists',
    agr.document_id AS 'agreement_id',
    agr.doc_num AS 'agreement_num',
    agr.doc_date AS 'agreement_date',
    YEAR(agr.doc_date) AS 'agreement_year',
    agr.topic AS 'agreement_topic',
    agr.search_name AS 'agreement_search_name',
    agr.is_text_exists AS 'agreement_text_exists',
    akt.document_id AS 'akt_id',
    akt.doc_num AS 'akt_num',
    akt.doc_date AS 'akt_date',
    YEAR(akt.doc_date) AS 'akt_year',
    akt.topic AS 'akt_topic',
    akt.search_name AS 'akt_search_name',
    akt.is_text_exists AS 'akt_text_exists'
FROM
    view_privatization p
    LEFT OUTER JOIN view_balans bal ON bal.balans_id = p.balans_id
    LEFT OUTER JOIN view_documents rish ON rish.id = p.rishen_doc_id
    LEFT OUTER JOIN view_privatization_akts akt ON akt.privatization_id = p.privatization_id
    LEFT OUTER JOIN view_privatization_agreements agr ON agr.privatization_id = p.privatization_id

GO

/* view_priv_object_docs - detailed view of the priv_object_docs table */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_priv_object_docs]'))
DROP VIEW [dbo].[view_priv_object_docs]

GO

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

GO

/******************************************************************************/
/*                         Object right transitions                           */
/******************************************************************************/

/* view_obj_right_akts - Akts that are used in the object_rights table  */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_obj_right_akts]'))
DROP VIEW [dbo].[view_obj_right_akts]

GO

CREATE VIEW view_obj_right_akts
AS
SELECT * FROM documents WHERE (id IN (SELECT akt_id FROM object_rights))

GO

/* view_obj_right_rozp - Rozporadjennia that are used in the object_rights table  */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_obj_right_rozp]'))
DROP VIEW [dbo].[view_obj_right_rozp]

GO

CREATE VIEW view_obj_right_rozp
AS
SELECT * FROM documents WHERE (id IN (SELECT rozp_id FROM object_rights))

GO

/* view_obj_right_org_from - organizations that are used in the object_rights table  */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_obj_right_org_from]'))
DROP VIEW [dbo].[view_obj_right_org_from]

GO

CREATE VIEW view_obj_right_org_from
AS
SELECT * FROM view_organizations WHERE (organization_id IN (SELECT org_from_id FROM object_rights))

GO

/* view_obj_right_org_to - organizations that are used in the object_rights table  */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_obj_right_org_to]'))
DROP VIEW [dbo].[view_obj_right_org_to]

GO

CREATE VIEW view_obj_right_org_to
AS
SELECT * FROM view_organizations WHERE (organization_id IN (SELECT org_to_id FROM object_rights))

GO

/* view_object_rights - primary view that represents the 'object_rights' table */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_object_rights]'))
DROP VIEW [dbo].[view_object_rights]

GO

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

GO

/******************************************************************************/
/*                                Documents                                   */
/******************************************************************************/

/* view_parent_documents - selects all documents that have no child documents */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_parent_documents]'))
DROP VIEW [dbo].[view_parent_documents]

GO

CREATE VIEW [view_parent_documents]
AS
SELECT * FROM documents WHERE (id IN (SELECT document_id FROM building_docs))

GO

/* view_docs_objects_akts_base1 - a base view for view_docs_objects_akts (akts and related buildings) */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_docs_objects_akts_base1]'))
DROP VIEW [dbo].[view_docs_objects_akts_base1]

GO

CREATE VIEW [view_docs_objects_akts_base1]
AS
SELECT
       [dl].[link_id]
      ,[dl].[master_doc_id]
      ,[dl].[slave_doc_id]
      ,[dl].[parent_num]
      ,[dl].[parent_date]
      ,[dl].[parent_topic]
      ,[dl].[parent_search_name]
      ,[dl].[parent_kind]
      ,[dl].[parent_general_kind]
      ,[dl].[parent_text_exists]
      ,[dl].[child_num]
      ,[dl].[child_date]
      ,[dl].[child_topic]
      ,[dl].[child_search_name]
      ,[dl].[child_kind]
      ,[dl].[child_general_kind]
      ,[dl].[child_text_exists]
      ,[bd].building_id AS 'akt_building_id'
FROM
    view_doc_links dl
    INNER JOIN building_docs bd ON bd.document_id = dl.slave_doc_id
WHERE
    (dl.child_kind = 3) OR (dl.child_kind = 36) OR (dl.child_kind = 47)

GO

/* view_docs_objects_akts_base - a base view for view_docs_objects_akts */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_docs_objects_akts_base]'))
DROP VIEW [dbo].[view_docs_objects_akts_base]

GO

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

GO

/* view_docs_objects_akts */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_docs_objects_akts]'))
DROP VIEW [dbo].[view_docs_objects_akts]

GO

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

GO

/******************************************************************************/
/*                              Orendna Plata                                 */
/******************************************************************************/

/* view_rent_not_submitted */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_not_submitted]'))
DROP VIEW [dbo].[view_rent_not_submitted]

GO

CREATE VIEW [view_rent_not_submitted]
AS
WITH period AS (SELECT TOP 5 id, name FROM dict_rent_period ORDER BY period_end DESC)
SELECT
    arenda.org_balans_id organization_id,
    org.full_name,
    org.short_name,
    org.zkpo_code,
    org.sfera_upr_id AS 'org_sfera_upr_id',
    org.form_ownership_id AS 'org_form_ownership_id',
	org.addr_distr_new_id AS 'org_district_id',
    dict_org_occupation.name AS 'rent_occupation',
	org.director_email email,
	org.director_phone phone,
	org.director_fio responsible_fio,
    period.id AS 'period_id',
    period.name AS 'period_name'
FROM
	(select distinct org_balans_id from arenda where arenda.agreement_state in (1, 2, 3) and agreement_kind_id in (1, 3, 7, 8, 11, 18)) arenda
	cross join period
	left join
	(
		select distinct arenda.org_balans_id, period.id rent_period_id
		from arenda, period
		where exists(select 1 from arenda_payments where arenda_payments.arenda_id = arenda.id and arenda_payments.rent_period_id = period.id)
	) x
	ON arenda.org_balans_id = x.org_balans_id AND period.id = x.rent_period_id
    INNER JOIN organizations org ON org.id = arenda.org_balans_id
    LEFT OUTER JOIN dict_org_occupation on dict_org_occupation.id = org.occupation_id
WHERE x.org_balans_id is null

GO

/* view_rent_debt_grouped */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_debt_grouped]'))
DROP VIEW [dbo].[view_rent_debt_grouped]

GO

CREATE VIEW [view_rent_debt_grouped]
AS
SELECT
    rent_payment_id,
    SUM(debt_total) AS 'debt_total',
    SUM(debt_3_month) AS 'debt_3_month',
    SUM(debt_12_month) AS 'debt_12_month',
    SUM(debt_3_years) AS 'debt_3_years',
    SUM(debt_over_3_years) AS 'debt_over_3_years',
    SUM(debt_spysano) AS 'debt_spysano',
    SUM(num_zahodiv_total) AS 'num_zahodiv_total',
    SUM(num_zahodiv_zvit) AS 'num_zahodiv_zvit',
    SUM(num_pozov_total) AS 'num_pozov_total',
    SUM(num_pozov_zvit) AS 'num_pozov_zvit',
    SUM(num_pozov_zadov_total) AS 'num_pozov_zadov_total',
    SUM(num_pozov_zadov_zvit) AS 'num_pozov_zadov_zvit',
    SUM(num_pozov_vikon_total) AS 'num_pozov_vikon_total',
    SUM(num_pozov_vikon_zvit) AS 'num_pozov_vikon_zvit',
    SUM(debt_pogasheno_total) AS 'debt_pogasheno_total',
    SUM(debt_pogasheno_zvit) AS 'debt_pogasheno_zvit',
    SUM(debt_v_mezhah_vitrat) AS 'debt_v_mezhah_vitrat'
FROM
    rent_payment_debt
GROUP BY
    rent_payment_id

GO

/* view_rent_debt_by_renter */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_debt_by_renter]'))
DROP VIEW [dbo].[view_rent_debt_by_renter]

GO

CREATE VIEW [view_rent_debt_by_renter]
AS
SELECT
    rent_payment_id,
    rent_renter_org_id,
    agreement_flag,
    SUM(debt_total) AS 'debt_total',
    SUM(debt_3_month) AS 'debt_3_month',
    SUM(debt_12_month) AS 'debt_12_month',
    SUM(debt_3_years) AS 'debt_3_years',
    SUM(debt_over_3_years) AS 'debt_over_3_years',
    SUM(debt_spysano) AS 'debt_spysano',
    SUM(num_zahodiv_total) AS 'num_zahodiv_total',
    SUM(num_zahodiv_zvit) AS 'num_zahodiv_zvit',
    SUM(num_pozov_total) AS 'num_pozov_total',
    SUM(num_pozov_zvit) AS 'num_pozov_zvit',
    SUM(num_pozov_zadov_total) AS 'num_pozov_zadov_total',
    SUM(num_pozov_zadov_zvit) AS 'num_pozov_zadov_zvit',
    SUM(num_pozov_vikon_total) AS 'num_pozov_vikon_total',
    SUM(num_pozov_vikon_zvit) AS 'num_pozov_vikon_zvit',
    SUM(debt_pogasheno_total) AS 'debt_pogasheno_total',
    SUM(debt_pogasheno_zvit) AS 'debt_pogasheno_zvit',
    SUM(debt_v_mezhah_vitrat) AS 'debt_v_mezhah_vitrat'
FROM
    rent_payment_debt
GROUP BY
    rent_payment_id,
    rent_renter_org_id,
    agreement_flag

GO

/* view_rent_obj_grouped */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_obj_grouped]'))
DROP VIEW [dbo].[view_rent_obj_grouped]

GO

CREATE VIEW [view_rent_obj_grouped]
AS
SELECT
    rent_payment_id,
    SUM(sqr_total) AS 'sqr_total',
    SUM(sqr_rented) AS 'sqr_rented',
    SUM(sqr_free) AS 'sqr_free',
    SUM(sqr_korysna) AS 'sqr_korysna',
    SUM(sqr_mzk) AS 'sqr_mzk'
FROM
    rent_object
GROUP BY
    rent_payment_id

GO

/* view_rent_by_balans_org */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_by_balans_org]'))
DROP VIEW [dbo].[view_rent_by_balans_org]

GO

CREATE VIEW [view_rent_by_balans_org]
AS
SELECT
    p.id AS 'payment_id',
    p.org_balans_id,
    org.full_name AS 'org_full_name',
    org.short_name AS 'org_short_name',
    org.sfera_upr_id AS 'org_sfera_upr_id',
    org.form_ownership_id AS 'org_form_ownership_id',
    org.addr_distr_new_id AS 'org_district_id',
    rbo.rent_dkk_id,
    UPPER(dict_rent_dkk.name) AS 'rent_dkk_name',
    dict_rent_dkk.short_name AS 'rent_dkk_short_name',
    rbo.rent_occupation_id,
    UPPER(dict_rent_occupation.name) AS 'org_rent_occupation',
    p.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    p.sqr_balans,
    p.payment_zvit_uah,
    p.received_zvit_uah,
    p.received_zvit_uah * 0.5 AS 'rozrah_50_uah',
    p.received_nar_zvit_uah,
    p.debt_total_uah,
    p.debt_zvit_uah,
    p.budget_narah_50_uah,
    p.budget_zvit_50_uah,
    p.budget_prev_50_uah,
    p.budget_debt_50_uah,
    p.budget_debt_30_50_uah,
    p.sqr_cmk,
    p.budget_narah_50_cmk_uah,
    p.budget_zvit_50_cmk_uah,
    p.budget_debt_50_cmk_uah,
    p.budget_zvit_50_in_uah,
    p.zkpo_code,
    p.email,
    p.phone,
    p.responsible_fio,
    p.director_fio,
    p.buhgalter_fio,
    p.num_agreements,
    --(select SUM(
    p.num_renters,
    p.debt_v_mezhah_vitrat,
    p.budget_zvit_50_old,
    p.post_address,
    p.pererahunok_50_b,
    debt.debt_total,
    debt.debt_3_month,
    debt.debt_12_month,
    debt.debt_3_years,
    debt.debt_over_3_years,
    debt.debt_spysano,
    debt.num_zahodiv_total,
    debt.num_zahodiv_zvit,
    debt.num_pozov_total,
    debt.num_pozov_zvit,
    debt.num_pozov_zadov_total,
    debt.num_pozov_zadov_zvit,
    debt.num_pozov_vikon_total,
    debt.num_pozov_vikon_zvit,
    debt.debt_pogasheno_total,
    debt.debt_pogasheno_zvit,
    obj.sqr_total AS 'obj_sqr_total',
    obj.sqr_rented AS 'obj_sqr_rented',
    obj.sqr_free AS 'obj_sqr_free',
    obj.sqr_korysna AS 'obj_sqr_korysna',
    obj.sqr_mzk AS 'obj_sqr_mzk'
FROM
    rent_payment p
    LEFT OUTER JOIN dict_rent_period ON dict_rent_period.id = p.rent_period_id
    LEFT OUTER JOIN organizations org ON org.id = p.org_balans_id
    LEFT OUTER JOIN rent_balans_org rbo ON rbo.organization_id = p.org_balans_id
    LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = rbo.rent_occupation_id
    LEFT OUTER JOIN dict_rent_dkk ON dict_rent_dkk.id = rbo.rent_dkk_id
    LEFT OUTER JOIN view_rent_debt_grouped debt ON debt.rent_payment_id = p.id
    LEFT OUTER JOIN view_rent_obj_grouped obj ON obj.rent_payment_id = p.id


UNION ALL

SELECT
    -p.id AS 'payment_id',
    p.org_balans_id,
    org.full_name AS 'org_full_name',
    org.short_name AS 'org_short_name',
    org.sfera_upr_id AS 'org_sfera_upr_id',
    org.form_ownership_id AS 'org_form_ownership_id',
    org.addr_distr_new_id AS 'org_district_id',
    NULL rent_dkk_id,
    NULL AS 'rent_dkk_name',
    NULL AS 'rent_dkk_short_name',
    org.occupation_id,
    UPPER(dict_org_occupation.name) AS 'org_rent_occupation',
    p.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    b.sqr_total, --p.sqr_balans,
    p.payment_narah, --p.payment_zvit_uah,
    p.payment_received, --p.received_zvit_uah,
    p.payment_received * 0.5 AS 'rozrah_50_uah',
    p.payment_nar_zvit, --p.received_nar_zvit_uah,
    p.debt_total, --p.debt_total_uah,
    p.debt_zvit, --p.debt_zvit_uah,
	org_arc.budget_narah_50_uah,
    org_arc.budget_zvit_50_uah,
    org_arc.budget_prev_50_uah,
    p.budget_debt_50_uah,
    org_arc.budget_debt_30_50_uah,
    cmk.sqr_cmk,
    cmk.budget_narah_50_cmk_uah,
    cmk.budget_zvit_50_cmk_uah,
    cmk.budget_debt_50_cmk_uah,
    NULL, --p.budget_zvit_50_in_uah,
    org.zkpo_code,
    org.director_email,
    org.director_phone,
    p.modified_by,
    org.director_fio,
    org.buhgalter_fio,
    --p.num_agreements,
    (select SUM(s.num_agreements) from
		(SELECT COUNT(DISTINCT A1.agreement_num) as num_agreements
		FROM arenda a1
		LEFT OUTER JOIN arenda_payments p1 ON a1.id = p1.arenda_id AND p1.rent_period_id = p.rent_period_id
		where a1.agreement_state = 1 AND (A1.is_deleted IS NULL OR A1.is_deleted = 0) AND 
		a1.org_balans_id = p.org_balans_id
		GROUP BY a1.agreement_num,a1.agreement_date,a1.rent_start_date,a1.rent_finish_date) s
    
    ) as num_agreements,
    
    
    p.num_renters,
    p.debt_v_mezhah_vitrat,
    0, --p.budget_zvit_50_old, --not used
	isnull(org.addr_street_name + ', ', '')
	+ isnull(org.addr_nomer + ' ', '')
	+ isnull(org.addr_nomer2 + ' ', '')
	+ isnull(org.addr_korpus, '')
		'post_address',
    org_arc.budget_narah_50_uah, --p.pererahunok_50_b,
    p.debt_total,
    p.debt_3_month,
    p.debt_12_month,
    p.debt_3_years,
    p.debt_over_3_years,
    p.debt_spysano,
    p.num_zahodiv_total,
    p.num_zahodiv_zvit,
    p.num_pozov_total,
    p.num_pozov_zvit,
    p.num_pozov_zadov_total,
    p.num_pozov_zadov_zvit,
    p.num_pozov_vikon_total,
    p.num_pozov_vikon_zvit,
    p.debt_pogasheno_total,
    p.debt_pogasheno_zvit,
    b.sqr_total, --obj.sqr_total AS 'obj_sqr_total', --not used
    p.sqr_total_rent AS 'obj_sqr_rented',
    b.sqr_free, --obj.sqr_free AS 'obj_sqr_free',
    b.sqr_kor, --obj.sqr_korysna AS 'obj_sqr_korysna',
    b.sqr_free_mzk --obj.sqr_mzk AS 'obj_sqr_mzk'
FROM
	(SELECT a.org_balans_id
			,p.rent_period_id
			,max(p.id) id
			,max(p.modified_by) modified_by
			,sum(sqr_total_rent) sqr_total_rent
			,sum(payment_narah) payment_narah
			,sum(payment_received) payment_received
			,sum(payment_nar_zvit) payment_nar_zvit
			,sum(debt_total) debt_total
			,sum(debt_zvit) debt_zvit
			,sum(debt_3_month) debt_3_month
			,sum(debt_12_month) debt_12_month
			,sum(debt_3_years) debt_3_years
			,sum(debt_over_3_years) debt_over_3_years
			,sum(debt_v_mezhah_vitrat) debt_v_mezhah_vitrat
			,sum(debt_spysano) debt_spysano
			,sum(num_zahodiv_total) num_zahodiv_total
			,sum(num_zahodiv_zvit) num_zahodiv_zvit
			,sum(num_pozov_total) num_pozov_total
			,sum(num_pozov_zvit) num_pozov_zvit
			,sum(num_pozov_zadov_total) num_pozov_zadov_total
			,sum(num_pozov_zadov_zvit) num_pozov_zadov_zvit
			,sum(num_pozov_vikon_total) num_pozov_vikon_total
			,sum(num_pozov_vikon_zvit) num_pozov_vikon_zvit
			,sum(debt_pogasheno_total) debt_pogasheno_total
			,sum(debt_pogasheno_zvit) debt_pogasheno_zvit
			,sum(budget_debt_50_uah) budget_debt_50_uah
			--,count(distinct a.id) num_agreements
			,count(distinct a.org_renter_id) num_renters
		FROM [arenda_payments] p
		INNER JOIN arenda a ON a.id = p.arenda_id and a.agreement_state = 1
		GROUP BY a.org_balans_id, p.rent_period_id) p
	LEFT JOIN 
		(
		SELECT balans.organization_id, 
			SUM(balans.sqr_total) sqr_total, 
			SUM(balans.sqr_free) sqr_free,
			SUM(balans.sqr_kor) sqr_kor,
			SUM(balans.sqr_free_mzk) sqr_free_mzk
		FROM view_balans balans
		GROUP BY balans.organization_id
		) b ON b.organization_id = p.org_balans_id
    INNER JOIN dict_rent_period ON dict_rent_period.id = p.rent_period_id
    INNER JOIN organizations org ON org.id = p.org_balans_id
	CROSS APPLY
		(
			SELECT TOP 1
			   x.[id]
			  ,x.[master_org_id]
			  ,x.[last_state]
			  ,x.[occupation_id]
			  ,x.[status_id]
			  ,x.[form_gosp_id]
			  ,x.[form_ownership_id]
			  ,x.[gosp_struct_id]
			  ,x.[organ_id]
			  ,x.[industry_id]
			  ,x.[nomer_obj]
			  ,x.[zkpo_code]
			  ,x.[addr_distr_old_id]
			  ,x.[addr_distr_new_id]
			  ,x.[addr_street_name]
			  ,x.[addr_street_id]
			  ,x.[addr_nomer]
			  ,x.[addr_nomer2]
			  ,x.[addr_korpus]
			  ,x.[addr_zip_code]
			  ,x.[addr_misc]
			  ,x.[director_fio]
			  ,x.[director_phone]
			  ,x.[director_fio_kogo]
			  ,x.[director_title]
			  ,x.[director_title_kogo]
			  ,x.[director_doc]
			  ,x.[director_doc_kogo]
			  ,x.[director_email]
			  ,x.[buhgalter_fio]
			  ,x.[buhgalter_phone]
			  ,x.[buhgalter_email]
			  ,x.[num_buildings]
			  ,x.[full_name]
			  ,x.[short_name]
			  ,x.[priznak_id]
			  ,x.[title_form_id]
			  ,x.[form_1nf_id]
			  ,x.[vedomstvo_id]
			  ,x.[title_id]
			  ,x.[form_id]
			  ,x.[gosp_struct_type_id]
			  ,x.[search_name]
			  ,x.[name_komu]
			  ,x.[fax]
			  ,x.[registration_auth]
			  ,x.[registration_num]
			  ,x.[registration_date]
			  ,x.[registration_svidot]
			  ,x.[l_year]
			  ,x.[date_l_year]
			  ,x.[sqr_on_balance]
			  ,x.[sqr_manufact]
			  ,x.[sqr_non_manufact]
			  ,x.[sqr_free_for_rent]
			  ,x.[sqr_total]
			  ,x.[sqr_rented]
			  ,x.[sqr_privat]
			  ,x.[sqr_given_for_rent]
			  ,x.[sqr_znyata_z_balansu]
			  ,x.[sqr_prodaj]
			  ,x.[sqr_spisani_zneseni]
			  ,x.[sqr_peredana]
			  ,x.[num_objects]
			  ,x.[kved_code]
			  ,x.[date_stat_spravka]
			  ,x.[koatuu]
			  ,x.[modified_by]
			  ,x.[modify_date]
			  ,x.[share_type_id]
			  ,x.[share]
			  ,x.[bank_name]
			  ,x.[bank_mfo]
			  ,x.[is_deleted]
			  ,x.[del_date]
			  ,x.[otdel_gukv_id]
			  ,x.[arch_id]
			  ,x.[arch_flag]
			  ,x.[is_liquidated]
			  ,x.[liquidation_date]
			  ,x.[pidp_rda]
			  ,x.[is_arend]
			  ,x.[beg_state_date]
			  ,x.[end_state_date]
			  ,x.[mayno_id]
			  ,x.[contact_email]
			  ,x.[contact_posada_id]
			  ,x.[nadhodjennya_id]
			  ,x.[vibuttya_id]
			  ,x.[privat_status_id]
			  ,x.[cur_state_id]
			  ,x.[sfera_upr_id]
			  ,x.[plan_zone_id]
			  ,x.[registr_org_id]
			  ,x.[nadhodjennya_date]
			  ,x.[vibuttya_date]
			  ,x.[chastka]
			  ,x.[registration_rish]
			  ,x.[registration_dov_date]
			  ,x.[registration_corp]
			  ,x.[strok_start_date]
			  ,x.[strok_end_date]
			  ,x.[stat_fond]
			  ,x.[size_plus]
			  ,x.[addr_zip_code_3]
			  ,x.[old_industry_id]
			  ,x.[old_occupation_id]
			  ,x.[old_organ_id]
			  ,x.[form_vlasn_vibuttya_id]
			  ,x.[addr_city]
			  ,x.[addr_flat_num]
			  ,x.[povnovajennia]
			  ,x.[povnov_osoba_fio]
			  ,x.[povnov_passp_seria]
			  ,x.[povnov_passp_num]
			  ,x.[povnov_passp_auth]
			  ,x.[povnov_passp_date]
			  ,x.[director_passp_seria]
			  ,x.[director_passp_num]
			  ,x.[director_passp_auth]
			  ,x.[director_passp_date]
			  ,x.[registration_svid_date]
			  ,x.[origin_db]
			  ,x.[budg_payments_rate]
			  ,x.[is_under_closing]
			  ,x.[phys_addr_street_id]
			  ,x.[phys_addr_district_id]
			  ,x.[phys_addr_nomer]
			  ,x.[phys_addr_zip_code]
			  ,x.[phys_addr_misc]
			  ,x.[contribution_rate]
			  ,x.[budget_narah_50_uah]
			  ,x.[budget_zvit_50_uah]
			  ,x.[budget_prev_50_uah]
			  ,x.[budget_debt_30_50_uah]
			  ,x.[is_special_organization]
			  ,x.[payment_budget_special]
			  ,x.[konkurs_payments]
			  ,x.[unknown_payments]
			  ,x.[unknown_payment_note]
			FROM (
				SELECT CAST(GETDATE() AS date) effective_date
					  ,[id]
					  ,[master_org_id]
					  ,[last_state]
					  ,[occupation_id]
					  ,[status_id]
					  ,[form_gosp_id]
					  ,[form_ownership_id]
					  ,[gosp_struct_id]
					  ,[organ_id]
					  ,[industry_id]
					  ,[nomer_obj]
					  ,[zkpo_code]
					  ,[addr_distr_old_id]
					  ,[addr_distr_new_id]
					  ,[addr_street_name]
					  ,[addr_street_id]
					  ,[addr_nomer]
					  ,[addr_nomer2]
					  ,[addr_korpus]
					  ,[addr_zip_code]
					  ,[addr_misc]
					  ,[director_fio]
					  ,[director_phone]
					  ,[director_fio_kogo]
					  ,[director_title]
					  ,[director_title_kogo]
					  ,[director_doc]
					  ,[director_doc_kogo]
					  ,[director_email]
					  ,[buhgalter_fio]
					  ,[buhgalter_phone]
					  ,[buhgalter_email]
					  ,[num_buildings]
					  ,[full_name]
					  ,[short_name]
					  ,[priznak_id]
					  ,[title_form_id]
					  ,[form_1nf_id]
					  ,[vedomstvo_id]
					  ,[title_id]
					  ,[form_id]
					  ,[gosp_struct_type_id]
					  ,[search_name]
					  ,[name_komu]
					  ,[fax]
					  ,[registration_auth]
					  ,[registration_num]
					  ,[registration_date]
					  ,[registration_svidot]
					  ,[l_year]
					  ,[date_l_year]
					  ,[sqr_on_balance]
					  ,[sqr_manufact]
					  ,[sqr_non_manufact]
					  ,[sqr_free_for_rent]
					  ,[sqr_total]
					  ,[sqr_rented]
					  ,[sqr_privat]
					  ,[sqr_given_for_rent]
					  ,[sqr_znyata_z_balansu]
					  ,[sqr_prodaj]
					  ,[sqr_spisani_zneseni]
					  ,[sqr_peredana]
					  ,[num_objects]
					  ,[kved_code]
					  ,[date_stat_spravka]
					  ,[koatuu]
					  ,[modified_by]
					  ,[modify_date]
					  ,[share_type_id]
					  ,[share]
					  ,[bank_name]
					  ,[bank_mfo]
					  ,[is_deleted]
					  ,[del_date]
					  ,[otdel_gukv_id]
					  ,[arch_id]
					  ,[arch_flag]
					  ,[is_liquidated]
					  ,[liquidation_date]
					  ,[pidp_rda]
					  ,[is_arend]
					  ,[beg_state_date]
					  ,[end_state_date]
					  ,[mayno_id]
					  ,[contact_email]
					  ,[contact_posada_id]
					  ,[nadhodjennya_id]
					  ,[vibuttya_id]
					  ,[privat_status_id]
					  ,[cur_state_id]
					  ,[sfera_upr_id]
					  ,[plan_zone_id]
					  ,[registr_org_id]
					  ,[nadhodjennya_date]
					  ,[vibuttya_date]
					  ,[chastka]
					  ,[registration_rish]
					  ,[registration_dov_date]
					  ,[registration_corp]
					  ,[strok_start_date]
					  ,[strok_end_date]
					  ,[stat_fond]
					  ,[size_plus]
					  ,[addr_zip_code_3]
					  ,[old_industry_id]
					  ,[old_occupation_id]
					  ,[old_organ_id]
					  ,[form_vlasn_vibuttya_id]
					  ,[addr_city]
					  ,[addr_flat_num]
					  ,[povnovajennia]
					  ,[povnov_osoba_fio]
					  ,[povnov_passp_seria]
					  ,[povnov_passp_num]
					  ,[povnov_passp_auth]
					  ,[povnov_passp_date]
					  ,[director_passp_seria]
					  ,[director_passp_num]
					  ,[director_passp_auth]
					  ,[director_passp_date]
					  ,[registration_svid_date]
					  ,[origin_db]
					  ,[budg_payments_rate]
					  ,[is_under_closing]
					  ,[phys_addr_street_id]
					  ,[phys_addr_district_id]
					  ,[phys_addr_nomer]
					  ,[phys_addr_zip_code]
					  ,[phys_addr_misc]
					  ,[contribution_rate]
					  ,[budget_narah_50_uah]
					  ,[budget_zvit_50_uah]
					  ,[budget_prev_50_uah]
					  ,[budget_debt_30_50_uah]
					  ,[is_special_organization]
					  ,[payment_budget_special]
					  ,[konkurs_payments]
					  ,[unknown_payments]
					  ,[unknown_payment_note]
				FROM [organizations], (select max(id) rent_period_id from dict_rent_period) dict_rent_period
				UNION ALL
				SELECT archive_create_date effective_date
					  ,[id]
					  ,[master_org_id]
					  ,[last_state]
					  ,[occupation_id]
					  ,[status_id]
					  ,[form_gosp_id]
					  ,[form_ownership_id]
					  ,[gosp_struct_id]
					  ,[organ_id]
					  ,[industry_id]
					  ,[nomer_obj]
					  ,[zkpo_code]
					  ,[addr_distr_old_id]
					  ,[addr_distr_new_id]
					  ,[addr_street_name]
					  ,[addr_street_id]
					  ,[addr_nomer]
					  ,[addr_nomer2]
					  ,[addr_korpus]
					  ,[addr_zip_code]
					  ,[addr_misc]
					  ,[director_fio]
					  ,[director_phone]
					  ,[director_fio_kogo]
					  ,[director_title]
					  ,[director_title_kogo]
					  ,[director_doc]
					  ,[director_doc_kogo]
					  ,[director_email]
					  ,[buhgalter_fio]
					  ,[buhgalter_phone]
					  ,[buhgalter_email]
					  ,[num_buildings]
					  ,[full_name]
					  ,[short_name]
					  ,[priznak_id]
					  ,[title_form_id]
					  ,[form_1nf_id]
					  ,[vedomstvo_id]
					  ,[title_id]
					  ,[form_id]
					  ,[gosp_struct_type_id]
					  ,[search_name]
					  ,[name_komu]
					  ,[fax]
					  ,[registration_auth]
					  ,[registration_num]
					  ,[registration_date]
					  ,[registration_svidot]
					  ,[l_year]
					  ,[date_l_year]
					  ,[sqr_on_balance]
					  ,[sqr_manufact]
					  ,[sqr_non_manufact]
					  ,[sqr_free_for_rent]
					  ,[sqr_total]
					  ,[sqr_rented]
					  ,[sqr_privat]
					  ,[sqr_given_for_rent]
					  ,[sqr_znyata_z_balansu]
					  ,[sqr_prodaj]
					  ,[sqr_spisani_zneseni]
					  ,[sqr_peredana]
					  ,[num_objects]
					  ,[kved_code]
					  ,[date_stat_spravka]
					  ,[koatuu]
					  ,[modified_by]
					  ,[modify_date]
					  ,[share_type_id]
					  ,[share]
					  ,[bank_name]
					  ,[bank_mfo]
					  ,[is_deleted]
					  ,[del_date]
					  ,[otdel_gukv_id]
					  ,[arch_id]
					  ,[arch_flag]
					  ,[is_liquidated]
					  ,[liquidation_date]
					  ,[pidp_rda]
					  ,[is_arend]
					  ,[beg_state_date]
					  ,[end_state_date]
					  ,[mayno_id]
					  ,[contact_email]
					  ,[contact_posada_id]
					  ,[nadhodjennya_id]
					  ,[vibuttya_id]
					  ,[privat_status_id]
					  ,[cur_state_id]
					  ,[sfera_upr_id]
					  ,[plan_zone_id]
					  ,[registr_org_id]
					  ,[nadhodjennya_date]
					  ,[vibuttya_date]
					  ,[chastka]
					  ,[registration_rish]
					  ,[registration_dov_date]
					  ,[registration_corp]
					  ,[strok_start_date]
					  ,[strok_end_date]
					  ,[stat_fond]
					  ,[size_plus]
					  ,[addr_zip_code_3]
					  ,[old_industry_id]
					  ,[old_occupation_id]
					  ,[old_organ_id]
					  ,[form_vlasn_vibuttya_id]
					  ,[addr_city]
					  ,[addr_flat_num]
					  ,[povnovajennia]
					  ,[povnov_osoba_fio]
					  ,[povnov_passp_seria]
					  ,[povnov_passp_num]
					  ,[povnov_passp_auth]
					  ,[povnov_passp_date]
					  ,[director_passp_seria]
					  ,[director_passp_num]
					  ,[director_passp_auth]
					  ,[director_passp_date]
					  ,[registration_svid_date]
					  ,[origin_db]
					  ,[budg_payments_rate]
					  ,[is_under_closing]
					  ,[phys_addr_street_id]
					  ,[phys_addr_district_id]
					  ,[phys_addr_nomer]
					  ,[phys_addr_zip_code]
					  ,[phys_addr_misc]
					  ,[contribution_rate]
					  ,[budget_narah_50_uah]
					  ,[budget_zvit_50_uah]
					  ,[budget_prev_50_uah]
					  ,[budget_debt_30_50_uah]
					  ,[is_special_organization]
					  ,[payment_budget_special]
					  ,[konkurs_payments]
					  ,[unknown_payments]
					  ,[unknown_payment_note]
				FROM arch_organizations
			) x WHERE x.id = p.org_balans_id
				AND DATEADD(d, 20, dict_rent_period.period_end) >= x.effective_date
			ORDER BY x.effective_date DESC
		) as org_arc
    LEFT OUTER JOIN dict_org_occupation ON dict_org_occupation.id = org_arc.occupation_id
	LEFT OUTER JOIN
		(SELECT
			arenda_rented.org_renter_id,
			arenda_payments_cmk.rent_period_id,
			SUM(arenda_payments_cmk.cmk_sqr_rented) sqr_cmk,
			SUM(arenda_payments_cmk.cmk_payment_to_budget) budget_zvit_50_cmk_uah,
			SUM(arenda_payments_cmk.cmk_payment_narah) budget_narah_50_cmk_uah,
			SUM(arenda_payments_cmk.cmk_rent_debt) budget_debt_50_cmk_uah
		FROM
			arenda_rented
			INNER JOIN arenda_payments_cmk on arenda_payments_cmk.arenda_rented_id = arenda_rented.id
		WHERE
			ISNULL(arenda_rented.is_deleted, 0) = 0
		GROUP BY
			arenda_rented.org_renter_id,
			arenda_payments_cmk.rent_period_id) cmk ON cmk.org_renter_id = p.org_balans_id AND cmk.rent_period_id = p.rent_period_id
WHERE NOT EXISTS (SELECT 1 FROM rent_payment WHERE rent_payment.org_balans_id = p.org_balans_id AND rent_payment.rent_period_id = p.rent_period_id)

GO

/* view_rent_by_renters */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_by_renters]'))
DROP VIEW [dbo].[view_rent_by_renters]

GO

CREATE VIEW [view_rent_by_renters]
AS
SELECT
    pbr.id AS 'rent_id',
    pbr.rent_payment_id,
    pbr.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    pbr.rent_renter_org_id,
    pbr.renter_organization_id,
    renter.name AS 'renter_name',
    renter.zkpo_code AS 'renter_zkpo',
    bal_org.id AS 'bal_org_id',
    bal_org.full_name AS 'bal_org_full_name',
    bal_org.short_name AS 'bal_org_short_name',
    bal_org.zkpo_code AS 'bal_org_zkpo',
    bal_org.sfera_upr_id AS 'bal_org_sfera_upr_id',
    bal_org.form_ownership_id AS 'bal_org_form_ownership_id',
    bal_org.addr_distr_new_id AS 'bal_org_district_id',
    rent_balans_org.rent_occupation_id AS 'bal_org_occupation_id',
    dict_rent_occupation.name AS 'bal_org_occupation',
    giver_org.id AS 'giver_org_id',
    giver_org.full_name AS 'giver_org_full_name',
    giver_org.short_name AS 'giver_org_short_name',
    giver_org.zkpo_code AS 'giver_org_zkpo',
    giver_org.sfera_upr_id AS 'giver_org_sfera_upr_id',
    giver_org.form_ownership_id AS 'giver_org_form_ownership_id',
    giver_org.addr_distr_new_id AS 'giver_org_district_id',
    pbr.sqr_total_rent,
    pbr.sqr_payed_by_percent,
    pbr.sqr_payed_by_1uah,
    pbr.sqr_payed_hourly,
    pbr.payment_narah,
    pbr.payment_received,
    pbr.payment_budget_50_uah,
    debt.debt_total,
    debt.debt_3_month,
    debt.debt_12_month,
    debt.debt_3_years,
    debt.debt_over_3_years,
    debt.debt_spysano,
    debt.num_zahodiv_total,
    debt.num_zahodiv_zvit,
    debt.num_pozov_total,
    debt.num_pozov_zvit,
    debt.num_pozov_zadov_total,
    debt.num_pozov_zadov_zvit,
    debt.num_pozov_vikon_total,
    debt.num_pozov_vikon_zvit,
    debt.debt_pogasheno_total,
    debt.debt_pogasheno_zvit,
    debt.debt_v_mezhah_vitrat,
    CASE WHEN pbr.agreement_flag = 1 THEN N'ТАК' ELSE N'НІ' END AS 'rent_agreement_active',
    pbr.agreement_flag AS 'rent_agreement_active_int'
FROM
    rent_payment_by_renter pbr
    INNER JOIN rent_payment pay ON pay.id = pbr.rent_payment_id
    LEFT OUTER JOIN organizations bal_org ON bal_org.id = pay.org_balans_id
    LEFT OUTER JOIN organizations giver_org ON giver_org.id = pbr.giver_organization_id
    LEFT OUTER JOIN rent_balans_org ON rent_balans_org.organization_id = pay.org_balans_id
    LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = rent_balans_org.rent_occupation_id
    LEFT OUTER JOIN dict_rent_period ON dict_rent_period.id = pbr.rent_period_id
    LEFT OUTER JOIN rent_renter_org renter ON renter.id = pbr.rent_renter_org_id
    LEFT OUTER JOIN view_rent_debt_by_renter debt ON
        debt.rent_payment_id = pbr.rent_payment_id AND
		debt.rent_renter_org_id = pbr.rent_renter_org_id AND
		debt.agreement_flag = pbr.agreement_flag

UNION ALL

SELECT
    -pbr.id AS 'rent_id',
    -pbr.id rent_payment_id,
    pbr.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    pbr.org_renter_id rent_renter_org_id,
    pbr.org_renter_id renter_organization_id,
    renter_org.full_name AS 'renter_name',
    renter_org.zkpo_code AS 'renter_zkpo',
    bal_org.id AS 'bal_org_id',
    bal_org.full_name AS 'bal_org_full_name',
    bal_org.short_name AS 'bal_org_short_name',
    bal_org.zkpo_code AS 'bal_org_zkpo',
    bal_org.sfera_upr_id AS 'bal_org_sfera_upr_id',
    bal_org.form_ownership_id AS 'bal_org_form_ownership_id',
    bal_org.addr_distr_new_id AS 'bal_org_district_id',
    dict_rent_occupation.id AS 'bal_org_occupation_id',
    dict_rent_occupation.name AS 'bal_org_occupation',
    giver_org.id AS 'giver_org_id',
    giver_org.full_name AS 'giver_org_full_name',
    giver_org.short_name AS 'giver_org_short_name',
    giver_org.zkpo_code AS 'giver_org_zkpo',
    giver_org.sfera_upr_id AS 'giver_org_sfera_upr_id',
    giver_org.form_ownership_id AS 'giver_org_form_ownership_id',
    giver_org.addr_distr_new_id AS 'giver_org_district_id',
    pbr.sqr_total_rent,
    pbr.sqr_payed_by_percent,
    pbr.sqr_payed_by_1uah,
    pbr.sqr_payed_hourly,
    pbr.payment_narah,
    pbr.payment_received,
	NULL, --renter_org.budget_narah_50_uah, --pbr.payment_budget_50_uah,
    pbr.debt_total,
    pbr.debt_3_month,
    pbr.debt_12_month,
    pbr.debt_3_years,
    pbr.debt_over_3_years,
    pbr.debt_spysano,
    pbr.num_zahodiv_total,
    pbr.num_zahodiv_zvit,
    pbr.num_pozov_total,
    pbr.num_pozov_zvit,
    pbr.num_pozov_zadov_total,
    pbr.num_pozov_zadov_zvit,
    pbr.num_pozov_vikon_total,
    pbr.num_pozov_vikon_zvit,
    pbr.debt_pogasheno_total,
    pbr.debt_pogasheno_zvit,
    pbr.debt_v_mezhah_vitrat,
    CASE WHEN pbr.agreement_state = 1 THEN N'ТАК' ELSE N'НІ' END AS 'rent_agreement_active',
    pbr.agreement_state AS 'rent_agreement_active_int'
FROM
	(SELECT a.org_balans_id
			,a.org_giver_id
			,a.org_renter_id
			,p.rent_period_id
			,max(p.id) id
			,max(p.modified_by) modified_by
			,sum(sqr_total_rent) sqr_total_rent
			,sum(sqr_payed_by_percent) sqr_payed_by_percent
			,sum(sqr_payed_by_1uah) sqr_payed_by_1uah
			,sum(sqr_payed_hourly) sqr_payed_hourly
			,sum(payment_narah) payment_narah
			,sum(payment_received) payment_received
			,sum(payment_nar_zvit) payment_nar_zvit
			,sum(debt_total) debt_total
			,sum(debt_zvit) debt_zvit
			,sum(debt_3_month) debt_3_month
			,sum(debt_12_month) debt_12_month
			,sum(debt_3_years) debt_3_years
			,sum(debt_over_3_years) debt_over_3_years
			,sum(debt_v_mezhah_vitrat) debt_v_mezhah_vitrat
			,sum(debt_spysano) debt_spysano
			,sum(num_zahodiv_total) num_zahodiv_total
			,sum(num_zahodiv_zvit) num_zahodiv_zvit
			,sum(num_pozov_total) num_pozov_total
			,sum(num_pozov_zvit) num_pozov_zvit
			,sum(num_pozov_zadov_total) num_pozov_zadov_total
			,sum(num_pozov_zadov_zvit) num_pozov_zadov_zvit
			,sum(num_pozov_vikon_total) num_pozov_vikon_total
			,sum(num_pozov_vikon_zvit) num_pozov_vikon_zvit
			,sum(debt_pogasheno_total) debt_pogasheno_total
			,sum(debt_pogasheno_zvit) debt_pogasheno_zvit
			,sum(budget_debt_50_uah) budget_debt_50_uah
			,max(a.agreement_state) agreement_state
		FROM arenda_payments p
		INNER JOIN arenda a ON a.id = p.arenda_id
		GROUP BY a.org_balans_id, a.org_giver_id, a.org_renter_id, p.rent_period_id) pbr
    LEFT OUTER JOIN organizations bal_org ON bal_org.id = pbr.org_balans_id
    LEFT OUTER JOIN organizations giver_org ON giver_org.id = pbr.org_giver_id
    LEFT OUTER JOIN organizations renter_org ON renter_org.id = pbr.org_renter_id
	--LEFT OUTER JOIN dict_org_occupation ON dict_org_occupation.id = bal_org.occupation_id
    LEFT OUTER JOIN rent_balans_org ON rent_balans_org.organization_id = pbr.org_balans_id
    LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = rent_balans_org.rent_occupation_id
    LEFT OUTER JOIN dict_rent_period ON dict_rent_period.id = pbr.rent_period_id
GO

/* view_rent_by_buildings */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_by_buildings]'))
DROP VIEW [dbo].[view_rent_by_buildings]

GO

CREATE VIEW [view_rent_by_buildings]
AS
SELECT
    obj.id AS 'rent_object_id',
    obj.building_id,
    obj.rent_payment_id,
    obj.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    obj.addr_street,
    obj.addr_street_type,
    obj.addr_num,
    org.id AS 'bal_org_id',
    org.full_name AS 'bal_org_full_name',
    org.short_name AS 'bal_org_short_name',
    org.zkpo_code AS 'bal_org_zkpo',
    org.sfera_upr_id AS 'bal_org_sfera_upr_id',
    org.form_ownership_id AS 'bal_org_form_ownership_id',
    org.addr_distr_new_id AS 'bal_org_district_id',
    bal_org.rent_occupation_id AS 'bal_org_occupation_id',
    dict_rent_occupation.name AS 'bal_org_occupation',
    pay.phone AS 'bal_org_phone',
    pay.email AS 'bal_org_email',
    pay.responsible_fio AS 'bal_org_responsible_fio',
    obj.sqr_total,
    obj.sqr_rented,
    obj.sqr_free,
    obj.sqr_korysna,
    obj.sqr_mzk,
    obj.floors,
    dict_tech_state.name AS 'tech_state',
    CASE WHEN obj.is_energo = 0 THEN N'НІ' ELSE N'ТАК' END AS 'is_energo',
    CASE WHEN obj.is_vodo = 0 THEN N'НІ' ELSE N'ТАК' END AS 'is_vodo',
    CASE WHEN obj.is_teplo = 0 THEN N'НІ' ELSE N'ТАК' END AS 'is_teplo',
    obj.district_id,
    dict_districts2.name AS 'district',
    obj.purpose,
    obj.rent_note_id,
    dict_rent_note.name AS 'rent_note'
FROM
    rent_object obj
    LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = obj.district_id
    LEFT OUTER JOIN dict_rent_period ON dict_rent_period.id = obj.rent_period_id
    LEFT OUTER JOIN dict_rent_note ON dict_rent_note.id = obj.rent_note_id
    LEFT OUTER JOIN dict_tech_state ON dict_tech_state.id = obj.tech_state_id
    LEFT OUTER JOIN rent_payment pay ON pay.id = obj.rent_payment_id
    LEFT OUTER JOIN rent_balans_org bal_org ON bal_org.organization_id = pay.org_balans_id
    LEFT OUTER JOIN organizations org ON org.id = pay.org_balans_id
    LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = bal_org.rent_occupation_id

UNION ALL

SELECT
    obj.rent_object_id,
    obj.building_id,
    obj.rent_payment_id,
    obj.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    obj.addr_street,
    obj.addr_street_type,
    obj.addr_num,
    org.id AS 'bal_org_id',
    org.full_name AS 'bal_org_full_name',
    org.short_name AS 'bal_org_short_name',
    org.zkpo_code AS 'bal_org_zkpo',
    org.sfera_upr_id AS 'bal_org_sfera_upr_id',
    org.form_ownership_id AS 'bal_org_form_ownership_id',
    org.addr_distr_new_id AS 'bal_org_district_id',
    bal_org.rent_occupation_id AS 'bal_org_occupation_id',
    dict_rent_occupation.name AS 'bal_org_occupation',
    pay.phone AS 'bal_org_phone',
    pay.email AS 'bal_org_email',
    pay.responsible_fio AS 'bal_org_responsible_fio',
    obj.sqr_total,
    obj.sqr_rented,
    obj.sqr_free,
    obj.sqr_korysna,
    obj.sqr_mzk,
    obj.floors,
    obj.condition AS 'tech_state',
    NULL is_energo,
    NULL is_vodo,
    NULL is_teplo,
    obj.district_id,
    obj.district AS 'district',
    obj.purpose,
    obj.rent_note_id,
    dict_rent_note.name AS 'rent_note'
FROM
	(
		SELECT -ROW_NUMBER() OVER (ORDER BY arenda.building_id) rent_object_id, 
			arenda.building_id,
			arenda_payments.id rent_payment_id,
			arenda_payments.rent_period_id,
			dict_streets.name addr_street,
			dict_streets.kind addr_street_type,
			buildings.addr_nomer addr_num,
			balans.sqr_total,
			arenda.rent_square sqr_rented,
			balans.sqr_free,
			balans.sqr_kor sqr_korysna,
			balans.sqr_free_mzk sqr_mzk,
			balans.floors,
			buildings.addr_distr_new_id district_id,
			arenda.purpose_str purpose,
			NULL rent_note_id,
			balans.condition,
			arenda.org_balans_id,
			dict_districts2.name district
		FROM arenda
		INNER JOIN arenda_payments ON arenda_payments.arenda_id = arenda.id
		INNER JOIN buildings ON buildings.id = arenda.building_id
		LEFT JOIN view_balans balans ON balans.balans_id = arenda.balans_id
		LEFT JOIN dict_streets ON dict_streets.id = buildings.addr_street_id
		LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = buildings.addr_distr_new_id
		WHERE arenda.agreement_state = 1
	) obj
    LEFT OUTER JOIN dict_rent_period ON dict_rent_period.id = obj.rent_period_id
    LEFT OUTER JOIN dict_rent_note ON dict_rent_note.id = obj.rent_note_id
    LEFT OUTER JOIN rent_payment pay ON pay.id = obj.rent_payment_id
    LEFT OUTER JOIN rent_balans_org bal_org ON bal_org.organization_id = obj.org_balans_id
    LEFT OUTER JOIN organizations org ON org.id = obj.org_balans_id
    LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = bal_org.rent_occupation_id

GO

/* view_rent_by_occupation */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_by_occupation]'))
DROP VIEW [dbo].[view_rent_by_occupation]

GO

CREATE VIEW [view_rent_by_occupation]
AS
SELECT
    rent_period_id,
    rent_period,
    bal_org_occupation_id,
    bal_org_occupation,
    SUM(sqr_total_rent) AS 'sqr_total_rent',
    SUM(sqr_payed_by_percent) AS 'sqr_payed_by_percent',
    SUM(sqr_payed_by_1uah) AS 'sqr_payed_by_1uah',
    SUM(sqr_payed_hourly) AS 'sqr_payed_hourly',
    SUM(payment_narah) AS 'payment_narah',
    SUM(payment_received) AS 'payment_received',
    SUM(payment_budget_50_uah) AS 'payment_budget_50_uah',
    SUM(debt_total) AS 'debt_total',
    SUM(debt_3_month) AS 'debt_3_month',
    SUM(debt_12_month) AS 'debt_12_month',
    SUM(debt_3_years) AS 'debt_3_years',
    SUM(debt_over_3_years) AS 'debt_over_3_years',
    SUM(debt_spysano) AS 'debt_spysano',
    SUM(num_zahodiv_total) AS 'num_zahodiv_total',
    SUM(num_zahodiv_zvit) AS 'num_zahodiv_zvit',
    SUM(num_pozov_total) AS 'num_pozov_total',
    SUM(num_pozov_zvit) AS 'num_pozov_zvit',
    SUM(num_pozov_zadov_total) AS 'num_pozov_zadov_total',
    SUM(num_pozov_zadov_zvit) AS 'num_pozov_zadov_zvit',
    SUM(num_pozov_vikon_total) AS 'num_pozov_vikon_total',
    SUM(num_pozov_vikon_zvit) AS 'num_pozov_vikon_zvit',
    SUM(debt_pogasheno_total) AS 'debt_pogasheno_total',
    SUM(debt_pogasheno_zvit) AS 'debt_pogasheno_zvit',
    SUM(debt_v_mezhah_vitrat) AS 'debt_v_mezhah_vitrat'
FROM
    view_rent_by_renters
GROUP BY
    rent_period_id,
    rent_period,
    bal_org_occupation_id,
    bal_org_occupation

GO

/* view_rent_by_occupation_active - similar to 'view_rent_by_occupation', but only active agreements are included */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_by_occupation_active]'))
DROP VIEW [dbo].[view_rent_by_occupation_active]

GO

CREATE VIEW [view_rent_by_occupation_active]
AS
SELECT
    rent_period_id,
    rent_period,
    bal_org_occupation_id,
    bal_org_occupation,
    SUM(sqr_total_rent) AS 'sqr_total_rent',
    SUM(sqr_payed_by_percent) AS 'sqr_payed_by_percent',
    SUM(sqr_payed_by_1uah) AS 'sqr_payed_by_1uah',
    SUM(sqr_payed_hourly) AS 'sqr_payed_hourly',
    SUM(payment_narah) AS 'payment_narah',
    SUM(payment_received) AS 'payment_received',
    SUM(payment_budget_50_uah) AS 'payment_budget_50_uah',
    SUM(debt_total) AS 'debt_total',
    SUM(debt_3_month) AS 'debt_3_month',
    SUM(debt_12_month) AS 'debt_12_month',
    SUM(debt_3_years) AS 'debt_3_years',
    SUM(debt_over_3_years) AS 'debt_over_3_years',
    SUM(debt_spysano) AS 'debt_spysano',
    SUM(num_zahodiv_total) AS 'num_zahodiv_total',
    SUM(num_zahodiv_zvit) AS 'num_zahodiv_zvit',
    SUM(num_pozov_total) AS 'num_pozov_total',
    SUM(num_pozov_zvit) AS 'num_pozov_zvit',
    SUM(num_pozov_zadov_total) AS 'num_pozov_zadov_total',
    SUM(num_pozov_zadov_zvit) AS 'num_pozov_zadov_zvit',
    SUM(num_pozov_vikon_total) AS 'num_pozov_vikon_total',
    SUM(num_pozov_vikon_zvit) AS 'num_pozov_vikon_zvit',
    SUM(debt_pogasheno_total) AS 'debt_pogasheno_total',
    SUM(debt_pogasheno_zvit) AS 'debt_pogasheno_zvit',
    SUM(debt_v_mezhah_vitrat) AS 'debt_v_mezhah_vitrat'
FROM
    view_rent_by_renters
WHERE
    rent_agreement_active_int = 1
GROUP BY
    rent_period_id,
    rent_period,
    bal_org_occupation_id,
    bal_org_occupation

GO

/* view_rent_by_occupation_inactive - similar to 'view_rent_by_occupation', but only inactive agreements are included */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_by_occupation_inactive]'))
DROP VIEW [dbo].[view_rent_by_occupation_inactive]

GO

CREATE VIEW [view_rent_by_occupation_inactive]
AS
SELECT
    rent_period_id,
    rent_period,
    bal_org_occupation_id,
    bal_org_occupation,
    SUM(sqr_total_rent) AS 'sqr_total_rent',
    SUM(sqr_payed_by_percent) AS 'sqr_payed_by_percent',
    SUM(sqr_payed_by_1uah) AS 'sqr_payed_by_1uah',
    SUM(sqr_payed_hourly) AS 'sqr_payed_hourly',
    SUM(payment_narah) AS 'payment_narah',
    SUM(payment_received) AS 'payment_received',
    SUM(payment_budget_50_uah) AS 'payment_budget_50_uah',
    SUM(debt_total) AS 'debt_total',
    SUM(debt_3_month) AS 'debt_3_month',
    SUM(debt_12_month) AS 'debt_12_month',
    SUM(debt_3_years) AS 'debt_3_years',
    SUM(debt_over_3_years) AS 'debt_over_3_years',
    SUM(debt_spysano) AS 'debt_spysano',
    SUM(num_zahodiv_total) AS 'num_zahodiv_total',
    SUM(num_zahodiv_zvit) AS 'num_zahodiv_zvit',
    SUM(num_pozov_total) AS 'num_pozov_total',
    SUM(num_pozov_zvit) AS 'num_pozov_zvit',
    SUM(num_pozov_zadov_total) AS 'num_pozov_zadov_total',
    SUM(num_pozov_zadov_zvit) AS 'num_pozov_zadov_zvit',
    SUM(num_pozov_vikon_total) AS 'num_pozov_vikon_total',
    SUM(num_pozov_vikon_zvit) AS 'num_pozov_vikon_zvit',
    SUM(debt_pogasheno_total) AS 'debt_pogasheno_total',
    SUM(debt_pogasheno_zvit) AS 'debt_pogasheno_zvit',
    SUM(debt_v_mezhah_vitrat) AS 'debt_v_mezhah_vitrat'
FROM
    view_rent_by_renters
WHERE
    rent_agreement_active_int <> 1
GROUP BY
    rent_period_id,
    rent_period,
    bal_org_occupation_id,
    bal_org_occupation

GO

/* view_rent_total_renters */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_total_renters]'))
DROP VIEW [dbo].[view_rent_total_renters]

GO

CREATE VIEW [view_rent_total_renters]
AS
SELECT
    pay.rent_period_id,
    bal_org.rent_occupation_id,
    SUM(pay.num_renters) AS 'total_num_renters'
FROM
    rent_payment pay
    LEFT OUTER JOIN rent_balans_org bal_org ON bal_org.organization_id = pay.org_balans_id
GROUP BY
    pay.rent_period_id,
    bal_org.rent_occupation_id

GO

/* fnRentByOccupation - stored procedure that mimics the 'view_rent_by_occupation' view, plus additional logic */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnRentByOccupation]'))
	DROP PROCEDURE [dbo].[fnRentByOccupation]
GO

CREATE PROCEDURE dbo.fnRentByOccupation
(	
	@RDA_DISTRICT_ID INTEGER,
	@RENT_AGREEMENT_ACTIVE INTEGER /* 0 - don't care, 1 - active, 2 - inactive */
)
AS
	Declare @temp_table table
	(
		rent_period_id INTEGER,
		rent_period VARCHAR(64),
		bal_org_occupation_id INTEGER,
		bal_org_occupation  VARCHAR(128),
		sqr_total_rent NUMERIC(15,3),
		sqr_payed_by_percent NUMERIC(15,3),
		sqr_payed_by_1uah NUMERIC(15,3),
		sqr_payed_hourly NUMERIC(15,3),
		payment_narah NUMERIC(15,3),
		payment_received NUMERIC(15,3),
		payment_budget_50_uah NUMERIC(15,3),
		debt_total NUMERIC(15,3),
		debt_3_month NUMERIC(15,3),
		debt_12_month NUMERIC(15,3),
		debt_3_years NUMERIC(15,3),
		debt_over_3_years NUMERIC(15,3),
		debt_spysano NUMERIC(15,3),
		num_zahodiv_total NUMERIC(15,3),
		num_zahodiv_zvit NUMERIC(15,3),
		num_pozov_total NUMERIC(15,3),
		num_pozov_zvit NUMERIC(15,3),
		num_pozov_zadov_total NUMERIC(15,3),
		num_pozov_zadov_zvit NUMERIC(15,3),
		num_pozov_vikon_total NUMERIC(15,3),
		num_pozov_vikon_zvit NUMERIC(15,3),
		debt_pogasheno_total NUMERIC(15,3),
		debt_pogasheno_zvit NUMERIC(15,3),
		debt_v_mezhah_vitrat NUMERIC(15,3)
	)

	INSERT INTO @temp_table
	SELECT
		rent_period_id,
		rent_period,
		bal_org_occupation_id,
		bal_org_occupation,
		SUM(sqr_total_rent) AS 'sqr_total_rent',
		SUM(sqr_payed_by_percent) AS 'sqr_payed_by_percent',
		SUM(sqr_payed_by_1uah) AS 'sqr_payed_by_1uah',
		SUM(sqr_payed_hourly) AS 'sqr_payed_hourly',
		SUM(payment_narah) AS 'payment_narah',
		SUM(payment_received) AS 'payment_received',
		SUM(payment_budget_50_uah) AS 'payment_budget_50_uah',
		SUM(debt_total) AS 'debt_total',
		SUM(debt_3_month) AS 'debt_3_month',
		SUM(debt_12_month) AS 'debt_12_month',
		SUM(debt_3_years) AS 'debt_3_years',
		SUM(debt_over_3_years) AS 'debt_over_3_years',
		SUM(debt_spysano) AS 'debt_spysano',
		SUM(num_zahodiv_total) AS 'num_zahodiv_total',
		SUM(num_zahodiv_zvit) AS 'num_zahodiv_zvit',
		SUM(num_pozov_total) AS 'num_pozov_total',
		SUM(num_pozov_zvit) AS 'num_pozov_zvit',
		SUM(num_pozov_zadov_total) AS 'num_pozov_zadov_total',
		SUM(num_pozov_zadov_zvit) AS 'num_pozov_zadov_zvit',
		SUM(num_pozov_vikon_total) AS 'num_pozov_vikon_total',
		SUM(num_pozov_vikon_zvit) AS 'num_pozov_vikon_zvit',
		SUM(debt_pogasheno_total) AS 'debt_pogasheno_total',
		SUM(debt_pogasheno_zvit) AS 'debt_pogasheno_zvit',
		SUM(debt_v_mezhah_vitrat) AS 'debt_v_mezhah_vitrat'
	FROM
		view_rent_by_renters
	WHERE
		( @RDA_DISTRICT_ID = 0 OR
          (bal_org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND bal_org_district_id = @RDA_DISTRICT_ID) OR
          (giver_org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND giver_org_district_id = @RDA_DISTRICT_ID) )
        AND
        ( @RENT_AGREEMENT_ACTIVE = 0 OR
          (@RENT_AGREEMENT_ACTIVE = 1 AND rent_agreement_active_int = 1) OR
          (@RENT_AGREEMENT_ACTIVE = 2 AND rent_agreement_active_int <> 1) )
	GROUP BY
		rent_period_id,
		rent_period,
		bal_org_occupation_id,
		bal_org_occupation
		
    /**/
    
    Declare @temp_table2 table
	(
		rent_period_id INTEGER,
		rent_occupation_id INTEGER,
		total_num_renters INTEGER
	)
	
	INSERT INTO @temp_table2
    SELECT
		pay.rent_period_id,
		bal_org.rent_occupation_id,
		SUM(pay.num_renters) AS 'total_num_renters'
	FROM
		rent_payment pay
		LEFT OUTER JOIN rent_balans_org bal_org ON bal_org.organization_id = pay.org_balans_id
		LEFT OUTER JOIN organizations org ON org.id = pay.org_balans_id
	WHERE
		@RDA_DISTRICT_ID = 0 OR (org.form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND org.addr_distr_new_id = @RDA_DISTRICT_ID)
	GROUP BY
		pay.rent_period_id,
		bal_org.rent_occupation_id
    
    /**/
    
	SELECT
		rbo.*,
		tr.total_num_renters
	FROM
		@temp_table rbo
        LEFT OUTER JOIN @temp_table2 tr ON tr.rent_period_id = rbo.rent_period_id AND tr.rent_occupation_id = rbo.bal_org_occupation_id
GO

GO

/* view_rent_payments */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_payments]'))
DROP VIEW [dbo].[view_rent_payments]

GO

CREATE VIEW [view_rent_payments]
AS
SELECT
    id,
    org_balans_id,
    rent_period_id,
    sqr_balans,
    payment_zvit_uah,
    received_zvit_uah,
    received_nar_zvit_uah,
    debt_total_uah,
    debt_zvit_uah,
    budget_narah_50_uah,
    budget_zvit_50_uah,
    budget_prev_50_uah,
    budget_debt_50_uah,
    budget_debt_30_50_uah,
    sqr_cmk,
    budget_narah_50_cmk_uah,
    budget_zvit_50_cmk_uah,
    budget_debt_50_cmk_uah,
    budget_zvit_50_in_uah,
    zkpo_code,
    modify_date,
    modified_by,
    email,
    phone,
    responsible_fio,
    director_fio,
    buhgalter_fio,
    num_agreements,
    num_renters,
    debt_v_mezhah_vitrat,
    budget_zvit_50_old,
    post_address,
    pererahunok_50_b,
    rent_sqr.sqr_rent_total,
    CASE WHEN rent_sqr.sqr_rent_total > 0 THEN payment_zvit_uah /rent_sqr.sqr_rent_total ELSE 0 END AS 'avg_rent_payment'
FROM
    rent_payment pay
    OUTER APPLY (SELECT SUM(sqr_total_rent) AS 'sqr_rent_total' FROM rent_payment_by_renter pbr WHERE pbr.rent_payment_id = pay.id) rent_sqr

UNION ALL

SELECT
    -p.id id,
    p.org_balans_id,
    p.rent_period_id,
    b.sqr_total sqr_balans,
    p.payment_narah, --payment_zvit_uah,
    p.payment_received, --received_zvit_uah,
    p.payment_nar_zvit, --received_nar_zvit_uah,
    p.debt_total, --debt_total_uah,
    p.debt_zvit, --debt_zvit_uah,
    org_arc.budget_narah_50_uah,
    org_arc.budget_zvit_50_uah,
    org_arc.budget_prev_50_uah,
    p.budget_debt_50_uah,
    org_arc.budget_debt_30_50_uah,
    cmk.sqr_cmk,
    cmk.budget_narah_50_cmk_uah,
    cmk.budget_zvit_50_cmk_uah,
    cmk.budget_debt_50_cmk_uah,
    NULL, --budget_zvit_50_in_uah,
    org.zkpo_code,
    p.modify_date,
    p.modified_by,
    org.director_email, --email,
    org.director_phone, --phone,
    org.director_fio, --responsible_fio,
    org.director_fio, --director_fio,
    org.buhgalter_fio, --buhgalter_fio,
    p.num_agreements,
    p.num_renters,
    p.debt_v_mezhah_vitrat,
    0, --budget_zvit_50_old,
	isnull(org.addr_street_name + ', ', '')
	+ isnull(org.addr_nomer + ' ', '')
	+ isnull(org.addr_nomer2 + ' ', '')
	+ isnull(org.addr_korpus, '')
		post_address,
    org_arc.budget_narah_50_uah, --p.pererahunok_50_b
    p.sqr_total_rent,
    CASE WHEN p.sqr_total_rent > 0 THEN p.payment_narah / p.sqr_total_rent ELSE 0 END AS 'avg_rent_payment'
FROM
	(SELECT a.org_balans_id
			,p.rent_period_id
			,max(p.id) id
			,max(p.modified_by) modified_by
			,max(p.modify_date) modify_date
			,sum(sqr_total_rent) sqr_total_rent
			,sum(payment_narah) payment_narah
			,sum(payment_received) payment_received
			,sum(payment_nar_zvit) payment_nar_zvit
			,sum(debt_total) debt_total
			,sum(debt_zvit) debt_zvit
			,sum(debt_3_month) debt_3_month
			,sum(debt_12_month) debt_12_month
			,sum(debt_3_years) debt_3_years
			,sum(debt_over_3_years) debt_over_3_years
			,sum(debt_v_mezhah_vitrat) debt_v_mezhah_vitrat
			,sum(debt_spysano) debt_spysano
			,sum(num_zahodiv_total) num_zahodiv_total
			,sum(num_zahodiv_zvit) num_zahodiv_zvit
			,sum(num_pozov_total) num_pozov_total
			,sum(num_pozov_zvit) num_pozov_zvit
			,sum(num_pozov_zadov_total) num_pozov_zadov_total
			,sum(num_pozov_zadov_zvit) num_pozov_zadov_zvit
			,sum(num_pozov_vikon_total) num_pozov_vikon_total
			,sum(num_pozov_vikon_zvit) num_pozov_vikon_zvit
			,sum(debt_pogasheno_total) debt_pogasheno_total
			,sum(debt_pogasheno_zvit) debt_pogasheno_zvit
			,sum(budget_debt_50_uah) budget_debt_50_uah
			,count(distinct a.id) num_agreements
			,count(distinct a.org_renter_id) num_renters
		FROM [arenda_payments] p
		INNER JOIN arenda a ON a.id = p.arenda_id
		GROUP BY a.org_balans_id, p.rent_period_id) p
	LEFT JOIN 
		(
		SELECT balans.organization_id, 
			SUM(balans.sqr_total) sqr_total, 
			SUM(balans.sqr_free) sqr_free,
			SUM(balans.sqr_kor) sqr_kor,
			SUM(balans.sqr_free_mzk) sqr_free_mzk
		FROM view_balans balans
		GROUP BY balans.organization_id
		) b ON b.organization_id = p.org_balans_id
    INNER JOIN dict_rent_period ON dict_rent_period.id = p.rent_period_id
    INNER JOIN organizations org ON org.id = p.org_balans_id
	CROSS APPLY
		(
			SELECT TOP 1
			   x.[id]
			  ,x.[master_org_id]
			  ,x.[last_state]
			  ,x.[occupation_id]
			  ,x.[status_id]
			  ,x.[form_gosp_id]
			  ,x.[form_ownership_id]
			  ,x.[gosp_struct_id]
			  ,x.[organ_id]
			  ,x.[industry_id]
			  ,x.[nomer_obj]
			  ,x.[zkpo_code]
			  ,x.[addr_distr_old_id]
			  ,x.[addr_distr_new_id]
			  ,x.[addr_street_name]
			  ,x.[addr_street_id]
			  ,x.[addr_nomer]
			  ,x.[addr_nomer2]
			  ,x.[addr_korpus]
			  ,x.[addr_zip_code]
			  ,x.[addr_misc]
			  ,x.[director_fio]
			  ,x.[director_phone]
			  ,x.[director_fio_kogo]
			  ,x.[director_title]
			  ,x.[director_title_kogo]
			  ,x.[director_doc]
			  ,x.[director_doc_kogo]
			  ,x.[director_email]
			  ,x.[buhgalter_fio]
			  ,x.[buhgalter_phone]
			  ,x.[buhgalter_email]
			  ,x.[num_buildings]
			  ,x.[full_name]
			  ,x.[short_name]
			  ,x.[priznak_id]
			  ,x.[title_form_id]
			  ,x.[form_1nf_id]
			  ,x.[vedomstvo_id]
			  ,x.[title_id]
			  ,x.[form_id]
			  ,x.[gosp_struct_type_id]
			  ,x.[search_name]
			  ,x.[name_komu]
			  ,x.[fax]
			  ,x.[registration_auth]
			  ,x.[registration_num]
			  ,x.[registration_date]
			  ,x.[registration_svidot]
			  ,x.[l_year]
			  ,x.[date_l_year]
			  ,x.[sqr_on_balance]
			  ,x.[sqr_manufact]
			  ,x.[sqr_non_manufact]
			  ,x.[sqr_free_for_rent]
			  ,x.[sqr_total]
			  ,x.[sqr_rented]
			  ,x.[sqr_privat]
			  ,x.[sqr_given_for_rent]
			  ,x.[sqr_znyata_z_balansu]
			  ,x.[sqr_prodaj]
			  ,x.[sqr_spisani_zneseni]
			  ,x.[sqr_peredana]
			  ,x.[num_objects]
			  ,x.[kved_code]
			  ,x.[date_stat_spravka]
			  ,x.[koatuu]
			  ,x.[modified_by]
			  ,x.[modify_date]
			  ,x.[share_type_id]
			  ,x.[share]
			  ,x.[bank_name]
			  ,x.[bank_mfo]
			  ,x.[is_deleted]
			  ,x.[del_date]
			  ,x.[otdel_gukv_id]
			  ,x.[arch_id]
			  ,x.[arch_flag]
			  ,x.[is_liquidated]
			  ,x.[liquidation_date]
			  ,x.[pidp_rda]
			  ,x.[is_arend]
			  ,x.[beg_state_date]
			  ,x.[end_state_date]
			  ,x.[mayno_id]
			  ,x.[contact_email]
			  ,x.[contact_posada_id]
			  ,x.[nadhodjennya_id]
			  ,x.[vibuttya_id]
			  ,x.[privat_status_id]
			  ,x.[cur_state_id]
			  ,x.[sfera_upr_id]
			  ,x.[plan_zone_id]
			  ,x.[registr_org_id]
			  ,x.[nadhodjennya_date]
			  ,x.[vibuttya_date]
			  ,x.[chastka]
			  ,x.[registration_rish]
			  ,x.[registration_dov_date]
			  ,x.[registration_corp]
			  ,x.[strok_start_date]
			  ,x.[strok_end_date]
			  ,x.[stat_fond]
			  ,x.[size_plus]
			  ,x.[addr_zip_code_3]
			  ,x.[old_industry_id]
			  ,x.[old_occupation_id]
			  ,x.[old_organ_id]
			  ,x.[form_vlasn_vibuttya_id]
			  ,x.[addr_city]
			  ,x.[addr_flat_num]
			  ,x.[povnovajennia]
			  ,x.[povnov_osoba_fio]
			  ,x.[povnov_passp_seria]
			  ,x.[povnov_passp_num]
			  ,x.[povnov_passp_auth]
			  ,x.[povnov_passp_date]
			  ,x.[director_passp_seria]
			  ,x.[director_passp_num]
			  ,x.[director_passp_auth]
			  ,x.[director_passp_date]
			  ,x.[registration_svid_date]
			  ,x.[origin_db]
			  ,x.[budg_payments_rate]
			  ,x.[is_under_closing]
			  ,x.[phys_addr_street_id]
			  ,x.[phys_addr_district_id]
			  ,x.[phys_addr_nomer]
			  ,x.[phys_addr_zip_code]
			  ,x.[phys_addr_misc]
			  ,x.[contribution_rate]
			  ,x.[budget_narah_50_uah]
			  ,x.[budget_zvit_50_uah]
			  ,x.[budget_prev_50_uah]
			  ,x.[budget_debt_30_50_uah]
			  ,x.[is_special_organization]
			  ,x.[payment_budget_special]
			  ,x.[konkurs_payments]
			  ,x.[unknown_payments]
			  ,x.[unknown_payment_note]
			FROM (
				SELECT CAST(GETDATE() AS date) effective_date
					  ,[id]
					  ,[master_org_id]
					  ,[last_state]
					  ,[occupation_id]
					  ,[status_id]
					  ,[form_gosp_id]
					  ,[form_ownership_id]
					  ,[gosp_struct_id]
					  ,[organ_id]
					  ,[industry_id]
					  ,[nomer_obj]
					  ,[zkpo_code]
					  ,[addr_distr_old_id]
					  ,[addr_distr_new_id]
					  ,[addr_street_name]
					  ,[addr_street_id]
					  ,[addr_nomer]
					  ,[addr_nomer2]
					  ,[addr_korpus]
					  ,[addr_zip_code]
					  ,[addr_misc]
					  ,[director_fio]
					  ,[director_phone]
					  ,[director_fio_kogo]
					  ,[director_title]
					  ,[director_title_kogo]
					  ,[director_doc]
					  ,[director_doc_kogo]
					  ,[director_email]
					  ,[buhgalter_fio]
					  ,[buhgalter_phone]
					  ,[buhgalter_email]
					  ,[num_buildings]
					  ,[full_name]
					  ,[short_name]
					  ,[priznak_id]
					  ,[title_form_id]
					  ,[form_1nf_id]
					  ,[vedomstvo_id]
					  ,[title_id]
					  ,[form_id]
					  ,[gosp_struct_type_id]
					  ,[search_name]
					  ,[name_komu]
					  ,[fax]
					  ,[registration_auth]
					  ,[registration_num]
					  ,[registration_date]
					  ,[registration_svidot]
					  ,[l_year]
					  ,[date_l_year]
					  ,[sqr_on_balance]
					  ,[sqr_manufact]
					  ,[sqr_non_manufact]
					  ,[sqr_free_for_rent]
					  ,[sqr_total]
					  ,[sqr_rented]
					  ,[sqr_privat]
					  ,[sqr_given_for_rent]
					  ,[sqr_znyata_z_balansu]
					  ,[sqr_prodaj]
					  ,[sqr_spisani_zneseni]
					  ,[sqr_peredana]
					  ,[num_objects]
					  ,[kved_code]
					  ,[date_stat_spravka]
					  ,[koatuu]
					  ,[modified_by]
					  ,[modify_date]
					  ,[share_type_id]
					  ,[share]
					  ,[bank_name]
					  ,[bank_mfo]
					  ,[is_deleted]
					  ,[del_date]
					  ,[otdel_gukv_id]
					  ,[arch_id]
					  ,[arch_flag]
					  ,[is_liquidated]
					  ,[liquidation_date]
					  ,[pidp_rda]
					  ,[is_arend]
					  ,[beg_state_date]
					  ,[end_state_date]
					  ,[mayno_id]
					  ,[contact_email]
					  ,[contact_posada_id]
					  ,[nadhodjennya_id]
					  ,[vibuttya_id]
					  ,[privat_status_id]
					  ,[cur_state_id]
					  ,[sfera_upr_id]
					  ,[plan_zone_id]
					  ,[registr_org_id]
					  ,[nadhodjennya_date]
					  ,[vibuttya_date]
					  ,[chastka]
					  ,[registration_rish]
					  ,[registration_dov_date]
					  ,[registration_corp]
					  ,[strok_start_date]
					  ,[strok_end_date]
					  ,[stat_fond]
					  ,[size_plus]
					  ,[addr_zip_code_3]
					  ,[old_industry_id]
					  ,[old_occupation_id]
					  ,[old_organ_id]
					  ,[form_vlasn_vibuttya_id]
					  ,[addr_city]
					  ,[addr_flat_num]
					  ,[povnovajennia]
					  ,[povnov_osoba_fio]
					  ,[povnov_passp_seria]
					  ,[povnov_passp_num]
					  ,[povnov_passp_auth]
					  ,[povnov_passp_date]
					  ,[director_passp_seria]
					  ,[director_passp_num]
					  ,[director_passp_auth]
					  ,[director_passp_date]
					  ,[registration_svid_date]
					  ,[origin_db]
					  ,[budg_payments_rate]
					  ,[is_under_closing]
					  ,[phys_addr_street_id]
					  ,[phys_addr_district_id]
					  ,[phys_addr_nomer]
					  ,[phys_addr_zip_code]
					  ,[phys_addr_misc]
					  ,[contribution_rate]
					  ,[budget_narah_50_uah]
					  ,[budget_zvit_50_uah]
					  ,[budget_prev_50_uah]
					  ,[budget_debt_30_50_uah]
					  ,[is_special_organization]
					  ,[payment_budget_special]
					  ,[konkurs_payments]
					  ,[unknown_payments]
					  ,[unknown_payment_note]
				FROM [organizations], (select max(id) rent_period_id from dict_rent_period) dict_rent_period
				UNION ALL
				SELECT archive_create_date effective_date
					  ,[id]
					  ,[master_org_id]
					  ,[last_state]
					  ,[occupation_id]
					  ,[status_id]
					  ,[form_gosp_id]
					  ,[form_ownership_id]
					  ,[gosp_struct_id]
					  ,[organ_id]
					  ,[industry_id]
					  ,[nomer_obj]
					  ,[zkpo_code]
					  ,[addr_distr_old_id]
					  ,[addr_distr_new_id]
					  ,[addr_street_name]
					  ,[addr_street_id]
					  ,[addr_nomer]
					  ,[addr_nomer2]
					  ,[addr_korpus]
					  ,[addr_zip_code]
					  ,[addr_misc]
					  ,[director_fio]
					  ,[director_phone]
					  ,[director_fio_kogo]
					  ,[director_title]
					  ,[director_title_kogo]
					  ,[director_doc]
					  ,[director_doc_kogo]
					  ,[director_email]
					  ,[buhgalter_fio]
					  ,[buhgalter_phone]
					  ,[buhgalter_email]
					  ,[num_buildings]
					  ,[full_name]
					  ,[short_name]
					  ,[priznak_id]
					  ,[title_form_id]
					  ,[form_1nf_id]
					  ,[vedomstvo_id]
					  ,[title_id]
					  ,[form_id]
					  ,[gosp_struct_type_id]
					  ,[search_name]
					  ,[name_komu]
					  ,[fax]
					  ,[registration_auth]
					  ,[registration_num]
					  ,[registration_date]
					  ,[registration_svidot]
					  ,[l_year]
					  ,[date_l_year]
					  ,[sqr_on_balance]
					  ,[sqr_manufact]
					  ,[sqr_non_manufact]
					  ,[sqr_free_for_rent]
					  ,[sqr_total]
					  ,[sqr_rented]
					  ,[sqr_privat]
					  ,[sqr_given_for_rent]
					  ,[sqr_znyata_z_balansu]
					  ,[sqr_prodaj]
					  ,[sqr_spisani_zneseni]
					  ,[sqr_peredana]
					  ,[num_objects]
					  ,[kved_code]
					  ,[date_stat_spravka]
					  ,[koatuu]
					  ,[modified_by]
					  ,[modify_date]
					  ,[share_type_id]
					  ,[share]
					  ,[bank_name]
					  ,[bank_mfo]
					  ,[is_deleted]
					  ,[del_date]
					  ,[otdel_gukv_id]
					  ,[arch_id]
					  ,[arch_flag]
					  ,[is_liquidated]
					  ,[liquidation_date]
					  ,[pidp_rda]
					  ,[is_arend]
					  ,[beg_state_date]
					  ,[end_state_date]
					  ,[mayno_id]
					  ,[contact_email]
					  ,[contact_posada_id]
					  ,[nadhodjennya_id]
					  ,[vibuttya_id]
					  ,[privat_status_id]
					  ,[cur_state_id]
					  ,[sfera_upr_id]
					  ,[plan_zone_id]
					  ,[registr_org_id]
					  ,[nadhodjennya_date]
					  ,[vibuttya_date]
					  ,[chastka]
					  ,[registration_rish]
					  ,[registration_dov_date]
					  ,[registration_corp]
					  ,[strok_start_date]
					  ,[strok_end_date]
					  ,[stat_fond]
					  ,[size_plus]
					  ,[addr_zip_code_3]
					  ,[old_industry_id]
					  ,[old_occupation_id]
					  ,[old_organ_id]
					  ,[form_vlasn_vibuttya_id]
					  ,[addr_city]
					  ,[addr_flat_num]
					  ,[povnovajennia]
					  ,[povnov_osoba_fio]
					  ,[povnov_passp_seria]
					  ,[povnov_passp_num]
					  ,[povnov_passp_auth]
					  ,[povnov_passp_date]
					  ,[director_passp_seria]
					  ,[director_passp_num]
					  ,[director_passp_auth]
					  ,[director_passp_date]
					  ,[registration_svid_date]
					  ,[origin_db]
					  ,[budg_payments_rate]
					  ,[is_under_closing]
					  ,[phys_addr_street_id]
					  ,[phys_addr_district_id]
					  ,[phys_addr_nomer]
					  ,[phys_addr_zip_code]
					  ,[phys_addr_misc]
					  ,[contribution_rate]
					  ,[budget_narah_50_uah]
					  ,[budget_zvit_50_uah]
					  ,[budget_prev_50_uah]
					  ,[budget_debt_30_50_uah]
					  ,[is_special_organization]
					  ,[payment_budget_special]
					  ,[konkurs_payments]
					  ,[unknown_payments]
					  ,[unknown_payment_note]
				FROM arch_organizations
			) x WHERE x.id = p.org_balans_id
				AND DATEADD(d, 20, dict_rent_period.period_end) >= x.effective_date
			ORDER BY x.effective_date DESC
		) as org_arc
	LEFT OUTER JOIN
		(SELECT
			arenda_rented.org_renter_id,
			arenda_payments_cmk.rent_period_id,
			SUM(arenda_payments_cmk.cmk_sqr_rented) sqr_cmk,
			SUM(arenda_payments_cmk.cmk_payment_to_budget) budget_zvit_50_cmk_uah,
			SUM(arenda_payments_cmk.cmk_payment_narah) budget_narah_50_cmk_uah,
			SUM(arenda_payments_cmk.cmk_rent_debt) budget_debt_50_cmk_uah
		FROM
			arenda_rented
			INNER JOIN arenda_payments_cmk on arenda_payments_cmk.arenda_rented_id = arenda_rented.id
		WHERE
			ISNULL(arenda_rented.is_deleted, 0) = 0
		GROUP BY
			arenda_rented.org_renter_id,
			arenda_payments_cmk.rent_period_id) cmk ON cmk.org_renter_id = p.org_balans_id AND cmk.rent_period_id = p.rent_period_id
WHERE NOT EXISTS (SELECT 1 FROM rent_payment WHERE rent_payment.org_balans_id = p.org_balans_id AND rent_payment.rent_period_id = p.rent_period_id)

GO

/* view_rent_pay_for_arenda */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_pay_for_arenda]'))
DROP VIEW [dbo].[view_rent_pay_for_arenda]

GO

CREATE VIEW view_rent_pay_for_arenda
AS
SELECT
    pbr.rent_period_id,
    renter.organization_id,
    SUM(pbr.payment_narah) AS 'payment_narah',
    SUM(pbr.payment_received) AS 'payment_received'
FROM
    rent_payment_by_renter pbr
    LEFT OUTER JOIN rent_renter_org renter ON renter.id = pbr.rent_renter_org_id
GROUP BY
    pbr.rent_period_id,
    renter.organization_id

GO

/* view_rent_debt_for_arenda */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_rent_debt_for_arenda]'))
DROP VIEW [dbo].[view_rent_debt_for_arenda]

GO

CREATE VIEW view_rent_debt_for_arenda
AS
SELECT
    d.rent_period_id,
    renter.organization_id,
    SUM(d.debt_total) AS 'debt_total'
FROM
    rent_payment_debt d
    LEFT OUTER JOIN rent_renter_org renter ON renter.id = d.rent_renter_org_id
GROUP BY
    d.rent_period_id,
    renter.organization_id

GO

/******************************************************************************/
/*                                Assessment                                  */
/******************************************************************************/

/* view_assessment */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_assessment]'))
DROP VIEW [dbo].[view_assessment]

GO

CREATE VIEW view_assessment
AS
SELECT
    en.id AS 'valuation_id',
    en.org_renter_id,
    COALESCE(en.org_renter_name, renter.full_name) AS 'renter_name',
    en.org_balans_id,
    COALESCE(en.org_balans_name, bal_org.full_name) AS 'balans_org_name',
    en.building_id,
    dict_districts2.name AS 'district',
    en.addr_street_name AS 'street_full_name',
    CASE
        WHEN LEN(RTRIM(LTRIM(en.addr_number1))) = 0 THEN RTRIM(LTRIM(en.addr_number2))
        WHEN LEN(RTRIM(LTRIM(en.addr_number2))) = 0 THEN RTRIM(LTRIM(en.addr_number1))
        ELSE RTRIM(LTRIM(en.addr_number1)) + ' ' + RTRIM(LTRIM(en.addr_number2))
    END AS 'addr_nomer',
    in_doc.doc_dates AS 'in_doc_dates',
    in_doc.doc_numbers AS 'in_doc_numbers',
    in_doc.control_dates AS 'in_doc_ctl_dates',
    in_doc.korrespondents AS 'in_doc_korr',
    in_doc.rezenz_names AS 'in_doc_rezenz_names',
    out_doc.doc_dates AS 'out_doc_dates',
    out_doc.doc_numbers AS 'out_doc_numbers',
    out_doc.rezenz_kinds AS 'out_rezenz_kinds',
    dict_expert_obj_type.name AS 'expert_obj_type',
    en.obj_square,
    COALESCE(en.expert_name, dict_expert.full_name) AS 'expert_name',
    dict_expert_rezenz.name AS 'rezenz_name',
    en.cost_1_usd,
    en.cost_prim,
    en.valuation_date,
    en.final_date,
    detail.floors AS 'floors',
    en.arch_num,
    en.arch_date,
    en.is_archived,
    CASE WHEN en.is_archived = 1 THEN N'ТАК' ELSE N'НІ' END AS 'arch_state',
    CASE WHEN en.is_stand_oc = 1 THEN N'СТАНДАРТИЗОВАНА' ELSE N'НЕЗАЛЕЖНА' END AS 'valuation_kind',
    en.note_text
FROM
    expert_note en
    LEFT OUTER JOIN organizations renter ON renter.id = en.org_renter_id
    LEFT OUTER JOIN organizations bal_org ON bal_org.id = en.org_balans_id
    LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = en.addr_district_id
    LEFT OUTER JOIN dict_expert ON dict_expert.id = en.expert_id
    LEFT OUTER JOIN dict_expert_rezenz ON dict_expert_rezenz.id = en.rezenz_id
    LEFT OUTER JOIN dict_expert_obj_type ON dict_expert_obj_type.id = en.expert_obj_type_id
    LEFT OUTER JOIN expert_input_doc_grouped in_doc ON in_doc.expert_note_id = en.id
    LEFT OUTER JOIN expert_output_doc_grouped out_doc ON out_doc.expert_note_id = en.id
    LEFT OUTER JOIN expert_note_detail_grouped detail ON detail.expert_note_id = en.id
WHERE
    en.is_deleted = 0 AND
    (NOT (en.org_balans_id IS NULL) OR NOT (en.building_id IS NULL) OR NOT (en.expert_id IS NULL))

GO

/* view_experts */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_experts]'))
DROP VIEW [dbo].[view_experts]

GO

CREATE VIEW view_experts
AS
SELECT
    ex.id AS 'expert_id',
    ex.full_name,
    ex.short_name,
    ex.fio_boss,
    ex.tel_boss,
    dict_districts2.name AS 'district',
    dict_streets.name AS 'street_full_name',
    addr_zip_code,
    addr_number AS 'addr_nomer',
    certificate_num,
    certificate_date,
    certificate_end_date,
    case_num,
    addr_full,
    note
FROM
    dict_expert ex
    LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = ex.addr_district_id
    LEFT OUTER JOIN dict_streets ON dict_streets.id = ex.addr_street_id
WHERE
    ex.is_deleted <> 1

GO

/******************************************************************************/
/*                                 Reports                                    */
/******************************************************************************/

/* report_documents - the primary report about documents and their related documents */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[report_documents]'))
DROP VIEW [dbo].[report_documents]

GO

CREATE VIEW [report_documents]
AS
SELECT
       [dep].id AS 'link_id'
      ,[parent_doc].[id] AS 'parent_id'
      ,[parent_doc].[kind] AS 'parent_kind'
      ,[parent_doc].[general_kind] AS 'parent_general_kind'
      ,CASE WHEN ((parent_doc.kind LIKE 'РОЗПОРЯДЖЕННЯ%') OR (parent_doc.kind LIKE 'РІШЕННЯ%')) THEN N'ТАК' ELSE N'НІ' END AS 'parent_is_rozp'
      ,[parent_doc].[doc_date] AS 'parent_date'
      ,YEAR(parent_doc.doc_date) AS 'parent_date_year'
      ,DATEPART(Quarter, parent_doc.doc_date)  AS 'parent_date_quarter'
      ,[parent_doc].[doc_num] AS 'parent_num'
      ,[parent_doc].[topic] AS 'parent_topic'
      ,[parent_doc].[note] AS 'parent_note'
      ,[parent_doc].[search_name] AS 'parent_search_name'
      ,[parent_doc].[receive_date] AS 'parent_receive_date'
      ,[parent_doc].[commission] AS 'parent_commission'
      ,[parent_doc].[source] AS 'parent_source'
      ,[parent_doc].[state] AS 'parent_state'
      ,[parent_doc].[summa] AS 'parent_summa'
      ,[parent_doc].[summa_zalishkova] AS 'parent_summa_zalishkova'
      ,[parent_doc].[is_text_exists] AS 'parent_text_exists'
      ,[parent_doc].[extern_doc_id] AS 'parent_extern_doc_id'
      ,[child_doc].[id] AS 'child_id'
      ,[child_doc].[kind] AS 'child_kind'
      ,[child_doc].[general_kind] AS 'child_general_kind'
      ,[child_doc].[doc_date] AS 'child_date'
      ,YEAR(child_doc.doc_date) AS 'child_date_year'
      ,DATEPART(Quarter, child_doc.doc_date)  AS 'child_date_quarter'
      ,[child_doc].[doc_num] AS 'child_num'
      ,[child_doc].[topic] AS 'child_topic'
      ,[child_doc].[note] AS 'child_note'
      ,[child_doc].[search_name] AS 'child_search_name'
      ,[child_doc].[receive_date] AS 'child_receive_date'
      ,[child_doc].[commission] AS 'child_commission'
      ,[child_doc].[source] AS 'child_source'
      ,[child_doc].[state] AS 'child_state'
      ,[child_doc].[summa] AS 'child_summa'
      ,[child_doc].[summa_zalishkova] AS 'child_summa_zalishkova'
      ,[child_doc].[is_text_exists] AS 'child_text_exists'
      ,[child_doc].[extern_doc_id] AS 'child_extern_doc_id'
FROM
      view_documents parent_doc
      LEFT OUTER JOIN doc_dependencies dep ON dep.master_doc_id = parent_doc.id
      LEFT OUTER JOIN view_documents child_doc ON child_doc.id = dep.slave_doc_id
/* WHERE
      NOT (parent_doc.id IN (SELECT slave_doc_id FROM doc_dependencies)) */

GO

/* report_financial - financial analysis */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[report_financial]'))
DROP VIEW [dbo].[report_financial]

GO

CREATE VIEW [report_financial]
AS
SELECT
       [org].[organization_id]
      ,[full_name]
      ,[short_name]
      ,[zkpo_code]
      ,COALESCE(old_industry, N'НЕ ВИЗНАЧЕНО') AS 'old_industry'
      ,COALESCE(old_occupation, N'НЕ ВИЗНАЧЕНО') AS 'old_occupation'
      ,[old_organ]
      ,[status]
      ,COALESCE(occupation, N'НЕ ВИЗНАЧЕНО') AS 'occupation'
      ,[form_gosp]
      ,[form_of_ownership]
      ,[gosp_struct]
      ,COALESCE(industry, N'НЕ ВИЗНАЧЕНО') AS 'industry'
      ,[vedomstvo]
      ,[org_form]
      ,[addr_district]
      ,[addr_street_name]
      ,[addr_nomer]
      ,[addr_korpus]
      ,[addr_zip_code]
      ,[director_fio]
      ,[director_phone]
      ,[buhgalter_fio]
      ,[buhgalter_phone]
      ,[fax]
      ,[registration_auth]
      ,[registration_num]
      ,[registration_date]
      ,[registration_svidot]
      ,[kved_code]
      ,[koatuu]
      ,[otdel_gukv]
      ,[mayno]
      ,[is_liquidated]
      ,[liquidation_date]
      ,[contact_email]
      ,[contact_posada]
      ,[sfera_upr]
      ,org.budg_payments_rate AS 'budg_payments_rate'
      ,CASE WHEN LEN(addr_zip_code) > 0 THEN addr_zip_code + ', ' ELSE '' END +
       LTRIM(RTRIM(addr_street_name)) + ' ' + LTRIM(RTRIM(addr_nomer)) + COALESCE(' ' + addr_korpus, '') AS 'addr_address'
	  , CASE WHEN is_under_closing = 1 THEN N'ТАК' ELSE N'НІ' END AS [is_under_closing]       
FROM
       view_organizations org
WHERE
       (org.origin_db = 2) AND
       ((org.end_state_date IS NULL) OR (org.end_state_date > GETDATE()))

GO

/* report_balans_and_arenda - combines information about balans with information about arenda */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[report_balans_and_arenda]'))
DROP VIEW [dbo].[report_balans_and_arenda]

GO

CREATE VIEW [report_balans_and_arenda]
AS
SELECT
       [bal].[balans_id]
      ,[bal].[building_id]
      ,[bal].[organization_id]
      ,[bal].[org_full_name]
      ,[bal].[org_short_name]
      ,[bal].[org_zkpo_code]
      ,[bal].[org_industry]
      ,[bal].[org_occupation]
      ,[bal].[org_old_industry]
      ,[bal].[org_old_occupation]
      ,[bal].[org_vedomstvo]
      ,[bal].[org_ownership]
      ,[bal].[district]
      ,[bal].[street_full_name]
      ,[bal].[addr_nomer]
      ,[bal].[sqr_total] AS 'balans_sqr_total'
      ,[bal].[sqr_pidval] AS 'balans_sqr_pidval'
      ,[bal].[sqr_vlas_potreb] AS 'balans_sqr_vlas_potreb'
      ,[bal].[sqr_free] AS 'balans_sqr_free'
      ,[bal].[sqr_in_rent] AS 'balans_sqr_in_rent'
      ,[bal].[sqr_privatizov] AS 'balans_sqr_privatizov'
      ,[bal].[sqr_not_for_rent] AS 'balans_sqr_not_for_rent'
      ,[bal].[sqr_gurtoj] AS 'balans_sqr_gurtoj'
      ,[bal].[sqr_non_habit] AS 'balans_sqr_non_habit'
      ,[bal].[cost_balans] AS 'balans_cost_balans'
      ,[bal].[cost_expert_1m] AS 'balans_cost_expert_1m'
      ,[bal].[cost_expert_total] AS 'balans_cost_expert_total'
      ,[bal].[cost_zalishkova] AS 'balans_cost_zalishkova'
      ,[bal].[num_rent_agr]
      ,[bal].[num_privat_apt]
      ,[bal].[approval_by]
      ,[bal].[approval_num]
      ,[bal].[approval_date]
      ,[bal].[o26_code]
      ,[bal].[bti_condition]
      ,[bal].[num_floors]
      ,[bal].[org_maintain_id]
      ,[bal].[org_maintainer_full_name]
      ,[bal].[org_maintainer_short_name]
      ,[bal].[org_maintainer_zkpo_code]
      ,[bal].[otdel_gukv]
      ,[bal].[form_ownership]
      ,[bal].[ownership_type]
      ,[bal].[object_kind]
      ,[bal].[object_type]
      ,[bal].[condition]
      ,[bal].[purpose_group]
      ,[bal].[purpose]
      ,[bal].[history]
      ,[bal].[date_expert]
      ,[bal].[floors]
      ,[bal].[obj_bti_code]
      ,[bal].[memo]
      ,[bal].[note]
      ,[bal].[znos]
      ,[bal].[znos_date]
      ,[bal].[input_date]
      ,[bal].[addr_zip_code]
      ,[bal].[construct_year]
      ,[bal].[oatuu_code]
      ,[bal].[facade]
      ,[bal].[is_in_privat]
      ,[bal].[form_ownership_int]
      ,[bal].[org_ownership_int]
      ,[bal].[balans_obj_name]
      ,[bal].[sqr_free_total]
      ,[bal].[sqr_free_korysna]
      ,[bal].[sqr_free_mzk]
      ,[bal].[free_sqr_floors]
      ,[bal].[free_sqr_purpose]
      ,[bal].[org_sfera_upr_id]
      ,[bal].[org_district_id] 
      ,[ar].[arenda_id]
      ,[ar].[org_renter_id]
      ,[ar].[org_renter_full_name]
      ,[ar].[org_renter_short_name]
      ,[ar].[org_renter_zkpo]
      ,[ar].[org_renter_industry]
      ,[ar].[org_renter_occupation]
      ,[ar].[org_renter_vedomstvo]
      ,[ar].[org_giver_id]
      ,[ar].[org_giver_full_name]
      ,[ar].[org_giver_short_name]
      ,[ar].[org_giver_zkpo]
      ,[ar].[org_giver_industry]
      ,[ar].[org_giver_occupation]
      ,[ar].[org_giver_vedomstvo]
      ,[ar].[object_name]
      ,[ar].[object_note]
      ,[ar].[is_privat]
      ,[ar].[agreement_kind]
      ,[ar].[agreement_date]
      ,[ar].[agreement_date_year]
      ,[ar].[agreement_date_quarter]
      ,[ar].[agreement_num]
      ,[ar].[floor_number]
      ,[ar].[cost_narah] AS 'arenda_cost_narah'
      ,[ar].[cost_payed] AS 'arenda_cost_payed'
      ,[ar].[cost_debt] AS 'arenda_cost_debt'
      ,[ar].[cost_agreement] AS 'arenda_cost_agreement'
      ,[ar].[cost_expert_1m] AS 'arenda_cost_expert_1m'
      ,[ar].[cost_expert_total] AS 'arenda_cost_expert_total'
      ,[ar].[debt_timespan]
      ,[ar].[pidstava]
      ,[ar].[pidstava_date]
      ,[ar].[pidstava_num]
      ,[ar].[pidstava_display]
      ,[ar].[rent_start_date]
      ,[ar].[rent_start_year]
      ,[ar].[rent_start_quarter]
      ,[ar].[rent_finish_date]
      ,[ar].[rent_finish_year]
      ,[ar].[rent_finish_quarter]
      ,[ar].[rent_actual_finish_date]
      ,[ar].[actual_finish_year]
      ,[ar].[actual_finish_quarter]
      ,[ar].[rent_rate_percent]
      --,[ar].[rent_rate_uah]
	  ,[ar].[rent_rate_percent] as 'rent_rate_uah'
      ,[ar].[rent_square]
      ,[ar].[rishennya_code]
      ,[ar].[num_akt]
      ,[ar].[date_akt]
      ,[ar].[is_subarenda]
      ,[ar].[payment_type]
      ,[ar].[agreement_active]
      ,[ar].[agreement_active_int]
FROM
    view_balans bal
    LEFT OUTER JOIN view_arenda ar ON ar.balans_id = bal.balans_id

GO

/******************************************************************************/
/*                    Creation of the materialized views                      */
/******************************************************************************/

IF OBJECT_ID('view_docs_objects_akts_materialized') IS NOT NULL
DROP TABLE view_docs_objects_akts_materialized

GO

IF OBJECT_ID('m_view_docs_objects_akts') IS NOT NULL
DROP TABLE m_view_docs_objects_akts

GO

SELECT * INTO m_view_docs_objects_akts FROM view_docs_objects_akts

GO

IF OBJECT_ID('m_view_object_rights') IS NOT NULL
DROP TABLE m_view_object_rights

GO

SELECT *
INTO m_view_object_rights
FROM view_object_rights
ORDER BY building_id, rozp_doc_id, akt_id, object_type, object_kind, name, sqr_transferred, len_transferred, right_name

GO

IF OBJECT_ID('m_report_balans_and_arenda') IS NOT NULL
DROP TABLE m_report_balans_and_arenda

GO

SELECT * 
INTO m_report_balans_and_arenda
FROM report_balans_and_arenda

GO

IF OBJECT_ID('m_view_arenda') IS NOT NULL
DROP TABLE m_view_arenda

GO

SELECT view_arenda.*, IDENTITY(INTEGER, 1, 1) AS 'id'
INTO m_view_arenda
FROM view_arenda

GO

IF OBJECT_ID('m_view_arenda_agreements') IS NOT NULL
DROP TABLE m_view_arenda_agreements

GO

SELECT *
INTO m_view_arenda_agreements
FROM view_arenda_agreements
WHERE
    (is_privat_int <> 1 OR is_privat_int IS NULL) AND
    (agreement_kind_id IS NULL OR
    ((agreement_kind_id <> 9) AND
     (agreement_kind_id <> 10) AND
     (agreement_kind_id <> 16) AND
     (agreement_kind_id <> 17)))

GO

IF OBJECT_ID('m_view_rent_pay_for_arenda') IS NOT NULL
DROP TABLE m_view_rent_pay_for_arenda

GO

SELECT * 
INTO m_view_rent_pay_for_arenda
FROM view_rent_pay_for_arenda

IF OBJECT_ID('m_view_rent_debt_for_arenda') IS NOT NULL
DROP TABLE m_view_rent_debt_for_arenda

GO

SELECT * 
INTO m_view_rent_debt_for_arenda
FROM view_rent_debt_for_arenda

/******************************************************************************/
/*                            1NF reporting views                             */
/******************************************************************************/

IF OBJECT_ID('view_reports1nf') IS NOT NULL
DROP VIEW view_reports1nf

GO

CREATE VIEW view_reports1nf
AS
SELECT
    rep.organization_id,
    rep.is_reviewed,
    rep.create_date,
    rep.id AS 'report_id',
    [org].[full_name],
    [org].[short_name],
    [org].[zkpo_code],
    [dict_org_industry].[name] AS 'industry',
    [dict_org_occupation].[name] AS 'occupation',
    [dict_org_status].[name] AS 'status',
    [dict_org_form_gosp].[name] AS 'form_gosp',
    [dict_org_ownership].[name] AS 'form_of_ownership',
    [dict_org_gosp_struct].[name] AS 'gosp_struct',
    [dict_org_vedomstvo].[name] AS 'vedomstvo',
    [dict_org_form].[name] AS 'org_form',
    [dict_org_gosp_struct_type].[name] AS 'gosp_struct_type',
    [dict_org_sfera_upr].[name] AS 'sfera_upr',
    [dict_org_old_industry].[name] AS 'old_industry',
    [dict_org_old_occupation].[name] AS 'old_occupation',
    [dict_org_old_organ].[name] AS 'old_organ',
    [org].[addr_city],
    [dict_districts2].[name] AS 'addr_district',
    [org].[addr_street_name],
    [org].[addr_nomer],
    [org].[addr_korpus],
    [org].[addr_zip_code],
    [director_fio],
    [director_phone],
    [buhgalter_fio],
    [buhgalter_phone],
    [num_buildings],
    [fax],
    [registration_auth],
    [registration_num],
    [registration_date],
    [registration_svidot],
    [sqr_on_balance],
    [org].[sqr_total],
    [num_objects],
    [kved_code],
    [koatuu],
    [dict_org_mayno].[name] AS 'mayno',
    CASE WHEN [is_liquidated] = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_liquidated',
    [liquidation_date],
    [contact_email],
    [dict_org_contact_posada].[name] AS 'contact_posada',
    COALESCE([dict_org_registr_org].[name], registration_auth) AS 'registr_org',
    org.modified_by,
    org.modify_date,
    org.sfera_upr_id AS 'org_sfera_upr_id',
    org.form_ownership_id AS 'org_form_ownership_id',
    org.addr_distr_new_id AS 'org_district_id',
      
    CASE WHEN is_reviewed = 1 THEN N'Перевірено' ELSE N'Не перевірено' END AS 'review_state',
    CASE WHEN
		EXISTS (SELECT id FROM reports1nf_balans bal WHERE bal.report_id = rep.id AND (bal.submit_date IS NULL OR bal.modify_date > bal.submit_date) AND ((bal.is_deleted is null) or (bal.is_deleted = 0))) OR
		EXISTS (SELECT id FROM reports1nf_balans_deleted bd WHERE bd.report_id = rep.id AND (bd.submit_date IS NULL OR bd.modify_date > bd.submit_date) AND ((bd.is_deleted is null) or (bd.is_deleted = 0))) OR
		EXISTS (SELECT id FROM reports1nf_arenda ar WHERE ar.report_id = rep.id AND (ar.submit_date IS NULL OR ar.modify_date > ar.submit_date) AND ((ar.is_deleted is null) or (ar.is_deleted = 0))) OR
		EXISTS (SELECT id FROM reports1nf_arenda_rented artd WHERE artd.report_id = rep.id AND (artd.submit_date IS NULL OR artd.modify_date > artd.submit_date) AND ((artd.is_deleted is null) or (artd.is_deleted = 0))) OR
		EXISTS (SELECT id FROM reports1nf_org_info oi WHERE oi.report_id = rep.id AND (oi.submit_date IS NULL OR oi.modify_date > oi.submit_date) AND ((oi.is_deleted is null) or (oi.is_deleted = 0)))
	THEN N'Не надісланий' ELSE N'Надісланий' END AS 'cur_state',
	(SELECT MAX(b.submit_date) FROM reports1nf_balans b WHERE b.report_id = rep.id) AS 'bal_max_submit_date',
	(SELECT MAX(bd.submit_date) FROM reports1nf_balans_deleted bd WHERE bd.report_id = rep.id) AS 'bal_del_max_submit_date',
	(SELECT MAX(ar.submit_date) FROM reports1nf_arenda ar WHERE ar.report_id = rep.id) AS 'arenda_max_submit_date',
	(SELECT MAX(artd.submit_date) FROM reports1nf_arenda_rented artd WHERE artd.report_id = rep.id) AS 'arenda_rented_max_submit_date',
	(SELECT MAX(o.submit_date) FROM reports1nf_org_info o WHERE o.report_id = rep.id) AS 'org_max_submit_date'
FROM
    reports1nf rep
    LEFT OUTER JOIN reports1nf_org_info org on org.report_id = rep.id
    LEFT OUTER JOIN dict_org_industry ON org.industry_id = dict_org_industry.id
    LEFT OUTER JOIN dict_org_occupation ON org.occupation_id = dict_org_occupation.id
    LEFT OUTER JOIN dict_org_status ON org.status_id = dict_org_status.id
    LEFT OUTER JOIN dict_org_form_gosp ON org.form_gosp_id = dict_org_form_gosp.id
    LEFT OUTER JOIN dict_org_ownership ON org.form_ownership_id = dict_org_ownership.id
    LEFT OUTER JOIN dict_org_gosp_struct ON org.gosp_struct_id = dict_org_gosp_struct.id
    LEFT OUTER JOIN dict_org_gosp_struct_type ON org.gosp_struct_type_id = dict_org_gosp_struct_type.id
    LEFT OUTER JOIN dict_org_vedomstvo ON org.vedomstvo_id = dict_org_vedomstvo.id
    LEFT OUTER JOIN dict_org_form ON org.form_id = dict_org_form.id
    LEFT OUTER JOIN dict_org_mayno ON org.mayno_id = dict_org_mayno.id
    LEFT OUTER JOIN dict_org_contact_posada ON org.contact_posada_id = dict_org_contact_posada.id
    LEFT OUTER JOIN dict_org_sfera_upr ON org.sfera_upr_id = dict_org_sfera_upr.id
    LEFT OUTER JOIN dict_org_registr_org ON org.registr_org_id = dict_org_registr_org.id
    LEFT OUTER JOIN dict_org_old_industry ON org.old_industry_id = dict_org_old_industry.id
    LEFT OUTER JOIN dict_org_old_occupation ON org.old_occupation_id = dict_org_old_occupation.id
    LEFT OUTER JOIN dict_org_old_organ ON org.old_organ_id = dict_org_old_organ.id
    LEFT OUTER JOIN dict_districts2 ON org.addr_distr_new_id = dict_districts2.id

GO

/******************************************************************************/
/*                                 Misc Views                                 */
/******************************************************************************/

IF OBJECT_ID('view_building_renters_no_agreement') IS NOT NULL
DROP VIEW view_building_renters_no_agreement

GO

CREATE VIEW view_building_renters_no_agreement
AS
SELECT DISTINCT
    renter.id AS 'org_renter_id',
    decs.building_id
FROM
    arenda_decisions decs
    INNER JOIN arenda_applications appl ON appl.id = decs.application_id
    INNER JOIN doc_appendices app ON app.id = decs.appendix_rasp_id
    INNER JOIN documents doc ON doc.id = app.doc_id
    LEFT OUTER JOIN dict_rent_decisions decision ON decision.id = decs.decision_id
    LEFT OUTER JOIN organizations renter ON renter.id = decs.org_renter_id
WHERE
    decs.is_subarenda <> 2 AND
    decs.decision_id IN (21, 22, 24) AND
    NOT (appl.appl_letter_date IS NULL) AND
    NOT EXISTS (SELECT ar.id FROM arenda ar INNER JOIN link_arenda_2_decisions lnk ON lnk.arenda_id = ar.id WHERE
        ar.org_renter_id = renter.id AND RTRIM(LTRIM(lnk.doc_num)) = RTRIM(LTRIM(doc.doc_num)))

GO
