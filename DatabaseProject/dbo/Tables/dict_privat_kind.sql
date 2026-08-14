CREATE TABLE [dbo].[dict_privat_kind] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (20)  NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

