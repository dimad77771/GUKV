CREATE TABLE [dbo].[fin_report_columns] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [col_index]   INT           NOT NULL,
    [form_id]     INT           NOT NULL,
    [is_disabled] INT           NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_fin_col_form] FOREIGN KEY ([form_id]) REFERENCES [dbo].[fin_report_forms] ([id])
);

