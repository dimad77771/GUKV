CREATE   FUNCTION [dbo].[narah_sum_by_monthes]
(	
	@val numeric(15, 3), @perc numeric(15, 5), @mon_count int
)
RETURNS @OUTPUT TABLE (mnum int not null, psum decimal(18, 2) not null)
AS
BEGIN
	declare @mnum int = 1;

	while(@mnum <= @mon_count)
	begin
		insert into @OUTPUT(mnum, psum) values(@mnum, @val)

		set @val = round(@val * (1 + @perc / 100.0),2)

		set @mnum = @mnum + 1
	end
	

	RETURN
END
