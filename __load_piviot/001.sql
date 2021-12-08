----drop table zzzzzexceldata
--create table zzzzzexceldata(
--	efile nvarchar(4000)
--	shnam nvarchar(4000)
--	rnum integer 
--	cnum integer 
--	vv nvarchar(4000)
--	primary key(efileshnamrnumcnum)
--)
--GO

-- C#: E:\PROJECTS\Cpab.Risk.Etc\BuildTriggers\ExcelLoad003.cs

---- delete from zzzzzexceldata
---- select * from zzzzzexceldata
----zzz20211205a_2017
----nvarchar(4000)
---- select distinct vv cnum from zzzzzexceldata where rnum = 1 order by cnum

--select * into GUKV..zzzzzexceldata from zzzzzexceldata

--use GUKV

--select shnamcount(distinct rnum) from zzzzzexceldata group by shnam
	--річний 2017 ред	389
	--річний 2018 ред	451
	--річний 2019 ред	488
	--річний 2020 ред	459


-- select * from zzzzzexceldata A where shnam = 'річний 2017 ред' and rnum = 389
-- delete from zzzzzexceldata where shnam = 'річний 2017 ред' and rnum = 389


--select shnam cnum vv from zzzzzexceldata where rnum = 1

--alter table zzzzzexceldata add primary key(efileshnamrnumcnum)

/*
select
*
(select count(*) from zzzzzexceldata Q where Q.rnum = 1 and Q.vv = T.vv and Q.shnam = 'річний 2017 ред') as [2017]
(select count(*) from zzzzzexceldata Q where Q.rnum = 1 and Q.vv = T.vv and Q.shnam = 'річний 2018 ред') as [2018]
(select count(*) from zzzzzexceldata Q where Q.rnum = 1 and Q.vv = T.vv and Q.shnam = 'річний 2019 ред') as [2019]
(select count(*) from zzzzzexceldata Q where Q.rnum = 1 and Q.vv = T.vv and Q.shnam = 'річний 2020 ред') as [2020]
from
(
	select 
	vvcount(*) cnt
	from 
	(
		select 
			efileshnamrnumcnum
			case 
				when vv = 'Перераховано до бюджету % за звітний період всього з 1 січня поточного року грн. (без ПДВ)' 
					then 'Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)'
				when vv like 'Перераховано коштів до бюджету у звітному періоді _КАЗНАЧЕЙСТВО_ грн. (без ПДВ)'
					then 'Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)'
				when vv in ('Загальна вільна площа що може бути надана в оренду кв.м.''Загальна Площа вільних приміщень кв.м.')
					then 'Загальна вільна площа що може бути надана в оренду кв.м.'
				when vv in ('Надходження орендної плати за звітний період всього грн. (без ПДВ)''Отримано орендної плати всього за звітний період грн. (без ПДВ)')
					then 'Надходження орендної плати за звітний період всього грн. (без ПДВ)'
				when vv in ('- у тому числі з нарахованої за звітний період (без боргів та переплат)''Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)')
					then 'Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)'
				when vv in ('- у тому числі погашення заборгованості минулих періодів грн.''Погашення заборгованості минулих періодів')
					then 'Погашення заборгованості минулих періодів'
				--when vv in ('Назва Вулиці''Номер Будинку''Район')
				--	then 'добавить. Для отсуств.годов взять из новых'
				when vv in (
					'Кількість орендарів'
					'- у тому числі з нарахованої авансової орендної плати / забезпечувального депозиту грн.'
					'- у тому числі знято надмірно нарахованої за звітний період'
					'- у тому числі переплата орендної плати за звітний період грн.'
					'Авансова орендна плата / Забезпечувальний депозит (нараховано) грн.'
					'Госп. Структура'
					'Дата актуализации данных'
					'Дата Останнього Надсилання'
					'Дата останнього прийому'
					'Заборгованість з нарахованої авансової орендної плати грн. (без ПДВ)'
					'Кількість договорів інформація за якими буде надіслана'
					'Кількість об''єктів інформація за якими буде надіслана'
					'Надходження авансової орендної плати / забезпечувального депозиту у звітному періоді грн. (без ПДВ)'
					'Нараховано орендної плати без урахування надмірно нарахованої плати грн. (без ПДВ)'
					'Переплата орендної плати всього грн. (без ПДВ)'
					'Повернення переплати орендної плати всього у звітному періоді грн. (без ПДВ)'
					'Поточний стан звіту'
					'Примітки'
					'Примітки балансоутримувача'
					'Сальдо авансової орендної плати / забезпечувального депозиту на кінець звітного періоду грн. (без ПДВ)'
					'Сальдо платежів до бюджету (переплата на початок року) грн.'
					'Ставка відрахувань до бюджету (%)'
					'Стан актуализации данных'
					'Стан Звіту'
					'Стан юр.особи'
					'ЦМК Заборгованість по орендній платі грн. (без ПДВ)'
					'ЦМК Нарахована орендна плата грн. (без ПДВ)'
					'ЦМК Перераховано до бюджету грн. (без ПДВ)'
					'ЦМК Площа в оренді кв.м.'
					)
					then '-'
				else
					vv
			end as vv
		from zzzzzexceldata Q
	) TT
	where rnum = 1 group by vv 
) T
where T.cnt <> 4
--and T.vv = '-'
order by 1
*/

