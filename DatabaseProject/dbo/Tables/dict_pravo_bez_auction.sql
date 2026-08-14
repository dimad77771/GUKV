CREATE TABLE [dbo].[dict_pravo_bez_auction] (
    [id]          INT            NOT NULL,
    [ordnum]      INT            NOT NULL,
    [name]        VARCHAR (1000) NULL,
    [modified_by] VARCHAR (128)  NULL,
    [modify_date] DATE           NULL,
    [maepravo]    VARCHAR (500)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

