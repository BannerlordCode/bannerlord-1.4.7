using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004B RID: 75
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class JoinBattleMessage : Message
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00003135 File Offset: 0x00001335
		// (set) Token: 0x06000182 RID: 386 RVA: 0x0000313D File Offset: 0x0000133D
		[JsonProperty]
		public BattleServerInformationForClient BattleServerInformation { get; private set; }

		// Token: 0x06000183 RID: 387 RVA: 0x00003146 File Offset: 0x00001346
		public JoinBattleMessage()
		{
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000314E File Offset: 0x0000134E
		public JoinBattleMessage(BattleServerInformationForClient battleServerInformation)
		{
			this.BattleServerInformation = battleServerInformation;
		}
	}
}
