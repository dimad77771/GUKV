$PBExportHeader$w_crit_payment.srw
forward
global type w_crit_payment from Window
end type
type dw_1 from datawindow within w_crit_payment
end type
type dw_e from datawindow within w_crit_payment
end type
type cb_3 from commandbutton within w_crit_payment
end type
type cb_2 from commandbutton within w_crit_payment
end type
type cb_1 from commandbutton within w_crit_payment
end type
end forward

global type w_crit_payment from Window
int X=416
int Y=424
int Width=2423
int Height=1144
boolean TitleBar=true
string Title="Критерий для поиска"
long BackColor=79741120
boolean ControlMenu=true
WindowType WindowType=response!
dw_1 dw_1
dw_e dw_e
cb_3 cb_3
cb_2 cb_2
cb_1 cb_1
end type
global w_crit_payment w_crit_payment

type variables
datastore		ds_crit

string		pf
string		txt_dwdropdown

end variables

forward prototypes
public subroutine init ()
public subroutine ok ()
public subroutine pick_val (long row, string col)
end prototypes

public subroutine init ();boolean isauto
u_dw u_dw
s_parm parm

	//параметры
parm = message.powerobjectparm
ds_crit = parm.p1
pf = parm.p2
isauto = parm.p3

	//init
dw_1.settransobject(sqlca)
dw_1.insertrow(0)
u_dw.copy(dw_1, ds_crit)

	//center
f_wnd_center(this)

	//isauto
if isauto then
	post ok()
end if
end subroutine
public subroutine ok ();long i, k
string val, col, wh_v, typ, wh
u_string_set vals
s_parm parm
dwobject dwo
u_dw u_dw

	//dwo
dwo = dw_1.object
if dw_1.accepttext() = -1 then return
wh = "1=1"

	//анкета
for i= 1 to f_dw_colcount(dw_1)
	col = f_dw_colcod(dw_1, i)
	
	val = trim(sn(dw_1.getitemstring(1, col)))
	if val = "" then continue
	
	if val = "-" then
		if typ = "C" or col = "pepcat" then
			wh_v = "isnull(" + col + ",'')=''"
		else
			wh_v = col + " is null"
		end if
	else	
		if in(col, {"person_id"}) then
			typ = "N"
		elseif in(col, {"pepdb", "pepde", "pepconf"}) then
			typ = "D"
		else
			typ = "C"
		end if
	
		if col = "pepcat" then
			vals = f_str_parse(f_str_replace(val, "~r~n", ""), ";")
			wh_v = ""
			for k = 1 to vals.count
				wh_v += iif(wh_v="","",",") + dd(vals.data[k])
			next
			wh_v = col + " in (" + wh_v + ")"
		//elseif col = "person_nam" then
			
		else
			if not f_str_where(val, typ, pf + col, wh_v) then goto err_100
		end if
	end if
	wh += " and " + wh_v
next

u_dw.copy(ds_crit, dw_1)
parm.p1 = wh
//dp(wh)
closewithreturn(this, parm)

return
err_100:
	f_dw_focus_set(dw_1, 1, col)
	f_txt_err(wh_v)
	return

	
end subroutine

public subroutine pick_val (long row, string col);//s_parm parm
//
//if col = "click_pepcat" then
//	parm.p1 = sn(dw_1.object.pepcat[1])
//	openwithparm(w_crit_payment_pepcat, parm)
//	if not f_parm(parm) then return
//		
//	dw_1.object.pepcat[1] = parm.p1
//end if
//
end subroutine
on w_crit_payment.create
this.dw_1=create dw_1
this.dw_e=create dw_e
this.cb_3=create cb_3
this.cb_2=create cb_2
this.cb_1=create cb_1
this.Control[]={this.dw_1,&
this.dw_e,&
this.cb_3,&
this.cb_2,&
this.cb_1}
end on

on w_crit_payment.destroy
destroy(this.dw_1)
destroy(this.dw_e)
destroy(this.cb_3)
destroy(this.cb_2)
destroy(this.cb_1)
end on

event open;init()
end event

type dw_1 from datawindow within w_crit_payment
event dwndropdown pbm_dwndropdown
int Width=1984
int Height=1060
int TabOrder=10
string DataObject="c_payment"
boolean Border=false
end type

event dwndropdown;string str

txt_dwdropdown = ""

str = this.gettext()
if SelectedStart() = len(str) + 1 then txt_dwdropdown = str 


end event

event dberror;return f_dw_dberror(sqldbcode, sqlerrtext)

end event

event itemchanged;if dwo.name = "pepcat" then
	txt_dwdropdown = dw_1.object.pepcat[row]
	if txt_dwdropdown <> "" then
		accepttext()
		post setitem(row, dwo.name, txt_dwdropdown + ";~r~n" + data)
	end if
end if

end event

event itemerror;return f_dw_itemerror()
end event

event rbuttondown;f_dw_rmenu(this, row, dwo)


end event

event buttonclicked;pick_val(row, dwo.name)
end event

type dw_e from datawindow within w_crit_payment
int X=2560
int Y=608
int Width=297
int Height=396
int TabOrder=50
boolean Visible=false
boolean LiveScroll=true
end type

type cb_3 from commandbutton within w_crit_payment
int X=1998
int Y=128
int Width=366
int Height=92
int TabOrder=30
string Text="Отмена"
boolean Cancel=true
int TextSize=-8
int Weight=400
string FaceName="MS Sans Serif"
FontFamily FontFamily=Swiss!
FontPitch FontPitch=Variable!
end type

event clicked;close(parent)
end event

type cb_2 from commandbutton within w_crit_payment
int X=1998
int Y=16
int Width=366
int Height=92
int TabOrder=20
string Text="ОК"
boolean Default=true
int TextSize=-8
int Weight=400
string FaceName="MS Sans Serif"
FontFamily FontFamily=Swiss!
FontPitch FontPitch=Variable!
end type

event clicked;ok()
end event

type cb_1 from commandbutton within w_crit_payment
int X=1998
int Y=328
int Width=366
int Height=92
int TabOrder=40
string Text="Очистить"
int TextSize=-8
int Weight=400
string FaceName="MS Sans Serif"
FontFamily FontFamily=Swiss!
FontPitch FontPitch=Variable!
end type

event clicked;dw_1.reset()
dw_1.insertrow(0)


end event

