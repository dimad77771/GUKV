CREATE TABLE [dbo].[arenda_decision_objects] (
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
    [blob_location]  VARBINARY (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_ado_appl] FOREIGN KEY ([application_id]) REFERENCES [dbo].[arenda_applications] ([id]),
    CONSTRAINT [fk_ado_building] FOREIGN KEY ([building_id]) REFERENCES [dbo].[buildings] ([id]),
    CONSTRAINT [fk_ado_rishen] FOREIGN KEY ([rishen_id]) REFERENCES [dbo].[arenda_decisions] ([id])
);

