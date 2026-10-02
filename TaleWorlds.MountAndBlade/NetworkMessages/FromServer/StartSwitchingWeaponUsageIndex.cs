using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C5 RID: 197
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class StartSwitchingWeaponUsageIndex : GameNetworkMessage
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x0000DC74 File Offset: 0x0000BE74
		// (set) Token: 0x0600080C RID: 2060 RVA: 0x0000DC7C File Offset: 0x0000BE7C
		public int AgentIndex { get; private set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0000DC85 File Offset: 0x0000BE85
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x0000DC8D File Offset: 0x0000BE8D
		public EquipmentIndex EquipmentIndex { get; private set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0000DC96 File Offset: 0x0000BE96
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x0000DC9E File Offset: 0x0000BE9E
		public int UsageIndex { get; private set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0000DCA7 File Offset: 0x0000BEA7
		// (set) Token: 0x06000812 RID: 2066 RVA: 0x0000DCAF File Offset: 0x0000BEAF
		public Agent.UsageDirection CurrentMovementFlagUsageDirection { get; private set; }

		// Token: 0x06000813 RID: 2067 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
		public StartSwitchingWeaponUsageIndex(int agentIndex, EquipmentIndex equipmentIndex, int usageIndex, Agent.UsageDirection currentMovementFlagUsageDirection)
		{
			this.AgentIndex = agentIndex;
			this.EquipmentIndex = equipmentIndex;
			this.UsageIndex = usageIndex;
			this.CurrentMovementFlagUsageDirection = currentMovementFlagUsageDirection;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0000DCDD File Offset: 0x0000BEDD
		public StartSwitchingWeaponUsageIndex()
		{
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0000DCE8 File Offset: 0x0000BEE8
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.EquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.UsageIndex = (int)((short)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponUsageIndexCompressionInfo, ref flag));
			this.CurrentMovementFlagUsageDirection = (Agent.UsageDirection)GameNetworkMessage.ReadIntFromPacket(CompressionMission.UsageDirectionCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0000DD3C File Offset: 0x0000BF3C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.EquipmentIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.UsageIndex, CompressionMission.WeaponUsageIndexCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.CurrentMovementFlagUsageDirection, CompressionMission.UsageDirectionCompressionInfo);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0000DD79 File Offset: 0x0000BF79
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.EquipmentDetailed;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0000DD80 File Offset: 0x0000BF80
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "StartSwitchingWeaponUsageIndex: ", this.UsageIndex, " for weapon with EquipmentIndex: ", this.EquipmentIndex, " on Agent with agent-index: ", this.AgentIndex });
		}
	}
}
