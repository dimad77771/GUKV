CREATE TABLE [dbo].[arch_balans_photos] (
    [id]                       INT              NOT NULL,
    [bal_id]                   INT              NOT NULL,
    [file_name]                VARCHAR (255)    NOT NULL,
    [file_ext]                 VARCHAR (255)    NOT NULL,
    [user_id]                  UNIQUEIDENTIFIER NOT NULL,
    [create_date]              DATETIME         NOT NULL,
    [archive_id]               INT              IDENTITY (1, 1) NOT NULL,
    [archive_balans_link_code] INT              NOT NULL,
    CONSTRAINT [PK_arch_balans_photos] PRIMARY KEY CLUSTERED ([archive_id] ASC)
);

