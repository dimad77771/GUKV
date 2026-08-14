CREATE VIEW [dbo].[view_reports1nf]
AS
SELECT
    rep.organization_id,
    rep.is_reviewed,
    rep.create_date,
    rep.id AS 'report_id',
    [org].[full_name],
    [org].[short_name],
    [org].[zkpo_code],
    [dict_org_industry].[name] AS 'industry',
    [dict_org_occupation].[name] AS 'occupation',
    [dict_org_status].[name] AS 'status',
    [dict_org_form_gosp].[name] AS 'form_gosp',
    [dict_org_ownership].[name] AS 'form_of_ownership',
    [dict_org_gosp_struct].[name] AS 'gosp_struct',
    [dict_org_vedomstvo].[name] AS 'vedomstvo',
    [dict_org_form].[name] AS 'org_form',
    [dict_org_gosp_struct_type].[name] AS 'gosp_struct_type',
    [dict_org_sfera_upr].[name] AS 'sfera_upr',
    [dict_org_old_industry].[name] AS 'old_industry',
    [dict_org_old_occupation].[name] AS 'old_occupation',
    [dict_org_old_organ].[name] AS 'old_organ',
    [org].[addr_city],
    [dict_districts2].[name] AS 'addr_district',
    [org].[addr_street_name],
    [org].[addr_nomer],
    [org].[addr_korpus],
    [org].[addr_zip_code],
    [director_fio],
    [director_phone],
    [director_title],
    [buhgalter_fio],
    [buhgalter_phone],
    [num_buildings],
    [fax],
    [registration_auth],
    [registration_num],
    [registration_date],
    [registration_svidot],
    [sqr_on_balance],
    [org].[sqr_total],
    [num_objects],
    [kved_code],
    [koatuu],
    [dict_org_mayno].[name] AS 'mayno',
    CASE WHEN [is_liquidated] = 1 THEN N'ТАК' ELSE N'НІ' END AS 'is_liquidated',
    [liquidation_date],
    [contact_email],
    [dict_org_contact_posada].[name] AS 'contact_posada',
    COALESCE([dict_org_registr_org].[name], registration_auth) AS 'registr_org',
    org.modified_by,
    org.modify_date,
    org.sfera_upr_id AS 'org_sfera_upr_id',
    org.form_ownership_id AS 'org_form_ownership_id',
    org.addr_distr_new_id AS 'org_district_id',
      
    CASE WHEN is_reviewed = 1 THEN N'Перевірено' ELSE N'Не перевірено' END AS 'review_state',
    CASE WHEN
		chk_subm.cnt > 0 OR		--  перевірка своєчасності надсилання звіту
		EXISTS (SELECT id FROM reports1nf_balans bal WHERE bal.report_id = rep.id AND (bal.submit_date IS NULL OR bal.modify_date > bal.submit_date) AND ((bal.is_deleted is null) or (bal.is_deleted = 0))) OR
		EXISTS (SELECT id FROM reports1nf_balans_deleted bd WHERE bd.report_id = rep.id AND (bd.submit_date IS NULL OR bd.modify_date > bd.submit_date) AND ((bd.is_deleted is null) or (bd.is_deleted = 0))) OR
	--	EXISTS (SELECT id FROM reports1nf_arenda ar WHERE ar.report_id = rep.id AND (ar.submit_date IS NULL OR ar.modify_date > ar.submit_date) AND ((ar.is_deleted is null) or (ar.is_deleted = 0))) OR
		EXISTS (SELECT id FROM reports1nf_arenda_rented artd WHERE artd.report_id = rep.id AND (artd.submit_date IS NULL OR artd.modify_date > artd.submit_date) AND ((artd.is_deleted is null) or (artd.is_deleted = 0))) OR
		EXISTS (SELECT id FROM reports1nf_org_info oi WHERE oi.report_id = rep.id AND (oi.submit_date IS NULL OR oi.modify_date > oi.submit_date) AND ((oi.is_deleted is null) or (oi.is_deleted = 0)))
	THEN N'Не надісланий' ELSE N'Надісланий' END AS 'cur_state',
