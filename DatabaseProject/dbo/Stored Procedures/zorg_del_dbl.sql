
-- =============================================
-- Example:
/*
	begin tran
		exec zorg_del_dbl
		select * from tmp_dbl where mark = '*' 
	rollback tran
*/	
-- =============================================
CREATE PROCEDURE [dbo].[zorg_del_dbl]
AS
BEGIN
	SET NOCOUNT ON;

	declare 
		@id int=null
	, @act_date datetime = null
	
	declare crs cursor for select cast(idc_org as int) as id, act_date from [dbo].[tmp_dbl] where mark = '*' and act_date is null
	
	open crs
	fetch next from crs into @id, @act_date

	while @@fetch_status = 0

/*b0*/	begin
print @id

		exec dbo.repare_org_list @new_org_id = @id

-- позначаємо запис як оброблений
		update dbo.tmp_dbl set act_date = getdate() where cast(idc_org as int) = @id and mark='*'
	 
		fetch next from crs into @id, @act_date

/*e0*/	end

	close crs
	deallocate crs

END
