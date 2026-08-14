CREATE TABLE [dbo].[zzzz20220109_002] (
    [eqv_nom]            INT           NULL,
    [A_addrnom]          VARCHAR (227) NULL,
    [B_addrnom]          VARCHAR (227) NULL,
    [eqv_1]              INT           NOT NULL,
    [eqv_2]              INT           NOT NULL,
    [A_addr_street_id]   INT           NULL,
    [B_addr_street_id]   INT           NULL,
    [A_addr_street_name] VARCHAR (128) NULL,
    [B_addr_street_name] VARCHAR (128) NULL,
    [id]                 INT           NOT NULL,
    [master_building_id] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

