CREATE TABLE [dbo].[ClipbordForUsers] (
    [username]     VARCHAR (1000) NOT NULL,
    [clipborddata] VARCHAR (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([username] ASC)
);

