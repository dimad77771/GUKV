
CREATE PROCEDURE dbo.fnCreateRentedObject
(	
	@ARENDA_RENTED_ID INTEGER OUTPUT
)
AS
	DECLARE @TmpTable TABLE (arenda_rented_id INTEGER)
	
	INSERT INTO arenda_rented (is_cmk) OUTPUT INSERTED.id INTO @TmpTable VALUES (0)

	SET @ARENDA_RENTED_ID = (select arenda_rented_id from @TmpTable)