CREATE TABLE [dbo].[sys_report_corrections] (
    [id]              INT           IDENTITY (1, 1) NOT NULL,
    [is_corrected]    INT           NULL,
    [correction_kind] INT           NULL,
    [obj_descr]       VARCHAR (MAX) NULL,
    [action_descr]    VARCHAR (MAX) NULL,
    [reason_descr]    VARCHAR (MAX) NULL,
    [modify_date]     DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

