CREATE TABLE [dbo].[fin_report_rows] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [row_index]   INT           NOT NULL,
    [form_id]     INT           NOT NULL,
    [is_disabled] INT           NULL,
    [modified_by] VARCHAR (128) NULL,
    [modify_date] DATE          NULL,
    [name]        VARCHAR (150) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_fin_row_form] FOREIGN KEY ([form_id]) REFERENCES [dbo].[fin_report_forms] ([id])
);

