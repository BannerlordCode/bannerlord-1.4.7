using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000095 RID: 149
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemoveEquippedWeapon : GameNetworkMessage
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0000AAC6 File Offset: 0x00008CC6
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x0000AACE File Offset: 0x00008CCE
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0000AAD7 File Offset: 0x00008CD7
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x0000AADF File Offset: 0x00008CDF
		public int AgentIndex { get; private set; }

		// Token: 0x060005E8 RID: 1512 RVA: 0x0000AAE8 File Offset: 0x00008CE8
		public RemoveEquippedWeapon(int agentIndex, EquipmentIndex slot)
		{
			this.AgentIndex = agentIndex;
			this.SlotIndex = slot;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0000AAFE File Offset: 0x00008CFE
		public RemoveEquippedWeapon()
		{
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0000AB06 File Offset: 0x00008D06
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.SlotIndex, CompressionMission.ItemSlotCompressionInfo);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0000AB24 File Offset: 0x00008D24
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SlotIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0000AB53 File Offset: 0x00008D53
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0000AB5B File Offset: 0x00008D5B
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Remove equipped weapon from SlotIndex: ", this.SlotIndex, " on agent with agent-index: ", this.AgentIndex });
		}
	}
}
