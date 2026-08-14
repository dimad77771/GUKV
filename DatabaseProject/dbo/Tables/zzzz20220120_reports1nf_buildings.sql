CREATE TABLE [dbo].[zzzz20220120_reports1nf_buildings] (
    [id]                 INT           NOT NULL,
    [master_building_id] INT           NULL,
    [addr_street_name]   VARCHAR (128) NULL,
    [addr_street_id]     INT           NULL,
    [street_full_name]   VARCHAR (128) NULL,
    [addr_nomer1]        VARCHAR (32)  NULL,
    [addr_nomer2]        VARCHAR (32)  NULL,
    [addr_nomer3]        VARCHAR (32)  NULL,
    [addr_nomer]         VARCHAR (98)  NULL,
    [addr_misc]          VARCHAR (128) NULL,
    [addr_korpus_flag]   INT           NULL,
    [addr_korpus]        VARCHAR (255) NULL,
    [addr_zip_code]      VARCHAR (24)  NULL,
    [addr_address]       VARCHAR (356) NULL,
    [unique_id]          INT           IDENTITY (1, 1) NOT NULL
);

