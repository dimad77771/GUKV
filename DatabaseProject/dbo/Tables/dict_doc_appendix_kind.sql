CREATE TABLE [dbo].[dict_doc_appendix_kind] (
    [id]            INT           NOT NULL,
    [name]          VARCHAR (60)  NULL,
    [modified_by]   VARCHAR (128) NULL,
    [modify_date]   DATE          NULL,
    [otdel_gukv_id] INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_doc_appendix_kind_otdel] FOREIGN KEY ([otdel_gukv_id]) REFERENCES [dbo].[dict_otdel_gukv] ([id])
);

