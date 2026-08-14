CREATE TABLE [dbo].[zzzz20250810_C] (
    [from_bal_id]    INT NOT NULL,
    [from_report_id] INT NOT NULL,
    [to_bal_id]      INT NOT NULL,
    [to_report_id]   INT NOT NULL,
    UNIQUE NONCLUSTERED ([from_bal_id] ASC),
    UNIQUE NONCLUSTERED ([to_bal_id] ASC)
);

