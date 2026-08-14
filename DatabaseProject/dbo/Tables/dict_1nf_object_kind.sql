CREATE TABLE [dbo].[dict_1nf_object_kind] (
    [id]   INT           NOT NULL,
    [code] VARCHAR (16)  NULL,
    [name] VARCHAR (256) DEFAULT (' ') NULL,
    [ord]  INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

