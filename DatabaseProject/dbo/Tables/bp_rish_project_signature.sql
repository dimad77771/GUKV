CREATE TABLE [dbo].[bp_rish_project_signature] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [project_id]   INT           NOT NULL,
    [person_name]  VARCHAR (128) NOT NULL,
    [person_title] VARCHAR (128) NOT NULL,
    [signed_on]    DATE          NULL,
    [ordinal_pos]  INT           NOT NULL,
    CONSTRAINT [PK_bp_rish_project_signature] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_bp_rish_project_signature_bp_rish_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[bp_rish_project] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

