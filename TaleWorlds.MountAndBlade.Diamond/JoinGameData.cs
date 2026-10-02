using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200012C RID: 300
	[Serializable]
	public class JoinGameData
	{
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0000BC4F File Offset: 0x00009E4F
		// (set) Token: 0x060007E9 RID: 2025 RVA: 0x0000BC57 File Offset: 0x00009E57
		public GameServerProperties GameServerProperties { get; set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x0000BC60 File Offset: 0x00009E60
		// (set) Token: 0x060007EB RID: 2027 RVA: 0x0000BC68 File Offset: 0x00009E68
		public int PeerIndex { get; set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0000BC71 File Offset: 0x00009E71
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x0000BC79 File Offset: 0x00009E79
		public int SessionKey { get; set; }

		// Token: 0x060007EE RID: 2030 RVA: 0x0000BC82 File Offset: 0x00009E82
		public JoinGameData()
		{
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0000BC8A File Offset: 0x00009E8A
		public JoinGameData(GameServerProperties gameServerProperties, int peerIndex, int sessionKey)
		{
			this.GameServerProperties = gameServerProperties;
			this.PeerIndex = peerIndex;
			this.SessionKey = sessionKey;
		}
	}
}
