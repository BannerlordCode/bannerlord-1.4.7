using System;
using Helpers;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000395 RID: 917
	public class InventoryState : PlayerGameState
	{
		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06003514 RID: 13588 RVA: 0x000D97A1 File Offset: 0x000D79A1
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06003515 RID: 13589 RVA: 0x000D97A4 File Offset: 0x000D79A4
		// (set) Token: 0x06003516 RID: 13590 RVA: 0x000D97AC File Offset: 0x000D79AC
		public InventoryLogic InventoryLogic { get; set; }

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06003517 RID: 13591 RVA: 0x000D97B5 File Offset: 0x000D79B5
		// (set) Token: 0x06003518 RID: 13592 RVA: 0x000D97BD File Offset: 0x000D79BD
		public InventoryScreenHelper.InventoryMode InventoryMode { get; set; }

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06003519 RID: 13593 RVA: 0x000D97C6 File Offset: 0x000D79C6
		// (set) Token: 0x0600351A RID: 13594 RVA: 0x000D97CE File Offset: 0x000D79CE
		public Action DoneLogicExtrasDelegate { get; set; }

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x0600351B RID: 13595 RVA: 0x000D97D7 File Offset: 0x000D79D7
		// (set) Token: 0x0600351C RID: 13596 RVA: 0x000D97DF File Offset: 0x000D79DF
		public IInventoryStateHandler Handler { get; set; }
	}
}
