CREATE TABLE [dbo].[dict_org_old_organ] (
    [id]              INT           NOT NULL,
    [name]            VARCHAR (200) NULL,
    [old_industry_id] INT           NULL,
    [modified_by]     VARCHAR (128) NULL,
    [modify_date]     DATE          NULL,
    [use_for_user]    SMALLINT      NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_dict_org_old_organ_old_industry] FOREIGN KEY ([old_industry_id]) REFERENCES [dbo].[dict_org_old_industry] ([id])
);

