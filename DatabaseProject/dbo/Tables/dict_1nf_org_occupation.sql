CREATE TABLE [dbo].[dict_1nf_org_occupation] (
    [id]          INT            NOT NULL,
    [name]        VARCHAR (1000) NULL,
    [branch_code] INT            NULL,
    [kvedcod]     VARCHAR (100)  NULL,
    [kvednam]     VARCHAR (1000) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

