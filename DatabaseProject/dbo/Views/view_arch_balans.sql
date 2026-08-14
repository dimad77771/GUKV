

CREATE VIEW [dbo].[view_arch_balans]
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
--      ,[dict_balans_bti].[name] AS 'bti_condition'
      ,[ab].bti_id as 'bti_condition'
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

      ,[ab].[free_sqr_condition_id] as privacynote
      ,[ab].[free_sqr_location] as realestateobj
      ,org_holder.gosp_struct

FROM
    [dbo].[arch_balans] ab
    LEFT OUTER JOIN dict_balans_o26 ON ab.o26_id = dict_balans_o26.id
--    LEFT OUTER JOIN dict_balans_bti ON ab.bti_id = dict_balans_bti.id
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
