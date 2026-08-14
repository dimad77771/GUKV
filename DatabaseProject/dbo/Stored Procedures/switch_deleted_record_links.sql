
-- =============================================
-- Author:		<Ershov E.>
-- Create date: <22.05.2017>
-- Modify date:
--
-- Description:	<Удаляет из заданной таблицы записи по id-списку с переключением их ссылок на один заданный id.>
--
-- Example:
/*
	select *	-- id = 9826 не удалять
	from dbo.organizations
	where zkpo_code = '37371727'

	begin tran
		select * from dbo.organizations where id in (9826, 308545, 99405569, 99405570, 99405795)
	exec delete_records_from_table 'dbo.organizations', '308545, 99405569, 99405570, 99405795', 9826
		select * from dbo.organizations where id in (9826, 308545, 99405569, 99405570, 99405795)
	rollback tran
*/	
-- =============================================
CREATE PROCEDURE [dbo].[switch_deleted_record_links]
	 @main_table_name nvarchar(128) = null
	,@false_id_list nvarchar(max) = null
	,@true_id int = null
AS
BEGIN
	SET NOCOUNT ON
	
	-- Список таблиц и полей, связанных внешними ссылками с заданной таблицей
	declare @fk table (constraint_name varchar(100), table_from varchar(100), column_from varchar(100), table_to varchar(100), column_to varchar(100), delete_rule varchar(100))
	insert into @fk
		select rc.CONSTRAINT_NAME as constraint_name
			,kcu1.TABLE_SCHEMA + '.' + kcu1.TABLE_NAME as table_from
			,substring((select ', ' + COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where (TABLE_SCHEMA + '.' + TABLE_NAME = kcu1.TABLE_SCHEMA + '.' + kcu1.TABLE_NAME) and (CONSTRAINT_NAME = rc.CONSTRAINT_NAME) order by ORDINAL_POSITION for xml path('')), 3, 255) as column_from
			,kcu2.TABLE_SCHEMA + '.' + kcu2.TABLE_NAME as table_to
			,substring((select ', ' + COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where (TABLE_SCHEMA + '.' + TABLE_NAME = kcu2.TABLE_SCHEMA + '.' + kcu2.TABLE_NAME) and (CONSTRAINT_NAME = rc.UNIQUE_CONSTRAINT_NAME) order by ORDINAL_POSITION for xml path('')), 3, 255) as column_to
			,min(rc.DELETE_RULE) as delete_rule
		from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
			join INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu1 on kcu1.CONSTRAINT_SCHEMA + '.' + kcu1.CONSTRAINT_NAME = rc.CONSTRAINT_SCHEMA + '.' + rc.CONSTRAINT_NAME
			join INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu2 on kcu2.CONSTRAINT_SCHEMA + '.' + kcu2.CONSTRAINT_NAME = rc.UNIQUE_CONSTRAINT_SCHEMA + '.' + rc.UNIQUE_CONSTRAINT_NAME
		where kcu1.ORDINAL_POSITION = kcu2.ORDINAL_POSITION and kcu2.TABLE_SCHEMA + '.' + kcu2.TABLE_NAME = @main_table_name
		group by rc.CONSTRAINT_NAME, rc.UNIQUE_CONSTRAINT_NAME, kcu1.TABLE_SCHEMA + '.' + kcu1.TABLE_NAME, kcu2.TABLE_SCHEMA + '.' + kcu2.TABLE_NAME
	--select * from @fk

	-- Заменить ссылки в связанных таблицах
	declare @sql_str nvarchar(max)
	declare @table_from nvarchar(128), @column_from nvarchar(128)
	declare crs cursor for select table_from, column_from from @fk
	open crs
	fetch next from crs into @table_from, @column_from
	while @@fetch_status = 0
	begin
		set @sql_str = 'update ' + @table_from + ' set ' + @column_from + ' = ' + cast(@true_id as nvarchar(10)) + ' where ' + @column_from + ' in (' + @false_id_list + ') and ' + @column_from + ' != ' + cast(@true_id as nvarchar(10))
		print @sql_str
		exec (@sql_str)
	
		fetch next from crs into @table_from, @column_from
	end
	close crs
	deallocate crs
	
	-- Удалить из основной таблицы записи, на которые уже нет ссылок
	set @sql_str = 'delete from ' + @main_table_name + ' where id in (' + @false_id_list + ') and id != ' + cast(@true_id as nvarchar(10))
	print char(13) + @sql_str
	exec (@sql_str)
END

