CREATE FUNCTION [dbo].[kazna_total_info]
(	
	@db datetime, @de datetime
)
RETURNS @OUTPUT TABLE (ident_bal_zkpo varchar(100), [pay_sum] decimal(18, 2) NULL)
AS
BEGIN
	if (@db is null)
	begin
		select
			@db = (SELECT top 1 cast(cast(Q.period_year as varchar(10)) + '0101' as date) FROM dict_rent_period Q order by id desc), 
	        @de = (SELECT top 1 Q.period_end FROM dict_rent_period Q order by id desc);
	end

	insert into @OUTPUT
	select  	
		ident_bal_zkpo, 
		sum(case when A.is_return = 1 then -1 else 1 end * A.pay_sum)
	from exchequer.payments A 	
	left join exchequer.lookup_1 B on B.bal_zkpo = A.ident_bal_zkpo 	
	left join exchequer.lookup_corrpay C on C.corrpay = A.corrpay 	
	where 1=1
	and isnull(A.rowstatus,'') <> '0' 
	and pay_date >= @db and pay_date <= @de 	
	--and isnull(A.is_return,0) <> 1
	and ident_bal_zkpo <> ''
	group by ident_bal_zkpo

	RETURN
END