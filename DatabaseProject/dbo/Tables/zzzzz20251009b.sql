CREATE TABLE [dbo].[zzzzz20251009b] (
    [arenda_id]    INT           NULL,
    [report_id]    INT           NOT NULL,
    [to_report_id] INT           NOT NULL,
    [org_id]       INT           NOT NULL,
    [to_org_id]    INT           NOT NULL,
    [cnt]          INT           NULL,
    [zkpo_1]       VARCHAR (100) NULL,
    [zkpo_2]       VARCHAR (100) NULL,
    [dognum]       VARCHAR (100) NULL,
    [sdat]         VARCHAR (100) NULL,
    UNIQUE NONCLUSTERED ([arenda_id] ASC, [report_id] ASC),
    UNIQUE NONCLUSTERED ([arenda_id] ASC, [to_report_id] ASC)
);

