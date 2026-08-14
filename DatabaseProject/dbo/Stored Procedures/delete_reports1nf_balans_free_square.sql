CREATE PROCEDURE [dbo].[delete_reports1nf_balans_free_square] 
	@id int
AS
BEGIN
	if exists (select * from freecycle_orendar A where A.freecycle_id in (select Q.freecycle_id from freecycle Q WHERE Q.free_square_id = @id) and A.is_deleted = 0)
	begin
		RAISERROR ('Картка процесу передачі в оренду вільного приміщення в процеси роботи. Вилучення неможливе', 11, 11);
		RETURN;
	end;

	if exists (select * from reports1nf_balans_free_square A where A.id = @id and substring(isnull(komis_protocol,''),1,1) not in ('','0'))
	begin
		RAISERROR ('Об''єкт погоджено орендодавцем! Усі зміни ТІЛЬКИ з його дозволу за тел: 202-61-76, 202-61-77, 202-61-96 !', 11, 11);
		RETURN;
	end;

	delete from freecycle_step where freecycle_id in (select Q.freecycle_id from freecycle Q WHERE Q.free_square_id = @id);
	delete from freecycle_orendar where freecycle_id in (select Q.freecycle_id from freecycle Q WHERE Q.free_square_id = @id);
	DELETE FROM [freecycle] WHERE [free_square_id] = @id;
	DELETE FROM [reports1nf_balans_free_square] WHERE [id] = @id;
END
