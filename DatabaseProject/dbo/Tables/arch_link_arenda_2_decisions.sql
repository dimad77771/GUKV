CREATE TABLE [dbo].[arch_link_arenda_2_decisions] (
    [arenda_id]                INT             NULL,
    [rishen_id]                INT             NULL,
    [ord]                      INT             NULL,
    [modified_by]              VARCHAR (128)   NULL,
    [modify_date]              DATE            NULL,
    [doc_num]                  VARCHAR (18)    NULL,
    [doc_date]                 DATE            NULL,
    [doc_dodatok]              VARCHAR (2)     NULL,
    [doc_punkt]                INT             NULL,
    [purpose_str]              VARCHAR (255)   NULL,
    [rent_square]              NUMERIC (15, 2) NULL,
    [decision_id]              INT             NULL,
    [doc_raspor_id]            INT             NULL,
    [pidstava]                 VARCHAR (255)   NULL,
    [archive_id]               INT             IDENTITY (1, 1) NOT NULL,
    [archive_arenda_link_code] INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([archive_id] ASC)
);

