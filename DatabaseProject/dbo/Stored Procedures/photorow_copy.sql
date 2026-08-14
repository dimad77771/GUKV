CREATE PROCEDURE [dbo].[photorow_copy]
    @photofilenameSrc [varchar](8000), @photofilenameDst [varchar](8000) 
AS
BEGIN
	delete from photorow where photofilename = @photofilenameDst

	insert into photorow(photofilename, photofilebytes) select @photofilenameDst, photofilebytes from photorow where photofilename = @photofilenameSrc
END