--    CASE WHEN chk_subm.cnt > 0	THEN N'Не надісланий' ELSE N'Надісланий' END AS 'cur_state',

	(SELECT MAX(b.submit_date) FROM reports1nf_balans b WHERE b.report_id = rep.id) AS 'bal_max_submit_date',
	(SELECT MAX(bd.submit_date) FROM reports1nf_balans_deleted bd WHERE bd.report_id = rep.id) AS 'bal_del_max_submit_date',
	(SELECT MAX(ar.submit_date) FROM reports1nf_arenda ar WHERE ar.report_id = rep.id) AS 'arenda_max_submit_date',
	(SELECT MAX(artd.submit_date) FROM reports1nf_arenda_rented artd WHERE artd.report_id = rep.id) AS 'arenda_rented_max_submit_date',
	(SELECT MAX(o.submit_date) FROM reports1nf_org_info o WHERE o.report_id = rep.id) AS 'org_max_submit_date',
	
	rep.stan_recieve_id,
	rep.stan_recieve_date,
	dict_stan_recieve.stan_recieve_name,
	rep.stan_recieve_description,
	org.old_organ_id,
	rep.inventar_recieve_date,
	rep.rep_modified_by,
	rep.rep_modify_date,
	rep.orandodavec_user_id,
	rep.zvit_last_created
	
	
FROM
    reports1nf rep
    LEFT OUTER JOIN reports1nf_org_info org on org.report_id = rep.id
    LEFT OUTER JOIN dict_org_industry ON org.industry_id = dict_org_industry.id
    LEFT OUTER JOIN dict_org_occupation ON org.occupation_id = dict_org_occupation.id
    LEFT OUTER JOIN dict_org_status ON org.status_id = dict_org_status.id
    LEFT OUTER JOIN dict_org_form_gosp ON org.form_gosp_id = dict_org_form_gosp.id
    LEFT OUTER JOIN dict_org_ownership ON org.form_ownership_id = dict_org_ownership.id
    LEFT OUTER JOIN dict_org_gosp_struct ON org.gosp_struct_id = dict_org_gosp_struct.id
    LEFT OUTER JOIN dict_org_gosp_struct_type ON org.gosp_struct_type_id = dict_org_gosp_struct_type.id
    LEFT OUTER JOIN dict_org_vedomstvo ON org.vedomstvo_id = dict_org_vedomstvo.id
    LEFT OUTER JOIN dict_org_form ON org.form_id = dict_org_form.id
    LEFT OUTER JOIN dict_org_mayno ON org.mayno_id = dict_org_mayno.id
    LEFT OUTER JOIN dict_org_contact_posada ON org.contact_posada_id = dict_org_contact_posada.id
    LEFT OUTER JOIN dict_org_sfera_upr ON org.sfera_upr_id = dict_org_sfera_upr.id
    LEFT OUTER JOIN dict_org_registr_org ON org.registr_org_id = dict_org_registr_org.id
    LEFT OUTER JOIN dict_org_old_industry ON org.old_industry_id = dict_org_old_industry.id
    LEFT OUTER JOIN dict_org_old_occupation ON org.old_occupation_id = dict_org_old_occupation.id
    LEFT OUTER JOIN dict_org_old_organ ON org.old_organ_id = dict_org_old_organ.id
    LEFT OUTER JOIN dict_districts2 ON org.addr_distr_new_id = dict_districts2.id
    LEFT OUTER JOIN dict_stan_recieve ON rep.stan_recieve_id = dict_stan_recieve.stan_recieve_id
	outer apply (SELECT report_id = r.id, cnt = (select count(*) FROM reports1nf_arenda ar 
			LEFT JOIN arenda a ON a.id = ar.id
            join dbo.reports1nf_arenda_payments ap on ap.arenda_id = ar.id and ap.id in (select id from dbo.reports1nf_arenda_payments t1 where t1.arenda_id = ap.arenda_id and t1.report_id = ar.report_id and t1.rent_period_id = (select MAX(rent_period_id) from dbo.reports1nf_arenda_payments t2 where t2.arenda_id = t1.arenda_id and t2.report_id = t1.report_id ))
			join [dbo].[dict_rent_period] per on per.is_active = 1
			where ar.agreement_state in (1, 2, 3) 
			and (ar.submit_date not between per.period_start and DATEADD(day, 61, per.period_end) or ar.modify_date > ar.submit_date or ap.rent_period_id <> per.id)
			and isnull(ar.is_deleted, 0) = 0 
			and isnull(a.is_deleted, 0) = 0
			and r.id  = ar.report_id ) from reports1nf r where rep.id = r.id) chk_subm

