CREATE TABLE [dbo].[doc_dependencies] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [master_doc_id]  INT           NULL,
    [slave_doc_id]   INT           NULL,
    [depend_kind_id] INT           NULL,
    [modified_by]    VARCHAR (128) NULL,
    [modify_date]    DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_doc_depend_kind] FOREIGN KEY ([depend_kind_id]) REFERENCES [dbo].[dict_doc_depend_kind] ([id]),
    CONSTRAINT [fk_doc_depend_master] FOREIGN KEY ([master_doc_id]) REFERENCES [dbo].[documents] ([id]),
    CONSTRAINT [fk_doc_depend_slave] FOREIGN KEY ([slave_doc_id]) REFERENCES [dbo].[documents] ([id])
);

