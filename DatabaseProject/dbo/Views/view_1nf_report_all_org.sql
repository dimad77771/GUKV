
CREATE VIEW [view_1nf_report_all_org]
AS
SELECT
    DISTINCT organization_id
FROM
    view_balans
WHERE
    org_ownership_int IN (32,33,34) OR form_ownership_int IN (32,33,34)
UNION
SELECT
    DISTINCT ar.org_renter_id AS 'organization_id'
FROM
    view_arenda ar
    INNER JOIN organizations org ON org.id = ar.org_renter_id
WHERE
    balans_form_ownership_int IN (32,33,34) AND
    org.form_ownership_id IN (32,33,34)
