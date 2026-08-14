CREATE TABLE [dbo].[corpordvig] (
    [id]           INT             IDENTITY (1, 1) NOT NULL,
    [uridzkpo]     VARCHAR (1000)  NULL,
    [uridname]     VARCHAR (1000)  NULL,
    [dateoper]     DATE            NULL,
    [uchotrim]     VARCHAR (1000)  NULL,
    [uchpered]     VARCHAR (1000)  NULL,
    [typeoper]     VARCHAR (1000)  NULL,
    [akckol]       NUMERIC (20, 2) NULL,
    [akcvart]      NUMERIC (20, 2) NULL,
    [akctype]      VARCHAR (1000)  NULL,
    [partfact]     NUMERIC (20, 2) NULL,
    [partplan]     NUMERIC (20, 2) NULL,
    [methodoper]   VARCHAR (1000)  NULL,
    [docoper]      VARCHAR (1000)  NULL,
    [platnik]      VARCHAR (1000)  NULL,
    [oderzh]       VARCHAR (1000)  NULL,
    [modify_date2] DATETIME        NULL,
    [modified_by2] VARCHAR (128)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

