using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000088 RID: 136
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class CreatePremadeGameMessage : Message
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00003C9A File Offset: 0x00001E9A
		// (set) Token: 0x06000295 RID: 661 RVA: 0x00003CA2 File Offset: 0x00001EA2
		[JsonProperty]
		public string PremadeGameName { get; private set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000296 RID: 662 RVA: 0x00003CAB File Offset: 0x00001EAB
		// (set) Token: 0x06000297 RID: 663 RVA: 0x00003CB3 File Offset: 0x00001EB3
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000298 RID: 664 RVA: 0x00003CBC File Offset: 0x00001EBC
		// (set) Token: 0x06000299 RID: 665 RVA: 0x00003CC4 File Offset: 0x00001EC4
		[JsonProperty]
		public string MapName { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00003CCD File Offset: 0x00001ECD
		// (set) Token: 0x0600029B RID: 667 RVA: 0x00003CD5 File Offset: 0x00001ED5
		[JsonProperty]
		public string FactionA { get; private set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00003CDE File Offset: 0x00001EDE
		// (set) Token: 0x0600029D RID: 669 RVA: 0x00003CE6 File Offset: 0x00001EE6
		[JsonProperty]
		public string FactionB { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00003CEF File Offset: 0x00001EEF
		// (set) Token: 0x0600029F RID: 671 RVA: 0x00003CF7 File Offset: 0x00001EF7
		[JsonProperty]
		public string Password { get; private set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00003D00 File Offset: 0x00001F00
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x00003D08 File Offset: 0x00001F08
		[JsonProperty]
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x060002A2 RID: 674 RVA: 0x00003D11 File Offset: 0x00001F11
		public CreatePremadeGameMessage()
		{
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00003D19 File Offset: 0x00001F19
		public CreatePremadeGameMessage(string premadeGameName, string gameType, string mapName, string factionA, string factionB, string password, PremadeGameType premadeGameType)
		{
			this.PremadeGameName = premadeGameName;
			this.GameType = gameType;
			this.MapName = mapName;
			this.FactionA = factionA;
			this.FactionB = factionB;
			this.Password = password;
			this.PremadeGameType = premadeGameType;
		}
	}
}
