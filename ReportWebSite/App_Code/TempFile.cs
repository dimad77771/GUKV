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
        System.IO.File.AppendAllText(@"c:\tmp\debug333.log", Path.GetTempPath());
        _fileName = Path.GetTempFileName();
	}

    /// <summary>
    /// Get the name of the temporary file
    /// </summary>
    public string FileName { get { return _fileName; } }

    /// <summary>
    /// Creates a tempoary file which is a copy of some existing file
    /// </summary>
    /// <param name="existingFileName">Name of the existing file</param>
    /// <returns>The created temporary file</returns>
    public static TempFile FromExistingFile(string existingFileName)
    {
        TempFile tempFile = new TempFile();

        File.Copy(existingFileName, tempFile.FileName, true);

        return tempFile;
    }

    #region IDisposable Members

    public void Dispose()
    {
        File.Delete(_fileName);
    }

    #endregion
}