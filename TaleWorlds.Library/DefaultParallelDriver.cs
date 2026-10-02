using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace TaleWorlds.Library
{
	// Token: 0x0200009A RID: 154
	public sealed class DefaultParallelDriver : IParallelDriver
	{
		// Token: 0x0600057C RID: 1404 RVA: 0x00013630 File Offset: 0x00011830
		public void For(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate body, int grainSize)
		{
			Parallel.ForEach<Tuple<int, int>>(Partitioner.Create(fromInclusive, toExclusive, grainSize), Common.ParallelOptions, delegate(Tuple<int, int> range, ParallelLoopState loopState)
			{
				body(range.Item1, range.Item2);
			});
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001366A File Offset: 0x0001186A
		public void ForWithoutRenderThread(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate body, int grainSize)
		{
			this.For(fromInclusive, toExclusive, body, grainSize);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00013677 File Offset: 0x00011877
		public void ForWithoutRenderThreadDt(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate body, int grainSize)
		{
			this.For(fromInclusive, toExclusive, deltaTime, body, grainSize);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00013688 File Offset: 0x00011888
		public void For(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate body, int grainSize)
		{
			Parallel.ForEach<Tuple<int, int>>(Partitioner.Create(fromInclusive, toExclusive, grainSize), Common.ParallelOptions, delegate(Tuple<int, int> range, ParallelLoopState loopState)
			{
				body(range.Item1, range.Item2, deltaTime);
			});
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000136CA File Offset: 0x000118CA
		public ulong GetMainThreadId()
		{
			return 0UL;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000136CE File Offset: 0x000118CE
		public ulong GetCurrentThreadId()
		{
			return 0UL;
		}
	}
}
