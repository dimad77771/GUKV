CREATE TABLE [yurdep].[l_styp] (
    [cod]       VARCHAR (30)  NOT NULL,
    [nam]       VARCHAR (100) NULL,
    [grp]       VARCHAR (100) NULL,
    [ord]       INT           NULL,
    [docnam]    VARCHAR (255) NULL,
    [flag_lsud] SMALLINT      NULL,
    CONSTRAINT [l_styp_x] PRIMARY KEY CLUSTERED ([cod] ASC)
);

