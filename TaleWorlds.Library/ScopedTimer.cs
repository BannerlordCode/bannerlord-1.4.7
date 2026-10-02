using System;
using System.Diagnostics;

namespace TaleWorlds.Library
{
	// Token: 0x0200008C RID: 140
	public class ScopedTimer : IDisposable
	{
		// Token: 0x0600050A RID: 1290 RVA: 0x000123B7 File Offset: 0x000105B7
		public ScopedTimer(string scopeName)
		{
			this.scopeName_ = scopeName;
			this.watch_ = new Stopwatch();
			this.watch_.Start();
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000123DC File Offset: 0x000105DC
		public void Dispose()
		{
			this.watch_.Stop();
			Console.WriteLine(string.Concat(new object[]
			{
				"ScopedTimer: ",
				this.scopeName_,
				" elapsed ms: ",
				this.watch_.Elapsed.TotalMilliseconds
			}));
		}

		// Token: 0x0400018F RID: 399
		private readonly Stopwatch watch_;

		// Token: 0x04000190 RID: 400
		private readonly string scopeName_;
	}
}
