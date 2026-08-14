
CREATE PROCEDURE dbo.fnAddBalansObjectToReport
(	
	@BALANS_ID INTEGER
)
AS
	/* Check if report exists for this organization, before doing anything else */
	IF EXISTS (SELECT rep.id FROM reports1nf rep INNER JOIN balans bal ON bal.organization_id = rep.organization_id WHERE bal.id = @BALANS_ID)
	BEGIN
		/* If this balans object is already present in the report, do nothing */
		IF NOT EXISTS (SELECT id FROM reports1nf_balans WHERE id = @BALANS_ID) 
		BEGIN
			/* Get a report ID */
			DECLARE @REPORT_ID INTEGER
			DECLARE @BALANS_BUILDING_ID INTEGER
			
			SELECT @REPORT_ID = rep.id, @BALANS_BUILDING_ID = bal.building_id
				FROM reports1nf rep
				INNER JOIN balans bal ON bal.organization_id = rep.organization_id
				WHERE bal.id = @BALANS_ID

			/* Copy information about balans objects to the 'report1nf_balans' table */
			INSERT INTO reports1nf_balans
			([id]
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
      ,[validation_errors])
			
			
			SELECT bal.[id]
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
			,
				@REPORT_ID AS 'report_id',
				NULL AS 'building_1nf_unique_id',
				NULL AS 'submit_date',
				0 AS 'is_valid',
				'' AS 'validation_errors'
			FROM balans bal
			WHERE bal.id = @BALANS_ID

			/* Copy building information */
			DECLARE @BUILDING_UNIQUE_ID INTEGER
			DECLARE @TmpTableBuildingUniqueId TABLE (unique_id INTEGER)
			
			INSERT INTO reports1nf_buildings OUTPUT INSERTED.unique_id INTO @TmpTableBuildingUniqueId
				SELECT b.*, @REPORT_ID AS 'report_id' FROM buildings b WHERE b.id = @BALANS_BUILDING_ID

			SET @BUILDING_UNIQUE_ID = (SELECT unique_id FROM @TmpTableBuildingUniqueId)

			UPDATE reports1nf_balans SET building_1nf_unique_id = @BUILDING_UNIQUE_ID
				WHERE report_id = @REPORT_ID AND id = @BALANS_ID
		END
	END