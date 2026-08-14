CREATE TABLE [dbo].[valsndogovor] (
    [id]           INT             IDENTITY (1, 1) NOT NULL,
    [nomer_zapis]  VARCHAR (4000)  NULL,
    [priyom_date]  DATETIME        NULL,
    [nazva_vlasn]  VARCHAR (4000)  NULL,
    [address]      VARCHAR (4000)  NULL,
    [plosha]       DECIMAL (18, 2) NULL,
    [result]       VARCHAR (4000)  NULL,
    [modify_date2] DATETIME        NULL,
    [modified_by2] VARCHAR (128)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

