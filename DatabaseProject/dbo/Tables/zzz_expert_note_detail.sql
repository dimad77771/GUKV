CREATE TABLE [dbo].[zzz_expert_note_detail] (
    [id]              INT             IDENTITY (1, 1) NOT NULL,
    [expert_note_id]  INT             NULL,
    [obj_square]      NUMERIC (15, 2) NULL,
    [cost_1_usd]      NUMERIC (15, 2) NULL,
    [cost_1_usd_flag] INT             NULL,
    [valuation_date]  DATE            NULL,
    [floors]          VARCHAR (100)   NULL,
    [purpose]         VARCHAR (100)   NULL,
    [note]            VARCHAR (255)   NULL,
    [modified_by]     VARCHAR (128)   NULL,
    [modify_date]     DATE            NULL,
    [is_deleted]      INT             NULL,
    [del_date]        DATE            NULL
);

