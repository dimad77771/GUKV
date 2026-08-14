
CREATE VIEW [view_doc_links]
AS
SELECT
    dep.id AS 'link_id',
    dep.master_doc_id,
    dep.slave_doc_id,
    parent.doc_num AS 'parent_num',
    parent.doc_date AS 'parent_date',
    parent.topic AS 'parent_topic',
    parent.search_name AS 'parent_search_name',
    parent.kind_id AS 'parent_kind',
    parent.general_kind_id AS 'parent_general_kind',
    parent.is_text_exists AS 'parent_text_exists',
    parent.is_priv_rishen AS 'parent_priv_rishen',
    child.doc_num AS 'child_num',
    child.doc_date AS 'child_date',
    child.topic AS 'child_topic',
    child.search_name AS 'child_search_name',
    child.kind_id AS 'child_kind',
    child.general_kind_id AS 'child_general_kind',
    child.is_text_exists AS 'child_text_exists'
FROM
    doc_dependencies AS dep
    INNER JOIN documents AS parent ON dep.master_doc_id = parent.id
    INNER JOIN documents AS child ON dep.slave_doc_id = child.id
WHERE    
    (NOT dep.master_doc_id IS NULL) AND
    (NOT dep.slave_doc_id IS NULL)
