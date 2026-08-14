CREATE TABLE [dbo].[zzz_expert_output_doc] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [expert_note_id] INT           NULL,
    [doc_date]       DATE          NULL,
    [doc_num]        VARCHAR (20)  NULL,
    [rezenz_type_id] INT           NULL,
    [modify_date]    DATE          NULL,
    [modified_by]    VARCHAR (128) NULL,
    [is_deleted]     INT           NULL,
    [del_date]       DATE          NULL,
    [rezenz_date]    DATE          NULL
);

