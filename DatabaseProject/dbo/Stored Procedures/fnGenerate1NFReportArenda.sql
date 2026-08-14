
CREATE PROCEDURE dbo.fnGenerate1NFReportArenda
(	
	@ORG_ID INTEGER,
	@REPORT_ID INTEGER
)
AS
	/* Copy information about rent agreements to the 'report1nf_arenda' table */
	INSERT INTO reports1nf_arenda SELECT ar.*, @REPORT_ID AS 'report_id', NULL AS 'submit_date', NULL AS 'building_1nf_unique_id', 0 as 'is_valid', '' AS 'validation_errors'
		FROM arenda ar
		WHERE ar.org_balans_id = @ORG_ID AND (ar.is_deleted IS NULL OR ar.is_deleted = 0) AND (ar.is_privat IS NULL OR ar.is_privat = 0)
		
	/* Copy building information and all the related objects (notes, decisions, payments) for each rent agreement */
	DECLARE @ARENDA_ID INTEGER
	DECLARE @ARENDA_BUILDING_ID INTEGER
	DECLARE @BUILDING_UNIQUE_ID INTEGER
	DECLARE @TmpTableBuildingUniqueId TABLE (unique_id INTEGER)
	
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
				date_expert, payment_type_id, invent_no, note_status_id, @REPORT_ID AS 'report_id'
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

