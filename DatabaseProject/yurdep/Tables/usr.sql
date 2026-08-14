CREATE TABLE [yurdep].[usr] (
    [usr]    VARCHAR (100) NOT NULL,
    [pwd]    VARCHAR (255) NULL,
    [admin]  SMALLINT      NULL,
    [isread] SMALLINT      NULL,
    CONSTRAINT [usr_x] PRIMARY KEY CLUSTERED ([usr] ASC)
);

