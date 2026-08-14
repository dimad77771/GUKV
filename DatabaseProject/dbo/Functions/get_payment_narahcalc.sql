CREATE   function [dbo].[get_payment_narahcalc] ( @arenda_id integer, @report_id integer, @ignore_reports1nf_arenda_payments integer)
returns [numeric](15, 2)
as 
BEGIN 
	declare @ret [numeric](15, 2);

	select 
		@ret = sum(A.narah_sum)
	from reports1nf_payment_narahcalc A 
	,
	(
		select 
			case when isnull(use_last_december,0) = 0 then db0 else DATEADD(MONTH,-1, db0) end as db,
			case when isnull(use_last_december,0) = 0 then de0 else DATEADD(MONTH,-1, de0) end as de
		from
		(
			SELECT 
				cast(concat(period_year,'01','01') as date) db0, 
				DATEADD(month,period_quarter - 1,cast(concat(period_year,'01','01') as date)) de0, 
				--1 as use_last_december,
				(select top 1 Q.use_last_december from reports1nf_arenda_payments Q where Q.arenda_id = @arenda_id and Q.report_id = @report_id and Q.rent_period_id in (SELECT QQ.id FROM dict_rent_period QQ where QQ.is_active = 1)) as use_last_december,
				* 
			FROM dict_rent_period QQ where QQ.is_active = 1
		) T
	) B
	where arenda_id = @arenda_id and report_id = @report_id
	and A.narah_date >= B.db and A.narah_date <= B.de
	and 
	(
		@ignore_reports1nf_arenda_payments = 1
			or
		exists (select * from reports1nf_arenda_payments Q where Q.arenda_id = @arenda_id and Q.report_id = @report_id and Q.rent_period_id in (SELECT QQ.id FROM dict_rent_period QQ where QQ.is_active = 1))
	)

	return @ret;
END
