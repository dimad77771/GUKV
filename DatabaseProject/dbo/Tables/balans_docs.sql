CREATE TABLE [dbo].[balans_docs] (
    [id]               INT IDENTITY (1, 1) NOT NULL,
    [balans_id]        INT NULL,
    [building_id]      INT NULL,
    [link_kind]        INT NULL,
    [building_docs_id] INT NULL,
    [sort_field]       INT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_balans_docs_balans] FOREIGN KEY ([balans_id]) REFERENCES [dbo].[balans] ([id]),
    CONSTRAINT [fk_balans_docs_bdoc] FOREIGN KEY ([building_docs_id]) REFERENCES [dbo].[building_docs] ([id]),
    CONSTRAINT [fk_balans_docs_building] FOREIGN KEY ([building_id]) REFERENCES [dbo].[buildings] ([id])
);

