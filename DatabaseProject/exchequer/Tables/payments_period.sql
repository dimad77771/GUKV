CREATE TABLE [exchequer].[payments_period] (
    [payment_id]   INT      NOT NULL,
    [ident_period] DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([payment_id] ASC, [ident_period] ASC),
    FOREIGN KEY ([payment_id]) REFERENCES [exchequer].[payments] ([payment_id]) ON DELETE CASCADE
);

