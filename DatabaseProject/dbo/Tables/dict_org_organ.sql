CREATE TABLE [dbo].[dict_org_organ] (
    [id]            INT           NOT NULL,
    [name]          VARCHAR (50)  NULL,
    [industry_code] INT           NULL,
    [modified_by]   VARCHAR (128) NULL,
    [modify_date]   DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

