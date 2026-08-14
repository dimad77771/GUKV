CREATE TABLE [dbo].[dict_tech_state] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (64)  DEFAULT (' ') NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

