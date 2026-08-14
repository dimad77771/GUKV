
CREATE VIEW view_experts
AS
SELECT
    ex.id AS 'expert_id',
    ex.full_name,
    ex.short_name,
    ex.fio_boss,
    ex.tel_boss,
    dict_districts2.name AS 'district',
    dict_streets.name AS 'street_full_name',
    addr_zip_code,
    addr_number AS 'addr_nomer',
    certificate_num,
    certificate_date,
    certificate_end_date,
    case_num,
    addr_full,
    note
FROM
    dict_expert ex
    LEFT OUTER JOIN dict_districts2 ON dict_districts2.id = ex.addr_district_id
    LEFT OUTER JOIN dict_streets ON dict_streets.id = ex.addr_street_id
WHERE
    ex.is_deleted <> 1
