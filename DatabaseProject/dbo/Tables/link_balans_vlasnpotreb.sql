CREATE TABLE [dbo].[link_balans_vlasnpotreb] (
    [id]             INT             IDENTITY (1, 1) NOT NULL,
    [balans_id]      INT             NULL,
    [struct_edinich] VARCHAR (400)   NULL,
    [kolvo_osob]     NUMERIC (15, 2) NULL,
    [rent_square]    NUMERIC (15, 2) NULL,
    [ord]            INT             NULL,
    [modified_by]    VARCHAR (128)   NULL,
    [modify_date]    DATE            NULL,
    [doc_num]        VARCHAR (18)    NULL,
    [doc_date]       DATE            NULL,
    [doc_dodatok]    VARCHAR (2)     NULL,
    [doc_punkt]      INT             NULL,
    [purpose_str]    VARCHAR (255)   NULL,
    [decision_id]    INT             NULL,
    [doc_raspor_id]  INT             NULL,
    [pidstava]       VARCHAR (255)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_link_balans_vlasnpotreb_balans] FOREIGN KEY ([balans_id]) REFERENCES [dbo].[balans] ([id])
);

