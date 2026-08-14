CREATE TABLE [dbo].[dict_org_old_organ_addinfo] (
    [id]   INT            NOT NULL,
    [zkpo] VARCHAR (20)   NULL,
    [info] VARCHAR (1000) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([id]) REFERENCES [dbo].[dict_org_old_organ] ([id])
);

