using System;
using System.Threading;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200006F RID: 111
	public sealed class NativeParallelDriver : IParallelDriver
	{
		// Token: 0x06000A49 RID: 2633 RVA: 0x0000A724 File Offset: 0x00008924
		public void For(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate loopBody, int grainSize)
		{
			long num = Interlocked.Increment(ref NativeParallelDriver.LoopBodyHolder.UniqueLoopBodyKeySeed) % 256L;
			checked
			{
				NativeParallelDriver._loopBodyCache[(int)((IntPtr)num)].LoopBody = loopBody;
				Utilities.ParallelFor(fromInclusive, toExclusive, num, grainSize);
				NativeParallelDriver._loopBodyCache[(int)((IntPtr)num)].LoopBody = null;
			}
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0000A774 File Offset: 0x00008974
		public void ForWithoutRenderThread(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate loopBody, int grainSize)
		{
			long num = Interlocked.Increment(ref NativeParallelDriver.LoopBodyHolder.UniqueLoopBodyKeySeed) % 256L;
			checked
			{
				NativeParallelDriver._loopBodyCache[(int)((IntPtr)num)].LoopBody = loopBody;
				Utilities.ParallelForWithoutRenderThread(fromInclusive, toExclusive, num, grainSize);
				NativeParallelDriver._loopBodyCache[(int)((IntPtr)num)].LoopBody = null;
			}
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0000A7C4 File Offset: 0x000089C4
		public void ForWithoutRenderThreadDt(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate loopBody, int grainSize)
		{
			long num = Interlocked.Increment(ref NativeParallelDriver.LoopBodyWithDtHolder.UniqueLoopBodyKeySeed) % 256L;
			checked
			{
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].LoopBody = loopBody;
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].DeltaTime = deltaTime;
				Utilities.ParallelForWithoutRenderThreadDt(fromInclusive, toExclusive, num, grainSize);
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].LoopBody = null;
			}
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0000A824 File Offset: 0x00008A24
		public void For(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate loopBody, int grainSize)
		{
			long num = Interlocked.Increment(ref NativeParallelDriver.LoopBodyWithDtHolder.UniqueLoopBodyKeySeed) % 256L;
			checked
			{
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].LoopBody = loopBody;
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].DeltaTime = deltaTime;
				Utilities.ParallelForWithDt(fromInclusive, toExclusive, num, grainSize);
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)num)].LoopBody = null;
			}
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0000A884 File Offset: 0x00008A84
		public ulong GetMainThreadId()
		{
			return Utilities.GetMainThreadId();
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0000A88B File Offset: 0x00008A8B
		public ulong GetCurrentThreadId()
		{
			return Utilities.GetCurrentThreadId();
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0000A892 File Offset: 0x00008A92
		[EngineCallback(null, false)]
		internal static void ParalelForLoopBodyCaller(long loopBodyKey, int localStartIndex, int localEndIndex)
		{
			NativeParallelDriver._loopBodyCache[(int)(checked((IntPtr)loopBodyKey))].LoopBody(localStartIndex, localEndIndex);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0000A8AC File Offset: 0x00008AAC
		[EngineCallback(null, false)]
		internal static void ParalelForLoopBodyWithDtCaller(long loopBodyKey, int localStartIndex, int localEndIndex)
		{
			checked
			{
				NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)loopBodyKey)].LoopBody(localStartIndex, localEndIndex, NativeParallelDriver._loopBodyWithDtCache[(int)((IntPtr)loopBodyKey)].DeltaTime);
			}
		}

		// Token: 0x04000158 RID: 344
		private const int K = 256;

		// Token: 0x04000159 RID: 345
		private static readonly NativeParallelDriver.LoopBodyHolder[] _loopBodyCache = new NativeParallelDriver.LoopBodyHolder[256];

		// Token: 0x0400015A RID: 346
		private static readonly NativeParallelDriver.LoopBodyWithDtHolder[] _loopBodyWithDtCache = new NativeParallelDriver.LoopBodyWithDtHolder[256];

		// Token: 0x020000CB RID: 203
		private struct LoopBodyHolder
		{
			// Token: 0x04000430 RID: 1072
			public static long UniqueLoopBodyKeySeed;

			// Token: 0x04000431 RID: 1073
			public TWParallel.ParallelForAuxPredicate LoopBody;
		}

		// Token: 0x020000CC RID: 204
		private struct LoopBodyWithDtHolder
		{
			// Token: 0x04000432 RID: 1074
			public static long UniqueLoopBodyKeySeed;

			// Token: 0x04000433 RID: 1075
			public TWParallel.ParallelForWithDtAuxPredicate LoopBody;

			// Token: 0x04000434 RID: 1076
			public float DeltaTime;
		}
	}
}
