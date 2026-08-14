CREATE TABLE [dbo].[reports1nf_comments] (
    [id]                INT           IDENTITY (1, 1) NOT NULL,
    [report_id]         INT           NOT NULL,
    [organization_id]   INT           NULL,
    [balans_id]         INT           NULL,
    [balans_deleted_id] INT           NULL,
    [arenda_id]         INT           NULL,
    [arenda_rented_id]  INT           NULL,
    [control_id]        VARCHAR (64)  NULL,
    [control_title]     VARCHAR (256) NULL,
    [comment]           VARCHAR (MAX) NULL,
    [is_wrong_data]     INT           DEFAULT ((0)) NULL,
    [comment_date]      DATETIME      NOT NULL,
    [comment_user]      VARCHAR (64)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_reports1nf_comments_reports1nf] FOREIGN KEY ([report_id]) REFERENCES [dbo].[reports1nf] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);

