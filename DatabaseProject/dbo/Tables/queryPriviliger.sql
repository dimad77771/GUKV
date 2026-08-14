CREATE TABLE [dbo].[queryPriviliger] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [zamovn]         VARCHAR (4000) NULL,
    [plosha]         VARCHAR (1000) NULL,
    [adresat]        VARCHAR (4000) NULL,
    [regzvern]       VARCHAR (255)  NULL,
    [datzvern]       DATETIME       NULL,
    [regnum]         VARCHAR (255)  NULL,
    [dat]            DATETIME       NULL,
    [modify_date2]   DATETIME       NULL,
    [modified_by2]   VARCHAR (128)  NULL,
    [org_info_id]    INT            NULL,
    [addr_street_id] INT            NULL,
    [addr_nomer]     VARCHAR (100)  NULL,
    [total_free_sqr] NUMERIC (9, 2) NULL,
    [primitke]       VARCHAR (8000) NULL,
    [vilne_id]       INT            NULL,
    [vidpovid_num]   VARCHAR (200)  NULL,
    [vidpovid_date]  DATE           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([addr_street_id]) REFERENCES [dbo].[dict_streets] ([id]),
    FOREIGN KEY ([org_info_id]) REFERENCES [dbo].[reports1nf_org_info] ([report_id])
);

