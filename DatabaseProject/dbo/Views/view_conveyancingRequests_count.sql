create view view_conveyancingRequests_count as 
select 
[org].[report_id], count(*) as cnt
from reports1nf_org_info org
join 
(
	SELECT req.*, 
			CASE WHEN req.status = 1 THEN 'Введено'
					WHEN req.status = 2 THEN 'Відхилено'
					WHEN req.status = 3 THEN 'Підтверджено'
					ELSE 'Невідомий'
			END AS text_status,
			CASE WHEN req.conveyancing_type = 1 THEN 'Передача об’єкту з балансу на баланс'
					WHEN req.conveyancing_type = 2 THEN 'Передача об’єкту з балансу на баланс'
					WHEN req.conveyancing_type = 3 THEN 'Списання об’єкту з балансу шляхом зносу'
					WHEN req.conveyancing_type = 4 THEN 'Списання об’єкту з балансу шляхом приватизації'
					WHEN req.conveyancing_type = 5 THEN 'Постановка  новозбудованого об’єкту на баланс'
					ELSE 'Невідомий'
			END AS type_of_conveyancing, 
			vb.district, vb.street_full_name, vb.addr_nomer, vb.sqr_total, 
			org_from.zkpo_code org_from_zkpo, org_from.full_name org_from_name, org_to.zkpo_code org_to_zkpo, org_to.full_name org_to_name
			FROM transfer_requests req LEFT JOIN view_balans_all vb ON req.balans_id = vb.balans_id 
			LEFT JOIN organizations org_from ON req.org_from_id = org_from.id and (org_from.is_deleted is null or org_from.is_deleted = 0)
			LEFT JOIN organizations org_to ON req.org_to_id = org_to.id and (org_to.is_deleted is null or org_to.is_deleted = 0)
			WHERE (req.status <> 3) AND (req.is_object_exists = 1) 
        
			UNION

			SELECT req.*, 
			CASE WHEN req.status = 1 THEN 'Введено'
					WHEN req.status = 2 THEN 'Відхилено'
					WHEN req.status = 3 THEN 'Підтверджено'
					ELSE 'Невідомий'
			END AS text_status,
			CASE WHEN req.conveyancing_type = 1 THEN 'Передача об’єкту з балансу на баланс'
					WHEN req.conveyancing_type = 2 THEN 'Передача об’єкту з балансу на баланс'
					WHEN req.conveyancing_type = 3 THEN 'Списання об’єкту з балансу шляхом зносу'
					WHEN req.conveyancing_type = 4 THEN 'Списання об’єкту з балансу шляхом приватизації'
					WHEN req.conveyancing_type = 5 THEN 'Постановка  новозбудованого об’єкту на баланс'
					ELSE 'Невідомий'
			END AS type_of_conveyancing, 
			b.district, b.street_full_name, b.addr_nomer, ub.sqr_total,
			org_from.zkpo_code org_from_zkpo, org_from.full_name org_from_name, org_to.zkpo_code org_to_zkpo, org_to.full_name org_to_name
			FROM transfer_requests req LEFT JOIN unverified_balans ub ON req.balans_id = ub.id 
			LEFT JOIN organizations org_from ON req.org_from_id = org_from.id AND (org_from.is_deleted is null or org_from.is_deleted = 0)
			LEFT JOIN organizations org_to ON req.org_to_id = org_to.id AND (org_to.is_deleted is null or org_to.is_deleted = 0)
			LEFT JOIN view_buildings b ON b.building_id = ub.building_id
			WHERE (req.status <> 3) AND (req.is_object_exists <> 1) 
) req on (req.org_from_id = [org].[report_id] OR req.org_id_for_confirm = [org].[report_id] OR (req.org_to_id = [org].[report_id] AND req.conveyancing_type IN(1, 5))) 
group by [org].[report_id]