CREATE TABLE [dbo].[org_by_period] (
    [period_id]         INT NOT NULL,
    [org_occupation_id] INT NOT NULL,
    [org_id]            INT NOT NULL,
    CONSTRAINT [PK_org_by_period] PRIMARY KEY CLUSTERED ([org_id] ASC, [org_occupation_id] ASC, [period_id] ASC)
);

