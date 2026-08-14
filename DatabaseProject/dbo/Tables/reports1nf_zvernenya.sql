CREATE TABLE [dbo].[reports1nf_zvernenya] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [balans_id]        INT            NOT NULL,
    [report_id]        INT            NOT NULL,
    [zvernen_dat]      DATE           NULL,
    [zvernen_vid]      VARCHAR (8000) NULL,
    [bazhana_ploshad]  VARCHAR (8000) NULL,
    [possible_using]   VARCHAR (1000) NULL,
    [modify_date]      DATETIME       NULL,
    [modified_by]      VARCHAR (128)  NULL,
    [czilove_vikorist] INT            NULL,
    CONSTRAINT [PK_reports1nf_zvernenya] PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([czilove_vikorist]) REFERENCES [dbo].[dict_czilove_vikorist] ([id])
);

