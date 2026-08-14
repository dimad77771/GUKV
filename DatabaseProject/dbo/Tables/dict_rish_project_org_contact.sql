CREATE TABLE [dbo].[dict_rish_project_org_contact] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [project_org_id] INT           NOT NULL,
    [contact_name]   VARCHAR (128) NOT NULL,
    [contact_title]  VARCHAR (128) NULL,
    [contact_phone]  VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dict_rish_project_org_contact] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_dict_rish_project_org_contact_dict_rish_project_org] FOREIGN KEY ([project_org_id]) REFERENCES [dbo].[dict_rish_project_org] ([id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [UC_dict_rish_project_org_contact] UNIQUE NONCLUSTERED ([project_org_id] ASC, [contact_name] ASC)
);

