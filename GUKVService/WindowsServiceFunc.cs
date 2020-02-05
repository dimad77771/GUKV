using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog.Config;
using NLog.Targets;
using NLog;
using System.Diagnostics;
using System.ServiceProcess;

namespace GUKVService
{
	public static class WindowsServiceFunc
	{
		public static void Run()
		{
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new WindowsService()
			};
			ServiceBase.Run(ServicesToRun);
		}
	}
}