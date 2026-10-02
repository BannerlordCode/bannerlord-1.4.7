using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000110 RID: 272
	[Serializable]
	public class CosmeticItemInfo
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x000073EE File Offset: 0x000055EE
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x000073F6 File Offset: 0x000055F6
		public string TroopId { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x000073FF File Offset: 0x000055FF
		// (set) Token: 0x060005D8 RID: 1496 RVA: 0x00007407 File Offset: 0x00005607
		public string CosmeticIndex { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x00007410 File Offset: 0x00005610
		// (set) Token: 0x060005DA RID: 1498 RVA: 0x00007418 File Offset: 0x00005618
		public bool IsEquipped { get; set; }

		// Token: 0x060005DB RID: 1499 RVA: 0x00007421 File Offset: 0x00005621
		public CosmeticItemInfo()
		{
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00007429 File Offset: 0x00005629
		public CosmeticItemInfo(string troopId, string cosmeticIndex, bool isEquipped)
		{
			this.TroopId = troopId;
			this.CosmeticIndex = cosmeticIndex;
			this.IsEquipped = isEquipped;
		}
	}
}
