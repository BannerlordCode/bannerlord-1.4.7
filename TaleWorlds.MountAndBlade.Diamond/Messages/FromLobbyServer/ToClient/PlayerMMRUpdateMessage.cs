using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Ranked;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000059 RID: 89
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PlayerMMRUpdateMessage : Message
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00003428 File Offset: 0x00001628
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00003430 File Offset: 0x00001630
		[JsonProperty]
		public RankBarInfo OldInfo { get; private set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00003439 File Offset: 0x00001639
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00003441 File Offset: 0x00001641
		[JsonProperty]
		public RankBarInfo NewInfo { get; private set; }

		// Token: 0x060001CB RID: 459 RVA: 0x0000344A File Offset: 0x0000164A
		public PlayerMMRUpdateMessage()
		{
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00003452 File Offset: 0x00001652
		public PlayerMMRUpdateMessage(RankBarInfo oldInfo, RankBarInfo newInfo)
		{
			this.OldInfo = oldInfo;
			this.NewInfo = newInfo;
		}
	}
}
