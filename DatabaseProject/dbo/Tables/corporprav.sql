CREATE TABLE [dbo].[corporprav] (
    [id]           INT             IDENTITY (1, 1) NOT NULL,
    [uridname]     VARCHAR (1000)  NULL,
    [uridzkpo]     VARCHAR (1000)  NULL,
    [vlasname]     VARCHAR (1000)  NULL,
    [vlasicod]     VARCHAR (1000)  NULL,
    [vlaszkpo]     VARCHAR (1000)  NULL,
    [vlaskopfg]    VARCHAR (1000)  NULL,
    [vlasaddr]     VARCHAR (1000)  NULL,
    [isvlas]       BIT             DEFAULT ((0)) NOT NULL,
    [akckol]       NUMERIC (20, 2) NULL,
    [akcvart]      NUMERIC (20, 2) NULL,
    [akcpart]      NUMERIC (20, 2) NULL,
    [akcform]      VARCHAR (1000)  NULL,
    [nomvart]      NUMERIC (20, 2) NULL,
    [stanobj]      VARCHAR (1000)  NULL,
    [modify_date2] DATETIME        NULL,
    [modified_by2] VARCHAR (128)   NULL,
    [ownershipvid] VARCHAR (1000)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

