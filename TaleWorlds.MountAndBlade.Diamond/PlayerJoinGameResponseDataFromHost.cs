using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200012A RID: 298
	[Serializable]
	public class PlayerJoinGameResponseDataFromHost
	{
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0000B9B8 File Offset: 0x00009BB8
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x0000B9C0 File Offset: 0x00009BC0
		public PlayerId PlayerId { get; set; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0000B9C9 File Offset: 0x00009BC9
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0000B9D1 File Offset: 0x00009BD1
		public int PeerIndex { get; set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0000B9DA File Offset: 0x00009BDA
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x0000B9E2 File Offset: 0x00009BE2
		public int SessionKey { get; set; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0000B9EB File Offset: 0x00009BEB
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x0000B9F3 File Offset: 0x00009BF3
		public bool IsAdmin { get; set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0000B9FC File Offset: 0x00009BFC
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x0000BA04 File Offset: 0x00009C04
		public CustomGameJoinResponse CustomGameJoinResponse { get; set; }
	}
}
