CREATE FUNCTION [dbo].[efn_split_string]
(@input_string NVARCHAR (MAX) NULL, @delimiter_string NVARCHAR (100) NULL)
RETURNS 
     TABLE (
        [value] NVARCHAR (4000) NULL)
AS
 EXTERNAL NAME [SqlStringUtils].[StringSplitter].[Split]

