//              Copyright Sybase, Inc. 1996
//
//      Sybase, Inc. ("Sybase") claims copyright in this
//      program and documentation as an unpublished work, versions of
//      which were first licensed on the date indicated in the foregoing
//      notice.  Claim of copyright does not imply waiver of Sybase's
//      other rights.
//
//-----------------------------------------------------------------------------
//
//  pbgcpprt.h
//
//  C++ wrapper classes for the public ("PBI") runtime interfaces.
//
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	File scope globals
//------------------------------------------------------------------------------

const PBCHAR * PBCPP_ERR_ALLOCATION_FAILED =
	PBTEXT("Allocation failed");
const PBCHAR * PBCPP_ERR_INVALID_OBJECT =	
	PBTEXT("Invalid object");
const PBCHAR * PBCPP_ERR_INVALID_ARGUMENT =
	PBTEXT("Invalid argument");
const PBCHAR * PBCPP_ERR_PROPERTY_NAME_INTERFACE_CREATE_FAILED = 
	PBTEXT("Property name interface create failed");
const PBCHAR * PBCPP_ERR_PROPERTY_VALUE_INTERFACE_CREATE_FAILED = 
	PBTEXT("Property value interface create failed");
const PBCHAR * PBCPP_ERR_METHOD_NAME_INTERFACE_CREATE_FAILED =
	PBTEXT("Method name interface create failed");
const PBCHAR * PBCPP_ERR_RETURN_VALUE_INTERFACE_CREATE_FAILED =
	PBTEXT("Method return value interface create failed");
const PBCHAR * PBCPP_ERR_ARGUMENT_VALUE_INTERFACE_CREATE_FAILED =
	PBTEXT("Method argument value interface create failed");
const PBCHAR * PBCPP_ERR_ARGUMENT_ASSIGNMENT_FAILED =
	PBTEXT("Method argument assignment failed");
const PBCHAR * PBCPP_ERR_SESSION_FETCH_FAILED =
	PBTEXT("Session fetch failed");
const PBCHAR * PBCPP_ERR_PROPERTY_ASSIGNMENT_FAILED =
	PBTEXT("Property assignment failed");
const PBCHAR * PBCPP_ERR_PROPERTY_FETCH_FAILED =
	PBTEXT("Property fetch failed");
const PBCHAR * PBCPP_ERR_PROPERTY_VALUE_FETCH_FAILED =
	PBTEXT("Property value fetch failed");
const PBCHAR * PBCPP_ERR_PROPERTY_VALUE_ASSIGNMENT_FAILED =
	PBTEXT("Property value assignment failed");
const PBCHAR * PBCPP_ERR_RETURN_VALUE_ASSIGNMENT_FAILED =
	PBTEXT("Return value assignment failed");
const PBCHAR * PBCPP_ERR_ARGUMENT_INTERFACE_CREATE_FAILED =
	PBTEXT("Method argument interface create failed");
const PBCHAR * PBCPP_ERR_DO_METHOD_FAILED =
	PBTEXT("Method execution failed");
const PBCHAR * PBCPP_ERR_RETURN_VALUE_FETCH_FAILED =
	PBTEXT("Method return value fetch failed");
const PBCHAR * PBCPP_ERR_ARGUMENT_VALUE_ASSIGNMENT_FAILED =
	PBTEXT("Method argument value assignment failed");
const PBCHAR * PBCPP_ERR_REF_ARG_VALUE_FETCH_FAILED =
	PBTEXT("Method reference argument value fetch failed");
const PBCHAR * PBCPP_ERR_REF_ARG_VALUE_ASSIGNMENT_FAILED =
	PBTEXT("Method reference argument return value assignment failed");

PBSession GetMasterSession( )
{
	static PBSession PBCPP_MASTER_SESSION;
	return PBCPP_MASTER_SESSION;
}


//------------------------------------------------------------------------------
//	PBUnknown member function implementations
//------------------------------------------------------------------------------

PBUnknown::PBUnknown( ) :
	m_pInterface(NULL)
{
}

//------------------------------------------------------------------------------

PBUnknown::PBUnknown( PBIUnknown * pInterface ) :
	m_pInterface(pInterface)
{
	if (m_pInterface)	m_pInterface->AddRef( );
}

//------------------------------------------------------------------------------

PBUnknown::PBUnknown( const PBUnknown & other ) :
	m_pInterface(other.m_pInterface)
{
	if (m_pInterface)	m_pInterface->AddRef( );
}

//------------------------------------------------------------------------------

PBUnknown::~PBUnknown( )
{
	if (m_pInterface)	m_pInterface->Release( );
}

//------------------------------------------------------------------------------

PBUnknown & PBUnknown::operator = ( const PBUnknown & other )
{
	if (this != &other)
	{
		SetInterface(other.m_pInterface);
	}

	return *this;
}

//------------------------------------------------------------------------------

PBBOOL PBUnknown::SetInterfaceOn( PBUnknown & other, PBIUnknown * pInterface )
{
	return other.SetInterface(pInterface);
}

//------------------------------------------------------------------------------

PBBOOL PBUnknown::SetInterface( PBIUnknown * pInterface )
{
	if (m_pInterface)	m_pInterface->Release( );
	m_pInterface = pInterface;
	if (m_pInterface)	m_pInterface->AddRef( );

	return PBTRUE;
}

//----------------------------------------------------------------------------
//  PBSession member function implementations
//----------------------------------------------------------------------------

PBSession::PBSession( ) :
    PBUnknown( )
{
    PBISession * pThis = NULL;

    PBIString * pAppName = NULL;
    PBIString * pLibraryList = NULL;

    if (SUCCEEDED(CreatePBIString(pszAPPNAME, &pAppName)) &&
        SUCCEEDED(CreatePBIString(pszLIBRARYLIST, &pLibraryList)) &&
        SUCCEEDED(CreatePBISession(pAppName, pLibraryList, eLIBRARYTYPE, NULL, &pThis)))
	{
        SetInterface(pThis);
	}

    if (pAppName)       pAppName->Release( );
    if (pLibraryList)   pLibraryList->Release( );
    if (pThis)          pThis->Release( );
}

//----------------------------------------------------------------------------

PBSession::PBSession( const PBSession & other ) :
    PBUnknown(other)
{
}

