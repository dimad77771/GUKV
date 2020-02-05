using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ExtDataEntry.Properties;

namespace ExtDataEntry.Models
{
    public class FileAttachment
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public bool IsFolder { get; set; }
        
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public DateTime LastModified { get; set; }

        private static string GetFolderPath(string scope, int recordID)
        {
            return Path.Combine(Settings.Default.SharedFilesDirectory, scope, recordID.ToString());
        }

        public static IEnumerable<FileAttachment> Select(string scope, int recordID)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");

            yield return new FileAttachment()
            {
                ID = "\\ROOT",
                ParentID = "\\NONE",
                IsFolder = true,
                Name = "~\\",
            };

            if (recordID <= 0)
                yield break;

            string path = GetFolderPath(scope, recordID);
            if (!Directory.Exists(path))
                yield break;

            foreach (string fileName in Directory.GetFiles(path))
            {
                yield return new FileAttachment()
                {
                    ID = Path.GetFileName(fileName),
                    ParentID = "\\ROOT",
                    Name = Path.GetFileName(fileName),
                    Image = File.ReadAllBytes(fileName),
                    LastModified = (new FileInfo(fileName)).LastWriteTime,
                };
            }
        }

        public static void Insert(string scope, int recordID, string Name, byte[] Image)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");
            if (recordID <= 0)
                throw new ArgumentOutOfRangeException("recordID");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("fileName must have a value");
            if (Image == null || Image.Length == 0)
                throw new ArgumentException("image must have a value");

            string path = GetFolderPath(scope, recordID);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllBytes(Path.Combine(path, Name), Image);
        }

        public static void Delete(string scope, int recordID, string id /* attachmentID */)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");
            if (recordID <= 0)
                throw new ArgumentOutOfRangeException("recordID");
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("id must have a value");

            string name = id;
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("fileName must have a value");

            string path = GetFolderPath(scope, recordID);
            if (Directory.Exists(path))
                File.Delete(Path.Combine(path, name));
        }

        internal static void DeleteAll(string scope, int recordID)
        {
            if (string.IsNullOrEmpty(scope))
                throw new ArgumentException("scope must have a value");
            if (recordID <= 0)
                throw new ArgumentOutOfRangeException("recordID");

            string path = GetFolderPath(scope, recordID);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
    }
}
