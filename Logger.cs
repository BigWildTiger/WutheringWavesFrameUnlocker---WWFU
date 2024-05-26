using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WutheringWavesFrameUnlocker___WWFU
{

	public enum LogType { Debug, Info, Error }

	public class LogMessage
	{
		public LogType logtype;
		public string logmsg;

		public LogMessage(LogType log, string msg)
		{
			logtype = log;
			logmsg = msg;
		}
	}

	public class Logger
	{
		public static string filename = string.Format("{0}{1}{2}", Application.StartupPath, DateTime.Now.ToString("yyyy-MM-dd_"), "log.log");

		public static Queue<LogMessage> logQueue = new Queue<LogMessage>();

		private static readonly object writeLock = new object();

		public static void WriteLog(LogType logtype, string str, bool bwritenow = false)
		{


			string logmsg = string.Format("[{0}] {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), str);
			Debug.WriteLine(logmsg);
			logQueue.Enqueue(new LogMessage(logtype, logmsg));
			if (bwritenow == true)
			{
				lock (writeLock)
				{
					string wasLogFileData;
					if (File.Exists(filename))
					{
						wasLogFileData = File.ReadAllText(filename, UTF8Encoding.UTF8);
					}
					else
					{
						wasLogFileData = string.Empty;
					}

					using (StreamWriter fw = new StreamWriter(filename, append: false, UTF8Encoding.UTF8))
					{
						while (logQueue.Count > 0)
						{
							LogMessage log = logQueue.Dequeue();
							StringBuilder readAllText = new StringBuilder();
							readAllText.Append(log.logmsg);
							readAllText.Append(wasLogFileData.ToString());
							fw.Write(readAllText.ToString());
							readAllText.Clear();

							fw.Flush();
							fw.Close();
						}
					}
					wasLogFileData = string.Empty;
					//using (var inputFile = File.OpenRead(filename))
					//using (ReverseStream inputFileReversed = new ReverseStream(inputFile))
					//using (var outputFile = File.Open(filename + ".rev", FileMode.Create, FileAccess.Write))
					//{
					//	inputFileReversed.CopyTo(outputFile);
					//}
				}
			}
		}

		public static void FlushLogFile()
		{
			string logmsg = string.Format("[{0}] {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "Deleted All Log history..");
			Debug.WriteLine(logmsg);
			lock (writeLock)
			{
				using (StreamWriter fw = new StreamWriter(filename))
				{
					fw.Write(logmsg);
				}
			}
		}
	}
}