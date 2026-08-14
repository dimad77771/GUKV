CREATE TABLE [dbo].[zzzzz20251208__del__reports1nf_payment_narahcalc] (
    [id]          INT             IDENTITY (1, 1) NOT NULL,
    [report_id]   INT             NOT NULL,
    [arenda_id]   INT             NOT NULL,
    [narah_date]  DATE            NOT NULL,
    [narah_sum]   NUMERIC (15, 2) NULL,
    [modify_date] DATE            NULL,
    [modified_by] VARCHAR (128)   NULL
);

