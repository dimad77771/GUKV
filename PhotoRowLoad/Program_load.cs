//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PhotoRowLoad
//{
//	class Program_load
//	{
//		static void Main(string[] args)
//		{
//			var loaddirs = new[] 
//			{ 
//				//@"E:\PROJECTS\DKVSOURCESFINALEDITION_v20\ReportWebSite\ImgContent", 
//				//@"E:\PROJECTS\DKVSOURCESFINALEDITION_v20\ReportWebSite\ImgFree",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgContent",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgFree",



//				//@"I:\___BACKUP_2020_11_20\gukv\ImgContent\1NF",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgContent\1NFARENDA",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgContent\Arch",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgContent\Arenda",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgContent\Balans",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgContent\DOCUMENTPHOTO",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgContent\tmp",

//				//@"I:\___BACKUP_2020_11_20\gukv\ImgFree",


//				//@"I:\___BACKUP_2020_11_20\gukv\ImgFree\1NF",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgFree\Balans",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgFree\freecycle_step_documents",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgFree\free_square_current_stage_documents",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgFree\privatisat_documents",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgFree\reports1nf_arenda_dogcontinue_current_stage_documents",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgFree\reports1nf_arenda_dogcontinue_photos",
//				//@"I:\___BACKUP_2020_11_20\gukv\ImgFree\reports1nf_report_documents",

//				@"I:\___BACKUP_2020_11_20\gukv\ImgFree\Arch",
//			};

//			//var connectionString = @"data source=DESKTOP-KCF43RE;initial catalog=GUKV__20201003_v20;persist security info=True;user id=sa;password=sql;MultipleActiveResultSets=True;App=GUKVReportApp;Max Pool Size=32000;Connect Timeout=100";
//			var connectionString = @"Data Source=DKV-EIS\DKVSQL2016;Initial Catalog=GUKV;UID=sa;PWD=DKV135790; Max Pool Size=32767; App=GUKVwebApp";


//			var connection = new SqlConnection(connectionString);
//            connection.Open();
//			var tran = connection.BeginTransaction();

//			var dirnum = 0;
//			foreach (var dir in loaddirs)
//			{
//				dirnum++;
//				var filenpp = 0;
//				var allfiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
//				foreach (var photofilename in allfiles)
//				{
//					filenpp++;
//					Console.WriteLine("dir=" + dirnum + ":  " + filenpp + "/" + allfiles.Length);

//					var photofilebytes = File.ReadAllBytes(photofilename);

//					var sql = "insert into photorow(photofilename, photofilebytes) values(@photofilename, @photofilebytes)";
//					SqlCommand cmd = new SqlCommand(sql, connection, tran);
//					var fname = photofilename.ToUpper().Replace(@"I:\___BACKUP_2020_11_20\GUKV", @"C:\INETPUB\WWWROOT\GUKV");
//					cmd.Parameters.Add(new SqlParameter("photofilename", fname));
//					cmd.Parameters.Add(new SqlParameter("photofilebytes", photofilebytes));
//					cmd.ExecuteNonQuery();
//					cmd.Dispose();
//				}
//			}

//			tran.Commit();
//			connection.Close();
//		}
//    }
//}
