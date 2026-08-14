CREATE TABLE [dbo].[zzz20240228_auction_uchasnik] (
    [id]             INT              IDENTITY (1, 1) NOT NULL,
    [free_square_id] INT              NOT NULL,
    [UserId]         UNIQUEIDENTIFIER NOT NULL,
    [zayavka_date]   DATETIME         NOT NULL,
    [is_arhiv]       BIT              NOT NULL,
    [create_date]    DATETIME         NULL,
    [create_by]      VARCHAR (128)    NULL,
    [modify_date]    DATETIME         NULL,
    [modified_by]    VARCHAR (128)    NULL
);

