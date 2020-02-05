using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;

namespace GUKVService
{
	public partial class WindowsService : ServiceBase
	{
		public WindowsService()
		{
		}

		protected override void OnStart(string[] args)
		{
			NLog.Debug("WindowsService.OnStart1");
			NLog.Debug("is64BitProcess=" + (IntPtr.Size == 8));
			var proc = new MainCycle();
			proc.InWindowsService = true;
			var ts = new ThreadStart(async () => await proc.Run());
			m_shutdownEvent = new ManualResetEvent(false);
			m_thread = new Thread(ts);
			m_thread.Start();
			base.OnStart(args);
		}

		protected override void OnStop()
		{
			NLog.Debug("WindowsService.OnStop");
			m_thread.Abort();

			// signal the event to shutdown
			//m_shutdownEvent.Set();

			// wait for the thread to stop giving it 10 seconds
			//m_thread.Join(10000);

			// call the base class 
			base.OnStop();

		}

		protected Thread m_thread;
		protected ManualResetEvent m_shutdownEvent;
		protected TimeSpan m_delay;
		protected string[] m_args;
	}
}