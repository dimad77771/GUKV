CREATE TABLE [dbo].[inflation_month_prognoz] (
    [year]      INT             NOT NULL,
    [month]     INT             NOT NULL,
    [inflation] DECIMAL (15, 3) NOT NULL,
    PRIMARY KEY CLUSTERED ([year] ASC, [month] ASC)
);

