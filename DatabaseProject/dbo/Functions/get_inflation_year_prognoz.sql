CREATE FUNCTION [dbo].[get_inflation_year_prognoz](@year integer, @target_year_inflation decimal(15,3))
RETURNS TABLE 
AS
RETURN
WITH M AS
(
	select
	--(select exp(sum(log(inflation / 100.0))) from O) total,
	B.inflation inflation_1,
	Y.yy, 
	A.month,
	A.inflation inflation_0,
	target_inflation
	from 
	(
		select @year yy, @target_year_inflation as target_inflation
	) Y
	cross apply (select * from inflation_month where year in (select top 1 year from inflation_month A group by year having count(*) = 12 order by year desc)) A
	left join inflation_month B on B.year = Y.yy and B.month = A.month
),
T as
(
	select
	isnull(exp(sum(log(case when inflation_1 is     null then inflation_0 end / 100.0))),1.0) sinf_0,
	isnull(exp(sum(log(case when inflation_1 is not null then inflation_1 end / 100.0))),1.0) sinf_1,
	sum(case when inflation_1 is null then 1 else 0 end) as neiz_count,
	(select top 1 target_inflation / 100.0 from M) as target
	from M
)

select
[month], [prognoz_inflation]
from
(
	select
	--(select POWER(target / inf_0 * inf_1 , 1.0 / neiz_count) from T),
	case when inflation_1 is null then inflation_0 * (select POWER(target / (sinf_1 * sinf_0), 1.0 / neiz_count) from T) end as prognoz_inflation,
	*
	--into #qq4
	from M
) R
where prognoz_inflation is not null
