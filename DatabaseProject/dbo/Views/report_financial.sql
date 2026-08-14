
CREATE VIEW [report_financial]
AS
SELECT
       [org].[organization_id]
      ,[full_name]
      ,[short_name]
      ,[zkpo_code]
      ,COALESCE(old_industry, N'НЕ ВИЗНАЧЕНО') AS 'old_industry'
      ,COALESCE(old_occupation, N'НЕ ВИЗНАЧЕНО') AS 'old_occupation'
      ,[old_organ]
      ,[status]
      ,COALESCE(occupation, N'НЕ ВИЗНАЧЕНО') AS 'occupation'
      ,[form_gosp]
      ,[form_of_ownership]
      ,[gosp_struct]
      ,COALESCE(industry, N'НЕ ВИЗНАЧЕНО') AS 'industry'
      ,[vedomstvo]
      ,[org_form]
      ,[addr_district]
      ,[addr_street_name]
      ,[addr_nomer]
      ,[addr_korpus]
      ,[addr_zip_code]
      ,[director_fio]
      ,[director_phone]
      ,[buhgalter_fio]
      ,[buhgalter_phone]
      ,[fax]
      ,[registration_auth]
      ,[registration_num]
      ,[registration_date]
      ,[registration_svidot]
      ,[kved_code]
      ,[koatuu]
      ,[otdel_gukv]
      ,[mayno]
      ,[is_liquidated]
      ,[liquidation_date]
      ,[contact_email]
      ,[contact_posada]
      ,[sfera_upr]
      ,org.budg_payments_rate AS 'budg_payments_rate'
      ,CASE WHEN LEN(addr_zip_code) > 0 THEN addr_zip_code + ', ' ELSE '' END +
       LTRIM(RTRIM(addr_street_name)) + ' ' + LTRIM(RTRIM(addr_nomer)) + COALESCE(' ' + addr_korpus, '') AS 'addr_address'
	  , CASE WHEN is_under_closing = 1 THEN N'ТАК' ELSE N'НІ' END AS [is_under_closing]       
FROM
       view_organizations org
WHERE
       (org.origin_db = 2) AND
       ((org.end_state_date IS NULL) OR (org.end_state_date > GETDATE()))
