using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000102 RID: 258
	[Serializable]
	public class ClanCreationPlayerData
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00006F85 File Offset: 0x00005185
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x00006F8D File Offset: 0x0000518D
		public PlayerSessionId PlayerSessionId { get; private set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00006F96 File Offset: 0x00005196
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00006F9E File Offset: 0x0000519E
		public ClanCreationAnswer ClanCreationAnswer { get; private set; }

		// Token: 0x0600057D RID: 1405 RVA: 0x00006FA7 File Offset: 0x000051A7
		public ClanCreationPlayerData(PlayerSessionId playerSessionId, ClanCreationAnswer answer)
		{
			this.PlayerSessionId = playerSessionId;
			this.ClanCreationAnswer = answer;
		}
	}
}
