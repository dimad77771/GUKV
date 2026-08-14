
CREATE VIEW [view_arch_buildings]
AS
SELECT [arch_buildings].[archive_id],
       [arch_buildings].[id] AS 'building_id'
      ,[arch_buildings].[street_full_name]
      ,[dict_districts2].[name] AS 'district'
      ,(LTRIM(RTRIM(isnull(arch_buildings.addr_nomer1, ''))) + ' ' +
        LTRIM(RTRIM(isnull(arch_buildings.addr_nomer2, ''))) + ' ' +
        LTRIM(RTRIM(isnull(arch_buildings.addr_nomer3, '')))) AS 'addr_nomer'
      ,LTRIM(RTRIM(isnull(arch_buildings.addr_nomer1, ''))) AS 'addr_nomer1'
      ,[arch_buildings].[addr_zip_code]
      ,[arch_buildings].[addr_misc]
      ,[dict_tech_state].[name] AS 'condition'
      ,[arch_buildings].[num_floors]
      ,[arch_buildings].[construct_year]
      ,[arch_buildings].[bti_code]
      ,[dict_history].[name] AS 'history'
      ,[dict_object_type].[name] AS 'object_type'
      ,[dict_object_kind].[name] AS 'object_kind'
      ,[arch_buildings].[cost_balans]
      ,[arch_buildings].[sqr_total]
      ,[arch_buildings].[sqr_habit]
      ,[arch_buildings].[sqr_rented]
      ,[arch_buildings].[sqr_zagal]
      ,[arch_buildings].[sqr_for_rent]
      ,[arch_buildings].[sqr_non_habit]
      ,[arch_buildings].[sqr_pidval]
      ,[arch_buildings].[additional_info]
      ,[arch_buildings].[oatuu_id] AS 'oatuu_code'
      ,[dict_facade].[name] AS 'facade'
      ,[dict_oatuu].[name] AS 'region'
      ,[arch_buildings].sqr_dk AS 'sqr_object_dk'
      ,[arch_buildings].sqr_mk AS 'sqr_object_mk'
      ,[arch_buildings].sqr_rk AS 'sqr_object_rk'
      ,[arch_buildings].sqr_other AS 'sqr_object_other'
      ,[arch_buildings].is_deleted AS 'building_deleted'
      ,CASE WHEN arch_buildings.is_deleted = 1 THEN N'ТАК' ELSE N'НІ' END AS 'building_deleted_txt'
      ,[arch_buildings].modified_by
      ,[arch_buildings].modify_date
      ,[arch_buildings].addr_distr_new_id
FROM
    arch_buildings
    LEFT OUTER JOIN dict_districts2 ON arch_buildings.addr_distr_new_id = dict_districts2.id
    LEFT OUTER JOIN dict_tech_state ON arch_buildings.tech_condition_id = dict_tech_state.id
    LEFT OUTER JOIN dict_object_kind ON arch_buildings.object_kind_id = dict_object_kind.id
    LEFT OUTER JOIN dict_object_type ON arch_buildings.object_type_id = dict_object_type.id
    LEFT OUTER JOIN dict_facade ON arch_buildings.facade_id = dict_facade.id
    LEFT OUTER JOIN dict_history ON arch_buildings.history_id = dict_history.id
    LEFT OUTER JOIN dict_oatuu ON arch_buildings.oatuu_id = dict_oatuu.id
WHERE
	(master_building_id IS NULL)
