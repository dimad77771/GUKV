CREATE TABLE [dbo].[privatisat_documents] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [free_square_id] INT           NOT NULL,
    [file_name]      VARCHAR (255) NOT NULL,
    [file_ext]       VARCHAR (255) NOT NULL,
    [modify_date]    DATETIME      NOT NULL,
    [modified_by]    VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_privatisat_documents] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK__privatisat_documents__privatisat] FOREIGN KEY ([free_square_id]) REFERENCES [dbo].[privatisat] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

