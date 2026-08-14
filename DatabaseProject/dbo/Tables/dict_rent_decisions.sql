CREATE TABLE [dbo].[dict_rent_decisions] (
    [id]               INT           NOT NULL,
    [name]             VARCHAR (40)  NULL,
    [name_abbr]        VARCHAR (2)   NULL,
    [modified_by]      VARCHAR (128) NULL,
    [modify_date]      DATE          NULL,
    [name_report_form] VARCHAR (255) NULL,
    [name_order_form]  VARCHAR (40)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

