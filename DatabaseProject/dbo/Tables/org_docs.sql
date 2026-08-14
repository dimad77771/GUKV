CREATE TABLE [dbo].[org_docs] (
    [organization_id] INT           NOT NULL,
    [document_id]     INT           NOT NULL,
    [link_kind_id]    INT           NULL,
    [modified_by]     VARCHAR (128) NULL,
    [modify_date]     DATE          NULL,
    PRIMARY KEY CLUSTERED ([organization_id] ASC, [document_id] ASC),
    CONSTRAINT [fk_org_docs_document] FOREIGN KEY ([document_id]) REFERENCES [dbo].[documents] ([id]),
    CONSTRAINT [fk_org_docs_link_kind] FOREIGN KEY ([link_kind_id]) REFERENCES [dbo].[dict_org_doc_link_kind] ([id]),
    CONSTRAINT [fk_org_docs_organization] FOREIGN KEY ([organization_id]) REFERENCES [dbo].[organizations] ([id])
);

