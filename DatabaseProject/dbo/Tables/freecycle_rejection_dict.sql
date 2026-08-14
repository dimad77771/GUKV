CREATE TABLE [dbo].[freecycle_rejection_dict] (
    [rejection_id]   INT            NOT NULL,
    [rejection_cod]  VARCHAR (100)  NOT NULL,
    [rejection_name] VARCHAR (1000) NOT NULL,
    [step_ord]       INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([rejection_cod] ASC)
);

