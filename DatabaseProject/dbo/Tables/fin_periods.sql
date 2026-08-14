CREATE TABLE [dbo].[fin_periods] (
    [id]          INT          IDENTITY (1, 1) NOT NULL,
    [period_name] VARCHAR (48) NULL,
    [period_code] VARCHAR (48) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

