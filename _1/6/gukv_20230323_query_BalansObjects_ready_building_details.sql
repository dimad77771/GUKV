select
org_full_name as [Балансоутримувач - Повна Назва],
org_zkpo_code as [Балансоутримувач - Код ЄДРПОУ],
district as [Район],
street_full_name as [Назва Вулиці],
addr_nomer as [Номер Будинку],
orggospupr as [Орган госп. упр.],
total_free_sqr as [Площа вільних приміщень (кв.м.)],
sqr_total as [Площа нежилих приміщень об'єкту (кв.м.)],
sqr_vlas_potreb as [Площа об'єкту Для Власних Потреб (кв.м.)],
sqr_kor as [Корисна площа об'єкту (кв.м.)],
object_kind as [Вид Об'єкту відповідно Класифікатора майна],
object_type as [Тип Об'єкту],
condition as [Стан Об'єкту],
org_ownership as [Балансоутримувач - Форма Власності],
balans_obj_name as [Назва Об'єкту],
modify_date as [Дата Актуальності],
sphera_dialnosti as [Сфера діяльності]
from
(
	SELECT 
	--vb.* 
	vb.org_full_name,
	vb.org_zkpo_code,
	vb.district,
	vb.street_full_name,
	vb.addr_nomer,
	vb.sqr_total,
	vb.sqr_vlas_potreb,
	vb.sqr_kor,
	vb.object_kind,
	vb.object_type,
	vb.condition,
	vb.org_ownership,
	vb.balans_obj_name,
	vb.modify_date
	,b.sqr_total as sqr_total_bld
	,b.sqr_pidval as sqr_pidval_bld
	,b.sqr_mk as sqr_mk_bld
	,b.sqr_dk as sqr_dk_bld
	,b.sqr_rk as sqr_rk_bld
	,b.sqr_other as sqr_other_bld
	,b.sqr_rented as sqr_rented_bld
	,b.sqr_zagal as sqr_zagal_bld
	,b.sqr_for_rent as sqr_for_rent_bld
	,b.sqr_habit as sqr_habit_bld
	,b.sqr_non_habit as sqr_non_habit_bld
	,b.sqr_loft as sqr_loft_bld 
	,isnull(ddd.name, 'Невідомо') as sphera_dialnosti
	,b.construct_year
	,b.expl_enter_year
	,case when exists (select 1 from reports1nf_balans q where q.id = vb.balans_id) then 1 else 0 end as ex_reports1nf_balans 
	,(select top 1 q.report_id from reports1nf_balans q where q.id = vb.balans_id) as reports1nf_report_id
	,case when vb.balans_id in (select b.id from dbo.reports1nf_balans b where b.organization_id = vb.organization_id and ISNULL(b.is_deleted, 0) = 0 ) then 1 else 0 end as is_dpz_object
	,(select sum(case when fs.is_included = 1 then fs.total_free_sqr else 0 end) as total_free_sqr from reports1nf_balans_free_square fs where fs.balans_id = bal.id and fs.report_id = bal.report_id) as total_free_sqr 

	, orggospupr = (select old_organ from view_organizations WHERE organization_id =  vb.organization_id)
	, vb.balans_id as balans_id_

		FROM view_balans_all vb
		LEFT JOIN reports1nf_balans bal on vb.balans_id = bal.id
		LEFT JOIN reports1nf_buildings b on bal.building_1nf_unique_id = b.unique_id
		LEFT OUTER JOIN (select obp.org_id,occ.name from org_by_period obp
			 join dict_rent_occupation occ on occ.id = obp.org_occupation_id
			  where obp.period_id = (select top 1 id from dict_rent_period order by id desc)) DDD ON DDD.org_id = vb.organization_id
		WHERE
 		 ((1 = 0) OR (1 <> 0 AND vb.balans_id in (select b.id from dbo.reports1nf_balans b where b.organization_id = vb.organization_id and ISNULL(b.is_deleted, 0) = 0 ) )) AND
			((0 = 0) OR (0 <> 0 AND (vb.org_ownership_int IN (32,33,34) OR vb.form_ownership_int IN (32,33,34)))) AND
			((0 = 1) OR (0 = 0 AND (vb.is_deleted IS NULL OR vb.is_deleted = 0 OR vb.is_not_accepted = 1))) AND
			((0 = 0) OR (vb.org_ownership_int in (select id from dict_org_ownership where is_rda = 1) AND vb.org_district_id = 0))
) T
where sphera_dialnosti <> 'НЕВИЗНАЧЕНІ'
--and org_zkpo_code = '37393777'
