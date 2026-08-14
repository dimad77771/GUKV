CREATE TABLE [dbo].[free_proc_step_dict] (
    [step_id]     INT            NOT NULL,
    [step_ord]    INT            NOT NULL,
    [step_name]   VARCHAR (1000) NOT NULL,
    [lookup_name] VARCHAR (1000) NULL,
    [public_name] VARCHAR (1000) NULL,
    [level]       INT            NULL,
    [color]       VARCHAR (100)  NULL,
    [site_text]   VARCHAR (1000) NULL,
    [allow_zayav] SMALLINT       NULL,
    PRIMARY KEY CLUSTERED ([step_id] ASC)
);

