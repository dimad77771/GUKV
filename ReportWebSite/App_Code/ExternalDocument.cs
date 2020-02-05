using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.IO;

public static class ExternalDocument
{
    /// <summary>
    /// Creates a copy of the specified file and assigns it a unique name suitable for external_document
    /// table usage. The return value contains the full path to the unique file name.
    /// </summary>
    public static string Copy(string originalFile, bool temporary)
    {
        string targetFile = GenerateExternalDocumentName(originalFile, temporary);
        File.Copy(originalFile, targetFile, true);
        return targetFile;
    }

    /// <summary>
    /// Saves content of the specified stream into a file with a unique name suitable for external_document
    /// table usage. The return value contains the full path to the unique file name.
    /// </summary>
    public static string Copy(Stream originalFileContent, string originalFileName, bool temporary)
    {
        string targetFile = GenerateExternalDocumentName(originalFileName, temporary);
        using (FileStream stream = new FileStream(targetFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
        {
            originalFileContent.CopyTo(stream);
        }
        return targetFile;
    }

    /// <summary>
    /// "Commits" an external document file held in the temporary storage into the main storage by moving it.
    /// Returns the updated full path to the file in the main storage. The file name does not change. If the
    /// file does not belong to the temporary storage, nothing happens (no error is generated) and the input
    /// file name is returned as-is.
    /// </summary>
    public static string Commit(string tempFileName)
    {
        if (string.IsNullOrEmpty(tempFileName))
            return tempFileName;

        string path = Path.GetDirectoryName(tempFileName);
        if (path != GetExternalDocumentStorePath(true))
            return tempFileName;

        string targetFile = Path.Combine(GetExternalDocumentStorePath(false), Path.GetFileName(tempFileName));
        File.Move(tempFileName, targetFile);
        return targetFile;
    }

    /// <summary>
    /// Generate a unique file name suitable for external_document table usage. The parameter value of
    /// originalFile may or may not be used while generating the unique file name. The results of this
    /// function call are non-repeatable, that is when called repeatedly with the same parameter, the
    /// returning value will be distinctly different. This function generates the file name only, it
    /// does not create the file itself.
    /// </summary>
    public static string GenerateExternalDocumentName(string originalFile, bool temporary)
    {
        return Path.Combine(GetExternalDocumentStorePath(temporary), Guid.NewGuid().ToString("N"))
            + Path.GetExtension(originalFile);
    }

    public static string GetExternalDocumentStorePath(bool temporary)
    {
        string path = WebConfigurationManager.AppSettings["ExternalDocumentsPath"];
        if (temporary)
            path = Path.Combine(path, "Temp");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }
}
