CREATE TABLE [dbo].[balans_other_docs] (
    [id]              INT IDENTITY (1, 1) NOT NULL,
    [balans_other_id] INT NOT NULL,
    [document_id]     INT NOT NULL,
    CONSTRAINT [PK_balans_other_docs] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_balans_other_docs_balans_other] FOREIGN KEY ([balans_other_id]) REFERENCES [dbo].[balans_other] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_balans_other_docs_documents] FOREIGN KEY ([document_id]) REFERENCES [dbo].[documents] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

