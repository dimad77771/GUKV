
CREATE VIEW [dbo].[view_organizations_name]
AS
SELECT
    [organizations].[id] AS 'organization_id'
   ,case when [short_name] <> '' then [short_name] else [full_name] end as s_name
   ,[organizations].[full_name]
   ,[organizations].[short_name]
   ,[organizations].[zkpo_code]
FROM [dbo].[organizations]

--SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
--SELECT * FROM [freecycle_step_dict]
--SELECT * FROM [freecycle_step]
--SELECT org.* FROM organizations org LEFT JOIN reports1nf_arenda agr ON agr.org_renter_id = org.id
--SELECT org.* FROM organizations org
--                    LEFT JOIN freecycle_orendar agr
--                    ON agr.org_orendar_id = org.id
--                    WHERE agr.freecycle_orendar_id = 2
--SELECT org.* FROM organizations org where id = 1
--SELECT org.* FROM organizations org 
--                    LEFT JOIN freecycle_orendar agr
--                    ON agr.org_orendar_id = org.id and agr.freecycle_orendar_id = -1
--                    WHERE agr.freecycle_orendar_id = -1
--ToDatabase();
--SELECT org.* FROM organizations org where zkpo_code = '22222222'
--select * from [freecycle_step_dict]
--select * from [freecycle_step]
--select * from [organizations]
--select short_name, full_name from [organizations] where full_name <> '' order by short_name