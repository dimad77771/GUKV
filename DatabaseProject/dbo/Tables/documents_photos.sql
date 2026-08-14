CREATE TABLE [dbo].[documents_photos] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [document_id] INT              NOT NULL,
    [file_name]   VARCHAR (255)    NOT NULL,
    [file_ext]    VARCHAR (255)    NOT NULL,
    [user_id]     UNIQUEIDENTIFIER NOT NULL,
    [create_date] DATETIME         NOT NULL,
    CONSTRAINT [PK_documents_photos] PRIMARY KEY CLUSTERED ([id] ASC)
);

