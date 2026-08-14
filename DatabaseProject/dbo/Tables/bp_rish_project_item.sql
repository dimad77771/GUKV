CREATE TABLE [dbo].[bp_rish_project_item] (
    [id]                    INT           IDENTITY (1, 1) NOT NULL,
    [project_id]            INT           NOT NULL,
    [parent_item_id]        INT           NULL,
    [ordinal_pos]           INT           NULL,
    [intro_text]            VARCHAR (MAX) NULL,
    [outro_text]            VARCHAR (MAX) NULL,
    [building_id]           INT           NULL,
    [balans_id]             INT           NULL,
    [org_from_id]           INT           NULL,
    [org_to_id]             INT           NULL,
    [right_id]              INT           NULL,
    [explanation]           VARCHAR (MAX) NULL,
    [is_table]              BIT           NOT NULL,
    [arbitrary_text]        VARCHAR (MAX) NULL,
    [external_document_id]  INT           NULL,
    [use_external_document] INT           NULL,
    [table_type]            INT           NULL,
    CONSTRAINT [PK__bp_rish___3213E83F7115C565] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_bp_rish_project_item_proj] FOREIGN KEY ([project_id]) REFERENCES [dbo].[bp_rish_project] ([id])
);

