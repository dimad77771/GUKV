CREATE TABLE [dbo].[organizations_from_derzh_reestr] (
    [zkpo_code]  VARCHAR (16)  NOT NULL,
    [full_name]  VARCHAR (255) NOT NULL,
    [short_name] VARCHAR (255) NOT NULL,
    [status_id]  INT           NOT NULL,
    CONSTRAINT [PK__organizations_from_derzh_reestr] PRIMARY KEY CLUSTERED ([zkpo_code] ASC)
);

