using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoRowLoad
{
	class Program
	{
		static void Main(string[] args)
		{
			var loaddirs = new[] 
			{ 
				@"E:\PROJECTS\DKVSOURCESFINALEDITION_v20\ReportWebSite\ImgContent", 
				@"E:\PROJECTS\DKVSOURCESFINALEDITION_v20\ReportWebSite\ImgFree" 
			};
			var connectionString = @"data source=DESKTOP-KCF43RE;initial catalog=GUKV__20201003_v20;persist security info=True;user id=sa;password=sql;MultipleActiveResultSets=True;App=GUKVReportApp;Max Pool Size=32000;Connect Timeout=100";
			var connection = new SqlConnection(connectionString);
            connection.Open();
			var tran = connection.BeginTransaction();

			var dirnum = 0;
			foreach (var dir in loaddirs)
			{
				dirnum++;
				var filenpp = 0;
				var allfiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
				foreach (var photofilename in allfiles)
				{
					filenpp++;
					Console.WriteLine("dir=" + dirnum + ":  " + filenpp + "/" + allfiles.Length);

					var photofilebytes = File.ReadAllBytes(photofilename);

					var sql = "insert into photorow(photofilename, photofilebytes) values(@photofilename, @photofilebytes)";
					SqlCommand cmd = new SqlCommand(sql, connection, tran);
					cmd.Parameters.Add(new SqlParameter("photofilename", photofilename));
					cmd.Parameters.Add(new SqlParameter("photofilebytes", photofilebytes));
					cmd.ExecuteNonQuery();
					cmd.Dispose();
				}
			}

			tran.Commit();
			connection.Close();
		}
    }
}
