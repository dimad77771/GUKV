CREATE TABLE [dbo].[reports1nf_photos] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [bal_id]      INT              NOT NULL,
    [file_name]   VARCHAR (255)    NOT NULL,
    [file_ext]    VARCHAR (255)    NOT NULL,
    [user_id]     UNIQUEIDENTIFIER NOT NULL,
    [create_date] DATETIME         NOT NULL,
    CONSTRAINT [PK_reports1nf_photos] PRIMARY KEY CLUSTERED ([id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [indx_reports1nf_photos_bal_id]
    ON [dbo].[reports1nf_photos]([bal_id] ASC);

