using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoRowLoad
{
	class Program_load
	{
		static void Main(string[] args)
		{
			//var connectionString = @"data source=DESKTOP-KCF43RE;initial catalog=GUKV__20201003_v20;persist security info=True;user id=sa;password=sql;MultipleActiveResultSets=True;App=GUKVReportApp;Max Pool Size=32000;Connect Timeout=100";
			var connectionString = @"Data Source=DKV-EIS\DKVSQL2016;Initial Catalog=GUKV;UID=sa;PWD=DKV135790; Max Pool Size=32767; App=GUKVwebApp";

			var connection = new SqlConnection(connectionString);
            connection.Open();

			var filenpp = 0;
			using (var cmd = new SqlCommand("select photofilename, photofilebytes from photorow", connection))
			{
				using (SqlDataReader r = cmd.ExecuteReader())
				{
					while (r.Read())
					{
						filenpp++;
						if (filenpp % 100 == 0 || filenpp < 100)
						{
							Console.WriteLine("filenpp=" + filenpp);
						}

						var photofilename = r.GetString(0);
						var photofilebytes = r.GetSqlBytes(1).Value;

						var dir = Path.GetDirectoryName(photofilename);

						if (!Directory.Exists(dir))
						{
							Directory.CreateDirectory(dir);
						}
						File.WriteAllBytes(photofilename, photofilebytes);
					}
					r.Close();
				}
			}
			connection.Close();

			Console.WriteLine("END");
		}
    }
}
