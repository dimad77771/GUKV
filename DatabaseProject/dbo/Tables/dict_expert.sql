CREATE TABLE [dbo].[dict_expert] (
    [id]                   INT           NOT NULL,
    [full_name]            VARCHAR (250) NULL,
    [short_name]           VARCHAR (100) NULL,
    [modified_by]          VARCHAR (128) NULL,
    [modify_date]          DATE          NULL,
    [fio_boss]             VARCHAR (100) NULL,
    [tel_boss]             VARCHAR (80)  NULL,
    [addr_district_id]     INT           NULL,
    [addr_street_id]       INT           NULL,
    [addr_zip_code]        VARCHAR (20)  NULL,
    [addr_number]          VARCHAR (60)  NULL,
    [certificate_num]      VARCHAR (15)  NULL,
    [certificate_date]     DATE          NULL,
    [certificate_end_date] DATE          NULL,
    [case_num]             INT           NULL,
    [addr_full]            VARCHAR (255) NULL,
    [note]                 VARCHAR (255) NULL,
    [is_deleted]           INT           DEFAULT ((0)) NULL,
    [del_date]             DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_dict_expert_district] FOREIGN KEY ([addr_district_id]) REFERENCES [dbo].[dict_districts2] ([id])
);

