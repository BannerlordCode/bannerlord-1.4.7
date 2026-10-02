using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x0200003C RID: 60
	public class LoadResult
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000BEE9 File Offset: 0x0000A0E9
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000BEF1 File Offset: 0x0000A0F1
		public object Root { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000BEFA File Offset: 0x0000A0FA
		// (set) Token: 0x0600025A RID: 602 RVA: 0x0000BF02 File Offset: 0x0000A102
		public bool Successful { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000BF0B File Offset: 0x0000A10B
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000BF13 File Offset: 0x0000A113
		public LoadError[] Errors { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000BF1C File Offset: 0x0000A11C
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000BF24 File Offset: 0x0000A124
		public MetaData MetaData { get; private set; }

		// Token: 0x0600025F RID: 607 RVA: 0x0000BF2D File Offset: 0x0000A12D
		private LoadResult()
		{
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000BF35 File Offset: 0x0000A135
		internal static LoadResult CreateSuccessful(object root, MetaData metaData, LoadCallbackInitializator loadCallbackInitializator)
		{
			return new LoadResult
			{
				Root = root,
				Successful = true,
				MetaData = metaData,
				_loadCallbackInitializator = loadCallbackInitializator
			};
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000BF58 File Offset: 0x0000A158
		internal static LoadResult CreateFailed(IEnumerable<LoadError> errors)
		{
			return new LoadResult
			{
				Successful = false,
				Errors = errors.ToArray<LoadError>()
			};
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000BF72 File Offset: 0x0000A172
		public void InitializeObjects()
		{
			this._loadCallbackInitializator.InitializeObjects();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000BF7F File Offset: 0x0000A17F
		public void AfterInitializeObjects()
		{
			this._loadCallbackInitializator.AfterInitializeObjects();
		}

		// Token: 0x040000B9 RID: 185
		private LoadCallbackInitializator _loadCallbackInitializator;
	}
}
