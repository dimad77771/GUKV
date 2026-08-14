CREATE PROCEDURE [dbo].[update_inflation_prognoz]
AS
BEGIN

delete from inflation_year_prognoz

insert into inflation_year_prognoz
select
period_year + B.vv as yy,
B.target_year_inflation
from (SELECT Year(DATEADD(day, 1, period_end)) as period_year FROM dict_rent_period QQ where QQ.is_active = 1) A
cross apply 
(
	select 0 as vv, isnull(prognoz_inflation_1,100.0) as target_year_inflation from [current_inflation] union 
	select 1 as vv, isnull(prognoz_inflation_2,100.0) as target_year_inflation from [current_inflation] union 
	select 2 as vv, isnull(prognoz_inflation_3,100.0) as target_year_inflation from [current_inflation] union 
	select 3 as vv, isnull(prognoz_inflation_4,100.0) as target_year_inflation from [current_inflation]
) B 

delete from inflation_month_prognoz

insert into inflation_month_prognoz(year,month,inflation) 
select
B.year,
C.*
from (SELECT Year(DATEADD(day, 1, period_end)) as period_year FROM dict_rent_period QQ where QQ.is_active = 1) A
cross apply inflation_year_prognoz B
cross apply get_inflation_year_prognoz(B.year, B.inflation) C

END