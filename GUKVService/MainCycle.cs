using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SqlClient;

namespace GUKVService
{
	public class MainCycle
	{
		public bool InWindowsService { get; set; }

		

		public async Task Run()
		{
			try
			{
				NLog.Debug("Start service...");

				while (true)
				{
					var task1 = Task.Run(() => WorkerLoop(OneCycle_1, "Clear ImgContent"));
					var task2 = Task.Run(() => WorkerLoop(Calc_Reptab_RentAgreements, "Reptab_RentAgreements refresh"));

					await Task.WhenAll(task2);
					await Task.WhenAll(task1, task2);
				}
			}
			catch (Exception ex)
			{
				NLog.Error(ex.ToString());
				throw new AggregateException(ex);
			}
		}

		private async Task WorkerLoop(Func<Task> cycleFunc, string name)
		{
			var count = 0;
			while (true)
			{
				try
				{
					count++;
					NLog.Debug($"{name} Start. Iteration: {count}");
					await cycleFunc();
					NLog.Debug($"{name} End. Iteration: {count}");
				}
				catch (Exception ex)
				{
					NLog.Error($"{name} ERROR: {ex}");
				}
			}
		}

		async Task OneCycle_1()
		{
			int seconds = Int32.Parse(ConfigurationManager.AppSettings["Service.CycleSeconds"]);

			var dir = ConfigurationManager.AppSettings["ImgContent.Directory"];
			var lifeMinutes = Int32.Parse(ConfigurationManager.AppSettings["ImgContent.LifeMinutes"]);
			var subdirs = Directory.GetDirectories(dir);
			foreach (var subdir in subdirs)
			{
				var lenguid = default(Guid).ToString().Length;
				var sname = Path.GetFileName(subdir);
				if (sname.ToLower().EndsWith("_a") || sname.ToLower().EndsWith("_b") || sname.ToLower().EndsWith("_c") || sname.ToLower().EndsWith("_d") || sname.ToLower().EndsWith("_e"))
				{
					sname = sname.Substring(0, sname.Length - 2);
				}
				if (sname.Length >= lenguid + 1)
				{
					var sname2 = sname.Substring(sname.Length - lenguid - 1);
					if (sname2.Substring(0, 1) == "_")
					{
						if (Guid.TryParse(sname2.Substring(1), out Guid guid))
						{
							var modifyTime = Directory.GetLastWriteTime(subdir);
							if ((DateTime.Now - modifyTime).TotalMinutes > lifeMinutes)
							{
								try
								{
									NLog.Debug("Delete:" + subdir);
								}
								catch (Exception ex)
								{
									NLog.Error(ex.ToString());
								}
								Directory.Delete(subdir, true);
							}
						}
					}
				}
			}

			await Task.Delay(seconds * 1000);
		}


		public async Task Calc_Reptab_RentAgreements()
		{
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GUKVConnectionString"].ConnectionString))
			{
				await connection.OpenAsync();

				var lastWriteCount = await GetNumOfWrites(connection);

				while (true)
				{
					NLog.Debug($"Reptab_RentAgreements: Refresh report table. Start");
					await ExecRentAgreementsUpdate(connection);
					NLog.Debug($"Reptab_RentAgreements: Refresh report table. Finish");

					while (true)
					{
						var currentWriteCount = await GetNumOfWrites(connection);
						if (currentWriteCount != lastWriteCount)
						{
							NLog.Debug($"Reptab_RentAgreements: Detected DB write activity: {lastWriteCount} -> {currentWriteCount}");
							lastWriteCount = currentWriteCount;
							break;
						}
						NLog.Debug($"Reptab_RentAgreements: No DB write activity: {lastWriteCount} == {currentWriteCount}");
						await Task.Delay(1000);
					}
				}
			}
		}

		private async Task ExecRentAgreementsUpdate(SqlConnection connection)
		{
			using (var cmd = new SqlCommand("exec [dbo].[reptab_RentAgreements_update]", connection))
			{
				cmd.CommandTimeout = 300;
				await cmd.ExecuteNonQueryAsync();
				NLog.Debug("Executed reptab_RentAgreements_update");
			}
		}

		private async Task<long> GetNumOfWrites(SqlConnection connection)
		{
			using (var cmd = new SqlCommand(@"
		SELECT num_of_writes 
		FROM sys.dm_io_virtual_file_stats(NULL, NULL) 
		WHERE file_id = 2 AND database_id = DB_ID()", connection))
			{
				var result = await cmd.ExecuteScalarAsync();
				if (result != null && result != DBNull.Value)
					return Convert.ToInt64(result);
				else
					return -1;
			}
		}

	}
}
