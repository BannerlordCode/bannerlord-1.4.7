using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000078 RID: 120
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AssignAsClanOfficerMessage : Message
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00003A39 File Offset: 0x00001C39
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00003A41 File Offset: 0x00001C41
		[JsonProperty]
		public PlayerId AssignedPlayerId { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00003A4A File Offset: 0x00001C4A
		// (set) Token: 0x0600025B RID: 603 RVA: 0x00003A52 File Offset: 0x00001C52
		[JsonProperty]
		public bool DontUseNameForUnknownPlayer { get; private set; }

		// Token: 0x0600025C RID: 604 RVA: 0x00003A5B File Offset: 0x00001C5B
		public AssignAsClanOfficerMessage()
		{
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00003A63 File Offset: 0x00001C63
		public AssignAsClanOfficerMessage(PlayerId assignedPlayerId, bool dontUseNameForUnknownPlayer)
		{
			this.AssignedPlayerId = assignedPlayerId;
			this.DontUseNameForUnknownPlayer = dontUseNameForUnknownPlayer;
		}
	}
}
