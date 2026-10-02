using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000141 RID: 321
	[Serializable]
	public class PlayerBattleServerInformation
	{
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x0000CBAA File Offset: 0x0000ADAA
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x0000CBB2 File Offset: 0x0000ADB2
		public int PeerIndex { get; set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x0000CBBB File Offset: 0x0000ADBB
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x0000CBC3 File Offset: 0x0000ADC3
		public int SessionKey { get; set; }

		// Token: 0x060008A2 RID: 2210 RVA: 0x0000CBCC File Offset: 0x0000ADCC
		public PlayerBattleServerInformation()
		{
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0000CBD4 File Offset: 0x0000ADD4
		public PlayerBattleServerInformation(int peerIndex, int sessionKey)
		{
			this.PeerIndex = peerIndex;
			this.SessionKey = sessionKey;
		}
	}
}
