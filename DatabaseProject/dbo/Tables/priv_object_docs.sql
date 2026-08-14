CREATE TABLE [dbo].[priv_object_docs] (
    [privatization_id] INT           NOT NULL,
    [master_doc_id]    INT           NULL,
    [document_id]      INT           NOT NULL,
    [modified_by]      VARCHAR (128) NULL,
    [modify_date]      DATE          NULL,
    PRIMARY KEY CLUSTERED ([privatization_id] ASC, [document_id] ASC),
    CONSTRAINT [fk_priv_docs_document] FOREIGN KEY ([document_id]) REFERENCES [dbo].[documents] ([id]),
    CONSTRAINT [fk_priv_docs_privatization] FOREIGN KEY ([privatization_id]) REFERENCES [dbo].[privatization] ([id])
);