-- drop table zzzzz20211205col

--select
--*
--into zzzzz20211205col
--from
--(
--	select 
--	efileshnamrnumcnumvv
--	case 
--		when vv = 'Перераховано до бюджету % за звітний період всього з 1 січня поточного року грн. (без ПДВ)' 
--			then 'Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)'
--		when vv like 'Перераховано коштів до бюджету у звітному періоді _КАЗНАЧЕЙСТВО_ грн. (без ПДВ)'
--			then 'Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)'
--		when vv in ('Загальна вільна площа що може бути надана в оренду кв.м.''Загальна Площа вільних приміщень кв.м.')
--			then 'Загальна вільна площа що може бути надана в оренду кв.м.'
--		when vv in ('Надходження орендної плати за звітний період всього грн. (без ПДВ)''Отримано орендної плати всього за звітний період грн. (без ПДВ)')
--			then 'Надходження орендної плати за звітний період всього грн. (без ПДВ)'
--		when vv in ('- у тому числі з нарахованої за звітний період (без боргів та переплат)''Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)')
--			then 'Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)'
--		when vv in ('- у тому числі погашення заборгованості минулих періодів грн.''Погашення заборгованості минулих періодів')
--			then 'Погашення заборгованості минулих періодів'
--		--when vv in ('Назва Вулиці''Номер Будинку''Район')
--		--	then 'добавить. Для отсуств.годов взять из новых'
--		when vv = '- в тому числі (із загальної заборгованості) заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)'
--			then '- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)'
--		when vv in (
--			'Кількість орендарів'
--			'- у тому числі з нарахованої авансової орендної плати / забезпечувального депозиту грн.'
--			'- у тому числі знято надмірно нарахованої за звітний період'
--			'- у тому числі переплата орендної плати за звітний період грн.'
--			'Авансова орендна плата / Забезпечувальний депозит (нараховано) грн.'
--			'Госп. Структура'
--			'Дата актуализации данных'
--			'Дата Останнього Надсилання'
--			'Дата останнього прийому'
--			'Заборгованість з нарахованої авансової орендної плати грн. (без ПДВ)'
--			'Кількість договорів інформація за якими буде надіслана'
--			'Кількість об''єктів інформація за якими буде надіслана'
--			'Надходження авансової орендної плати / забезпечувального депозиту у звітному періоді грн. (без ПДВ)'
--			'Нараховано орендної плати без урахування надмірно нарахованої плати грн. (без ПДВ)'
--			'Переплата орендної плати всього грн. (без ПДВ)'
--			'Повернення переплати орендної плати всього у звітному періоді грн. (без ПДВ)'
--			'Поточний стан звіту'
--			'Примітки'
--			'Примітки балансоутримувача'
--			'Сальдо авансової орендної плати / забезпечувального депозиту на кінець звітного періоду грн. (без ПДВ)'
--			'Сальдо платежів до бюджету (переплата на початок року) грн.'
--			'Ставка відрахувань до бюджету (%)'
--			'Стан актуализации данных'
--			'Стан Звіту'
--			'Стан юр.особи'
--			'ЦМК Заборгованість по орендній платі грн. (без ПДВ)'
--			'ЦМК Нарахована орендна плата грн. (без ПДВ)'
--			'ЦМК Перераховано до бюджету грн. (без ПДВ)'
--			'ЦМК Площа в оренді кв.м.'
--			)
--			then '-'
--		else
--			vv
--	end as col
--	from zzzzzexceldata Q
--) TT
--where rnum = 1 
--and col <> '-'

