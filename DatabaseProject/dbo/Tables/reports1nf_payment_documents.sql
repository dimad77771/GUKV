CREATE TABLE [dbo].[reports1nf_payment_documents] (
    [id]                INT             IDENTITY (1, 1) NOT NULL,
    [report_id]         INT             NOT NULL,
    [arenda_id]         INT             NULL,
    [payment_date]      DATE            NOT NULL,
    [payment_number]    VARCHAR (64)    NULL,
    [payment_sum]       NUMERIC (15, 2) NULL,
    [payment_purpose]   VARCHAR (256)   NULL,
    [modify_date]       DATE            NULL,
    [modified_by]       VARCHAR (128)   NULL,
    [rent_period_id]    INT             NOT NULL,
    [payment_sm_1]      NUMERIC (15, 2) NULL,
    [payment_sm_2]      NUMERIC (15, 2) NULL,
    [payment_sm_3]      NUMERIC (15, 2) NULL,
    [payment_sm_4]      NUMERIC (15, 2) NULL,
    [source_excel_file] VARCHAR (1000)  NULL,
    [source_excel_row]  INT             NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

