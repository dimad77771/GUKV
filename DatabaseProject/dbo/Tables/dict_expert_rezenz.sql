CREATE TABLE [dbo].[dict_expert_rezenz] (
    [id]         INT           NOT NULL,
    [name]       VARCHAR (100) NULL,
    [is_deleted] INT           DEFAULT ((0)) NULL,
    [is_dkv]     SMALLINT      DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

