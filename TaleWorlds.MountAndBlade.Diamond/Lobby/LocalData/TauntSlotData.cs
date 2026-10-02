using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000176 RID: 374
	public class TauntSlotData : MultiplayerLocalData
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x000111DA File Offset: 0x0000F3DA
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x000111E2 File Offset: 0x0000F3E2
		public string PlayerId { get; set; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x000111EB File Offset: 0x0000F3EB
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x000111F3 File Offset: 0x0000F3F3
		public List<TauntIndexData> TauntIndices { get; set; }

		// Token: 0x06000A88 RID: 2696 RVA: 0x000111FC File Offset: 0x0000F3FC
		public TauntSlotData(string playerId)
		{
			this.PlayerId = playerId;
			this.TauntIndices = new List<TauntIndexData>();
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00011218 File Offset: 0x0000F418
		public override bool HasSameContentWith(MultiplayerLocalData other)
		{
			TauntSlotData tauntSlotData;
			return (tauntSlotData = other as TauntSlotData) != null && this.PlayerId == tauntSlotData.PlayerId && this.TauntIndices.SequenceEqual<TauntIndexData>(tauntSlotData.TauntIndices);
		}
	}
}
