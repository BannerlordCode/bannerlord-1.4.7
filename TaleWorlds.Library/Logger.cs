using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace TaleWorlds.Library
{
	// Token: 0x0200005D RID: 93
	public class Logger
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x00007D7D File Offset: 0x00005F7D
		// (set) Token: 0x060002A9 RID: 681 RVA: 0x00007D85 File Offset: 0x00005F85
		public bool LogOnlyErrors { get; set; }

		// Token: 0x060002AB RID: 683 RVA: 0x00007DE1 File Offset: 0x00005FE1
		public Logger(string name)
			: this(name, false, false, false, 1, -1, false)
		{
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00007DF0 File Offset: 0x00005FF0
		public Logger(string name, bool writeErrorsToDifferentFile, bool logOnlyErrors, bool doNotUseProcessId, int numFiles = 1, int totalFileSize = -1, bool overwrite = false)
		{
			string text = AppDomain.CurrentDomain.FriendlyName;
			text = Path.GetFileNameWithoutExtension(text);
			this._name = name;
			this._writeErrorsToDifferentFile = writeErrorsToDifferentFile;
			this.LogOnlyErrors = logOnlyErrors;
			this._logQueue = new Queue<HTMLDebugData>();
			int id = Process.GetCurrentProcess().Id;
			DateTime now = DateTime.Now;
			string text2 = Logger.LogsFolder;
			if (!doNotUseProcessId)
			{
				string text3 = string.Concat(new object[]
				{
					text,
					"_",
					now.ToString("yyyyMMdd"),
					"_",
					now.ToString("hhmmss"),
					"_",
					id
				});
				text2 = text2 + "/" + text3;
			}
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			this._fileManager = new Logger.FileManager(text2, this._name, numFiles, totalFileSize, overwrite, writeErrorsToDifferentFile);
			List<Logger> loggers = Logger._loggers;
			lock (loggers)
			{
				if (Logger._thread == null)
				{
					Logger._thread = new Thread(new ThreadStart(Logger.ThreadMain));
					Logger._thread.IsBackground = true;
					Logger._thread.Priority = ThreadPriority.BelowNormal;
					Logger._thread.Start();
				}
				Logger._loggers.Add(this);
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00007F50 File Offset: 0x00006150
		private static void ThreadMain()
		{
			while (Logger._running)
			{
				try
				{
					Logger.Printer();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Exception on network debug thread: " + ex.Message);
				}
			}
			Logger._isOver = true;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00007F9C File Offset: 0x0000619C
		private static void Printer()
		{
			while ((Logger._running || Logger._printedOnThisCycle) && Logger._loggers.Count > 0)
			{
				Logger._printedOnThisCycle = false;
				List<Logger> loggers = Logger._loggers;
				lock (loggers)
				{
					using (List<Logger>.Enumerator enumerator = Logger._loggers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.DoLoggingJob())
							{
								Logger._printedOnThisCycle = true;
							}
						}
					}
				}
				if (!Logger._printedOnThisCycle)
				{
					Thread.Sleep(1);
				}
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000804C File Offset: 0x0000624C
		private bool DoLoggingJob()
		{
			bool flag = false;
			HTMLDebugData htmldebugData = null;
			Queue<HTMLDebugData> logQueue = this._logQueue;
			lock (logQueue)
			{
				if (this._logQueue.Count > 0)
				{
					htmldebugData = this._logQueue.Dequeue();
				}
			}
			if (htmldebugData != null)
			{
				FileStream fileStream = this._fileManager.GetFileStream();
				flag = true;
				htmldebugData.Print(fileStream, Logger._logFileEncoding, true);
				if ((htmldebugData.Info == HTMLDebugCategory.Error || htmldebugData.Info == HTMLDebugCategory.Warning) && this._writeErrorsToDifferentFile)
				{
					htmldebugData.Print(this._fileManager.GetErrorFileStream(), Logger._logFileEncoding, false);
				}
				this._fileManager.CheckForFileSize();
			}
			return flag;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00008104 File Offset: 0x00006304
		public void Print(string log, HTMLDebugCategory debugInfo = HTMLDebugCategory.General)
		{
			this.Print(log, debugInfo, true);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00008110 File Offset: 0x00006310
		public void Print(string log, HTMLDebugCategory debugInfo, bool printOnGlobal)
		{
			if (!this.LogOnlyErrors || (this.LogOnlyErrors && debugInfo == HTMLDebugCategory.Error) || (this.LogOnlyErrors && debugInfo == HTMLDebugCategory.Warning))
			{
				HTMLDebugData htmldebugData = new HTMLDebugData(log, debugInfo);
				Queue<HTMLDebugData> logQueue = this._logQueue;
				lock (logQueue)
				{
					this._logQueue.Enqueue(htmldebugData);
				}
				if (printOnGlobal)
				{
					Debug.Print(log, 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00008194 File Offset: 0x00006394
		public static void FinishAndCloseAll()
		{
			List<Logger> loggers = Logger._loggers;
			lock (loggers)
			{
				Logger._running = false;
				Logger._printedOnThisCycle = true;
			}
			while (!Logger._isOver)
			{
			}
		}

		// Token: 0x04000107 RID: 263
		private Queue<HTMLDebugData> _logQueue;

		// Token: 0x04000108 RID: 264
		private static Encoding _logFileEncoding = Encoding.UTF8;

		// Token: 0x04000109 RID: 265
		private string _name;

		// Token: 0x0400010A RID: 266
		private bool _writeErrorsToDifferentFile;

		// Token: 0x0400010C RID: 268
		private static List<Logger> _loggers = new List<Logger>();

		// Token: 0x0400010D RID: 269
		private Logger.FileManager _fileManager;

		// Token: 0x0400010E RID: 270
		private static Thread _thread;

		// Token: 0x0400010F RID: 271
		private static bool _running = true;

		// Token: 0x04000110 RID: 272
		private static bool _printedOnThisCycle = false;

		// Token: 0x04000111 RID: 273
		private static bool _isOver = false;

		// Token: 0x04000112 RID: 274
		public static string LogsFolder = Environment.CurrentDirectory + "\\logs";

		// Token: 0x020000D6 RID: 214
		private class FileManager
		{
			// Token: 0x06000764 RID: 1892 RVA: 0x00018798 File Offset: 0x00016998
			public FileManager(string path, string name, int numFiles, int maxTotalSize, bool overwrite, bool logErrorsToDifferentFile)
			{
				if (maxTotalSize < numFiles * 64 * 1024)
				{
					this._numFiles = 1;
					this._isCheckingFileSize = false;
				}
				else
				{
					this._numFiles = numFiles;
					if (numFiles <= 0)
					{
						this._numFiles = 1;
						this._isCheckingFileSize = false;
					}
					this._maxFileSize = maxTotalSize / this._numFiles;
					this._isCheckingFileSize = true;
				}
				this._streams = new FileStream[this._numFiles];
				this._currentStreamIndex = 0;
				try
				{
					for (int i = 0; i < this._numFiles; i++)
					{
						string text = name + "_" + i;
						string text2 = path + "/" + text + ".html";
						this._streams[i] = (overwrite ? new FileStream(text2, FileMode.Create) : new FileStream(text2, FileMode.OpenOrCreate));
						this.FillEmptyStream(this._streams[i]);
					}
					if (logErrorsToDifferentFile)
					{
						string text3 = path + "/" + name + "_errors.html";
						this._errorStream = (overwrite ? new FileStream(text3, FileMode.Create) : new FileStream(text3, FileMode.OpenOrCreate));
						this.FillEmptyStream(this._errorStream);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error when creating log file(s): " + ex.GetBaseException().Message);
					for (int j = 0; j < this._numFiles; j++)
					{
						string text4 = name + "__" + j;
						string text5 = path + "/" + text4 + ".html";
						this._streams[j] = (overwrite ? new FileStream(text5, FileMode.Create) : new FileStream(text5, FileMode.OpenOrCreate));
						this.FillEmptyStream(this._streams[j]);
					}
					if (logErrorsToDifferentFile)
					{
						string text6 = path + "/" + name + "_errors.html";
						this._errorStream = (overwrite ? new FileStream(text6, FileMode.Create) : new FileStream(text6, FileMode.OpenOrCreate));
						this.FillEmptyStream(this._errorStream);
					}
				}
			}

			// Token: 0x06000765 RID: 1893 RVA: 0x0001898C File Offset: 0x00016B8C
			public FileStream GetFileStream()
			{
				return this._streams[this._currentStreamIndex];
			}

			// Token: 0x06000766 RID: 1894 RVA: 0x0001899B File Offset: 0x00016B9B
			public FileStream GetErrorFileStream()
			{
				return this._errorStream;
			}

			// Token: 0x06000767 RID: 1895 RVA: 0x000189A4 File Offset: 0x00016BA4
			public void CheckForFileSize()
			{
				if (this._isCheckingFileSize && this._streams[this._currentStreamIndex].Length > (long)this._maxFileSize)
				{
					this._currentStreamIndex = (this._currentStreamIndex + 1) % this._numFiles;
					this.ResetFileStream(this._streams[this._currentStreamIndex]);
				}
			}

			// Token: 0x06000768 RID: 1896 RVA: 0x000189FC File Offset: 0x00016BFC
			public void ShutDown()
			{
				for (int i = 0; i < this._numFiles; i++)
				{
					this._streams[i].Close();
					this._streams[i] = null;
				}
				if (this._errorStream != null)
				{
					this._errorStream.Close();
					this._errorStream = null;
				}
			}

			// Token: 0x06000769 RID: 1897 RVA: 0x00018A4C File Offset: 0x00016C4C
			private void FillEmptyStream(FileStream stream)
			{
				if (stream.Length == 0L)
				{
					string text = "<table></table>";
					byte[] bytes = Logger._logFileEncoding.GetBytes(text);
					stream.Write(bytes, 0, bytes.Length);
				}
			}

			// Token: 0x0600076A RID: 1898 RVA: 0x00018A7E File Offset: 0x00016C7E
			private void ResetFileStream(FileStream stream)
			{
				stream.SetLength(0L);
				this.FillEmptyStream(stream);
			}

			// Token: 0x040002B0 RID: 688
			private bool _isCheckingFileSize;

			// Token: 0x040002B1 RID: 689
			private int _maxFileSize;

			// Token: 0x040002B2 RID: 690
			private int _numFiles;

			// Token: 0x040002B3 RID: 691
			private FileStream[] _streams;

			// Token: 0x040002B4 RID: 692
			private int _currentStreamIndex;

			// Token: 0x040002B5 RID: 693
			private FileStream _errorStream;
		}
	}
}
