
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
