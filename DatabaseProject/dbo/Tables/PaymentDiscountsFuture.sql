CREATE TABLE [dbo].[PaymentDiscountsFuture] (
    [id]         INT             IDENTITY (1, 1) NOT NULL,
    [name]       VARCHAR (1000)  NOT NULL,
    [percent]    NUMERIC (15, 3) NOT NULL,
    [date1]      DATETIME        NULL,
    [date2]      DATETIME        NULL,
    [ExistsName] VARCHAR (1000)  NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    UNIQUE NONCLUSTERED ([name] ASC)
);

