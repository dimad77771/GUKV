CREATE TABLE [dbo].[inflation_year_prognoz] (
    [year]      INT             NOT NULL,
    [inflation] DECIMAL (15, 3) NOT NULL,
    PRIMARY KEY CLUSTERED ([year] ASC)
);

