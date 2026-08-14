CREATE TABLE [dbo].[bp_rish_project_table] (
    [id]                      INT             IDENTITY (1, 1) NOT NULL,
    [bp_rish_project_item_id] INT             NOT NULL,
    [name]                    VARCHAR (MAX)   NULL,
    [address]                 VARCHAR (512)   NULL,
    [addr_street_name]        VARCHAR (500)   NULL,
    [addr_nomer]              VARCHAR (32)    NULL,
    [addr_misc]               VARCHAR (128)   NULL,
    [addr_distr]              VARCHAR (64)    NULL,
    [obj_type]                VARCHAR (64)    NULL,
    [obj_kind]                VARCHAR (64)    NULL,
    [year_built]              INT             NULL,
    [sqr_total]               NUMERIC (18, 2) NULL,
    [inv_number]              VARCHAR (32)    NULL,
    [initial_cost]            DECIMAL (18, 2) NULL,
    [remaining_cost]          DECIMAL (18, 2) NULL,
    [location]                VARCHAR (512)   NULL,
    [commissioned_date]       DATE            NULL,
    [is_acted_on]             BIT             NULL,
    CONSTRAINT [PK_bp_rish_project_table] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_bp_rish_project_table_bp_rish_project_item] FOREIGN KEY ([bp_rish_project_item_id]) REFERENCES [dbo].[bp_rish_project_item] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

