CREATE TABLE [dbo].[reports1nf_accounts] (
    [id]                INT              IDENTITY (1, 1) NOT NULL,
    [UserId]            UNIQUEIDENTIFIER NOT NULL,
    [organization_id]   INT              NOT NULL,
    [rda_district_id]   INT              NULL,
    [fin_add_showed]    INT              NULL,
    [misto_district_id] INT              NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

