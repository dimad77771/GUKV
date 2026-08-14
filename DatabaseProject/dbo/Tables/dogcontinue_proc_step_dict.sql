CREATE TABLE [dbo].[dogcontinue_proc_step_dict] (
    [step_id]     INT            NOT NULL,
    [step_ord]    INT            NOT NULL,
    [step_name]   VARCHAR (1000) NOT NULL,
    [lookup_name] VARCHAR (1000) NULL,
    [public_name] VARCHAR (1000) NULL,
    [level]       INT            NULL,
    PRIMARY KEY CLUSTERED ([step_id] ASC)
);

