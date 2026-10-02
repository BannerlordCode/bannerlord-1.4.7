using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000014 RID: 20
	public class WindowsFramework
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x0000585A File Offset: 0x00003A5A
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00005862 File Offset: 0x00003A62
		public WindowsFrameworkThreadConfig ThreadConfig { get; set; }

		// Token: 0x060000F9 RID: 249 RVA: 0x0000586B File Offset: 0x00003A6B
		public WindowsFramework()
		{
			this._timer = new Stopwatch();
			this._messageCommunicators = new List<IMessageCommunicator>();
			this.IsActive = false;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005890 File Offset: 0x00003A90
		public void Initialize(FrameworkDomain[] frameworkDomains)
		{
			this._frameworkDomains = frameworkDomains;
			this.IsActive = true;
			if (this.ThreadConfig == WindowsFrameworkThreadConfig.SingleThread)
			{
				this._frameworkDomainThreads = new Thread[1];
				this.CreateThread(0);
				return;
			}
			if (this.ThreadConfig == WindowsFrameworkThreadConfig.MultiThread)
			{
				this._frameworkDomainThreads = new Thread[frameworkDomains.Length];
				for (int i = 0; i < frameworkDomains.Length; i++)
				{
					this.CreateThread(i);
				}
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000058F4 File Offset: 0x00003AF4
		private void CreateThread(int index)
		{
			Common.SetInvariantCulture();
			this._frameworkDomainThreads[index] = new Thread(new ParameterizedThreadStart(this.MainLoop));
			this._frameworkDomainThreads[index].SetApartmentState(ApartmentState.STA);
			this._frameworkDomainThreads[index].Name = this._frameworkDomains[index].ToString() + " Thread";
			this._frameworkDomainThreads[index].CurrentCulture = CultureInfo.InvariantCulture;
			this._frameworkDomainThreads[index].CurrentUICulture = CultureInfo.InvariantCulture;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005975 File Offset: 0x00003B75
		public void RegisterMessageCommunicator(IMessageCommunicator communicator)
		{
			this._messageCommunicators.Add(communicator);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005983 File Offset: 0x00003B83
		public void UnRegisterMessageCommunicator(IMessageCommunicator communicator)
		{
			this._messageCommunicators.Remove(communicator);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005994 File Offset: 0x00003B94
		private void MessageLoop()
		{
			try
			{
				if (this.ThreadConfig == WindowsFrameworkThreadConfig.NoThread)
				{
					int num = 0;
					while (this._frameworkDomains != null && num < this._frameworkDomains.Length)
					{
						this._frameworkDomains[num].Update();
						num++;
					}
				}
				for (int i = 0; i < this._messageCommunicators.Count; i++)
				{
					this._messageCommunicators[i].MessageLoop();
				}
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print(ex.StackTrace, 0, Debug.DebugColor.White, 17592186044416UL);
				throw;
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005A3C File Offset: 0x00003C3C
		private void MainLoop(object parameter)
		{
			try
			{
				if (this.ThreadConfig == WindowsFrameworkThreadConfig.SingleThread)
				{
					while (this.IsActive)
					{
						for (int i = 0; i < this._frameworkDomains.Length; i++)
						{
							this._frameworkDomains[i].Update();
						}
					}
				}
				else if (this.ThreadConfig == WindowsFrameworkThreadConfig.MultiThread)
				{
					FrameworkDomain frameworkDomain = parameter as FrameworkDomain;
					while (this.IsActive)
					{
						frameworkDomain.Update();
					}
				}
				Interlocked.Increment(ref this._abortedThreadCount);
				this.OnFinalize();
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print(ex.StackTrace, 0, Debug.DebugColor.White, 17592186044416UL);
				throw;
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005AF4 File Offset: 0x00003CF4
		public void Stop()
		{
			this.IsActive = false;
			this.OnFinalize();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005B04 File Offset: 0x00003D04
		public void OnFinalize()
		{
			if (this._frameworkDomainThreads != null && this._abortedThreadCount != this._frameworkDomainThreads.Length)
			{
				return;
			}
			this._frameworkDomainThreads = null;
			FrameworkDomain[] frameworkDomains = this._frameworkDomains;
			for (int i = 0; i < frameworkDomains.Length; i++)
			{
				frameworkDomains[i].Destroy();
			}
			this._frameworkDomains = null;
			this.IsFinalized = true;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005B5C File Offset: 0x00003D5C
		public void Start()
		{
			this._timer.Start();
			this.IsActive = true;
			if (this.ThreadConfig == WindowsFrameworkThreadConfig.SingleThread)
			{
				this._frameworkDomainThreads[0].Start();
			}
			else if (this.ThreadConfig == WindowsFrameworkThreadConfig.MultiThread)
			{
				for (int i = 0; i < this._frameworkDomains.Length; i++)
				{
					this._frameworkDomainThreads[i].Start(this._frameworkDomains[i]);
				}
			}
			NativeMessage nativeMessage = default(NativeMessage);
			if (this.ThreadConfig == WindowsFrameworkThreadConfig.NoThread)
			{
				while (this.IsActive)
				{
					if (User32.PeekMessage(out nativeMessage, IntPtr.Zero, 0U, 0U, 1U))
					{
						User32.TranslateMessage(ref nativeMessage);
						User32.DispatchMessage(ref nativeMessage);
					}
					this.MessageLoop();
				}
				return;
			}
			while (this.IsActive)
			{
				if (User32.PeekMessage(out nativeMessage, IntPtr.Zero, 0U, 0U, 1U))
				{
					if (nativeMessage.msg == WindowMessage.Quit)
					{
						break;
					}
					User32.TranslateMessage(ref nativeMessage);
					User32.DispatchMessage(ref nativeMessage);
				}
				this.MessageLoop();
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00005C3F File Offset: 0x00003E3F
		public long ElapsedTicks
		{
			get
			{
				return this._timer.ElapsedTicks;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005C4C File Offset: 0x00003E4C
		public long TicksPerSecond
		{
			get
			{
				return Stopwatch.Frequency;
			}
		}

		// Token: 0x04000058 RID: 88
		public bool IsActive;

		// Token: 0x04000059 RID: 89
		private FrameworkDomain[] _frameworkDomains;

		// Token: 0x0400005A RID: 90
		private Thread[] _frameworkDomainThreads;

		// Token: 0x0400005B RID: 91
		private Stopwatch _timer;

		// Token: 0x0400005D RID: 93
		private List<IMessageCommunicator> _messageCommunicators;

		// Token: 0x0400005E RID: 94
		public bool IsFinalized;

		// Token: 0x0400005F RID: 95
		private int _abortedThreadCount;
	}
}
