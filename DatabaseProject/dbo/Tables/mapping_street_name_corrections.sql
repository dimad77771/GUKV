CREATE TABLE [dbo].[mapping_street_name_corrections] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [street_pattern] VARCHAR (128) NULL,
    [street_name]    VARCHAR (128) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

