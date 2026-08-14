CREATE TABLE [dbo].[dict_privat_state] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (50)  NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

