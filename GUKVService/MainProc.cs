using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKVService
{
	public class MainProc
	{
		static void Main(string[] args)
		{
			//sc create GUKVService binpath= "E:\PROJECTS\DKVSOURCESFINALEDITION\GUKVService\bin\Debug\GUKVService.exe --service"
			//sc delete GUKVService

			NLog.Init();
			if (args != null && args.Length == 1 && args[0] == "--service")
			{
				NLog.Debug("Start WindowsServiceFunc.Run");
				WindowsServiceFunc.Run();
			}
			else
			{
				//консольный вариант
				var mainCycle = new MainCycle();
				AsyncContext.Run(mainCycle.Run);
			}
		}
	}
}
