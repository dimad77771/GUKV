
create   function [dbo].[IsCabinetOrendodavecz] ( @userId varchar(100) )
returns smallint
as 
BEGIN 
	declare @rez smallint

	set @rez = case when @userId <> '' and exists (select 1 from [aspnet_Membership] A where A.IsCabinetOrendodavecz = 1 and A.UserId = cast(@userId as uniqueidentifier)) then 1 else 0 end

	return @rez;
END
