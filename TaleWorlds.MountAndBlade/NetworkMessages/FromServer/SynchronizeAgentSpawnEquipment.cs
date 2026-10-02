using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C8 RID: 200
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SynchronizeAgentSpawnEquipment : GameNetworkMessage
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0000DFA9 File Offset: 0x0000C1A9
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0000DFB1 File Offset: 0x0000C1B1
		public int AgentIndex { get; private set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0000DFBA File Offset: 0x0000C1BA
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0000DFC2 File Offset: 0x0000C1C2
		public Equipment SpawnEquipment { get; private set; }

		// Token: 0x06000833 RID: 2099 RVA: 0x0000DFCC File Offset: 0x0000C1CC
		public SynchronizeAgentSpawnEquipment(int agentIndex, Equipment spawnEquipment)
		{
			this.AgentIndex = agentIndex;
			this.SpawnEquipment = new Equipment();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				this.SpawnEquipment[equipmentIndex] = spawnEquipment.GetEquipmentFromSlot(equipmentIndex);
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0000E011 File Offset: 0x0000C211
		public SynchronizeAgentSpawnEquipment()
		{
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0000E01C File Offset: 0x0000C21C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SpawnEquipment = new Equipment();
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				this.SpawnEquipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, ModuleNetworkData.ReadItemReferenceFromPacket(MBObjectManager.Instance, ref flag));
			}
			return flag;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0000E06C File Offset: 0x0000C26C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				ModuleNetworkData.WriteItemReferenceToPacket(this.SpawnEquipment.GetEquipmentFromSlot(equipmentIndex));
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0000E0A2 File Offset: 0x0000C2A2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Agents;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0000E0AA File Offset: 0x0000C2AA
		protected override string OnGetLogFormat()
		{
			return "Equipment synchronized for agent-index: " + this.AgentIndex;
		}
	}
}
