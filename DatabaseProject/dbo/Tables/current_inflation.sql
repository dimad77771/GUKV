CREATE TABLE [dbo].[current_inflation] (
    [inflation]              NUMERIC (15, 3) NOT NULL,
    [next_inflation]         NUMERIC (15, 3) NULL,
    [id]                     INT             NOT NULL,
    [prognoz_inflation_this] NUMERIC (15, 3) NULL,
    [prognoz_inflation_next] NUMERIC (15, 3) NULL,
    [prognoz_inflation_1]    NUMERIC (15, 3) NULL,
    [prognoz_inflation_2]    NUMERIC (15, 3) NULL,
    [prognoz_inflation_3]    NUMERIC (15, 3) NULL,
    [prognoz_inflation_4]    NUMERIC (15, 3) NULL,
    [rozrah_persion]         INT             NULL,
    [prognoz_znigka_1]       NUMERIC (15, 3) NULL,
    [prognoz_znigka_2]       NUMERIC (15, 3) NULL,
    [prognoz_znigka_3]       NUMERIC (15, 3) NULL,
    [prognoz_znigka_4]       NUMERIC (15, 3) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

