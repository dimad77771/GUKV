CREATE TABLE [dbo].[reports1nf] (
    [id]                       INT            IDENTITY (1, 1) NOT NULL,
    [organization_id]          INT            NOT NULL,
    [is_reviewed]              INT            DEFAULT ((0)) NULL,
    [create_date]              DATETIME       NOT NULL,
    [stan_recieve_id]          INT            DEFAULT ((1)) NOT NULL,
    [stan_recieve_date]        DATETIME       NULL,
    [stan_recieve_description] VARCHAR (8000) NULL,
    [inventar_recieve_date]    DATETIME       NULL,
    [rep_modified_by]          VARCHAR (128)  NULL,
    [rep_modify_date]          DATE           NULL,
    [orandodavec_user_id]      INT            NULL,
    [zvit_last_created]        DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([orandodavec_user_id]) REFERENCES [dbo].[dict_orandodavec_user] ([id]),
    CONSTRAINT [FK__reports1nf__dict_stan_recieve] FOREIGN KEY ([stan_recieve_id]) REFERENCES [dbo].[dict_stan_recieve] ([stan_recieve_id]),
    UNIQUE NONCLUSTERED ([organization_id] ASC)
);

