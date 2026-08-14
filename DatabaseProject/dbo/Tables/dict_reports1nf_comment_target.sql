CREATE TABLE [dbo].[dict_reports1nf_comment_target] (
    [id]       INT           IDENTITY (0, 1) NOT NULL,
    [category] INT           NULL,
    [name]     VARCHAR (128) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

