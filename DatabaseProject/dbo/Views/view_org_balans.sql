

CREATE VIEW [dbo].[view_org_balans]
AS
SELECT
	[bal].balans_id
   ,[bal].[building_id]
   ,[bal].[organization_id]
   ,[bal].[org_full_name]
   ,[bal].[org_short_name]
   ,[bal].[org_zkpo_code]
   ,[bal].[sqr_total] AS 'sqr_balans'
   ,[bal].[cost_balans]
   ,[bal].[cost_expert_1m]
   ,[bal].[cost_expert_total]
   ,[bal].[purpose_group]
   ,[bal].[purpose]
   ,[bal].[ownership_type]
   ,[bal].[otdel_gukv]
   ,[b].[street_full_name]
   ,[b].[addr_nomer]
   ,[b].[district]
   ,[b].[addr_zip_code]
   ,[b].[condition]
   ,[b].[bti_code]
   ,[b].[history]
   ,[b].[object_type]
   ,[b].[object_kind]
   ,[b].[sqr_total]
   ,[b].[facade]
FROM
    [GUKV].[dbo].[view_balans] AS bal
    INNER JOIN view_buildings b ON bal.building_id = b.building_id

