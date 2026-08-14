


-- =============================================
-- Author:		<Ершов, pgv>
-- Create date: <09.06.2017>
-- Modify date:
--
-- Description:	<Замена кодов организаций в таблице org_by_period с проверкой на появление дублей записей.>

-- Example:
/*
	begin tran
		exec repair_org_by_period @new_org_id = 1145, @update_org_id_list = '301651, 401144, 401145'
		select * from org_by_period where period_id = 31 order by period_id, org_id 
	rollback tran
*/	
-- =============================================
CREATE PROCEDURE [dbo].[repair_org_by_period]
	 @new_org_id int = null						-- id-код организации, на который нужно заменить
	,@update_org_id_list nvarchar(max) = null	-- список заменяемых id-кодов
	,@period_id int = 31						-- id-код активного периода
AS
BEGIN
	SET NOCOUNT ON;

	declare @org_occupation_id int = null
	
	if exists (select 1 from dbo.org_by_period where period_id = @period_id and org_id = @new_org_id)
		delete from dbo.org_by_period where period_id = @period_id and org_id in (select * from dbo.efn_split_string(@update_org_id_list, ','))
	else
		begin
			set @org_occupation_id = (select top 1 org_occupation_id from dbo.org_by_period where period_id = @period_id and org_id in (select * from dbo.efn_split_string(@update_org_id_list, ',')))
			if @org_occupation_id is not null
				begin
					insert into dbo.org_by_period (period_id, org_occupation_id, org_id) values (@period_id, @org_occupation_id, @new_org_id)
					delete from dbo.org_by_period where period_id = @period_id and org_id in (select * from dbo.efn_split_string(@update_org_id_list, ','))
				end
			else 
				insert into dbo.org_by_period values (@period_id, 11, @new_org_id)
		end

	--declare @crs_period_id int, @crs_org_occupation_id int, @crs_org_id int
	--declare crs cursor for select period_id, org_occupation_id, org_id from dbo.org_by_period where period_id = @period_id and org_id in (select * from dbo.efn_split_string(@update_org_id_list, ','))
	--open crs
	--fetch next from crs into @crs_period_id, @crs_org_occupation_id, @crs_org_id
	--while @@fetch_status = 0
	--begin
	--	if not exists (select 1 from dbo.org_by_period where period_id = @crs_period_id and org_occupation_id = @crs_org_occupation_id and org_id = @new_org_id)
	--		update dbo.org_by_period set org_id = @new_org_id where period_id = @crs_period_id and org_occupation_id = @crs_org_occupation_id and org_id = @crs_org_id
	--	else
	--		delete from dbo.org_by_period where period_id = @crs_period_id and org_occupation_id = @crs_org_occupation_id and org_id = @crs_org_id
		
	--	fetch next from crs into @crs_period_id, @crs_org_occupation_id, @crs_org_id
	--end
	--close crs
	--deallocate crs

END



