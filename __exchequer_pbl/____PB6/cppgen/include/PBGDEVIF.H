//              Copyright Sybase, Inc. 1996
//
//      Sybase, Inc. ("Sybase") claims copyright in this
//      program and documentation as an unpublished work, versions of
//      which were first licensed on the date indicated in the foregoing
//      notice.  Claim of copyright does not imply waiver of Sybase's
//      other rights.
//-----------------------------------------------------------------------------
//      pbgdevif.h
//      PowerBuilder development time interface declarations.
//      wrlane, dsotkotwitz     3/4/97
//
//      The interfaces declared here are provided by PowerBuilder to 
//      component technology generators for use at development time.
//
//-----------------------------------------------------------------------------

#ifndef PBGDEVIF_H
#define PBGDEVIF_H
 
#ifndef PBGTYPES_H
#include "pbgtypes.h"
#endif
#ifndef PBGIUNIF_H
#include "pbgiunif.h"
#endif
#ifndef PBGRUNIF_H
#include "pbgrunif.h"
#endif


extern "C"
{
extern PBIID PBIBuildServices_IID;
extern PBIID PBIBuildError_IID;
extern PBIID PBIClassNameIterator_IID;
extern PBIID PBIGeneratorPersist_IID;
extern PBIID PBIGeneratorHost_IID;
};


//      forward declarations for convenience.

class PBIBuildServices;
class PBIBuildError;
class PBIClassNameIterator;
class PBIGeneratorPersist;
class PBIGeneratorHost;


//-----------------------------------------------------------------------------
//			PBIBuildServices
//
//	PBIBuildServices is the interface through which a variety of compiling,
//	linking, and other deployment services are provided to the generators
//	and other clients.  Call QueryInterface on PBIBuildServices to get
//	interfaces on particular tools;  the specific interface(s) available
//	from a PBIBuildService will depend on the actual tools that are
//	encapsulated by the interface.  The toolset itself is indicated by
//	the class ID that is used to create the PBIBuildServices interface.
//
//-----------------------------------------------------------------------------

class PBIBuildServices  :       public PBIUnknown
{
};

extern "C"
{
PBWINAPI_(PBRESULT)     CreatePBIBuildServices( 
	PBISession * pSession,
	PBCLSIDREF idBuildServicesCLSID,
	PBIBuildServices ** ppServices );
};

 
//-----------------------------------------------------------------------------
//                                              PBIBuildError
//-----------------------------------------------------------------------------

class PBIBuildError     :       public PBIUnknown
{
  public:

	virtual PBRESULT        GetErrorMessage( PBIString ** ) = 0;
};

extern "C"
{
PBWINAPI_(PBRESULT) CreatePBIBuildError(
	PBIString * pErrorMessage,
	PBISession * pSession,
	PBIBuildError ** ppError );
};


//-----------------------------------------------------------------------------
//                      PBIClassNameIterator 
//
//      PBIClassNameIterator is an iterator type for PowerBuilder class
//      types.  Class types are represented by name, captured as a 
//      instance of PBString.  A simple iteration interface is provided:
//
//      o       GetFirst
//              Get the first class name in the list.
//
//      o       GetNext
//              Get the next class name in the list.  If the end of 
//              the list is achieved, the class name will be null,
//              and an error value will be returned.
//
//      o       GetCount
//              Returns the number of classes in the list.
//
//      o       Clone
//              Returns a copy of the current iterator.  Used to capture
//              and store the current item pointer of the current iterator.
//                      
//      These functions are declared here as pure virtual functions.  They
//      must be implemented by derived iterator types.
//      
//-----------------------------------------------------------------------------

//      TODO:  Move to IEnumXXX style interface. wrl 3/11/97

class PBIString;

class PBIClassNameIterator  :  public PBIUnknown
{
  public:

    virtual PBRESULT    GetFirst( PBIString ** pString ) = 0;
    virtual PBRESULT    GetNext( PBIString ** pString ) = 0;
    virtual PBRESULT    GetCount( PBULONG * pCount ) = 0;
    virtual PBRESULT    Clone( PBIClassNameIterator ** ppIterator ) = 0;
};

extern "C"
{
PBWINAPI_(PBRESULT) CreatePBIClassNameIterator(
	PBIString * pType,
	PBIString * pLibrary,
	PBIClassNameIterator ** ppIterator );
};


//-----------------------------------------------------------------------------
//                      PBGeneratorPersist 
//
//      PBGeneratorPersist is an interface provided by PowerBuilder for use
//      by component technology generators.  It gives the generators access
//      to PowerBuilders data storage facility, allowing generator state to
//      be read and written to persistent storage.  
//
//      PBGeneratorPersist is based on the OLE IStream interface.
//
//-----------------------------------------------------------------------------


class PBIGeneratorPersist : public PBIUnknown
{
  public:

    virtual PBRESULT    Read( 
		PBPVOID         pvBuffer,
		PBULONG         ulBufferSize,
		PBULONG *       pulBytesRead ) = 0;
    virtual PBRESULT    Write( 
		PBPVOID         pvBuffer,
		PBULONG         ulBufferSize,
		PBULONG *       pulBytesWritten ) = 0;
    virtual PBRESULT    Seek( 
		PBLONG          lOffset,
		PBDWORD         ulOrigin,
		PBULONG *       pulNewPosition ) = 0;   

    virtual PBRESULT    GetSize( PBULONG * pulSize ) = 0;
};


//-----------------------------------------------------------------------------
//                      PBIGeneratorHost 
//
//      PBIGeneratorHost is an interface provided by PowerBuilder for use
//      by component technology generators.  It gives generators access
//      to general services provided by the PowerBuilder environment.
//
//      The interface includes:
//
//      o       OnGenerationStart
//              Called by generators when they are about to begin component 
//              generation.  Allows PowerBuilder to put up a "cancel" dialog 
//              and take any other appropriate actions.
//
//      o       OnGenerationComplete
//              Called by generators when component generation is complete. 
//              Allows PowerBuilder to take down the "cancel" dialog and take 
//              any other appropriate actions.
//
//      o       ShowGenerationStatus.
//              Called by generators at appropriate points to allow 
//              PowerBuilder to host the display of the generator status.  
//              Generator status is obtained by calling the generator's 
//              GetGeneratorStatus method.
//              
//
//-----------------------------------------------------------------------------

class PBIGenerator;

class PBIGeneratorHost : public PBIUnknown
{
  public:

	virtual PBRESULT	OnGenerationComplete( PBIGenerator * pGenerator ) = 0;
	virtual PBRESULT	OnGenerationStart( PBIGenerator * pGenerator ) = 0;
	virtual PBRESULT	ShowGenerationStatus( PBIGenerator * pGenerator ) = 0;
	virtual	PBRESULT	UpdateHostStatus( PBIString * pStatus ) = 0;
	virtual PBRESULT	DidUserAbort( PBBOOL * bUserAbort ) = 0;
	virtual PBRESULT	GetSession( PBISession ** ppSession ) = 0;
};


#endif  //  PBGDEVIF_H
