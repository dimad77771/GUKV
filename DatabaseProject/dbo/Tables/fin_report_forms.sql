CREATE TABLE [dbo].[fin_report_forms] (
    [id]             INT           NOT NULL,
    [is_active]      VARCHAR (1)   NULL,
    [report_type_id] INT           NOT NULL,
    [form_type_id]   INT           NOT NULL,
    [form_name]      VARCHAR (200) NULL,
    [modified_by]    VARCHAR (128) NULL,
    [modify_date]    DATE          NULL,
    [form_number]    VARCHAR (20)  NULL,
    [file_name1]     VARCHAR (250) NULL,
    [period_id]      INT           NOT NULL,
    [form_kind_id]   INT           DEFAULT ((1)) NOT NULL,
    [file_name2]     VARCHAR (50)  NULL,
    [num_fixed_cols] INT           NULL,
    [num_fixed_rows] INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_fin_form_kind] FOREIGN KEY ([form_kind_id]) REFERENCES [dbo].[dict_fin_form_kind] ([id]),
    CONSTRAINT [fk_fin_form_period] FOREIGN KEY ([period_id]) REFERENCES [dbo].[fin_report_periods] ([id]),
    CONSTRAINT [fk_fin_form_report_type] FOREIGN KEY ([report_type_id]) REFERENCES [dbo].[dict_fin_report_type] ([id]),
    CONSTRAINT [fk_fin_form_type] FOREIGN KEY ([form_type_id]) REFERENCES [dbo].[dict_fin_form_type] ([id])
);

