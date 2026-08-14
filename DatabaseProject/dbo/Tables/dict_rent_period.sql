CREATE TABLE [dbo].[dict_rent_period] (
    [id]             INT           NOT NULL,
    [name]           VARCHAR (255) NULL,
    [period_start]   DATE          NULL,
    [period_end]     DATE          NULL,
    [period_quarter] INT           NULL,
    [period_year]    INT           NULL,
    [is_active]      INT           DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

