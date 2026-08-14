CREATE TABLE [dbo].[dict_kved] (
    [id]          INT           NOT NULL,
    [code]        VARCHAR (16)  NULL,
    [name]        VARCHAR (250) NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

