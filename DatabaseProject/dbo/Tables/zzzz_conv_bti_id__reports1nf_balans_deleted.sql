CREATE TABLE [dbo].[zzzz_conv_bti_id__reports1nf_balans_deleted] (
    [report_id]  INT           NOT NULL,
    [id]         INT           NOT NULL,
    [old_bti_id] INT           NULL,
    [new_bti_id] VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([report_id] ASC, [id] ASC)
);

