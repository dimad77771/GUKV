
/******************************************************************************/
/*                              System tables                                 */
/******************************************************************************/

/* reports */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[reports]') AND type in (N'U'))
DROP TABLE [dbo].[reports]

CREATE TABLE reports (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	organization_id    INTEGER,
	report_year        INTEGER,
	save_date          DATETIME, /* When this report was saved by the client */
	import_date        DATE,
	folder_name        VARCHAR(MAX),
	folder_path        VARCHAR(MAX),
	load_status        INTEGER DEFAULT 0, /* 0 = imported, 1 = exported to 1NF */
	validation_status  INTEGER DEFAULT 0, /* 0 = not validated yet, 1 = validation passed, -1 = validation failed */
	data_status        INTEGER DEFAULT 0, /* 0 = not checked, -1 = outdated, 1 = latest and should be imported */
	modified_by        VARCHAR(64),
	modify_date        DATE
);

ALTER TABLE reports ADD PRIMARY KEY (id);

/* validation_errors */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[validation_errors]') AND type in (N'U'))
DROP TABLE [dbo].[validation_errors]

CREATE TABLE validation_errors (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	report_id          INTEGER,
	obj_id             INTEGER,
	error_kind         INTEGER,
	token              VARCHAR(MAX), /* Unique identifier; combines report_id, obj_id and error_kind */
	object_descr       VARCHAR(MAX), /* Entity description (object, organization, etc) */
	error_msg          VARCHAR(MAX),
	is_overridden      INTEGER DEFAULT 0,
	modified_by        VARCHAR(64),
	modify_date        DATE
);

ALTER TABLE validation_errors ADD PRIMARY KEY (id);

/* duplicate_zkpo */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[duplicate_zkpo]') AND type in (N'U'))
DROP TABLE [dbo].[duplicate_zkpo]

CREATE TABLE duplicate_zkpo (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	zkpo               VARCHAR(32),
	org_id             INTEGER
);

ALTER TABLE duplicate_zkpo ADD PRIMARY KEY (id);

/* manual_zkpo_match */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[manual_zkpo_match]') AND type in (N'U'))
DROP TABLE [dbo].[manual_zkpo_match]

CREATE TABLE manual_zkpo_match (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	zkpo               VARCHAR(32),
	full_name          VARCHAR(MAX),
	org_id_1nf         INTEGER
);

ALTER TABLE manual_zkpo_match ADD PRIMARY KEY (id);

/******************************************************************************/
/*                          Source database tables                            */
/******************************************************************************/

/* object_1nf */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[object_1nf]') AND type in (N'U'))
DROP TABLE [dbo].[object_1nf]

CREATE TABLE object_1nf (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	report_id          INTEGER, /* ID of the report from 'reports' table */
	match_id           INTEGER, /* ID of the matched object in 1NF database */
	create_id          INTEGER, /* ID of the created object in 1NF database, if no match was found */

	/* The following fields are imported from the source database */
	[object_kod] [int] NULL,
	[address] [varchar](max) NULL,
	[unloading] [int] NULL,
	[edited] [int] NULL,
	[ulkod2] [int] NULL,
	[ikod] [varchar](128) NULL,
	[budyear] [int] NULL,
	[history] [int] NULL,
	[newdistr] [int] NULL,
	[ulkod] [int] NULL,
	[vidobj] [int] NULL,
	[adrdop] [varchar](max) NULL,
	[typobj] [int] NULL,
	[nomer1] [varchar](64) NULL,
	[texstan_obj] [int] NULL,
	[nomer2] [varchar](64) NULL,
	[oznaka_bud] [int] NULL,
	[pnum] [varchar](64) NULL,
	[nomer3] [varchar](64) NULL,
	[dodat_vid] [varchar](max) NULL,
	[szag] [numeric](9, 2) NULL,
	[v_nej] [numeric](9, 2) NULL,
	[v_jil] [numeric](9, 2) NULL,
	[s_gurt] [numeric](9, 2) NULL
);

ALTER TABLE object_1nf ADD PRIMARY KEY (id);

/* sorg_1nf */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sorg_1nf]') AND type in (N'U'))
DROP TABLE [dbo].[sorg_1nf]

