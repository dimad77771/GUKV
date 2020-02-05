using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// Creates a name for, and creates a zero-size temporary file. The file is deleted when .Dispose()
/// method is called.
/// </summary>
public class TempFile : IDisposable
{
    private readonly string _fileName;

	public TempFile()
	{
        _fileName = Path.GetTempFileName();
	}

    /// <summary>
    /// Get the name of the temporary file
    /// </summary>
    public string FileName { get { return _fileName; } }

    #region IDisposable Members

    public void Dispose()
    {
        File.Delete(_fileName);
    }

    #endregion
}