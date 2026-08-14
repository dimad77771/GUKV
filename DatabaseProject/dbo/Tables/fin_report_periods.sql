CREATE TABLE [dbo].[fin_report_periods] (
    [id]           INT           NOT NULL,
    [name]         VARCHAR (30)  NULL,
    [period_start] DATE          NULL,
    [modified_by]  VARCHAR (128) NULL,
    [modify_date]  DATE          NULL,
    [is_fin_plan]  INT           NULL,
    [parent_id]    INT           NULL,
    [is_deleted]   INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

