CREATE TABLE [dbo].[dict_org_title] (
    [id]           INT           NOT NULL,
    [name_rus]     VARCHAR (40)  NULL,
    [name_ukr]     VARCHAR (40)  NULL,
    [modified_by]  VARCHAR (128) NULL,
    [modify_date]  DATE          NULL,
    [display_name] AS            ([name_ukr]),
    PRIMARY KEY CLUSTERED ([id] ASC)
);

