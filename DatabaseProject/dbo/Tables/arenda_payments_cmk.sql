CREATE TABLE [dbo].[arenda_payments_cmk] (
    [id]                    INT             IDENTITY (1, 1) NOT NULL,
    [arenda_rented_id]      INT             NOT NULL,
    [rent_period_id]        INT             NULL,
    [cmk_sqr_rented]        NUMERIC (15, 3) NULL,
    [cmk_payment_narah]     NUMERIC (15, 3) NULL,
    [cmk_payment_to_budget] NUMERIC (15, 3) NULL,
    [cmk_rent_debt]         NUMERIC (15, 3) NULL,
    [modify_date]           DATETIME        NULL,
    [modified_by]           VARCHAR (128)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

