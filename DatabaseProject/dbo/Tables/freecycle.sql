CREATE TABLE [dbo].[freecycle] (
    [freecycle_id]   INT           IDENTITY (1, 1) NOT NULL,
    [free_square_id] INT           NOT NULL,
    [cycle_num]      INT           NOT NULL,
    [start_date]     DATETIME      NULL,
    [finish_date]    VARCHAR (128) NULL,
    [finish_code]    DATETIME      NULL,
    [modify_date]    DATETIME      NULL,
    [modified_by]    VARCHAR (128) NULL,
    [is_deleted]     BIT           DEFAULT ((0)) NOT NULL,
    [reg_number]     AS            ((CONVERT([varchar](100),[free_square_id],0)+'/')+CONVERT([varchar](100),[cycle_num],0)) PERSISTED,
    CONSTRAINT [PK_freecycle] PRIMARY KEY CLUSTERED ([freecycle_id] ASC),
    FOREIGN KEY ([free_square_id]) REFERENCES [dbo].[reports1nf_balans_free_square] ([id]),
    CONSTRAINT [UK_freecycle] UNIQUE NONCLUSTERED ([free_square_id] ASC, [cycle_num] ASC)
);

