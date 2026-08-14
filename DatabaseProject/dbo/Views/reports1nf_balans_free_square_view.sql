
create   view reports1nf_balans_free_square_view as 
select
fs.id as free_square_id,
org.zkpo_code as balans_zkpo
FROM view_reports1nf rep
join reports1nf_balans bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join reports1nf_balans_free_square fs on fs.balans_id = bal.id and fs.report_id = rep.report_id
join reports1nf_org_info org on org.id = bal.organization_id
--where fs.id = 5709
