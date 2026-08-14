
create   function [dbo].[CabinetOrendarStage] ( @free_square_id integer, @userId varchar(100) )
returns varchar(100)
as 
BEGIN 
	declare @rez varchar(100)

	select 
		@rez = 
		case 
			when (@userId = '-') or (isnull(@userId,'') <> '' and A.winnerUserid = cast(@userId as uniqueidentifier)) then
				case 
				when freecycle_step_dict_id in (200880,200900)
					then 'podpis_all'
				when freecycle_step_dict_id = 200870
					then 'podpis_balansoderzhatel_orendar'
				when freecycle_step_dict_id = 200840
					then 'podpis_balansoderzhatel'
				when freecycle_step_dict_id = 200860
					then 'vidmova'
				when zayavka_date is not null
					then 'zayavka_podana'
			end
		end 
	from 
	(
		select 
		R.*,
		B.UserId as winnerUserid,
		B.zayavka_date
		from [reports1nf_balans_free_square] R
		left join [auction_uchasnik_name] B on B.id = R.winner_id

	) A
	where id = @free_square_id

	return isnull(@rez,'');
END
