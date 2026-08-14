
CREATE PROCEDURE dbo.fnDeleteBalansObjectInReport
(	
	@BALANS_ID INTEGER,
	@COPY_TO_DELETED BIT,
	@VIDCH_TYPE_ID INTEGER,
	@VIDCH_DOC_NUM VARCHAR(24),
	@VIDCH_DOC_DATE datetime,
	@VIDCH_ORG_ID INTEGER
)
AS
	IF EXISTS (SELECT id FROM reports1nf_balans WHERE id = @BALANS_ID)
	BEGIN
		/* Get some properties of the primary balans object (which is already deleted) */
		DECLARE @MODIFIED_BY VARCHAR(128)
		DECLARE @MODIFY_DATE DATETIME
		DECLARE @DEL_DATE DATE
		
		SELECT
			@MODIFIED_BY = bal.modified_by,
			@MODIFY_DATE = bal.modify_date,
			@DEL_DATE = bal.del_date
		FROM balans bal
		WHERE bal.id = @BALANS_ID

		IF (@COPY_TO_DELETED = 1)
		BEGIN
			/* Copy the object to the 'reports1nf_balans_deleted' table */
			INSERT INTO reports1nf_balans_deleted
			SELECT 
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
				,[obj_nomer]
				,[obj_bti_code]
				,[obj_addr_misc]
				,[obj_street_misc]
				,@MODIFIED_BY AS 'modified_by'
				,@MODIFY_DATE AS 'modify_date'
				,[form_giver_id]
				,[org_maintain_id]
				,[update_src_id]
				,1 AS 'is_deleted'
				,@DEL_DATE AS 'del_date'
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
				--,[vidch_doc_type]
				,3 as 'vidch_doc_type'
				--,[vidch_doc_num]
				,@VIDCH_DOC_NUM as 'vidch_doc_num'
				--,[vidch_doc_date]
				,@VIDCH_DOC_DATE as 'vidch_doc_date'
				--,[vidch_type_id]
				,@VIDCH_TYPE_ID AS 'vidch_type_id'
				--,[vidch_org_id]
				,CASE WHEN @VIDCH_ORG_ID > 0 THEN @VIDCH_ORG_ID ELSE [vidch_org_id] END 'vidch_org_id'
				,[vidch_cost]
				,[sqr_engineering]
				,[obj_status_id]
				,[report_id]
				,NULL
				,0 AS 'is_valid'
				,'' AS 'validation_errors'
			FROM reports1nf_balans WHERE id = @BALANS_ID
		END;

		/* Delete the object from the 'reports1nf_balans' table */
		DELETE FROM reports1nf_balans WHERE id = @BALANS_ID
	END