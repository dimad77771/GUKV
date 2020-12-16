Release Notes for PowerBuilder[TM] Version 6.5 (Wintel)

(c) 1991-1998 Sybase, Inc. and its subsidiaries. All rights reserved.
Updated 7/20/98

************************************************************************
If you are viewing this file in Wordpad, please use the word-wrap option
************************************************************************

Thank you for choosing PowerBuilder as your development tool!  Please read this document to learn about last minute updates to the product and documentation.
 
************************************************************************
BEFORE INSTALLING:
MAKE SURE THAT THE SQLANYWHERE DATABASE IS NOT RUNNING
************************************************************************

Section I:    General Information
Section II:   PowerBuilder Documentation Addendum

===============================
Section I: General Information
===============================

System Requirements
-------------------

Windows
- 486 PC or higher
- 16 MB RAM (24 recommended)
- 45 MB hard drive space, depending on configuration
- CD ROM drive
- VGA monitor
- Windows 95 or Windows NT 3.51 or higher for development
- Windows 95, Windows NT, or Window 3.x  for deployment

Support Statements (this information also appears in the PowerStudio readme)
------------------
* 16-bit Applications not supported on 32-bit platform

Since the PowerBuilder development environment can produce applications for 16 or 32-bit native deployment on Microsoft Windows, the applications must be deployed to the corresponding Windows platform. 16-bit applications are only supported on Microsoft Windows 3.x

* Termination or elimination of 16-bit deployment capabilities

Sybase is committed to supplying application development tools for strategic enterprise platforms. In order to provide our customers with the highest quality products and the most up to date functionality, we will continue to focus our efforts on the mainstream market. PowerBuilder 6.x is the last release to support 16-bit deployment on Windows 3.1. Starting with the next major release the development environment will only produce 32-bit executables (p-code or machine) for Microsoft Windows.

* Limited Win-OS2 support

PowerBuilder is built and tested on several standard operating systems, Microsoft Windows, Sun Solaris, HP-UX, and IBM AIX. While our development and testing process focuses on these operating systems as the primary development and deployment platforms, Sybase will make best attempts to resolve or work around any compatibility problems encountered while running PowerBuilder applications with IBM Win-OS2 provided the issue is reproducible under Microsoft Windows as well. As with any bug, it must be reproduced and documented so that it can be assigned a case number.

* Limited Exceed PC X-Server support

PowerBuilder is built and tested on several standard operating systems, Microsoft Windows, Sun Solaris, HP-UX, and IBM AIX. While our development and testing process focuses on these operating systems as the primary development and deployment platforms, Sybase will make best attempts to resolve or work around any compatibility problems specific to PowerBuilder applications encountered while running an Exceed v6.0 PC X-Server, provided it does not adversely affect cross-platform consistency or capabilities, including all Enterprise development and deployment aspects of PowerBuilder. Other PC X-Servers would be addressed only if the issue is reproducible on the platform's primary graphics head. As with any bug, it must be reproduced and documented so that it can be assigned a case number.

* Save and close all applications before uninstalling ADT

Uninstalling the Advanced Developer Toolkit (ADT) without first uninstalling PowerBuilder will cause Windows NT to crash.  Please be sure to save and close all applications before performing an uninstall procedure.  Alternatively, if you uninstall PowerBuilder first (before uninstalling the ADT), Windows NT will not crash.

* Preferred Image Editor not configured for InfoMaker

After InfoMaker is installed the icon in the InfoMaker Toolbar for the preferred picture editor does not function correctly.  To correct this problem, edit the pictview.ini file to include the following statement in the Editor section.  
	[Editor]
	program=C:\Program Files\Sybase\ArtGal\wimgedit.exe

* No default pbl file for InfoMaker during Custom installation

If you do not install the InfoMaker tutorial during a Custom installation, there is no default .pbl file for InfoMaker.  During the initial launch, Infomaker will prompt you with a default .pbl file name.   You should select "No" to the default, and then enter any suitable name for the .pbl file.  If you click "Yes", the program will close, and you will be prompted again when you launch InfoMaker the next time. 

* Profile feature sample out of date

The sample file (profiler.pbp) offered with the profile feature is out of date, and will thus give an error message.  Any source file created by the user will work correctly and can be used instead of the sample file.

(end of information in PowerStudio readme)

Generators
----------
The PowerBuilder COM Object Generator and the PowerBuilder JavaBean Proxy Generator are included on this CD, but must be installed separately. The JavaBean Proxy Generator requires the ObjectSpace JGL 3.1.0 library and JDK 1.1.6 

