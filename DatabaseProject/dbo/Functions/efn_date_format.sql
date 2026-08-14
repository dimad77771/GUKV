CREATE FUNCTION [dbo].[efn_date_format]
(@input_datetime DATETIME NULL, @format_string NVARCHAR (50) NULL, @culture_string NVARCHAR (5) NULL)
RETURNS NVARCHAR (50)
AS
 EXTERNAL NAME [SqlStringUtils].[StringConverter].[DateFormat]

