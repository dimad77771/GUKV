CREATE   FUNCTION [dbo].[bigborg_arenda](@email varchar(1000))
RETURNS @OUTPUT TABLE (arenda_id integer, big_month_koef decimal(18,2))
AS
BEGIN

if @email = '-' RETURN;

insert into @OUTPUT
select
U.id, big_month_koef
from arenda U
cross apply
(
	select
	isnull(borg_now,0) / case when moth_plata > 0 then moth_plata end as big_month_koef
	from
	(
		select 
			narah_1 + narah_2 + narah_3
				+
			isnull((select debt_total from gukv2016_arenda_payments where arenda_id = U.id and rent_period_id = T.last_rent_period_id),0)
				-
			isnull(
			(select sum(isnull(Q.payment_sm_1,0) + isnull(Q.payment_sm_3,0)) from reports1nf_payment_documents Q 
				where Q.arenda_id = U.id and Q.rent_period_id = T.active_rent_period_id
			),0) as borg_now,

			isnull((select sum(cost_agreement) from arenda_notes where arenda_id = U.id and (is_deleted IS NULL OR is_deleted = 0)),0) as moth_plata 
		from
		(
			select
			(select Q.narah_sum from reports1nf_payment_narahcalc Q where Q.arenda_id = U.id and Q.narah_date = T.pdat_1) * case when today >= dat_1 then 1 else 0 end as narah_1,
			(select Q.narah_sum from reports1nf_payment_narahcalc Q where Q.arenda_id = U.id and Q.narah_date = T.pdat_2) * case when today >= dat_2 then 1 else 0 end as narah_2,
			(select Q.narah_sum from reports1nf_payment_narahcalc Q where Q.arenda_id = U.id and Q.narah_date = T.pdat_3) * case when today >= dat_3 then 1 else 0 end as narah_3,
			case when today >= dat_2 then 1 else 0 end plus_2,
			case when today >= dat_3 then 1 else 0 end plus_3,
			*
			from
			(
				SELECT 
				top 1 

				getdate() today,
				--cast('2024-12-22 17:23:12' as date) today,

				DATEADD(month, 0, Q.period_start) pdat_1,
				DATEADD(month, 1, Q.period_start) pdat_2,
				DATEADD(month, 2, Q.period_start) pdat_3,
				DATEADD(day,22 - 1,DATEADD(month, 1, Q.period_start)) dat_1,
				DATEADD(day,22 - 1,DATEADD(month, 2, Q.period_start)) dat_2,
				DATEADD(day,22 - 1,DATEADD(month, 3, Q.period_start)) dat_3,
		
				Q.id active_rent_period_id,
				(SELECT top 1 QQ.id FROM dict_rent_period QQ where QQ.is_active = 0 order by 1 desc) last_rent_period_id

				FROM dict_rent_period Q
				where Q.is_active = 1
			) T
		) T
	) T
) G
where 1=1
and big_month_koef > 4.0
and
(
	isnull(@email,'') = ''
		OR
	U.orandodavec_user_id in (select Q.id from dict_orandodavec_user Q where Q.email = @email)
)



RETURN
END
