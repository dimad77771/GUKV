CREATE TABLE [dbo].[expert_output_doc] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [expert_note_id] INT           NULL,
    [doc_date]       DATE          NULL,
    [doc_num]        VARCHAR (20)  NULL,
    [rezenz_type_id] INT           NULL,
    [modify_date]    DATE          NULL,
    [modified_by]    VARCHAR (128) NULL,
    [is_deleted]     INT           DEFAULT ((0)) NULL,
    [del_date]       DATE          NULL,
    [rezenz_date]    DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_expert_output_doc_note] FOREIGN KEY ([expert_note_id]) REFERENCES [dbo].[expert_note] ([id]),
    CONSTRAINT [fk_expert_output_doc_rezenz_type] FOREIGN KEY ([rezenz_type_id]) REFERENCES [dbo].[dict_expert_rezenz_type] ([id])
);

