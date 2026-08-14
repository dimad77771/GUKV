CREATE TABLE [dbo].[dict_privat_subordination] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (255) NULL,
    [code]        VARCHAR (50)  NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

