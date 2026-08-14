
-- =============================================
-- Author:		<>
-- Create date: <13.07.2017>
-- Modify date:
--
-- Description:	<Замена кодов организаций в таблицах с проверкой на появление дублей записей.>

-- Example:
/*
	begin tran
		exec update_org_list @new_org_id = 1145, @update_org_list = '301651, 401144, 401150'
		select * from org_by_period where period_id = 31 order by period_id, org_id
	rollback tran
*/	
-- =============================================
CREATE PROCEDURE [dbo].[update_org_list]
	 @new_org_id int = null						-- id-код организации, на который нужно заменить
	,@update_org_list nvarchar(max) = null		-- список заменяемых id-кодов
	,@period_id int = 31						-- id-код активного периода
AS
BEGIN
	SET NOCOUNT ON


--------------------------------------------------------------
-- замена организаций в таблицах

if @new_org_id is null or @update_org_list is null
	begin
		print 'Порожні списки кодів'
		Return
	end
if exists (select 1 where @new_org_id in (select * from dbo.efn_split_string(@update_org_list, ',')))
	begin
		print 'Код, що залишаємо, не повинен бути у списку видалених'
		Return
	end

print 'start ...'


delete from dbo.reports1nf
--set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.reports1nf_accounts
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.reports1nf_arenda
set org_balans_id = @new_org_id
where org_balans_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.reports1nf_arenda
set org_renter_id = @new_org_id
where org_renter_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.reports1nf_arenda
set org_giver_id = @new_org_id
where org_giver_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.reports1nf_arenda_rented
set org_renter_id = @new_org_id
where org_renter_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.reports1nf_arenda_rented
set org_giver_id = @new_org_id
where org_giver_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.reports1nf_balans
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.reports1nf_balans
set vidch_org_id = @new_org_id
where vidch_org_id in (select * from dbo.efn_split_string(@update_org_list, ','))  

update dbo.reports1nf_balans
set org_maintain_id = @new_org_id
where org_maintain_id in (select * from dbo.efn_split_string(@update_org_list, ','))  

update dbo.reports1nf_balans_deleted
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.reports1nf_comments
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 


--------------------------------------------------------------
update dbo.arenda
set org_balans_id = @new_org_id
where org_balans_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.arenda
set org_renter_id = @new_org_id
where org_renter_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.arenda
set org_giver_id = @new_org_id
where org_giver_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.arenda_decisions
set org_balans_id = @new_org_id
where org_balans_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.arenda_decisions
set org_renter_id = @new_org_id
where org_renter_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.arenda_decisions
set org_giver_id = @new_org_id
where org_giver_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.balans
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.balans
set vidch_org_id = @new_org_id
where vidch_org_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.balans
set org_maintain_id = @new_org_id
where org_maintain_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.balans_other
set org_id = @new_org_id
where org_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.building_docs
set org_id = @new_org_id
where org_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.building_docs
set last_org_id = @new_org_id
where last_org_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.expert_note
set org_balans_id = @new_org_id
where org_balans_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.expert_note
set org_renter_id = @new_org_id
where org_renter_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.arenda_applications
set org_renter_id = @new_org_id
where org_renter_id in (select * from dbo.efn_split_string(@update_org_list, ','))



---------------------------------------------------------------------------


update dbo.arch_arenda
set org_balans_id = @new_org_id
where org_balans_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.arch_arenda
set org_renter_id = @new_org_id
where org_renter_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.arch_arenda
set org_giver_id = @new_org_id
where org_giver_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.arch_balans
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.arch_balans
set vidch_org_id = @new_org_id
where vidch_org_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.arch_balans
set org_maintain_id = @new_org_id
where org_maintain_id in (select * from dbo.efn_split_string(@update_org_list, ','))

-------------------------------------------------------------------------------


update dbo.fin_budget_rate_by_org
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.fin_form_changes
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.fin_report_formula_values
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.fin_report_values
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.object_rights
set org_from_id = @new_org_id
where org_from_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.object_rights
set org_to_id = @new_org_id
where org_to_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.org_docs
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.privatization
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))


update dbo.freecycle_orendar
set org_orendar_id = @new_org_id
where org_orendar_id in (select * from dbo.efn_split_string(@update_org_list, ','))


update dbo.transfer_requests
set org_from_id = @new_org_id
where org_from_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.transfer_requests
set org_to_id = @new_org_id
where org_to_id in (select * from dbo.efn_split_string(@update_org_list, ','))

update dbo.transfer_requests
set org_id_for_confirm = @new_org_id
where org_id_for_confirm in (select * from dbo.efn_split_string(@update_org_list, ','))



--------------------------------------------------------------

update dbo.out_of_city_organizations
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))

if exists (select organization_id from dbo.user_notification_settings where organization_id = @new_org_id)
	delete from dbo.user_notification_settings
		where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))
else
	begin
		update top (1) dbo.user_notification_settings
			set organization_id = @new_org_id
			where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))
		delete from dbo.user_notification_settings
			where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))
	end

--------------------------------------------------------------

if exists (select organization_id from dbo.rent_balans_org where organization_id = @new_org_id)
	delete from dbo.rent_balans_org
		where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))
else
	begin
		update top (1) dbo.rent_balans_org
			set organization_id = @new_org_id
			where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))
		delete from dbo.rent_balans_org
			where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))
	end


update dbo.rent_free_square
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.rent_payment
set org_balans_id = @new_org_id
where org_balans_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

----update dbo.rent_payment_by_renter
----set rent_renter_org_id = @new_org_id
----where rent_renter_org_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.rent_payment_by_renter
set renter_organization_id = @new_org_id
where renter_organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.rent_payment_by_renter
set giver_organization_id = @new_org_id
where giver_organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

----update dbo.rent_payment_debt
----set rent_renter_org_id = @new_org_id
----where rent_renter_org_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.rent_payment_debt
set giver_organization_id = @new_org_id
where giver_organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.rent_renter_org
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.rent_vipiski
set organization_id = @new_org_id
where organization_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

update dbo.rent_vipiski
set org_balans_id = @new_org_id
where org_balans_id in (select * from dbo.efn_split_string(@update_org_list, ',')) 

--------------------------------------------------------------

--update dbo.m_report_balans_and_arenda
--set organization_id = @new_org_id
--where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))

--update dbo.m_view_rent_debt_for_arenda
--set organization_id = @new_org_id
--where organization_id in (select * from dbo.efn_split_string(@update_org_list, ','))

--------------------------------------------------------------


-- замена организаций в таблице org_by_period
exec update_org_id_in_org_by_period @new_org_id = @new_org_id, @update_org_id_list = @update_org_list

--процедура удаления организаций
--exec switch_deleted_record_links 'dbo.organizations', @false_id_list = @update_org_list, @true_id = @new_org_id

delete from dbo.organizations
	where id in (select * from dbo.efn_split_string(@update_org_list, ','))


print 'Дублі видалено.'

END



