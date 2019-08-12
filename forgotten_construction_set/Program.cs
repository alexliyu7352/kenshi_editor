using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	internal static class Program
	{
		private static Logger logger;

		public static string[] args
		{
			get;
			set;
		}

		static Program()
		{
			Program.logger = LogManager.GetLogger("FCS");
		}

		private static void FCS_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception exceptionObject = e.ExceptionObject as Exception;
			if (exceptionObject == null)
			{
				Program.logger.Error("UnhandledException");
				return;
			}
			Program.logger.Error<string, string, string>("Exception: {0} ({1}). Stacktrace: {2}", exceptionObject.Message, exceptionObject.Source, exceptionObject.StackTrace);
		}

		[STAThread]
		private static void Main(string[] args)
		{
			Program.args = args;
			NLog.LogLevel info = NLog.LogLevel.Info;
			string[] strArrays = args;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				if (strArrays[i] == "--debug")
				{
					info = NLog.LogLevel.Debug;
				}
			}
			info = NLog.LogLevel.Debug;
			LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
			FileTarget fileTarget = new FileTarget();
			loggingConfiguration.AddTarget("file", fileTarget);
			fileTarget.FileName = Path.Combine(directoryName, string.Concat(fileNameWithoutExtension, ".log"));
			fileTarget.Layout = "${longdate} | ${level:uppercase=true} | ${logger} | ${message}";
			fileTarget.MaxArchiveFiles = 0;
			LoggingRule loggingRule = new LoggingRule("*", info, fileTarget);
			loggingConfiguration.LoggingRules.Add(loggingRule);
			LogManager.Configuration = loggingConfiguration;
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.FCS_UnhandledException);
			Program.logger.Info("** Start **");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new baseForm());
			Program.logger.Info("** Shutdown **");
		}
	}
}