CREATE AGGREGATE [dbo].[efn_concat_string](@input_string NVARCHAR (MAX) NULL, @delimiter_string NVARCHAR (100) NULL, @order_by_string NVARCHAR (256) NULL, @direction INT NULL)
    RETURNS NVARCHAR (MAX)
    EXTERNAL NAME [SqlStringUtils].[StringConcatenator];

