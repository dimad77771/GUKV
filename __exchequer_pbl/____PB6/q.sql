// DECLARE LOCAL TEMPORARY TABLE qfindparm(
//	typ varchar(100),
//	v1 long varchar,
//	v2 long varchar,
//	v3 long varchar,
//	cat_set long varchar,
//	zag_set long varchar,
//	obl long varchar,
//	datrog date,
//	yyrog integer,
//	isdebug integer,
// ) ON COMMIT PRESERVE ROWS;
//
// DECLARE LOCAL TEMPORARY TABLE qfindrez(
//	tab varchar(100) not null,
//	idser long varchar not null,
//	cat long varchar,
//	zag long varchar,
//	idser2 integer not null,
//	id integer default autoincrement primary key,
//	unique(tab, idser2)
// ) ON COMMIT PRESERVE ROWS;
//
// DECLARE LOCAL TEMPORARY TABLE qfindone(
//	htm long varchar,
//	txt long varchar,
//	fio long varchar,
//	datr long varchar,
//	adr long varchar,
//	reg long varchar,
//	idser integer,
// ) ON COMMIT PRESERVE ROWS;
//
// DECLARE LOCAL TEMPORARY TABLE tmpq1(a integer) ON COMMIT PRESERVE ROWS;
//
// DECLARE LOCAL TEMPORARY TABLE qfindall(
//	cat long varchar,
//	fio long varchar,
//	datr long varchar,
//	adr long varchar,
//	reg long varchar,
//	zag long varchar,
//	txt long varchar,
//	htm long varchar,
//	tab long varchar,
//	id integer default autoincrement,
// ) ON COMMIT PRESERVE ROWS;



//insert into qfindparm(typ, v1, v2, v3, datrog, yyrog, cat_set, obl) values('fio', 'ƒ∆Œ√ŒÀ‹','—≈–√≤…','¿Õ“ŒÕŒ¬»◊',null,null,'','');

//exec html_find;
//select * from qfindrez;



//delete from qfindone;
//create variable @q long varchar;
//select idser into @q from qfindrez where id = 1;
//exec html_w1130(@q);
//exec html_w974(@q);

//delete from qfindall;
//insert into qfindall(cat, fio, datr, adr, reg, zag, txt, htm, tab)
//select cat, fio, datr, adr, reg, zag, txt, htm, tab
//from qfindrez, qfindone where qfindrez.id=1;


//select htm from qfindall where id=2;
//select * from qfindall ;