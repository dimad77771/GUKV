CREATE TABLE [dbo].[expert_input_doc] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [expert_note_id]   INT           NULL,
    [doc_date]         DATE          NULL,
    [doc_num]          VARCHAR (20)  NULL,
    [control_date]     DATE          NULL,
    [korrespondent_id] INT           NULL,
    [modify_date]      DATE          NULL,
    [modified_by]      VARCHAR (128) NULL,
    [mvd_doc_id]       INT           NULL,
    [is_deleted]       INT           DEFAULT ((0)) NULL,
    [del_date]         DATE          NULL,
    [rezenz_name]      VARCHAR (255) NULL,
    [rezenz_id]        INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_expert_input_doc_korr] FOREIGN KEY ([korrespondent_id]) REFERENCES [dbo].[dict_expert_korr] ([id]),
    CONSTRAINT [fk_expert_input_doc_note] FOREIGN KEY ([expert_note_id]) REFERENCES [dbo].[expert_note] ([id]),
    CONSTRAINT [fk_expert_input_doc_rezenz] FOREIGN KEY ([rezenz_id]) REFERENCES [dbo].[dict_expert_rezenz] ([id])
);

