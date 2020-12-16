//				Copyright Sybase, Inc. 1996
//
//		Sybase, Inc. ("Sybase") claims copyright in this
//		program and documentation as an unpublished work, versions of 
//		which were first licensed on the date indicated in the foregoing
//		notice.  Claim of copyright does not imply waiver of Sybase's
//		other rights.
//
//-----------------------------------------------------------------------------
//
//	pbgcpprt.h
//
//	C++ wrapper classes for the public ("PBI") runtime interfaces.
//
//	PowerBuilder exposes a set of public interfaces on the runtime
//	library ("pbvm").  These interfaces, declared in header files
//	pbgrunif.h and pbgtypif.h, provide access to a limited set of
//	runtime assets and functions to developers working outside of the 
//	PowerBuilder development environment.
//
//	For convenience, several of these interfaces have been further
//	wrapped inside of concrete C++ classes, declared below. 
//
//	Note that these classes are intended for use in the context of C++ code
//	emitted by the PowerBuilder C++ generator.  The implementations for these
//	classes depends on the presence of code segments provided by the generator,
//	and they will not function properly if used independently.
//
//-----------------------------------------------------------------------------

#ifndef PBGCPPRT_H
#define PBGCPPRT_H

//-----------------------------------------------------------------------------
//	Forward declarations for the PowerBuilder runtime interfaces.
//-----------------------------------------------------------------------------

class PBIUnknown;
class PBIString;
class PBIDateTime;
class PBIDate;
class PBITime;
class PBIDecimal;
class PBIBlob;
class PBIArrayBounds;
class PBIArrayBoundsList;
class PBIArray;
class PBISession;
class PBIInstance;

//-----------------------------------------------------------------------------
//	Forward declarations for the concrete C++ wrapper classes.
//-----------------------------------------------------------------------------

class PBUnknown;
class PBString;
class PBDateTime;
class PBDate;
class PBTime;
class PBDecimal;
class PBBlob;
class PBArrayBounds;
class PBArrayBoundsList;
template <class T> class PBArray;
class PBSession;
class PBInstance;

//-----------------------------------------------------------------------------
//	PBUnknown
//
//	Base class of all interface wrapper types.  Manages the pointer to the
//	underlying interface instance for all concrete objects.  
//
//	Intended for use only as a base class.  No public interface, cannot
//	be independently created or used.
//-----------------------------------------------------------------------------

class PBUnknown
{
  protected:

	//	Constructors, destructor, operator =

	PBUnknown( );
	PBUnknown( PBIUnknown * );
	PBUnknown( const PBUnknown & );
 
	virtual ~PBUnknown( );

	PBUnknown & operator = ( const PBUnknown & );

	//	Interface management.

	PBBOOL				SetInterface( PBIUnknown * );
 
	PBIUnknown *		GetInterface( );
	PBIUnknown *		GetInterface( ) const;

	static PBBOOL   	SetInterfaceOn( PBUnknown &, PBIUnknown * );
	static PBIUnknown * GetInterfaceFrom( const PBUnknown & );

  private:

	PBIUnknown *		m_pInterface;
};

//-----------------------------------------------------------------------------
//	PBUnknown inline function implementations.
//-----------------------------------------------------------------------------

inline PBIUnknown * PBUnknown::GetInterface( )
{
	return m_pInterface;
}

inline PBIUnknown * PBUnknown::GetInterface( ) const
{
	return m_pInterface;
}

inline PBIUnknown * PBUnknown::GetInterfaceFrom( const PBUnknown & other )
{
	return other.m_pInterface;
}

//-----------------------------------------------------------------------------
//	PBString
//
//	Simple null-terminated sequence of characters.  
//
//	The individual characters in the string are declared to be PBCHAR.
//	PBCHAR is defined in header pbgtypes.h to be either an 8-bit ASCII
//	character, or a 16-bit wchar_t.  The choice of which depends on whether
//	symbol UNICODE has been defined.
//-----------------------------------------------------------------------------

