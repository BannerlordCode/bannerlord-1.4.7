using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaleWorlds.Library
{
	// Token: 0x0200009B RID: 155
	public static class TWParallel
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x000136DA File Offset: 0x000118DA
		public static void InitializeAndSetImplementation(IParallelDriver parallelDriver)
		{
			TWParallel._parallelDriver = parallelDriver;
			TWParallel._mainThreadId = TWParallel.GetMainThreadId();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000136EC File Offset: 0x000118EC
		public static ParallelLoopResult ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body)
		{
			return Parallel.ForEach<TSource>(Partitioner.Create<TSource>(source), Common.ParallelOptions, body);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000136FF File Offset: 0x000118FF
		[Obsolete("Please use For() not ForEach() for better Parallel Performance.", true)]
		public static void ForEach<TSource>(IList<TSource> source, Action<TSource> body)
		{
			Parallel.ForEach<TSource>(Partitioner.Create<TSource>(source), Common.ParallelOptions, body);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00013713 File Offset: 0x00011913
		public static void For(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate body, int grainSize = 16)
		{
			if (toExclusive - fromInclusive < grainSize)
			{
				body(fromInclusive, toExclusive);
				return;
			}
			TWParallel._parallelDriver.For(fromInclusive, toExclusive, body, grainSize);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00013732 File Offset: 0x00011932
		public static void ForWithoutRenderThread(int fromInclusive, int toExclusive, TWParallel.ParallelForAuxPredicate body, int grainSize = 16)
		{
			if (toExclusive - fromInclusive < grainSize)
			{
				body(fromInclusive, toExclusive);
				return;
			}
			TWParallel._parallelDriver.ForWithoutRenderThread(fromInclusive, toExclusive, body, grainSize);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00013751 File Offset: 0x00011951
		public static void ForWithoutRenderThreadDt(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate body, int grainSize = 16)
		{
			if (toExclusive - fromInclusive < grainSize)
			{
				body(fromInclusive, toExclusive, deltaTime);
				return;
			}
			TWParallel._parallelDriver.ForWithoutRenderThreadDt(fromInclusive, toExclusive, deltaTime, body, grainSize);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00013774 File Offset: 0x00011974
		public static void For(int fromInclusive, int toExclusive, float deltaTime, TWParallel.ParallelForWithDtAuxPredicate body, int grainSize = 16)
		{
			if (toExclusive - fromInclusive < grainSize)
			{
				body(fromInclusive, toExclusive, deltaTime);
				return;
			}
			TWParallel._parallelDriver.For(fromInclusive, toExclusive, deltaTime, body, grainSize);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00013797 File Offset: 0x00011997
		[Conditional("_RGL_KEEP_ASSERTS")]
		public static void AssertIsMainThread()
		{
			TWParallel.GetCurrentThreadId();
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001379F File Offset: 0x0001199F
		public static bool IsMainThread()
		{
			return TWParallel._mainThreadId == TWParallel.GetCurrentThreadId();
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x000137AD File Offset: 0x000119AD
		private static ulong GetMainThreadId()
		{
			return TWParallel._parallelDriver.GetMainThreadId();
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x000137B9 File Offset: 0x000119B9
		internal static ulong GetCurrentThreadId()
		{
			return TWParallel._parallelDriver.GetCurrentThreadId();
		}

		// Token: 0x040001AE RID: 430
		private static IParallelDriver _parallelDriver = new DefaultParallelDriver();

		// Token: 0x040001AF RID: 431
		private static ulong _mainThreadId;

		// Token: 0x020000EB RID: 235
		// (Invoke) Token: 0x060007BE RID: 1982
		public delegate void ParallelForAuxPredicate(int localStartIndex, int localEndIndex);

		// Token: 0x020000EC RID: 236
		// (Invoke) Token: 0x060007C2 RID: 1986
		public delegate void ParallelForWithDtAuxPredicate(int localStartIndex, int localEndIndex, float dt);
	}
}
