CREATE TABLE [dbo].[user_reports] (
    [id]            INT              IDENTITY (1, 1) NOT NULL,
    [usr_id]        UNIQUEIDENTIFIER NULL,
    [folder_id]     INT              NULL,
    [report_name]   VARCHAR (1024)   NULL,
    [category]      INT              NOT NULL,
    [grid_layout]   VARCHAR (MAX)    NULL,
    [pre_filter]    VARCHAR (MAX)    NULL,
    [fixed_columns] VARCHAR (1024)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

