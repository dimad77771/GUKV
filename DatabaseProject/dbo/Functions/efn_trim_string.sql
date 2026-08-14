CREATE FUNCTION [dbo].[efn_trim_string]
(@input_string NVARCHAR (MAX) NULL, @left_trim_charset NVARCHAR (100) NULL, @right_trim_charset NVARCHAR (100) NULL)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SqlStringUtils].[StringTrimmer].[Trim]

