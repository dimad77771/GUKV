CREATE TABLE [dbo].[reports1nf_org_info_new_contribution_rate] (
    [report_id]         INT NOT NULL,
    [contribution_rate] INT NOT NULL,
    CONSTRAINT [PK__reports1nf_org_info_new_contribution_rate] PRIMARY KEY CLUSTERED ([report_id] ASC),
    CONSTRAINT [FK_reports1nf_org_info_new_contribution_rate_reports1nf] FOREIGN KEY ([report_id]) REFERENCES [dbo].[reports1nf_org_info] ([report_id]) ON DELETE CASCADE ON UPDATE CASCADE
);

