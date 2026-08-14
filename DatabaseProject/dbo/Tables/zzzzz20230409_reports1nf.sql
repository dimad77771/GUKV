CREATE TABLE [dbo].[zzzzz20230409_reports1nf] (
    [id]                       INT            IDENTITY (1, 1) NOT NULL,
    [organization_id]          INT            NOT NULL,
    [is_reviewed]              INT            NULL,
    [create_date]              DATETIME       NOT NULL,
    [stan_recieve_id]          INT            NOT NULL,
    [stan_recieve_date]        DATETIME       NULL,
    [stan_recieve_description] VARCHAR (1000) NULL,
    [inventar_recieve_date]    DATETIME       NULL
);

