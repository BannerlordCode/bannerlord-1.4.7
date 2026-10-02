using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009E RID: 158
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetAgentPrefabComponentVisibility : GameNetworkMessage
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0000B398 File Offset: 0x00009598
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x0000B3A0 File Offset: 0x000095A0
		public int AgentIndex { get; private set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0000B3A9 File Offset: 0x000095A9
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x0000B3B1 File Offset: 0x000095B1
		public int ComponentIndex { get; private set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0000B3BA File Offset: 0x000095BA
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0000B3C2 File Offset: 0x000095C2
		public bool Visibility { get; private set; }

		// Token: 0x06000652 RID: 1618 RVA: 0x0000B3CB File Offset: 0x000095CB
		public SetAgentPrefabComponentVisibility(int agentIndex, int componentIndex, bool visibility)
		{
			this.AgentIndex = agentIndex;
			this.ComponentIndex = componentIndex;
			this.Visibility = visibility;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0000B3E8 File Offset: 0x000095E8
		public SetAgentPrefabComponentVisibility()
		{
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0000B3F0 File Offset: 0x000095F0
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.ComponentIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.AgentPrefabComponentIndexCompressionInfo, ref flag);
			this.Visibility = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0000B42C File Offset: 0x0000962C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket(this.ComponentIndex, CompressionMission.AgentPrefabComponentIndexCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.Visibility);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0000B454 File Offset: 0x00009654
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0000B45C File Offset: 0x0000965C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set Component with index: ",
				this.ComponentIndex,
				" to be ",
				this.Visibility ? "visible" : "invisible",
				" on Agent with agent-index: ",
				this.AgentIndex
			});
		}
	}
}
