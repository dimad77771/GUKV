/*
  TestBase.H
  Test-Tool-API the abstract C++ Interface (ie, protocol definition).

  ------------------------------------------------------------------
  All functions return a SHORT value, which is the result code.
  The result code is one of PBTest_ERROR_*** described below.
  The only guarantee with these values is that ZERO is NO-Error.
  ------------------------------------------------------------------
  Notice: this follows the PowerBuilder standard in that all
  public interfaces are compatible with COM IUnknown.
  ------------------------------------------------------------------

  Author:	Reed Shilts			Dec-96
  Copyright (C) 1996, Powersoft Corporation. All Rights Reserved.
  Copyright (C) 1997, Powersoft Corporation. All Rights Reserved.
 */
#ifndef _INC_TESTBASE_H
#define _INC_TESTBASE_H

//#define DllExport __declspec( dllexport )



// ------------------------------------------------------------------
// Errors which may be returned.
// ------------------------------------------------------------------
#define  PBTest_ERROR_None			0	// guaranteed to be zero
#define  PBTest_ERROR_Unknown		1
#define  PBTest_ERROR_BadHwnd		2
#define  PBTest_ERROR_NotControl	3
#define  PBTest_ERROR_NotDW			4
#define  PBTest_ERROR_NotRTE		5




// ------------------------------------------------------------------
// Create one of these babies (like a factory)
// If you use "Create_PBTest()", you MUST use "ptr->Release()" to destroy.
//
// Can also create the non-general interfaces with ObjectFactory()
// off of the general interface.
//
// This function exists because an external user may have a different
// memory allocator, and mixing them would be bad....
// ------------------------------------------------------------------
class PBTest_General;		// Forward Declaration needed

extern "C" {
  PBTest_General FAR * Create_PBTest();
}




// ------------------------------------------------------------------
// General (root level) interface
// ------------------------------------------------------------------
class /*DllExport*/ PBTest_General
{
public:
	// ------------------------------------------
	// PowerBuilder standard beginning (IUnknown Compatible)
	// ------------------------------------------
	virtual ULONG STDMETHODCALLTYPE QueryInterface(
		/* [in] */			REFIID			riid,
		/* [iid_is][out] */ void FAR * FAR *ppvObject) = 0;

	virtual ULONG STDMETHODCALLTYPE AddRef( void) = 0;

	virtual ULONG STDMETHODCALLTYPE Release( void) = 0;


	// ------------------------------------------
    // Custom methods...
    // ------------------------------------------
    virtual SHORT VersionDetails(
        /* [out] */ LONG  FAR * plMajorVersion,
        /* [out] */ LONG  FAR * plMinorVersion,
        /* [out] */ LONG  FAR * plMaintLevel	= NULL,
        /* [out] */ LONG  FAR * plBuildNumber	= NULL,
        /* [out] */ LONG  FAR * plFlags			= NULL
		) = 0;


    virtual SHORT IsPowerBuilderObject(
        /* [in] */  HWND      hwndToTest,
        /* [out] */ BOOL FAR *pbIsTrue
        ) = 0;

    virtual SHORT ObjectClassName(
        /* [in]  */ HWND    hwndToLookup,
        /* [in]  */ LONG    cbMaxNameLen,
        /* [out] */ TCHAR FAR *achClassName		// caller allocated memory
        ) = 0;

    virtual SHORT ObjectIUnknown(
        /* [in]  */ HWND                hwndToLookup,
        /* [out] */ IUnknown FAR* FAR*  ppObject) = 0;

	// Factory to create ANOTHER interface based on HWND and CLSID
	// Similar to QueryInterface, the caller must Release() when done.
	// The '*ppvObject' will be one of the PBTest_**** defined below.
	virtual SHORT ObjectFactory(
		/* [in]	 */ HWND				hwndToUse,
		/* [in]	 */ REFIID				riid,
		/* [iid_is][out] */ VOID FAR * FAR *ppvObject
		) = 0;

protected:
    PBTest_General();
    virtual ~PBTest_General();

private:
    // protection from what shouldn't happen
    PBTest_General( const PBTest_General & src );   // copy ctor
    PBTest_General & operator=(PBTest_General &);   // class assignment
};




