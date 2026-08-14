CREATE TABLE [dbo].[freecycle_orendar] (
    [freecycle_orendar_id] INT           IDENTITY (1, 1) NOT NULL,
    [freecycle_id]         INT           NOT NULL,
    [npp]                  INT           NOT NULL,
    [org_orendar_id]       INT           NOT NULL,
    [modify_date]          DATETIME      NULL,
    [modified_by]          VARCHAR (128) NULL,
    [is_deleted]           BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_freecycle_orendar] PRIMARY KEY CLUSTERED ([freecycle_orendar_id] ASC),
    FOREIGN KEY ([freecycle_id]) REFERENCES [dbo].[freecycle] ([freecycle_id]),
    FOREIGN KEY ([org_orendar_id]) REFERENCES [dbo].[organizations] ([id])
);

