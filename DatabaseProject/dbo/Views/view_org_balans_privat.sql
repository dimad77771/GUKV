
CREATE VIEW [view_org_balans_privat]
AS
SELECT
	[orgbal].[balans_id]
   ,[orgbal].[building_id]
   ,[orgbal].[organization_id]
   ,[orgbal].[org_full_name]
   ,[orgbal].[org_short_name]
   ,[orgbal].[org_zkpo_code]
   ,[orgbal].[sqr_balans]
   ,[orgbal].[cost_balans]
   ,[orgbal].[cost_extert_1m]
   ,[orgbal].[cost_expert_total]
   ,[orgbal].[purpose_group]
   ,[orgbal].[purpose]
   ,[orgbal].[ownership_type]
   ,[orgbal].[otdel_gukv]
   ,[orgbal].[street_full_name]
   ,[orgbal].[addr_nomer]
   ,[orgbal].[district]
   ,[orgbal].[addr_zip_code]
   ,[orgbal].[condition]
   ,[orgbal].[bti_code]
   ,[orgbal].[history]
   ,[orgbal].[object_type]
   ,[orgbal].[object_kind]
   ,[orgbal].[sqr_total]
   ,[orgbal].[facade]
   ,[pr].[obj_name] AS 'object_name'
   ,[dict_privat_obj_group].[name] AS 'object_group'
FROM
    [GUKV].[dbo].[view_org_balans] AS orgbal
    CROSS APPLY (SELECT TOP 1 building_id, obj_name, obj_group_id FROM privatization WHERE privatization.building_id = orgbal.building_id) pr
    INNER JOIN dict_privat_obj_group ON pr.obj_group_id = dict_privat_obj_group.id    