//----------------------------------------------------------------------------

PBSession::~PBSession( )
{
}

//----------------------------------------------------------------------------

PBSession & PBSession::operator = ( const PBSession & other )
{
    if (this != &other)
    {
        PBUnknown::operator = (other);
    }

    return *this;
}

//----------------------------------------------------------------------------

PBBOOL PBSession::IsValid( ) const
{
    return (GetSession( )) ? PBTRUE : PBFALSE;
}

//----------------------------------------------------------------------------
//  PBInstance member function implementations
//----------------------------------------------------------------------------

PBInstance::PBInstance( const PBSession & session, const PBString & name ) :
    PBUnknown( ),
    m_LastErrorMessage( )
{
    PBISession * pSession   = (PBISession *)GetInterfaceFrom(session);
    PBIString * pName       = (PBIString *)GetInterfaceFrom(name);

    PBIInstance * pThis = NULL;

    if (SUCCEEDED(CreatePBIInstance(pName, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis) pThis->Release( );
}

//----------------------------------------------------------------------------

PBInstance::PBInstance( const PBString & libraryPath, const PBString & name ) : 
    PBUnknown( ),
    m_LastErrorMessage( )
{
	PBISession * pSession = NULL;
	PBIString * pLibraryList = NULL;
	PBIString * pNewLibraryList = NULL;
	PBIString * pName = (PBIString *)GetInterfaceFrom(name);
	PBIString * pNewLibraryPath = (PBIString *)GetInterfaceFrom(libraryPath);
	PBIInstance * pThis = NULL;
	
	PBRESULT pbResult = PBRESULT_OK;

	if (SUCCEEDED(pbResult))
		pbResult = CreatePBIString(pszLIBRARYLIST, &pLibraryList);
	if (SUCCEEDED(pbResult))
		pbResult = PBUtil_ReplaceLibraryPathsIn(
			pLibraryList,
			pNewLibraryPath,
			&pNewLibraryList);
	if (SUCCEEDED(pbResult))
		pbResult = CreatePBISession(
			NULL,
			pNewLibraryList,
			eLIBRARYTYPE,
			NULL,
			&pSession);
	if (SUCCEEDED(pbResult))
		pbResult = CreatePBIInstance(pName, pSession, &pThis);			 		
	if (SUCCEEDED(pbResult))
		SetInterface(pThis);

	if (pThis) pThis->Release( );
	if (pLibraryList) pLibraryList->Release( );
	if (pNewLibraryList) pNewLibraryList->Release( );
	if (pSession) pSession->Release( );
}

//----------------------------------------------------------------------------

PBInstance::PBInstance( const PBInstance & other ) :
    PBUnknown(other),
    m_LastErrorMessage( )
{
}

//----------------------------------------------------------------------------

PBInstance::~PBInstance( )
{
}

//----------------------------------------------------------------------------

PBInstance & PBInstance::operator = ( const PBInstance & other )
{
    if (this != &other)
    {
        PBUnknown::operator = (other);
        m_LastErrorMessage = other.m_LastErrorMessage;
    }

    return *this;
}

//----------------------------------------------------------------------------

PBBOOL PBInstance::IsValid( ) const
{
    return (GetInstance( )) ? PBTRUE : PBFALSE;
}


//----------------------------------------------------------------------------
//	PBString member function implementations
//----------------------------------------------------------------------------

PBString::PBString( ) :
	PBUnknown( )
{
	PBIString * pThis = NULL;

	if (SUCCEEDED(CreatePBIString(PBTEXT(""), &pThis)))
		SetInterface(pThis);

	if (pThis) pThis->Release( );
}

//----------------------------------------------------------------------------

PBString::PBString( const PBCHAR * lpszValue ) :
	PBUnknown( )
{
	PBIString * pThis = NULL;

	if (SUCCEEDED(CreatePBIString((PBCHAR *)lpszValue, &pThis)))
		SetInterface(pThis);

	if (pThis) pThis->Release( );
}

//----------------------------------------------------------------------------

PBString::PBString( const PBString & other ) :
	PBUnknown(other)
{
}

//----------------------------------------------------------------------------

PBString::~PBString( )
{
}

//----------------------------------------------------------------------------

PBString & PBString::operator = ( const PBString & other )
{
	if (this != &other)
	{
		PBUnknown::operator = (other);
	}

	return *this;
}
	
//----------------------------------------------------------------------------

PBBOOL PBString::operator == ( const PBString & other ) const
{
	PBINT iComparison = 0;
	return (CompareTo(other, iComparison) && (iComparison == 0));
}

//----------------------------------------------------------------------------

PBString::operator const PBCHAR * ( ) const
{
	const PBCHAR * szValue = NULL;
	GetValue(szValue);
	return szValue;
}

//----------------------------------------------------------------------------

PBBOOL PBString::GetValue( const PBCHAR * & lpszValue ) const
{
	PBIString * pThis = GetString( );

	if (!pThis)
		return PBFALSE;

	PBCHAR * szValue = NULL;
 
	if (FAILED(pThis->GetValue(&szValue)))
		return PBFALSE;

	lpszValue = (const PBCHAR *)szValue;
 
	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBString::GetLength( PBULONG & ulLength ) const
{
	PBIString * pThis = GetString( );

	if (!pThis)
		return PBFALSE;

	if (FAILED(pThis->GetLength(&ulLength)))
		return PBFALSE;

	return PBTRUE;
}


//----------------------------------------------------------------------------

PBBOOL PBString::GetValueAt( PBULONG ulOffset, PBCHAR & cValue ) const
{
	PBIString * pThis = GetString( );

	if (pThis)
		return PBFALSE;

	if (FAILED(pThis->GetValueAt(ulOffset, &cValue)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBString::SubString( 
	PBULONG ulFrom, 
	PBULONG ulTo, 
	PBString & subString ) const
{
	PBIString * pThis = GetString( );
	
	if (!pThis)
		return PBFALSE;

	PBIString * pSubString = NULL;

	if (FAILED(pThis->SubString(ulFrom, ulTo, &pSubString)))
		return PBFALSE;

	PBBOOL bOK = SetInterfaceOn(subString, pSubString);

	if (pSubString) pSubString->Release( );

	return bOK;
}

//----------------------------------------------------------------------------

PBBOOL PBString::IsNull( PBBOOL & bIsNull ) const
{
	PBIString * pThis = GetString( );

	if (!pThis)
		return PBFALSE;

	if (FAILED(pThis->IsNull(&bIsNull)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBString::CompareTo( const PBString & other, PBINT & iComparison ) const
{
	PBIString * pThis = GetString( );
	PBIString * pOther = (PBIString *)GetInterfaceFrom(other);

	if (!pThis || !pOther)
		return PBFALSE;

 	if (FAILED(pThis->CompareTo(pOther, &iComparison)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBString::IsValid( ) const
{
	return (GetString( )) ? PBTRUE : PBFALSE;
}

//----------------------------------------------------------------------------
//	PBDateTime member function implementations
//----------------------------------------------------------------------------

PBDateTime::PBDateTime( ) :
	PBUnknown( )
{
	PBIDate * pDate		=	NULL;
	PBITime * pTime		=	NULL;
	PBIDateTime * pThis	=	NULL;

	if (SUCCEEDED(CreatePBIDateToday(&pDate)) 	&&
		SUCCEEDED(CreatePBITimeNow(&pTime))		&&
		SUCCEEDED(CreatePBIDateTime(pDate, pTime, &pThis)))
	{
		SetInterface(pThis);
	}

	if (pDate)	pDate->Release( );
	if (pTime)	pTime->Release( );
	if (pThis)	pThis->Release( );
}

//----------------------------------------------------------------------------

PBDateTime::PBDateTime( const PBDate & date, const PBTime & time ) :
	PBUnknown( )
{
	PBIDate * pDate 	= (PBIDate *)GetInterfaceFrom(date);
	PBITime * pTime 	= (PBITime *)GetInterfaceFrom(time);

	PBIDateTime * pThis = NULL;

	if (SUCCEEDED(CreatePBIDateTime(pDate, pTime, &pThis)))
		SetInterface(pThis);

	if (pThis)	pThis->Release( );
}

//----------------------------------------------------------------------------

PBDateTime::PBDateTime( const PBDateTime & other ) :
	PBUnknown(other)
{
}

//----------------------------------------------------------------------------

PBDateTime::~PBDateTime( )
{
}

//----------------------------------------------------------------------------

PBDateTime & PBDateTime::operator = ( const PBDateTime & other )
{
	if (this != &other)
	{
		PBUnknown::operator = (other);
	}

	return *this;
}

//----------------------------------------------------------------------------

PBBOOL PBDateTime::GetDate( PBDate & date ) const
{
	PBIDateTime * pThis = GetDateTime( );

	if (!pThis)
		return PBFALSE;

	PBIDate * 	pDate 	= NULL;

	if (SUCCEEDED(pThis->GetDate(&pDate)))
		return PBFALSE;

	PBBOOL bOK = SetInterfaceOn(date, pDate);

	if (pDate)	pDate->Release( );

	return bOK;
}

//----------------------------------------------------------------------------

PBBOOL PBDateTime::GetTime( PBTime & time ) const
{
    PBIDateTime * pThis = GetDateTime( );

    if (!pThis)
        return PBFALSE;

    PBITime *   pTime   = NULL;

    if (SUCCEEDED(pThis->GetTime(&pTime)))
		return PBFALSE;

    PBBOOL bOK = SetInterfaceOn(time, pTime);

    if (pTime)  pTime->Release( );

    return bOK;
}

//----------------------------------------------------------------------------
 
PBBOOL PBDateTime::IsNull( PBBOOL & bIsNull ) const
{
    PBIDateTime * pThis = GetDateTime( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->IsNull(&bIsNull)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDateTime::CompareTo( 	
	const PBDateTime & other, 
	PBINT & iComparison ) const
{
    PBIDateTime * pThis	 = GetDateTime( );
    PBIDateTime * pOther = (PBIDateTime *)GetInterfaceFrom(other);

    if (!pThis || !pOther)
        return PBFALSE;

    if (FAILED(pThis->CompareTo(pOther, &iComparison)))
		return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDateTime::IsValid( ) const
{
	return (GetDateTime( )) ? PBTRUE : PBFALSE;
}

//----------------------------------------------------------------------------
//	PBDate member function implementations
//----------------------------------------------------------------------------

PBDate::PBDate( ) :
	PBUnknown( )
{
	PBIDate * pThis	= NULL;

	if (SUCCEEDED(CreatePBIDateToday(&pThis)))
		SetInterface(pThis);

	if (pThis) pThis->Release( );
}

//----------------------------------------------------------------------------

PBDate::PBDate( PBSHORT iYear, PBINT iMonth, PBINT iDay ) :
	PBUnknown( )
{
	PBIDate * pThis = NULL;

	if (SUCCEEDED(CreatePBIDate(iYear, iMonth, iDay, &pThis)))
		SetInterface(pThis);

	if (pThis) pThis->Release( );
}

//----------------------------------------------------------------------------

PBDate::PBDate( const PBDate & other ) :
	PBUnknown(other)
{
}

//----------------------------------------------------------------------------

PBDate::~PBDate( )
{
}
 
//----------------------------------------------------------------------------
 
PBDate & PBDate::operator = ( const PBDate & other )
{
	if (this != &other)
	{
		PBUnknown::operator = (other);
	}

	return *this;
}

//----------------------------------------------------------------------------

PBBOOL PBDate::GetYear( PBSHORT & iYear ) const
{
    PBIDate * pThis = GetDate( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->GetYear(&iYear)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDate::GetMonth( PBINT & iMonth ) const
{
    PBIDate * pThis = GetDate( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->GetMonth(&iMonth)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDate::GetDayOfMonth( PBINT & iDay ) const
{
    PBIDate * pThis = GetDate( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->GetDayOfMonth(&iDay)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDate::GetDayOfWeek( PBINT & iDay ) const
{
	PBIDate * pThis = GetDate( );

	if (!pThis)
		return PBFALSE;

	if (FAILED(pThis->GetDayOfWeek(&iDay)))
		return PBFALSE;

	return PBTRUE;
}
 
//----------------------------------------------------------------------------

PBBOOL PBDate::DaysAfter( const PBDate & other, PBLONG & lDays ) const
{
	PBIDate * pThis	= GetDate( );
	PBIDate * pOther = (PBIDate *)GetInterfaceFrom(other);

	if (!pThis || !pOther)
		return PBFALSE;

	if (FAILED(pThis->DaysAfter(pOther, &lDays)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDate::RelativeDate( PBLONG lDays, PBDate & relativeDate ) const
{
	PBIDate * pThis = GetDate( );

	if (!pThis)
		return PBFALSE;

	PBIDate * pRelativeDate = NULL;

	if (SUCCEEDED(pThis->RelativeDate(lDays, &pRelativeDate)))
		return PBFALSE;

	PBBOOL bOk = SetInterfaceOn(relativeDate, pRelativeDate);

	if (pRelativeDate)	pRelativeDate->Release( );

	return bOk;
}
	
//----------------------------------------------------------------------------

PBBOOL PBDate::IsNull( PBBOOL & bIsNull ) const
{
	PBIDate * pThis = GetDate( );
	
	if (!pThis)
		return PBFALSE;

	if (FAILED(pThis->IsNull(&bIsNull)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDate::CompareTo( const PBDate & other, PBINT & iComparison ) const
{
    PBIDate * pThis = GetDate( );
    PBIDate * pOther = (PBIDate *)GetInterfaceFrom(other);

    if (!pThis || !pOther)
        return PBFALSE;

    if (FAILED(pThis->CompareTo(pOther, &iComparison)))
		return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDate::IsValid( ) const
{
	return (GetDate( )) ? PBTRUE : PBFALSE;
}

//----------------------------------------------------------------------------
//	PBTime member function implementations
//----------------------------------------------------------------------------

PBTime::PBTime( ) :
	PBUnknown( )
{
	PBITime * pThis = NULL;

	if (SUCCEEDED(CreatePBITimeNow(&pThis)))
		SetInterface(pThis);

	if (pThis)	pThis->Release( );
}

//----------------------------------------------------------------------------

PBTime::PBTime( PBINT iHour, PBINT iMinute, PBINT iSecond ) :
	PBUnknown( )
{
	PBITime * pThis = NULL;

	if (SUCCEEDED(CreatePBITime(iHour, iMinute, iSecond, &pThis)))
		SetInterface(pThis);

	if (pThis)	pThis->Release( );
}

//----------------------------------------------------------------------------

PBTime::PBTime( const PBTime & other ) :
	PBUnknown(other)
{
}

//----------------------------------------------------------------------------

PBTime::~PBTime( )
{
}

//----------------------------------------------------------------------------

PBTime & PBTime::operator = ( const PBTime & other )
{
	if (this != &other)
	{
		PBUnknown::operator = (other);
	}

	return *this;
}

//----------------------------------------------------------------------------

PBBOOL PBTime::GetHour( PBINT & iHour ) const
{
    PBITime * pThis = GetTime( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->GetHour(&iHour)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBTime::GetMinute( PBINT & iMinute ) const
{
    PBITime * pThis = GetTime( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->GetMinute(&iMinute)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBTime::GetSecond( PBINT & iSecond ) const
{
	PBITime * pThis = GetTime( );

	if (!pThis)
		return PBFALSE;

	if (FAILED(pThis->GetSecond(&iSecond)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBTime::SecondsAfter( const PBTime & other, PBLONG & lSeconds ) const 
{
	PBITime *	pThis  	=	GetTime( );
	PBITime *	pOther	=	(PBITime *)GetInterfaceFrom(other);

	if (!pThis || !pOther)
		return PBFALSE;

	if (FAILED(pThis->SecondsAfter(pOther, &lSeconds)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBTime::RelativeTime( PBLONG lSeconds, PBTime & relativeTime ) const
{
	PBITime * pThis = GetTime( );

	if (!pThis)
		return PBFALSE;

	PBITime * pRelativeTime = NULL;

	if (SUCCEEDED(pThis->RelativeTime(lSeconds, &pRelativeTime)))
		return PBFALSE;

	PBBOOL bOk = SetInterfaceOn(relativeTime, pRelativeTime);

	if (pRelativeTime)	pRelativeTime->Release( );

	return bOk;
}

//----------------------------------------------------------------------------

PBBOOL PBTime::IsNull( PBBOOL & bIsNull ) const
{
	PBITime * pThis = GetTime( );

	if (!pThis)
		return PBFALSE;

	if (FAILED(pThis->IsNull(&bIsNull)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBTime::CompareTo( const PBTime & other, PBINT & iComparison ) const
{
    PBITime * pThis     = GetTime( );
    PBITime * pOther    = (PBITime *)GetInterfaceFrom(other);

    if (!pThis || !pOther)
        return PBFALSE;

    if (FAILED(pThis->CompareTo(pOther, &iComparison)))
		return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBTime::IsValid( ) const
{
	return (GetTime( )) ? PBTRUE : PBFALSE;
}

//----------------------------------------------------------------------------
//	PBDecimal member function implementations
//----------------------------------------------------------------------------

PBDecimal::PBDecimal( ) :
    PBUnknown( )
{
	PBIDecimal * pThis = NULL;

    if (SUCCEEDED(CreatePBIDecimal(&pThis)))
        SetInterface(pThis);

    if (pThis) pThis->Release( );
}

//----------------------------------------------------------------------------

PBDecimal::PBDecimal( PBDOUBLE value ) :
	PBUnknown( )
{
	PBIDecimal * pThis = NULL;

	if (SUCCEEDED(CreatePBIDecimal(&pThis)) &&
		SUCCEEDED(pThis->SetDecimalFromDouble(value)))
	{
		SetInterface(pThis);
	}
}

//----------------------------------------------------------------------------

PBDecimal::PBDecimal( const PBDecimal & other ) :
    PBUnknown(other)
{
}

//----------------------------------------------------------------------------

PBDecimal::~PBDecimal( )
{
}

//----------------------------------------------------------------------------

PBDecimal & PBDecimal::operator = ( const PBDecimal & other )
{
	if (this != &other)
	{
		PBUnknown::operator = (other);
	}

	return *this;
}

//----------------------------------------------------------------------------

PBDecimal::operator PBDOUBLE( ) const
{
	if (!IsValid( ))
		return PBDOUBLE_NULL;

	PBDOUBLE value;
	PBIDecimal * pThis = GetDecimal( );

	pThis->GetDecimalAsDouble(&value);

	return value;
}

//----------------------------------------------------------------------------

PBBOOL PBDecimal::GetValue( PBDOUBLE & value ) const
{
	if (!IsValid( ))
		return PBFALSE;

	PBIDecimal * pThis = GetDecimal( );
	pThis->GetDecimalAsDouble(&value);

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDecimal::SetValue( PBDOUBLE value )
{
	if (!IsValid( ))
		return PBFALSE;

	PBIDecimal * pThis = GetDecimal( );
	pThis->SetDecimalFromDouble(value);

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDecimal::IsNull( PBBOOL & bIsNull ) const
{
    PBIDecimal * pThis = GetDecimal( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->IsNull(&bIsNull)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBDecimal::CompareTo( const PBDecimal & other, PBINT & iComparison ) const
{
    PBIDecimal * pThis = GetDecimal( );
    PBIDecimal * pOther = (PBIDecimal *)GetInterfaceFrom(other);

    if (!pThis || !pOther)
        return PBFALSE;

    if (FAILED(pThis->CompareTo(pOther, &iComparison)))
		return PBFALSE;

    return PBTRUE;
}


//----------------------------------------------------------------------------

PBBOOL PBDecimal::IsValid( ) const
{
	return (GetDecimal( )) ? PBTRUE : PBFALSE;
}

//----------------------------------------------------------------------------
//	PBBlob member function implementations
//----------------------------------------------------------------------------

PBBlob::PBBlob( ) :
    PBUnknown( )
{
	PBIBlob * pThis = NULL;

	if (SUCCEEDED(CreatePBIBlob(NULL, 0, &pThis)))
		SetInterface(pThis);

    if (pThis) pThis->Release( );
}


//----------------------------------------------------------------------------

PBBlob::PBBlob( PBPVOID pData, PBULONG ulSize ) :
	PBUnknown( )
{
	PBIBlob * pThis = NULL;

	if (SUCCEEDED(CreatePBIBlob(ulSize, pData, &pThis)))
		SetInterface(pThis);

	if (pThis)	pThis->Release( );
}

//----------------------------------------------------------------------------

PBBlob::PBBlob( const PBBlob & other ) :
	PBUnknown(other)
{
}

//----------------------------------------------------------------------------

PBBlob::~PBBlob( )
{
}

//----------------------------------------------------------------------------

PBBlob & PBBlob::operator = ( const PBBlob & other )
{
	if (this != &other)
	{
		PBUnknown::operator = (other);
	}

	return *this;
}

//----------------------------------------------------------------------------

PBBOOL PBBlob::GetSize( PBULONG & ulSize ) const
{
	PBIBlob * pThis = GetBlob( );

	if (!pThis)
		return PBFALSE;

	if (FAILED(pThis->GetSize(&ulSize)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBBlob::GetData( PBPVOID & pData ) const
{
	PBIBlob * pThis = GetBlob( );

	if (!pThis)
		return PBFALSE;

	if (FAILED(pThis->GetData(&pData)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBBlob::InsertData( PBULONG ulOffset, const PBBlob & data )
{
	PBIBlob * pThis = GetBlob( );
	PBIBlob * pOther = (PBIBlob *)GetInterfaceFrom(data);

	if (!pThis || !pOther)
		return PBFALSE;

	if (FAILED(pThis->InsertData(ulOffset, pOther)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBBlob::ExtractData( 
	PBULONG ulStart, 
	PBULONG ulSize, 
	PBBlob & extract ) const
{
	PBIBlob * pThis = GetBlob( );

	if (!pThis)
		return PBFALSE;

	PBIBlob * pExtract = NULL;

	if (FAILED(pThis->ExtractData(ulStart, ulSize, &pExtract)))
		return PBFALSE;

	PBBOOL bOk = SetInterfaceOn(extract, pExtract);

	if (pExtract) pExtract->Release( );

	return bOk;
}

//----------------------------------------------------------------------------

PBBOOL PBBlob::IsNull( PBBOOL & bIsNull ) const
{
	PBIBlob * pThis = GetBlob( );

	if (!pThis)
		return PBFALSE;

	if (FAILED(pThis->IsNull(&bIsNull)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBBlob::IsValid( ) const
{
	return (GetBlob( )) ? PBTRUE : PBFALSE;
}

//----------------------------------------------------------------------------
//	PBArrayBounds member function implementations
//----------------------------------------------------------------------------

PBArrayBounds::PBArrayBounds( ) :
	PBUnknown( )
{
	PBISession * pSession = (PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBounds * pThis = NULL;

	if (SUCCEEDED(CreatePBIArrayBounds((PBLONG)0, (PBLONG)0, pSession, &pThis)))
		SetInterface(pThis);

	if (pThis)	pThis->Release( );
}

//----------------------------------------------------------------------------

PBArrayBounds::PBArrayBounds( PBLONG lUpper, PBLONG lLower ) :
    PBUnknown( )
{
	PBISession * pSession = (PBISession *)GetInterfaceFrom(GetMasterSession( ));
    PBIArrayBounds * pThis = NULL;

    if (SUCCEEDED(CreatePBIArrayBounds(lUpper, lLower, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis) pThis->Release( );
}

//----------------------------------------------------------------------------

PBArrayBounds::PBArrayBounds( const PBArrayBounds & other ) :
    PBUnknown(other)
{
}

//----------------------------------------------------------------------------

PBArrayBounds::~PBArrayBounds( )
{
}

//----------------------------------------------------------------------------

PBArrayBounds & PBArrayBounds::operator = ( const PBArrayBounds & other )
{
    if (this != &other)
    {
        PBUnknown::operator = (other);
    }

    return *this;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBounds::GetUpperBound( PBLONG & lUpper ) const
{
    PBIArrayBounds * pThis = GetArrayBounds( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->GetUpperBound(&lUpper)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBounds::SetUpperBound( PBLONG lUpper )
{
    PBIArrayBounds * pThis = GetArrayBounds( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->SetUpperBound(lUpper)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBounds::GetLowerBound( PBLONG & lLower ) const
{
    PBIArrayBounds * pThis = GetArrayBounds( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->GetLowerBound(&lLower)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBounds::SetLowerBound( PBLONG lLower )
{
    PBIArrayBounds * pThis = GetArrayBounds( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->SetLowerBound(lLower)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBounds::IsValid( ) const
{
    return (GetArrayBounds( )) ? PBTRUE : PBFALSE;
}

//----------------------------------------------------------------------------
//	PBArrayBoundsList
//----------------------------------------------------------------------------

PBArrayBoundsList::PBArrayBoundsList( ) :
	PBUnknown( )
{
	PBIArrayBoundsList * pThis = NULL;
 
	if (SUCCEEDED(CreatePBIArrayBoundsList(&pThis)))
		SetInterface(pThis);
 
	if (pThis)	pThis->Release( );
}

//----------------------------------------------------------------------------

PBArrayBoundsList::PBArrayBoundsList( const PBArrayBoundsList & other ) :
	PBUnknown(other)
{
}

//----------------------------------------------------------------------------

PBArrayBoundsList::~PBArrayBoundsList( )
{
}

//----------------------------------------------------------------------------

PBArrayBoundsList & PBArrayBoundsList::operator = ( const PBArrayBoundsList & other )
{
	if (this != &other)
	{
		PBUnknown::operator = (other);
	}

	return *this;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBoundsList::Add( const PBArrayBounds & bounds )
{
    PBIArrayBoundsList * pBoundsList = GetArrayBoundsList( );
    PBIArrayBounds * pBounds = (PBIArrayBounds *)GetInterfaceFrom(bounds);

    if (!pBoundsList || !pBounds)
        return PBFALSE;
    if (FAILED(pBoundsList->Add(pBounds)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBoundsList::Remove( const PBArrayBounds & bounds )
{
    PBIArrayBoundsList * pBoundsList = GetArrayBoundsList( );
    PBIArrayBounds * pBounds = (PBIArrayBounds *)GetInterfaceFrom(bounds);

    if (!pBoundsList || !pBounds)
        return PBFALSE;
    if (FAILED(pBoundsList->Remove(pBounds)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBoundsList::GetCount( PBINDEX & count ) const
{
	PBIArrayBoundsList * pBoundsList = GetArrayBoundsList( );

	if (!pBoundsList)
		return PBFALSE;
	if (FAILED(pBoundsList->GetCount(&count)))
		return PBFALSE;

	return PBTRUE;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBoundsList::GetFirst( PBArrayBounds & bounds )
{
    PBIArrayBoundsList * pBoundsList = GetArrayBoundsList( );

    if (!pBoundsList)
        return PBFALSE;

    PBIArrayBounds * pBounds = NULL;

    if (FAILED(pBoundsList->GetFirst(&pBounds)))
        return PBFALSE;

    PBBOOL bOK = SetInterfaceOn(bounds, pBounds);

    if (pBounds) pBounds->Release( );

    return bOK;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBoundsList::GetNext( PBArrayBounds & bounds )
{
    PBIArrayBoundsList * pBoundsList = GetArrayBoundsList( );

    if (!pBoundsList)
        return PBFALSE;

    PBIArrayBounds * pBounds = NULL;

    if (FAILED(pBoundsList->GetNext(&pBounds)))
        return PBFALSE;

    PBBOOL bOK = SetInterfaceOn(bounds, pBounds);

    if (pBounds) pBounds->Release( );

    return bOK;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBoundsList::GetLast( PBArrayBounds & bounds )
{
    PBIArrayBoundsList * pBoundsList = GetArrayBoundsList( );

    if (!pBoundsList)
        return PBFALSE;

    PBIArrayBounds * pBounds = NULL;

    if (FAILED(pBoundsList->GetLast(&pBounds)))
        return PBFALSE;

    PBBOOL bOK = SetInterfaceOn(bounds, pBounds);

    if (pBounds) pBounds->Release( );

    return bOK;
}

//----------------------------------------------------------------------------

PBBOOL PBArrayBoundsList::GetPrevious( PBArrayBounds & bounds )
{
    PBIArrayBoundsList * pBoundsList = GetArrayBoundsList( );

    if (!pBoundsList)
        return PBFALSE;

	PBIArrayBounds * pBounds = NULL;

    if (FAILED(pBoundsList->GetPrevious(&pBounds)))
        return PBFALSE;
	
	PBBOOL bOK = SetInterfaceOn(bounds, pBounds);

	if (pBounds) pBounds->Release( );

    return bOK;
}


//----------------------------------------------------------------------------
//	PBArray<T> member function implementations
//----------------------------------------------------------------------------

template<class T>
PBArray<T>::PBArray( ) :
    PBUnknown( )
{
}

   
//----------------------------------------------------------------------------

template<class T>
PBArray<T>::PBArray( const PBArrayBoundsList & ) :
	PBUnknown( )
{
	SetInterface(NULL);
}
 

//----------------------------------------------------------------------------

template<class T>
PBArray<T>::PBArray( const PBArray<T> & other ) :
    PBUnknown(other)
{
}

//----------------------------------------------------------------------------

template<class T>
PBArray<T>::~PBArray( )
{
}

//----------------------------------------------------------------------------

template<class T>
PBArray<T> & PBArray<T>::operator = ( const PBArray<T> & other )
{
    if (this != &other)
    {
        PBUnknown::operator = (other);
    }

    return *this;
}

//----------------------------------------------------------------------------

template<class T>
PBBOOL PBArray<T>::GetDimensions( PBINDEX & cDimensions ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    if (FAILED(pThis->GetDimensions(&cDimensions)))
        return PBFALSE;

    return PBTRUE;
}

//----------------------------------------------------------------------------

template<class T>
PBBOOL PBArray<T>::GetBounds( PBArrayBoundsList & BoundsList ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

	PBIArrayBoundsList * pBoundsList = NULL;
	
	if (FAILED(pThis->GetBounds(&pBoundsList)))
		return PBFALSE;

	PBBOOL bOK = SetInterfaceOn(BoundsList, pBoundsList);

	if (pBoundsList) pBoundsList->Release( );

    return bOK;
}

//----------------------------------------------------------------------------

template<class T>
PBBOOL PBArray<T>::IsValid( ) const
{
    return (GetArray( )) ? PBTRUE : PBFALSE;
}

//----------------------------------------------------------------------------
//	PBArray<T> explicit instantations for basic types.
//----------------------------------------------------------------------------

#ifdef PBGEN_EXPLICIT_ARRAY_INSTANTIATIONS_NEEDED
template class PBArray<PBCHAR>;
template class PBArray<PBBOOL>;
template class PBArray<PBSHORT>;
template class PBArray<PBUSHORT>;
template class PBArray<PBLONG>;
template class PBArray<PBULONG>;
template class PBArray<PBFLOAT>;
template class PBArray<PBDOUBLE>;
template class PBArray<PBString>;
template class PBArray<PBDateTime>;
template class PBArray<PBDate>;
template class PBArray<PBTime>;
template class PBArray<PBBlob>;
template class PBArray<PBDecimal>;
template class PBArray<PBInstance>;
#endif	//	PBGEN_EXPLICIT_INSTANTIATIONS_NEEDED

//----------------------------------------------------------------------------
//	PBArray<T> templated member function implementations for basic types.
//----------------------------------------------------------------------------

PBArray<PBCHAR>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeChar, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBBOOL>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeBoolean, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBSHORT>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeShort, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBUSHORT>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeUShort, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBLONG>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeLong, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBULONG>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeULong, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBFLOAT>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeFloat, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBDOUBLE>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeDouble, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBString>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeString, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBDecimal>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeDecimal, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBDate>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeDate, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBTime>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeTime, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBDateTime>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeDateTime, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBArray<PBBlob>::PBArray( const PBArrayBoundsList & boundsList ) :
    PBUnknown( )
{
    PBISession * pSession = 
		(PBISession *)GetInterfaceFrom(GetMasterSession( ));
	PBIArrayBoundsList * pBoundsList = 
		(PBIArrayBoundsList *)GetInterfaceFrom(boundsList);
    PBIArray * pThis = NULL;

    if (SUCCEEDED(CreatePBIArray(pBoundsList, PBValueTypeBlob, pSession, &pThis)))
        SetInterface(pThis);

    if (pThis)  pThis->Release( );
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBCHAR>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBCHAR & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBIValue * pValue = NULL;
    PBRESULT pbResult = PBRESULT_OK;

    pbResult = pThis->GetValueAt(pOffsets, dims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsChar(&value);

    if (pValue) pValue->Release( );

    return (SUCCEEDED(pbResult));
}

//----------------------------------------------------------------------------


PBBOOL PBArray<PBBOOL>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBBOOL & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBIValue * pValue = NULL;
    PBRESULT pbResult = PBRESULT_OK;

    pbResult = pThis->GetValueAt(pOffsets, dims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsBoolean(&value);

    if (pValue) pValue->Release( );

    return (SUCCEEDED(pbResult));
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBSHORT>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBSHORT & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBIValue * pValue = NULL;
    PBRESULT pbResult = PBRESULT_OK;

    pbResult = pThis->GetValueAt(pOffsets, dims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsShort(&value);

    if (pValue) pValue->Release( );

    return (SUCCEEDED(pbResult));
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBUSHORT>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBUSHORT & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBIValue * pValue = NULL;
    PBRESULT pbResult = PBRESULT_OK;

    pbResult = pThis->GetValueAt(pOffsets, dims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsUShort(&value);

    if (pValue) pValue->Release( );

    return (SUCCEEDED(pbResult));
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBLONG>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBLONG & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBIValue * pValue = NULL;
    PBRESULT pbResult = PBRESULT_OK;

    pbResult = pThis->GetValueAt(pOffsets, dims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsLong(&value);

    if (pValue) pValue->Release( );

    return (SUCCEEDED(pbResult));
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBULONG>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBULONG & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBIValue * pValue = NULL;
    PBRESULT pbResult = PBRESULT_OK;

    pbResult = pThis->GetValueAt(pOffsets, dims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsULong(&value);

    if (pValue) pValue->Release( );

    return (SUCCEEDED(pbResult));
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBFLOAT>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBFLOAT & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBIValue * pValue = NULL;
    PBRESULT pbResult = PBRESULT_OK;

    pbResult = pThis->GetValueAt(pOffsets, dims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsFloat(&value);

    if (pValue) pValue->Release( );

    return (SUCCEEDED(pbResult));
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBDOUBLE>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBDOUBLE & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBIValue * pValue = NULL;
    PBRESULT pbResult = PBRESULT_OK;

    pbResult = pThis->GetValueAt(pOffsets, dims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsDouble(&value);

    if (pValue) pValue->Release( );

    return (SUCCEEDED(pbResult));
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBString>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBString & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;
    PBBOOL bOk = PBFALSE;

    PBIValue * pValue = NULL;
    PBIString * pString = NULL;

    PBINDEX iDims = (PBINDEX)dims;

    pbResult = pThis->GetValueAt(pOffsets, iDims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsString(&pString);

    if (SUCCEEDED(pbResult))
        bOk = SetInterfaceOn(value, pString);

    if (pString)    pString->Release( );
    if (pValue)     pValue->Release( );

    return bOk;
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBDecimal>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBDecimal & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;
    PBBOOL bOk = PBFALSE;

    PBIValue * pValue = NULL;
    PBIDecimal * pDecimal = NULL;

    PBINDEX iDims = (PBINDEX)dims;

    pbResult = pThis->GetValueAt(pOffsets, iDims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsDecimal(&pDecimal);

    if (SUCCEEDED(pbResult))
        bOk = SetInterfaceOn(value, pDecimal);

    if (pDecimal)   pDecimal->Release( );
    if (pValue)     pValue->Release( );

    return bOk;
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBDate>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBDate & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;
    PBBOOL bOk = PBFALSE;

    PBIValue * pValue = NULL;
    PBIDate * pDate = NULL;

    PBINDEX iDims = (PBINDEX)dims;

    pbResult = pThis->GetValueAt(pOffsets, iDims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsDate(&pDate);

    if (SUCCEEDED(pbResult))
        bOk = SetInterfaceOn(value, pDate);

    if (pDate)      pDate->Release( );
    if (pValue)     pValue->Release( );

    return bOk;
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBTime>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBTime & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;
    PBBOOL bOk = PBFALSE;

    PBIValue * pValue = NULL;
    PBITime * pTime = NULL;

    PBINDEX iDims = (PBINDEX)dims;

    pbResult = pThis->GetValueAt(pOffsets, iDims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsTime(&pTime);

    if (SUCCEEDED(pbResult))
        bOk = SetInterfaceOn(value, pTime);

    if (pTime)      pTime->Release( );
    if (pValue)     pValue->Release( );

    return bOk;
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBDateTime>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBDateTime & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;
    PBBOOL bOk = PBFALSE;

    PBIValue * pValue = NULL;
    PBIDateTime * pDateTime = NULL;

    PBINDEX iDims = (PBINDEX)dims;

    pbResult = pThis->GetValueAt(pOffsets, iDims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsDateTime(&pDateTime);

    if (SUCCEEDED(pbResult))
        bOk = SetInterfaceOn(value, pDateTime);

    if (pDateTime)  pDateTime->Release( );
    if (pValue)     pValue->Release( );

    return bOk;
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBBlob>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBBlob & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;
    PBBOOL bOk = PBFALSE;

    PBIValue * pValue = NULL;
    PBIBlob * pBlob = NULL;

    PBINDEX iDims = (PBINDEX)dims;

    pbResult = pThis->GetValueAt(pOffsets, iDims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsBlob(&pBlob);

    if (SUCCEEDED(pbResult))
        bOk = SetInterfaceOn(value, pBlob);

    if (pBlob)      pBlob->Release( );
    if (pValue)     pValue->Release( );

    return bOk;
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBInstance>::GetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    PBInstance & value ) const
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;
    PBBOOL bOk = PBFALSE;

    PBIValue * pValue = NULL;
    PBIInstance * pInstance = NULL;

    PBINDEX iDims = (PBINDEX)dims;

    pbResult = pThis->GetValueAt(pOffsets, iDims, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->GetValueAsInstance(&pInstance);

    if (SUCCEEDED(pbResult))
        bOk = SetInterfaceOn(value, pInstance);

    if (pInstance)  pInstance->Release( );
    if (pValue)     pValue->Release( );

    return bOk;
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBCHAR>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBCHAR & value )
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue *  pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeChar, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromChar(value);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return SUCCEEDED(pbResult);
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBBOOL>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBBOOL & value )
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue *  pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeBoolean, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromBoolean(value);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return SUCCEEDED(pbResult);
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBSHORT>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBSHORT & value )
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue *  pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeShort, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromShort(value);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return SUCCEEDED(pbResult);
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBUSHORT>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBUSHORT & value )
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue *  pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeUShort, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromUShort(value);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return SUCCEEDED(pbResult);
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBLONG>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBLONG & value )
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue *  pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeLong, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromLong(value);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return SUCCEEDED(pbResult);
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBULONG>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBULONG & value )
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue *  pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeULong, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromULong(value);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return SUCCEEDED(pbResult);
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBFLOAT>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBFLOAT & value )
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue *  pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeFloat, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromFloat(value);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return SUCCEEDED(pbResult);
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBDOUBLE>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBDOUBLE & value )
{
    PBIArray * pThis = GetArray( );

    if (!pThis)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue *  pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeDouble, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromDouble(value);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return SUCCEEDED(pbResult);
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBString>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBString & value )
{
    PBIArray * pThis = GetArray( );
    PBIString * pString = (PBIString *)GetInterfaceFrom(value);

    if (!pThis || !pString)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue * pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeString, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromString(pString);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return (SUCCEEDED(pbResult));
}


//----------------------------------------------------------------------------

PBBOOL PBArray<PBDecimal>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBDecimal & value )
{
    PBIArray * pThis = GetArray( );
    PBIDecimal * pDecimal = (PBIDecimal *)GetInterfaceFrom(value);

    if (!pThis || !pDecimal)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue * pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeDecimal, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromDecimal(pDecimal);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return (SUCCEEDED(pbResult));
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBDate>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBDate & value )
{
    PBIArray * pThis = GetArray( );
    PBIDate * pDate = (PBIDate *)GetInterfaceFrom(value);

    if (!pThis || !pDate)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue * pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeDate, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromDate(pDate);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return (SUCCEEDED(pbResult));
}


//----------------------------------------------------------------------------

PBBOOL PBArray<PBTime>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBTime & value )
{
    PBIArray * pThis = GetArray( );
    PBITime * pTime = (PBITime *)GetInterfaceFrom(value);

    if (!pThis || !pTime)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue * pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeTime, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromTime(pTime);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return (SUCCEEDED(pbResult));
}


//----------------------------------------------------------------------------

PBBOOL PBArray<PBDateTime>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBDateTime & value )
{
    PBIArray * pThis = GetArray( );
    PBIDateTime * pDateTime = (PBIDateTime *)GetInterfaceFrom(value);

    if (!pThis || !pDateTime)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue * pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeDateTime, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromDateTime(pDateTime);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return (SUCCEEDED(pbResult));
}


//----------------------------------------------------------------------------

PBBOOL PBArray<PBBlob>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBBlob & value )
{
    PBIArray * pThis = GetArray( );
    PBIBlob * pBlob = (PBIBlob *)GetInterfaceFrom(value);

    if (!pThis || !pBlob)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue * pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeBlob, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromBlob(pBlob);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return (SUCCEEDED(pbResult));
}

//----------------------------------------------------------------------------

PBBOOL PBArray<PBInstance>::SetValueAt(
    PBLONG * pOffsets,
    PBLONG dims,
    const PBInstance & value )
{
    PBIArray * pThis = GetArray( );
    PBIInstance * pInstance = (PBIInstance *)GetInterfaceFrom(value);

    if (!pThis || !pInstance)
        return PBFALSE;

    PBRESULT pbResult = PBRESULT_OK;

    PBISession * pSession = NULL;
    PBIValue * pValue = NULL;

    pbResult = pThis->GetSession(&pSession);

    if (SUCCEEDED(pbResult))
        pbResult = CreatePBIValue(PBValueTypeInstance, pSession, &pValue);

    if (SUCCEEDED(pbResult))
        pbResult = pValue->SetValueFromInstance(pInstance);

    if (SUCCEEDED(pbResult))
        pbResult = pThis->SetValueAt(pOffsets, dims, pValue);

    if (pValue)     pValue->Release( );
    if (pSession)   pSession->Release( );

    return (SUCCEEDED(pbResult));
}


