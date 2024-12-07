select
	balans_id as [Картка],
	organization_id as [ID Балансоутримувача],
	org_full_name as [Балансоутримувач - Повна Назва],
	org_short_name as [Балансоутримувач - Коротка Назва],
	org_zkpo_code as [Балансоутримувач - Код ЄДРПОУ],
	org_industry as [Балансоутримувач - Галузь],
	org_occupation as [Балансоутримувач - Вид Діяльності],
	org_street_name as [Балансоутримувач - Вулиця],
	org_street_nomer as [Балансоутримувач - Номер будинку],
	district as [Район],
	street_full_name as [Назва Вулиці],
	addr_nomer as [Номер Будинку],
	geodata_map_opoints as [Координати на мапі],
	orggospupr as [Орган госп. упр.],
	total_free_sqr as [Площа вільних приміщень (кв.м.)],
	total_free_count as [Кількість об'єктів що мають вільну площу],
	sqr_habit_bld as [Площа житлового фонду будинку (кв.м.)],
	sqr_pidval_bld as [Площа підвалу будинку (кв.м.)],
	sqr_loft_bld as [Площа горища будинку (кв.м.)],
	sqr_total as [Площа нежилих приміщень об'єкту (кв.м.)],
	sqr_vlas_potreb as [Площа об'єкту Для Власних Потреб (кв.м.)],
	sum_rent_square as [Площа об'єкту що знаходиться в оренді (кв.м.)],
	sqr_kor as [Корисна площа об'єкту (кв.м.)],
	sqr_engineering as [Площа техніко-інженерних потреб об'єкту (кв.м.)],
	cost_balans as [Первісна (переоцінена) вартість, тис. грн.],
	cost_expert_total as [Ринкова вартість, тис. грн.],
	cost_zalishkova as [Залишкова Вартість, тис.грн.],
	count_ref_balans as [Кількість Договорів Оренди],
	obj_bti_code as [Інвентаризаційний № справи],
	date_bti as [Дата виготовлення технічного паспорту],
	realestateobj as [Реєстрація у Державному реєстрі (Об'єкт нерухомого майна)],
	privacynote as [Реєстрація у Державному реєстрі (Номер запису про право власності)],
	bti_condition as [Реєстрація у Державному реєстрі (Реєстраційний номер об'єкту нерухомого майна)],
	invent_no_bti as [Інвентарний номер об'єкту],
	num_floors as [Кількість поверхів загальна],
	construct_year as [Рік будівництва],
	expl_enter_year as [Рік здачі в експлуатацію],
	otdel_gukv as [Стан юр.особи],
	form_ownership as [Форма Власності Об'єкту],
	ownership_type as [Право],
	object_kind as [Вид Об'єкту відповідно Класифікатора майна],
	object_type as [Тип Об'єкту],
	condition as [Стан Об'єкту],
	purpose_group as [Група Призначення],
	purpose as [Призначення],
	history as [Історична Цінність],
	floors as [Розташування приміщення (поверх)],
	znos as [Знос (грн.)],
	znos_date as [Знос станом на],
	org_vedomstvo as [Балансоутримувач - Орган Управління],
	org_ownership as [Балансоутримувач - Форма Власності],
	is_in_privat as [Будинок В Програмі Приватизації],
	balans_obj_name as [Назва Об'єкту],
	modify_date as [Дата Актуальності],
	sphera_dialnosti as [Сфера діяльності],
	gosp_struct as [Госп. Структура],
	org_contacts as [Контактні телефони],
	znizhino_flag as [Знищено],
	znizhino_shkoda as [Пошкоджено],
	znizhino_zvitakt as [Наявність звіту / акта про обстеження],
	znizhino_stanom as [станом на],
	znizhino_primitka as [Примітки р/з],
	note as [Примітки],
	balans_id_ as [ID об'єкту]
from
(
	SELECT 
	vb.balans_id
	,vb.organization_id
	,vb.org_full_name
	,vb.org_short_name
	,vb.org_zkpo_code
	,vb.org_industry
	,vb.org_occupation
	,vb.org_street_name
	,vb.org_street_nomer
	,vb.district
	,vb.street_full_name
	,vb.addr_nomer
	,vb.sqr_total
	,vb.sqr_vlas_potreb
	,vb.sqr_kor
	,vb.sqr_engineering
	,vb.cost_balans
	,vb.cost_expert_total
	,vb.cost_zalishkova
	,vb.obj_bti_code
	,vb.date_bti
	,vb.realestateobj
	,vb.privacynote
	,vb.bti_condition
	,vb.invent_no_bti
	,vb.num_floors
	,vb.otdel_gukv
	,vb.form_ownership
	,vb.ownership_type
	,vb.object_kind
	,vb.object_type
	,vb.condition
	,vb.purpose_group
	,vb.purpose
	,vb.history
	,vb.floors
	,vb.znos
	,vb.znos_date
	,vb.org_vedomstvo
	,vb.org_ownership
	,vb.is_in_privat
	,vb.balans_obj_name
	,vb.modify_date
	,vb.gosp_struct
	,vb.org_contacts
	,vb.znizhino_flag
	,vb.znizhino_shkoda
	,vb.znizhino_zvitakt
	,vb.znizhino_stanom
	,vb.znizhino_primitka
	,vb.note
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
	,(select sum(case when fs.is_included = 1 then 1 else 0 end) from reports1nf_balans_free_square fs where fs.balans_id = bal.id and fs.report_id = bal.report_id) as total_free_count

	, orggospupr = (select old_organ from view_organizations WHERE organization_id =  vb.organization_id)
	, vb.balans_id as balans_id_
	, W.sum_rent_square
	, W.count_ref_balans
	, bal.geodata_map_opoints

		FROM view_balans_all vb
		LEFT JOIN reports1nf_balans bal on vb.balans_id = bal.id
		LEFT JOIN reports1nf_buildings b on bal.building_1nf_unique_id = b.unique_id
		LEFT OUTER JOIN (select obp.org_id,occ.name from org_by_period obp
			 join dict_rent_occupation occ on occ.id = obp.org_occupation_id
			  where obp.period_id = (select top 1 id from dict_rent_period order by id desc)) DDD ON DDD.org_id = vb.organization_id
		OUTER APPLY
		(
			select sum(Q.rent_square) as sum_rent_square, count(distinct Q.arenda_id) count_ref_balans from view_arenda Q where Q.ref_balans_id = vb.balans_id and isnull(Q.is_deleted,0)=0
		) W

		WHERE
 		 ((1 = 0) OR (1 <> 0 AND vb.balans_id in (select b.id from dbo.reports1nf_balans b where b.organization_id = vb.organization_id and ISNULL(b.is_deleted, 0) = 0 ) )) AND
			((0 = 0) OR (0 <> 0 AND (vb.org_ownership_int IN (32,33,34) OR vb.form_ownership_int IN (32,33,34)))) AND
			((0 = 1) OR (0 = 0 AND (vb.is_deleted IS NULL OR vb.is_deleted = 0 OR vb.is_not_accepted = 1))) AND
			((0 = 0) OR (vb.org_ownership_int in (select id from dict_org_ownership where is_rda = 1) AND vb.org_district_id = 0))
) T