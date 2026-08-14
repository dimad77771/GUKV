CREATE TABLE [dbo].[fin_report_values] (
    [id]              INT             IDENTITY (1, 1) NOT NULL,
    [organization_id] INT             NULL,
    [form_id]         INT             NULL,
    [period_id]       INT             NULL,
    [num_row]         INT             NULL,
    [num_col]         INT             NULL,
    [expense_code]    INT             NULL,
    [value]           NUMERIC (18, 4) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_fin_values_org] FOREIGN KEY ([organization_id]) REFERENCES [dbo].[organizations] ([id]),
    CONSTRAINT [fk_fin_values_period] FOREIGN KEY ([period_id]) REFERENCES [dbo].[fin_report_periods] ([id])
);

