CREATE TABLE [dbo].[unverified_balans] (
    [id]                 INT             IDENTITY (1, 1) NOT NULL,
    [sqr_total]          NUMERIC (9, 2)  NULL,
    [cost_balans]        NUMERIC (15, 2) NULL,
    [form_ownership_id]  INT             NULL,
    [object_kind_id]     INT             NULL,
    [object_type_id]     INT             NULL,
    [purpose_group_id]   INT             NULL,
    [purpose_id]         INT             NULL,
    [purpose_str]        NVARCHAR (255)  NULL,
    [addr_street_id]     INT             NULL,
    [addr_district_id]   INT             NULL,
    [addr_nomer1]        VARCHAR (32)    NULL,
    [addr_nomer2]        VARCHAR (32)    NULL,
    [addr_nomer3]        VARCHAR (32)    NULL,
    [building_id]        INT             NULL,
    [is_building_exists] BIT             NULL,
    CONSTRAINT [PK_unverified_balans] PRIMARY KEY CLUSTERED ([id] ASC)
);

