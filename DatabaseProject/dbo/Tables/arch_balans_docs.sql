CREATE TABLE [dbo].[arch_balans_docs] (
    [balans_id]                INT NULL,
    [building_id]              INT NULL,
    [link_kind]                INT NULL,
    [building_docs_id]         INT NULL,
    [sort_field]               INT NULL,
    [archive_id]               INT IDENTITY (1, 1) NOT NULL,
    [archive_balans_link_code] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([archive_id] ASC)
);

