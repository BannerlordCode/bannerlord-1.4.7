using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000074 RID: 116
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AgentSetTeam : GameNetworkMessage
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00007C41 File Offset: 0x00005E41
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x00007C49 File Offset: 0x00005E49
		public int AgentIndex { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00007C52 File Offset: 0x00005E52
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x00007C5A File Offset: 0x00005E5A
		public int TeamIndex { get; private set; }

		// Token: 0x06000422 RID: 1058 RVA: 0x00007C63 File Offset: 0x00005E63
		public AgentSetTeam(int agentIndex, int teamIndex)
		{
			this.AgentIndex = agentIndex;
			this.TeamIndex = teamIndex;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00007C79 File Offset: 0x00005E79
		public AgentSetTeam()
		{
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00007C84 File Offset: 0x00005E84
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.TeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00007CAE File Offset: 0x00005EAE
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteTeamIndexToPacket(this.TeamIndex);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00007CC6 File Offset: 0x00005EC6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00007CCE File Offset: 0x00005ECE
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Assign agent with agent-index: ", this.AgentIndex, " to team: ", this.TeamIndex });
		}
	}
}
