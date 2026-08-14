CREATE TABLE [dbo].[adresesUrbio_street] (
    [street_id]                    UNIQUEIDENTIFIER NOT NULL,
    [fullName]                     VARCHAR (1000)   NULL,
    [shortName]                    VARCHAR (1000)   NULL,
    [fullToponym]                  VARCHAR (1000)   NULL,
    [shortToponym]                 VARCHAR (1000)   NULL,
    [isToponymBeforeName]          BIT              NOT NULL,
    [cadastreCode]                 VARCHAR (1000)   NULL,
    [uniqueMarker_fullText]        VARCHAR (1000)   NULL,
    [uniqueMarker_shortText]       VARCHAR (1000)   NULL,
    [locality_id]                  UNIQUEIDENTIFIER NULL,
    [locality_fullName]            VARCHAR (1000)   NULL,
    [locality_shortName]           VARCHAR (1000)   NULL,
    [locality_fullToponym]         VARCHAR (1000)   NULL,
    [locality_shortToponym]        VARCHAR (1000)   NULL,
    [locality_isToponymBeforeName] BIT              NOT NULL,
    [description_fullText]         VARCHAR (1000)   NULL,
    [description_shortText]        VARCHAR (1000)   NULL,
    [asString]                     VARCHAR (1000)   NULL,
    [locale]                       VARCHAR (1000)   NULL,
    PRIMARY KEY CLUSTERED ([street_id] ASC)
);

