
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
