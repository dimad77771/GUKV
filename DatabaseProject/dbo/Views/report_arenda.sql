
CREATE VIEW [report_arenda]
AS
SELECT
    [organizations].[full_name]
   ,[organizations].[short_name]
   ,[organizations].[zkpo_code]
   ,[dict_org_occupation].[name] AS 'occupation'
   ,[dict_org_status].[name] AS 'status'
   ,[dict_org_form_gosp].[name] AS 'form_gosp'
   ,[dict_org_ownership].[name] AS 'form_of_ownership'
   ,[dict_org_gosp_struct].[name] AS 'gosp_struct'
   ,[dict_org_organ].[name] AS 'organ'
   ,[dict_org_industry].[name] AS 'industry'
   ,[dict_org_priznak].[name] AS 'priznak'
   ,[dict_org_vedomstvo].[name] AS 'vedomstvo'
   ,[dict_org_title].[display_name] AS 'title'
   ,[dict_org_form].[name] AS 'org_form'
   ,[dict_org_gosp_struct_type].[name] AS 'gosp_struct_type'
   ,[dict_org_share_type].[name] AS 'share_type'
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
   ,CASE WHEN [is_liquidated] = 1 THEN N'ТАК' ELSE N'НI' END AS 'is_liquidated'
   ,[liquidation_date]
   ,[contact_email]
   ,[dict_org_contact_posada].[name] AS 'contact_posada'
   ,[dict_org_nadhodjennya].[name] AS 'nadhodjennya'
   ,[dict_org_vibuttya].[name] AS 'vibuttya'
   ,[dict_org_privat_status].[name] AS 'privat_status'
   ,[dict_org_cur_state].[name] AS 'cur_state'
   ,[dict_org_sfera_upr].[name] AS 'sfera_upr'
   ,[dict_org_plan_zone].[name] AS 'plan_zone'
   ,[dict_org_registr_org].[name] AS 'registr_org'
   ,[nadhodjennya_date]
   ,[vibuttya_date]

   ,[ar].[agreement_date] AS 'rent_agreement_date'
   ,[ar].[agreement_num] AS 'rent_agreement_num'
   ,[ar].[rent_start_date]
   ,[ar].[rent_finish_date]
   ,[ar].[street_full_name] AS 'building_street'
   ,[ar].[addr_nomer] AS 'building_nomer'
   ,[ar].[district] AS 'building_district'
   ,[ar].[addr_zip_code] AS 'building_zip_code'
   ,[ar].[condition] AS 'building_condition'
   ,[ar].[bti_code] AS 'building_bti_code'
   ,[ar].[history] AS 'building_history'
   ,[ar].[object_type] AS 'building_type'
   ,[ar].[object_kind] AS 'building_kind'
   ,[ar].[sqr_total] AS 'building_sqr_total'
   ,[ar].[facade] AS 'building_facade'
FROM
    [GUKV].[dbo].[organizations]
    LEFT OUTER JOIN dict_org_occupation ON organizations.occupation_id = dict_org_occupation.id
    LEFT OUTER JOIN dict_org_status ON organizations.status_id = dict_org_status.id
    LEFT OUTER JOIN dict_org_form_gosp ON organizations.form_gosp_id = dict_org_form_gosp.id
    LEFT OUTER JOIN dict_org_ownership ON organizations.form_ownership_id = dict_org_ownership.id
    LEFT OUTER JOIN dict_org_gosp_struct ON organizations.gosp_struct_id = dict_org_gosp_struct.id
    LEFT OUTER JOIN dict_org_organ ON organizations.organ_id = dict_org_organ.id
    LEFT OUTER JOIN dict_org_industry ON organizations.industry_id = dict_org_industry.id
    LEFT OUTER JOIN dict_districts2 ON organizations.addr_distr_new_id = dict_districts2.id
    LEFT OUTER JOIN dict_org_priznak ON organizations.priznak_id = dict_org_priznak.id
    LEFT OUTER JOIN dict_org_vedomstvo ON organizations.vedomstvo_id = dict_org_vedomstvo.id
    LEFT OUTER JOIN dict_org_title ON organizations.title_id = dict_org_title.id
    LEFT OUTER JOIN dict_org_form ON organizations.form_id = dict_org_form.id
    LEFT OUTER JOIN dict_org_gosp_struct_type ON organizations.gosp_struct_type_id = dict_org_gosp_struct_type.id
    LEFT OUTER JOIN dict_org_share_type ON organizations.share_type_id = dict_org_share_type.id
    LEFT OUTER JOIN dict_otdel_gukv ON organizations.otdel_gukv_id = dict_otdel_gukv.id
    LEFT OUTER JOIN dict_org_mayno ON organizations.mayno_id = dict_org_mayno.id
    LEFT OUTER JOIN dict_org_contact_posada ON organizations.contact_posada_id = dict_org_contact_posada.id
    LEFT OUTER JOIN dict_org_nadhodjennya ON organizations.nadhodjennya_id = dict_org_nadhodjennya.id
    LEFT OUTER JOIN dict_org_vibuttya ON organizations.vibuttya_id = dict_org_vibuttya.id
    LEFT OUTER JOIN dict_org_privat_status ON organizations.privat_status_id = dict_org_privat_status.id
    LEFT OUTER JOIN dict_org_cur_state ON organizations.cur_state_id = dict_org_cur_state.id
    LEFT OUTER JOIN dict_org_sfera_upr ON organizations.sfera_upr_id = dict_org_sfera_upr.id
    LEFT OUTER JOIN dict_org_plan_zone ON organizations.plan_zone_id = dict_org_plan_zone.id
    LEFT OUTER JOIN dict_org_registr_org ON organizations.registr_org_id = dict_org_registr_org.id
    /**/
    LEFT OUTER JOIN view_org_arenda ar ON organizations.id = ar.org_renter_id
