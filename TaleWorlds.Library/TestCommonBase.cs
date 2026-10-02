using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaleWorlds.Library
{
	// Token: 0x02000093 RID: 147
	public abstract class TestCommonBase
	{
		// Token: 0x06000540 RID: 1344
		public abstract void Tick();

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00012A47 File Offset: 0x00010C47
		public static TestCommonBase BaseInstance
		{
			get
			{
				return TestCommonBase._baseInstance;
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00012A4E File Offset: 0x00010C4E
		public void StartTimeoutTimer()
		{
			this.timeoutTimerStart = DateTime.Now;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00012A5B File Offset: 0x00010C5B
		public void ToggleTimeoutTimer()
		{
			this.timeoutTimerEnabled = !this.timeoutTimerEnabled;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00012A6C File Offset: 0x00010C6C
		public bool CheckTimeoutTimer()
		{
			return this.timeoutTimerEnabled && DateTime.Now.Subtract(this.timeoutTimerStart).TotalSeconds > (double)this.commonWaitTimeoutLimits;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00012AAA File Offset: 0x00010CAA
		protected TestCommonBase()
		{
			TestCommonBase._baseInstance = this;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00012AE0 File Offset: 0x00010CE0
		public virtual string GetGameStatus()
		{
			return "";
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00012AE8 File Offset: 0x00010CE8
		public void WaitFor(double seconds)
		{
			if (!this.isParallelThread)
			{
				DateTime now = DateTime.Now;
				while ((DateTime.Now - now).TotalSeconds < seconds)
				{
					Monitor.Pulse(this.TestLock);
					Monitor.Wait(this.TestLock);
				}
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00012B34 File Offset: 0x00010D34
		public virtual async Task WaitUntil(Func<bool> func)
		{
			while (!func())
			{
				await this.WaitForAsync(0.1);
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00012B81 File Offset: 0x00010D81
		public Task WaitForAsync(double seconds, Random random)
		{
			return Task.Delay((int)(seconds * 1000.0 * random.NextDouble()));
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00012B9B File Offset: 0x00010D9B
		public Task WaitForAsync(double seconds)
		{
			return Task.Delay((int)(seconds * 1000.0));
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00012BAE File Offset: 0x00010DAE
		public static string GetAttachmentsFolderPath()
		{
			return "..\\..\\..\\Tools\\TestAutomation\\Attachments\\";
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00012BB5 File Offset: 0x00010DB5
		public virtual void OnFinalize()
		{
			TestCommonBase._baseInstance = null;
		}

		// Token: 0x0400019B RID: 411
		public int TestRandomSeed;

		// Token: 0x0400019C RID: 412
		public bool IsTestEnabled;

		// Token: 0x0400019D RID: 413
		public bool isParallelThread;

		// Token: 0x0400019E RID: 414
		public string SceneNameToOpenOnStartup;

		// Token: 0x0400019F RID: 415
		public object TestLock = new object();

		// Token: 0x040001A0 RID: 416
		private static TestCommonBase _baseInstance;

		// Token: 0x040001A1 RID: 417
		private DateTime timeoutTimerStart = DateTime.Now;

		// Token: 0x040001A2 RID: 418
		private bool timeoutTimerEnabled = true;

		// Token: 0x040001A3 RID: 419
		private int commonWaitTimeoutLimits = 1140;
	}
}
