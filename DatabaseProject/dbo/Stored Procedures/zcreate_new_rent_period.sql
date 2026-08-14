
-- =============================================
-- Author:		<pgv>
-- Create date: <05.11.2018>
-- Modify date:
--
-- Description:	Створення нового звітного періода

-------------------------------------------------------------------
CREATE PROCEDURE [dbo].[zcreate_new_rent_period]
AS
BEGIN
	SET NOCOUNT ON;

declare	 @new_id int = null
    ,@new_name varchar(255) = null
    ,@new_period_start date = null
    ,@new_period_end date = null
    ,@new_period_quarter int = null
    ,@new_period_year int = null
    ,@new_is_active int = 1
	,@monn varchar(9) = ' місяців '
	,@day varchar(2) = '30'

	if exists (select 1 from dbo.dict_rent_period where is_active = 1)
		begin
			set @new_id = 1 + (select id from dbo.dict_rent_period where is_active = 1)
			set @new_period_quarter = 3 + (select period_quarter from dbo.dict_rent_period where is_active = 1)
			if @new_period_quarter > 12
				begin 
					set @new_period_quarter = 3
					set @monn = ' місяці '
				end
			set @new_period_year = (select period_year from dbo.dict_rent_period where is_active = 1)
			if @new_period_quarter = 3 
				set @new_period_year = @new_period_year + 1
			set @new_name = cast(@new_period_quarter as varchar(2)) + @monn + CAST(@new_period_year as varchar(4))

			set @new_period_start = CAST(@new_period_year as varchar(4)) + '/'+ CAST(@new_period_quarter - 2 as varchar(2))+ '/01' 
			if @new_period_quarter = 3 or @new_period_quarter = 12
				set @day = '31'
			set @new_period_end = CAST(@new_period_year as varchar(4)) + '/'+ CAST(@new_period_quarter as varchar(2))+ '/' + @day


			update dbo.dict_rent_period set is_active = 0 where id = @new_id - 1
  
			insert into dbo.dict_rent_period (id, name, period_start, period_end, period_quarter, period_year, is_active)
			values (@new_id, @new_name, @new_period_start, @new_period_end, @new_period_quarter, @new_period_year, @new_is_active )

-- перенесення сфери управління з попереднього періоду
			insert into [dbo].[org_by_period] ([period_id],[org_occupation_id],[org_id])
			SELECT  @new_id,[org_occupation_id],[org_id] FROM [dbo].[org_by_period] where period_id = @new_id - 1

	
		end		

END



