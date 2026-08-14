CREATE TABLE [dbo].[dict_free_square_stage] (
    [id]      INT            NOT NULL,
    [cod]     VARCHAR (100)  NOT NULL,
    [name]    VARCHAR (1000) NOT NULL,
    [ord]     INT            NOT NULL,
    [istitle] BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

