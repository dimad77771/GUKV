CREATE TABLE [dbo].[fin_form_changes] (
    [id]              INT           IDENTITY (1, 1) NOT NULL,
    [change_date]     DATE          NULL,
    [order_num]       VARCHAR (20)  NULL,
    [modified_by]     VARCHAR (128) NULL,
    [modify_date]     DATE          NULL,
    [form_id]         INT           NULL,
    [organization_id] INT           NULL,
    [note]            VARCHAR (126) NULL,
    [is_active]       INT           NULL,
    [form_status]     INT           NULL,
    [save_date]       DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_fin_form_changes_form] FOREIGN KEY ([form_id]) REFERENCES [dbo].[fin_report_forms] ([id]),
    CONSTRAINT [fk_fin_form_changes_org] FOREIGN KEY ([organization_id]) REFERENCES [dbo].[organizations] ([id])
);

