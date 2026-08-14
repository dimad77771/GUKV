CREATE TABLE [dbo].[rent_payment_by_renter] (
    [id]                     INT             IDENTITY (1, 1) NOT NULL,
    [rent_renter_org_id]     INT             NULL,
    [sqr_total_rent]         NUMERIC (15, 3) NULL,
    [sqr_payed_by_percent]   NUMERIC (15, 3) NULL,
    [sqr_payed_by_1uah]      NUMERIC (15, 3) NULL,
    [sqr_payed_hourly]       NUMERIC (15, 3) NULL,
    [payment_narah]          NUMERIC (15, 3) NULL,
    [payment_received]       NUMERIC (15, 3) NULL,
    [payment_budget_50_uah]  NUMERIC (15, 3) NULL,
    [rent_payment_id]        INT             NULL,
    [renter_organization_id] INT             NULL,
    [rent_period_id]         INT             NULL,
    [giver_organization_id]  INT             NULL,
    [agreement_flag]         INT             DEFAULT ((0)) NULL,
    [payment_nar_zvit]       NUMERIC (15, 3) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_payment_by_renter_org_giver] FOREIGN KEY ([giver_organization_id]) REFERENCES [dbo].[organizations] ([id]),
    CONSTRAINT [fk_payment_by_renter_org_renter] FOREIGN KEY ([renter_organization_id]) REFERENCES [dbo].[organizations] ([id]),
    CONSTRAINT [fk_payment_by_renter_payment] FOREIGN KEY ([rent_payment_id]) REFERENCES [dbo].[rent_payment] ([id]),
    CONSTRAINT [fk_payment_by_renter_period] FOREIGN KEY ([rent_period_id]) REFERENCES [dbo].[dict_rent_period] ([id]),
    CONSTRAINT [fk_payment_by_renter_renter] FOREIGN KEY ([rent_renter_org_id]) REFERENCES [dbo].[rent_renter_org] ([id])
);

