
CREATE   VIEW [dbo].[view_assessment]
AS
SELECT
	en.[id]
	,en.[org_renter_id]
	,en.[org_renter_name]
	,en.[org_balans_id]
	,en.[org_balans_name]
	,en.[building_id]
	,en.[balans_id]
	,en.[addr_district_id]
	,en.[expert_obj_type_id]
	,en.[obj_square]
	,en.[expert_id]
	,en.[expert_name]
	,en.[modified_by]
	,en.[modify_date]
	,en.[rezenz_id]
	,en.[addr_street_name]
	,en.[cost_1_usd]
	,en.[cost_prim]
	,en.[valuation_date]
	,en.[final_date]
	,en.[arch_num]
	,en.[arch_date]
	,en.[is_archived]
	,en.[is_stand_oc]
	,en.[is_deleted]
	,en.[del_date]
	,en.[addr_number1]
	,en.[addr_number2]
	,en.[note_text]
	,en.[addr_nomer]
	,en.[addr_street_id]
	,en.[valuation_kind_id]
	,renter.full_name as renter_name
	,bal_org.full_name as balans_org_name
	,dict_districts2.name as district
	,streets.name as street_full_name
	,dict_expert_obj_type.name as expert_obj_type
	,kind.name as valuation_kind
	,case when dict_expert.short_name <> '' then dict_expert.short_name else dict_expert.full_name end as dict_expert_name
	,stan.name as stan_name

	,(select Stuff(
		(SELECT char(10) + case when Q.doc_num <> '' then Q.doc_num else '-' end
			FROM expert_input_doc Q WHERE Q.expert_note_id = en.id order by Q.doc_date, Q.id
		FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as inputdoc_doc_num

	--,(select Stuff(
	--	(SELECT char(10) + case when Q.doc_date is null then '-' else convert(varchar(100),Q.doc_date,104) end
	--		FROM expert_input_doc Q WHERE Q.expert_note_id = en.id order by Q.doc_date, Q.id
	--	FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as inputdoc_doc_date

	,(SELECT top 1 Q.doc_date FROM expert_input_doc Q WHERE Q.expert_note_id = en.id order by Q.doc_date desc) as inputdoc_doc_date

	--,(select Stuff(
	--	(SELECT char(10) + case when Q.control_date is null then '-' else convert(varchar(100),Q.control_date,104) end
	--		FROM expert_input_doc Q WHERE Q.expert_note_id = en.id order by Q.doc_date, Q.id
	--	FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as inputdoc_control_date
	,(select top 1 Q.control_date FROM expert_input_doc Q WHERE Q.expert_note_id = en.id order by Q.control_date desc) as inputdoc_control_date

	--,(select Stuff(
	--	(SELECT char(10) + case when Q.rezenz_name <> '' then Q.rezenz_name else '-' end
	--		FROM expert_input_doc Q WHERE Q.expert_note_id = en.id order by Q.doc_date, Q.id
	--	FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as inputdoc_rezenz_name
	,(select Stuff(
		(SELECT char(10) + case when Q2.name <> '' then Q2.name else '-' end
			FROM expert_input_doc Q left join dict_expert_korr Q2 on Q2.id = Q.korrespondent_id WHERE Q.expert_note_id = en.id order by Q.doc_date, Q.id
		FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as inputdoc_korrespondent
	,(select Stuff(
		(SELECT char(10) + case when Q2.name <> '' then Q2.name else '-' end
			FROM expert_input_doc Q left join dict_expert_rezenz Q2 on Q2.id = Q.rezenz_id WHERE Q.expert_note_id = en.id order by Q.doc_date, Q.id
		FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as inputdoc_rezenz_name

	,case 
		when 
			exists (select 1 FROM expert_input_doc Q left join dict_expert_rezenz Q2 on Q2.id = Q.rezenz_id WHERE Q.expert_note_id = en.id and Q2.is_dkv = 1)
				and
			exists (select 1 FROM expert_input_doc Q left join dict_expert_rezenz Q2 on Q2.id = Q.rezenz_id WHERE Q.expert_note_id = en.id and Q2.is_dkv = 0)
				then 'ДКВ та не ДКВ'
		when 
			exists (select 1 FROM expert_input_doc Q left join dict_expert_rezenz Q2 on Q2.id = Q.rezenz_id WHERE Q.expert_note_id = en.id and Q2.is_dkv = 1)
				and
			not exists (select 1 FROM expert_input_doc Q left join dict_expert_rezenz Q2 on Q2.id = Q.rezenz_id WHERE Q.expert_note_id = en.id and Q2.is_dkv = 0)
				then 'ДКВ'
		when 
			not exists (select 1 FROM expert_input_doc Q left join dict_expert_rezenz Q2 on Q2.id = Q.rezenz_id WHERE Q.expert_note_id = en.id and Q2.is_dkv = 1)
				and
			exists (select 1 FROM expert_input_doc Q left join dict_expert_rezenz Q2 on Q2.id = Q.rezenz_id WHERE Q.expert_note_id = en.id and Q2.is_dkv = 0)
				then 'не ДКВ'
		else '' 
	  end as inputdoc_rezenz_type


	,(select Stuff(
		(SELECT char(10) + case when Q.doc_num <> '' then Q.doc_num else '-' end
			FROM expert_output_doc Q WHERE Q.expert_note_id = en.id order by Q.doc_date, Q.id
		FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as outputdoc_doc_num

	--,(select Stuff(
	--	(SELECT char(10) + case when Q.doc_date is null then '-' else convert(varchar(100),Q.doc_date,104) end
	--		FROM expert_output_doc Q WHERE Q.expert_note_id = en.id order by Q.doc_date, Q.id
	--	FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as outputdoc_doc_date
	,(SELECT top 1 Q.doc_date FROM expert_output_doc Q WHERE Q.expert_note_id = en.id order by Q.doc_date desc) as outputdoc_doc_date

	,(select Stuff(
		(SELECT char(10) + case when Q2.name <> '' then Q2.name else '-' end
			FROM expert_output_doc Q left join dict_expert_rezenz_type Q2 on Q2.id = Q.rezenz_type_id WHERE Q.expert_note_id = en.id order by Q.doc_date, Q.id
		FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as outputdoc_rezenz

	--,(select Stuff(
	--	(SELECT char(10) + case when Q.rezenz_date is null then '-' else convert(varchar(100),Q.rezenz_date,104) end
	--		FROM expert_output_doc Q WHERE Q.expert_note_id = en.id order by Q.doc_date, Q.id
	--	FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as outputdoc_rezenz_date
	,(SELECT top 1 Q.rezenz_date FROM expert_output_doc Q WHERE Q.expert_note_id = en.id order by Q.rezenz_date desc) as outputdoc_rezenz_date

	,(select Stuff(
		(SELECT char(10) + case when Q.obj_square > 0 then convert(varchar(100), Q.obj_square) else '-' end
			FROM expert_note_detail Q WHERE Q.expert_note_id = en.id order by Q.obj_square desc, Q.id
		FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as details_obj_square
	,(select Stuff(
		(SELECT char(10) + case when Q.floors <> '' then Q.floors else '-' end
			FROM expert_note_detail Q WHERE Q.expert_note_id = en.id order by Q.obj_square desc, Q.id
		FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as details_floors
	,(select Stuff(
		(SELECT char(10) + case when Q.purpose <> '' then Q.purpose else '-' end
			FROM expert_note_detail Q WHERE Q.expert_note_id = en.id order by Q.obj_square desc, Q.id
		FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as details_purpose
	,(select Stuff(
		(SELECT char(10) + case when Q.cost_1_usd > 0 then convert(varchar(100), Q.cost_1_usd) else '-' end
			FROM expert_note_detail Q WHERE Q.expert_note_id = en.id order by Q.obj_square desc, Q.id
		FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as details_cost_1_usd

	--,(select Stuff(
	--	(SELECT char(10) + case when Q.valuation_date is null then '-' else convert(varchar(100),Q.valuation_date,104) end
	--		FROM expert_note_detail Q WHERE Q.expert_note_id = en.id order by Q.obj_square desc, Q.id
	--	FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')) as valuation_dates
	,(SELECT top 1 Q.valuation_date FROM expert_note_detail Q WHERE Q.expert_note_id = en.id order by Q.valuation_date desc) as valuation_dates

	,(select top 1 Q2.color_rgb FROM expert_output_doc Q left join dict_expert_rezenz_type Q2 on Q.rezenz_type_id = Q2.id 
			WHERE Q.expert_note_id = en.id order by Q.doc_date desc) as background_color_rgb


FROM
    expert_note en
    LEFT OUTER JOIN organizations renter ON renter.id = en.org_renter_id
    LEFT OUTER JOIN organizations bal_org ON bal_org.id = en.org_balans_id
    LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = en.addr_district_id
    LEFT OUTER JOIN dict_expert ON dict_expert.id = en.expert_id
    LEFT OUTER JOIN dict_expert_rezenz ON dict_expert_rezenz.id = en.rezenz_id
    LEFT OUTER JOIN dict_expert_obj_type ON dict_expert_obj_type.id = en.expert_obj_type_id
	LEFT OUTER JOIN dict_expert_stan stan ON stan.id = en.stan_id
    LEFT OUTER JOIN expert_input_doc_grouped in_doc ON in_doc.expert_note_id = en.id
    LEFT OUTER JOIN expert_output_doc_grouped out_doc ON out_doc.expert_note_id = en.id
    LEFT OUTER JOIN expert_note_detail_grouped detail ON detail.expert_note_id = en.id
	LEFT OUTER JOIN dict_streets streets ON streets.id = en.addr_street_id
	LEFT OUTER JOIN dict_expert_valuation_kind kind ON kind.id = en.valuation_kind_id
where isnull(en.is_deleted,0) = 0