class PBString : public PBUnknown
{
  public:

	//	Constructors, destructor, operator =
	//
	//	A null-constructed PBString will NOT be null-valued, but will
	//	contain an empty string.
	//	When a PBString is constructed from a PBCHAR *, the PBString will
	//	make its own copy of the string contents.  The caller retains 
	//	ownership of the argument string.

    PBString( );
    PBString( const PBCHAR * );
    PBString( const PBString & );

    virtual ~PBString( );

    PBString & operator = ( const PBString & other );

	//	Equality, inequality.

	PBBOOL operator == ( const PBString & other ) const;
	PBBOOL operator != ( const PBString & other ) const;

	//	Cast the PBString to PBCHAR *
	//
	//	The PBString retains ownership of the character array pointed to.
	//	This is only intended to simplify using PBStrings with runtime
	//	libraries or other functions expecting a character pointer.
	//	Do not use it to modify or delete the string value!

	operator const PBCHAR * ( ) const;

	//	Accessor functions.
	//	
	//	GetValue has the same caveats as the cast operator (see above).
	//	For GetValueAt( ) and SubString( ), it's the callers responsibility
	//	to insure that the offsets are in bounds.  The values of the
	//	out parameters are not defined if the offsets are not in bounds.
	//	All offsets are zero-based.

	PBBOOL	GetValue( const PBCHAR * & value ) const;
	PBBOOL	GetLength( PBULONG & length ) const;
	PBBOOL	GetValueAt( PBULONG offset, PBCHAR & value ) const;
	PBBOOL	SubString( PBULONG from, PBULONG to, PBString & subString) const;

	//	IsNull( ) is true if the string has no actual value.  Note that
	//	an empty string is NOT null valued.
	//	CompareTo( ) populates out argument "comparison" with a value 
	//	greater than, equal to, or less than zero depending on whether "this"
	//	PBString is greater than, equal to, or less than "other" in value.
	
	PBBOOL	IsNull( PBBOOL & isNull ) const;
	PBBOOL	CompareTo( const PBString & other, PBINT & comparison ) const;

	//	PBStrings are valid if the underlying interface pointer is not null.

	PBBOOL	IsValid( ) const;

  protected:

    PBIString *     GetString( );
	PBIString *     GetString( ) const;
};

//-----------------------------------------------------------------------------
//	PBString inline method implementations
//-----------------------------------------------------------------------------

inline PBBOOL PBString::operator != ( const PBString & other ) const
{
    return !(*this == other);
}

inline PBIString * PBString::GetString( )
{
    return (PBIString *)PBUnknown::GetInterface( );
}

inline PBIString * PBString::GetString( ) const
{
    return (PBIString *)PBUnknown::GetInterface( );
}


//-----------------------------------------------------------------------------
//	PBDateTime
//	
//	Contains a date and a time. 
//-----------------------------------------------------------------------------

class PBDateTime : public PBUnknown
{
  public:

	//	Constructors, destructor, operator =

    PBDateTime( );
    PBDateTime( const PBDate & date, const PBTime & time );
    PBDateTime( const PBDateTime & );

    virtual ~PBDateTime( );

    PBDateTime & operator = ( const PBDateTime & );

	//	Get the constituent date and/or time

	PBBOOL	GetDate( PBDate & date ) const;
	PBBOOL	GetTime( PBTime & time ) const;

	//	IsNull( ) is true if the date and time have no actual value
	//	CompareTo( ) populates out argument "comparison" with a value 
	//	greater than, equal to, or less than zero, depending on whether
	//	"this" PBDateTime is earlier than, equal to, or later than "other".

	PBBOOL	IsNull( PBBOOL & isNull ) const;
	PBBOOL	CompareTo( const PBDateTime & other, PBINT & comparison) const;

	//	A PBDateTime is valid if its underlying interface pointer is not null.

	PBBOOL	IsValid( ) const;

  protected:

    PBIDateTime *   GetDateTime( );
    PBIDateTime *   GetDateTime( ) const;
};


