CREATE TABLE [dbo].[doc_appendices] (
    [id]           INT           NOT NULL,
    [app_date]     DATE          NULL,
    [app_doc_num]  VARCHAR (15)  NULL,
    [app_num]      VARCHAR (2)   NULL,
    [app_num_user] VARCHAR (2)   NULL,
    [is_archived]  VARCHAR (1)   NULL,
    [kind_id]      INT           NULL,
    [modified_by]  VARCHAR (128) NULL,
    [modify_date]  DATE          NULL,
    [doc_id]       INT           NULL,
    [doc_date]     DATE          NULL,
    [doc_num]      VARCHAR (15)  NULL,
    [stadiya_id]   INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_appendices_doc] FOREIGN KEY ([doc_id]) REFERENCES [dbo].[documents] ([id]),
    CONSTRAINT [fk_appendices_kind] FOREIGN KEY ([kind_id]) REFERENCES [dbo].[dict_doc_appendix_kind] ([id]),
    CONSTRAINT [fk_appendices_stadiya] FOREIGN KEY ([stadiya_id]) REFERENCES [dbo].[dict_doc_stadiya] ([id])
);

