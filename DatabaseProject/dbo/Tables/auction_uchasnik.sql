CREATE TABLE [dbo].[auction_uchasnik] (
    [id]             INT              IDENTITY (1, 1) NOT NULL,
    [free_square_id] INT              NOT NULL,
    [UserId]         UNIQUEIDENTIFIER NOT NULL,
    [zayavka_date]   DATETIME         NOT NULL,
    [is_arhiv]       BIT              DEFAULT ((0)) NOT NULL,
    [create_date]    DATETIME         NULL,
    [create_by]      VARCHAR (128)    NULL,
    [modify_date]    DATETIME         NULL,
    [modified_by]    VARCHAR (128)    NULL,
    CONSTRAINT [PK_auction_uchasnik] PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId]),
    CONSTRAINT [FK_auction_uchasnik__reports1nf_balans_free_square] FOREIGN KEY ([free_square_id]) REFERENCES [dbo].[reports1nf_balans_free_square] ([id])
);

