// select *, case when T.grp_1 = 'Невідомо' then 999 else 1 end as grp_1_ord, case when T.grp_1 like '% район' then 'Р' else 'М' end as grp_2 into #repall from ( 	select  	case when A.corrpay in (SELECT Q.corrpay from exchequer.lookup_corrpay Q where Q.corrpay_name  like '%від%прибутку%згідно%угоди%') then 1 else 0 end as in_ugoda, 	case when A.ident_bal_zkpo <> '' then A.ident_bal_zkpo else '999999999999' end as p_zkpo,  	case when A.ident_bal_zkpo <> '' then B.bal_name else 'БАЛАНСОУТРИМУВАЧ НЕВІДОМИЙ' end as p_name,  	A.pay_sum,  	A.pay_date, 	isnull(( 		select  		occ.name  		from org_by_period obp 		join dict_rent_period per on per.id = obp.period_id and per.is_active = 1 		join dict_rent_occupation occ on occ.id = obp.org_occupation_id 		join reports1nf_org_info org on org.id = obp.org_id 		where org.zkpo_code = A.ident_bal_zkpo 	),'Невідомо') as grp_1, 	case when A.is_return = 1 then '-' else '+' end as grp_9 	from exchequer.payments A 	left join exchequer.lookup_1 B on B.bal_zkpo = A.ident_bal_zkpo 	left join exchequer.lookup_corrpay C on C.corrpay = A.corrpay 	where isnull(A.rowstatus,'') <> '0' ) as T where 1=1 and pay_date >= '2019-07-01' and pay_date <= '2019-12-31'
//;

//select p_zkpo,p_name,pay_date, grp_9,grp_2,grp_1,grp_1_ord, sum(pay_sum) psum into #rep1 from #repall group by p_zkpo,p_name,pay_date, grp_9,grp_2,grp_1,grp_1_ord;

//select in_ugoda,pay_date, sum(pay_sum) psum into #rep_ugoda from #repall where grp_9 = '+' group by in_ugoda, pay_date;

//select *, in_ugoda,pay_date, psum from #rep_ugoda where in_ugoda = 1;

//select in_ugoda,pay_date, sum(pay_sum) psum from #repall where grp_9 = '+' group by in_ugoda, pay_date;
//select * from #repall;

//select p_zkpo,p_name,pay_date, sum(pay_sum) psum from #repall where grp_9 = '+' and in_ugoda = 1 group by p_zkpo,p_name,pay_date;

select 
	yy, mm, 
	sum(pay_sum) psum, 
	sum(case when in_ugoda = 0 then pay_sum else 0 end) psum_0,
	sum(case when in_ugoda = 1 then pay_sum else 0 end) psum_1
from (select year(pay_date) yy, month(pay_date) mm, in_ugoda, pay_sum from #repall) T 
group by yy, mm 
order by 1,2
;