CREATE TABLE sorg_1nf (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	report_id          INTEGER, /* ID of the report from 'reports' table */
	match_id           INTEGER, /* ID of the matched organization in 1NF database */
	create_id          INTEGER, /* ID of the created organization in 1NF database, if no match was found */

	/* The following fields are imported from the source database */
	[kod_obj] [int] NULL,
	[main_org] [int] NULL,
	[unloading] [int] NULL,
	[is_arend] [int] NULL,
	[edited] [int] NULL,
	[full_name_obj] [varchar](255) NULL,
	[short_name_obj] [varchar](255) NULL,
	[post_index] [varchar](24) NULL,
	[name_ul] [varchar](100) NULL,
	[nomer_doma] [varchar](30) NULL,
	[nomer_korpus] [varchar](20) NULL,
	[kod_zkpo] [varchar](16) NULL,
	[fio_boss] [varchar](70) NULL,
	[post_boss] [varchar](70) NULL,
	[tel_boss] [varchar](24) NULL,
	[koatuu] [varchar](16) NULL,
	[kod_rayon2] [int] NULL,
	[kod_status] [int] NULL,
	[kod_form_vlasn] [int] NULL,
	[kod_vidom_nal] [int] NULL,
	[kod_organ] [int] NULL,
	[kod_galuz] [int] NULL,
	[kod_vid_dial] [int] NULL,
	[kod_kved] [varchar](8) NULL,
	[kod_org_form] [int] NULL,
	[kod_form_gosp] [int] NULL,
	[persent_akcia] [varchar](50) NULL,
	[pl_zagal] [numeric](9, 2) NULL,
	[pl_balans] [numeric](9, 2) NULL,
	[pl_virob_prizn] [numeric](9, 2) NULL,
	[pl_not_virob_prizn] [numeric](9, 2) NULL,
	[pl_arenduetsya] [numeric](9, 2) NULL,
	[pl_nadana_v_a] [numeric](9, 2) NULL,
	[pl_zn_bal] [numeric](9, 2) NULL,
	[prodaj] [numeric](9, 2) NULL,
	[znes_bud] [numeric](9, 2) NULL,
	[pered_na_bal] [numeric](9, 2) NULL	
);

ALTER TABLE sorg_1nf ADD PRIMARY KEY (id);

/* balans_1nf */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[balans_1nf]') AND type in (N'U'))
DROP TABLE [dbo].[balans_1nf]

CREATE TABLE balans_1nf (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	report_id          INTEGER, /* ID of the report from 'reports' table */
	match_id           INTEGER, /* ID of the matched entry in 1NF database */
	create_id          INTEGER, /* ID of the created entry in 1NF database, if no match was found */

	/* The following fields are imported from the source database */
	[balans_id] [int] NULL,
	[object_id] [int] NULL,
	[organization_id] [int] NULL,
	[deleted] [int] NULL,
	[unloading] [int] NULL,
	[edited] [int] NULL,
	[bal_id_del] [int] NULL,
	[spos_vidch] [int] NULL,
	[pl_vidch] [numeric](9, 2) NULL,
	[vart_vidch] [numeric](15, 3) NULL,
	[dockind_vidch] [int] NULL,
	[docnum_vidch] [varchar](64) NULL,
	[docdate_vidch] [date] NULL,
	[form_vlasn] [int] NULL,
	[pravo] [int] NULL,
	[dockind_korist] [int] NULL,
	[docnum_korist] [varchar](64) NULL,
	[docdate_korist] [date] NULL,
	[dockind_bal] [int] NULL,
	[docnum_bal] [varchar](64) NULL,
	[docdate_bal] [date] NULL,
	[kindobj] [int] NULL,
	[typeobj] [int] NULL,
	[date_bti] [date] NULL,
	[obj_kodbti] [varchar](64) NULL,
	[reesno] [varchar](64) NULL,
	[grpurp] [int] NULL,
	[purpose] [int] NULL,
	[purp_str] [varchar](max) NULL,
	[floats] [varchar](128) NULL,
	[texstan_bal] [int] NULL,
	[sqr_zag] [numeric](9, 2) NULL,
	[sqr_nej] [numeric](9, 2) NULL,
	[sqr_gurtoj] [numeric](9, 2) NULL,
	[sqr_kor] [numeric](9, 2) NULL,
	[sqr_pidv] [numeric](9, 2) NULL,
	[sqr_vlas] [numeric](9, 2) NULL,
	[sqr_free] [numeric](9, 2) NULL,
	[v_dog_orenda] [int] NULL,
	[v_sqr_orenda] [numeric](9, 2) NULL,
	[balans_vartist] [numeric](15, 3) NULL,
	[zal_vart] [numeric](15, 3) NULL,
	[znos] [numeric](15, 3) NULL,
	[znos_date] [date] NULL,
	[vartist_rink] [numeric](15, 3) NULL,
	[rink_vart_dt] [date] NULL,
	[vartist_exp_v] [numeric](15, 3) NULL,
	[dtexp] [date] NULL	
);

