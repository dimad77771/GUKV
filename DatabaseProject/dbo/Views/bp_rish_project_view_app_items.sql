
CREATE VIEW bp_rish_project_view_app_items
AS
SELECT
    itm.id AS 'item_id'
   ,itm.appendix_id
   ,itm.app_item_index
   ,itm.app_item_text
   ,itm.building_id
   ,itm.balans_id
   ,b.street_full_name + ' ' + b.addr_nomer AS 'building_addr'
   ,bal.org_short_name + ', ' + CONVERT(VARCHAR(32), bal.sqr_total) + ' кв.м.' AS 'balans_descr'
FROM
    bp_rish_project_app_item itm
    LEFT OUTER JOIN buildings b ON b.id = itm.building_id
    LEFT OUTER JOIN view_balans bal ON bal.balans_id = itm.balans_id
