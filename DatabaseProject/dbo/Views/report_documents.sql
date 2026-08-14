
CREATE VIEW [report_documents]
AS
SELECT
       [dep].id AS 'link_id'
      ,[parent_doc].[id] AS 'parent_id'
      ,[parent_doc].[kind] AS 'parent_kind'
      ,[parent_doc].[general_kind] AS 'parent_general_kind'
      ,CASE WHEN ((parent_doc.kind LIKE 'РОЗПОРЯДЖЕННЯ%') OR (parent_doc.kind LIKE 'РІШЕННЯ%')) THEN N'ТАК' ELSE N'НІ' END AS 'parent_is_rozp'
      ,[parent_doc].[doc_date] AS 'parent_date'
      ,YEAR(parent_doc.doc_date) AS 'parent_date_year'
      ,DATEPART(Quarter, parent_doc.doc_date)  AS 'parent_date_quarter'
      ,[parent_doc].[doc_num] AS 'parent_num'
      ,[parent_doc].[topic] AS 'parent_topic'
      ,[parent_doc].[note] AS 'parent_note'
      ,[parent_doc].[search_name] AS 'parent_search_name'
      ,[parent_doc].[receive_date] AS 'parent_receive_date'
      ,[parent_doc].[commission] AS 'parent_commission'
      ,[parent_doc].[source] AS 'parent_source'
      ,[parent_doc].[state] AS 'parent_state'
      ,[parent_doc].[summa] AS 'parent_summa'
      ,[parent_doc].[summa_zalishkova] AS 'parent_summa_zalishkova'
      ,[parent_doc].[is_text_exists] AS 'parent_text_exists'
      ,[parent_doc].[extern_doc_id] AS 'parent_extern_doc_id'
      ,[child_doc].[id] AS 'child_id'
      ,[child_doc].[kind] AS 'child_kind'
      ,[child_doc].[general_kind] AS 'child_general_kind'
      ,[child_doc].[doc_date] AS 'child_date'
      ,YEAR(child_doc.doc_date) AS 'child_date_year'
      ,DATEPART(Quarter, child_doc.doc_date)  AS 'child_date_quarter'
      ,[child_doc].[doc_num] AS 'child_num'
      ,[child_doc].[topic] AS 'child_topic'
      ,[child_doc].[note] AS 'child_note'
      ,[child_doc].[search_name] AS 'child_search_name'
      ,[child_doc].[receive_date] AS 'child_receive_date'
      ,[child_doc].[commission] AS 'child_commission'
      ,[child_doc].[source] AS 'child_source'
      ,[child_doc].[state] AS 'child_state'
      ,[child_doc].[summa] AS 'child_summa'
      ,[child_doc].[summa_zalishkova] AS 'child_summa_zalishkova'
      ,[child_doc].[is_text_exists] AS 'child_text_exists'
      ,[child_doc].[extern_doc_id] AS 'child_extern_doc_id'
FROM
      view_documents parent_doc
      LEFT OUTER JOIN doc_dependencies dep ON dep.master_doc_id = parent_doc.id
      LEFT OUTER JOIN view_documents child_doc ON child_doc.id = dep.slave_doc_id
/* WHERE
      NOT (parent_doc.id IN (SELECT slave_doc_id FROM doc_dependencies)) */
