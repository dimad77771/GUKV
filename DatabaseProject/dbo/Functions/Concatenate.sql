CREATE AGGREGATE [dbo].[Concatenate](@Value NVARCHAR (MAX) NULL)
    RETURNS NVARCHAR (MAX)
    EXTERNAL NAME [ITG.EIS.SqlServerExtensions].[Concatenate];

