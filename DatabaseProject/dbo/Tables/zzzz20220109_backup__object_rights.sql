CREATE TABLE [dbo].[zzzz20220109_backup__object_rights] (
    [id]               INT             IDENTITY (1, 1) NOT NULL,
    [building_id]      INT             NOT NULL,
    [org_from_id]      INT             NULL,
    [org_to_id]        INT             NULL,
    [right_id]         INT             NULL,
    [transfer_date]    DATE            NULL,
    [transfer_year]    INT             NULL,
    [transfer_quarter] INT             NULL,
    [akt_id]           INT             NULL,
    [rozp_id]          INT             NULL,
    [modified_by]      VARCHAR (128)   NULL,
    [modify_date]      DATE            NULL,
    [name]             VARCHAR (255)   NULL,
    [characteristic]   VARCHAR (MAX)   NULL,
    [misc_info]        VARCHAR (MAX)   NULL,
    [sum_balans]       NUMERIC (15, 3) NULL,
    [sum_zalishkova]   NUMERIC (15, 3) NULL,
    [sqr_transferred]  NUMERIC (9, 3)  NULL,
    [len_transferred]  NUMERIC (9, 2)  NULL
);

