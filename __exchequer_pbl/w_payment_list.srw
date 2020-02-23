$PBExportHeader$w_payment_list.srw
forward
global type w_payment_list from Window
end type
type dw_1 from datawindow within w_payment_list
end type
end forward

global type w_payment_list from Window
int X=677
int Y=424
int Width=3493
int Height=2032
boolean TitleBar=true
string Title="Платежі"
string MenuName="m_payment_list"
long BackColor=80269524
boolean ControlMenu=true
boolean MinBox=true
boolean MaxBox=true
boolean Resizable=true
dw_1 dw_1
end type
global w_payment_list w_payment_list

type variables
string		sql_dw

datastore		ds_crit

boolean		is_first

boolean		is_import_start
string		import_srcfile

long		row_multiselect

boolean		ignore_itemerror
end variables

forward prototypes
public subroutine init ()
public function boolean find ()
public subroutine del ()
public function integer itemchanged_func (long row, dwobject dwo, string data)
public function boolean sav ()
public subroutine reset ()
public subroutine add ()
public function string get_date_list (ref date ret[], string arg)
public function boolean import_dbf ()
public subroutine trash_go ()
public subroutine refresh_status (long row)
end prototypes

public subroutine init ();long r
boolean is_cancel_import_start
s_parm parm


	//parm
is_first = true	
if f_parm(parm) then
	if classname(parm.p1) <> "any" then is_import_start = parm.p1
end if

	//import_dbf
if is_import_start then
	if not import_dbf() then
		is_cancel_import_start = true
		post close(this)
	end if
end if


if not is_cancel_import_start then
		//ds_crit
	ds_crit = create datastore
	ds_crit.dataobject = "c_payment"
	ds_crit.insertrow(0)
	if import_srcfile <> "" then
		ds_crit.object.srcfile[1] = import_srcfile
	end if
	
		//title
	if import_srcfile <> "" then	
		title += " (імпорт з файлу ~"" + import_srcfile + "~")"
	end if
	
	
		//retrieve
	dw_1.settransobject(sqlca)
	sql_dw = dw_1.getsqlselect()
	
		//f_wnd_init
	f_wnd_init(this)
	
		//сразу поиск документа
	if not find() then
		post close(this)
	end if
end if
end subroutine

public function boolean find ();long k, document_id
string wh, sql, docset
s_parm parm

	//sav
if not sav() then return false

	//wh
parm.p1 = ds_crit
parm.p2 = ""
parm.p3 = (is_first and is_import_start) //isauto
openwithparm(w_crit_payment, parm)
if not isvalid(message.powerobjectparm) then return false
parm = message.powerobjectparm
wh = parm.p1


	//retrieve
f_dw_where(dw_1, sql_dw, wh)
sql = dw_1.getsqlselect()
if dw_1.retrieve() = -1 then return false
//dp(dw_1.getsqlselect())
dw_1.setfocus()
is_first = false

return true
end function

public subroutine del ();long r

	//row
r = dw_1.getrow()
if r = 0 then return

	//quest
if not f_txt_quest2("Удалять запись на лицо «" + dw_1.object.person_nam[r] + "» ?") then return

dw_1.deleterow(r)


//	//commit
////commit;
//return
////err_end:
//	rollback;
//	return
////err_1:
//	f_txt_err("Удалить можно только документ, владельцем которого Вы являетесь")
//	return
end subroutine

public function integer itemchanged_func (long row, dwobject dwo, string data);string bal_zkpo, bal_name, ident_bal_zkpo, rowstatus

if dwo.name = "ident_bal_zkpo" then
	if trim(sn(data)) <> "" then
		ident_bal_zkpo = trim(sn(data))

		setnull(bal_zkpo)
		select bal_zkpo, bal_name into :bal_zkpo, :bal_name from exchequer.lookup_1 where bal_zkpo = :ident_bal_zkpo;
		if sqlca.sqlcode = -1 then signalerror(0, sn(sqlca.sqlerrtext))
		
		if isnull(bal_zkpo) then
			//dw_1.post setcolumn("ident_bal_zkpo")
			f_txt_err("Балансоутримувач з ЄДРПОУ ~"" + ident_bal_zkpo + "~" не знайдений")
			ignore_itemerror = true
			return 1
		end if
		
		dw_1.object.ident_bal_name[row] = bal_name
	else
		dw_1.object.ident_bal_name[row] = ""
	end if
end if

if in(dwo.name, {"ident_bal_zkpo", "ident_period_list"}) then
	refresh_status(row)
	
