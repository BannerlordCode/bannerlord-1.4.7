using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x0200001C RID: 28
	public class BasicGameModels : GameModelsManager
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000698C File Offset: 0x00004B8C
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00006994 File Offset: 0x00004B94
		public RidingModel RidingModel { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000699D File Offset: 0x00004B9D
		// (set) Token: 0x06000189 RID: 393 RVA: 0x000069A5 File Offset: 0x00004BA5
		public ItemCategorySelector ItemCategorySelector { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600018A RID: 394 RVA: 0x000069AE File Offset: 0x00004BAE
		// (set) Token: 0x0600018B RID: 395 RVA: 0x000069B6 File Offset: 0x00004BB6
		public ItemValueModel ItemValueModel { get; private set; }

		// Token: 0x0600018C RID: 396 RVA: 0x000069BF File Offset: 0x00004BBF
		public BasicGameModels(IEnumerable<GameModel> inputComponents)
			: base(inputComponents)
		{
			this.RidingModel = base.GetGameModel<RidingModel>();
			this.ItemCategorySelector = base.GetGameModel<ItemCategorySelector>();
			this.ItemValueModel = base.GetGameModel<ItemValueModel>();
		}
	}
}
