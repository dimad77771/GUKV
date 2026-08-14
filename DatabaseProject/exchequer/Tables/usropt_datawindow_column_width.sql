CREATE TABLE [exchequer].[usropt_datawindow_column_width] (
    [usr]     VARCHAR (100)  NOT NULL,
    [dwname]  VARCHAR (1000) NOT NULL,
    [context] VARCHAR (1000) NOT NULL,
    [colcod]  VARCHAR (1000) NOT NULL,
    [width]   INT            NOT NULL,
    [posX]    INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([usr] ASC, [dwname] ASC, [context] ASC, [colcod] ASC)
);

