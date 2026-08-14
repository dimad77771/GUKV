CREATE TABLE [yurdep].[delo] (
    [delo_id] INT           NOT NULL,
    [ddat]    DATETIME      NULL,
    [dnum]    VARCHAR (50)  NULL,
    [ousr]    VARCHAR (100) NULL,
    [opis]    TEXT          NULL,
    [klas]    VARCHAR (50)  NULL,
    [prokur]  VARCHAR (255) NULL,
    [uch_1]   TEXT          NULL,
    [uch_2]   TEXT          NULL,
    [uch_3]   TEXT          NULL,
    [reg]     VARCHAR (5)   NULL,
    [prim]    TEXT          NULL,
    [dcat]    VARCHAR (20)  NULL,
    [spor]    TEXT          NULL,
    [arhiv]   TEXT          NULL,
    CONSTRAINT [delo_x] PRIMARY KEY CLUSTERED ([delo_id] ASC)
);

