CREATE TABLE [dbo].[reports1nf_btiphoto] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [bal_id]      INT              NOT NULL,
    [file_name]   VARCHAR (255)    NOT NULL,
    [file_ext]    VARCHAR (255)    NOT NULL,
    [user_id]     UNIQUEIDENTIFIER NOT NULL,
    [create_date] DATETIME         NOT NULL,
    CONSTRAINT [PK_reports1nf_btiphoto] PRIMARY KEY CLUSTERED ([id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [indx_reports1nf_btiphoto_bal_id]
    ON [dbo].[reports1nf_btiphoto]([bal_id] ASC);

