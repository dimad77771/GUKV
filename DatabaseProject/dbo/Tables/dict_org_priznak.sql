CREATE TABLE [dbo].[dict_org_priznak] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (30)  NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

