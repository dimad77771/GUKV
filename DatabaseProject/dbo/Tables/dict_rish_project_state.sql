CREATE TABLE [dbo].[dict_rish_project_state] (
    [id]    INT           IDENTITY (1, 1) NOT NULL,
    [name]  VARCHAR (128) NULL,
    [order] INT           NOT NULL,
    [flags] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

