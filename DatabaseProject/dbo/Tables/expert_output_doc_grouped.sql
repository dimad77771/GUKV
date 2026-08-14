CREATE TABLE [dbo].[expert_output_doc_grouped] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [expert_note_id] INT           NULL,
    [doc_dates]      VARCHAR (MAX) NULL,
    [doc_numbers]    VARCHAR (MAX) NULL,
    [rezenz_kinds]   VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

