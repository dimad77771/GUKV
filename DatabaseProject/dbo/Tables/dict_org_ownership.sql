CREATE TABLE [dbo].[dict_org_ownership] (
    [id]           INT           NOT NULL,
    [name]         VARCHAR (40)  NULL,
    [modified_by]  VARCHAR (128) NULL,
    [modify_date]  DATE          NULL,
    [display_name] VARCHAR (40)  NULL,
    [is_rda]       BIT           CONSTRAINT [DF_dict_org_ownership_is_rda] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

