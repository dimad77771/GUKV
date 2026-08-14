
CREATE VIEW [view_documents]
AS
SELECT [doc].[id]
      ,[doc].[kind_id]
      ,[dict_doc_kind].[name] AS 'kind'
      ,[doc].[general_kind_id]
      ,[dict_doc_general_kind].[name] AS 'general_kind'
      ,[doc].[doc_date]
      ,[doc].[doc_num]
      ,[doc].[topic]
      ,[doc].[note]
      ,[doc].[search_name]
      ,[doc].[receive_date]
      ,[dict_doc_commission].[name] AS 'commission'
      ,[dict_doc_source].[name] AS 'source'
      ,[dict_doc_state].[name] AS 'state'
      ,[doc].[summa]
      ,[doc].[summa_zalishkova]
      ,[doc].[is_text_exists]
      ,[doc].[is_priv_rishen]
      ,[doc].[extern_doc_id]
      ,COALESCE(dict_doc_kind.name, N'Документ') + N' № ' + COALESCE(RTRIM(doc_num), N'Б/Н') +
       COALESCE(N' вiд ' + CONVERT(VARCHAR(32), doc_date, 104), N'') AS 'display_name'
FROM
      documents doc
      LEFT OUTER JOIN dict_doc_kind ON dict_doc_kind.id = doc.kind_id
      LEFT OUTER JOIN dict_doc_general_kind ON dict_doc_general_kind.id = doc.general_kind_id
      LEFT OUTER JOIN dict_doc_commission ON dict_doc_commission.id = doc.commission_id
      LEFT OUTER JOIN dict_doc_source ON dict_doc_source.id = doc.source_id
      LEFT OUTER JOIN dict_doc_state ON dict_doc_state.id = doc.state_id
