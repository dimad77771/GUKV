
CREATE VIEW [view_arenda_link_2_decisions]
AS
SELECT
    link.id AS 'link_id',
    link.arenda_id,
    link.rishen_id,
    link.ord,
    link.modified_by,
    link.modify_date,
    link.doc_num,
    link.doc_date,
    link.doc_dodatok,
    link.doc_punkt,
    link.purpose_str,
    link.rent_square,
    dict_rent_decisions.name AS 'decision',
    link.doc_raspor_id,
    link.pidstava,
    COALESCE(pidstava, N'Документ') + N' № ' + COALESCE(RTRIM(doc_num), N'Б/Н') +
        COALESCE(N' вiд ' + CONVERT(VARCHAR(32), doc_date, 104), N'') AS 'doc_display_name'
FROM
    link_arenda_2_decisions link
    LEFT OUTER JOIN dict_rent_decisions ON dict_rent_decisions.id = link.decision_id
