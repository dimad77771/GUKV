
CREATE VIEW view_building_renters_no_agreement
AS
SELECT DISTINCT
    renter.id AS 'org_renter_id',
    decs.building_id
FROM
    arenda_decisions decs
    INNER JOIN arenda_applications appl ON appl.id = decs.application_id
    INNER JOIN doc_appendices app ON app.id = decs.appendix_rasp_id
    INNER JOIN documents doc ON doc.id = app.doc_id
    LEFT OUTER JOIN dict_rent_decisions decision ON decision.id = decs.decision_id
    LEFT OUTER JOIN organizations renter ON renter.id = decs.org_renter_id
WHERE
    decs.is_subarenda <> 2 AND
    decs.decision_id IN (21, 22, 24) AND
    NOT (appl.appl_letter_date IS NULL) AND
    NOT EXISTS (SELECT ar.id FROM arenda ar INNER JOIN link_arenda_2_decisions lnk ON lnk.arenda_id = ar.id WHERE
        ar.org_renter_id = renter.id AND RTRIM(LTRIM(lnk.doc_num)) = RTRIM(LTRIM(doc.doc_num)))
