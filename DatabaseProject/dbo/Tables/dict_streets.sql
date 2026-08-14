CREATE TABLE [dbo].[dict_streets] (
    [id]                  INT           NOT NULL,
    [name]                VARCHAR (160) DEFAULT (' ') NULL,
    [name_output]         VARCHAR (160) DEFAULT (' ') NULL,
    [kind]                VARCHAR (50)  DEFAULT (' ') NULL,
    [stan]                INT           NOT NULL,
    [activity]            INT           NULL,
    [renamed_from_doc_id] INT           NULL,
    [renamed_to_doc_id]   INT           NULL,
    [parent_id]           INT           NULL,
    [modified_by]         VARCHAR (128) NULL,
    [modify_date]         DATE          NULL,
    [region_id]           INT           NULL,
    CONSTRAINT [PK_dict_streets] PRIMARY KEY CLUSTERED ([id] ASC)
);

