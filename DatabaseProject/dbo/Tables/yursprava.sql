CREATE TABLE [dbo].[yursprava] (
    [id]           INT            IDENTITY (1, 1) NOT NULL,
    [god]          INT            NULL,
    [npp]          INT            NULL,
    [nomsprava]    VARCHAR (MAX)  NULL,
    [pozivach]     VARCHAR (MAX)  NULL,
    [vidpovid]     VARCHAR (MAX)  NULL,
    [tretlico]     VARCHAR (MAX)  NULL,
    [predmet]      VARCHAR (MAX)  NULL,
    [sudperv]      VARCHAR (MAX)  NULL,
    [curstan]      VARCHAR (MAX)  NULL,
    [fiotel]       VARCHAR (MAX)  NULL,
    [apelac]       VARCHAR (MAX)  NULL,
    [kasac]        VARCHAR (MAX)  NULL,
    [pidstav]      VARCHAR (MAX)  NULL,
    [hlopotan]     VARCHAR (MAX)  NULL,
    [dopomoga]     VARCHAR (MAX)  NULL,
    [vazhno]       VARCHAR (MAX)  NULL,
    [modify_date2] DATETIME       NULL,
    [modified_by2] VARCHAR (128)  NULL,
    [objecturl]    VARCHAR (1000) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

