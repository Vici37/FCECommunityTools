namespace FortressCraft.Community
{
	using System;
	using System.IO;
	using System.Threading;
	using System.Reflection;
	using System.Collections.Generic;

	/// <summary>
	///		A simple, fluent, Thread Safe Logger.
	/// </summary>
	public class Log
	{
		private string _file;
		private Severity _severity;
		private readonly LockingQueue<LogData> _queue; 

		/// <summary>
		///		Create a new instance
		/// </summary>
		private Log(string file, Severity level)
		{
			this._file = file;
			this._severity = level;
			this._queue = new LockingQueue<LogData>();

			using (var fs = File.OpenWrite(this._file))
			{
				fs.SetLength(0);
			}

			ThreadPool.QueueUserWorkItem(Write);
		}

		/// <summary>
		///		Thread-Safely write the queued log messages to disk.
		/// </summary>
		private void Write(Object state)
		{
			LogData item;
			while (true)
			{
				if (this._queue.Count == 0)
				{
					Thread.Sleep(200);
					continue;
				}

				using (var fs = File.OpenWrite(this._file))
				using (var stream = new StreamWriter(fs))
				{
					while (this._queue.Count != 0 && (item = this._queue.Dequeue()) != default(LogData))
					{
						stream.WriteLine($"[{item.Severity}] {item.Time} - {item.Message}");
					}
				}

			}
		}

		/// <summary>
		///		Creates a new Logger instance
		/// </summary>
		/// <param name="file">The file to write to</param>
		/// <param name="level">The lowest leveled messages to write to the log</param>
		/// <returns>A new instane of the Log class</returns>
		public static Log To(string file = null, Severity level = Severity.Info)
		{
			return new Log(
				file ?? Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "output.log"),
				level
			);
		}

		/// <summary>
		///		Queue a message to be written to the log with the requested severity.
		/// </summary>
		/// <param name="severity">The <see cref="Severity">Severity</see> of the message</param>
		/// <param name="message">The message</param>
		/// <returns>this</returns>
		public Log Write(Severity severity, string message)
		{
			if (this._severity > severity || this._severity == Severity.None)
				return this;

			this._queue.Enqueue(new LogData(message, DateTime.Now, severity));

			return this;
		}

		/// <summary>
		///		Writes a <see cref="Severity">Info</see> message to the log
		/// </summary>
		/// <param name="message">The Message</param>
		/// <param name="args">Optional arguments to format the message with</param>
		/// <returns>this</returns>
		public Log Info(string message, params object[] args)
		{
			return this.Write(Severity.Info, string.Format(message, args));
		}

		/// <summary>
		///		Writes a <see cref="Severity">Debug</see> message to the log
		/// </summary>
		/// <param name="message">The Message</param>
		/// <param name="args">Optional arguments to format the message with</param>
		/// <returns>this</returns>
		public Log Debug(string message, params object[] args)
		{
			return this.Write(Severity.Debug, string.Format(message, args));
		}

		/// <summary>
		///		Writes a <see cref="Severity">Warning</see> message to the log
		/// </summary>
		/// <param name="message">The Message</param>
		/// <param name="args">Optional arguments to format the message with</param>
		/// <returns>this</returns>
		public Log Warning(string message, params object[] args)
		{
			return this.Write(Severity.Warning, string.Format(message, args));
		}

		/// <summary>
		///		Writes a <see cref="Severity">Error</see> message to the log
		/// </summary>
		/// <param name="message">The Message</param>
		/// <param name="args">Optional arguments to format the message with</param>
		/// <returns>this</returns>
		public Log Error(string message, params object[] args)
		{
			return this.Write(Severity.Error, string.Format(message, args));
		}

		/// <summary>
		///		The "Severity" of the message.
		/// </summary>
		public enum Severity
		{
			None = 0,
			Info = 1,
			Debug = 2,
			Warning = 3,
			Error = 4,
		}

		/// <summary>
		///		Contains the details of a Log Message.
		///		Since Unity uses .Net 3.5, not 4+ q.q
		/// </summary>
		internal class LogData
		{
			public string Message;
			public DateTime Time;
			public Severity Severity;

			public LogData(string message, DateTime time, Severity severity)
			{
				this.Message = message;
				this.Time = time;
				this.Severity = severity;
			}
		}

		/// <summary>
		///		A Simple Thread-Safe Queue usings Locks
		///		
		///		Stripped down version of: https://gist.github.com/jaredjenkins/5421892
		/// </summary>
		internal class LockingQueue<T>
		{
			private readonly object syncLock = new object();
			private Queue<T> queue;

			public int Count
			{
				get
				{
					lock (syncLock)
					{
						return queue.Count;
					}
				}
			}

			public LockingQueue()
			{
				this.queue = new Queue<T>();
			}

			public void Enqueue(T obj)
			{
				lock (syncLock)
				{
					queue.Enqueue(obj);
				}
			}

			public T Dequeue()
			{
				lock (syncLock)
				{
					return queue.Dequeue();
				}
			}
		}
	}
}
