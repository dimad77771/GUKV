CREATE TABLE [dbo].[dict_org_share_type] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (24)  NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

