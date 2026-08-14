CREATE TABLE [dbo].[dict_privat_obj_group] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (1)   NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

