CREATE TABLE [dbo].[reports1nf_arenda_insurance_attachfiles] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [arenda_id]   INT           NOT NULL,
    [report_id]   INT           NOT NULL,
    [file_name]   VARCHAR (255) NOT NULL,
    [file_ext]    VARCHAR (255) NOT NULL,
    [modify_date] DATETIME      NOT NULL,
    [modified_by] VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_reports1nf_arenda_insurance_attachfiles] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_reports1nf_arenda_insurance_attachfiles_reports1nf_arenda]
        FOREIGN KEY ([report_id], [arenda_id])
        REFERENCES [dbo].[reports1nf_arenda] ([report_id], [id])
        ON DELETE CASCADE ON UPDATE CASCADE
);

