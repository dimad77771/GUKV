CREATE TABLE [dbo].[balans_photos] (
    [id]          INT              NOT NULL,
    [bal_id]      INT              NOT NULL,
    [file_name]   VARCHAR (255)    NOT NULL,
    [file_ext]    VARCHAR (255)    NOT NULL,
    [user_id]     UNIQUEIDENTIFIER NOT NULL,
    [create_date] DATETIME         NOT NULL,
    CONSTRAINT [PK_balans_photos] PRIMARY KEY CLUSTERED ([id] ASC)
);

