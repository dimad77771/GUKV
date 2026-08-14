CREATE TABLE [dbo].[user_folders] (
    [id]               INT              IDENTITY (1, 1) NOT NULL,
    [parent_folder_id] INT              NULL,
    [usr_id]           UNIQUEIDENTIFIER NULL,
    [folder_name]      VARCHAR (255)    NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

