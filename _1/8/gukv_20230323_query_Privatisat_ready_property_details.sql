select
	privat_year as [Рік],
	kmr_number as [Номер рішення КМР],
	kmr_date as [Дата рішення КМР],
	org_name as [Балансоутримувач],
	org_info_id as [Код ЄДРПОУ],
	organizator as [Організатор продажу],
	obj_name as [Назва об’єкта],
	addr_distr_new_id as [Район],
	addr_street_id as [Назва вулиці],
	addr_nomer as [Номер будинку],
	geodata_map_points as [Координати на мапі],
	orendar as [Орендар],
	total_free_sqr as [Площа об’єкта, кв.м],
	sposib_privat as [Спосіб приватизації],
	document_privat as [Документи щодо приватизації об’єкта],
	obj_price as [Ціна продажу, грн.],
	buyer_name as [Покупець (назва)],
	buyer_adr_street as [Покупець (Назва вулиці)],
	buyer_adr_nomer as [Покупець (Номер будинку, літери, корпус)],
	prozoro_number as [Унікальний код обєкту у ЕТС Прозорро-продажі],
	modify_date2 as [Дата редагу-вання],
	modified_by2 as [Користувач]
from
(
	SELECT 
		(select Q.name from dict_streets Q where Q.id = A.addr_street_id) as addr_street
		  ,*
		  ,(select Q.zkpo_code from reports1nf_org_info Q where Q.report_id = A.org_info_id) as zkpo_code
		  ,(select Q.short_name from reports1nf_org_info Q where Q.report_id = A.org_info_id) as org_name
		  ,(select Q.name from dict_1nf_districts2 Q where Q.id = A.addr_distr_new_id) as addr_distr
	FROM [privatisat] A
) T