//-----------------------------------------------------------------------------
//	PBDateTime inline method implementations
//-----------------------------------------------------------------------------

inline PBIDateTime * PBDateTime::GetDateTime( )
{
    return (PBIDateTime *)PBUnknown::GetInterface( );
}

inline PBIDateTime * PBDateTime::GetDateTime( ) const
{
    return (PBIDateTime *)PBUnknown::GetInterface( );
}


//-----------------------------------------------------------------------------
//	PBDate
//	
//	A calendar date.
//-----------------------------------------------------------------------------

class PBDate : public PBUnknown
{
  public:

	//	Constructors, destructor, operator =

    PBDate( );
    PBDate( PBSHORT iYear, PBINT iMonth, PBINT iDayOfMonth );
    PBDate( const PBDate & );

    virtual ~PBDate( );

    PBDate & operator = ( const PBDate & );

	//	Get the year, month or day.  The day can be retrieved either
	//	as the day of the month or day of the week.  

	PBBOOL	GetYear( PBSHORT & year ) const;
	PBBOOL	GetMonth( PBINT & month ) const;
	PBBOOL	GetDayOfMonth( PBINT & day ) const;
	PBBOOL	GetDayOfWeek( PBINT & day ) const;

	//	Compute the difference in days between "this" date and "other".
	//	The difference will be positive if "other" is later than "this".

	PBBOOL	DaysAfter( const PBDate & other, PBLONG & days ) const;

	//	Set out argument "relativeDate" to the date value "days" after
	//	the current value of "this".

	PBBOOL	RelativeDate( PBLONG days, PBDate & relativeDate ) const;

	//	IsNull( ) is true if the date has no actual value.
	//	CompareTo( ) populates out argument "comparison" with a value 
	//	greater than, equal to, or less than zero, depending on whether 
	//	"this" PBDate is earlier than, equal to, or later than "other".

    PBBOOL  IsNull( PBBOOL & isNull ) const;
    PBBOOL  CompareTo( const PBDate & other, PBINT & comparison ) const;
	
	//	A PBDate is valid if its underlying interface pointer is not null.

	PBBOOL	IsValid( ) const;

  protected:

    PBIDate *   	GetDate( );
    PBIDate *   	GetDate( ) const;
};


//-----------------------------------------------------------------------------
//	PBDate inline method implementations.
//-----------------------------------------------------------------------------

inline PBIDate * PBDate::GetDate( )
{
    return (PBIDate *)PBUnknown::GetInterface( );
}

inline PBIDate * PBDate::GetDate( ) const
{
    return (PBIDate *)PBUnknown::GetInterface( );
}


//-----------------------------------------------------------------------------
//	PBTime
//
//	Time of day.
//-----------------------------------------------------------------------------

class PBTime : public PBUnknown
{
  public:

	//	Constructors, destructor, operator =

    PBTime( );
    PBTime( PBINT iHour, PBINT iMinute, PBINT iSecond );
    PBTime( const PBTime & );

    virtual ~PBTime( );

    PBTime & operator = ( const PBTime & );

	//	Get the hour, minute, or second element of the time.

	PBBOOL	GetHour( PBINT & hour ) const;
	PBBOOL	GetMinute( PBINT & minute ) const;
	PBBOOL	GetSecond( PBINT & second ) const;

	//	Compute the difference in seconds between "this" time and "other".
	//	The difference will be positive if "other" is later than "this".
	PBBOOL	SecondsAfter( const PBTime & other, PBLONG & seconds ) const;

	//	Set out argument "relativeTime" to the time value "seconds" after
	//	the current value of "this".

	PBBOOL	RelativeTime( PBLONG seconds, PBTime & relativeTime ) const;

	//	IsNull( ) is true if the time has no actual value.
	//	CompareTo( ) populates out argument "comparison" with a value
	//	greater than, equal to, or less than zero, depending on whether
	//	"this" PBTime is earlier than, equal to, or later than "other".

