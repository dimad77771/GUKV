CREATE TABLE [dbo].[dict_rish_project_type] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [name]        VARCHAR (128) NULL,
    [doc_kind_id] INT           NOT NULL,
    CONSTRAINT [PK_dict_rish_project_type] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UC_dict_rish_project_type] UNIQUE NONCLUSTERED ([name] ASC)
);

