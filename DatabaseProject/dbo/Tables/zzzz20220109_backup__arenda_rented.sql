CREATE TABLE [dbo].[zzzz20220109_backup__arenda_rented] (
    [id]                    INT             IDENTITY (1, 1) NOT NULL,
    [building_id]           INT             NULL,
    [org_renter_id]         INT             NULL,
    [org_giver_id]          INT             NULL,
    [payment_type_id]       INT             NULL,
    [agreement_date]        DATE            NULL,
    [agreement_num]         VARCHAR (18)    NULL,
    [rent_start_date]       DATE            NULL,
    [rent_finish_date]      DATE            NULL,
    [rent_square]           NUMERIC (9, 2)  NULL,
    [is_subarenda]          INT             NULL,
    [is_cmk]                INT             NULL,
    [cmk_sqr_rented]        NUMERIC (15, 3) NULL,
    [cmk_payment_narah]     NUMERIC (15, 3) NULL,
    [cmk_payment_to_budget] NUMERIC (15, 3) NULL,
    [cmk_rent_debt]         NUMERIC (15, 3) NULL,
    [modify_date]           DATETIME        NULL,
    [modified_by]           VARCHAR (128)   NULL,
    [is_deleted]            INT             NULL,
    [del_date]              DATETIME        NULL
);

