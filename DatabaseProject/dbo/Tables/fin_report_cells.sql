CREATE TABLE [dbo].[fin_report_cells] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [form_id]     INT           NOT NULL,
    [row_index]   INT           NOT NULL,
    [col_index]   INT           NOT NULL,
    [is_disabled] INT           NOT NULL,
    [cell_text]   VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_fin_cell_form] FOREIGN KEY ([form_id]) REFERENCES [dbo].[fin_report_forms] ([id])
);

