CREATE TABLE [dbo].[rent_vipiski] (
    [id]                   INT             IDENTITY (1, 1) NOT NULL,
    [zkpo_code]            VARCHAR (13)    NULL,
    [payment_date]         DATE            NULL,
    [payment_sum]          NUMERIC (15, 3) NULL,
    [rent_payment_type_id] INT             NULL,
    [org_balans_id]        INT             NULL,
    [organization_id]      INT             NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_rent_vipiski_org] FOREIGN KEY ([organization_id]) REFERENCES [dbo].[organizations] ([id]),
    CONSTRAINT [fk_rent_vipiski_org_balans] FOREIGN KEY ([org_balans_id]) REFERENCES [dbo].[organizations] ([id]),
    CONSTRAINT [fk_rent_vipiski_payment_type] FOREIGN KEY ([rent_payment_type_id]) REFERENCES [dbo].[dict_rent_payment_type] ([id])
);

