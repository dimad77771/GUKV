CREATE TABLE [dbo].[dict_org_title_form] (
    [id]              INT           NOT NULL,
    [name_ukr_male]   VARCHAR (52)  NULL,
    [name_ukr_fem]    VARCHAR (52)  NULL,
    [name_ukr_single] VARCHAR (52)  NULL,
    [name_ukr_mult]   VARCHAR (52)  NULL,
    [name_rus_male]   VARCHAR (52)  NULL,
    [name_rus_fem]    VARCHAR (52)  NULL,
    [name_rus_single] VARCHAR (52)  NULL,
    [name_rus_mult]   VARCHAR (52)  NULL,
    [modified_by]     VARCHAR (128) NULL,
    [modify_date]     DATE          NULL,
    [display_name]    AS            ([name_ukr_male]),
    PRIMARY KEY CLUSTERED ([id] ASC)
);

