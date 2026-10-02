using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009A RID: 154
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentHealth : GameNetworkMessage
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0000B065 File Offset: 0x00009265
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x0000B06D File Offset: 0x0000926D
		public int AgentIndex { get; private set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0000B076 File Offset: 0x00009276
		// (set) Token: 0x06000627 RID: 1575 RVA: 0x0000B07E File Offset: 0x0000927E
		public int Health { get; private set; }

		// Token: 0x06000628 RID: 1576 RVA: 0x0000B087 File Offset: 0x00009287
		public SetAgentHealth(int agentIndex, int newHealth)
		{
			this.AgentIndex = agentIndex;
			this.Health = newHealth;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0000B09D File Offset: 0x0000929D
		public SetAgentHealth()
		{
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0000B0A8 File Offset: 0x000092A8
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.Health = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentHealthCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0000B0D7 File Offset: 0x000092D7
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.Health, CompressionMission.AgentHealthCompressionInfo);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0000B0F4 File Offset: 0x000092F4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0000B0FC File Offset: 0x000092FC
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set agent health to: ", this.Health, ", for agent-index: ", this.AgentIndex });
		}
	}
}