    PBBOOL  IsNull( PBBOOL & isNull ) const;
    PBBOOL  CompareTo( const PBTime & other, PBINT & comparison ) const;

	//	A PBTime is valid if its underlying interface pointer is not null.

	PBBOOL	IsValid( ) const;
 
  protected:

    PBITime *       GetTime( );
    PBITime *       GetTime( ) const;
};


//-----------------------------------------------------------------------------
//	PBTime inline method implementations
//-----------------------------------------------------------------------------

inline PBITime * PBTime::GetTime( )
{
    return (PBITime *)PBUnknown::GetInterface( );
}

inline PBITime * PBTime::GetTime( ) const
{
    return (PBITime *)PBUnknown::GetInterface( );
}


//-----------------------------------------------------------------------------
//	PBDecimal
//
//	A signed decimal number with up to 18 digits.
//-----------------------------------------------------------------------------

class PBDecimal : public PBUnknown
{
  public:

	//	Constructors, destructors, operator =

    PBDecimal( );
	PBDecimal( PBDOUBLE );
    PBDecimal( const PBDecimal & );

    virtual ~PBDecimal( );

    PBDecimal & operator = ( const PBDecimal & );

	//	Cast the PBDecimal to a PBDOUBLE

	operator PBDOUBLE( ) const;

	//	Get or set the decimal value from a PBDOUBLE

	PBBOOL 	GetValue( PBDOUBLE & value ) const;
	PBBOOL	SetValue( PBDOUBLE value );
	
	//	IsNull( ) is true if the decimal has no actual value.
	//	CompareTo( ) populates out argument "comparison" with a value 
	//	greater than, equal to, or less than zero, depending on whether
	//	"this" PBDecimal is greater than, equal to, or less than "other".

	PBBOOL	IsNull( PBBOOL & isNull ) const;
	PBBOOL	CompareTo( const PBDecimal & other, PBINT & comparison ) const;

	//	A PBDecimal is valid if its underlying interface pointer is not null.

	PBBOOL	IsValid( ) const;

  protected:

    PBIDecimal *    GetDecimal( );
    PBIDecimal *    GetDecimal( ) const;
};

//-----------------------------------------------------------------------------
//	PBIDecimal inline method implementations.
//-----------------------------------------------------------------------------

inline PBIDecimal * PBDecimal::GetDecimal( )
{
    return (PBIDecimal *)PBUnknown::GetInterface( );
}

inline PBIDecimal * PBDecimal::GetDecimal( ) const
{
    return (PBIDecimal *)PBUnknown::GetInterface( );
}


//-----------------------------------------------------------------------------
//	PBBlob
//
//	A binary large object.
//-----------------------------------------------------------------------------

class PBBlob : public PBUnknown
{
  public:

	//	Constructors, destructors, operator =

	PBBlob( );
	PBBlob( PBPVOID pData, PBULONG ulSize );
	PBBlob( const PBBlob & );

	virtual ~PBBlob( );

	PBBlob & operator = ( const PBBlob & );

	//	Get the size of the data stream and / or a pointer to
	//	the data stream.  The PBBlob retains ownership of the 
	//	data stream -- do not delete or modify the data pointed
	//	to by the data pointer.

	PBBOOL	GetSize( PBULONG & size ) const;
	PBBOOL 	GetData( PBPVOID & data ) const;

	//	Insert or extract binary data into the data stream.
	//	It is the callers responsibility to insure that the offsets
	//	are in bounds.  If out of bounds, the value of the out argument
	//	data stream is undefined.

	PBBOOL	InsertData( PBULONG offset, const PBBlob & data );
	PBBOOL	ExtractData( PBULONG start, PBULONG size, PBBlob & extract ) const;

	//	IsNull( ) is true when the blob does not contain any actual data.
	//	A PBBlob is valid if its underlying interface pointer is not null.

	PBBOOL	IsNull( PBBOOL & isNull ) const;
	PBBOOL	IsValid( ) const;

  protected:

	PBIBlob *		GetBlob( );
	PBIBlob *		GetBlob( ) const;
};

