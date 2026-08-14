CREATE TABLE [dbo].[dict_object_kind] (
    [id]          INT           NOT NULL,
    [code]        VARCHAR (16)  NULL,
    [name]        VARCHAR (256) DEFAULT (' ') NULL,
    [mnemonic]    VARCHAR (8)   DEFAULT (' ') NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

