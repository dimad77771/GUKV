
CREATE PROCEDURE [dbo].[fnPushRentedObjectToArchive]
(	
	@ARENDA_RENTED_ID INTEGER
)
AS
	INSERT INTO arch_arenda_rented
	(
		id,
		building_id,
		org_renter_id,
		org_giver_id,
		payment_type_id,
		agreement_date,
		agreement_num,
		rent_start_date,
		rent_finish_date,
		rent_square,
		is_subarenda,
		is_cmk,
		cmk_sqr_rented,
		cmk_payment_narah,
		cmk_payment_to_budget,
		cmk_rent_debt,
		modify_date,
		modified_by,
		is_deleted,
		del_date,
		archive_entry_timestamp
		,rent_period_id
		,report_id
	)
	SELECT
		id,
		building_id,
		org_renter_id,
		org_giver_id,
		payment_type_id,
		agreement_date,
		agreement_num,
		rent_start_date,
		rent_finish_date,
		rent_square,
		is_subarenda,
		is_cmk,
		cmk_sqr_rented,
		cmk_payment_narah,
		cmk_payment_to_budget,
		cmk_rent_debt,
		modify_date,
		modified_by,
		is_deleted,
		del_date,
		GETDATE() AS 'archive_entry_timestamp'
		,(select top 1 id from dbo.dict_rent_period where is_active = 1 order by id desc)
		,(SELECT  ar.report_id FROM reports1nf_arenda_rented ar WHERE ar.arenda_rented_id = @ARENDA_RENTED_ID)
	FROM arenda_rented WHERE id = @ARENDA_RENTED_ID