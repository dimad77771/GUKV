CREATE TABLE [dbo].[adresesUrbio_street_incorrect] (
    [street_id]           UNIQUEIDENTIFIER NOT NULL,
    [rownpp]              INT              NOT NULL,
    [fullName]            VARCHAR (1000)   NULL,
    [shortName]           VARCHAR (1000)   NULL,
    [fullToponym]         VARCHAR (1000)   NULL,
    [shortToponym]        VARCHAR (1000)   NULL,
    [isToponymBeforeName] BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([street_id] ASC, [rownpp] ASC),
    FOREIGN KEY ([street_id]) REFERENCES [dbo].[adresesUrbio_street] ([street_id])
);

