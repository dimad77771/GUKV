CREATE TABLE [dbo].[expert_input_doc_grouped] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [expert_note_id] INT           NULL,
    [doc_dates]      VARCHAR (MAX) NULL,
    [doc_numbers]    VARCHAR (MAX) NULL,
    [control_dates]  VARCHAR (MAX) NULL,
    [korrespondents] VARCHAR (MAX) NULL,
    [rezenz_names]   VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

