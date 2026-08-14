CREATE TABLE [dbo].[reports1nf_payment_narah_prognoz] (
    [id]         INT             IDENTITY (1, 1) NOT NULL,
    [report_id]  INT             NOT NULL,
    [arenda_id]  INT             NULL,
    [narah_date] DATE            NOT NULL,
    [narah_sum]  NUMERIC (15, 2) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

