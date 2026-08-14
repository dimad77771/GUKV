
CREATE VIEW [report_objects]
AS
SELECT
    [b].[building_id] AS 'building_id'
   ,[b].[street_full_name]
   ,[b].[addr_nomer]
   ,[b].[district]
   ,[b].[addr_zip_code]
   ,[b].[condition]
   ,[b].[num_floors]
   ,[b].[construct_year]
   ,[b].[bti_code]
   ,[b].[history]
   ,[b].[object_type]
   ,[b].[object_kind]
   ,[b].[sqr_total]
   ,[b].[sqr_habit]
   ,[b].[sqr_non_habit]
   ,[b].[additional_info]
   ,[b].[oatuu_code]
   ,[b].[facade]

   ,[bal].[org_full_name] AS 'balans_org_full_name'
   ,[bal].[org_short_name] AS 'balans_org_short_name'
   ,[bal].[sqr_total] AS 'balans_sqr_total'
   ,[bal].[sqr_pidval] AS 'balans_sqr_pidval'
   ,[bal].[sqr_vlas_potreb] AS 'balans_sqr_vlas_potreb'
   ,[bal].[sqr_free] AS 'balans_sqr_free'
   ,[bal].[sqr_in_rent] AS 'balans_sqr_in_rent'
   ,[bal].[sqr_privatizov] AS 'balans_sqr_privatizov'
   ,[bal].[sqr_not_for_rent] AS 'balans_sqr_not_for_rent'
   ,[bal].[sqr_gurtoj] AS 'balans_sqr_gurtoj'
   ,[bal].[sqr_non_habit] AS 'balans_sqr_non_habit'
   ,[bal].[sqr_kor] AS 'balans_sqr_kor'
   ,[bal].[cost_balans] AS 'balans_cost'
   ,[bal].[cost_extert_1m] AS 'balans_cost_extert_1m'
   ,[bal].[cost_expert_total] AS 'balans_cost_expert_total'
   ,[bal].[num_rent_agr] AS 'balans_num_rent_agreements'
   ,[bal].[num_privat_apt] AS 'balans_num_privat_apartments'
   ,[bal].[o26_code] AS 'balans_o26_code'
   ,[bal].[bti_condition] AS 'balans_bti_condition'
   ,[bal].[purpose_group] AS 'balans_purpose_group'
   ,[bal].[purpose] AS 'balans_purpose'
   ,[bal].[num_floors] AS 'balans_num_floors'
   ,[bal].[ownership_type] AS 'balans_ownership_type'
   ,[bal].[org_maintainer_full_name] AS 'balans_maintainer_full_name'
   ,[bal].[org_maintainer_short_name] AS 'balans_maintainer_short_name'
   ,[bal].[date_expert] AS 'balans_date_expert'
   ,[bal].[otdel_gukv] AS 'balans_otdel_gukv'

   ,[ar].[org_renter_full_name] AS 'arenda_renter_full_name'
   ,[ar].[org_renter_short_name] AS 'arenda_renter_short_name'
   ,[ar].[purpose_group] AS 'arenda_purpose_group'
   ,[ar].[purpose] AS 'arenda_purpose'
   ,[ar].[is_privat] AS 'arenda_is_privat'
   ,[ar].[agreement_kind] AS 'arenda_agreement_kind'
   ,[ar].[agreement_date] AS 'arenda_agreement_date'
   ,[ar].[agreement_num] AS 'arenda_agreement_num'
   ,[ar].[floor_number] AS 'arenda_floor_number'
   ,[ar].[cost_narah] AS 'arenda_cost_narah'
   ,[ar].[cost_payed] AS 'arenda_cost_payed'
   ,[ar].[cost_debt] AS 'arenda_debt'
   ,[ar].[cost_agreement] AS 'arenda_cost_agreement'
   ,[ar].[cost_expert_1m] AS 'arenda_cost_expert_1m'
   ,[ar].[cost_expert_total] AS 'arenda_cost_expert_total'
   ,[ar].[pidstava] AS 'arenda_pidstava'
   ,[ar].[pidstava_date] AS 'arenda_pidstava_date'
   ,[ar].[pidstava_num] AS 'arenda_pidstava_num'
   ,[ar].[rent_start_date] AS 'arenda_start_date'
   ,[ar].[rent_finish_date] AS 'arenda_finish_date'
   ,[ar].[rent_actual_finish_date] AS 'arenda_actual_finish_date'
   ,[ar].[rent_rate_percent] AS 'arenda_rate'
   ,[ar].[rent_rate_uah] AS 'arenda_rate_uah'
   ,[ar].[rent_square] AS 'arenda_square'
   ,[ar].[rishennya_code] AS 'arenda_rishennya_code'
   ,[ar].[is_subarenda] AS 'arenda_is_subarenda'
   ,[ar].[payment_type] AS 'arenda_payment_type'
FROM
    view_buildings AS b
    LEFT OUTER JOIN view_balans bal ON b.building_id = bal.building_id
    LEFT OUTER JOIN view_arenda ar ON b.building_id = ar.building_id
