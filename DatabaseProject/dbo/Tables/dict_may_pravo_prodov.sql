CREATE TABLE [dbo].[dict_may_pravo_prodov] (
    [id]          INT            NOT NULL,
    [ordnum]      INT            NOT NULL,
    [name]        VARCHAR (1000) NULL,
    [modified_by] VARCHAR (128)  NULL,
    [modify_date] DATE           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

