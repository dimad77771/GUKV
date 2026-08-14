
CREATE PROCEDURE dbo.fnCreateUniqueBuilding
(	
	@BUILDING_ID INTEGER,
	@REPORT_ID INTEGER,
	@BUILDING_UNIQUE_ID INTEGER OUTPUT
)
AS
	DECLARE @TmpTable TABLE (unique_id INTEGER)
	
	INSERT INTO reports1nf_buildings OUTPUT INSERTED.unique_id INTO @TmpTable
		SELECT b.*, @REPORT_ID AS 'report_id' FROM buildings b WHERE b.id = @BUILDING_ID

	SET @BUILDING_UNIQUE_ID = (select unique_id from @TmpTable)