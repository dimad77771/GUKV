CREATE TABLE [dbo].[dict_org_vedomstvo] (
    [id]          INT           NOT NULL,
    [name]        VARCHAR (255) NULL,
    [full_name]   VARCHAR (255) NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

