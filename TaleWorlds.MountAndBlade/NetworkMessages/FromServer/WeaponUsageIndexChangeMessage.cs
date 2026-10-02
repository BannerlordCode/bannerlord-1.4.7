using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D1 RID: 209
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class WeaponUsageIndexChangeMessage : GameNetworkMessage
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x0000E8E3 File Offset: 0x0000CAE3
		// (set) Token: 0x06000894 RID: 2196 RVA: 0x0000E8EB File Offset: 0x0000CAEB
		public int AgentIndex { get; private set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x0000E8F4 File Offset: 0x0000CAF4
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x0000E8FC File Offset: 0x0000CAFC
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x0000E905 File Offset: 0x0000CB05
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x0000E90D File Offset: 0x0000CB0D
		public int UsageIndex { get; private set; }

		// Token: 0x06000899 RID: 2201 RVA: 0x0000E916 File Offset: 0x0000CB16
		public WeaponUsageIndexChangeMessage()
		{
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0000E91E File Offset: 0x0000CB1E
		public WeaponUsageIndexChangeMessage(int agentIndex, EquipmentIndex slotIndex, int usageIndex)
		{
			this.AgentIndex = agentIndex;
			this.SlotIndex = slotIndex;
			this.UsageIndex = usageIndex;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0000E93C File Offset: 0x0000CB3C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SlotIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.UsageIndex = (int)((short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponUsageIndexCompressionInfo, ref flag));
			return flag;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0000E97E File Offset: 0x0000CB7E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.SlotIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.UsageIndex, CompressionMission.WeaponUsageIndexCompressionInfo);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0000E9AB File Offset: 0x0000CBAB
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionDetailed;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0000E9B4 File Offset: 0x0000CBB4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set Weapon Usage Index: ", this.UsageIndex, " for weapon with EquipmentIndex: ", this.SlotIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