//	rowstatus = dw_1.object.rowstatus[row]
//	if rowstatus = "0" then
//		//nothig
//	else
//		if sn(trim(f_dw_val(dw_1, row, "ident_bal_zkpo"))) <> "" and sn(trim(f_dw_val(dw_1, row, "ident_period_list"))) <> "" then
//			dw_1.object.rowstatus[row] = "M"
//		else
//			dw_1.object.rowstatus[row] = "-"
//		end if
//	end if
end if

return 0
end function

public function boolean sav ();long row, payment_id, i, j
string err
date dat, dats[]


	
	//init
if dw_1.accepttext() = -1 then return false
if not f_dw_modify(dw_1) then return true

	//nulltest
if not f_dw_nulltest(dw_1, {"person_id"}) then return false

////обновить для лиц (готовим)
//do while true
//	row = dw_1.getnextmodified(row, primary!)
//	if row = 0 then exit
//	person_id = dw_1.object.person_id[row]
//	person_set.add(person_id)
//	updaterow_set.add(row)
//loop
//for row = 1 to dw_1.DeletedCount()
//	person_id = dw_1.GetItemNumber(row, "person_id", delete!, false)
//	person_set.add(person_id)
//next
//
//	//обновить свящи 
//for i = 1 to updaterow_set.count
//	if not link_update(updaterow_set.data[i]) then goto err_end
//next

	//update
if dw_1.update(true, false) = -1 then goto err_end


	//exchequer.payments_period
do while true
	row = dw_1.getnextmodified(row, primary!)
	if row = 0 then exit
	
	payment_id = dw_1.object.payment_id[row]
	err = get_date_list(dats, dw_1.object.ident_period_list[row])
	if err <> "" then goto err_100
		
	delete from exchequer.payments_period where payment_id = :payment_id;
	if sqlca.sqlcode = -1 then goto err_sql
	for i = 1 to upperbound(dats)
		dat = dats[i]
		if day(dat) <> 1 then goto err_200
		
		for j = 1 to i - 1
			if dat = dats[j] then goto err_300
		next
		
		insert into exchequer.payments_period(payment_id, ident_period)
		values(:payment_id, :dats[i]);
		if sqlca.sqlcode = -1 then goto err_sql
	next
loop



	
	//end
dw_1.resetupdate()
commit;
return true
err_100:
	f_dw_focus(dw_1, "ident_period_list", row)
	f_txt_err(err)
	goto err_end
err_200:
	f_dw_focus(dw_1, "ident_period_list", row)
	f_txt_err("Необхідно вказати дату з першим днем місяця (" + string(dat,"dd.mm.yyyy") + ")")
	goto err_end
err_300:
	f_dw_focus(dw_1, "ident_period_list", row)
	f_txt_err("Дубль дати " + string(dat,"dd.mm.yyyy"))
	goto err_end
err_sql:
	f_txt_errsql()
	goto err_end
err_end:
	rollback;
	return false
end function

public subroutine reset ();f_dw_reset(dw_1)
end subroutine

public subroutine add ();long row, p_id
string person_nam

row = f_dw_add(dw_1)
//if not isnull(open_person_id) then
//	dw_1.object.person_id[row] = open_person_id
//	
//		select nam_person_f(person_id), person_id into :person_nam, :p_id from person where person_id = :open_person_id;
//		if sqlca.sqlcode = -1 then signalerror(0, sn(sqlca.sqlerrtext))
//	
//		if not isnull(p_id) then
//			dw_1.object.person_nam[row] = person_nam
//		end if
//end if
end subroutine

public function string get_date_list (ref date ret[], string arg);long i
string vv
date dat, empty[]
u_string_set vals

ret = empty
arg = trim(sn(arg))
if arg = "" then
	return ""
end if

vals = f_str_parse(arg, ";")
for i = 1 to vals.count
	vv = trim(vals.data[i])
	dat = f_ddmmyyyy2date(vv)
	if isnull(dat) then
		return "Строка ~"" + vv + "~" є невірною датою (формат дати повинен бути ДД.ММ.РРРР)"
	end if
	
	ret[upperbound(ret) + 1] = dat
next
return ""
end function

public function boolean import_dbf ();long i, r, n, ret, ecnt, payment_id, f
string str, col, pathname, filename, srcfile, qtxt, pay_zkpo, rowstatus
date dat_period
datastore ds, ds2, ds3, dl1, dl2
dwobject dso, dso2
u_pay_text_parser u_pay_text_parser


if GetFileOpenName("Вкажіть файл для завантаження", pathname, filename, "dbf", "DBF-файли (*.dbf),*.dbf") <> 1 then return false
srcfile = filename

