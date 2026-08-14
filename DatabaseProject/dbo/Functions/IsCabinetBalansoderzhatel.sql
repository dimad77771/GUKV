
create   function [dbo].[IsCabinetBalansoderzhatel] ( @userId varchar(100), @zkpo varchar(100) )
returns smallint
as 
BEGIN 
	declare @rez smallint

	set @rez = case when @userId <> '' and @zkpo <> '' and exists (select 1 from [aspnet_Membership] A where A.CabinetBalansoderzhatelZkpo = @zkpo and A.UserId = cast(@userId as uniqueidentifier)) then 1 else 0 end

	return @rez;
END
