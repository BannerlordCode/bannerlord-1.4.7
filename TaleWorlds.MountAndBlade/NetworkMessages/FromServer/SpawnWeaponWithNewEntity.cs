using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C4 RID: 196
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SpawnWeaponWithNewEntity : GameNetworkMessage
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0000DA36 File Offset: 0x0000BC36
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x0000DA3E File Offset: 0x0000BC3E
		public MissionWeapon Weapon { get; private set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0000DA47 File Offset: 0x0000BC47
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x0000DA4F File Offset: 0x0000BC4F
		public Mission.WeaponSpawnFlags WeaponSpawnFlags { get; private set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0000DA58 File Offset: 0x0000BC58
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x0000DA60 File Offset: 0x0000BC60
		public int ForcedIndex { get; private set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0000DA69 File Offset: 0x0000BC69
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x0000DA71 File Offset: 0x0000BC71
		public MatrixFrame Frame { get; private set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0000DA7A File Offset: 0x0000BC7A
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x0000DA82 File Offset: 0x0000BC82
		public MissionObjectId ParentMissionObjectId { get; private set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0000DA8B File Offset: 0x0000BC8B
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x0000DA93 File Offset: 0x0000BC93
		public bool IsVisible { get; private set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x0000DA9C File Offset: 0x0000BC9C
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x0000DAA4 File Offset: 0x0000BCA4
		public bool HasLifeTime { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0000DAAD File Offset: 0x0000BCAD
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x0000DAB5 File Offset: 0x0000BCB5
		public bool SpawnedOnACorpse { get; private set; }

		// Token: 0x06000805 RID: 2053 RVA: 0x0000DAC0 File Offset: 0x0000BCC0
		public SpawnWeaponWithNewEntity(MissionWeapon weapon, Mission.WeaponSpawnFlags weaponSpawnFlags, int forcedIndex, MatrixFrame frame, MissionObjectId parentMissionObjectId, bool isVisible, bool hasLifeTime, bool spawnedOnACorpse)
		{
			this.Weapon = weapon;
			this.WeaponSpawnFlags = weaponSpawnFlags;
			this.ForcedIndex = forcedIndex;
			this.Frame = frame;
			this.ParentMissionObjectId = parentMissionObjectId;
			this.IsVisible = isVisible;
			this.HasLifeTime = hasLifeTime;
			this.SpawnedOnACorpse = spawnedOnACorpse;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0000DB10 File Offset: 0x0000BD10
		public SpawnWeaponWithNewEntity()
		{
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0000DB18 File Offset: 0x0000BD18
		protected override bool OnRead()
		{
			bool flag = true;
			this.Weapon = ModuleNetworkData.ReadWeaponReferenceFromPacket(MBObjectManager.Instance, ref flag);
			this.Frame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref flag);
			this.WeaponSpawnFlags = (Mission.WeaponSpawnFlags)GameNetworkMessage.ReadUintFromPacket(CompressionMission.SpawnedItemWeaponSpawnFlagCompressionInfo, ref flag);
			this.ForcedIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref flag);
			this.ParentMissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.IsVisible = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.HasLifeTime = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.SpawnedOnACorpse = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0000DBA0 File Offset: 0x0000BDA0
		protected override void OnWrite()
		{
			ModuleNetworkData.WriteWeaponReferenceToPacket(this.Weapon);
			GameNetworkMessage.WriteMatrixFrameToPacket(this.Frame);
			GameNetworkMessage.WriteUintToPacket((uint)this.WeaponSpawnFlags, CompressionMission.SpawnedItemWeaponSpawnFlagCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ForcedIndex, CompressionBasic.MissionObjectIDCompressionInfo);
			GameNetworkMessage.WriteMissionObjectIdToPacket((this.ParentMissionObjectId.Id >= 0) ? this.ParentMissionObjectId : MissionObjectId.Invalid);
			GameNetworkMessage.WriteBoolToPacket(this.IsVisible);
			GameNetworkMessage.WriteBoolToPacket(this.HasLifeTime);
			GameNetworkMessage.WriteBoolToPacket(this.SpawnedOnACorpse);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0000DC24 File Offset: 0x0000BE24
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Items;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0000DC28 File Offset: 0x0000BE28
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Spawn Weapon with name: ",
				this.Weapon.Item.Name,
				", and with ID: ",
				this.ForcedIndex
			});
		}
	}
}
