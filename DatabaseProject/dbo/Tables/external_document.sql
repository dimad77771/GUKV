CREATE TABLE [dbo].[external_document] (
    [id]              INT           IDENTITY (1, 1) NOT NULL,
    [name]            VARCHAR (512) NOT NULL,
    [unique_filename] VARCHAR (512) NOT NULL,
    CONSTRAINT [PK_external_document] PRIMARY KEY CLUSTERED ([id] ASC)
);

