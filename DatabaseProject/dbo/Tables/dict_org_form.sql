CREATE TABLE [dbo].[dict_org_form] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (160) NOT NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    [stat_code]   INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

