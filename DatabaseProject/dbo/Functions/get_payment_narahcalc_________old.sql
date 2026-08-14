CREATE   function [dbo].[get_payment_narahcalc_________old] ( @arenda_id integer, @report_id integer, @ignore_reports1nf_arenda_payments integer)
--ALTER   function [dbo].[get_payment_narahcalc] ( @arenda_id integer, @report_id integer, @ignore_reports1nf_arenda_payments integer)
returns [numeric](15, 2)
as 
BEGIN 
	declare @ret [numeric](15, 2);

	select 
		@ret = sum(A.narah_sum)
	from reports1nf_payment_narahcalc A 
	,(SELECT cast(concat(period_year,'01','01') as date) db, DATEADD(month,period_quarter - 1,cast(concat(period_year,'01','01') as date)) de, * FROM dict_rent_period QQ where QQ.is_active = 1) B
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


