using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200005B RID: 91
	[Serializable]
	public class PlayerRemovedFromCustomGame : Message
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00003470 File Offset: 0x00001670
		// (set) Token: 0x060001CF RID: 463 RVA: 0x00003478 File Offset: 0x00001678
		[JsonProperty]
		public DisconnectType DisconnectType { get; private set; }

		// Token: 0x060001D0 RID: 464 RVA: 0x00003481 File Offset: 0x00001681
		public PlayerRemovedFromCustomGame()
		{
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00003489 File Offset: 0x00001689
		public PlayerRemovedFromCustomGame(DisconnectType disconnectType)
		{
			this.DisconnectType = disconnectType;
		}
	}
}
