CREATE TABLE [yurdep].[l_reg] (
    [reg]  VARCHAR (5)   NOT NULL,
    [nam]  VARCHAR (100) NULL,
    [nam2] VARCHAR (100) NULL,
    [zkpo] VARCHAR (50)  NULL,
    [adr]  VARCHAR (255) NULL,
    CONSTRAINT [l_reg_x] PRIMARY KEY CLUSTERED ([reg] ASC)
);

