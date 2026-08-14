CREATE TABLE [dbo].[fin_budget_rate_by_org] (
    [id]              INT            IDENTITY (1, 1) NOT NULL,
    [period_id]       INT            NULL,
    [organization_id] INT            NULL,
    [payments_rate]   NUMERIC (5, 2) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

