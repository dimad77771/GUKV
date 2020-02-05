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
using System.Configuration;

namespace GUKVService
{
	public static class NLog
	{
		public static void Init()
		{
			var useNlog = Int32.Parse(ConfigurationManager.AppSettings["NLog.Run"]);
			if (useNlog != 1) return;

			SesseionRowId = Guid.NewGuid();
			System.Diagnostics.Trace.CorrelationManager.ActivityId = SesseionRowId;
			var config = new LoggingConfiguration();

			//fileTarget
			var fileTarget = new FileTarget();
			config.AddTarget("file", fileTarget);
			fileTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss} ${logger} ${message}";
			fileTarget.FileName = "${basedir}/Logs/alog.txt";
			//fileTarget.ArchiveAboveSize = 1000;
			fileTarget.ArchiveFileName = "${basedir}/Logs/log.{#}.txt";
			fileTarget.ArchiveNumbering = ArchiveNumberingMode.DateAndSequence;
			fileTarget.ArchiveEvery = FileArchivePeriod.Day;
			fileTarget.ArchiveDateFormat = "yyyyMMdd";
			fileTarget.KeepFileOpen = true;
			fileTarget.OpenFileCacheTimeout = 30;
			var rule2 = new LoggingRule("*", LogLevel.Trace, fileTarget);
			config.LoggingRules.Add(rule2);

			//webTarget
			/*
			var baseUrl = ConfigurationManager.AppSettings["service.url"];
			var url = baseUrl + "api/lookups/PostNLogItem";
			var webServiceTarget = new WebServiceTarget();
			config.AddTarget("WebService", webServiceTarget);
			webServiceTarget.Name = "ws";
			webServiceTarget.Url = new Uri(url);
			webServiceTarget.Protocol = WebServiceProtocol.JsonPost;
			webServiceTarget.Encoding = Encoding.UTF8;
			webServiceTarget.Parameters.Add(new MethodCallParameter("date", @"${date:format=yyyy-MM-ddTHH\:mm\:ss\.fffffffZ}", typeof(String)));
			webServiceTarget.Parameters.Add(new MethodCallParameter("level", @"${level}", typeof(String)));
			webServiceTarget.Parameters.Add(new MethodCallParameter("logger", @"${logger}", typeof(String)));
			webServiceTarget.Parameters.Add(new MethodCallParameter("message", @"${message}", typeof(String)));
			webServiceTarget.Parameters.Add(new MethodCallParameter("activityid", @"${activityid}", typeof(String)));
			webServiceTarget.Parameters.Add(new MethodCallParameter("machinename", @"${machinename}", typeof(String)));
			var rule3 = new LoggingRule("*", LogLevel.Trace, webServiceTarget);
			config.LoggingRules.Add(rule3);
			*/



			LogManager.Configuration = config;


			////Fatal
			////Error
			////Warn
			////Info
			////Debug
			////Trace

			//var tagetFile = new FileTarget
			//{
			//	Name = "file",
			//	FileName = "${basedir}/file.txt",
			//	Layout = "${message}",
			//	//ArchiveFileName = "log.{#}.txt",
			//	//ArchiveNumbering = ArchiveNumberingMode.Date,
			//	//ArchiveEvery = FileArchivePeriod.Day,
			//	//ArchiveDateFormat = "yyyyMMdd",
			//	//KeepFileOpen = true,
			//	//OpenFileCacheTimeout = 30,
			//	AutoFlush = true,
			//};
		}
		public static Guid SesseionRowId;

		public static void Trace(object arg)
		{
			LogManager.GetCurrentClassLogger().Trace(arg);
		}

		public static void Info(object arg)
		{
			LogManager.GetCurrentClassLogger().Info(arg);
		}

		public static void Debug(object arg)
		{
			LogManager.GetCurrentClassLogger().Debug(arg);
		}

		public static void Error(object arg)
		{
			LogManager.GetCurrentClassLogger().Error(arg);
		}

		public static void Warn(object arg)
		{
			LogManager.GetCurrentClassLogger().Warn(arg);
		}

		public static void Fatal(object arg)
		{
			LogManager.GetCurrentClassLogger().Fatal(arg);
		}


		public static void vv(Func<object> outFunction = null)
		{
			var callSite = new StackTrace(1, true).GetFrame(0);
			var filename = callSite.GetFileName();
			var linenumber = callSite.GetFileLineNumber();
			var fileinfo = filename + " [" + linenumber + "]";

			var funcResult = "";
			if (outFunction != null)
			{
				try
				{
					var rez = outFunction();
					funcResult = rez.ToString();
				}
				catch (Exception ex)
				{
					funcResult = "Exception: " + ex.ToString();
				}
			}

			var str = fileinfo + (outFunction == null ? "" : " -- " + funcResult);
			Trace(str);
		}

		public static void stack()
		{
			Trace(GetFullStackTraceInfo());
		}

		public static string GetFullStackTraceInfo()
		{
			var rez = "StackTrace:";
			var stack = new StackTrace(1, true);
			for (int i = 0; i < stack.FrameCount; i++)
			{
				var callSite = stack.GetFrame(i);
				var filename = callSite.GetFileName();
				var linenumber = callSite.GetFileLineNumber();
				if (!string.IsNullOrEmpty(filename))
				{
					var fileinfo = filename + " [" + linenumber + "]";
					rez += "\n\t" + fileinfo;
				}
			}
			return rez;
		}

		public static void Flush()
		{
			LogManager.Flush();
		}

	}
}