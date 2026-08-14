CREATE TABLE [exchequer].[payments] (
    [payment_id]     INT             IDENTITY (1, 1) NOT NULL,
    [pay_zkpo]       VARCHAR (100)   NULL,
    [pay_name]       VARCHAR (8000)  NULL,
    [pay_date]       DATETIME        NULL,
    [pay_sum]        DECIMAL (18, 2) NULL,
    [pay_text]       VARCHAR (MAX)   NULL,
    [ident_bal_zkpo] VARCHAR (100)   NULL,
    [ident_bal_name] VARCHAR (8000)  NULL,
    [rowstatus]      VARCHAR (10)    NOT NULL,
    [srcfile]        VARCHAR (255)   NULL,
    [corrpay]        INT             NULL,
    [is_return]      AS              (case when [corrpay]=(11) then (1) else (0) end),
    PRIMARY KEY CLUSTERED ([payment_id] ASC),
    CHECK ([rowstatus]='0' OR [rowstatus]='-' OR [rowstatus]='M' OR [rowstatus]='A'),
    FOREIGN KEY ([corrpay]) REFERENCES [exchequer].[lookup_corrpay] ([corrpay])
);


GO
CREATE NONCLUSTERED INDEX [indx_payments_srcfile]
    ON [exchequer].[payments]([srcfile] ASC);

