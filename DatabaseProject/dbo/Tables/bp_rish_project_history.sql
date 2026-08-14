CREATE TABLE [dbo].[bp_rish_project_history] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [project_id]  INT              NOT NULL,
    [change_text] VARCHAR (MAX)    NOT NULL,
    [modified_by] UNIQUEIDENTIFIER NOT NULL,
    [modify_date] DATETIME         NOT NULL,
    CONSTRAINT [PK__bp_rish___3213E83F678C5B2B] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_bp_rish_project_history_proj] FOREIGN KEY ([project_id]) REFERENCES [dbo].[bp_rish_project] ([id])
);

