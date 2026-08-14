CREATE TABLE [dbo].[dict_rish_project_org] (
    [id]   INT           IDENTITY (1, 1) NOT NULL,
    [name] VARCHAR (128) NULL,
    CONSTRAINT [PK_dict_rish_project_org] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UC_dict_rish_project_org] UNIQUE NONCLUSTERED ([name] ASC)
);

