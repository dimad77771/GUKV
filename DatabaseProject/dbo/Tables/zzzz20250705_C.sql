CREATE TABLE [dbo].[zzzz20250705_C] (
    [from_bal_id]    INT NULL,
    [from_report_id] INT NOT NULL,
    [to_bal_id]      INT NULL,
    [to_report_id]   INT NOT NULL,
    UNIQUE NONCLUSTERED ([from_bal_id] ASC),
    UNIQUE NONCLUSTERED ([to_bal_id] ASC)
);

