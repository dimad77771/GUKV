
CREATE function [dbo].[get_kazna_total____backup] ( @zkpo varchar(1000), @db datetime, @de datetime)
returns decimal(18, 2)
as 
BEGIN 
	declare @rez decimal(18, 2);

	declare @repall TABLE(
	[in_ugoda] [int] NOT NULL,
	[p_zkpo] [varchar](100) NULL,
	[p_name] [varchar](8000) NULL,
	[pay_sum] [decimal](18, 2) NULL,
	[pay_date] [datetime] NULL,
	[grp_1] [varchar](255) NOT NULL,
	[grp_9] [varchar](1) NOT NULL,
	[grp_1_ord] [int] NOT NULL,
	[grp_2] [varchar](1) NOT NULL) 

	--return 0;

	if (@db is null)
	begin
		select
			@db = (SELECT top 1 cast(cast(Q.period_year as varchar(10)) + '0101' as date) FROM dict_rent_period Q order by id desc), 
	        @de = (SELECT top 1 Q.period_end FROM dict_rent_period Q order by id desc);
	end


	insert into @repall
    select 
	*, 
	case when T.grp_1 = 'Невідомо' then 999 else 1 end as grp_1_ord, case when T.grp_1 like '% район' then 'Р' else 'М' end as grp_2 
	from 
	( 	
		select  	
			case when A.corrpay in (SELECT Q.corrpay from exchequer.lookup_corrpay Q where Q.corrpay_name  like '%від%прибутку%згідно%угоди%') then 1 else 0 end as in_ugoda, 	
			case when A.ident_bal_zkpo <> '' then A.ident_bal_zkpo else '999999999999' end as p_zkpo,  	
			case when A.ident_bal_zkpo <> '' then B.bal_name else 'БАЛАНСОУТРИМУВАЧ НЕВІДОМИЙ' end as p_name,  	
			A.pay_sum,  	
			A.pay_date, 	
			isnull(
				(select occ.name 
					from org_by_period obp 
					join dict_rent_period per on per.id = obp.period_id and per.is_active = 1 
					join dict_rent_occupation occ on occ.id = obp.org_occupation_id 		
					join reports1nf_org_info org on org.id = obp.org_id 		
					where org.zkpo_code = A.ident_bal_zkpo 	),
			'Невідомо') as grp_1, 	
			case when A.is_return = 1 then '-' else '+' end as grp_9 	
		from exchequer.payments A 	
		left join exchequer.lookup_1 B on B.bal_zkpo = A.ident_bal_zkpo 	
		left join exchequer.lookup_corrpay C on C.corrpay = A.corrpay 	
		where isnull(A.rowstatus,'') <> '0' 
	) as T where 1=1 
	and pay_date >= @db and pay_date <= @de and p_zkpo = @zkpo;
	
	select @rez = sum(A.pay_sum) from @repall A where A.grp_9 = '+';

	return isnull(@rez,0);
END
