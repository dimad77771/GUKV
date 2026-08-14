CREATE TABLE [dbo].[rent_renter_org] (
    [id]              INT           NOT NULL,
    [name]            VARCHAR (255) NULL,
    [zkpo_code]       VARCHAR (64)  NULL,
    [organization_id] INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_rent_renter_org_org] FOREIGN KEY ([organization_id]) REFERENCES [dbo].[organizations] ([id])
);

