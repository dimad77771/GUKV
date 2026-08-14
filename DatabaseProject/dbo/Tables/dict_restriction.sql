CREATE TABLE [dbo].[dict_restriction] (
    [id]          INT           NOT NULL,
    [group_id]    INT           NULL,
    [name]        VARCHAR (70)  NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

