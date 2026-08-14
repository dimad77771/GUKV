CREATE TABLE [dbo].[reports1nf_arendaphotos] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [arenda_id]   INT              NOT NULL,
    [file_name]   VARCHAR (255)    NOT NULL,
    [file_ext]    VARCHAR (255)    NOT NULL,
    [user_id]     UNIQUEIDENTIFIER NOT NULL,
    [create_date] DATETIME         NOT NULL,
    CONSTRAINT [PK_reports1nf_arendaphotos] PRIMARY KEY CLUSTERED ([id] ASC)
);

