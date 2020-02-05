$PBExportHeader$w_main.srw
$PBExportComments$Главное окно
forward
global type w_main from Window
end type
type mdi_1 from mdiclient within w_main
end type
end forward

global type w_main from Window
int X=370
int Y=260
int Width=2917
int Height=1856
boolean TitleBar=true
string Title="АСАР"
string MenuName="m_main"
long BackColor=79741120
boolean ControlMenu=true
boolean MinBox=true
boolean MaxBox=true
boolean Resizable=true
WindowState WindowState=maximized!
WindowType WindowType=mdi!
event custom01 pbm_custom01
mdi_1 mdi_1
end type
global w_main w_main

type prototypes
function long mem_copy(ulong hnd, ref string ret, ref long debug ) library "C:\_mp\dll\PBMyOle\Debug\PBMyOle.dll"


end prototypes

type variables




end variables

forward prototypes
public subroutine init ()
public subroutine etc_func ()
end prototypes

public subroutine init ();long i, cl, ck
string val
menu m[]
m_main menu 

//g_isadm = false

	//f_version
post f_version()

	//почистиь
f_clear_tmpdir()

	//title
//title = g_basenam

//	//login
//g_pwd = ""
//if g_release = 1 then
//	if gu_options.has_usr() then
//		open(w_login)
//		if message.doubleparm <> 1 then halt
//	end if
//end if



	//etc_func
etc_func()
	
//timer(1)

return
//err_sql:
	return
	
end subroutine
public subroutine etc_func ();open(w_tmp_web, w_main)
end subroutine

on w_main.create
if this.MenuName = "m_main" then this.MenuID = create m_main
this.mdi_1=create mdi_1
this.Control[]={this.mdi_1}
end on

on w_main.destroy
if IsValid(MenuID) then destroy(MenuID)
destroy(this.mdi_1)
end on

event open;init()
end event

event close;	//почистить каталог
if not g_debug then
	f_clear_tmpdir()
end if
if g_debug then HALT
end event

event timer;//ulong hnd
//u_external_func ue
//
//dfile("111")
//hnd = ue.FindWindowExA(0, 0, "#32770", "About")
//dfile(hnd)
//dfile(ue.DestroyWindow(hnd))
//
//
end event

event closequery;if g_debug = false then
	if not f_txt_quest("Выходить из программы ?") then return 1
end if
end event

type mdi_1 from mdiclient within w_main
long BackColor=276856960
end type

