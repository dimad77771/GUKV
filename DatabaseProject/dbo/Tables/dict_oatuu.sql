CREATE TABLE [dbo].[dict_oatuu] (
    [id]          VARCHAR (12)  NOT NULL,
    [name]        VARCHAR (128) DEFAULT (' ') NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