Connecting to DB2
-----------------
We now include an Intersolv driver for connecting to DB2.  This is available on all platforms.  Detailed information is available in Online Help.

Database Information
--------------------
SQLAnywhere 5.5.04 is installed for Windows 32-bit platforms.

Note when installing SQL Anywhere and Object Cycle, two entries are made in the ODBC manager:
  Sybase SQL Anywhere 5.0  5.05.041867  WOD50T.DLL
  Sybase SQL Anywhere 5.5  5.05.041867  WOD50T.DLL
They are exactly the same, except for the name. As a result, two entries will appear in the ODBC administrator. Although they both work correctly, third party products which install databases (like Riverton HOW) will register the data source name with the more recent version.

Oracle 8 Driver - PowerBuilder 6.0 has an option to install the Oracle 8 native database interface.  For further information, see online help.  

DirectConnect
-------------
PBDIR60.DLL will not connect due to problems identified after the final PowerBuilder 6.5 build.  The fixes for the DirectConnect driver will be posted on http://support.sybase.com.  To install the fix, rename the file installed by PowerBuilder 6.5, download the file PBDIR60.DLL, and replace the file installed by PowerBuilder 6.5.

The PowerBuilder DirectConnect interface will ONLY support connections to a DB2/MVS database via the DirectConnect for MVS access service Version 11.1.1 P4 or higher. At this time the PowerBuilder DirectConnect interface is NOT intended to support connections to the Transaction Router Service (TRS) or any other service on the DirectConnect server. Additional services will be supported in PowerBuilder 7.0.

DataWindow Synchronization
--------------------------
The set of DataWindow Synchronization features which deal with multiple sources and targets as described in the documentation is not supported in this release. The supported configurations are: 
- single source, single target for update
- single source, multiple target for read-only (broadcast)

The return value of the DataWindow Synchronization GetChanges() function is not reliable when used with the optional cookie parameter.

Distributed PowerBuilder Components
-----------------------------------
If you used Distributed PowerBuilder with PowerBuilder 5.0, you will notice a couple of changes.  Most Distributed PowerBuilder functionality has been incorporated into the deployment DLLs; therefore, when you install PowerBuilder on a client machine, you have the software necessary to use Distributed PowerBuilder from that client. 

Also, we found that few people used the Open Client/Open Server driver with Distributed PB, so have dropped that driver for PowerBuilder 6.0. We continue to support Winsock and Named Pipes drivers.

Fewer DLLs /Libraries
---------------------
There are fewer DLLs (libraries on non-Wintel platforms) than there used to be for PowerBuilder. Functionality has been combined, and you can now deploy an application with as few as 1 DLL (PBVM60.DLL), but realistically, you'll probably use three ... PBVM60.DLL, PBDWE60.DLL (for DataWindow functionality) and one DLL for whichever database you are using.

Installing on NT4.0
-------------------
You must have Administrator privileges to fully install this product on NT 4.0.

InstallShield
-------------
InstallShield 5.0 Free is included with PowerBuilder for Windows.  IS5Free can be used to build installable files for your PowerBuilder applications.  

Internet Explorer 4.0
---------------------
At the time PB 6.0 was released, Microsoft IE 4.0 had just been released and some compatibility issues with PowerBuilder 6.0 were identified, but it was too late to correct them. Those issues have been resolved in PB 6.5.

Migration Information
---------------------
Please read the Technical Migration Document in the \Support directory on the CD to see what behavior changes you can expect to see in PowerBuilder 6.0. This document is updated with every release candidate, and also includes information on migrating from PowerBuilder 4.0.

Online Books
------------
Online Books are now available over the web!  The URL for the DynaWeb version of the PB6 and IM6 online books is: http://calas.sybase.com.
This URL is also available under the PowerBuilder and InfoMaker Help menus.

OLE Synchronization DLLs
------------------------
Some customers have been experiencing OLE problems due to mis-matched OLE DLLs.  OLE32.DLL must be dated 1/26/97 - if you have an older version, you will experience problems.  To get the update, you may have to reinstall the OS and upgrade with appropriate MS OS service packs.  Also the DLL rpcrt4.dll must be kept in sync with ole32.dll.

Third Party Vendors using ORCA
------------------------------
The following files may be found in the ORCA folder on the CD:

		pbtapi.h
		pborca.h
		pborca.lib


Silent Install
--------------
On the Windows platform, you can create a "silent" install for PowerBuilder - one which can be set up and run without user intervention.  This is handy for system administrators.  See the documentation (Deploying your Application in Application Techniques) for further information.