//-----------------------------------------------------------------------------
//	PBIBlob inline method implementations
//-----------------------------------------------------------------------------

inline PBIBlob * PBBlob::GetBlob( )
{
    return (PBIBlob *)PBUnknown::GetInterface( );
}

inline PBIBlob * PBBlob::GetBlob( ) const
{
    return (PBIBlob *)PBUnknown::GetInterface( );
}

 
//-----------------------------------------------------------------------------
//	PBArrayBounds
//
//	Defines the minimum and maximum number of entries that can exist in a
//	particular dimension of an array.
//-----------------------------------------------------------------------------

class PBArrayBounds : public PBUnknown
{
  public:

	//	Constructors, destructor, operator =

	PBArrayBounds( );
	PBArrayBounds( PBLONG upper, PBLONG lower );
	PBArrayBounds( const PBArrayBounds & );

	virtual ~PBArrayBounds( );

	PBArrayBounds & operator = ( const PBArrayBounds & );

	//	Get and / or set the upper and lower bounds of the array

	PBBOOL  GetUpperBound( PBLONG & ) const;
	PBBOOL  SetUpperBound( PBLONG );

	PBBOOL  GetLowerBound( PBLONG & ) const;
	PBBOOL  SetLowerBound( PBLONG );

	//	A PBArrayBounds is valid if its underlying interface pointer is not null.

	PBBOOL  IsValid( ) const;

  protected:
 
	PBIArrayBounds * GetArrayBounds( ) const;
	PBIArrayBounds * GetArrayBounds( );
};

//-----------------------------------------------------------------------------
//	PBArrayBounds inline method implementations.
//-----------------------------------------------------------------------------

inline PBIArrayBounds * PBArrayBounds::GetArrayBounds( ) const
{
        return (PBIArrayBounds *)GetInterface( );
}

inline PBIArrayBounds * PBArrayBounds::GetArrayBounds( )
{
        return (PBIArrayBounds *)GetInterface( );
}


//-----------------------------------------------------------------------------
//	PBArrayBoundsList
//
//	A list of array bounds.  Used to specify the bounds for all dimensions of
//	an array, where one list entry is found for each array dimension.
//-----------------------------------------------------------------------------

class PBArrayBoundsList : public PBUnknown
{
  public:

	//	Constructors, destructor, operator =

  	PBArrayBoundsList( );
	PBArrayBoundsList( const PBArrayBoundsList & );

	virtual ~PBArrayBoundsList( );

	PBArrayBoundsList & operator = ( const PBArrayBoundsList & );

	//	Add or remove a bounds instance from the list.

	PBBOOL	Add( const PBArrayBounds & );
	PBBOOL	Remove( const PBArrayBounds & );

	//	Get the number of bounds in the list.

	PBBOOL	GetCount( PBINDEX & ) const;

	//	Iterate through the list in either direction.
	//	These methods return PBFALSE when the end of the list
	//	is achieved.

	PBBOOL	GetFirst( PBArrayBounds & );
	PBBOOL	GetNext( PBArrayBounds & );

	PBBOOL	GetLast( PBArrayBounds & );
	PBBOOL	GetPrevious( PBArrayBounds & );

  protected:

	PBIArrayBoundsList *	GetArrayBoundsList( ) const;
	PBIArrayBoundsList *	GetArrayBoundsList( );
};

//-----------------------------------------------------------------------------
//	PBIArrayBoundsList inline method implementations.
//-----------------------------------------------------------------------------

inline PBIArrayBoundsList * PBArrayBoundsList::GetArrayBoundsList( ) const
{
        return (PBIArrayBoundsList *)GetInterface( );
}

inline PBIArrayBoundsList * PBArrayBoundsList::GetArrayBoundsList( )
{
        return (PBIArrayBoundsList *)GetInterface( );
}


//-----------------------------------------------------------------------------
//	PBArray<T>
//
//	A templated array type.  
//-----------------------------------------------------------------------------

