
/******************************************************************************/
/*        Pre-processing steps which are performed after data migration       */
/******************************************************************************/

/*
   'Balans' database temporary tables
*/

/* This table duplicates the structure of 'organizations' table */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[temp_balans_org]') AND type in (N'U'))
DROP TABLE [dbo].[temp_balans_org]

SELECT * INTO temp_balans_org FROM organizations

/* This table duplicates the structure of 'buildings' table */

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[temp_buildings]') AND type in (N'U'))
DROP TABLE [dbo].[temp_buildings]

SELECT * INTO temp_buildings FROM buildings

ALTER TABLE temp_buildings DROP COLUMN addr_nomer

ALTER TABLE temp_buildings DROP COLUMN addr_address

/* Add temporary columns to the 'organizations' table */

ALTER TABLE organizations ADD temp_org_kind_id INTEGER NULL

ALTER TABLE arch_organizations ADD temp_org_kind_id INTEGER NULL

/* Add temporary columns to the 'building_docs' table */

ALTER TABLE building_docs ADD old_id INTEGER NULL

/*
   'Arenda' database temporary columns
*/

/* ALTER TABLE arenda_decisions ADD appendix_id_protok INTEGER NULL /* KODP */ */
/* ALTER TABLE arenda_decisions ADD appendix_id_raspor INTEGER NULL /* KODR */ */
ALTER TABLE arenda_decisions ADD blob_rish_etap1 VARBINARY(MAX) NULL /* TRISH */
ALTER TABLE arenda_decisions ADD blob_rish2_etap1 VARBINARY(MAX) NULL /* TRISHP */
ALTER TABLE arenda_decisions ADD blob_nazva VARBINARY(MAX) NULL /* NAZ */
ALTER TABLE arenda_decisions ADD blob_rish_etap2 VARBINARY(MAX) NULL /* TRISH1 */
ALTER TABLE arenda_decisions ADD blob_rish2_etap2 VARBINARY(MAX) NULL /* TRISHP1 */

ALTER TABLE arenda_decision_objects ADD blob_name VARBINARY(MAX) NULL /* NAZ */
ALTER TABLE arenda_decision_objects ADD blob_location VARBINARY(MAX) NULL /* ROZTASH */

GO

/*
   Archive state tables - temporary columns
*/

ALTER TABLE arch_buildings ADD archive_link_code INTEGER
ALTER TABLE arch_buildings ADD archive_link_code_prev INTEGER
ALTER TABLE arch_buildings ADD archive_link_code_next INTEGER

ALTER TABLE arch_organizations ADD archive_link_code INTEGER
ALTER TABLE arch_organizations ADD archive_link_code_prev INTEGER
ALTER TABLE arch_organizations ADD archive_link_code_next INTEGER

/* ALTER TABLE arch_arenda ADD archive_link_code INTEGER - this code is permanent, not temporary in 'arch_arenda' table */
ALTER TABLE arch_arenda ADD archive_link_code_prev INTEGER
ALTER TABLE arch_arenda ADD archive_link_code_next INTEGER

/* ALTER TABLE arch_balans ADD archive_link_code INTEGER - this code is permanent, not temporary in 'arch_balans' table */
ALTER TABLE arch_balans ADD archive_link_code_prev INTEGER
ALTER TABLE arch_balans ADD archive_link_code_next INTEGER

GO