ALTER TABLE balans_1nf ADD PRIMARY KEY (id);
	
/* arenda_1nf */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[arenda_1nf]') AND type in (N'U'))
DROP TABLE [dbo].[arenda_1nf]

CREATE TABLE arenda_1nf (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	report_id          INTEGER, /* ID of the report from 'reports' table */
	match_id           INTEGER, /* ID of the matched entry in 1NF database */
	create_id          INTEGER, /* ID of the created entry in 1NF database, if no match was found */

	/* The following fields are imported from the source database */
	[id_arenda] [int] NOT NULL,
	[orgiv] [int] NULL,
	[balid] [int] NULL,
	[org_kod] [int] NULL,
	[balans_kod] [int] NULL,
	[object_kod] [int] NULL,
	[deleted] [int] NULL,
	[unloading] [int] NULL,
	[edited] [int] NULL,
	[dogkind] [int] NULL,
	[subar] [int] NULL,
	[dognum] [varchar](64) NULL,
	[dogdate] [date] NULL,
	[start_orenda] [date] NULL,
	[finish_orenda] [date] NULL,
	[finish_orenda_fact] [date] NULL,
	[pidstava_fact] [varchar](max) NULL,
	[square] [numeric](9, 2) NULL,
	[plata_dogovor] [numeric](15, 3) NULL,
	[vartist_exp] [numeric](15, 3) NULL,
	[dtexp_dog] [date] NULL,
	[kilk_prim] [int] NULL
);

ALTER TABLE arenda_1nf ADD PRIMARY KEY (id);

/* arenda_prim */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[arenda_prim]') AND type in (N'U'))
DROP TABLE [dbo].[arenda_prim]

CREATE TABLE arenda_prim (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	report_id          INTEGER, /* ID of the report from 'reports' table */

	/* The following fields are imported from the source database */
	[id_arenda_prim] [int] NULL,
	[arenda] [int] NULL,
	[deleted] [int] NULL,
	[unloading] [int] NULL,
	[edited] [int] NULL,
	[num] [int] NULL,
	[square] [numeric](9, 2) NULL,
	[descript] [varchar](max) NULL,
	[grpurp] [int] NULL,
	[purp] [int] NULL,
	[purp_str] [varchar](max) NULL,
	[vartist_exp] [numeric](15, 3) NULL,
	[dtexp] [date] NULL,
	[ar_stavka] [numeric](15, 3) NULL,
	[price] [numeric](15, 3) NULL,
	[kod_vidplata] [int] NULL,
	[narah] [numeric](15, 3) NULL	
);	

ALTER TABLE arenda_prim ADD PRIMARY KEY (id);
	
/* arrish */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[arrish]') AND type in (N'U'))
DROP TABLE [dbo].[arrish]

CREATE TABLE arrish (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	report_id          INTEGER, /* ID of the report from 'reports' table */
	match_id           INTEGER, /* ID of the matched entry in 1NF database */
	create_id          INTEGER, /* ID of the created entry in 1NF database, if no match was found */

	/* The following fields are imported from the source database */
	[id_arrish] [int] NULL,
	[arenda] [int] NULL,
	[rishen] [int] NULL,
	[deleted] [int] NULL,
	[unloading] [int] NULL,
	[edited] [int] NULL,
	[pidstava] [varchar](max) NULL,
	[nom] [varchar](64) NULL,
	[dt_doc] [date] NULL
);	
	
ALTER TABLE arrish ADD PRIMARY KEY (id);

/* rishen */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rishen]') AND type in (N'U'))
DROP TABLE [dbo].[rishen]

