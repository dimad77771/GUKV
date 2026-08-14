CREATE TABLE [yurdep].[housr] (
    [housr_id] INT           IDENTITY (1, 1) NOT NULL,
    [delo_id]  INT           NULL,
    [ousr]     VARCHAR (200) NULL,
    [db]       DATETIME      NULL,
    [de]       DATETIME      NULL,
    [hprim]    TEXT          NULL,
    CONSTRAINT [housr_x] PRIMARY KEY CLUSTERED ([housr_id] ASC)
);

