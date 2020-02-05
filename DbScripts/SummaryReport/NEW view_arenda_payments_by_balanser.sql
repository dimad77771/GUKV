USE [GUKV]
GO

/****** Object:  Table [dbo].[org_by_period]    Script Date: 25.11.2016 20:04:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[org_by_period](
	[period_id] [int] NOT NULL,
	[org_occupation_id] [int] NOT NULL,
	[org_id] [int] NOT NULL,
 CONSTRAINT [PK_org_by_period] PRIMARY KEY CLUSTERED 
(
	[org_id] ASC,
	[org_occupation_id] ASC,
	[period_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [GUKV]
GO

/****** Object:  Table [dbo].[DictSummaryReportSphere]    Script Date: 25.11.2016 20:05:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DictSummaryReportSphere](
	[id] [int] NOT NULL,
	[name] [varchar](150) NOT NULL,
 CONSTRAINT [PK_DictSummaryReportSphere] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


---------------------------------------------------------------add arenda_archive_link_code to arenda_payments



UPDATE arenda_payments SET arenda_archive_link_code = (select max(archive_link_code) from arch_arenda where arch_arenda.id = arenda_payments.arenda_id)
update arenda_payments set balans_org_id = (select org_balans_id from arenda where arenda.id = arenda_payments.arenda_id)

delete from org_by_period
insert into org_by_period (period_id,org_id,org_occupation_id)
select p.id,rbo.organization_id,rbo.rent_occupation_id 
from rent_balans_org rbo
left outer join dict_rent_period p on p.id > 0

delete from DictSummaryReportSphere
insert into DictSummaryReportSphere (id, name)
select * from [dbo].[dict_rent_occupation]















IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_arenda_payments_by_balanser]'))
DROP VIEW [dbo].[view_arenda_payments_by_balanser]

GO

CREATE VIEW [view_arenda_payments_by_balanser]
AS

select 
ap.rent_period_id,
per.name as 'rent_period_name',
occ.id as 'occupation_id',
occ.name as 'occupation_name',
count(ap.id) as 'arenda_count',
sum(ap.[sqr_total_rent]) as 'sqr_total_rent',
sum(ap.[sqr_payed_by_percent]) as 'sqr_payed_by_percent',
sum(ap.[sqr_payed_by_1uah]) as 'sqr_payed_by_1uah',
sum(ap.[sqr_payed_hourly]) as 'sqr_payed_hourly',
sum(ap.[payment_narah]) as 'payment_narah',
sum(ap.[last_year_saldo]) as 'last_year_saldo',
sum(ap.[payment_received]) as 'payment_received',
sum(ap.[payment_nar_zvit]) as 'payment_nar_zvit',
sum(ap.[payment_budget_special]) as 'payment_budget_special',
sum(ap.[debt_total]) as 'debt_total',
sum(ap.[debt_zvit]) as 'debt_zvit',
sum(ap.[debt_3_month]) as 'debt_3_month',
sum(ap.[debt_12_month]) as 'debt_12_month',
sum(ap.[debt_3_years]) as 'debt_3_years',
sum(ap.[debt_over_3_years]) as 'debt_over_3_years',
sum(ap.[debt_v_mezhah_vitrat]) as 'debt_v_mezhah_vitrat',
sum(ap.[debt_spysano]) as 'debt_spysano',
sum(ap.[num_zahodiv_total]) as 'num_zahodiv_total',
sum(ap.[num_zahodiv_zvit]) as 'num_zahodiv_zvit',
sum(ap.[num_pozov_total]) as 'num_pozov_total',
sum(ap.[num_pozov_zvit]) as 'num_pozov_zvit',
sum(ap.[num_pozov_zadov_total]) as 'num_pozov_zadov_total',
sum(ap.[num_pozov_zadov_zvit]) as 'num_pozov_zadov_zvit',
sum(ap.[num_pozov_vikon_total]) as 'num_pozov_vikon_total',
sum(ap.[num_pozov_vikon_zvit]) as 'num_pozov_vikon_zvit',
sum(ap.[debt_pogasheno_total]) as 'debt_pogasheno_total',
sum(ap.[debt_pogasheno_zvit]) as 'debt_pogasheno_zvit',
sum(ap.[budget_narah_50_uah]) as 'budget_narah_50_uah',
sum(ap.[budget_zvit_50_uah]) as 'budget_zvit_50_uah',
sum(ap.[budget_prev_50_uah]) as 'budget_prev_50_uah',
sum(ap.[budget_debt_50_uah]) as 'budget_debt_50_uah',
sum(ap.[budget_debt_30_50_uah]) as 'budget_debt_30_50_uah',
sum(ap.[old_debts_payed]) as 'old_debts_payed'
from arenda_payments ap
join arch_arenda a on ap.arenda_id=a.id and a.archive_link_code = ap.arenda_archive_link_code and a.id = ap.arenda_id
join org_by_period obp on obp.period_id = ap.rent_period_id and obp.org_id = a.org_balans_id
join dict_rent_period per on per.id = ap.rent_period_id
join dict_rent_occupation occ on occ.id=obp.org_occupation_id
where 
(a.agreement_state = 1 ) and (a.is_deleted is null OR a.is_deleted = 0)
and a.org_balans_id = 1 and ap.rent_period_id = 29 
group by
ap.rent_period_id,
per.name,
occ.id,
occ.name