--select * from zzzzz20211205col

--select len(col) * from (select distinct col efile from zzzzz20211205col) A order by 1 desc

/*
select
replace(replace(replace(replace(replace(
'(select Q.vv from zzzzzexceldata Q where Q.efile = ''<efile>'' and Q.shnam = ''<shnam>'' and Q.cnum = <cnum> and Q.rnum = <rnum>) as [<col>]' 
'<efile>'efile)
'<shnam>'shnam)
'<cnum>'CONCAT(''r_cnum))
'<rnum>''A.rnum')
'<col>'col)
as asql
*
from
(
	select
	isnull( (select Q.cnum from zzzzz20211205col Q where Q.efile = A.efile and Q.shnam = B.shnam and Q.col = A.col) -1) as r_cnum
	*
	from (select distinct col efile from zzzzz20211205col) A
	join (select distinct shnam cast(REPLACE(REPLACE(shnam'річний ''')' ред''') as int) yy from zzzzz20211205col) B on 1=1
) T
--where yy = 2017
--where yy = 2018
--where yy = 2019
where yy = 2020


select
2017 yy
rnum
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 23 and Q.rnum = A.rnum) as [- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 22 and Q.rnum = A.rnum) as [- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 26 and Q.rnum = A.rnum) as [- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 27 and Q.rnum = A.rnum) as [Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 28 and Q.rnum = A.rnum) as [Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 15 and Q.rnum = A.rnum) as [Загальна вільна площа що може бути надана в оренду кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 21 and Q.rnum = A.rnum) as [Загальна заборгованість по орендній платі грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 7 and Q.rnum = A.rnum) as [Загальна площа що знаходиться на балансі кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 9 and Q.rnum = A.rnum) as [Загальна площа що знята з балансу кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 12 and Q.rnum = A.rnum) as [Загальна площа що надається в оренду кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 13 and Q.rnum = A.rnum) as [Кількість договорів оренди]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 14 and Q.rnum = A.rnum) as [Кількість договорів орендування]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 10 and Q.rnum = A.rnum) as [Кількість об'єктів знятих з балансу]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 6 and Q.rnum = A.rnum) as [Кількість об'єктів на балансі]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 2 and Q.rnum = A.rnum) as [Код ЄДРПОУ]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 8 and Q.rnum = A.rnum) as [Корисна площа що знаходиться на балансі кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 18 and Q.rnum = A.rnum) as [Надходження орендної плати за звітний період всього грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Назва Вулиці]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 1 and Q.rnum = A.rnum) as [Назва Організації]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 24 and Q.rnum = A.rnum) as [Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 16 and Q.rnum = A.rnum) as [Нараховано орендної плати за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Номер Будинку]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 19 and Q.rnum = A.rnum) as [Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Отримано орендної плати в тому числі інші платежі]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 25 and Q.rnum = A.rnum) as [Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 11 and Q.rnum = A.rnum) as [Площа що орендується кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 20 and Q.rnum = A.rnum) as [Погашення заборгованості минулих періодів]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Район]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 17 and Q.rnum = A.rnum) as [Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2017 ред' and Q.cnum = 5 and Q.rnum = A.rnum) as [Сфера діяльності]
into zzzzz20211205_2017
from (select distinct Q.rnum from zzzzzexceldata Q where Q.shnam = 'річний 2017 ред' and Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.rnum > 1) A
order by rnum


select
2018 yy
rnum
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 23 and Q.rnum = A.rnum) as [- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 22 and Q.rnum = A.rnum) as [- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 26 and Q.rnum = A.rnum) as [- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 27 and Q.rnum = A.rnum) as [Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 28 and Q.rnum = A.rnum) as [Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 15 and Q.rnum = A.rnum) as [Загальна вільна площа що може бути надана в оренду кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 21 and Q.rnum = A.rnum) as [Загальна заборгованість по орендній платі грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 7 and Q.rnum = A.rnum) as [Загальна площа що знаходиться на балансі кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 9 and Q.rnum = A.rnum) as [Загальна площа що знята з балансу кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 12 and Q.rnum = A.rnum) as [Загальна площа що надається в оренду кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 13 and Q.rnum = A.rnum) as [Кількість договорів оренди]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 14 and Q.rnum = A.rnum) as [Кількість договорів орендування]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 10 and Q.rnum = A.rnum) as [Кількість об'єктів знятих з балансу]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 6 and Q.rnum = A.rnum) as [Кількість об'єктів на балансі]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 2 and Q.rnum = A.rnum) as [Код ЄДРПОУ]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 8 and Q.rnum = A.rnum) as [Корисна площа що знаходиться на балансі кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 18 and Q.rnum = A.rnum) as [Надходження орендної плати за звітний період всього грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Назва Вулиці]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 1 and Q.rnum = A.rnum) as [Назва Організації]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 24 and Q.rnum = A.rnum) as [Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 16 and Q.rnum = A.rnum) as [Нараховано орендної плати за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Номер Будинку]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 19 and Q.rnum = A.rnum) as [Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Отримано орендної плати в тому числі інші платежі]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 25 and Q.rnum = A.rnum) as [Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 11 and Q.rnum = A.rnum) as [Площа що орендується кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 20 and Q.rnum = A.rnum) as [Погашення заборгованості минулих періодів]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Район]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 17 and Q.rnum = A.rnum) as [Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2018 ред' and Q.cnum = 5 and Q.rnum = A.rnum) as [Сфера діяльності]
into zzzzz20211205_2018
from (select distinct Q.rnum from zzzzzexceldata Q where Q.shnam = 'річний 2018 ред' and Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.rnum > 1) A
order by rnum

select * from zzzzz20211205_2018


select
2019 yy
rnum
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 31 and Q.rnum = A.rnum) as [- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 30 and Q.rnum = A.rnum) as [- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 34 and Q.rnum = A.rnum) as [- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 35 and Q.rnum = A.rnum) as [Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 36 and Q.rnum = A.rnum) as [Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 22 and Q.rnum = A.rnum) as [Загальна вільна площа що може бути надана в оренду кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 29 and Q.rnum = A.rnum) as [Загальна заборгованість по орендній платі грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 13 and Q.rnum = A.rnum) as [Загальна площа що знаходиться на балансі кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 15 and Q.rnum = A.rnum) as [Загальна площа що знята з балансу кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 18 and Q.rnum = A.rnum) as [Загальна площа що надається в оренду кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 19 and Q.rnum = A.rnum) as [Кількість договорів оренди]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 21 and Q.rnum = A.rnum) as [Кількість договорів орендування]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 16 and Q.rnum = A.rnum) as [Кількість об'єктів знятих з балансу]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 9 and Q.rnum = A.rnum) as [Кількість об'єктів на балансі]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 2 and Q.rnum = A.rnum) as [Код ЄДРПОУ]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 14 and Q.rnum = A.rnum) as [Корисна площа що знаходиться на балансі кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 25 and Q.rnum = A.rnum) as [Надходження орендної плати за звітний період всього грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 7 and Q.rnum = A.rnum) as [Назва Вулиці]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 1 and Q.rnum = A.rnum) as [Назва Організації]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 32 and Q.rnum = A.rnum) as [Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 23 and Q.rnum = A.rnum) as [Нараховано орендної плати за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 8 and Q.rnum = A.rnum) as [Номер Будинку]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 26 and Q.rnum = A.rnum) as [Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 27 and Q.rnum = A.rnum) as [Отримано орендної плати в тому числі інші платежі]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = -1 and Q.rnum = A.rnum) as [Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 33 and Q.rnum = A.rnum) as [Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 17 and Q.rnum = A.rnum) as [Площа що орендується кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 28 and Q.rnum = A.rnum) as [Погашення заборгованості минулих періодів]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 6 and Q.rnum = A.rnum) as [Район]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 24 and Q.rnum = A.rnum) as [Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2019 ред' and Q.cnum = 5 and Q.rnum = A.rnum) as [Сфера діяльності]
into zzzzz20211205_2019
from (select distinct Q.rnum from zzzzzexceldata Q where Q.shnam = 'річний 2019 ред' and Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.rnum > 1) A
order by rnum

--select A.[Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)] from zzzzz20211205_2019 A

select
2020 yy
rnum
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 44 and Q.rnum = A.rnum) as [- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 43 and Q.rnum = A.rnum) as [- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 48 and Q.rnum = A.rnum) as [- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 49 and Q.rnum = A.rnum) as [Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 50 and Q.rnum = A.rnum) as [Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 25 and Q.rnum = A.rnum) as [Загальна вільна площа що може бути надана в оренду кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 42 and Q.rnum = A.rnum) as [Загальна заборгованість по орендній платі грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 16 and Q.rnum = A.rnum) as [Загальна площа що знаходиться на балансі кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 18 and Q.rnum = A.rnum) as [Загальна площа що знята з балансу кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 21 and Q.rnum = A.rnum) as [Загальна площа що надається в оренду кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 22 and Q.rnum = A.rnum) as [Кількість договорів оренди]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 24 and Q.rnum = A.rnum) as [Кількість договорів орендування]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 19 and Q.rnum = A.rnum) as [Кількість об'єктів знятих з балансу]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 15 and Q.rnum = A.rnum) as [Кількість об'єктів на балансі]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 2 and Q.rnum = A.rnum) as [Код ЄДРПОУ]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 17 and Q.rnum = A.rnum) as [Корисна площа що знаходиться на балансі кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 32 and Q.rnum = A.rnum) as [Надходження орендної плати за звітний період всього грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 11 and Q.rnum = A.rnum) as [Назва Вулиці]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 1 and Q.rnum = A.rnum) as [Назва Організації]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 45 and Q.rnum = A.rnum) as [Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 27 and Q.rnum = A.rnum) as [Нараховано орендної плати за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 12 and Q.rnum = A.rnum) as [Номер Будинку]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 33 and Q.rnum = A.rnum) as [Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 41 and Q.rnum = A.rnum) as [Отримано орендної плати в тому числі інші платежі]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 51 and Q.rnum = A.rnum) as [Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 47 and Q.rnum = A.rnum) as [Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 20 and Q.rnum = A.rnum) as [Площа що орендується кв.м.]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 35 and Q.rnum = A.rnum) as [Погашення заборгованості минулих періодів]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 10 and Q.rnum = A.rnum) as [Район]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 29 and Q.rnum = A.rnum) as [Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)]
(select Q.vv from zzzzzexceldata Q where Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.shnam = 'річний 2020 ред' and Q.cnum = 9 and Q.rnum = A.rnum) as [Сфера діяльності]
into zzzzz20211205_2020
from (select distinct Q.rnum from zzzzzexceldata Q where Q.shnam = 'річний 2020 ред' and Q.efile = 'E:\PROJECTS\DKVSOURCESFINALEDITION_v20\__load_piviot\2021_11_23_2304_ПРИЙОМ_ЗВІТІВ_2017_2018_2019_2020_ОСНОВА.xlsx' and Q.rnum > 1) A
order by rnum

select * from zzzzz20211205_2020
*/

