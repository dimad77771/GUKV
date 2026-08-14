CREATE TABLE [dbo].[reports1nf_arenda_dogcontinue_current_stage_documents] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [free_square_id] INT           NOT NULL,
    [file_name]      VARCHAR (255) NOT NULL,
    [file_ext]       VARCHAR (255) NOT NULL,
    [modify_date]    DATETIME      NOT NULL,
    [modified_by]    VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_reports1nf_arenda_dogcontinue_current_stage_documents] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_reports1nf_arenda_dogcontinue_current_stage_documents_reports1nf_arenda_dogcontinue] FOREIGN KEY ([free_square_id]) REFERENCES [dbo].[reports1nf_arenda_dogcontinue] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

