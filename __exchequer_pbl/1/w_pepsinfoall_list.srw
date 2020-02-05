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
string Title="Таблица PEP-лиц"
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

long		open_person_id

long		row_multiselect
end variables

forward prototypes
public subroutine init ()
public function boolean find ()
public subroutine del ()
public function integer itemchanged_func (long row, dwobject dwo, string data)
public function boolean sav ()
public subroutine reset ()
public subroutine add ()
end prototypes

public subroutine init ();long r
s_parm parm


	//parm
is_first = true	
setnull(open_person_id)
if f_parm(parm) then
	if classname(parm.p1) <> "any" then open_person_id = parm.p1
end if

	//ds_crit
ds_crit = create datastore
ds_crit.dataobject = "c_payment"
ds_crit.insertrow(0)
if not isnull(open_person_id) then
	ds_crit.object.person_id[1] = s(open_person_id)
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
end subroutine
public function boolean find ();long k, document_id
string wh, sql, docset
s_parm parm

	//sav
if not sav() then return false

	//wh
parm.p1 = ds_crit
parm.p2 = ""
parm.p3 = is_first = true and isnull(open_person_id) = false
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

public function integer itemchanged_func (long row, dwobject dwo, string data);long p_id, person_id
string person_nam

if dwo.name = "pepcat" then
	if sn(dw_1.object.pepcat[row]) <> "" then
		if not f_txt_quest2("Изменять категорию строки?") then 
			dw_1.post setcolumn("person_nam")
			return 2;
		end if
	end if
end if

if dwo.name = "person_id" then
	if sn(data) <> "" then
		person_id = long(data)

		setnull(p_id)
		select nam_person_f(person_id), person_id into :person_nam, :p_id from person where person_id = :person_id;
		if sqlca.sqlcode = -1 then signalerror(0, sn(sqlca.sqlerrtext))
		
		if isnull(p_id) then
			dw_1.post setcolumn("person_nam")
			post f_txt_err("Лицо с ID=" + s(person_id) + " не найдено")
		end if
		
		dw_1.object.person_nam[row] = person_nam
	end if
end if


return 0
end function

public function boolean sav ();long row, person_id, i
string sql
u_long_set person_set, updaterow_set
	
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
if dw_1.update() = -1 then goto err_end

////обновить для лиц (обновляем)
//for i = 1 to person_set.count
//	person_id = person_set.data[i]
//	if isnull(person_id) then continue
//	//db(person_id)
//	
//	sql = "exec payment_recalc " + dd(person_id)
//	if not f_sqlexec(sql) then return false
//next

	
	//end
commit;
return true
err_end:
	rollback;
	return false
end function
public subroutine reset ();f_dw_reset(dw_1)
end subroutine

public subroutine add ();long row, p_id
string person_nam

row = f_dw_add(dw_1)
if not isnull(open_person_id) then
	dw_1.object.person_id[row] = open_person_id
	
		select nam_person_f(person_id), person_id into :person_nam, :p_id from person where person_id = :open_person_id;
		if sqlca.sqlcode = -1 then signalerror(0, sn(sqlca.sqlerrtext))
	
		if not isnull(p_id) then
			dw_1.object.person_nam[row] = person_nam
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
