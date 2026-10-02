using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000AC RID: 172
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class InitializeSession : LoginMessage
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000410F File Offset: 0x0000230F
		// (set) Token: 0x06000305 RID: 773 RVA: 0x00004117 File Offset: 0x00002317
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00004120 File Offset: 0x00002320
		// (set) Token: 0x06000307 RID: 775 RVA: 0x00004128 File Offset: 0x00002328
		[JsonProperty]
		public string PlayerName { get; private set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00004131 File Offset: 0x00002331
		// (set) Token: 0x06000309 RID: 777 RVA: 0x00004139 File Offset: 0x00002339
		[JsonProperty]
		public ApplicationVersion ApplicationVersion { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600030A RID: 778 RVA: 0x00004142 File Offset: 0x00002342
		// (set) Token: 0x0600030B RID: 779 RVA: 0x0000414A File Offset: 0x0000234A
		[JsonProperty]
		public string ConnectionPassword { get; private set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00004153 File Offset: 0x00002353
		// (set) Token: 0x0600030D RID: 781 RVA: 0x0000415B File Offset: 0x0000235B
		[JsonProperty]
		public ModuleInfoModel[] LoadedModules { get; private set; }

		// Token: 0x0600030E RID: 782 RVA: 0x00004164 File Offset: 0x00002364
		public InitializeSession()
		{
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000416C File Offset: 0x0000236C
		public InitializeSession(PlayerId playerId, string playerName, AccessObject accessObject, ApplicationVersion applicationVersion, string connectionPassword, ModuleInfoModel[] loadedModules)
			: base(playerId.ConvertToPeerId(), accessObject)
		{
			this.PlayerId = playerId;
			this.PlayerName = playerName;
			this.ApplicationVersion = applicationVersion;
			this.ConnectionPassword = connectionPassword;
			this.LoadedModules = loadedModules;
		}
	}
}
