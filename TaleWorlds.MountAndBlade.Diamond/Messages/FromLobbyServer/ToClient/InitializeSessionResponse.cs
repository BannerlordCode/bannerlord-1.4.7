using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000047 RID: 71
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class InitializeSessionResponse : LoginResultObject
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00002FD2 File Offset: 0x000011D2
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00002FDA File Offset: 0x000011DA
		[JsonProperty]
		public PlayerData PlayerData { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00002FE3 File Offset: 0x000011E3
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00002FEB File Offset: 0x000011EB
		[JsonProperty]
		public ServerStatus ServerStatus { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00002FF4 File Offset: 0x000011F4
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00002FFC File Offset: 0x000011FC
		[JsonProperty]
		public AvailableScenes AvailableScenes { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00003005 File Offset: 0x00001205
		// (set) Token: 0x06000168 RID: 360 RVA: 0x0000300D File Offset: 0x0000120D
		[JsonProperty]
		public SupportedFeatures SupportedFeatures { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00003016 File Offset: 0x00001216
		// (set) Token: 0x0600016A RID: 362 RVA: 0x0000301E File Offset: 0x0000121E
		[JsonProperty]
		public bool HasPendingRejoin { get; private set; }

		// Token: 0x0600016B RID: 363 RVA: 0x00003027 File Offset: 0x00001227
		public InitializeSessionResponse()
		{
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000302F File Offset: 0x0000122F
		public InitializeSessionResponse(PlayerData playerData, ServerStatus serverStatus, AvailableScenes availableScenes, SupportedFeatures supportedFeatures, bool hasPendingRejoin)
		{
			this.PlayerData = playerData;
			this.ServerStatus = serverStatus;
			this.AvailableScenes = availableScenes;
			this.SupportedFeatures = supportedFeatures;
			this.HasPendingRejoin = hasPendingRejoin;
		}
	}
}
