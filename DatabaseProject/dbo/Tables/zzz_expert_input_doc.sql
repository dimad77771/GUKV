CREATE TABLE [dbo].[zzz_expert_input_doc] (
    [id]               INT           IDENTITY (1, 1) NOT NULL,
    [expert_note_id]   INT           NULL,
    [doc_date]         DATE          NULL,
    [doc_num]          VARCHAR (20)  NULL,
    [control_date]     DATE          NULL,
    [korrespondent_id] INT           NULL,
    [modify_date]      DATE          NULL,
    [modified_by]      VARCHAR (128) NULL,
    [mvd_doc_id]       INT           NULL,
    [is_deleted]       INT           NULL,
    [del_date]         DATE          NULL,
    [rezenz_name]      VARCHAR (255) NULL,
    [rezenz_id]        INT           NULL
);

