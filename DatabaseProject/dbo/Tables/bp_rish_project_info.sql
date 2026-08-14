CREATE TABLE [dbo].[bp_rish_project_info] (
    [id]                 INT              IDENTITY (1, 1) NOT NULL,
    [project_id]         INT              NOT NULL,
    [document_num]       VARCHAR (64)     NULL,
    [document_date]      DATE             NULL,
    [created_by]         UNIQUEIDENTIFIER NOT NULL,
    [create_date]        DATETIME         NOT NULL,
    [modified_by]        UNIQUEIDENTIFIER NOT NULL,
    [modify_date]        DATETIME         NOT NULL,
    [project_type_id]    INT              NOT NULL,
    [project_contact_id] INT              NOT NULL,
    [subject]            VARCHAR (MAX)    NULL,
    [is_exported]        INT              DEFAULT ((0)) NULL,
    [document_id]        INT              NULL,
    CONSTRAINT [PK_bp_rish_project_info] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_bp_rish_project_info_bp_rish_project] FOREIGN KEY ([project_id]) REFERENCES [dbo].[bp_rish_project] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_org_contact] FOREIGN KEY ([project_contact_id]) REFERENCES [dbo].[dict_rish_project_org_contact] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_bp_rish_project_info_dict_rish_project_type] FOREIGN KEY ([project_type_id]) REFERENCES [dbo].[dict_rish_project_type] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_bp_rish_project_info_documents] FOREIGN KEY ([document_id]) REFERENCES [dbo].[documents] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UC_bp_rish_project_info] UNIQUE NONCLUSTERED ([project_id] ASC)
);

