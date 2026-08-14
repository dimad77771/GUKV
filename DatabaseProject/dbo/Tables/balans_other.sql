CREATE TABLE [dbo].[balans_other] (
    [id]                  INT             IDENTITY (1, 1) NOT NULL,
    [org_id]              INT             NOT NULL,
    [name]                VARCHAR (MAX)   NULL,
    [address]             VARCHAR (512)   NULL,
    [inv_number]          VARCHAR (32)    NULL,
    [initial_cost]        DECIMAL (18, 2) NULL,
    [remaining_cost]      DECIMAL (18, 2) NULL,
    [location]            VARCHAR (512)   NULL,
    [commissioned_date]   DATE            NULL,
    [decommissioned_date] DATE            NULL,
    [document_number]     VARCHAR (20)    NULL,
    [document_date]       DATE            NULL,
    [on_balance_date]     DATE            NULL,
    CONSTRAINT [PK_balans_other] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_balans_other_organizations] FOREIGN KEY ([org_id]) REFERENCES [dbo].[organizations] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

