CREATE TABLE [exchequer].[lookup_2] (
    [pay_zkpo] VARCHAR (100)  NOT NULL,
    [pay_name] VARCHAR (8000) NULL,
    [bal_zkpo] VARCHAR (100)  NOT NULL,
    [bal_name] VARCHAR (8000) NULL,
    PRIMARY KEY CLUSTERED ([pay_zkpo] ASC)
);

