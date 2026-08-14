CREATE TABLE [dbo].[zzzzz20250727_correct_2] (
    [report_buildings] INT NOT NULL,
    [report_balans]    INT NOT NULL,
    [building_id]      INT NOT NULL,
    [balans_id]        INT NOT NULL,
    [unique_id]        INT NOT NULL,
    UNIQUE NONCLUSTERED ([unique_id] ASC)
);

