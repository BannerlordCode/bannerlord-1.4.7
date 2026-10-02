using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000073 RID: 115
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AgentSetFormation : GameNetworkMessage
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00007B74 File Offset: 0x00005D74
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x00007B7C File Offset: 0x00005D7C
		public int AgentIndex { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00007B85 File Offset: 0x00005D85
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x00007B8D File Offset: 0x00005D8D
		public int FormationIndex { get; private set; }

		// Token: 0x06000418 RID: 1048 RVA: 0x00007B96 File Offset: 0x00005D96
		public AgentSetFormation(int agentIndex, int formationIndex)
		{
			this.AgentIndex = agentIndex;
			this.FormationIndex = formationIndex;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00007BAC File Offset: 0x00005DAC
		public AgentSetFormation()
		{
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00007BB4 File Offset: 0x00005DB4
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00007BE3 File Offset: 0x00005DE3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00007C00 File Offset: 0x00005E00
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00007C08 File Offset: 0x00005E08
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Assign agent with agent-index: ", this.AgentIndex, " to formation with index: ", this.FormationIndex });
		}
	}
}
