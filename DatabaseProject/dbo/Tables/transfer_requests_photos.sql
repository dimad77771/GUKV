CREATE TABLE [dbo].[transfer_requests_photos] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [request_id]  INT              NOT NULL,
    [file_name]   VARCHAR (255)    NOT NULL,
    [file_ext]    VARCHAR (255)    NOT NULL,
    [user_id]     UNIQUEIDENTIFIER NOT NULL,
    [create_date] DATETIME         NOT NULL,
    CONSTRAINT [PK_transfer_requests_photos] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_transfer_requests_photos_request_id] FOREIGN KEY ([request_id]) REFERENCES [dbo].[transfer_requests] ([request_id]) ON DELETE CASCADE
);

