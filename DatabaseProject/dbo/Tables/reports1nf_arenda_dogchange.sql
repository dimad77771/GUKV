CREATE TABLE [dbo].[reports1nf_arenda_dogchange] (
    [id]                      INT             IDENTITY (1, 1) NOT NULL,
    [arenda_id]               INT             NOT NULL,
    [report_id]               INT             NOT NULL,
    [agreement_date]          DATE            NULL,
    [agreement_num]           VARCHAR (100)   NULL,
    [rent_start_date]         DATE            NULL,
    [rent_finish_date]        DATE            NULL,
    [rent_actual_finish_date] DATE            NULL,
    [rent_rate]               NUMERIC (15, 3) NULL,
    [base_month]              DATE            NULL,
    [method_calc_id]          INT             NULL,
    [invnum_rent]             VARCHAR (MAX)   NULL,
    [rent_used]               NUMERIC (15, 2) NULL,
    CONSTRAINT [PK_reports1nf_arenda_dogchange] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK__reports1nf_arenda_dogchange__reports1nf_arenda] FOREIGN KEY ([report_id], [arenda_id]) REFERENCES [dbo].[reports1nf_arenda] ([report_id], [id]) ON DELETE CASCADE ON UPDATE CASCADE
);

