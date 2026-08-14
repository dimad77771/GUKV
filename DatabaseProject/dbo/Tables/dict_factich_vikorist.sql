CREATE TABLE [dbo].[dict_factich_vikorist] (
    [id]          INT            NOT NULL,
    [full_name]   VARCHAR (2000) NULL,
    [short_name]  VARCHAR (100)  NULL,
    [rental_rate] NUMERIC (5, 1) NULL,
    CONSTRAINT [PK__dict_fac__3213E83F6AEE8C84] PRIMARY KEY CLUSTERED ([id] ASC)
);

