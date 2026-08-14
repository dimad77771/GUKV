CREATE TABLE [dbo].[arenda_applications] (
    [id]               INT             NOT NULL,
    [renter_name]      VARCHAR (100)   NULL,
    [rent_goal]        VARCHAR (252)   NULL,
    [rent_term]        VARCHAR (252)   NULL,
    [building_id]      INT             NULL,
    [appl_letter_num]  VARCHAR (9)     NULL,
    [appl_letter_date] DATE            NULL,
    [modified_by]      VARCHAR (128)   NULL,
    [modify_date]      DATE            NULL,
    [appl_sqr]         NUMERIC (15, 2) NULL,
    [org_renter_id]    INT             NULL,
    [subarenda_id]     INT             NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_arenda_appl_building] FOREIGN KEY ([building_id]) REFERENCES [dbo].[documents] ([id]),
    CONSTRAINT [fk_arenda_appl_renter] FOREIGN KEY ([org_renter_id]) REFERENCES [dbo].[organizations] ([id])
);

