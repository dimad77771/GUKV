
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
