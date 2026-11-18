USE [GUKV20261002];
GO
IF OBJECT_ID(N'dbo.dict_lessor_representatives', N'U') IS NULL
    CREATE TABLE dbo.dict_lessor_representatives (name varchar(500) NULL);
GO