select count(*) into :ecnt from exchequer.payments where srcfile = :srcfile;
if sqlca.sqlcode = -1 then goto err_sql
if ecnt > 0 then
	qtxt = "Файл ~"" + srcfile + "~" вже був завантажений (" + s(ecnt) + " записів). Записи будуть замінені. Продовжувати завантаження ?"
	if not f_txt_quest2(qtxt) then goto err_end
	
	delete from exchequer.payments where srcfile = :srcfile;
	if sqlca.sqlcode = -1 then goto err_sql
end if


ds = f_ds_create("d_dbf_import")
dso = ds.object
ret = ds.importfile(pathname)
if ret <= 0 then goto err_100
	
for i = 1 to f_ds_colcount(ds)
	col = ds.describe("#" + s(i) + ".name")
	
	if not f_ds_coltype(ds, col) = "C" then continue
	for r = 1 to ds.rowcount()
		str = ds.getitemstring(r, col)
		str = f_str_dos2win(str)
		ds.setitem(r, col, str)
	next
next


if not f_ds_create2(dl1, "d_lookup_1", "") then goto err_end
if not f_ds_create2(dl2, "d_lookup_2", "") then goto err_end

ds2 = f_ds_create("d_payment_list")
dso2 = ds2.object
for r = 1 to ds.rowcount()
	if nn(dso.S[r]) <= 0 then continue
	
	n = ds2.insertrow(0)
	dso2.pay_zkpo[n] = dso.OKPO[r]
	dso2.pay_name[n] = dso.NAMK[r]
	dso2.pay_date[n] = dso.FDAT[r]
	dso2.pay_sum[n] = nn(dso.S[r]) / 100
	dso2.pay_text[n] = dso.NAZN[r]
	dso2.srcfile[n] = srcfile
	
	dat_period = u_pay_text_parser.parse_pay_text(dso.NAZN[r])
	
	
	pay_zkpo = trim(sn(dso2.pay_zkpo[n]))
	f = f_ds_find(dl1, "bal_zkpo='" + pay_zkpo + "'")
	if f > 0 then
		dso2.ident_bal_zkpo[n] = dl1.object.bal_zkpo[f]
		dso2.ident_bal_name[n] = dl1.object.bal_name[f]
	end if
	
	f = f_ds_find(dl2, "pay_zkpo='" + pay_zkpo + "'")
	if f > 0 then
		dso2.ident_bal_zkpo[n] = dl2.object.bal_zkpo[f]
		dso2.ident_bal_name[n] = dl2.object.bal_name[f]
	end if	
	
	
	if dso2.ident_bal_zkpo[n] <> "" and not isnull(dat_period) then
		rowstatus = "A"
	else
		rowstatus = "-"
	end if
	dso2.rowstatus[n] = rowstatus
	
	if ds2.update() = -1 then goto err_end	
	
	if not isnull(dat_period) then
		payment_id = dso2.payment_id[n]

		insert into exchequer.payments_period(payment_id, ident_period) values(:payment_id, :dat_period);
		if sqlca.sqlcode = -1 then goto err_sql
	end if
next
commit;

//if not f_ds_create2(ds3, "d_payment_list", "srcfile=" + dd(srcfile)) then goto err_end
//ds3.rowscopy(1, ds3.rowcount(), primary!, dw_1, 1, primary!)
//dw_1.setrow(1)



f_txt_info("Імпорт файлу завершився успішно. Завантажено записів: " + s(ds2.rowcount()))
destroy ds
destroy ds2
destroy ds3
destroy dl1
destroy dl2

import_srcfile = srcfile
return true
err_100:
	f_txt_err("Імпорт файлу ~"" + pathname + "~"повернув помилку " + s(ret))
	goto err_end
err_sql:
	f_txt_errsql()
	goto err_end
err_end:
	rollback;
	return false
	
	

end function
public subroutine trash_go ();long r
string qst, rowstatus

	//row
r = dw_1.getrow()
if r = 0 then return

	//quest
if sn(dw_1.object.rowstatus[r]) <> "0" then
	qst = "Змінити статус рядка на ~"сміття~" ?"
	rowstatus = "0"
else
	qst = "Прибрати статус ~"сміття~" з рядка ?"
	rowstatus = "-"
end if
if not f_txt_quest2(qst) then return

if rowstatus <> "" then
	dw_1.object.rowstatus[r] = rowstatus
end if
refresh_status(r)

//dw_1.deleterow(r)


