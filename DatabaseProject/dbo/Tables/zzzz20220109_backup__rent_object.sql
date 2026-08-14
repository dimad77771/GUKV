CREATE TABLE [dbo].[zzzz20220109_backup__rent_object] (
    [id]               INT             IDENTITY (1, 1) NOT NULL,
    [building_id]      INT             NULL,
    [rent_payment_id]  INT             NULL,
    [rent_period_id]   INT             NULL,
    [addr_street]      VARCHAR (255)   NULL,
    [addr_street_type] VARCHAR (64)    NULL,
    [addr_num]         VARCHAR (255)   NULL,
    [sqr_total]        NUMERIC (15, 3) NULL,
    [sqr_rented]       NUMERIC (15, 3) NULL,
    [sqr_free]         NUMERIC (15, 3) NULL,
    [sqr_korysna]      NUMERIC (15, 3) NULL,
    [sqr_mzk]          NUMERIC (15, 3) NULL,
    [floors]           VARCHAR (255)   NULL,
    [tech_state_id]    INT             NULL,
    [is_energo]        INT             NULL,
    [is_vodo]          INT             NULL,
    [is_teplo]         INT             NULL,
    [district_id]      INT             NULL,
    [purpose]          VARCHAR (255)   NULL,
    [rent_note_id]     INT             NULL
);

