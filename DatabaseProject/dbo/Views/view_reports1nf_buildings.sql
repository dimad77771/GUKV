

CREATE VIEW [dbo].[view_reports1nf_buildings]
AS
SELECT [buildings].[id] AS 'building_id'
      ,[buildings].[street_full_name]
      ,[dict_districts2].[name] AS 'district'
      ,[buildings].[addr_nomer]
      ,[buildings].[addr_nomer1]
      ,[buildings].[addr_nomer2]
      ,[buildings].[addr_nomer3]
      ,[buildings].[addr_zip_code]
      ,[buildings].[addr_misc]
      ,[dict_tech_state].[name] AS 'condition'
      ,[buildings].[num_floors]
      ,[buildings].[construct_year]
      ,[buildings].[bti_code]
      ,[dict_history].[name] AS 'history'
      ,[dict_object_type].[name] AS 'object_type'
      ,[dict_object_kind].[name] AS 'object_kind'
      ,[buildings].[cost_balans]
      ,[buildings].[sqr_total]
      ,[buildings].[sqr_habit]
      ,[buildings].[sqr_rented]
      ,[buildings].[sqr_zagal]
      ,[buildings].[sqr_for_rent]
      ,[buildings].[sqr_non_habit]
      ,[buildings].[sqr_pidval]
      ,[buildings].[additional_info]
      ,[buildings].[oatuu_id] AS 'oatuu_code'
      ,[dict_facade].[name] AS 'facade'
      ,[dict_oatuu].[name] AS 'region'
      ,[buildings].sqr_dk AS 'sqr_object_dk'
      ,[buildings].sqr_mk AS 'sqr_object_mk'
      ,[buildings].sqr_rk AS 'sqr_object_rk'
      ,[buildings].sqr_other AS 'sqr_object_other'
      ,[buildings].is_deleted AS 'building_deleted'
      ,CASE WHEN priv.id IS NULL THEN N'НІ' ELSE N'ТАК' END AS 'is_in_privat'
      ,[buildings].modified_by
      ,[buildings].modify_date
      ,[buildings].addr_street_id
      ,[buildings].addr_street_id2
      ,[buildings].addr_distr_new_id
      ,[buildings].[unique_id] 
	  ,[buildings].[report_id]

FROM
    reports1nf_buildings buildings
    OUTER APPLY (SELECT TOP 1 id FROM privatization p WHERE p.building_id = buildings.id AND p.privat_state_id = 1) priv
    LEFT OUTER JOIN dict_districts2 ON buildings.addr_distr_new_id = dict_districts2.id
    LEFT OUTER JOIN dict_tech_state ON buildings.tech_condition_id = dict_tech_state.id
    LEFT OUTER JOIN dict_object_kind ON buildings.object_kind_id = dict_object_kind.id
    LEFT OUTER JOIN dict_object_type ON buildings.object_type_id = dict_object_type.id
    LEFT OUTER JOIN dict_facade ON buildings.facade_id = dict_facade.id
    LEFT OUTER JOIN dict_history ON buildings.history_id = dict_history.id
    LEFT OUTER JOIN dict_oatuu ON buildings.oatuu_id = dict_oatuu.id
WHERE
	(master_building_id IS NULL)