--select 
--[yy][rnum][- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)][- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)][- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)][Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)][Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)][Загальна вільна площа що може бути надана в оренду кв.м.][Загальна заборгованість по орендній платі грн. (без ПДВ)][Загальна площа що знаходиться на балансі кв.м.][Загальна площа що знята з балансу кв.м.][Загальна площа що надається в оренду кв.м.][Кількість договорів оренди][Кількість договорів орендування][Кількість об'єктів знятих з балансу][Кількість об'єктів на балансі][Код ЄДРПОУ][Корисна площа що знаходиться на балансі кв.м.][Надходження орендної плати за звітний період всього грн. (без ПДВ)][Назва Вулиці][Назва Організації][Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)][Нараховано орендної плати за звітний період грн. (без ПДВ)][Номер Будинку][Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)][Отримано орендної плати в тому числі інші платежі][Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')][Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)][Площа що орендується кв.м.][Погашення заборгованості минулих періодів][Район][Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)][Сфера діяльності]
--into zzzzz20211205b
--from zzzzz20211205_2017 union all
--select 
--[yy][rnum][- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)][- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)][- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)][Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)][Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)][Загальна вільна площа що може бути надана в оренду кв.м.][Загальна заборгованість по орендній платі грн. (без ПДВ)][Загальна площа що знаходиться на балансі кв.м.][Загальна площа що знята з балансу кв.м.][Загальна площа що надається в оренду кв.м.][Кількість договорів оренди][Кількість договорів орендування][Кількість об'єктів знятих з балансу][Кількість об'єктів на балансі][Код ЄДРПОУ][Корисна площа що знаходиться на балансі кв.м.][Надходження орендної плати за звітний період всього грн. (без ПДВ)][Назва Вулиці][Назва Організації][Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)][Нараховано орендної плати за звітний період грн. (без ПДВ)][Номер Будинку][Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)][Отримано орендної плати в тому числі інші платежі][Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')][Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)][Площа що орендується кв.м.][Погашення заборгованості минулих періодів][Район][Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)][Сфера діяльності]
--from zzzzz20211205_2018 union all
--select 
--[yy][rnum][- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)][- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)][- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)][Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)][Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)][Загальна вільна площа що може бути надана в оренду кв.м.][Загальна заборгованість по орендній платі грн. (без ПДВ)][Загальна площа що знаходиться на балансі кв.м.][Загальна площа що знята з балансу кв.м.][Загальна площа що надається в оренду кв.м.][Кількість договорів оренди][Кількість договорів орендування][Кількість об'єктів знятих з балансу][Кількість об'єктів на балансі][Код ЄДРПОУ][Корисна площа що знаходиться на балансі кв.м.][Надходження орендної плати за звітний період всього грн. (без ПДВ)][Назва Вулиці][Назва Організації][Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)][Нараховано орендної плати за звітний період грн. (без ПДВ)][Номер Будинку][Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)][Отримано орендної плати в тому числі інші платежі][Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')][Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)][Площа що орендується кв.м.][Погашення заборгованості минулих періодів][Район][Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)][Сфера діяльності]
--from zzzzz20211205_2019 union all
--select 
--[yy][rnum][- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)][- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)][- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)][Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)][Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)][Загальна вільна площа що може бути надана в оренду кв.м.][Загальна заборгованість по орендній платі грн. (без ПДВ)][Загальна площа що знаходиться на балансі кв.м.][Загальна площа що знята з балансу кв.м.][Загальна площа що надається в оренду кв.м.][Кількість договорів оренди][Кількість договорів орендування][Кількість об'єктів знятих з балансу][Кількість об'єктів на балансі][Код ЄДРПОУ][Корисна площа що знаходиться на балансі кв.м.][Надходження орендної плати за звітний період всього грн. (без ПДВ)][Назва Вулиці][Назва Організації][Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)][Нараховано орендної плати за звітний період грн. (без ПДВ)][Номер Будинку][Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)][Отримано орендної плати в тому числі інші платежі][Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')][Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)][Площа що орендується кв.м.][Погашення заборгованості минулих періодів][Район][Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)][Сфера діяльності]
--from zzzzz20211205_2020

--select 
--* 
--from zzzzz20211205b A


--select
--(select Q.cnum from zzzzz20211205col Q where Q.shnam = 'річний 2020 ред' and Q.col = A.COLUMN_NAME collate Cyrillic_General_CI_AS) rr_cnum
--'case when [' + A.COLUMN_NAME + '] <> '''' then cast([' + A.COLUMN_NAME + '] as decimal(185)) end as [' + A.COLUMN_NAME + ']' colname
--*
--from INFORMATION_SCHEMA.COLUMNS A
--where A.TABLE_NAME = 'zzzzz20211205b'
--order by 1


--select * from zzzzz20211205col
--select cast('' as decimal(185))

/*
select
[yy]
[rnum]
[Назва Організації]
[Код ЄДРПОУ]
[Сфера діяльності]
[Район]
[Назва Вулиці]
[Номер Будинку]
case when [Кількість об'єктів на балансі] <> '' then cast([Кількість об'єктів на балансі] as int) end as [Кількість об'єктів на балансі]
case when [Загальна площа що знаходиться на балансі кв.м.] <> '' then cast([Загальна площа що знаходиться на балансі кв.м.] as decimal(185)) end as [Загальна площа що знаходиться на балансі кв.м.]
case when [Корисна площа що знаходиться на балансі кв.м.] <> '' then cast([Корисна площа що знаходиться на балансі кв.м.] as decimal(185)) end as [Корисна площа що знаходиться на балансі кв.м.]
case when [Загальна площа що знята з балансу кв.м.] <> '' then cast([Загальна площа що знята з балансу кв.м.] as decimal(185)) end as [Загальна площа що знята з балансу кв.м.]
case when [Кількість об'єктів знятих з балансу] <> '' then cast([Кількість об'єктів знятих з балансу] as int) end as [Кількість об'єктів знятих з балансу]
case when [Площа що орендується кв.м.] <> '' then cast([Площа що орендується кв.м.] as decimal(185)) end as [Площа що орендується кв.м.]
case when [Загальна площа що надається в оренду кв.м.] <> '' then cast([Загальна площа що надається в оренду кв.м.] as decimal(185)) end as [Загальна площа що надається в оренду кв.м.]
case when [Кількість договорів оренди] <> '' then cast([Кількість договорів оренди] as int) end as [Кількість договорів оренди]
case when [Кількість договорів орендування] <> '' then cast([Кількість договорів орендування] as int) end as [Кількість договорів орендування]
case when [Загальна вільна площа що може бути надана в оренду кв.м.] <> '' then cast([Загальна вільна площа що може бути надана в оренду кв.м.] as decimal(185)) end as [Загальна вільна площа що може бути надана в оренду кв.м.]
case when [Нараховано орендної плати за звітний період грн. (без ПДВ)] <> '' then cast([Нараховано орендної плати за звітний період грн. (без ПДВ)] as decimal(185)) end as [Нараховано орендної плати за звітний період грн. (без ПДВ)]
case when [Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)] <> '' then cast([Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)] as decimal(185)) end as [Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)]
case when [Надходження орендної плати за звітний період всього грн. (без ПДВ)] <> '' then cast([Надходження орендної плати за звітний період всього грн. (без ПДВ)] as decimal(185)) end as [Надходження орендної плати за звітний період всього грн. (без ПДВ)]
case when [Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)] <> '' then cast([Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)] as decimal(185)) end as [Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)]
case when [Погашення заборгованості минулих періодів] <> '' then cast([Погашення заборгованості минулих періодів] as decimal(185)) end as [Погашення заборгованості минулих періодів]
case when [Отримано орендної плати в тому числі інші платежі] <> '' then cast([Отримано орендної плати в тому числі інші платежі] as decimal(185)) end as [Отримано орендної плати в тому числі інші платежі]
case when [Загальна заборгованість по орендній платі грн. (без ПДВ)] <> '' then cast([Загальна заборгованість по орендній платі грн. (без ПДВ)] as decimal(185)) end as [Загальна заборгованість по орендній платі грн. (без ПДВ)]
case when [- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)] <> '' then cast([- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)] as decimal(185)) end as [- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)]
case when [- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)] <> '' then cast([- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)] as decimal(185)) end as [- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)]
case when [Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)] <> '' then cast([Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)] as decimal(185)) end as [Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)]
case when [Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)] <> '' then cast([Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)] as decimal(185)) end as [Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)]
case when [- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)] <> '' then cast([- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)] as decimal(185)) end as [- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)]
case when [Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)] <> '' then cast([Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)] as decimal(185)) end as [Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)]
case when [Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)] <> '' then cast([Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)] as decimal(185)) end as [Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)]
case when [Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')] <> '' then cast([Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')] as decimal(185)) end as [Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')]
into zzzzz20211205d
from zzzzz20211205b A
where case when A.yy in (20172018) and A.rnum = 2 then 1 else 0 end = 0
--where yy = 2017	and rnum = 187
--and [Загальна площа що знаходиться на балансі кв.м.] = ''
--select [Загальна площа що знаходиться на балансі кв.м.]* from zzzzz20211205b A where ISNUMERIC([Загальна площа що знаходиться на балансі кв.м.]) = 0
*/


--SELECT
--[yy]
--[rnum]
--[Назва Організації] as nam 
--[Код ЄДРПОУ] as zkpo
--[Сфера діяльності] as sfera
--[Район] as rayon
--[Назва Вулиці] as ulic
--[Номер Будинку] as dom
--[Кількість об'єктів на балансі] as v1
--[Загальна площа що знаходиться на балансі кв.м.] as v2
--[Корисна площа що знаходиться на балансі кв.м.] as v3
--[Загальна площа що знята з балансу кв.м.] as v4
--[Кількість об'єктів знятих з балансу] as v5
--[Площа що орендується кв.м.] as v6
--[Загальна площа що надається в оренду кв.м.] as v7
--[Кількість договорів оренди] as v8
--[Кількість договорів орендування] as v9
--[Загальна вільна площа що може бути надана в оренду кв.м.] as v10
--[Нараховано орендної плати за звітний період грн. (без ПДВ)] as v11
--[Сальдо на початок року (не змінна впродовж року величина) грн.(без ПДВ)] as v12
--[Надходження орендної плати за звітний період всього грн. (без ПДВ)] as v13
--[Отримано орендної плати в тому числі з нарахованої за звітний період грн. (без ПДВ)] as v14
--[Погашення заборгованості минулих періодів] as v15
--[Отримано орендної плати в тому числі інші платежі] as v16
--[Загальна заборгованість по орендній платі грн. (без ПДВ)] as v17
--[- в тому числі заборгованість по орендній платі за звітний період  грн. (без ПДВ)] as v18
--[- в тому числі заборгованість з орендної плати розмір якої встановлено в межах витрат на утримання грн. (без ПДВ)] as v19
--[Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період грн. (без ПДВ)] as v20
--[Перераховано коштів до бюджету у звітному періоді "КАЗНАЧЕЙСТВО" грн. (без ПДВ)] as v21
--[- в тому числі перераховано до бюджету % боргів у звітному періоді з 1 січня поточного року за попередні роки грн. (без ПДВ)] as v22
--[Заборгованість зі сплати % до бюджету від оренди майна за  звітний період грн. (без ПДВ)] as v23
--[Заборгованість зі сплати % до бюджету від оренди майна минулих років грн. (без ПДВ)] as v24
--[Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')] as v25
--into zzzzz20211205e
--FROM [zzzzz20211205d] A


--select * from zzzzz20211205e

--CREATE TABLE OlapReportTotalYear(
--	[yy] [int] NOT NULL
--	[rnum] [int] NOT NULL
--	[nam] [nvarchar](4000) NULL
--	[zkpo] [nvarchar](4000) NULL
--	[sfera] [nvarchar](4000) NULL
--	[rayon] [nvarchar](4000) NULL
--	[ulic] [nvarchar](4000) NULL
--	[dom] [nvarchar](4000) NULL
--	[v1] [int] NULL
--	[v2] [decimal](18 5) NULL
--	[v3] [decimal](18 5) NULL
--	[v4] [decimal](18 5) NULL
--	[v5] [int] NULL
--	[v6] [decimal](18 5) NULL
--	[v7] [decimal](18 5) NULL
--	[v8] [int] NULL
--	[v9] [int] NULL
--	[v10] [decimal](18 5) NULL
--	[v11] [decimal](18 5) NULL
--	[v12] [decimal](18 5) NULL
--	[v13] [decimal](18 5) NULL
--	[v14] [decimal](18 5) NULL
--	[v15] [decimal](18 5) NULL
--	[v16] [decimal](18 5) NULL
--	[v17] [decimal](18 5) NULL
--	[v18] [decimal](18 5) NULL
--	[v19] [decimal](18 5) NULL
--	[v20] [decimal](18 5) NULL
--	[v21] [decimal](18 5) NULL
--	[v22] [decimal](18 5) NULL
--	[v23] [decimal](18 5) NULL
--	[v24] [decimal](18 5) NULL
--	[v25] [decimal](18 5) NULL
--	primary key([yy][rnum])
--	unique([yy][zkpo])
--)
--GO


/*
select zkpoyy from (select distinct * from zzzzz20211205e) t group by zkpoyy having count(*) > 1
select * from zzzzz20211205e where zkpo = '03327664' and yy = 2018 and v2 = 15
delete from zzzzz20211205e where zkpo = '03327664' and yy = 2018 and v2 = 15

select * from zzzzz20211205e where zkpo = '30382088' and yy = 2018 and rnum = 317
delete from zzzzz20211205e where zkpo = '30382088' and yy = 2018 and rnum = 317

insert into OlapReportTotalYear(
[yy][rnum][nam][zkpo][sfera][rayon][ulic][dom][v1][v2][v3][v4][v5][v6][v7][v8][v9][v10][v11][v12][v13][v14][v15][v16][v17][v18][v19][v20][v21][v22][v23][v24][v25]
)
select 
[yy][rnum][nam][zkpo][sfera][rayon][ulic][dom][v1][v2][v3][v4][v5][v6][v7][v8][v9][v10][v11][v12][v13][v14][v15][v16][v17][v18][v19][v20][v21][v22][v23][v24][v25]
from zzzzz20211205e
*/

--select sfera from OlapReportTotalYear where yy in (20172018)

--update OlapReportTotalYear set 
--[rayon] = (select top 1 Q.[rayon] from OlapReportTotalYear Q where Q.zkpo = OlapReportTotalYear.zkpo and Q.yy in (20192020) and Q.[rayon] <> '' order by Q.yy desc)
--[ulic] = (select top 1 Q.[ulic] from OlapReportTotalYear Q where Q.zkpo = OlapReportTotalYear.zkpo and Q.yy in (20192020) and Q.[ulic] <> '' order by Q.yy desc)
--[dom] = (select top 1 Q.[dom] from OlapReportTotalYear Q where Q.zkpo = OlapReportTotalYear.zkpo and Q.yy in (20192020) and Q.[dom] <> '' order by Q.yy desc)
--where yy in (20172018)

--select [rayon][ulic][dom] from OlapReportTotalYear where yy in (20172018)

--ALTER USER [yurdep] WITH LOGIN = [yurdep]