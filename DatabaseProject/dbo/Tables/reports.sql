CREATE TABLE [dbo].[reports] (
    [id]              INT             IDENTITY (1, 1) NOT NULL,
    [number]          NVARCHAR (20)   NOT NULL,
    [name]            NVARCHAR (250)  NOT NULL,
    [sql_query]       NVARCHAR (MAX)  NOT NULL,
    [template_name]   NVARCHAR (250)  NOT NULL,
    [template]        VARBINARY (MAX) NOT NULL,
    [input_params]    NVARCHAR (MAX)  NULL,
    [category_id]     INT             NULL,
    [display_script]  NVARCHAR (MAX)  NULL,
    [data_class_name] NVARCHAR (255)  NULL,
    [Date_changes]    DATETIME        DEFAULT (getdate()) NOT NULL,
    [UserName]        NVARCHAR (256)  DEFAULT (suser_sname()) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    UNIQUE NONCLUSTERED ([number] ASC)
);

