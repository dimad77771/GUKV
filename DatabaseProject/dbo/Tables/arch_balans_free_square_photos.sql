CREATE TABLE [dbo].[arch_balans_free_square_photos] (
    [id]                       INT           NOT NULL,
    [free_square_id]           INT           NOT NULL,
    [file_name]                VARCHAR (255) NOT NULL,
    [file_ext]                 VARCHAR (255) NOT NULL,
    [modify_date]              DATETIME      NOT NULL,
    [modified_by]              VARCHAR (128) NOT NULL,
    [original_id]              INT           NOT NULL,
    [archive_id]               INT           IDENTITY (1, 1) NOT NULL,
    [archive_balans_link_code] INT           NOT NULL,
    CONSTRAINT [PK_arch_balans_free_square_photos] PRIMARY KEY CLUSTERED ([archive_id] ASC)
);

