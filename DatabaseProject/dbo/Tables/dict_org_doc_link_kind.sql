CREATE TABLE [dbo].[dict_org_doc_link_kind] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (64)  NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

