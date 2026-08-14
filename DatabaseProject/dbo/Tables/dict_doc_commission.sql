CREATE TABLE [dbo].[dict_doc_commission] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (40)  NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

