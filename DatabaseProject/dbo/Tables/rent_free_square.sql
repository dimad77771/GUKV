CREATE TABLE [dbo].[rent_free_square] (
    [id]               INT             IDENTITY (1, 1) NOT NULL,
    [building_id]      INT             NULL,
    [organization_id]  INT             NULL,
    [rent_period_id]   INT             NULL,
    [sqr_free_total]   NUMERIC (15, 3) NULL,
    [sqr_free_korysna] NUMERIC (15, 3) NULL,
    [sqr_free_mzk]     NUMERIC (15, 3) NULL,
    [free_sqr_floors]  VARCHAR (MAX)   NULL,
    [free_sqr_purpose] VARCHAR (MAX)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_rent_free_square_building] FOREIGN KEY ([building_id]) REFERENCES [dbo].[buildings] ([id]),
    CONSTRAINT [fk_rent_free_square_org] FOREIGN KEY ([organization_id]) REFERENCES [dbo].[organizations] ([id]),
    CONSTRAINT [fk_rent_free_square_period] FOREIGN KEY ([rent_period_id]) REFERENCES [dbo].[dict_rent_period] ([id])
);