//	//commit
////commit;
//return
////err_end:
//	rollback;
//	return
////err_1:
//	f_txt_err("Удалить можно только документ, владельцем которого Вы являетесь")
//	return
end subroutine

public subroutine refresh_status (long row);string rowstatus


rowstatus = f_dw_val(dw_1, row, "rowstatus")

if rowstatus = "0" then
	//nothig
else
	if sn(trim(f_dw_val(dw_1, row, "ident_bal_zkpo"))) <> "" and sn(trim(f_dw_val(dw_1, row, "ident_period_list"))) <> "" then
		dw_1.object.rowstatus[row] = "M"
	else
		dw_1.object.rowstatus[row] = "-"
	end if
end if

end subroutine

on w_payment_list.create
if this.MenuName = "m_payment_list" then this.MenuID = create m_payment_list
this.dw_1=create dw_1
this.Control[]={this.dw_1}
end on

on w_payment_list.destroy
if IsValid(MenuID) then destroy(MenuID)
destroy(this.dw_1)
end on

event open;init()
end event

event resize;f_wnd_fill(this, dw_1)
end event

event close;destroy ds_crit
end event

event closequery;if not sav() then return 1
end event

type dw_1 from datawindow within w_payment_list
event mousemove pbm_mousemove
event mouseup pbm_lbuttonup
int Y=4
int Width=3387
int Height=1616
int TabOrder=10
string DragIcon="C:\_AN\pictures\PLUS.ico"
string DataObject="d_payment_list"
boolean Border=false
boolean HScrollBar=true
boolean VScrollBar=true
boolean LiveScroll=true
end type

event clicked;setrow(row)

f_dw_multiselect_s(this, row_multiselect)


end event

event dberror;return f_dw_dberror(sqldbcode, sqlerrtext)


end event

event rbuttondown;if dwo.name = "select" then
	dw_1.setrow(row)
	menuid.PopMenu(w_main.PointerX(), w_main.PointerY())
else
	r_menu_payment_list menu
	menu = create r_menu_payment_list
	if dwo.name = "lorgan_nam" then
		menu.m_main.m_m_8b.visible = true
	end if
	f_dw_rmenu_a(this, row, dwo, menu)
	destroy menu	
end if
end event

event retrieveend;f_retrieve_end(this)

end event

event retrievestart;f_retrieve_start(this)

end event

event retrieverow;return f_retrieve_row(this, row)

end event

event sqlpreview;//db(sqlsyntax)
end event

event itemchanged;return itemchanged_func(row, dwo, data)
end event

event dragdrop;//long id_d, n, r, k, bbb
//string nam_d, orgnam, txt
//u_long_set rows
//datawindow dw
//w_obj_list wnd_1
//
//if dwo.name = "lorgan_nam" then
//	choose case source.getparent().classname()
//	case "w_obj_list"
//		wnd_1 = source.getparent()
//		dw = wnd_1.dw_1
//		if wnd_1.tab <> "organ" then return	
//		id_d = dw.object.id[dw.getrow()]
//		nam_d = dw.object.nam[dw.getrow()]
//	case else
//		return
//	end choose
//	
//	for r = 1 to dw_1.rowcount()
//		if dw_1.isselected(r) then
//			rows.add(r)
//		end if
//	next
//	
//	
//	dw_1.setredraw(false)
//	bbb = long(dw_1.Object.DataWindow.FirstRowOnPage)
//	
//	if rows.count = 0 then
//		dw_1.object.lorgan_id[row] = id_d
//		dw_1.object.lorgan_nam[row] = get_organ_nam(id_d)
//	else
//		orgnam = get_organ_nam(id_d)
//		txt = "Выделено записей: " + s(rows.count) + "~r~n"
//		txt += "Для всех этих записей будет установлена организация «" + orgnam + "» (ID=" + s(id_d) + ")" + "~r~n~r~n"
//		txt += "Подтверждаете изменения ?"
//		if not f_txt_quest(txt) then return
//			
//		for k = 1 to rows.count
//			r = rows.data[k]
//			
//			dw_1.object.lorgan_id[r] = id_d
//			dw_1.object.lorgan_nam[r] = get_organ_nam(id_d)
//		next
//	end if
//	
//	
//	//dp(bbb)
//	//dw_1.Object.DataWindow.FirstRowOnPage = bbb
//	dw_1.scrolltorow(bbb)
//	dw_1.setredraw(true)
//	
//	
//	
//end if
end event

event itemerror;if ignore_itemerror then
	ignore_itemerror = false
	return 1
end if
	
end event

