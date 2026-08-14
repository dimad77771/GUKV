CREATE TABLE [dbo].[fin_report_formula_values] (
    [id]              INT             IDENTITY (1, 1) NOT NULL,
    [organization_id] INT             NULL,
    [formula_id]      INT             NULL,
    [period_id]       INT             NULL,
    [value]           NUMERIC (15, 2) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_fin_formula_values_formula] FOREIGN KEY ([formula_id]) REFERENCES [dbo].[fin_report_formulae] ([id]),
    CONSTRAINT [fk_fin_formula_values_org] FOREIGN KEY ([organization_id]) REFERENCES [dbo].[organizations] ([id]),
    CONSTRAINT [fk_fin_formula_values_period] FOREIGN KEY ([period_id]) REFERENCES [dbo].[fin_report_periods] ([id])
);

