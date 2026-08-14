CREATE TABLE [dbo].[arenda_photos] (
    [id]          INT              NOT NULL,
    [arenda_id]   INT              NOT NULL,
    [file_name]   VARCHAR (255)    NOT NULL,
    [file_ext]    VARCHAR (255)    NOT NULL,
    [user_id]     UNIQUEIDENTIFIER NOT NULL,
    [create_date] DATETIME         NOT NULL,
    CONSTRAINT [PK_arenda_photos] PRIMARY KEY CLUSTERED ([id] ASC)
);