CREATE TABLE rishen (
    id                 INTEGER IDENTITY(1,1), /* Auto-generated ID */
	report_id          INTEGER, /* ID of the report from 'reports' table */

	/* The following fields are imported from the source database */
	[kod] [int] NULL,
	[nom] [varchar](64) NULL,
	[dt] [date] NULL,
	[name] [varchar](max) NULL,
	[system_ar] [int] NULL,
	[balans_id] [int] NULL,
	[object] [int] NULL,
	[org] [int] NULL,
	[s_zamov] [numeric](9, 2) NULL
);

ALTER TABLE rishen ADD PRIMARY KEY (id);

/* sgrpurpose */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sgrpurpose]') AND type in (N'U'))
DROP TABLE [dbo].[sgrpurpose]

/* shistory */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[shistory]') AND type in (N'U'))
DROP TABLE [dbo].[shistory]

/* skinddog */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[skinddog]') AND type in (N'U'))
DROP TABLE [dbo].[skinddog]

/* skinddok */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[skinddok]') AND type in (N'U'))
DROP TABLE [dbo].[skinddok]

/* skindobj */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[skindobj]') AND type in (N'U'))
DROP TABLE [dbo].[skindobj]

/* spravo */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spravo]') AND type in (N'U'))
DROP TABLE [dbo].[spravo]

/* spurpose */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spurpose]') AND type in (N'U'))
DROP TABLE [dbo].[spurpose]

/* ssubar */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ssubar]') AND type in (N'U'))
DROP TABLE [dbo].[ssubar]

/* stexstan */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[stexstan]') AND type in (N'U'))
DROP TABLE [dbo].[stexstan]

/* stypeobj */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[stypeobj]') AND type in (N'U'))
DROP TABLE [dbo].[stypeobj]

/* sul */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sul]') AND type in (N'U'))
DROP TABLE [dbo].[sul]

/* s_form_gosp */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_form_gosp]') AND type in (N'U'))
DROP TABLE [dbo].[s_form_gosp]

/* s_form_vlasn */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_form_vlasn]') AND type in (N'U'))
DROP TABLE [dbo].[s_form_vlasn]

/* s_galuz */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_galuz]') AND type in (N'U'))
DROP TABLE [dbo].[s_galuz]

/* s_kved */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_kved]') AND type in (N'U'))
DROP TABLE [dbo].[s_kved]

/* s_organ */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_organ]') AND type in (N'U'))
DROP TABLE [dbo].[s_organ]

/* s_org_form */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_org_form]') AND type in (N'U'))
DROP TABLE [dbo].[s_org_form]

/* s_oznaka_bud */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_oznaka_bud]') AND type in (N'U'))
DROP TABLE [dbo].[s_oznaka_bud]

/* s_rayon2 */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_rayon2]') AND type in (N'U'))
DROP TABLE [dbo].[s_rayon2]

/* s_status */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_status]') AND type in (N'U'))
DROP TABLE [dbo].[s_status]

/* s_vidch_type */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_vidch_type]') AND type in (N'U'))
DROP TABLE [dbo].[s_vidch_type]

/* s_vidom_nal */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_vidom_nal]') AND type in (N'U'))
DROP TABLE [dbo].[s_vidom_nal]

/* s_vidplata */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_vidplata]') AND type in (N'U'))
DROP TABLE [dbo].[s_vidplata]

/* s_vid_dial */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[s_vid_dial]') AND type in (N'U'))
DROP TABLE [dbo].[s_vid_dial]

GO

/******************************************************************************/
/*                                   Views                                    */
/******************************************************************************/

/* view_error_count */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_error_count]'))
DROP VIEW [dbo].[view_error_count]

GO

CREATE VIEW [view_error_count]
AS
SELECT
    report_id,
    COUNT(*) AS 'error_count'
FROM
    validation_errors
GROUP BY
    report_id

GO

/* view_overridden_error_count */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_overridden_error_count]'))
DROP VIEW [dbo].[view_overridden_error_count]

GO

CREATE VIEW [view_overridden_error_count]
AS
SELECT
    report_id,
    COUNT(*) AS 'error_count'
FROM
    validation_errors
WHERE
    is_overridden = 1
GROUP BY
    report_id

GO

