using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000036 RID: 54
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SetFollowedAgent : GameNetworkMessage
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00004051 File Offset: 0x00002251
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00004059 File Offset: 0x00002259
		public int AgentIndex { get; private set; }

		// Token: 0x060001A7 RID: 423 RVA: 0x00004062 File Offset: 0x00002262
		public SetFollowedAgent(int agentIndex)
		{
			this.AgentIndex = agentIndex;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00004071 File Offset: 0x00002271
		public SetFollowedAgent()
		{
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000407C File Offset: 0x0000227C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00004099 File Offset: 0x00002299
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000040A6 File Offset: 0x000022A6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000040AE File Offset: 0x000022AE
		protected override string OnGetLogFormat()
		{
			return "Peer switched spectating an agent";
		}
	}
}
