using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.Configuration;

namespace ExtDataEntry.Models
{
    public static class LLLLhotorowUtils
    {
        public static byte[] Read(string photofilename, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            //System.IO.File.AppendAllText(@"C:\inetpub\wwwroot\gukv\Test\log.txt", "Read: photofilename=" + photofilename + "\n");

            if (!File.Exists(photofilename))
			{
                return new byte[0];
			}

            var bytes = File.ReadAllBytes(photofilename);
            return bytes;
        }

        public static bool Exists(string photofilename, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            return File.Exists(photofilename);
        }

        public static string[] GetFiles(string photofiledir, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            if (!Directory.Exists(photofiledir))
			{
                return new string[0];
			}
            var files = Directory.GetFiles(photofiledir);
            return files;
        }

        public static void Write(string photofilename, byte[] photofilebytes, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            var dir = Path.GetDirectoryName(photofilename);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllBytes(photofilename, photofilebytes);
        }

        public static void Copy(string photofilenameSrc, string photofilenameDst, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            if (File.Exists(photofilenameSrc))
            { 
                var dir = Path.GetDirectoryName(photofilenameDst);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.Copy(photofilenameSrc, photofilenameDst, true);
            }
        }

        public static void Delete(string photofilename, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            if (File.Exists(photofilename))
            {
                File.Delete(photofilename);
            }
        }

        public static void DeleteAll(string photofiledir, SqlConnection connectionSql, SqlTransaction sqlTransaction = null)
        {
            if (Directory.Exists(photofiledir))
			{
                foreach (string file in Directory.GetFiles(photofiledir))
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        public static string ImgFreeSquareRootFolder
        {
            get
            {
                return WebConfigurationManager.AppSettings["ImgFreeSquareRootFolder"];
            }
        }

        public static string ImgContentRootFolder
        {
            get
            {
                return WebConfigurationManager.AppSettings["ImgContentRootFolder"];
            }
        }
    }
}
