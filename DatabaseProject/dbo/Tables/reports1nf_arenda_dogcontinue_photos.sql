CREATE TABLE [dbo].[reports1nf_arenda_dogcontinue_photos] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [free_square_id] INT           NOT NULL,
    [file_name]      VARCHAR (255) NOT NULL,
    [file_ext]       VARCHAR (255) NOT NULL,
    [modify_date]    DATETIME      NOT NULL,
    [modified_by]    VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_reports1nf_arenda_dogcontinue_photos] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_reports1nf_arenda_dogcontinue_photos_reports1nf_arenda_dogcontinue] FOREIGN KEY ([free_square_id]) REFERENCES [dbo].[reports1nf_arenda_dogcontinue] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

