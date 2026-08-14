CREATE PROCEDURE [dbo].[fnGenerateInitial1NFReport]
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
	([id]
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
    ,[report_id]
    ,[submit_date]
    ,[budget_narah_50_uah]
    ,[budget_zvit_50_uah]
    ,[budget_prev_50_uah]
    ,[budget_debt_30_50_uah]
    ,[is_special_organization]
    ,[payment_budget_special]
    ,[buhgalter_email]
    ,[konkurs_payments]
    ,[unknown_payments]
    ,[unknown_payment_note]
    ,[prim_balanc]
    ,[bank_rahunok]
    ,[ouprav_id]
    ,[pravform_id]
    ,[planuvania_1]
    ,[planuvania_2]
    ,[planuvania_3]
    ,[planuvania_4]
    ,[planuvania_5]
	)
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
      ,'' as prim_balanc
	  ,'' as bank_rahunok
	  ,org.ouprav_id
	  ,org.pravform_id
	  ,null,null,null,null,null
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
	INSERT INTO reports1nf_balans_deleted([id],[year_balans],[building_id],[organization_id],[sqr_total],[sqr_pidval],[sqr_vlas_potreb],[sqr_free],[sqr_in_rent],[sqr_privatizov],[sqr_not_for_rent],[sqr_gurtoj],[sqr_non_habit],[sqr_kor],[cost_balans],[cost_fair],[cost_expert_1m],[cost_expert_total],[cost_rent_narah],[cost_rent_payed],[cost_debt],[cost_zalishkova],[cost_rinkova],[cost_fair_1m],[num_people],[num_rent_agr],[num_privat_apt],[o26_id],[bti_id],[approval_by],[approval_num],[approval_date],[form_ownership_id],[object_kind_id],[object_type_id],[history_id],[tech_condition_id],[purpose_group_id],[purpose_id],[purpose_str],[floors],[priznak_1nf],[ownership_type_id],[obj_street_name],[obj_street_id],[obj_street_name2],[obj_street_id2],[obj_nomer1],[obj_nomer2],[obj_nomer3],[obj_nomer],[obj_bti_code],[obj_addr_misc],[obj_street_misc],[modified_by],[modify_date],[form_giver_id],[org_maintain_id],[update_src_id],[is_deleted],[del_date],[znos],[znos_date],[date_expert],[reestr_no],[fair_cost_date],[otdel_gukv_id],[date_bti],[arch_id],[arch_flag],[date_cost_rinkova],[memo],[note],[is_free_sqr],[free_sqr_useful],[free_sqr_condition_id],[free_sqr_location],[ownership_doc_type],[ownership_doc_num],[ownership_doc_date],[balans_doc_type],[balans_doc_num],[balans_doc_date],[vidch_doc_type],[vidch_doc_num],[vidch_doc_date],[vidch_type_id],[vidch_org_id],[vidch_cost],[sqr_engineering],[obj_status_id],[report_id],[submit_date],[is_valid],[validation_errors])
	SELECT [id],[year_balans],[building_id],[organization_id],[sqr_total],[sqr_pidval],[sqr_vlas_potreb],[sqr_free],[sqr_in_rent],[sqr_privatizov],[sqr_not_for_rent],[sqr_gurtoj],[sqr_non_habit],[sqr_kor],[cost_balans],[cost_fair],[cost_expert_1m],[cost_expert_total],[cost_rent_narah],[cost_rent_payed],[cost_debt],[cost_zalishkova],[cost_rinkova],[cost_fair_1m],[num_people],[num_rent_agr],[num_privat_apt],[o26_id],[bti_id],[approval_by],[approval_num],[approval_date],[form_ownership_id],[object_kind_id],[object_type_id],[history_id],[tech_condition_id],[purpose_group_id],[purpose_id],[purpose_str],[floors],[priznak_1nf],[ownership_type_id],[obj_street_name],[obj_street_id],[obj_street_name2],[obj_street_id2],[obj_nomer1],[obj_nomer2],[obj_nomer3],[obj_nomer],[obj_bti_code],[obj_addr_misc],[obj_street_misc],[modified_by],[modify_date],[form_giver_id],[org_maintain_id],[update_src_id],[is_deleted],[del_date],[znos],[znos_date],[date_expert],[reestr_no],[fair_cost_date],[otdel_gukv_id],[date_bti],[arch_id],[arch_flag],[date_cost_rinkova],[memo],[note],[is_free_sqr],[free_sqr_useful],[free_sqr_condition_id],[free_sqr_location],[ownership_doc_type],[ownership_doc_num],[ownership_doc_date],[balans_doc_type],[balans_doc_num],[balans_doc_date],[vidch_doc_type],[vidch_doc_num],[vidch_doc_date],[vidch_type_id],[vidch_org_id],[vidch_cost],[sqr_engineering],[obj_status_id],
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
		,base_month
		,method_calc_id
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
				date_expert, payment_type_id, invent_no, note_status_id, @REPORT_ID AS 'report_id',
				null as zapezh_deposit,	null as ref_balans_id,	null as factich_vikorist_id
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
