
CREATE PROCEDURE [dbo].[fnArendaClone]
(	
	@arenda_id INTEGER,
	@report_id INTEGER,
	@modified_by varchar(100),
	@modify_date datetime,
	@new_arenda_id INTEGER out
)
AS
BEGIN
	declare @minExistingId integer;
	declare @newAgreementId integer;
	declare @new_unique_id integer;
	declare @agreement_date datetime;
	declare @agreement_num varchar(1000);

	set @minExistingId = 0
	SELECT @minExistingId = MIN(id) FROM reports1nf_arenda WHERE report_id = @report_id
	set @newAgreementId = case when @minExistingId < 0 then @minExistingId - 1 else -1 end;

	select B.* 
	into #reports1nf_buildings 
	from [reports1nf_arenda] A 
	join [reports1nf_buildings] B on B.unique_id = A.building_1nf_unique_id and B.id = A.building_id 
	where A.id = @arenda_id and A.report_id = @report_id

	if (select count(*) from #reports1nf_buildings) <> 1
	begin
		RAISERROR ('Помилка', 11, 11);
		RETURN;
	end;

	insert into reports1nf_buildings(id, master_building_id, addr_street_name, addr_street_id, addr_street_name2, addr_street_id2, street_full_name, addr_distr_old_id, addr_distr_new_id, addr_nomer1, addr_nomer2, addr_nomer3, addr_nomer, addr_misc, addr_korpus_flag, addr_korpus, addr_zip_code, addr_address, tech_condition_id, date_begin, date_end, num_floors, construct_year, condition_year, is_condition_valid, bti_code, history_id, object_type_id, object_kind_id, is_land, modify_date, modified_by, kadastr_code, cost_balans, sqr_total, sqr_pidval, sqr_mk, sqr_dk, sqr_rk, sqr_other, sqr_rented, sqr_zagal, sqr_for_rent, sqr_habit, sqr_non_habit, additional_info, is_deleted, del_date, oatuu_id, updpr, arch_id, arch_flag, facade_id, nomer_int, characteristics, expl_enter_year, is_basement_exists, is_loft_exists, sqr_loft, report_id)
	select id, master_building_id, addr_street_name, addr_street_id, addr_street_name2, addr_street_id2, street_full_name, addr_distr_old_id, addr_distr_new_id, addr_nomer1, addr_nomer2, addr_nomer3, addr_nomer, addr_misc, addr_korpus_flag, addr_korpus, addr_zip_code, addr_address, tech_condition_id, date_begin, date_end, num_floors, construct_year, condition_year, is_condition_valid, bti_code, history_id, object_type_id, object_kind_id, is_land, modify_date, modified_by, kadastr_code, cost_balans, sqr_total, sqr_pidval, sqr_mk, sqr_dk, sqr_rk, sqr_other, sqr_rented, sqr_zagal, sqr_for_rent, sqr_habit, sqr_non_habit, additional_info, is_deleted, del_date, oatuu_id, updpr, arch_id, arch_flag, facade_id, nomer_int, characteristics, expl_enter_year, is_basement_exists, is_loft_exists, sqr_loft, report_id
	from #reports1nf_buildings;
	select @new_unique_id = SCOPE_IDENTITY();

	select @agreement_date = agreement_date, @agreement_num = isnull(ltrim(ltrim(agreement_num)),'')
	from [reports1nf_arenda] A where A.id = @arenda_id and A.report_id = @report_id

	set @agreement_date = null
	set @agreement_num = 
		case 
			when @agreement_num like '%-[0-9]' 
				then left(@agreement_num, len(@agreement_num) - 1) + cast(cast(right(@agreement_num,1) as integer) + 1 as varchar(100))
			when @agreement_num like '%-[0-9][0-9]' 
				then left(@agreement_num, len(@agreement_num) - 2) + cast(cast(right(@agreement_num,2) as integer) + 1 as varchar(100))
			else @agreement_num + '-1'
		end


	insert into [reports1nf_arenda](id, building_id, org_balans_id, org_renter_id, org_giver_id, balans_id, rent_year, object_kind_id, purpose_group_id, purpose_id, purpose_str, name, is_privat, update_src_id, agreement_kind_id, agreement_date, agreement_num, agreement_str, floor_number, num_people, cost_narah, cost_payed, cost_debt, cost_agreement, cost_expert_1m, cost_expert_total, pidstava, pidstava_date, pidstava_num, pidstava_fact, pidstava2, pidstava_num2, pidstava_date2, pidstava_display, rent_start_date, rent_finish_date, rent_actual_finish_date, rent_rate, rent_rate_uah, rent_square, priznak_1nf, debt_timespan, order_num, order_date, order_no2, rishennya_id, is_inactive, inactive_date, is_deleted, del_date, date_expert, is_subarenda, privat_kind_id, num_primirnikiv, date_expl_enter, num_akt, date_akt, num_bti, date_bti, svidotstvo_serial, svidotstvo_num, svidotstvo_date, payment_type_id, arch_id, modified_by, modify_date, note, agreement_state, is_insured, insurance_start, insurance_end, insurance_sum, report_id, building_1nf_unique_id, submit_date, is_loan_agreement, is_valid, validation_errors) 
	select @newAgreementId, building_id, org_balans_id, org_renter_id, org_giver_id, balans_id, rent_year, object_kind_id, purpose_group_id, purpose_id, purpose_str, name, is_privat, update_src_id, agreement_kind_id, @agreement_date, @agreement_num, agreement_str, floor_number, num_people, cost_narah, cost_payed, cost_debt, cost_agreement, cost_expert_1m, cost_expert_total, pidstava, pidstava_date, pidstava_num, pidstava_fact, pidstava2, pidstava_num2, pidstava_date2, pidstava_display, rent_start_date, rent_finish_date, rent_actual_finish_date, rent_rate, rent_rate_uah, rent_square, priznak_1nf, debt_timespan, order_num, order_date, order_no2, rishennya_id, is_inactive, inactive_date, is_deleted, del_date, date_expert, is_subarenda, privat_kind_id, num_primirnikiv, date_expl_enter, num_akt, date_akt, num_bti, date_bti, svidotstvo_serial, svidotstvo_num, svidotstvo_date, payment_type_id, arch_id, @modified_by, @modify_date, note, agreement_state, is_insured, insurance_start, insurance_end, insurance_sum, report_id, @new_unique_id, null, is_loan_agreement, is_valid, validation_errors
	from [reports1nf_arenda] A where A.id = @arenda_id and A.report_id = @report_id

	insert into [reports1nf_arenda_payments] (report_id, arenda_id) VALUES (@report_id, @newAgreementId)

	insert into [reports1nf_arenda_decisions](arenda_id, rishen_id, ord, modified_by, modify_date, doc_num, doc_date, doc_dodatok, doc_punkt, purpose_str, rent_square, decision_id, doc_raspor_id, pidstava, report_id)
	select @newAgreementId, rishen_id, ord, modified_by, modify_date, doc_num, doc_date, doc_dodatok, doc_punkt, purpose_str, rent_square, decision_id, doc_raspor_id, pidstava, report_id
	from [reports1nf_arenda_decisions] A where A.arenda_id = @arenda_id and A.report_id = @report_id

	insert into [reports1nf_arenda_notes](arenda_id, purpose_group_id, purpose_id, purpose_str, rent_square, modify_date, modified_by, note, rent_rate, rent_rate_uah, cost_narah, cost_agreement, is_deleted, del_date, cost_expert_total, date_expert, payment_type_id, invent_no, note_status_id, report_id)
	select @newAgreementId, purpose_group_id, purpose_id, purpose_str, rent_square, modify_date, modified_by, note, rent_rate, rent_rate_uah, cost_narah, cost_agreement, is_deleted, del_date, cost_expert_total, date_expert, payment_type_id, invent_no, note_status_id, report_id
	from [reports1nf_arenda_notes] A where A.arenda_id = @arenda_id and A.report_id = @report_id

	set @new_arenda_id = @newAgreementId;
	select @newAgreementId as newAgreementId;
END
