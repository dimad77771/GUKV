CREATE VIEW [dbo].[arenda_payment_problems] as 
select
*
from
(
	select
	case when 
		rent_period_id = active_rent_period_id 
			and
		(
			isnull(nar_sum,0) <> isnull(payment_narah,0)
				or
			isnull(pay_1,0) <> isnull(payment_nar_zvit,0)
				or
			isnull(pay_2,0) <> isnull(avance_paymentnar,0)
				or
			isnull(pay_3,0) <> isnull(old_debts_payed,0)
				or
			isnull(pay_4,0) <> isnull(return_orend_payed,0)
		)
	then 1 else 0 end as is_problem,
	*
	from
	(
		select
		N.arenda_id, N.report_id,

		R.rent_period_id,
		T.id active_rent_period_id,

		(select sum(A.narah_sum) from reports1nf_payment_narahcalc A where A.report_id = N.report_id and A.arenda_id = N.arenda_id and A.narah_date >= start_dd and A.narah_date <= start_ee) nar_sum,
		R.payment_narah,

		W.pay_1, R.payment_nar_zvit,
		W.pay_2, R.avance_paymentnar,
		W.pay_3, old_debts_payed,
		W.pay_4, return_orend_payed


	
		from 
		(
			SELECT 
			cast(concat(period_year,'-','1','-1') as date) start_dd,
			cast(concat(period_year,'-',period_quarter,'-1') as date) start_ee,
			* 
			FROM dict_rent_period A
			where A.is_active = 1
		) T
		cross apply
		(
			--		select 83159 as arenda_id, 209 as report_id
			select distinct arenda_id, report_id, rent_period_id from reports1nf_arenda_payments Q where Q.rent_period_id = T.id 
		) N
		cross apply
		(
			SELECT 
				sum(Q.payment_sm_1) pay_1,
				sum(Q.payment_sm_2) pay_2,
				sum(Q.payment_sm_3) pay_3,
				sum(Q.payment_sm_4) pay_4
			FROM reports1nf_payment_documents Q 
			WHERE Q.report_id = N.report_id and Q.arenda_id = N.arenda_id and Q.rent_period_id in (select Z.id from dict_rent_period Z where Z.period_year = T.period_year)
		) W
		cross apply
		(
			select 
				payment_nar_zvit,
				avance_paymentnar,
				old_debts_payed,
				return_orend_payed,

				payment_narah,
				rent_period_id
			from reports1nf_arenda_payments Q 
			where Q.report_id = N.report_id and Q.arenda_id = N.arenda_id and Q.rent_period_id = T.id
		) R
	) G
) G
--where is_problem = 1
--select * from reports1nf_payment_narahcalc A where A.report_id = N.report_id and A.arenda_id = N.arenda_id



--select [dbo].[IsArendaPaymentsProblem](arenda_id, report_id) uu, arenda_id, report_id from reports1nf_arenda_payments Q where Q.rent_period_id = 64 and Q.report_id = 209 and Q.arenda_id = 83129