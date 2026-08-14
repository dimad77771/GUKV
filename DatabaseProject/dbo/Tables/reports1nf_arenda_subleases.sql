CREATE TABLE [dbo].[reports1nf_arenda_subleases] (
    [id]                 INT             IDENTITY (1, 1) NOT NULL,
    [arenda_id]          INT             NULL,
    [payment_type_id]    INT             NULL,
    [agreement_date]     DATE            NULL,
    [agreement_num]      VARCHAR (18)    NULL,
    [rent_start_date]    DATE            NULL,
    [rent_finish_date]   DATE            NULL,
    [rent_square]        NUMERIC (9, 2)  NULL,
    [rent_payment_month] NUMERIC (15, 3) NULL,
    [report_id]          INT             NOT NULL,
    [modified_by]        VARCHAR (128)   NULL,
    [modify_date]        DATE            NULL,
    [using_possible_id]  INT             NULL,
    PRIMARY KEY CLUSTERED ([report_id] ASC, [id] ASC),
    CONSTRAINT [FK__reports1n__using__225C629A] FOREIGN KEY ([using_possible_id]) REFERENCES [dbo].[dict_rental_rate] ([id]),
    CONSTRAINT [FK_reports1nf_arenda_subleases_reports1nf] FOREIGN KEY ([report_id]) REFERENCES [dbo].[reports1nf] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

