$PBExportHeader$u_usropt_datawindow_column_width.sru
forward
global type u_usropt_datawindow_column_width from nonvisualobject
end type
end forward

global type u_usropt_datawindow_column_width from nonvisualobject autoinstantiate
end type

forward prototypes
public subroutine sav (datawindow dw, string context)
public subroutine sav (datawindow dw)
public subroutine load (datawindow dw, string context, boolean use_posx)
public subroutine load (datawindow dw, string context)
end prototypes

public subroutine sav (datawindow dw, string context);long n, i
string colcod, dwname
long width, posx
u_string_set columns
u_dw u_dw
datastore ds


ds = f_ds_create("d_usropt_datawindow_column_width")

dwname = dw.dataobject
columns = u_dw.g_column(dw)
for i = 1 to columns.count
	colcod = columns.data[i]
	width = long(dw.describe(colcod + ".width"))
	posx = long(dw.describe(colcod + ".X"))
	
	n = ds.insertrow(0)
	ds.object.usr[n] = g_uid
	ds.object.dwname[n] = dwname
	ds.object.context[n] = context
	ds.object.colcod[n] = colcod
	ds.object.width[n] = width
	ds.object.posx[n] = posx
next

delete from usropt_datawindow_column_width where usr = :g_uid and dwname = :dwname and context = :context;
if sqlca.sqlcode = -1 then signalerror(0, sn(sqlca.sqlerrtext))

if ds.update() = -1 then signalerror(0, "")
commit;

destroy ds
end subroutine

public subroutine sav (datawindow dw);sav(dw, "")
end subroutine

public subroutine load (datawindow dw, string context, boolean use_posx);string colcod, dwname, wh, err
long i, width, r, posx
u_string_set columns
u_dw u_dw
datastore ds


dwname = dw.dataobject
wh = "usr=" + dd(g_uid) + " and dwname=" + dd(dwname) + " and context=" + dd(context)
if not f_ds_create2(ds, "d_usropt_datawindow_column_width", wh) then signalerror(0,"")
ds.setsort("posX A")
ds.sort()

columns = u_dw.g_column(dw)
for r = 1 to ds.rowcount()
	colcod = ds.object.colcod[r]
	i = columns.find(colcod)
	if i <= 0 then continue
	
	width = ds.getitemnumber(r, "width")
	posx = ds.getitemnumber(r, "posx")
	
	if width > 0 then
		err = dw.modify(colcod + ".width=" + s(width))
		if err <> "" then signalerror(0, err)
	end if
	
	if posx > 0 and use_posx then
		err = dw.modify(colcod + ".X=" + s(posx))
		if err <> "" then signalerror(0, err)		
	end if	
next



destroy ds
end subroutine
public subroutine load (datawindow dw, string context);load(dw, context, false)
end subroutine

on u_usropt_datawindow_column_width.create
TriggerEvent( this, "constructor" )
end on

on u_usropt_datawindow_column_width.destroy
TriggerEvent( this, "destructor" )
end on

