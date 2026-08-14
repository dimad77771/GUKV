CREATE TABLE [dbo].[balans_free_square_photos] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [free_square_id] INT           NOT NULL,
    [file_name]      VARCHAR (255) NOT NULL,
    [file_ext]       VARCHAR (255) NOT NULL,
    [modify_date]    DATETIME      NOT NULL,
    [modified_by]    VARCHAR (128) NOT NULL,
    [original_id]    INT           NOT NULL,
    CONSTRAINT [PK_balans_free_square_photos] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_balans_free_square_photos_balans_free_square] FOREIGN KEY ([free_square_id]) REFERENCES [dbo].[balans_free_square] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

