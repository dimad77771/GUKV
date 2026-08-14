CREATE TABLE [dbo].[dict_balans_purpose] (
    [id]           INT           NOT NULL,
    [group_code]   INT           NULL,
    [name]         VARCHAR (160) NULL,
    [modified_by]  VARCHAR (128) NULL,
    [modify_date]  DATE          NULL,
    [display_name] AS            ((str([group_code])+'-')+[name]),
    PRIMARY KEY CLUSTERED ([id] ASC)
);

