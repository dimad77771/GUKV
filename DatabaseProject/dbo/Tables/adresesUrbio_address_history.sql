CREATE TABLE [dbo].[adresesUrbio_address_history] (
    [address_id]                      UNIQUEIDENTIFIER NOT NULL,
    [rownpp]                          INT              NOT NULL,
    [fullName]                        VARCHAR (1000)   NULL,
    [shortName]                       VARCHAR (1000)   NULL,
    [fullToponym]                     VARCHAR (1000)   NULL,
    [shortToponym]                    VARCHAR (1000)   NULL,
    [isToponymBeforeName]             BIT              NOT NULL,
    [secondLevel_fullName]            VARCHAR (1000)   NULL,
    [secondLevel_shortName]           VARCHAR (1000)   NULL,
    [secondLevel_fullToponym]         VARCHAR (1000)   NULL,
    [secondLevel_shortToponym]        VARCHAR (1000)   NULL,
    [secondLevel_isToponymBeforeName] BIT              NOT NULL,
    [thirdLevel_fullName]             VARCHAR (1000)   NULL,
    [thirdLevel_shortName]            VARCHAR (1000)   NULL,
    [thirdLevel_fullToponym]          VARCHAR (1000)   NULL,
    [thirdLevel_shortToponym]         VARCHAR (1000)   NULL,
    [thirdLevel_isToponymBeforeName]  BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([address_id] ASC, [rownpp] ASC),
    FOREIGN KEY ([address_id]) REFERENCES [dbo].[adresesUrbio_address] ([address_id])
);

