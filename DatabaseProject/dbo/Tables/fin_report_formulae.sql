CREATE TABLE [dbo].[fin_report_formulae] (
    [id]          INT            NOT NULL,
    [period_id]   INT            NULL,
    [name]        VARCHAR (255)  NULL,
    [formula]     VARCHAR (1000) NULL,
    [modified_by] VARCHAR (128)  NULL,
    [modify_date] DATE           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_fin_formulae_period] FOREIGN KEY ([period_id]) REFERENCES [dbo].[fin_report_periods] ([id])
);

