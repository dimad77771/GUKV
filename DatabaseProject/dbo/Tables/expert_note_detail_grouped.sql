CREATE TABLE [dbo].[expert_note_detail_grouped] (
    [id]              INT           IDENTITY (1, 1) NOT NULL,
    [expert_note_id]  INT           NULL,
    [obj_squares]     VARCHAR (MAX) NULL,
    [costs_1_usd]     VARCHAR (MAX) NULL,
    [valuation_dates] VARCHAR (MAX) NULL,
    [floors]          VARCHAR (MAX) NULL,
    [purposes]        VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

