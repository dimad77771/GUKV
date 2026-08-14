CREATE TABLE [dbo].[freecycle_step] (
    [freecycle_step_id]    INT             IDENTITY (1, 1) NOT NULL,
    [freecycle_id]         INT             NOT NULL,
    [freecycle_orendar_id] INT             NOT NULL,
    [step_id]              INT             NOT NULL,
    [step_date]            DATETIME        NULL,
    [step_docnum]          VARCHAR (512)   NULL,
    [step_c1]              DATETIME        NULL,
    [step_c2]              DECIMAL (18, 2) NULL,
    [step_c3]              VARCHAR (512)   NULL,
    [modify_date]          DATETIME        NULL,
    [modified_by]          VARCHAR (128)   NULL,
    [is_deleted]           BIT             DEFAULT ((0)) NOT NULL,
    [step_c4]              INT             NULL,
    CONSTRAINT [PK_freecycle_step] PRIMARY KEY CLUSTERED ([freecycle_step_id] ASC),
    FOREIGN KEY ([freecycle_id]) REFERENCES [dbo].[freecycle] ([freecycle_id]),
    FOREIGN KEY ([freecycle_orendar_id]) REFERENCES [dbo].[freecycle_orendar] ([freecycle_orendar_id]),
    FOREIGN KEY ([step_id]) REFERENCES [dbo].[freecycle_step_dict] ([step_id]),
    CONSTRAINT [UK_freecycle_step] UNIQUE NONCLUSTERED ([freecycle_id] ASC, [freecycle_orendar_id] ASC, [step_id] ASC)
);

