-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================

CREATE function [dbo].[cm2s] (@s nvarchar(20) )
returns  nvarchar(2)
as 
BEGIN 
	declare @res nvarchar(2)
    SET @res=(
		SELECT 
			Case LOWER(left(@s,3))
				WHEN 'січ' then '01'
				WHEN 'лют' then '02'
				WHEN 'бер' then '03'
				WHEN 'кві' then '04'
				WHEN 'тра' then '05'
				WHEN 'чер' then '06'
				WHEN 'лип' then '07'
				WHEN 'сер' then '08'
				WHEN 'вер' then '09'
				WHEN 'жов' then '10'
				WHEN 'лис' then '11'
				WHEN 'гру' then '12'
			End
		)
	 return  @res
END

