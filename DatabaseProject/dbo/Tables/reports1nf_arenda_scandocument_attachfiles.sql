CREATE TABLE [dbo].[reports1nf_arenda_scandocument_attachfiles] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [free_square_id] INT           NOT NULL,
    [file_name]      VARCHAR (255) NOT NULL,
    [file_ext]       VARCHAR (255) NOT NULL,
    [modify_date]    DATETIME      NOT NULL,
    [modified_by]    VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_reports1nf_arenda_scandocument_attachfiles] PRIMARY KEY CLUSTERED ([id] ASC)
);

