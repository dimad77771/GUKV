CREATE TABLE [dbo].[dict_rental_rate] (
    [id]              INT            NOT NULL,
    [full_name]       VARCHAR (2000) NULL,
    [short_name]      VARCHAR (100)  NULL,
    [rental_rate]     NUMERIC (5, 2) NULL,
    [new_rental_rate] NUMERIC (5, 2) NULL,
    CONSTRAINT [PK__dict_ren__3213E83F23F43791] PRIMARY KEY CLUSTERED ([id] ASC)
);

