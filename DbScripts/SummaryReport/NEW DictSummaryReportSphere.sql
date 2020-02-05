/* Чтобы предотвратить возможность потери данных, необходимо внимательно просмотреть этот скрипт, прежде чем запускать его вне контекста конструктора баз данных.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO

CREATE TABLE dbo.DictSummaryReportSphere
	(
	id int NOT NULL IDENTITY (1, 1),
	name varchar(150) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.DictSummaryReportSphere ADD CONSTRAINT
	PK_DictSummaryReportSphere PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.DictSummaryReportSphere SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
