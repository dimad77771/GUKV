CREATE TABLE [dbo].[arenda_notes] (
    [id]                  INT             IDENTITY (1, 1) NOT NULL,
    [arenda_id]           INT             NULL,
    [purpose_group_id]    INT             NULL,
    [purpose_id]          INT             NULL,
    [purpose_str]         VARCHAR (252)   NULL,
    [rent_square]         NUMERIC (9, 2)  NULL,
    [modify_date]         DATE            NULL,
    [modified_by]         VARCHAR (128)   NULL,
    [note]                VARCHAR (252)   NULL,
    [rent_rate]           NUMERIC (15, 3) NULL,
    [rent_rate_uah]       NUMERIC (15, 3) NULL,
    [cost_narah]          NUMERIC (15, 3) NULL,
    [cost_agreement]      NUMERIC (15, 3) NULL,
    [is_deleted]          INT             NULL,
    [del_date]            DATE            NULL,
    [cost_expert_total]   NUMERIC (15, 3) NULL,
    [date_expert]         DATE            NULL,
    [payment_type_id]     INT             NULL,
    [invent_no]           VARCHAR (128)   NULL,
    [note_status_id]      INT             NULL,
    [zapezh_deposit]      NUMERIC (15, 3) NULL,
    [ref_balans_id]       INT             NULL,
    [factich_vikorist_id] INT             NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_arenda_notes_arenda] FOREIGN KEY ([arenda_id]) REFERENCES [dbo].[arenda] ([id]),
    CONSTRAINT [fk_arenda_notes_payment_type] FOREIGN KEY ([payment_type_id]) REFERENCES [dbo].[dict_rental_rate] ([id]),
    CONSTRAINT [fk_arenda_notes_purpose] FOREIGN KEY ([purpose_id]) REFERENCES [dbo].[dict_balans_purpose] ([id]),
    CONSTRAINT [fk_arenda_notes_purpose_group] FOREIGN KEY ([purpose_group_id]) REFERENCES [dbo].[dict_balans_purpose_group] ([id]),
    CONSTRAINT [fk_arenda_notes_status] FOREIGN KEY ([note_status_id]) REFERENCES [dbo].[dict_arenda_note_status] ([id])
);


GO
CREATE NONCLUSTERED INDEX [indx__arenda_notes__ref_balans_id]
    ON [dbo].[arenda_notes]([ref_balans_id] ASC);


GO
CREATE NONCLUSTERED INDEX [indx_arenda_notes_arenda_id]
    ON [dbo].[arenda_notes]([arenda_id] ASC);

