USE [GUKV]
GO

ALTER TABLE [dbo].[organizations] DROP CONSTRAINT [fk_organizations_old_occupation]
GO

UPDATE [dbo].[organizations] SET [old_occupation_id] =1 WHERE ID=1
GO
UPDATE [dbo].[organizations] SET [old_occupation_id] =2 WHERE ID!=1
GO

USE [GUKV]
GO

ALTER TABLE [dbo].[organizations]  WITH NOCHECK ADD  CONSTRAINT [fk_organizations_old_occupation] 
FOREIGN KEY([old_occupation_id])
REFERENCES [dbo].DictSummaryReportSphere ([id])
GO

ALTER TABLE [dbo].[organizations] CHECK CONSTRAINT [fk_organizations_old_occupation]
GO