DataWindow Scrollbar Operation
------------------------------
When scrolling through certain groups, two of the same Group Header with subsets of the group, re-displayed on the screen. The scrollbar operation for DataWindows was changed in 6.0.  The virtual scrolling enhancement allows any row to be at the top of a screen view (even in a group dw), and print preview scrolling to scroll from the top of the report to the bottom of the report, all in one slide.  This means that it is possible to scroll to a point that shows the bottom half of one page and the top half of the next page.  If the arrow keys or the page up/down keys are used exclusively, then the issue does not arise.  The scrollbar enhancement was added to override the navigation keys in browser context.

PBWEB.INI Cannot be found
-------------------------
The message "c:\Windows\PBWEB.INI" cannot be found is caused by an incorrect file location. The PBWEB.INI should be moved from the ..\sybase\pb6\it directory to your system directory (e.g. c:\windows).

Application path for sample 
---------------------------
The application path for the niexample is incorrect. Use the application to correct the pbl name. The correct name is niexdpbs.pbl (typically located in ..\sybase\pb6\appgal\javasamp).

Localized Deployment Kits
-------------------------
The localized deployment kits cannot be installed directly from the PowerBuilder 6.5 install. You will need to navigate to the Ldddk16 or Ldddk32 folder on the PowerBuilder 6.5 CD and run setup.exe from there.

On a 16-bit machine, the English 16-bit deployment kit must be installed from setup.exe in the Dddk16 folder on the PowerBuilder 6.5 CD. 


==================================================
Section II:    PowerBuilder Documentation Addendum
==================================================

This section contains information and known Documentation errors in the books shipped with PowerBuilder 6.0.x

Building Internet Applications with PowerBuilder
------------------------------------------------
Chapter 9 of the Building Internet Applications with PowerBuilder 
manual incorrectly states that Microsoft Internet Explorer requires
the NCompass ScriptActive plug-in. It is Netscape Navigator that
requires the NCompass ScriptActive plug-in.

This has been fixed in the online Help and the online books but is incorrect in the hardcopy book.

Chapter 9 of the Building Internet Applications with PowerBuilder 
manual provides an incorrect list of modules required by the 
PowerBuilder window ActiveX. The correct list is:
   - mfc42.dll
   - msvc42.dll
   - url.dll
   - urlmon.dll
This has been fixed in the online Help and the online books but is incorrect in the hardcopy book.

Also, to use VBScript with the PowerBuilder window ActiveX, you need
Version 2.0 (or higher) of vbscript.dll.

PowerBuilder 6.0 User's Guide
-----------------------------
Chapter 18 of the PB 6.0 User's Guide provides inaccurate information about deploying an application to a user's machine.  It says: "Setup installs the components you selected. By default, Setup installs system DLLs in the appropriate system directory for your operating system, prompting you for an action if an update fails".  

In the case of the deployment kit, we install the components under a directory called DDDK6 so that they are easily identifiable, rather than to the system directory.

PFC Object Reference
--------------------
The following objects were not documented in the PFC Object Reference:
   - n_cst_mru
   - n_cst_tmgmultiple
   - n_cst_tmgsingle
   - n_cst_linkedlistbase
   - n_cst_linkedlistnode
   - n_cst_linkedlistnodecompare
   - n_cst_list
   - n_cst_nodecomparebase
   - n_cst_nodebase
   - n_cst_queue
   - n_cst_tree
   - n_cst_treenode
   - n_cst_treenodecompare

These objects are documented in the online Help and on the Powersoft Documentation web site. 

In the printed PFC Object Reference, the function and event lists for the following objects are incorrect:
   - u_lvs
   - n_cst_lvsrv_datasource
   - n_cst_tvsrv_levelsource

The function and event lists in the online Help and on the Powersoft Documentation web site are correct.
**********************************************************************
(c) 1991-1998 Sybase, Inc. and its subsidiaries. All rights reserved. Sybase, Inc. and its subsidiaries ("Sybase") claim copyright in this Program and documentation as an unpublished work, versions of which were first licensed on the date indicated in the foregoing notice. Claim of copyright does not imply waiver of Sybase's other rights. See Notice of Proprietary Rights.

NOTICE OF PROPRIETARY RIGHTS
This computer program and documentation are confidential trade secrets and the property of Sybase, Inc. and its subsidiaries. Use, examination, reproduction, copying, disassembly, decompilation, transfer and/or disclosure to others, in whole or in part, are strictly prohibited except with the express prior written consent of Sybase, Inc. and its subsidiaries.