// ------------------------------------------------------------------
// DataWindow specific
// ------------------------------------------------------------------
class /*DllExport*/ PBTest_DW
{
public:
	// ------------------------------------------
	// PowerBuilder standard beginning (IUnknown Compatible)
	// ------------------------------------------
	virtual ULONG STDMETHODCALLTYPE QueryInterface(
		/* [in] */			REFIID			riid,
		/* [iid_is][out] */ void FAR * FAR *ppvObject) = 0;

	virtual ULONG STDMETHODCALLTYPE AddRef( void) = 0;

	virtual ULONG STDMETHODCALLTYPE Release( void) = 0;


    // ------------------------------------------
    // Custom methods...
    // ------------------------------------------
    virtual SHORT Reset(                     // can be called at any time
        /* [in]  */ HWND hwndToUse ) = 0;

    virtual SHORT get_Column(
        /* [out] */ LONG FAR * pColumn) = 0;

    virtual SHORT put_Column(
        /* [in]  */ LONG dwColumn) = 0;

    virtual SHORT get_Row(
        /* [out] */ LONG FAR * pRow ) = 0;

    virtual SHORT put_Row(
        /* [in]  */ LONG dwRow) = 0;

    virtual SHORT get_RowCount(
        /* [out] */ LONG FAR * pRow ) = 0;

    virtual SHORT get_BandAtPointer(
        /* [in]  */      LONG    cbResultBufLen,
        /* [in] [out] */ TCHAR FAR *achResult // caller allocated memory
        ) = 0;


    virtual SHORT get_ItemAsNumber(
        /* [out] */ DOUBLE FAR *pResult) = 0;

    virtual SHORT get_ItemAsString(
        /* [in]  */      LONG      cbResultBufLen,
        /* [in] [out] */ TCHAR FAR *achResult) = 0;   // caller allocated memory

    virtual SHORT Describe(         // unclear about freeing this memory....
        /* [in]  */ LPCTSTR      pszSource,
        /* [out] */ LPTSTR FAR * ppszResult ) = 0;

    virtual SHORT GetChild(
        /* [in]  */ LPCTSTR    pszChildName,
        /* [out] */ HWND FAR *phwndChildDW ) = 0;

    virtual SHORT GetItemDateTime(
        /* [in]  */ LONG      dwRow,
        /* [in]  */ LONG      dwColumn,
        /* [out] */ LONG FAR *pdwYear,
        /* [out] */ LONG FAR *pdwMonth,
        /* [out] */ LONG FAR *pdwDay,
        /* [out] */ LONG FAR *pdwHour,
        /* [out] */ LONG FAR *pdwMin,
        /* [out] */ LONG FAR *pdwSec,
        /* [out] */ LONG FAR *pdwMicroSeconds ) = 0;


    virtual SHORT GetObjectAtPointer(
        /* [in]  */     LONG      cbResultBufLen,
        /* [in][out] */ TCHAR FAR *achResult     // caller allocated memory
        ) = 0;

    virtual SHORT SaveAs(
        /* [in]  */ LONG	dwSaveAsType,
        /* [in]  */ BOOL	bIncludeHeadings,
        /* [in]  */ LPCTSTR	pszFileName ) = 0;

    virtual SHORT SelectRow(
        /* [in]  */ LONG	dwRowToSelect,
        /* [in]  */ BOOL	bSelectState ) = 0;

    virtual SHORT SelectedRow(
        /* [out] */ LONG FAR *pdwSelectedRow ) = 0;

