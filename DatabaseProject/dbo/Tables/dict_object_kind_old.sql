CREATE TABLE [dbo].[dict_object_kind_old] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (64)  DEFAULT (' ') NULL,
    [mnemonic]    VARCHAR (8)   DEFAULT (' ') NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

