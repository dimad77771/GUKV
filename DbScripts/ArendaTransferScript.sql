--C йо "дхпейж╡ъ г српхлюммъ рю наяксцнбсбюммъ фхркнбнцн тнмдс ябърньхмяэйнцн п-мс 36037999 ОЕПЕДЮРЭ 
--БЯЕ ДНЦНБНПЮ ХЯОНКЭГНБЮМХЪ ОНЛЕЫЕМХИ Б йо "йепсчвю йнлоюм╡ъ г наяксцнбсбюммъ фхркнбнцн тнмдс ябърньхмяэйнцн пюинмс л. йх╙бю" 39607507

select 
(select o.id from organizations o 
join reports1nf rep on rep.organization_id = o.id
where o.zkpo_code = '32525507' and o.master_org_id is null) as org_id_from,
(select rep.id from organizations o 
join reports1nf rep on rep.organization_id = o.id
where o.zkpo_code = '32525507' and o.master_org_id is null) as report_id_from,
(select o.id from organizations o 
join reports1nf rep on rep.organization_id = o.id
where o.zkpo_code = '32375554' and o.master_org_id is null) as org_to_id,
(select rep.id from organizations o 
join reports1nf rep on rep.organization_id = o.id
where o.zkpo_code = '32375554' and o.master_org_id is null) as report_to_id


-- 3 = REPORT_ID_FROM
-- 669 = REPORT_ID_TO

-- 76 = REPORT_ID_FROM
-- 337 = REPORT_ID_TO

UPDATE reports1nf_arenda SET modify_date = dateadd(second, 1, submit_date) WHERE report_id = 76
UPDATE reports1nf_arenda SET modify_date = dateadd(second, 1, submit_date) WHERE report_id = 337

select 
--'UPDATE reports1nf_arenda SET modify_date = dateadd(second, 1, submit_date) WHERE id = ' + CONVERT(varchar, s.id) AS UPDATE_FOR_SEND_FROM,
-- нропюбйю б дйб FROM
-- нропюбйю б дйб TO
-- оепецемепхпнбюрэ VIEW
-- опнбепхрэ йнкхвеярбн
-- оепецемепхпнбюрэ яйпхор б яксвюе еякх ашкх -1
'UPDATE arenda SET id = ' + CONVERT(varchar, s.id) + CASE WHEN (s.org_giver_id = (select organization_id from reports1nf where id = 76)) THEN ', org_giver_id = (select organization_id from reports1nf where id = 337)' ELSE '' END + CASE WHEN (s.org_renter_id = (select organization_id from reports1nf where id = 76)) THEN ', org_renter_id = (select organization_id from reports1nf where id = 337)' ELSE '' END + CASE WHEN (s.org_balans_id = (select organization_id from reports1nf where id = 76)) THEN ', org_balans_id = (select organization_id from reports1nf where id = 337)' ELSE '' END + ' WHERE id = ' + CONVERT(varchar, s.id) AS TRANSFER_DKV,
'UPDATE reports1nf_arenda SET report_id = 337, modify_date = dateadd(second, 1, submit_date)' + CASE WHEN (s.org_giver_id = (select organization_id from reports1nf where id = 76)) THEN ', org_giver_id = (select organization_id from reports1nf where id = 337)' ELSE '' END + CASE WHEN (s.org_renter_id = (select organization_id from reports1nf where id = 76)) THEN ', org_renter_id = (select organization_id from reports1nf where id = 337)' ELSE '' END + CASE WHEN (s.org_balans_id = (select organization_id from reports1nf where id = 76)) THEN ', org_balans_id = (select organization_id from reports1nf where id = 337)' ELSE '' END + ' WHERE id = ' + CONVERT(varchar, s.id) AS TRANSFER_BALANS,
'UPDATE reports1nf_buildings SET report_id = 337 where unique_id = ' + CONVERT(varchar, s.building_1nf_unique_id) + ' AND report_id = 76' AS TRANSFER_BUILDINGS,
'UPDATE reports1nf_arenda_decisions SET report_id = 337 WHERE arenda_id = ' + CONVERT(varchar, s.id) as TRANSFER_reports1nf_arenda_decisions,
'UPDATE reports1nf_arenda_notes SET report_id = 337 WHERE arenda_id = ' + CONVERT(varchar, s.id) as TRANSFER_reports1nf_arenda_notes,
'UPDATE reports1nf_arenda_payments SET report_id = 337 WHERE arenda_id = ' + CONVERT(varchar, s.id) as TRANSFER_reports1nf_arenda_payments,
'UPDATE reports1nf_payment_documents SET report_id = 337 WHERE arenda_id = ' + CONVERT(varchar, s.id) as TRANSFER_reports1nf_payment_documents,
'UPDATE reports1nf_comments SET report_id = 337 WHERE arenda_id = ' + CONVERT(varchar, s.id) + ' AND report_id = 76' as TRANSFER_reports1nf_comments
-- оепецемепхпнбюрэ VIEW
-- опнбепхрэ йнкхвеярбн
from 
(select * from reports1nf_arenda where report_id = 76) s


-- онлерйю мю сдюкемхе хлонпрхпнбюммнцн днцнбнпю, йнрнпши ъбкъеряъ дсакел
--UPDATE arenda SET is_deleted = 1 WHERE id = 24318
