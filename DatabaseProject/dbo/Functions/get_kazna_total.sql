
CREATE function [dbo].[get_kazna_total] ( @zkpo varchar(1000), @db datetime, @de datetime)
returns decimal(18, 2)
as 
BEGIN 
	declare @rez decimal(18, 2);

	select @rez = A.pay_sum
	from kazna_total_info(@db, @de) A
	where A.ident_bal_zkpo = @zkpo


	return isnull(@rez,0);
END
