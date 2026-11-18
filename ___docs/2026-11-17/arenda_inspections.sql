USE [GUKV20261002];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;
IF OBJECT_ID('dbo.reports1nf_arenda_inspections', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.reports1nf_arenda_inspections (
        id int IDENTITY PRIMARY KEY CLUSTERED,
        report_id int NOT NULL,
        arenda_id int NOT NULL,
        order_num varchar(500) NULL,
        order_date date NULL,
        lessor_rep varchar(500) NULL,
        holder_rep varchar(500) NULL,
        tenant_rep varchar(500) NULL,
        holder_name varchar(500) NULL,
        holder_street varchar(500) NULL,
        holder_house varchar(500) NULL,
        area decimal(18,2) NULL,
        agreement_num varchar(500) NULL,
        agreement_date varchar(500) NULL,
        property_use varchar(max) NULL,
        condition varchar(500) NULL,
        sublease varchar(500) NULL,
        area_matches varchar(500) NULL,
        rent_payment varchar(500) NULL,
        insurance varchar(500) NULL,
        accessibility varchar(500) NULL,
        other_info varchar(max) NULL,
        documents varchar(max) NULL,
        violations varchar(500) NULL,
        warning varchar(max) NULL,
        recorded_by varchar(500) NULL,
        receipt varchar(max) NULL,
        sent_date date NULL,
        sent_address varchar(500) NULL,
        sender varchar(500) NULL,
        act_num varchar(500) NULL,
        act_date date NULL,
        CONSTRAINT FK_arenda_inspections_agreement FOREIGN KEY (report_id, arenda_id)
            REFERENCES dbo.reports1nf_arenda (report_id, id) ON UPDATE CASCADE ON DELETE CASCADE
    );
    CREATE INDEX IX_arenda_inspections_agreement ON dbo.reports1nf_arenda_inspections(report_id, arenda_id);
    CREATE INDEX IX_arenda_inspections_act ON dbo.reports1nf_arenda_inspections(act_date, act_num);
END;
COMMIT;
GO
