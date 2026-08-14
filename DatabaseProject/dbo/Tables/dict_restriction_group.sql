CREATE TABLE [dbo].[dict_restriction_group] (
    [id]          INT           NOT NULL,
    [name]        CHAR (50)     NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

