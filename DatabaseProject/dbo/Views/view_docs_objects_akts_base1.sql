
CREATE VIEW [view_docs_objects_akts_base1]
AS
SELECT
       [dl].[link_id]
      ,[dl].[master_doc_id]
      ,[dl].[slave_doc_id]
      ,[dl].[parent_num]
      ,[dl].[parent_date]
      ,[dl].[parent_topic]
      ,[dl].[parent_search_name]
      ,[dl].[parent_kind]
      ,[dl].[parent_general_kind]
      ,[dl].[parent_text_exists]
      ,[dl].[child_num]
      ,[dl].[child_date]
      ,[dl].[child_topic]
      ,[dl].[child_search_name]
      ,[dl].[child_kind]
      ,[dl].[child_general_kind]
      ,[dl].[child_text_exists]
      ,[bd].building_id AS 'akt_building_id'
FROM
    view_doc_links dl
    INNER JOIN building_docs bd ON bd.document_id = dl.slave_doc_id
WHERE
    (dl.child_kind = 3) OR (dl.child_kind = 36) OR (dl.child_kind = 47)