/* view_reports */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_reports]'))
DROP VIEW [dbo].[view_reports]

GO

CREATE VIEW [view_reports]
AS
SELECT
    rep.id as 'report_id',
    COALESCE(org1nf.kod_zkpo, org.zkpo_code) AS 'zkpo_code',
    COALESCE(org1nf.full_name_obj, org.full_name) AS 'full_name',
    rep.import_date,
    rep.folder_path,   
    rep.validation_status,
    CASE
        WHEN rep.load_status = 1 THEN N'ЗАВАНТАЖЕНО ДО 1НФ'
        WHEN rep.load_status <> 1 AND rep.validation_status > 0 THEN N'ПЕРЕВІРЕНО'
        WHEN rep.load_status <> 1 AND rep.validation_status < 0 THEN N'НЕ ПРОЙШОВ ПЕРЕВІРКУ'
        WHEN rep.load_status <> 1 AND rep.validation_status = 0 THEN N'НАПРАВЛЕНО НА ПОВТОРНУ ПЕРЕВІРКУ'
        ELSE N'НЕВИЗНАЧЕНО'
    END AS 'report_status',
    COALESCE(ec.error_count, 0) AS 'error_count',
    COALESCE(oec.error_count, 0) AS 'overridden_error_count',
    COALESCE(org1nf.fio_boss, org.director_fio) AS 'director_fio',
    COALESCE(org1nf.tel_boss, org.director_phone) AS 'director_phone',
    org.buhgalter_fio,
    org.buhgalter_phone,
    org.fax,
    org.contact_email,
    rep.modified_by,
    rep.modify_date
FROM
    [Import1NF].[dbo].reports rep
    LEFT OUTER JOIN [Import1NF].[dbo].sorg_1nf org1nf ON org1nf.kod_obj = rep.organization_id AND org1nf.report_id = rep.id
    INNER JOIN [GUKV].[dbo].organizations org ON org.id = rep.organization_id
    LEFT OUTER JOIN view_error_count ec ON ec.report_id = rep.id
    LEFT OUTER JOIN view_overridden_error_count oec ON oec.report_id = rep.id
WHERE
    rep.data_status = 1

GO

/* view_validation_errors */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_validation_errors]'))
DROP VIEW [dbo].[view_validation_errors]

GO

CREATE VIEW [view_validation_errors]
AS
SELECT
    id,
    report_id,
    obj_id,
    error_kind,
    token,
    object_descr,
    error_msg,
    is_overridden,
    CASE
        WHEN error_kind >= 100 AND error_kind < 200 THEN N'Будинок'
        WHEN error_kind >= 200 AND error_kind < 300 THEN N'Організація'
        WHEN error_kind >= 300 AND error_kind < 400 THEN N'Об''єкт на балансі'
        WHEN error_kind >= 400 AND error_kind < 500 THEN N'Договір оренди'
    END AS 'error_kind_descr',
    CASE WHEN error_kind IN (101, 102, 201, 203, 301, 401) THEN 1 ELSE 0 END AS 'is_critical',
    modified_by,
    modify_date
FROM
    validation_errors

GO

/* view_1nf_submitters */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[view_1nf_submitters]'))
DROP VIEW [dbo].[view_1nf_submitters]

GO

CREATE VIEW [view_1nf_submitters]
AS
SELECT
    org.*,
    CASE WHEN EXISTS (SELECT id FROM reports r WHERE r.organization_id = org.organization_id) THEN 'ТАК' ELSE 'НІ' END AS 'report_exists',
    CASE WHEN EXISTS (SELECT id FROM reports r WHERE r.organization_id = org.organization_id AND r.load_status = 1) THEN 'ТАК' ELSE 'НІ' END AS 'report_accepted',
    (SELECT COUNT(*) FROM [GUKV].[dbo].[balans] bal WHERE bal.organization_id = org.organization_id AND (bal.is_deleted IS NULL OR bal.is_deleted = 0)) AS 'num_balans_objects',
    CASE WHEN org.origin_db = 2 THEN 'БАЛАНС' ELSE '1НФ' END AS 'origin_db_str'
FROM
    [GUKV].[dbo].[view_organizations] org
