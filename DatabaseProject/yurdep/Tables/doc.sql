CREATE TABLE [yurdep].[doc] (
    [doc_id]  INT           NOT NULL,
    [delo_id] INT           NULL,
    [stp_id]  INT           NULL,
    [docdat]  DATETIME      NULL,
    [docnum]  VARCHAR (255) NULL,
    [docnam]  VARCHAR (255) NULL,
    CONSTRAINT [doc_x] PRIMARY KEY CLUSTERED ([doc_id] ASC)
);

