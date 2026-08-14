CREATE VIEW [dbo].[view_arenda_agreements]
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
      ,[an].[agreement_state]
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
      ,isnull(arenda_dogchange.rent_used, [an].[rent_square]) as 'rent_square'
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
      ,an.date_expert
	  ,[org_holder].form_gosp as 'form_gosp'
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

	OUTER APPLY 
	(
		SELECT 
		top 1
		Q.rent_used
		FROM reports1nf_arenda_dogchange Q
		WHERE Q.arenda_id = an.id 
		and rent_start_date <= cast(getdate() as date) 
		and (rent_actual_finish_date is null or rent_actual_finish_date >= cast(getdate() as date))
		order by Q.rent_start_date desc
	) arenda_dogchange

WHERE
    (NOT [an].[building_id] IS NULL) AND
    (NOT [an].[org_renter_id] IS NULL)
