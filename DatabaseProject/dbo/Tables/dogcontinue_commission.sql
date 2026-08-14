CREATE TABLE [dbo].[dogcontinue_commission] (
    [id]                       INT           IDENTITY (1, 1) NOT NULL,
    [commission_num]           VARCHAR (100) NOT NULL,
    [commission_date]          DATETIME      NULL,
    [commission_status]        VARCHAR (100) NULL,
    [deputies_list]            VARCHAR (MAX) NULL,
    [dkv_head]                 VARCHAR (200) NULL,
    [district_representatives] VARCHAR (MAX) NULL,
    [modify_date]              DATETIME      CONSTRAINT [DF_dogcontinue_commission_modify_date] DEFAULT (getdate()) NOT NULL,
    [modified_by]              VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dogcontinue_commission] PRIMARY KEY CLUSTERED ([id] ASC)
);

