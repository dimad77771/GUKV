CREATE TABLE [dbo].[bp_rish_project] (
    [id]                    INT           IDENTITY (1, 1) NOT NULL,
    [name]                  VARCHAR (MAX) NOT NULL,
    [intro_text]            VARCHAR (MAX) NULL,
    [outro_text]            VARCHAR (MAX) NULL,
    [app_for_project_id]    INT           NULL,
    [ordinal_pos]           INT           NULL,
    [external_document_id]  INT           NULL,
    [use_external_document] BIT           NULL,
    CONSTRAINT [PK__bp_rish___3213E83F61D381D5] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_bp_rish_project_bp_rish_project] FOREIGN KEY ([app_for_project_id]) REFERENCES [dbo].[bp_rish_project] ([id])
);

