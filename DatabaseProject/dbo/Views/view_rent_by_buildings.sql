
CREATE VIEW [view_rent_by_buildings]
AS
SELECT
    obj.id AS 'rent_object_id',
    obj.building_id,
    obj.rent_payment_id,
    obj.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    obj.addr_street,
    obj.addr_street_type,
    obj.addr_num,
    org.id AS 'bal_org_id',
    org.full_name AS 'bal_org_full_name',
    org.short_name AS 'bal_org_short_name',
    org.zkpo_code AS 'bal_org_zkpo',
    org.sfera_upr_id AS 'bal_org_sfera_upr_id',
    org.form_ownership_id AS 'bal_org_form_ownership_id',
    org.addr_distr_new_id AS 'bal_org_district_id',
    bal_org.rent_occupation_id AS 'bal_org_occupation_id',
    dict_rent_occupation.name AS 'bal_org_occupation',
    pay.phone AS 'bal_org_phone',
    pay.email AS 'bal_org_email',
    pay.responsible_fio AS 'bal_org_responsible_fio',
    obj.sqr_total,
    obj.sqr_rented,
    obj.sqr_free,
    obj.sqr_korysna,
    obj.sqr_mzk,
    obj.floors,
    dict_tech_state.name AS 'tech_state',
    CASE WHEN obj.is_energo = 0 THEN N'НІ' ELSE N'ТАК' END AS 'is_energo',
    CASE WHEN obj.is_vodo = 0 THEN N'НІ' ELSE N'ТАК' END AS 'is_vodo',
    CASE WHEN obj.is_teplo = 0 THEN N'НІ' ELSE N'ТАК' END AS 'is_teplo',
    obj.district_id,
    dict_districts2.name AS 'district',
    obj.purpose,
    obj.rent_note_id,
    dict_rent_note.name AS 'rent_note'
FROM
    rent_object obj
    LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = obj.district_id
    LEFT OUTER JOIN dict_rent_period ON dict_rent_period.id = obj.rent_period_id
    LEFT OUTER JOIN dict_rent_note ON dict_rent_note.id = obj.rent_note_id
    LEFT OUTER JOIN dict_tech_state ON dict_tech_state.id = obj.tech_state_id
    LEFT OUTER JOIN rent_payment pay ON pay.id = obj.rent_payment_id
    LEFT OUTER JOIN rent_balans_org bal_org ON bal_org.organization_id = pay.org_balans_id
    LEFT OUTER JOIN organizations org ON org.id = pay.org_balans_id
    LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = bal_org.rent_occupation_id

UNION ALL

SELECT
    obj.rent_object_id,
    obj.building_id,
    obj.rent_payment_id,
    obj.rent_period_id,
    dict_rent_period.name AS 'rent_period',
    obj.addr_street,
    obj.addr_street_type,
    obj.addr_num,
    org.id AS 'bal_org_id',
    org.full_name AS 'bal_org_full_name',
    org.short_name AS 'bal_org_short_name',
    org.zkpo_code AS 'bal_org_zkpo',
    org.sfera_upr_id AS 'bal_org_sfera_upr_id',
    org.form_ownership_id AS 'bal_org_form_ownership_id',
    org.addr_distr_new_id AS 'bal_org_district_id',
    bal_org.rent_occupation_id AS 'bal_org_occupation_id',
    dict_rent_occupation.name AS 'bal_org_occupation',
    pay.phone AS 'bal_org_phone',
    pay.email AS 'bal_org_email',
    pay.responsible_fio AS 'bal_org_responsible_fio',
    obj.sqr_total,
    obj.sqr_rented,
    obj.sqr_free,
    obj.sqr_korysna,
    obj.sqr_mzk,
    obj.floors,
    obj.condition AS 'tech_state',
    NULL is_energo,
    NULL is_vodo,
    NULL is_teplo,
    obj.district_id,
    obj.district AS 'district',
    obj.purpose,
    obj.rent_note_id,
    dict_rent_note.name AS 'rent_note'
FROM
	(
		SELECT -ROW_NUMBER() OVER (ORDER BY arenda.building_id) rent_object_id, 
			arenda.building_id,
			arenda_payments.id rent_payment_id,
			arenda_payments.rent_period_id,
			dict_streets.name addr_street,
			dict_streets.kind addr_street_type,
			buildings.addr_nomer addr_num,
			balans.sqr_total,
			arenda.rent_square sqr_rented,
			balans.sqr_free,
			balans.sqr_kor sqr_korysna,
			balans.sqr_free_mzk sqr_mzk,
			balans.floors,
			buildings.addr_distr_new_id district_id,
			arenda.purpose_str purpose,
			NULL rent_note_id,
			balans.condition,
			arenda.org_balans_id,
			dict_districts2.name district
		FROM arenda
		INNER JOIN arenda_payments ON arenda_payments.arenda_id = arenda.id
		INNER JOIN buildings ON buildings.id = arenda.building_id
		LEFT JOIN view_balans balans ON balans.balans_id = arenda.balans_id
		LEFT JOIN dict_streets ON dict_streets.id = buildings.addr_street_id
		LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = buildings.addr_distr_new_id
		WHERE arenda.agreement_state = 1
	) obj
    LEFT OUTER JOIN dict_rent_period ON dict_rent_period.id = obj.rent_period_id
    LEFT OUTER JOIN dict_rent_note ON dict_rent_note.id = obj.rent_note_id
    LEFT OUTER JOIN rent_payment pay ON pay.id = obj.rent_payment_id
    LEFT OUTER JOIN rent_balans_org bal_org ON bal_org.organization_id = obj.org_balans_id
    LEFT OUTER JOIN organizations org ON org.id = obj.org_balans_id
    LEFT OUTER JOIN dict_rent_occupation ON dict_rent_occupation.id = bal_org.rent_occupation_id
