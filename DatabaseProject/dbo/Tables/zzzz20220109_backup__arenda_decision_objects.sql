CREATE TABLE [dbo].[zzzz20220109_backup__arenda_decision_objects] (
    [id]             INT             IDENTITY (1, 1) NOT NULL,
    [name]           VARCHAR (MAX)   NULL,
    [location]       VARCHAR (MAX)   NULL,
    [application_id] INT             NULL,
    [rishen_id]      INT             NULL,
    [building_id]    INT             NULL,
    [poverh]         INT             NULL,
    [modified_by]    VARCHAR (128)   NULL,
    [modify_date]    DATE            NULL,
    [obj_square]     NUMERIC (15, 2) NULL,
    [stavka]         VARCHAR (80)    NULL,
    [stavka_m]       VARCHAR (80)    NULL,
    [blob_name]      VARBINARY (MAX) NULL,
    [blob_location]  VARBINARY (MAX) NULL
);

