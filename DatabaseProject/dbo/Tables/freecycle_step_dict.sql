CREATE TABLE [dbo].[freecycle_step_dict] (
    [step_id]    INT            NOT NULL,
    [step_cod]   VARCHAR (100)  NOT NULL,
    [step_name]  VARCHAR (1000) NOT NULL,
    [step_ord]   INT            NOT NULL,
    [istitle]    BIT            NOT NULL,
    [header_dat] VARCHAR (1000) DEFAULT ('') NOT NULL,
    [header_doc] VARCHAR (1000) DEFAULT ('') NOT NULL,
    [header_c1]  VARCHAR (1000) DEFAULT ('') NOT NULL,
    [header_c2]  VARCHAR (1000) DEFAULT ('') NOT NULL,
    [header_c3]  VARCHAR (1000) DEFAULT ('') NOT NULL,
    [header_c4]  VARCHAR (1000) DEFAULT ('') NOT NULL,
    [header_c5]  VARCHAR (1000) DEFAULT ('') NOT NULL,
    [is_deleted] BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([step_id] ASC)
);

