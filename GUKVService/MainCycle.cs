using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;


namespace GUKVService
{
	public class MainCycle
	{
		public bool InWindowsService { get; set; }

		public async Task Run()
		{
			try
			{
				NLog.Debug("MainCycle--1");
				var seconds = Int32.Parse(ConfigurationManager.AppSettings["Service.CycleSeconds"]);
				NLog.Debug("MainCycle--2");
				var cyclenum = 0;
				NLog.Debug("MainCycle--3");

				while (true)
				{
					cyclenum++;
					var str = "Cycle start. Count=" + cyclenum;
					Console.WriteLine(str);
					NLog.Debug(str);

					try
					{
						await OneCycle();
					}
					catch (Exception ex)
					{
						NLog.Error(ex.ToString());
						Console.WriteLine(ex.ToString());
					}

					Thread.Sleep(seconds * 1000);
				}
			}
			catch (Exception ex)
			{
				NLog.Error(ex.ToString());
				throw new AggregateException(ex);
			}
		}

		public async Task OneCycle()
		{
			var dir = ConfigurationManager.AppSettings["ImgContent.Directory"];
			var lifeMinutes = Int32.Parse(ConfigurationManager.AppSettings["ImgContent.LifeMinutes"]);
			var subdirs = Directory.GetDirectories(dir);
			foreach(var subdir in subdirs)
			{
				var lenguid = default(Guid).ToString().Length;
				var sname = Path.GetFileName(subdir);
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
								catch(Exception ex)
								{
									NLog.Error(ex.ToString());
								}
								Directory.Delete(subdir, true);
							}
						}
					}
				}
			}

		}

	}
}
