select 
	uridlico_id,
	(select list(c, '\x0D\x0A') from LATERAL(select isnull(max(accnam),'') + ' (' + isnull(accmfo,'') + '): ' + list(accrah) as c from uridlico_bank where uridlico_id = t.uridlico_id and (accrah like '35%' or accrah like '26%') group by accmfo) a) as bankrahlist
from uridlico t where uridlico_id in (848439, 149782, 443935)
;
