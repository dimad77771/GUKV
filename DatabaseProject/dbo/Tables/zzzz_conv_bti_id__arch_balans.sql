CREATE TABLE [dbo].[zzzz_conv_bti_id__arch_balans] (
    [archive_id] INT           NOT NULL,
    [old_bti_id] INT           NULL,
    [new_bti_id] VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([archive_id] ASC)
);

