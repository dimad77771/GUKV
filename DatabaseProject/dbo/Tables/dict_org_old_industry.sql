CREATE TABLE [dbo].[dict_org_old_industry] (
    [id]                 INT            NOT NULL,
    [name]               VARCHAR (200)  NULL,
    [modified_by]        VARCHAR (128)  NULL,
    [modify_date]        DATE           NULL,
    [budg_payments_rate] NUMERIC (5, 2) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

