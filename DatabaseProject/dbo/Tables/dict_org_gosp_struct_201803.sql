CREATE TABLE [dbo].[dict_org_gosp_struct_201803] (
    [id]            INT           NOT NULL,
    [name]          VARCHAR (32)  NULL,
    [industry_code] INT           NULL,
    [modified_by]   VARCHAR (128) NULL,
    [modify_date]   DATE          NULL,
    [display_name]  AS            ((str([industry_code])+'-')+[name]),
    PRIMARY KEY CLUSTERED ([id] ASC)
);

