
CREATE VIEW [view_privatization_doc_links]
AS
SELECT
    dl.link_id, 
    dl.master_doc_id, 
    dl.slave_doc_id, 
    dl.parent_num, 
    dl.parent_date,
    YEAR(dl.parent_date) AS 'parent_date_year',
    DATEPART(Quarter, dl.parent_date)  AS 'parent_date_quarter',
    dl.parent_topic, 
    dl.parent_search_name, 
    dl.parent_kind AS 'parent_kind_id',
    p_kind.name AS 'parent_kind',
    dl.parent_text_exists, 
    dl.child_num, 
    dl.child_date, 
    YEAR(dl.child_date) AS 'child_date_year',
    DATEPART(Quarter, dl.child_date)  AS 'child_date_quarter',
    dl.child_topic, 
    dl.child_search_name, 
    dl.child_kind AS 'child_kind_id',
    c_kind.name AS 'child_kind',
    dl.child_text_exists
FROM
    view_doc_links dl
    LEFT OUTER JOIN dict_doc_kind p_kind ON p_kind.id = dl.parent_kind
    LEFT OUTER JOIN dict_doc_kind c_kind ON c_kind.id = dl.child_kind
WHERE
    (dl.parent_priv_rishen = 1) AND
    ((c_kind.name LIKE 'РІШЕННЯ%') OR (c_kind.name LIKE 'РОЗПОРЯДЖЕННЯ%'))
