CREATE TABLE [dbo].[rent_balans_org] (
    [organization_id]    INT NOT NULL,
    [rent_occupation_id] INT NULL,
    [rent_dkk_id]        INT NULL,
    [p_energo_kanal]     INT DEFAULT ((0)) NULL,
    [p_zvit]             INT DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([organization_id] ASC),
    CONSTRAINT [fk_rent_balans_org_dkk] FOREIGN KEY ([rent_dkk_id]) REFERENCES [dbo].[dict_rent_dkk] ([id]),
    CONSTRAINT [fk_rent_balans_org_occupation] FOREIGN KEY ([rent_occupation_id]) REFERENCES [dbo].[dict_rent_occupation] ([id]),
    CONSTRAINT [fk_rent_balans_org_org] FOREIGN KEY ([organization_id]) REFERENCES [dbo].[organizations] ([id])
);

