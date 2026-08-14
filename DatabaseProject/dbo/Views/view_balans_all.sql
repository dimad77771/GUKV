CREATE VIEW [dbo].[view_balans_all]
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
	        ,[org_holder].[addr_street_name] AS 'org_street_name'
            ,[org_holder].[addr_nomer] AS 'org_street_nomer'

      ,[b].[district]      
      ,[b].[street_full_name]
      ,[b].[addr_nomer]
      ,[b].[addr_misc]
      ,[balans].[sqr_total]
      ,[balans].[sqr_pidval]
      ,[balans].[sqr_vlas_potreb]
      ,[balans].free_sqr_useful as [sqr_free]
      ,[balans].[sqr_in_rent]
      ,[balans].[sqr_privatizov]
      ,[balans].[sqr_not_for_rent]
      ,[balans].[sqr_gurtoj]
      ,[balans].[sqr_non_habit]
      ,[balans].[sqr_kor]
      ,[balans].sqr_engineering
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
--      ,[dict_balans_bti].[name] AS 'bti_condition'
      ,[balans].[bti_id] AS 'bti_condition'
      --,[balans].[floors] AS 'num_floors'
	  ,[b].[num_floors]
      ,[balans].[org_maintain_id]
      ,[org_maintainer].[full_name] AS 'org_maintainer_full_name'
      ,[org_maintainer].[short_name] AS 'org_maintainer_short_name'
      ,[org_maintainer].[zkpo_code] AS 'org_maintainer_zkpo_code'
--      ,[dict_otdel_gukv].[name] AS 'otdel_gukv'
      ,org_holder.otdel_gukv
      ,[dict_org_ownership].[name] AS 'form_ownership'
      ,[balans].[form_ownership_id] AS 'form_ownership_int'
      ,[dict_balans_ownership_type].[name] AS 'ownership_type'
      ,dict_object_kind.code + '-'+[dict_object_kind].[name] AS 'object_kind'
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
      ,[balans].[free_sqr_condition_id] as privacynote
      ,[balans].[free_sqr_location] as realestateobj
      ,org_holder.gosp_struct
      ,isnull(org_holder.director_phone+';','') + isnull(org_holder.buhgalter_phone+';','') + isnull(org_holder.director_title, '') AS org_contacts
	  ,[balans].[znizhino_flag]
	  ,[balans].[znizhino_shkoda]
	  ,[balans].[znizhino_zvitakt]
	  ,[balans].[znizhino_primitka]
	  ,[balans].[znizhino_stanom]
	  ,[dict_object_kind].[code] AS 'object_kind_code'
	  ,[dict_object_kind].[name] AS 'object_kind_name'
FROM
    [dbo].[balans]
    LEFT OUTER JOIN dict_balans_o26 ON balans.o26_id = dict_balans_o26.id
--    LEFT OUTER JOIN dict_balans_bti ON balans.bti_id = dict_balans_bti.id
    LEFT OUTER JOIN dict_balans_purpose_group ON balans.purpose_group_id = dict_balans_purpose_group.id
    LEFT OUTER JOIN dict_balans_purpose ON balans.purpose_id = dict_balans_purpose.id
    LEFT OUTER JOIN dict_balans_ownership_type ON balans.ownership_type_id = dict_balans_ownership_type.id
--    LEFT OUTER JOIN dict_otdel_gukv ON balans.otdel_gukv_id = dict_otdel_gukv.id
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
	(SELECT TOP 1 bal2.id FROM balans bal2 INNER JOIN buildings b2 ON b2.id = bal2.building_id WHERE
			b2.addr_street_id = b.addr_street_id and
			b2.addr_nomer1 = b.addr_nomer1 and
			bal2.id != balans.id and
			bal2.sqr_total = balans.sqr_total and
			ISNULL(bal2.is_deleted, 0) = 0
	) twin
WHERE
    (NOT [balans].[building_id] IS NULL)
    AND
    (NOT [balans].[organization_id] IS NULL)

