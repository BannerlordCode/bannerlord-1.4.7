using System;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000177 RID: 375
	public struct TauntIndexData
	{
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x00011257 File Offset: 0x0000F457
		// (set) Token: 0x06000A8B RID: 2699 RVA: 0x0001125F File Offset: 0x0000F45F
		public string TauntId { get; set; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x00011268 File Offset: 0x0000F468
		// (set) Token: 0x06000A8D RID: 2701 RVA: 0x00011270 File Offset: 0x0000F470
		public int TauntIndex { get; set; }

		// Token: 0x06000A8E RID: 2702 RVA: 0x00011279 File Offset: 0x0000F479
		public TauntIndexData(string tauntId, int tauntIndex)
		{
			this.TauntId = tauntId;
			this.TauntIndex = tauntIndex;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0001128C File Offset: 0x0000F48C
		public override bool Equals(object obj)
		{
			if (obj is TauntIndexData)
			{
				TauntIndexData tauntIndexData = (TauntIndexData)obj;
				return this.TauntId == tauntIndexData.TauntId && this.TauntIndex == tauntIndexData.TauntIndex;
			}
			return false;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x000112D4 File Offset: 0x0000F4D4
		public override int GetHashCode()
		{
			return (this.TauntId.GetHashCode() * 397) ^ this.TauntIndex.GetHashCode();
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00011301 File Offset: 0x0000F501
		public static bool operator ==(TauntIndexData first, TauntIndexData second)
		{
			return first.TauntId == second.TauntId && first.TauntIndex == second.TauntIndex;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0001132A File Offset: 0x0000F52A
		public static bool operator !=(TauntIndexData first, TauntIndexData second)
		{
			return !(first == second);
		}
	}
}
