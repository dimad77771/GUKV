CREATE TABLE [dbo].[bp_rish_project_state] (
    [id]                INT              IDENTITY (1, 1) NOT NULL,
    [project_id]        INT              NOT NULL,
    [state_id]          INT              NOT NULL,
    [entered_on]        DATETIME         NOT NULL,
    [entered_by]        UNIQUEIDENTIFIER NOT NULL,
    [exited_on]         DATETIME         NULL,
    [exited_by]         UNIQUEIDENTIFIER NULL,
    [cover_letter_no]   VARCHAR (128)    NULL,
    [cover_letter_date] DATE             NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_bp_rish_project_state_bp_rish_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[bp_rish_project] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_bp_rish_project_state_dict_rish_project_state] FOREIGN KEY ([state_id]) REFERENCES [dbo].[dict_rish_project_state] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

