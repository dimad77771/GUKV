CREATE TABLE [dbo].[reports1nf_org_corprava] (
    [id]           INT             IDENTITY (1, 1) NOT NULL,
    [report_id]    INT             NOT NULL,
    [vlasn_name]   VARCHAR (1000)  NULL,
    [vlasn_inn]    VARCHAR (1000)  NULL,
    [vlasn_zkpo]   VARCHAR (1000)  NULL,
    [vlasn_kopfg]  VARCHAR (1000)  NULL,
    [vlasn_uradr]  VARCHAR (4000)  NULL,
    [is_zasnobn]   BIT             DEFAULT ((0)) NOT NULL,
    [kol_akcia]    NUMERIC (15)    NULL,
    [total_price]  NUMERIC (15, 2) NULL,
    [part_price]   NUMERIC (15, 3) NULL,
    [stat_found]   NUMERIC (15, 2) NULL,
    [form_akcia]   VARCHAR (1000)  NULL,
    [nominal_vart] NUMERIC (15, 2) NULL,
    [stan_vlasn]   VARCHAR (1000)  NULL,
    [primitki]     VARCHAR (1000)  NULL,
    CONSTRAINT [PK_reports1nf_org_corprava] PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([report_id]) REFERENCES [dbo].[reports1nf_org_info] ([report_id])
);

