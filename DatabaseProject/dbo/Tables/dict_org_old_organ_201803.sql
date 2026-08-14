CREATE TABLE [dbo].[dict_org_old_organ_201803] (
    [id]              INT           NOT NULL,
    [name]            VARCHAR (200) NULL,
    [old_industry_id] INT           NULL,
    [modified_by]     VARCHAR (128) NULL,
    [modify_date]     DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

