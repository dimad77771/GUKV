CREATE VIEW [dbo].[view_arenda_and_notes]
AS
SELECT 
       [arenda].[id]
      ,[notes].[id] AS 'arenda_note_id'
      ,[arenda].[building_id]
      ,[arenda].[org_balans_id]
      ,[arenda].[org_renter_id]
      ,[arenda].[org_giver_id]
      ,[arenda].[balans_id]
      ,[arenda].[rent_year]
      ,[arenda].[object_kind_id]
      ,COALESCE(notes.purpose_group_id, arenda.purpose_group_id) AS 'purpose_group_id'
      ,COALESCE(notes.purpose_id, arenda.purpose_id) AS 'purpose_id'
      ,COALESCE(notes.purpose_str, arenda.purpose_str) AS 'purpose_str'
      ,[arenda].[name]
      ,[arenda].[is_privat]
      ,[arenda].[update_src_id]
      ,[arenda].[agreement_kind_id]
      ,[arenda].[agreement_date]
      ,[arenda].[agreement_num]
      ,CASE WHEN NOT (RTRIM(LTRIM(arenda.agreement_num)) LIKE '%[^0-9]%') THEN CAST(RTRIM(LTRIM(arenda.agreement_num)) AS bigint) ELSE NULL END AS 'agreement_num_int'
      ,[arenda].[agreement_str]
      ,[arenda].[floor_number]
      ,[arenda].[num_people]
      ,COALESCE(notes.cost_narah, arenda.cost_narah) AS 'cost_narah'
      ,[arenda].[cost_payed]
      ,[arenda].[cost_debt]
      ,COALESCE(notes.cost_agreement, arenda.cost_agreement) AS 'cost_agreement'
      ,[arenda].[cost_expert_1m]
      --,COALESCE(notes.cost_expert_total, arenda.cost_expert_total) AS 'cost_expert_total'
      ,notes.cost_expert_total AS 'cost_expert_total'
      ,arenda.cost_expert_total AS 'cost_expert_total_agr'
      ,[arenda].[pidstava]
      ,[arenda].[pidstava_date]
      ,[arenda].[pidstava_num]
      ,[arenda].[pidstava_fact]
      ,[arenda].[pidstava2]
      ,[arenda].[pidstava_num2]
      ,[arenda].[pidstava_date2]
      ,[arenda].[pidstava_display]
      ,[arenda].[rent_start_date]
      ,[arenda].[rent_finish_date]
      ,[arenda].[rent_actual_finish_date]
      ,COALESCE(notes.rent_rate, arenda.rent_rate) AS 'rent_rate'
      --,COALESCE(notes.rent_rate_uah, arenda.rent_rate_uah) AS 'rent_rate_uah' -- Замена при отображении а также для импорта на портал
      ,COALESCE(notes.rent_rate_uah, arenda.rent_rate_uah) AS 'rent_rate_uah'
      ,COALESCE(notes.rent_square, arenda.rent_square) AS 'rent_square'
      ,[arenda].[priznak_1nf]
      ,[arenda].[debt_timespan]
      ,[arenda].[order_num]
      ,[arenda].[order_date]
      ,[arenda].[order_no2]
      ,[arenda].[rishennya_id]
      ,[arenda].[is_inactive]
      ,[arenda].[inactive_date]
      ,[arenda].[is_deleted]
      ,[arenda].[del_date]
      ,COALESCE(notes.date_expert, arenda.date_expert) AS 'date_expert'
      ,[arenda].[is_subarenda]
      ,[arenda].[privat_kind_id]
      ,[arenda].[num_primirnikiv]
      ,[arenda].[date_expl_enter]
      ,[arenda].[num_akt]
      ,[arenda].[date_akt]
      ,[arenda].[num_bti]
      ,[arenda].[date_bti]
      ,[arenda].[svidotstvo_serial]
      ,[arenda].[svidotstvo_num]
      ,[arenda].[svidotstvo_date]
      --,COALESCE(notes.payment_type_id, arenda.payment_type_id) AS 'payment_type_id'
      ,arenda.payment_type_id AS 'payment_type_id'
      ,notes.payment_type_id AS 'payment_type_id_obj'
      ,[arenda].[arch_id]
      ,[arenda].[modified_by]
      ,[arenda].[modify_date]
      ,COALESCE(notes.note, arenda.note) AS 'note'
      ,arenda.agreement_state
      ,notes.invent_no as invent_no_agr_obj
      ,notes.note_status_id 
	  ,notes.factich_vikorist_id AS 'factich_vikorist_id'
	  ,notes.ref_balans_id
FROM
       arenda
       LEFT OUTER JOIN (SELECT * FROM arenda_notes WHERE is_deleted = 0) notes ON notes.arenda_id = arenda.id