    virtual SHORT IsSelected(
        /* [in]  */ LONG		dwRowToSelect,
        /* [out] */ BOOL FAR *	pbIsTrue
        ) = 0;

    virtual SHORT SetAllEvents( void ) = 0;


protected:
    PBTest_DW();                               // default ctor
    virtual ~PBTest_DW();

private:
    // protection from what shouldn't happen
    PBTest_DW( const PBTest_DW & src );   // copy ctor
    PBTest_DW & operator=(PBTest_DW &);   // class assignment
};



// ------------------------------------------------------------------
// RichTextEdit specific
// ------------------------------------------------------------------
class /*DllExport*/ PBTest_RTE
{
public:
	// ------------------------------------------
	// PowerBuilder standard beginning (IUnknown Compatible)
	// ------------------------------------------
	virtual ULONG STDMETHODCALLTYPE QueryInterface(
		/* [in] */			REFIID			riid,
		/* [iid_is][out] */ void FAR * FAR *ppvObject) = 0;

	virtual ULONG STDMETHODCALLTYPE AddRef( void) = 0;

	virtual ULONG STDMETHODCALLTYPE Release( void) = 0;


    // ------------------------------------------
    // Custom methods...
    // ------------------------------------------
    virtual SHORT Reset(                     // can be called at any time
        /* [in]  */ HWND hwndToUse ) = 0;

    virtual SHORT get_LineCount(
        /* [out] */ LONG FAR *pLineCount ) = 0;

    virtual SHORT GetSelection(
        /* [out] */ LONG FAR *pdwFromLine,
        /* [out] */ LONG FAR *pdwFromColumn,
        /* [out] */ LONG FAR *pdwToLine,
        /* [out] */ LONG FAR *pdwToColumn ) = 0;

    virtual SHORT SetSelection(
        /* [in]  */ LONG dwFromLine,
        /* [in]  */ LONG dwFromColumn,
        /* [in]  */ LONG dwToLine,
        /* [in]  */ LONG dwToColumn ) = 0;


    virtual SHORT LineLength(
        /* [in]  */ LONG      dwLine,
        /* [out] */ LONG FAR *pLineLength ) = 0;

    virtual SHORT GetLine(
        /* [in]  */ LONG dwLine,
        /* [in]  */ LONG cbResultBufLen,
        /* [in][out] */ TCHAR FAR *achLineText) = 0;     // caller allocated memory

    virtual SHORT GetCharFormat(
        /* [out] */ LONG FAR *pdwRetValue,
        /* [out] */ LONG FAR *pdwMask,
        /* [out] */ LONG FAR *pdwEffects,
        /* [out] */ LONG FAR *pdwHeight,
        /* [out] */ LONG FAR *pdwColorRef,
        /* [out] */ LONG FAR *pdwCharSet,
        /* [out] */ LONG FAR *pdwPitchAndFamily,
        /* [in]  */ LONG      cbMaxFaceNameLen,
        /* [out] */ TCHAR FAR *achFaceName ) = 0;        // caller allocated memory


    virtual SHORT SetInsertionPoint(
        /* [in]  */ LONG Line,
        /* [in]  */ LONG Column ) = 0;

    virtual SHORT GetInsertionPoint(
        /* [out] */ LONG FAR *pLine,
        /* [out] */ LONG FAR *pColumn ) = 0;


    virtual SHORT GetCurrentBand(
        /* [out] */ LONG FAR *pdwBand ) = 0;


    virtual SHORT SetCurrentBand(
        /* [in]  */ LONG dwAttributes ) = 0;     // FNX_RTEBAND_xxxxx

protected:
    PBTest_RTE();                          // default ctor
    virtual ~PBTest_RTE();

private:
    // protection from what shouldn't happen
    PBTest_RTE( const PBTest_RTE & src );     // copy ctor
    PBTest_RTE & operator=(PBTest_RTE &);     // class assignment
};


#endif  // _INC_TESTBASE_H

//--eof--

