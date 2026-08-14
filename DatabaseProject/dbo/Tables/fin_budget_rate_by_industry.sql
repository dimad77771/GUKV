CREATE TABLE [dbo].[fin_budget_rate_by_industry] (
    [id]                INT            IDENTITY (1, 1) NOT NULL,
    [period_id]         INT            NULL,
    [old_industry_id]   INT            NULL,
    [old_occupation_id] INT            DEFAULT ((0)) NULL,
    [payments_rate]     NUMERIC (5, 2) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

