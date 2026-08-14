CREATE TABLE [yurdep].[stp_uch] (
    [stp_uch_id] INT           NOT NULL,
    [delo_id]    INT           NULL,
    [stp_id]     INT           NULL,
    [utyp]       SMALLINT      NULL,
    [unam]       VARCHAR (255) NULL,
    [uzkpo]      VARCHAR (30)  NULL,
    [uadr]       VARCHAR (255) NULL,
    [trvim]      SMALLINT      NULL,
    [trtyp]      SMALLINT      NULL,
    [isfdmu]     SMALLINT      NULL,
    CONSTRAINT [stp_uch_x] PRIMARY KEY CLUSTERED ([stp_uch_id] ASC)
);

