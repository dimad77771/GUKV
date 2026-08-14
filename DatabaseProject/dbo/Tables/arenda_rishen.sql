CREATE TABLE [dbo].[arenda_rishen] (
    [id]             INT             IDENTITY (1, 1) NOT NULL,
    [arenda_id]      INT             NULL,
    [application_id] INT             NULL,
    [ord]            INT             NULL,
    [modified_by]    VARCHAR (24)    NULL,
    [modify_date]    DATE            NULL,
    [doc_num]        VARCHAR (18)    NULL,
    [doc_date]       DATE            NULL,
    [doc_dodatok]    VARCHAR (2)     NULL,
    [doc_punkt]      INT             NULL,
    [purpose_str]    VARCHAR (255)   NULL,
    [rent_square]    NUMERIC (15, 2) NULL,
    [decision_id]    INT             NULL,
    [doc_raspor_id]  INT             NULL,
    [pidstava]       VARCHAR (255)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

