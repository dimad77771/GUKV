CREATE   VIEW auction_uchasnik_name AS
select A.id, A.free_square_id, A.UserId, A.zayavka_date, A.is_arhiv, case when C.NameF <> '' then rtrim(ltrim(concat(C.NameF,' ',C.NameI,' ',C.NameO))) else B.UserName end as uchasnik_name 
from [auction_uchasnik] A 
join aspnet_Users B on B.UserId = A.UserId 
left join aspnet_Membership C on C.UserId = A.UserId
