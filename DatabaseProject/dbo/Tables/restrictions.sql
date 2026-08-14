CREATE TABLE [dbo].[restrictions] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [document_id]    INT           NOT NULL,
    [restriction_id] INT           NOT NULL,
    [restr_group_id] INT           NOT NULL,
    [modified_by]    VARCHAR (128) NULL,
    [modify_date]    DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_restrictions_doc] FOREIGN KEY ([document_id]) REFERENCES [dbo].[documents] ([id]),
    CONSTRAINT [fk_restrictions_restr_group] FOREIGN KEY ([restr_group_id]) REFERENCES [dbo].[dict_restriction_group] ([id]),
    CONSTRAINT [fk_restrictions_restriction] FOREIGN KEY ([restriction_id]) REFERENCES [dbo].[dict_restriction] ([id])
);

