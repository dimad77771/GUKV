CREATE TABLE [dbo].[link_arenda_2_decisions] (
    [id]            INT             IDENTITY (1, 1) NOT NULL,
    [arenda_id]     INT             NULL,
    [rishen_id]     INT             NULL,
    [ord]           INT             NULL,
    [modified_by]   VARCHAR (128)   NULL,
    [modify_date]   DATE            NULL,
    [doc_num]       VARCHAR (18)    NULL,
    [doc_date]      DATE            NULL,
    [doc_dodatok]   VARCHAR (2)     NULL,
    [doc_punkt]     INT             NULL,
    [purpose_str]   VARCHAR (255)   NULL,
    [rent_square]   NUMERIC (15, 2) NULL,
    [decision_id]   INT             NULL,
    [doc_raspor_id] INT             NULL,
    [pidstava]      VARCHAR (255)   NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_arenda2decision_appl] FOREIGN KEY ([rishen_id]) REFERENCES [dbo].[arenda_decisions] ([id]),
    CONSTRAINT [fk_arenda2decision_arenda] FOREIGN KEY ([arenda_id]) REFERENCES [dbo].[arenda] ([id]),
    CONSTRAINT [fk_arenda2decision_decision] FOREIGN KEY ([decision_id]) REFERENCES [dbo].[dict_rent_decisions] ([id]),
    CONSTRAINT [fk_arenda2decision_raspor] FOREIGN KEY ([doc_raspor_id]) REFERENCES [dbo].[documents] ([id])
);


GO
CREATE NONCLUSTERED INDEX [indx_link_arenda_2_decisions_arenda_id]
    ON [dbo].[link_arenda_2_decisions]([arenda_id] ASC);

