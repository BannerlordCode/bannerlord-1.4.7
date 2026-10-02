using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000091 RID: 145
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MakeAgentDead : GameNetworkMessage
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x0000A78F File Offset: 0x0000898F
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x0000A797 File Offset: 0x00008997
		public int AgentIndex { get; private set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0000A7A0 File Offset: 0x000089A0
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x0000A7A8 File Offset: 0x000089A8
		public bool IsKilled { get; private set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0000A7B1 File Offset: 0x000089B1
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x0000A7B9 File Offset: 0x000089B9
		public ActionIndexCache ActionCodeIndex { get; private set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0000A7C2 File Offset: 0x000089C2
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x0000A7CA File Offset: 0x000089CA
		public int CorpsesToFadeIndex { get; private set; }

		// Token: 0x060005C2 RID: 1474 RVA: 0x0000A7D3 File Offset: 0x000089D3
		public MakeAgentDead(int agentIndex, bool isKilled, ActionIndexCache actionCodeIndex, int corpsesToFadeIndex = -1)
		{
			this.AgentIndex = agentIndex;
			this.IsKilled = isKilled;
			this.ActionCodeIndex = actionCodeIndex;
			this.CorpsesToFadeIndex = corpsesToFadeIndex;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0000A7F8 File Offset: 0x000089F8
		public MakeAgentDead()
		{
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0000A800 File Offset: 0x00008A00
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.IsKilled = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.ActionCodeIndex = new ActionIndexCache(GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ActionCodeCompressionInfo, ref flag));
			this.CorpsesToFadeIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0000A850 File Offset: 0x00008A50
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteBoolToPacket(this.IsKilled);
			GameNetworkMessage.WriteIntToPacket(this.ActionCodeIndex.Index, CompressionBasic.ActionCodeCompressionInfo);
			GameNetworkMessage.WriteAgentIndexToPacket(this.CorpsesToFadeIndex);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0000A896 File Offset: 0x00008A96
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0000A89B File Offset: 0x00008A9B
		protected override string OnGetLogFormat()
		{
			return "Make Agent Dead on Agent with agent-index: " + this.AgentIndex;
		}
	}
}
