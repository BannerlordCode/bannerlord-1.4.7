using System;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000174 RID: 372
	public class PlayerInfo
	{
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x00010D5D File Offset: 0x0000EF5D
		// (set) Token: 0x06000A6B RID: 2667 RVA: 0x00010D65 File Offset: 0x0000EF65
		public string PlayerId { get; set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00010D6E File Offset: 0x0000EF6E
		// (set) Token: 0x06000A6D RID: 2669 RVA: 0x00010D76 File Offset: 0x0000EF76
		public string Username { get; set; }

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x00010D7F File Offset: 0x0000EF7F
		// (set) Token: 0x06000A6F RID: 2671 RVA: 0x00010D87 File Offset: 0x0000EF87
		public int ForcedIndex { get; set; }

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x00010D90 File Offset: 0x0000EF90
		// (set) Token: 0x06000A71 RID: 2673 RVA: 0x00010D98 File Offset: 0x0000EF98
		public int TeamNo { get; set; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x00010DA1 File Offset: 0x0000EFA1
		// (set) Token: 0x06000A73 RID: 2675 RVA: 0x00010DA9 File Offset: 0x0000EFA9
		public int Kill { get; set; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x00010DB2 File Offset: 0x0000EFB2
		// (set) Token: 0x06000A75 RID: 2677 RVA: 0x00010DBA File Offset: 0x0000EFBA
		public int Death { get; set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00010DC3 File Offset: 0x0000EFC3
		// (set) Token: 0x06000A77 RID: 2679 RVA: 0x00010DCB File Offset: 0x0000EFCB
		public int Assist { get; set; }

		// Token: 0x06000A78 RID: 2680 RVA: 0x00010DD4 File Offset: 0x0000EFD4
		public bool HasSameContentWith(PlayerInfo other)
		{
			return this.PlayerId == other.PlayerId && this.Username == other.Username && this.ForcedIndex == other.ForcedIndex && this.TeamNo == other.TeamNo && this.Kill == other.Kill && this.Death == other.Death && this.Assist == other.Assist;
		}
	}
}
