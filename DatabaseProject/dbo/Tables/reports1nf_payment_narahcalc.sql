CREATE TABLE [dbo].[reports1nf_payment_narahcalc] (
    [id]          INT             IDENTITY (1, 1) NOT NULL,
    [report_id]   INT             NOT NULL,
    [arenda_id]   INT             NOT NULL,
    [narah_date]  DATE            NOT NULL,
    [narah_sum]   NUMERIC (15, 2) NULL,
    [modify_date] DATE            NULL,
    [modified_by] VARCHAR (128)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK__reports1nf_payment_narahcalc__reports1nf_arenda] FOREIGN KEY ([report_id], [arenda_id]) REFERENCES [dbo].[reports1nf_arenda] ([report_id], [id]) ON DELETE CASCADE ON UPDATE CASCADE,
    UNIQUE NONCLUSTERED ([report_id] ASC, [arenda_id] ASC, [narah_date] ASC)
);


GO
CREATE NONCLUSTERED INDEX [indx__reports1nf_payment_narahcalc__id]
    ON [dbo].[reports1nf_payment_narahcalc]([id] ASC);

