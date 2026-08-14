CREATE FUNCTION [dbo].[efn_html2text]
(@input_html_string NVARCHAR (MAX) NULL)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SqlStringUtils].[StringConverter].[Html2Text]