template<class T> class PBArray : public PBUnknown
{
  public:

	//	Constructors, destructor, operator =

	PBArray( );
	PBArray( const PBArrayBoundsList & pBoundsList );
	PBArray( const PBArray<T> & );

	virtual ~PBArray( );

	PBArray<T> & operator = ( const PBArray<T> & );

	//	Get / set an array occurence.  The arguments are:
	//	o	An array of offsets, one per array dimension
	//	o	The number of offsets (and array dimensions)
	//	o	The value to set / populate

	PBBOOL  GetValueAt( PBLONG * pOffsets, PBLONG dims, T & value ) const;
	PBBOOL  SetValueAt( PBLONG * pOffsets, PBLONG dims, const T & value );

	//	Get the number of dimensions
	//	Get the bounds array

	PBBOOL  GetDimensions( PBINDEX & cDimensions ) const;
	PBBOOL  GetBounds( PBArrayBoundsList & pBoundsList ) const;

	//	A PBArray is valid if its underlying interface pointer is not null.

	PBBOOL  IsValid( ) const;

  protected:

	PBIArray * GetArray( ) const;
	PBIArray * GetArray( );
};

//-----------------------------------------------------------------------------
//	PBIArray<T> inline method implementations.
//-----------------------------------------------------------------------------

template<class T> inline PBIArray * PBArray<T>::GetArray( ) const
{
        return (PBIArray *)GetInterface( );
}

template<class T> inline PBIArray * PBArray<T>::GetArray( )
{
        return (PBIArray *)GetInterface( );
}

//-----------------------------------------------------------------------------
//  PBSession
//
//  Establishes a context for the other classes to operate within the
//  PowerBuilder runtime.
//-----------------------------------------------------------------------------

class PBSession : public PBUnknown
{
  public:

    //  Constructors, destructor, operator =

    PBSession( );
    PBSession( const PBSession & other );

    virtual ~PBSession( );

    PBSession & operator = ( const PBSession & other );

    //  A session is valid if its underlying interface is not null.

    PBBOOL IsValid( ) const;

  protected:

    PBISession * GetSession( ) const;
    PBISession * GetSession( );
};

//-----------------------------------------------------------------------------
//  PBSession inline function implementations
//-----------------------------------------------------------------------------

inline PBISession * PBSession::GetSession( ) const
{
    return (PBISession *)GetInterface( );
}

inline PBISession * PBSession::GetSession( )
{
    return (PBISession *)GetInterface( );
}

//-----------------------------------------------------------------------------
//  PBInstance
//
//  The base class for all generated object types.
//-----------------------------------------------------------------------------

class PBInstance : public PBUnknown
{
  public:

    //  Destructor.

    virtual ~PBInstance( );

    //  A PBInstance is valid if its underlying interface is not null.

    PBBOOL  IsValid( ) const;

    //  LastErrorMessage( ) will return any error messages created
    //  by the last operation on the PBInstance.

    PBString LastErrorMessage( ) const;

  protected:

    //  Constructors, operator =

    PBInstance( const PBSession &, const PBString & );
	PBInstance( const PBString &, const PBString & );
    PBInstance( const PBInstance & );

    PBInstance & operator = ( const PBInstance & );

    //  Interface management.

    PBIInstance * GetInstance( ) const;
    PBIInstance * GetInstance( );


    void LastErrorMessage( PBString );


  private:

    PBString m_LastErrorMessage;
};

//-----------------------------------------------------------------------------
//  PBInstance inline function implementations.
//-----------------------------------------------------------------------------

inline PBIInstance * PBInstance::GetInstance( ) const
{
    return (PBIInstance *)GetInterface( );
}

inline PBIInstance * PBInstance::GetInstance( )
{
    return (PBIInstance *)GetInterface( );
}

inline PBString PBInstance::LastErrorMessage( ) const
{
    return m_LastErrorMessage;
}

inline void PBInstance::LastErrorMessage( PBString strMessage )
{
    m_LastErrorMessage = strMessage;
}


#endif	//	PBGCPPRT_H
