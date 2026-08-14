CREATE TABLE [dbo].[freecycle_step_documents] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [free_square_id] INT           NOT NULL,
    [file_name]      VARCHAR (255) NOT NULL,
    [file_ext]       VARCHAR (255) NOT NULL,
    [modify_date]    DATETIME      NOT NULL,
    [modified_by]    VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_freecycle_step_documents] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_freecycle_step_documents_freecycle_step] FOREIGN KEY ([free_square_id]) REFERENCES [dbo].[freecycle_step] ([freecycle_step_id]) ON DELETE CASCADE
);

