CREATE TABLE [dbo].[dict_1nf_streets] (
    [id]   INT           NOT NULL,
    [name] VARCHAR (160) DEFAULT (' ') NULL,
    [stan] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC, [stan] ASC)
);

