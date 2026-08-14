
CREATE PROCEDURE dbo.fnCreateRentAgreement
(	
	@ARENDA_ID INTEGER OUTPUT
)
AS
	SET @ARENDA_ID = (select MAX(id) + 1 from arenda)
	
	INSERT INTO arenda (id, is_deleted) VALUES (@ARENDA_ID, 0)