CREATE TABLE [dbo].[user_notification_settings] (
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [organization_id] INT              NOT NULL,
    [is_notify]       INT              DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC, [organization_id] ASC)
);

