using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C3 RID: 195
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SpawnWeaponAsDropFromAgent : GameNetworkMessage
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0000D81A File Offset: 0x0000BA1A
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x0000D822 File Offset: 0x0000BA22
		public int AgentIndex { get; private set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0000D82B File Offset: 0x0000BA2B
		// (set) Token: 0x060007E6 RID: 2022 RVA: 0x0000D833 File Offset: 0x0000BA33
		public EquipmentIndex EquipmentIndex { get; private set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0000D83C File Offset: 0x0000BA3C
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x0000D844 File Offset: 0x0000BA44
		public Vec3 Velocity { get; private set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0000D84D File Offset: 0x0000BA4D
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x0000D855 File Offset: 0x0000BA55
		public Vec3 AngularVelocity { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0000D85E File Offset: 0x0000BA5E
		// (set) Token: 0x060007EC RID: 2028 RVA: 0x0000D866 File Offset: 0x0000BA66
		public Mission.WeaponSpawnFlags WeaponSpawnFlags { get; private set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0000D86F File Offset: 0x0000BA6F
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0000D877 File Offset: 0x0000BA77
		public int ForcedIndex { get; private set; }

		// Token: 0x060007EF RID: 2031 RVA: 0x0000D880 File Offset: 0x0000BA80
		public SpawnWeaponAsDropFromAgent(int agentIndex, EquipmentIndex equipmentIndex, Vec3 velocity, Vec3 angularVelocity, Mission.WeaponSpawnFlags weaponSpawnFlags, int forcedIndex)
		{
			this.AgentIndex = agentIndex;
			this.EquipmentIndex = equipmentIndex;
			this.Velocity = velocity;
			this.AngularVelocity = angularVelocity;
			this.WeaponSpawnFlags = weaponSpawnFlags;
			this.ForcedIndex = forcedIndex;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0000D8B5 File Offset: 0x0000BAB5
		public SpawnWeaponAsDropFromAgent()
		{
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0000D8C0 File Offset: 0x0000BAC0
		protected override bool OnRead()
		{
			bool flag = true;
			this.AgentIndex = GameNetworkMessage.ReadAgentIndexFromPacket(ref flag);
			this.EquipmentIndex = (EquipmentIndex)GameNetworkMessage.ReadIntFromPacket(CompressionMission.ItemSlotCompressionInfo, ref flag);
			this.WeaponSpawnFlags = (Mission.WeaponSpawnFlags)GameNetworkMessage.ReadUintFromPacket(CompressionMission.SpawnedItemWeaponSpawnFlagCompressionInfo, ref flag);
			if (this.WeaponSpawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics))
			{
				this.Velocity = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.SpawnedItemVelocityCompressionInfo, ref flag);
				this.AngularVelocity = GameNetworkMessage.ReadVec3FromPacket(CompressionMission.SpawnedItemAngularVelocityCompressionInfo, ref flag);
			}
			else
			{
				this.Velocity = Vec3.Zero;
				this.AngularVelocity = Vec3.Zero;
			}
			this.ForcedIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0000D960 File Offset: 0x0000BB60
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteAgentIndexToPacket(this.AgentIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.EquipmentIndex, CompressionMission.ItemSlotCompressionInfo);
			GameNetworkMessage.WriteUintToPacket((uint)this.WeaponSpawnFlags, CompressionMission.SpawnedItemWeaponSpawnFlagCompressionInfo);
			if (this.WeaponSpawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics))
			{
				GameNetworkMessage.WriteVec3ToPacket(this.Velocity, CompressionMission.SpawnedItemVelocityCompressionInfo);
				GameNetworkMessage.WriteVec3ToPacket(this.AngularVelocity, CompressionMission.SpawnedItemAngularVelocityCompressionInfo);
			}
			GameNetworkMessage.WriteIntToPacket(this.ForcedIndex, CompressionBasic.MissionObjectIDCompressionInfo);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0000D9D6 File Offset: 0x0000BBD6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Spawn Weapon from agent with agent-index: ", this.AgentIndex, " from equipment index: ", this.EquipmentIndex, ", and with ID: ", this.ForcedIndex });
		}
	}
}
