CREATE TABLE [dbo].[transfer_requests_akt_attachfiles] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [free_square_id] INT           NOT NULL,
    [file_name]      VARCHAR (255) NOT NULL,
    [file_ext]       VARCHAR (255) NOT NULL,
    [modify_date]    DATETIME      NOT NULL,
    [modified_by]    VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_transfer_requests_akt_attachfiles] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_transfer_requests_akt_attachfiles_transfer_requests] FOREIGN KEY ([free_square_id]) REFERENCES [dbo].[transfer_requests] ([request_id]) ON DELETE CASCADE ON UPDATE CASCADE
);