WHERE
    (
		org.organization_id IN (SELECT organization_id FROM [GUKV].[dbo].[view_1nf_report_all_org]) OR
		org.organization_id IN (SELECT organization_id FROM reports)
	) AND
    ((org.org_deleted IS NULL) OR (org.org_deleted = 0))   

GO

/******************************************************************************/
/*                           Manual ZKPO overrides                            */
/******************************************************************************/

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01189910', NULL, 7721)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01189910', 'КИЇВСЬКА МІСЬКА ФІЛІЯ ВІДКРИТОГО АКЦІОНЕРНОГО ТОВАРИСТВА "УКРТЕЛЕКОМ"', 141714)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01189910', 'КИЇВСЬКА МІСЬКА ФІЛІЯ ПУБЛІЧНОГО АКЦІОНЕРНОГО ТОВАРИСТВА "УКРТЕЛЕКОМ"', 141714)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01189910', 'ПУБЛІЧНЕ АКЦІОНЕРНЕ ТОВАРИСТВО "УКРТЕЛЕКОМ"/КИЇВСЬКА МІСЬКА ФІЛІЯ', 141714)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01189910', 'ЦЕНТР ЕЛЕКТРОЗВ''ЯЗКУ N 2 КИЇВСЬКОЇ МІСЬКОЇ ФІЛІЇ ВІДКРИТОГО АКЦІОНЕРНОГО ТОВАРИСТВА  "УКРТЕЛЕКОМ"', 26733)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01189979', NULL, 5143)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01189979', 'КИЇВСЬКА МІСЬКА ДИРЕКЦІЯ УКРАЇНСЬКОГО ДЕРЖАВНОГО ПІДПРИЄМСТВА ПОШТОВОГО ЗВ''ЯЗКУ "УКРПОШТА"', 13836)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01189979', 'ЛІВОБЕРЕЖНИЙ ПОШТАМТ КИЇВСЬКОЇ МІСЬКОЇ ДИРЕКЦІЇ УКРАЇНСЬКОГО ДЕРЖАВНОГО ПІДПРИЄМСТВА ПОШТОВОГО ЗВ''ЯЗКУ "УКРПОШТА"', 7358)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01189979', 'ПІВДЕННИЙ ПОШТАМТ КИЇВСЬКОЇ МІСЬКОЇ ДИРЕКЦІЇ УКРАЇНСЬКОГО ДЕРЖАВНОГО ПІДПРИЄМСТВА ПОШТОВОГО ЗВ''ЯЗКУ "УКРПОШТА"', 2047)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01189979', 'ПІВНІЧНИЙ ПОШТАМТ КИЇВСЬКОЇ МІСЬКОЇ ДИРЕКЦІЇ УКРАЇНСЬКОГО ДЕРЖАВНОГО ПІДПРИЄМСТВА ПОШТОВОГО ЗВ''ЯЗКУ "УКРПОШТА"', 583)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01994095', NULL, 137834)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('01994095', 'КИЇВСЬКИЙ МІСЬКИЙ КЛІНІЧНИЙ ЕНДОКРИНОЛОГІЧНИЙ ЦЕНТР', 138730)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('02363095', NULL, 135005)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('02916594', NULL, 336)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('03327664', NULL, 615)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('08592201', NULL, 2407)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('09807750', 'ПУБЛІЧНЕ АКЦІОНЕРНЕ ТОВАРИСТВО "УКРСИББАНК"', 131969)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('09807750', 'ФІЗИЧНА ОСОБА-ПІДПРИЄМЕЦЬ ГАЛАЙБА СОФІЯ ПАВЛІВНА', 139872)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('14277001', NULL, 131779)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('14360570', NULL, 131229)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('19489216', NULL, 139570)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('21655633', 'ЕКЗАРХІЯ УКР. ГРЕКО-КАТОЛИЦЬКОЇ ЦЕРКВИ', 2785)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('22908846', 'ОСН "КОМІТЕТ МІКРОРАЙОНУ "ЦЕНТР-3"', 136127)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('22908846', NULL, 136151)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('24923100', NULL, 17737)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('26385523', NULL, 140191)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('31392783', NULL, 13979)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('31627374', 'ПОВНЕ ТОВАРИСТВО "ЧЕСНИЙ ЛОМБАРД", ТОВ "МАСТ І КОМПАНІЯ"', 138785)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('32040793', NULL, 137247)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('33228613', NULL, 135132)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('33695540', NULL, 141942)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('34539443', NULL, 22476)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('35509619', NULL, 140190)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('35525049', NULL, 138980)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('36413095', NULL, 141926)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37441694', NULL, 141943)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37445442', NULL, 6225)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37445442', 'УПРАВЛІННЯ ОСВІТИ ОБОЛОНСЬКОЇ РАЙОННОЇ В МІСТІ КИЄВІ ДЕРЖАВНОЇ АДМІНІСТРАЦІЇ (ДОШКІЛЬНІ ЗАКЛАДИ)', 6226)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37470086', NULL, 8463)

/* INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37479398', 'ТОВАРИСТВО З ОБМЕЖЕНОЮ ВІДПОВІДАЛЬНІСТЮ "СТІЛІАНЛЄЛЬ"', -1) */
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37479398', 'УПРАВЛІННЯ ОСВІТИ ГОЛОСІЇВСЬКОЇ РАЙОННОЇ В МІСТІ КИЄВІ ДЕРЖАВНОЇ АДМІНІСТРАЦІЇ', 7572)
/* INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37479398', 'УПРАВЛІННЯ ОСВІТИ ГОЛОСІЇВСЬКОЇ РАЙОННОЇ В МІСТІ КИЄВІ ДЕРЖАВНОЇ АДМІНІСТРАЦІЇ (ДОШКІЛЬНІ ЗАКЛАДИ)', -1) */

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37501684', NULL, 6373)
/* INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37501684', 'ВИЩИЙ ПРИВАТНИЙ НАВЧАЛЬНИЙ ЗАКЛАД "КИЇВСЬКА АКАДЕМІЯ ПЕРУКАРСЬКОГО МИСТЕЦТВА', -1) */
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37501684', 'УПРАВЛІННЯ ОСВІТИ ДЕСНЯНСЬКОЇ РАЙОННОЇ В МІСТІ КИЄВІ ДЕРЖАВНОЇ АДМІНІСТРАЦІЇ (ДОШКІЛЬНІ ЗАКЛАДИ)', 6375)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('37509427', NULL, 141878)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('04013583', NULL, 1346)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('1732409394', NULL, 141608)

/* INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('1983516350', 'СПД - ФІЗИЧНА ОСОБА ЮРАСОВА  Т.  Р..', -1) */

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('2578022111', NULL, 139840)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('2740008798', NULL, 134211)

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('2930920779', NULL, 141937)

/* ---------------------- */

INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('21534705', NULL, 1455)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('02910019', 'ПРОКУРАТУРА М. КИЄВА', 553)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('14360920', 'АКЦІОНЕРНИЙ КОМЕРЦІЙНИЙ БАНК "ПРАВЕКС-БАНК"', 3991)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('34691374', NULL, 628)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('05460172', 'ПУБЛІЧНА БІБЛІОТЕКА ІМЕНІ ЛЕСІ УКРАЇНКИ ДЛЯ ДОРОСЛИХ М. КИЄВА', 3924)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('2456505703', NULL, 135804)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('14357579', 'ПРАТ "ОТІС"', 9721)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('26112340', 'ПУБЛІЧНЕ АКЦІОНЕРНЕ ТОВАРИСТВО  "АК "КИЇВВОДОКАНАЛ" (ДЕВГ)', 472)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('21602192', 'ОРГАНІЗАЦІЯ ВЕТЕРАНІВ  СВЯТОШИНСЬКОГО РАЙОНУ', 16998)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('26388088', 'УПРАВЛІННЯ У СПРАВАХ СІМ"Ї, МОЛОДІ ТА СПОРТУ СВЯТОШИНСЬКОЇ РАЙОННОЇ В МІСТІ КИЄВІ ДЕРЖАВНОЇ АДМІНІСТРАЦІЇ', 134655)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('32068819', NULL, 131786)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('02760560', 'ПУБЛІЧНЕ АКЦІОНЕРНЕ ТОВАРИСТВО ДЕРЖАВНИЙ ОЩАДНИЙ БАНК УКРАЇНИ', 27398)
INSERT INTO manual_zkpo_match (zkpo, full_name, org_id_1nf) VALUES ('33945249', NULL, 137246)

GO
