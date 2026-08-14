CREATE TABLE [dbo].[zzzz20220109_ref_tables] (
    [constraint_name]                [sysname]      NOT NULL,
    [referencing_table_name]         [sysname]      NULL,
    [referencing_column_name]        NVARCHAR (128) NULL,
    [referenced_table_name]          [sysname]      NULL,
    [referenced_column_name]         NVARCHAR (128) NULL,
    [delete_referential_action_desc] VARCHAR (11)   NULL,
    [update_referential_action_desc] VARCHAR (11)   NULL
);

