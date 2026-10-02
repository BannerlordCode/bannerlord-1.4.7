using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000087 RID: 135
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class EquipWeaponFromSpawnedItemEntity : GameNetworkMessage
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00009BD5 File Offset: 0x00007DD5
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x00009BDD File Offset: 0x00007DDD
		public MissionObjectId SpawnedItemEntityId { get; private set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x00009BE6 File Offset: 0x00007DE6
		// (set) Token: 0x06000546 RID: 1350 RVA: 0x00009BEE File Offset: 0x00007DEE
		public EquipmentIndex SlotIndex { get; private set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x00009BF7 File Offset: 0x00007DF7
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x00009BFF File Offset: 0x00007DFF
		public int AgentIndex { get; private set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00009C08 File Offset: 0x00007E08
		// (set) Token: 0x0600054A RID: 1354 RVA: 0x00009C10 File Offset: 0x00007E10
		public bool RemoveWeapon { get; private set; }

		// Token: 0x0600054B RID: 1355 RVA: 0x00009C19 File Offset: 0x00007E19
		public EquipWeaponFromSpawnedItemEntity(int agentIndex, EquipmentIndex slot, MissionObjectId spawnedItemEntityId, bool removeWeapon)
		{
			this.AgentIndex = agentIndex;
			this.SlotIndex = slot;
			this.SpawnedItemEntityId = spawnedItemEntityId;
			this.RemoveWeapon = removeWeapon;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00009C3E File Offset: 0x00007E3E
		public EquipWeaponFromSpawnedItemEntity()
		{
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00009C48 File Offset: 0x00007E48
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteMissionObjectIdToPacket((this.SpawnedItemEntityId.Id >= 0) ? this.SpawnedItemEntityId : MissionObjectId.Invalid);
			GameNetworkMessage.WriteIntToPacket((int)this.SlotIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.RemoveWeapon);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00009C9C File Offset: 0x00007E9C
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.SpawnedItemEntityId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.SlotIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.RemoveWeapon = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00009CE5 File Offset: 0x00007EE5
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items | MultiplayerMessageFilter.AgentsDetailed;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00009CF0 File Offset: 0x00007EF0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"EquipWeaponFromSpawnedItemEntity with missionObjectId: ",
				this.SpawnedItemEntityId,
				" to SlotIndex: ",
				this.SlotIndex,
				" on agent-index: ",
				this.AgentIndex,
				" RemoveWeapon: ",
				this.RemoveWeapon.ToString()
			});
		}
	}
}
